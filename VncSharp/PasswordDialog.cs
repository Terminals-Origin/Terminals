// VncSharp - .NET VNC Client Library
// Copyright (C) 2004  David Humphrey, Chuck Borgh, Matt Cyr
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace VncSharp
{
	/// <summary>
	/// A simple GUI Form for obtaining a user's password.  More elaborate interfaces could be used, but this is the default.
	/// </summary>
	public class PasswordDialog : Form
	{
		Button btnOk;
		Button btnCancel;
		TextBox txtPassword;

		Container components = null;

		private PasswordDialog()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Gets the Password entered by the user.
		/// </summary>
		public string Password {
			get {
				return txtPassword.Text;
			}
		}

		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.txtPassword = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// btnOk
			// 
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Location = new System.Drawing.Point(144, 8);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(64, 23);
			this.btnOk.TabIndex = 1;
			this.btnOk.Text = "OK";
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(144, 40);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(64, 23);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "Cancel";
			// 
			// txtPassword
			// 
			this.txtPassword.Location = new System.Drawing.Point(16, 16);
			this.txtPassword.Name = "txtPassword";
			this.txtPassword.PasswordChar = '*';
			this.txtPassword.Size = new System.Drawing.Size(112, 20);
			this.txtPassword.TabIndex = 0;
			this.txtPassword.Text = "";
			// 
			// ConnectionPassword
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(216, 73);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.txtPassword,
																		  this.btnCancel,
																		  this.btnOk});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ConnectionPassword";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Password";
			this.ResumeLayout(false);

		}
		#endregion
		
		/// <summary>
		/// Creates an instance of PasswordDialog and uses it to obtain the user's password.
		/// </summary>
		/// <returns>Returns the user's password as entered, or null if he/she clicked Cancel.</returns>
		public static string GetPassword()
		{
			using(PasswordDialog dialog = new PasswordDialog()) {
				if (dialog.ShowDialog() == DialogResult.OK) {
					return dialog.Password;
				} else {
					// If the user clicks Cancel, return null and not the empty string.
					return null;
				}
			}
		}
	}
}