using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms.Design;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace TabControl
{
    public class TabControlItemDesigner : ParentControlDesigner
    {
        #region Fields

        TabControlItem TabControl;

        #endregion

        #region Init & Dispose

        public override void Initialize(IComponent component)
        {
            base.Initialize(component);
            TabControl = component as TabControlItem;
        }

        #endregion

        #region Overrides

        protected override void PreFilterProperties(System.Collections.IDictionary properties)
        {
            base.PreFilterProperties(properties);

            properties.Remove("Dock");
            properties.Remove("AutoScroll");
            properties.Remove("AutoScrollMargin");
            properties.Remove("AutoScrollMinSize");
            properties.Remove("DockPadding");
            properties.Remove("DrawGrid");
            properties.Remove("Font");
            properties.Remove("Size");
            properties.Remove("Padding");
            properties.Remove("MinimumSize");
            properties.Remove("MaximumSize");
            properties.Remove("Margin");
            properties.Remove("ForeColor");
            properties.Remove("BackColor");
            properties.Remove("BackgroundImage");
            properties.Remove("BackgroundImageLayout");
            properties.Remove("Location");
            properties.Remove("RightToLeft");
            properties.Remove("GridSize");
            properties.Remove("ImeMode");
            properties.Remove("Anchor");
            properties.Remove("BorderStyle");
            properties.Remove("AutoSize");
            properties.Remove("AutoSizeMode");
        }

        public override SelectionRules SelectionRules
        {
            get
            {
                return 0;
            }
        }

        public override bool CanBeParentedTo(IDesigner parentDesigner)
        {
            return (parentDesigner.Component is TabControl);
        }

        protected override void OnPaintAdornments(PaintEventArgs pe)
        {
            if (TabControl != null)
            {
                using (Pen p = new Pen(SystemColors.ControlDark))
                {
                    p.DashStyle = DashStyle.Dash;
                    pe.Graphics.DrawRectangle(p, 0, 0, TabControl.Width - 1, TabControl.Height - 1);
                }
            }
        }

        #endregion
    }
}
