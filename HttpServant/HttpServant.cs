// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * HttpServant
 *   MetaTweet Servant which provides HTTP service
 *   Part of MetaTweet
 * Copyright © 2008-2010 Takeshi KIRIYA (aka takeshik) <takeshik@users.sf.net>
 * All rights reserved.
 * 
 * This file is part of HttpServant.
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
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Achiral;
using Achiral.Extension;
using XSpect.Extension;
using XSpect.MetaTweet.Modules;
using System.Timers;
using XSpect.Codecs;
using XSpect.MetaTweet.Modules.Properties;
using ServerResources = XSpect.MetaTweet.Properties.Resources;

namespace XSpect.MetaTweet.Modules
{
    public class HttpServant
        : ServantModule
    {
        private static readonly ImageConverter _imageConverter = new ImageConverter();

        private readonly HttpListener _listener;

        private readonly SHA512CryptoServiceProvider _sha = new SHA512CryptoServiceProvider();

        public HttpServant()
        {
            this._listener = new HttpListener();
            this._listener.IgnoreWriteExceptions = true;
        }

        protected override void InitializeImpl()
        {
            this._listener.Prefixes.AddRange(this.Configuration.ResolveValue<List<String>>("prefixes"));
            this._listener.AuthenticationSchemes = AuthenticationSchemes.Basic;
            this._listener.Realm = "MetaTweet HTTP Service (" + this.Name + ")";
            base.InitializeImpl();
        }

        protected override void StartImpl()
        {
            this._listener.Start();
            this.Configuration.ResolveValue<Int32>("threadCount").Times(this.BeginGetContext);
        }

        protected override void StopImpl()
        {
            this._listener.Stop();
        }

        protected override void Dispose(Boolean disposing)
        {
            this._listener.Close();
            base.Dispose(disposing);
        }

        private void BeginGetContext()
        {
            this._listener.BeginGetContext(this.OnRequested, null);
        }

        private void OnRequested(IAsyncResult asyncResult)
        {
            if (this._listener.IsListening)
            {
                try
                {
                    this.HandleRequest(this._listener.EndGetContext(asyncResult));
                }
                finally
                {
                    this.BeginGetContext();
                }
            }
        }

        private void HandleRequest(HttpListenerContext context)
        {
            try
            {
                if (context.Request.Url.PathAndQuery == "/")
                {
                    this.SendResponse(context, GetContentType(
                        String.Format(
                            ServerResources.HtmlTemplate,
                            String.Format(
                                Resources.IndexPage,
                                context.Request.Url.Host,
                                context.Request.Url.Port
                            ),
                            "MetaTweet HTTP Service",
                            ThisAssembly.EntireCommitId,
                            context.Request.Url.Host,
                            context.Request.Url.Port
                        )
                    ));
                }
                else if (context.Request.Url.PathAndQuery.StartsWith("/res/"))
                {
                    this.SendResponse(context, GetContentType(
                        Resources.ResourceManager.GetObject(
                            context.Request.Url.PathAndQuery.Substring(5).Replace('.', '_')
                        )
                    ));
                }
                else if (context.Request.Url.PathAndQuery.StartsWithAny("/$", "/!"))
                {
                    this.SendResponseIfAuthenticated(context, GetContentType(
                        RequestToServer(context.Request.Url.PathAndQuery)
                    ));
                }
                else
                {
                    this.SendResponse(context, GetContentType(
                        String.Format(
                            ServerResources.HtmlTemplate,
                            "<h1>Not Found</h1><p>The resource you requested is not found on this server.</p>",
                            "Not Found",
                            ThisAssembly.EntireCommitId,
                            context.Request.Url.Host,
                            context.Request.Url.Port
                        )
                    ));
                }
            }
            catch (Exception ex)
            {
                this.SendResponse(context, GetContentType(
                    String.Format(
                        ServerResources.HtmlTemplate,
                        String.Format(
                            "<h1>{0}</h1><p>{1}</p><pre>{2}</pre>",
                            ex.GetType().FullName,
                            ex.Message,
                            ex.StackTrace
                        ),
                        "Exception caught",
                        ThisAssembly.EntireCommitId,
                        context.Request.Url.Host,
                        context.Request.Url.Port
                    )
                ));
            }
        }

        private Object RequestToServer(String requestString)
        {
            return this.Host.Request(Request.Parse(requestString));
        }

        private Tuple<String, Byte[]> GetContentType(Object obj)
        {
            if (obj is String)
            {
                String str = obj as String;
                return Make.Tuple(
                    str.StartsWith("<?xml")
                        ? str.Contains("<html")
                              ? "application/xhtml+xml"
                              : "application/xml"
                        : "text/plain",
                    Encoding.UTF8.GetBytes(str)
                );
            }
            else if (obj is Bitmap)
            {
                return Make.Tuple(
                    ImageCodecInfo.GetImageDecoders()
                        .Single(c => c.FormatID == (obj as Bitmap).RawFormat.Guid)
                        .MimeType,
                    _imageConverter.ConvertTo(obj, typeof(Byte[])) as Byte[]
                );
            }
            else if (obj == null)
            {
                throw new NullReferenceException("Returned null reference.");
            }
            else
            {
                throw new NotSupportedException("To output " + obj.GetType().FullName + " is not supported.");
            }
        }

        private void SendResponse(HttpListenerContext context, Tuple<String, Byte[]> data, params Action<HttpListenerContext>[] additionalActions)
        {
            context.Response.ContentType = data.Item1;
            context.Response.ContentLength64 = data.Item2.LongLength;
            additionalActions.ForEach(a => a(context));
            context.Response.OutputStream.Write(data.Item2, 0, data.Item2.Length);
            context.Response.OutputStream.Flush();
            context.Response.OutputStream.Dispose();
            this.Log.DebugFormat("Request {0} from {1} completed.", context.Request.RawUrl, context.Request.RemoteEndPoint.ToString());
            context.Response.Close();
        }

        private void SendResponseIfAuthenticated(HttpListenerContext context, Tuple<String, Byte[]> data, params Action<HttpListenerContext>[] additionalActions)
        {
            HttpListenerBasicIdentity identity = context.User.Identity as HttpListenerBasicIdentity;
            if (
                context.Request.IsAuthenticated &&
                identity.Name == this.Configuration.ResolveValue<String>("authentication", "userName") &&
                new String(this._sha.ComputeHash(
                    Encoding.UTF8.GetBytes(identity.Password))
                        .SelectMany(b => b.ToString("x2").ToCharArray())
                        .ToArray()
                ) == this.Configuration.ResolveValue<String>("authentication", "password")
            )
            {
                this.SendResponse(context, data, additionalActions);
            }
            else
            {
                this.SendResponse(context,
                    GetContentType(
                        String.Format(
                            ServerResources.HtmlTemplate,
                            "<h1>Unauthorized</h1><p>The resource you requested requires authentication.</p>",
                            "Unauthorized",
                            ThisAssembly.EntireCommitId,
                            context.Request.Url.Host,
                            context.Request.Url.Port
                        )
                    ),
                    r => r.Response.StatusCode = (Int32) HttpStatusCode.Unauthorized
                );
            }
        }
    }
}
