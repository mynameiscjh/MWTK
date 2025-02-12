using BattleCharacterProfile;
using Don_Eyuil.Don_Eyuil.Player.Buff;
using Don_Eyuil.San_Sora.Player.Buff;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static BattleCharacterProfile.BattleCharacterProfileUI;

namespace Don_Eyuil.San_Sora
{
    public class DiceCardAbility_SanSora_Dice01 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]消耗至多15层[血羽]每消耗5层便使本骰子重复投掷1次并施加2层[流血]";
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            if(_repeatMax == -1)
            {
                int Count = Math.Min(3,BattleUnitBuf_Feather.GetBufStack<BattleUnitBuf_Feather>(owner) / 5);
                _repeatMax = Count;
                target.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Bleeding, 2 * Count);
                BattleUnitBuf_Feather.GainBuf<BattleUnitBuf_Feather>(owner, -15);
            }
            //0 1 2 3 
            if (this._repeatCount < _repeatMax)
            {
                this._repeatCount++;
                base.ActivateBonusAttackDice();
            }
            else
            {
                _repeatMax = -1;
            }
        }
        private int _repeatMax = -1;
        private int _repeatCount;
    }
    public class DiceCardAbility_SanSora_Dice02 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]获得4层[血羽]";
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            BattleUnitBuf_Feather.GainBuf<BattleUnitBuf_Feather>(owner, 4);
        }
        public override void OnSucceedAreaAttack(BattleUnitModel target)
        {
            BattleUnitBuf_Feather.GainBuf<BattleUnitBuf_Feather>(owner, 4);
        }
    }
    public class DiceCardAbility_SanSora_Dice03 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]若目标拥有至少3层[流血]则使自身在下一幕得2层[迅捷]";
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            if(target.bufListDetail.GetKewordBufStack(KeywordBuf.Bleeding) >= 3)
            {
                owner.bufListDetail.AddKeywordBufByCard(KeywordBuf.Quickness, 2, owner);
            }
        }
    }
    public class DiceCardAbility_SanSora_Dice04 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]消耗3层[血羽]对目标下两幕对目标施加4层[流血]";
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            if(BattleUnitBuf_Feather.UseBuf<BattleUnitBuf_Feather>(owner,3))
            {
                target.bufListDetail.AddKeywordBufNextNextByCard(KeywordBuf.Bleeding, 4,owner);
                target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding,4,owner);
            }
        }
    }
    //群体书页
    public class DiceCardAbility_SanSora_Dice05 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]施加2层[流血]";
        public override void OnSucceedAreaAttack(BattleUnitModel target)
        {
            target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, 2, owner);
        }
    }
    public class DiceCardAbility_SanSora_Dice06 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]这一幕使目标被施加的[流血]层数+1";
        public class BattleUnitBuf_HardBloodBleedingAddition : BattleUnitBuf_Don_Eyuil
        {
            public BattleUnitBuf_HardBloodBleedingAddition(BattleUnitModel model) : base(model) { }
            public override void BeforeAddKeywordBuf(KeywordBuf BufType, ref int Stack)
            {
                if(BufType == KeywordBuf.Bleeding) { Stack += 1; }
            }
            public override void OnRoundEnd()
            {
                this.Destroy();
            }
        }
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            BattleUnitBuf_HardBloodBleedingAddition.GainBuf<BattleUnitBuf_HardBloodBleedingAddition>(target, 1);
        }
    }
    public class DiceCardAbility_SanSora_Dice07 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]若本骰子基础值不低于6则使本骰子重复投掷一次(至多2次)";
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            if (behavior.DiceVanillaValue >= 6 && this._repeatCount < 2)
            {
                this._repeatCount++;
                base.ActivateBonusAttackDice();
            }
        }
        private int _repeatCount;
    }
    public class DiceCardAbility_SanSora_Dice23 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]这一幕对目标施加3层[流血]";
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            target.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Bleeding, 3, owner);
        }
    }
    public class DiceCardAbility_SanSora_Dice24 : DiceCardAbilityBase
    {
        public static string Desc = "[拼点失败]这一幕与下一幕使自身获得3层[强壮]";
        public override void OnLoseParrying()
        {
            owner.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Strength,3, owner);
            owner.bufListDetail.AddKeywordBufByCard(KeywordBuf.Strength, 3, owner);
        }
    }
    public class DiceCardAbility_SanSora_Dice25 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]这一幕自身每获得1点正面情感便时自身获得1层[血羽]";
        public class BattleUnitBuf_HardBloodBleedingAddition : BattleUnitBuf_Don_Eyuil
        {
            public BattleUnitBuf_HardBloodBleedingAddition(BattleUnitModel model) : base(model) { }
            public override void BeforeAddEmotionCoin(EmotionCoinType CoinType, ref int Count)
            {
                if(CoinType == EmotionCoinType.Positive)
                {
                    BattleUnitBuf_Feather.GainBuf<BattleUnitBuf_Feather>(owner, Count);
                }
            }
            public override void OnRoundEnd()
            {
                this.Destroy();
            }
        }
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            BattleUnitBuf_HardBloodBleedingAddition.GainBuf<BattleUnitBuf_HardBloodBleedingAddition>(owner, 1);
        }
    }
    public class DiceCardAbility_SanSora_Dice26 : DiceCardAbilityBase
    {
        public override void BeforeRollDice()
        {
            behavior.ApplyDiceStatBonus(new DiceStatBonus
            {
                dmgRate = -9999,
                breakRate = -9999
            });
        }
        public static string Desc = "本骰子不造成伤害与混乱伤害";
    }
}
