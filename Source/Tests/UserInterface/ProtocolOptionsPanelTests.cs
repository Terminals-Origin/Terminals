﻿using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Terminals.Common.Connections;
using Terminals.Connections;
using Terminals.Connections.ICA;
using Terminals.Connections.VMRC;
using Terminals.Connections.VNC;
using Terminals.Data;
using Terminals.Data.Credentials;
using Terminals.Forms;
using Terminals.Forms.EditFavorite;
using Tests.Connections;
using Tests.FilePersisted;
using Tests.Helpers;
using Terminals.Plugins.Putty;

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
        private static readonly IPersistence persistence = TestMocksFactory.CreatePersistence().Object;

        private readonly ConnectionManager connectionManager = TestConnectionManager.Instance;

        private readonly ProtocolOptionsPanel protocolPanel = new ProtocolOptionsPanel();
        private readonly MockChildProtocolControl childProtocolControlMock = new MockChildProtocolControl();

        [TestInitialize]
        public void SetUp()
        {
            var irrelevantPersistence = TestMocksFactory.CreatePersistence();
            var irelevantForm = new Mock<INewTerminalForm>().Object;
            var validator = new NewTerminalFormValidator(irrelevantPersistence.Object, TestConnectionManager.Instance, irelevantForm);
            protocolPanel.RegisterValidations(validator);
            this.protocolPanel.CredentialsFactory = new GuardedCredentialFactory(irrelevantPersistence.Object);
            this.protocolPanel.Persistence = irrelevantPersistence.Object;
        }

        // Rdp has 5 controls, so test separate roundtrip for selected property in each of them
        [TestMethod]
        public void Rdp_LoadSave_KeepsDisableWallPaper()
        {
            this.AssertExpectedPropertyValue<RdpOptions, bool>(KnownConnectionConstants.RDP,
                  (options, newValue) => options.UserInterface.DisableWallPaper = newValue,
                  options => options.UserInterface.DisableWallPaper,
                  true);
        }
        
        [TestMethod]
        public void Rdp_LoadSave_KeepsLoadBalanceInfo()
        {
            this.AssertExpectedPropertyValue<RdpOptions, string>(KnownConnectionConstants.RDP,
                  (options, newValue) => options.UserInterface.LoadBalanceInfo = newValue,
                  options => options.UserInterface.LoadBalanceInfo,
                  "LoadBalanceInfo");
        }

        [TestMethod]
        public void Rdp_LoadSave_KeepsRedirectSmartCards()
        {
            this.AssertExpectedPropertyValue<RdpOptions, bool>(KnownConnectionConstants.RDP,
                  (options, newValue) => options.Redirect.SmartCards = newValue,
                  options => options.Redirect.SmartCards,
                  true);
        }

        [TestMethod]
        public void Rdp_LoadSave_KeepsWorkingFolder()
        {
            this.AssertExpectedPropertyValue<RdpOptions, string>(KnownConnectionConstants.RDP,
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
            this.AssertExpectedPropertyValue<RdpOptions, string>(KnownConnectionConstants.RDP,
                  ConfigureTsGateway,
                  GetDomain,
                  "TsGwDomain");
        }

        private static string GetDomain(RdpOptions options)
        {
            var guarded = new GuardedSecurity(persistence, options.TsGateway.Security);
            return guarded.Domain;
        }

        private static void ConfigureTsGateway(RdpOptions options, string newValue)
        {
            options.TsGateway.SeparateLogin = true;
            var guarded = new GuardedSecurity(persistence, options.TsGateway.Security);
            guarded.Domain = newValue;
        }



        [TestMethod]
        public void Vnc_LoadSave_KeepsDisplayNumber()
        {
            this.AssertExpectedPropertyValue<VncOptions, int>(VncConnectionPlugin.VNC,
                  (options, newValue) => options.DisplayNumber = newValue,
                  options => options.DisplayNumber,
                  EXPECTED_NUMBER);
        }

        [TestMethod]
        public void Vmrc_LoadSave_KeepsReducedColorsMode()
        {
            this.AssertExpectedPropertyValue<VMRCOptions, bool>(VmrcConnectionPlugin.VMRC,
                  (options, newValue) => options.ReducedColorsMode = newValue,
                  options => options.ReducedColorsMode,
                  true);
        }

        [TestMethod]
        public void Ica_LoadSave_KeepsReducedApplicationName()
        {
            this.AssertExpectedPropertyValue<ICAOptions, string>(ICAConnectionPlugin.ICA_CITRIX,
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
            this.ReloadProtocolPanel(source.Protocol);
            this.protocolPanel.LoadFrom(source);
            this.protocolPanel.SaveTo(result);
            return result;
        }

        [TestMethod]
        public void ProtocolChilds_LoadFrom_CallsChildLoadFrom()
        {
            protocolPanel.Controls.Add(childProtocolControlMock);
            IFavorite irelevant = TestMocksFactory.CreateFavorite();
            protocolPanel.LoadFrom(irelevant);
            Assert.IsTrue(childProtocolControlMock.Loaded, "LoadFrom has to call LoadFrom on all his childs");
        }

        [TestMethod]
        public void ProtocolChilds_SaveTo_CallsChildSaveTo()
        {
            protocolPanel.Controls.Add(childProtocolControlMock);
            IFavorite irelevant = TestMocksFactory.CreateFavorite();
            protocolPanel.SaveTo(irelevant);
            Assert.IsTrue(childProtocolControlMock.Saved, "SveTo has to call SveTo on all his childs");
        }

        [TestMethod]
        public void Rdp_ReloadControls_ImplementIProtocolOptionsControl()
        {
            this.AssertLoadedControlsImplementInterface(KnownConnectionConstants.RDP);
        }

        [TestMethod]
        public void Vnc_ReloadControls_ImplementIProtocolOptionsControl()
        {
            this.AssertLoadedControlsImplementInterface(VncConnectionPlugin.VNC);
        }

        [TestMethod]
        public void SSh_ReloadControls_ImplementIProtocolOptionsControl()
        {
            this.AssertLoadedControlsImplementInterface(SshConnectionPlugin.SSH);
        }

        [TestMethod]
        public void Telnet_ReloadControls_ImplementIProtocolOptionsControl()
        {
            this.AssertLoadedControlsImplementInterface(TelnetConnectionPlugin.TELNET);
        }

        [TestMethod]
        public void Vmrc_ReloadControls_ImplementIProtocolOptionsControl()
        {
            this.AssertLoadedControlsImplementInterface(VmrcConnectionPlugin.VMRC);
        }

        [TestMethod]
        public void Ica_ReloadControls_ImplementIProtocolOptionsControl()
        {
            this.AssertLoadedControlsImplementInterface(ICAConnectionPlugin.ICA_CITRIX);
        }

        private void AssertLoadedControlsImplementInterface(string protocol)
        {
            this.ReloadProtocolPanel(protocol);
            int validControls = this.protocolPanel.Controls.OfType<IProtocolOptionsControl>().Count();
            int allControls = this.protocolPanel.Controls.Count;
            const string MESSAGEFORMAT = "All {0} protocol option controls have to implement IProtocolOptionsControl";
            string message = string.Format(MESSAGEFORMAT, protocol);
            Assert.AreEqual(allControls, validControls, message);
        }

        private void ReloadProtocolPanel(string protocol)
        {
            var controls = this.connectionManager.CreateControls(protocol);
            this.protocolPanel.ReloadControls(controls);
        }

        private Favorite CreateFavorite(string protocol)
        {
            Favorite source = TestMocksFactory.CreateFavorite();
            this.connectionManager.ChangeProtocol(source, protocol);
            return source;
        }
    }
}
