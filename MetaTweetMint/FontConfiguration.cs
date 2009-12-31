// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetMint
 *   Extensible GUI client for MetaTweet
 *   Part of MetaTweet
 * Copyright © 2009-2010 Takeshi KIRIYA (aka takeshik) <takeshik@users.sf.net>
 * All rights reserved.
 * 
 * This file is part of MetaTweetMint.
 * 
 * This program is free software; you can redistribute it and/or modify it
 * under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation; either version 3 of the License, or (at your
 * option) any later version.
 * 
 * This program is distributed in the hope that it will be useful, but
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
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Windows.Forms;
using Achiral;
using Achiral.Extension;
using Microsoft.Scripting.Hosting;
using XSpect.Configuration;
using XSpect.Extension;
using XSpect.MetaTweet.Modules;
using XSpect.Reflection;

namespace XSpect.MetaTweet.Clients.Mint
{
    public sealed class FontConfiguration
        : Object
    {
        private static readonly FontConverter _fontConverter = new FontConverter();

        private readonly XmlConfiguration _configuration;

        private Font _default;

        private Font _monospace;

        private Font _minibuffer;

        private Font _minibufferTitle;

        private Font _modeLine;

        private Font _statusBar;

        public Font Default
        {
            get
            {
                return this._default ?? (this._default = this.GetFont(
                    this._configuration.ResolveValue<String>("default")
                ));
            }
        }

        public Font Monospace
        {
            get
            {
                return this._monospace ?? (this._monospace = this.GetFont(
                    this._configuration.ResolveValue<String>("monospace")
                ));
            }
        }

        public Font Minibuffer
        {
            get
            {
                return this._minibuffer ?? (this._minibuffer = this.GetFont(
                    this._configuration.ResolveValue<String>("minibuffer")
                ));
            }
        }

        public Font MinibufferTitle
        {
            get
            {
                return this._minibufferTitle ?? (this._minibufferTitle = this.GetFont(
                    this._configuration.ResolveValue<String>("minibufferTitle")
                ));
            }
        }

        public Font ModeLine
        {
            get
            {
                return this._modeLine ?? (this._modeLine = this.GetFont(
                    this._configuration.ResolveValue<String>("modeLine")
                ));
            }
        }

        public Font StatusBar
        {
            get
            {
                return this._statusBar ?? (this._statusBar = this.GetFont(
                    this._configuration.ResolveValue<String>("statusBar")
                ));
            }
        }

        public FontConfiguration(XmlConfiguration configuration)
        {
            this._configuration = configuration;
        }

        private Font GetFont(String str)
        {
            if (str.StartsWith("!"))
            {
                return this.GetType()
                    .GetProperty(str.Substring(1), BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase)
                    .GetValue(this, null) as Font;
            }
            else
            {
                return _fontConverter.ConvertFromString(str) as Font;
            }
        }
    }
}