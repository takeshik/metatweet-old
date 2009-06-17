// -*- mode: csharp; encoding: utf-8; -*-
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
using Achiral;

namespace XSpect.MetaTweet
{
    /// <summary>
    /// 引数を持たないフック リストを表します。
    /// </summary>
    public class Hook
        : HookBase<Action, Action<Exception>>
    {
        /// <summary>
        /// <see cref="Hook"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public Hook()
        {
            this.Before = new List<Action>();
            this.After = new List<Action>();
            this.Failed = new List<Action<Exception>>();
        }

        /// <summary>
        /// フックを適用した、返り値のないコードを実行します。
        /// </summary>
        /// <param name="body">実行するコード。</param>
        public void Execute(Action body)
        {
            try
            {
                foreach (Action hook in this.Before)
                {
                    hook();
                }
                body();
                foreach (Action hook in this.After)
                {
                    hook();
                }
            }
            catch (Exception ex)
            {
                foreach (Action<Exception> hook in this.Failed)
                {
                    hook(ex);
                }
            }
        }

        /// <summary>
        /// フックを適用した、返り値のあるコードを実行します。
        /// </summary>
        /// <typeparam name="TResult">返り値の型。</typeparam>
        /// <param name="body">実行するコード。</param>
        /// <returns><paramref name="body"/> の返り値。</returns>
        public TResult Execute<TResult>(Func<TResult> body)
        {
            try
            {
                foreach (Action hook in this.Before)
                {
                    hook();
                }
                TResult result = body();
                foreach (Action hook in this.After)
                {
                    hook();
                }
                return result;
            }
            catch (Exception ex)
            {
                foreach (Action<Exception> hook in this.Failed)
                {
                    hook(ex);
                }
                return default(TResult);
            }
        }
    }

    /// <summary>
    /// 引数を 1 つ持つフック リストを表します。
    /// </summary>
    /// <typeparam name="T">引数の型。</typeparam>
    public class Hook<T>
        : HookBase<Action<T>, Action<T, Exception>>
    {
        /// <summary>
        /// <see cref="Hook{T}"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public Hook()
        {
            this.Before = new List<Action<T>>();
            this.After = new List<Action<T>>();
            this.Failed = new List<Action<T, Exception>>();
        }

        /// <summary>
        /// フックを適用した、返り値のないコードを実行します。
        /// </summary>
        /// <param name="body">実行するコード。</param>
        /// <param name="arg">引数の値。</param>
        public void Execute(Action<T> body, T arg)
        {
            try
            {
                foreach (Action<T> hook in this.Before)
                {
                    hook(arg);
                }
                body(arg);
                foreach (Action<T> hook in this.After)
                {
                    hook(arg);
                }
            }
            catch (Exception ex)
            {
                if (arg is ILoggable)
                {
                    (arg as ILoggable).Log.Fatal("Unhandled exception occured.", ex);
                }
                foreach (Action<T, Exception> hook in this.Failed)
                {
                    hook(arg, ex);
                }
            }
        }

        /// <summary>
        /// フックを適用した、返り値のあるコードを実行します。
        /// </summary>
        /// <param name="body">実行するコード。</param>
        /// <param name="arg">引数の値。</param>
        /// <returns><paramref name="body"/> の返り値。</returns>
        public TResult Execute<TResult>(Func<T, TResult> body, T arg)
        {
            try
            {
                foreach (Action<T> hook in this.Before)
                {
                    hook(arg);
                }
                TResult result = body(arg);
                foreach (Action<T> hook in this.After)
                {
                    hook(arg);
                }
                return result;
            }
            catch (Exception ex)
            {
                if (arg is ILoggable)
                {
                    (arg as ILoggable).Log.Fatal("Unhandled exception occured.", ex);
                }
                foreach (Action<T, Exception> hook in this.Failed)
                {
                    hook(arg, ex);
                }
                return default(TResult);
            }
        }
    }

