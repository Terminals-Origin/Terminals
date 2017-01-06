using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Terminals.Forms.Controls
{
    /// <summary>
    /// Build the image list icons from the plugins collection
    /// </summary>
    internal class ProtocolImageListBuilder
    {
        private readonly Func<IDictionary<string, Image>> provideIcons;

        public ProtocolImageListBuilder(Func<IDictionary<string, Image>> provideIcons)
        {
            this.provideIcons = provideIcons;
        }

        public void Build(ImageList imageList)
        {
            imageList.TransparentColor = Color.Magenta;
            // dont clear the image list, it may be is already populated with extra icons
            var icons = imageList.Images;

            foreach (KeyValuePair<string, Image> definition in provideIcons())
            {
                ReplaceIcon(icons, definition);
            }
        }

        private static void ReplaceIcon(ImageList.ImageCollection icons, KeyValuePair<string, Image> definition)
        {
            if (icons.ContainsKey(definition.Key))
                icons.RemoveByKey(definition.Key);

            icons.Add(definition.Key, definition.Value);
        }
    }
}
