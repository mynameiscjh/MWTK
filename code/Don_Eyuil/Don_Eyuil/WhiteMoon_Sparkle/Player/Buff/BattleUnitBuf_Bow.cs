using LOR_DiceSystem;
using System;
using System.Collections.Generic;

namespace Don_Eyuil.WhiteMoon_Sparkle.Player.Buff
{
    public class BattleUnitBuf_Bow : BattleUnitBuf_Don_Eyuil
    {
        public static string Desc =
            "千斤弓(主)[中立buff]:\r\n使自身所有普通战斗书页类型暂时更改为远程,且书页中所有防御型骰子类型暂时更改为突刺\r\n目标书页骰子每与自身拼点1次便使目标书页所有骰子威力-1\r\n拼点开始时将1颗闪避(4～8)置入书页末尾(每幕至多触发1次)\r\n" +
            "\r\n千斤弓(副)[中立buff]:\r\n目标上一幕每承受1次伤害便使自身对其造成的伤害与混乱伤害+5%(至多25%)\r\n" +
            "\r\n千斤弓+(强化主)[中立buff]:\r\n所有骰子威力+1\r\n使自身所有普通战斗书页类型暂时更改为远程,且书页中所有防御型骰子类型暂时更改为突刺\r\n目标书页骰子每与自身拼点1次便使目标书页所有骰子威力-1\r\n拼点开始时将1颗闪避(5～8[拼点胜利]视为以1颗点数相同的突刺骰子命中1次)置入书页末尾(每幕至多触发1次)\r\n" +
            "\r\n千斤弓+(强化副)[中立buff]:\r\n目标上一幕每承受1次伤害便使自身对其造成的伤害与混乱伤害+10%(至多50%)\r\n" +
            "\r\n千斤弓(主副同用额外效果)[中立buff]:\r\n根据自身书页装备的速度骰子使书页中的最后1颗进攻型骰类型更改为四色骰子\r\nx=自身骰子最终值\r\n第1颗速度骰子:赤:命中时造成1.5x点物理伤害(不计算抗性)\r\n第2颗速度骰子:白:命中时造成1.5x点混乱伤害(不计算抗性)\r\n第3颗速度骰子:黑:命中时造成x点伤害与混乱伤害(不计算抗性)\r\n第4颗速度骰子:蓝:命中时追加目标最大生命值x%点伤害(追加的伤害无法超过2x)\r\n(覆盖原本的骰子效果)\r\n";

        public bool IsIntensify = false;

        public Dictionary<BattleUnitModel, int> dic = new Dictionary<BattleUnitModel, int>();

        public override void OnRoundStart()
        {
            dic.Clear();
            foreach (var item in BattleObjectManager.instance.GetAliveList())
            {
                if (!item.bufListDetail.HasBuf<BattleUnitBuf_Count>())
                {
                    item.bufListDetail.AddBuf(new BattleUnitBuf_Count(item));
                }
                dic.Add(item, GetBufStack<BattleUnitBuf_Count>(item));
            }
            fl = false;
        }

        public class BattleUnitBuf_Count : BattleUnitBuf_Don_Eyuil
        {
            public BattleUnitBuf_Count(BattleUnitModel model) : base(model)
            {
            }

            public override void OnRoundStartAfter()
            {
                this.stack = 0;
            }

            public override void BeforeTakeDamage(BattleUnitModel attacker, int dmg)
            {
                this.stack++;
            }
        }

        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            if (!IsIntensify && BattleUnitBuf_Sparkle.Instance.SubWeapons.Contains(this))
            {
                if (dic.TryGetValue(behavior.card.target, out int v))
                {
                    behavior.ApplyDiceStatBonus(new DiceStatBonus() { dmgRate = Math.Min(25, v * 5), breakRate = Math.Min(25, v * 5) });
                }
            }
            if (IsIntensify && BattleUnitBuf_Sparkle.Instance.SubWeapons.Contains(this))
            {
                if (dic.TryGetValue(behavior.card.target, out int v))
                {
                    behavior.ApplyDiceStatBonus(new DiceStatBonus() { dmgRate = Math.Min(50, v * 10), breakRate = Math.Min(50, v * 10) });
                }
            }
            if (IsIntensify && BattleUnitBuf_Sparkle.Instance.PrimaryWeapons.Contains(this))
            {
                behavior.ApplyDiceStatBonus(new DiceStatBonus() { power = 1 });
            }
        }

