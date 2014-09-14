using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Terminals.Connections;
using Terminals.Data;
using Terminals.Forms;
using Terminals.Forms.EditFavorite;

namespace Tests.UserInterface
{
    /// <summary>
    /// Setup of roundtrip tests to check, if the favorite is properly loaded and saved by the user control.
    /// </summary>
    [TestClass]
    public class FavoritePropertiesControlTests
    {
        private const string EXPECTED_TEXT = "EXPECTED_TEXT";
        private const string MESSAGE = "Roundtrip should keep original value";
        
        private readonly FavoritePropertiesControl propertiesControl = new FavoritePropertiesControl();

        private readonly List<IGroup> groups = new List<IGroup>()
        {
            new Group() { Name = EXPECTED_TEXT }
        };

        private IPersistence irelevantPersistence;

        [TestInitialize]
        public void SetUp()
        {
            var persistenceStub = new Mock<IPersistence>();
            var credentials = new Mock<ICredentials>();
            credentials.Setup(cr => cr.GetEnumerator()).Returns(new List<ICredentialSet>().GetEnumerator());
            persistenceStub.SetupGet(p => p.Credentials).Returns(credentials.Object);
            var groupsStub = new Mock<IGroups>();
            groupsStub.Setup(g => g.GetEnumerator()).Returns(new List<IGroup>().GetEnumerator());
            persistenceStub.SetupGet(persistence => persistence.Groups).Returns(groupsStub.Object);
            this.irelevantPersistence = persistenceStub.Object;
            this.propertiesControl.AssignPersistence(irelevantPersistence);
        }

        [TestMethod]
        public void RdpGatewayNode_SelectNode_ChangesTitle()
        {
            this.LoadPropertiesControl();
            this.SimulateGatewayNodeSelection();
            var title = this.propertiesControl.Controls["titleLabel"];
            const string CHANGED_TITLE_MESSAGE = "Changing selected node to protocol options control, has to update control title";
            Assert.AreEqual("RDP Options - TS Gateway", title.Text, CHANGED_TITLE_MESSAGE);
        }

        /// <summary>
        /// Because the treeView.SelectedNode doesnt fire selection event from unit test.
        /// </summary>
        private void SimulateGatewayNodeSelection()
        {
            var treeView = this.propertiesControl.Controls["treeView"] as TreeView;
            var optionsPanel = this.propertiesControl.Controls["protocolOptionsPanel1"];
            optionsPanel.Show(); // otherwise the panel fires the selection it self.
            TreeNode tsgwNode = treeView.Nodes[4].Nodes[4];
            var wrapper = new PrivateObject(this.propertiesControl);
            var selection = new TreeViewEventArgs(tsgwNode);
            var privateFlags = BindingFlags.NonPublic | BindingFlags.Instance;
            wrapper.Invoke("TreeViewAfterSelect", privateFlags, new object[] { treeView, selection });
        }

        [TestMethod]
        public void ServerName_LoadSave_KeepsValue()
        {
            Favorite source = ProtocolOptionsPanelTests.CreateFavorite(this.groups);
            source.ServerName = EXPECTED_TEXT;
            Favorite result = this.LoadAndSaveToResult(source);
            Assert.AreEqual(EXPECTED_TEXT, result.ServerName, MESSAGE);
        }

        [TestMethod]
        public void Notes_LoadSave_KeepsValue()
        {
            Favorite source = ProtocolOptionsPanelTests.CreateFavorite(this.groups);
            source.Notes = EXPECTED_TEXT;
            // because of internal encoding of Notes, we need to cast to IFavorite here
            IFavorite result = this.LoadAndSaveToResult(source);
            Assert.AreEqual(EXPECTED_TEXT, result.Notes, MESSAGE);
        }

        [TestMethod]
        public void BeforeExecuteCommand_LoadSave_KeepsValue()
        {
            Favorite source = ProtocolOptionsPanelTests.CreateFavorite(this.groups);
            source.ExecuteBeforeConnect.Command = EXPECTED_TEXT;
            Favorite result = this.LoadAndSaveToResult(source);
            Assert.AreEqual(EXPECTED_TEXT, result.ExecuteBeforeConnect.Command, MESSAGE);
        }

        [TestMethod]
        public void VncProcol_LoadSave_KeepsProtocolPropertiesType()
        {
            this.LoadPropertiesControl();
            Favorite source = ProtocolOptionsPanelTests.CreateFavorite(this.groups);
            source.Protocol = ConnectionManager.VNC;
            Favorite result = this.LoadAndSaveToResult(source);
            const string PROTOCOL_MESSAGE = "Roundtrip has to preserve the protocol properties";
            Assert.IsInstanceOfType(result.ProtocolProperties, typeof(VncOptions), PROTOCOL_MESSAGE);
        }

        private void LoadPropertiesControl()
        {
            var formStub = new Mock<INewTerminalForm>();
            // return RDP doesnt play rule, because the validation asks only for non web protocol
            formStub.SetupGet(form => form.PortText).Returns(ConnectionManager.RDP);
            var formValidator = new NewTerminalFormValidator(this.irelevantPersistence, formStub.Object);
            this.propertiesControl.RegisterValidations(formValidator);
            this.propertiesControl.LoadContent();
        }

        [TestMethod]
        public void FavoriteGroup_LoadFrom_IsLoadedAsSelected()
        {
            Favorite source = ProtocolOptionsPanelTests.CreateFavorite(this.groups);
            this.propertiesControl.LoadFrom(source);
            List<IGroup> newlySelected = this.propertiesControl.GetNewlySelectedGroups();
            Assert.AreEqual(1, newlySelected.Count, "Not changed selection, has to return identical groups");
        }

        private Favorite LoadAndSaveToResult(Favorite source)
        {
            this.propertiesControl.LoadFrom(source);
            Favorite result = ProtocolOptionsPanelTests.CreateFavorite(this.groups);
            this.propertiesControl.SaveTo(result);
            return result;
        }
    }
}
