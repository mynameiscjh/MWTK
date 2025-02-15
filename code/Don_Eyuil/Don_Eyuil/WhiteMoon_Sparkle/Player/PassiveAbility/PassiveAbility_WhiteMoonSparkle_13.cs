using Don_Eyuil.WhiteMoon_Sparkle.Player.Buff;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Don_Eyuil.WhiteMoon_Sparkle.Player.PassiveAbility
{
    public class PassiveAbility_WhiteMoonSparkle_13 : PassiveAbilityBase_Don_Eyuil
    {
        public static string Desc = "自身拥有三种武器\r\n开启舞台时可各选择一种主武器和副武器(可重复)若选择了同种武器则获得附加效果(每幕可在战斗准备阶段更改1次)\r\n自身情感等级达到2/4级时可额外选择1种武器强化/额外应用一种副武器\r\n场上每有3名敌方角色死亡则可额外选择一次上述效果 若友方角色死亡同样同样可以选择一次上述效果\r\n";

        public override void OnWaveStart()
        {
            if (BattleUnitBuf_Sparkle.Instance != null && StageController.Instance.CurrentWave != 1)
            {
                owner.bufListDetail.AddBuf(BattleUnitBuf_Sparkle.Instance);
                foreach (var item in BattleUnitBuf_Sparkle.Instance.PrimaryWeapons)
                {
                    if (!owner.bufListDetail.GetActivatedBufList().Contains(item))
                    {
                        owner.bufListDetail.AddBuf(item);
                    }

                }
                foreach (var item in BattleUnitBuf_Sparkle.Instance.SubWeapons)
                {
                    if (!owner.bufListDetail.GetActivatedBufList().Contains(item))
                    {
                        owner.bufListDetail.AddBuf(item);
                    }
                }
            }
            else
            {
                BattleUnitBuf_Don_Eyuil.GainBuf<BattleUnitBuf_Sparkle>(owner, 1);
                BattleManagerUI.Instance.ui_levelup.StartCoroutine(BattleUnitBuf_Sparkle.Instance.SelectWeapons());
            }
            if (BattleObjectManager.instance.GetAliveList().Exists(x => x.Book.BookId == MyId.Book_堂_埃尤尔之页))
            {
                BattleUnitBuf_Don_Eyuil.GainBuf<BattleUnitBuf_Inherit>(owner, 1);
            }
        }

        bool fl2 = false, fl4 = false;

        public override void OnRoundStart()
        {
            if ((owner.emotionDetail.EmotionLevel == 2 && !fl2) || (owner.emotionDetail.EmotionLevel == 4 && !fl4))
            {
                if (owner.emotionDetail.EmotionLevel == 2)
                {
                    fl2 = true;
                }
                if (owner.emotionDetail.EmotionLevel == 4)
                {
                    fl4 = true;
                }
                BattleManagerUI.Instance.ui_levelup.StartCoroutine(SelectWeapon(false));
            }
        }
        int count = 0;
        public override void OnDieOtherUnit(BattleUnitModel unit)
        {
            if (unit.faction == Faction.Enemy)
            {
                count++;
            }
            if (unit.faction == Faction.Player)
            {
                BattleManagerUI.Instance.ui_levelup.StartCoroutine(SelectWeapon(false));
            }
            if (count % 3 == 0 && count > 0)
            {
                BattleManagerUI.Instance.ui_levelup.StartCoroutine(SelectWeapon(false));
            }
        }

        public IEnumerator SelectWeapon(bool isPrimary)
        {
            yield return new WaitUntil(() => BattleManagerUI.Instance.ui_levelup.IsEnabled == false);

            var emoCards = new List<EmotionEgoXmlInfo>()
            {
                new EmotionEgoXmlInfo_Mod(MyId.Card_Desc_泉之龙_秋之莲),
                new EmotionEgoXmlInfo_Mod(MyId.Card_Desc_千斤弓),
                new EmotionEgoXmlInfo_Mod(MyId.Card_Desc_月之剑),
            };
            BattleManagerUI.Instance.ui_levelup.SetRootCanvas(true);
            BattleManagerUI.Instance.ui_levelup.InitEgo(Math.Min(3, emoCards.Count), emoCards);
            BattleManagerUI.Instance.ui_levelup.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(1).GetChild(1).GetChild(0).GetComponent<Image>().sprite = TKS_BloodFiend_Initializer.ArtWorks["玩家硬血术统一图标"];
            if (isPrimary)
            {
                BattleManagerUI.Instance.ui_levelup.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "选择主武器书页";
            }
            else
            {
                BattleManagerUI.Instance.ui_levelup.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "选择副武器书页";
            }
            BattleManagerUI.Instance.ui_levelup._emotionLevels.Do(x => x.Set(false, false, false));
            yield return new WaitUntil(() => BattleManagerUI.Instance.ui_levelup.IsEnabled == false);
        }

    }
}
