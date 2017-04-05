using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Data;
using Terminals.Forms.Controls;
using Tests.Connections;
using Terminals.Plugins.Putty;

namespace Tests.FilePersisted
{
    /// <summary>
    /// This test is called only against File pesistence, we dont test SQL persistence, 
    /// because it works on business layer, so the persistence type shouldnt matter.
    /// May be consider testing also the cancel method.
    /// </summary>
    [TestClass]
    public class SearchEngineTest : FilePersistedTestLab
    {
        private IFavorite favoriteSsh;

        private const string PROTOCOL = SshConnectionPlugin.SSH;

        private IFavorite favoriteNamed;

        private const string NAME = "Named";

        private IFavorite favoriteServerName;
        
        private const string SERVER_NAME = "ServerName";

        private IFavorite favoritePort;

        private const int PORT = 123;

        private IFavorite favoriteNotes;

        private const String NOTES_A = "another NOTES in another text";

        private const string NOTES = "NOTES";

        // it is a test, we dont care about the dispose
        private readonly CancellationTokenSource cancelSource = new CancellationTokenSource();

        [TestInitialize]
        public void PrepareTestSet()
        {
            this.Persistence.StartDelayedUpdate();
            this.favoriteSsh = CreateConfiguredFavorite(f => TestConnectionManager.Instance.ChangeProtocol(f, PROTOCOL));
            this.favoriteNamed = CreateConfiguredFavorite(f => f.Name = NAME);
            this.favoriteServerName = CreateConfiguredFavorite(f => f.ServerName = SERVER_NAME);
            this.favoritePort = CreateConfiguredFavorite(f => f.Port = PORT);
            this.favoriteNotes = CreateConfiguredFavorite(f => f.Notes = NOTES_A);
            this.Persistence.SaveAndFinishDelayedUpdate();
        }

        private IFavorite CreateConfiguredFavorite(Action<IFavorite> configure)
        {
            var favorite = this.AddFavorite();
            configure(favorite);
            this.Persistence.Favorites.Update(favorite);
            return favorite;
        }

        [TestMethod]
        public void SearchNotCaseSensitive()
        {
            this.TestSearchFavoriteByProperty(NOTES.ToLower(), this.favoriteNotes);
        }

        /// <summary>
        /// Checks wether all words in search phrase apply OR operator, e.g. "port name" searches for favorties containing port OR name.
        /// </summary>
        [TestMethod]
        public void SearchPhraseUsesOr()
        {
            string phrase = string.Format("{0} {1}", PORT, NOTES);
            List<IFavorite> foundFavorites = this.CallSearch(phrase);
            Assert.AreEqual(2, foundFavorites.Count, "Found different number of favorites when searching in multiple properties");
            Assert.IsTrue(foundFavorites.Contains(this.favoriteNotes), "Search didnt found favorite with notes");
            Assert.IsTrue(foundFavorites.Contains(this.favoritePort), "Search didnt found favorite with port number");
        }

        [TestMethod]
        public void SearchFavoriteByProperties()
        {
            this.TestSearchFavoriteByProperty(PROTOCOL, this.favoriteSsh);
            this.TestSearchFavoriteByProperty(NAME, this.favoriteNamed);
            this.TestSearchFavoriteByProperty(SERVER_NAME, this.favoriteServerName);
            this.TestSearchFavoriteByProperty(PORT.ToString(), this.favoritePort);
            // this tests also, if the search phrase is only a part of searched property
            this.TestSearchFavoriteByProperty(NOTES, this.favoriteNotes);
        }

        private void TestSearchFavoriteByProperty(string phrase, IFavorite expected)
        {
            List<IFavorite> foundFavorites = this.CallSearch(phrase);
            AssertPropertySearchResutl(foundFavorites, expected);
        }

        private List<IFavorite> CallSearch(string phrase)
        {
            var searchEngine = new FavoritesSearch(this.Persistence.Favorites, this.cancelSource.Token, phrase);
            Task<List<IFavorite>> searchTask = searchEngine.FindAsync();
            searchTask.Wait();
            return searchTask.Result;
        }

        private static void AssertPropertySearchResutl(List<IFavorite> foundFavorites, IFavorite expected)
        {
            Assert.AreEqual(1, foundFavorites.Count, "Found different number of favorites by property");
            var foundFavorite = foundFavorites.FirstOrDefault(result => result.StoreIdEquals(expected));
            Assert.IsNotNull(foundFavorite, "Search has found different favorite");
        }
    }
}
