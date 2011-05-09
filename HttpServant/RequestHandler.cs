// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * HttpServant
 *   MetaTweet Servant which provides HTTP service
 *   Part of MetaTweet
 * Copyright © 2008-2011 Takeshi KIRIYA (aka takeshik) <takeshik@users.sf.net>
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
using System.Collections;
using System.Collections.Generic;
using System.Concurrency;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Linq;
using System.Reflection;
using System.Text;
using Achiral;
using Achiral.Extension;
using HttpServer;
using HttpServer.Headers;
using HttpServer.Messages;
using XSpect.Codecs;
using XSpect.Extension;
using XSpect.MetaTweet.Requesting;
namespace XSpect.MetaTweet.Modules
{
    public class RequestHandler
        : HttpServer.Modules.IModule
    {
        private static readonly ResponseWriter _generator = new ResponseWriter();

        public HttpServant Servant
        {
            get;
            private set;
        }

        public RequestHandler(HttpServant parent)
        {
            this.Servant = parent;
        }

        public ProcessingResult Process(RequestContext context)
        {
            if (context.Request.Uri.PathAndQuery.StartsWith("/!") || context.Request.Uri.PathAndQuery.StartsWith("/$"))
            {
                IObservable<Object> source = this.Servant.Host.RequestManager
                    .Execute(Request.Parse(context.Request.Uri.PathAndQuery.UriDecode()))
                    .If(
                        o => o.GetType().GetInterface("System.IObservable`1") != null,
                        o => (IObservable<Object>) typeof(RequestHandler)
                            .GetMethod("AsObject", BindingFlags.NonPublic | BindingFlags.Static)
                            .MakeGenericMethod(o.GetType().GetInterface("System.IObservable`1").GetGenericArguments())
                            .Invoke(null, Make.Array(o)),
                        o => o is IEnumerable && !(o is String)
                            ? ((IEnumerable) o).Cast<Object>().ToObservable()
                            : Observable.Return(o)
                    );
                context.Response.Status = HttpStatusCode.OK;
                context.Response.Reason = "OK";
                context.Response.ContentLength.Name = "X-Dummy";
                context.Response.Add(new StringHeader("Transfer-Encoding", "chunked"));
                Boolean headersSent = false;
                source
                    .Do(o => context.Response.Body.Flush())
                    .Catch((IOException ex) => Observable.Empty<Object>())
                    .Do(o =>
                    {
                        if (o is String)
                        {
                            String data = (String) o;
                            if (!headersSent)
                            {
                                context.Response.ContentType.Value = InferContentType(data);
                                _generator.SendHeaders(context.HttpContext, context.Response);
                                headersSent = true;
                            }
                            SendChunk(context, Encoding.UTF8.GetBytes(data));
                        }
                        else if (o is Byte[])
                        {
                            Byte[] data = (Byte[]) o;
                            if (!headersSent)
                            {
                                context.Response.ContentType.Value = InferContentType(data);
                                _generator.SendHeaders(context.HttpContext, context.Response);
                                headersSent = true;
                            }
                            SendChunk(context, data);
                        }
                    },
                    ex =>
                    {
                        if (!(ex is IOException))
                        {
                            throw ex;
                        }
                    },
                    () => SendChunk(context, new Byte[0]))
                    .Run();
                return ProcessingResult.Abort;
            }
            else
            {
                return ProcessingResult.Continue;
            }
        }

        private static IObservable<Object> AsObject<TSource>(IObservable<TSource> source)
        {
            return source.Select(_ => (Object) _);
        }

        private static void SendChunk(RequestContext context, Byte[] data)
        {
            context.HttpContext.Stream.Write(
                Encoding.ASCII.GetBytes(data.Length.ToString("x") + "\r\n")
                    .Concat(data)
                    .Concat(new Byte[] { 13, 10, })
                    .ToArray()
            );
        }

        private static String InferContentType(String str)
        {
            return (str.StartsWith("<")
                ? "application/xml"
                : str.StartsWith("{") || str.StartsWith("[")
                      ? "application/json"
                      : "text/plain"
            ) + "; charset=utf-8";
        }

        private static String InferContentType(Byte[] data)
        {
            return new MemoryStream(data).Dispose(s => Bitmap.FromStream(s).Dispose(i => i.RawFormat.Guid))
                .Let(g => ImageCodecInfo.GetImageDecoders().FirstOrDefault(c => c.FormatID == g).MimeType)
                ?? "application/octet-stream";
        }
    }
}