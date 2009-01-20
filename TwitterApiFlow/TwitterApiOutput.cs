// -*- mode: csharp; encoding: utf-8; -*-
/* MetaTweet
 *   Hub system for micro-blog communication services
 * TwitterApiFlow
 *   MetaTweet Input/Output modules which provides Twitter access with API
 *   Part of MetaTweet
 * Copyright © 2008-2009 Takeshi KIRIYA, XSpect Project <takeshik@xspect.org>
 * All rights reserved.
 * 
 * This file is part of TwitterApiFlow.
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
using System.Xml;
using XSpect.MetaTweet.ObjectModel;

namespace XSpect.MetaTweet
{
    public class TwitterApiOutput
        : OutputFlowModule
    {
        [FlowInterface("/.xml")]
        public XmlDocument OutputXml(IEnumerable<StorageObject> source, String param, IDictionary<String, String> args)
        {
            throw new NotImplementedException();
        }

        [FlowInterface("/.rss")]
        public XmlDocument OutputRss(IEnumerable<StorageObject> source, String param, IDictionary<String, String> args)
        {
            throw new NotImplementedException();
        }

        [FlowInterface("/.atom")]
        public XmlDocument OutputAtom(IEnumerable<StorageObject> source, String param, IDictionary<String, String> args)
        {
            throw new NotImplementedException();
        }

        [FlowInterface("/.json")]
        public String OutputJson(IEnumerable<StorageObject> source, String param, IDictionary<String, String> args)
        {
            throw new NotImplementedException();
        }

    }
}