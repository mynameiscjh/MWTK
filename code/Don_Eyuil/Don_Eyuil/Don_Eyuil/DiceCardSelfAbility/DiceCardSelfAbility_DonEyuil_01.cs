using HarmonyLib;
using LOR_DiceSystem;
using System.Collections.Generic;
using UnityEngine;

namespace Don_Eyuil.Don_Eyuil.DiceCardSelfAbility
{
    public class DiceCardSelfAbility_DonEyuil_01 : DiceCardSelfAbilityBase
    {
        public static string Desc = "本书页将额外命中两名敌方角色";
        public List<BattleUnitModel> luckyDog = new List<BattleUnitModel>();
        public override void OnUseCard()
        {
            if (BattleObjectManager.instance.GetAliveList_opponent(owner.faction).Count <= 2)
            {
                luckyDog = new List<BattleUnitModel>(BattleObjectManager.instance.GetAliveList_opponent(owner.faction));
                return;
            }
            var temp_list = new List<BattleUnitModel>(BattleObjectManager.instance.GetAliveList_opponent(owner.faction));
            for (int i = 0; i < 2; i++)
            {
                var temp = RandomUtil.SelectOne(temp_list);
                temp_list.Remove(temp);
                luckyDog.Add(temp);
            }
        }

        public override void OnRollDice(BattleDiceBehavior behavior)
        {
            foreach (var item in luckyDog)
            {
                GiveDamageForSubTarget(behavior, item);
            }
        }

