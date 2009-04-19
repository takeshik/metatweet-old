// -*- mode: csharp; encoding: utf-8; -*-
// $Id$
/* GenProc - General Processor
 * Copyright (C) 2007-2008 Takeshi KIRIYA, XSpect Project <takeshik@xspect.org>
 * All rights reserved.
 * 
 * This file is part of GenProc.
 * 
 * This program is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License as published by the Free
 * Software Foundation; either version 3 of the License, or (at your option)
 * any later version.
 * 
 * This program is distributed in the hope that it will be useful, but
 * WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
 * or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License
 * for more details. 
 * 
 * You should have received a copy of the GNU General Public License along
 * with this program. If not, see <http://www.gnu.org/licenses/>, or write to
 * the Free Software Foundation, Inc., 51 Franklin Street - Fifth Floor,
 * Boston, MA 02110-1301, USA.
 */

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using XSpect;

namespace XSpect.Windows.Forms
{
	public partial class ExceptionForm
		: Form
	{
        private readonly ExceptionHandler _handler;

        private readonly Uri _btsUri;

		public ExceptionForm(Exception ex)
			: this(ex, new Uri("https://bugs.xspect.org/"))
		{
		}

        public ExceptionForm(Exception ex, Uri uri)
        {
            InitializeComponent();

            this.exceptionTextBox.Font = new Font(
                Control.DefaultFont.FontFamily.Name,
                Control.DefaultFont.Size * 1.5f,
                Control.DefaultFont.Style | FontStyle.Bold,
                Control.DefaultFont.Unit,
                Control.DefaultFont.GdiCharSet,
                Control.DefaultFont.GdiVerticalFont
            );

            this.messageTextBox.Font = Control.DefaultFont;

            this._handler = new ExceptionHandler(ex);
            this.Initialize();
            this._btsUri = uri;
        }

		public void Initialize()
		{
            this.exceptionTextBox.Text = this._handler.Exception.GetType().FullName;
            this.messageTextBox.Text = this._handler.Exception.Message;
            this.informationTextBox.Text = this._handler.GetDiagnosticMessage();
        }

		private void ExceptionForm_FormClosed(Object sender, FormClosedEventArgs e)
		{
			this.Dispose();
		}

		private void exitButton_Click(Object sender, EventArgs e)
		{
			Environment.Exit(Int32.MaxValue);
		}

		private void debugButton_Click(Object sender, EventArgs e)
		{
			if (!Debugger.IsAttached)
			{
				Debugger.Launch();
			}
			Debugger.Break();
		}

        private void continueButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void abortButton_Click(object sender, EventArgs e)
        {
            Environment.FailFast(this._handler.Exception.Message);
        }

        private void btsLinkLabel_LinkClicked(Object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.btsLinkLabel.LinkVisited = true;
            Process.Start(this._btsUri.ToString());
        }
	}
}
