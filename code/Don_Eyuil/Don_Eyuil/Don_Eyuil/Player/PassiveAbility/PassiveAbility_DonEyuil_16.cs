using System;

namespace Don_Eyuil.Don_Eyuil.Player.PassiveAbility
{
    public class PassiveAbility_DonEyuil_16 : PassiveAbilityBase_Don_Eyuil
    {
        public override string debugDesc => "自身所有第一颗进攻型骰子命中带有\"流血\"的目标时将恢复等同于自身骰子基础值的体力\r\n在一幕中自身每恢复3点体力便在下一幕获得1层\"伤害提升\"(至多3层)\r\n";

        public bool fl = false;

        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            fl = true;
        }

        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            if (behavior.card.target.bufListDetail.HasBuf<BattleUnitBuf_bleeding>() && fl)
            {
                owner.RecoverHP(2);
                fl = false;
            }
        }

        int count = 0;

        public override void OnRoundStart()
        {
            owner.bufListDetail.AddKeywordBufByCard(KeywordBuf.DmgUp, Math.Min(3, count / 3), owner);

            count = 0;
        }

        public override void BeforeRecoverHP(ref int v)
        {
            count += v;
        }

    }
}
