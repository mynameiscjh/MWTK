using Don_Eyuil.Buff;
using Don_Eyuil.Don_Eyuil.Buff;
using HarmonyLib;

namespace Don_Eyuil.Don_Eyuil.Player.Buff
{
    public class BattleUnitBuf_HardBlood : BattleUnitBuf_Don_Eyuil
    {
        public static string Desc = "把百般武艺包装在一起";

        protected override string keywordId => "BattleUnitBuf_HardBlood";
        public override int paramInBufDesc => ActivatedNum;

        public BattleUnitBuf_HardBlood(BattleUnitModel model) : base(model)
        {
            typeof(BattleUnitBuf).GetField("_bufIcon", AccessTools.all).SetValue(this, TKS_BloodFiend_Initializer.ArtWorks["玩家硬血术统一图标"]);
            typeof(BattleUnitBuf).GetField("_iconInit", AccessTools.all).SetValue(this, true);
        }

        public BattleUnitBuf_Sword Sword => GetBuf<BattleUnitBuf_Sword>(_owner);
        public BattleUnitBuf_Lance Lance => GetBuf<BattleUnitBuf_Lance>(_owner);
        public BattleUnitBuf_Sickle Sickle => GetBuf<BattleUnitBuf_Sickle>(_owner);
        public BattleUnitBuf_Blade Blade => GetBuf<BattleUnitBuf_Blade>(_owner);
        public BattleUnitBuf_DoubleSwords DoubleSwords => GetBuf<BattleUnitBuf_DoubleSwords>(_owner);
        public BattleUnitBuf_Armour Armour => GetBuf<BattleUnitBuf_Armour>(_owner);
        public BattleUnitBuf_Bow Bow => GetBuf<BattleUnitBuf_Bow>(_owner);
        public BattleUnitBuf_Scourge Scourge => GetBuf<BattleUnitBuf_Scourge>(_owner);
        public BattleUnitBuf_Umbrella Umbrella => GetBuf<BattleUnitBuf_Umbrella>(_owner);
        public bool IsAllActivate => _owner.bufListDetail.HasBuf<BattleUnitBuf_Sword>() && _owner.bufListDetail.HasBuf<BattleUnitBuf_Lance>() && _owner.bufListDetail.HasBuf<BattleUnitBuf_Sickle>() && _owner.bufListDetail.HasBuf<BattleUnitBuf_Blade>() && _owner.bufListDetail.HasBuf<BattleUnitBuf_DoubleSwords>() && _owner.bufListDetail.HasBuf<BattleUnitBuf_Armour>() && _owner.bufListDetail.HasBuf<BattleUnitBuf_Bow>() && _owner.bufListDetail.HasBuf<BattleUnitBuf_Scourge>() && _owner.bufListDetail.HasBuf<BattleUnitBuf_Umbrella>();

        public int ActivatedNum
        {
            get
            {
                int temp = 0;
                temp += Sword != null ? 1 : 0;
                temp += Lance != null ? 1 : 0;
                temp += Sickle != null ? 1 : 0;
                temp += Blade != null ? 1 : 0;
                temp += DoubleSwords != null ? 1 : 0;
                temp += Armour != null ? 1 : 0;
                temp += Bow != null ? 1 : 0;
                temp += Scourge != null ? 1 : 0;
                temp += Umbrella != null ? 1 : 0;
                return temp;
            }
        }

        public override void OnRoundEnd()
        {
            _owner.bufListDetail.GetBufUIDataList().bufActivatedList.Find(x => x.bufClassType == typeof(BattleUnitBuf_HardBlood)).bufActivatedText = bufActivatedText;
        }

        public override string bufActivatedText
        {
            get
            {
                var temp = base.bufActivatedText;

                temp += Sword != null ? "\r\n" + Sword.bufActivatedText : "";
                temp += Lance != null ? "\r\n" + Lance.bufActivatedText : "";
                temp += Sickle != null ? "\r\n" + Sickle.bufActivatedText : "";
                temp += Blade != null ? "\r\n" + Blade.bufActivatedText : "";
                temp += DoubleSwords != null ? "\r\n" + DoubleSwords.bufActivatedText : "";
                temp += Armour != null ? "\r\n" + Armour.bufActivatedText : "";
                temp += Bow != null ? "\r\n" + Bow.bufActivatedText : "";
                temp += Scourge != null ? "\r\n" + Scourge.bufActivatedText : "";
                temp += Umbrella != null ? "\r\n" + Umbrella.bufActivatedText : "";

                return temp;
            }
        }
    }
}