    /// <summary>
    /// 引数を 2 つ持つフック リストを表します。
    /// </summary>
    /// <typeparam name="T1">第 1 引数の型。</typeparam>
    /// <typeparam name="T2">第 2 引数の型。</typeparam>
    public class Hook<T1, T2>
        : HookBase<Action<T1, T2>, Action<T1, T2, Exception>>
    {
        /// <summary>
        /// <see cref="Hook{T1, T2}"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public Hook()
        {
            this.Before = new List<Action<T1, T2>>();
            this.After = new List<Action<T1, T2>>();
            this.Failed = new List<Action<T1, T2, Exception>>();
        }

        /// <summary>
        /// フックを適用した、返り値のないコードを実行します。
        /// </summary>
        /// <param name="body">実行するコード。</param>
        /// <param name="arg1">第 1 引数の値。</param>
        /// <param name="arg2">第 2 引数の値。</param>
        public void Execute(Action<T1, T2> body, T1 arg1, T2 arg2)
        {
            try
            {
                foreach (Action<T1, T2> hook in this.Before)
                {
                    hook(arg1, arg2);
                }
                body(arg1, arg2);
                foreach (Action<T1, T2> hook in this.After)
                {
                    hook(arg1, arg2);
                }
            }
            catch (Exception ex)
            {
                if (arg1 is ILoggable)
                {
                    (arg1 as ILoggable).Log.Fatal("Unhandled exception occured.", ex);
                }
                foreach (Action<T1, T2, Exception> hook in this.Failed)
                {
                    hook(arg1, arg2, ex);
                }
            }
        }

        /// <summary>
        /// フックを適用した、返り値のあるコードを実行します。
        /// </summary>
        /// <param name="body">実行するコード。</param>
        /// <param name="arg1">第 1 引数の値。</param>
        /// <param name="arg2">第 2 引数の値。</param>
        /// <returns><paramref name="body"/> の返り値。</returns>
        public TResult Execute<TResult>(Func<T1, T2, TResult> body, T1 arg1, T2 arg2)
        {
            try
            {
                foreach (Action<T1, T2> hook in this.Before)
                {
                    hook(arg1, arg2);
                }
                TResult result = body(arg1, arg2);
                foreach (Action<T1, T2> hook in this.After)
                {
                    hook(arg1, arg2);
                }
                return result;
            }
            catch (Exception ex)
            {
                if (arg1 is ILoggable)
                {
                    (arg1 as ILoggable).Log.Fatal("Unhandled exception occured.", ex);
                }
                foreach (Action<T1, T2, Exception> hook in this.Failed)
                {
                    hook(arg1, arg2, ex);
                }
                return default(TResult);
            }
        }
    }

    /// <summary>
    /// 引数を 3 つ持つフック リストを表します。
    /// </summary>
    /// <typeparam name="T1">第 1 引数の型。</typeparam>
    /// <typeparam name="T2">第 2 引数の型。</typeparam>
    /// <typeparam name="T3">第 3 引数の型。</typeparam>
    public class Hook<T1, T2, T3>
        : HookBase<Action<T1, T2, T3>, Action<T1, T2, T3, Exception>>
    {
        /// <summary>
        /// <see cref="Hook{T1, T2, T3}"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public Hook()
        {
            this.Before = new List<Action<T1, T2, T3>>();
            this.After = new List<Action<T1, T2, T3>>();
            this.Failed = new List<Action<T1, T2, T3, Exception>>();
        }

        /// <summary>
        /// フックを適用した、返り値のないコードを実行します。
        /// </summary>
        /// <param name="body">実行するコード。</param>
        /// <param name="arg1">第 1 引数の値。</param>
        /// <param name="arg2">第 2 引数の値。</param>
        /// <param name="arg3">第 3 引数の値。</param>
        public void Execute(Action<T1, T2, T3> body, T1 arg1, T2 arg2, T3 arg3)
        {
            try
            {
                foreach (Action<T1, T2, T3> hook in this.Before)
                {
                    hook(arg1, arg2, arg3);
                }
                body(arg1, arg2, arg3);
                foreach (Action<T1, T2, T3> hook in this.After)
                {
                    hook(arg1, arg2, arg3);
                }
            }
            catch (Exception ex)
            {
                if (arg1 is ILoggable)
                {
                    (arg1 as ILoggable).Log.Fatal("Unhandled exception occured.", ex);
                }
                foreach (Action<T1, T2, T3, Exception> hook in this.Failed)
                {
                    hook(arg1, arg2, arg3, ex);
                }
            }
        }

        /// <summary>
        /// フックを適用した、返り値のあるコードを実行します。
        /// </summary>
        /// <param name="body">実行するコード。</param>
        /// <param name="arg1">第 1 引数の値。</param>
        /// <param name="arg2">第 2 引数の値。</param>
        /// <param name="arg3">第 3 引数の値。</param>
        /// <returns><paramref name="body"/> の返り値。</returns>
        public TResult Execute<TResult>(Func<T1, T2, T3, TResult> body, T1 arg1, T2 arg2, T3 arg3)
        {
            try
            {
                foreach (Action<T1, T2, T3> hook in this.Before)
                {
                    hook(arg1, arg2, arg3);
                }
                TResult result = body(arg1, arg2, arg3);
                foreach (Action<T1, T2, T3> hook in this.After)
                {
                    hook(arg1, arg2, arg3);
                }
                return result;
            }
            catch (Exception ex)
            {
                if (arg1 is ILoggable)
                {
                    (arg1 as ILoggable).Log.Fatal("Unhandled exception occured.", ex);
                }
                foreach (Action<T1, T2, T3, Exception> hook in this.Failed)
                {
                    hook(arg1, arg2, arg3, ex);
                }
                return default(TResult);
            }
        }
    }

