// -*- mode: csharp; encoding: utf-8; -*-
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetServer
 *   Server library of MetaTweet
 *   Part of MetaTweet
 * Copyright © 2008-2009 Takeshi KIRIYA, XSpect Project <takeshik@xspect.org>
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
using System.Data;
using System.Collections.Generic;
using System.Linq;

namespace XSpect.MetaTweet.ObjectModel
{
    [Serializable()]
    public class ReplyElement
        : StorageObject<StorageDataSet.ReplyMapDataTable, StorageDataSet.ReplyMapRow>
    {
        public Post Post
        {
            get
            {
                return this.Storage.GetPost(this.UnderlyingDataRow.PostsRowParentByFK_Posts_ReplyMap);
            }
            set
            {
                this.UnderlyingDataRow.PostsRowParentByFK_Posts_ReplyMap = value.UnderlyingDataRow;
            }
        }

        public Post InReplyToPost
        {
            get
            {
                return this.Storage.GetPost(this.UnderlyingDataRow.PostsRowParentByFK_PostsInReplyTo_ReplyMap);
            }
            set
            {
                this.UnderlyingDataRow.PostsRowParentByFK_PostsInReplyTo_ReplyMap = value.UnderlyingDataRow;
            }
        }

        internal ReplyElement()
        {
        }

        public override String ToString()
        {
            return String.Format("{0} => {1}", this.Post.ToString(), this.InReplyToPost.ToString());
        }

        public override void Update()
        {
            this.Storage.Update(this.UnderlyingDataRow);
        }
    }
}