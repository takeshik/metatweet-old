// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetObjectModel
 *   Object model and Storage interface for MetaTweet and other systems
 *   Part of MetaTweet
 * Copyright © 2008-2009 Takeshi KIRIYA, XSpect Project <takeshik@users.sf.net>
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
using System.Collections.Generic;

namespace XSpect.MetaTweet.Objects
{
    /// <summary>
    /// エンティティ モデルに依存しないアカウントの基本実装を表します。
    /// </summary>
    public interface IAccount
        : IComparable<IAccount>,
          IEquatable<IAccount>
    {
        /// <summary>
        /// このアカウントによる、指定されたカテゴリの最新のアクティビティを取得します。
        /// </summary>
        /// <param name="category">取得するアクティビティのカテゴリ。</param>
        /// <returns>このアカウントによる、指定されたカテゴリの最新のアクティビティ。</returns>
        IActivity this[String category]
        {
            get;
        }

        /// <summary>
        /// このアカウントによる、指定されたカテゴリの、指定した時点で最新のアクティビティを取得します。
        /// </summary>
        /// <param name="category">取得するアクティビティのカテゴリ。</param>
        /// <param name="baseline">取得するアクティビティのタイムスタンプの上限。</param>
        /// <returns>このアカウントによる、指定されたカテゴリの、指定した時点で最新のアクティビティ。</returns>
        IActivity this[String category, DateTime baseline]
        {
            get;
        }

        /// <summary>
        /// このアカウントを一意に識別するグローバル一意識別子 (GUID) 値を取得または設定します。
        /// </summary>
        /// <value>
        /// このアカウントを一意に識別するグローバル一意識別子 (GUID) 値。
        /// </value>
        Guid AccountId
        {
            get;
            set;
        }

        /// <summary>
        /// このアカウントに関連付けられているサービスを表す文字列を取得または設定します。
        /// </summary>
        /// <value>
        /// このアカウントに関連付けられているサービスを表す文字列を取得または設定します。
        /// </value>
        String Realm
        {
            get;
            set;
        }

        /// <summary>
        /// このアカウントによって行われたアクティビティのシーケンスを取得します。
        /// </summary>
        /// <value>
        /// このアカウントによって行われたアクティビティのシーケンス。
        /// </value>
        IEnumerable<IActivity> Activities
        {
            get;
        }

        /// <summary>
        /// このアカウントに関連付けられたアノテーションのシーケンスを取得します。
        /// </summary>
        /// <value>
        /// このアカウントに関連付けられたアノテーションのシーケンス。
        /// </value>
        IEnumerable<IAnnotation> Annotations
        {
            get;
        }

        /// <summary>
        /// このアカウントに関連付けられたアノテーションの意味となる文字列のシーケンスを取得します。
        /// </summary>
        /// <value>
        /// このアカウントに関連付けられたアノテーションの意味となる文字列のシーケンス。
        /// </value>
        IEnumerable<String> Annotating
        {
            get;
        }

        /// <summary>
        /// このアカウントに関連付けられたリレーションのシーケンスを取得します。
        /// </summary>
        /// <value>
        /// このアカウントに関連付けられたリレーションのシーケンス。
        /// </value>
        IEnumerable<IRelation> Relations
        {
            get;
        }

        /// <summary>
        /// このアカウントが対象として関連付けられたリレーションのシーケンスを取得します。
        /// </summary>
        /// <value>
        /// このアカウントが対象として関連付けられたリレーションのシーケンス。
        /// </value>
        IEnumerable<IRelation> ReverseRelations
        {
            get;
        }

        /// <summary>
        /// このアカウントに関連付けられたリレーションの意味と、対象となるアカウントの組のシーケンスを取得します。
        /// </summary>
        /// <value>
        /// このアカウントに関連付けられたリレーションの意味と、対象となるアカウントの組のシーケンス。
        /// </value>
        IEnumerable<KeyValuePair<String, IAccount>> Relating
        {
            get;
        }

        /// <summary>
        /// このアカウントが対象として関連付けられたリレーションの意味と、関連付けたアカウントの組のシーケンスを取得します。
        /// </summary>
        /// <value>
        /// このアカウントが対象として関連付けられたリレーションの意味と、関連付けたアカウントの組のシーケンス。
        /// </value>
        IEnumerable<KeyValuePair<String, IAccount>> Relators
        {
            get;
        }

        /// <summary>
        /// このアカウントに関連付けられたマークのシーケンスを取得します。
        /// </summary>
        /// <value>
        /// このアカウントに関連付けられたマークのシーケンス。
        /// </value>
        IEnumerable<IMark> Marks
        {
            get;
        }

        /// <summary>
        /// このアカウントに関連付けられたマークの意味と、対象となるアクティビティの組のシーケンスを取得します。
        /// </summary>
        /// <value>
        /// このアカウントに関連付けられたマークの意味と、対象となるアクティビティの組のシーケンス。
        /// </value>
        IEnumerable<KeyValuePair<String, IActivity>> Marking
        {
            get;
        }

        /// <summary>
        /// 指定したアカウントが、このアカウントと完全に等しいかどうかを判断します。
        /// </summary>
        /// <param name="other">このアカウントと比較するアカウント。</param>
        /// <returns>指定したアカウントがこのアカウントと完全に等しい場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
        Boolean EqualsExact(IAccount other);

        /// <summary>
        /// カテゴリとサブ ID を指定して、このアカウントによるアクティビティのシーケンスを取得します。
        /// </summary>
        /// <param name="category">取得するアクティビティのカテゴリ。</param>
        /// <param name="subId">取得するアクティビティのサブ ID。</param>
        /// <returns>指定したカテゴリとサブ ID の、このアカウントによるアクティビティのシーケンス。</returns>
        IEnumerable<IActivity> ActivitiesOf(String category, String subId);

        /// <summary>
        /// カテゴリを指定して、このアカウントによるアクティビティのシーケンスを取得します。
        /// </summary>
        /// <param name="category">取得するアクティビティのカテゴリ。</param>
        /// <returns>指定したカテゴリの、このアカウントによるアクティビティのシーケンス。</returns>
        IEnumerable<IActivity> ActivitiesOf(String category);

        /// <summary>
        /// 意味を指定して、このアカウントに関連付けられたリレーションの対象となるアカウントのシーケンスを取得します。
        /// </summary>
        /// <param name="name">リレーションの意味。</param>
        /// <returns>このアカウントに、指定した意味で関連付けられたリレーションの対象となるアカウントのシーケンス。</returns>
        IEnumerable<IAccount> RelatingOf(String name);

        /// <summary>
        /// 意味を指定して、このアカウントが対象として関連付けられたリレーションを関連付けたアカウントのシーケンスを取得します。
        /// </summary>
        /// <param name="name">リレーションの意味。</param>
        /// <returns>このアカウントに、指定した意味でこのアカウントが対象として関連付けられたリレーションを関連付けたアカウントのシーケンス。</returns>
        IEnumerable<IAccount> RelatorsOf(String name);

        /// <summary>
        /// 意味を指定して、このアカウントに関連付けられたマークの対象となるアクティビティのシーケンスを取得します。
        /// </summary>
        /// <param name="name">マークの意味。</param>
        /// <returns>このアカウントに、指定した意味で関連付けられたマークの対象となるアクティビティのシーケンス。</returns>
        IEnumerable<IActivity> MarkingOf(String name);

        /// <summary>
        /// このアカウントに、指定した意味でアノテーションが関連付けられているかどうかを示す値を取得します。
        /// </summary>
        /// <param name="name">アノテーションの意味。</param>
        /// <returns>
        /// このアカウントに、指定した意味でアノテーションが関連付けられている場合は <c>true</c>。それ以外の場合は <c>false</c>。
        /// </returns>
        Boolean IsAnnotating(String name);

        /// <summary>
        /// このアカウントに、指定した意味とアカウントでリレーションが関連付けられているかどうかを示す値を取得します。
        /// </summary>
        /// <param name="name">アノテーションの意味。</param>
        /// <param name="account">対象とするアカウント。</param>
        /// <returns>
        /// このアカウントに、指定した意味とアカウントでリレーションが関連付けられている場合は <c>true</c>。それ以外の場合は <c>false</c>。
        /// </returns>
        Boolean IsRelating(String name, IAccount account);

        /// <summary>
        /// 指定したアカウントに、指定した意味でこのアカウントを対象としてリレーションが関連付けられているかどうかを示す値を取得します。
        /// </summary>
        /// <param name="name">アノテーションの意味。</param>
        /// <param name="account">リレーションが関連付けられているかどうかを取得するアカウント。</param>
        /// <returns>
        /// 指定したアカウントに、指定した意味でこのアカウントを対象としてリレーションが関連付けられている場合は <c>true</c>。それ以外の場合は <c>false</c>。
        /// </returns>
        Boolean IsRelated(String name, IAccount account);

        /// <summary>
        /// このアカウントが、指定した意味でアカウントでリレーションが関連付けられているかどうかを示す値を取得します。
        /// </summary>
        /// <param name="name">マークの名前。</param>
        /// <param name="activity">対象とするアクティビティ。</param>
        /// <returns>
        /// このアカウントに、指定した意味とアクティビティでリレーションが関連付けられている場合は <c>true</c>。それ以外の場合は <c>false</c>。
        /// </returns>
        Boolean IsMarking(String name, IActivity activity);

        /// <summary>
        /// このアカウントによるアクティビティを追加します。
        /// </summary>
        /// <param name="timestamp">アクティビティのタイムスタンプ。</param>
        /// <param name="category">アクティビティのカテゴリ。</param>
        /// <param name="subId">アクティビティのサブ ID。</param>
        /// <param name="userAgent">アクティビティのユーザ エージェント。</param>
        /// <param name="value">アクティビティの値。</param>
        /// <param name="data">アクティビティのデータ。</param>
        /// <returns>追加されたアクティビティ。</returns>
        IActivity Act(DateTime timestamp, String category, String subId, String userAgent, String value, Byte[] data);

        /// <summary>
        /// このアカウントによるアクティビティを追加します。
        /// </summary>
        /// <param name="timestamp">アクティビティのタイムスタンプ。</param>
        /// <param name="category">アクティビティのカテゴリ。</param>
        /// <param name="subId">アクティビティのサブ ID。</param>
        /// <returns>追加されたアクティビティ。</returns>
        IActivity Act(DateTime timestamp, String category, String subId);

        /// <summary>
        /// このアカウントにアノテーションを関連付けます。
        /// </summary>
        /// <param name="name">アノテーションの意味。</param>
        /// <returns>関連付けられたアノテーション。</returns>
        IAnnotation Annotate(String name);

        /// <summary>
        /// このアカウントにリレーションを関連付けます。
        /// </summary>
        /// <param name="name">リレーションの意味。</param>
        /// <param name="relateTo">リレーションの対象となるアカウント。</param>
        /// <returns>関連付けられたリレーション。</returns>
        IRelation Relate(String name, IAccount relateTo);

        /// <summary>
        /// このアカウントを対象として、指定したアカウントにリレーションを関連付けます。
        /// </summary>
        /// <param name="name">リレーションの意味。</param>
        /// <param name="relatedFrom">リレーションを関連付けるアカウント。</param>
        /// <returns>関連付けられたリレーション。</returns>
        IRelation Related(String name, IAccount relatedFrom);

        /// <summary>
        /// このアカウントにマークを関連付けます。
        /// </summary>
        /// <param name="name">マークの意味。</param>
        /// <param name="markTo">マークの対象となるアクティビティ。</param>
        /// <returns>関連付けたマーク。</returns>
        IMark Mark(String name, IActivity markTo);
    }
}