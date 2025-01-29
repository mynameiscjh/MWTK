using LOR_BattleUnit_UI;

namespace Don_Eyuil.San_Sora.Player.Buff
{
    public class BattleUnitBuf_Lance : BattleUnitBuf_SanHardBlood
    {
        public static string Desc = "若本速度骰子使用书页时速度不低于6则使该书页施加的”流血”层数翻倍且以本书页消耗”血羽”时返还消耗量/2";

        public BattleUnitBuf_Lance(SpeedDiceUI dice) : base(dice)
        {
            //dice.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = TKS_BloodFiend_Initializer.ArtWorks["血枪骰子"];
        }

        public override void BeforeAddKeywordBuf(KeywordBuf BufType, ref int Stack)
        {
            if (_owner.currentDiceAction == null)
            {
                return;
            }

            if (_owner.currentDiceAction == Card && _owner.currentDiceAction.speedDiceResultValue >= 6 && BufType == KeywordBuf.Bleeding)
            {
                Stack *= 2;
            }
        }
    }
}
