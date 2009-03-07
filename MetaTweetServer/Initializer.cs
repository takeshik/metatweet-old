using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XSpect.MetaTweet.Modules;
using XSpect.Configuration;
using XSpect.MetaTweet.Properties;
using System.Reflection;
using XSpect.Extension;

namespace XSpect.MetaTweet
{
    public class Initializer
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
            _host.ModuleManager.AddHook.After.Add((self, domain, key, typeName, configFile) =>
                RegisterModuleHook(self.GetModules(domain, key).Single(m => m.GetType().FullName == typeName))
            );
            _host.ModuleManager.RemoveHook.After.Add((self, domain, type, key) =>
                self.Parent.Log.InfoFormat(Resources.ModuleRemoved, domain, type.FullName, key)
            );
        }

        private static void RegisterModuleHook(IModule module)
        {
            if (module is InputFlowModule)
            {
                InputFlowModule input = module as InputFlowModule;
                input.InputHook.Before.Add((self, selector, storage, args) =>
                    self.Host.Log.InfoFormat(
                        Resources.InputFlowInputStarted,
                        self.Name,
                        selector,
                        storage is StorageModule
                            ? (storage as StorageModule).Name
                            : String.Format("({0})", storage.GetType().FullName),
                        args.Inspect().ForEachLine(l => new String(' ', 4) + l)
                    )
                );
                input.InputHook.After.Add((self, selector, storage, args) =>
                    self.Host.Log.InfoFormat(Resources.InputFlowInputFinished, self.Name)
                );
            }
            else if (module is FilterFlowModule)
            {
                FilterFlowModule filter = module as FilterFlowModule;
                filter.FilterHook.Before.Add((self, selector, source, storage, args) =>
                    self.Host.Log.InfoFormat(
                        Resources.FilterFlowFilterStarted,
                        self.Name,
                        selector,
                        source.Count(),
                        storage is StorageModule
                            ? (storage as StorageModule).Name
                            : String.Format("({0})", storage.GetType().FullName),
                        args.Inspect().ForEachLine(l => new String(' ', 4) + l)
                    )
                );
                filter.FilterHook.After.Add((self, selector, source, storage, args) =>
                    self.Host.Log.InfoFormat(Resources.FilterFlowFilterStarted, self.Name)
                );
            }
            else if (module is OutputFlowModule)
            {
                OutputFlowModule output = module as OutputFlowModule;
                output.OutputHook.Before.Add((self, selector, source, storage, args, type) =>
                    self.Host.Log.InfoFormat(
                        Resources.OutputFlowOutputStarted,
                        self.Name,
                        selector,
                        source.Count(),
                        storage is StorageModule
                            ? (storage as StorageModule).Name
                            : String.Format("({0})", storage.GetType().FullName),
                        args.Inspect().ForEachLine(l => new String(' ', 4) + l),
                        type.FullName
                    )
                );
                output.OutputHook.After.Add((self, selector, source, storage, args, type) =>
                    self.Host.Log.InfoFormat(Resources.OutputFlowOutputFinished, self.Name)
                );
            }
            else if (module is ServantModule)
            {
                ServantModule servant = module as ServantModule;
            }
            else if (module is StorageModule)
            {
                StorageModule storage = module as StorageModule;
            }
        }
    }
}