    /// <summary>
    /// 引数を 4 つ持つフック リストを表します。
    /// </summary>
    /// <typeparam name="T1">第 1 引数の型。</typeparam>
    /// <typeparam name="T2">第 2 引数の型。</typeparam>
    /// <typeparam name="T3">第 3 引数の型。</typeparam>
    /// <typeparam name="T4">第 4 引数の型。</typeparam>
    public class Hook<T1, T2, T3, T4>
        : HookBase<Action<T1, T2, T3, T4>, Action<T1, T2, T3, T4, Exception>>
    {
        /// <summary>
        /// <see cref="Hook{T1, T2, T3, T4}"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public Hook()
        {
            this.Before = new List<Action<T1, T2, T3, T4>>();
            this.After = new List<Action<T1, T2, T3, T4>>();
            this.Failed = new List<Action<T1, T2, T3, T4, Exception>>();
        }

        /// <summary>
        /// フックを適用した、返り値のないコードを実行します。
        /// </summary>
        /// <param name="body">実行するコード。</param>
        /// <param name="arg1">第 1 引数の値。</param>
        /// <param name="arg2">第 2 引数の値。</param>
        /// <param name="arg3">第 3 引数の値。</param>
        /// <param name="arg4">第 4 引数の値。</param>
        public void Execute(Action<T1, T2, T3, T4> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            try
            {
                foreach (Action<T1, T2, T3, T4> hook in this.Before)
                {
                    hook(arg1, arg2, arg3, arg4);
                }
                body(arg1, arg2, arg3, arg4);
                foreach (Action<T1, T2, T3, T4> hook in this.After)
                {
                    hook(arg1, arg2, arg3, arg4);
                }
            }
            catch (Exception ex)
            {
                if (arg1 is ILoggable)
                {
                    (arg1 as ILoggable).Log.Fatal("Unhandled exception occured.", ex);
                }
                foreach (Action<T1, T2, T3, T4, Exception> hook in this.Failed)
                {
                    hook(arg1, arg2, arg3, arg4, ex);
                }
            }
        }

        /// <summary>
        /// フックを適用した、返り値のあるコードを実行します。
        /// </summary>
        /// <param name="body">実行するコード。</param>
        /// <param name="arg1">第 1 引数の値。</param>
        /// <param name="arg2">第 2 引数の値。</param>
        /// <param name="arg3">第 3 引数の値。</param>
        /// <param name="arg4">第 4 引数の値。</param>
        /// <returns><paramref name="body"/> の返り値。</returns>
        public TResult Execute<TResult>(Func<T1, T2, T3, T4, TResult> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            try
            {
                foreach (Action<T1, T2, T3, T4> hook in this.Before)
                {
                    hook(arg1, arg2, arg3, arg4);
                }
                TResult result = body(arg1, arg2, arg3, arg4);
                foreach (Action<T1, T2, T3, T4> hook in this.After)
                {
                    hook(arg1, arg2, arg3, arg4);
                }
                return result;
            }
            catch (Exception ex)
            {
                if (arg1 is ILoggable)
                {
                    (arg1 as ILoggable).Log.Fatal("Unhandled exception occured.", ex);
                }
                foreach (Action<T1, T2, T3, T4, Exception> hook in this.Failed)
                {
                    hook(arg1, arg2, arg3, arg4, ex);
                }
                return default(TResult);
            }
        }
    }