        public static void GiveDamageForSubTarget(BattleDiceBehavior behavior, BattleUnitModel target)
        {
            bool flag = target != null && behavior.card.card.XmlData.Spec.Ranged == CardRange.Near && behavior.card.card.XmlData.Spec.affection == CardAffection.Team;
            bool flag2 = flag;
            if (flag2)
            {
                bool flag3 = target.IsDead();
                DiceStatBonus diceStatBonus = (DiceStatBonus)behavior.GetType().GetField("_statBonus", AccessTools.all).GetValue(behavior);
                DiceStatBonus diceStatBonus2 = diceStatBonus.Copy();
                behavior.abilityList.ForEach(delegate (DiceCardAbilityBase x)
                {
                    x.BeforeGiveDamage();
                });
                behavior.abilityList.ForEach(delegate (DiceCardAbilityBase x)
                {
                    x.BeforeGiveDamage(target);
                });
                behavior.owner.BeforeGiveDamage(behavior);
                int num = (int)behavior.GetType().GetField("_damageReductionByGuard", AccessTools.all).GetValue(behavior);
                int num2 = (int)behavior.GetType().GetField("_diceDamageAll", AccessTools.all).GetValue(behavior);
                int diceResultValue = behavior.DiceResultValue;
                bool flag4 = diceResultValue >= 30 && behavior.card.target.bufListDetail.GetActivatedBuf(KeywordBuf.ClawCounter) != null;
                bool flag5 = flag4;
                if (flag5)
                {
                    behavior.SetBlocked(true);
                    behavior.card.target.bufListDetail.RemoveBufAll(typeof(BattleUnitBuf_clawCounter));
                }
                bool isBlocked = behavior.IsBlocked;
                bool flag6 = !isBlocked;
                if (flag6)
                {
                    double num3 = (double)(diceResultValue - num + diceStatBonus.dmg + behavior.owner.UnitData.unitData.giftInventory.GetStatBonus_Dmg(behavior.behaviourInCard.Detail));
                    num3 -= (double)target.GetDamageReduction(behavior);
                    int num4 = diceStatBonus.dmgRate + target.GetDamageIncreaseRate();
                    float num5 = 1f + (float)num4 / 100f;
                    bool flag7 = num5 < 0f;
                    bool flag8 = flag7;
                    if (flag8)
                    {
                        num5 = 0f;
                    }
                    num3 *= (double)num5;
                    num3 *= (double)target.GetDamageRate();
                    bool flag9 = target.emotionDetail.IsTakeDamageDouble();
                    bool flag10 = flag9;
                    if (flag10)
                    {
                        num3 *= 2.0;
                    }
                    bool flag11 = behavior.owner.emotionDetail.IsGiveDamageDouble();
                    bool flag12 = flag11;
                    if (flag12)
                    {
                        num3 *= 2.0;
                    }
                    double num6 = (double)(diceResultValue - num + diceStatBonus.breakDmg);
                    num6 -= (double)target.GetBreakDamageReduction(behavior);
                    int num7 = diceStatBonus.breakRate + target.GetBreakDamageIncreaseRate();
                    num6 *= (double)(1f + (float)num7 / 100f);
                    num6 *= (double)target.GetBreakDamageRate();
                    Vector3 normalized = (target.view.WorldPosition - behavior.owner.view.WorldPosition).normalized;
                    AtkResist atkResist = AtkResist.Normal;
                    AtkResist atkResist2 = AtkResist.Normal;
                    atkResist = target.GetResistHP(behavior.behaviourInCard.Detail);
                    atkResist2 = target.GetResistBP(behavior.behaviourInCard.Detail);
                    bool flag13 = false;
                    foreach (DiceCardAbilityBase diceCardAbilityBase in behavior.abilityList)
                    {
                        bool isPercentDmg = diceCardAbilityBase.IsPercentDmg;
                        bool flag14 = isPercentDmg;
                        if (flag14)
                        {
                            flag13 = true;
                            break;
                        }
                    }
                    bool flag15 = flag13;
                    bool flag16 = flag15;
                    if (flag16)
                    {
                        int num8 = int.MaxValue;
                        foreach (DiceCardAbilityBase diceCardAbilityBase2 in behavior.abilityList)
                        {
                            int maximumPercentDmg = diceCardAbilityBase2.GetMaximumPercentDmg();
                            bool flag17 = maximumPercentDmg < num8;
                            bool flag18 = flag17;
                            if (flag18)
                            {
                                num8 = maximumPercentDmg;
                            }
                        }
                        double num9 = (double)target.MaxHp * (num3 / 100.0);
                        bool flag19 = num9 >= (double)target.MaxHp;
                        bool flag20 = flag19;
                        if (flag20)
                        {
                            num9 = (double)target.MaxHp;
                        }
                        num3 = (double)Mathf.Min((float)(num9 * (double)BookModel.GetResistRate(AtkResist.Weak)), (float)num8);
                    }
                    else
                    {
                        num3 *= (double)BookModel.GetResistRate(atkResist);
                    }
                    num6 *= (double)BookModel.GetResistRate(atkResist2);
                    num3 = target.ChangeDamage(behavior.owner, num3);
                    bool flag21 = behavior.card != null && behavior.card.cardAbility != null && behavior.card.cardAbility.IsTrueDamage();
                    bool flag22 = flag21;
                    if (flag22)
                    {
                        num3 = (double)diceResultValue;
                        num6 = (double)diceResultValue;
                    }
                    int num10 = (int)num3;
                    int num11 = (int)num6;
                    bool flag23 = num10 < 1;
                    bool flag24 = flag23;
                    if (flag24)
                    {
                        num10 = 0;
                    }
                    bool flag25 = behavior.abilityList.Exists((DiceCardAbilityBase x) => x.Invalidity);
                    bool flag26 = flag25;
                    if (flag26)
                    {
                        num10 = 0;
                        num11 = 0;
                    }
                    bool flag27 = num11 < 1;
                    bool flag28 = flag27;
                    if (flag28)
                    {
                        num11 = 0;
                    }
                    bool flag29 = behavior.card.card.GetSpec().Ranged == CardRange.Far && target.passiveDetail.IsImmuneByFarAtk();
                    bool flag30 = flag29;
                    if (flag30)
                    {
                        num10 = 0;
                        num11 = 0;
                    }
                    bool flag31 = target.passiveDetail.IsInvincible();
                    bool flag32 = flag31;
                    if (flag32)
                    {
                        num10 = 0;
                        num11 = 0;
                    }
                    num10 = TakeDamageSpecificForSubTarget(target, num10, DamageType.Attack, behavior.owner, KeywordBuf.None, behavior.behaviourInCard.Detail);
                    behavior.owner.emotionDetail.CheckDmg(num10, target);
                    behavior.owner.passiveDetail.AfterGiveDamage(num10);
                    bool flag33 = behavior.card.cardAbility != null;
                    if (flag33)
                    {
                        behavior.card.cardAbility.AfterGiveDamage(num10, target);
                    }
                    TakeBreakDamageSpeceificForSubTarget(target.breakDetail, target, num11, behavior.behaviourInCard.Detail, DamageType.Attack, behavior.owner, atkResist2, KeywordBuf.None);
                    num2 += num10;
                    behavior.GetType().GetField("_diceDamageAll", AccessTools.all).SetValue(behavior, num2);
                    behavior.owner.battleCardResultLog.SetDamageGiven(num10);
                    behavior.owner.battleCardResultLog.SetBreakDmgGiven(num11);
                    target.battleCardResultLog.SetDeathState(target.IsDeadReal());
                    behavior.owner.history.damageAtOneRoundByDice += num10;
                    bool flag34 = !target.IsDead();
                    if (flag34)
                    {
                        target.OnTakeDamageByAttack(behavior, num10);
                        target.OnTakeBreakDamageByAttack(behavior, num11);
                        bool flag35 = behavior.abilityList.Count <= 0;
                        if (flag35)
                        {
                            string script = behavior.behaviourInCard.Script;
                            bool flag36 = script != string.Empty;
                            if (flag36)
                            {
                                DiceCardAbilityBase diceCardAbilityBase3 = Singleton<AssemblyManager>.Instance.CreateInstance_DiceCardAbility(script);
                                bool flag37 = diceCardAbilityBase3 != null;
                                if (flag37)
                                {
                                    behavior.AddAbility(diceCardAbilityBase3);
                                }
                            }
                        }
                        behavior.OnEventDiceAbility(DiceCardAbilityBase.DiceCardPassiveType.SuccessAtk, target);
                        behavior.card.OnSucceedAttack(behavior);
                        behavior.card.OnSucceedAtkEvent();
                        behavior.owner.passiveDetail.OnSucceedAttackEvent(behavior);
                    }
                }
            }
        }

