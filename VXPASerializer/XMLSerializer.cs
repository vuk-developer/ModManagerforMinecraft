using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using VXPASerializer.Models;

namespace VXPASerializer
{
    public class InterXML
    {

        string ModsFolder = Environment.GetEnvironmentVariable("appdata")+"\\.minecraft\\mods";
        string DefaultFile = Directory.GetFiles(Environment.GetEnvironmentVariable("appdata") + "/.manifestsv/").Where(e => e.EndsWith("xvuk")).ToList()[0];
        public InterXML(string path)
        {
            ModsFolder = path;
        }
        public InterXML()
        {
            
        }
        public async void CreateInterXml(string profile, string modsPath)
        {
            List<VMXMod> VMXMods = GetMods(modsPath);
            string path = Environment.GetEnvironmentVariable("appdata") + "/.manifestsv/config.vml";
            XDocument xDocument = new XDocument(
                new XElement("VMX.ManifestX")
                );
            xDocument.Root.Add(new XElement("VMX.ModsDirectory"),
                          new XElement("VMX.Profile", profile));
            XElement modsManifest = new XElement("VMX.Manifest");
            foreach (VMXMod mod in VMXMods)
            {
                XElement modpropX = new XElement("VMX.Mod");
                modpropX.SetAttributeValue("id", mod.Id());
                
                modpropX.SetAttributeValue("name", mod.Name());
                
                modpropX.SetAttributeValue("desc", mod.Description());
                
                modpropX.SetAttributeValue("ver", mod.Version());
                
                modpropX.SetAttributeValue("framework", mod.Framework());

                modpropX.SetAttributeValue("filename", mod._filename);

                modpropX.SetAttributeValue("date", mod.Date());

                modsManifest.Add(modpropX);
                
            }
            xDocument.Root.Add(modsManifest);

            Directory.CreateDirectory(Environment.GetEnvironmentVariable("appdata")+"/.manifestsv");
            xDocument.Save(path);

            ManifestXZip(path, modsPath, Environment.GetEnvironmentVariable("appdata") + $"/.manifestsv/mnfstx_{profile}.vxpa");


        }

        public VMXProfile GetModsFromXML(string filename = null)
        {
            string temp = Environment.GetEnvironmentVariable("appdata") + "/.manifestsv/temp/";

            if (filename != null)
            {
                using (ZipArchive zipArchive = ZipFile.OpenRead(filename))
                {
                    if (Directory.Exists(temp))
                    {
                        Directory.Delete(temp, true);
                    }
                    zipArchive.ExtractToDirectory(temp);
                }
            }
            else
            {
                using (ZipArchive zipArchive = ZipFile.OpenRead(DefaultFile))
                {
                    if (Directory.Exists(temp))
                    {
                        Directory.Delete(temp, true);
                    }
                    zipArchive.ExtractToDirectory(temp);
                }
            }

            VMXProfile profile = new VMXProfile();
            XDocument xmlDoc = XDocument.Load(temp+ "_conf.vml");

            var xmlElements = xmlDoc.Element("VMX.ManifestX")
                                    .Element("VMX.Manifest");
            profile.Id = xmlDoc.Element("VMX.ManifestX")
                               .Element("VMX.Profile")
                               .Value
                               .ToString();
            foreach (XElement xmlElement in xmlElements.Descendants())
            {
                
                
                VMXMod mod = new VMXMod();
                mod._id = xmlElement.Attribute("id").Value.ToString();
                mod._name = xmlElement.Attribute("name").Value.ToString();
                mod._version = xmlElement.Attribute("ver").Value.ToString();
                mod._description = xmlElement.Attribute("desc").Value.ToString();
                mod._filename = xmlElement.Attribute("filename").Value.ToString();
                mod._framework = xmlElement.Attribute("framework").Value.ToString();
                mod._dateTime = DateTime.Now;

                
                
                profile.Mods.Add(mod);
            }

            return profile;

        }
        public List<VMXMod> GetMods(string path) 
        { 
            List<VMXMod> mods = new List<VMXMod>();

            foreach (string item in Directory.GetFiles(path))
            {

                using (ZipArchive archive = ZipFile.Open(item, ZipArchiveMode.Update))
                {

                    ZipArchiveEntry entry = archive.GetEntry(@"META-INF/mods.toml");
                    if (entry != null)
                    {
                        using (StreamReader sr = new StreamReader(entry.Open()))
                        {
                            mods.Add(MLClassifier.ToMod(sr.ReadToEnd(), Path.GetFileName(item)));
                        }
                    }

                }
            }
            return mods;
        }

        public void ManifestXZip(string manifestPath, string modsFolder, string manifestX)
        {
            using (ZipArchive zipArchive = ZipFile.Open(manifestX, ZipArchiveMode.Create))
            {
                zipArchive.CreateEntryFromFile(manifestPath, "_conf.vml");
                File.Delete(manifestPath);
                foreach (string file in Directory.GetFiles(modsFolder))
                {
                    zipArchive.CreateEntryFromFile(file, $"_res/{Path.GetFileName(file)}");
                }
            }

        }


    }
}
