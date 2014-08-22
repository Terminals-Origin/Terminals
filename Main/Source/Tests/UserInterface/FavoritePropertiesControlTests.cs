using System;
using System.Collections.Generic;
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
        public void ServerName_LoadSave_KeepsValue()
        {
            Favorite source = CreateFavorite();
            source.ServerName = EXPECTED_TEXT;
            Favorite result = this.LoadAndSaveToResult(source);
            Assert.AreEqual(EXPECTED_TEXT, result.ServerName, MESSAGE);
        }

        [TestMethod]
        public void BeforeExecuteCommand_LoadSave_KeepsValue()
        {
            Favorite source = CreateFavorite();
            source.ExecuteBeforeConnect.Command = EXPECTED_TEXT;
            Favorite result = this.LoadAndSaveToResult(source);
            Assert.AreEqual(EXPECTED_TEXT, result.ExecuteBeforeConnect.Command, MESSAGE);
        }

        [TestMethod]
        public void VncProcol_LoadSave_KeepsProtocolPropertiesType()
        {
            this.LoadPropertiesControl();
            Favorite source = CreateFavorite();
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
            Favorite source = CreateFavorite();
            this.propertiesControl.LoadFrom(source);
            List<IGroup> newlySelected = this.propertiesControl.GetNewlySelectedGroups();
            Assert.AreEqual(1, newlySelected.Count, "Not changed selection, has to return identical groups");
        }

        private Favorite LoadAndSaveToResult(Favorite source)
        {
            this.propertiesControl.LoadFrom(source);
            Favorite result = CreateFavorite();
            this.propertiesControl.SaveTo(result);
            return result;
        }

        private Favorite CreateFavorite()
        {
            var favoriteGroupsStub = new Mock<IFavoriteGroups>();
            favoriteGroupsStub.Setup(fg => fg.GetGroupsContainingFavorite(It.IsAny<Guid>())).Returns(groups);
            var favorite = new Favorite();
            favorite.AssignStores(new PersistenceSecurity(), favoriteGroupsStub.Object);
            return favorite;
        }
    }
}
