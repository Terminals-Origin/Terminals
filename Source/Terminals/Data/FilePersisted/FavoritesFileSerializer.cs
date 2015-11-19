using Unified;

namespace Terminals.Data.FilePersisted
{
    internal class FavoritesFileSerializer
    {
        public void SerializeToXml(FavoritesFile fileContent, string fileLocation)
        {
            Serialize.SerializeXMLToDisk(fileContent, fileLocation);
        }

        public FavoritesFile Deserialize(string fileLocation)
        {
            return Serialize.DeserializeXMLFromDisk(fileLocation, typeof(FavoritesFile)) as FavoritesFile;
        }
    }
}
