using static Don_Eyuil.Don_Eyuil.Player.DiceCardSelfAbility.DiceCardSelfAbility_DonEyuil_69;

namespace Don_Eyuil.Don_Eyuil.Player.DiceCardAbility
{
    public class DiceCardAbility_DonEyuil_49 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]重复触发目标5次[流血]";
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            for (int i = 0; i < 5; i++)
            {
                if (target.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_bleeding) == null)
                {
                    return;
                }
                target.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_bleeding).AfterDiceAction(behavior);
            }
            BattleUnitBuf_Don_Eyuil.GetBuf<BattleUnitBuf_AddThistles>(owner)?.Destroy();
        }
    }
}
