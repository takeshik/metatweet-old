using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace XSpect.MetaTweet.Test
{
    public class Program
    {
        public static void Main(String[] args)
        {
            Console.WriteLine("MetaTweet Server Testing Host\n");

            Console.Write("ServerCore: initializing ");
            var c = new ServerCore();
            Console.WriteLine("OK");

            Console.Write("SQLiteStorage: initializing ");
            c.LoadAssembly("asm.sqlite", new SQLiteStorage().GetType().Assembly.GetName());
            c.LoadModule("sqlite", typeof(SQLiteStorage));
            var s = c.GetStorage("sqlite") as SQLiteStorage;
            s.Initialize(@"data source=MetaTweet.db;binaryguid=False");
            Console.Write("creating ");
            s.CreateTables();
            s.Connect();
            Console.WriteLine("OK");

            Console.Write("TwitterApiInput: initializing ");
            c.LoadAssembly("asm.twitter", new TwitterApiInput().GetType().Assembly.GetName());
            c.LoadModule("twitter", typeof(TwitterApiInput));
            var i = c.GetInput("twitter") as TwitterApiInput;
            i.Realm = "com.twitter";
            Console.Write("credentialing ");
            try
            {
                using (StreamReader reader = new StreamReader(@"mtw-credential"))
                {
                    i.Initialize(new Dictionary<String, String>()
                    {
                        {"username", reader.ReadLine()},
                        {"password", reader.ReadLine()},
                    });
                }
            }
            catch(FileNotFoundException)
            {
                Console.Write("(failed: use empty) ");
                i.Initialize(new Dictionary<String, String>()
                {
                    {"username", ""},
                    {"password", ""},
                });
            }
            Console.WriteLine("OK");

            Console.Write("RemotingServant: initializing ");
            c.LoadAssembly("asm.remoting", new RemotingServant().GetType().Assembly.GetName());
            c.LoadModule("remoting", typeof(RemotingServant));
            var r = c.GetServant("remoting") as RemotingServant;
            r.Initialize(new Dictionary<String, String>());
            Console.Write("starting ");
            r.Start();
            Console.WriteLine("OK");
            int count = 0;
            while (true)
            {
                if (count++ % 60 == 0)
                {
                    var list = i.FetchFriendsTimeline("", s, new Dictionary<String, String>()
                    {
                        {"count", "100"},
                    }).ToList();
                    s.Update();
                    foreach (var o in list)
                    {
                        Console.WriteLine(o);
                    }
                }
                Console.WriteLine("Acc: {0:0000} Act: {1:0000} Pst: {2:0000} Flw: {3:0000} Fav: {4:0000} Tag: {5:0000} Rep: {6:0000}",
                    s.UnderlyingDataSet.Accounts.Count,
                    s.UnderlyingDataSet.Activities.Count,
                    s.UnderlyingDataSet.Posts.Count,
                    s.UnderlyingDataSet.FollowMap.Count,
                    s.UnderlyingDataSet.FavorMap.Count,
                    s.UnderlyingDataSet.TagMap.Count,
                    s.UnderlyingDataSet.ReplyMap.Count
                );
                Thread.Sleep(1000);
            }
        }
    }
}
