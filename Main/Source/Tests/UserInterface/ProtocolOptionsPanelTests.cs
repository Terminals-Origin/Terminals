using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Terminals.Connections;
using Terminals.Data;
using Terminals.Forms;
using Terminals.Forms.EditFavorite;
using Tests.FilePersisted;

namespace Tests.UserInterface
{
    /// <summary>
    /// We dont test:
    /// - RAS: not a real protocol, also it doesnt work now
    /// - Http and Https: they dont have any controls.
    /// </summary>
    [TestClass]
    public class ProtocolOptionsPanelTests
    {
        private const int EXPECTED_NUMBER = 5;
        private const string PROTOCOL_MESSAGE = "Protocol Roundtrip has to preserve the property value";

        private readonly ProtocolOptionsPanel protocolPanel = new ProtocolOptionsPanel();
        private readonly MockChildProtocolControl childProtocolControlMock = new MockChildProtocolControl();

        [TestInitialize]
        public void SetUp()
        {
            var irelevantPersistence = new Mock<IPersistence>().Object;
            var irelevantForm = new Mock<INewTerminalForm>().Object;
            var validator = new NewTerminalFormValidator(irelevantPersistence, irelevantForm);
            protocolPanel.RegisterValidations(validator);
        }

        // Rdp has 5 controls, so test separate roundtrip for selected property in each of them
        [TestMethod]
        public void Rdp_LoadSave_KeepsDisableWallPaper()
        {
            this.AssertExpectedPropertyValue<RdpOptions, bool>(ConnectionManager.RDP,
                  (options, newValue) => options.UserInterface.DisableWallPaper = newValue,
                  options => options.UserInterface.DisableWallPaper,
                  true);
        }
        
        [TestMethod]
        public void Rdp_LoadSave_KeepsLoadBalanceInfo()
        {
            this.AssertExpectedPropertyValue<RdpOptions, string>(ConnectionManager.RDP,
                  (options, newValue) => options.UserInterface.LoadBalanceInfo = newValue,
                  options => options.UserInterface.LoadBalanceInfo,
                  "LoadBalanceInfo");
        }

        [TestMethod]
        public void Rdp_LoadSave_KeepsRedirectSmartCards()
        {
            this.AssertExpectedPropertyValue<RdpOptions, bool>(ConnectionManager.RDP,
                  (options, newValue) => options.Redirect.SmartCards = newValue,
                  options => options.Redirect.SmartCards,
                  true);
        }

        [TestMethod]
        public void Rdp_LoadSave_KeepsWorkingFolder()
        {
            this.AssertExpectedPropertyValue<RdpOptions, string>(ConnectionManager.RDP,
                  SetWorkingFolder,
                  options => options.Security.WorkingFolder,
                  "WorkingFolder");
        }

        private static void SetWorkingFolder(RdpOptions options, string newValue)
        {
            // we have to enable the security, otherwise the save method skips the nested properties
            options.Security.Enabled = true;
            options.Security.WorkingFolder = newValue;
        }

        [TestMethod]
        public void Rdp_LoadSave_KeepsGatewayDomain()
        {
            this.AssertExpectedPropertyValue<RdpOptions, string>(ConnectionManager.RDP,
                  ConfigureTsGateway,
                  options => options.TsGateway.Security.Domain,
                  "TsGwDomain");
        }

        private void ConfigureTsGateway(RdpOptions options, string newValue)
        {
            options.TsGateway.SeparateLogin = true;
            options.TsGateway.Security.Domain = newValue;
        }

        [TestMethod]
        public void Vnc_LoadSave_KeepsDisplayNumber()
        {
            this.AssertExpectedPropertyValue<VncOptions, int>(ConnectionManager.VNC,
                  (options, newValue) => options.DisplayNumber = newValue,
                  options => options.DisplayNumber,
                  EXPECTED_NUMBER);
        }

        [TestMethod]
        public void SSh_LoadSave_KeepsSSHKeyFile()
        {
            // because of statics used in Settings.SshKeys
            FilePersistedTestLab.SetDefaultFileLocations();
            this.AssertExpectedPropertyValue<SshOptions, string>(ConnectionManager.SSH,
                  (options, newValue) => options.SSHKeyFile = newValue,
                  options => options.SSHKeyFile,
                  "ExpectedKeyFile");
        }

        [TestMethod]
        public void Telnet_LoadSave_KeepsColumns()
        {
            this.AssertExpectedPropertyValue<ConsoleOptions, int>(ConnectionManager.TELNET,
                  (options, newValue) => options.Columns = newValue,
                  options => options.Columns,
                  EXPECTED_NUMBER);
        }

        [TestMethod]
        public void Vmrc_LoadSave_KeepsReducedColorsMode()
        {
            this.AssertExpectedPropertyValue<VMRCOptions, bool>(ConnectionManager.VMRC,
                  (options, newValue) => options.ReducedColorsMode = newValue,
                  options => options.ReducedColorsMode,
                  true);
        }

