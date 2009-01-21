using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace XSpect.MetaTweet.Test
{
    internal class Program
    {
        private static void Main(String[] args)
        {
            Console.WriteLine("Launching debugger...");
            Debugger.Launch();
            Debugger.Break();
            var c = new ServerCore();
            var s = new SQLiteStorage();
            s.Register(c, "sqlite");
            s.Initialize(@"data source=C:\MetaTweet.db;binaryguid=False");
            s.DropTables();
            s.CreateTables();
            s.Connect();
            var i = new TwitterApiInput();
            i.Register(c, "twitter");
            i.Realm = "com.twitter";
            i.Initialize(new Dictionary<String, String>()
            {
            });
            var r = i.FetchPublicTimeline("", s, new Dictionary<String, String>()
            {
                {"count", "100"},
            }).ToList();

        }
    }
}
