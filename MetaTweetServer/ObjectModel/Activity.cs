// -*- mode: csharp; encoding: utf-8; -*-
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetServer
 *   Server library of MetaTweet
 *   Part of MetaTweet
 * Copyright © 2008 Takeshi KIRIYA, XSpect Project <takeshik@xspect.org>
 * All rights reserved.
 * 
 * This file is part of MetaTweetServer.
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
using Achiral.Extension;

namespace XSpect.MetaTweet.ObjectModel
{
    [Serializable()]
    public class Activity
        : StorageObject
    {
        private Account _account;

        private DateTime _timestamp;

        private String _category;

        private String _value;

        private Object _data;

        private IEnumerable<String> _tags;

        public Account Account
        {
            get
            {
                return this._account;
            }
            set
            {
                this._account = value;
            }
        }

        public DateTime Timestamp
        {
            get
            {
                return this._timestamp;
            }
            set
            {
                this._timestamp = value;
            }
        }

        public String Category
        {
            get
            {
                return this._category;
            }
            set
            {
                this._category = value;
            }
        }

        public String Value
        {
            get
            {
                return this._value;
            }
            set
            {
                this._value = value;
            }
        }

        public Object Data
        {
            get
            {
                return　this._data;
            }
            set
            {
                this._data = value;
            }
        }

        public IEnumerable<String> Tags
        {
            get
            {
                return this._tags;
            }
            set
            {
                this._tags = value;
            }
        }
    }
}