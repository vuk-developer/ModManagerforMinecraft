using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using VukXML;

namespace Marshalate
{
    public class Marshal
    {
        string mnfst = string.Empty;
        public void As(string modsFolder, VukJavaMod mod, string mnfstX)
        {
            using (ZipArchive zipArchive = ZipFile.Open(mnfstX, ZipArchiveMode.Read))
            {
                ZipArchiveEntry s = zipArchive.Entries.ToList().Find((e) => e.Name.EndsWith(mod._filename));
                if (File.Exists(modsFolder + "/" + mod._filename))
                {
                    File.Delete(modsFolder + "/" + mod._filename);
                }
                s.ExtractToFile(modsFolder + "/"+mod._filename);
            }
        }

        public void TakeOut(string modsFolder, string filename)
        {
            File.Delete(modsFolder+"/"+filename);
        }

        public void AllClear(string modsFolder)
        {
            foreach (string item in Directory.GetFiles(modsFolder))
            {
                File.Delete(item);
            }
        }


    }
}
