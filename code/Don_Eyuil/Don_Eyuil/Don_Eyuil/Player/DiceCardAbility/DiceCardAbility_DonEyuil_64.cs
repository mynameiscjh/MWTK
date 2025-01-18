using System.Collections.Generic;

namespace Don_Eyuil.Don_Eyuil.Player.DiceCardAbility
{
    public class DiceCardAbility_DonEyuil_64 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]重复触发目标(自身设置的硬血术书页)次[流血]";

        public override void OnSucceedAreaAttack(BattleUnitModel target)
        {
            var temp = owner.UnitData.unitData.GetDeckForBattle(1).FindAll(x => x.Name.Contains("堂埃尤尔派硬血术")).Count;
            var buf = target.bufListDetail.GetFieldValue<List<BattleUnitBuf>>("_bufList").Find(x => x is BattleUnitBuf_bleeding) as BattleUnitBuf_bleeding;
            if (buf == null)
            {
                return;
            }
            for (int i = 0; i < temp; i++)
            {
                buf.AfterDiceAction(behavior);
            }
        }
    }
}
