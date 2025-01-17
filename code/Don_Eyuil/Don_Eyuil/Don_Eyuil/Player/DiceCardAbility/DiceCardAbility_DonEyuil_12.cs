using Don_Eyuil.Buff;

namespace Don_Eyuil.DiceCardAbility
{
    public class DiceCardAbility_DonEyuil_12 : DiceCardAbilityBase
    {
        public static string Desc = "本骰子将重复投掷2次自身每有10层[结晶硬血]便使该次数+1\r\n[命中时]对目标施加1层[流血]";
        public int count = 2;
        bool fl = false;
        public override void OnSucceedAttack()
        {
            if (!fl)
            {
                fl = true;
                var temp = BattleUnitBuf_BleedCrystal.GetBuf(owner);
                count += temp != null ? temp.stack / 10 : 0;
            }

            if (count > 0)
            {
                ActivateBonusAttackDice();
                count--;
            }

            this.card.target.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Bleeding, 1);

            base.OnRollDice();
        }
    }
}
