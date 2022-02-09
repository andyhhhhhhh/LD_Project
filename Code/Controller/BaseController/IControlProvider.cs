using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseController
{

    public static class IControlProvider<T> where T : class
    {
        interface ICreate
        {
            T Create();
        }
        class Creater<U> : ICreate where U : T, new()
        {
            public T Create()
            {
                return new U();
            }
        }
        class SingletonCreater<U> : ICreate where U : T, new()
        {
            class InstanceClass
            {
                public static readonly T Instance = new U();
            }
            public T Create()
            {
                return InstanceClass.Instance;
            }
        }
        #region 无参数的
        //static ICreate creater;
        //public static T Get()
        //{
        //    return creater.Create();
        //}
        //public static void Reg<S>() where S : T, new()
        //{
        //    creater = new Creater<S>();
        //}
        //public static void RegSingleton<S>() where S : T, new()
        //{
        //    creater = new SingletonCreater<S>();
        //}
        #endregion

        #region 有参数的
        static IDictionary<Type, ICreate> creaters = new System.Collections.Concurrent.ConcurrentDictionary<Type, ICreate>();
        public static T Get()
        {
            ICreate ct;
            Type t = typeof(T);
            if (creaters.TryGetValue(t, out ct))
                return ct.Create();
            throw new Exception(t.Name + "未注册");
        }
        public static void Register<Interface>() where Interface : T, new()
        {
            try
            {
                if (!creaters.ContainsKey(typeof(T)))
                {
                    creaters.Add(typeof(T), new SingletonCreater<Interface>());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
