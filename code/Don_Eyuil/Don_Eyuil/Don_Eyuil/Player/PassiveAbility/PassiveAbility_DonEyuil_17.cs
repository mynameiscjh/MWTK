
using Don_Eyuil.Don_Eyuil.Player.Buff;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Don_Eyuil.Don_Eyuil.Player.PassiveAbility
{
    public class PassiveAbility_DonEyuil_17 : PassiveAbilityBase
    {
        public override string debugDesc => "在一幕中自身每获得3点正面情感便使自身在下一幕中获得1层\"强壮\"与\"迅捷\"(至多2层)\r\n获得正面情感时若自身情感槽已被填满则使情感最低的一名友方角色获得1点正面情感\r\n若开启舞台时只有自身一名我方角色…\r\n(获得效果 光荣的决斗)\r\n(不可转移)\r\n";

        public override void OnWaveStart()
        {
            BattleUnitBuf_Don_Eyuil.GainBuf<BattleUnitBuf_Emo>(owner, 1);
        }

        public int posNum = 0;

        public override void OnRoundStart()
        {
            posNum = owner.emotionDetail.PositiveCoins.Count;

            if (BattleObjectManager.instance.GetAliveList(owner.faction).Count == 1 && !owner.IsDead())
            {
                BattleUnitBuf_Don_Eyuil.GetOrAddBuf<BattleUnitBuf_Duel>(owner);
            }

        }
        public override void OnRoundEnd()
        {
            int temp = owner.emotionDetail.PositiveCoins.Count;

            owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Strength, Math.Min((temp - posNum) / 3, 2));
            owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Quickness, Math.Min((temp - posNum) / 3, 2));
        }

        public class BattleUnitBuf_Emo : BattleUnitBuf_Don_Eyuil
        {
            public override void BeforeAddEmotionCoin(EmotionCoinType CoinType, ref int Count)
            {
                if (_owner.emotionDetail.AllEmotionCoins.Count >= _owner.emotionDetail.MaximumCoinNumber && CoinType == EmotionCoinType.Positive)
                {
                    var temp = new List<BattleUnitModel>(BattleObjectManager.instance.GetAliveList(_owner.faction));
                    temp.Remove(_owner);

                    if (temp.Count <= 0)
                    {
                        return;
                    }

                    temp.Sort((x, y) => x.emotionDetail.EmotionLevel - y.emotionDetail.EmotionLevel);

                    temp.First()?.emotionDetail?.CreateEmotionCoin(EmotionCoinType.Positive, 1);
                }
            }

            public BattleUnitBuf_Emo(BattleUnitModel model) : base(model)
            {
            }
        }

    }

}
