using BattleCharacterProfile;
using Don_Eyuil.Don_Eyuil.Player.Buff;
using Don_Eyuil.San_Sora.Player.Buff;
using HarmonyLib;
using Steamworks.ServerList;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WorkParser;
using static BattleCharacterProfile.BattleCharacterProfileUI;
using static BattleUnitBuf_DanteGraspInformation;

namespace Don_Eyuil.San_Sora
{
    //摇曳
    public class BattleUnitBuf_SanFlicker_Enemy : BattleUnitBuf_Don_Eyuil
    {
        protected override string keywordId => "BattleUnitBuf_SanFlicker_Enemy";
        //这一幕自身被击中时获得5点护盾并对目标施加3层"流血"
        public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
        {
            BattleUnitBuf_BloodShield.GainBuf<BattleUnitBuf_BloodShield>(owner, 5);
            if(atkDice!=null && atkDice.owner != null)
            {
                atkDice.owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Bleeding, 3);
            }
        }
        public override void OnRoundEnd()
        {
            this.Destroy();
        }
        public BattleUnitBuf_SanFlicker_Enemy(BattleUnitModel model) : base(model)
        {
            typeof(BattleUnitBuf).GetField("_bufIcon", AccessTools.all).SetValue(this, TKS_BloodFiend_Initializer.ArtWorks["摇曳"]);
            typeof(BattleUnitBuf).GetField("_iconInit", AccessTools.all).SetValue(this, true);
            this.stack = 0;
        }
    }
    //桑空派变体硬血术
    public class BattleUnitBuf_SanSora_HardBloodArt_Enemy : BattleUnitBuf_Don_Eyuil
    {
        protected override string keywordId => "BattleUnitBuf_SanSora_HardBloodArt_Enemy";
        public BattleUnitBuf_SanSora_HardBloodArt_Enemy(BattleUnitModel model) : base(model)
        {
            typeof(BattleUnitBuf).GetField("_bufIcon", AccessTools.all).SetValue(this, TKS_BloodFiend_Initializer.ArtWorks["桑空派变体硬血术"]);
            typeof(BattleUnitBuf).GetField("_iconInit", AccessTools.all).SetValue(this, true);
            this.stack = 0;
        }
        public override string bufActivatedText
        {
            get
            {
                var baseDesc = base.bufActivatedText;
                if (HardBloodTuple.Item1 == null)
                {
                    return base.bufActivatedText;
                }
                baseDesc = TextDataModel.GetText("San_Sora_HardBloodPrefix", HardBloodTuple.Item1?.Count, HardBloodTuple.Item1?.FirstOrDefault().bufActivatedName)
                    + " " + (HardBloodTuple.Item2 != null? TextDataModel.GetText("San_Sora_HardBloodPostfix",HardBloodTuple.Item2?.Count,HardBloodTuple.Item2?.FirstOrDefault().bufActivatedName) :"") 
                    + "\r\n" + baseDesc;
                baseDesc += HardBloodTuple.Item1 != null ? "\r\n" + HardBloodTuple.Item1?.FirstOrDefault().bufActivatedText : "";
                baseDesc += HardBloodTuple.Item2 != null ? "\r\n" + HardBloodTuple.Item2?.FirstOrDefault().bufActivatedText : "";
                return baseDesc;
            }
        }
        public (List<BattleUnitBuf_SanHardBlood>, List<BattleUnitBuf_SanHardBlood>) HardBloodTuple = (null,null);
    }
}
