using Don_Eyuil.Buff;
using Don_Eyuil.Don_Eyuil.Buff;
using Don_Eyuil.Don_Eyuil.Player.Buff;
using HarmonyLib;
using LOR_DiceSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

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

        [HarmonyPatch(typeof(UIEquipDeckCardList), "SetDeckLayout")]
        [HarmonyPostfix]
        public static void UIEquipDeckCardList_SetDeckLayout_Post(UIEquipDeckCardList __instance)
        {
            if (__instance.currentunit.bookItem.BookId == MyTools.Create(10000001))
            {
                var deckTabsController = __instance.GetFieldValue<UICustomTabsController>("deckTabsController");
                deckTabsController.transform.GetChild(1).GetComponent<HorizontalLayoutGroup>().enabled = false;
                deckTabsController.CustomTabs[0].TabName.text = "并非硬血术卡组";
                deckTabsController.CustomTabs[0].transform.localPosition = new Vector3(118.8028f, 20.65f, 0);
                deckTabsController.CustomTabs[1].TabName.text = "硬血术卡组";
                deckTabsController.CustomTabs[1].transform.localPosition = new Vector3(312.7585f, 20.65f, 0);
                deckTabsController.CustomTabs[2].gameObject.SetActive(false);
                deckTabsController.CustomTabs[3].gameObject.SetActive(false);
            }
            else
            {
                var deckTabsController = __instance.GetFieldValue<UICustomTabsController>("deckTabsController");
                deckTabsController.transform.GetChild(1).GetComponent<HorizontalLayoutGroup>().enabled = true;
            }
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

        public enum DeckId
        {
            Normal, HardBlood
        }
        public static DeckId Current_DeckId = DeckId.Normal;
        public static List<LorId> HardBloodCards = new List<LorId>()
        {   MyId.Card_堂埃尤尔派硬血术1式_血剑_2,
            MyId.Card_堂埃尤尔派硬血术2式_血枪_2,
            MyId.Card_堂埃尤尔派硬血术3式_血镰_2,
            MyId.Card_堂埃尤尔派硬血术4式_血刃_2,
            MyId.Card_堂埃尤尔派硬血术5式_双剑_2,
            MyId.Card_堂埃尤尔派硬血术6式_血甲_2,
            MyId.Card_堂埃尤尔派硬血术7式_血弓_2,
            MyId.Card_堂埃尤尔派硬血术8式_血鞭_2,
            MyId.Card_堂埃尤尔派硬血术9式_血伞_2,
        };

        [HarmonyPatch(typeof(BookModel), "GetOnlyCards")]
        [HarmonyPostfix]
        public static void BookModel_GetOnlyCards_Post(ref List<DiceCardXmlInfo> __result, BookModel __instance)
        {
            if (__instance.BookId != MyId.Book_堂_埃尤尔之页)
            {
                return;
            }
            __result.Clear();
            foreach (var item in HardBloodCards)
            {
                var card = ItemXmlDataList.instance.GetCardItem(item);
                __result.Add(card);
            }
        }

        [HarmonyPatch(typeof(UIInvenCardListScroll), "ApplyFilterAll")]
        [HarmonyPostfix]
        public static void UIInvenCardListScroll_ApplyFilterAll_Post(List<DiceCardItemModel> ____currentCardListForFilter, UIInvenCardListScroll __instance)
        {
            if (__instance.GetFieldValue<UnitDataModel>("_unitdata") == null)
            {
                return;
            }
            if (__instance.GetFieldValue<UnitDataModel>("_unitdata").bookItem.BookId != MyId.Book_堂_埃尤尔之页)
            {
                return;
            }
            if (Current_DeckId == DeckId.HardBlood)
            {
                ____currentCardListForFilter.Clear();
                foreach (var item in HardBloodCards)
                {
                    var card = ItemXmlDataList.instance.GetCardItem(item);
                    DiceCardItemModel itemModel = new DiceCardItemModel(card);
                    itemModel.num = 99;
                    ____currentCardListForFilter.Add(itemModel);
                }
            }
            else
            {
                ____currentCardListForFilter.RemoveAll(x => HardBloodCards.Exists(item => item == x.ClassInfo.id));
                ____currentCardListForFilter.RemoveAll(x => x.GetID() == MyId.Card_堂埃尤尔派硬血术终式_La_Sangre_2);
            }
            __instance.SetCardsData(__instance.GetCurrentPageList());
        }

        [HarmonyPatch(typeof(DeckModel), "AddCardFromInventory")]
        [HarmonyPostfix]
        public static void DeckModel_AddCardFromInventory_Post(ref CardEquipState __result, LorId cardId, List<DiceCardXmlInfo> ____deck)
        {
            if (__result == CardEquipState.Equippable)
            {
                return;
            }
            if (____deck.Count >= 9)
            {
                return;
            }
            if (HardBloodCards.Contains(cardId))
            {
                __result = CardEquipState.Equippable;
            }
        }

        [HarmonyPatch(typeof(UIEquipDeckCardList), "OnChangeDeckTab")]
        [HarmonyPostfix]
        public static void UIEquipDeckCardList_OnChangeDeckTab_Post(UIEquipDeckCardList __instance, UICustomTabsController ___deckTabsController)
        {
            if (__instance.currentunit == null)
            {
                return;
            }
            if (!__instance.currentunit.bookItem.IsMultiDeck())
            {
                return;
            }
            if (__instance.currentunit.bookItem.BookId == MyId.Book_堂_埃尤尔之页)
            {
                int currentIndex = ___deckTabsController.GetCurrentIndex();
                if (currentIndex == 1)
                {
                    Current_DeckId = DeckId.HardBlood;
                }
                else
                {
                    Current_DeckId = DeckId.Normal;
                }
                var temp = __instance.transform.parent.parent.parent.GetChild(1).GetChild(0).GetComponent<UIInvenCardListScroll>();
                temp.ApplyFilterAll();
            }
            return;
        }

        [HarmonyPatch(typeof(InventoryModel), "RemoveCard")]
        [HarmonyPrefix]
        public static bool InventoryModel_RemoveCard_Pre(LorId cardId, ref bool __result)
        {
            if (HardBloodCards.Contains(cardId))
            {
                __result = true;
                return false;
            }
            return true;
        }

        [HarmonyPatch(typeof(UIDetailCardSlot), "SetData")]
        [HarmonyPostfix]
        public static void UIDetailCardSlot_SetData_Post(DiceCardItemModel cardmodel, GameObject ___ob_selfAbility)
        {
            try
            {
                GameObject gameObject = ___ob_selfAbility.transform.parent.parent.parent.gameObject;
                bool flag = gameObject != null && gameObject.name.Contains("[Rect]RightPanel");
                if (flag)
                {
                    gameObject.GetComponentsInChildren<Image>().ToList<Image>().ForEach(delegate (Image x)
                    {
                        Debug.Log("Image.name:" + ((x != null) ? x.name : null));
                    });
                    Image image = gameObject.GetComponentsInChildren<Image>().FirstOrDefault((Image x) => x.name.Contains("[Image]BgFrame"));
                    bool flag2 = image != null;
                    if (flag2)
                    {
                        bool flag3 = cardmodel != null && cardmodel.ClassInfo.IsEgo();
                        if (flag3)
                        {
                            image.overrideSprite = TKS_BloodFiend_Initializer.ArtWorks["RightPanel"];
                        }
                        else
                        {
                            image.overrideSprite = null;
                        }
                    }
                }
            }
            catch
            {
            }
        }

        [HarmonyPatch(typeof(UIOriginCardSlot), "SetData")]
        [HarmonyPostfix]
        public static void UIOriginCardSlot_SetData_Post(UIOriginCardSlot __instance, DiceCardItemModel cardmodel, ref DiceCardItemModel ____cardModel, ref Image[] ___img_linearDodge, ref Image ___img_Artwork, ref Image[] ___img_Frames)
        {
            try
            {
                bool flag = ____cardModel != null && ____cardModel.ClassInfo.IsEgo();
                if (flag)
                {
                    for (int i = 0; i < ___img_Frames.Length; i++)
                    {
                        bool flag2 = !(___img_Frames[i] == null);
                        if (flag2)
                        {
                            ___img_Frames[i].color = Color.white;
                        }
                    }
                    Image image = ___img_Frames.FirstOrDefault((Image x) => x.name.Contains("[Image]NormalFrame"));
                    bool flag3 = image != null;
                    if (flag3)
                    {
                        image.overrideSprite = TKS_BloodFiend_Initializer.ArtWorks["LeftPanel"];
                    }
                    Image image2 = ___img_Frames.FirstOrDefault((Image x) => x.name.Contains("[Image]NormalLinearDodge"));
                    bool flag4 = image2 != null;
                    if (flag4)
                    {
                        image2.gameObject.SetActive(false);
                    }
                    for (int j = 0; j < ___img_linearDodge.Length; j++)
                    {
                        bool flag5 = !(___img_linearDodge[j] == null);
                        if (flag5)
                        {
                            ___img_linearDodge[j].gameObject.SetActive(false);
                        }
                    }
                }
                else
                {
                    Image[] array = ___img_Frames;
                    bool flag6 = array == null;
                    Image image3;
                    if (flag6)
                    {
                        image3 = null;
                    }
                    else
                    {
                        image3 = array.FirstOrDefault((Image x) => x.name.Contains("[Image]NormalFrame"));
                    }
                    Image image4 = image3;
                    bool flag7 = image4 != null;
                    if (flag7)
                    {
                        image4.overrideSprite = null;
                    }
                }
            }
            catch
            {
            }
        }

        [HarmonyPatch(typeof(UIOriginCardSlot), "SetData")]
        [HarmonyPrefix]
        public static bool UIOriginCardSlot_SetData_Pre(UIOriginCardSlot __instance, ref DiceCardItemModel ____cardModel, ref Image[] ___img_linearDodge, ref Image[] ___img_Frames)
        {
            try
            {
                Image image = ___img_Frames.FirstOrDefault((Image x) => x.name.Contains("[Image]NormalLinearDodge"));
                bool flag = image != null;
                if (flag)
                {
                    image.gameObject.SetActive(true);
                }
                for (int i = 0; i < ___img_linearDodge.Length; i++)
                {
                    bool flag2 = !(___img_linearDodge[i] == null);
                    if (flag2)
                    {
                        ___img_linearDodge[i].gameObject.SetActive(true);
                    }
                }
            }
            catch
            {
            }
            return true;
        }

#if false
        [HarmonyPatch(typeof(UIOriginCardSlot), "SetHighlightedSlot")]
        [HarmonyPostfix]
        public static void UIOriginCardSlot_SetHighlightedSlot_Post(UIOriginCardSlot __instance, bool on, ref DiceCardItemModel ____cardModel, ref Image[] ___img_Frames)
        {
            bool flag = !on && ____cardModel != null && ____cardModel.ClassInfo.IsEgo();
            if (flag)
            {
                for (int i = 0; i < ___img_Frames.Length; i++)
                {
                    bool flag2 = !(___img_Frames[i] == null);
                    if (flag2)
                    {
                        ___img_Frames[i].color = Color.white;
                    }
                }
            }
        }
#endif
        //应该需要hp把特定buff加上
        [HarmonyPatch(typeof(LevelUpUI), "OnSelectEgoCard")]
        [HarmonyPrefix]
        public static bool LevelUpUI_OnSelectEgoCard_Pre(BattleDiceCardUI picked)
        {
            if (HardBloodCards.Exists(x => x == picked.CardModel.GetID()))
            {
                var I39 = BattleObjectManager.instance.GetAliveList().Find(x => x.Book.BookId == MyId.Book_堂_埃尤尔之页);
                if (I39 == null)
                {
                    return true;
                }

                if (BattleUnitBuf_Don_Eyuil.GetBuf<BattleUnitBuf_HardBlood>(I39) == null)
                {
                    BattleUnitBuf_Don_Eyuil.GainBuf<BattleUnitBuf_HardBlood>(I39, 0);
                }

                // 创建映射字典
                var cardToBufMap = new Dictionary<LorId, System.Type>
                {
                    { MyId.Card_堂埃尤尔派硬血术1式_血剑_2, typeof(BattleUnitBuf_Sword) },
                    { MyId.Card_堂埃尤尔派硬血术2式_血枪_2, typeof(BattleUnitBuf_Lance) },
                    { MyId.Card_堂埃尤尔派硬血术3式_血镰_2, typeof(BattleUnitBuf_Sickle) },
                    { MyId.Card_堂埃尤尔派硬血术4式_血刃_2, typeof(BattleUnitBuf_Blade) },
                    { MyId.Card_堂埃尤尔派硬血术5式_双剑_2, typeof(BattleUnitBuf_DoubleSwords) },
                    { MyId.Card_堂埃尤尔派硬血术6式_血甲_2, typeof(BattleUnitBuf_Armour) },
                    { MyId.Card_堂埃尤尔派硬血术7式_血弓_2, typeof(BattleUnitBuf_Bow) },
                    { MyId.Card_堂埃尤尔派硬血术8式_血鞭_2, typeof(BattleUnitBuf_Scourge) },
                    { MyId.Card_堂埃尤尔派硬血术9式_血伞_2, typeof(BattleUnitBuf_Umbrella) }
                };

                if (cardToBufMap.TryGetValue(picked.CardModel.GetID(), out System.Type bufType))
                {
                    var method = typeof(BattleUnitBuf_Don_Eyuil).GetMethod("GainBuf").MakeGenericMethod(bufType);
                    method.Invoke(null, new object[] { I39, 1, BufReadyType.ThisRound });
                }

                I39.personalEgoDetail.AddCard(picked.CardModel.GetID());
                BattleManagerUI.Instance.ui_levelup.StartCoroutine(BattleManagerUI.Instance.ui_levelup.InvokeMethod<IEnumerator>("OnSelectRoutine"));

                return false;
            }

            return true;
        }


    }
}
