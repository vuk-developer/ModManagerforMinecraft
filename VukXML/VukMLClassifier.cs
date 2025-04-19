using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VukXML;
using Tomlyn.Helpers;
using Tomlyn.Model;
using Tomlyn.Parsing;
using Tomlyn;

namespace VukXML
{
    public class VukMLClassifier
    {
        public static VukJavaMod ToMod(string sr, string filename)
        {
            VukJavaMod modPrime = new VukJavaMod();
            TomlTable table = Toml.ToModel(sr);
            TomlTableArray tb = table["mods"] as TomlTableArray;


            VukJavaMod mod = new VukJavaMod(Guid.NewGuid().ToString(),
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
