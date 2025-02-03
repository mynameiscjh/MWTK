using Don_Eyuil.San_Sora.Player.Buff;

namespace Don_Eyuil.San_Sora.Player.DiceCardAbility
{
    public class DiceCardAbility_SanSora_Dice18 : DiceCardAbilityBase
    {
        public static string Desc = "若本书页应用在[血剑]骰子上则使本骰子重复投掷1次";
        bool fl = false;
        public override void OnSucceedAttack()
        {
            if (fl) { return; }
            var buf = BattleUnitBuf_SanSora.GetBuf<BattleUnitBuf_SanSora>(owner);
            if (buf == null || buf.Sword == null || buf.Sword.Card == null)
            {
                return;
            }
            if (buf.Sword.Card == card)
            {
                this.ActivateBonusAttackDice();
                fl = true;
            }
        }
    }
}
