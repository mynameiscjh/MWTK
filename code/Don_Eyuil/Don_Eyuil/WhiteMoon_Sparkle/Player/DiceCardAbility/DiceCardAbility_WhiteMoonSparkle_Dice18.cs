namespace Don_Eyuil.WhiteMoon_Sparkle.Player.DiceCardAbility
{
    public class DiceCardAbility_WhiteMoonSparkle_Dice18 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]使自身获得1点正面情感";

        public override void OnSucceedAttack()
        {
            owner.emotionDetail.CreateEmotionCoin(EmotionCoinType.Positive);
        }
    }
}
