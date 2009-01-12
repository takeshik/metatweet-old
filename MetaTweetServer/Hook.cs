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
        private readonly List<Action> _before = new List<Action>();

        private readonly List<Action> _after = new List<Action>();
        
        private readonly List<Action<Exception>> _failed = new List<Action<Exception>>();

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

    public class Hook<T>
        : HookBase<Action<T>, Action<T, Exception>>
    {
        private readonly List<Action<T>> _before = new List<Action<T>>();

        private readonly List<Action<T>> _after = new List<Action<T>>();

        private readonly List<Action<T, Exception>> _failed = new List<Action<T, Exception>>();

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
                foreach (Action<T, Exception> hook in this.Failed)
                {
                    hook(arg, ex);
                }
            }
        }

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
                foreach (Action<T, Exception> hook in this.Failed)
                {
                    hook(arg, ex);
                }
                return default(TResult);
            }
        }
    }

    public class Hook<T1, T2>
        : HookBase<Action<T1, T2>, Action<T1, T2, Exception>>
    {
        private readonly List<Action<T1, T2>> _before = new List<Action<T1, T2>>();

        private readonly List<Action<T1, T2>> _after = new List<Action<T1, T2>>();

        private readonly List<Action<T1, T2, Exception>> _failed = new List<Action<T1, T2, Exception>>();

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
                foreach (Action<T1, T2, Exception> hook in this.Failed)
                {
                    hook(arg1, arg2, ex);
                }
            }
        }

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
                foreach (Action<T1, T2, Exception> hook in this.Failed)
                {
                    hook(arg1, arg2, ex);
                }
                return default(TResult);
            }
        }
    }

    public class Hook<T1, T2, T3>
        : HookBase<Action<T1, T2, T3>, Action<T1, T2, T3, Exception>>
    {
        private readonly List<Action<T1, T2, T3>> _before = new List<Action<T1, T2, T3>>();

        private readonly List<Action<T1, T2, T3>> _after = new List<Action<T1, T2, T3>>();

        private readonly List<Action<T1, T2, T3, Exception>> _failed = new List<Action<T1, T2, T3, Exception>>();

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
                foreach (Action<T1, T2, T3, Exception> hook in this.Failed)
                {
                    hook(arg1, arg2, arg3, ex);
                }
            }
        }

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
                foreach (Action<T1, T2, T3, Exception> hook in this.Failed)
                {
                    hook(arg1, arg2, arg3, ex);
                }
                return default(TResult);
            }
        }
    }

    public class Hook<T1, T2, T3, T4>
        : HookBase<Action<T1, T2, T3, T4>, Action<T1, T2, T3, T4, Exception>>
    {
        private readonly List<Action<T1, T2, T3, T4>> _before = new List<Action<T1, T2, T3, T4>>();

        private readonly List<Action<T1, T2, T3, T4>> _after = new List<Action<T1, T2, T3, T4>>();

        private readonly List<Action<T1, T2, T3, T4, Exception>> _failed = new List<Action<T1, T2, T3, T4, Exception>>();

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
                foreach (Action<T1, T2, T3, T4, Exception> hook in this.Failed)
                {
                    hook(arg1, arg2, arg3, arg4, ex);
                }
            }
        }

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
                foreach (Action<T1, T2, T3, T4, Exception> hook in this.Failed)
                {
                    hook(arg1, arg2, arg3, arg4, ex);
                }
                return default(TResult);
            }
        }
    }

}