using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Tomlyn.Helpers;
using Tomlyn.Model;
using Tomlyn.Parsing;
using Tomlyn;
using VXPASerializer.Models;

namespace VXPASerializer
{
    public class MLClassifier
    {
        public static VMXMod ToMod(string sr, string filename)
        {
            VMXMod modPrime = new VMXMod();
            TomlTable table = Toml.ToModel(sr);
            TomlTableArray tb = table["mods"] as TomlTableArray;


            VMXMod mod = new VMXMod(Guid.NewGuid().ToString(),
                                            (string)tb[0]["modId"],
                                            (string)tb[0]["description"],
                                            (string)tb[0]["version"],
                                            filename,
                                            "forge",
                                            DateTime.Now
                                            );
            modPrime = mod;

            return modPrime;
        }
    }
}
