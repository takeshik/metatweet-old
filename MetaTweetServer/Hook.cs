// -*- mode: csharp; encoding: utf-8; -*-
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetServer
 *   Server library of MetaTweet
 *   Part of MetaTweet
 * Copyright © 2008-2009 Takeshi KIRIYA, XSpect Project <takeshik@xspect.org>
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

namespace XSpect.MetaTweet
{
    public class Hook
        : HookBase<Action, Action<Exception>>
    {
        private readonly List<Action> _before;

        private readonly List<Action> _after;

        private readonly List<Action<Exception>> _failed;

        public override IList<Action> Before
        {
            get
            {
                return this._before;
            }
        }

        public override IList<Action> After
        {
            get
            {
                return this._after;
            }
        }

        public override IList<Action<Exception>> Failed
        {
            get
            {
                return this._failed;
            }
        }

        public Hook()
        {
            this._before = new List<Action>();
            this._after = new List<Action>();
            this._failed = new List<Action<Exception>>();
        }

        public void Execute(Action body)
        {
#if !DEBUG
            try
            {
#endif
                foreach (Action hook in this.Before)
                {
                    hook();
                }
                body();
                foreach (Action hook in this.After)
                {
                    hook();
                }
#if !DEBUG
            }

            catch (Exception ex)
            {
                foreach (Action<Exception> hook in this.Failed)
                {
                    hook(ex);
                }
            }
#endif
        }

        public TResult Execute<TResult>(Func<TResult> body)
        {
#if !DEBUG
            try
            {
#endif
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
#if !DEBUG
            }

            catch (Exception ex)
            {
                foreach (Action<Exception> hook in this.Failed)
                {
                    hook(ex);
                }
                return default(TResult);
            }
#endif
        }
    }

    public class Hook<T>
        : HookBase<Action<T>, Action<T, Exception>>
    {
        private readonly List<Action<T>> _before;

        private readonly List<Action<T>> _after;

        private readonly List<Action<T, Exception>> _failed;

        public override IList<Action<T>> Before
        {
            get
            {
                return this._before;
            }
        }

        public override IList<Action<T>> After
        {
            get
            {
                return this._after;
            }
        }

        public override IList<Action<T, Exception>> Failed
        {
            get
            {
                return this._failed;
            }
        }

        public Hook()
        {
            this._before = new List<Action<T>>();
            this._after = new List<Action<T>>();
            this._failed = new List<Action<T, Exception>>();
        }

        public void Execute(Action<T> body, T arg)
        {
#if !DEBUG
            try
            {
#endif
                foreach (Action<T> hook in this.Before)
                {
                    hook(arg);
                }
                body(arg);
                foreach (Action<T> hook in this.After)
                {
                    hook(arg);
                }
#if !DEBUG
            }

            catch (Exception ex)
            {
                foreach (Action<T, Exception> hook in this.Failed)
                {
                    hook(arg, ex);
                }
            }
#endif
        }

        public TResult Execute<TResult>(Func<T, TResult> body, T arg)
        {
#if !DEBUG
            try
            {
#endif
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
#if !DEBUG
            }

            catch (Exception ex)
            {
                foreach (Action<T, Exception> hook in this.Failed)
                {
                    hook(arg, ex);
                }
                return default(TResult);
            }
#endif
        }
    }

