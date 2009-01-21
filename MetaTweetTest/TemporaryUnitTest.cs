// -*- mode: csharp; encoding: utf-8; -*-
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetTest
 *   Test codes for MetaTweet
 *   Part of MetaTweet
 * Copyright © 2008-2009 Takeshi KIRIYA, XSpect Project <takeshik@xspect.org>
 * All rights reserved.
 * 
 * This file is part of MetaTweetTest.
 * 
 * This program is free software; you can redistribute it and/or modify it
 * under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation; either version 3 of the License, or (at your
 * option) any later version.
 * 
 * This program is distributed in the hope that it will be useful, but
 * WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
 * or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public
 * License for more details. 
 * 
 * You should have received a copy of the GNU Lesser General Public License
 * along with this program. If not, see <http://www.gnu.org/licenses/>,
 * or write to the Free Software Foundation, Inc., 51 Franklin Street,
 * Fifth Floor, Boston, MA 02110-1301, USA.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XSpect.MetaTweet.ObjectModel;
using System.Xml;
using System.IO;

namespace XSpect.MetaTweet.Test
{
    /// <summary>
    /// UnitTest の概要の説明
    /// </summary>
    [TestClass]
    public class TemporaryUnitTest
    {
        public TemporaryUnitTest()
        {
            //
            // TODO: コンストラクタ ロジックをここに追加します
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///現在のテストの実行についての情報および機能を
        ///提供するテスト コンテキストを取得または設定します。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 追加のテスト属性
        //
        // テストを作成する際には、次の追加属性を使用できます:
        //
        // クラス内で最初のテストを実行する前に、ClassInitialize を使用してコードを実行してください
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // クラス内のテストをすべて実行したら、ClassCleanup を使用してコードを実行してください
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // 各テストを実行する前に、TestInitialize を使用してコードを実行してください
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // 各テストを実行した後に、TestCleanup を使用してコードを実行してください
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void ObjectModelTest()
        {
            var s = new SQLiteStorage();
            s.Initialize(@"data source=C:\MetaTweet.db;binaryguid=False");
            s.DropTables();
            s.CreateTables();
            s.Connect();

            Func<Int32, Guid> g = (Int32 i) =>
            {
                return new Guid(new String(i.ToString("x")[0], 32));
            };

            List<Account> accs = new List<Account>();

            for (Int32 i = 1; i < 10; ++i)
            {
                accs.Add(s.NewAccount());
                accs.Last().AccountId = g(i);
                accs.Last().Realm = "com.twitter";
                accs.Last().Update();
            }

            accs[0].AddFollowing(accs[1]);
            accs[0].AddFollower(accs[2]);
            accs[0].AddFollower(accs[3]);
            accs[1].AddFollowing(accs[0]);
            accs[1].AddFollowing(accs[2]);
            accs[1].AddFollowing(accs[3]);
            accs[4].AddFollower(accs[0]);

            Activity act;

            act = accs[0].NewActivity();
            act.Timestamp = new DateTime(2000, 1, 1, 0, 0, 0);
            act.Category = "Id";
            act.Value = "12345678";
            act.Update();
            
            act = accs[0].NewActivity();
            act.Timestamp = new DateTime(2000, 1, 1, 0, 0, 0);
            act.Category = "ScreenName";
            act.Value = "johnd";
            act.Update();
            act.AddTag("Test");

            act = accs[0].NewActivity();
            act.Timestamp = new DateTime(2000, 1, 1, 0, 0, 0);
            act.Category = "Name";
            act.Value = "John Doe";
            act.Update();

            act = accs[1].NewActivity();
            act.Timestamp = new DateTime(2000, 1, 1, 0, 0, 0);
            act.Category = "Id";
            act.Value = "87654321";
            act.Update();

            act = accs[1].NewActivity();
            act.Timestamp = new DateTime(2000, 1, 1, 0, 0, 0);
            act.Category = "ScreenName";
            act.Value = "janed";
            act.Update();

            act = accs[1].NewActivity();
            act.Timestamp = new DateTime(2000, 1, 1, 0, 0, 0);
            act.Category = "Name";
            act.Value = "Jane Doe";
            act.Update();
            act.AddTag("Test");
            act.AddTag("Test2");

            act = accs[0].NewActivity();
            act.Timestamp = new DateTime(2000, 1, 1, 4, 0, 0);
            act.Category = "Name";
            act.Value = "John A. Doe";
            act.Update();

            act = accs[0].NewActivity();
            act.Timestamp = new DateTime(2000, 1, 1, 6, 0, 0);
            act.Category = "Post";
            act.Value = "t01";
            act.Update();

            var pst = act.ToPost();
            pst.Text = "Test post.";
            pst.Source = "web";
            pst.IsFavorited = true;
            pst.IsRestricted = true;
            pst.FavoriteCount = 3;
            pst.Update();

            accs[0].Delete();
            accs[0].Update();
        }

        [TestMethod]
        public void ServerCoreTest()
        {
            var c = new ServerCore();
            c.LoadAssembly("sqlite", "SQLiteStorage.dll");
            c.LoadAssembly("sqlite", "SQLiteStorage.dll");
            c.LoadModule("sqlite", "XSpect.MetaTweet.SQLiteStorage");
            c.LoadAssembly("remoting", "RemotingServant.dll");
            c.LoadModule("remoting", "XSpect.MetaTweet.RemotingServant");
            var i = new TwitterApiInput();
            using (StreamReader reader = new StreamReader(@"C:\mtw-credential"))
            {
                i.Initialize(new Dictionary<String, String>()
                {
                    {"username", reader.ReadLine()},
                    {"password", reader.ReadLine()},
                });
            }
            var xres = i.InvokeRest(new System.Uri("http://twitter.com/statuses/friends_timeline.xml?id=takeshik"), "POST");
            i.Register(c, "twitterclient");
        }

        [TestMethod]
        public void TwitterApiInputTest()
        {
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
            using (StreamReader reader = new StreamReader(@"C:\mtw-credential"))
            {
                i.Initialize(new Dictionary<String, String>()
                {
                    {"username", reader.ReadLine()},
                    {"password", reader.ReadLine()},
                });
            }
            var r = i.FetchFriendsTimeline("", s, new Dictionary<String, String>()
            {
                {"count", "100"},
            }).ToList();
        }
    }
}
　