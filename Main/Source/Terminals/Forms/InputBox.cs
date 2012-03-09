using System;
using System.Drawing;
using System.Windows.Forms;

namespace Terminals.Forms
{
    /// <summary>
    /// Form used for prompts user to enter simple text value.
    /// Not thread safe.
    /// </summary>
    internal partial class InputBox : Form
    {
        private static InputBox frmInputDialog = new InputBox();
        
        private static readonly InputBoxResult outputResponse = new InputBoxResult();
        private static InputBoxResult OutputResponse
        {
            get
            {
                return outputResponse;
            }
        }

        /// <summary>
        /// Prevent creation outside of this class
        /// </summary>
        private InputBox()
        {
            InitializeComponent();
        }

        #region Private function, InputBox Form move and change size

        private static void LoadForm(string formCaption, string formPrompt, string defaultValue, int x = 0, int y = 0)
        {
            ResetResponse();
            frmInputDialog.AssignTexts(formCaption, formPrompt, defaultValue);
            frmInputDialog.SetupNewLocation(x, y);
            frmInputDialog.UpdateInputSelection();
        }

        private void SetupNewLocation(int x, int y)
        {
            // Retrieve the working rectangle from the Screen class
            // using the PrimaryScreen and the WorkingArea properties.
            Rectangle workingRectangle = Screen.PrimaryScreen.WorkingArea;

            if ((x > 0 && x < workingRectangle.Width - 100) && (y > 0 && y < workingRectangle.Height - 100))
            {
                frmInputDialog.StartPosition = FormStartPosition.Manual;
                frmInputDialog.Location = new Point(x, y);
            }
            else
            {
                frmInputDialog.StartPosition = FormStartPosition.CenterScreen;
            }
        }

        private static void ResetResponse()
        {
            OutputResponse.ReturnCode = DialogResult.Ignore;
            OutputResponse.Text = String.Empty;
        }

        #endregion

        #region Not static

        private void UpdateInputSelection()
        {
            txtInput.SelectionStart = 0;
            txtInput.SelectionLength = txtInput.Text.Length;
            txtInput.Focus();
        }

        private void AssignTexts(string title, string prompt, string defaultValue)
        {
            this.txtInput.PasswordChar = '\0';
            this.Text = title;
            this.lblPrompt.Text = prompt;
            this.txtInput.Text = defaultValue;
        }

        private void AssignPasswordChar(char passwordChar)
        {
            this.txtInput.PasswordChar = passwordChar;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            OutputResponse.ReturnCode = DialogResult.OK;
            OutputResponse.Text = txtInput.Text;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            OutputResponse.ReturnCode = DialogResult.Cancel;
            OutputResponse.Text = String.Empty; //Clean output response
        }

        private void InputBox_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void lblPrompt_TextChanged(object sender, EventArgs e)
        {
            this.lblPrompt.Height = this.MessureControlHeightByText(this.lblPrompt) + 2; // 2 = border
            this.UpdateFormHeight();
        }

        private int MessureControlHeightByText(Control controlToMessure)
        {
            Graphics graphics = this.CreateGraphics();
            SizeF newSize = graphics.MeasureString(controlToMessure.Text, controlToMessure.Font, controlToMessure.Width);
            return (int)Math.Round(newSize.Height);
        }

        private void UpdateFormHeight()
        {
            // Calculated from default Form size 410x130
            // and lbl, txt size 380x 18(20)
            // 92 = default form height - lbl.height - txt.height
            this.Height = 92 + this.lblPrompt.Height + this.txtInput.Height;
        }

        #endregion

        #region Static Show functions

        internal static InputBoxResult Show(String prompt)
        {
            LoadForm("Prompt", prompt, string.Empty);
            frmInputDialog.ShowDialog();
            return OutputResponse;
        }

        internal static InputBoxResult Show(String prompt, String title, Char passwordChar)
        {
            LoadForm(title, prompt, string.Empty);
            frmInputDialog.AssignPasswordChar(passwordChar);
            frmInputDialog.ShowDialog();
            return OutputResponse;
        }

        internal static InputBoxResult Show(String prompt, String title)
        {
            LoadForm(title, prompt, string.Empty);
            frmInputDialog.ShowDialog();
            return OutputResponse;
        }

        internal static InputBoxResult Show(String prompt, String title, String defaultValue)
        {
            LoadForm(title, prompt, defaultValue);
            frmInputDialog.ShowDialog();
            return OutputResponse;
        }

        internal static InputBoxResult Show(String prompt, String title, String defaultValue, Int32 xPos, Int32 yPos)
        {
            if (xPos < 0)
                xPos = 0;
            if (yPos < 0)
                yPos = 0;

            LoadForm(title, prompt, defaultValue, xPos, yPos);
            frmInputDialog.ShowDialog();
            return OutputResponse;
        }

        #endregion
    }
}
