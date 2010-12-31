// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetObjectModel
 *   Object model and Storage interface for MetaTweet and other systems
 *   Part of MetaTweet
 * Copyright © 2008-2011 Takeshi KIRIYA (aka takeshik) <takeshik@users.sf.net>
 * All rights reserved.
 * 
 * This file is part of MetaTweetObjectModel.
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

namespace XSpect.MetaTweet.Objects
{
    // Copied from Linx (licensed under the MIT license), re-license under the
    // GNU LGPL version 3 or later.

    internal class TransparentEnumerable<T>
        : MarshalByRefObject,
          IEnumerable<T>
    {
        private readonly IEnumerable<T> _enumerable;

        public TransparentEnumerable(IEnumerable<T> enumerable)
        {
            this._enumerable = enumerable;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new TransparentEnumerator<T>(this._enumerable.GetEnumerator());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    internal class TransparentEnumerator<T>
        : MarshalByRefObject,
          IEnumerator<T>
    {
        private readonly IEnumerator<T> _enumerator;

        public TransparentEnumerator(IEnumerator<T> enumerator)
        {
            this._enumerator = enumerator;
        }

        public void Dispose()
        {
            this._enumerator.Dispose();
        }

        public Boolean MoveNext()
        {
            return this._enumerator.MoveNext();
        }

        public void Reset()
        {
            this._enumerator.Reset();
        }

        public T Current
        {
            get
            {
                return this._enumerator.Current;
            }
        }

        Object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }
    }

    internal static class TransparencyExtension
    {
        public static IEnumerable<TSource> AsTransparent<TSource>(this IEnumerable<TSource> source)
        {
            return new TransparentEnumerable<TSource>(source);
        }
    }
}