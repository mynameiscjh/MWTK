using System.Linq;

namespace Don_Eyuil.San_Sora.Player.PassiveAbility
{
    public class PassiveAbility_SanSora_12 : PassiveAbilityBase
    {
        public override string debugDesc => "同一舞台中自身每累计获得10点正面情感便使自身永久获得1层”迅捷”与”伤害提升”(至多2层)";
        int count = 0;
        public override void OnWaveStart()
        {
            count = owner.emotionDetail.AllEmotionCoins.Where(x => x.CoinType == EmotionCoinType.Positive).Count();
        }
        public override void OnRoundStart()
        {
            var temp = owner.emotionDetail.AllEmotionCoins.Where(x => x.CoinType == EmotionCoinType.Positive).Count() - count;

            if (temp >= 10)
            {
                owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Quickness, 1);
                owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.DmgUp, 1);
            }
            if (temp >= 20)
            {
                owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Quickness, 1);
                owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.DmgUp, 1);
            }
        }
    }
}
