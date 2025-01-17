namespace Don_Eyuil.Don_Eyuil.Buff
{
    public class BattleUnitBuf_DeepWound : BattleUnitBuf
    {
        public static string Desc = "这一幕受到的\"流血\"伤害增加50%";
        public override float DmgFactor(int dmg, DamageType type = DamageType.ETC, KeywordBuf keyword = KeywordBuf.None)
        {
            if (keyword == KeywordBuf.Bleeding)
            {
                return 1.5f;
            }
            return base.DmgFactor(dmg, type, keyword);
        }
    }
}
