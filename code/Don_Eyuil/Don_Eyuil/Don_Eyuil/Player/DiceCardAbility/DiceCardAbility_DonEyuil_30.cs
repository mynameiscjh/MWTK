using Don_Eyuil.Don_Eyuil.Buff;

namespace Don_Eyuil.Don_Eyuil.DiceCardAbility
{
    public class DiceCardAbility_DonEyuil_30 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]对目标施加等同于目标[流血]层数的[血晶荆棘]";
        public override void OnSucceedAreaAttack(BattleUnitModel target)
        {
            if (!target.bufListDetail.HasBuf<BattleUnitBuf_bleeding>())
            {
                return;
            }

            //BattleUnitBuf_Don_Eyuil.GetBufStack<BattleUnitBuf_bleeding>()
            BattleUnitBuf_BloodCrystalThorn.GainBuf<BattleUnitBuf_BloodCrystalThorn>(target, target.bufListDetail.GetKewordBufStack(KeywordBuf.Bleeding));

        }
    }
}
