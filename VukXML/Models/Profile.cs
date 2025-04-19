using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VukXML
{
    public class VukXProfile
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public List<VukJavaMod> Mods { get; set;} = new List<VukJavaMod>();

        public VukXProfile() { }

        
    }
}
