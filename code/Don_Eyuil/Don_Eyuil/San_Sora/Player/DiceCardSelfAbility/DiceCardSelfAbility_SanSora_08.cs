namespace Don_Eyuil.San_Sora.Player.DiceCardSelfAbility
{
    public class DiceCardSelfAbility_SanSora_08 : DiceCardSelfAbilityBase
    {
        public static string Desc = "[战斗开始]这一幕对自身施加3层[流血] 这一幕中若自身至少受到10点[流血]伤害则额外恢复1点光芒";

        public override void OnStartBattle()
        {
            owner.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Bleeding, 3, owner);
            owner.bufListDetail.AddBuf(new BattleUnitBuf_Count(owner));
        }

        public class BattleUnitBuf_Count : BattleUnitBuf_Don_Eyuil
        {
            public BattleUnitBuf_Count(BattleUnitModel model) : base(model)
            {
            }

            public int count = 0;

            public override void AfterTakeBleedingDamage(int Dmg)
            {
                count += Dmg;
            }

            public override void OnRoundEnd()
            {
                if (count >= 10)
                {
                    owner.cardSlotDetail.RecoverPlayPoint(1);
                }
                count = 0;
                this.Destroy();
            }
        }

    }
}
