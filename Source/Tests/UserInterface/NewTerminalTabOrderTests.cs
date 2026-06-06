using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Forms.EditFavorite;

namespace Tests.UserInterface
{
    /// <summary>
    /// Regression tests for issue #132: the General and Groups sections of the
    /// New Connection dialog had random/colliding tab order indices, which made
    /// keyboard navigation jump around the form. These tests assert the tab order
    /// of each section follows the visible reading order (top to bottom, then left
    /// to right) and that focusable controls do not share a tab index.
    /// </summary>
    [TestClass]
    public class NewTerminalTabOrderTests
    {
        /// <summary>
        /// Controls on the same visual row may differ slightly in Top (a label and
        /// its field are not pixel aligned). Treat anything within this band as one row.
        /// </summary>
        private const int RowTolerance = 12;

        [TestMethod]
        public void GeneralProperties_TabOrder_FollowsReadingOrder()
        {
            using (var control = new GeneralPropertiesUserControl())
                AssertReadingOrder(control);
        }

        [TestMethod]
        public void Groups_TabOrder_FollowsReadingOrder()
        {
            using (var control = new GroupsControl())
                AssertReadingOrder(control);
        }

        [TestMethod]
        public void GeneralProperties_FocusableControls_HaveUniqueTabIndex()
        {
            using (var control = new GeneralPropertiesUserControl())
                AssertUniqueTabIndex(control);
        }

        [TestMethod]
        public void Groups_FocusableControls_HaveUniqueTabIndex()
        {
            using (var control = new GroupsControl())
                AssertUniqueTabIndex(control);
        }

        /// <summary>
        /// Walks the focusable controls in tab order and verifies each next control is
        /// either below the previous one, or on the same row but further right - never
        /// jumping back up the form.
        /// </summary>
        private static void AssertReadingOrder(Control container)
        {
            List<Control> ordered = FocusableControls(container)
                .OrderBy(c => c.TabIndex)
                .ToList();

            for (int i = 1; i < ordered.Count; i++)
            {
                Control previous = ordered[i - 1];
                Control current = ordered[i];

                bool sameRow = System.Math.Abs(current.Top - previous.Top) <= RowTolerance;
                bool isBelow = current.Top - previous.Top > RowTolerance;
                bool rightOnSameRow = sameRow && current.Left >= previous.Left;

                string message = string.Format(
                    "Tab order jumps backwards: '{0}' (TabIndex {1}, {2}) should not come before '{3}' (TabIndex {4}, {5}).",
                    previous.Name, previous.TabIndex, FormatLocation(previous),
                    current.Name, current.TabIndex, FormatLocation(current));

                Assert.IsTrue(isBelow || rightOnSameRow, message);
            }
        }

        private static void AssertUniqueTabIndex(Control container)
        {
            List<Control> focusable = FocusableControls(container).ToList();
            var duplicates = focusable
                .GroupBy(c => c.TabIndex)
                .Where(g => g.Count() > 1)
                .Select(g => string.Format("TabIndex {0}: {1}", g.Key, string.Join(", ", g.Select(c => c.Name))))
                .ToList();

            Assert.AreEqual(0, duplicates.Count,
                "Focusable controls must not share a tab index. Collisions: " + string.Join(" | ", duplicates));
        }

        private static IEnumerable<Control> FocusableControls(Control container)
        {
            // Labels (TabStop == false) and hidden alternates are skipped by real tab
            // navigation, so they are excluded here too.
            return container.Controls.Cast<Control>().Where(c => c.TabStop && c.Visible);
        }

        private static string FormatLocation(Control control)
        {
            return string.Format("Top={0},Left={1}", control.Top, control.Left);
        }
    }
}