        // Token: 0x0600006D RID: 109 RVA: 0x00007494 File Offset: 0x00005694
        public static void TakeBreakDamageSpeceificForSubTarget(BattleUnitBreakDetail breakDetail, BattleUnitModel owner, int damage, BehaviourDetail detail, DamageType type = DamageType.Attack, BattleUnitModel attacker = null, AtkResist atkResist = AtkResist.Normal, KeywordBuf keyword = KeywordBuf.None)
        {
            bool flag = owner.turnState == BattleUnitTurnState.BREAK;
            bool flag2 = !flag;
            if (flag2)
            {
                damage = Mathf.RoundToInt((float)damage * owner.BreakDmgFactor(damage, type, keyword));
                bool flag3 = owner.IsInvincibleBp(attacker);
                bool flag4 = flag3;
                if (flag4)
                {
                    damage = 0;
                }
                bool flag5 = owner.IsImmuneBreakDmg(type);
                bool flag6 = flag5;
                if (flag6)
                {
                    damage = 0;
                }
                damage -= owner.GetBreakDamageReductionAll(damage, type, attacker);
                bool flag7 = damage < 0;
                bool flag8 = flag7;
                if (flag8)
                {
                    damage = 0;
                }
                bool flag9 = owner.BeforeTakeBreakDamage(attacker, damage);
                bool flag10 = flag9;
                if (flag10)
                {
                    damage = 0;
                }
                bool flag11 = Singleton<StageController>.Instance.IsLogState();
                bool flag12 = flag11;
                if (flag12)
                {
                    BattleCardTotalResult battleCardResultLog = owner.battleCardResultLog;
                    if (battleCardResultLog != null)
                    {
                        battleCardResultLog.SetBreakDmgTaken(damage, detail, atkResist);
                    }
                    BattleCardTotalResult battleCardResultLog2 = owner.battleCardResultLog;
                    if (battleCardResultLog2 != null)
                    {
                        battleCardResultLog2.SetBreakState(false);
                    }
                }
                else
                {
                    owner.view.BreakDamaged(damage, detail, attacker, atkResist);
                }
                bool flag13 = breakDetail.breakGauge <= 0;
                bool flag14 = !flag13;
                if (flag14)
                {
                    bool flag15 = owner.passiveDetail.IsDamageReductionForEvent();
                    int num = damage;
                    bool flag16 = flag15;
                    bool flag17 = flag16;
                    if (flag17)
                    {
                        num /= 2;
                    }
                    owner.history.takeBreakDamageAtOneRound += Mathf.Min(breakDetail.breakGauge, num);
                    owner.breakDetail.breakGauge -= num;
                    bool flag18 = owner.breakDetail.breakGauge <= 0;
                    bool flag19 = flag18;
                    if (flag19)
                    {
                        bool flag20 = owner.IsStraighten();
                        bool flag21 = flag20;
                        if (flag21)
                        {
                            owner.breakDetail.breakGauge = 1;
                        }
                        else
                        {
                            owner.breakDetail.breakGauge = 0;
                            owner.breakDetail.LoseBreakLife(attacker);
                        }
                    }
                }
            }
        }

