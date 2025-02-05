using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;
using System.Reflection.Emit;
using HyperCard;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;
using UI;
using static CharacterSound;
using System.Runtime.InteropServices;
using Don_Eyuil.San_Sora.Player.Buff;
using Don_Eyuil;
namespace Don_Eyuil.San_Sora
{
    public class PassiveAbility_SanSora_01 : PassiveAbilityBase
    {
        //200年前的回忆
        public override string debugDesc => "每幕开始时移除手中的书页并将意图使用的书页置入手中\r\n自身书页费用为0";
        /*(自身每幕将随机选择血甲外的两种硬血术以buff的形式平分填入每颗骰子
        一阶段:初始速度骰子*4此后每幕增加1颗至多6颗
            第一幕: 摧垮 致伤 致伤 释血化刃 剩余的速度骰子随机填入摧垮 致伤 释血化刃
            第二幕:利血贯穿 伴血猛袭 伴血猛袭 冲锋截断 摧垮 摧垮 剩余的速度骰子随机填入摧垮 致伤 释血化刃
            第三幕: 速度骰子变为1颗固定为血甲并固定使用桑空派变体硬血术6式-血甲
            第四幕：速度骰子变为4颗且所有骰子为血甲固定使用硬血为铠
            循环
            //开启舞台时获得500层护盾
            //护盾消耗完时改变自身行动逻辑并获得新的被动在此之前体力无法低于500
            //(恢复所有混乱抗性并清除自身负面buff并获得被动”亲族们仍在苦难之中!”与”若能实现那理想中的共存”)
        二阶段:
            第一幕:速度骰子变为7颗 受苦的亲族正在增多 利血贯穿 利血贯穿 伴血猛袭 冲锋截断 血刃剔除 剩余的速度骰子随机填入伴血猛袭与摧垮
            第二幕：速度骰子变为6颗 利血贯穿 冲锋截断 血刃剔除 血刃剔除 释血化刃 释血化刃
            第三幕：速度骰子变为6颗 冲锋截断 冲锋截断 伴血猛袭 剩余的速度骰子随机填入伴血猛袭与摧垮
            第四幕:速度骰子变为4颗 若能摆脱这可怖的疾病...血刃剔除 血刃剔除 血刃剔除
            循环
        三阶段速度骰子固定为7颗
            每幕固定使用1-2张翱翔向梦其余骰子随机填入血刃剔除 伴血猛袭 冲锋截断 释血化刃 利血贯穿
            第二幕固定使用一张桑空派变体硬血术终式-La Sangre
            第四幕速度骰子固定为1颗速度改为99且无法选中 使用 直到触及到那梦想!后司书接待胜利)*/

        public static int Phase = 1;
        public static int Phase2Round = 0;
        public static int Phase3Round = 0;
        public override void OnWaveStart()
        {
            Phase = 1; Phase2Round = 0; Phase3Round = 0;
        }

        public override int GetMinHp()
        {
            switch (Phase)
            {
                case 1: case 2: return 500;
                case 3: return 100;
                default: return base.GetMinHp();
            }
        }

    }
    public class PassiveAbility_SanSora_02: PassiveAbilityBase
    {
        //桑空派变体硬血术
        public override string debugDesc => "自身将定期将速度骰子设置为不同的硬血术效果\r\n命中目标时将恢复等同于骰子基础值的体力";
        public override void OnSucceedAreaAttack(BattleDiceBehavior behavior, BattleUnitModel target)
        {
            owner.RecoverHP(behavior.DiceVanillaValue); 
        }
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            if(behavior != null && behavior.card != null && !(behavior.card.card.XmlData.Spec.Ranged == CardRange.FarArea || behavior.card.card.XmlData.Spec.Ranged == CardRange.FarAreaEach))
            {
                owner.RecoverHP(behavior.DiceVanillaValue);
            }
        }
    }
    public class PassiveAbility_SanSora_03 : PassiveAbilityBase
    {
        //展翼向梦
        public override string debugDesc => "自身命中目标时获得2层”血羽”\r\n每幕结束时若自身”血羽”不低于10层则使自身在下一幕中获得1层”迅捷”";
        public override void OnRoundEnd()
        {
            if(BattleUnitBuf_Feather.GetBufStack<BattleUnitBuf_Feather>(owner) >= 10)
            {
                owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Quickness, 1);
            }
        }
        public override void OnSucceedAreaAttack(BattleDiceBehavior behavior, BattleUnitModel target)
        {
            BattleUnitBuf_Feather.GainBuf<BattleUnitBuf_Feather>(owner, 2);
        }
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            if (behavior != null && behavior.card != null && !(behavior.card.card.XmlData.Spec.Ranged == CardRange.FarArea || behavior.card.card.XmlData.Spec.Ranged == CardRange.FarAreaEach))
            {
                BattleUnitBuf_Feather.GainBuf<BattleUnitBuf_Feather>(owner, 2);
            }
        }
    }
    public class PassiveAbility_SanSora_04 : PassiveAbilityBase
    {
        //血甲
        public override string debugDesc => "开启舞台时获得500层护盾\r\n护盾消耗完时改变自身行动逻辑并获得新的被动在此之前体力无法低于500";
        public override void OnWaveStart()
        {
            BattleUnitBuf_BloodShield.GainBuf<BattleUnitBuf_BloodShield>(owner, 500);
        }
    }
}
