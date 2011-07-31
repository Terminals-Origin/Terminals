using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Terminals.Forms
{
    internal abstract class OptionDialogCategoryPanel : Panel
    {
        private Panel panel1;

        public OptionDialogCategoryPanel()
        {
            this.panel1.Dock = DockStyle.Fill;
            this.panel1.AutoScroll = true;
        }

        public abstract void Init();
        public abstract Boolean Save();
    }
}
