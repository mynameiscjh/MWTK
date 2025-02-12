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
                    new CodeInstruction(OpCodes.Call).WithInternalDelegate<PatchTools.UnmanagedDelegateTypes.UnmanagedDelegate_2<BattlePlayingCardSlotDetail,int>>((BattlePlayingCardSlotDetail Detail,int* value)=>
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
                    new CodeInstruction(OpCodes.Call).WithInternalDelegate<PatchTools.UnmanagedDelegateTypes.UnmanagedDelegate_3<BattleUnitEmotionDetail,EmotionCoinType,int>>((BattleUnitEmotionDetail Detail, EmotionCoinType CoinType,int* Count)=>
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
                    new CodeInstruction(OpCodes.Call).WithInternalDelegate<PatchTools.UnmanagedDelegateTypes.UnmanagedDelegate_3<KeywordBuf,BattleUnitModel,int,BattleUnitModel>>((KeywordBuf BufType, BattleUnitModel Target,int* Stack,BattleUnitModel Adder)=>
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
                __instance.bufListDetail.GetActivatedBufList().ForEach(x =>
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
                            new CodeInstruction(OpCodes.Call).WithInternalDelegate<PatchTools.UnmanagedDelegateTypes.UnmanagedDelegate<BattleUnitModel,int,KeywordBuf>>((BattleUnitModel Model, int dmg, KeywordBuf keyword)=>
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
    public static class PatchTools
    {
        public static class UnmanagedDelegateTypes
        {
            public static void Inner_UnmanagedDelegateTypesBuilder(int ArgNum)
            {
                IEnumerable<IEnumerable<T>> GetCombinations<T>(IEnumerable<T> elements, int k)
                {
                    return k == 0 ? new[] { new T[0] } :
                           elements.SelectMany((e, i) =>
                               GetCombinations(elements.Skip(i + 1), k - 1).Select(c => new[] { e }.Concat(c)));
                }
                for (int i = 1; i <= 5; i++)
                {
                    string GenereDeclare = "in T";
                    for (int GeneC = 0; GeneC < i - 1; GeneC++)
                    {
                        GenereDeclare += ",in T" + (GeneC + 1);
                    }
                    string ArgDeclare = "";
                    List<int> N = Enumerable.Range(1, i).ToList();
                    for (int m = 1; m <= N.Count; m++)
                    {
                        var combinations = GetCombinations(N, m);
                        foreach (var combination in combinations)
                        {
                            ArgDeclare = "";
                            for (int q = 1; q <= N.Count; q++)
                            {
                                ArgDeclare += $",T{(q - 1 != 0 ? (q - 1).ToString() : "")}{(combination.Any(x => x == q) ? "*" : "")} A{(q - 1 != 0 ? (q - 1).ToString() : "")}";
                            }
                            Console.WriteLine($"public unsafe delegate void UnmanagedDelegate_{String.Join("", combination)}<{GenereDeclare}>({ArgDeclare.Substring(1)});");
                        }
                    }
                    ArgDeclare = "";
                    for (int q = 1; q <= N.Count; q++)
                    {
                        ArgDeclare += $",T{(q - 1 != 0 ? (q - 1).ToString() : "")} A{(q - 1 != 0 ? (q - 1).ToString() : "")}";
                    }
                    Console.WriteLine($"public unsafe delegate void UnmanagedDelegate<{GenereDeclare}>({ArgDeclare.Substring(1)});");
                }
            }
            public static void Inner_UnmanagedDelegateTypes_WithRetBuilder(int ArgNum)
            {
                IEnumerable<IEnumerable<T>> GetCombinations<T>(IEnumerable<T> elements, int k)
                {
                    return k == 0 ? new[] { new T[0] } :
                           elements.SelectMany((e, i) =>
                               GetCombinations(elements.Skip(i + 1), k - 1).Select(c => new[] { e }.Concat(c)));
                }
                for (int i = 1; i <= 5; ArgNum++)
                {
                    string GenereDeclare = "out TResult,in T";
                    for (int GeneC = 0; GeneC < i - 1; GeneC++)
                    {
                        GenereDeclare += ",in T" + (GeneC + 1);
                    }
                    string ArgDeclare = "";
                    List<int> N = Enumerable.Range(1, i).ToList();
                    for (int m = 1; m <= N.Count; m++)
                    {
                        var combinations = GetCombinations(N, m);
                        foreach (var combination in combinations)
                        {
                            ArgDeclare = "";
                            for (int q = 1; q <= N.Count; q++)
                            {
                                ArgDeclare += $",T{(q - 1 != 0 ? (q - 1).ToString() : "")}{(combination.Any(x => x == q) ? "*" : "")} A{(q - 1 != 0 ? (q - 1).ToString() : "")}";
                            }
                            Console.WriteLine($"public unsafe delegate void UnmanagedDelegateWithRet_{String.Join("", combination)}<{GenereDeclare}>({ArgDeclare.Substring(1)});");
                        }
                    }
                    ArgDeclare = "";
                    for (int q = 1; q <= N.Count; q++)
                    {
                        ArgDeclare += $",T{(q - 1 != 0 ? (q - 1).ToString() : "")} A{(q - 1 != 0 ? (q - 1).ToString() : "")}";
                    }
                    Console.WriteLine($"public unsafe delegate void UnmanagedDelegateWithRet<{GenereDeclare}>({ArgDeclare.Substring(1)});");
                }
            }
            public unsafe delegate TResult UnmanagedDelegateWithRet_1<out TResult, in T>(T* A);
            public unsafe delegate TResult UnmanagedDelegateWithRet<out TResult, in T>(T A);
            public unsafe delegate TResult UnmanagedDelegateWithRet_1<out TResult, in T, in T1>(T* A, T1 A1);
            public unsafe delegate TResult UnmanagedDelegateWithRet_2<out TResult, in T, in T1>(T A, T1* A1);
            public unsafe delegate TResult UnmanagedDelegateWithRet_12<out TResult, in T, in T1>(T* A, T1* A1);
            public unsafe delegate TResult UnmanagedDelegateWithRet<out TResult, in T, in T1>(T A, T1 A1);
            public unsafe delegate TResult UnmanagedDelegateWithRet_1<out TResult, in T, in T1, in T2>(T* A, T1 A1, T2 A2);
            public unsafe delegate TResult UnmanagedDelegateWithRet_2<out TResult, in T, in T1, in T2>(T A, T1* A1, T2 A2);
            public unsafe delegate TResult UnmanagedDelegateWithRet_3<out TResult, in T, in T1, in T2>(T A, T1 A1, T2* A2);
            public unsafe delegate TResult UnmanagedDelegateWithRet_12<out TResult, in T, in T1, in T2>(T* A, T1* A1, T2 A2);
            public unsafe delegate TResult UnmanagedDelegateWithRet_13<out TResult, in T, in T1, in T2>(T* A, T1 A1, T2* A2);
            public unsafe delegate TResult UnmanagedDelegateWithRet_23<out TResult, in T, in T1, in T2>(T A, T1* A1, T2* A2);
            public unsafe delegate TResult UnmanagedDelegateWithRet_123<out TResult, in T, in T1, in T2>(T* A, T1* A1, T2* A2);
            public unsafe delegate TResult UnmanagedDelegateWithRet<out TResult, in T, in T1, in T2>(T A, T1 A1, T2 A2);
            public unsafe delegate TResult UnmanagedDelegateWithRet_1<out TResult, in T, in T1, in T2, in T3>(T* A, T1 A1, T2 A2, T3 A3);
            public unsafe delegate TResult UnmanagedDelegateWithRet_2<out TResult, in T, in T1, in T2, in T3>(T A, T1* A1, T2 A2, T3 A3);
            public unsafe delegate TResult UnmanagedDelegateWithRet_3<out TResult, in T, in T1, in T2, in T3>(T A, T1 A1, T2* A2, T3 A3);
            public unsafe delegate TResult UnmanagedDelegateWithRet_4<out TResult, in T, in T1, in T2, in T3>(T A, T1 A1, T2 A2, T3* A3);
            public unsafe delegate TResult UnmanagedDelegateWithRet_12<out TResult, in T, in T1, in T2, in T3>(T* A, T1* A1, T2 A2, T3 A3); 
            public unsafe delegate TResult UnmanagedDelegateWithRet_13<out TResult, in T, in T1, in T2, in T3>(T* A, T1 A1, T2* A2, T3 A3); 
            public unsafe delegate TResult UnmanagedDelegateWithRet_14<out TResult, in T, in T1, in T2, in T3>(T* A, T1 A1, T2 A2, T3* A3); 
            public unsafe delegate TResult UnmanagedDelegateWithRet_23<out TResult, in T, in T1, in T2, in T3>(T A, T1* A1, T2* A2, T3 A3); 
            public unsafe delegate TResult UnmanagedDelegateWithRet_24<out TResult, in T, in T1, in T2, in T3>(T A, T1* A1, T2 A2, T3* A3); 
            public unsafe delegate TResult UnmanagedDelegateWithRet_34<out TResult, in T, in T1, in T2, in T3>(T A, T1 A1, T2* A2, T3* A3); 
            public unsafe delegate TResult UnmanagedDelegateWithRet_123<out TResult, in T, in T1, in T2, in T3>(T* A, T1* A1, T2* A2, T3 A3);
            public unsafe delegate TResult UnmanagedDelegateWithRet_124<out TResult, in T, in T1, in T2, in T3>(T* A, T1* A1, T2 A2, T3* A3);
            public unsafe delegate TResult UnmanagedDelegateWithRet_134<out TResult, in T, in T1, in T2, in T3>(T* A, T1 A1, T2* A2, T3* A3);
            public unsafe delegate TResult UnmanagedDelegateWithRet_234<out TResult, in T, in T1, in T2, in T3>(T A, T1* A1, T2* A2, T3* A3);
            public unsafe delegate TResult UnmanagedDelegateWithRet_1234<out TResult, in T, in T1, in T2, in T3>(T* A, T1* A1, T2* A2, T3* A3);
            public unsafe delegate TResult UnmanagedDelegateWithRet<out TResult, in T, in T1, in T2, in T3>(T A, T1 A1, T2 A2, T3 A3);
            public unsafe delegate TResult UnmanagedDelegateWithRet_1<out TResult, in T, in T1, in T2, in T3, in T4>(T* A, T1 A1, T2 A2, T3 A3, T4 A4);
            public unsafe delegate TResult UnmanagedDelegateWithRet_2<out TResult, in T, in T1, in T2, in T3, in T4>(T A, T1* A1, T2 A2, T3 A3, T4 A4);
            public unsafe delegate TResult UnmanagedDelegateWithRet_3<out TResult, in T, in T1, in T2, in T3, in T4>(T A, T1 A1, T2* A2, T3 A3, T4 A4);
            public unsafe delegate TResult UnmanagedDelegateWithRet_4<out TResult, in T, in T1, in T2, in T3, in T4>(T A, T1 A1, T2 A2, T3* A3, T4 A4);
            public unsafe delegate TResult UnmanagedDelegateWithRet_5<out TResult, in T, in T1, in T2, in T3, in T4>(T A, T1 A1, T2 A2, T3 A3, T4* A4);
            public unsafe delegate TResult UnmanagedDelegateWithRet_12<out TResult, in T, in T1, in T2, in T3, in T4>(T* A, T1* A1, T2 A2, T3 A3, T4 A4);
            public unsafe delegate TResult UnmanagedDelegateWithRet_13<out TResult, in T, in T1, in T2, in T3, in T4>(T* A, T1 A1, T2* A2, T3 A3, T4 A4);
            public unsafe delegate TResult UnmanagedDelegateWithRet_14<out TResult, in T, in T1, in T2, in T3, in T4>(T* A, T1 A1, T2 A2, T3* A3, T4 A4);
            public unsafe delegate TResult UnmanagedDelegateWithRet_15<out TResult, in T, in T1, in T2, in T3, in T4>(T* A, T1 A1, T2 A2, T3 A3, T4* A4);
            public unsafe delegate TResult UnmanagedDelegateWithRet_23<out TResult, in T, in T1, in T2, in T3, in T4>(T A, T1* A1, T2* A2, T3 A3, T4 A4);
            public unsafe delegate TResult UnmanagedDelegateWithRet_24<out TResult, in T, in T1, in T2, in T3, in T4>(T A, T1* A1, T2 A2, T3* A3, T4 A4);
            public unsafe delegate TResult UnmanagedDelegateWithRet_25<out TResult, in T, in T1, in T2, in T3, in T4>(T A, T1* A1, T2 A2, T3 A3, T4* A4);
            public unsafe delegate TResult UnmanagedDelegateWithRet_34<out TResult, in T, in T1, in T2, in T3, in T4>(T A, T1 A1, T2* A2, T3* A3, T4 A4);
            public unsafe delegate TResult UnmanagedDelegateWithRet_35<out TResult, in T, in T1, in T2, in T3, in T4>(T A, T1 A1, T2* A2, T3 A3, T4* A4);
            public unsafe delegate TResult UnmanagedDelegateWithRet_45<out TResult, in T, in T1, in T2, in T3, in T4>(T A, T1 A1, T2 A2, T3* A3, T4* A4);
            public unsafe delegate TResult UnmanagedDelegateWithRet_123<out TResult, in T, in T1, in T2, in T3, in T4>(T* A, T1* A1, T2* A2, T3 A3, T4 A4);
            public unsafe delegate TResult UnmanagedDelegateWithRet_124<out TResult, in T, in T1, in T2, in T3, in T4>(T* A, T1* A1, T2 A2, T3* A3, T4 A4);
            public unsafe delegate TResult UnmanagedDelegateWithRet_125<out TResult, in T, in T1, in T2, in T3, in T4>(T* A, T1* A1, T2 A2, T3 A3, T4* A4);
            public unsafe delegate TResult UnmanagedDelegateWithRet_134<out TResult, in T, in T1, in T2, in T3, in T4>(T* A, T1 A1, T2* A2, T3* A3, T4 A4);
            public unsafe delegate TResult UnmanagedDelegateWithRet_135<out TResult, in T, in T1, in T2, in T3, in T4>(T* A, T1 A1, T2* A2, T3 A3, T4* A4);
            public unsafe delegate TResult UnmanagedDelegateWithRet_145<out TResult, in T, in T1, in T2, in T3, in T4>(T* A, T1 A1, T2 A2, T3* A3, T4* A4);
            public unsafe delegate TResult UnmanagedDelegateWithRet_234<out TResult, in T, in T1, in T2, in T3, in T4>(T A, T1* A1, T2* A2, T3* A3, T4 A4);
            public unsafe delegate TResult UnmanagedDelegateWithRet_235<out TResult, in T, in T1, in T2, in T3, in T4>(T A, T1* A1, T2* A2, T3 A3, T4* A4);
            public unsafe delegate TResult UnmanagedDelegateWithRet_245<out TResult, in T, in T1, in T2, in T3, in T4>(T A, T1* A1, T2 A2, T3* A3, T4* A4);
            public unsafe delegate TResult UnmanagedDelegateWithRet_345<out TResult, in T, in T1, in T2, in T3, in T4>(T A, T1 A1, T2* A2, T3* A3, T4* A4);
            public unsafe delegate TResult UnmanagedDelegateWithRet_1234<out TResult, in T, in T1, in T2, in T3, in T4>(T* A, T1* A1, T2* A2, T3* A3, T4 A4);
            public unsafe delegate TResult UnmanagedDelegateWithRet_1235<out TResult, in T, in T1, in T2, in T3, in T4>(T* A, T1* A1, T2* A2, T3 A3, T4* A4);
            public unsafe delegate TResult UnmanagedDelegateWithRet_1245<out TResult, in T, in T1, in T2, in T3, in T4>(T* A, T1* A1, T2 A2, T3* A3, T4* A4);
            public unsafe delegate TResult UnmanagedDelegateWithRet_1345<out TResult, in T, in T1, in T2, in T3, in T4>(T* A, T1 A1, T2* A2, T3* A3, T4* A4);
            public unsafe delegate TResult UnmanagedDelegateWithRet_2345<out TResult, in T, in T1, in T2, in T3, in T4>(T A, T1* A1, T2* A2, T3* A3, T4* A4);
            public unsafe delegate TResult UnmanagedDelegateWithRet_12345<out TResult, in T, in T1, in T2, in T3, in T4>(T* A, T1* A1, T2* A2, T3* A3, T4* A4);
            public unsafe delegate TResult UnmanagedDelegateWithRet<out TResult, in T, in T1, in T2, in T3, in T4>(T A, T1 A1, T2 A2, T3 A3, T4 A4);
            public unsafe delegate void UnmanagedDelegate_1<in T>(T* A);
            public unsafe delegate void UnmanagedDelegate<in T>(T A);
            public unsafe delegate void UnmanagedDelegate_1<in T, in T1>(T* A, T1 A1);
            public unsafe delegate void UnmanagedDelegate_2<in T, in T1>(T A, T1* A1);
            public unsafe delegate void UnmanagedDelegate_12<in T, in T1>(T* A, T1* A1);
            public unsafe delegate void UnmanagedDelegate<in T, in T1>(T A, T1 A1);
            public unsafe delegate void UnmanagedDelegate_1<in T, in T1, in T2>(T* A, T1 A1, T2 A2);
            public unsafe delegate void UnmanagedDelegate_2<in T, in T1, in T2>(T A, T1* A1, T2 A2);
            public unsafe delegate void UnmanagedDelegate_3<in T, in T1, in T2>(T A, T1 A1, T2* A2);
            public unsafe delegate void UnmanagedDelegate_12<in T, in T1, in T2>(T* A, T1* A1, T2 A2);
            public unsafe delegate void UnmanagedDelegate_13<in T, in T1, in T2>(T* A, T1 A1, T2* A2);
            public unsafe delegate void UnmanagedDelegate_23<in T, in T1, in T2>(T A, T1* A1, T2* A2);
            public unsafe delegate void UnmanagedDelegate_123<in T, in T1, in T2>(T* A, T1* A1, T2* A2);
            public unsafe delegate void UnmanagedDelegate<in T, in T1, in T2>(T A, T1 A1, T2 A2);
            public unsafe delegate void UnmanagedDelegate_1<in T, in T1, in T2, in T3>(T* A, T1 A1, T2 A2, T3 A3);
            public unsafe delegate void UnmanagedDelegate_2<in T, in T1, in T2, in T3>(T A, T1* A1, T2 A2, T3 A3);
            public unsafe delegate void UnmanagedDelegate_3<in T, in T1, in T2, in T3>(T A, T1 A1, T2* A2, T3 A3);
            public unsafe delegate void UnmanagedDelegate_4<in T, in T1, in T2, in T3>(T A, T1 A1, T2 A2, T3* A3);
            public unsafe delegate void UnmanagedDelegate_12<in T, in T1, in T2, in T3>(T* A, T1* A1, T2 A2, T3 A3);
            public unsafe delegate void UnmanagedDelegate_13<in T, in T1, in T2, in T3>(T* A, T1 A1, T2* A2, T3 A3);
            public unsafe delegate void UnmanagedDelegate_14<in T, in T1, in T2, in T3>(T* A, T1 A1, T2 A2, T3* A3);
            public unsafe delegate void UnmanagedDelegate_23<in T, in T1, in T2, in T3>(T A, T1* A1, T2* A2, T3 A3);
            public unsafe delegate void UnmanagedDelegate_24<in T, in T1, in T2, in T3>(T A, T1* A1, T2 A2, T3* A3);
            public unsafe delegate void UnmanagedDelegate_34<in T, in T1, in T2, in T3>(T A, T1 A1, T2* A2, T3* A3);
            public unsafe delegate void UnmanagedDelegate_123<in T, in T1, in T2, in T3>(T* A, T1* A1, T2* A2, T3 A3);
            public unsafe delegate void UnmanagedDelegate_124<in T, in T1, in T2, in T3>(T* A, T1* A1, T2 A2, T3* A3);
            public unsafe delegate void UnmanagedDelegate_134<in T, in T1, in T2, in T3>(T* A, T1 A1, T2* A2, T3* A3);
            public unsafe delegate void UnmanagedDelegate_234<in T, in T1, in T2, in T3>(T A, T1* A1, T2* A2, T3* A3);
            public unsafe delegate void UnmanagedDelegate_1234<in T, in T1, in T2, in T3>(T* A, T1* A1, T2* A2, T3* A3);
            public unsafe delegate void UnmanagedDelegate<in T, in T1, in T2, in T3>(T A, T1 A1, T2 A2, T3 A3);
            public unsafe delegate void UnmanagedDelegate_1<in T, in T1, in T2, in T3, in T4>(T* A, T1 A1, T2 A2, T3 A3, T4 A4);
            public unsafe delegate void UnmanagedDelegate_2<in T, in T1, in T2, in T3, in T4>(T A, T1* A1, T2 A2, T3 A3, T4 A4);
            public unsafe delegate void UnmanagedDelegate_3<in T, in T1, in T2, in T3, in T4>(T A, T1 A1, T2* A2, T3 A3, T4 A4);
            public unsafe delegate void UnmanagedDelegate_4<in T, in T1, in T2, in T3, in T4>(T A, T1 A1, T2 A2, T3* A3, T4 A4);
            public unsafe delegate void UnmanagedDelegate_5<in T, in T1, in T2, in T3, in T4>(T A, T1 A1, T2 A2, T3 A3, T4* A4);
            public unsafe delegate void UnmanagedDelegate_12<in T, in T1, in T2, in T3, in T4>(T* A, T1* A1, T2 A2, T3 A3, T4 A4);
            public unsafe delegate void UnmanagedDelegate_13<in T, in T1, in T2, in T3, in T4>(T* A, T1 A1, T2* A2, T3 A3, T4 A4);
            public unsafe delegate void UnmanagedDelegate_14<in T, in T1, in T2, in T3, in T4>(T* A, T1 A1, T2 A2, T3* A3, T4 A4);
            public unsafe delegate void UnmanagedDelegate_15<in T, in T1, in T2, in T3, in T4>(T* A, T1 A1, T2 A2, T3 A3, T4* A4);
            public unsafe delegate void UnmanagedDelegate_23<in T, in T1, in T2, in T3, in T4>(T A, T1* A1, T2* A2, T3 A3, T4 A4);
            public unsafe delegate void UnmanagedDelegate_24<in T, in T1, in T2, in T3, in T4>(T A, T1* A1, T2 A2, T3* A3, T4 A4);
            public unsafe delegate void UnmanagedDelegate_25<in T, in T1, in T2, in T3, in T4>(T A, T1* A1, T2 A2, T3 A3, T4* A4);
            public unsafe delegate void UnmanagedDelegate_34<in T, in T1, in T2, in T3, in T4>(T A, T1 A1, T2* A2, T3* A3, T4 A4);
            public unsafe delegate void UnmanagedDelegate_35<in T, in T1, in T2, in T3, in T4>(T A, T1 A1, T2* A2, T3 A3, T4* A4);
            public unsafe delegate void UnmanagedDelegate_45<in T, in T1, in T2, in T3, in T4>(T A, T1 A1, T2 A2, T3* A3, T4* A4);
            public unsafe delegate void UnmanagedDelegate_123<in T, in T1, in T2, in T3, in T4>(T* A, T1* A1, T2* A2, T3 A3, T4 A4);
            public unsafe delegate void UnmanagedDelegate_124<in T, in T1, in T2, in T3, in T4>(T* A, T1* A1, T2 A2, T3* A3, T4 A4);
            public unsafe delegate void UnmanagedDelegate_125<in T, in T1, in T2, in T3, in T4>(T* A, T1* A1, T2 A2, T3 A3, T4* A4);
            public unsafe delegate void UnmanagedDelegate_134<in T, in T1, in T2, in T3, in T4>(T* A, T1 A1, T2* A2, T3* A3, T4 A4);
            public unsafe delegate void UnmanagedDelegate_135<in T, in T1, in T2, in T3, in T4>(T* A, T1 A1, T2* A2, T3 A3, T4* A4);
            public unsafe delegate void UnmanagedDelegate_145<in T, in T1, in T2, in T3, in T4>(T* A, T1 A1, T2 A2, T3* A3, T4* A4);
            public unsafe delegate void UnmanagedDelegate_234<in T, in T1, in T2, in T3, in T4>(T A, T1* A1, T2* A2, T3* A3, T4 A4);
            public unsafe delegate void UnmanagedDelegate_235<in T, in T1, in T2, in T3, in T4>(T A, T1* A1, T2* A2, T3 A3, T4* A4);
            public unsafe delegate void UnmanagedDelegate_245<in T, in T1, in T2, in T3, in T4>(T A, T1* A1, T2 A2, T3* A3, T4* A4);
            public unsafe delegate void UnmanagedDelegate_345<in T, in T1, in T2, in T3, in T4>(T A, T1 A1, T2* A2, T3* A3, T4* A4);
            public unsafe delegate void UnmanagedDelegate_1234<in T, in T1, in T2, in T3, in T4>(T* A, T1* A1, T2* A2, T3* A3, T4 A4);
            public unsafe delegate void UnmanagedDelegate_1235<in T, in T1, in T2, in T3, in T4>(T* A, T1* A1, T2* A2, T3 A3, T4* A4);
            public unsafe delegate void UnmanagedDelegate_1245<in T, in T1, in T2, in T3, in T4>(T* A, T1* A1, T2 A2, T3* A3, T4* A4);
            public unsafe delegate void UnmanagedDelegate_1345<in T, in T1, in T2, in T3, in T4>(T* A, T1 A1, T2* A2, T3* A3, T4* A4);
            public unsafe delegate void UnmanagedDelegate_2345<in T, in T1, in T2, in T3, in T4>(T A, T1* A1, T2* A2, T3* A3, T4* A4);
            public unsafe delegate void UnmanagedDelegate_12345<in T, in T1, in T2, in T3, in T4>(T* A, T1* A1, T2* A2, T3* A3, T4* A4);
            public unsafe delegate void UnmanagedDelegate<in T, in T1, in T2, in T3, in T4>(T A, T1 A1, T2 A2, T3 A3, T4 A4);

        }
        public static Dictionary<string, Delegate> InternalDelegateCache = new Dictionary<string, Delegate>() { };
        public unsafe static CodeInstruction DefinitionInternalDelegate<T>(T action,string CacheKeyPostfix = "", string CacheKey = null) where T : Delegate
        {
            Type[] Arg = (from x in action.Method.GetParameters()
                          select x.ParameterType).ToArray<Type>();
            DynamicMethodDefinition dynamicMethodDefinition = new DynamicMethodDefinition(action.Method.Name, action.Method.ReturnType, Arg);
            ILGenerator ilgenerator = dynamicMethodDefinition.GetILGenerator();
            Type TargetType = action.Target.GetType();
            string DelegateInMemoryCacheKey = (String.IsNullOrWhiteSpace(CacheKey) ? new System.Diagnostics.StackFrame(1).GetMethod().DeclaringType.Name + "_Trigger" : CacheKey) + (String.IsNullOrWhiteSpace(CacheKeyPostfix) ? "" : $"_{CacheKeyPostfix}");
            if (!InternalDelegateCache.ContainsKey(DelegateInMemoryCacheKey)) { InternalDelegateCache.Add(DelegateInMemoryCacheKey, action); }
            if ((action.Target != null && TargetType.GetFields().Any((FieldInfo x) => !x.IsStatic)) || action.Target == null)
            {
                ilgenerator.Emit(OpCodes.Ldsfld, AccessTools.Field(typeof(PatchTools), "InternalDelegateCache"));
                ilgenerator.Emit(OpCodes.Ldstr, DelegateInMemoryCacheKey);
                ilgenerator.Emit(OpCodes.Callvirt, AccessTools.Method(typeof(Dictionary<int, Delegate>), "get_Item"));
            }
            else
            {
                if (action.Target == null)
                {
                    ilgenerator.Emit(OpCodes.Ldnull);
                }
                else
                {
                    ilgenerator.Emit(OpCodes.Newobj, AccessTools.FirstConstructor(TargetType, (ConstructorInfo x) => x.GetParameters().Length == 0 && !x.IsStatic));
                }
                ilgenerator.Emit(OpCodes.Ldftn, action.Method);
                ilgenerator.Emit(OpCodes.Newobj, AccessTools.Constructor(typeof(T), new Type[]
                {
                    typeof(object),
                    typeof(IntPtr)
                }, false));
            }
            for (int i = 0; i < Arg.Length; i++)
            {
                ilgenerator.Emit(OpCodes.Ldarg_S, (short)i);
            }
            ilgenerator.Emit(OpCodes.Callvirt, AccessTools.Method(typeof(T), "Invoke"));
            ilgenerator.Emit(OpCodes.Ret);
            return new CodeInstruction(OpCodes.Call, dynamicMethodDefinition.Generate());
        }

        /// <summary>
        /// <para>根据传入的委托在InternalDelegateCache内生成一个默认为该方法调用者所处类的类名作为Key储存于该字典内的内存内委托 并返回一条调用该委托的IL语句</para>
        /// <para>*****重点：对于Ref的处理 使用PatchTools.UnmanagedDelegateTypes类下的委托 委托的命名规范为UnmanagedDelegate[是否带有返回值:WithRet][_类型为指针的参数id(从0开始且不加间隔 从左向右打即可)]</para>
        /// <para>例:(int a, int b, string* c, float* d) => {return "aa"}的需要返回string的委托 使用UnmanagedDelegateWithRet_34[[string, int, int, string, float]]作为泛型参数 (双中括号换尖括号 智能注释里不让打)</para>
        /// <para>*****重点II：在调用该方法返回的IL字段前的参数准备中 对于指针类型的参数处理方式与处理Ref一致：即读取地址到堆栈上即可 但是指针需要再加一条Conv_U(部分情况会有不同 但大部分情况你用Conv_U指定是对的 具体可以参考官方Opcodes对这几个Conv的解释)</para>
        /// <para>例II:(LdLoca_S,1)，(Conv_U,null),[本方法]</para>
        /// 若一个Tranpiler内生成了两个及以上的Delegate请从第二个开始使用CacheKeyPostfix参数命名一个标志后缀(随便打就可以 只是为了区分key)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="postFixCodeIns"></param>
        /// <param name="action"></param>
        /// <param name="CacheKeyPostfix">若一个Tranpiler内生成了两个及以上的Delegate请从第二个开始使用这个参数命名</param>
        /// <returns>new CodeInstruction(Opcodes.Call,内存内生成的调用传入的委托的方法)->可以理解成new CodeInstruction(Opcodes.CallVirt,传入委托.Invoke)->其实就是调用传入委托</returns>
        public static CodeInstruction WithInternalDelegate<T>(this CodeInstruction postFixCodeIns, T action, string CacheKeyPostfix = "") where T : Delegate
        {
            return DefinitionInternalDelegate<T>(action, CacheKeyPostfix, new System.Diagnostics.StackFrame(1).GetMethod().DeclaringType.Name + "_Trigger");
        }
        /// <summary>
        /// 尝试根据给定的当前类型与CacheKeyPostfix获取内存内存储的委托
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="CacheKeyPostfix"></param>
        /// <returns></returns>
        public static Delegate GetInternalDelegate(this Type Type, string CacheKeyPostfix = "")
        {
            Delegate Result = null;
            if (Type != null)
            {
                PatchTools.InternalDelegateCache.TryGetValue(Type.Name + "_Trigger" + (String.IsNullOrWhiteSpace(CacheKeyPostfix) ? "" : $"_{CacheKeyPostfix}"),out Result);
            }
            return Result;
        }

        public static Delegate GetInternalDelegate<T>(this Type Type, string CacheKeyPostfix = "") where T:Delegate
        {
            Delegate Result = null;
            Result = GetInternalDelegate(Type, CacheKeyPostfix);
            if(Result != null)
            {
                return (T)Result;
            }
            return Result;
        }
    }
    public static class MyTools
    {

        public static bool ISNULL(this object obj, params string[] names)
        {
            object temp = obj;
            foreach (string name in names)
            {
                if (temp == null)
                {
                    return false;
                }
                temp = temp.GetFieldValue(null, name);
            }
            return true;
        }
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
        public static CustomMapHandler CMH
        {
            get
            {
                return CustomMapHandler.GetCMU(TKS_BloodFiend_Initializer.packageId);
            }
        }
        public static LorId Create(int v)
        {
            return new LorId(TKS_BloodFiend_Initializer.packageId, v);
        }

        public static List<T> TKSRandomUtil<T>(List<T> ListToRandom_Arg, int randomnum, bool canbethesame = false, bool copywhenempty = true)
        {
    
            List<T> list = new List<T>();
            var ListToRandom = ListToRandom_Arg;
            T item = default(T);
            for (int i = 0; i < randomnum; i++)
            {
                if (ListToRandom.Count >= 1)
                {
                    item = RandomUtil.SelectOne<T>(ListToRandom);
                    if (!canbethesame)
                    {
                        ListToRandom.Remove(item);
                    }
                    list.Add(item);
                }
                else
                {
                    if (!copywhenempty)
                    {
                        break;
                    }
                    list.Add(item);
                }
            }
            return list;
        }
    }
}
