using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms.Design;
using System.ComponentModel.Design;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;

namespace TabControl
{
    public class TabControlDesigner : ParentControlDesigner
    {
        #region Fields

        IComponentChangeService changeService;

        #endregion

        #region Initialize & Dispose

        public override void Initialize(System.ComponentModel.IComponent component)
        {
            base.Initialize(component);
            
            //Design services
            changeService = (IComponentChangeService)GetService(typeof(IComponentChangeService));

            //Bind design events
            changeService.ComponentRemoving += new ComponentEventHandler(OnRemoving);

            Verbs.Add(new DesignerVerb("Add TabControl", new EventHandler(OnAddTabControl)));
            Verbs.Add(new DesignerVerb("Remove TabControl", new EventHandler(OnRemoveTabControl)));
        }

        protected override void Dispose(bool disposing)
        {
            changeService.ComponentRemoving -= new ComponentEventHandler(OnRemoving);

            base.Dispose(disposing);
        }

        #endregion

        #region EventHandler

        private void OnRemoving(object sender, ComponentEventArgs e)
        {
            IDesignerHost host = (IDesignerHost) GetService(typeof (IDesignerHost));

            //Removing a button
            if (e.Component is TabControlItem)
            {
                TabControlItem itm = e.Component as TabControlItem;
                if (Control.Items.Contains(itm))
                {
                    changeService.OnComponentChanging(Control, null);
                    Control.RemoveTab(itm);
                    changeService.OnComponentChanged(Control, null, null, null);
                    return;
                }
            }

            if (e.Component is TabControl)
            {
                for (int i = Control.Items.Count - 1; i >= 0; i--)
                {
                    TabControlItem itm = Control.Items[i];
                    changeService.OnComponentChanging(Control, null);
                    Control.RemoveTab(itm);
                    host.DestroyComponent(itm);
                    changeService.OnComponentChanged(Control, null, null, null);
                }
            }
        }

        private void OnAddTabControl(object sender, EventArgs e)
        {
            IDesignerHost host = (IDesignerHost)GetService(typeof(IDesignerHost));
            DesignerTransaction transaction = host.CreateTransaction("Add TabControl");
            TabControlItem itm = (TabControlItem)host.CreateComponent(typeof(TabControlItem));
            changeService.OnComponentChanging(Control, null);
            Control.AddTab(itm);
            int indx = Control.Items.IndexOf(itm) + 1;
            itm.Title = "TabControl Page " + indx.ToString();
            Control.SelectItem(itm);
            changeService.OnComponentChanged(Control, null, null, null);
            transaction.Commit();
        }

        private void OnRemoveTabControl(object sender, EventArgs e)
        {
            IDesignerHost host = (IDesignerHost)GetService(typeof(IDesignerHost));
            DesignerTransaction transaction = host.CreateTransaction("Remove Button");
            changeService.OnComponentChanging(Control, null);
            TabControlItem itm = Control.Items[Control.Items.Count - 1];
            Control.UnSelectItem(itm);
            Control.Items.Remove(itm);
            changeService.OnComponentChanged(Control, null, null, null);
            transaction.Commit();
        }

        #endregion

        #region Overrides

        protected override void PreFilterProperties(System.Collections.IDictionary properties)
        {
            base.PreFilterProperties(properties);

            properties.Remove("DockPadding");
            properties.Remove("DrawGrid");
            properties.Remove("Margin");
            properties.Remove("Padding");
            properties.Remove("BorderStyle");
            properties.Remove("ForeColor");
            properties.Remove("BackColor");
            properties.Remove("BackgroundImage");
            properties.Remove("BackgroundImageLayout");
            properties.Remove("GridSize");
            properties.Remove("ImeMode");
        }

        protected override void WndProc(ref Message msg)
        {
            if (msg.Msg == 0x201)
            {
                Point pt = Control.PointToClient(Cursor.Position);
                TabControlItem itm = Control.GetTabItemByPoint(pt);
                if (itm != null)
                {
                    Control.SelectedItem = itm;
                    ArrayList selection = new ArrayList();
                    selection.Add(itm);
                    ISelectionService selectionService = (ISelectionService)GetService(typeof(ISelectionService));
                    selectionService.SetSelectedComponents(selection);
                }
            }

            base.WndProc(ref msg);
        }

        //    if (msg.Msg == 0x200)
        //    {
        //        Control.Invalidate();
        //    }
        //    if (msg.Msg == 0x2a3)
        //    {
        //        Control.Invalidate();
        //    }

        //    base.WndProc(ref msg);
        //}

        //protected override bool GetHitTest(Point point)
        //{
        //    TabControlItem itm = Control.GetTabItemByPoint(Control.PointToClient(point));
        //    return (itm != null);
        //}

        public override System.Collections.ICollection AssociatedComponents
        {
            get
            {
                return Control.Items;
            }
        }

        public new virtual TabControl Control
        {
            get
            {
                return base.Control as TabControl;
            }
        }

        #endregion

    }
}
