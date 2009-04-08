// -*- mode: csharp; encoding: utf-8; -*-
/* XSpect Common Framework - Generic utility class library
 * Copyright © 2008-2009 Takeshi KIRIYA, XSpect Project <takeshik@users.sf.net>
 * All rights reserved.
 * 
 * This file is part of XSpect Common Framework.
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
using System.Linq;
using System.IO;
using Achiral.Extension;
using Achiral;
using System.Linq.Expressions;

namespace XSpect.Extension.Dynamic
{
    public static class Ops
    {
        private static Func<TLeft, TRight, TResult> Lambda<TLeft, TRight, TResult>(Func<ParameterExpression, ParameterExpression, BinaryExpression> op)
        {
            ParameterExpression lvalue = Expression.Parameter(typeof(TLeft), "lvalue");
            ParameterExpression rvalue = Expression.Parameter(typeof(TRight), "rvalue");

            return Expression.Lambda<Func<TLeft, TRight, TResult>>(op(lvalue, rvalue), lvalue, rvalue).Compile();
        }

        private static Func<TLeft, TRight, TLeft> Lambda<TLeft, TRight>(Func<ParameterExpression, ParameterExpression, BinaryExpression> op)
        {
            return Lambda<TLeft, TRight, TLeft>(op);
        }

        private static Func<TOperand, TOperand> Lambda<TOperand>(Func<ParameterExpression, UnaryExpression> op)
        {
            ParameterExpression operand = Expression.Parameter(typeof(TOperand), "operand");

            return Expression.Lambda<Func<TOperand, TOperand>>(op(operand), operand).Compile();
        }

        public static TLeft Add<TLeft, TRight>(this TLeft lvalue, TRight rvalue)
        {
            return Lambda<TLeft, TRight>(Expression.Add)(lvalue, rvalue);
        }

        public static TLeft Subtract<TLeft, TRight>(this TLeft lvalue, TRight rvalue)
        {
            return Lambda<TLeft, TRight>(Expression.Subtract)(lvalue, rvalue);
        }

        public static TLeft Sub<TLeft, TRight>(this TLeft lvalue, TRight rvalue)
        {
            return lvalue.Subtract(rvalue);
        }

        public static TLeft Multiply<TLeft, TRight>(this TLeft lvalue, TRight rvalue)
        {
            return Lambda<TLeft, TRight>(Expression.Multiply)(lvalue, rvalue);
        }

        public static TLeft Mul<TLeft, TRight>(this TLeft lvalue, TRight rvalue)
        {
            return lvalue.Multiply(rvalue);
        }

        public static TLeft Divide<TLeft, TRight>(this TLeft lvalue, TRight rvalue)
        {
            return Lambda<TLeft, TRight>(Expression.Divide)(lvalue, rvalue);
        }

        public static TLeft Div<TLeft, TRight>(this TLeft lvalue, TRight rvalue)
        {
            return lvalue.Divide(rvalue);
        }

        public static TLeft Modulo<TLeft, TRight>(this TLeft lvalue, TRight rvalue)
        {
            return Lambda<TLeft, TRight>(Expression.Modulo)(lvalue, rvalue);
        }

        public static TLeft Mod<TLeft, TRight>(this TLeft lvalue, TRight rvalue)
        {
            return lvalue.Modulo(rvalue);
        }

        public static TLeft And<TLeft, TRight>(this TLeft lvalue, TRight rvalue)
        {
            return Lambda<TLeft, TRight>(Expression.And)(lvalue, rvalue);
        }

        public static TLeft AndAlso<TLeft, TRight>(this TLeft lvalue, TRight rvalue)
        {
            return Lambda<TLeft, TRight>(Expression.AndAlso)(lvalue, rvalue);
        }

        public static TLeft Or<TLeft, TRight>(this TLeft lvalue, TRight rvalue)
        {
            return Lambda<TLeft, TRight>(Expression.Or)(lvalue, rvalue);
        }

        public static TLeft OrElse<TLeft, TRight>(this TLeft lvalue, TRight rvalue)
        {
            return Lambda<TLeft, TRight>(Expression.OrElse)(lvalue, rvalue);
        }

        public static TLeft ExclusiveOr<TLeft, TRight>(this TLeft lvalue, TRight rvalue)
        {
            return Lambda<TLeft, TRight>(Expression.ExclusiveOr)(lvalue, rvalue);
        }

        public static TLeft Xor<TLeft, TRight>(this TLeft lvalue, TRight rvalue)
        {
            return lvalue.ExclusiveOr(rvalue);
        }

        public static TLeft LeftShift<TLeft, TRight>(this TLeft lvalue, TRight rvalue)
        {
            return Lambda<TLeft, TRight>(Expression.LeftShift)(lvalue, rvalue);
        }

        public static TLeft Lsh<TLeft, TRight>(this TLeft lvalue, TRight rvalue)
        {
            return lvalue.LeftShift(rvalue);
        }

        public static TLeft RightShift<TLeft, TRight>(this TLeft lvalue, TRight rvalue)
        {
            return Lambda<TLeft, TRight>(Expression.RightShift)(lvalue, rvalue);
        }

        public static TLeft Rsh<TLeft, TRight>(this TLeft lvalue, TRight rvalue)
        {
            return lvalue.RightShift(rvalue);
        }

        public static TOperand Plus<TOperand>(this TOperand value)
        {
            return Lambda<TOperand>(Expression.UnaryPlus)(value);
        }

        public static TOperand Negate<TOperand>(this TOperand value)
        {
            return Lambda<TOperand>(Expression.Negate)(value);
        }

        public static TOperand Neg<TOperand>(this TOperand value)
        {
            return value.Negate();
        }

        public static TOperand Not<TOperand>(this TOperand value)
        {
            return Lambda<TOperand>(Expression.Not)(value);
        }

        public static Boolean Equal<TLeft, TRight>(this TLeft lvalue, TRight rvalue)
        {
            return Lambda<TLeft, TRight, Boolean>(Expression.Equal)(lvalue, rvalue);
        }

        public static Boolean Eq<TLeft, TRight>(this TLeft lvalue, TRight rvalue)
        {
            return lvalue.Equal(rvalue);
        }

        public static Boolean NotEqual<TLeft, TRight>(this TLeft lvalue, TRight rvalue)
        {
            return Lambda<TLeft, TRight, Boolean>(Expression.NotEqual)(lvalue, rvalue);
        }

        public static Boolean Ne<TLeft, TRight>(this TLeft lvalue, TRight rvalue)
        {
            return lvalue.NotEqual(rvalue);
        }

        public static Boolean LessThan<TLeft, TRight>(this TLeft lvalue, TRight rvalue)
        {
            return Lambda<TLeft, TRight, Boolean>(Expression.LessThan)(lvalue, rvalue);
        }

        public static Boolean Lt<TLeft, TRight>(this TLeft lvalue, TRight rvalue)
        {
            return lvalue.LessThan(rvalue);
        }

        public static Boolean LessThanOrEqual<TLeft, TRight>(this TLeft lvalue, TRight rvalue)
        {
            return Lambda<TLeft, TRight, Boolean>(Expression.LessThanOrEqual)(lvalue, rvalue);
        }

        public static Boolean Le<TLeft, TRight>(this TLeft lvalue, TRight rvalue)
        {
            return lvalue.LessThanOrEqual(rvalue);
        }

        public static Boolean GreaterThan<TLeft, TRight>(this TLeft lvalue, TRight rvalue)
        {
            return Lambda<TLeft, TRight, Boolean>(Expression.GreaterThan)(lvalue, rvalue);
        }

        public static Boolean Gt<TLeft, TRight>(this TLeft lvalue, TRight rvalue)
        {
            return lvalue.GreaterThan(rvalue);
        }

        public static Boolean GreaterThanOrEqual<TLeft, TRight>(this TLeft lvalue, TRight rvalue)
        {
            return Lambda<TLeft, TRight, Boolean>(Expression.GreaterThanOrEqual)(lvalue, rvalue);
        }

        public static Boolean Ge<TLeft, TRight>(this TLeft lvalue, TRight rvalue)
        {
            return lvalue.GreaterThanOrEqual(rvalue);
        }
    }
}