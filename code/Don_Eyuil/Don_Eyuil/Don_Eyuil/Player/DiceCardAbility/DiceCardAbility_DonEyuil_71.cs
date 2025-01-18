namespace Don_Eyuil.Don_Eyuil.Player.DiceCardAbility
{
    public class DiceCardAbility_DonEyuil_71 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]将自身的[流血]转移至目标";
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            if (!owner.bufListDetail.HasBuf<BattleUnitBuf_bleeding>())
            {
                return;
            }
            var buf = owner.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_bleeding);
            target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, buf.stack, owner);
            buf.Destroy();
        }
    }
}