        // Token: 0x0600006E RID: 110 RVA: 0x00007660 File Offset: 0x00005860
        public static int TakeDamageSpecificForSubTarget(BattleUnitModel model, int v, DamageType type = DamageType.Attack, BattleUnitModel attacker = null, KeywordBuf keyword = KeywordBuf.None, BehaviourDetail detail = BehaviourDetail.None)
        {
            bool flag = model.IsDead();
            bool flag2 = flag;
            int result;
            if (flag2)
            {
                result = 0;
            }
            else
            {
                int num = (int)model.hp;
                int num2 = v;
                bool flag3 = num2 <= 0;
                bool flag4 = flag3;
                if (flag4)
                {
                    num2 = 0;
                }
                bool flag5 = model.BeforeTakeDamage(attacker, num2 - model.GetDamageReductionAll());
                num2 = Mathf.RoundToInt((float)num2 * model.DmgFactor(num2, type, keyword));
                num2 -= model.GetDamageReductionAll();
                bool flag6 = num2 <= 0;
                bool flag7 = flag6;
                if (flag7)
                {
                    num2 = 0;
                }
                bool flag8 = flag5;
                bool flag9 = flag8;
                if (flag9)
                {
                    num2 = 0;
                }
                bool flag10 = model.IsInvincibleHp(attacker);
                bool flag11 = flag10;
                if (flag11)
                {
                    num2 = 0;
                }
                bool flag12 = model.IsImmuneDmg();
                bool flag13 = flag12;
                if (flag13)
                {
                    num2 = 0;
                }
                bool flag14 = model.IsImmuneDmg(type, keyword);
                bool flag15 = flag14;
                if (flag15)
                {
                    num2 = 0;
                }
                bool flag16 = num2 > 0;
                bool flag17 = flag16;
                if (flag17)
                {
                    model.OnLoseHp(num2);
                }
                bool flag18 = model.GetDamageReductionAll() > 0 && model.faction == Faction.Enemy && model.UnitData.unitData.EnemyUnitId == 30024 && attacker != null && attacker.faction == Faction.Player && num2 > 0;
                bool flag19 = flag18;
                if (flag19)
                {
                    attacker.UnitData.historyInStage.pierceOscarbuf++;
                }
                int num3 = num2;
                bool flag20 = model.passiveDetail.IsDamageReductionForEvent();
                bool flag21 = flag20;
                if (flag21)
                {
                    float num4 = 0f;
                    float num5 = (float)num;
                    int num6 = num2;
                    bool flag22 = num6 > 500;
                    bool flag23 = flag22;
                    if (flag23)
                    {
                        num6 = 500;
                    }
                    bool flag24 = num6 > 0;
                    bool flag25 = flag24;
                    if (flag25)
                    {
                        for (int i = 0; i < num6; i++)
                        {
                            float num7 = (num5 - num4) / (float)model.MaxHp;
                            num4 += num7;
                        }
                    }
                    num3 = (int)num4;
                }
                model.SetHp((int)(model.hp - (float)num3));
                model.AfterTakeDamage(attacker, num2);
                bool flag26 = Singleton<StageController>.Instance.IsLogState();
                bool flag27 = flag26;
                if (flag27)
                {
                    bool flag28 = detail != BehaviourDetail.None;
                    bool flag29 = flag28;
                    if (flag29)
                    {
                        int face = 100;
                        int dmg = num2;
                        model.view.PrintBloodSprites(dmg, model.hp);
                        AtkResist resistHP = model.GetResistHP(detail);
                        model.view.Damaged(dmg, detail, face, attacker, resistHP);
                    }
                    else
                    {
                        BattleCardTotalResult battleCardResultLog = model.battleCardResultLog;
                        if (battleCardResultLog != null)
                        {
                            battleCardResultLog.SetDamageTaken(num2, 100, BehaviourDetail.None, model.Book.GetResistHP(BehaviourDetail.None));
                        }
                    }
                }
                else
                {
                    int dmg2 = num2;
                    model.view.PrintBloodSprites(dmg2, model.hp);
                    bool flag30 = detail != BehaviourDetail.None;
                    bool flag31 = flag30;
                    if (flag31)
                    {
                        int face2 = 100;
                        AtkResist resistHP2 = model.GetResistHP(detail);
                        model.view.Damaged(dmg2, detail, face2, attacker, resistHP2);
                    }
                    else
                    {
                        SingletonBehavior<AttackEffectManager>.Instance.CreateDamagedTextEffectWithoutResist(num2, 1, model);
                    }
                    bool flag32 = !Singleton<StageController>.Instance.IsLogState();
                    bool flag33 = flag32;
                    if (flag33)
                    {
                        SingletonBehavior<BattleManagerUI>.Instance.ui_unitListInfoSummary.UpdateCharacterProfile(model, model.faction, model.hp, model.breakDetail.breakGauge, null);
                    }
                }
                bool flag34 = attacker != null;
                bool flag35 = flag34;
                if (flag35)
                {
                    model.lastAttacker = attacker;
                }
                BattleObjectManager.instance.GetList();
                bool flag36 = model.IsImmortal() && model.hp <= 1f;
                bool flag37 = flag36;
                if (flag37)
                {
                    model.SetHp(1);
                }
                int minHp = model.GetMinHp();
                bool flag38 = model.hp < (float)minHp;
                bool flag39 = flag38;
                if (flag39)
                {
                    model.SetHp(minHp);
                }
                bool flag40 = model.hp <= 0f;
                bool flag41 = flag40;
                if (flag41)
                {
                    model.OnHpZero();
                }
                bool flag42 = model.hp <= (float)model.Book.DeadLine;
                bool flag43 = flag42;
                if (flag43)
                {
                    bool flag44 = model.lastAttacker != null;
                    bool flag45 = flag44;
                    if (flag45)
                    {
                        model.Die(model.lastAttacker, true);
                    }
                    else
                    {
                        model.Die(null, true);
                    }
                }
                model.CheckGiftOnTakeDamage(num2, type, attacker, keyword);
                result = num2;
            }
            return result;
        }
    }
}
