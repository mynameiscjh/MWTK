using CustomMapUtility;
using HarmonyLib;
using JetBrains.Annotations;
using LOR_DiceSystem;
using Steamworks.Ugc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using UnityEngine;
using MonoMod.Utils;
using MonoMod.Utils.Cil;
using System.Runtime.Remoting.Messaging;
using System.Runtime.CompilerServices;
namespace Don_Eyuil
{
    public class EmotionEgoXmlInfo_Mod : EmotionEgoXmlInfo
    {
        public string packageId = "";


        public EmotionEgoXmlInfo_Mod(LorId id)
        {
            this.packageId = id.packageId;
            this._CardId = id.id;
        }
        public EmotionEgoXmlInfo_Mod()
        {

        }
        [HarmonyPatch(typeof(EmotionEgoXmlInfo), "get_CardId")]
        [HarmonyPostfix]
        public static void EmotionEgoXmlInfo_get_CardId_Post(EmotionEgoXmlInfo __instance, ref LorId __result)
        {
            if (__instance is EmotionEgoXmlInfo_Mod)
            {
                __result = new LorId((__instance as EmotionEgoXmlInfo_Mod).packageId, __instance._CardId);

                if (!ItemXmlDataList.instance.GetFieldValue<Dictionary<LorId, DiceCardXmlInfo>>("_cardInfoTable").TryGetValue(__result, out var v))
                {
                    var card = ItemXmlDataList.instance.GetCardItem(__result);
                    ItemXmlDataList.instance.GetFieldValue<Dictionary<LorId, DiceCardXmlInfo>>("_cardInfoTable").Add(__result, card);
                }
            }
        }
    }
    public class RedDiceCardAbility : DiceCardAbilityBase
    {
        public static Dictionary<BattlePlayingCardDataInUnitModel, List<BattleDiceBehavior>> RedDice = new Dictionary<BattlePlayingCardDataInUnitModel, List<BattleDiceBehavior>>();
        public override bool IsImmuneDestory => true;
        public virtual void BeforNewRoll(BattleParryingManager.ParryingTeam teamEnemy, BattleParryingManager.ParryingTeam teamLibrarian)
        {
        }
        //这里是给骰子染色的
#if false

        [HarmonyPatch(typeof(BattleDiceCardUI), "SetCard")]
        [HarmonyPostfix]
        public static void BattleDiceCardUI_SetCard_Post(BattleDiceCardModel cardModel, BattleDiceCardUI __instance)
        {
            for (int i = 0; i < cardModel.GetBehaviourList().Count; i++)
            {
                if (cardModel.GetBehaviourList()[i].Script.StartsWith("RED__"))
                {
                    switch (cardModel.GetBehaviourList()[i].Detail)
                    {
                        case BehaviourDetail.Slash:
                            __instance.img_behaviourDetatilList[i].sprite = TKS_BloodFiend_Initializer.ArtWorks["SlashRed"];
                            break;
                        case BehaviourDetail.Penetrate:
                            __instance.img_behaviourDetatilList[i].sprite = TKS_BloodFiend_Initializer.ArtWorks["PenetrateRed"];
                            break;
                        case BehaviourDetail.Hit:
                            __instance.img_behaviourDetatilList[i].sprite = TKS_BloodFiend_Initializer.ArtWorks["HitRed"];
                            break;
                        default:
                            break;
                    }
                }
            }
        }


