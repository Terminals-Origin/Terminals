// *****************************************************************************
// 
//  (c) Crownwood Consulting Limited 2002 
//  All rights reserved. The software and associated documentation 
//  supplied hereunder are the proprietary information of Crownwood Consulting 
//	Limited, Haxey, North Lincolnshire, England and are supplied subject to 
//	licence terms.
// 
//  Magic Version 1.7 	www.dotnetmagic.com
// *****************************************************************************

using System;
using System.Xml;
using Crownwood.Magic.Collections;

namespace Crownwood.Magic.Collections
{
    public class StringCollection : CollectionWithEvents
    {
        public String Add(String value)
        {
            // Use base class to process actual collection operation
            base.List.Add(value as object);

            return value;
        }

        public void AddRange(String[] values)
        {
            // Use existing method to add each array entry
            foreach(String item in values)
                Add(item);
        }

        public void Remove(String value)
        {
            // Use base class to process actual collection operation
            base.List.Remove(value as object);
        }

        public void Insert(int index, String value)
        {
            // Use base class to process actual collection operation
            base.List.Insert(index, value as object);
        }

        public bool Contains(String value)
        {
			// Value comparison
			foreach(String s in base.List)
				if (value.Equals(s))
					return true;

			return false;
        }

        public bool Contains(StringCollection values)
        {
			foreach(String c in values)
			{
	            // Use base class to process actual collection operation
				if (Contains(c))
					return true;
			}

			return false;
        }

        public String this[int index]
        {
            // Use base class to process actual collection operation
            get { return (base.List[index] as String); }
        }

        public int IndexOf(String value)
        {
            // Find the 0 based index of the requested entry
            return base.List.IndexOf(value);
        }

		public void SaveToXml(string name, XmlTextWriter xmlOut)
		{
			xmlOut.WriteStartElement(name);
			xmlOut.WriteAttributeString("Count", this.Count.ToString());

			foreach(String s in base.List)
			{
				xmlOut.WriteStartElement("Item");
				xmlOut.WriteAttributeString("Name", s);
				xmlOut.WriteEndElement();
			}

			xmlOut.WriteEndElement();
		}

		public void LoadFromXml(string name, XmlTextReader xmlIn)
		{
			// Move to next xml node
			if (!xmlIn.Read())
				throw new ArgumentException("Could not read in next expected node");

			// Check it has the expected name
			if (xmlIn.Name != name)
				throw new ArgumentException("Incorrect node name found");

			this.Clear();

			// Grab raw position information
			string attrCount = xmlIn.GetAttribute(0);

			// Convert from string to proper types
			int count = int.Parse(attrCount);

			for(int index=0; index<count; index++)
			{
				// Move to next xml node
				if (!xmlIn.Read())
					throw new ArgumentException("Could not read in next expected node");

				// Check it has the expected name
				if (xmlIn.Name != "Item")
					throw new ArgumentException("Incorrect node name found");

				this.Add(xmlIn.GetAttribute(0));
			}

			if (count > 0)
			{
				// Move over the end element of the collection
				if (!xmlIn.Read())
					throw new ArgumentException("Could not read in next expected node");

				// Check it has the expected name
				if (xmlIn.Name != name)
					throw new ArgumentException("Incorrect node name found");
			}
		}
    }
}
