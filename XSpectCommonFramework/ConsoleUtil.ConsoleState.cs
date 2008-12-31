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

namespace XSpect
{
    partial class ConsoleUtil
        : Object
    {
        private struct ConsoleState
        {
            private ConsoleColor _foregroundColor;
            private ConsoleColor _backgroundColor;
            private Int32 _cursorLeft;
            private Int32 _cursorTop;
            private Boolean _cursorVisible;
            private String _title;
            private Int32 _bufferWidth;
            private Int32 _bufferHeight;
            private Int32 _windowWidth;
            private Int32 _windowHeight;
            private Int32 _windowLeft;
            private Int32 _windowTop;

            public static ConsoleState Capture()
            {
                ConsoleState state;
                state._foregroundColor = Console.ForegroundColor;
                state._backgroundColor = Console.BackgroundColor;
                state._cursorLeft = Console.CursorLeft;
                state._cursorTop = Console.CursorTop;
                state._cursorVisible = Console.CursorVisible;
                state._title = Console.Title;
                state._bufferWidth = Console.BufferWidth;
                state._bufferHeight = Console.BufferHeight;
                state._windowWidth = Console.WindowWidth;
                state._windowHeight = Console.WindowHeight;
                state._windowLeft = Console.WindowLeft;
                state._windowTop = Console.WindowTop;
                return state;
            }

            public void Restore(Boolean restoreCursorPosition)
            {
                Console.ForegroundColor = this._foregroundColor;
                Console.BackgroundColor = this._backgroundColor;

                if (restoreCursorPosition)
                {
                    Console.CursorLeft = this._cursorLeft;
                    Console.CursorTop = this._cursorTop;
                }

                try
                {
                    Console.CursorVisible = this._cursorVisible;
                    Console.Title = this._title;
                    Console.BufferWidth = this._bufferWidth;
                    Console.BufferHeight = this._bufferHeight;
                    Console.WindowWidth = this._windowWidth;
                    Console.WindowHeight = this._windowHeight;
                    Console.WindowLeft = this._windowLeft;
                    Console.WindowTop = this._windowTop;
                }
                catch (Exception)
                {
                    // STUB
                    // TODO: Think what he have to do.
                }
            }
        }
    }
}