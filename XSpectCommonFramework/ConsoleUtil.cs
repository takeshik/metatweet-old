// -*- mode: csharp; encoding: utf-8; -*-
/* XSpect Common Framework - Generic Utility Class Library
 * Copyright © 2008-2009 Takeshi KIRIYA, XSpect Project <takeshik@xspect.org>
 * All rights reserved.
 * 
 * This file is part of XSpect Common Framework.
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
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace XSpect
{
    public static partial class ConsoleUtil
        : Object
    {
        public static void Write(String format)
        {
            /*String text = String.Empty;

            foreach (String str in format.Select(c => Convert.ToInt32(c) < 256
                ? c.ToString()
                : "&#x" + Convert.ToInt32(c).ToString("x") + ";"
            ))
            {
                text += str;
            }*/

            XDocument xml = XDocument.Parse(String.Format(
                #region XML
@"<?xml version=""1.0"" encoding=""utf-16"" standalone=""yes""?>
<XSConsoleMarkup>{0}</XSConsoleMarkup>",
                #endregion
                format
                    .Replace("\r\n", "\n")
                    .Replace('\r', '\n')
                    .Replace("\n", Environment.NewLine)
            ));
            Markup(xml.Element("XSConsoleMarkup").Nodes());
        }

        public static void Write(String format, params Object[] args)
        {
            Write(String.Format(format, args));
        }

        public static void WriteLine(String format)
        {
            Write(format + Environment.NewLine);
        }

        public static void WriteLine(String format, params Object[] args)
        {
            WriteLine(String.Format(format, args));
        }

        public static String WriteAndReadLine(String format)
        {
            Write(format);
            return Console.ReadLine();
        }

        public static String WriteAndReadLine(String format, params Object[] args)
        {
            Write(format, args);
            return Console.ReadLine();
        }

        private static void Markup(IEnumerable<XNode> nodes)
        {
            ConsoleState state = ConsoleState.Capture();

            foreach (XNode node in nodes)
            {
                Boolean wasCursorMovedByUser = false;
                if (node is XElement)
                {
                    XElement element = (XElement) node;
                    switch (element.Name.LocalName)
                    {
                        case "color":
                            if (element.Attribute("fg") != null)
                            {
                                Console.ForegroundColor = (ConsoleColor) Enum.Parse(typeof(ConsoleColor), element.Attribute("fg").Value, true);
                            }
                            if (element.Attribute("bg") != null)
                            {
                                Console.BackgroundColor = (ConsoleColor) Enum.Parse(typeof(ConsoleColor), element.Attribute("bg").Value, true);
                            }
                            break;
                        case "cursor":
                            if (element.Attribute("x") != null)
                            {
                                Int32 x = Int32.Parse(element.Attribute("x").Value);
                                Console.CursorLeft = x >= 0 ? x : Console.WindowWidth + x;
                                wasCursorMovedByUser = true;
                            }
                            else if (element.Attribute("dx") != null)
                            {
                                Console.CursorLeft += Int32.Parse(element.Attribute("dx").Value);
                                wasCursorMovedByUser = true;
                            }
                            if (element.Attribute("y") != null)
                            {
                                Int32 y = Int32.Parse(element.Attribute("y").Value);
                                Console.CursorTop = y >= 0 ? y : Console.WindowHeight + y;
                                wasCursorMovedByUser = true;
                            }
                            else if (element.Attribute("dy") != null)
                            {
                                Console.CursorTop += Int32.Parse(element.Attribute("dy").Value);
                                wasCursorMovedByUser = true;
                            }
                            if (element.Attribute("size") != null)
                            {
                                Console.CursorSize=Int32.Parse(element.Attribute("size").Value);
                            }
                            if (element.Attribute("show") != null)
                            {
                                Console.CursorVisible = Boolean.Parse(element.Attribute("show").Value);
                            }
                            break;
                        case "window":
                            if (element.Attribute("x") != null)
                            {
                                Console.WindowLeft = Int32.Parse(element.Attribute("x").Value);
                            }
                            if (element.Attribute("y") != null)
                            {
                                Console.WindowTop = Int32.Parse(element.Attribute("y").Value);
                            }
                            if (element.Attribute("width") != null)
                            {
                                Console.WindowWidth = Int32.Parse(element.Attribute("width").Value);
                            }
                            if (element.Attribute("height") != null)
                            {
                                Console.WindowHeight = Int32.Parse(element.Attribute("height").Value);
                            }
                            if (element.Attribute("bufwidth") != null)
                            {
                                Console.BufferWidth = Int32.Parse(element.Attribute("bufwidth").Value);
                            }
                            if (element.Attribute("bufheight") != null)
                            {
                                Console.BufferHeight = Int32.Parse(element.Attribute("bufheight").Value);
                            }
                            if (element.Attribute("title") != null)
                            {
                                Console.Title = element.Attribute("title").Value;
                            }
                            break;
                        case "beep":
                            if (element.Attribute("freq") != null)
                            {
                                Console.Beep(Int32.Parse(element.Attribute("freq").Value), Int32.Parse(element.Attribute("duration").Value));
                            }
                            else
                            {
                                Console.Beep();
                            }
                            break;
                    }

                    if (element.Nodes().Any())
                    {
                        Markup(element.Nodes());
                        state.Restore(wasCursorMovedByUser);
                    }
                }
                else if (node is XText)
                {
                    Console.Write(((XText) node).Value);
                }
            }
        }
    }
}
