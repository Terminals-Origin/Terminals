using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
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
        private const string FILE_NAME = "SerializationTest.xml";

        private const string VNC_ELEMENT = @"
     <Favorite id=""d0a609d9-09a4-4f8d-8ed3-e58048a2369d"" xmlns=""http://Terminals.codeplex.com"">
      <Protocol>VNC</Protocol>
      <Port>3389</Port>
      <Security />
      <NewWindow>false</NewWindow>
      <ExecuteBeforeConnect>
        <Execute>false</Execute>
        <WaitForExit>false</WaitForExit>
      </ExecuteBeforeConnect>
      <Display>
        <Height>0</Height>
        <Width>0</Width>
        <DesktopSize>FitToWindow</DesktopSize>
        <Colors>Bits32</Colors>
      </Display>
      <VncOptions>
        <AutoScale>false</AutoScale>
        <ViewOnly>false</ViewOnly>
        <DisplayNumber>0</DisplayNumber>
      </VncOptions>
    </Favorite>";

        private static readonly XElement vncCachedFavorite = XDocument.Parse(VNC_ELEMENT).Root;


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
        public void RdpOnlyPluginAndCachedVncXml_Serialize_SavesUnknonwCachedFavorite()
        {
            var rdpOnlyManager = TestConnectionManager.CreateRdpOnlyManager();
            FavoritesFile file = CreateTestFile(KnownConnectionConstants.RDP);
            var unknownFavorites = new List<XElement>() { vncCachedFavorite };
            var context = new SerializationContext(file, unknownFavorites);
            var limitedSerializer = new FavoritesFileSerializer(rdpOnlyManager);

            limitedSerializer.SerializeContext(context, FILE_NAME);
            string saved = File.ReadAllText(FILE_NAME);

            bool savedVnc = saved.Contains("<Protocol>VNC</Protocol>");
            Assert.IsTrue(savedVnc, "The saved content has to contain both elemente");
        }

        [TestMethod]
        public void RdpOnlyPlugin_Deserialize_LoadsVncAsUnknown()
        {
            AssertDeserializedWithRdpOnlyPlugin(VncConnectionPlugin.VNC, true, false);
        }

        [TestMethod]
        public void RdpOnlyPlugin_Deserialize_LoadsRdpAsKnown()
        {
            AssertDeserializedWithRdpOnlyPlugin(KnownConnectionConstants.RDP, false, true);
        }

        private static void AssertDeserializedWithRdpOnlyPlugin(string expectedProtocol, bool expectedUnknown, bool expectedKnown)
        {
            var fullSerializer = new FavoritesFileSerializer(TestConnectionManager.Instance);
            FavoritesFile file = CreateTestFile(VncConnectionPlugin.VNC, KnownConnectionConstants.RDP);
            fullSerializer.SerializeToXml(file, FILE_NAME);
            var rdpOnlyManager = TestConnectionManager.CreateRdpOnlyManager();
            var limitedSerializer = new FavoritesFileSerializer(rdpOnlyManager);
            var loaded = limitedSerializer.DeserializeContext(FILE_NAME);
            AssertDeserialized(loaded, expectedProtocol, expectedUnknown, expectedKnown);
        }

        private static void AssertDeserialized(SerializationContext loaded, string expectedProtocol,
            bool expectedUnknown, bool expectedKnown)
        {
            bool hasUnknown = loaded.Unknown.Any(e => e.Value.Contains(expectedProtocol));
            bool hasKnown = loaded.File.Favorites.Any(f => f.Protocol == expectedProtocol);
            const string MESSAGE = "Deserialized '{0}' as Unknown = '{1}' (expected '{2}') and Known = '{3}' (expected '{4}').";
            Console.WriteLine(MESSAGE, expectedProtocol, hasUnknown, expectedUnknown, hasKnown, expectedKnown);
            bool passed = hasUnknown == expectedUnknown && hasKnown == expectedKnown;
            Assert.IsTrue(passed, "The not identified favorites cant be lost.");
        }

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
            serializer.SerializeToXml(file, FILE_NAME);
            FavoritesFile loaded = serializer.Deserialize(FILE_NAME);
            Favorite target = loaded.Favorites[0];
            return target.ProtocolProperties.GetType().FullName == testCase.Item2.FullName;
        }

        private static FavoritesFile CreateTestFile(params string[] protocols)
        {
            Favorite[] favorites = protocols.Select(ToFavorites).ToArray();
            var file = new FavoritesFile();
            file.Favorites = favorites;
            return file;
        }

        private static Favorite ToFavorites(string protocol)
        {
            var favorite = new Favorite();
            TestConnectionManager.Instance.ChangeProtocol(favorite, protocol);
            return favorite;
        }
    }
}
