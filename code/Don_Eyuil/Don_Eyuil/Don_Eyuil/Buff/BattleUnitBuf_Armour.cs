using UnityEngine;

namespace Don_Eyuil.Don_Eyuil.Buff
{
    public class BattleUnitBuf_Armour : BattleUnitBuf
    {
        public static string Desc = "自身护盾减少时将视作受到等量\"流血\"伤害\r\n每幕结束时自身每有一颗未被使用的防御型骰子便使自身获得10点护盾\r\n自身拥有护盾时被命中时对命中者施加1-2层\"流血\"\r\n自身陷入混乱时若护盾层数不低于30层则消耗30层护盾并解除混乱";

        public override void Init(BattleUnitModel owner)
        {
            base.Init(owner);
            if (owner.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_PhysicalShield) is BattleUnitBuf_PhysicalShield)
            {
                var buf = owner.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_PhysicalShield) as BattleUnitBuf_PhysicalShield;
                buf.ChangeColor(new Color(1, 0, 0, 1));
            }
        }

        public override void OnRoundEnd()
        {
            foreach (var item in _owner.cardSlotDetail.keepCard.cardBehaviorQueue)
            {
                if (item.Type == LOR_DiceSystem.BehaviourType.Def)
                {
                    BattleUnitBuf_PhysicalShield.AddBuf(_owner, 10);
                }
            }
        }

        public override void OnBreakState()
        {
            if (!_owner.bufListDetail.HasBuf<BattleUnitBuf_PhysicalShield>() || BattleUnitBuf_PhysicalShield.GetBuf(_owner) < 30)
            {
                return;
            }
            var buf = _owner.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_PhysicalShield) as BattleUnitBuf_PhysicalShield;
            buf.ReduceShield(30);
            this._owner.breakDetail.ResetGauge();
        }

        public override double ChangeDamage(BattleUnitModel attacker, double dmg)
        {
            if (_owner.bufListDetail.HasBuf<BattleUnitBuf_PhysicalShield>())
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
