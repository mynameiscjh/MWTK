using HarmonyLib;

namespace Don_Eyuil.San_Sora.Player.Buff
{
    public class BattleUnitBuf_Crystal_HardBlood : BattleUnitBuf_Don_Eyuil
    {
        protected override string keywordId => "BattleUnitBuf_Crystal_HardBlood";
        //至多30层
        //可配合硬血术效果
        public override int GetMaxStack() => 30;
        public BattleUnitBuf_Crystal_HardBlood(BattleUnitModel model) : base(model)
        {
            typeof(BattleUnitBuf).GetField("_bufIcon", AccessTools.all).SetValue(this, TKS_BloodFiend_Initializer.ArtWorks["结晶硬血"]);
            typeof(BattleUnitBuf).GetField("_iconInit", AccessTools.all).SetValue(this, true);
            this.stack = 0;
        }
    }
}
