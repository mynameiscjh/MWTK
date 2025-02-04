using Don_Eyuil.Don_Eyuil.Player.PassiveAbility;
using Don_Eyuil.San_Sora.Player.Buff;
using HarmonyLib;
using LOR_BattleUnit_UI;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Don_Eyuil.San_Sora.Player.PassiveAbility
{
    public class PassiveAbility_SanSora_10 : PassiveAbilityBase
    {
        public override string debugDesc => "自身拥有一套额外的卡组可设置至多4种\"硬血术\"书页\r\n自身将根据放于”硬血术”书页依次使自身的骰子获得对应效果\r\n自身可以使用个人书页”桑空派变体硬血术终式-La Sangre”\r\n(不可转移)";

        public List<LorId> list = null;

        public static Dictionary<LorId, Type> map = new Dictionary<LorId, Type>()
        {
            { MyId.Card_Desc_桑空派变体硬血术1式_血剑, typeof(BattleUnitBuf_Sword)},
            { MyId.Card_Desc_桑空派变体硬血术2式_血枪, typeof(BattleUnitBuf_Lance)},
            { MyId.Card_Desc_桑空派变体硬血术3式_血镰, typeof(BattleUnitBuf_Sickle)},
            { MyId.Card_Desc_桑空派变体硬血术4式_血刃, typeof(BattleUnitBuf_Blade)},
            { MyId.Card_Desc_桑空派变体硬血术5式_双剑, typeof(BattleUnitBuf_DoubleSwords)},
            { MyId.Card_Desc_桑空派变体硬血术6式_血甲, typeof(BattleUnitBuf_Armour)},
            { MyId.Card_Desc_桑空派变体硬血术7式_血弓, typeof(BattleUnitBuf_Bow)},
            { MyId.Card_Desc_桑空派变体硬血术8式_血鞭, typeof(BattleUnitBuf_Scourge)},
        };

        public override void OnWaveStart()
        {
            list = owner.Book.GetCardListByIndex(1).FindAll(x => HardBloodCards.HardBloodCards_San_Sora.Contains(x.id)).Select(x => x.id).ToList();
            BattleUnitBuf_Don_Eyuil.GainBuf<BattleUnitBuf_SanSora>(owner, 1);
            owner.personalEgoDetail.AddCard(MyId.Card_桑空派变体硬血术终式_La_Sangre);
        }

        public override void OnRollSpeedDice()
        {
            owner.bufListDetail.GetActivatedBufList().DoIf(x => x is BattleUnitBuf_SanHardBlood, x => x.Destroy());
            for (int i = 0; i < owner.speedDiceCount; i++)
            {
                var temp = owner.view.speedDiceSetterUI.GetFieldValue<List<SpeedDiceUI>>("_speedDices")[i];

                if (list[i] == MyId.Card_Desc_桑空派变体硬血术1式_血剑)
                {
                    BattleUnitBuf_SanHardBlood.GainBuf<BattleUnitBuf_Sword>(temp);
                }
                else if (list[i] == MyId.Card_Desc_桑空派变体硬血术2式_血枪)
                {
                    BattleUnitBuf_SanHardBlood.GainBuf<BattleUnitBuf_Lance>(temp);
                }
                else if (list[i] == MyId.Card_Desc_桑空派变体硬血术3式_血镰)
                {
                    BattleUnitBuf_SanHardBlood.GainBuf<BattleUnitBuf_Sickle>(temp);
                }
                else if (list[i] == MyId.Card_Desc_桑空派变体硬血术4式_血刃)
                {
                    BattleUnitBuf_SanHardBlood.GainBuf<BattleUnitBuf_Blade>(temp);
                }
                else if (list[i] == MyId.Card_Desc_桑空派变体硬血术5式_双剑)
                {
                    BattleUnitBuf_SanHardBlood.GainBuf<BattleUnitBuf_DoubleSwords>(temp);
                }
                else if (list[i] == MyId.Card_Desc_桑空派变体硬血术6式_血甲)
                {
                    BattleUnitBuf_SanHardBlood.GainBuf<BattleUnitBuf_Armour>(temp);
                }
                else if (list[i] == MyId.Card_Desc_桑空派变体硬血术7式_血弓)
                {
                    BattleUnitBuf_SanHardBlood.GainBuf<BattleUnitBuf_Bow>(temp);
                }
                else if (list[i] == MyId.Card_Desc_桑空派变体硬血术8式_血鞭)
                {
                    BattleUnitBuf_SanHardBlood.GainBuf<BattleUnitBuf_Scourge>(temp);
                }
            }
        }

        [HarmonyPatch(typeof(UILibrarianEquipInfoSlot), "SetData")]
        [HarmonyPostfix]
        public static void UILibrarianEquipInfoSlot_SetData_Post(BookPassiveInfo passive, Image ___Frame, TextMeshProUGUI ___txt_cost)
        {
            if (passive == null || passive.passive.id != MyTools.Create(27))
            {
                return;
            }
            ___Frame.color = Color.red;
            ___txt_cost.text = "";
        }

    }
}
