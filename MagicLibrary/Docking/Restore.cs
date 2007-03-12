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
using System.Drawing;
using System.Windows.Forms;
using Crownwood.Magic.Common;
using Crownwood.Magic.Docking;
using Crownwood.Magic.Collections;

namespace Crownwood.Magic.Docking
{
    public class Restore
    {
		// Instance fields
		protected Restore _child;

		public Restore()
		{
			// Default state
			_child = null;
		}

		public Restore(Restore child)
		{
			// Remember parameter
			_child = child;
		}

        public Restore Child
        {
            get { return _child; }
            set { _child = value; }
        }

        public virtual void PerformRestore(DockingManager dm) {}
		public virtual void PerformRestore(Window w) {}
        public virtual void PerformRestore(Zone z) {}
        public virtual void PerformRestore() {}

		public virtual void Reconnect(DockingManager dm)
		{
			if (_child != null)
				_child.Reconnect(dm);
		}

		public virtual void SaveToXml(XmlTextWriter xmlOut)
		{
			// Must define my type so loading can recreate my instance
			xmlOut.WriteAttributeString("Type", this.GetType().ToString());

			SaveInternalToXml(xmlOut);

			// Output the child object			
			xmlOut.WriteStartElement("Child");

			if (_child == null)
				xmlOut.WriteAttributeString("Type", "null");
			else
				_child.SaveToXml(xmlOut);

			xmlOut.WriteEndElement();
		}

		public virtual void LoadFromXml(XmlTextReader xmlIn, int formatVersion)
		{
			LoadInternalFromXml(xmlIn, formatVersion);

			// Move to next xml node
			if (!xmlIn.Read())
				throw new ArgumentException("Could not read in next expected node");

			// Check it has the expected name
			if (xmlIn.Name != "Child")
				throw new ArgumentException("Node 'Child' expected but not found");

			string type = xmlIn.GetAttribute(0);
			
			if (type != "null")
				_child = CreateFromXml(xmlIn, false, formatVersion);

			// Move past the end element
			if (!xmlIn.Read())
				throw new ArgumentException("Could not read in next expected node");
		
			// Check it has the expected name
			if (xmlIn.NodeType != XmlNodeType.EndElement)
				throw new ArgumentException("EndElement expected but not found");
		}

		public virtual void SaveInternalToXml(XmlTextWriter xmlOut) {}
		public virtual void LoadInternalFromXml(XmlTextReader xmlIn, int formatVersion) {}

		public static Restore CreateFromXml(XmlTextReader xmlIn, bool readIn, int formatVersion)
		{
			if (readIn)
			{
				// Move to next xml node
				if (!xmlIn.Read())
					throw new ArgumentException("Could not read in next expected node");
			}
			
			// Grab type name of the object to create
			string attrType = xmlIn.GetAttribute(0);

			// Convert from string to a Type description object
			Type newType = Type.GetType(attrType);

			// Create an instance of this object which must derive from Restore base class
			Restore newRestore = newType.Assembly.CreateInstance(attrType) as Restore;

			// Ask the object to load itself
			newRestore.LoadFromXml(xmlIn, formatVersion);

			return newRestore;
		}
	}

	public class RestoreContent : Restore
	{
		// Instance fields
		protected String _title;
		protected Content _content;

		public RestoreContent()
			: base()
		{
			// Default state
			_title = "";
			_content = null;
		}

		public RestoreContent(Content content)
			: base()
		{
			// Remember parameter
			_title = content.Title;
			_content = content;
		}

		public RestoreContent(Restore child, Content content)
			: base(child)
		{
			// Remember parameter
			_title = content.Title;
			_content = content;
		}

		public override void Reconnect(DockingManager dm)
		{
			// Connect to the current instance of required content object
			_content = dm.Contents[_title];

			base.Reconnect(dm);
		}

		public override void SaveInternalToXml(XmlTextWriter xmlOut)
		{
			base.SaveInternalToXml(xmlOut);
			xmlOut.WriteStartElement("Content");
			xmlOut.WriteAttributeString("Name", _content.Title);
			xmlOut.WriteEndElement();				
		}

