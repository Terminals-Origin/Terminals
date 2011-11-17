using System.Collections.Generic;
using System.Collections.Specialized;

namespace WalburySoftware
{
    public partial class TerminalEmulator
    {
        /// <summary>
        /// Collection of values to store as backup for scrollbar moves
        /// </summary>
        private class ScrollBackBuffer
        {
            internal StringCollection Characters { get; private set; }
            internal List<CharAttribStruct[]> Attributes { get; private set; }
            internal int Count
            {
                get { return this.Characters.Count; }
            }

            internal ScrollBackBuffer()
            {
                this.Characters = new StringCollection();
                this.Attributes = new List<CharAttribStruct[]>();
            }

            internal void RemoveLast()
            {
                this.Characters.RemoveAt(0);
                this.Attributes.RemoveAt(0);
            }

            internal void Add(string line, CharAttribStruct[] lineAttributes)
            {
                this.Characters.Add(line);
                this.Attributes.Add(lineAttributes);
            }

            internal void Insert(int index, string line, CharAttribStruct[] lineAttributes)
            {
                this.Characters.Insert(index, line);
                this.Attributes.Insert(index, lineAttributes);
            }

            internal void ReplaceValues(int rowIndex, string newChars, CharAttribStruct[] newAttributes)
            {
                this.Characters[rowIndex] = newChars;
                this.Attributes[rowIndex] = newAttributes;
            }
        }
    }
}
