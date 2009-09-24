// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
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
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;
using Achiral;
using Achiral.Extension;
using XSpect;
using XSpect.Extension;

namespace XSpect.Configuration
{
    [XmlRoot("configuration", Namespace = "urn:XSpect.Configuration.XmlConfiguration")]
    public partial class XmlConfiguration
        : KeyedCollection<String, XmlConfiguration.Entry>,
          IXmlSerializable
    {
        public const Int32 Version = 2;

        public new Entry this[String key]
        {
            get
            {
                return this.Resolve(key);
            }
        }

        public FileInfo ConfigurationFile
        {
            get;
            set;
        }

        public ICollection<XmlConfiguration> BaseConfigurations
        {
            get;
            set;
        }

        public IEnumerable<XmlConfiguration> Hierarchy
        {
            get
            {
                return Make.Array(this)
                    .Concat(this.BaseConfigurations
                        .CascadeBreadthFirst(c => c.BaseConfigurations)
                    );
            }
        }

        public Boolean IsExterned
        {
            get;
            set;
        }

        public XmlConfiguration()
        {
            this.BaseConfigurations = new List<XmlConfiguration>();
        }

        public XmlConfiguration(FileInfo file)
            : this()
        {
            this.ConfigurationFile = file;
        }

        public XmlConfiguration(String path)
            : this(new FileInfo(path))
        {
        }

        public static XmlConfiguration Load(FileInfo file)
        {
            XmlConfiguration config = new XmlConfiguration(file);
            if (file.Exists)
            {
                config.Import(file);
            }
            return config;
        }

        public static XmlConfiguration Load(String path)
        {
            return Load(new FileInfo(path));
        }

        protected override String GetKeyForItem(Entry item)
        {
            return item.Key;
        }

        protected override void InsertItem(Int32 index, Entry item)
        {
            (item as Entry<XmlConfiguration>).Null(entry => entry.Value.BaseConfigurations.AddRange(
                this.BaseConfigurations
                    .Select(c => c.TryResolve<XmlConfiguration>(entry.Key))
                    .Where(e => e != null)
                    .Select(e => e.Value)
            ));
            base.InsertItem(index, item);
        }

        #region Implementation of IXmlSerializable

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            // HACK: Measure for XML Serializing, calls .ctor(), so ConfigurationFile is null.
            if (this.ConfigurationFile == null && !reader.BaseURI.IsNullOrEmpty())
            {
                this.ConfigurationFile = new FileInfo(new Uri(reader.BaseURI).LocalPath);
            }
            XDocument.Load(reader).Root.Elements().ForEach(xe =>
            {
                switch (xe.Name.LocalName)
                {
                    case "metadata":
                        if (xe.Attribute("version").Value != Version.ToString())
                        {
                            throw new InvalidDataException(String.Format(
                                "Invalid version: expected v{0} but this is v{1}",
                                Version,
                                xe.Attribute("version").Value
                            ));
                        }
                        break;
                    case "refer":
                        this.Clear();
                        FileInfo file = new FileInfo(new Uri(
                            new Uri(this.ConfigurationFile.FullName),
                            xe.Attribute("path").Value).LocalPath
                        );
                        this.ReadXml(XmlReader.Create(file.FullName));
                        this.ConfigurationFile = file;
                        this.IsExterned = true;
                        return;
                    case "base":
                        this.BaseConfigurations
                            .Add(Load(new Uri(
                                new Uri(this.ConfigurationFile.FullName),
                                xe.Attribute("path").Value).LocalPath)
                            );
                        break;
                    case "entry":
                        XComment xcname = xe.Nodes().OfType<XComment>()
                            .SingleOrDefault(xc => xc.Value.StartsWith("NAME: "));
                        XComment xcdescription = xe.Nodes().OfType<XComment>()
                            .SingleOrDefault(xc => xc.Value.StartsWith("DESC: "));
                        XElement xvalue = xe.Elements().FirstOrDefault();
                        Type type = Type.GetType(xe.Attribute("type").Value);
                        this.Update(xvalue != null
                            ? Entry.Create(
                                  this,
                                  type,
                                  xe.Attribute("key").Value,
                                  type == typeof(XmlConfiguration)
                                      ? new XmlConfiguration(this.ConfigurationFile)
                                            .Do(c => xvalue.CreateReader().Dispose(c.ReadXml))
                                      : new XmlSerializer(type).Deserialize(xvalue.CreateReader()),
                                  xcname.Null(xc => xcname.Value.Substring(6)), // "NAME: "
                                  xcdescription.Null(xc => xcdescription.Value.Substring(6)) // "DESC: "
                              )
                            : Entry.Create(
                                  this,
                                  type,
                                  xe.Attribute("key").Value,
                                  xcname.Null(xc => xcname.Value.Substring(6)), // "NAME: "
                                  xcdescription.Null(xc => xcdescription.Value.Substring(6)) // "DESC: "
                              )
                        );
                        break;
                    default:
                        break;
                }
            });
        }