		public override void LoadInternalFromXml(XmlTextReader xmlIn, int formatVersion)
		{
			base.LoadInternalFromXml(xmlIn, formatVersion);

			// Move to next xml node
			if (!xmlIn.Read())
				throw new ArgumentException("Could not read in next expected node");

			// Check it has the expected name
			if (xmlIn.Name != "Content")
				throw new ArgumentException("Node 'Content' expected but not found");

			// Grab type name of the object to create
			_title = xmlIn.GetAttribute(0);
		}
	}
	
	public class RestoreContentState : RestoreContent
	{
		// Instance fields
		protected State _state;

		public RestoreContentState()
			: base()
		{
		}

		public RestoreContentState(State state, Content content)
			: base(content)
		{
			// Remember parameter
			_state = state;
		}

		public RestoreContentState(Restore child, State state, Content content)
			: base(child, content)
		{
			// Remember parameter
			_state = state;
		}

		public override void PerformRestore(DockingManager dm)
		{
			// Use the existing DockingManager method that will create a Window appropriate for 
			// this Content and then add a new Zone for hosting the Window. It will always place
			// the Zone at the inner most level
			dm.AddContentWithState(_content, _state);				
		}

		public override void SaveInternalToXml(XmlTextWriter xmlOut)
		{
			base.SaveInternalToXml(xmlOut);
			xmlOut.WriteStartElement("State");
			xmlOut.WriteAttributeString("Value", _state.ToString());
			xmlOut.WriteEndElement();				
		}

		public override void LoadInternalFromXml(XmlTextReader xmlIn, int formatVersion)
		{
			base.LoadInternalFromXml(xmlIn, formatVersion);

			// Move to next xml node
			if (!xmlIn.Read())
				throw new ArgumentException("Could not read in next expected node");

			// Check it has the expected name
			if (xmlIn.Name != "State")
				throw new ArgumentException("Node 'State' expected but not found");

			// Grab type state of the object to create
			string attrState = xmlIn.GetAttribute(0);

			// Convert from string to enumeration value
			_state = (State)Enum.Parse(typeof(State), attrState);
		}
	}
	
	public class RestoreAutoHideState : RestoreContentState
	{
	    // Instance fields
	    
	    public RestoreAutoHideState()
	        : base()
	    {
	    }
        
        public RestoreAutoHideState(State state, Content content)
            : base(state, content)
        {
        }

        public RestoreAutoHideState(Restore child, State state, Content content)
            : base(child, state, content)
        {
        }
    
        public override void PerformRestore(DockingManager dm)
        {
            // Create collection of Contents to auto hide
            ContentCollection cc = new ContentCollection();
            
            // In this case, there is only one
            cc.Add(_content);
        
            // Add to appropriate AutoHidePanel based on _state
            dm.AutoHideContents(cc, _state);
        }
    }

    public class RestoreAutoHideAffinity : RestoreAutoHideState
    {
        // Instance fields
        protected StringCollection _next;
        protected StringCollection _previous;
        protected StringCollection _nextAll;
        protected StringCollection _previousAll;

        public RestoreAutoHideAffinity()
            : base()
        {
            // Must always point to valid reference
            _next = new StringCollection();
            _previous = new StringCollection();
            _nextAll = new StringCollection();
            _previousAll = new StringCollection();
        }

        public RestoreAutoHideAffinity(Restore child, 
                                       State state,
                                       Content content, 
                                       StringCollection next,
                                       StringCollection previous,
                                       StringCollection nextAll,
                                       StringCollection previousAll)
        : base(child, state, content)
        {
            // Remember parameters
            _next = next;				
            _previous = previous;	
            _nextAll = nextAll;				
            _previousAll = previousAll;	
        }

        public override void PerformRestore(DockingManager dm)
        {   
            // Get the correct target panel from state
            AutoHidePanel ahp = dm.AutoHidePanelForState(_state);
            
            ahp.AddContent(_content, _next, _previous, _nextAll, _previousAll);
        }

        public override void SaveInternalToXml(XmlTextWriter xmlOut)
        {
            base.SaveInternalToXml(xmlOut);
            _next.SaveToXml("Next", xmlOut);
            _previous.SaveToXml("Previous", xmlOut);
            _nextAll.SaveToXml("NextAll", xmlOut);
            _previousAll.SaveToXml("PreviousAll", xmlOut);
        }

