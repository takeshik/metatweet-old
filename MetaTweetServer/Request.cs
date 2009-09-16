// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetServer
 *   Server library of MetaTweet
 *   Part of MetaTweet
 * Copyright © 2008-2009 Takeshi KIRIYA, XSpect Project <takeshik@users.sf.net>
 * All rights reserved.
 * 
 * This file is part of MetaTweetServer.
 * 
 * This library is free software; you can redistribute it and/or modify it
 * under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation; either version 3 of the License, or (at your
 * option) any later version.
 * 
 * This library is distributed in the hope that it will be useful, but
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
using System.Text.RegularExpressions;
using System.Linq;
using XSpect.MetaTweet.ObjectModel;
using XSpect.MetaTweet.Modules;
using System.Collections;
using Achiral;
using Achiral.Extension;
using XSpect.Extension;

namespace XSpect.MetaTweet
{
    /// <summary>
    /// サーバ オブジェクトに対する要求を表します。
    /// </summary>
    /// <remarks>
    /// <para>サーバ オブジェクトに対する要求、即ちロードされているモジュールに動作を行わせる操作は、1 以上の要求単位の連結として表現されます。要求単位は、取得したデータを格納する <see cref="Storage"/>、実際に動作を行う <see cref="FlowModule"/>、動作の具体的内容を指定するための文字列、および引数として定義されます。</para>
    /// <para>要求 (および要求単位) は特定の書式に基づいた文字列によって定義されます。<see cref="Parse(String)"/> および <see cref="TryParse"/> メソッドによりこの文字列から要求を生成できます。要求文字列の書式は以下に示すとおりです:</para>
    /// <para><c>/[$STORAGE]![FLOW]/SELECTOR[?ARGUMENTS]</c></para>
    /// <para>上において、<c>[ ... ]</c> 間は省略できることを示します。大文字のアルファベットで表される非終端記号の説明を以下に示します:</para>
    /// <list type="bullet">
    /// <item>
    /// <term>STORAGE</term>
    /// <description>取得したデータの入出力に用いる <see cref="Storage"/> の名前。省略した場合は一つ前 (左) の要求単位のストレージ名。先端の場合は <c>main</c>。</description>
    /// </item>
    /// <item>
    /// <term>FLOW</term>
    /// <description>実際の処理を行う <see cref="FlowModule"/> の名前。省略した場合は一つ前 (左) の要求単位のモジュール名。先端の場合は <c>sys</c>。</description>
    /// </item>
    /// <item>
    /// <term>SELECTOR</term>
    /// <description><see cref="FlowModule"/> に対し照合されるセレクタ文字列。照合の際には文字列の先頭に <c>/</c> が補われます。</description>
    /// </item>
    /// <item>
    /// <term>ARGUMENTS</term>
    /// <description>要求に与えられる引数。<c>KEY=VALUE</c> として表される、文字列のキーと値の組のリスト。組のデリミタは <c>&amp;</c>です。URI クエリと同一の形式です。</description>
    /// </item>
    /// </list>
    /// <para>また、文字列からの要求の生成においては、互換表記変換、エスケープ処理、および文字参照変換処理が適用されます。</para>
    /// <para>互換表記変換は、HTTP スキーマの URI に近い表記を許容するための処理で、右端の要求単位に対し、以下のような処理が適用されます:</para>
    /// <list type="bullet">
    /// <item><description><c>.../selector.ext</c> から <c>.../selector/!/.ext</c> へ</description></item>
    /// <item><description><c>.../selector.ext?key=value</c> から <c>.../selector?key=value/!/.ext</c> へ</description></item>
    /// <item><description><c>...//.ext</c> から <c>...//!/.ext</c> へ</description></item>
    /// <item><description><c>...//.ext?key=value</c> から <c>.../?key=value/!/.ext</c> へ</description></item>
    /// </list>
    /// <para><see cref="OriginalString"/> の値において、この処理が行われる前の要求文字列は維持されません。<see cref="OriginalString"/> の値はこの変換が行われた後のものになります。</para>
    /// <para>エスケープ処理は、<c>\</c> とそれに続く一文字を (エスケープ シーケンス) 後述する文字参照に置き換える処理です。大半の文字は同一の文字を表す文字参照に変換されますが、次に挙げる文字は特別な記号に置き換えられます:</para>
    /// <list type="bullet">
    /// <item><description><c>\0</c>: ASCII NUL (文字参照: <c>#0;</c>)</description></item>
    /// <item><description><c>\a</c>: ASCII BEL (文字参照: <c>#7;</c>)</description></item>
    /// <item><description><c>\e</c>: ASCII ESC (文字参照: <c>#27;</c>)</description></item>
    /// <item><description><c>\f</c>: ASCII FF (文字参照: <c>#12;</c>)</description></item>
    /// <item><description><c>\n</c>: ASCII LF (文字参照: <c>#10;</c>)</description></item>
    /// <item><description><c>\r</c>: ASCII CR (文字参照: <c>#13;</c>)</description></item>
    /// <item><description><c>\s</c>: ASCII SPC (文字参照: <c>#32;</c>)</description></item>
    /// <item><description><c>\t</c>: ASCII HT (文字参照: <c>#9;</c>)</description></item>
    /// <item><description><c>\v</c>: ASCII VT (文字参照: <c>#11;</c>)</description></item>
    /// </list>
    /// <para><see cref="OriginalString"/> の値において、エスケープ シーケンスはすべて文字参照に変換され、維持されません。</para>
    /// <para>文字参照変換処理は、文字参照を通常の文字に変換する処理です。文字参照は正規表現 <c>#x?\d+;</c> として定義される表記で、XML における文字参照 <c>&amp;#x?\d+;</c> に完全に一致し、提示された数値 (x が前置されている場合は 16 進値) のコード ポイントを持つ文字に変換されます。また、その副作用として、変換された結果の文字が要求文字列内で特別な意味を持つ場合、その効果は抑止されます。</para>
    /// <para><see cref="OriginalString"/> の値において、文字参照はあるがままの状態で完全に維持されます。</para>
    /// </remarks>
    /// <seealso cref="ServerCore.Request"/>
    [Serializable()]
    public class Request
        : Object,
          IEnumerable<Request>
    {
        private static readonly IDictionary<String, String> _escapeCharTable = Create.Table(
            @"\0", GetCharacterReference('\0'),
            @"\a", GetCharacterReference('\a'),
            @"\e", GetCharacterReference('\x1b'),
            @"\f", GetCharacterReference('\f'),
            @"\n", GetCharacterReference('\n'),
            @"\r", GetCharacterReference('\r'),
            @"\s", GetCharacterReference(' '),
            @"\t", GetCharacterReference('\t'),
            @"\v", GetCharacterReference('\v'),
            @"\\", GetCharacterReference('\\'),
            @"\#", GetCharacterReference('#'),
            @"\;", GetCharacterReference(';')
        );

        private readonly Request _followingRequest;

        /// <summary>
        /// 取得したデータの入出力に用いる <see cref="Storage"/> の名前を取得します。
        /// </summary>
        /// <value>
        /// 取得したデータの入出力に用いる <see cref="Storage"/> の名前。
        /// </value>
        public String StorageName
        {
            get;
            private set;
        }

        /// <summary>
        /// 実際の処理を行う <see cref="FlowModule"/> の名前を取得します。
        /// </summary>
        /// <value>
        /// 実際の処理を行う <see cref="FlowModule"/> の名前。
        /// </value>
        public String FlowName
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="FlowModule"/> に対し照合されるセレクタ文字列を取得します。
        /// </summary>
        /// <value>
        /// <see cref="FlowModule"/> に対し照合されるセレクタ文字列。セレクタは文字 <c>/</c> から開始します。
        /// </value>
        public String Selector
        {
            get;
            private set;
        }

        /// <summary>
        /// 要求に与えられる引数のリストを取得します。
        /// </summary>
        /// <value>
        /// 要求に与えられる引数のリスト。
        /// </value>
        public IDictionary<String, String> Arguments
        {
            get;
            private set;
        }

        /// <summary>
        /// 正規化される前の文字列を取得します。
        /// </summary>
        /// <value>
        /// 正規化される前の文字列。要求が文字列から生成されなかった場合は <c>null</c>。
        /// </value>
        /// <remarks>
        /// 文字列から要求が生成された場合、実際に生成のために与えられた文字列を取得します。ただし、末尾の要求単位に関しては、適用される互換性のための変換 (<see cref="T:Request"/> を参照) が適用された状態の文字列が返されます。
        /// </remarks>
        public String OriginalString
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="Request"/> の新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="storageName">取得したデータの入出力に用いる <see cref="Storage"/> の名前。</param>
        /// <param name="flowName">実際の処理を行う <see cref="FlowModule"/> の名前。</param>
        /// <param name="selector"><see cref="FlowModule"/> に対し照合されるセレクタ文字列。</param>
        /// <param name="arguments">要求に与える引数のリスト。</param>
        /// <param name="followingRequest">この要求に継続する次の要求。</param>
        public Request(
            String storageName,
            String flowName,
            String selector,
            IDictionary<String, String> arguments,
            Request followingRequest
        )
            : this(
                storageName,
                flowName,
                selector,
                arguments,
                null,
                followingRequest
            )
        {
        }

        /// <summary>
        /// <see cref="Request"/> の新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="storageName">取得したデータの入出力に用いる <see cref="Storage"/> の名前。</param>
        /// <param name="flowName">実際の処理を行う <see cref="FlowModule"/> の名前。</param>
        /// <param name="selector"><see cref="FlowModule"/> に対し照合されるセレクタ文字列。</param>
        /// <param name="arguments">要求に与える引数のリスト。</param>
        public Request(
            String storageName,
            String flowName,
            String selector,
            IDictionary<String, String> arguments
        )
            : this(
                storageName,
                flowName,
                selector,
                arguments,
                null,
                null
            )
        {
        }

        /// <summary>
        /// <see cref="Request"/> の新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="storageName">取得したデータの入出力に用いる <see cref="Storage"/> の名前。</param>
        /// <param name="flowName">実際の処理を行う <see cref="FlowModule"/> の名前。</param>
        /// <param name="selector"><see cref="FlowModule"/> に対し照合されるセレクタ文字列。</param>
        /// <param name="followingRequest">この要求に継続する次の要求。</param>
        public Request(
            String storageName,
            String flowName,
            String selector,
            Request followingRequest
        )
            : this(
                storageName,
                flowName,
                selector,
                null,
                null,
                followingRequest
            )
        {
        }

        /// <summary>
        /// <see cref="Request"/> の新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="storageName">取得したデータの入出力に用いる <see cref="Storage"/> の名前。</param>
        /// <param name="flowName">実際の処理を行う <see cref="FlowModule"/> の名前。</param>
        /// <param name="selector"><see cref="FlowModule"/> に対し照合されるセレクタ文字列。</param>
        public Request(
            String storageName,
            String flowName,
            String selector
        )
            : this(
                storageName,
                flowName,
                selector,
                null,
                null,
                null
            )
        {
        }

        /// <summary>
        /// <see cref="Request"/> の新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="storageName">取得したデータの入出力に用いる <see cref="Storage"/> の名前。</param>
        /// <param name="flowName">実際の処理を行う <see cref="FlowModule"/> の名前。</param>
        /// <param name="selector"><see cref="FlowModule"/> に対し照合されるセレクタ文字列。</param>
        /// <param name="arguments">要求に与える引数のリスト。</param>
        /// <param name="originalString">実際に生成のために与えられた文字列。</param>
        /// <param name="followingRequest">この要求に継続する次の要求。</param>
        private Request(
            String storageName,
            String flowName,
            String selector,
            IDictionary<String, String> arguments,
            String originalString,
            Request followingRequest
        )
        {
            this.StorageName = storageName;
            this.FlowName = flowName;
            this.Selector = selector.StartsWith("/") ? selector : "/" + selector;
            this.Arguments = arguments ?? new Dictionary<String, String>();
            this.OriginalString = originalString;
            this._followingRequest = followingRequest;
        }

        /// <summary>
        /// 要求文字列から <see cref="Request"/> を生成します。
        /// </summary>
        /// <param name="str">生成のために与えられる文字列。</param>
        /// <returns>生成された要求。</returns>
        public static Request Parse(String str)
        {
            str.Replace(_escapeCharTable);
            Regex.Replace(str, "\\.", m => GetCharacterReference(m.Value[0]));
            if (!str.EndsWith("/") && str[str.LastIndexOf('/') + 1] != '.')
            {
                // example.ext?foo=bar -> example?foo=bar/!/.ext
                str = Regex.Replace(str, @"(\.[^?]*)(\?.*)?$", @"$2/!/$1");
            }
            if (str.Contains("//."))
            {
                // //.ext?foo=bar -> /?foo=bar/!/.ext
                str = Regex.Replace(str, @"//(\.[^?]*)(\?.*)?$", @"/$2/!/$1");
            }

            return Make.Array(default(String))
                // Skip first empty string
                .Concat(Regex.Split(str, "/[!$]").SkipWhile(String.IsNullOrEmpty))
                .Pairwise((prev, one) => Make.Tuple(prev, one))
                .Do(tuples => Parse(tuples.First().Item2, "main", "sys", tuples.Skip(1)));
        }

        /// <summary>
        /// 要求文字列から <see cref="Request"/> を生成します。
        /// </summary>
        /// <param name="str">生成のために与えられる文字列。</param>
        /// <param name="storage">既定値として与える <see cref="Request"/> の <see cref="StorageName"/>。</param>
        /// <param name="flow">既定値として与える <see cref="Request"/> の <see cref="FlowName"/>。</param>
        /// <param name="rest">後続する要求文字列と、その一つ前の要求文字列の組のシーケンス。</param>
        /// <returns>生成された要求。</returns>
        protected static Request Parse(
            String str,
            String storage,
            String flow,
            IEnumerable<Tuple<String, String>> rest
        )
        {
            String original = str;
            String prefixes = str.Substring(0, str.IndexOf('/'));

            // a) .../$storage!module/... -> storage!module/...
            // b) .../$storage!/...       -> storage!/...
            // c) .../!module/...         -> module/...
            // d) .../!/...               -> /...
            if (prefixes.Contains("!"))
            {
                original = original.Insert(0, "/$");

                if (!prefixes.EndsWith("!")) // a) Specified Storage and Module
                {
                    String[] prefixArray = prefixes.Split('!');
                    storage = prefixArray[0];
                    flow = prefixArray[1];
                }
                else // b) Specified Storage
                {
                    storage = prefixes.TrimEnd('!');
                    // Module is taken over.
                }
            }
            else
            {
                original = original.Insert(0, "/!");

                if (prefixes != String.Empty) // c) Specified Module
                {
                    // Storage is taken over.
                    flow = prefixes;
                }
                else // d) Specified nothing
                {
                    // Do nothing; Storage and Module are taken over.
                }
            }

            String selector;
            Dictionary<String, String> argumentDictionary = new Dictionary<String, String>();

            if (str.Contains("?"))
            {
                selector = str.Substring(prefixes.Length, str.IndexOf('?') - prefixes.Length);
                String arguments = str.Substring(prefixes.Length + selector.Length);
                argumentDictionary.AddRange(arguments
                    .TrimStart('?')
                    .Split('&')
                    .Select(s => s.Split('='))
                    .Select(s => Create.KeyValuePair(s[0], s[1]))
                );
            }
            else
            {
                selector = str.Substring(prefixes.Length);
            }

            return new Request(
                storage = ResolveCharacterReferences(storage),
                flow = ResolveCharacterReferences(flow),
                ResolveCharacterReferences(selector),
                argumentDictionary.SelectKeyValue(
                    k => ResolveCharacterReferences(k),
                    v => ResolveCharacterReferences(v)
                ),
                original,
                // rest: tuple (prev, one)
                rest.Any() ? Parse(rest.First().Item2, storage, flow, rest.Skip(1)) : null
            );
        }

        /// <summary>
        /// 要求文字列から <see cref="Request"/> を生成します。戻り値は、変換が成功したかどうかを示します。
        /// </summary>
        /// <param name="str">生成のために与えられる文字列。</param>
        /// <param name="request">生成が成功した場合、このメソッドが返されるときに、生成された要求を格納します。変換に失敗した場合は <c>null</c> を格納します。このパラメータは初期化せずに渡されます。</param>
        /// <returns></returns>
        public static Boolean TryParse(String str, out Request request)
        {
            return str.Try(Request.Parse, out request);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<Request> GetEnumerator()
        {
            yield return this;
            if (this._followingRequest == null)
            {
                yield break;
            }
            yield return this._followingRequest;
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override String ToString()
        {
            return _escapeCharTable.ReverseKeyValue().Do(table =>
                this.Select(req => String.Format(
                    "/${0}!{1}{2}{3}",
                    req.StorageName.Replace(table),
                    req.FlowName.Replace(table),
                    req.Selector.Replace(table),
                    req.Arguments.SelectKeyValue(k => k.Replace(table), v => v.Replace(table))
                        .ToUriQuery()
                )).Join(String.Empty)
            );
        }

        private static String GetCharacterReference(Char c)
        {
            return "#" + (Int32) c + ";";
        }

        private static String ResolveCharacterReferences(String str)
        {
            return Regex.Replace(str, @"#\d+;", m =>
                ((Char) (m.Value.StartsWith("#x")
                    ? Int32.Parse(m.Value.Substring(2).TrimEnd(';'), System.Globalization.NumberStyles.HexNumber)
                    : Int32.Parse(m.Value.Trim('#', ';'))
                )).ToString()
            );
        }
    }
}
