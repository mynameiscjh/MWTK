using static Don_Eyuil.WhiteMoon_Sparkle.Player.Buff.BattleUnitBuf_Sparkle;

namespace Don_Eyuil.WhiteMoon_Sparkle.Player.Buff
{
    public class BattleUnitBuf_Year : BattleUnitBuf_Don_Eyuil
    {
        public static string Desc =
            "泉之龙/秋之莲（主）[中立buff]:\r\n自身所有骰子最大值+3\r\n自身防御型骰子拼点胜利时将骰子类型更改为随机进攻型骰子\r\n自身拼点失败时将骰子类型更改为招架\r\n自身每幕首次拼点胜利时使自身下一幕获得1层迅捷\r\n" +
            "\r\n泉之龙/秋之莲（副）[中立buff]:\r\n自身每幕第1张书页造成的伤害减少至50%但命中时额外造成1次伤害(可触发骰子命中效果以外的命中效果)\r\n" +
            "\r\n泉之龙/秋之莲+（强化主）[中立buff]:\r\n自身所有骰子最大值+3\r\n自身防御型骰子拼点胜利时将骰子类型更改为随机进攻型骰子\r\n自身拼点失败时将骰子类型更改为招架\r\n自身拼点胜利时使自身下一幕获得1层迅捷(至多2层)自身速度高于目标时拼点胜利时造成的伤害与混乱伤害+3拼点失败时受到的伤害与混乱伤害-2\r\n" +
            "\r\n泉之龙/秋之莲+（强化副）[中立buff]:\r\n自身每幕前2张书页造成的伤害减少至75%但命中时额外造成2次伤害（可触发骰子命中效果以外的命中效果）\r\n" +
            "\r\n泉之龙/秋之莲（主副同用额外效果）[中立buff]:\r\n自身命中目标时使下颗骰子最小值+1\r\n自身每命中敌人5/8次便使自身恢复1点光芒/抽取1张书页\r\n";

        public BattleUnitBuf_Year(BattleUnitModel model) : base(model)
        {
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
            if (Instance.SubWeapons.Contains(this) && !IsIntensify)
            {
                foreach (var item in _owner.cardSlotDetail.cardAry)
                {
                    item?.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus() { dmgRate = -50 });
                }
            }

            if (Instance.SubWeapons.Contains(this) && IsIntensify)
            {
                foreach (var item in _owner.cardSlotDetail.cardAry)
                {
                    item?.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus() { dmgRate = -75 });
                }
            }
        }

        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            if (Instance.SubWeapons.Contains(this) && !IsIntensify)
            {
                behavior.GiveDamage(behavior.card.target);
            }
            if (Instance.SubWeapons.Contains(this) && IsIntensify)
            {
                behavior.GiveDamage(behavior.card.target);
                behavior.GiveDamage(behavior.card.target);
            }
        }

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

        public class BattleDiceCardBuf_TransDice : BattleDiceCardBuf { }
    }
}
