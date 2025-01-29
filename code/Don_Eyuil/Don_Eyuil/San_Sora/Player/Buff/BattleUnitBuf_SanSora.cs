using HarmonyLib;

namespace Don_Eyuil.San_Sora.Player.Buff
{
    public class BattleUnitBuf_SanSora : BattleUnitBuf_Don_Eyuil
    {
        public static string Desc = "把八般武艺包装在一起";

        public static BattleUnitBuf_SanSora Instance
        {
            get
            {
                foreach (var item in BattleObjectManager.instance.GetAliveList(Faction.Player))
                {
                    if (item.bufListDetail.HasBuf<BattleUnitBuf_SanSora>())
                    {
                        return GetBuf<BattleUnitBuf_SanSora>(item);
                    }
                }
                return null;
            }
        }

        public BattleUnitBuf_Sword Sword => GetBuf<BattleUnitBuf_Sword>(_owner);
        public BattleUnitBuf_Lance Lance => GetBuf<BattleUnitBuf_Lance>(_owner);
        public BattleUnitBuf_Blade Blade => GetBuf<BattleUnitBuf_Blade>(_owner);
        public BattleUnitBuf_Bow Bow => GetBuf<BattleUnitBuf_Bow>(_owner);
        public BattleUnitBuf_DoubleSwords DoubleSwords => GetBuf<BattleUnitBuf_DoubleSwords>(_owner);
        public BattleUnitBuf_Scourge Scourge => GetBuf<BattleUnitBuf_Scourge>(_owner);
        public BattleUnitBuf_Sickle Sickle => GetBuf<BattleUnitBuf_Sickle>(_owner);
        public BattleUnitBuf_Armour Armour => GetBuf<BattleUnitBuf_Armour>(_owner);

        public override string bufActivatedText
        {
            get
            {
                string temp = "如下\n";
                temp += Sword != null ? $"{nameof(BattleUnitBuf_Sword)}: 附加于第{Sword.Index}颗骰子\n" : "";
                temp += Lance != null ? $"{nameof(BattleUnitBuf_Lance)}: 附加于第{Lance.Index}颗骰子\n" : "";
                temp += Blade != null ? $"{nameof(BattleUnitBuf_Blade)} : 附加于第{Blade.Index}颗骰子\n" : "";
                temp += Bow != null ? $"{nameof(BattleUnitBuf_Bow)} : 附加于第{Bow.Index}颗骰子\n" : "";
                temp += DoubleSwords != null ? $"{nameof(BattleUnitBuf_DoubleSwords)} : 附加于第{DoubleSwords.Index}颗骰子\n" : "";
                temp += Scourge != null ? $"{nameof(BattleUnitBuf_Scourge)} : 附加于第{Scourge.Index}颗骰子\n" : "";
                temp += Sickle != null ? $"{nameof(BattleUnitBuf_Sickle)} : 附加于第{Sickle.Index}颗骰子\n" : "";
                temp += Armour != null ? $"{nameof(BattleUnitBuf_Armour)}  : 附加于第{Armour.Index}颗骰子\n" : "";
                return temp;
            }
        }

        protected override string keywordId => "BattleUnitBuf_SanSora";

        public BattleUnitBuf_SanSora(BattleUnitModel model) : base(model)
        {
            typeof(BattleUnitBuf).GetField("_bufIcon", AccessTools.all).SetValue(this, TKS_BloodFiend_Initializer.ArtWorks["桑空硬血"]);
            typeof(BattleUnitBuf).GetField("_iconInit", AccessTools.all).SetValue(this, true);
        }
    }

}
