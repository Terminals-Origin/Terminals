using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Common.Connections;
using Terminals.Connections.ICA;
using Terminals.Connections.Terminal;
using Terminals.Connections.VMRC;
using Terminals.Connections.VNC;
using Terminals.Data;
using Terminals.Data.FilePersisted;

namespace Tests.Connections
{
    [TestClass]
    public class FavoriteSerializerTests
    {
        private static readonly Tuple<string, Type>[] testCases = new Tuple<string, Type>[]
        {
            new Tuple<string, Type>(KnownConnectionConstants.RDP, typeof(RdpOptions)),
            new Tuple<string, Type>(VncConnectionPlugin.VNC, typeof(VncOptions)),
            new Tuple<string, Type>(VmrcConnectionPlugin.VMRC, typeof(VMRCOptions)),
            new Tuple<string, Type>(TelnetConnectionPlugin.TELNET, typeof(ConsoleOptions)),
            new Tuple<string, Type>(SshConnectionPlugin.SSH, typeof(SshOptions)),
            new Tuple<string, Type>(KnownConnectionConstants.HTTP, typeof(WebOptions)),
            new Tuple<string, Type>(KnownConnectionConstants.HTTPS, typeof(WebOptions)),
            new Tuple<string, Type>(ICAConnectionPlugin.ICA_CITRIX, typeof(ICAOptions))
        };

        [TestMethod]
        public void AllProtocols_SerializeDeserializeToXml_RestoresValues()
        {
            bool allValid = testCases.All(RestoreXmlSerializedFavorite);
            const string MESSAGE = "Without working roundtrip of data serialization we are not able to persist the options.";
            Assert.IsTrue(allValid, MESSAGE);
        }
        
        private static bool RestoreXmlSerializedFavorite(Tuple<string, Type> testCase)
        {
            var serializer = new FavoritesFileSerializer(TestConnectionManager.Instance);
            FavoritesFile file = CreateTestFile(testCase.Item1);
            const string FILE_NAME = "SerializationTest.xml";
            serializer.SerializeToXml(file, FILE_NAME);
            FavoritesFile loaded = serializer.Deserialize(FILE_NAME);
            Favorite target = loaded.Favorites[0];
            return target.ProtocolProperties.GetType().FullName == testCase.Item2.FullName;
        }

        private static FavoritesFile CreateTestFile(string testCase)
        {
            var source = new Favorite();
            TestConnectionManager.Instance.ChangeProtocol(source, testCase);
            var file = new FavoritesFile();
            file.Favorites = new Favorite[] {source};
            return file;
        }
    }
}
