using HarmonyLib;
using System.Collections.Generic;
using System.Linq;

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
                string temp = Singleton<BattleEffectTextsXmlList>.Instance.GetEffectTextDesc(keywordId, paramInBufDesc);

                var tempList = new List<BattleUnitBuf_SanHardBlood> { Sword, Lance, Blade, Bow, DoubleSwords, Scourge, Sickle, Armour }.Where(x => x != null).Sort((x, y) => x.Index - y.Index).ToList();

                foreach (var item in tempList)
                {
                    temp += "\r\n" + item.bufActivatedText;
                }

                return temp;
            }
        }

        protected override string keywordId => "BattleUnitBuf_SanSora_HardBloodArt";

        public BattleUnitBuf_SanSora(BattleUnitModel model) : base(model)
        {
            typeof(BattleUnitBuf).GetField("_bufIcon", AccessTools.all).SetValue(this, TKS_BloodFiend_Initializer.ArtWorks["桑空硬血"]);
            typeof(BattleUnitBuf).GetField("_iconInit", AccessTools.all).SetValue(this, true);
        }

        public override int SpeedDiceNumAdder()
        {
            if (BattleObjectManager.instance.GetAliveList(Faction.Player).Exists(x => x.Book.BookId == MyId.Book_堂_埃尤尔之页)) return 1;
            return base.SpeedDiceNumAdder();
        }

        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            if (BattleObjectManager.instance.GetAliveList(Faction.Player).Exists(x => x.Book.BookId == MyId.Book_堂_埃尤尔之页))
            {
                behavior.ApplyDiceStatBonus(new DiceStatBonus() { min = 1 });
            }
        }

    }

}
