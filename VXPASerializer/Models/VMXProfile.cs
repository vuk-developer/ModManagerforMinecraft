using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VXPASerializer.Models
{
    public class VMXProfile
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public List<VMXMod> Mods { get; set;} = new List<VMXMod>();

        public VMXProfile() { }

        
    }
}
