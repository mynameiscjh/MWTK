namespace Don_Eyuil.Don_Eyuil.DiceCardAbility
{
    public class DiceCardAbility_DonEyuil_27 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]消耗目标3层[流血]层流血并使本骰子重复投掷1次(至多4次)";
        int count = 0;
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            if (!target.bufListDetail.HasBuf<BattleUnitBuf_bleeding>())
            {
                return;
            }
            var buf = target.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_bleeding) as BattleUnitBuf_bleeding;
            if (buf.stack >= 3 && count <= 4)
            {
                buf.stack -= 3;
                count++;
                behavior.isBonusAttack = true;
            }
        }
    }
}