        [HarmonyPatch(typeof(BattleDiceCard_BehaviourDescUI), "SetBehaviourInfo")]
        [HarmonyPostfix]
        public static void BattleDiceCard_BehaviourDescUI_SetBehaviourInfo_Post(DiceBehaviour behaviour, BattleDiceCard_BehaviourDescUI __instance)
        {
            if (behaviour.Script.StartsWith("RED__"))
            {
                switch (behaviour.Detail)
                {
                    case BehaviourDetail.Slash:
                        __instance.img_detail.sprite = TKS_BloodFiend_Initializer.ArtWorks["SlashRed"];
                        break;
                    case BehaviourDetail.Penetrate:
                        __instance.img_detail.sprite = TKS_BloodFiend_Initializer.ArtWorks["PenetrateRed"];
                        break;
                    case BehaviourDetail.Hit:
                        __instance.img_detail.sprite = TKS_BloodFiend_Initializer.ArtWorks["HitRed"];
                        break;
                    default:
                        break;
                }
            }
        }

        [HarmonyPatch(typeof(UIOriginCardSlot), "SetData")]
        [HarmonyPostfix]
        public static void UIOriginCardSlot_SetData_Post(DiceCardItemModel cardmodel, UIOriginCardSlot __instance)
        {
            if (cardmodel == null) { return; }
            for (int i = 0; i < cardmodel.ClassInfo.DiceBehaviourList.Count; i++)
            {
                if (cardmodel.ClassInfo.DiceBehaviourList[i].Script.StartsWith("RED__"))
                {
                    switch (cardmodel.ClassInfo.DiceBehaviourList[i].Detail)
                    {
                        case BehaviourDetail.Slash:
                            __instance.GetFieldValue<Image[]>("img_BehaviourIcons")[i].sprite = TKS_BloodFiend_Initializer.ArtWorks["SlashRed"];
                            break;
                        case BehaviourDetail.Penetrate:
                            __instance.GetFieldValue<Image[]>("img_BehaviourIcons")[i].sprite = TKS_BloodFiend_Initializer.ArtWorks["PenetrateRed"];
                            break;
                        case BehaviourDetail.Hit:
                            __instance.GetFieldValue<Image[]>("img_BehaviourIcons")[i].sprite = TKS_BloodFiend_Initializer.ArtWorks["HitRed"];
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        [HarmonyPatch(typeof(UIDetailCardDescSlot), "SetBehaviourInfo")]
        [HarmonyPostfix]
        public static void UIDetailCardDescSlot_SetBehaviourInfo_Post(DiceBehaviour behaviour, UIDetailCardDescSlot __instance)
        {
            if (behaviour.Script.StartsWith("RED__"))
            {
                switch (behaviour.Detail)
                {
                    case BehaviourDetail.Slash:
                        __instance.img_detail.sprite = TKS_BloodFiend_Initializer.ArtWorks["SlashRed"];
                        break;
                    case BehaviourDetail.Penetrate:
                        __instance.img_detail.sprite = TKS_BloodFiend_Initializer.ArtWorks["PenetrateRed"];
                        break;
                    case BehaviourDetail.Hit:
                        __instance.img_detail.sprite = TKS_BloodFiend_Initializer.ArtWorks["HitRed"];
                        break;
                    default:
                        break;
                }
            }
        }
#endif
        [HarmonyPostfix]
        [HarmonyPatch(typeof(BattleParryingManager), "GetDecisionResult")]
        public static void BattleParryingManager_GetDecisionResult_Pre(BattleParryingManager.ParryingTeam teamA, BattleParryingManager.ParryingTeam teamB, BattleParryingManager.ParryingDecisionResult __result)
        {
            if (__result == BattleParryingManager.ParryingDecisionResult.WinLibrarian)
            {
                if (teamA.playingCard.currentBehavior.behaviourInCard.Script.StartsWith("RED__") || teamA.playingCard.currentBehavior.abilityList.Find(x => x is RedDiceCardAbility) != null)
                {
                    if (!RedDice.ContainsKey(teamA.playingCard))
                    {
                        RedDice.Add(teamA.playingCard, new List<BattleDiceBehavior> { teamA.playingCard.currentBehavior });
                    }
                    else
                    {
                        RedDice[teamA.playingCard].Add(teamA.playingCard.currentBehavior);
                    }
                }
            }

            if (__result == BattleParryingManager.ParryingDecisionResult.WinEnemy)
            {
                if (teamB.playingCard.currentBehavior.behaviourInCard.Script.StartsWith("RED__") || teamB.playingCard.currentBehavior.abilityList.Find(x => x is RedDiceCardAbility) != null)
                {
                    if (!RedDice.ContainsKey(teamB.playingCard))
                    {
                        RedDice.Add(teamB.playingCard, new List<BattleDiceBehavior> { teamB.playingCard.currentBehavior });
                    }
                    else
                    {
                        RedDice[teamB.playingCard].Add(teamB.playingCard.currentBehavior);
                    }
                }
            }
        }


        [HarmonyPatch(typeof(BattleParryingManager), "CheckParryingEnd")]
        [HarmonyPrefix]
        public static void BattleParryingManager_CheckParryingEnd_Pre(BattleParryingManager.ParryingTeam ____teamEnemy, BattleParryingManager.ParryingTeam ____teamLibrarian)
        {
            bool flag = ____teamEnemy.unit.IsDead() || ____teamLibrarian.unit.IsDead() || ____teamEnemy.unit.IsExtinction() || ____teamLibrarian.unit.IsExtinction() || ____teamEnemy.isKeepedCard || ____teamLibrarian.isKeepedCard || ____teamEnemy.DiceExists() || ____teamLibrarian.DiceExists();
            if (!flag)
            {
                if (RedDice.TryGetValue(____teamEnemy.playingCard, out List<BattleDiceBehavior> dicesEnemy))
                {
                    foreach (var behavior in dicesEnemy)
                    {
                        behavior.ApplyDiceStatBonus(new DiceStatBonus { max = -3 });
                        ____teamEnemy.playingCard.AddDice(behavior);
                        var ability = behavior.abilityList.Find(x => x is RedDiceCardAbility);
                        if (ability != null)
                        {
                            ((RedDiceCardAbility)ability).BeforNewRoll(____teamEnemy, ____teamLibrarian);
                        }

                    }
                    ____teamEnemy.playingCard.NextDice();
                    RedDice.Remove(____teamEnemy.playingCard);
                }
                if (RedDice.TryGetValue(____teamLibrarian.playingCard, out List<BattleDiceBehavior> dicesLibrarian))
                {
                    foreach (var behavior in dicesLibrarian)
                    {
                        behavior.ApplyDiceStatBonus(new DiceStatBonus { max = -3 });
                        ____teamLibrarian.playingCard.AddDice(behavior);
                        var ability = behavior.abilityList.Find(x => x is RedDiceCardAbility);
                        if (ability != null)
                        {
                            ((RedDiceCardAbility)ability).BeforNewRoll(____teamEnemy, ____teamLibrarian);
                        }

                    }
                    ____teamLibrarian.playingCard.NextDice();
                    RedDice.Remove(____teamLibrarian.playingCard);
                }
            }
        }


    }

    public class PassiveAbilityBase_Don_Eyuil : PassiveAbilityBase
    {
        public virtual void OnStartBattleTheLast()
        {
        }
        public class OnStartBattleTheLastPatch
        {
            [HarmonyPatch(typeof(StageController), "ActivateStartBattleEffectPhase")]
            [HarmonyPostfix]
            public static void StageController_ActivateStartBattleEffectPhase_Post()
            {
                BattleObjectManager.instance.GetAliveList().Do(x => x.passiveDetail.PassiveList.FindAll(n => n is PassiveAbilityBase_Don_Eyuil).Do(y => (y as PassiveAbilityBase_Don_Eyuil).OnStartBattleTheLast()));
            }
        }
        public virtual void AfterApplyEnemyCard()
        {

        }
        [HarmonyPatch]
        public class AfterApplyEnemyCardPatch
        {
            public static MethodBase TargetMethod()
            {
                return typeof(StageController).GetMethod("ApplyEnemyCardPhase", AccessTools.all);
            }
            public static void Postfix(StageController __instance)
            {
                BattleObjectManager.instance.GetAliveList().Do(x => x.passiveDetail.PassiveList.FindAll(n => n is PassiveAbilityBase_Don_Eyuil).Do(y => (y as PassiveAbilityBase_Don_Eyuil).AfterApplyEnemyCard()));
            }
        }

        public virtual void BeforeRecoverHP(ref int v) { }

        [HarmonyPatch(typeof(BattleUnitModel), "RecoverHP")]
        [HarmonyPrefix]
        public static void BattleUnitModel_RecoverHP_Pre(BattleUnitModel __instance, ref int v)
        {
            foreach (var item in __instance.passiveDetail.PassiveList)
            {
                if (item is PassiveAbilityBase_Don_Eyuil)
                {
                    (item as PassiveAbilityBase_Don_Eyuil).BeforeRecoverHP(ref v);
                }
            }
        }
    }
    public class BattleUnitBuf_Don_Eyuil : BattleUnitBuf
    {
        public virtual bool CanForcelyAggro(BattleUnitModel target) => false;
        public class CanForcelyAggroPatch
        {
            [HarmonyPatch(typeof(BattleUnitModel), "CanChangeAttackTarget")]
            [HarmonyPrefix]
            public static bool BattleUnitModel_CanChangeAttackTarget_Prefix(BattleUnitModel __instance, BattleUnitModel target, ref bool __result)
            {
                foreach (var x in __instance.bufListDetail.GetActivatedBufList())
                {
                    if (!x.IsDestroyed() && x is BattleUnitBuf_Don_Eyuil)
                    {
                        if ((x as BattleUnitBuf_Don_Eyuil).CanForcelyAggro(target))
                        {
                            __result = true;
                            return false;
                        }
                    }
                }
                return true;
            }
        }
        public virtual void BeforeRecoverPlayPoint(ref int value)
        {

        }
        public class BeforeRecoverPlayPointPatch
        {
            [HarmonyPatch(typeof(BattlePlayingCardSlotDetail), "RecoverPlayPoint")]
            [HarmonyTranspiler]
            public unsafe static IEnumerable<CodeInstruction> BattlePlayingCardSlotDetail_RecoverPlayPoint_Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
                codes.InsertRange(0, new List<CodeInstruction>()
                {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldarga_S,1),
                    new CodeInstruction(OpCodes.Conv_U),
                    new CodeInstruction(OpCodes.Call).CallInternalDelegate<PatchTools.UnmanagedDelegateTypes.UnmanagedDelegate_2<BattlePlayingCardSlotDetail,int>>((BattlePlayingCardSlotDetail Detail,int* value)=>
                    {
                        var Model = Detail != null ? Detail.GetFieldValue<BattleUnitModel>("_self") : null;
                        if (Model != null)
                        {
                            foreach (var Buf in Model.bufListDetail.GetActivatedBufList())
                            {
                                if (!Buf.IsDestroyed() && Buf is BattleUnitBuf_Don_Eyuil)
                                {
                                    Debug.LogError("BeforeRecoverPlayPoint:" + *value);
                                    (Buf as BattleUnitBuf_Don_Eyuil).BeforeRecoverPlayPoint(ref *value);
                                }
                            }
                        }
                    }),
                    //new CodeInstruction(OpCodes.Call,AccessTools.Method(typeof(BeforeRecoverPlayPointPatch),"Trigger_RecoverPlayPoint_Before")),
                });
                return codes.AsEnumerable<CodeInstruction>();
            }
        }
        public virtual void BeforeRecoverHp(int v)
        {

        }
        public class BeforeRecoverHpPatch
        {
            [HarmonyPatch(typeof(BattleUnitModel), "RecoverHP")]
            [HarmonyPrefix]
            public static void BattleUnitModel_RecoverHP_Pre(BattleUnitModel __instance, int v)
            {
                __instance.bufListDetail.GetActivatedBufList().DoIf(cond => !cond.IsDestroyed() && cond is BattleUnitBuf_Don_Eyuil, x => (x as BattleUnitBuf_Don_Eyuil).BeforeRecoverHp(v));
            }
        }
        public virtual void BeforeAddEmotionCoin(EmotionCoinType CoinType, ref int Count)
        {

        }
        public class BeforeAddEmotionCoinPatch
        {
            [HarmonyPatch(typeof(BattleUnitEmotionDetail), "CreateEmotionCoin")]
            [HarmonyTranspiler]
            public unsafe static IEnumerable<CodeInstruction> BattleUnitEmotionDetail_CreateEmotionCoin_Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
                //Label? L ;
                codes.InsertRange(0, new List<CodeInstruction>()
                {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldarg_1),
                    new CodeInstruction(OpCodes.Ldarga_S,2),
                    new CodeInstruction(OpCodes.Conv_U),
                    new CodeInstruction(OpCodes.Call).CallInternalDelegate<PatchTools.UnmanagedDelegateTypes.UnmanagedDelegate_3<BattleUnitEmotionDetail,EmotionCoinType,int>>((BattleUnitEmotionDetail Detail, EmotionCoinType CoinType,int* Count)=>
                    {
                        var Model = Detail != null ? Detail.GetFieldValue<BattleUnitModel>("_self") : null;
                        if (Model != null)
                        {
                            foreach (var Buf in Model.bufListDetail.GetActivatedBufList())
                            {
                                if (!Buf.IsDestroyed() && Buf is BattleUnitBuf_Don_Eyuil)
                                {
                                    (Buf as BattleUnitBuf_Don_Eyuil).BeforeAddEmotionCoin(CoinType, ref *Count);
                                    Debug.LogError("BeforeAddEmotionCoin:" + CoinType + "," + *Count);
                                }
                            }
                        }
                    }),
                    //new CodeInstruction(OpCodes.Call,AccessTools.Method(typeof(BeforeAddEmotionCoinPatch),"Trigger_CreateEmotionCoin_Before")),
                });
                /*
                for (int i = 1; i < codes.Count; i++)
                {
                    if (codes[i].opcode == OpCodes.Call && codes[i].operand.ToString().Contains("get_MaximumEmotionLevel") && codes[i + 1].Branches(out L))
                    {
                        Debug.LogError("BBBBBBBBBBBBBBBBBBBBBB" + codes.FindIndex((CodeInstruction code) => code.labels.Contains(L.Value)));
                        codes.InsertRange(codes.FindIndex((CodeInstruction code) => code.labels.Contains(L.Value)),new List<CodeInstruction>()
                        { 
                            new CodeInstruction(OpCodes.Ldarg_0),
                            new CodeInstruction(OpCodes.Ldarg_1),
                            new CodeInstruction(OpCodes.Ldarga_S,2),
                            new CodeInstruction(OpCodes.Call,AccessTools.Method(typeof(BeforeAddEmotionCoinPatch),"Trigger_CreateEmotionCoin_Before")),
                        });
                      
                    }
                }
                */
                return codes.AsEnumerable<CodeInstruction>();
            }
        }
        public virtual void BeforeAddKeywordBuf(KeywordBuf BufType, ref int Stack)
        {

        }
        public virtual void BeforeAddKeywordBuf(BattleUnitModel Adder,KeywordBuf BufType, ref int Stack)
        {

        }
        public virtual void BeforeOtherUnitAddKeywordBuf(KeywordBuf BufType, BattleUnitModel Target, ref int Stack)
        {

        }
        public virtual void BeforeOtherUnitAddKeywordBuf(BattleUnitModel Adder, KeywordBuf BufType, BattleUnitModel Target, ref int Stack)
        {

        }
        public class BeforeAddKeywordBufPatch
        {
            [HarmonyPatch(typeof(BattleUnitBufListDetail), "AddKeywordBufThisRoundByEtc")]
            [HarmonyPatch(typeof(BattleUnitBufListDetail), "AddKeywordBufByEtc")]
            [HarmonyPatch(typeof(BattleUnitBufListDetail), "AddKeywordBufThisRoundByCard")]
            [HarmonyPatch(typeof(BattleUnitBufListDetail), "AddKeywordBufByCard")]
            [HarmonyPatch(typeof(BattleUnitBufListDetail), "AddKeywordBufNextNextByCard")]
            [HarmonyTranspiler]
            public unsafe static IEnumerable<CodeInstruction> BattleUnitBufListDetail_AddKeywordBuf_Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator ILcodegenerator)
            {
                List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
                Label l = ILcodegenerator.DefineLabel();
                codes[0].labels.Add(l);
                codes.InsertRange(0, new List<CodeInstruction>()
                {
                    new CodeInstruction(OpCodes.Ldarg_1),
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldfld,AccessTools.Field(typeof(BattleUnitBufListDetail),"_self")),
                    new CodeInstruction(OpCodes.Ldarga,2),
                    new CodeInstruction(OpCodes.Conv_U),
                    new CodeInstruction(OpCodes.Ldarg_3),
                    new CodeInstruction(OpCodes.Call).CallInternalDelegate<PatchTools.UnmanagedDelegateTypes.UnmanagedDelegate_3<KeywordBuf,BattleUnitModel,int,BattleUnitModel>>((KeywordBuf BufType, BattleUnitModel Target,int* Stack,BattleUnitModel Adder)=>
                    {
                        if(*Stack > 0)
                        {
                            Adder = Adder != null ? Adder : Target;
                            foreach (var Buf in Target.bufListDetail.GetActivatedBufList())
                            {
                                if (!Buf.IsDestroyed() && Buf is BattleUnitBuf_Don_Eyuil)
                                {
                                    (Buf as BattleUnitBuf_Don_Eyuil).BeforeAddKeywordBuf(BufType, ref *Stack);//Target = owner所以没有一参传入
                                    (Buf as BattleUnitBuf_Don_Eyuil).BeforeAddKeywordBuf(Adder, BufType, ref *Stack);
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
                                        (Buf as BattleUnitBuf_Don_Eyuil).BeforeOtherUnitAddKeywordBuf(BufType, Target, ref *Stack);
                                        (Buf as BattleUnitBuf_Don_Eyuil).BeforeOtherUnitAddKeywordBuf(Adder,BufType, Target, ref *Stack);
                                    }
                                }
                            }
                        }
                    }),
                    new CodeInstruction(OpCodes.Ldarg_2),
                    new CodeInstruction(OpCodes.Ldc_I4_0),
                    new CodeInstruction(OpCodes.Bgt_S,l),
                    new CodeInstruction(OpCodes.Ret),
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
                new List<BattleUnitBuf>(__instance.bufListDetail.GetActivatedBufList())
                    .ForEach(x =>
                    {
                        if (!x.IsDestroyed() && x is BattleUnitBuf_Don_Eyuil)
                        {
                            (x as BattleUnitBuf_Don_Eyuil).OnStartBattle();
                        }
                    });
            }
        }

