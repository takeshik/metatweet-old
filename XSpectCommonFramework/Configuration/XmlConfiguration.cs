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
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace XSpect.Configuration
{
    public class XmlConfiguration
        : Object
    {
        XmlDocument _configDocument;

        XDocument _configXDocument;

        public XmlElement RootElement
        {
            get
            {
                return this._configDocument.DocumentElement;
            }
        }

        public XElement RootXElement
        {
            get
            {
                return this._configXDocument.Root;
            }
        }

        public XmlConfiguration()
        {
            this._configDocument = new XmlDocument();
            this._configXDocument = new XDocument();
        }

        public static String GetParent(String path)
        {
            return path.Substring(0, path.LastIndexOf('/'));
        }

        private XPathNavigator RetrievePath(String path)
        {
            return this.RetrievePath(path, true);
        }

        private XPathNavigator RetrievePath(String path, Boolean createIfNotExists)
        {
            XPathNavigator xnav = this._configDocument.CreateNavigator();
            xnav = xnav.SelectSingleNode("/configuration");
            foreach (String fragment in path.Split(new Char[] { '/', }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (fragment.StartsWith("@"))
                {
                    if (xnav.SelectSingleNode(fragment) == null)
                    {
                        xnav.CreateAttribute(null, fragment.Substring(1), null, null);
                        return xnav.SelectSingleNode(fragment);
                    }
                }
                else
                {
                    switch (xnav.Select(fragment).Count)
                    {
                        case 0:
                            if (createIfNotExists)
                            {
                                xnav.AppendChildElement(null, fragment, null, null);
                            }
                            break;
                        case 1:
                            break;
                        default:
                            return null;
                    }
                    xnav = xnav.SelectSingleNode(fragment);
                }
            }
            return xnav;
        }

        public void Load(String fileName)
        {
            this._configXDocument = XDocument.Load(fileName);
            this._configDocument.Load(this._configXDocument.CreateReader());
        }

        public void Load(XmlReader reader)
        {
            this._configDocument.Load(reader);
            this._configDocument.Load(reader);
        }

        public void Save(String fileName)
        {
            this._configDocument.Save(fileName);
        }

        public void Save(XmlWriter writer)
        {
            this._configDocument.Save(writer);
        }

        public Boolean TryGetValues<T>(String path, out IEnumerable<T> value)
        {
            XPathNodeIterator xiter = this.RootElement.CreateNavigator().Select("/configuration/" + path);
            value = xiter.Count > 0 ? xiter.Cast<XPathNavigator>().Select(xnav => (T) xnav.ValueAs(typeof(T))) : null;
            return xiter.Count > 0;
        }

        public IEnumerable<T> GetValues<T>(String path)
        {
            IEnumerable<T> values;
            if (this.TryGetValues<T>(path, out values))
            {
                return values;
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }

        public IEnumerable<T> GetValuesOrDefault<T>(String path)
        {
            IEnumerable<T> values;
            this.TryGetValues<T>(path, out values);
            return values;
        }

        public IEnumerable<T> GetValuesOrSet<T>(String path, IEnumerable<T> alternates)
        {
            IEnumerable<T> values;
            if (this.TryGetValues<T>(path, out values))
            {
                return values;
            }
            else
            {
                this.SetValues<T>(path, alternates);
                return alternates;
            }
        }

        public IEnumerable<T> GetValuesOrSet<T>(String path, T alternate)
        {
            IEnumerable<T> values;
            if (this.TryGetValues<T>(path, out values))
            {
                return values;
            }
            else
            {
                this.SetValue<T>(path, alternate);
                return new T[] { alternate, };
            }
        }

        public Boolean TryGetValue<T>(String path, out T value)
        {
            IEnumerable<T> result;
            this.TryGetValues<T>(path, out result);
            value = result.FirstOrDefault();
            return result.Count() == 1;
        }

        public T GetValue<T>(String path)
        {
            T value;
            if (this.TryGetValue<T>(path, out value))
            {
                return value;
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }

        public T GetValueOrDefault<T>(String path)
        {
            T value;
            this.TryGetValue<T>(path, out value);
            return value;
        }

        public T GetValueOrSet<T>(String path, T alternate)
        {
            T value;
            if (this.TryGetValue<T>(path, out value))
            {
                return value;
            }
            else
            {
                this.SetValue<T>(path, alternate);
                return alternate;
            }
        }

        public Boolean TryGetElement(String path, out XmlElement element)
        {
            XmlNode xnode = this.RootElement.SelectSingleNode("/configuration/" + path);
            if (xnode != null && xnode is XmlElement)
            {
                element = (XmlElement) xnode;
                return true;
            }
            else
            {
                element = null;
                return false;
            }
        }

        public XmlElement GetElement(String path)
        {
            XmlElement xelement;
            if (this.TryGetElement(path, out xelement))
            {
                return xelement;
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }

        public XmlElement GetElementOrDefault(String path)
        {
            XmlElement xelement;
            this.TryGetElement(path, out xelement);
            return xelement;
        }

        public Boolean TryGetElements(String path, out XmlElement[] elements)
        {
            XmlNodeList xnodes = this.RootElement.SelectNodes("/configuration/" + path);
            if (xnodes.Count > 0)
            {
                elements = xnodes.OfType<XmlElement>().ToArray();
                return true;
            }
            else
            {
                elements = null;
                return false;
            }
        }

        public XmlElement[] GetElements(String path)
        {
            XmlElement[] xelements;
            if (this.TryGetElements(path, out xelements))
            {
                return xelements;
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }

        // TODO: (Try)GetXElement(s)

        public void AddValue<T>(String path, T value)
        {
            this.AddValues<T>(path, new T[] { value, });
        }

        public void AddValues<T>(String path, IEnumerable<T> values)
        {
            XPathNavigator xnav = this.RetrievePath(path);
            foreach (T value in values)
            {
                xnav.InsertElementAfter(null, path.Substring(path.LastIndexOf('/') + 1), null, null);
                xnav.SetTypedValue(value);
                xnav.MoveToNext(XPathNodeType.Element);
            }
            xnav.DeleteSelf();
        }

        public void SetValue<T>(String path, T value)
        {
            this.DeleteValues(path, true);
            this.AddValue<T>(path, value);
        }

        public void SetValues<T>(String path, IEnumerable<T> values)
        {
            this.DeleteValues(path, true);
            this.AddValues<T>(path, values);
        }

        public void DeleteValue(String path, Boolean deleteOnlyInTail)
        {
            XPathNavigator xnav = this.RetrievePath(path, false);
            if (xnav != null && !xnav.HasChildren || !deleteOnlyInTail)
            {
                xnav.DeleteSelf();
            }
        }

        public void DeleteValues(String path, Boolean deleteOnlyInTail)
        {
            XPathNodeIterator xiter = this.RootElement.CreateNavigator().Select("/configuration/" + path);
            foreach (XPathNavigator xnav in xiter.Cast<XPathNavigator>())
            {
                xnav.DeleteSelf();
            }
        }
    }
}