        public override void OnStartBattle()
        {
            if (BattleUnitBuf_Sparkle.Instance.PrimaryWeapons.Contains(this))
            {
                foreach (var item in _owner.cardSlotDetail.cardAry)
                {
                    if (item == null)
                    {
                        continue;
                    }

                    //_owner.view.charAppearance.SetAltMotion(ActionDetail.Penetrate, ActionDetail.Fire);
                    //_owner.view.charAppearance.SetAltMotion(ActionDetail.Hit, ActionDetail.Fire);
                    //_owner.view.charAppearance.SetAltMotion(ActionDetail.Slash, ActionDetail.Fire);
                    if (!item.card.XmlData.IsFloorEgo() && !item.card.XmlData.IsOnlyPage())
                    {
                        var spec = item.card.XmlData.Spec.Copy();
                        spec.Ranged = CardRange.Far;
                        item.card.XmlData.Spec = spec;
                        var list = new List<DiceBehaviour>();
                        foreach (var dice in item.card.XmlData.DiceBehaviourList)
                        {
                            var copy = dice.Copy();
                            if (dice.Detail == BehaviourDetail.Guard)
                            {
                                copy.Type = BehaviourType.Atk;
                                copy.Detail = BehaviourDetail.Penetrate;
                            }
                            if (IsIntensify && dice.Type == BehaviourType.Def)
                            {
                                copy.Type = BehaviourType.Atk;
                                copy.Detail = BehaviourDetail.Penetrate;
                            }
                            list.Add(copy);
                        }
                        item.card.XmlData.DiceBehaviourList = list;

#if false
                        foreach (var dice in new List<BattleDiceBehavior>(item.cardBehaviorQueue))
                        {
                            if (dice == null)
                            {
                                continue;
                            }
                            if (dice.Detail == BehaviourDetail.Guard)
                            {
                                dice.DestroyDice(DiceUITiming.Start);
                                var temp = new BattleDiceBehavior()
                                {
                                    behaviourInCard = new DiceBehaviour()
                                    {
                                        Min = dice.GetDiceMin(),
                                        Dice = dice.GetDiceMax(),
                                        Type = BehaviourType.Atk,
                                        Detail = BehaviourDetail.Penetrate,
                                        MotionDetail = MotionDetail.S,
                                        MotionDetailDefault = MotionDetail.N,
                                    },
                                    card = item,
                                    abilityList = new List<DiceCardAbilityBase>(),
                                };
                                item.AddDice(temp);
                            }
                            if (IsIntensify && dice.Type == BehaviourType.Def)
                            {
                                dice.DestroyDice(DiceUITiming.Start);
                                var temp = new BattleDiceBehavior()
                                {
                                    behaviourInCard = new DiceBehaviour()
                                    {
                                        Min = dice.GetDiceMin(),
                                        Dice = dice.GetDiceMax(),
                                        Type = BehaviourType.Atk,
                                        Detail = BehaviourDetail.Penetrate,
                                        MotionDetail = MotionDetail.S,
                                        MotionDetailDefault = MotionDetail.N,
                                    },
                                    card = item,
                                    abilityList = new List<DiceCardAbilityBase>(),
                                };
                                item.AddDice(temp);
                            }
                        }
#endif
                    }
                }
            }
            if (BattleUnitBuf_Sparkle.Instance.PrimaryWeapons.Contains(this) && BattleUnitBuf_Sparkle.Instance.SubWeapons.Contains(this))
            {
                for (int i = 0; i < _owner.speedDiceCount; i++)
                {
                    if (_owner.cardSlotDetail.cardAry[i] == null)
                    {
                        continue;
                    }

                    switch (i)
                    {
                        case 0:
                            _owner.cardSlotDetail.cardAry[0].ApplyDiceAbility(DiceMatch.LastDice, AssemblyManager.Instance.CreateInstance_DiceCardAbility("Red"));
                            break;
                        case 1:
                            _owner.cardSlotDetail.cardAry[1].ApplyDiceAbility(DiceMatch.LastDice, AssemblyManager.Instance.CreateInstance_DiceCardAbility("White"));
                            break;
                        case 2:
                            _owner.cardSlotDetail.cardAry[2].ApplyDiceAbility(DiceMatch.LastDice, AssemblyManager.Instance.CreateInstance_DiceCardAbility("Black"));
                            break;
                        case 3:
                            _owner.cardSlotDetail.cardAry[3].ApplyDiceAbility(DiceMatch.LastDice, AssemblyManager.Instance.CreateInstance_DiceCardAbility("Blue"));
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public class DiceCardAbility_Red : DiceCardAbilityBase
        {
            public static string Desc = "[命中时]造成1.5倍骰子点数点物理伤害(不计算抗性)";
            public override void OnSucceedAttack(BattleUnitModel target)
            {
                target.TakeDamage((int)(behavior.DiceResultValue * 1.5), attacker: owner);
            }
        }
        public class DiceCardAbility_White : DiceCardAbilityBase
        {
            public static string Desc = "[命中时]造成1.5倍骰子点数点混乱伤害(不计算抗性)";
            public override void OnSucceedAttack(BattleUnitModel target)
            {
                target.TakeBreakDamage((int)(behavior.DiceResultValue * 1.5), attacker: owner);
            }
        }
        public class DiceCardAbility_Black : DiceCardAbilityBase
        {
            public static string Desc = "[命中时]造成骰子点数点伤害与混乱伤害(不计算抗性)";
            public override void OnSucceedAttack(BattleUnitModel target)
            {
                target.TakeDamage(behavior.DiceResultValue, attacker: owner);
                target.TakeBreakDamage(behavior.DiceResultValue, attacker: owner);
            }
        }
        public class DiceCardAbility_Blue : DiceCardAbilityBase
        {
            public static string Desc = "命中时追加目标最大生命值百分之骰子点数点点伤害(追加的伤害无法超过2x)";
            public override void OnSucceedAttack(BattleUnitModel target)
            {
                int temp = Math.Min(behavior.DiceResultValue * 2, target.MaxHp * behavior.DiceResultValue / 100);
                target.TakeDamage(temp, attacker: owner);
            }
        }

        bool fl = false;
        public override void OnStartParrying(BattlePlayingCardDataInUnitModel card)
        {
            if (!fl && BattleUnitBuf_Sparkle.Instance.PrimaryWeapons.Contains(this) && !IsIntensify)
            {
                var dice = new BattleDiceBehavior();
                dice.behaviourInCard = new DiceBehaviour()
                {
                    Min = 4,
                    Dice = 8,
                    Type = BehaviourType.Def,
                    Detail = BehaviourDetail.Evasion,
                    MotionDetail = MotionDetail.E,
                    //MotionDetailDefault = MotionDetail.N,
                    //KnockbackPower = 1,
                    //EffectRes = "",
                    //ActionScript = "",
                    //Script = "",
                    //Desc = ""
                };
                _owner.currentDiceAction.AddDice(dice);
                fl = true;
            }
            if (!fl && BattleUnitBuf_Sparkle.Instance.PrimaryWeapons.Contains(this) && IsIntensify)
            {
                var dice = new BattleDiceBehavior();
                dice.behaviourInCard = new LOR_DiceSystem.DiceBehaviour()
                {
                    Min = 5,
                    Dice = 8,
                    Type = LOR_DiceSystem.BehaviourType.Def,
                    Detail = LOR_DiceSystem.BehaviourDetail.Evasion,
                    MotionDetail = LOR_DiceSystem.MotionDetail.E,
                    //MotionDetailDefault = LOR_DiceSystem.MotionDetail.N,
                    //KnockbackPower = 1,
                    //EffectRes = "",
                    //ActionScript = "",
                    Script = "ChangeToPenetrate",
                    //Desc = ""
                };
                dice.AddAbility(AssemblyManager.Instance.CreateInstance_DiceCardAbility("ChangeToPenetrate"));
                _owner.currentDiceAction.AddDice(dice);
                fl = true;
            }
        }

        public class DiceCardAbility_ChangeToPenetrate : DiceCardAbilityBase
        {
            public static string Desc = "[拼点胜利]视为以1颗点数相同的突刺骰子命中1次";
            public override void OnWinParrying()
            {
                var dice = behavior.behaviourInCard.Copy();
                behavior.behaviourInCard.Type = BehaviourType.Atk;
                behavior.behaviourInCard.Detail = BehaviourDetail.Penetrate;
                behavior.GiveDamage(behavior.card.target);
                behavior.behaviourInCard = dice;
            }
        }

        public override void OnRollDice(BattleDiceBehavior behavior)
        {
            if (BattleUnitBuf_Sparkle.Instance.PrimaryWeapons.Contains(this))
            {
                behavior.TargetDice?.card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus() { power = -1 });
            }

        }

        public BattleUnitBuf_Bow(BattleUnitModel model) : base(model)
        {
            this.SetFieldValue("_bufIcon", TKS_BloodFiend_Initializer.ArtWorks["千斤弓"]);
            this.SetFieldValue("_iconInit", true);
        }

        public override string BuffName
        {
            get
            {
                string temp = string.Empty;
                if (BattleUnitBuf_Sparkle.Instance.PrimaryWeapons.Contains(this) && !IsIntensify)
                {
                    temp += BattleEffectTextsXmlList.Instance.GetEffectTextName("BattleUnitBuf_ThousandPoundBow") + " ";
                }
                if (BattleUnitBuf_Sparkle.Instance.PrimaryWeapons.Contains(this) && IsIntensify)
                {
                    temp += BattleEffectTextsXmlList.Instance.GetEffectTextName("BattleUnitBuf_ThousandPoundBow_Reinforced") + " ";
                }
                if (BattleUnitBuf_Sparkle.Instance.SubWeapons.Contains(this) && !IsIntensify)
                {
                    temp += BattleEffectTextsXmlList.Instance.GetEffectTextName("BattleUnitBuf_ThousandPoundBow_Secondary") + " ";
                }
                if (BattleUnitBuf_Sparkle.Instance.SubWeapons.Contains(this) && IsIntensify)
                {
                    temp += BattleEffectTextsXmlList.Instance.GetEffectTextName("BattleUnitBuf_ThousandPoundBow_Secondary_Reinforced") + " ";
                }
                if (BattleUnitBuf_Sparkle.Instance.SubWeapons.Contains(this) && BattleUnitBuf_Sparkle.Instance.PrimaryWeapons.Contains(this))
                {
                    temp += BattleEffectTextsXmlList.Instance.GetEffectTextName("BattleUnitBuf_ThousandPoundBow_Together") + " ";
                }
                return temp;
            }
        }

        public override string bufActivatedText
        {
            get
            {
                string temp = string.Empty;
                if (BattleUnitBuf_Sparkle.Instance.PrimaryWeapons.Contains(this) && !IsIntensify)
                {
                    temp += BattleEffectTextsXmlList.Instance.GetEffectTextDesc("BattleUnitBuf_ThousandPoundBow") + "\r\n";
                }
                if (BattleUnitBuf_Sparkle.Instance.PrimaryWeapons.Contains(this) && IsIntensify)
                {
                    temp += BattleEffectTextsXmlList.Instance.GetEffectTextDesc("BattleUnitBuf_ThousandPoundBow_Reinforced") + "\r\n";
                }
                if (BattleUnitBuf_Sparkle.Instance.SubWeapons.Contains(this) && !IsIntensify)
                {
                    temp += BattleEffectTextsXmlList.Instance.GetEffectTextDesc("BattleUnitBuf_ThousandPoundBow_Secondary") + "\r\n";
                }
                if (BattleUnitBuf_Sparkle.Instance.SubWeapons.Contains(this) && IsIntensify)
                {
                    temp += BattleEffectTextsXmlList.Instance.GetEffectTextDesc("BattleUnitBuf_ThousandPoundBow_Secondary_Reinforced") + "\r\n";
                }
                if (BattleUnitBuf_Sparkle.Instance.SubWeapons.Contains(this) && BattleUnitBuf_Sparkle.Instance.PrimaryWeapons.Contains(this))
                {
                    temp += BattleEffectTextsXmlList.Instance.GetEffectTextDesc("BattleUnitBuf_ThousandPoundBow_Together") + "\r\n";
                }
                return temp;
            }
        }
    }
}
