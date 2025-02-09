using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Don_Eyuil.WhiteMoon_Sparkle.Player.DiceCardAbility
{
    public class DiceCardAbility_WhiteMoonSparkle_Dice27 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]下一幕开始时抽取1张书页";

        public override void OnSucceedAttack()
        {
            BattleUnitBuf_Don_Eyuil.GainBuf<BattleUnitBuf_Card>(owner, 1);
        }

        public class BattleUnitBuf_Card : BattleUnitBuf_Don_Eyuil
        {
            public override void OnRoundStart()
            {
                _owner.allyCardDetail.DrawCards(1);
                Destroy();
            }

            public BattleUnitBuf_Card(BattleUnitModel model) : base(model)
            {
            }
        }
    }
}
