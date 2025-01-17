using System.Collections.Generic;

namespace Don_Eyuil.PassiveAbility
{

    public class PassiveAbility_DonEyuil_15 : PassiveAbilityBase
    {
        public override string debugDesc => "堂埃尤尔派硬血术 0费 特殊\r\n自身拥有一套额外的卡组可设置\"硬血术\"书页\r\n情感等级达到0/2/4时可以选择激活设置的\"硬血术\"书页情感等级达到4级后每有一名角色因流血死亡则可额外激活一次设置的\"硬血术\"书页\r\n（这里的选择激活硬血术界面用选EGO的那个levelup的UI做)\r\n（实现上面，基本就是正常的多写一个卡组就可以了)\r\n\r\n自身可使用个人书页\"堂埃尤尔派硬血术终式\"且无法使用楼层E.G.O书页\r\n";

        public List<LorId> Cards = new List<LorId>()
        {
            MyId.Card_堂埃尤尔派硬血术1式_血剑_2,
            MyId.Card_堂埃尤尔派硬血术2式_血枪_2,
            MyId.Card_堂埃尤尔派硬血术3式_血镰_2,
            MyId.Card_堂埃尤尔派硬血术4式_血刃_2,
            MyId.Card_堂埃尤尔派硬血术5式_双剑_2,
            MyId.Card_堂埃尤尔派硬血术6式_血甲_2,
            MyId.Card_堂埃尤尔派硬血术7式_血弓_2,
            MyId.Card_堂埃尤尔派硬血术8式_血鞭_2,
            MyId.Card_堂埃尤尔派硬血术9式_血伞_2,
            MyId.Card_堂埃尤尔派硬血术终式_La_Sangre_2,
        };

        public override void OnRoundStart()
        {
            if (owner.emotionDetail.EmotionLevel == 0 || owner.emotionDetail.EmotionLevel == 2 || owner.emotionDetail.EmotionLevel == 4 || (fl && owner.emotionDetail.EmotionLevel > 4))
            {
                ShowCards();
                fl = false;
            }
        }

        public bool fl = false;

        public override void OnDieOtherUnit(BattleUnitModel unit)
        {
            if (unit.bufListDetail.HasBuf<BattleUnitBuf_BleedDmg>() && (unit.bufListDetail.GetFieldValue<List<BattleUnitBuf>>("_bufList").Find(x => x is BattleUnitBuf_BleedDmg) as BattleUnitBuf_BleedDmg).fl)
            {
                fl = true;
            }
        }

        public class BattleUnitBuf_BleedDmg : BattleUnitBuf
        {
            public bool fl = false;
            public override float DmgFactor(int dmg, DamageType type = DamageType.ETC, KeywordBuf keyword = KeywordBuf.None)
            {
                if (fl)
                {
                    return base.DmgFactor(dmg, type, keyword);
                }
                if (keyword == KeywordBuf.Bleeding && this._owner.hp <= (float)dmg)
                {
                    fl = true;
                }
                return base.DmgFactor(dmg, type, keyword);
            }
        }

        public static int count = 0;

        public void ShowCards()
        {
            if (Cards != null && Cards.Count > 0)
            {
                count++;
                LorId temp1 = RandomUtil.SelectOne(Cards);
                Cards.Remove(temp1);
                LorId temp2 = RandomUtil.SelectOne(Cards);
                Cards.Remove(temp2);
                LorId temp3 = RandomUtil.SelectOne(Cards);
                Cards.Remove(temp3);
                BattleManagerUI.Instance.ui_levelup.SetRootCanvas(true);
                BattleManagerUI.Instance.ui_levelup.InitEgo(3, new List<EmotionEgoXmlInfo>() { new EmotionEgoXmlInfo_Mod(temp1), new EmotionEgoXmlInfo_Mod(temp2), new EmotionEgoXmlInfo_Mod(temp3) });
                //怀疑效果不是这么实现的 可能会出bug
            }
        }

        //应该需要hp把特定buff加上
    }
}
