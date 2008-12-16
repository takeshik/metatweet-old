using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using XSpect.MetaTweet.Properties;

namespace XSpect.MetaTweet
{
    public partial class ServerHost
        : ServiceBase
    {
		private ServerCore _server = new ServerCore();

        public ServerHost()
        {
            InitializeComponent();
        }
        
        public override EventLog EventLog
        {
            get
            {
				// Use ServerCore#Log (log4net.ILog) instead.
				return null;
            }
        }

        protected override void OnContinue()
        {
			this._server.Resume();
        }

        protected override void OnPause()
        {
			this._server.Pause();
        }

        protected override void OnStart(String[] args)
        {
			this._server.Start(null /* stub */);
        }

        protected override void OnStop()
        {
			this._server.Stop();
        }
    }
}
