using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Net.Cache;
using System.IO;

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

        private Int32 _timeout;
        
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

        private T ProcessResponse<T>(HttpWebResponse response, StreamProcess<T> processor)
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

        public T Get<T>(Uri uri, StreamProcess<T> processor)
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

        public T Post<T>(Uri uri, Byte[] data, StreamProcess<T> processor)
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
