// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * TwitterApiFlow
 *   MetaTweet Input/Output modules which provides Twitter access with API
 *   Part of MetaTweet
 * Copyright © 2008-2011 Takeshi KIRIYA (aka takeshik) <takeshik@users.sf.net>
 * All rights reserved.
 * 
 * This file is part of TwitterApiFlow.
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
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using LinqToTwitter;
using XSpect.MetaTweet.Objects;

namespace XSpect.MetaTweet.Modules
{
    [CLSCompliant(false)]
    public class Authorizer
        : OAuthAuthorizer,
          ITwitterAuthorizer
    {
        private static readonly Byte[] _entropy = Convert.FromBase64String(@"
mKba/BXCh/qRuaeQQ+ZiTSFozBy6qyjFvzwGRol6JvUG7lcDTsGDwJtMLK9DKz4jdangqZonXg1A
d9E9axvoRR0E+Bba1zQ7T9YkkHqQd7Ve+OesdV9DUuhVxAiSWDGd6dinIg1GW1P2XRzLO0JJjxta
kby+FOVDQZUKCouAl1cFHF29IRpkBex00PkGMFq5+UDtb/59Lrxt8yyqKO9XS6Gj1Z55gnxakLAQ
TcaHmACHgA9u5DitsJD24ZO4Sw/zU5Z6k+yrCHON1yAxtrSsr2E9zXsKFgFrGxBhSQpUChYBQhwI
NAoWAUUdWesXEyMZBQIPAREKBxQCHAOXB9PEGwEDsFVAIWx40KsXl3pVDVnfxnG+o6AeAbq5qe3J
l2cKFgFrGxBhSQpUChYBQhwVDhsCOAMGWh1Z6BcTIBkFAg8BEQoHFAIcA5cH08YKGeedZ8I9nzPp
rC1gf68VfhOesVB737DyeGkaBYo1DhtJAhsYCApBFQ4bAglIChYBNBsQIQkCRgJLC0RO6BcTIBkF
Ag8BEQoHFAIcA5cH0+wKGeedZ8KOoEv2Ros0t0CXkLeeyu/hhewIuvjHEIE1DhtJAhsYCApBFQ4b
AglIChYBKwYXXRVAEbZmAgInSVhWGA0BBAQdCwESAQMfFYADsFVAIagi8n/b+bkmljeCryOLwmHn
2W1ClIWhHmMKFgFrGxBhSQpUChYBQhwVDhsCLQVKDEALRE7qFxMiB2UOGQUCDwETCgMPFAIcA5cK
GeedZ8JOn5Y+L6RnwSB6ZxWfVC+NiaLNqij8+L0J2vwArmxlAvZ28awChgNdXdjmvJ2ipmGFZFVE
rsqlXrVYoQ/os+d+caFlnuz1M1FAbdKbyN75UjYZdT6w5LVhOMvd38NlaO3c7A4CQaiQD4j5gzd2
MOG0ywQGSLEFXrscYObzj1mjeqiBn2EO8EHIpfsAz4IY4uRHbJhiHZQQpILFQ1NlG+L2i2U/0Nh6
fFVz5UHmanJZZH/lTNIElAOrMA/EppV6zFyr/h55D+LV98FOlIaSV4JObwLWzg3vbq6/3dY2xOLl
LuHNCiNfHuTQ4Td80Pm1kFbPX7BB1SV7M+mKJ7rQ4wUmAQCKzSnatXTgUE7TWe5i+T34ljbCqWGV
iTBA/DcG8B7v+4u33b+cab0lMTANK0C0+YQsQfY1Eb5sMd0V45K+YunTmVKOwLjpfFS5nLX69J/c
GOFDHMb6+Fy+/E/zsNmuKAmSAqsnSmYrwLWWJCOtzyqsF6UmMoa6jOmCCAGFYJwN8ijvgtwMu4H3
degM0h2rt9q2uhtiHIqqmXtFreb2Se09a1/DMSQCLsinJeHPLAlReRNedEf7+QdFOrW1yw8efQ=="
        );

        public AccountId AccountId
        {
            get;
            protected set;
        }

        public Func<Uri, String> GetPin
        {
            get;
            set;
        }

        private readonly FileInfo _tokenFile;

        public Authorizer(FileInfo tokenFile)
        {
            this._tokenFile = tokenFile;
            this.Credentials = new InMemoryCredentials()
            {
                ConsumerKey = "yR1QZk9UQSxuMEpaYLclNw",
                ConsumerSecret = "tcg66ewkX96Kk9m6MQam2GWhXBqpY74UJpqIfqqCA",
            };
            this.OAuthTwitter = new OAuthTwitter()
            {
                OAuthConsumerKey = this.Credentials.ConsumerKey,
                OAuthConsumerSecret = this.Credentials.ConsumerSecret,
                OAuthUserAgent = "MetaTweet Twitter API modules",
            };
        }

        public void Authorize()
        {
            if (!this.IsAuthorized)
            {
                this.Load();
                if (!this.IsAuthorized)
                {
                    String screenName;
                    String userID;
                    if (this.GetPin == null)
                    {
                        throw new InvalidOperationException("GetPin must have a handler before calling Authorize.");
                    }
                    String link = this.OAuthTwitter.AuthorizationLinkGet(this.OAuthRequestTokenUrl, this.OAuthAuthorizeUrl, "oob", false);
                    String verifier = this.GetPin(new Uri(link));
                    String oAuthToken = new Uri(link).Query
                        .TrimStart('?')
                        .Split('&')
                        .Select(_ => _.Split('='))
                        .SingleOrDefault(_ => _[0] == "oauth_token")[1];
                    this.OAuthTwitter.AccessTokenGet(oAuthToken, verifier, this.OAuthAccessTokenUrl, "", out screenName, out userID);
                    this.ScreenName = screenName;
                    this.UserId = userID;
                    this.Credentials.OAuthToken = this.OAuthTwitter.OAuthToken;
                    this.Credentials.AccessToken = this.OAuthTwitter.OAuthTokenSecret;
                    this.AccountId = AccountId.Create("com.twitter", Create.Table("Id", userID));
                    this.Save();
                }
            }
        }

        public void Load()
        {
            if (this._tokenFile.Exists)
            {
                try
                {
                    String[] data = Encoding.UTF32.GetString(ProtectedData.Unprotect(
                        File.ReadAllBytes(this._tokenFile.FullName),
                        _entropy,
                        DataProtectionScope.LocalMachine
                    )).Split(new String[] { "\0\0\0\0" }, 5, StringSplitOptions.None);
                    this.Credentials.OAuthToken = data[0];
                    this.Credentials.AccessToken = data[1];
                    this.UserId = data[2];
                    this.ScreenName = data[3];
                    this.AccountId = data[4];
                }
                catch (Exception)
                {
                    this.Credentials.OAuthToken = null;
                    this.Credentials.AccessToken = null;
                    this.UserId = null;
                    this.ScreenName = null;
                    this.AccountId = null;
                }
                finally
                {
                    this.OAuthTwitter.OAuthToken = this.Credentials.OAuthToken;
                    this.OAuthTwitter.OAuthTokenSecret = this.Credentials.AccessToken;
                }
            }
        }

        public void Load(String credentialsString)
        {
            String[] credentials = credentialsString.Split(',');
            this.Credentials.OAuthToken = credentials[0];
            this.Credentials.AccessToken = credentials[1];
        }

        public void Save()
        {
            if (this.IsAuthorized)
            {
                File.WriteAllBytes(
                    this._tokenFile.FullName,
                    ProtectedData.Protect(
                        Encoding.UTF32.GetBytes(
                            String.Join("\0\0\0\0", new String[]
                            {
                                this.Credentials.OAuthToken,
                                this.Credentials.AccessToken,
                                this.UserId,
                                this.ScreenName,
                                this.AccountId,
                            })
                        ),
                        _entropy,
                        DataProtectionScope.LocalMachine
                    )
                );
            }
        }
    }
}