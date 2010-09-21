using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using DotNetOpenAuth.OAuth.ChannelElements;
using DotNetOpenAuth.OAuth.Messages;

namespace XSpect.MetaTweet.Modules
{
    internal class TokenManager : IConsumerTokenManager
    {
        private readonly Dictionary<String, String> _tokenSecrets;

        private readonly FileInfo _tokenFile;

        public String ConsumerKey
        {
            get
            {
                return "yR1QZk9UQSxuMEpaYLclNw";
            }
        }

        public String ConsumerSecret
        {
            get
            {
                return "tcg66ewkX96Kk9m6MQam2GWhXBqpY74UJpqIfqqCA";
            }
        }

        public String AccessToken
        {
            get;
            private set;
        }

        public TokenManager(FileInfo tokenFile)
        {
            this._tokenSecrets = new Dictionary<String, String>();
            this._tokenFile = tokenFile;
            if (tokenFile.Exists)
            {
                this.Load();
            }
        }

        public String GetTokenSecret(String token)
        {
            return this._tokenSecrets[token];
        }

        public void StoreNewRequestToken(UnauthorizedTokenRequest request, ITokenSecretContainingMessage response)
        {
            this._tokenSecrets[response.Token] = response.TokenSecret;
        }

        public void ExpireRequestTokenAndStoreNewAccessToken(String consumerKey, String requestToken, String accessToken, String accessTokenSecret)
        {
            this._tokenSecrets.Remove(requestToken);
            this.AccessToken = accessToken;
            this._tokenSecrets[accessToken] = accessTokenSecret;
            this.Save();
        }

        public TokenType GetTokenType(String token)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            File.WriteAllBytes(this._tokenFile.FullName, ProtectedData.Protect(
                Encoding.BigEndianUnicode.GetBytes(this.AccessToken + "\0" + this._tokenSecrets[this.AccessToken]),
                Encoding.BigEndianUnicode.GetBytes("MetaTweet"),
                DataProtectionScope.CurrentUser
            ));
        }

        public void Load()
        {
            String[] data = Encoding.BigEndianUnicode.GetString(ProtectedData.Unprotect(
                File.ReadAllBytes(this._tokenFile.FullName),
                Encoding.BigEndianUnicode.GetBytes("MetaTweet"),
                DataProtectionScope.CurrentUser
            )).Split('\0');
            this.AccessToken = data[0];
            this._tokenSecrets[this.AccessToken] = data[1];
        }

        public void Clear()
        {
            this._tokenSecrets.Clear();
            this.AccessToken = null;
        }
    }
}