namespace Don_Eyuil.Don_Eyuil.Player.DiceCardAbility
{
    public class DiceCardAbility_DonEyuil_Dice_31 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]使本书页施加[流血]时额外触发一次";

        public override void OnSucceedAttack(BattleUnitModel target)
        {
            target.bufListDetail.GetActivatedBuf(KeywordBuf.Bleeding).AfterDiceAction(behavior);
        }
    }
}
