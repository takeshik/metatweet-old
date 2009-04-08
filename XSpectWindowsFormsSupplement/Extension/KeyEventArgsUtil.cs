// -*- mode: csharp; encoding: utf-8; -*-
/* XSpect Windows Forms Supplement - Supplemental library for Windows Forms
 * Copyright © 2009 Takeshi KIRIYA, XSpect Project <takeshik@users.sf.net>
 * All rights reserved.
 * 
 * This file is part of XSpect Windows Forms Supplement.
 * 
 * This library is free software; you can redistribute it and/or modify it
 * under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation; either version 3 of the License, or (at your
 * option) any later version.
 * 
 * This library is distributed in the hope that it will be useful, but
 * WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
 * or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public
 * License for more details. 
 * 
 * You should have received a copy of the GNU Lesser General Public License
 * along with this program. If not, see <http://www.gnu.org/licenses/>,
 * or write to the Free Software Foundation, Inc., 51 Franklin Street,
 * Fifth Floor, Boston, MA 02110-1301, USA.
 */

using System;
using System.Windows.Forms;

namespace XSpect.Extension
{
    public static class KeyEventArgsUtil
    {
        public static String ToKeyString(this KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey || e.KeyCode == Keys.Menu || e.KeyCode == Keys.ShiftKey)
            {
                return null;
            }
            return
                (e.Control ? "C-" : String.Empty) +
                (e.Alt ? "M-" : String.Empty) +
                (e.Shift ? "S-" : String.Empty) +
                e.KeyCode.ToString().If(s => s.Length == 1, s => s.ToLower());
        }
    }
}