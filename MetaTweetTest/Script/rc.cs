using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XSpect.MetaTweet.Modules;
using XSpect.Configuration;

namespace XSpect.MetaTweet.Test.Script
{
    public class Rc
        : Object
    {
        public static void Initialize(ServerCore host, IDictionary<String, String> args)
        {
            host.ModuleManager.Load("SQLiteStorage");
            host.ModuleManager.Add("SQLiteStorage", "sqlite", "XSpect.MetaTweet.SQLiteStorage");
            var storage = host.ModuleManager.GetModule<StorageModule>("sqlite");
            storage.Initialize(XmlConfiguration.Load("SQLiteStorage.conf.xml"));
            // {"connection", @"data source=MetaTweet.db;binaryguid=False"}

            host.ModuleManager.Load("TwitterApiFlow");
            host.ModuleManager.Add("TwitterApiFlow", "twitter", "XSpect.MetaTweet.TwitterApiInput");
            var input = host.ModuleManager.GetModule<InputFlowModule>("twitter");
            input.Realm = "com.twitter";
            input.Initialize(XmlConfiguration.Load("TwitterApiInput.conf.xml"));
            // {"username", "YOUR-TWITTER-USERNAME"}
            //{"password", "YOUR-TWITTER-PASSWORD"}

            host.ModuleManager.Add("TwitterApiFlow", "twitter", "XSpect.MetaTweet.TwitterApiOutput");
            var output = host.ModuleManager.GetModule<OutputFlowModule>("twitter");
            output.Initialize(XmlConfiguration.Load("TwitterApiOutput.conf.xml"));

            host.ModuleManager.Load("RemotingServant");
            host.ModuleManager.Add("RemotingServant", "remoting", "XSpect.MetaTweet.RemotingServant");
            var servant = host.ModuleManager.GetModule<ServantModule>("remoting");
            servant.Initialize(XmlConfiguration.Load("RemotingServant.conf.xml"));
            // {"port", "7784"}
        }
    }
}
