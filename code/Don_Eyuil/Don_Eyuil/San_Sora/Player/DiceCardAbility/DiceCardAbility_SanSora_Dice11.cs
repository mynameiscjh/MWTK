namespace Don_Eyuil.San_Sora.Player.DiceCardAbility
{
    public class DiceCardAbility_SanSora_Dice11 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]若目标带有[流血]则抽取一张书页";

        public override void OnSucceedAttack(BattleUnitModel target)
        {
            if (target.bufListDetail.HasBuf<BattleUnitBuf_bleeding>())
            {
                owner.allyCardDetail.DrawCards(1);
            }
        }
    }
}
