namespace Don_Eyuil.Don_Eyuil.Player.DiceCardAbility
{
    public class DiceCardAbility_DonEyuil_Dice_29 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]将自身[流血]转移至目标";

        public override void OnSucceedAttack(BattleUnitModel target)
        {
            var bufStack = owner.bufListDetail.GetActivatedBuf(KeywordBuf.Bleeding)?.stack ?? 0;

            target.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Bleeding, bufStack, owner);

            owner.bufListDetail.GetActivatedBuf(KeywordBuf.Bleeding)?.Destroy();
        }
    }
}
