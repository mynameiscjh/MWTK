﻿using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Don_Eyuil.WhiteMoon_Sparkle.Player.Buff
{
    public class BattleUnitBuf_Sparkle : BattleUnitBuf_Don_Eyuil
    {
        public static string Desc = "管理百般武艺";

        public List<BattleUnitBuf_Don_Eyuil> PrimaryWeapons = new List<BattleUnitBuf_Don_Eyuil>();
        public List<BattleUnitBuf_Don_Eyuil> SubWeapons = new List<BattleUnitBuf_Don_Eyuil>();
        static BattleUnitBuf_Sparkle _instance = null;
        public static BattleUnitBuf_Sparkle Instance
        {
            get
            {
                if (_instance == null)
                {
                    foreach (var item in BattleObjectManager.instance.GetAliveList())
                    {
                        if (GetBuf<BattleUnitBuf_Sparkle>(item) != null)
                        {
                            _instance = GetBuf<BattleUnitBuf_Sparkle>(item);
                            break;
                        }
                    }
                    if (_instance == null)
                    {
                        var model = BattleObjectManager.instance.GetAliveList().Find(x => x.Book.BookId == MyId.Book_小耀之页);
                        if (model != null)
                        {
                            _instance = GainBuf<BattleUnitBuf_Sparkle>(model, 1);
                        }
                    }
                    return _instance;
                }
                return _instance;
            }
        }

        public void SelectPrimaryWeapon()
        {
            var emoCards = new List<EmotionEgoXmlInfo>()
            {
                new EmotionEgoXmlInfo_Mod(MyId.Card_Desc_泉之龙_秋之莲),
                new EmotionEgoXmlInfo_Mod(MyId.Card_Desc_千斤弓),
                new EmotionEgoXmlInfo_Mod(MyId.Card_Desc_月之剑),
            };

            BattleManagerUI.Instance.ui_levelup.SetRootCanvas(true);
            BattleManagerUI.Instance.ui_levelup.InitEgo(Math.Min(3, emoCards.Count), emoCards);
            BattleManagerUI.Instance.ui_levelup.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(1).GetChild(1).GetChild(0).GetComponent<Image>().sprite = TKS_BloodFiend_Initializer.ArtWorks["玩家硬血术统一图标"];
            BattleManagerUI.Instance.ui_levelup.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "选择主武器书页";
            BattleManagerUI.Instance.ui_levelup._emotionLevels.Do(x => x.Set(false, false, false));
        }

        public void SelectSubWeapon()
        {
            var emoCards = new List<EmotionEgoXmlInfo>()
            {
                new EmotionEgoXmlInfo_Mod(MyId.Card_Desc_泉之龙_秋之莲),
                new EmotionEgoXmlInfo_Mod(MyId.Card_Desc_千斤弓),
                new EmotionEgoXmlInfo_Mod(MyId.Card_Desc_月之剑),
            };

            BattleManagerUI.Instance.ui_levelup.SetRootCanvas(true);
            BattleManagerUI.Instance.ui_levelup.InitEgo(Math.Min(3, emoCards.Count), emoCards);
            BattleManagerUI.Instance.ui_levelup.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(1).GetChild(1).GetChild(0).GetComponent<Image>().sprite = TKS_BloodFiend_Initializer.ArtWorks["玩家硬血术统一图标"];
            BattleManagerUI.Instance.ui_levelup.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "选择副武器书页";
            BattleManagerUI.Instance.ui_levelup._emotionLevels.Do(x => x.Set(false, false, false));
        }

        public void AddSubWeapon<T>() where T : BattleUnitBuf_Don_Eyuil
        {
            var buf = GetBuf<T>(owner);
            if (buf != null && SubWeapons.Contains(buf))
            {
                buf.SetFieldValue<bool>("IsIntensify", true);
            }
            if (buf != null && !SubWeapons.Contains(buf))
            {
                SubWeapons.Add(buf);
                return;
            }
            if (buf == null)
            {
                SubWeapons.Add(GainBuf<T>(_owner, 1));
            }
        }

        public void AddPrimaryWeapon<T>() where T : BattleUnitBuf_Don_Eyuil
        {
            var buf = GetBuf<T>(owner);
            if (buf != null && PrimaryWeapons.Contains(buf))
            {
                buf.SetFieldValue<bool>("IsIntensify", true);
            }
            if (buf != null && !PrimaryWeapons.Contains(buf))
            {
                PrimaryWeapons.Add(buf);
                return;
            }
            if (buf == null)
            {
                PrimaryWeapons.Add(GainBuf<T>(_owner, 1));
            }
        }

        public IEnumerator SelectWeapons()
        {
            yield return new WaitUntil(() => BattleManagerUI.Instance.ui_levelup.IsEnabled == false);

            SelectPrimaryWeapon();
            yield return new WaitUntil(() => BattleManagerUI.Instance.ui_levelup.IsEnabled == false);

            SelectSubWeapon();
            yield return new WaitUntil(() => BattleManagerUI.Instance.ui_levelup.IsEnabled == false);
        }

        public BattleUnitBuf_Sparkle(BattleUnitModel model) : base(model)
        {
            PrimaryWeapons = new List<BattleUnitBuf_Don_Eyuil>();
            SubWeapons = new List<BattleUnitBuf_Don_Eyuil>();
            _instance = this;
        }

        public override void Destroy()
        {
            base.Destroy();
            PrimaryWeapons = new List<BattleUnitBuf_Don_Eyuil>();
            SubWeapons = new List<BattleUnitBuf_Don_Eyuil>();
        }

    }

}
