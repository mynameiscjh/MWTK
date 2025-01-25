namespace Don_Eyuil.Don_Eyuil.Player.DiceCardSelfAbility
{
    public class DiceCardSelfAbility_DonEyuil_67 : DiceCardSelfAbilityBase
    {
        //TODO: 实现仅限装备了硬血术8式和9式时装备效果
        public static string Desc = "仅限装备了硬血术8式和9式时装备\r\n[使用时]恢复1点光芒\r\n若自身在这一幕时至少承受了6次流血伤害则在本幕结束时额外恢复1点光芒\r\n";
        public override string[] Keywords => new string[] { "Bleeding_Keyword", "DonEyuil", "DonEyuil_8_9" };
        public override void OnUseCard()
        {
            owner.cardSlotDetail.RecoverPlayPointByCard(1);
            owner.bufListDetail.AddBuf(new BattleUnitBuf_BleedCount(owner));
        }
        public class BattleUnitBuf_BleedCount : BattleUnitBuf_Don_Eyuil
        {
            public int count = 0;
            public BattleUnitBuf_BleedCount(BattleUnitModel model) : base(model)
            {
            }
            public override void OnRoundStart()
            {
                count = 0;
            }
            public override void AfterTakeBleedingDamage(int Dmg)
            {
                count++;
            }
            public override void OnRoundEnd()
            {
                if (count >= 6)
                {
                    _owner.cardSlotDetail.RecoverPlayPointByCard(1);
                }
            }
        }
    }
}

