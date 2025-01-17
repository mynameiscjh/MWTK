namespace Don_Eyuil.DiceCardAbility
{
    public class DiceCardAbility_greta_destroyDice : DiceCardAbilityBase
    {
        public static string Desc = "[拼点胜利/失败]摧毁目标/自身书页所有骰子";

        public override void OnWinParrying()
        {
            BattlePlayingCardDataInUnitModel card = base.card;
            if (card == null)
            {
                return;
            }
            BattleUnitModel target = card.target;
            if (target == null)
            {
                return;
            }
            BattlePlayingCardDataInUnitModel currentDiceAction = target.currentDiceAction;
            if (currentDiceAction == null)
            {
                return;
            }
            currentDiceAction.DestroyDice(DiceMatch.AllDice, DiceUITiming.Start);
        }

        public override void OnLoseParrying()
        {
            base.card.DestroyDice(DiceMatch.AllDice, DiceUITiming.Start);
        }

    }
}
