using Don_Eyuil.San_Sora.Player.Buff;
using static Don_Eyuil.San_Sora.Player.DiceCardSelfAbility.DiceCardSelfAbility_SanSora_13;

namespace Don_Eyuil.San_Sora.Player.DiceCardAbility
{
    public class DiceCardAbility_SanSora_Dice21 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]这一幕自身每获得1点正面情感便时自身获得1层[血羽]";

        public override void OnSucceedAttack()
        {
            var buf = BattleUnitBuf_EmoCoin.GetBuf<BattleUnitBuf_EmoCoin>(owner);
            if (buf == null) { return; }
            BattleUnitBuf_Don_Eyuil.GainBuf<BattleUnitBuf_Feather>(owner, buf.count);
        }
    }
}