    public class Hook<T1, T2>
        : HookBase<Action<T1, T2>, Action<T1, T2, Exception>>
    {
        private readonly List<Action<T1, T2>> _before;

        private readonly List<Action<T1, T2>> _after;

        private readonly List<Action<T1, T2, Exception>> _failed;

        public override IList<Action<T1, T2>> Before
        {
            get
            {
                return this._before;
            }
        }

        public override IList<Action<T1, T2>> After
        {
            get
            {
                return this._after;
            }
        }

        public override IList<Action<T1, T2, Exception>> Failed
        {
            get
            {
                return this._failed;
            }
        }

        public Hook()
        {
            this._before = new List<Action<T1, T2>>();
            this._after = new List<Action<T1, T2>>();
            this._failed = new List<Action<T1, T2, Exception>>();
        }

        public void Execute(Action<T1, T2> body, T1 arg1, T2 arg2)
        {
#if !DEBUG
            try
            {
#endif
                foreach (Action<T1, T2> hook in this.Before)
                {
                    hook(arg1, arg2);
                }
                body(arg1, arg2);
                foreach (Action<T1, T2> hook in this.After)
                {
                    hook(arg1, arg2);
                }
#if !DEBUG
            }

            catch (Exception ex)
            {
                foreach (Action<T1, T2, Exception> hook in this.Failed)
                {
                    hook(arg1, arg2, ex);
                }
            }
#endif
        }

        public TResult Execute<TResult>(Func<T1, T2, TResult> body, T1 arg1, T2 arg2)
        {
#if !DEBUG
            try
            {
#endif
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
#if !DEBUG
            }

            catch (Exception ex)
            {
                foreach (Action<T1, T2, Exception> hook in this.Failed)
                {
                    hook(arg1, arg2, ex);
                }
                return default(TResult);
            }
#endif
        }
    }

    public class Hook<T1, T2, T3>
        : HookBase<Action<T1, T2, T3>, Action<T1, T2, T3, Exception>>
    {
        private readonly List<Action<T1, T2, T3>> _before;

        private readonly List<Action<T1, T2, T3>> _after;

        private readonly List<Action<T1, T2, T3, Exception>> _failed;

        public override IList<Action<T1, T2, T3>> Before
        {
            get
            {
                return this._before;
            }
        }

        public override IList<Action<T1, T2, T3>> After
        {
            get
            {
                return this._after;
            }
        }

        public override IList<Action<T1, T2, T3, Exception>> Failed
        {
            get
            {
                return this._failed;
            }
        }

        public Hook()
        {
            this._before = new List<Action<T1, T2, T3>>();
            this._after = new List<Action<T1, T2, T3>>();
            this._failed = new List<Action<T1, T2, T3, Exception>>();
        }

        public void Execute(Action<T1, T2, T3> body, T1 arg1, T2 arg2, T3 arg3)
        {
#if !DEBUG
            try
            {
#endif
                foreach (Action<T1, T2, T3> hook in this.Before)
                {
                    hook(arg1, arg2, arg3);
                }
                body(arg1, arg2, arg3);
                foreach (Action<T1, T2, T3> hook in this.After)
                {
                    hook(arg1, arg2, arg3);
                }
#if !DEBUG
            }

            catch (Exception ex)
            {
                foreach (Action<T1, T2, T3, Exception> hook in this.Failed)
                {
                    hook(arg1, arg2, arg3, ex);
                }
            }
#endif
        }

        public TResult Execute<TResult>(Func<T1, T2, T3, TResult> body, T1 arg1, T2 arg2, T3 arg3)
        {
#if !DEBUG
            try
            {
#endif
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
#if !DEBUG
            }

            catch (Exception ex)
            {
                foreach (Action<T1, T2, T3, Exception> hook in this.Failed)
                {
                    hook(arg1, arg2, arg3, ex);
                }
                return default(TResult);
            }
#endif
        }
    }