    /// <summary>
    /// 引数を 5 つ持つフック リストを表します。
    /// </summary>
    /// <typeparam name="T1">第 1 引数の型。</typeparam>
    /// <typeparam name="T2">第 2 引数の型。</typeparam>
    /// <typeparam name="T3">第 3 引数の型。</typeparam>
    /// <typeparam name="T4">第 4 引数の型。</typeparam>
    /// <typeparam name="T5">第 5 引数の型。</typeparam>
    public class Hook<T1, T2, T3, T4, T5>
        : HookBase<Action<T1, T2, T3, T4, T5>, Action<T1, T2, T3, T4, T5, Exception>>
    {
        /// <summary>
        /// <see cref="Hook{T1, T2, T3, T4, T5}"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public Hook()
        {
            this.Before = new List<Action<T1, T2, T3, T4, T5>>();
            this.After = new List<Action<T1, T2, T3, T4, T5>>();
            this.Failed = new List<Action<T1, T2, T3, T4, T5, Exception>>();
        }

        /// <summary>
        /// フックを適用した、返り値のないコードを実行します。
        /// </summary>
        /// <param name="body">実行するコード。</param>
        /// <param name="arg1">第 1 引数の値。</param>
        /// <param name="arg2">第 2 引数の値。</param>
        /// <param name="arg3">第 3 引数の値。</param>
        /// <param name="arg4">第 4 引数の値。</param>
        /// <param name="arg5">第 5 引数の値。</param>
        public void Execute(Action<T1, T2, T3, T4, T5> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            try
            {
                foreach (Action<T1, T2, T3, T4, T5> hook in this.Before)
                {
                    hook(arg1, arg2, arg3, arg4, arg5);
                }
                body(arg1, arg2, arg3, arg4, arg5);
                foreach (Action<T1, T2, T3, T4, T5> hook in this.After)
                {
                    hook(arg1, arg2, arg3, arg4, arg5);
                }
            }
            catch (Exception ex)
            {
                if (arg1 is ILoggable)
                {
                    (arg1 as ILoggable).Log.Fatal("Unhandled exception occured.", ex);
                }
                foreach (Action<T1, T2, T3, T4, T5, Exception> hook in this.Failed)
                {
                    hook(arg1, arg2, arg3, arg4, arg5, ex);
                }
            }
        }

        /// <summary>
        /// フックを適用した、返り値のあるコードを実行します。
        /// </summary>
        /// <param name="body">実行するコード。</param>
        /// <param name="arg1">第 1 引数の値。</param>
        /// <param name="arg2">第 2 引数の値。</param>
        /// <param name="arg3">第 3 引数の値。</param>
        /// <param name="arg4">第 4 引数の値。</param>
        /// <param name="arg5">第 5 引数の値。</param>
        /// <returns><paramref name="body"/> の返り値。</returns>
        public TResult Execute<TResult>(Func<T1, T2, T3, T4, T5, TResult> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            try
            {
                foreach (Action<T1, T2, T3, T4, T5> hook in this.Before)
                {
                    hook(arg1, arg2, arg3, arg4, arg5);
                }
                TResult result = body(arg1, arg2, arg3, arg4, arg5);
                foreach (Action<T1, T2, T3, T4, T5> hook in this.After)
                {
                    hook(arg1, arg2, arg3, arg4, arg5);
                }
                return result;
            }
            catch (Exception ex)
            {
                if (arg1 is ILoggable)
                {
                    (arg1 as ILoggable).Log.Fatal("Unhandled exception occured.", ex);
                }
                foreach (Action<T1, T2, T3, T4, T5, Exception> hook in this.Failed)
                {
                    hook(arg1, arg2, arg3, arg4, arg5, ex);
                }
                return default(TResult);
            }
        }
    }