        public virtual void AfterTakeBleedingDamage(int Dmg)
        {

        }
        public virtual void AfterOtherUnitTakeBleedingDamage(BattleUnitModel Unit, int Dmg)
        {

        }
        public class OnTakeBleedingDamagePatch
        {
            //public static void Trigger_BleedingDmg_After(BattleUnitModel Model, int dmg, KeywordBuf keyword)
            //{

            //}
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
                            new CodeInstruction(OpCodes.Call).CallInternalDelegate<PatchTools.UnmanagedDelegateTypes.UnmanagedDelegate<BattleUnitModel,int,KeywordBuf>>((BattleUnitModel Model, int dmg, KeywordBuf keyword)=>
                            {
                                if (keyword == KeywordBuf.Bleeding && dmg > 0)
                                {
                                    Debug.LogError(Model.Book.Name + "TakeBleedingDmg:" + dmg);
                                    Model.bufListDetail.GetActivatedBufList().DoIf(cond => !cond.IsDestroyed() && cond is BattleUnitBuf_Don_Eyuil, x => (x as BattleUnitBuf_Don_Eyuil).AfterTakeBleedingDamage(dmg));
                                    List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList();
                                    aliveList.Remove(Model);
                                    aliveList.Do(x1 => x1.bufListDetail.GetActivatedBufList().DoIf(cond => !cond.IsDestroyed() && cond is BattleUnitBuf_Don_Eyuil, x => (x as BattleUnitBuf_Don_Eyuil).AfterOtherUnitTakeBleedingDamage(Model, dmg)));
                                }
                            }),
                        });
                    }
                }
                return codes.AsEnumerable<CodeInstruction>();
            }
        }
        public virtual void OnGainEyuilBufStack(BattleUnitBuf_Don_Eyuil Buff, ref int stack)
        {

        }
        public virtual void OnOtherUnitGainEyuilBufStack(BattleUnitBuf_Don_Eyuil Buff, BattleUnitModel Target, ref int stack)
        {

        }
        public class OnGainEyuilBufStackPatch
        {
            public static void Trigger_GainEyuilBufStack(BattleUnitBuf_Don_Eyuil Buff, BattleUnitModel Target,ref int stack)
            {
                foreach (var Buf in Target.bufListDetail.GetActivatedBufList())
                {
                    if (!Buf.IsDestroyed() && Buf is BattleUnitBuf_Don_Eyuil)
                    {
                        (Buf as BattleUnitBuf_Don_Eyuil).OnGainEyuilBufStack(Buff, ref stack);
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
                            (Buf as BattleUnitBuf_Don_Eyuil).OnOtherUnitGainEyuilBufStack(Buff, Target,ref stack);
                        }
                    }
                }
            }
        }
        public virtual int GetMaxStack() => -1;
        public virtual void Add(int stack)
        {
            //stack = GetMaxStack() >= 0 ? GetMaxStack() - Math.Min(this.stack, GetMaxStack()) : stack;
            this.stack += stack;
            if (GetMaxStack() >= 0)
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
        public static List<T> GetAllBufOnField<T>(Faction? Faction = null, BufReadyType ReadyType = BufReadyType.ThisRound) where T : BattleUnitBuf_Don_Eyuil
        {
            List<BattleUnitModel> UnitList = Faction.HasValue ? BattleObjectManager.instance.GetAliveList(Faction.Value) : BattleObjectManager.instance.GetAliveList();
            List<T> ResultList = new List<T>() { };
            foreach (BattleUnitModel model in UnitList)
            {
                T BuffInstance = GetBuf<T>(model, ReadyType);
                if (BuffInstance != null)
                {
                    ResultList.Add(BuffInstance);
                }
            }
            return ResultList;
        }
        public static List<BattleUnitModel> GetAllUnitWithBuf<T>(Faction? Faction = null, BufReadyType ReadyType = BufReadyType.ThisRound) where T : BattleUnitBuf_Don_Eyuil
        {
            List<BattleUnitModel> UnitList = Faction.HasValue ? BattleObjectManager.instance.GetAliveList(Faction.Value) : BattleObjectManager.instance.GetAliveList();
            List<BattleUnitModel> ResultList = new List<BattleUnitModel>() { };
            foreach (BattleUnitModel model in UnitList)
            {
                T BuffInstance = GetBuf<T>(model, ReadyType);
                if (BuffInstance != null)
                {
                    ResultList.Add(model);
                }
            }
            return ResultList;
        }

        public virtual void OnUseBuf(ref int stack) { }

        public static bool UseBuf<T>(BattleUnitModel model, int stack) where T : BattleUnitBuf_Don_Eyuil
        {
            T BuffInstance = GetBuf<T>(model, BufReadyType.ThisRound);
            if (BuffInstance != null && BuffInstance.stack >= stack)
            {
                BuffInstance.OnUseBuf(ref stack);
                GainBuf<T>(model, -stack);
                //stack *= -1;
                //OnGainEyuilBufStackPatch.Trigger_GainEyuilBufStack(BuffInstance, model, ref stack);
                //BuffInstance.Add(stack);
                return true;
            }
            return false;
        }

        public static T GainBuf<T>(BattleUnitModel model, int stack, BufReadyType ReadyType = BufReadyType.ThisRound) where T : BattleUnitBuf_Don_Eyuil
        {
            T BuffInstance = GetOrAddBuf<T>(model, ReadyType);
            if (BuffInstance != null)
            {
                OnGainEyuilBufStackPatch.Trigger_GainEyuilBufStack(BuffInstance, model, ref stack);
                BuffInstance.Add(stack);
            }
            return BuffInstance;
        }
        public static T GetBuf<T>(BattleUnitModel model, BufReadyType ReadyType = BufReadyType.ThisRound) where T : BattleUnitBuf_Don_Eyuil
        {
            switch (ReadyType)
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
        public static T GetOrAddBuf<T>(BattleUnitModel model, BufReadyType ReadyType = BufReadyType.ThisRound) where T : BattleUnitBuf_Don_Eyuil
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

        public BattleUnitModel owner { get { return this._owner; } }
    }
    public static class ExtraMethods
    {
        public static void GiveDamage_SubTarget(this BattleDiceBehavior behavior, BattleUnitModel OriginalTarget, int EnemyCount)
        {
            var owner = behavior.owner;
            if (owner != null)
            {
                if (!behavior.HasFlag(TKS_BloodFiend_Initializer.TKS_EnumExtension.DiceFlagExtension.HasGivenDamage_SubTarget))
                {
                    var AliveList = BattleObjectManager.instance.GetAliveList_opponent(owner.faction);
                    AliveList.Remove(OriginalTarget);
                    behavior.GiveDamage_SubTarget((EnemyCount == -1 ? AliveList : MyTools.TKSRandomUtil(AliveList.ToList(), EnemyCount, false, false)).ToArray());
                }
            }
        }
        public static void GiveDamage_SubTarget(this BattleDiceBehavior behavior, params BattleUnitModel[] target)
        {
            if (behavior != null && !behavior.HasFlag(TKS_BloodFiend_Initializer.TKS_EnumExtension.DiceFlagExtension.HasGivenDamage_SubTarget))
            {
                behavior.AddFlag(TKS_BloodFiend_Initializer.TKS_EnumExtension.DiceFlagExtension.HasGivenDamage_SubTarget);
                if (behavior.owner.battleCardResultLog == null) { behavior.owner.battleCardResultLog = new BattleCardTotalResult(behavior.card); }
                behavior.card.earlyTarget = behavior.card.target;
                behavior.card.earlyTargetOrder = behavior.card.targetSlotOrder;
                target.Do(x =>
                {
                    if (x.battleCardResultLog == null) { x.battleCardResultLog = new BattleCardTotalResult(x.currentDiceAction); }
                    //Debug.LogError(behavior.Detail + "givesubdamage:" + x.Book.Name);
                    behavior.SetFieldValue<BattleDiceBehavior>("_targetDice", null);
                    behavior.card.target = x;
                    behavior.card.targetSlotOrder = 0;
                    behavior.GiveDamage(x);
                });
                behavior.card.target = behavior.card.earlyTarget;
                behavior.card.targetSlotOrder = behavior.card.earlyTargetOrder;
            }

        }
    }
}
