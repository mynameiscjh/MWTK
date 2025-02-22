﻿namespace Don_Eyuil.Don_Eyuil.Player.DiceCardAbility
{
    public class DiceCardAbility_DonEyuil_57 : DiceCardAbilityBase
    {
        public static string Desc = "[拼点胜利]施加4层[流血]并摧毁目标书页剩余骰子";
        public override string[] Keywords => new string[] { "Bleeding_Keyword" };
        public override void OnWinParrying()
        {
            this.card.target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, 4, owner);
            this.behavior.TargetDice.card.DestroyDice(DiceMatch.AllDice);
        }
    }
}
