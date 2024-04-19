using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Security.Cryptography.X509Certificates;
using System.IO;

namespace Rogue
{
    internal class MapReader
    {
        public Map? LoadJSON(string filename)
        {
            string fileContent;
            if (!File.Exists(filename))
            {
                return null;
            }
            using (StreamReader reader = File.OpenText(filename))
            {
                fileContent = reader.ReadToEnd();
            }
            Map? deserializedProduct = JsonConvert.DeserializeObject<Map>(fileContent);
            return deserializedProduct;
        }
    }
}
