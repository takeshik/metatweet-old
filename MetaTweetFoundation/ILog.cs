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

namespace XSpect.MetaTweet
{
    /// <summary>
    /// ログ出力の機能を提供します。
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// このオブジェクトを保持する <see cref="ILogManager"/> オブジェクトを取得します。
        /// </summary>
        /// <value>
        /// このオブジェクトを保持する <see cref="ILogManager"/> オブジェクト。
        /// </value>
        ILogManager Parent
        {
            get;
        }

        /// <summary>
        /// Verbose レベル (レベル 10000) のログを出力します。
        /// </summary>
        /// <param name="format">出力するメッセージを表す書式指定文字列。</param>
        /// <param name="args"><paramref name="format"/> を使用して書き込むオブジェクトの配列。</param>
        void Verbose(String format, params Object[] args);

        /// <summary>
        /// Verbose レベル (レベル 10000) のログを出力します。
        /// </summary>
        /// <param name="message">出力するメッセージ。</param>
        /// <param name="exception">情報を出力する例外。</param>
        void Verbose(String message, Exception exception);

        /// <summary>
        /// Trace レベル (レベル 20000) のログを出力します。
        /// </summary>
        /// <param name="format">出力するメッセージを表す書式指定文字列。</param>
        /// <param name="args"><paramref name="format"/> を使用して書き込むオブジェクトの配列。</param>
        void Trace(String format, params Object[] args);

        /// <summary>
        /// Trace レベル (レベル 20000) のログを出力します。
        /// </summary>
        /// <param name="message">出力するメッセージ。</param>
        /// <param name="exception">情報を出力する例外。</param>
        void Trace(String message, Exception exception);

        /// <summary>
        /// Debug レベル (レベル 30000) のログを出力します。
        /// </summary>
        /// <param name="format">出力するメッセージを表す書式指定文字列。</param>
        /// <param name="args"><paramref name="format"/> を使用して書き込むオブジェクトの配列。</param>
        void Debug(String format, params Object[] args);

        /// <summary>
        /// Debug レベル (レベル 30000) のログを出力します。
        /// </summary>
        /// <param name="message">出力するメッセージ。</param>
        /// <param name="exception">情報を出力する例外。</param>
        void Debug(String message, Exception exception);

        /// <summary>
        /// Info レベル (レベル 40000) のログを出力します。
        /// </summary>
        /// <param name="format">出力するメッセージを表す書式指定文字列。</param>
        /// <param name="args"><paramref name="format"/> を使用して書き込むオブジェクトの配列。</param>
        void Info(String format, params Object[] args);

        /// <summary>
        /// Info レベル (レベル 40000) のログを出力します。
        /// </summary>
        /// <param name="message">出力するメッセージ。</param>
        /// <param name="exception">情報を出力する例外。</param>
        void Info(String message, Exception exception);

        /// <summary>
        /// Notice レベル (レベル 50000) のログを出力します。
        /// </summary>
        /// <param name="format">出力するメッセージを表す書式指定文字列。</param>
        /// <param name="args"><paramref name="format"/> を使用して書き込むオブジェクトの配列。</param>
        void Notice(String format, params Object[] args);

        /// <summary>
        /// Notice レベル (レベル 50000) のログを出力します。
        /// </summary>
        /// <param name="message">出力するメッセージ。</param>
        /// <param name="exception">情報を出力する例外。</param>
        void Notice(String message, Exception exception);

        /// <summary>
        /// Warn レベル (レベル 60000) のログを出力します。
        /// </summary>
        /// <param name="format">出力するメッセージを表す書式指定文字列。</param>
        /// <param name="args"><paramref name="format"/> を使用して書き込むオブジェクトの配列。</param>
        void Warn(String format, params Object[] args);

        /// <summary>
        /// Warn レベル (レベル 60000) のログを出力します。
        /// </summary>
        /// <param name="message">出力するメッセージ。</param>
        /// <param name="exception">情報を出力する例外。</param>
        void Warn(String message, Exception exception);

        /// <summary>
        /// Error レベル (レベル 70000) のログを出力します。
        /// </summary>
        /// <param name="format">出力するメッセージを表す書式指定文字列。</param>
        /// <param name="args"><paramref name="format"/> を使用して書き込むオブジェクトの配列。</param>
        void Error(String format, params Object[] args);

        /// <summary>
        /// Error レベル (レベル 70000) のログを出力します。
        /// </summary>
        /// <param name="message">出力するメッセージ。</param>
        /// <param name="exception">情報を出力する例外。</param>
        void Error(String message, Exception exception);

        /// <summary>
        /// Severe レベル (レベル 80000) のログを出力します。
        /// </summary>
        /// <param name="format">出力するメッセージを表す書式指定文字列。</param>
        /// <param name="args"><paramref name="format"/> を使用して書き込むオブジェクトの配列。</param>
        void Severe(String format, params Object[] args);

        /// <summary>
        /// Severe レベル (レベル 80000) のログを出力します。
        /// </summary>
        /// <param name="message">出力するメッセージ。</param>
        /// <param name="exception">情報を出力する例外。</param>
        void Severe(String message, Exception exception);

        /// <summary>
        /// Critical レベル (レベル 90000) のログを出力します。
        /// </summary>
        /// <param name="format">出力するメッセージを表す書式指定文字列。</param>
        /// <param name="args"><paramref name="format"/> を使用して書き込むオブジェクトの配列。</param>
        void Critical(String format, params Object[] args);

        /// <summary>
        /// Critical レベル (レベル 90000) のログを出力します。
        /// </summary>
        /// <param name="message">出力するメッセージ。</param>
        /// <param name="exception">情報を出力する例外。</param>
        void Critical(String message, Exception exception);

        /// <summary>
        /// Alert レベル (レベル 100000) のログを出力します。
        /// </summary>
        /// <param name="format">出力するメッセージを表す書式指定文字列。</param>
        /// <param name="args"><paramref name="format"/> を使用して書き込むオブジェクトの配列。</param>
        void Alert(String format, params Object[] args);

        /// <summary>
        /// Alert レベル (レベル 100000) のログを出力します。
        /// </summary>
        /// <param name="message">出力するメッセージ。</param>
        /// <param name="exception">情報を出力する例外。</param>
        void Alert(String message, Exception exception);

        /// <summary>
        /// Fatal レベル (レベル 110000) のログを出力します。
        /// </summary>
        /// <param name="format">出力するメッセージを表す書式指定文字列。</param>
        /// <param name="args"><paramref name="format"/> を使用して書き込むオブジェクトの配列。</param>
        void Fatal(String format, params Object[] args);

        /// <summary>
        /// Fatal レベル (レベル 110000) のログを出力します。
        /// </summary>
        /// <param name="message">出力するメッセージ。</param>
        /// <param name="exception">情報を出力する例外。</param>
        void Fatal(String message, Exception exception);

        /// <summary>
        /// Emergency レベル (レベル 120000) のログを出力します。
        /// </summary>
        /// <param name="format">出力するメッセージを表す書式指定文字列。</param>
        /// <param name="args"><paramref name="format"/> を使用して書き込むオブジェクトの配列。</param>
        void Emergency(String format, params Object[] args);

        /// <summary>
        /// Emergency レベル (レベル 120000) のログを出力します。
        /// </summary>
        /// <param name="message">出力するメッセージ。</param>
        /// <param name="exception">情報を出力する例外。</param>
        void Emergency(String message, Exception exception);
    }
}