    public class Hook<T1, T2, T3, T4>
        : HookBase<Action<T1, T2, T3, T4>, Action<T1, T2, T3, T4, Exception>>
    {
        private readonly List<Action<T1, T2, T3, T4>> _before;

        private readonly List<Action<T1, T2, T3, T4>> _after;

        private readonly List<Action<T1, T2, T3, T4, Exception>> _failed;

        public override IList<Action<T1, T2, T3, T4>> Before
        {
            get
            {
                return this._before;
            }
        }

        public override IList<Action<T1, T2, T3, T4>> After
        {
            get
            {
                return this._after;
            }
        }

        public override IList<Action<T1, T2, T3, T4, Exception>> Failed
        {
            get
            {
                return this._failed;
            }
        }

        public Hook()
        {
            this._before = new List<Action<T1, T2, T3, T4>>();
            this._after = new List<Action<T1, T2, T3, T4>>();
            this._failed = new List<Action<T1, T2, T3, T4, Exception>>();
        }

        public void Execute(Action<T1, T2, T3, T4> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
#if !DEBUG
            try
            {
#endif
                foreach (Action<T1, T2, T3, T4> hook in this.Before)
                {
                    hook(arg1, arg2, arg3, arg4);
                }
                body(arg1, arg2, arg3, arg4);
                foreach (Action<T1, T2, T3, T4> hook in this.After)
                {
                    hook(arg1, arg2, arg3, arg4);
                }
#if !DEBUG
            }

            catch (Exception ex)
            {
                foreach (Action<T1, T2, T3, T4, Exception> hook in this.Failed)
                {
                    hook(arg1, arg2, arg3, arg4, ex);
                }
            }
#endif
        }

        public TResult Execute<TResult>(Func<T1, T2, T3, T4, TResult> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
#if !DEBUG
            try
            {
#endif
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
#if !DEBUG
            }

            catch (Exception ex)
            {
                foreach (Action<T1, T2, T3, T4, Exception> hook in this.Failed)
                {
                    hook(arg1, arg2, arg3, arg4, ex);
                }
                return default(TResult);
            }
#endif
        }
    }

    public class Hook<T1, T2, T3, T4, T5>
    : HookBase<Action<T1, T2, T3, T4, T5>, Action<T1, T2, T3, T4, T5, Exception>>
    {
        private readonly List<Action<T1, T2, T3, T4, T5>> _before;

        private readonly List<Action<T1, T2, T3, T4, T5>> _after;

        private readonly List<Action<T1, T2, T3, T4, T5, Exception>> _failed;

        public override IList<Action<T1, T2, T3, T4, T5>> Before
        {
            get
            {
                return this._before;
            }
        }

        public override IList<Action<T1, T2, T3, T4, T5>> After
        {
            get
            {
                return this._after;
            }
        }

        public override IList<Action<T1, T2, T3, T4, T5, Exception>> Failed
        {
            get
            {
                return this._failed;
            }
        }

        public Hook()
        {
            this._before = new List<Action<T1, T2, T3, T4, T5>>();
            this._after = new List<Action<T1, T2, T3, T4, T5>>();
            this._failed = new List<Action<T1, T2, T3, T4, T5, Exception>>();
        }

        public void Execute(Action<T1, T2, T3, T4, T5> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
#if !DEBUG
            try
            {
#endif
                foreach (Action<T1, T2, T3, T4, T5> hook in this.Before)
                {
                    hook(arg1, arg2, arg3, arg4, arg5);
                }
                body(arg1, arg2, arg3, arg4, arg5);
                foreach (Action<T1, T2, T3, T4, T5> hook in this.After)
                {
                    hook(arg1, arg2, arg3, arg4, arg5);
                }
#if !DEBUG
            }

            catch (Exception ex)
            {
                foreach (Action<T1, T2, T3, T4, T5, Exception> hook in this.Failed)
                {
                    hook(arg1, arg2, arg3, arg4, arg5, ex);
                }
            }
#endif
        }

        public TResult Execute<TResult>(Func<T1, T2, T3, T4, T5, TResult> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
#if !DEBUG
            try
            {
#endif
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
#if !DEBUG
            }

            catch (Exception ex)
            {
                foreach (Action<T1, T2, T3, T4, T5, Exception> hook in this.Failed)
                {
                    hook(arg1, arg2, arg3, arg4, arg5, ex);
                }
                return default(TResult);
            }
#endif
        }
    }
}