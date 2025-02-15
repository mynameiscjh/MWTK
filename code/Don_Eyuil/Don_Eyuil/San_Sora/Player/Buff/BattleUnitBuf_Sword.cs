using LOR_BattleUnit_UI;

namespace Don_Eyuil.San_Sora.Player.Buff
{
    public class BattleUnitBuf_Sword : BattleUnitBuf_SanHardBlood
    {
        public static string Desc = "桑空派变体硬血术1式-血剑\r\n本速度骰子中投掷进攻型骰子时不再受到流血伤害转而获得等量”结晶硬血”(单次至多3层)\r\n自身”结晶硬血”层数不低于10时本速度骰子中使用书页时消耗3层并使进攻型骰子威力+2\r\n";
        protected override string keywordId => $"BattleUnitBuf_SanSora_HardBloodArt_BloodSword";
        public BattleUnitBuf_Sword(SpeedDiceUI dice) : base(dice)
        {
            //dice.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = TKS_BloodFiend_Initializer.ArtWorks["血剑骰子"];
        }

        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            if (behavior.card == Card && BattleUnitBuf_Don_Eyuil.GetBufStack<BattleUnitBuf_Crystal_HardBlood>(owner) >= 10 && behavior.Type == LOR_DiceSystem.BehaviourType.Atk)
            {
                BattleUnitBuf_Don_Eyuil.UseBuf<BattleUnitBuf_Crystal_HardBlood>(owner, 3);
                behavior.ApplyDiceStatBonus(new DiceStatBonus() { power = 2 });
            }
        }

        public override float DmgFactor(int dmg, DamageType type = DamageType.ETC, KeywordBuf keyword = KeywordBuf.None)
        {
            if (_owner.currentDiceAction == Card && keyword == KeywordBuf.Bleeding)
            {
                BattleUnitBuf_Don_Eyuil.GainBuf<BattleUnitBuf_Crystal_HardBlood>(_owner, dmg);
                return 0;
            }

            return base.DmgFactor(dmg, type, keyword);
        }
    }
}
