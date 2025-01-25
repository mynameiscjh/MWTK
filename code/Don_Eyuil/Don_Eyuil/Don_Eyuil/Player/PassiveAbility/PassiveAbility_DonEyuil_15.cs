using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Don_Eyuil.Don_Eyuil.Player.PassiveAbility
{

    public class PassiveAbility_DonEyuil_15 : PassiveAbilityBase
    {
        public override string debugDesc => "堂埃尤尔派硬血术 0费 特殊\r\n自身拥有一套额外的卡组可设置\"硬血术\"书页\r\n情感等级达到0/2/4时可以选择激活设置的\"硬血术\"书页情感等级达到4级后每有一名角色因流血死亡则可额外激活一次设置的\"硬血术\"书页\r\n（这里的选择激活硬血术界面用选EGO的那个levelup的UI做)\r\n（实现上面，基本就是正常的多写一个卡组就可以了)\r\n\r\n自身可使用个人书页\"堂埃尤尔派硬血术终式\"且无法使用楼层E.G.O书页\r\n";



        public override void OnWaveStart()
        {
            HardBloodCards.cards_Don_Eyuil = new List<LorId>(this.owner.UnitData.unitData.GetDeckForBattle(1).Where(item => HardBloodCards.map_Don_Eyuil.Values.Contains(item.id)).Select(item => item.id));
            owner.personalEgoDetail.AddCard(MyId.Card_堂埃尤尔派硬血术终式_La_Sangre_2);
            fl0 = false;
            fl2 = false;
            fl4 = false;
        }

        bool fl0 = false;
        bool fl2 = false;
        bool fl4 = false;

        public override void OnRoundStart()
        {
            if ((owner.emotionDetail.EmotionLevel == 0 && !fl0) || (owner.emotionDetail.EmotionLevel == 2 && !fl2) || (owner.emotionDetail.EmotionLevel == 4 && !fl4) || (fl && owner.emotionDetail.EmotionLevel > 4))
            {
                ShowCards();
                fl = false;
                if (owner.emotionDetail.EmotionLevel == 0)
                {
                    fl0 = true;
                }
                if (owner.emotionDetail.EmotionLevel == 2)
                {
                    fl2 = true;
                }
                if (owner.emotionDetail.EmotionLevel == 4)
                {
                    fl4 = true;
                }
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



        public void ShowCards()
        {
            if (HardBloodCards.cards_Don_Eyuil == null || HardBloodCards.cards_Don_Eyuil.Count <= 0)
            {
                return;
            }
            var Cards = new List<LorId>(HardBloodCards.cards_Don_Eyuil);
            var temp = new List<LorId>();
            for (int i = 0; i < 3; i++)
            {
                if (Cards.Count <= 0)
                {
                    break;
                }
                LorId temp1 = RandomUtil.SelectOne(Cards);
                Cards.Remove(temp1);
                temp.Add(temp1);
            }
            var emoCards = new List<EmotionEgoXmlInfo>(temp.Count);
            foreach (var item in temp)
            {
                emoCards.Add(new EmotionEgoXmlInfo_Mod(item));
            }


            BattleManagerUI.Instance.ui_levelup.SetRootCanvas(true);
            BattleManagerUI.Instance.ui_levelup.InitEgo(Math.Min(3, emoCards.Count), emoCards);
            BattleManagerUI.Instance.ui_levelup.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(1).GetChild(1).GetChild(0).GetComponent<Image>().sprite = TKS_BloodFiend_Initializer.ArtWorks["玩家硬血术统一图标"];
            BattleManagerUI.Instance.ui_levelup.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "选择硬血流书页";
            BattleManagerUI.Instance.ui_levelup._emotionLevels.Do(x => x.Set(false, false, false));

        }

        [HarmonyPatch(typeof(UILibrarianEquipInfoSlot), "SetData")]
        [HarmonyPostfix]
        public static void UILibrarianEquipInfoSlot_SetData_Post(BookPassiveInfo passive, Image ___Frame, TextMeshProUGUI ___txt_cost)
        {
            if (passive == null || passive.passive.id != MyTools.Create(15))
            {
                return;
            }
            ___Frame.color = Color.red;
            ___txt_cost.text = "";
        }
    }


}