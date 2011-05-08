// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetFoundation
 *   Common library to access MetaTweet platform
 *   Part of MetaTweet
 * Copyright © 2008-2011 Takeshi KIRIYA (aka takeshik) <takeshik@users.sf.net>
 * All rights reserved.
 * 
 * This file is part of MetaTweetFoundation.
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace XSpect.MetaTweet.Requesting
{
    public class Request
        : Object,
          IEnumerable<Fragment>
    {
        public IEnumerable<Fragment> Fragments
        {
            get;
            private set;
        }

        public String OriginalString
        {
            get;
            private set;
        }

        public Request(IEnumerable<Fragment> fragments)
            : this(fragments, String.Concat(fragments.Select(f => f.ToString())))
        {
        }

        public Request(params Fragment[] fragments)
            : this((IEnumerable<Fragment>) fragments)
        {
        }

        public override String ToString()
        {
            return String.Concat(this.Select(f => f.ToString()));
        }

        public IEnumerator<Fragment> GetEnumerator()
        {
            return this.Fragments.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private Request(IEnumerable<Fragment> fragments, String originalString)
        {
            this.Fragments = fragments;
            this.OriginalString = originalString;
        }

        public static Request Parse(String str)
        {
            return new Request(GetFragments(
                Regex.Matches(str, @"(/(?:\$[^!@\(\)\-]+)?[!@\(\)\-])(.*?(?=/[\$!@\(\)\-]|$))")
                    .Cast<Match>()
                    .Select(m => new[] { m.Groups[1].Value, m.Groups[2].Value, })
            ), str);
        }

        private static IEnumerable<Fragment> GetFragments(IEnumerable<String[]> input)
        {
            Stack<Tuple<IDictionary<String, String>, LinkedList<Fragment>>> scopeStack
                = new Stack<Tuple<IDictionary<String, String>, LinkedList<Fragment>>>();
            foreach (String[] data in input)
            {
                Fragment fragment = null;
                switch (data[0].Last())
                {
                    case '(':
                        scopeStack.Push(Tuple.Create(GetVariableTable(data[0]), new LinkedList<Fragment>()));
                        break;
                    case ')':
                        Tuple<IDictionary<String, String>, LinkedList<Fragment>> tuple = scopeStack.Pop();
                        fragment = new ScopeFragment(tuple.Item1, tuple.Item2.ToArray());
                        break;
                    case '!':
                        Int32 index = data[1].IndexOf("?");
                        Dictionary<String, String> arguments = index != -1
                            ? data[1].Substring(index + 1).Split('&')
                                  .Select(p => p.Split(new Char[] { '=' }, 2))
                                  .ToDictionary(p => p[0], p => p[1][0] == '='
                                      ? Encoding.UTF8.GetString(DecodeBase64String(p[1]))
                                      : p[1]
                                  )
                            : new Dictionary<String, String>();
                        String flowName = data[1].First() == '/' ? "" : data[1].Substring(0, data[1].IndexOf('/'));
                        String selector = data[1].Substring(flowName.Length, index != -1 ? index - flowName.Length : data[1].Length - flowName.Length);
                        fragment = new FlowFragment(GetVariableTable(data[0]), flowName, selector, arguments);
                        break;
                    case '@':
                        fragment = new CodeFragment(GetVariableTable(data[0]), data[1]);
                        break;
                    case '-':
                        fragment = new OperatorFragment(GetVariableTable(data[0]), data[1]);
                        break;
                    default:
                        break;
                }
                if (fragment != null)
                {
                    if (scopeStack.Any())
                    {
                        scopeStack.Peek().Item2.AddLast(fragment);
                    }
                    else
                    {
                        yield return fragment;
                    }
                }
            }
        }

        private static IDictionary<String, String> GetVariableTable(String str)
        {
            return str[1] == '$'
                ? str
                      .Substring(2, str.Length - 3)
                      .Split('$')
                      .Select(p => p.Split(new Char[] { '=' }, 2))
                      .ToDictionary(p => p[0], p => p[1][0] == '='
                          ? Encoding.UTF8.GetString(DecodeBase64String(p[1]))
                          : p[1]
                      )
                : new Dictionary<String, String>();
        }

        private static Byte[] DecodeBase64String(String str)
        {
            if (str[0] == '=')
            {
                str = str.Substring(1);
            }
            str = str.Replace('+', '-').Replace('_', '/');
            return Convert.FromBase64String(str.Length % 4 == 0
                ? str
                : str + new String('=', 4 - (str.Length % 4))
            );
        }
    }
}