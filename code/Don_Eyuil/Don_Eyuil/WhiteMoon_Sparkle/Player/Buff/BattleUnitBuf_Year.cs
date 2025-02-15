using System.Collections.Generic;
using static Don_Eyuil.TKS_BloodFiend_Initializer.TKS_EnumExtension;
using static Don_Eyuil.WhiteMoon_Sparkle.Player.Buff.BattleUnitBuf_Sparkle;
using Don_Eyuil.Don_Eyuil.Player.PassiveAbility;
using Don_Eyuil.San_Sora.Player.PassiveAbility;
using Don_Eyuil.WhiteMoon_Sparkle.Player.Buff;
using Don_Eyuil.WhiteMoon_Sparkle.Player.PassiveAbility;
using EnumExtenderV2;
using LOR_DiceSystem;
using LOR_XML;
using System;
using System.IO;
using System.Linq;
using System.Xml;
//using Workshop;
using System.Xml.Serialization;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;
using static Don_Eyuil.WhiteMoon_Sparkle.Player.Buff.BattleUnitBuf_Year;
using Debug = UnityEngine.Debug;
using File = System.IO.File;
using Don_Eyuil.San_Sora;
namespace Don_Eyuil.WhiteMoon_Sparkle.Player.Buff
{
    public class BattleUnitBuf_Year : BattleUnitBuf_Don_Eyuil
    {
        public static string Desc =
            "泉之龙/秋之莲(主)[中立buff]:\r\n自身所有骰子最大值+3\r\n自身防御型骰子拼点胜利时将骰子类型更改为随机进攻型骰子\r\n自身拼点失败时将骰子类型更改为招架\r\n自身每幕首次拼点胜利时使自身下一幕获得1层迅捷\r\n" +
            "\r\n泉之龙/秋之莲(副)[中立buff]:\r\n自身每幕第1张书页造成的伤害减少至50%但命中时额外造成1次伤害(可触发骰子命中效果以外的命中效果)\r\n" +
            "\r\n泉之龙/秋之莲+(强化主)[中立buff]:\r\n自身所有骰子最大值+3\r\n自身防御型骰子拼点胜利时将骰子类型更改为随机进攻型骰子\r\n自身拼点失败时将骰子类型更改为招架\r\n自身拼点胜利时使自身下一幕获得1层迅捷(至多2层)自身速度高于目标时拼点胜利时造成的伤害与混乱伤害+3拼点失败时受到的伤害与混乱伤害-2\r\n" +
            "\r\n泉之龙/秋之莲+(强化副)[中立buff]:\r\n自身每幕前2张书页造成的伤害减少至75%但命中时额外造成2次伤害(可触发骰子命中效果以外的命中效果)\r\n" +
            "\r\n泉之龙/秋之莲(主副同用额外效果)[中立buff]:\r\n自身命中目标时使下颗骰子最小值+1\r\n自身每命中敌人5/8次便使自身恢复1点光芒/抽取1张书页\r\n";

        public BattleUnitBuf_Year(BattleUnitModel model) : base(model)
        {
            this.SetFieldValue("_bufIcon", TKS_BloodFiend_Initializer.ArtWorks["泉之龙秋之莲"]);
            this.SetFieldValue("_iconInit", true);
        }

        public bool IsIntensify = false;

        public override void OnStartBattle()
        {
            if (_owner.cardSlotDetail == null || _owner.cardSlotDetail.cardAry == null || _owner.cardSlotDetail.cardAry.Count == 0)
            {
                return;
            }
            if (Instance.PrimaryWeapons.Contains(this))
            {
                foreach (var item in _owner.cardSlotDetail.cardAry)
                {
                    item?.card?.AddBuf(new BattleDiceCardBuf_TransDice());
                    item?.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus() { max = 3 });
                }
            }
            list.Clear();
        }
        List<BattlePlayingCardDataInUnitModel> list = new List<BattlePlayingCardDataInUnitModel>();

        public int count_attack = 0;

