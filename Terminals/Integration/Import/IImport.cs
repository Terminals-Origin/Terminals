using System;
using System.Collections.Generic;
using System.Text;

namespace Terminals.Integration.Import
{
    interface IImport
    {
        FavoriteConfigurationElementCollection ImportFavorites(string Filename);
        string KnownExtension
        {
            get;
        }

    }
}