using System.Linq;

namespace Don_Eyuil.WhiteMoon_Sparkle.Player.DiceCardAbility
{
    public class DiceCardAbility_WhiteMoonSparkle_Dice15 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]下一幕对目标施加1层束缚与麻痹并使目标层数最多的负面状态层数+15%(向上取整)";

        public override void OnSucceedAttack(BattleUnitModel target)
        {
            target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Blurry, 1, owner);
            target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Paralysis, 1, owner);
            var list = target.bufListDetail.GetActivatedBufList().Where(x => x.positiveType == BufPositiveType.Negative).ToList();
            list.Sort((x, y) => y.stack - x.stack);
            if (list.Count > 0)
            {
                list.First().stack += (int)(list.First().stack * 0.15f + 0.5f);
            }
        }
    }
}
