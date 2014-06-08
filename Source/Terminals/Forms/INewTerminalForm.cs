using System;
using System.Windows.Forms;
using Terminals.Data;

namespace Terminals.Forms
{
    /// <summary>
    /// Representation of validated favorite edit Form.
    /// </summary>
    internal interface INewTerminalForm
    {
        bool EditingNew { get; }

        Guid EditedId { get; }

        string ProtocolText { get; }

        string ServerNameText { get; }

        string PortText { get; }

        bool ValidateChildren();

        void FillFavoriteFromControls(IFavorite favorite);

        void SetErrorInfo(Control validationBinding, string nameResultMessage);

        Uri GetFullUrlFromHttpTextBox();
    }
}