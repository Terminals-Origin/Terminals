/* RssModuleItem.cs
 * ================
 * 
 * RSS.NET (http://rss-net.sf.net/)
 * Copyright © 2002, 2003 George Tsiokos. All Rights Reserved.
 * 
 * RSS 2.0 (http://blogs.law.harvard.edu/tech/rss)
 * RSS 2.0 is offered by the Berkman Center for Internet & Society at 
 * Harvard Law School under the terms of the Attribution/Share Alike 
 * Creative Commons license.
 * 
 * Permission is hereby granted, free of charge, to any person obtaining 
 * a copy of this software and associated documentation files (the "Software"), 
 * to deal in the Software without restriction, including without limitation 
 * the rights to use, copy, modify, merge, publish, distribute, sublicense, 
 * and/or sell copies of the Software, and to permit persons to whom the 
 * Software is furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
 * THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN 
 * THE SOFTWARE.
*/
using System;
using System.Collections;

namespace Unified.Rss
{
	/// <summary>A module may contain any number of items (either channel-based or item-based).</summary>
	[System.Serializable()]
	public class RssModuleItem : RssElement
	{
		private bool _bRequired = false;
		private string _sElementName = RssDefault.String;
		private string _sElementText = RssDefault.String;
		private RssModuleItemCollection _rssSubElements = new RssModuleItemCollection();

		/// <summary>Initialize a new instance of the RssModuleItem class</summary>
		public RssModuleItem()
		{
		}

		/// <summary>Initialize a new instance of the RssModuleItem class</summary>
		/// <param name="name">The name of this RssModuleItem.</param>
		public RssModuleItem(string name)
		{
			this._sElementName = RssDefault.Check(name);
		}

		/// <summary>Initialize a new instance of the RssModuleItem class</summary>
		/// <param name="name">The name of this RssModuleItem.</param>
		/// <param name="required">Is text required for this RssModuleItem?</param>
		public RssModuleItem(string name, bool required) : this(name)
		{
			this._bRequired = required;
		}

		/// <summary>Initialize a new instance of the RssModuleItem class</summary>
		/// <param name="name">The name of this RssModuleItem.</param>
		/// <param name="text">The text contained within this RssModuleItem.</param>
		public RssModuleItem(string name, string text) : this(name)
		{
			this._sElementText = RssDefault.Check(text);
		}

		/// <summary>Initialize a new instance of the RssModuleItem class</summary>
		/// <param name="name">The name of this RssModuleItem.</param>
		/// <param name="required">Is text required for this RssModuleItem?</param>
		/// <param name="text">The text contained within this RssModuleItem.</param>
		public RssModuleItem(string name, bool required, string text) : this(name, required)
		{
			this._sElementText = RssDefault.Check(text);
		}

		/// <summary>Initialize a new instance of the RssModuleItem class</summary>
		/// <param name="name">The name of this RssModuleItem.</param>
		/// <param name="text">The text contained within this RssModuleItem.</param>
		/// <param name="subElements">The sub-elements of this RssModuleItem (if any exist).</param>
		public RssModuleItem(string name, string text, RssModuleItemCollection subElements) : this(name, text)
		{
			this._rssSubElements = subElements;
		}

		/// <summary>Initialize a new instance of the RssModuleItem class</summary>
		/// <param name="name">The name of this RssModuleItem.</param>
		/// <param name="required">Is text required for this RssModuleItem?</param>
		/// <param name="text">The text contained within this RssModuleItem.</param>
		/// <param name="subElements">The sub-elements of this RssModuleItem (if any exist).</param>
		public RssModuleItem(string name, bool required, string text, RssModuleItemCollection subElements) : this(name, required, text)
		{
			this._rssSubElements = subElements;
		}

		/// <summary>Returns a string representation of the current Object.</summary>
		/// <returns>The item's title, description, or "RssModuleItem" if the title and description are blank.</returns>
		public override string ToString()
		{
			if (this._sElementName != null)
				return this._sElementName;
			else if (this._sElementText != null)
				return this._sElementText;
			else
				return "RssModuleItem";
		}

		/// <summary>
		/// The name of this RssModuleItem.
		/// </summary>
		public string Name
		{
			get { return this._sElementName; }
			set { this._sElementName = RssDefault.Check(value); }
		}

		/// <summary>
		/// The text contained within this RssModuleItem.
		/// </summary>
		public string Text
		{
			get { return this._sElementText; }
			set { this._sElementText = RssDefault.Check(value); }
		}

		/// <summary>
		/// The sub-elements of this RssModuleItem (if any exist).
		/// </summary>
		public RssModuleItemCollection SubElements
		{
			get { return this._rssSubElements; }
			set { this._rssSubElements = value;}
		}

		/// <summary>
		/// Is text for this element required?
		/// </summary>
		public bool IsRequired
		{
			get { return this._bRequired; }
		}
	}
}
