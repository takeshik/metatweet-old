using System;
using System.Collections.Generic;

namespace XSpect.MetaTweet.Modules
{
    partial class ModuleDomain
    {
        [Serializable()]
        public delegate T Callback<T>();

        [Serializable()]
        public delegate T ParameterizedCallback<T>(AppDomainDataAccessor data);

        [Serializable()]
        public delegate void Callback();

        [Serializable()]
        public delegate void ParameterizedCallback(AppDomainDataAccessor data);

        [Serializable()]
        public class DoCallbackHelper
            : Object
        {
            protected AppDomain Domain
            {
                get;
                private set;
            }

            protected String Prefix
            {
                get;
                set;
            }

            protected String LockObjectDataPrefix
            {
                get
                {
                    return "<ModuleDomain+DoCallbackHelper.LockObject>_" + this.Prefix;
                }
            }

            protected String ArgumentDataPrefix
            {
                get
                {
                    return "<ModuleDomain+DoCallbackHelper.Arguments>_" + this.Prefix + ":";
                }
            }

            protected Delegate CallbackDelegate
            {
                get;
                private set;
            }

            protected IDictionary<String, Object> Arguments
            {
                get;
                private set;
            }

            protected DoCallbackHelper(AppDomain domain, Delegate callback, IDictionary<String, Object> arguments)
            {
                this.Domain = domain;
                this.CallbackDelegate = callback;
                this.Arguments = arguments;
            }

            public DoCallbackHelper(AppDomain domain, Callback callback)
                : this(domain, callback, null)
            {
            }

            public DoCallbackHelper(AppDomain domain, ParameterizedCallback callback, IDictionary<String, Object> arguments)
                : this(domain, (Delegate) callback, arguments)
            {
                this.SetPrefix();
            }

            public void DoCallback()
            {
                if (this.Arguments != null)
                {
                    this.Wind();
                    this.Domain.DoCallBack(() =>
                        ((ParameterizedCallback) this.CallbackDelegate)(new AppDomainDataAccessor(this.Domain, this.ArgumentDataPrefix, true))
                    );
                    this.Unwind();
                }
                else
                {
                    this.Domain.DoCallBack(() => ((Callback) this.CallbackDelegate)());
                }
            }

            protected void SetPrefix()
            {
                this.Prefix = String.Empty;
                do
                {
                    this.Prefix = DateTime.UtcNow.Ticks + "-" + Guid.NewGuid().ToString("N");
                } while (this.Domain.GetData(this.LockObjectDataPrefix) != null);
                this.Domain.SetData(this.LockObjectDataPrefix, new Object());
            }

            protected void Wind()
            {
                foreach (KeyValuePair<String, Object> p in this.Arguments)
                {
                    this.Domain.SetData(this.ArgumentDataPrefix + p.Key, p.Value);
                }
            }

            protected void Unwind()
            {
                foreach (KeyValuePair<String, Object> p in this.Arguments)
                {
                    this.Domain.SetData(this.ArgumentDataPrefix + p.Key, null);
                }
            }
        }

        [Serializable()]
        public class DoCallbackHelper<T>
            : DoCallbackHelper
        {
            protected String ReturnValueDataPrefix
            {
                get
                {
                    return "<ModuleDomain+DoCallbackHelper.ReturnValue>_" + this.Prefix;
                }
            }

            public DoCallbackHelper(AppDomain domain, Callback<T> callback)
                : base(domain, callback, null)
            {
                this.SetPrefix();
            }

            public DoCallbackHelper(AppDomain domain, ParameterizedCallback<T> callback, IDictionary<String, Object> arguments)
                : base(domain, callback, arguments)
            {
                this.SetPrefix();
            }

            public new T DoCallback()
            {
                if (this.Arguments != null)
                {
                    this.Wind();
                    this.Domain.DoCallBack(() => AppDomain.CurrentDomain.SetData(
                        this.ReturnValueDataPrefix,
                        ((ParameterizedCallback<T>) this.CallbackDelegate)(new AppDomainDataAccessor(this.Domain, this.ArgumentDataPrefix, true))
                    ));
                    this.Unwind();
                }
                else
                {
                    this.Domain.DoCallBack(() => AppDomain.CurrentDomain.SetData(
                        this.ReturnValueDataPrefix,
                        ((Callback<T>) this.CallbackDelegate)()
                    ));
                }
                T value = (T) this.Domain.GetData(this.ReturnValueDataPrefix);
                this.Domain.SetData(this.ReturnValueDataPrefix, null);
                return value;
            }
        }
    }
}