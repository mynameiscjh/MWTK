using Don_Eyuil.Buff;

namespace Don_Eyuil.Don_Eyuil.Player.DiceCardAbility
{
    public class DiceCardAbility_DonEyuil_46 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]使自身获得5层[硬血结晶]";

        public override void OnSucceedAttack(BattleUnitModel target)
        {
            BattleUnitBuf_BleedCrystal.GainBuf(owner, 5);
        }
    }
}
