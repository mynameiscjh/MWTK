using System.Linq;

namespace Don_Eyuil.WhiteMoon_Sparkle.Player.DiceCardAbility
{
    public class DiceCardAbility_WhiteMoonSparkle_Dice04 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]使情感硬币最多的1名友方角色获得2点正面情感";

        public override void OnSucceedAttack()
        {
            var temp = BattleObjectManager.instance.GetAliveList(owner.faction);
            temp.Sort((x, y) => y.emotionDetail.AllEmotionCoins.Count - x.emotionDetail.AllEmotionCoins.Count);
            if (temp.Count > 0)
            {
                temp.First().emotionDetail.CreateEmotionCoin(EmotionCoinType.Positive, 2);
            }
        }
    }
}