        public void WriteXml(XmlWriter writer)
        {
            new XNode[]
            {
                new XComment(
                    " Generated by XmlConfiguration class in XSpect Common Framework "
                ),
                new XElement("metadata",
                    new XAttribute("version", Version)
                ),
            }
                .Concat(this.BaseConfigurations
                    .Where(x => x.ConfigurationFile != null)
                    .Select(config =>
                        new XElement("base",
                            new XAttribute(
                                "path",
                                new Uri(this.ConfigurationFile.FullName)
                                    .MakeRelativeUri(new Uri(config.ConfigurationFile.FullName))
                            )
                        ) as XNode
                    )
                )
                .Concat(this.Select(entry =>
                {
                    XmlConfiguration config;
                    XElement xvalue;

                    if(!entry.IsValueDefined)
                    {
                        xvalue = null;
                    }
                    else if (entry is Entry<XmlConfiguration>
                        && (config = (entry as Entry<XmlConfiguration>).Value).IsExterned)
                    {
                        config.Save();
                        xvalue = new XElement("refer",
                            new XAttribute(
                                "path",
                                new Uri(this.ConfigurationFile.FullName)
                                    .MakeRelativeUri(new Uri(config.ConfigurationFile.FullName))
                            )
                        );
                    }
                    else
                    {
                        xvalue = new MemoryStream().Dispose(s =>
                        {
                            new XmlSerializer(entry.Type).Serialize(s, entry.UntypedValue);
                            s.Seek(0, SeekOrigin.Begin);
                            return XmlReader.Create(s).Dispose(r => XElement.Load(r));
                        });
                    }

                    return new XElement("entry",
                        new XAttribute("key", entry.Key),
                        new XAttribute("type", entry.Type.AssemblyQualifiedName),
                        entry.IsNameDefined ? new XComment("NAME: " + entry.Name) : null,
                        entry.IsDescriptionDefined ? new XComment("DESC: " + entry.Description) : null,
                        xvalue
                    ) as XNode;
                }))
                .ForEach(xn => xn.WriteTo(writer));
        }

        #endregion

        public void Add<T>(String key, T value, String name, String description)
        {
            this.Add(new Entry<T>(this)
            {
                Key = key,
                Name = name,
                Description = description,
                Value = value,
            });
        }

        public void Add<T>(String key, String name, String description)
        {
            this.Add(new Entry<T>(this)
            {
                Key = key,
                Name = name,
                Description = description,
                IsValueDefined = false,
            });
        }

        public void Add<T>(String key, T value)
        {
            this.Add(key, value, null, null);
        }

        public void Add<T>(String key)
        {
            this.Add<T>(key, null, null);
        }

        public Boolean Update<T>(String key, T value, String name, String description)
        {
            if (this.Contains(key))
            {
                Entry<T> entry = this.Get<T>(key);
                entry.Value = value;
                entry.Name = name;
                entry.Description = description;
                return false;
            }
            else
            {
                this.Add(key, value, name, description);
                return true;
            }
        }

        public Boolean Update<T>(String key, String name, String description)
        {
            if (this.Contains(key))
            {
                Entry<T> entry = this.Get<T>(key);
                entry.IsValueDefined = false;
                entry.Name = name;
                entry.Description = description;
                return false;
            }
            else
            {
                this.Add<T>(key, name, description);
                return true;
            }
        }

        public Boolean Update<T>(String key, T value)
        {
            if (this.Contains(key))
            {
                this.Get<T>(key).Value = value;
                return false;
            }
            else
            {
                this.Add(key, value);
                return true;
            }
        }

        public Boolean Update<T>(String key)
        {
            if (this.Contains(key))
            {
                this.Get<T>(key).IsValueDefined = false;
                return false;
            }
            else
            {
                this.Add<T>(key);
                return true;
            }
        }

        public Boolean Update(Entry entry)
        {
            Boolean contained = this.Contains(entry);
            if (contained)
            {
                this.Remove(entry);
            }
            this.Add(entry);
            return contained;
        }

        public Boolean Exists(String key)
        {
            return this.Hierarchy.Any(c => c.Contains(key));
        }

        public Entry Get(String key)
        {
            return base[key];
        }

        public Entry Get(params String[] keys)
        {
            return this.Walk((c, k) => c.ResolveChild(k), keys.Take(keys.Count() - 1))[keys.Last()];
        }

        public Entry TryGet(params String[] keys)
        {
            XmlConfiguration config = this.Walk((c, k) => c.ResolveChild(k), keys.Take(keys.Count() - 1));
            String key = keys.Last();
            return config.Contains(key) ? config.Get(key) : null;
        }

        public Boolean TryGet(String key, out Entry value)
        {
            return this.Contains(key)
                ? (value = this.Get(key)).True()
                : (value = null).False();
        }

