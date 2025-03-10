﻿using Don_Eyuil.Don_Eyuil.Player.Buff;
using Don_Eyuil.WhiteMoon_Sparkle.Player.Buff;
using HarmonyLib;
using LOR_DiceSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Don_Eyuil.Don_Eyuil.Player.PassiveAbility
{
    public static class HardBloodCards
    {
        public static List<LorId> cards_Don_Eyuil = new List<LorId>();

        public static Dictionary<LorId, LorId> map_Don_Eyuil = new Dictionary<LorId, LorId>()
        {
            {MyTools.Create(45), MyTools.Create(68)},
            {MyTools.Create(46), MyTools.Create(69)},
            {MyTools.Create(47), MyTools.Create(70)},
            {MyTools.Create(48), MyTools.Create(71)},
            {MyTools.Create(49), MyTools.Create(72)},
            {MyTools.Create(50), MyTools.Create(73)},
            {MyTools.Create(51), MyTools.Create(74)},
            {MyTools.Create(52), MyTools.Create(75)},
            {MyTools.Create(53), MyTools.Create(76)},
        };

        public static List<(LorId, List<LorId>)> cardRemovaList_Don_Eyuil = new List<(LorId, List<LorId>)>
                {
                     ( MyId.Card_血伞挥打_2, new List<LorId>{MyId.Card_堂埃尤尔派硬血术8式_血鞭_2, MyId.Card_堂埃尤尔派硬血术9式_血伞_2}),
                     ( MyId.Card_旋转_绽放把_2, new List<LorId>{MyId.Card_堂埃尤尔派硬血术9式_血伞_2}),
                     ( MyId.Card_凝血化锋_2, new List<LorId>{MyId.Card_堂埃尤尔派硬血术1式_血剑_2, MyId.Card_堂埃尤尔派硬血术5式_双剑_2}),
                     ( MyId.Card_纵血为刃_2, new List<LorId>{ MyId.Card_堂埃尤尔派硬血术5式_双剑_2, MyId.Card_堂埃尤尔派硬血术6式_血甲_2}),
                     ( MyId.Card_硬血截断_2, new List<LorId>{ MyId.Card_堂埃尤尔派硬血术2式_血枪_2}),
                     ( MyId.Card_血如泉涌_2, new List<LorId>{ MyId.Card_堂埃尤尔派硬血术7式_血弓_2, MyId.Card_堂埃尤尔派硬血术8式_血鞭_2}),
                     ( MyId.Card_梦之冒险_2, new List<LorId>{ MyId.Card_堂埃尤尔派硬血术1式_血剑_2, MyId.Card_堂埃尤尔派硬血术2式_血枪_2}),
                };

        public static List<LorId> HardBloodCards_Don_Eyuil = new List<LorId>()
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

        public static List<LorId> HardBloodCards_San_Sora = new List<LorId>()
        {
            MyId.Card_Desc_桑空派变体硬血术1式_血剑,
            MyId.Card_Desc_桑空派变体硬血术2式_血枪,
            MyId.Card_Desc_桑空派变体硬血术3式_血镰,
            MyId.Card_Desc_桑空派变体硬血术4式_血刃,
            MyId.Card_Desc_桑空派变体硬血术5式_双剑,
            MyId.Card_Desc_桑空派变体硬血术6式_血甲,
            MyId.Card_Desc_桑空派变体硬血术7式_血弓,
            MyId.Card_Desc_桑空派变体硬血术8式_血鞭,
        };

        public static List<LorId> ChosenCard = new List<LorId>();

        public static Dictionary<LorId, Type> cardToBufMap_Don_Eyuil = new Dictionary<LorId, System.Type>
                {
                    { map_Don_Eyuil[MyId.Card_堂埃尤尔派硬血术1式_血剑_2], typeof(Buff.BattleUnitBuf_Sword) },
                    { map_Don_Eyuil[MyId.Card_堂埃尤尔派硬血术2式_血枪_2], typeof(BattleUnitBuf_Lance) },
                    { map_Don_Eyuil[MyId.Card_堂埃尤尔派硬血术3式_血镰_2], typeof(BattleUnitBuf_Sickle) },
                    { map_Don_Eyuil[MyId.Card_堂埃尤尔派硬血术4式_血刃_2], typeof(BattleUnitBuf_Blade) },
                    { map_Don_Eyuil[MyId.Card_堂埃尤尔派硬血术5式_双剑_2], typeof(BattleUnitBuf_DoubleSwords) },
                    { map_Don_Eyuil[MyId.Card_堂埃尤尔派硬血术6式_血甲_2], typeof(BattleUnitBuf_Armour) },
                    { map_Don_Eyuil[MyId.Card_堂埃尤尔派硬血术7式_血弓_2], typeof(Buff.BattleUnitBuf_Bow) },
                    { map_Don_Eyuil[MyId.Card_堂埃尤尔派硬血术8式_血鞭_2], typeof(BattleUnitBuf_Scourge) },
                    { map_Don_Eyuil[MyId.Card_堂埃尤尔派硬血术9式_血伞_2], typeof(BattleUnitBuf_Umbrella) }
                };

        //不使用楼层ego的整个pre(月亮计划你该死啊)
        [HarmonyPatch(typeof(BattleUnitCardsInHandUI), "UpdateCardList")]
        [HarmonyPrefix]
        public static bool BattleUnitCardsInHandUI_UpdateCardList_Pre(BattleUnitCardsInHandUI.HandState ____handState, BattleUnitModel ____selectedUnit, BattleUnitModel ____hOveredUnit, BattleUnitCardsInHandUI __instance, ref float ____xInterval, List<BattleDiceCardUI> ____activatedCardList, List<BattleDiceCardUI> ____cardList)
        {
            if (!__instance.IsActivated())
            {
                return false;
            }
            List<BattleDiceCardModel> list = new List<BattleDiceCardModel>();
            if (____handState == BattleUnitCardsInHandUI.HandState.BattleCard)
            {
                ____xInterval = 60f;
                if (____selectedUnit != null)
                {
                    list = ____selectedUnit.allyCardDetail.GetHand();
                }
                else if (____hOveredUnit != null)
                {
                    list = ____hOveredUnit.allyCardDetail.GetHand();
                }
                if (list.Count >= 9)
                {
                    ____xInterval = ____xInterval * 8f / (float)list.Count;
                }
            }
            else if (____handState == BattleUnitCardsInHandUI.HandState.EgoCard)
            {
                ____xInterval = 65f;
                BattleUnitModel battleUnitModel = null;
                if (____selectedUnit != null)
                {
                    battleUnitModel = ____selectedUnit;
                }
                else if (____hOveredUnit != null)
                {
                    battleUnitModel = ____hOveredUnit;
                }
                if (battleUnitModel != null && battleUnitModel.personalEgoDetail.ExistsCard())
                {
                    list = battleUnitModel.personalEgoDetail.GetHand();
                }
                if (battleUnitModel != null && battleUnitModel.Book.GetBookClassInfoId() != 250022 && battleUnitModel.Book.GetBookClassInfoId() != MyId.Book_堂_埃尤尔之页)
                {
                    list.AddRange(Singleton<SpecialCardListModel>.Instance.GetHand());
                }
                if (list.Count >= 9)
                {
                    ____xInterval = ____xInterval * 8f / (float)list.Count;
                }
            }
            ____activatedCardList.Clear();
            List<BattleDiceCardModel> list2 = list;
            int num = 0;
            while (num < list2.Count && num < ____cardList.Count)
            {
                ____cardList[num].gameObject.SetActive(true);
                ____cardList[num].SetCard(list2[num], Array.Empty<BattleDiceCardUI.Option>());
                ____cardList[num].SetDefault();
                ____cardList[num].ResetSiblingIndex();
                ____activatedCardList.Add(____cardList[num]);
                num++;
            }
            for (int i = 0; i < ____activatedCardList.Count; i++)
            {
                Navigation navigation = default(Navigation);
                navigation.mode = Navigation.Mode.Explicit;
                if (i > 0)
                {
                    navigation.selectOnLeft = ____activatedCardList[i - 1].selectable;
                }
                else if (____activatedCardList.Count >= 2)
                {
                    navigation.selectOnLeft = ____activatedCardList[____activatedCardList.Count - 1].selectable;
                }
                else
                {
                    navigation.selectOnLeft = null;
                }
                if (i < ____activatedCardList.Count - 1)
                {
                    navigation.selectOnRight = ____activatedCardList[i + 1].selectable;
                }
                else if (____activatedCardList.Count >= 2)
                {
                    navigation.selectOnRight = ____activatedCardList[0].selectable;
                }
                else
                {
                    navigation.selectOnRight = null;
                }
                ____activatedCardList[i].selectable.navigation = navigation;
                ____activatedCardList[i].selectable.parentSelectable = __instance.selectablePanel;
            }
            if (____activatedCardList.Count == 0)
            {
                if (UIControlManager.isControllerInput)
                {
                    __instance.emptyCardImage.gameObject.SetActive(true);
                }
            }
            else
            {
                __instance.emptyCardImage.gameObject.SetActive(false);
            }
            __instance.SetSelectedCardUI(null);
            for (int j = list2.Count; j < ____cardList.Count; j++)
            {
                ____cardList[j].gameObject.SetActive(false);
            }
            if (____selectedUnit == null)
            {
                __instance.InvokeMethod("SetActivatedCardsDefaultPos");
                return false;
            }
            if (__instance._beforeSelectDice != SingletonBehavior<BattleManagerUI>.Instance.selectedAllyDice && __instance._beforeSelectDice != null)
            {
                __instance.InvokeMethod("SetActivatedCardsDefaultPos");
            }
            return false;
        }

        //分两个卡组
        [HarmonyPatch(typeof(UIEquipDeckCardList), "SetDeckLayout")]
        [HarmonyPostfix]
        public static void UIEquipDeckCardList_SetDeckLayout_Post(UIEquipDeckCardList __instance)
        {
            var deckTabsController = __instance.GetFieldValue<UICustomTabsController>("deckTabsController");

            if (__instance.currentunit.bookItem.BookId == MyId.Book_堂_埃尤尔之页)
            {
                if (deckTabsController.transform.GetChild(1).GetComponent<HorizontalLayoutGroup>() != null)
                {
                    deckTabsController.transform.GetChild(1).GetComponent<HorizontalLayoutGroup>().enabled = false;
                }
                deckTabsController.CustomTabs[0].TabName.text = "并非硬血术卡组";
                deckTabsController.CustomTabs[0].transform.localPosition = new Vector3(118.8028f, 20.65f, 0);
                deckTabsController.CustomTabs[1].TabName.text = "硬血术卡组";
                deckTabsController.CustomTabs[1].transform.localPosition = new Vector3(312.7585f, 20.65f, 0);
                deckTabsController.CustomTabs[2].gameObject.SetActive(false);
                deckTabsController.CustomTabs[3].gameObject.SetActive(false);
                return;
            }
            if (__instance.currentunit.bookItem.BookId == MyId.Book_桑空之页)
            {
                if (deckTabsController.transform.GetChild(1).GetComponent<HorizontalLayoutGroup>() != null)
                {
                    deckTabsController.transform.GetChild(1).GetComponent<HorizontalLayoutGroup>().enabled = false;
                }
                deckTabsController.CustomTabs[0].TabName.text = "并非桑空卡组";
                deckTabsController.CustomTabs[0].transform.localPosition = new Vector3(118.8028f, 20.65f, 0);
                deckTabsController.CustomTabs[1].TabName.text = "桑空卡组";
                deckTabsController.CustomTabs[1].transform.localPosition = new Vector3(312.7585f, 20.65f, 0);
                deckTabsController.CustomTabs[2].gameObject.SetActive(false);
                deckTabsController.CustomTabs[3].gameObject.SetActive(false);

                if (__instance.currentunit.bookItem.GetCurrentDeckIndex() == 1)
                {
                    __instance.transform.GetChild(1).GetChild(0).GetChild(4).gameObject.SetActive(false);
                    __instance.transform.GetChild(1).GetChild(0).GetChild(5).gameObject.SetActive(false);
                    __instance.transform.GetChild(1).GetChild(0).GetChild(6).gameObject.SetActive(false);
                    __instance.transform.GetChild(1).GetChild(0).GetChild(7).gameObject.SetActive(false);
                    __instance.transform.GetChild(1).GetChild(0).GetChild(8).gameObject.SetActive(false);
                }
                else
                {
                    __instance.transform.GetChild(1).GetChild(0).GetChild(4).gameObject.SetActive(true);
                    __instance.transform.GetChild(1).GetChild(0).GetChild(5).gameObject.SetActive(true);
                    __instance.transform.GetChild(1).GetChild(0).GetChild(6).gameObject.SetActive(true);
                    __instance.transform.GetChild(1).GetChild(0).GetChild(7).gameObject.SetActive(true);
                    __instance.transform.GetChild(1).GetChild(0).GetChild(8).gameObject.SetActive(true);
                }

                return;
            }
            deckTabsController.CustomTabs[0].gameObject.SetActive(false);
            deckTabsController.CustomTabs[1].gameObject.SetActive(false);
            deckTabsController.CustomTabs[2].gameObject.SetActive(false);
            deckTabsController.CustomTabs[3].gameObject.SetActive(false);
            deckTabsController.CustomTabs[0].gameObject.SetActive(true);
            deckTabsController.CustomTabs[1].gameObject.SetActive(true);
            deckTabsController.CustomTabs[2].gameObject.SetActive(true);
            deckTabsController.CustomTabs[3].gameObject.SetActive(true);
            deckTabsController.transform.GetChild(1).GetComponent<HorizontalLayoutGroup>().enabled = true;

        }

        //更改专属卡(月亮计划你该死啊)
        [HarmonyPatch(typeof(BookModel), "GetOnlyCards")]
        [HarmonyPostfix]
        public static void BookModel_GetOnlyCards_Post(ref List<DiceCardXmlInfo> __result, BookModel __instance)
        {
            if (__instance.BookId.packageId != TKS_BloodFiend_Initializer.packageId)
            {
                return;
            }
            __result.Clear();
            foreach (var item in __instance.ClassInfo.EquipEffect.OnlyCard)
            {
                var card = ItemXmlDataList.instance.GetCardItem(MyTools.Create(item));
                __result.Add(card);
            }
            if (__instance.BookId == MyId.Book_堂_埃尤尔之页)
            {
                __result.AddRange(map_Don_Eyuil.Values.ToList().Select(x => ItemXmlDataList.instance.GetCardItem(x)));
            }
        }

        //实现了硬血卡组只能配硬血卡 特殊卡只能配特殊硬血卡组后才能配(我你该死啊)
        [HarmonyPatch(typeof(UIInvenCardListScroll), "ApplyFilterAll")]
        [HarmonyPostfix]
        public static void UIInvenCardListScroll_ApplyFilterAll_Post(List<DiceCardItemModel> ____currentCardListForFilter, UIInvenCardListScroll __instance)
        {
            var temp = __instance.GetFieldValue<UnitDataModel>("_unitdata");
            if (temp == null)
            {
                return;
            }
            if (temp.bookItem.BookId == MyId.Book_堂_埃尤尔之页)
            {
                if (temp.bookItem.GetCurrentDeckIndex() == 1)
                {
                    ____currentCardListForFilter.Clear();
                    foreach (var item in HardBloodCards_Don_Eyuil)
                    {
                        var card = ItemXmlDataList.instance.GetCardItem(map_Don_Eyuil[item]);
                        DiceCardItemModel itemModel = new DiceCardItemModel(card);
                        itemModel.num = 99;
                        ____currentCardListForFilter.Add(itemModel);
                        ____currentCardListForFilter.RemoveAll(x => x.GetID() == MyId.Card_堂埃尤尔派硬血术终式_La_Sangre_2);
                    }
                }
                else
                {
                    ____currentCardListForFilter.RemoveAll(x => map_Don_Eyuil.Values.ToList().Exists(item => item == x.ClassInfo.id));
                    ____currentCardListForFilter.RemoveAll(x => HardBloodCards_Don_Eyuil.Exists(item => item == x.ClassInfo.id));
                    ____currentCardListForFilter.RemoveAll(x => x.GetID() == MyId.Card_堂埃尤尔派硬血术终式_La_Sangre_2);

                    var list = temp.bookItem.GetCardListByIndex(1).Select(x => x.id).ToList();

                    var temp_object = GameObject.Find("UI_Object/[CG]PPForForceCg/FrontCanvas/[Panel]BattlePagePanel(Clone)/PanelActiveController/[Librarian]Left_Panel/[Script]LibrarianDeckPanel/[Script]CardDeckPanel");

                    foreach (var item in cardRemovaList_Don_Eyuil)
                    {
                        foreach (var needCard in item.Item2)
                        {
                            var A = map_Don_Eyuil[needCard];
                            if (!list.Contains(A))
                            {
                                ____currentCardListForFilter.RemoveAll(x => x.GetID() == item.Item1);
                                temp.bookItem.MoveCardFromCurrentDeckToInventory(item.Item1);
                                var component = temp_object?.GetComponent<UIEquipDeckCardList>();

                                component?.SetCardsData(component?.currentunit?.GetDeckCardModelAll());
                            }
                        }
                    }
                }
                foreach (var item in HardBloodCards_San_Sora)
                {
                    ____currentCardListForFilter.RemoveAll(x => x.GetID() == item);
                }
                __instance.SetCardsData(__instance.GetCurrentPageList());
                return;
            }
            if (temp.bookItem.BookId == MyId.Book_桑空之页)
            {
                if (temp.bookItem.GetCurrentDeckIndex() == 1)
                {
                    if (temp.bookItem.GetCardListFromCurrentDeck().Count >= 4)
                    {
                        var temp_object = GameObject.Find("UI_Object/[CG]PPForForceCg/FrontCanvas/[Panel]BattlePagePanel(Clone)/PanelActiveController/[Librarian]Left_Panel/[Script]LibrarianDeckPanel/[Script]CardDeckPanel");
                        for (int i = 4; i < temp.bookItem.GetCardListFromCurrentDeck().Count; i++)
                        {
                            temp.bookItem.MoveCardFromCurrentDeckToInventory(temp.bookItem.GetCardListFromCurrentDeck()[i].id);
                            var component = temp_object?.GetComponent<UIEquipDeckCardList>();

                            component?.SetCardsData(component?.currentunit?.GetDeckCardModelAll());
                        }
                    }

                    ____currentCardListForFilter.Clear();
                    foreach (var item in HardBloodCards_San_Sora)
                    {
                        var card = ItemXmlDataList.instance.GetCardItem(item);
                        DiceCardItemModel itemModel = new DiceCardItemModel(card);
                        itemModel.num = 99;
                        ____currentCardListForFilter.Add(itemModel);
                    }

                    __instance.SetCardsData(__instance.GetCurrentPageList());
                    return;
                }
            }

            foreach (var item in HardBloodCards_San_Sora)
            {
                ____currentCardListForFilter.RemoveAll(x => x.GetID() == item);
            }
            ____currentCardListForFilter.RemoveAll(x => map_Don_Eyuil.Values.ToList().Exists(item => item == x.ClassInfo.id));
            __instance.SetCardsData(__instance.GetCurrentPageList());
        }

        //取消硬血卡的特殊限制
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

            if (UI.UIController.Instance.CurrentUnit.bookItem.GetCurrentDeckIndex() == 1 && UI.UIController.Instance.CurrentUnit.bookItem.BookId == MyId.Book_桑空之页 && UI.UIController.Instance.CurrentUnit.bookItem.GetCardListFromCurrentDeck().Count >= 5)
            {
                __result = CardEquipState.FullOfDeck;
            }

            if (HardBloodCards_Don_Eyuil.Contains(cardId))
            {
                __result = CardEquipState.Equippable;
            }
            if (HardBloodCards_San_Sora.Contains(cardId))
            {
                __result = CardEquipState.Equippable;
            }
            if (map_Don_Eyuil.Values.ToList().Contains(cardId))
            {
                __result = CardEquipState.Equippable;
            }
        }

        [HarmonyPatch(typeof(BookModel), "AddCardFromInventoryToCurrentDeck")]
        [HarmonyPostfix]
        public static void BookModel_AddCardFromInventoryToCurrentDeck_Pos(BookModel __instance, ref CardEquipState __result)
        {
            if (__instance.GetCurrentDeckIndex() == 1 && __instance.BookId == MyId.Book_桑空之页 && __instance.GetCardListFromCurrentDeck().Count >= 5)
            {
                __result = CardEquipState.FullOfDeck;
            }
        }


        //跟新
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

                var temp = __instance.transform.parent.parent.parent.GetChild(1).GetChild(0).GetComponent<UIInvenCardListScroll>();
                temp.ApplyFilterAll();
            }
            if (__instance.currentunit.bookItem.BookId == MyId.Book_桑空之页)
            {
                var temp = __instance.transform.parent.parent.parent.GetChild(1).GetChild(0).GetComponent<UIInvenCardListScroll>();
                temp.ApplyFilterAll();
            }
            return;
        }

        //取消硬血卡的特殊限制
        [HarmonyPatch(typeof(InventoryModel), "RemoveCard")]
        [HarmonyPrefix]
        public static bool InventoryModel_RemoveCard_Pre(LorId cardId, ref bool __result)
        {
            if (HardBloodCards_Don_Eyuil.Contains(cardId))
            {
                __result = true;
                return false;
            }
            if (HardBloodCards_San_Sora.Contains(cardId))
            {
                __result = true;
                return false;
            }
            if (map_Don_Eyuil.Values.Contains(cardId))
            {
                __result = true;
                return false;
            }
            return true;
        }

        //可以配ego书页
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
                        UnityEngine.Debug.Log("Image.name:" + (x?.name));
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

        //可以配ego书页
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

        //可以配ego书页
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

            if (BattleManagerUI.Instance.ui_levelup.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text == "选择主武器书页")
            {
                if (picked.CardModel.GetID() == MyId.Card_Desc_泉之龙_秋之莲)
                {
                    BattleUnitBuf_Sparkle.Instance.AddPrimaryWeapon<BattleUnitBuf_Year>();
                }
                if (picked.CardModel.GetID() == MyId.Card_Desc_千斤弓)
                {
                    BattleUnitBuf_Sparkle.Instance.AddPrimaryWeapon<WhiteMoon_Sparkle.Player.Buff.BattleUnitBuf_Bow>();
                }
                if (picked.CardModel.GetID() == MyId.Card_Desc_月之剑)
                {
                    BattleUnitBuf_Sparkle.Instance.AddPrimaryWeapon<WhiteMoon_Sparkle.Player.Buff.BattleUnitBuf_Sword>();
                }
                BattleManagerUI.Instance.ui_levelup.StartCoroutine(BattleManagerUI.Instance.ui_levelup.InvokeMethod<IEnumerator>("OnSelectRoutine"));
                return false;
            }

            if (BattleManagerUI.Instance.ui_levelup.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text == "选择副武器书页")
            {
                if (picked.CardModel.GetID() == MyId.Card_Desc_泉之龙_秋之莲)
                {
                    BattleUnitBuf_Sparkle.Instance.AddSubWeapon<BattleUnitBuf_Year>();
                }
                if (picked.CardModel.GetID() == MyId.Card_Desc_千斤弓)
                {
                    BattleUnitBuf_Sparkle.Instance.AddSubWeapon<WhiteMoon_Sparkle.Player.Buff.BattleUnitBuf_Bow>();
                }
                if (picked.CardModel.GetID() == MyId.Card_Desc_月之剑)
                {
                    BattleUnitBuf_Sparkle.Instance.AddSubWeapon<WhiteMoon_Sparkle.Player.Buff.BattleUnitBuf_Sword>();
                }
                BattleManagerUI.Instance.ui_levelup.StartCoroutine(BattleManagerUI.Instance.ui_levelup.InvokeMethod<IEnumerator>("OnSelectRoutine"));
                return false;
            }

            if (map_Don_Eyuil.Values.ToList().Exists(x => x == picked.CardModel.GetID()))
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

                if (cardToBufMap_Don_Eyuil.TryGetValue(picked.CardModel.GetID(), out System.Type bufType))
                {
                    if (typeof(BattleUnitBuf_Don_Eyuil).GetMethod("GetBuf").MakeGenericMethod(bufType).Invoke(null, new object[] { I39, BufReadyType.ThisRound }) == null)
                    {
                        var method = typeof(BattleUnitBuf_Don_Eyuil).GetMethod("GainBuf").MakeGenericMethod(bufType);
                        method.Invoke(null, new object[] { I39, 1, BufReadyType.ThisRound });
                    }
                }
                ChosenCard.Add(picked.CardModel.GetID());
                var temp = map_Don_Eyuil.ToList().Find(x => x.Value == picked.CardModel.GetID()).Key;
                cards_Don_Eyuil.Remove(picked.CardModel.GetID());
                I39.personalEgoDetail.AddCard(temp);
                BattleManagerUI.Instance.ui_levelup.StartCoroutine(BattleManagerUI.Instance.ui_levelup.InvokeMethod<IEnumerator>("OnSelectRoutine"));
                return false;
            }

            return true;
        }

    }
    public class PassiveAbility_DonEyuil_15 : PassiveAbilityBase
    {
        public override string debugDesc => "堂埃尤尔派硬血术 0费 特殊\r\n自身拥有一套额外的卡组可设置\"硬血术\"书页\r\n情感等级达到0/2/4时可以选择激活设置的\"硬血术\"书页情感等级达到4级后每有一名角色因流血死亡则可额外激活一次设置的\"硬血术\"书页\r\n(这里的选择激活硬血术界面用选EGO的那个levelup的UI做)\r\n(实现上面，基本就是正常的多写一个卡组就可以了)\r\n\r\n自身可使用个人书页\"堂埃尤尔派硬血术终式\"且无法使用楼层E.G.O书页\r\n";



        public override void OnWaveStart()
        {

            if (StageController.Instance.CurrentWave == 1)
            {
                HardBloodCards.ChosenCard.Clear();
            }

            HardBloodCards.cards_Don_Eyuil = new List<LorId>(this.owner.UnitData.unitData.GetDeckForBattle(1).Where(item => HardBloodCards.map_Don_Eyuil.Values.Contains(item.id) && !HardBloodCards.ChosenCard.Contains(item.id)).Select(item => item.id));

            foreach (var item in HardBloodCards.ChosenCard)
            {
                if (BattleUnitBuf_Don_Eyuil.GetBuf<BattleUnitBuf_HardBlood>(owner) == null)
                {
                    BattleUnitBuf_Don_Eyuil.GainBuf<BattleUnitBuf_HardBlood>(owner, 0);
                }

                if (HardBloodCards.cardToBufMap_Don_Eyuil.TryGetValue(item, out System.Type bufType))
                {
                    if (typeof(BattleUnitBuf_Don_Eyuil).GetMethod("GetBuf").MakeGenericMethod(bufType).Invoke(null, new object[] { owner, BufReadyType.ThisRound }) == null)
                    {
                        var method = typeof(BattleUnitBuf_Don_Eyuil).GetMethod("GainBuf").MakeGenericMethod(bufType);
                        method.Invoke(null, new object[] { owner, 1, BufReadyType.ThisRound });
                    }
                }

                var temp = HardBloodCards.map_Don_Eyuil.ToList().Find(x => x.Value == item).Key;
                owner.personalEgoDetail.AddCard(temp);
            }

            owner.personalEgoDetail.AddCard(MyId.Card_堂埃尤尔派硬血术终式_La_Sangre_2);
            fl0 = false;
            fl2 = false;
            fl4 = false;

            if (BattleObjectManager.instance.GetAliveList().Exists(x => x.Book.BookId == MyId.Book_小耀之页))
            {
                BattleUnitBuf_Don_Eyuil.GainBuf<BattleUnitBuf_Transmit>(owner, 1);
            }

        }

        bool fl0 = false;
        bool fl2 = false;
        bool fl4 = false;

        public override void OnRoundStart()
        {
            owner.allyCardDetail.AddTempCard(MyTools.Create(65535)).SetCostToZero();
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

            BattleManagerUI.Instance.ui_levelup.StartCoroutine(OnSelectRoutine(emoCards));
        }

        IEnumerator OnSelectRoutine(List<EmotionEgoXmlInfo> emoCards)
        {
            yield return new WaitUntil(() => BattleManagerUI.Instance.ui_levelup.IsEnabled == false);
            BattleManagerUI.Instance.ui_levelup.SetRootCanvas(true);
            BattleManagerUI.Instance.ui_levelup.InitEgo(Math.Min(3, emoCards.Count), emoCards);
            BattleManagerUI.Instance.ui_levelup.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(1).GetChild(1).GetChild(0).GetComponent<Image>().sprite = TKS_BloodFiend_Initializer.ArtWorks["玩家硬血术统一图标"];
            BattleManagerUI.Instance.ui_levelup.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "选择硬血流书页";
            BattleManagerUI.Instance.ui_levelup._emotionLevels.Do(x => x.Set(false, false, false));
            yield return new WaitUntil(() => BattleManagerUI.Instance.ui_levelup.IsEnabled == false);
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