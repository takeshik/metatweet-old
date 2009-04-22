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
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace XSpect.Extension
{
    public static class XNodeUtil
    {
        public static T XPathEvaluate<T>(this XNode node, String expression)
        {
            return (T) node.XPathEvaluate(expression);
        }

        public static IEnumerable<T> XPathSelect<T>(this XNode node, String expression)
            where T : XObject
        {
            return ((IEnumerable<Object>) node.XPathEvaluate(expression)).Cast<T>();
        }

        public static IEnumerable<XObject> XPathSelect(this XNode node, String expression)
        {
            return node.XPathSelect<XObject>(expression);
        }
    }
}