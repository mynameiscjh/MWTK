namespace Don_Eyuil.WhiteMoon_Sparkle.Player.Buff
{
    public class BattleUnitBuf_Flag : BattleUnitBuf_Don_Eyuil
    {
        public static string Desc = "收尾标记x[中立buff]:\r\n这一幕自身受到的非命中造成的伤害与混乱伤害+50x%\r\n";

        public override float DmgFactor(int dmg, DamageType type = DamageType.ETC, KeywordBuf keyword = KeywordBuf.None)
        {
            if (type != DamageType.Attack)
            {
                return 1.5f;
            }

            return base.DmgFactor(dmg, type, keyword);
        }

        public override float BreakDmgFactor(int dmg, DamageType type = DamageType.ETC, KeywordBuf keyword = KeywordBuf.None)
        {
            if (type != DamageType.Attack)
            {
                return 1.5f;
            }

            return base.DmgFactor(dmg, type, keyword);
        }

        public override void OnRoundEnd()
        {
            this.Destroy();
        }

        public BattleUnitBuf_Flag(BattleUnitModel model) : base(model)
        {
            this.SetFieldValue("_bufIcon", TKS_BloodFiend_Initializer.ArtWorks["收尾标记"]);
            this.SetFieldValue("_iconInit", true);
        }
    }
}
