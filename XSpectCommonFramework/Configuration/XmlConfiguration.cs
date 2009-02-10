// -*- mode: csharp; encoding: utf-8; -*-
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
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace XSpect.Configuration
{
    [XmlRoot(ElementName = "configuration", Namespace = "urn:XSpect.Configuration.XmlConfiguration")]
    public sealed class XmlConfiguration
        : Dictionary<String, List<Object>>,
          IXmlSerializable
    {
        #region IXmlSerializable メンバ

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            XDocument xdoc = XDocument.Load(reader);
            foreach (XElement xentry in xdoc.Descendants("entry"))
            {
                List<Object> values = new List<Object>();
                this.Add(xentry.Attribute("key").Value, values);
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            XElement xentry;

            foreach (KeyValuePair<String, List<Object>> entry in this)
            {
                xentry = new XElement("entry",
                    new XAttribute("key", entry.Key)
                );

                foreach (Object value in entry.Value)
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        new XmlSerializer(value.GetType()).Serialize(stream, value);
                        stream.Seek(0, SeekOrigin.Begin);
                        xentry.Add(XElement.Load(XmlReader.Create(stream)));
                    }
                }
                xentry.WriteTo(writer);
            }
        }

        #endregion

        public XmlConfiguration()
        {
        }

        public void Add(String key)
        {
            this.Add(key, new List<Object>());
        }

        public T GetValue<T>(String key)
        {
            return (T) this[key].Single();
        }

        public T GetValueOrDefault<T>(String key, T defaultValue)
        {
            if (this.ContainsKey(key))
            {
                return this.GetValue<T>(key);
            }
            else
            {
                this.Add(key, new List<Object>()
                {
                    defaultValue,
                });
                return defaultValue;
            }
        }

        public T GetValueOrDefault<T>(String key)
        {
            return this.GetValueOrDefault(key, default(T));
        }

        public IEnumerable<T> GetValues<T>(String key)
        {
            return this[key].OfType<T>();
        }

        public IEnumerable<T> GetValuesOrDefault<T>(String key, params T[] defaultValues)
        {
            if (this.ContainsKey(key))
            {
                return this.GetValues<T>(key);
            }
            else
            {
                this.Add(key, new List<Object>(defaultValues.Cast<Object>()));
                return defaultValues;
            }
        }

        public IEnumerable<T> GetValuesOrDefault<T>(String key)
        {
            return this.GetValuesOrDefault(key, default(T));
        }

        public Object GetValue(String key)
        {
            return this.GetValue<Object>(key);
        }

        public Object GetValueOrDefault(String key, Object defaultValue)
        {
            return this.GetValueOrDefault<Object>(key, defaultValue);
        }

        public Object GetValueOrDefault(String key)
        {
            return this.GetValueOrDefault<Object>(key);
        }

        public IEnumerable<Object> GetValues(String key)
        {
            return this.GetValues<Object>(key);
        }

        public IEnumerable<Object> GetValuesOrDefault(String key, params Object[] defaultValues)
        {
            return this.GetValuesOrDefault<Object>(key, defaultValues);
        }

        public IEnumerable<Object> GetValuesOrDefault(String key)
        {
            return this.GetValuesOrDefault<Object>(key);
        }

        public void Save(String path)
        {
            new XmlSerializer(typeof(XmlConfiguration)).Serialize(XmlWriter.Create(path), this);
        }

        public static XmlConfiguration Load(String path)
        {
            return new XmlSerializer(typeof(XmlConfiguration)).Deserialize(XmlReader.Create(path)) as XmlConfiguration;
        }
    }
}
