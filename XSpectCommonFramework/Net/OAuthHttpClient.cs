// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id: HttpClient.cs 358 2009-08-20 17:34:19Z takeshik $
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
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Achiral;
using Achiral.Extension;
using XSpect.Extension;
using System.Collections.Generic;

namespace XSpect.Net
{
    public class OAuthHttpClient
        : HttpClient,
          IDisposable
    {
        private readonly HMACSHA1 _hmac;

        private readonly MTRandom _random;

        public String ConsumerKey
        {
            get;
            set;
        }

        public String ConsumerSecret
        {
            get;
            set;
        }

        public String TokenSecret
        {
            get;
            set;
        }

        public Uri GetRequestTokenUri
        {
            get;
            set;
        }

        public OAuthHttpClient()
        {
            this._random = new MTRandom((UInt32) unchecked (DateTime.UtcNow.Ticks + Environment.TickCount));
            this._hmac = new HMACSHA1(); 
        }

        protected virtual void Dispose(Boolean disposing)
        {
            this._hmac.Clear();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private static String EncodeStrings(params String[] strings)
        {
            return strings.Select(s => UriCodec.Encode(s)).Join("&");
        }

        private String GenerateNonce()
        {
            return new String(this._random.NextChars((Char) 0x21, (Char) 0x7e).Take(32).ToArray());
        }

        private String CreateSignature(
            String tokenSecret,
            String requestMethod,
            Uri requestUri,
            IDictionary<String, String> requestArguments
        )
        {
            this._hmac.Key = Encoding.ASCII.GetBytes(EncodeStrings(this.ConsumerSecret, tokenSecret));
            return Convert.ToBase64String(this._hmac.ComputeHash(Encoding.ASCII.GetBytes(EncodeStrings(
                requestMethod,
                requestUri.ToString(),
                requestArguments.ToUriQuery()
            ))));
        }

        public String GetRequestToken()
        {
            throw new NotImplementedException();
        }
    }
}
