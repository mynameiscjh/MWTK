namespace Don_Eyuil.Buff
{
    public class BattleUnitBuf_Sword : BattleUnitBuf
    {
        public static string Desc = "自身施加\"流血\"时将对目标与自身额外施加1层\r\n每幕结束时自身每承受2点\"流血\"伤害便时自身获得1层\"硬血结晶\"\r\n若自身至少拥有15层\"硬血结晶\"则使自身斩击骰子最小值+2\r\n";
        public override int OnGiveKeywordBufByCard(BattleUnitBuf cardBuf, int stack, BattleUnitModel target)
        {
            if (cardBuf.bufType == KeywordBuf.Bleeding)
            {
                _owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Bleeding, 1);
                return 1;
            }

            return base.OnGiveKeywordBufByCard(cardBuf, stack, target);
        }

        public override void OnRoundEnd()
        {
            bleedDmg = 0;
            BattleUnitBuf_BleedCrystal.GainBuf(_owner, bleedDmg / 2);
        }

        public override void OnRollDice(BattleDiceBehavior behavior)
        {
            behavior.ApplyDiceStatBonus(new DiceStatBonus()
            {
                min = BattleUnitBuf_BleedCrystal.GetBuf(_owner).stack >= 15 ? 2 : 0
            });
        }

        public int bleedDmg = 0;

        public override float DmgFactor(int dmg, DamageType type = DamageType.ETC, KeywordBuf keyword = KeywordBuf.None)
        {
            if (keyword == KeywordBuf.Bleeding)
            {
                bleedDmg += dmg;
            }

            return base.DmgFactor(dmg, type, keyword);
        }
    }
}
