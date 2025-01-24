namespace Don_Eyuil.Don_Eyuil.Player.DiceCardAbility
{
    public class DiceCardAbility_DonEyuil_46 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]使自身获得5层[硬血结晶]";

        public override string[] Keywords => new string[] { "BattleUnitBuf_BleedCrystal" };

        public override void OnSucceedAttack(BattleUnitModel target)
        {
            BattleUnitBuf_HardBlood_Crystal.GainBuf<BattleUnitBuf_HardBlood_Crystal>(owner, 5);
        }
    }
}
