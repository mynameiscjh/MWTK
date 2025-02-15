using HarmonyLib;

namespace Don_Eyuil.San_Sora.Player.Buff
{
    public class BattleUnitBuf_Sway : BattleUnitBuf_Don_Eyuil
    {
        public static string Desc = "每幕结束时移除本效果并使自身获得等同于这一幕受到的”流血”伤害量的护盾\r\n自身被击中时将对目标施加1层”流血”\r\n";
        protected override string keywordId => $"BattleUnitBuf_SanFlicker";
        int count = 0;
        public override void OnRoundStart()
        {
            count = 0;
        }

        public override void AfterTakeBleedingDamage(int Dmg)
        {
            count += Dmg;
        }

        public override void OnRoundEnd()
        {
            GainBuf<BattleUnitBuf_BloodShield>(_owner, count);
            Destroy();
        }

        public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
        {
            _owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Bleeding, 1);
        }

        public BattleUnitBuf_Sway(BattleUnitModel model) : base(model)
        {
            typeof(BattleUnitBuf).GetField("_bufIcon", AccessTools.all).SetValue(this, TKS_BloodFiend_Initializer.ArtWorks["摇曳"]);
            typeof(BattleUnitBuf).GetField("_iconInit", AccessTools.all).SetValue(this, true);
        }
    }
}