        public override void LoadInternalFromXml(XmlTextReader xmlIn, int formatVersion)
        {
            base.LoadInternalFromXml(xmlIn, formatVersion);
            _next.LoadFromXml("Next", xmlIn);
            _previous.LoadFromXml("Previous", xmlIn);
            _nextAll.LoadFromXml("NextAll", xmlIn);
            _previousAll.LoadFromXml("PreviousAll", xmlIn);
        }
    }

	public class RestoreContentDockingAffinity : RestoreContentState
	{
		// Instance fields
		protected Size _size;
		protected Point _location;
		protected StringCollection _best;
		protected StringCollection _next;
		protected StringCollection _previous;
		protected StringCollection _nextAll;
		protected StringCollection _previousAll;

		public RestoreContentDockingAffinity()
			: base()
		{
			// Must always point to valid reference
			_best = new StringCollection();
			_next = new StringCollection();
			_previous = new StringCollection();
			_nextAll = new StringCollection();
			_previousAll = new StringCollection();
		}

		public RestoreContentDockingAffinity(Restore child, 
										     State state, 
											 Content content, 
											 StringCollection best,
											 StringCollection next,
											 StringCollection previous,
											 StringCollection nextAll,
											 StringCollection previousAll)
			: base(child, state, content)
		{
			// Remember parameters
			_best = best;
			_next = next;
			_previous = previous;
			_nextAll = nextAll;
			_previousAll = previousAll;
			_size = content.DisplaySize;
			_location = content.DisplayLocation;
		}

		public override void PerformRestore(DockingManager dm)
		{
			int count = dm.Container.Controls.Count;

			int min = -1;
			int max = count;

			if (dm.InnerControl != null)
				min = dm.Container.Controls.IndexOf(dm.InnerControl);

			if (dm.OuterControl != null)
				max = dm.OuterControlIndex();

			int beforeIndex = -1;
			int afterIndex = max;
			int beforeAllIndex = -1;
			int afterAllIndex = max;

			// Create a collection of the Zones in the appropriate direction
			for(int index=0; index<count; index++)
			{
				Zone z = dm.Container.Controls[index] as Zone;

				if (z != null)
				{
					StringCollection sc = ZoneHelper.ContentNames(z);
					
					if (_state == z.State)
					{
						if (sc.Contains(_best))
						{
							// Can we delegate to a child Restore object
							if (_child != null)
								_child.PerformRestore(z);
							else
							{
								// Just add an appropriate Window to start of the Zone
								dm.AddContentToZone(_content, z, 0);
							}
							return;
						}

						// If the WindowContent contains a Content previous to the target
						if (sc.Contains(_previous))
						{
							if (index > beforeIndex)
								beforeIndex = index;
						}
						
						// If the WindowContent contains a Content next to the target
						if (sc.Contains(_next))
						{
							if (index < afterIndex)
								afterIndex = index;
						}
					}
					else
					{
						// If the WindowContent contains a Content previous to the target
						if (sc.Contains(_previousAll))
						{
							if (index > beforeAllIndex)
								beforeAllIndex = index;
						}
						
						// If the WindowContent contains a Content next to the target
						if (sc.Contains(_nextAll))
						{
							if (index < afterAllIndex)
								afterAllIndex = index;
						}
					}
				}
			}

			dm.Container.SuspendLayout();

			// Create a new Zone with correct State
			Zone newZ = dm.CreateZoneForContent(_state);

			// Restore the correct content size/location values
			_content.DisplaySize = _size;
			_content.DisplayLocation = _location;

			// Add an appropriate Window to start of the Zone
			dm.AddContentToZone(_content, newZ, 0);

			// Did we find a valid 'before' Zone?
			if (beforeIndex != -1)
			{
				// Try and place more accurately according to other edge Zones
				if (beforeAllIndex > beforeIndex)
					beforeIndex = beforeAllIndex;

				// Check against limits
				if (beforeIndex >= max)
					beforeIndex = max - 1;

				dm.Container.Controls.SetChildIndex(newZ, beforeIndex + 1);
			}
			else
			{
				// Try and place more accurately according to other edge Zones
				if (afterAllIndex < afterIndex)
					afterIndex = afterAllIndex;

				// Check against limits
				if (afterIndex <= min)
					afterIndex = min + 1;
				
				if (afterIndex > min)
					dm.Container.Controls.SetChildIndex(newZ, afterIndex);
				else
				{
					// Set the Zone to be the least important of our Zones
					dm.ReorderZoneToInnerMost(newZ);
				}
			}

			dm.Container.ResumeLayout();
		}