    /// <summary>
    /// 引数を 6 つ持つフック リストを表します。
    /// </summary>
    /// <typeparam name="T1">第 1 引数の型。</typeparam>
    /// <typeparam name="T2">第 2 引数の型。</typeparam>
    /// <typeparam name="T3">第 3 引数の型。</typeparam>
    /// <typeparam name="T4">第 4 引数の型。</typeparam>
    /// <typeparam name="T5">第 5 引数の型。</typeparam>
    /// <typeparam name="T6">第 6 引数の型。</typeparam>
    public class Hook<T1, T2, T3, T4, T5, T6>
        : HookBase<Action<T1, T2, T3, T4, T5, T6>, Action<T1, T2, T3, T4, T5, T6, Exception>>
    {
        /// <summary>
        /// <see cref="Hook{T1, T2, T3, T4, T5, T6}"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public Hook()
        {
            this.Before = new List<Action<T1, T2, T3, T4, T5, T6>>();
            this.After = new List<Action<T1, T2, T3, T4, T5, T6>>();
            this.Failed = new List<Action<T1, T2, T3, T4, T5, T6, Exception>>();
        }

        /// <summary>
        /// フックを適用した、返り値のないコードを実行します。
        /// </summary>
        /// <param name="body">実行するコード。</param>
        /// <param name="arg1">第 1 引数の値。</param>
        /// <param name="arg2">第 2 引数の値。</param>
        /// <param name="arg3">第 3 引数の値。</param>
        /// <param name="arg4">第 4 引数の値。</param>
        /// <param name="arg5">第 5 引数の値。</param>
        /// <param name="arg6">第 6 引数の値。</param>
        public void Execute(Action<T1, T2, T3, T4, T5, T6> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            try
            {
                foreach (Action<T1, T2, T3, T4, T5, T6> hook in this.Before)
                {
                    hook(arg1, arg2, arg3, arg4, arg5, arg6);
                }
                body(arg1, arg2, arg3, arg4, arg5, arg6);
                foreach (Action<T1, T2, T3, T4, T5, T6> hook in this.After)
                {
                    hook(arg1, arg2, arg3, arg4, arg5, arg6);
                }
            }
            catch (Exception ex)
            {
                if (arg1 is ILoggable)
                {
                    (arg1 as ILoggable).Log.Fatal("Unhandled exception occured.", ex);
                }
                foreach (Action<T1, T2, T3, T4, T5, T6, Exception> hook in this.Failed)
                {
                    hook(arg1, arg2, arg3, arg4, arg5, arg6, ex);
                }
            }
        }

        /// <summary>
        /// フックを適用した、返り値のあるコードを実行します。
        /// </summary>
        /// <param name="body">実行するコード。</param>
        /// <param name="arg1">第 1 引数の値。</param>
        /// <param name="arg2">第 2 引数の値。</param>
        /// <param name="arg3">第 3 引数の値。</param>
        /// <param name="arg4">第 4 引数の値。</param>
        /// <param name="arg5">第 5 引数の値。</param>
        /// <param name="arg6">第 6 引数の値。</param>
        /// <returns><paramref name="body"/> の返り値。</returns>
        public TResult Execute<TResult>(Func<T1, T2, T3, T4, T5, T6, TResult> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            try
            {
                foreach (Action<T1, T2, T3, T4, T5, T6> hook in this.Before)
                {
                    hook(arg1, arg2, arg3, arg4, arg5, arg6);
                }
                TResult result = body(arg1, arg2, arg3, arg4, arg5, arg6);
                foreach (Action<T1, T2, T3, T4, T5, T6> hook in this.After)
                {
                    hook(arg1, arg2, arg3, arg4, arg5, arg6);
                }
                return result;
            }
            catch (Exception ex)
            {
                if (arg1 is ILoggable)
                {
                    (arg1 as ILoggable).Log.Fatal("Unhandled exception occured.", ex);
                }
                foreach (Action<T1, T2, T3, T4, T5, T6, Exception> hook in this.Failed)
                {
                    hook(arg1, arg2, arg3, arg4, arg5, arg6, ex);
                }
                return default(TResult);
            }
        }
    }

