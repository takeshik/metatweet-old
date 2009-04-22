// -*- mode: csharp; encoding: utf-8; -*-
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * TwitterWebFlow
 *   MetaTweet Input/Output modules which provides Twitter access with web scraping
 *   Part of MetaTweet
 * Copyright © 2008-2009 Takeshi KIRIYA, XSpect Project <takeshik@users.sf.net>
 * All rights reserved.
 * 
 * This file is part of TwitterWebFlow.
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
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.XPath;
using TidyNet;
using XSpect.Extension;
using XSpect.MetaTweet.Modules;
using XSpect.MetaTweet.ObjectModel;
using XSpect.Net;
using Achiral.Extension;

namespace XSpect.MetaTweet
{
    public class TwitterWebInput
        : InputFlowModule
    {
        private readonly HttpClient _client;

        private readonly Tidy _tidy;

        private readonly Func<HttpWebResponse, XDocument> _processor;

        private String _authenticityToken;

        public TwitterWebInput()
        {
            // HACK: For Twitter server (see: http://muumoo.jp/news/2009/01/11/0expectationfailed.html)
            // CONSIDER: Is this change permanent?
            ServicePointManager.Expect100Continue = false;

            this._client = new HttpClient("MetaTweet TwitterWebInput/1.0");

            this._tidy = new Tidy();
            this._tidy.Options.CharEncoding = CharEncoding.UTF8;
            this._tidy.Options.DocType = DocType.Strict;
            this._tidy.Options.DropEmptyParas = true;
            this._tidy.Options.DropFontTags = true;
            this._tidy.Options.EncloseBlockText = true;
            this._tidy.Options.EncloseText = true;
            this._tidy.Options.FixBackslash = true;
            this._tidy.Options.FixComments = true;
            this._tidy.Options.LiteralAttribs = true;
            this._tidy.Options.LogicalEmphasis = true;
            this._tidy.Options.MakeClean = true;
            this._tidy.Options.NumEntities = true;
            this._tidy.Options.QuoteAmpersand = true;
            this._tidy.Options.WrapAsp = true;
            this._tidy.Options.WrapAttVals = true;
            this._tidy.Options.WrapJste = true;
            this._tidy.Options.WrapPhp = true;
            this._tidy.Options.WrapScriptlets = true;
            this._tidy.Options.WrapSection = true;
            this._tidy.Options.XmlOut = true;

            this._processor = response => response.GetResponseStream().Dispose(stream =>
                XDocument.Parse(Regex.Replace(
                    new MemoryStream().Dispose(s =>
                    {
                        this._tidy.Parse(stream, s, new TidyMessageCollection());
                        s.Seek(0, SeekOrigin.Begin);
                        return Encoding.UTF8.GetString(s.GetBuffer());
                    }),
                    " xmlns=\".+?\"|<script.*?</script>",
                    String.Empty,
                    RegexOptions.Singleline
                ).TrimEnd('\0').Replace(ReplaceTables.XhtmlEntities))
            );

            this.Realm = this.Configuration.GetValueOrDefault("realm", "com.twitter");
            this.Login();
        }

        private void Login()
        {
            NetworkCredential credential = this.Configuration.GetValueOrDefault<NetworkCredential>("credential");
            this._authenticityToken = this._client.Get(new Uri("https://twitter.com/"), this._processor)
                .XPathEvaluate<String>(this.Configuration.GetChild("scrapingKeys").GetValueOrDefault(
                    "xpath:login.authenticityToken",
                    "string(//input[@id='authenticity_token']/@value)"
                ));
            this._client.Post(new Uri("https://twitter.com/sessions/"), Encoding.UTF8.GetBytes(String.Format(
                this.Configuration.GetChild("scrapingKeys").GetValueOrDefault(
                    "format:login.sessionPost",
                    "authenticity_token={0}&session[username_or_email]={1}&session[password]={2}&remember_me=1"
                ),
                this._authenticityToken,
                credential.UserName,
                credential.Password
            )), this._processor);
        }
    }
}