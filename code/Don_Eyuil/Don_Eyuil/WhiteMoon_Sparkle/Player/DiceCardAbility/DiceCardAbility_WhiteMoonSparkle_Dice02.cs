using System.Linq;

namespace Don_Eyuil.WhiteMoon_Sparkle.Player.DiceCardAbility
{
    public class DiceCardAbility_WhiteMoonSparkle_Dice02 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]使目前正面情感最多的我方角色下一幕获得1层[强壮]";

        public override void OnSucceedAttack()
        {
            var temp = BattleObjectManager.instance.GetAliveList(owner.faction);
            temp.Sort((x, y) => y.emotionDetail.PositiveCoins.Count - x.emotionDetail.PositiveCoins.Count);
            if (temp.Count <= 0)
            {
                return;
            }
            temp.First().bufListDetail.AddKeywordBufByCard(KeywordBuf.Strength, 1, owner);
        }
    }
}
