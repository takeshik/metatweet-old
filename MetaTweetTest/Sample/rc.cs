using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XSpect.MetaTweet.Modules;
using XSpect.Configuration;

namespace XSpect.MetaTweet.Test.Sample
{
    public class Rc
        : Object
    {
        public static void Initialize(ServerCore host, IDictionary<String, String> args)
        {
            host.ModuleManager.Load("SQLiteStorage");
            host.ModuleManager.Add("SQLiteStorage", "main", "XSpect.MetaTweet.SQLiteStorage");
            var storage = host.ModuleManager.GetModule<StorageModule>("sqlite");

            host.ModuleManager.Load("TwitterApiFlow");
            host.ModuleManager.Add("TwitterApiFlow", "twitter", "XSpect.MetaTweet.TwitterApiInput");
            var input = host.ModuleManager.GetModule<InputFlowModule>("twitter");
            input.Realm = "com.twitter";

            host.ModuleManager.Add("TwitterApiFlow", "twitter", "XSpect.MetaTweet.TwitterApiOutput");
            var output = host.ModuleManager.GetModule<OutputFlowModule>("twitter");

            host.ModuleManager.Load("RemotingServant");
            host.ModuleManager.Add("RemotingServant", "remoting", "XSpect.MetaTweet.RemotingServant");
            var servant = host.ModuleManager.GetModule<ServantModule>("remoting");
        }
    }
}
