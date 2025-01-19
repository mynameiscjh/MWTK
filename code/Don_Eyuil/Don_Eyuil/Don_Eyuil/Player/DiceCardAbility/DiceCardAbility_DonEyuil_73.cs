using Don_Eyuil.Buff;
using Don_Eyuil.Don_Eyuil.Player.Buff;

namespace Don_Eyuil.Don_Eyuil.Player.DiceCardAbility
{
    public class DiceCardAbility_DonEyuil_73 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]若自己已经激活[血枪]这一幕对目标施加2层[无法凝结的血]";
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            if (BattleUnitBuf_Don_Eyuil.GetBuf<BattleUnitBuf_HardBlood>(owner) == null)
            {
                return;
            }
            if (BattleUnitBuf_Don_Eyuil.GetBuf<BattleUnitBuf_HardBlood>(owner).Lance != null)
            {
                BattleUnitBuf_UncondensableBlood.GainBuf<BattleUnitBuf_UncondensableBlood>(target, 2);
            }
        }
    }
}