        public Entry<T> Get<T>(params String[] keys)
        {
            return this.Get(keys) as Entry<T>;
        }

        public Entry<T> TryGet<T>(params String[] keys)
        {
            return this.TryGet(keys) as Entry<T>;
        }

        public Boolean TryGet<T>(String key, out Entry<T> value)
        {
            return this.Contains(key)
                ? (value = this.Get<T>(key)).True()
                : (value = null).False();
        }

        public T GetValue<T>(params String[] keys)
        {
            return (this.Get(keys) as Entry<T>).Value;
        }

        public Boolean TryGetValue<T>(String key, out T value)
        {
            return this.Contains(key)
                ? (value = this.GetValue<T>(key)).True()
                : (value = default(T)).False();
        }

        public XmlConfiguration GetChild(String key)
        {
            return this.GetValue<XmlConfiguration>(key);
        }

        public Entry Resolve(String key)
        {
            foreach (XmlConfiguration config in this.Hierarchy)
            {
                if (config.Contains(key))
                {
                    return config.Get(key);
                }
            }
            throw new KeyNotFoundException();
        }

        public Entry Resolve(params String[] keys)
        {
            foreach (XmlConfiguration config in this
                .Walk((c, k) => c.ResolveChild(k), keys.Take(keys.Count() - 1))
                .Hierarchy
            )
            {
                String key = keys.Last();
                if (config.Contains(key))
                {
                    return config.Get(key);
                }
            }
            throw new KeyNotFoundException();
        }

        public Entry TryResolve(params String[] keys)
        {
            XmlConfiguration config = this.Walk((c, k) => c.ResolveChild(k), keys.Take(keys.Count() - 1));
            String key = keys.Last();
            return config.Exists(key) ? config.Resolve(key) : null;
        }

        public Boolean TryResolve(String key, out Entry value)
        {
            return this.Exists(key)
                ? (value = this.Resolve(key)).True()
                : (value = null).False();
        }

        public Entry<T> Resolve<T>(params String[] keys)
        {
            return this.Resolve(keys) as Entry<T>;
        }

        public Entry<T> TryResolve<T>(params String[] keys)
        {
            return this.Resolve(keys) as Entry<T>;
        }

        public Boolean TryResolve<T>(String key, out Entry<T> value)
        {
            return this.Exists(key)
                ? (value = this.Resolve<T>(key)).True()
                : (value = null).False();
        }

        public T ResolveValue<T>(params String[] keys)
        {
            return (this.Resolve(keys) as Entry<T>).Value;
        }

        public Boolean TryResolveValue<T>(String key, out T value)
        {
            return this.Exists(key)
                ? (value = this.ResolveValue<T>(key)).True()
                : (value = default(T)).False();
        }

        public IEnumerable<Entry> GetHierarchy(params String[] keys)
        {
            return this.Walk((c, k) => c.ResolveChild(k), keys.Take(keys.Count() - 1)).Hierarchy.Select(x => x.TryGet(keys.Last())).Where(e => e != null);
        }

        public IEnumerable<Entry<T>> GetHierarchy<T>(params String[] keys)
        {
            return this.Walk((c, k) => c.ResolveChild(k), keys.Take(keys.Count() - 1)).Hierarchy.Select(x => x.TryGet<T>(keys.Last())).Where(e => e != null);
        }

        public XmlConfiguration ResolveChild(String key)
        {
            return this.ResolveValue<XmlConfiguration>(key);
        }

        public IEnumerable<Entry<T>> OfEntryType<T>()
        {
            return this.OfType<Entry<T>>();
        }

        public IEnumerable<T> OfValueType<T>()
        {
            return this.OfEntryType<T>().Select(e => (T) e);
        }

        public IDictionary<String, Object> ToDictionary()
        {
            return this.ToDictionary(e => e.Key, e => e.UntypedValue);
        }

        public IDictionary<String, T> ToDictionary<T>()
        {
            return this.OfEntryType<T>().ToDictionary(e => e.Key, e => e.Value);
        }

        public void Import(FileInfo file)
        {
            if (this.ConfigurationFile == null)
            {
                this.ConfigurationFile = file;
            }
            this.ReadXml(XmlReader.Create(file.FullName));
        }

        public void Import(String path)
        {
            this.Import(new FileInfo(path));
        }

        public void Save()
        {
            this.Save(this.ConfigurationFile);
        }

        public void Save(FileInfo file)
        {
            new MemoryStream().Dispose(stream =>
                XmlWriter.Create(stream).Dispose(writer =>
                {
                    new XmlSerializer(typeof(XmlConfiguration)).Serialize(writer, this);
                    stream.Seek(0, SeekOrigin.Begin);
                    XDocument xdoc = XmlReader.Create(stream).Dispose(reader => XDocument.Load(reader));
                    xdoc.Save(file.FullName, SaveOptions.None);
                })
            );
        }

        public void Save(String path)
        {
            this.Save(new FileInfo(path));
        }
    }
}