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
        public virtual void BeforeAddKeywordBuf(KeywordBuf BufType, ref int Stack)
        {

        }
        public virtual void BeforeOtherUnitAddKeywordBuf(KeywordBuf BufType,BattleUnitModel Target, ref int Stack)
        {

        }
        public class BeforeAddKeywordBufPatch
        {
            public static void Trigger_AddKeywordBuf_Before(KeywordBuf BufType, BattleUnitModel Target, ref int Stack)
            {
                if(Stack > 0)
                {
                    foreach (var Buf in Target.bufListDetail.GetActivatedBufList())
                    {
                        if(!Buf.IsDestroyed() && Buf is BattleUnitBuf_Don_Eyuil)
                        {
                            (Buf as BattleUnitBuf_Don_Eyuil).BeforeAddKeywordBuf(BufType, ref Stack);//Target = owner所以没有一参传入
                        }
                    }
                    List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList();
                    aliveList.Remove(Target);
                    foreach (var Model in aliveList)
                    {
                        foreach (var Buf in Model.bufListDetail.GetActivatedBufList())
                        {
                            if (!Buf.IsDestroyed() && Buf is BattleUnitBuf_Don_Eyuil)
                            {
                                (Buf as BattleUnitBuf_Don_Eyuil).BeforeOtherUnitAddKeywordBuf(BufType, Target,ref Stack);
                            }
                        }
                    }                    // Model.bufListDetail.GetActivatedBufList().DoIf(cond => !cond.IsDestroyed() && cond is BattleUnitBuf_Don_Eyuil, x => (x as BattleUnitBuf_Don_Eyuil).AfterTakeBleedingDamage(dmg));

                    // aliveList.Do(x1 => x1.bufListDetail.GetActivatedBufList().DoIf(cond => !cond.IsDestroyed() && cond is BattleUnitBuf_Don_Eyuil, x => (x as BattleUnitBuf_Don_Eyuil).AfterOtherUnitTakeBleedingDamage(Model, dmg)));
                }
            }
            [HarmonyPatch(typeof(BattleUnitBufListDetail), "AddKeywordBufThisRoundByEtc")]
            [HarmonyPatch(typeof(BattleUnitBufListDetail), "AddKeywordBufByEtc")]
            [HarmonyPatch(typeof(BattleUnitBufListDetail), "AddKeywordBufThisRoundByCard")]
            [HarmonyPatch(typeof(BattleUnitBufListDetail), "AddKeywordBufByCard")]
            [HarmonyPatch(typeof(BattleUnitBufListDetail), "AddKeywordBufNextNextByCard")]
            [HarmonyTranspiler]
            public static IEnumerable<CodeInstruction> BattleUnitBufListDetail_AddKeywordBuf_Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
                codes.InsertRange(0, new List<CodeInstruction>()
                {
                        new CodeInstruction(OpCodes.Ldarg_1),
                        new CodeInstruction(OpCodes.Ldarg_0),
                        new CodeInstruction(OpCodes.Ldfld,AccessTools.Field(typeof(BattleUnitBufListDetail),"_self")),
                        new CodeInstruction(OpCodes.Ldarga,2),                       
                        new CodeInstruction(OpCodes.Call,AccessTools.Method(typeof(BeforeAddKeywordBufPatch),"Trigger_AddKeywordBuf_Before")),
                 });
                return codes.AsEnumerable<CodeInstruction>();
            }
        }

        public virtual void OnStartBattle() 
        { 

        }
        public class OnStartBattlePatch
        {
            [HarmonyPatch(typeof(BattleUnitModel), "OnStartBattle")]
            [HarmonyPostfix]
            public static void BattleUnitModel_OnStartBattle_Post(BattleUnitModel __instance)
            {
                __instance.bufListDetail.GetActivatedBufList().ForEach(x =>
                {
                    if (x is BattleUnitBuf_Don_Eyuil)
                    {
                        (x as BattleUnitBuf_Don_Eyuil).OnStartBattle();
                    }
                });
            }
        }


        public class OnTakeBleedingDamagePatch
        {
            public static void Trigger_BleedingDmg_After(BattleUnitModel Model,int dmg,KeywordBuf keyword)
            {
                if(keyword == KeywordBuf.Bleeding && dmg > 0)
                {
                    Model.bufListDetail.GetActivatedBufList().DoIf(cond => !cond.IsDestroyed() && cond is BattleUnitBuf_Don_Eyuil, x => (x as BattleUnitBuf_Don_Eyuil).AfterTakeBleedingDamage(dmg));
                    List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList();
                    aliveList.Remove(Model);
                    aliveList.Do(x1 => x1.bufListDetail.GetActivatedBufList().DoIf(cond => !cond.IsDestroyed() && cond is BattleUnitBuf_Don_Eyuil, x => (x as BattleUnitBuf_Don_Eyuil).AfterOtherUnitTakeBleedingDamage(Model,dmg))); 
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
        public virtual void AfterOtherUnitTakeBleedingDamage(BattleUnitModel Unit,int Dmg)
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
        public static T GainBuf<T>(BattleUnitModel model, int stack, BufReadyType ReadyType = BufReadyType.ThisRound) where T : BattleUnitBuf_Don_Eyuil
        {
            T BuffInstance = GetOrAddBuf<T>(model, ReadyType);
            if (BuffInstance != null)
            {
                BuffInstance.Add(stack);
            }
            return BuffInstance;
        }
        public static T GetBuf<T>(BattleUnitModel model, BufReadyType ReadyType = BufReadyType.ThisRound) where T : BattleUnitBuf_Don_Eyuil
        {
            switch(ReadyType)
            {
                case BufReadyType.ThisRound:
                    return model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is T && !x.IsDestroyed()) as T;
                case BufReadyType.NextRound:
                    return model.bufListDetail.GetReadyBufList().Find((BattleUnitBuf x) => x is T && !x.IsDestroyed()) as T;
                case BufReadyType.NextNextRound:
                    return model.bufListDetail.GetReadyReadyBufList().Find((BattleUnitBuf x) => x is T && !x.IsDestroyed()) as T;
                default:
                    return null;
            }
        }

        public static int GetBufStack<T>(BattleUnitModel model, BufReadyType ReadyType = BufReadyType.ThisRound) where T : BattleUnitBuf_Don_Eyuil
        {
            T BuffInstance = GetBuf<T>(model, ReadyType);
            if (BuffInstance != null)
            {
                return BuffInstance.stack;
            }
            return 0;
        }
        public static void RemoveBuf<T>(BattleUnitModel model, BufReadyType ReadyType = BufReadyType.ThisRound) where T : BattleUnitBuf_Don_Eyuil
        {
            T BuffInstance = GetBuf<T>(model, ReadyType);
            if (BuffInstance != null)
            {
                BuffInstance.Destroy();
            }
        }
        public virtual void AfterGetOrAddBuf()
        {

        }
        public static T GetOrAddBuf<T>(BattleUnitModel model,BufReadyType ReadyType = BufReadyType.ThisRound) where T : BattleUnitBuf_Don_Eyuil
        {
            T BuffInstance = GetBuf<T>(model, ReadyType);
            if (BuffInstance == null)
            {
                switch (ReadyType)
                {
                    case BufReadyType.ThisRound:
                        model.bufListDetail.AddBuf(Activator.CreateInstance(typeof(T), model) as T);
                        break;
                    case BufReadyType.NextRound:
                        model.bufListDetail.AddReadyBuf(Activator.CreateInstance(typeof(T), model) as T);
                        break;
                    case BufReadyType.NextNextRound:
                        model.bufListDetail.AddReadyReadyBuf(Activator.CreateInstance(typeof(T), model) as T);
                        break;
                    default:
                        break;
                }

                BuffInstance = GetBuf<T>(model, ReadyType);
                BuffInstance.AfterGetOrAddBuf();
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
