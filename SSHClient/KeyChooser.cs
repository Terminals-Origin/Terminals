
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace Terminals
{
    public partial class KeyChooser : UserControl
    {
        public KeyChooser()
        {
            InitializeComponent();
        }
        public override string Text
        {
        	get
        	{
        		return box.Text;
        	}
        	set
        	{
        		box.Text = value;
        	}
        }
       	public int SelectedIndex
        {
        	get
        	{
        		return box.SelectedIndex;
        	}
        	set
        	{
        		box.SelectedIndex = value;
        	}
        }
       	public ComboBox.ObjectCollection Items
        {
        	get
        	{
        		return box.Items;
        	}
        }
    }
}
