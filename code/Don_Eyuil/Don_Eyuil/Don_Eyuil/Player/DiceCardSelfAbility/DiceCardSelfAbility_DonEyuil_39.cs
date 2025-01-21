namespace Don_Eyuil.Don_Eyuil.Player.DiceCardSelfAbility
{
    //public class DiceCardSelfAbility_DonEyuil_39 : DiceCardSelfAbilityBase
    //{
    //    public static string Desc = "本书页命中时将恢复等同于伤害量25%的体力若这一幕中自身已经累积承受10点流血伤害则改为50%\r\n本书页恢复溢出的体力将转化为等量护盾\r\n";
    //    public override void OnUseCard()
    //    {
    //        owner.bufListDetail.AddBuf(new BattleUnitBuf_BleedCount(owner));
    //    }
    //    public override void AfterGiveDamage(int damage, BattleUnitModel target)
    //    {
    //        float temp = 0.25f;
    //        if (BattleUnitBuf_Don_Eyuil.GetBuf<BattleUnitBuf_BleedCount>(owner).count >= 10)
    //        {
    //            temp = 0.5f;
    //        }
    //        int temp2 = (int)(damage * temp);
    //        if (owner.hp + temp2 > owner.MaxHp)
    //        {
    //            owner.RecoverHP(owner.MaxHp - (int)owner.hp);
    //            BattleUnitBuf_BloodShield.GainBuf<BattleUnitBuf_BloodShield>(owner, temp2 + (int)owner.hp - owner.MaxHp );
    //        }
    //        else
    //        {
    //            owner.RecoverHP(temp2);
    //        }
    //    }
    //    public class BattleUnitBuf_BleedCount : BattleUnitBuf_Don_Eyuil
    //    {
    //        public int count = 0;
    //        public BattleUnitBuf_BleedCount(BattleUnitModel model) : base(model)
    //        {
    //        }
    //        public override void AfterTakeBleedingDamage(int Dmg)
    //        {
    //            count += Dmg;
    //        }
    //        public override void OnRoundEnd()
    //        {
    //            this.Destroy();
    //        }
    //    }
    //}
}
