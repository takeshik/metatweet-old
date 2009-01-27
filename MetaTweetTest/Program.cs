using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace XSpect.MetaTweet.Test
{
    internal class Program
    {
        internal static void Main(String[] args)
        {
            /*Console.WriteLine("Launching debugger...");
            Debugger.Launch();
            Debugger.Break();*/
            var c = new ServerCore();
            var s = new SQLiteStorage();
            s.Host = c;
            s.Name = "sqlite";
            s.Initialize(@"data source=C:\MetaTweet.db;binaryguid=False");
            s.DropTables();
            s.CreateTables();
            s.Connect();
            var i = new TwitterApiInput();
            i.Host = c;
            i.Name = "twitter";
            i.Realm = "com.twitter";
            using (StreamReader reader = new StreamReader(@"C:\mtw-credential"))
            {
                i.Initialize(new Dictionary<String, String>()
                {
                    {"username", reader.ReadLine()},
                    {"password", reader.ReadLine()},
                });
            }
            /*var r = i.FetchFriendsTimeline("", s, new Dictionary<String, String>()
            {
                {"count", "100"},
            }).ToList();*/
            var r = i.FetchUserTimeline("", s, new Dictionary<String, String>()
            {
                {"count", "100"},
                {"id", "takeshik"},
            }).ToList();
            s.Update();
        }
    }
}