        public override void OnSuccessAttack(BattleDiceBehavior behavior)
        {
            if (Instance.SubWeapons.Contains(this) && Instance.PrimaryWeapons.Contains(this))
            {
                behavior.card.ApplyDiceStatBonus(DiceMatch.NextDice, new DiceStatBonus() { min = 1 });
                count_attack++;
                if (count_attack % 5 == 0 && count_attack > 0)
                {
                    _owner.cardSlotDetail.RecoverPlayPoint(1);
                }
                if (count_attack % 8 == 0 && count_attack > 0)
                {
                    _owner.allyCardDetail.DrawCards(1);
                }
            }

            if (Instance.SubWeapons.Contains(this) && !IsIntensify && (list.Contains(behavior.card) || list.Count < 1) && !behavior.HasFlag(DiceFlagExtension.HasGivenDamage_BattleUnitBuf_Year))
            {
                var temp = new List<DiceCardAbilityBase>(behavior.abilityList);
                behavior.abilityList.Clear();
                if (!behavior.isBonusAttack)
                {
                    behavior.AddFlag(DiceFlagExtension.HasGivenDamage_BattleUnitBuf_Year);
                }
                behavior.ApplyDiceStatBonus(new DiceStatBonus() { dmgRate = -50 });
                behavior.GiveDamage(behavior.card.target);
                behavior.abilityList = temp;
                if (!list.Contains(behavior.card))
                {
                    list.Add(behavior.card);
                }
            }
            if (Instance.SubWeapons.Contains(this) && IsIntensify && (list.Contains(behavior.card) || list.Count < 2) && !behavior.HasFlag(DiceFlagExtension.HasGivenDamage_BattleUnitBuf_Year))
            {
                var temp = new List<DiceCardAbilityBase>(behavior.abilityList);
                behavior.abilityList.Clear();
                if (!behavior.isBonusAttack)
                {
                    behavior.AddFlag(DiceFlagExtension.HasGivenDamage_BattleUnitBuf_Year);
                }
                behavior.ApplyDiceStatBonus(new DiceStatBonus() { dmgRate = -75 });
                behavior.GiveDamage(behavior.card.target);
                behavior.GiveDamage(behavior.card.target);
                behavior.abilityList = temp;
                if (!list.Contains(behavior.card))
                {
                    list.Add(behavior.card);
                }
            }
        }

        int count_quickness = 0;

        public override void OnRoundStart()
        {
            count_quickness = 0;
        }