    /// <summary>
    /// 引数を 7 つ持つフック リストを表します。
    /// </summary>
    /// <typeparam name="T1">第 1 引数の型。</typeparam>
    /// <typeparam name="T2">第 2 引数の型。</typeparam>
    /// <typeparam name="T3">第 3 引数の型。</typeparam>
    /// <typeparam name="T4">第 4 引数の型。</typeparam>
    /// <typeparam name="T5">第 5 引数の型。</typeparam>
    /// <typeparam name="T6">第 6 引数の型。</typeparam>
    /// <typeparam name="T7">第 7 引数の型。</typeparam>
    public class Hook<T1, T2, T3, T4, T5, T6, T7>
        : HookBase<Action<T1, T2, T3, T4, T5, T6, T7>, Action<T1, T2, T3, T4, T5, T6, T7, Exception>>
    {
        /// <summary>
        /// <see cref="Hook{T1, T2, T3, T4, T5, T6, T7}"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public Hook()
        {
            this.Before = new List<Action<T1, T2, T3, T4, T5, T6, T7>>();
            this.After = new List<Action<T1, T2, T3, T4, T5, T6, T7>>();
            this.Failed = new List<Action<T1, T2, T3, T4, T5, T6, T7, Exception>>();
        }

        /// <summary>
        /// フックを適用した、返り値のないコードを実行します。
        /// </summary>
        /// <param name="body">実行するコード。</param>
        /// <param name="arg1">第 1 引数の値。</param>
        /// <param name="arg2">第 2 引数の値。</param>
        /// <param name="arg3">第 3 引数の値。</param>
        /// <param name="arg4">第 4 引数の値。</param>
        /// <param name="arg5">第 5 引数の値。</param>
        /// <param name="arg6">第 6 引数の値。</param>
        /// <param name="arg7">第 7 引数の値。</param>
        public void Execute(Action<T1, T2, T3, T4, T5, T6, T7> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            try
            {
                foreach (Action<T1, T2, T3, T4, T5, T6, T7> hook in this.Before)
                {
                    hook(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
                }
                body(arg1, arg2, arg3, arg4, arg5, arg6,arg7);
                foreach (Action<T1, T2, T3, T4, T5, T6, T7> hook in this.After)
                {
                    hook(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
                }
            }
            catch (Exception ex)
            {
                if (arg1 is ILoggable)
                {
                    (arg1 as ILoggable).Log.Fatal("Unhandled exception occured.", ex);
                }
                foreach (Action<T1, T2, T3, T4, T5, T6, T7, Exception> hook in this.Failed)
                {
                    hook(arg1, arg2, arg3, arg4, arg5, arg6, arg7, ex);
                }
            }
        }

        /// <summary>
        /// フックを適用した、返り値のあるコードを実行します。
        /// </summary>
        /// <param name="body">実行するコード。</param>
        /// <param name="arg1">第 1 引数の値。</param>
        /// <param name="arg2">第 2 引数の値。</param>
        /// <param name="arg3">第 3 引数の値。</param>
        /// <param name="arg4">第 4 引数の値。</param>
        /// <param name="arg5">第 5 引数の値。</param>
        /// <param name="arg6">第 6 引数の値。</param>
        /// <param name="arg7">第 7 引数の値。</param>
        /// <returns><paramref name="body"/> の返り値。</returns>
        public TResult Execute<TResult>(Func<T1, T2, T3, T4, T5, T6, T7, TResult> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            try
            {
                foreach (Action<T1, T2, T3, T4, T5, T6, T7> hook in this.Before)
                {
                    hook(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
                }
                TResult result = body(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
                foreach (Action<T1, T2, T3, T4, T5, T6, T7> hook in this.After)
                {
                    hook(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
                }
                return result;
            }
            catch (Exception ex)
            {
                if (arg1 is ILoggable)
                {
                    (arg1 as ILoggable).Log.Fatal("Unhandled exception occured.", ex);
                }
                foreach (Action<T1, T2, T3, T4, T5, T6, T7, Exception> hook in this.Failed)
                {
                    hook(arg1, arg2, arg3, arg4, arg5, arg6, arg7, ex);
                }
                return default(TResult);
            }
        }
    }

}