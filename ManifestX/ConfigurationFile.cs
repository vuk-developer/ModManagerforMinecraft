using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage;

namespace ManifestX
{
    public class ConfigurationFile
    {
        public ConfigurationDictionary<string, string> Configuration = new ConfigurationDictionary<string, string>();

        private XDocument xDocument = new XDocument();

        StorageFolder installFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;

        public static string configurationFile = string.Empty;

        
        public void Append(string key, string value)
        {
            XElement xElement = new XElement(key);
            xElement.SetAttributeValue("value", value);
            xDocument.Root.Add(xElement);

        }
        public void SetDefault()
        {
            Configuration.AddOrUpdate("VMX.Launcher", "NotImplemented");
            Configuration.AddOrUpdate("VMX.ModsDirectory", Environment.GetEnvironmentVariable("appdata")+"\\.minecraft\\mods");
            Configuration.AddOrUpdate("VMX.ManifestDepotDirectory", Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
        }
        public ConfigurationFile()
        {
            configurationFile = installFolder.Path + "/Configuration/configuration.vml";



            SetDefault();
            Save();
            Configuration.OnChange += (key, value, isNew) =>
            {
                Configuration_OnChange(key, value, isNew);
            };
        }
        public static XDocument FromBase64(string base64)
        {
            var bytes = Convert.FromBase64String(base64);
            var xmlString = Encoding.UTF8.GetString(bytes);
            return XDocument.Parse(xmlString);
        }

        public string ToBase64(XDocument doc)
        {
            var xmlString = doc.ToString(SaveOptions.DisableFormatting);
            var bytes = Encoding.UTF8.GetBytes(xmlString);
            return Convert.ToBase64String(bytes);
        }
        public void Save()
        {
            string s = ToBase64(xDocument);
            File.WriteAllText(configurationFile,s);
        }
        public static void Clear()
        {
            File.Delete(configurationFile);
        }
        private void Configuration_OnChange(string key, string value, bool isNew)
        {
            if (xDocument.Root.Elements().Any((e) => e.Name == key))
            {
                xDocument.Root.Element(key).SetAttributeValue("value", value);
            }
            else
            {
                Append(key, value);
            }
            
        }
        public string XToString()
        {
            return xDocument.ToString();
        }

        
    }
    public class ConfigurationDictionary<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> _dictionary = new();

        public event Action<TKey, TValue, bool>? OnChange;

        public void AddOrUpdate(TKey key, TValue value)
        {
            bool isNew = !_dictionary.ContainsKey(key);
            _dictionary[key] = value;
            OnChange?.Invoke(key, value, isNew);
        }
        public TValue? this[TKey key] 
        {
            get
            {
                if (_dictionary.TryGetValue(key, out var value))
                    return value;
                ConfigurationFile.Clear();
                Alert.Send("ManifestX EnginePanic", "Error 014");
                return default;
            }
            set => AddOrUpdate(key, value!);
        }

        public bool TryGetValue(TKey key, out TValue value) => _dictionary.TryGetValue(key, out value);

        public bool ContainsKey(TKey key) => _dictionary.ContainsKey(key);

        public int Count => _dictionary.Count;

        public IEnumerable<TKey> Keys => _dictionary.Keys;
        public IEnumerable <TValue> Values => _dictionary.Values;
    }
}
