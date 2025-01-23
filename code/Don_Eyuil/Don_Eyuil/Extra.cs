using EnumExtenderV2;
using HarmonyLib;
using HyperCard;
using LOR_BattleUnit_UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using static CharacterSound;
using static UI.UIIconManager;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.GridLayoutGroup;
using CustomMapUtility;

namespace Don_Eyuil
{
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
            public static void Trigger_RecoverPlayPoint_Before(BattlePlayingCardSlotDetail Detail,ref int value)
            {
                var Model = Detail != null ? Detail.GetFieldValue<BattleUnitModel>("_self") : null;
                if (Model != null)
                {
                    foreach (var Buf in Model.bufListDetail.GetActivatedBufList())
                    {
                        if (!Buf.IsDestroyed() && Buf is BattleUnitBuf_Don_Eyuil)
                        {
                            Debug.LogError("BeforeRecoverPlayPoint:" + value);
                            (Buf as BattleUnitBuf_Don_Eyuil).BeforeRecoverPlayPoint(ref value);
                        }
                    }
                }
            }

            [HarmonyPatch(typeof(BattlePlayingCardSlotDetail), "RecoverPlayPoint")]
            [HarmonyTranspiler]
            public static IEnumerable<CodeInstruction> BattlePlayingCardSlotDetail_RecoverPlayPoint_Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
                codes.InsertRange(0, new List<CodeInstruction>()
                { 
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldarga_S,1),
                    new CodeInstruction(OpCodes.Call,AccessTools.Method(typeof(BeforeRecoverPlayPointPatch),"Trigger_RecoverPlayPoint_Before")),
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
            public static void BattleUnitModel_RecoverHP_Pre(BattleUnitModel __instance,int v)
            {
                __instance.bufListDetail.GetActivatedBufList().DoIf(cond => !cond.IsDestroyed() && cond is BattleUnitBuf_Don_Eyuil, x => (x as BattleUnitBuf_Don_Eyuil).BeforeRecoverHp(v));
            }
        }
        public virtual void BeforeAddEmotionCoin(EmotionCoinType CoinType, ref int Count)
        {

        }
        public class BeforeAddEmotionCoinPatch
        {
            public static void Trigger_CreateEmotionCoin_Before(BattleUnitEmotionDetail Detail,EmotionCoinType CoinType, ref int Count)
            {

                var Model = Detail != null ? Detail.GetFieldValue<BattleUnitModel>("_self") : null;
                if(Model != null)
                {
                    foreach (var Buf in Model.bufListDetail.GetActivatedBufList())
                    {
                        if (!Buf.IsDestroyed() && Buf is BattleUnitBuf_Don_Eyuil)
                        {
                            (Buf as BattleUnitBuf_Don_Eyuil).BeforeAddEmotionCoin(CoinType, ref Count);
                            Debug.LogError("BeforeAddEmotionCoin:" + CoinType + "," + Count);
                        }
                    }
                }
            }
            [HarmonyPatch(typeof(BattleUnitEmotionDetail), "CreateEmotionCoin")]
            [HarmonyTranspiler]
            public static IEnumerable<CodeInstruction> BattleUnitEmotionDetail_CreateEmotionCoin_Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
                //Label? L ;
                codes.InsertRange(0, new List<CodeInstruction>()
                        {
                            new CodeInstruction(OpCodes.Ldarg_0),
                            new CodeInstruction(OpCodes.Ldarg_1),
                            new CodeInstruction(OpCodes.Ldarga_S,2),
                            new CodeInstruction(OpCodes.Call,AccessTools.Method(typeof(BeforeAddEmotionCoinPatch),"Trigger_CreateEmotionCoin_Before")),
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
        public virtual void BeforeOtherUnitAddKeywordBuf(KeywordBuf BufType, BattleUnitModel Target, ref int Stack)
        {

        }
        public class BeforeAddKeywordBufPatch
        {
            public static void Trigger_AddKeywordBuf_Before(KeywordBuf BufType, BattleUnitModel Target, ref int Stack)
            {
                if (Stack > 0)
                {
                    foreach (var Buf in Target.bufListDetail.GetActivatedBufList())
                    {
                        if (!Buf.IsDestroyed() && Buf is BattleUnitBuf_Don_Eyuil)
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
                                (Buf as BattleUnitBuf_Don_Eyuil).BeforeOtherUnitAddKeywordBuf(BufType, Target, ref Stack);
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
            public static IEnumerable<CodeInstruction> BattleUnitBufListDetail_AddKeywordBuf_Transpiler(IEnumerable<CodeInstruction> instructions,ILGenerator ILcodegenerator)
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
                        new CodeInstruction(OpCodes.Call,AccessTools.Method(typeof(BeforeAddKeywordBufPatch),"Trigger_AddKeywordBuf_Before")),
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
            public static void Trigger_BleedingDmg_After(BattleUnitModel Model, int dmg, KeywordBuf keyword)
            {
                if (keyword == KeywordBuf.Bleeding && dmg > 0)
                {
                    Debug.LogError(Model.Book.Name + "TakeBleedingDmg:" + dmg);
                    Model.bufListDetail.GetActivatedBufList().DoIf(cond => !cond.IsDestroyed() && cond is BattleUnitBuf_Don_Eyuil, x => (x as BattleUnitBuf_Don_Eyuil).AfterTakeBleedingDamage(dmg));
                    List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList();
                    aliveList.Remove(Model);
                    aliveList.Do(x1 => x1.bufListDetail.GetActivatedBufList().DoIf(cond => !cond.IsDestroyed() && cond is BattleUnitBuf_Don_Eyuil, x => (x as BattleUnitBuf_Don_Eyuil).AfterOtherUnitTakeBleedingDamage(Model, dmg)));
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


        public virtual int GetMaxStack() => -1;
        public virtual void Add(int stack)
        {
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
        public static List<BattleUnitModel> GetAllUnitWithBuf<T>(Faction? Faction = null,BufReadyType ReadyType = BufReadyType.ThisRound) where T : BattleUnitBuf_Don_Eyuil
        {
            List<BattleUnitModel> UnitList = Faction.HasValue? BattleObjectManager.instance.GetAliveList(Faction.Value): BattleObjectManager.instance.GetAliveList();
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

        public static bool UseBuf<T>(BattleUnitModel model, int stack) where T : BattleUnitBuf_Don_Eyuil
        {
            T BuffInstance = GetOrAddBuf<T>(model,BufReadyType.ThisRound);
            if (BuffInstance != null && BuffInstance.stack >= stack)
            {
                BuffInstance.Add(-stack);
                return true;
            }
            return false;
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

        public BattleUnitModel owner { get{ return this._owner; } }
    }
    public static class ExtraMethods
    {
        public static void GiveDamage_SubTarget(this BattleDiceBehavior behavior,BattleUnitModel OriginalTarget, int EnemyCount)
        {
            var owner = behavior.owner;
            if(owner != null)
            {
                if (!behavior.HasFlag(TKS_BloodFiend_Initializer.TKS_EnumExtension.DiceFlagExtension.HasGivenDamage_SubTarget))
                {
                    var AliveList = BattleObjectManager.instance.GetAliveList_opponent(owner.faction);
                    AliveList.Remove(OriginalTarget);
                    behavior.GiveDamage_SubTarget((EnemyCount == -1 ? AliveList : MyTools.TKSRandomUtil(AliveList.ToList(), EnemyCount, false, false)).ToArray());
                }
            }
        }
        public static void GiveDamage_SubTarget(this BattleDiceBehavior behavior,params BattleUnitModel[] target)
        {
            if(behavior != null && !behavior.HasFlag(TKS_BloodFiend_Initializer.TKS_EnumExtension.DiceFlagExtension.HasGivenDamage_SubTarget))
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
        public static CustomMapHandler CMH
        {
            get
            {
                return  CustomMapHandler.GetCMU(TKS_BloodFiend_Initializer.packageId);
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
