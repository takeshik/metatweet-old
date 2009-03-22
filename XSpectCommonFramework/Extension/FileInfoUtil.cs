﻿// -*- mode: csharp; encoding: utf-8; -*-
/* XSpect Common Framework - Generic Utility Class Library
 * Copyright © 2008-2009 Takeshi KIRIYA, XSpect Project <takeshik@users.sf.net>
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
using System.IO;
using Achiral.Extension;

namespace XSpect.Extension
{
    public static class FileInfoUtil
        : Object
    {
        public static Byte[] ReadAllBytes(this FileInfo file)
        {
            return File.ReadAllBytes(file.FullName);
        }

        public static String[] ReadAllLines(this FileInfo file)
        {
            return File.ReadAllLines(file.FullName);
        }

        public static String ReadAllText(this FileInfo file)
        {
            return File.ReadAllText(file.FullName);
        }

        public static void WriteAllBytes(this FileInfo file, Byte[] bytes)
        {
            File.WriteAllBytes(file.FullName, bytes);
        }

        public static void WriteAllLines(this FileInfo file, params String[] lines)
        {
            File.WriteAllLines(file.FullName, lines);
        }

        public static void WriteAllText(this FileInfo file, String text)
        {
            File.WriteAllText(file.FullName, text);
        }

    }
}