        [TestMethod]
        public void Ica_LoadSave_KeepsReducedApplicationName()
        {
            this.AssertExpectedPropertyValue<ICAOptions, string>(ConnectionManager.ICA_CITRIX,
                  (options, newValue) => options.ApplicationName = newValue,
                  options => options.ApplicationName,
                  "ApplicationName");
        }

        private void AssertExpectedPropertyValue<TOptions, TExpectedValue>(string protocol,
            Action<TOptions, TExpectedValue> setter, 
            Func<TOptions, TExpectedValue> getter,
            TExpectedValue expectedValue)
            where TOptions : ProtocolOptions
        {
            Favorite source = CreateFavorite(protocol);
            var properties = (TOptions)source.ProtocolProperties;
            setter(properties, expectedValue);
            Favorite result = this.LoadAndSaveToResult(source);
            var resultProperties = (TOptions)result.ProtocolProperties;
            var resultValue = getter(resultProperties);
            Assert.AreEqual(expectedValue, resultValue, PROTOCOL_MESSAGE);
        }

        private Favorite LoadAndSaveToResult(Favorite source)
        {
            // do ti before used on the control to prevent source damage by the load
            Favorite result = CreateFavorite(source.Protocol);
            this.protocolPanel.ReloadControls(source.Protocol);
            this.protocolPanel.LoadFrom(source);
            this.protocolPanel.SaveTo(result);
            return result;
        }

        [TestMethod]
        public void ProtocolChilds_LoadFrom_CallsChildLoadFrom()
        {
            protocolPanel.Controls.Add(childProtocolControlMock);
            IFavorite irelevant = CreateFavorite(new List<IGroup>());
            protocolPanel.LoadFrom(irelevant);
            Assert.IsTrue(childProtocolControlMock.Loaded, "LoadFrom has to call LoadFrom on all his childs");
        }

        [TestMethod]
        public void ProtocolChilds_SaveTo_CallsChildSaveTo()
        {
            protocolPanel.Controls.Add(childProtocolControlMock);
            IFavorite irelevant = CreateFavorite(new List<IGroup>());
            protocolPanel.SaveTo(irelevant);
            Assert.IsTrue(childProtocolControlMock.Saved, "SveTo has to call SveTo on all his childs");
        }

        [TestMethod]
        public void Rdp_ReloadControls_ImplementIProtocolOptionsControl()
        {
            this.AssertLoadedControlsImplementInterface(ConnectionManager.RDP);
        }

        [TestMethod]
        public void Vnc_ReloadControls_ImplementIProtocolOptionsControl()
        {
            this.AssertLoadedControlsImplementInterface(ConnectionManager.VNC);
        }

        [TestMethod]
        public void SSh_ReloadControls_ImplementIProtocolOptionsControl()
        {
            this.AssertLoadedControlsImplementInterface(ConnectionManager.SSH);
        }

        [TestMethod]
        public void Telnet_ReloadControls_ImplementIProtocolOptionsControl()
        {
            this.AssertLoadedControlsImplementInterface(ConnectionManager.TELNET);
        }

        [TestMethod]
        public void Vmrc_ReloadControls_ImplementIProtocolOptionsControl()
        {
            this.AssertLoadedControlsImplementInterface(ConnectionManager.VMRC);
        }

        [TestMethod]
        public void Ica_ReloadControls_ImplementIProtocolOptionsControl()
        {
            this.AssertLoadedControlsImplementInterface(ConnectionManager.ICA_CITRIX);
        }

        private void AssertLoadedControlsImplementInterface(string protocol)
        {
            this.protocolPanel.ReloadControls(protocol);
            int validControls = this.protocolPanel.Controls.OfType<IProtocolOptionsControl>().Count();
            int allControls = this.protocolPanel.Controls.Count;
            const string MESSAGEFORMAT = "All {0} protocol option controls have to implement IProtocolOptionsControl";
            string message = string.Format(MESSAGEFORMAT, protocol);
            Assert.AreEqual(allControls, validControls, message);
        }

        private static Favorite CreateFavorite(string protocol)
        {
            Favorite source = CreateFavorite(new List<IGroup>());
            source.Protocol = protocol;
            return source;
        }

        internal static Favorite CreateFavorite(List<IGroup> groups)
        {
            var favoriteGroupsStub = new Mock<IFavoriteGroups>();
            favoriteGroupsStub.Setup(fg => fg.GetGroupsContainingFavorite(It.IsAny<Guid>())).Returns(groups);
            var favorite = new Favorite();
            favorite.AssignStores(new PersistenceSecurity(), favoriteGroupsStub.Object);
            return favorite;
        }
    }
}
