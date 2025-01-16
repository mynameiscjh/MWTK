using HarmonyLib;
using System;
using Debug = UnityEngine.Debug;

namespace Don_Eyuil
{
    public static class MyTools
    {
        /// <summary>
        /// 反射
        /// </summary>
        /// <typeparam name="T">返回类型的值的类型</typeparam>
        /// <param name="obj">实例</param>
        /// <param name="name">变量名</param>
        /// <returns></returns>
        public static T GetFieldValue<T>(this object obj, string name)
        {
            var res = default(T);
            try
            {
                res = (T)obj.GetType().GetField(name, AccessTools.all).GetValue(obj);
            }
            catch (Exception ex)
            {
                Debug.Log($" : T GetFieldValue<T>(this object obj, string name) : {ex}");
            }

            return res;
        }

        public static object GetFieldValue(this object obj, Type t, string name)
        {
            object res = null;
            try
            {
                res = obj.GetType().GetField(name, AccessTools.all).GetValue(obj);
            }
            catch (Exception ex)
            {
                Debug.Log($" : T GetFieldValue<T>(this object obj, string name) : {ex}");
            }

            return res;
        }
        public static void SetFieldValue<T>(this object obj, string name, object value)
        {
            try
            {
                obj.GetType().GetField(name, AccessTools.all).SetValue(obj, value);
            }
            catch (Exception ex)
            {
                Debug.Log($" : T GetFieldValue<T>(this object obj, string name) : {ex}");
            }
        }
        /// <summary>
        /// 反射
        /// </summary>
        /// <typeparam name="T">返回类型的值的类型</typeparam>
        /// <param name="obj">实例</param>
        /// <param name="name">方法名</param>
        /// <param name="parameters">方法参数</param>
        /// <returns></returns>
        public static T InvokeMethod<T>(this object obj, string name, params object[] parameters)
        {
            var res = default(T);
            try
            {
                res = (T)obj.GetType().GetMethod(name, AccessTools.all).Invoke(obj, parameters);
            }
            catch (Exception ex)
            {
                Debug.Log(
                    $" : T InvokeMethod<T>(this object obj, string name, params object[] parameters) : {ex}");
            }

            return res;
        }

        public static object InvokeMethod(this object obj, Type t, string name, params object[] parameters)
        {
            object res = null;
            try
            {
                res = obj.GetType().GetMethod(name, AccessTools.all).Invoke(obj, parameters);
            }
            catch (Exception ex)
            {
                Debug.Log(
                    $" : object InvokeMethod(this object obj, Type t, string name, params object[] parameters) : {ex}");
            }

            return res;
        }

        public static void InvokeMethod(this object obj, string name, params object[] parameters)
        {
            try
            {
                obj.GetType().GetMethod(name, AccessTools.all).Invoke(obj, parameters);
            }
            catch (Exception ex)
            {
                Debug.Log(
                    $" : InvokeMethod(this object obj, string name, params object[] parameters) : {ex}");
            }

        }

        public static LorId Create(int v)
        {
            return new LorId(ModInit.id, v);
        }

    }

}
