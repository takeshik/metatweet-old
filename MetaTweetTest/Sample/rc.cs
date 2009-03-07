using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XSpect.MetaTweet.Modules;
using XSpect.Configuration;
using XSpect.MetaTweet.Properties;

namespace XSpect.MetaTweet.Test.Sample
{
    public class Rc
        : Object
    {
        private static ServerCore _host;

        public static void Initialize(ServerCore host, IDictionary<String, String> args)
        {
            _host = host;
            RegisterHooks();

            host.ModuleManager.Load("SQLiteStorage");
            host.ModuleManager.Add("SQLiteStorage", "main", "XSpect.MetaTweet.SQLiteStorage");

            host.ModuleManager.Load("TwitterApiFlow");
            host.ModuleManager.Add("TwitterApiFlow", "twitter", "XSpect.MetaTweet.TwitterApiInput");
            host.ModuleManager.Add("TwitterApiFlow", "twitter", "XSpect.MetaTweet.TwitterApiOutput");

            host.ModuleManager.Load("RemotingServant");
            host.ModuleManager.Add("RemotingServant", "remoting", "XSpect.MetaTweet.RemotingTcpServant");
            
            host.ModuleManager.Load("LocalServant");
            host.ModuleManager.Add("LocalServant", "local", "XSpect.MetaTweet.LocalServant");
        }

        private static void RegisterHooks()
        {
            _host.ModuleManager.LoadHook.After.Add((self, domain) =>
                self.Parent.Log.InfoFormat(Resources.ModuleLoaded, domain)
            );
            _host.ModuleManager.UnloadHook.After.Add((self, domain) =>
                self.Parent.Log.InfoFormat(Resources.ModuleUnloaded, domain)
            );
            _host.ModuleManager.ExecuteHook.Before.Add((self, domain, file) =>
                self.Parent.Log.InfoFormat(Resources.CodeExecuting, domain, file)
            );
            _host.ModuleManager.ExecuteHook.After.Add((self, domain, file) =>
                self.Parent.Log.InfoFormat(Resources.CodeExecuted, domain, file)
            );
            _host.ModuleManager.AddHook.After.Add((self, domain, key, typeName, configFile) =>
                self.Parent.Log.InfoFormat(Resources.ModuleAdded, domain, key, typeName, configFile.Name)
            );
            _host.ModuleManager.RemoveHook.After.Add((self, domain, type, key) =>
                self.Parent.Log.InfoFormat(Resources.ModuleRemoved, domain, type.FullName, key)
            );
        }
    }
}
