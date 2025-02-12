using BattleCharacterProfile;
using Don_Eyuil.Don_Eyuil.Player.Buff;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static BattleCharacterProfile.BattleCharacterProfileUI;

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
}
