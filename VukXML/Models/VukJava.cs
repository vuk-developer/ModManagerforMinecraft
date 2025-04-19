using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace VukXML
{
    public class VukJavaMod
    {
        public string? _id { get; set; }
        public string? _name { get; set; }
        public string? _description { get; set; }
        public string? _version { get; set; }
        public string? _framework { get; set; }
        public string? _filename { get; set; }

        public bool? _enabled = true;
        public DateTime? _dateTime { get; set; }

        public VukJavaMod(string id, string name, string desc, string version, string filename, string framework, DateTime dateTime)
        {
            if (id == null) { id = Guid.NewGuid().ToString(); }
            _id = id;
            _name = name;
            _description = desc;
            _version = version;
            _filename = filename;
            _framework = framework;
            if (dateTime == null) { dateTime = DateTime.Now; }
            _dateTime = dateTime;

        }
        public VukJavaMod()
        {
            
        }
        public string Name()
        {

            return _name;
        }
        public string Description()
        {
            return _description;
        }
        public string Version()
        {
            return _version;
        }
        public string Framework()
        {
            return _framework;
        }
        public DateTime Date()
        {
            return (DateTime)_dateTime;
        }
        public string Id()
        {
            return _id;
        }
        public override string ToString()
        {
          // bool v = 3 == 4 ? true : false;
            File.WriteAllText(@"C:\Users\Administrator\Desktop\JBT.v", $"id = {_id},\n name = {_name},\n desc = {_description.TrimEnd()},\n version = {_version},\n framework = {_framework},\n date = {_dateTime.ToString()}");
            return $"id = {_id},\n name = {_name},\n desc = {_description.TrimEnd()},\n version = {_version},\n framework = {_framework},\n date = {_dateTime.ToString()}";

            

        }

    }

}