		public override void SaveInternalToXml(XmlTextWriter xmlOut)
		{
			base.SaveInternalToXml(xmlOut);
			xmlOut.WriteStartElement("Position");
			xmlOut.WriteAttributeString("Size", ConversionHelper.SizeToString(_size));
			xmlOut.WriteAttributeString("Location", ConversionHelper.PointToString(_location));
			xmlOut.WriteEndElement();				
			_best.SaveToXml("Best", xmlOut);
			_next.SaveToXml("Next", xmlOut);
			_previous.SaveToXml("Previous", xmlOut);
			_nextAll.SaveToXml("NextAll", xmlOut);
			_previousAll.SaveToXml("PreviousAll", xmlOut);
		}

		public override void LoadInternalFromXml(XmlTextReader xmlIn, int formatVersion)
		{
			base.LoadInternalFromXml(xmlIn, formatVersion);

			// Move to next xml node
			if (!xmlIn.Read())
				throw new ArgumentException("Could not read in next expected node");

			// Check it has the expected name
			if (xmlIn.Name != "Position")
				throw new ArgumentException("Node 'Position' expected but not found");

			// Grab raw position information
			string attrSize = xmlIn.GetAttribute(0);
			string attrLocation = xmlIn.GetAttribute(1);

			// Convert from string to proper types
			_size = ConversionHelper.StringToSize(attrSize);
			_location = ConversionHelper.StringToPoint(attrLocation);

			_best.LoadFromXml("Best", xmlIn);
			_next.LoadFromXml("Next", xmlIn);
			_previous.LoadFromXml("Previous", xmlIn);
			_nextAll.LoadFromXml("NextAll", xmlIn);
			_previousAll.LoadFromXml("PreviousAll", xmlIn);
		}
	}

	public class RestoreContentFloatingAffinity : RestoreContentState
	{
		// Instance fields
		protected Size _size;
		protected Point _location;
		protected StringCollection _best;
		protected StringCollection _associates;

		public RestoreContentFloatingAffinity()
			: base()
		{
			// Must always point to valid reference
			_best = new StringCollection();
			_associates = new StringCollection();
		}

		public RestoreContentFloatingAffinity(Restore child, 
										      State state, 
											  Content content, 
											  StringCollection best,
											  StringCollection associates)
			: base(child, state, content)
		{
			// Remember parameters
			_best = best;
			_associates = associates;
			_size = content.DisplaySize;
			_location = content.DisplayLocation;

			// Remove target from collection of friends
			if (_best.Contains(content.Title))
				_best.Remove(content.Title);

			// Remove target from collection of associates
			if (_associates.Contains(content.Title))
				_associates.Remove(content.Title);
		}

		public override void PerformRestore(DockingManager dm)
		{
			// Grab a list of all floating forms
			Form[] owned = dm.Container.FindForm().OwnedForms;

			FloatingForm target = null;

			// Find the match to one of our best friends
			foreach(Form f in owned)
			{
				FloatingForm ff = f as FloatingForm;

				if (ff != null)
				{
					if (ZoneHelper.ContentNames(ff.Zone).Contains(_best))
					{
						target = ff;
						break;
					}
				}
			}

			// If no friends then try associates as second best option
			if (target == null)
			{
				// Find the match to one of our best friends
				foreach(Form f in owned)
				{
					FloatingForm ff = f as FloatingForm;

					if (ff != null)
					{
						if (ZoneHelper.ContentNames(ff.Zone).Contains(_associates))
						{
							target = ff;
							break;
						}
					}
				}
			}

			// If we found a friend/associate, then restore to it
			if (target != null)
			{
				// We should have a child and be able to restore to its Zone
				_child.PerformRestore(target.Zone);
			}
			else
			{
				// Restore its location/size
				_content.DisplayLocation = _location;
				_content.DisplaySize = _size;

				// Use the docking manage method to create us a new Floating Window at correct size/location
				dm.AddContentWithState(_content, State.Floating);
			}
		}

