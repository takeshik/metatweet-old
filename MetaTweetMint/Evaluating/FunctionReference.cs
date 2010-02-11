// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetMint
 *   Extensible GUI client for MetaTweet
 *   Part of MetaTweet
 * Copyright © 2009-2010 Takeshi KIRIYA (aka takeshik) <takeshik@users.sf.net>
 * All rights reserved.
 * 
 * This file is part of MetaTweetMint.
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

namespace XSpect.MetaTweet.Clients.Mint.Evaluating
{
    public class FunctionReference
        : IEvaluatable,
          IEquatable<FunctionReference>
    {
        public String FunctionName
        {
            get;
            private set;
        }

        public Boolean Equals(FunctionReference other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return Equals(other.FunctionName, this.FunctionName);
        }

        public override Boolean Equals(Object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != typeof (FunctionReference))
            {
                return false;
            }
            return Equals((FunctionReference) obj);
        }

        public override Int32 GetHashCode()
        {
            return this.FunctionName != null ? this.FunctionName.GetHashCode() : 0;
        }

        public static Boolean operator ==(FunctionReference left, FunctionReference right)
        {
            return Equals(left, right);
        }

        public static Boolean operator !=(FunctionReference left, FunctionReference right)
        {
            return !Equals(left, right);
        }

        public FunctionReference(String name)
        {
            this.FunctionName = name;
        }

        public Object Evaluate(ClientCore host, IDictionary<String, String> args)
        {
            return host.Functions[this.FunctionName].Evaluate(host, args);
        }
    }
}