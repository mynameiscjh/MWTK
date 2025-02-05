using Don_Eyuil.San_Sora.Player.Buff;

namespace Don_Eyuil.San_Sora.Player.DiceCardSelfAbility
{
    public class DiceCardSelfAbility_SanSora_13 : DiceCardSelfAbilityBase
    {
        public static string Desc = "[使用时]抽取1张书页并获得5层[血羽] 顺便帮第一颗骰子计算一下正面情感";

        public override void OnStartBattle()
        {
            base.OnStartBattle();
            BattleUnitBuf_Don_Eyuil.GainBuf<BattleUnitBuf_EmoCoin>(owner, 1);
        }

        public override void OnUseCard()
        {
            owner.allyCardDetail.DrawCards(1);
            BattleUnitBuf_Don_Eyuil.GainBuf<BattleUnitBuf_Feather>(owner, 5);
        }

        public class BattleUnitBuf_EmoCoin : BattleUnitBuf_Don_Eyuil
        {
            public int count = 0;
            public override void BeforeAddEmotionCoin(EmotionCoinType CoinType, ref int Count)
            {
                if (CoinType == EmotionCoinType.Positive)
                {
                    count += Count;
                }
            }

            public override void OnRoundEnd()
            {
                Destroy();
            }

            public BattleUnitBuf_EmoCoin(BattleUnitModel model) : base(model)
            {
            }
        }
    }
}
