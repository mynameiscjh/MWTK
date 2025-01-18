using Don_Eyuil.Buff;

namespace Don_Eyuil.Don_Eyuil.Player.DiceCardAbility
{
    public class DiceCardAbility_DonEyuil_48 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]消耗3层[结晶硬血]并施加3层[无法凝结的血]";
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            if (BattleUnitBuf_BleedCrystal.UseBuf(target, 3))
            {
                BattleUnitBuf_Flow.GainBuf(target, 3);
            }
        }
    }
}
