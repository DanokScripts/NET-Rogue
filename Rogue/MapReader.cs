using System;
using System.IO;
using Newtonsoft.Json;
using ZeroElectric.Vinculum;

namespace Rogue
{
    internal class MapReader
    {
        public GameMap? LoadJSON(string filename)
        {
            if (!File.Exists(filename))
            {
                return null;
            }

            string fileContent = File.ReadAllText(filename);
            GameMap? deserializedMap = JsonConvert.DeserializeObject<GameMap>(fileContent);
            return deserializedMap;
        }
    }
}
