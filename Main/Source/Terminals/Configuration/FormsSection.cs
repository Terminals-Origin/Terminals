using System.Configuration;

namespace Terminals
{
	/// <summary>
	/// Special configuration section for Form states.
	/// </summary>
	public class FormsSection : ConfigurationSection
	{
        internal const string FORMS = "FormsSection";
	    private const string FORMSCOLLECTION = "FormsCollection";

        /// <summary>
        /// Declare a collection element represented in the configuration file by the sub-section
        /// Note: the "IsDefaultCollection = false" instructs the .NET Framework to build a nested section.
        /// </summary>
        [ConfigurationProperty(FORMSCOLLECTION, IsDefaultCollection = false)]
        public FormsCollection Forms
        {
            get
            {
                return (FormsCollection)base[FORMSCOLLECTION];
            }
            set
            {
                this[FORMSCOLLECTION] = value;
            }
        }

        public void AddForm(FormStateConfigElement form)
        {
        	this.Forms.Add(form);
        }
	}
}