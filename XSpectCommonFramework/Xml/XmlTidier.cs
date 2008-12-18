// -*- mode: csharp; encoding: utf-8; -*-
/* XSpect Common Framework - Generic Utility Class Library
 * Copyright © 2008 Takeshi KIRIYA, XSpect Project <takeshik@xspect.org>
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
using System.IO;
using System.Text;
using System.Xml;
using TidyNet;

namespace XSpect.Xml
{
    public sealed class XmlTidier
    {
        private Tidy _tidy = new Tidy();

        public XmlTidier()
        {
            this._tidy.Options.BreakBeforeBR = true;
            this._tidy.Options.CharEncoding = CharEncoding.UTF8;
            this._tidy.Options.DocType = DocType.Omit;
            this._tidy.Options.DropEmptyParas = true;
            this._tidy.Options.DropFontTags = true;
            this._tidy.Options.EncloseBlockText = true;
            this._tidy.Options.EncloseText = true;
            this._tidy.Options.FixBackslash = true;
            this._tidy.Options.FixComments = true;
            this._tidy.Options.LiteralAttribs = false;
            this._tidy.Options.LogicalEmphasis = true;
            this._tidy.Options.MakeClean = true;
            this._tidy.Options.NumEntities = true;
            this._tidy.Options.QuoteAmpersand = true;
            this._tidy.Options.QuoteMarks = true;
            this._tidy.Options.QuoteNbsp = true;
            this._tidy.Options.WrapAsp = true;
            this._tidy.Options.WrapAttVals = true;
            this._tidy.Options.WrapJste = true;
            this._tidy.Options.WrapPhp = true;
            this._tidy.Options.WrapScriptlets = true;
            this._tidy.Options.WrapSection = true;
            this._tidy.Options.Xhtml = true;
            this._tidy.Options.XmlOut = true;
        }

        public Stream Tidy(Stream input)
        {
            MemoryStream output = new MemoryStream();
            this._tidy.Parse(input, output, new TidyMessageCollection());
            return output;
        }

        public XmlDocument Parse(String input)
        {
            using (MemoryStream inputStream = new MemoryStream(Encoding.UTF8.GetBytes(input), false))
            {
                using (MemoryStream outputStream = new MemoryStream())
                {
                    this._tidy.Parse(inputStream, outputStream, new TidyMessageCollection());
                    XmlDocument xdoc = new XmlDocument();
                    xdoc.Load(outputStream);
                    return xdoc;
                }
            }
        }
    }
}
