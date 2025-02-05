namespace Don_Eyuil.Don_Eyuil.Player.DiceCardSelfAbility
{
    public class DiceCardSelfAbility_DonEyuil_61 : DiceCardSelfAbilityBase
    {
        public static string Desc = "本书页命中目标时将使自身获得3点护盾";
        public override string[] Keywords => new string[] { "DonEyuil" };
        public override void OnSucceedAreaAttack(BattleUnitModel target)
        {
            BattleUnitBuf_BloodShield.GainBuf<BattleUnitBuf_BloodShield>(owner, 3);
        }
    }
}