        public override void OnWinParrying(BattleDiceBehavior behavior)
        {
            if ((count_quickness < 1 && Instance.PrimaryWeapons.Contains(this)) || (count_quickness < 2 && Instance.PrimaryWeapons.Contains(this) && IsIntensify))
            {
                _owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Quickness, 1, _owner);
                count_quickness++;
            }
            if (Instance.PrimaryWeapons.Contains(this) && IsIntensify)
            {
                behavior.ApplyDiceStatBonus(new DiceStatBonus() { dmg = 3, breakDmg = 3 });
            }
        }

        public override void OnLoseParrying(BattleDiceBehavior behavior)
        {
            if (Instance.PrimaryWeapons.Contains(this) && IsIntensify)
            {
                behavior.TargetDice.ApplyDiceStatBonus(new DiceStatBonus() { dmg = -2, breakDmg = -2 });
            }
        }
        public class BattleDiceCardBuf_TransDice : BattleDiceCardBuf {

        public class BattleDiceCardBuf_TransDice : BattleDiceCardBuf
        {

        }

        protected override string keywordId => "BattleUnitBuf_DragonoftheSpring_LotusinAutumn";

        public override string BuffName
        {
            get
            {
                string temp = string.Empty;
                if (Instance.PrimaryWeapons.Contains(this) && !IsIntensify)
                {
                    temp += BattleEffectTextsXmlList.Instance.GetEffectTextName("BattleUnitBuf_DragonoftheSpring_LotusinAutumn") + " ";
                }
                if (Instance.PrimaryWeapons.Contains(this) && IsIntensify)
                {
                    temp += BattleEffectTextsXmlList.Instance.GetEffectTextName("BattleUnitBuf_DragonoftheSpring_LotusinAutumn_Reinforced") + " ";
                }
                if (Instance.SubWeapons.Contains(this) && !IsIntensify)
                {
                    temp += BattleEffectTextsXmlList.Instance.GetEffectTextName("BattleUnitBuf_DragonoftheSpring_LotusinAutumn_Secondary") + " ";
                }
                if (Instance.SubWeapons.Contains(this) && IsIntensify)
                {
                    temp += BattleEffectTextsXmlList.Instance.GetEffectTextName("BattleUnitBuf_DragonoftheSpring_LotusinAutumn_Secondary_Reinforced") + " ";
                }
                if (Instance.SubWeapons.Contains(this) && Instance.PrimaryWeapons.Contains(this))
                {
                    temp += BattleEffectTextsXmlList.Instance.GetEffectTextName("BattleUnitBuf_DragonoftheSpring_LotusinAutumn_Together") + " ";
                }
                return temp;
            }
        }

        public override string bufActivatedText
        {
            get
            {
                string temp = string.Empty;
                if (Instance.PrimaryWeapons.Contains(this) && !IsIntensify)
                {
                    temp += BattleEffectTextsXmlList.Instance.GetEffectTextDesc("BattleUnitBuf_DragonoftheSpring_LotusinAutumn") + "\r\n";
                }
                if (Instance.PrimaryWeapons.Contains(this) && IsIntensify)
                {
                    temp += BattleEffectTextsXmlList.Instance.GetEffectTextDesc("BattleUnitBuf_DragonoftheSpring_LotusinAutumn_Reinforced") + "\r\n";
                }
                if (Instance.SubWeapons.Contains(this) && !IsIntensify)
                {
                    temp += BattleEffectTextsXmlList.Instance.GetEffectTextDesc("BattleUnitBuf_DragonoftheSpring_LotusinAutumn_Secondary") + "\r\n";
                }
                if (Instance.SubWeapons.Contains(this) && IsIntensify)
                {
                    temp += BattleEffectTextsXmlList.Instance.GetEffectTextDesc("BattleUnitBuf_DragonoftheSpring_LotusinAutumn_Secondary_Reinforced") + "\r\n";
                }
                if (Instance.SubWeapons.Contains(this) && Instance.PrimaryWeapons.Contains(this))
                {
                    temp += BattleEffectTextsXmlList.Instance.GetEffectTextDesc("BattleUnitBuf_DragonoftheSpring_LotusinAutumn_Together") + "\r\n";
                }
                return temp;
            }
        }
            [HarmonyPatch]
            public class DiceTransformPatch
            {
                public enum Team
                {
                    attacker, defender,
                    winner, loser,
                }
                public static BattleParryingManager.ParryingTeam GetParryingTeam(BattleParryingManager PM, Team T)
                {
                    switch (T)
                    {
                        case Team.attacker: return PM.GetFieldValue<BattleParryingManager.ParryingTeam>("_currentAttackerTeam");
                        case Team.defender: return PM.GetFieldValue<BattleParryingManager.ParryingTeam>("_currentDefenderTeam");
                        case Team.winner: return PM.GetFieldValue<BattleParryingManager.ParryingTeam>("_currentWinnerTeam");
                        case Team.loser: return PM.GetFieldValue<BattleParryingManager.ParryingTeam>("_currentLoserTeam");
                    }
                    return null;
                }
                public static void SetParryingTeam(BattleParryingManager PM, Team T, BattleParryingManager.ParryingTeam TM)
                {
                    switch (T)
                    {
                        case Team.attacker: PM.SetFieldValue<BattleParryingManager.ParryingTeam>("_currentAttackerTeam", TM); break;
                        case Team.defender: PM.SetFieldValue<BattleParryingManager.ParryingTeam>("_currentDefenderTeam", TM); break;
                        case Team.winner: PM.SetFieldValue<BattleParryingManager.ParryingTeam>("_currentWinnerTeam", TM); break;
                        case Team.loser: PM.SetFieldValue<BattleParryingManager.ParryingTeam>("_currentLoserTeam", TM); break;
                    }
                }

                [HarmonyPatch]
                public class TransBehavior_AtkVSDfnPatch
                {
                    public static MethodBase TargetMethod()
                    {
                        return AccessTools.Method(typeof(BattleParryingManager), "ActionPhaseAtkVSDfn");
                    }
                    //Defender = winner
                    public static bool CheckDiceCardAbility(BattleParryingManager PM)
                    {
                        if (GetParryingTeam(PM, Team.defender) != null
                            && GetParryingTeam(PM, Team.defender).playingCard != null
                            && GetParryingTeam(PM, Team.defender).playingCard.currentBehavior != null && GetParryingTeam(PM, Team.defender).playingCard.currentBehavior.card != null
                            && (GetParryingTeam(PM, Team.defender).playingCard.currentBehavior.card.card.XmlData.Script == "Testify_TransDice" || GetParryingTeam(PM, Team.defender).playingCard.card.HasBuf<BattleDiceCardBuf_TransDice>()))
                        {
                            return true;
                        }
                        return false;
                    }
                    //Defender = winner
                    public static void TransDice(BattleParryingManager PM)
                    {
                        if (PM != null)
                        {
                            if (GetParryingTeam(PM, Team.defender).playingCard.currentBehavior != null)
                            {
                                GetParryingTeam(PM, Team.defender).playingCard.currentBehavior.behaviourInCard.Type = BehaviourType.Atk;
                                GetParryingTeam(PM, Team.defender).playingCard.currentBehavior.behaviourInCard.Detail = RandomUtil.SelectOne(BehaviourDetail.Slash, BehaviourDetail.Hit, BehaviourDetail.Penetrate);
                                PM.InvokeMethod("ActionPhaseAtkVSAtk");
                                //GetParryingTeam(PM, Team.defender).playingCard.currentBehavior.behaviourInCard.Type = BehaviourType.Def;
                                //GetParryingTeam(PM, Team.defender).playingCard.currentBehavior.behaviourInCard.Detail = BehaviourDetail.Guard;
                            }

                        }
                    }
                    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator ILcodegenerator)
                    {
                        List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
                        Label? L = null;
                        for (int i = 1; i < codes.Count; i++)
                        {
                            if (codes[i].opcode == OpCodes.Ldarg_0
                                && codes[i + 1].opcode == OpCodes.Ldfld && codes[i + 1].operand == AccessTools.Field(typeof(BattleParryingManager), "_currentDefenderTeam")
                                && codes[i + 2].opcode == OpCodes.Ldarg_0
                                && codes[i + 3].opcode == OpCodes.Ldfld && codes[i + 3].operand == AccessTools.Field(typeof(BattleParryingManager), "_currentLoserTeam")
                                && codes[i + 4].opcode == OpCodes.Bne_Un && codes[i + 4].Branches(out L))
                            {
                                Label L2 = ILcodegenerator.DefineLabel();
                                int codeIndex = codes.FindIndex((CodeInstruction code) => code.labels.Contains(L.Value));
                                codes[i + 4].operand = L2;
                                codes.InsertRange(codeIndex, new List<CodeInstruction>()
                        {
                            new CodeInstruction(OpCodes.Ldarg_0).WithLabels(L2),
                            new CodeInstruction(OpCodes.Call,AccessTools.Method(typeof(TransBehavior_AtkVSDfnPatch),"CheckDiceCardAbility")),
                            new CodeInstruction(OpCodes.Brfalse_S,L),
                            new CodeInstruction(OpCodes.Ldarg_0),
                            new CodeInstruction(OpCodes.Call,AccessTools.Method(typeof(TransBehavior_AtkVSDfnPatch),"TransDice")),
                            new CodeInstruction(OpCodes.Ret)
                        });
                            }
                        }
                        return codes.AsEnumerable<CodeInstruction>();
                    }
                }
                [HarmonyPatch]
                public class TransBehavior_AtkVSAtkPatch
                {
                    public static MethodBase TargetMethod()
                    {
                        return AccessTools.Method(typeof(BattleParryingManager), "ActionPhaseAtkVSAtk");
                    }
                    //loser = defender 
                    public static bool CheckDiceCardAbility(BattleParryingManager PM)
                    {
                        if (GetParryingTeam(PM, Team.loser) != null
                            && GetParryingTeam(PM, Team.loser).playingCard != null
                            && GetParryingTeam(PM, Team.loser).playingCard.currentBehavior != null && GetParryingTeam(PM, Team.loser).playingCard.currentBehavior.card != null
                            && (GetParryingTeam(PM, Team.loser).playingCard.currentBehavior.card.card.XmlData.Script == "Testify_TransDice" || GetParryingTeam(PM, Team.loser).playingCard.card.HasBuf<BattleDiceCardBuf_TransDice>()))
                        {
                            return true;
                        }
                        return false;
                    }
                    //loser = defender 
                    public static void TransDice(BattleParryingManager PM)
                    {
                        if (PM != null)
                        {
                            SetParryingTeam(PM, Team.attacker, GetParryingTeam(PM, Team.winner));
                            SetParryingTeam(PM, Team.defender, GetParryingTeam(PM, Team.loser));
                            GetParryingTeam(PM, Team.loser).playingCard.currentBehavior.behaviourInCard.Type = BehaviourType.Def;
                            GetParryingTeam(PM, Team.loser).playingCard.currentBehavior.behaviourInCard.Detail = BehaviourDetail.Guard;
                            if (GetParryingTeam(PM, Team.winner) != null)
                            {
                                if (GetParryingTeam(PM, Team.winner).GetParryingDiceType() == BattleParryingManager.ParryingDiceType.Attack)
                                {
                                    PM.InvokeMethod("ActionPhaseAtkVSDfn");
                                }
                                else
                                {
                                    PM.InvokeMethod("ActionPhaseDfnVSDfn");
                                }
                            }
                        }
                    }
                    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator ILcodegenerator)
                    {
                        Debug.LogError("TRANSSSSSSSSSSSSSSSSSSSSSSSSSSS");
                        List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
                        Label L2 = ILcodegenerator.DefineLabel();
                        codes[0].labels.Add(L2);
                        codes.InsertRange(0, new List<CodeInstruction>()
                        {
                            new CodeInstruction(OpCodes.Ldarg_0).WithLabels(),
                            new CodeInstruction(OpCodes.Call,AccessTools.Method(typeof(TransBehavior_AtkVSAtkPatch),"CheckDiceCardAbility")),
                            new CodeInstruction(OpCodes.Brfalse_S,L2),
                            new CodeInstruction(OpCodes.Ldarg_0),
                            new CodeInstruction(OpCodes.Call,AccessTools.Method(typeof(TransBehavior_AtkVSAtkPatch),"TransDice")),
                            new CodeInstruction(OpCodes.Ret)
                        });
                        return codes.AsEnumerable<CodeInstruction>();
                    }
                }
            }
        }
    }
}
