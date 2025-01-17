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
namespace Don_Eyuil
{
    public class PassiveAbility_DonEyuil_01 : PassiveAbilityBase
    {
        public class BattleUnitBuf_HardBloodArt : BattleUnitBuf_Don_Eyuil
        {
            public BattleUnitBuf_HardBloodArt(BattleUnitModel model) : base(model)
            {
                typeof(BattleUnitBuf).GetField("_iconInit", AccessTools.all).SetValue(this, true);
                this.stack = 0;
            }
            //血剑:
            //自身斩击骰子最小值+2
            //自身施加"流血"时额外对自身和目标施加一层
            //每幕结束时自身每承受2点"流血"伤害便时自身获得1层"硬血结晶"
            public class BattleUnitBuf_HardBloodArt_BloodSword: BattleUnitBuf_HardBloodArt
            {
                public int BleedingDamageThisRound = 0;
                public override void OnRoundEnd()
                {
                    BattleUnitBuf_HardBlood_Crystal.GainBuf<BattleUnitBuf_HardBlood_Crystal>(_owner, BleedingDamageThisRound / 2);
                    BleedingDamageThisRound = 0;
                }
                public override void AfterTakeBleedingDamage(int Dmg)
                { 
                    BleedingDamageThisRound += Dmg;
                }
                public override void BeforeRollDice(BattleDiceBehavior behavior)
                {
                    if (behavior.Detail == BehaviourDetail.Slash)
                    {
                        BattleCardTotalResult battleCardResultLog = _owner.battleCardResultLog;
                        if (battleCardResultLog != null)
                        {
                            battleCardResultLog.SetBufs(this);
                        }
                        behavior.ApplyDiceStatBonus(new DiceStatBonus
                        {
                             power = 2
                        });
                    }
                }
                public override int OnGiveKeywordBufByCard(BattleUnitBuf cardBuf, int stack, BattleUnitModel target)
                {
                    if(cardBuf.bufType == KeywordBuf.Bleeding)
                    {
                        if (target != null && _owner != null)
                        {
                            _owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Bleeding, 1);
                            target.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Bleeding, 1);
                        }
                    }
                    return base.OnGiveKeywordBufByCard(cardBuf, stack, target); 
                }
                public BattleUnitBuf_HardBloodArt_BloodSword(BattleUnitModel model) : base(model)
                {
                    typeof(BattleUnitBuf).GetField("_bufIcon", AccessTools.all).SetValue(this,TKS_BloodFiend_Initializer.ArtWorks["敌方血剑"]);
                }
            }

