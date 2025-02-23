namespace Don_Eyuil.Don_Eyuil.Player.DiceCardSelfAbility
{
    public class DiceCardSelfAbility_DonEyuil_137 : DiceCardSelfAbilityBase
    {
        public static string Desc = "堂埃尤尔专属书页\r\n[使用时]获得1点正面情感若自身情感等级达到5则额外恢复1点光芒";

        public override void OnUseCard()
        {
            owner.emotionDetail.CreateEmotionCoin(EmotionCoinType.Positive, 1);
            if (owner.emotionDetail.EmotionLevel >= 5)
            {
                owner.cardSlotDetail.RecoverPlayPoint(1);
            }
        }
    }
}
