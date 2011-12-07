using System;
using System.Drawing;
using System.Windows.Forms;

namespace Terminals.Forms
{
    #region InputBox return result

    /// <summary>
    /// Class used to store the result of an InputBox.Show message.
    /// </summary>
    internal class InputBoxResult 
    {
        public DialogResult ReturnCode;
        public String Text;
    }

    #endregion

    /// <summary>
    /// Summary description for InputBox.
    /// </summary>
    internal static class InputBox
    {
        #region Declarations

        private static Form frmInputDialog;
        private static Label lblPrompt;
        private static Button btnOK;
        private static Button btnCancel;
        private static TextBox txtInput;

        private static String _formCaption = String.Empty;
        private static String _formPrompt = String.Empty;
        private static InputBoxResult _outputResponse = new InputBoxResult();
        private static String _defaultValue = String.Empty;
        private static Int32 _xPos = -1;
        private static Int32 _yPos = -1;

        #endregion

        #region Windows Form code

        private static void InitializeComponent()
        {
            // Create a new instance of the form.
            frmInputDialog = new Form();
            lblPrompt = new Label();
            btnOK = new Button();
            btnCancel = new Button();
            txtInput = new TextBox();
            frmInputDialog.SuspendLayout();
            // 
            // lblPrompt
            // 
            lblPrompt.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) | AnchorStyles.Right)));
            lblPrompt.BackColor = SystemColors.Control;
            lblPrompt.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((Byte)(0)));
            lblPrompt.Location = new Point(12, 9);
            lblPrompt.Name = "lblPrompt";
            lblPrompt.Size = new Size(302, 82);
            lblPrompt.TabIndex = 3;
            // 
            // btnOK
            // 
            btnOK.DialogResult = DialogResult.OK;
            btnOK.FlatStyle = FlatStyle.Popup;
            btnOK.Location = new Point(326, 8);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(64, 24);
            btnOK.TabIndex = 1;
            btnOK.Text = "&OK";
            btnOK.Click += new EventHandler(btnOK_Click);
            // 
            // btnCancel
            // 
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.FlatStyle = FlatStyle.Popup;
            btnCancel.Location = new Point(326, 40);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(64, 24);
            btnCancel.TabIndex = 2;
            btnCancel.Text = "&Cancel";
            btnCancel.Click += new EventHandler(btnCancel_Click);
            // 
            // txtInput
            // 
            txtInput.Location = new Point(8, 100);
            txtInput.Name = "txtInput";
            txtInput.Size = new Size(379, 20);
            txtInput.TabIndex = 0;
            txtInput.Text = "";
            // 
            // InputBoxDialog
            // 
            frmInputDialog.AutoScaleBaseSize = new Size(5, 13);
            frmInputDialog.ClientSize = new Size(398, 128);
            frmInputDialog.Controls.Add(txtInput);
            frmInputDialog.Controls.Add(btnCancel);
            frmInputDialog.Controls.Add(btnOK);
            frmInputDialog.Controls.Add(lblPrompt);
            frmInputDialog.FormBorderStyle = FormBorderStyle.FixedDialog;
            frmInputDialog.MaximizeBox = false;
            frmInputDialog.MinimizeBox = false;
            frmInputDialog.Name = "InputBoxDialog";
            frmInputDialog.ResumeLayout(false);

            frmInputDialog.AcceptButton = btnOK;
            frmInputDialog.CancelButton = btnCancel;
        }

        /// <summary>
        /// Dispose of controls used for the inputbox.
        /// </summary>
        private static void DisposeControls()
        {
            try
            {
                foreach (Control ctrl in frmInputDialog.Controls)
                {
                    ctrl.Dispose();
                }

                frmInputDialog.Dispose();
                frmInputDialog = null;
            }
            catch (Exception)
            {
            }
        }

        #endregion

        #region Private function, InputBox Form move and change size

        private static void LoadForm()
        {
            OutputResponse.ReturnCode = DialogResult.Ignore;
            OutputResponse.Text = String.Empty;

            txtInput.Text = _defaultValue;
            lblPrompt.Text = _formPrompt;
            frmInputDialog.Text = _formCaption;

            // Retrieve the working rectangle from the Screen class
            // using the PrimaryScreen and the WorkingArea properties.
            System.Drawing.Rectangle workingRectangle = Screen.PrimaryScreen.WorkingArea;

            if ((_xPos >= 0 && _xPos < workingRectangle.Width - 100) && (_yPos >= 0 && _yPos < workingRectangle.Height - 100))
            {
                frmInputDialog.StartPosition = FormStartPosition.Manual;
                frmInputDialog.Location = new System.Drawing.Point(_xPos, _yPos);
            }
            else
            {
                frmInputDialog.StartPosition = FormStartPosition.CenterScreen;
            }

            String PrompText = lblPrompt.Text;

            Int32 n = 0;
            Int32 Index = 0;
            while(PrompText.IndexOf("\n",Index) > -1)
            {
                Index = PrompText.IndexOf("\n",Index)+1;
                n++;
            }

            if( n == 0 )
                n = 1;

            System.Drawing.Point Txt = txtInput.Location; 
            Txt.Y = Txt.Y + (n*4);
            txtInput.Location = Txt; 
            System.Drawing.Size form = frmInputDialog.Size; 
            form.Height = form.Height + (n*4);
            frmInputDialog.Size = form; 

            txtInput.SelectionStart = 0;
            txtInput.SelectionLength = txtInput.Text.Length;
            txtInput.Focus();
        }

        #endregion

        #region Button control click event

        private static void btnOK_Click(object sender, System.EventArgs e)
        {
            OutputResponse.ReturnCode = DialogResult.OK;
            OutputResponse.Text = txtInput.Text;
            frmInputDialog.Dispose();
        }

        private static void btnCancel_Click(object sender, System.EventArgs e)
        {
            OutputResponse.ReturnCode = DialogResult.Cancel;
            OutputResponse.Text = String.Empty; //Clean output response
            frmInputDialog.Dispose();
        }

        #endregion

        #region Public Static Show functions

        public static InputBoxResult Show(String Prompt)
        {
            InitializeComponent();
            FormPrompt = Prompt;

            // Display the form as a modal dialog box.
            LoadForm();
            frmInputDialog.ShowDialog();
            DisposeControls();
            return OutputResponse;
        }

        public static InputBoxResult Show(String Prompt, String Title, Char PasswordChar)
        {
            InitializeComponent();

            FormCaption = Title;
            FormPrompt = Prompt;

            // Display the form as a modal dialog box.
            LoadForm();
            txtInput.PasswordChar = PasswordChar;
            frmInputDialog.ShowDialog();
            DisposeControls();
            return OutputResponse;
        }

        public static InputBoxResult Show(String Prompt, String Title)
        {
            InitializeComponent();

            FormCaption = Title;
            FormPrompt = Prompt;

            // Display the form as a modal dialog box.
            LoadForm();
            frmInputDialog.ShowDialog();
            DisposeControls();
            return OutputResponse;
        }

        public static InputBoxResult Show(String Prompt, String Title, String Default)
        {
            InitializeComponent();

            FormCaption = Title;
            FormPrompt = Prompt;
            DefaultValue = Default;

            // Display the form as a modal dialog box.
            LoadForm();
            frmInputDialog.ShowDialog();
            DisposeControls();
            return OutputResponse;
        }

        public static InputBoxResult Show(String Prompt, String Title, String Default, Int32 XPos, Int32 YPos)
        {
            InitializeComponent();
            FormCaption = Title;
            FormPrompt = Prompt;
            DefaultValue = Default;
            XPosition = XPos;
            YPosition = YPos;

            // Display the form as a modal dialog box.
            LoadForm();
            frmInputDialog.ShowDialog();
            DisposeControls();
            return OutputResponse;
        }

        #endregion

        #region Private Properties

        private static String FormCaption
        {
            set
            {
                _formCaption = value;
            }
        } // property FormCaption

        private static String FormPrompt
        {
            set
            {
                _formPrompt = value;
            }
        } // property FormPrompt

        private static InputBoxResult OutputResponse
        {
            get
            {
                return _outputResponse;
            }
            set
            {
                _outputResponse = value;
            }
        } // property InputResponse

        private static String DefaultValue
        {
            set
            {
                _defaultValue = value;
            }
        } // property DefaultValue

        private static Int32 XPosition
        {
            set
            {
                if( value >= 0 )
                    _xPos = value;
            }
        } // property XPos

        private static Int32 YPosition
        {
            set
            {
                if( value >= 0 )
                    _yPos = value;
            }
        } // property YPos

        #endregion 
    }
}
