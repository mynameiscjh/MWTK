namespace Don_Eyuil.Don_Eyuil.Buff
{
    public class BattleUnitBuf_Armour : BattleUnitBuf_Don_Eyuil
    {
        public static string Desc = "自身护盾减少时将视作受到等量\"流血\"伤害\r\n每幕结束时自身每有一颗未被使用的防御型骰子便使自身获得10点护盾\r\n自身拥有护盾时被命中时对命中者施加1-2层\"流血\"\r\n自身陷入混乱时若护盾层数不低于30层则消耗30层护盾并解除混乱";

        protected override string keywordId => "BattleUnitBuf_Armour";

        public BattleUnitBuf_Armour(BattleUnitModel model) : base(model)
        {

        }

        public override void OnRoundEnd()
        {
            foreach (var item in _owner.cardSlotDetail.keepCard.cardBehaviorQueue)
            {
                if (item.Type == LOR_DiceSystem.BehaviourType.Def)
                {
                    BattleUnitBuf_BloodShield.GainBuf<BattleUnitBuf_BloodShield>(_owner, 10);
                }
            }
        }
        public override void OnBreakState()
        {
            if (BattleUnitBuf_BloodShield.GetBufStack<BattleUnitBuf_BloodShield>(_owner) < 30)
            {
                return;
            }
            var buf = BattleUnitBuf_BloodShield.GetBuf<BattleUnitBuf_BloodShield>(_owner);
            buf.ReduceShield(30);
            this._owner.breakDetail.ResetGauge();
        }
        public override double ChangeDamage(BattleUnitModel attacker, double dmg)
        {
            if (BattleUnitBuf_BloodShield.GetBufStack<BattleUnitBuf_BloodShield>(_owner) > 0)
            {
                attacker.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Bleeding, UnityEngine.Random.Range(1, 2));
            }

            return base.ChangeDamage(attacker, dmg);
        }

        public override bool CanRecoverBreak(int amount)
        {
            return base.CanRecoverBreak(amount);
        }

    }
}