		public override void SaveInternalToXml(XmlTextWriter xmlOut)
		{
			base.SaveInternalToXml(xmlOut);
			xmlOut.WriteStartElement("Position");
			xmlOut.WriteAttributeString("Size", ConversionHelper.SizeToString(_size));
			xmlOut.WriteAttributeString("Location", ConversionHelper.PointToString(_location));
			xmlOut.WriteEndElement();				
			_best.SaveToXml("Best", xmlOut);
			_associates.SaveToXml("Associates", xmlOut);
		}

		public override void LoadInternalFromXml(XmlTextReader xmlIn, int formatVersion)
		{
			base.LoadInternalFromXml(xmlIn, formatVersion);

			// Move to next xml node
			if (!xmlIn.Read())
				throw new ArgumentException("Could not read in next expected node");

			// Check it has the expected name
			if (xmlIn.Name != "Position")
				throw new ArgumentException("Node 'Position' expected but not found");

			// Grab raw position information
			string attrSize = xmlIn.GetAttribute(0);
			string attrLocation = xmlIn.GetAttribute(1);

			// Convert from string to proper types
			_size = ConversionHelper.StringToSize(attrSize);
			_location = ConversionHelper.StringToPoint(attrLocation);

			_best.LoadFromXml("Best", xmlIn);
			_associates.LoadFromXml("Associates", xmlIn);
		}
	}

	public class RestoreZoneAffinity : RestoreContent
	{
		// Instance fields
		protected Decimal _space;
		protected StringCollection _best;
		protected StringCollection _next;
		protected StringCollection _previous;

		public RestoreZoneAffinity()
			: base()
		{
			// Default state
			_space = 50m;

			// Must always point to valid reference
			_best = new StringCollection();
			_next = new StringCollection();
			_previous = new StringCollection();
		}

		public RestoreZoneAffinity(Restore child, 
								   Content content, 
								   StringCollection best,
								   StringCollection next,
								   StringCollection previous)
			: base(child, content)
		{
			// Remember parameters
			_best = best;				
			_next = next;				
			_previous = previous;	
			
			if (content.Visible)			
				_space = content.ParentWindowContent.ZoneArea;
			else
				_space = 50m;
		}

		public override void PerformRestore(Zone z)
		{
			int count = z.Windows.Count;
			int beforeIndex = - 1;
			int afterIndex = count;
		
			// Find the match to one of our best friends
			for(int index=0; index<count; index++)
			{
				WindowContent wc = z.Windows[index] as WindowContent;

				if (wc != null)
				{
					// If this WindowContent contains a best friend, then add ourself here as well
					if (wc.Contents.Contains(_best))
					{
						if (_child == null)
						{
							// If we do not have a Restore object for the Window then just add
							// into the WindowContent at the end of the existing Contents
							wc.Contents.Add(_content);
						}
						else
						{
							// Get the child to restore as best as possible inside WindowContent
							_child.PerformRestore(wc);
						}

						return;
					}

					// If the WindowContent contains a Content previous to the target
					if (wc.Contents.Contains(_previous))
					{
						if (index > beforeIndex)
							beforeIndex = index;
					}
					
					// If the WindowContent contains a Content next to the target
					if (wc.Contents.Contains(_next))
					{
						if (index < afterIndex)
							afterIndex = index;
					}
				}
			}

			// If we get here then we did not find any best friends, this 
			// means we need to create a new WindowContent to host the Content.
			Window newW =  z.DockingManager.CreateWindowForContent(_content);

			ZoneSequence zs = z as ZoneSequence;

			// If this is inside a ZoneSequence instance
			if (zs != null)
			{
				// Do not reposition the Windows on the .Insert but instead ignore the
				// reposition and let it happen in the .ModifyWindowSpace. This reduces
				// the flicker that would occur otherwise
				zs.SuppressReposition();
			}

			// Need to find the best place in the order for the Content, start by
			// looking for the last 'previous' content and place immediately after it
			if (beforeIndex >= 0)
			{
				// Great, insert after it
				z.Windows.Insert(beforeIndex + 1, newW);
			}
			else
			{
				// No joy, so find the first 'next' content and place just before it, if
				// none are found then just add to the end of the collection.
				z.Windows.Insert(afterIndex, newW);
			}

			// If this is inside a ZoneSequence instance
			if (zs != null)
			{
				// We want to try and allocate the correct Zone space
				zs.ModifyWindowSpace(newW, _space);
			}
		}