            //血枪:
            //拼点时自身速度每高于目标1点便使自身所有骰子最大值+1(至多+4)
            //自身以高于目标至少2点的速度击中目标时将对目标施加1层"无法凝结的血"(每幕至多触发3次)
            //若击杀目标则在下一幕获得一层"热血尖枪"
            public class BattleUnitBuf_HardBloodArt_BloodLance : BattleUnitBuf_HardBloodArt
            {
                public override void BeforeRollDice(BattleDiceBehavior behavior)
                {
                    if (behavior != null && behavior.TargetDice != null)
                    {
                        behavior.ApplyDiceStatBonus(new DiceStatBonus()
                        {
                            max = Math.Max(0, Math.Min(behavior.card.speedDiceResultValue - behavior.TargetDice.card.speedDiceResultValue, 4))
                        });
                        BattleCardTotalResult battleCardResultLog = _owner.battleCardResultLog;
                        if (battleCardResultLog != null)
                        {
                            battleCardResultLog.SetBufs(this);
                        }
                    }
                }
                public int OnSuccessAttTargetNum = 0;
                public override void OnRoundEnd()
                {
                    OnSuccessAttTargetNum = 0;
                }
                public override void OnSuccessAttack(BattleDiceBehavior behavior)
                {
                    if (behavior != null && behavior.card.card.XmlData.Spec.Ranged != LOR_DiceSystem.CardRange.FarAreaEach && behavior.card.card.XmlData.Spec.Ranged != LOR_DiceSystem.CardRange.FarArea)
                    {
                        if (behavior.card.target != null)
                        {
                            if ((behavior.card.speedDiceResultValue - behavior.card.target.GetSpeedDiceResult(behavior.card.targetSlotOrder).value) >= 2)
                            {
                                OnSuccessAttTargetNum++;
                                if (OnSuccessAttTargetNum <= 3)
                                {
                                    BattleUnitBuf_UncondensableBlood.GainBuf<BattleUnitBuf_UncondensableBlood>(behavior.card.target, 1);
                                }
                            }
                        }
                    }
                }
                public override void OnKill(BattleUnitModel target)
                {
                    if (target != null)
                    {
                        BattleUnitBuf_WarmBloodLance.GainBuf<BattleUnitBuf_WarmBloodLance>(_owner, 1, BufReadyType.NextRound);
                    }
                }
                public BattleUnitBuf_HardBloodArt_BloodLance(BattleUnitModel model) : base(model)
                {
                    typeof(BattleUnitBuf).GetField("_bufIcon", AccessTools.all).SetValue(this, TKS_BloodFiend_Initializer.ArtWorks["敌方血枪"]);
                }
            }
            //双剑
            //自身受到单方面攻击时将以一颗(闪避4-8[拼点胜利]对目标施加1层[流血])迎击
            //自身承受"流血"伤害时每承受3点便使下一颗进攻型骰子伤害+1
            public class BattleUnitBuf_HardBloodArt_DoubleSwords : BattleUnitBuf_HardBloodArt
            {
                public int BleedingDamageThisRound = 0;
                public override void AfterTakeBleedingDamage(int Dmg)
                {
                    BleedingDamageThisRound += Dmg;
                }
                public override void OnRollDice(BattleDiceBehavior behavior)
                {
                    behavior.ApplyDiceStatBonus(new DiceStatBonus() { dmg =  BleedingDamageThisRound / 3 });
                    BattleCardTotalResult battleCardResultLog = _owner.battleCardResultLog;
                    if (battleCardResultLog != null)
                    {
                        battleCardResultLog.SetBufs(this);
                    }
                    BleedingDamageThisRound = 0;
                }
                public override void OnStartBattle()
                {
                    BattleDiceCardModel battleDiceCardModel = BattleDiceCardModel.CreatePlayingCard(ItemXmlDataList.instance.GetCardItem(MyId.Card_双剑反击闪避书页, false));
                    if (battleDiceCardModel != null)
                    {
                        foreach (BattleDiceBehavior behaviour in battleDiceCardModel.CreateDiceCardBehaviorList())
                        {
                            this._owner.cardSlotDetail.keepCard.AddBehaviourForOnlyDefense(battleDiceCardModel, behaviour);
                        }
                    }
                }
                public BattleUnitBuf_HardBloodArt_DoubleSwords(BattleUnitModel model) : base(model)
                {
                    typeof(BattleUnitBuf).GetField("_bufIcon", AccessTools.all).SetValue(this, TKS_BloodFiend_Initializer.ArtWorks["敌方双剑"]);
                }
            }
            //血刃
            //自身命中处于混乱状态的目标时将触发目标"流血"(每张书页至多1次)
           // 一幕中敌方每受到20点"流血"伤害便使自身获得1层”伤害强化”
            public class BattleUnitBuf_HardBloodArt_BloodBlade : BattleUnitBuf_HardBloodArt
            {
                public List<BattlePlayingCardDataInUnitModel> TriggeredCards = new List<BattlePlayingCardDataInUnitModel>() { };
                public override void OnSuccessAttack(BattleDiceBehavior behavior)
                {
                    if (behavior != null && behavior.card != null && !TriggeredCards.Exists(x => x == behavior.card))
                    {
                        if (behavior.card.target.IsBreakLifeZero())
                        {
                            var buf = behavior.card.target.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_bleeding) as BattleUnitBuf_bleeding;
                            if (buf != null)
                            {
                                buf.AfterDiceAction(behavior);
                                TriggeredCards.Add(behavior.card);
                            }
                        }
                    }
                }
                public int EnemyBleedingDamageThisRound = 0;
                public override void OnRoundEnd()
                {
                    _owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Bleeding, EnemyBleedingDamageThisRound / 20);
                    EnemyBleedingDamageThisRound = 0;
                }
                public override void AfterOtherUnitTakeBleedingDamage(BattleUnitModel Unit, int Dmg)
                {
                    if(Unit.faction != _owner.faction)
                    {
                        EnemyBleedingDamageThisRound += Dmg;
                    }
                }
                public BattleUnitBuf_HardBloodArt_BloodBlade(BattleUnitModel model) : base(model)
                {
                    typeof(BattleUnitBuf).GetField("_bufIcon", AccessTools.all).SetValue(this, TKS_BloodFiend_Initializer.ArtWorks["敌方血刃"]);
                }
            }
            //血弓
            //自身远程骰子最小值+3且命中目标时将在本幕对目标施加3层"流血"
            //自身将对每幕第一个击中的目标施加"深度创痕
            public class BattleUnitBuf_HardBloodArt_BloodBow: BattleUnitBuf_HardBloodArt
            {

                public override void OnRollDice(BattleDiceBehavior behavior)
                {
                    if(behavior != null && behavior.card != null && behavior.card.card != null && behavior.card.card.XmlData.Spec.Ranged == CardRange.Far)
                    {
                        behavior.ApplyDiceStatBonus(new DiceStatBonus() { min = 3 });
                    }
                }
                public bool HasTriggerOnSuccessAtl = false;
                public override void OnRoundEnd()
                {
                    HasTriggerOnSuccessAtl = false;
                }
                public override void OnSuccessAttack(BattleDiceBehavior behavior)
                {
                    if (behavior != null && behavior.card != null && behavior.card.card != null && behavior.card.card.XmlData.Spec.Ranged == CardRange.Far)
                    {
                        if(behavior.card.target != null)
                        {
                            behavior.card.target.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Bleeding, 3);
                            if(HasTriggerOnSuccessAtl == false)
                            {
                                HasTriggerOnSuccessAtl = true;
                                BattleUnitBuf_DeepWound.GainBuf<BattleUnitBuf_DeepWound>(behavior.card.target, 1);
                            }
                        }
                    }
                }
                public BattleUnitBuf_HardBloodArt_BloodBow(BattleUnitModel model) : base(model)
                {
                    typeof(BattleUnitBuf).GetField("_bufIcon", AccessTools.all).SetValue(this, TKS_BloodFiend_Initializer.ArtWorks["敌方血弓"]);
                }
            }
            //血鞭
            //自身以打击骰子施加"流血"时将额外对一名敌方角色施加等量流血
            //自身每幕最后一张书页施加"流血"时将额外对目标施加等量"血晶荆棘"
            public class BattleUnitBuf_HardBloodArt_BloodScourge : BattleUnitBuf_HardBloodArt
            {
                private bool _triggered;
                public override void OnRoundEnd() { _triggered = false; }
                public override void OnRoundStart() { _triggered = false; }
                public override int OnGiveKeywordBufByCard(BattleUnitBuf cardBuf, int stack, BattleUnitModel target)
                {
                    bool CheckCondition(BattleDiceBehavior behavior)
                    {
                        if (this._triggered)
                        {
                            return false;
                        }
                        BattlePlayingCardDataInUnitModel[] array = Singleton<StageController>.Instance.GetAllCards().ToArray();
                        BattlePlayingCardDataInUnitModel card = behavior.card;
                        foreach (BattlePlayingCardDataInUnitModel battlePlayingCardDataInUnitModel in array)
                        {
                            if (battlePlayingCardDataInUnitModel.owner == base._owner && battlePlayingCardDataInUnitModel != card)
                            {
                                return false;
                            }
                        }
                        return true;
                    }
                    if(_owner.currentDiceAction != null && _owner.currentDiceAction.currentBehavior != null && cardBuf.bufType == KeywordBuf.Bleeding)
                    {
                        if (CheckCondition(_owner.currentDiceAction.currentBehavior))
                        {
                            BattleUnitBuf_BloodCrystalThorn.GainBuf<BattleUnitBuf_BloodCrystalThorn>(target, stack,BufReadyType.NextRound);
                        }
                        if ( _owner.currentDiceAction.currentBehavior.Detail == BehaviourDetail.Hit)
                        {
                            var List = BattleObjectManager.instance.GetAliveList_opponent(_owner.faction);
                            if (List != null)
                            {
                                List.Remove(target);
                                if (List.Count > 0)
                                {
                                    RandomUtil.SelectOne(List).bufListDetail.AddKeywordBufByEtc(KeywordBuf.Bleeding, stack);
                                }
                            }
                            //.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Bleeding, stack);
                        }
                    }
                    return base.OnGiveKeywordBufByCard(cardBuf, stack, target);
                }
                public BattleUnitBuf_HardBloodArt_BloodScourge(BattleUnitModel model) : base(model)
                {
                    typeof(BattleUnitBuf).GetField("_bufIcon", AccessTools.all).SetValue(this, TKS_BloodFiend_Initializer.ArtWorks["敌方血鞭"]);
                }
            }
        }
        //200年前的回忆
        public override string debugDesc => "每幕开始时移除手中的书页并将意图使用的书页置入手中\r\n自身书页费用为0";
        /*
            每幕开始时移除手中的书页并将意图使用的书页置入手中
            自身书页费用为0
            (一阶段：自第二幕起幕有25%概率进入血甲模式
            其余将随机进入血剑-血枪 血刃-双剑 血镰-血剑 血弓-随机 血鞭-血伞模式
            初始5颗速度骰子每两幕增加一颗至多9颗
            除血甲状态，最后一个骰子固定使用血之宝库其余骰子随机填入对应硬血术除大招外书页
            每3幕使用一次当前硬血术大招书页
            如果进入血甲状态则当幕速度骰子变为3颗并使用堂埃尤尔硬血术6式-血甲与两张血液凝结，下幕速度骰子变为5颗并使用硬血化铠×3 血之壁垒×2
            二阶段：开幕清空自身负面状态并恢复所有混乱抗性, 不再切换硬血术状态
            第一幕：速度骰子×5 为仍在饥渴中的家人设下的晚宴 必须担负的责任 硬血截断 血之宝库 血之宝库
            第二幕：速度骰子×7 若能摆脱这可怖的疾病 必须担负的责任 这绝非理想中的共存...旋转!绽放吧!! 凝血化锋 硬血截断
            第三幕：速度骰子×8 必须担负的责任 若能摆脱这可怖的疾病 这绝非理想中的共存...这绝非理想中的共存...血如泉涌 血如泉涌 凝血化锋 血之宝库
            循环
            三阶段：开幕清空自身负面状态并恢复所有混乱抗性并召唤4个不同的凝结的情感
            第一幕：速度骰子×6 冲锋!驽骍难得! 梦之冒险 硬血截断 血如泉涌 血之宝库 血之宝库
            第二幕：速度骰子×5 梦之冒险 梦之冒险 血如泉涌 凝血化锋 旋转!绽放吧!!
            第三幕：速度骰子×5 梦之冒险 硬血截断 硬血截断 血之宝库 血之宝库
            第四幕：速度骰子×2 堂埃尤尔派硬血术终式-La Sangre 便以决斗作为这场战斗的结尾吧
            第五幕：速度骰子×4  4种决斗书页)
        */
        public override void OnRoundStart()
        {
            base.OnRoundStart();
        }
    }
    public class PassiveAbility_DonEyuil_02 : PassiveAbilityBase
    {
        //堂埃尤尔派硬血术
        public override string debugDesc => "自身将定期切换不同的硬血术状态并获得不同效果与书页";
    }
    public class PassiveAbility_DonEyuil_03 : PassiveAbilityBase
    {
        //埃尤尔之血
        public override string debugDesc => "拼点时目标与自身每有3层流血便使自身所有骰子威力+1(至多+3)\r\n命中目标时恢复等同于自身骰子最终值的体力";

        public override void OnStartParrying(BattlePlayingCardDataInUnitModel card)
        {
            card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus()
            {
                power = Math.Min((owner.bufListDetail.GetKewordBufStack(KeywordBuf.Bleeding) + card.target.bufListDetail.GetKewordBufStack(KeywordBuf.Bleeding)) / 3  ,3)
            });
        }
        public override void OnSucceedAreaAttack(BattleDiceBehavior behavior, BattleUnitModel target)
        {
            owner.RecoverHP(behavior.DiceResultValue);
        }
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            if(behavior != null && behavior.card.card.XmlData.Spec.Ranged != LOR_DiceSystem.CardRange.FarAreaEach && behavior.card.card.XmlData.Spec.Ranged != LOR_DiceSystem.CardRange.FarArea)
            {
                owner.RecoverHP(behavior.DiceResultValue);
            }
        }
    }
    public class PassiveAbility_DonEyuil_04 : PassiveAbilityBase
    {
        //光荣的决斗
        public override string debugDesc => "拼点时使双方所有骰子威力+3\r\n拼点胜利的一方造成的伤害增加30%";
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            var diceStat = new DiceStatBonus() { power = 3 };
            if(behavior.TargetDice != null)
            {
                behavior.ApplyDiceStatBonus(diceStat);
                behavior.TargetDice.ApplyDiceStatBonus(diceStat);
            }
        }
        public override void OnWinParrying(BattleDiceBehavior behavior)
        {
            if(behavior != null)
            {
                behavior.ApplyDiceStatBonus(new DiceStatBonus() { dmgRate = 30 });
            }
        }
        public override void OnLoseParrying(BattleDiceBehavior behavior)
        {
            if(behavior != null  && behavior.TargetDice != null)
            {
                behavior.TargetDice.ApplyDiceStatBonus(new DiceStatBonus() { dmgRate = 30 });
            }
        }
    }
    public class PassiveAbility_DonEyuil_05 : PassiveAbilityBase
    {
        //奔向梦想!驽骍难得!
        public override string debugDesc => "使所有应用了觉醒书页的司书威力+1\r\n自身体力首次低于400时获得新的被动并更改行动逻辑\r\n触发前体力无法低于400";
        //（获得被动“萦绕家族的疾病”“无法舍弃的责任”“若家人也能找到梦想”并获得500层护盾）
        public class BattleUnitBuf_RunningTowardDream : BattleUnitBuf
        {
            public override void OnRollDice(BattleDiceBehavior behavior)
            {
                if(behavior != null && _owner.emotionDetail.GetSelectedCardList().Find(x => x.XmlInfo.State == MentalState.Positive) != null)
                {
                    behavior.ApplyDiceStatBonus(new DiceStatBonus() { power = 1 });
                }
            }
        }
    }


}
