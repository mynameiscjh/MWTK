using EnumExtenderV2;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Xml;
using UnityEngine;
using Workshop;

namespace Don_Eyuil
{
    public class BattleUnitBuf_Don_Eyuil : BattleUnitBuf
    {
        public class OnTakeBleedingDamagePatch
        {
            public static void Trigger_BleedingDmg_After(BattleUnitModel Model,int dmg,KeywordBuf keyword)
            {
                if(keyword == KeywordBuf.Bleeding && dmg > 0)
                {
                    Model.bufListDetail.GetActivatedBufList().DoIf(cond => !cond.IsDestroyed() && cond is BattleUnitBuf_Don_Eyuil, x => (x as BattleUnitBuf_Don_Eyuil).AfterTakeBleedingDamage(dmg));
                }
            }
            [HarmonyPatch(typeof(BattleUnitModel), "TakeDamage")]
            [HarmonyTranspiler]
            public static IEnumerable<CodeInstruction> BattleUnitModel_TakeDamage_Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
                for (int i = 1; i < codes.Count; i++)
                {
                    if (codes[i].Calls(AccessTools.Method(typeof(BattleUnitModel), "set_hp")) && codes[i - 1].opcode == OpCodes.Sub)
                    {
                        codes.InsertRange(i + 1, new List<CodeInstruction>()
                        {
                            new CodeInstruction(OpCodes.Ldarg_0),
                            new CodeInstruction(OpCodes.Ldloc_2),
                            new CodeInstruction(OpCodes.Ldarg_S,4),
                            new CodeInstruction(OpCodes.Call,AccessTools.Method(typeof(OnTakeBleedingDamagePatch),"Trigger_BleedingDmg_After"))
                        });
                    }
                }
                return codes.AsEnumerable<CodeInstruction>();
            }
        }

        public virtual void AfterTakeBleedingDamage(int Dmg)
        {
            
        }
        public virtual int GetMaxStack() => -1;
        public virtual void Add(int stack)
        {
            this.stack += stack;
            if(GetMaxStack() >= 0)
            {
                this.stack = Math.Min(this.stack, GetMaxStack());
            }
            this.OnAddBuf(stack);
            if (this.stack <= 0)
            {
                this.Destroy();
            }
        }
        public BattleUnitBuf_Don_Eyuil(BattleUnitModel model)
        {
            this._owner = model;
        }
        public static T GainBuf<T>(BattleUnitModel model, int stack) where T : BattleUnitBuf_Don_Eyuil
        {
            T BuffInstance = GetOrAddBuf<T>(model);
            if (BuffInstance != null)
            {
                BuffInstance.Add(stack);
            }
            return BuffInstance;
        }
        public static T GetBuf<T>(BattleUnitModel model) where T : BattleUnitBuf_Don_Eyuil => model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is T && !x.IsDestroyed()) as T;

        public static int GetBufStack<T>(BattleUnitModel model) where T : BattleUnitBuf_Don_Eyuil
        {
            T BuffInstance = GetBuf<T>(model);
            if (BuffInstance != null)
            {
                return BuffInstance.stack;
            }
            return 0;
        }
        public static void RemoveBuf<T>(BattleUnitModel model) where T : BattleUnitBuf_Don_Eyuil
        {
            T BuffInstance = GetBuf<T>(model);
            if (BuffInstance != null)
            {
                BuffInstance.Destroy();
            }
        }
        public static T GetOrAddBuf<T>(BattleUnitModel model) where T : BattleUnitBuf_Don_Eyuil
        {
            T BuffInstance = GetBuf<T>(model);
            if (BuffInstance == null)
            {
                model.bufListDetail.AddBuf(Activator.CreateInstance(typeof(T), model) as T);
                BuffInstance = GetBuf<T>(model);
            }
            return BuffInstance;
        }
    }
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
            return new LorId(TKS_BloodFiend_Initializer.packageId, v);
        }

    }
}