		public override void SaveInternalToXml(XmlTextWriter xmlOut)
		{
			base.SaveInternalToXml(xmlOut);
			xmlOut.WriteStartElement("Space");
			xmlOut.WriteAttributeString("Value", _space.ToString());
			xmlOut.WriteEndElement();				
			_best.SaveToXml("Best", xmlOut);
			_next.SaveToXml("Next", xmlOut);
			_previous.SaveToXml("Previous", xmlOut);
		}

		public override void LoadInternalFromXml(XmlTextReader xmlIn, int formatVersion)
		{
			base.LoadInternalFromXml(xmlIn, formatVersion);

			// Move to next xml node
			if (!xmlIn.Read())
				throw new ArgumentException("Could not read in next expected node");

			// Check it has the expected name
			if (xmlIn.Name != "Space")
				throw new ArgumentException("Node 'Space' expected but not found");

			// Grab raw position information
			string attrSpace = xmlIn.GetAttribute(0);

			// Convert from string to proper type
			_space = Decimal.Parse(attrSpace);

			_best.LoadFromXml("Best", xmlIn);
			_next.LoadFromXml("Next", xmlIn);
			_previous.LoadFromXml("Previous", xmlIn);
		}
	}

	public class RestoreWindowContent : RestoreContent
	{
		// Instance fields
		protected bool _selected;
		protected StringCollection _next;
		protected StringCollection _previous;

		public RestoreWindowContent()
			: base()
		{
			// Must always point to valid reference
			_selected = false;
			_next = new StringCollection();
			_previous = new StringCollection();
		}

		public RestoreWindowContent(Restore child, 
									Content content, 
									StringCollection next, 
									StringCollection previous,
									bool selected)
			: base(child, content)
		{
			// Remember parameters
            _selected = selected;
            _next = next;
			_previous = previous;
		}

		public override void PerformRestore(Window w)
		{
			// We are only ever called for a WindowContent object
			WindowContent wc = w as WindowContent;

			int bestIndex = -1;

			foreach(String s in _previous)
			{
				if (wc.Contents.Contains(s))
				{
					int previousIndex = wc.Contents.IndexOf(wc.Contents[s]);

					if (previousIndex > bestIndex)
						bestIndex = previousIndex;
				}
			}

			// Did we find a previous Content?
			if (bestIndex >= 0)
			{
				// Great, insert after it
				wc.Contents.Insert(bestIndex + 1, _content);
			}
			else
			{
				bestIndex = wc.Contents.Count;

				foreach(String s in _next)
				{
					if (wc.Contents.Contains(s))
					{
						int nextIndex = wc.Contents.IndexOf(wc.Contents[s]);

						if (nextIndex < bestIndex)
							bestIndex = nextIndex;
					}
				}

				// Insert before the found entry (or at end if non found)
				wc.Contents.Insert(bestIndex, _content);
			}
			
			// Should this content become selected?
			if (_selected)
			    _content.BringToFront();
		}

		public override void SaveInternalToXml(XmlTextWriter xmlOut)
		{
			base.SaveInternalToXml(xmlOut);
			_next.SaveToXml("Next", xmlOut);
			_previous.SaveToXml("Previous", xmlOut);
        
            xmlOut.WriteStartElement("Selected");
            xmlOut.WriteAttributeString("Value", _selected.ToString());
            xmlOut.WriteEndElement();				
        }

		public override void LoadInternalFromXml(XmlTextReader xmlIn, int formatVersion)
		{
			base.LoadInternalFromXml(xmlIn, formatVersion);
			_next.LoadFromXml("Next", xmlIn);
			_previous.LoadFromXml("Previous", xmlIn);
        
            // _selected added in version 4 format
            if (formatVersion >= 4)
            {
                // Move to next xml node
                if (!xmlIn.Read())
                    throw new ArgumentException("Could not read in next expected node");

                // Check it has the expected name
                if (xmlIn.Name != "Selected")
                    throw new ArgumentException("Node 'Selected' expected but not found");

                // Convert attribute value to boolean value
                _selected = Convert.ToBoolean(xmlIn.GetAttribute(0));
            }
        }
	}
}
