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
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;
using Achiral;
using Achiral.Extension;
using Microsoft.Scripting.Runtime;
using XSpect;
using XSpect.Extension;

namespace XSpect.Reflection
{
    partial class CodeManager
    {
        [XmlRoot("language")]
        public sealed class LanguageSetting
            : IXmlSerializable
        {
            public String Name
            {
                get;
                set;
            }

            public ICollection<String> Identifiers
            {
                get;
                set;
            }

            public ICollection<String> Extensions
            {
                get;
                set;
            }

            public Type Type
            {
                get;
                set;
            }

            public IDictionary<String, String> Options
            {
                get;
                set;
            }

            public Boolean IsDynamicLanguage
            {
                get
                {
                    return this.Type.IsSubclassOf(typeof(LanguageContext));
                }
            }

            public LanguageSetting()
            {
                this.Identifiers = new List<String>();
                this.Extensions = new List<String>();
                this.Options = new Dictionary<String, String>();
            }

            #region Implementation of IXmlSerializable
            public XmlSchema GetSchema()
            {
                return null;
            }

            public void ReadXml(XmlReader reader)
            {
                XElement xlanguage = XDocument.Load(reader).Element("compiler");
                this.Name = xlanguage.Element("name").Value;
                this.Type = Type.GetType(xlanguage.Element("type").Value);
                this.Identifiers.AddRange(xlanguage
                    .Element("ids")
                    .Elements("id")
                    .Cast<String>()
                );
                this.Extensions.AddRange(xlanguage
                    .Element("extensions")
                    .Elements("extension")
                    .Cast<String>()
                );
                this.Options.AddRange(xlanguage
                    .Element("options")
                    .Elements("option")
                    .Select(xe => Create.KeyValuePair(xe.Attribute("key").Value, xe.Value))
                );
            }

            public void WriteXml(XmlWriter writer)
            {
                Make.Array(
                    new XElement("name", this.Name),
                    new XElement("type", this.Type.AssemblyQualifiedName),
                    new XElement("ids",
                        this.Identifiers.Select(l => new XElement("id", l))
                    ),
                    new XElement("extensions",
                        this.Extensions.Select(e => new XElement("extension", e))
                    ),
                    new XElement("options",
                        this.Options.Select(o => new XElement("option",
                            new XAttribute("key", o.Key),
                            o.Value
                        ))
                    )
                ).ForEach(xe => xe.WriteTo(writer));
            }
            #endregion
        }
    }
}