using System.Windows.Forms;

namespace Terminals
{
    internal partial class SortableUnboundGrid : DataGridView
    {
        public SortableUnboundGrid()
        {
            InitializeComponent();
        }

        internal static SortOrder GetNewSortDirection(DataGridViewColumn lastSortedColumn, DataGridViewColumn newColumn)
        {
            SortOrder newSortDirection = SortOrder.Ascending;
            if (lastSortedColumn != null)
            {
                if (lastSortedColumn == newColumn)
                {
                    if (newColumn.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                        newSortDirection = SortOrder.Descending;
                }
                lastSortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
            }

            return newSortDirection;
        }

        internal DataGridViewColumn FindLastSortedColumn()
        {
            foreach (DataGridViewColumn column in this.Columns)
            {
                if (column.HeaderCell.SortGlyphDirection != SortOrder.None)
                {
                    return column;
                }
            }
            return null;
        }
    }
}
