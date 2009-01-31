using System;
using System.Threading;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting;

namespace XSpect.MetaTweet.Clients
{
    public class MetaTweetClient
        : Object
    {
        private TcpClientChannel _channel = new TcpClientChannel();
        ServerCore _host;

        public ServerCore Host
        {
            get
            {
                return this._host;
            }
        }
        
        public void Connect()
        {
            ChannelServices.RegisterChannel(this._channel, false);
            RemotingConfiguration.RegisterWellKnownClientType(typeof(ServerCore), "tcp://localhost:7784/MetaTweet");
            this._host = new ServerCore();
        }

        public void Disconnect()
        {
            this._host = null;
            ChannelServices.UnregisterChannel(this._channel);
        }
    }
}