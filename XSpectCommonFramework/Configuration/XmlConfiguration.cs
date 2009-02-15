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
using Achiral;
using Achiral.Extension;
using System.Collections;

namespace XSpect.Configuration
{
    [XmlRoot(ElementName = "configuration", Namespace = "urn:XSpect.Configuration.XmlConfiguration")]
    public sealed class XmlConfiguration
        : IDictionary<String, List<Object>>,
          IXmlSerializable
    {
        private readonly Dictionary<String, KeyValuePair<Type, List<Object>>> _dictionary;

        #region IDictionary<String,List<Object>> メンバ

        public void Add(String key, List<Object> value)
        {
            this._dictionary.Add(key, new KeyValuePair<Type, List<Object>>(this.GetIdenticalType(value), value));
        }

        public Boolean ContainsKey(String key)
        {
            return this._dictionary.ContainsKey(key);
        }

        public ICollection<String> Keys
        {
            get
            {
                return this._dictionary.Keys;
            }
        }

        public Boolean Remove(String key)
        {
            return this._dictionary.Remove(key);
        }

        public Boolean TryGetValue(String key, out List<Object> value)
        {
            KeyValuePair<Type, List<Object>> outValue;
            Boolean result = this._dictionary.TryGetValue(key, out outValue);
            value = outValue.Value;
            return result;
        }

        public ICollection<List<Object>> Values
        {
            get
            {
                return this._dictionary.Values.Select(p => p.Value).ToArray();
            }
        }

        public List<Object> this[String key]
        {
            get
            {
                return this._dictionary[key].Value;
            }
            set
            {
                this._dictionary[key] = new KeyValuePair<Type, List<Object>>(this.GetIdenticalType(value), value);
            }
        }

        #endregion

        #region ICollection<KeyValuePair<String,List<Object>>> メンバ

        public void Add(KeyValuePair<String, List<Object>> item)
        {
            (this._dictionary as ICollection<KeyValuePair<String, KeyValuePair<Type, List<Object>>>>)
                .Add(this.GetInternalValue(item));
        }

        public void Clear()
        {
            this._dictionary.Clear();
        }

        public Boolean Contains(KeyValuePair<String, List<Object>> item)
        {
            return (this._dictionary as ICollection<KeyValuePair<String, KeyValuePair<Type, List<Object>>>>)
                .Contains(this.GetInternalValue(item));
        }

        public void CopyTo(KeyValuePair<String, List<Object>>[] array, Int32 arrayIndex)
        {
            (this._dictionary as ICollection<KeyValuePair<String, KeyValuePair<Type, List<Object>>>>)
                .CopyTo(array.Select(p => this.GetInternalValue(p)).ToArray(), arrayIndex);
        }

        public Int32 Count
        {
            get
            {
                return this._dictionary.Count;
            }
        }

        public Boolean IsReadOnly
        {
            get
            {
                return (this._dictionary as ICollection<KeyValuePair<String, KeyValuePair<Type, List<Object>>>>)
                    .IsReadOnly;
            }
        }

        public Boolean Remove(KeyValuePair<String, List<Object>> item)
        {
            return (this._dictionary as ICollection<KeyValuePair<String, KeyValuePair<Type, List<Object>>>>)
                .Remove(this.GetInternalValue(item));
        }

        #endregion

        #region IEnumerable<KeyValuePair<String,List<Object>>> メンバ

        public IEnumerator<KeyValuePair<String, List<Object>>> GetEnumerator()
        {
            return this._dictionary
                .Select(p => new KeyValuePair<String, List<Object>>(p.Key, p.Value.Value))
                .GetEnumerator();
        }

        #endregion

        #region IEnumerable メンバ

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

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
                Type type = Type.GetType(xentry.Attribute("type").Value);
                foreach (XElement xvalue in xentry.Elements())
                {
                    values.Add(new XmlSerializer(type).Deserialize(xvalue.CreateReader()));
                }
                this.Add(xentry.Attribute("key").Value, values);
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            XElement xentry;

            foreach (KeyValuePair<String, KeyValuePair<Type, List<Object>>> entry in this._dictionary)
            {
                xentry = new XElement("entry",
                    new XAttribute("key", entry.Key),
                    new XAttribute("type", entry.Value.Key.AssemblyQualifiedName)
                );

                foreach (Object value in entry.Value.Value)
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
            this._dictionary = new Dictionary<String, KeyValuePair<Type, List<Object>>>();
        }

        public void Add<T>(String key)
        {
            this._dictionary.Add(key, new KeyValuePair<Type, List<Object>>(typeof(T), new List<Object>()));
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
            using (MemoryStream stream = new MemoryStream())
            {
                new XmlSerializer(typeof(XmlConfiguration)).Serialize(XmlWriter.Create(stream), this);
                stream.Seek(0, SeekOrigin.Begin);
                XDocument xdoc = XDocument.Load(XmlReader.Create(stream));
                xdoc.Save(path);
            }
        }

        public static XmlConfiguration Load(String path)
        {
            return new XmlSerializer(typeof(XmlConfiguration)).Deserialize(XmlReader.Create(path)) as XmlConfiguration;
        }

        private Type GetIdenticalType(List<Object> value)
        {
            Type sample = value.First().GetType();
            if (!value.All(p => p.GetType() == sample))
            {
                throw new ArgumentException("Type in the sequence is not same", "value");
            }
            return sample;
        }

        private KeyValuePair<String, KeyValuePair<Type, List<Object>>> GetInternalValue(KeyValuePair<String, List<Object>> pair)
        {
            return new KeyValuePair<String, KeyValuePair<Type, List<Object>>>(
                pair.Key,
                new KeyValuePair<Type, List<Object>>(this.GetIdenticalType(pair.Value), pair.Value)
            );
        }
    }
}
