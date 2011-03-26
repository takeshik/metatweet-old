// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetServer
 *   Server library of MetaTweet
 *   Part of MetaTweet
 * Copyright © 2008-2011 Takeshi KIRIYA (aka takeshik) <takeshik@users.sf.net>
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
using System.Runtime.Serialization;

namespace XSpect.MetaTweet.Requesting
{
    [Serializable()]
    public class RequestTaskException
        : Exception
    {
        public RequestTask RequestTask
        {
            get;
            private set;
        }

        public RequestTaskException(RequestTask requestTask)
            : this(requestTask, null)
        {
        }

        public RequestTaskException(RequestTask requestTask, Exception innnerException)
            : this(String.Format(
                  "RequestTask #{0} ({1}) finished unsuccessfully at position {2} / {3}.",
                  requestTask.Id,
                  requestTask.Request,
                  requestTask.CurrentPosition,
                  requestTask.RequestFragmentCount
              ), innnerException, requestTask)
        {
        }

        public RequestTaskException(String message, RequestTask requestTask)
            : this(message, null, requestTask)
        {
        }

        public RequestTaskException(String message, Exception innerException, RequestTask requestTask)
            : base(message, innerException)
        {
            this.RequestTask = requestTask;
        }

        protected RequestTaskException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.RequestTask = (RequestTask) info.GetValue("RequestTask", typeof(RequestTask));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("RequestTask", this.RequestTask);
        }
    }
}