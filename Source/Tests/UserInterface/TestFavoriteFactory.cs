using System;
using System.Collections.Generic;
using Moq;
using Terminals.Data;

namespace Tests.UserInterface
{
    internal static class TestFavoriteFactory
    {
        internal static Favorite CreateFavorite(List<IGroup> groups)
        {
            var favoriteGroupsStub = new Mock<IFavoriteGroups>();
            favoriteGroupsStub.Setup(fg => fg.GetGroupsContainingFavorite(It.IsAny<Guid>())).Returns(groups);
            var favorite = new Favorite();
            favorite.AssignStores(favoriteGroupsStub.Object);
            return favorite;
        }
    }
}