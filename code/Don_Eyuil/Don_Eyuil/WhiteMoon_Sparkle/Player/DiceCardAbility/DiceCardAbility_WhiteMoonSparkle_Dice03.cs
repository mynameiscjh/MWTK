using System.Linq;

namespace Don_Eyuil.WhiteMoon_Sparkle.Player.DiceCardAbility
{
    public class DiceCardAbility_WhiteMoonSparkle_Dice03 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]下一幕开始时使目标层数最多的负面状态层数+20%(向上取整)";

        public override void OnSucceedAttack(BattleUnitModel target)
        {
            var temp = target.bufListDetail.GetActivatedBufList().Where(x => x.positiveType == BufPositiveType.Negative).ToList();
            temp.Sort((x, y) => y.stack - x.stack);
            if (temp.Count > 0)
            {
                temp.First().stack += (int)(temp.First().stack / 5.0f + 0.5f);
            }
        }
    }
}
