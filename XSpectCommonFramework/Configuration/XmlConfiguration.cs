// -*- mode: csharp; encoding: utf-8; -*-
// $Id$
/* XSpect Common Framework - Generic utility class library
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
using XSpect.Extension;

namespace XSpect.Configuration
{
    [XmlRoot(ElementName = "configuration", Namespace = "urn:XSpect.Configuration.XmlConfiguration")]
    public class XmlConfiguration
        : IDictionary<String, Object>,
          IXmlSerializable
    {
        private readonly Dictionary<String, KeyValuePair<Type, Object>> _dictionary;

        public FileInfo ConfigurationFile
        {
            get;
            protected set;
        }

        private ICollection<KeyValuePair<String, KeyValuePair<Type, Object>>> _dictionaryCollection
        {
            get
            {
                return this._dictionary;
            }
        }

        #region IDictionary<String,Object> メンバ

        public void Add(String key, Object value)
        {
            this._dictionary.Add(key, this.GetInternalValue(value));
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

        public Boolean TryGetValue(String key, out Object value)
        {
            return this.TryGetValue<Object>(key, out value);
        }

        public ICollection<Object> Values
        {
            get
            {
                return this._dictionary.Values.Select(p => p.Value).ToArray();
            }
        }

        public Object this[String key]
        {
            get
            {
                return this.GetValue(key);
            }
            set
            {
                this.SetValue(key, value);
            }
        }

        #endregion

        #region ICollection<KeyValuePair<String,Object>> メンバ

        public void Add(KeyValuePair<String, Object> item)
        {
            this._dictionaryCollection.Add(this.GetInternalValue(item));
        }

        public void Clear()
        {
            this._dictionary.Clear();
        }

        public Boolean Contains(KeyValuePair<String, Object> item)
        {
            return this._dictionaryCollection.Contains(this.GetInternalValue(item));
        }

        public void CopyTo(KeyValuePair<String, Object>[] array, Int32 arrayIndex)
        {
            this._dictionaryCollection.CopyTo(
                array
                    .Select(p => this.GetInternalValue(p))
                    .ToArray(),
                arrayIndex
            );
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
                return this._dictionaryCollection.IsReadOnly;
            }
        }

        public Boolean Remove(KeyValuePair<String, Object> item)
        {
            return this._dictionaryCollection.Remove(this.GetInternalValue(item));
        }

        #endregion

        #region IEnumerable<KeyValuePair<String,Object>> メンバ

        public IEnumerator<KeyValuePair<String, Object>> GetEnumerator()
        {
            return this._dictionary
                .Select(p => Create.KeyValuePair(p.Key, p.Value.Value))
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
            foreach (XElement xentry in xdoc.Root.Elements("entry"))
            {
                this.Add(
                    xentry.Attribute("key").Value,
                    new XmlSerializer(Type.GetType(xentry.Attribute("type").Value))
                        .Deserialize(xentry.Elements().SingleOrDefault().CreateReader())
                );
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            XElement xentry;

            foreach (KeyValuePair<String, KeyValuePair<Type, Object>> entry in this._dictionary)
            {
                xentry = new XElement("entry",
                    new XAttribute("key", entry.Key),
                    new XAttribute("type", entry.Value.Key.AssemblyQualifiedName)
                );

                using (MemoryStream stream = new MemoryStream())
                {
                    new XmlSerializer(entry.Value.Key).Serialize(stream, entry.Value.Value);
                    stream.Seek(0, SeekOrigin.Begin);
                    xentry.Add(XElement.Load(XmlReader.Create(stream)));
                }

                xentry.WriteTo(writer);
            }
        }

        #endregion

        public XmlConfiguration()
        {
            this._dictionary = new Dictionary<String, KeyValuePair<Type, Object>>();
        }

        public static XmlConfiguration Load(FileInfo file)
        {
            XmlConfiguration config;
            if (file.Exists)
            {
                config = XmlReader.Create(file.FullName)
                    .Dispose(reader => (XmlConfiguration) new XmlSerializer(typeof(XmlConfiguration))
                                                              .Deserialize(reader)
                    );
                config.ConfigurationFile = file;
            }
            else
            {
                config = new XmlConfiguration();
                config.Save(file);
            }
            return config;
        }

        public static XmlConfiguration Load(String path)
        {
            return Load(new FileInfo(path));
        }

        public void Save(FileInfo file)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (XmlWriter writer = XmlWriter.Create(stream))
                {
                    new XmlSerializer(typeof(XmlConfiguration)).Serialize(writer, this);
                }
                stream.Seek(0, SeekOrigin.Begin);
                XDocument xdoc = XmlReader.Create(stream).Dispose(reader => XDocument.Load(reader));
                xdoc.Save(file.FullName);
            }
            this.ConfigurationFile = file;
        }

        public void Save(String path)
        {
            this.Save(new FileInfo(path));
        }

        public void Save()
        {
            this.Save(this.ConfigurationFile);
        }

        private KeyValuePair<Type, Object> GetInternalValue(Object value)
        {
            return Create.KeyValuePair(value != null ? value.GetType() : typeof(Object), value);
        }

        private KeyValuePair<String, KeyValuePair<Type, Object>> GetInternalValue(KeyValuePair<String, Object> item)
        {
            return Create.KeyValuePair(item.Key, this.GetInternalValue(item.Value));
        }

        public T GetValue<T>(String key)
        {
            return (T) this._dictionary[key].Value;
        }

        public Object GetValue(String key)
        {
            return this.GetValue<Object>(key);
        }

        public void SetValue<T>(String key, T value)
        {
            this._dictionary[key] = this.GetInternalValue(value);
        }

        public void SetValue(String key, Object value)
        {
            this.SetValue<Object>(key, value);
        }

        public Boolean TryGetValue<T>(String key, out T value)
        {
            KeyValuePair<Type, Object> outValue;
            Boolean result = this._dictionary.TryGetValue(key, out outValue);
            value = result ? (T) outValue.Value : default(T);
            return result;
        }

        public T GetValueOrDefault<T>(String key, T defaultValue)
        {
            T value;
            if (this.TryGetValue<T>(key, out value))
            {
                return value;
            }
            else
            {
                this.Add(key, defaultValue);
                return defaultValue;
            }
        }

        public T GetValueOrDefault<T>(String key)
        {
            return this.GetValueOrDefault<T>(key, default(T));
        }

        public Object GetValueOrDefault(String key, Object defaultValue)
        {
            return this.GetValueOrDefault<Object>(key, defaultValue);
        }

        public Object GetValueOrDefault(String key)
        {
            return this.GetValueOrDefault<Object>(key);
        }

        public XmlConfiguration GetChild(String key)
        {
            return this.GetValueOrDefault(key, new XmlConfiguration());
        }
    }
}
