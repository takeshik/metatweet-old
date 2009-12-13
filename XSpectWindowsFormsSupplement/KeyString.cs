// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* XSpect Windows Forms Supplement - Supplemental library for Windows Forms
 * Copyright c 2009 Takeshi KIRIYA, XSpect Project <takeshik@users.sf.net>
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
using System.Linq;
using System.Windows.Forms;
using Achiral.Extension;
using XSpect.Extension;

namespace XSpect.Windows.Forms
{
    public static class KeyString
    {
        public const String Control = "C-";

        public const String Meta = "M-";

        public const String Shift = "S-";

        public static Keys[] GetKeysArray(String keyString)
        {
            return keyString.Split(' ').Select(s =>
            {
                Keys keys = (Keys) Enum.Parse(typeof(Keys), s.Substring(s.LastIndexOf('-') + 1), true);
                if (s.Contains(Control))
                {
                    keys |= Keys.Control;
                }
                if (s.Contains(Meta))
                {
                    keys |= Keys.Alt;
                }
                if (s.Contains(Shift))
                {
                    keys |= Keys.Shift;
                }
                return keys;
            }).ToArray();
        }

        public static String ToKeyString(this Keys keys)
        {
            if (
                (keys & Keys.KeyCode) == Keys.ControlKey ||
                (keys & Keys.KeyCode) == Keys.Menu ||
                (keys & Keys.KeyCode) == Keys.ShiftKey
            )
            {
                return null;
            }
            return
                ((keys & Keys.Control) == Keys.Control ? Control : String.Empty) +
                ((keys & Keys.Alt) == Keys.Alt ? Meta : String.Empty) +
                ((keys & Keys.Shift) == Keys.Shift ? Shift : String.Empty) +
                (keys & Keys.KeyCode).ToString().If(s => s.Length == 1, s => s.ToLower());
        }
    }
}
