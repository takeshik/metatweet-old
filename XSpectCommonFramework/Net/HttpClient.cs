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
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using Achiral.Extension;
using XSpect.Extension;
using System.Collections.Generic;

namespace XSpect.Net
{
    public class HttpClient
        : Object
    {
        private static readonly Func<HttpWebResponse, Byte[]> _byteArrayConverter
            = res => res.GetResponseStream().Dispose(s => s.ReadAll());

        private static readonly Func<HttpWebResponse, Encoding, String> _stringConverterBase
            = (res, enc) => enc.GetString(_byteArrayConverter(res));

        public Action<HttpWebRequest> RequestInitializer
        {
            get;
            set;
        }

        public Action<HttpWebResponse> ResponseHandler
        {
            get;
            set;
        }

        public CookieContainer Cookies
        {
            get;
            set;
        }

        public ICredentials Credentials
        {
            get;
            set;
        }

        public IWebProxy Proxy
        {
            get;
            set;
        }

        public HttpClient()
        {
            this.RequestInitializer += req =>
            {
                req.Accept = @"text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                req.AllowAutoRedirect = false;
                req.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                req.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
                req.CookieContainer = this.Cookies;
                req.Credentials = this.Credentials;
                req.KeepAlive = true;
                req.Pipelined = true;
                req.Proxy = this.Proxy;
            };
            this.ResponseHandler += res => res.Cookies
                .OfType<Cookie>()
                .ForEach(c =>
                {
                    c.Domain = c.Domain.StartsWith(".") ? c.Domain.Substring(1) : c.Domain;
                    this.Cookies.Add(c);
                });
        }

        public HttpClient(String userAgent)
            : this()
        {
            this.RequestInitializer += req => req.UserAgent = userAgent;
        }

        protected virtual HttpWebRequest CreateRequest(Uri uri, String method)
        {
            HttpWebRequest request = HttpWebRequest.Create(uri) as HttpWebRequest;
            this.RequestInitializer(request);
            request.Method = method;
            return request;
        }

        public virtual T Get<T>(Uri uri, Func<HttpWebResponse, T> converter)
        {
            return converter(this.CreateRequest(uri, "GET").GetResponse() as HttpWebResponse);
        }

        public Byte[] Get(Uri uri)
        {
            return this.Get(uri, _byteArrayConverter);
        }

        public String Get(Uri uri, Encoding encoding)
        {
            return this.Get(uri, _stringConverterBase.Bind2nd(encoding));
        }

        public virtual T Post<T>(Uri uri, Byte[] data, Func<HttpWebResponse, T> converter)
        {
            return converter(this.CreateRequest(uri, "POST")
                .Do(r => r.GetRequestStream().Dispose(s => s.Write(data, 0, data.Length)))
                .GetResponse()
            as HttpWebResponse);
        }

        public Byte[] Post(Uri uri, Byte[] data)
        {
            return this.Post(uri, data, _byteArrayConverter);
        }

        public String Post(Uri uri, Byte[] data, Encoding encoding)
        {
            return this.Post(uri, data, _stringConverterBase.Bind2nd(encoding));
        }

        public virtual T Put<T>(Uri uri, Func<HttpWebResponse, T> converter)
        {
            return converter(this.CreateRequest(uri, "PUT").GetResponse() as HttpWebResponse);
        }

        public Byte[] Put(Uri uri)
        {
            return this.Put(uri, _byteArrayConverter);
        }

        public String Put(Uri uri, Encoding encoding)
        {
            return this.Put(uri, _stringConverterBase.Bind2nd(encoding));
        }

        public virtual T Delete<T>(Uri uri, Func<HttpWebResponse, T> converter)
        {
            return converter(this.CreateRequest(uri, "DELETE").GetResponse() as HttpWebResponse);
        }

        public Byte[] Delete(Uri uri)
        {
            return this.Delete(uri, _byteArrayConverter);
        }

        public String Delete(Uri uri, Encoding encoding)
        {
            return this.Delete(uri, _stringConverterBase.Bind2nd(encoding));
        }
    }
}
