using System.Configuration;
using System.Drawing;
using System.Windows.Forms;

namespace Terminals
{
    /// <summary>
    /// Stored properties (Location, Size and State) of Windows Form in configuration file
    /// </summary>
    public class FormStateConfigElement : ConfigurationElement
    {
        private const string NAME = "name";
        private const string SIZE = "size";
        private const string LOCATION = "location";
        private const string STATE = "state";

        public FormStateConfigElement(Form form)
        {
            this.Name = form.Name;
            this.Size = form.Size;
            this.Location = form.Location;
            this.State = form.WindowState;
        }

        /// <summary>
        /// Default constructor, will use default values as defined
        /// </summary>
        public FormStateConfigElement()
        {
        }

        /// <summary>
        /// Constructor allowing name to be specified, will take the default values
        /// </summary>
        public FormStateConfigElement(string elementName)
        {
            this.Name = elementName;
        }

        [ConfigurationProperty(NAME, IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)this[NAME]; }
            set { this[NAME] = value; }
        }

        [ConfigurationProperty(SIZE, IsRequired = true)]
        public Size Size
        {
            get { return (Size)this[SIZE]; }
            set { this[SIZE] = value; }
        }

        [ConfigurationProperty(LOCATION, IsRequired = true)]
        public Point Location
        {
            get { return (Point)this[LOCATION]; }
            set { this[LOCATION] = value; }
        }

        [ConfigurationProperty(STATE, IsRequired = true)]
        public FormWindowState State
        {
            get { return (FormWindowState)this[STATE]; }
            set { this[STATE] = value; }
        }
    }
}