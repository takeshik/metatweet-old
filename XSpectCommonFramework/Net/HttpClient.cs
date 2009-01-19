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
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Reflection;
using System.Text;
using System.Threading;

namespace XSpect.Net
{
    public partial class HttpClient
        : Object
    {
        private WebHeaderCollection _additionalHeaders = new WebHeaderCollection();

        private CookieContainer _cookies = new CookieContainer();

        private NetworkCredential _credential = new NetworkCredential();

        private WebProxy _proxy = new WebProxy();

        private String _referer;

        private Int32 _timeout = System.Threading.Timeout.Infinite;
        
        private readonly String _userAgent;

        private Boolean _sendAdditionalHeaders = true;

        private Boolean _sendCookies = true;

        private Boolean _sendCredential = true;

        private Boolean _sendWithProxy = true;

        private Boolean _sendReferer = true;

        private Boolean _setRefererAutomatically = true;

        public WebHeaderCollection AdditionalHeaders
        {
            get
            {
                return this.SendAdditionalHeaders ? this._additionalHeaders : null;
            }
            set
            {
                this._additionalHeaders = value;
            }
        }

        public CookieContainer Cookies
        {
            get
            {
                return SendCookies ? this._cookies : null;
            }
            set
            {
                this._cookies = value;
            }
        }

        public NetworkCredential Credential
        {
            get
            {
                return SendCredential ? this._credential : null;
            }
            set
            {
                this._credential = value;
            }
        }

        public WebProxy Proxy
        {
            get
            {
                return SendWithProxy ? this._proxy : null;
            }
            set
            {
                this._proxy = value;
            }
        }

        public String Referer
        {
            get
            {
                return this.SendReferer ? this._referer : null;
            }
            set
            {
                this._referer = value;
            }
        }

        public Int32 Timeout
        {
            get
            {
                return this._timeout;
            }
            set
            {
                this._timeout = value;
            }
        }

        public String UserAgent
        {
            get
            {
                return this._userAgent;
            }
        }

        public Boolean SendAdditionalHeaders
        {
            get
            {
                return this._sendAdditionalHeaders;
            }
            set
            {
                this._sendAdditionalHeaders = value;
            }
        }

        public Boolean SendCookies
        {
            get
            {
                return this._sendCookies;
            }
            set
            {
                this._sendCookies = value;
            }
        }

        public Boolean SendCredential
        {
            get
            {
                return this._sendCredential;
            }
            set
            {
                this._sendCredential = value;
            }
        }

        public Boolean SendWithProxy
        {
            get
            {
                return this._sendWithProxy;
            }
            set
            {
                this._sendWithProxy = value;
            }
        }

        public Boolean SendReferer
        {
            get
            {
                return this._sendReferer;
            }
            set
            {
                this._sendReferer = value;
            }
        }

        public Boolean SetRefererAutomatically
        {
            get
            {
                return this._setRefererAutomatically;
            }
            set
            {
                this._setRefererAutomatically = value;
            }
        }

        public HttpClient(String additionalUserAgentString)
        {
            this._userAgent = String.Format(
                "XCF-HttpClient/{0} ({1}; U; {2}; .NET CLR {3})",
                Assembly.GetExecutingAssembly().GetName().Version.ToString(2),
                Environment.OSVersion.Platform == PlatformID.Win32NT
                    ? "Windows NT " + Environment.OSVersion.Version.ToString(2)
                    : Environment.OSVersion.VersionString,
                Thread.CurrentThread.CurrentCulture.Name,
                Environment.Version.ToString()
            );

            if (additionalUserAgentString != null)
            {
                this._userAgent += " " + additionalUserAgentString;
            }
        }

        private HttpWebRequest CreateRequest(Uri uri)
        {
            HttpWebRequest request = HttpWebRequest.Create(uri) as HttpWebRequest;
            request.Accept = @"text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            request.AllowAutoRedirect = false;
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
            request.CookieContainer = this.Cookies;
            request.Credentials = this.Credential;
            request.KeepAlive = false;
            request.Pipelined = true;
            request.Proxy = this.Proxy;
            request.Referer = this.Referer;
            request.Timeout = this.Timeout;
            request.UserAgent = this.UserAgent;
            return request;
        }

        private T ProcessResponse<T>(HttpWebResponse response, Func<Stream, T> processor)
        {
            foreach (Cookie cookie in response.Cookies)
            {
                cookie.Domain = cookie.Domain.StartsWith(".") ? cookie.Domain.Substring(1) : cookie.Domain;
                this._cookies.Add(cookie);
            }

            if (this.SetRefererAutomatically)
            {
                this.Referer = response.ResponseUri.OriginalString;
            }

            using (Stream responseStream = response.GetResponseStream())
            {
                return processor(responseStream);
            }
        }

        public T Get<T>(Uri uri, Func<Stream, T> processor)
        {
            HttpWebRequest request = this.CreateRequest(uri);
            return this.ProcessResponse(request.GetResponse() as HttpWebResponse, processor);
        }

        public Byte[] Get(Uri uri)
        {
            return this.Get<Byte[]>(uri, response =>
            {
                Byte[] buf = new Byte[response.Length];
                response.Read(buf, 0, buf.Length);
                return buf;
            });
        }

        public String Get(Uri uri, Encoding encoding)
        {
            return encoding.GetString(this.Get(uri));
        }

        public T Post<T>(Uri uri, Byte[] data, Func<Stream, T> processor)
        {
            HttpWebRequest request = this.CreateRequest(uri);
            request.ContentLength = data.Length;
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";

            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            return this.ProcessResponse(request.GetResponse() as HttpWebResponse, processor);
        }

        public Byte[] Post(Uri uri, Byte[] data)
        {
            return this.Post<Byte[]>(uri, data, response =>
            {
                Byte[] buf = new Byte[response.Length];
                response.Read(buf, 0, buf.Length);
                return buf;
            });
        }

        public String Post(Uri uri, Byte[] data, Encoding encoding)
        {
            return encoding.GetString(this.Post(uri, data));
        }
    }
}
