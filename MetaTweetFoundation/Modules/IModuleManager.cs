using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;

namespace XSpect.MetaTweet.Modules
{
    /// <summary>
    /// モジュールの管理を行う機能を提供します。
    /// </summary>
    /// <remarks>
    /// <para>モジュールは、MetaTweet に機能を実装する、自由に着脱可能な機構です。モジュールは、
    /// <list type="bullet">
    /// <item><description><see cref="IModule"/> を実装する型 (モジュール型)</description></item>
    /// <item><description>モジュール型を 1 以上含むアセンブリ (モジュール アセンブリ)</description></item>
    /// <item><description>モジュール型のインスタンス (モジュール オブジェクト)</description></item>
    /// </list>
    /// の総称です。</para>
    /// <para><see cref="Load(String)"/> メソッドを使用して、モジュール アセンブリをロードし、<see cref="Unload"/> メソッドを使用してアンロードすることができます。ロードとはモジュールのための独立した環境 (モジュール ドメイン: <see cref="ModuleDomain"/> オブジェクト) を構築し、その中でモジュールを読み込む作業です。アンロードとはロードしたモジュール アセンブリを解放し、ドメインを破棄する作業です。</para>
    /// <para>モジュール ドメインの基底構造はディレクトリです。モジュール ドメインが作成されると、ドメインの名前と同じディレクトリ内のアセンブリをロードします。</para>
    /// <para>モジュール オブジェクトの管理は、モジュール アセンブリが読み込まれた環境である <see cref="IModuleDomain"/> オブジェクトで行います。</para>
    /// </remarks>
    /// <seealso cref="IModuleDomain"/>
    public interface IModuleManager
        : IDisposable,
          ILoggable
    {
        /// <summary>
        /// このオブジェクトを保持する <see cref="IServerCore"/> オブジェクトを取得します。
        /// </summary>
        /// <value>
        /// このオブジェクトを保持する <see cref="IServerCore"/> オブジェクト。
        /// </value>
        IServerCore Parent
        {
            get;
        }

        /// <summary>
        /// このオブジェクトの設定を保持するオブジェクトを取得します。
        /// </summary>
        /// <value>
        /// このオブジェクトの設定を保持するオブジェクト。
        /// </value>
        dynamic Configuration
        {
            get;
        }

        /// <summary>
        /// 現在管理されているモジュール ドメインのシーケンスを取得します。
        /// </summary>
        /// <value>
        /// 現在管理されているモジュール ドメインのシーケンス。
        /// </value>
        IDictionary<String, IModuleDomain> Domains
        {
            get;
        }

        /// <summary>
        /// モジュール ドメインを作成し、モジュール アセンブリをロードします。
        /// </summary>
        /// <param name="domainName">ドメインの名前、即ち、ロードするモジュール アセンブリを含んだディレクトリの名前。</param>
        /// <returns>モジュール アセンブリがロードされたモジュール ドメイン。</returns>
        IModuleDomain Load(String domainName);

        void Load();

        /// <summary>
        /// モジュール ドメインをアンロードします。
        /// </summary>
        /// <param name="domainName">アンロードするモジュール ドメインの名前。</param>
        void Unload(String domainName);

        /// <summary>
        /// モジュール ドメインをリロードします。
        /// </summary>
        /// <param name="domainName">リロードするモジュール ドメインの名前。</param>
        void Reload(String domainName);

        #region GetModules / GetModule

        /// <summary>
        /// モジュール オブジェクトを検索します。
        /// </summary>
        /// <param name="domain">モジュール ドメインの名前。条件を指定しない場合は <c>null</c>。</param>
        /// <param name="key">モジュール オブジェクトを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <param name="type">モジュール オブジェクトの型を表すオブジェクト。条件を指定しない場合は <c>null</c>。</param>
        /// <returns>条件に合致するモジュールのシーケンス。</returns>
        IEnumerable<IModule> GetModules(String domain, String key, Type type);

        /// <summary>
        /// モジュール オブジェクトを検索します。
        /// </summary>
        /// <param name="key">モジュール オブジェクトを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <param name="type">モジュール オブジェクトの型を表すオブジェクト。条件を指定しない場合は <c>null</c>。</param>
        /// <returns>条件に合致するモジュールのシーケンス。</returns>
        IEnumerable<IModule> GetModules(String key, Type type);

        /// <summary>
        /// モジュール オブジェクトを検索します。
        /// </summary>
        /// <typeparam name="TModule">モジュール オブジェクトの型。</typeparam>
        /// <param name="domain">モジュール ドメインの名前。条件を指定しない場合は <c>null</c>。</param>
        /// <param name="key">モジュール オブジェクトを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <returns>条件に合致するモジュールのシーケンス。</returns>
        IEnumerable<TModule> GetModules<TModule>(String domain, String key)
            where TModule : IModule;

        /// <summary>
        /// モジュール オブジェクトを検索します。
        /// </summary>
        /// <typeparam name="TModule">モジュール オブジェクトの型。</typeparam>
        /// <param name="key">モジュール オブジェクトを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <returns>条件に合致するモジュールのシーケンス。</returns>
        IEnumerable<TModule> GetModules<TModule>(String key)
            where TModule : IModule;

        /// <summary>
        /// モジュール オブジェクトを検索します。
        /// </summary>
        /// <typeparam name="TModule">モジュール オブジェクトの型。</typeparam>
        /// <returns>条件に合致するモジュールのシーケンス。</returns>
        IEnumerable<TModule> GetModules<TModule>()
            where TModule : IModule;

        /// <summary>
        /// モジュール オブジェクトを検索します。
        /// </summary>
        /// <param name="domain">モジュール ドメインの名前。条件を指定しない場合は <c>null</c>。</param>
        /// <param name="key">モジュール オブジェクトを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <returns>条件に合致するモジュールのシーケンス。</returns>
        IEnumerable<IModule> GetModules(String domain, String key);

        /// <summary>
        /// モジュール オブジェクトを検索します。
        /// </summary>
        /// <param name="key">モジュール オブジェクトを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <returns>条件に合致するモジュールのシーケンス。</returns>
        IEnumerable<IModule> GetModules(String key);

        /// <summary>
        /// 全てのモジュール オブジェクトを取得します。
        /// </summary>
        /// <returns>全てのモジュールのシーケンス。</returns>
        IEnumerable<IModule> GetModules();

        /// <summary>
        /// モジュール オブジェクトを取得します。
        /// </summary>
        /// <param name="domain">モジュール ドメインの名前。条件を指定しない場合は <c>null</c>。</param>
        /// <param name="key">モジュール オブジェクトを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <param name="type">モジュール オブジェクトの型を表すオブジェクト。条件を指定しない場合は <c>null</c>。</param>
        /// <returns>一意に特定されたモジュール。</returns>
        IModule GetModule(String domain, String key, Type type);

        /// <summary>
        /// モジュール オブジェクトを取得します。
        /// </summary>
        /// <param name="key">モジュール オブジェクトを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <param name="type">モジュール オブジェクトの型を表すオブジェクト。条件を指定しない場合は <c>null</c>。</param>
        /// <returns></returns>
        IModule GetModule(String key, Type type);

        /// <summary>
        /// モジュール オブジェクトを取得します。
        /// </summary>
        /// <typeparam name="TModule">モジュール オブジェクトの型。</typeparam>
        /// <param name="domain">モジュール ドメインの名前。条件を指定しない場合は <c>null</c>。</param>
        /// <param name="key">モジュール オブジェクトを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <returns>一意に特定されたモジュール。</returns>
        TModule GetModule<TModule>(String domain, String key)
            where TModule : IModule;

        /// <summary>
        /// モジュール オブジェクトを取得します。
        /// </summary>
        /// <typeparam name="TModule">モジュール オブジェクトの型。</typeparam>
        /// <param name="key">モジュール オブジェクトを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <returns>一意に特定されたモジュール。</returns>
        TModule GetModule<TModule>(String key)
            where TModule : IModule;

        /// <summary>
        /// モジュール オブジェクトを取得します。
        /// </summary>
        /// <param name="domain">モジュール ドメインの名前。条件を指定しない場合は <c>null</c>。</param>
        /// <param name="key">モジュール オブジェクトを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <returns>一意に特定されたモジュール。</returns>
        IModule GetModule(String domain, String key);

        /// <summary>
        /// モジュール オブジェクトを取得します。
        /// </summary>
        /// <param name="key">モジュール オブジェクトを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <returns>一意に特定されたモジュール。</returns>
        IModule GetModule(String key);

        #endregion

        #region Execute

        /// <summary>
        /// スクリプト コードを実行します。
        /// </summary>
        /// <typeparam name="T">コードの返り値の型。</typeparam>
        /// <param name="language">コードの言語を表す文字列。</param>
        /// <param name="source">実行するコード。</param>
        /// <param name="arguments">コードに与える引数とその値のリスト。</param>
        /// <returns>コードの評価の結果となる返り値。</returns>
        T Execute<T>(String language, String source, params Expression<Func<Object, dynamic>>[] arguments);

        /// <summary>
        /// スクリプト コードを実行します。
        /// </summary>
        /// <param name="language">コードの言語を表す文字列。</param>
        /// <param name="source">実行するコード。</param>
        /// <param name="arguments">コードに与える引数とその値のリスト。</param>
        /// <returns>コードの評価の結果となる返り値。</returns>
        dynamic Execute(String language, String source, params Expression<Func<Object, dynamic>>[] arguments);

        /// <summary>
        /// スクリプト コードを実行します。
        /// </summary>
        /// <typeparam name="T">コードの返り値の型。</typeparam>
        /// <param name="file">実行するコード ファイル。</param>
        /// <param name="arguments">コードに与える引数とその値のリスト。</param>
        /// <returns>コードの評価の結果となる返り値。</returns>
        T Execute<T>(FileInfo file, params Expression<Func<Object, dynamic>>[] arguments);

        /// <summary>
        /// スクリプト コードを実行します。
        /// </summary>
        /// <param name="file">実行するコード ファイル。</param>
        /// <param name="arguments">コードに与える引数とその値のリスト。</param>
        /// <returns>コードの評価の結果となる返り値。</returns>
        dynamic Execute(FileInfo file, params Expression<Func<Object, dynamic>>[] arguments);

        #endregion
    }
}
