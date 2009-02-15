using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XSpect.MetaTweet.Test.Script
{
    public class Init
        : Object
    {
        public static void Initialize(ServerCore host, IDictionary<String, String> args)
        {
            host.ModuleManager.Execute(host.RootDirectory.GetFiles("rc.*").Single());
        }
    }
}
