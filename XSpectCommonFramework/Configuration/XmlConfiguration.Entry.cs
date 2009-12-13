// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* XSpect Common Framework - Generic utility class library
 * Copyright c 2008-2009 Takeshi KIRIYA, XSpect Project <takeshik@users.sf.net>
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
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Achiral;
using Achiral.Extension;
using XSpect;
using XSpect.Extension;

namespace XSpect.Configuration
{
    partial class XmlConfiguration
    {
        public abstract class Entry
            : Object,
              IEquatable<Entry>
        {
            private static readonly Type _entryType
                = Type.GetType("XSpect.Configuration.XmlConfiguration+Entry`1");

            private String _name;

            private String _description;

            public XmlConfiguration Parent
            {
                get;
                protected set;
            }

            public String Key
            {
                get;
                set;
            }

            public String Name
            {
                get
                {
                    if (this.IsNameDefined)
                    {
                        return this._name;
                    }
                    else
                    {
                        return this.Parent
                            .GetHierarchy(this.Key)
                            .ElementAtOrDefault(1)
                            .Null(e => e.Name);
                    }
                }
                set
                {
                    this.IsNameDefined = value != null;
                    this._name = value;
                }
            }

            public Boolean IsNameDefined
            {
                get
                {
                    return this._name != null;
                }
                set
                {
                    if (value)
                    {
                        this.Name = null;
                    }
                    else if (this._name == null)
                    {
                        this._name = String.Empty;
                    }
                }
            }

            public String Description
            {
                get
                {
                    if (this.IsDescriptionDefined)
                    {
                        return this._description;
                    }
                    else
                    {
                        return this.Parent
                            .GetHierarchy(this.Key)
                            .ElementAtOrDefault(1)
                            .Null(e => e.Description);
                    }
                }
                set
                {
                    this.IsDescriptionDefined = value != null;
                    this._description = value;
                }
            }

            public Boolean IsDescriptionDefined
            {
                get
                {
                    return this._description != null;
                }
                set
                {
                    if (value)
                    {
                        this.Description = null;
                    }
                    else if (this._description == null)
                    {
                        this._description = String.Empty;
                    }
                }
            }

            public abstract Object UntypedValue
            {
                get;
                set;
            }

            public Boolean IsValueDefined
            {
                get;
                set;
            }

            public abstract Type Type
            {
                get;
            }

            public override Boolean Equals(Object obj)
            {
                return obj is Entry && this.Equals(obj as Entry);
            }

            public override Int32 GetHashCode()
            {
                return unchecked
                    ((this.Parent != null ? Parent.GetHashCode() : 0) * 397) ^
                    (this.Key != null ? this.Key.GetHashCode() : 0);
            }

            public override String ToString()
            {
                return String.Format("{0} = {1} ({2}: {3})", this.Key, this.UntypedValue, this.Name ?? "(null)", this.Description ?? "(null)");
            }

            #region Implementation of IEquatable<Entry>

            public Boolean Equals(Entry other)
            {
                if (ReferenceEquals(null, other))
                {
                    return false;
                }
                if (ReferenceEquals(this, other))
                {
                    return true;
                }
                return this.Parent == other.Parent
                    && this.Key == other.Key;
            }

            #endregion

            public TReturn Get<TReturn>()
            {
                return (TReturn) this.UntypedValue;
            }

            public static Entry Create(XmlConfiguration parent, Type type, String key, Object value, String name, String description)
            {
                Entry entry = _entryType
                    .MakeGenericType(type)
                    .GetConstructor(Make.Array(typeof(XmlConfiguration)))
                    .Invoke(Make.Array(parent)) as Entry;
                entry.Key = key;
                entry.UntypedValue = value;
                entry.Name = name;
                entry.Description = description;
                return entry;
            }

            public static Entry Create(XmlConfiguration parent, Type type, String key, String name, String description)
            {
                Entry entry = _entryType
                    .MakeGenericType(type)
                    .GetConstructor(Make.Array(typeof(XmlConfiguration)))
                    .Invoke(Make.Array(parent)) as Entry;
                entry.Key = key;
                entry.IsValueDefined = false;
                entry.Name = name;
                entry.Description = description;
                return entry;
            }

            public static Entry Create(XmlConfiguration parent, Type type, String key, Object value)
            {
                return Create(parent, type, key, value, null, null);
            }

            public static Entry Create(XmlConfiguration parent, Type type, String key)
            {
                return Create(parent, type, key, null, null);
            }
        }

        public class Entry<T>
            : Entry
        {
            private T _value;

            public override Object UntypedValue
            {
                get
                {
                    return this.Value;
                }
                set
                {
                    this.Value = (T) value;
                }
            }

            public override Type Type
            {
                get
                {
                    return typeof(T);
                }
            }

            public T Value
            {
                get
                {
                    if (this.IsValueDefined)
                    {
                        return this._value;
                    }
                    else
                    {
                        return this.Parent
                            .GetHierarchy<T>(this.Key)
                            .ElementAtOrDefault(1)
                            .Null(e => e.Value);
                    }
                }
                set
                {
                    this.IsValueDefined = true;
                    this._value = value;
                }
            }

            public T Get()
            {
                return this.Value;
            }

            public Entry(XmlConfiguration parent)
            {
                this.Parent = parent;
            }

            public static explicit operator T(Entry<T> self)
            {
                return self.Value;
            }
        }
    }
}