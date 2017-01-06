using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace WalburySoftware
{
    public partial class TerminalEmulator : Control
    {
        #region Enum en Structs
        
        private enum Actions
        {
            None = 0,
            Dispatch = 1,
            Execute = 2,
            Ignore = 3,
            Collect = 4,
            NewCollect = 5,
            Param = 6,
            OscStart = 8,
            OscPut = 9,
            OscEnd = 10,
            Hook = 11,
            Unhook = 12,
            Put = 13,
            Print = 14
        }

        private struct ParserEventArgs
        {
            public Actions Action;
            public Char CurChar;
            public String CurSequence;
            public uc_Params CurParams;

            public ParserEventArgs(
                Actions p1,
                System.Char p2,
                System.String p3,
                uc_Params p4)
            {
                this.Action = p1;
                this.CurChar = p2;
                this.CurSequence = p3;
                this.CurParams = p4;
            }
        }

        #endregion

        #region Class uc_Params

        private class uc_Params
        {
            public System.Collections.ArrayList Elements = new System.Collections.ArrayList();

            public uc_Params()
            {
            }

            public Int32 Count()
            {
                return this.Elements.Count;
            }

            public void Clear()
            {
                this.Elements.Clear();
            }

            public void Add(Char CurChar)
            {
                if (this.Count() < 1)
                {
                    this.Elements.Add("0");
                }

                if (CurChar == ';')
                {
                    this.Elements.Add("0");
                }
                else
                {
                    int i = this.Elements.Count - 1;
                    this.Elements[i] = ((String)this.Elements[i] + CurChar.ToString());
                }
            }
        }

        #endregion

        #region Class uc_Parser
        
        /// <summary>
        /// This class provides functionality to parse the VT control characters fed
        /// to the terminal from the host machine and fire off the appropriate actions
        /// It implements Paul William's excellent VT500-Series Parser Model. 
        /// Paul's model can be found at vt100.net
        /// </summary>
        private class uc_Parser
        {
            States State = States.Ground;
            Char CurChar = '\0';
            String CurSequence = String.Empty;

            System.Collections.ArrayList ParamList = new System.Collections.ArrayList();
            uc_CharEvents CharEvents = new uc_CharEvents();
            uc_StateChangeEvents StateChangeEvents = new uc_StateChangeEvents();
            uc_Params CurParams = new uc_Params();

            public uc_Parser()
            {
            }

            public event ParserEventHandler ParserEvent;

            #region Enums
            
            private enum States
            {
                None = 0,
                Ground = 1,
                EscapeIntrmdt = 2,
                Escape = 3,
                CsiEntry = 4,
                CsiIgnore = 5,
                CsiParam = 6,
                CsiIntrmdt = 7,
                OscString = 8,
                SosPmApcString = 9,
                DcsEntry = 10,
                DcsParam = 11,
                DcsIntrmdt = 12,
                DcsIgnore = 13,
                DcsPassthrough = 14,
                Anywhere = 16
            }

            private enum Transitions
            {
                None = 0,
                Entry = 1,
                Exit = 2
            }

            #endregion

            #region Public Methods

            // Every character received is treated as an event which could change the state of
            // the parser. The following section finds out which event or state change this character
            // should trigger and also finds out where we should store the incoming character.
            // The character may be a command, part of a sequence or a parameter; or it might just need
            // binning.
            // The sequence is: state change, store character, do action.
            public void ParseString(String InString)
            {
                States NextState = States.None;
                Actions NextAction = Actions.None;
                Actions StateExitAction = Actions.None;
                Actions StateEntryAction = Actions.None;

                foreach (Char C in InString)
                {
                    this.CurChar = C;

                    // Get the next state and associated action based 
                    // on the current state and char event
                    this.CharEvents.GetStateEventAction(this.State, CurChar, ref NextState, ref NextAction);

                    // execute any actions arising from leaving the current state
                    if (NextState != States.None && NextState != this.State)
                    {
                        // check for state exit actions
                        this.StateChangeEvents.GetStateChangeAction(this.State, Transitions.Exit, ref StateExitAction);

                        // Process the exit action
                        if (StateExitAction != Actions.None)
                            this.DoAction(StateExitAction);
                    }

                    // process the action specified
                    if (NextAction != Actions.None)
                        this.DoAction(NextAction);

                    // set the new parser state and execute any actions arising entering the new state
                    if (NextState != States.None && NextState != this.State)
                    {
                        // change the parsers state attribute
                        this.State = NextState;

                        // check for state entry actions
                        this.StateChangeEvents.GetStateChangeAction(this.State, Transitions.Entry, ref StateExitAction);

                        // Process the entry action
                        if (StateEntryAction != Actions.None)
                            this.DoAction(StateEntryAction);
                    }
                }
            }

            private void DoAction(Actions NextAction)
            {
                // Manage the contents of the Sequence and Param Variables
                switch (NextAction)
                {
                    case Actions.Dispatch:
                    case Actions.Collect:
                        this.CurSequence += this.CurChar.ToString();
                        break;

                    case Actions.NewCollect:
                        this.CurSequence = this.CurChar.ToString();
                        this.CurParams.Clear();
                        break;

                    case Actions.Param:
                        this.CurParams.Add(this.CurChar);
                        break;

                    default:
                        break;
                }

                // send the external event requests
                switch (NextAction)
                {
                    case Actions.Dispatch:
                    case Actions.Execute:
                    case Actions.Put:
                    case Actions.OscStart:
                    case Actions.OscPut:
                    case Actions.OscEnd:
                    case Actions.Hook:
                    case Actions.Unhook:
                    case Actions.Print:

                        //                    Console.Write ("Sequence = {0}, Char = {1}, PrmCount = {2}, State = {3}, NextAction = {4}\n",
                        //                        this.CurSequence, this.CurChar.ToString (), this.CurParams.Count ().ToString (), 
                        //                        this.State.ToString (), NextAction.ToString ());

                        this.ParserEvent(this, new ParserEventArgs(NextAction, CurChar, CurSequence, CurParams));
                        break;

                    default:
                        break;
                }

                switch (NextAction)
                {
                    case Actions.Dispatch:
                        this.CurSequence = String.Empty;
                        this.CurParams.Clear();
                        break;

                    default:
                        break;
                }
            }

            #endregion

            #region Private Classes
            
            private struct uc_StateChangeInfo
            {
                public States State;
                public Transitions Transition;    // the next state we are going to 
                public Actions NextAction;

                public uc_StateChangeInfo(
                    States p1,
                    Transitions p2,
                    Actions p3)
                {
                    this.State = p1;
                    this.Transition = p2;
                    this.NextAction = p3;
                }
            }

            private struct uc_CharEventInfo
            {
                public States CurState;
                public Char CharFrom;
                public Char CharTo;
                public Actions NextAction;
                public States NextState;  // the next state we are going to 

                public uc_CharEventInfo(
                    States p1,
                    System.Char p2,
                    System.Char p3,
                    Actions p4,
                    States p5)
                {
                    this.CurState = p1;
                    this.CharFrom = p2;
                    this.CharTo = p3;
                    this.NextAction = p4;
                    this.NextState = p5;
                }
            }

            private class uc_StateChangeEvents
            {
                private uc_StateChangeInfo[] Elements = 
                {
                    new uc_StateChangeInfo (States.OscString,      Transitions.Entry, Actions.OscStart),
                    new uc_StateChangeInfo (States.OscString,      Transitions.Exit,  Actions.OscEnd),
                    new uc_StateChangeInfo (States.DcsPassthrough, Transitions.Entry, Actions.Hook),
                    new uc_StateChangeInfo (States.DcsPassthrough, Transitions.Exit,  Actions.Unhook)
                };

                public uc_StateChangeEvents()
                {
                }

                public Boolean GetStateChangeAction(
                    States State,
                    Transitions Transition,
                    ref Actions NextAction)
                {
                    uc_StateChangeInfo Element;

                    for (Int32 i = 0; i < this.Elements.Length; i++)
                    {
                        Element = this.Elements[i];

                        if (State == Element.State &&
                            Transition == Element.Transition)
                        {
                            NextAction = Element.NextAction;
                            return true;
                        }
                    }

                    return false;
                }
            }

            private class uc_CharEvents
            {
                public static uc_CharEventInfo[] Elements = 
                {
                    new uc_CharEventInfo (States.Anywhere,      '\x1B', '\x1B', Actions.NewCollect, States.Escape),
                    new uc_CharEventInfo (States.Anywhere,      '\x18', '\x18', Actions.Execute,    States.Ground),
                    new uc_CharEventInfo (States.Anywhere,      '\x1A', '\x1A', Actions.Execute,    States.Ground),
                    new uc_CharEventInfo (States.Anywhere,      '\x1A', '\x1A', Actions.Execute,    States.Ground),
                    new uc_CharEventInfo (States.Anywhere,      '\x80', '\x8F', Actions.Execute,    States.Ground),
                    new uc_CharEventInfo (States.Anywhere,      '\x91', '\x97', Actions.Execute,    States.Ground),
                    new uc_CharEventInfo (States.Anywhere,      '\x99', '\x99', Actions.Execute,    States.Ground),
                    new uc_CharEventInfo (States.Anywhere,      '\x9A', '\x9A', Actions.Execute,    States.Ground),
                    new uc_CharEventInfo (States.Anywhere,      '\x9C', '\x9C', Actions.Execute,    States.Ground),
                    new uc_CharEventInfo (States.Anywhere,      '\x98', '\x98', Actions.None,       States.SosPmApcString),
                    new uc_CharEventInfo (States.Anywhere,      '\x9E', '\x9F', Actions.None,       States.SosPmApcString),
                    new uc_CharEventInfo (States.Anywhere,      '\x90', '\x90', Actions.NewCollect, States.DcsEntry),
                    new uc_CharEventInfo (States.Anywhere,      '\x9D', '\x9D', Actions.None,       States.OscString),
                    new uc_CharEventInfo (States.Anywhere,      '\x9B', '\x9B', Actions.NewCollect, States.CsiEntry),
                    new uc_CharEventInfo (States.Ground,        '\x00', '\x17', Actions.Execute,    States.None),
                    new uc_CharEventInfo (States.Ground,        '\x00', '\x17', Actions.Execute,    States.None),
                    new uc_CharEventInfo (States.Ground,        '\x19', '\x19', Actions.Execute,    States.None),
                    new uc_CharEventInfo (States.Ground,        '\x1C', '\x1F', Actions.Execute,    States.None),
                    new uc_CharEventInfo (States.Ground,        '\x20', '\x7F', Actions.Print,      States.None),
                    new uc_CharEventInfo (States.Ground,        '\x80', '\x8F', Actions.Execute,    States.None),
                    new uc_CharEventInfo (States.Ground,        '\x91', '\x9A', Actions.Execute,    States.None),
                    new uc_CharEventInfo (States.Ground,        '\x9C', '\x9C', Actions.Execute,    States.None),
                    new uc_CharEventInfo (States.EscapeIntrmdt, '\x00', '\x17', Actions.Execute,    States.None),
                    new uc_CharEventInfo (States.EscapeIntrmdt, '\x19', '\x19', Actions.Execute,    States.None),
                    new uc_CharEventInfo (States.EscapeIntrmdt, '\x1C', '\x1F', Actions.Execute,    States.None),
                    new uc_CharEventInfo (States.EscapeIntrmdt, '\x20', '\x2F', Actions.Collect,    States.None),
                    new uc_CharEventInfo (States.EscapeIntrmdt, '\x30', '\x7E', Actions.Dispatch,   States.Ground),
                    new uc_CharEventInfo (States.Escape,        '\x00', '\x17', Actions.Execute,    States.None),
                    new uc_CharEventInfo (States.Escape,        '\x19', '\x19', Actions.Execute,    States.None),
                    new uc_CharEventInfo (States.Escape,        '\x1C', '\x1F', Actions.Execute,    States.None),
                    new uc_CharEventInfo (States.Escape,        '\x58', '\x58', Actions.None,       States.SosPmApcString),
                    new uc_CharEventInfo (States.Escape,        '\x5E', '\x5F', Actions.None,       States.SosPmApcString),
                    new uc_CharEventInfo (States.Escape,        '\x50', '\x50', Actions.Collect,    States.DcsEntry),
                    new uc_CharEventInfo (States.Escape,        '\x5D', '\x5D', Actions.None,       States.OscString),
                    new uc_CharEventInfo (States.Escape,        '\x5B', '\x5B', Actions.Collect,    States.CsiEntry),
                    new uc_CharEventInfo (States.Escape,        '\x30', '\x4F', Actions.Dispatch,   States.Ground),
                    new uc_CharEventInfo (States.Escape,        '\x51', '\x57', Actions.Dispatch,   States.Ground),
                    new uc_CharEventInfo (States.Escape,        '\x59', '\x5A', Actions.Dispatch,   States.Ground),
                    new uc_CharEventInfo (States.Escape,        '\x5C', '\x5C', Actions.Dispatch,   States.Ground),
                    new uc_CharEventInfo (States.Escape,        '\x60', '\x7E', Actions.Dispatch,   States.Ground),
                    new uc_CharEventInfo (States.Escape,        '\x20', '\x2F', Actions.Collect,    States.EscapeIntrmdt),
                    new uc_CharEventInfo (States.CsiEntry,      '\x00', '\x17', Actions.Execute,    States.None),
                    new uc_CharEventInfo (States.CsiEntry,      '\x19', '\x19', Actions.Execute,    States.None),
                    new uc_CharEventInfo (States.CsiEntry,      '\x1C', '\x1F', Actions.Execute,    States.None),
                    new uc_CharEventInfo (States.CsiEntry,      '\x20', '\x2F', Actions.Collect,    States.CsiIntrmdt),
                    new uc_CharEventInfo (States.CsiEntry,      '\x3A', '\x3A', Actions.None,       States.CsiIgnore),
                    new uc_CharEventInfo (States.CsiEntry,      '\x3C', '\x3F', Actions.Collect,    States.CsiParam),
                    new uc_CharEventInfo (States.CsiEntry,      '\x3C', '\x3F', Actions.Collect,    States.CsiParam),
                    new uc_CharEventInfo (States.CsiEntry,      '\x30', '\x39', Actions.Param,      States.CsiParam),
                    new uc_CharEventInfo (States.CsiEntry,      '\x3B', '\x3B', Actions.Param,      States.CsiParam),
                    new uc_CharEventInfo (States.CsiEntry,      '\x3C', '\x3F', Actions.Collect,    States.CsiParam),
                    new uc_CharEventInfo (States.CsiEntry,      '\x40', '\x7E', Actions.Dispatch,   States.Ground),
                    new uc_CharEventInfo (States.CsiParam,      '\x00', '\x17', Actions.Execute,    States.None),
                    new uc_CharEventInfo (States.CsiParam,      '\x19', '\x19', Actions.Execute,    States.None),
                    new uc_CharEventInfo (States.CsiParam,      '\x1C', '\x1F', Actions.Execute,    States.None),
                    new uc_CharEventInfo (States.CsiParam,      '\x30', '\x39', Actions.Param,      States.None),
                    new uc_CharEventInfo (States.CsiParam,      '\x3B', '\x3B', Actions.Param,      States.None),
                    new uc_CharEventInfo (States.CsiParam,      '\x3A', '\x3A', Actions.None,       States.CsiIgnore),
                    new uc_CharEventInfo (States.CsiParam,      '\x3C', '\x3F', Actions.None,       States.CsiIgnore),
                    new uc_CharEventInfo (States.CsiParam,      '\x20', '\x2F', Actions.Collect,    States.CsiIntrmdt),
                    new uc_CharEventInfo (States.CsiParam,      '\x40', '\x7E', Actions.Dispatch,   States.Ground),
                    new uc_CharEventInfo (States.CsiIgnore,     '\x00', '\x17', Actions.Execute,    States.None),
                    new uc_CharEventInfo (States.CsiIgnore,     '\x19', '\x19', Actions.Execute,    States.None),
                    new uc_CharEventInfo (States.CsiIgnore,     '\x1C', '\x1F', Actions.Execute,    States.None),
                    new uc_CharEventInfo (States.CsiIgnore,     '\x40', '\x7E', Actions.None,       States.Ground),
                    new uc_CharEventInfo (States.CsiIntrmdt,    '\x00', '\x17', Actions.Execute,    States.None),
                    new uc_CharEventInfo (States.CsiIntrmdt,    '\x19', '\x19', Actions.Execute,    States.None),
                    new uc_CharEventInfo (States.CsiIntrmdt,    '\x1C', '\x1F', Actions.Execute,    States.None),
                    new uc_CharEventInfo (States.CsiIntrmdt,    '\x20', '\x2F', Actions.Collect,    States.None),
                    new uc_CharEventInfo (States.CsiIntrmdt,    '\x30', '\x3F', Actions.None,       States.CsiIgnore),
                    new uc_CharEventInfo (States.CsiIntrmdt,    '\x40', '\x7E', Actions.Dispatch,   States.Ground),
                    new uc_CharEventInfo (States.SosPmApcString,'\x9C', '\x9C', Actions.None,       States.Ground),
                    new uc_CharEventInfo (States.DcsEntry,      '\x20', '\x2F', Actions.Collect,    States.DcsIntrmdt),
                    new uc_CharEventInfo (States.DcsEntry,      '\x3A', '\x3A', Actions.None,       States.DcsIgnore),
                    new uc_CharEventInfo (States.DcsEntry,      '\x30', '\x39', Actions.Param,      States.DcsParam),
                    new uc_CharEventInfo (States.DcsEntry,      '\x3B', '\x3B', Actions.Param,      States.DcsParam),
                    new uc_CharEventInfo (States.DcsEntry,      '\x3C', '\x3F', Actions.Collect,    States.DcsParam),
                    new uc_CharEventInfo (States.DcsEntry,      '\x40', '\x7E', Actions.None,       States.DcsPassthrough),
                    new uc_CharEventInfo (States.DcsIntrmdt,    '\x30', '\x3F', Actions.None,       States.DcsIgnore),
                    new uc_CharEventInfo (States.DcsIntrmdt,    '\x40', '\x7E', Actions.None,       States.DcsPassthrough),
                    new uc_CharEventInfo (States.DcsIgnore,     '\x9C', '\x9C', Actions.None,       States.Ground),
                    new uc_CharEventInfo (States.DcsParam,      '\x30', '\x39', Actions.Param,      States.None),
                    new uc_CharEventInfo (States.DcsParam,      '\x3B', '\x3B', Actions.Param,      States.None),
                    new uc_CharEventInfo (States.DcsParam,      '\x20', '\x2F', Actions.Collect,    States.DcsIntrmdt),
                    new uc_CharEventInfo (States.DcsParam,      '\x3A', '\x3A', Actions.None,       States.DcsIgnore),
                    new uc_CharEventInfo (States.DcsParam,      '\x3C', '\x3F', Actions.None,       States.DcsIgnore),
                    new uc_CharEventInfo (States.DcsPassthrough,'\x00', '\x17', Actions.Put,        States.None),
                    new uc_CharEventInfo (States.DcsPassthrough,'\x19', '\x19', Actions.Put,        States.None),
                    new uc_CharEventInfo (States.DcsPassthrough,'\x1C', '\x1F', Actions.Put,        States.None),
                    new uc_CharEventInfo (States.DcsPassthrough,'\x20', '\x7E', Actions.Put,        States.None),
                    new uc_CharEventInfo (States.DcsPassthrough,'\x9C', '\x9C', Actions.None,       States.Ground),
                    new uc_CharEventInfo (States.OscString,     '\x20', '\x7F', Actions.OscPut,     States.None),
                    new uc_CharEventInfo (States.OscString,     '\x9C', '\x9C', Actions.None,       States.Ground)
                };

                public uc_CharEvents()
                {
                }

                public Boolean GetStateEventAction(
                    States CurState,
                    Char CurChar,
                    ref States NextState,
                    ref Actions NextAction)
                {
                    uc_CharEventInfo Element;

                    // Codes A0-FF are treated exactly the same way as 20-7F
                    // so we can keep are state table smaller by converting before we look
                    // up the event associated with the character

                    if (CurChar >= '\xA0' &&
                        CurChar <= '\xFF')
                    {
                        CurChar -= '\x80';
                    }

                    for (Int32 i = 0; i < uc_CharEvents.Elements.Length; i++)
                    {
                        Element = uc_CharEvents.Elements[i];

                        if (CurChar >= Element.CharFrom &&
                            CurChar <= Element.CharTo &&
                            (CurState == Element.CurState || Element.CurState == States.Anywhere))
                        {
                            NextState = Element.NextState;
                            NextAction = Element.NextAction;
                            return true;
                        }
                    }

                    return false;
                }
            }

            #endregion
        }

        #endregion
    }
}
