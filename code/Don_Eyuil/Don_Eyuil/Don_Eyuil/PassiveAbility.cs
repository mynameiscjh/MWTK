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
using Don_Eyuil.Buff;
using static Don_Eyuil.PassiveAbility_DonEyuil_01;
using static Don_Eyuil.PassiveAbility_DonEyuil_02.HardBloodArtPair;
using static UnityEngine.UI.GridLayoutGroup;
using UI;
using static Don_Eyuil.PassiveAbility_DonEyuil_07;
namespace Don_Eyuil
{
    public class PassiveAbility_DonEyuil_01 : PassiveAbilityBase
    {

        //200年前的回忆
        public override string debugDesc => "每幕开始时移除手中的书页并将意图使用的书页置入手中\r\n自身书页费用为0";
        /*
            每幕开始时移除手中的书页并将意图使用的书页置入手中
            自身书页费用为0
            (一阶段：自第二幕起幕有25%概率进入血甲模式
            其余将随机进入血剑-血枪 血刃-双剑 血镰-血剑 血弓-随机 血鞭-血伞模式
            初始6颗速度骰子每两幕增加一颗至多9颗
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
        public static int Phase = 1;
        public static int Phase2Round = 0;
        public override void OnWaveStart()
        {
            Phase = 1; Phase2Round = 0;
        }
        public override int GetMinHp()
        {
            switch (Phase)
            {
                case 1: case 2: return 400;
                case 3: return 100;
                default:return base.GetMinHp();
            }
        }
        public void AddNewCard(LorId Id, int pro)
        {
            BattleDiceCardModel battleDiceCardModel = this.owner.allyCardDetail.AddTempCard(Id);
            if (battleDiceCardModel != null)
            {
                battleDiceCardModel.SetCostToZero(true);
                battleDiceCardModel.SetPriorityAdder(pro);
                battleDiceCardModel.temporary = true;
            }
        }
        public bool HasSelectPairThisRound = false;
        public override void OnRoundEndTheLast()
        {
            HasSelectPairThisRound = false;
            if(Phase == 1 && owner.hp <= 400)
            {
                Phase = 2;
                if(APassive02 != null && APassive02.CurrentArtPair != null)
                {
                    if(APassive02.CurrentArtPair.ComboType == HardBloodArtCombo.Sheild || APassive02.CurrentArtPair.ComboType == HardBloodArtCombo.Sheild2)
                    {
                        APassive02.SelectHardBloodArt(APassive02.CurrentArtPair, true);
                    }
                    owner.passiveDetail.DestroyPassive(APassive02);
                }
                owner.bufListDetail.GetActivatedBufList().DoIf(x => x.positiveType == BufPositiveType.Negative, y => y.Destroy());
                if (this.owner.turnState == BattleUnitTurnState.BREAK)
                {
                    this.owner.turnState = BattleUnitTurnState.WAIT_CARD;
                }
                this.owner.breakDetail.nextTurnBreak = false;
                this.owner.breakDetail.RecoverBreakLife(1, false);
                this.owner.breakDetail.RecoverBreak(this.owner.breakDetail.GetDefaultBreakGauge());
                BattleUnitBuf_BloodShield.GainBuf<BattleUnitBuf_BloodShield>(owner, 500);
                owner.passiveDetail.AddPassive(MyTools.Create(6));
                owner.passiveDetail.AddPassive(MyTools.Create(7));
                owner.passiveDetail.AddPassive(MyTools.Create(8));
            }
            if(Phase == 2 && BattleUnitBuf_BloodShield.GetBufStack<BattleUnitBuf_BloodShield>(owner)<=0)
            {
                Phase = 3;
                owner.passiveDetail.DestroyPassive(owner.passiveDetail.PassiveList.Find(x => x is PassiveAbility_DonEyuil_06));
                owner.passiveDetail.DestroyPassive(owner.passiveDetail.PassiveList.Find(x => x is PassiveAbility_DonEyuil_07));
                owner.passiveDetail.DestroyPassive(owner.passiveDetail.PassiveList.Find(x => x is PassiveAbility_DonEyuil_08));
                owner.passiveDetail.AddPassive(MyTools.Create(9));
            }
        }
        public override int SpeedDiceNumAdder()
        {
            int EmotionOffest = (owner.emotionDetail.EmotionLevel >= 4) ? -1 : 0;
            if(Phase == 1)
            {
                if (APassive02 != null)
                {
                    if (HasSelectPairThisRound == false)
                    {
                        APassive02.CurrentArtPair = APassive02.SelectHardBloodArt(APassive02.CurrentArtPair);
                        HasSelectPairThisRound = true;
                    }
                    if (APassive02.CurrentArtPair.ComboType == HardBloodArtCombo.Sheild) { return 2 + EmotionOffest; }
                    if (APassive02.CurrentArtPair.ComboType == HardBloodArtCombo.Sheild2) { return 4 + EmotionOffest; }
                    return 5 + Math.Min(4, Singleton<StageController>.Instance.RoundTurn / 2) + EmotionOffest;
                }
            }
            if(Phase == 2)
            {
                switch(Phase2Round)
                {
                    case 1: return 4 + EmotionOffest;
                    case 2: return 6 + EmotionOffest;
                    case 3: return 7 + EmotionOffest;
                }
            }
            return 0;
        }
        public PassiveAbility_DonEyuil_02 APassive02 => owner.passiveDetail.PassiveList.Find(x => x is PassiveAbility_DonEyuil_02) as PassiveAbility_DonEyuil_02;
        public override void OnRoundStart()
        {
            owner.allyCardDetail.ExhaustAllCardsInHand();
            int i = this.owner.Book.GetSpeedDiceRule(this.owner).diceNum - this.owner.Book.GetSpeedDiceRule(this.owner).breakedNum;
            int round = Singleton<StageController>.Instance.RoundTurn;
            if (Phase == 1 && APassive02 != null)
            { 
                //APassive02.CurrentArtPair = APassive02.SelectHardBloodArt(APassive02.CurrentArtPair);
                //Debug.LogError("CurrentArtPair:" + APassive02.CurrentArtPair.ComboType +"|||||||||" + String.Join(",", APassive02.CurrentArtPair.Arts));
                if (APassive02.CurrentArtPair.ComboType == HardBloodArtCombo.Sheild) 
                {
                    AddNewCard(MyId.Card_堂埃尤尔派硬血术6式_血甲_1, 999);
                    AddNewCard(MyId.Card_血液凝结,999);
                    AddNewCard(MyId.Card_血液凝结, 999);
                }
                else if (APassive02.CurrentArtPair.ComboType == HardBloodArtCombo.Sheild2) 
                {
                    AddNewCard(MyId.Card_硬血化铠, 999);
                    AddNewCard(MyId.Card_硬血化铠, 999);
                    AddNewCard(MyId.Card_硬血化铠, 999);
                    AddNewCard(MyId.Card_血之壁垒, 999);
                    AddNewCard(MyId.Card_血之壁垒, 999);
                }
                else
                {
                    if(round % 3 <= 0)
                    {
                        APassive02.CurrentArtPair.GetComboFinalCards().Do(x => {
                            AddNewCard(x, 999);
                            i--;
                        }) ;
                    }
                    while (i - 1 > 0)
                    {
                        AddNewCard(RandomUtil.SelectOne(APassive02.CurrentArtPair.GetComboCards()),500);
                        i--;
                    }
                    AddNewCard(MyId.Card_血之宝库_1, 200);
                    i = 0;
                }
            }
            if(Phase == 2)
            {
                /*二阶段：开幕清空自身负面状态并恢复所有混乱抗性, 不再切换硬血术状态
                    第一幕：速度骰子×5 为仍在饥渴中的家人设下的晚宴 必须担负的责任 硬血截断 血之宝库 血之宝库
                    第二幕：速度骰子×7 若能摆脱这可怖的疾病 必须担负的责任 这绝非理想中的共存...旋转!绽放吧!! 凝血化锋 硬血截断 血之宝库
                    第三幕：速度骰子×8 必须担负的责任 若能摆脱这可怖的疾病 这绝非理想中的共存...这绝非理想中的共存...血如泉涌 血如泉涌 凝血化锋 血之宝库
                */
                if (Phase2Round > 2)//0 1 2 Pass 3 -> 0
                {
                    Phase2Round = 0;
                }
                Phase2Round++;//0 1 2 -> 1 2 3
                switch(Phase2Round)
                {
                    case 1:
                        AddNewCard(MyId.Card_为仍在饥渴中的家人设下的晚宴, 999);
                        AddNewCard(MyId.Card_必须担负的责任, 999);
                        AddNewCard(MyId.Card_硬血截断_1, 999);
                        AddNewCard(MyId.Card_血之宝库_1, 999);
                        AddNewCard(MyId.Card_血之宝库_1, 999);
                    break;
                    case 2:
                        AddNewCard(MyId.Card_若能摆脱这可怖的疾病, 999);
                        AddNewCard(MyId.Card_必须担负的责任, 999);
                        AddNewCard(MyId.Card_这绝非理想中的共存, 999);
                        AddNewCard(MyId.Card_旋转_绽放把_1, 999);
                        AddNewCard(MyId.Card_凝血化锋_2, 999);
                        AddNewCard(MyId.Card_硬血截断_1, 999);
                        AddNewCard(MyId.Card_血之宝库_1, 999);
                    break;
                    case 3:
                        AddNewCard(MyId.Card_必须担负的责任, 999);
                        AddNewCard(MyId.Card_若能摆脱这可怖的疾病, 999);
                        AddNewCard(MyId.Card_这绝非理想中的共存, 999);
                        AddNewCard(MyId.Card_这绝非理想中的共存, 999);
                        AddNewCard(MyId.Card_血如泉涌_1, 999);
                        AddNewCard(MyId.Card_血如泉涌_1, 999);
                        AddNewCard(MyId.Card_凝血化锋_2, 999);
                        AddNewCard(MyId.Card_血之宝库_1, 999);
                    break;
                }

            }
        }
    }
    public class PassiveAbility_DonEyuil_02 : PassiveAbilityBase
    {
        public bool HasEnterBloodSickleArt = false;
        public override void OnWaveStart()
        {
            HasEnterBloodSickleArt = false;
        }
        public class BattleUnitBuf_HardBloodArt : BattleUnitBuf_Don_Eyuil
        {

            public virtual List<LorId> BloodArtCards { get; }
            public virtual List<LorId> BloodArtFinalCard { get; }
            //硬血术生效触发
            public override void AfterGetOrAddBuf()
            {
               
            }
            public BattleUnitBuf_HardBloodArt(BattleUnitModel model) : base(model)
            {
                typeof(BattleUnitBuf).GetField("_iconInit", AccessTools.all).SetValue(this, true);
                stack = 0;
            }
            //血剑:
            //自身斩击骰子最小值+2
            //自身施加"流血"时额外对自身和目标施加一层
            //每幕结束时自身每承受2点"流血"伤害便时自身获得1层"硬血结晶"
            public class BattleUnitBuf_HardBloodArt_BloodSword : BattleUnitBuf_HardBloodArt
            {
                protected override string keywordId => "BattleUnitBuf_Sword";
                public override List<LorId> BloodArtFinalCard => new List<LorId>() {MyId.Card_堂埃尤尔派硬血术1式_血剑_1 };
                public override List<LorId> BloodArtCards => new List<LorId>() { MyId.Card_血剑斩击, MyId.Card_凝血化锋_1, MyId.Card_剑刃截断 };
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
                    if (cardBuf.bufType == KeywordBuf.Bleeding)
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
                    typeof(BattleUnitBuf).GetField("_bufIcon", AccessTools.all).SetValue(this, TKS_BloodFiend_Initializer.ArtWorks["敌方血剑"]);
                }
            }
            //血枪:
            //拼点时自身速度每高于目标1点便使自身所有骰子最大值+1(至多+4)
            //自身以高于目标至少2点的速度击中目标时将对目标施加1层"无法凝结的血"(每幕至多触发3次)
            //若击杀目标则在下一幕获得一层"热血尖枪"
            public class BattleUnitBuf_HardBloodArt_BloodLance : BattleUnitBuf_HardBloodArt
            {
                protected override string keywordId => "BattleUnitBuf_Lance";
                public override List<LorId> BloodArtFinalCard => new List<LorId>() { MyId.Card_堂埃尤尔派硬血术2式_血枪_1 };
                public override List<LorId> BloodArtCards => new List<LorId>() { MyId.Card_高速穿刺, MyId.Card_长枪冲锋 };
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
                protected override string keywordId => "BattleUnitBuf_DoubleSwords";
                public override List<LorId> BloodArtFinalCard => new List<LorId>() { MyId.Card_堂埃尤尔派硬血术5式_双剑_1 };
                public override List<LorId> BloodArtCards => new List<LorId>() { MyId.Card_迅捷剑击 };
                public int BleedingDamageThisRound = 0;
                public override void AfterTakeBleedingDamage(int Dmg)
                {
                    BleedingDamageThisRound += Dmg;
                }
                public override void OnRollDice(BattleDiceBehavior behavior)
                {
                    behavior.ApplyDiceStatBonus(new DiceStatBonus() { dmg = BleedingDamageThisRound / 3 });
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
                protected override string keywordId => "BattleUnitBuf_Blade";
                public override List<LorId> BloodArtFinalCard => new List<LorId>() { MyId.Card_堂埃尤尔派硬血术4式_血刃_1};
                public override List<LorId> BloodArtCards => new List<LorId>() { MyId.Card_血刃割裂, MyId.Card_血刃环切 };

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
                    BloodArtFinalCard.Clear();
                }
                public override void AfterOtherUnitTakeBleedingDamage(BattleUnitModel Unit, int Dmg)
                {
                    if (Unit.faction != _owner.faction)
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
            public class BattleUnitBuf_HardBloodArt_BloodBow : BattleUnitBuf_HardBloodArt
            {
                protected override string keywordId => "BattleUnitBuf_Bow";
                public override List<LorId> BloodArtFinalCard => new List<LorId>() { MyId.Card_堂埃尤尔派硬血术7式_血弓_1 };
                public override List<LorId> BloodArtCards => new List<LorId>() { MyId.Card_穿云血箭, MyId.Card_血箭连射 };

                public override void OnRollDice(BattleDiceBehavior behavior)
                {
                    if (behavior != null && behavior.card != null && behavior.card.card != null && behavior.card.card.XmlData.Spec.Ranged == CardRange.Far)
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
                        if (behavior.card.target != null)
                        {
                            behavior.card.target.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Bleeding, 3);
                            if (HasTriggerOnSuccessAtl == false)
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
                protected override string keywordId => "BattleUnitBuf_Scourge";
                public override List<LorId> BloodArtFinalCard => new List<LorId>() { MyId.Card_堂埃尤尔派硬血术8式_血鞭_1 };
                public override List<LorId> BloodArtCards => new List<LorId>() { MyId.Card_血鞭抽打 };
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
                    if (_owner.currentDiceAction != null && _owner.currentDiceAction.currentBehavior != null && cardBuf.bufType == KeywordBuf.Bleeding)
                    {
                        if (CheckCondition(_owner.currentDiceAction.currentBehavior))
                        {
                            BattleUnitBuf_BloodCrystalThorn.GainBuf<BattleUnitBuf_BloodCrystalThorn>(target, stack, BufReadyType.NextRound);
                        }
                        if (_owner.currentDiceAction.currentBehavior.Detail == BehaviourDetail.Hit)
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
            //血伞
            //自身命中带有流血的目标时造成的混乱伤害增加25%同时使自身获得1层"结晶硬血"
            //下令战斗时若自身至少拥有6层"结晶硬血"则使自身获得一颗反击(突刺4-8)骰子
            public class BattleUnitBuf_HardBloodArt_BloodUmbrella : BattleUnitBuf_HardBloodArt
            {
                protected override string keywordId => "BattleUnitBuf_Umbrella";
                public override List<LorId> BloodArtFinalCard => new List<LorId>() { MyId.Card_堂埃尤尔派硬血术9式_血伞_1 };
                public override List<LorId> BloodArtCards => new List<LorId>() { MyId.Card_血伞挥打_1, MyId.Card_血伞反击};
                public override void OnSuccessAttack(BattleDiceBehavior behavior)
                {
                    if (behavior != null && behavior.card != null && behavior.card.target != null)
                    {
                        if (behavior.card.target.bufListDetail.GetActivatedBufList().Exists(x => x.bufType == KeywordBuf.Bleeding))
                        {
                            behavior.ApplyDiceStatBonus(new DiceStatBonus() { breakRate = 25 });
                            BattleUnitBuf_HardBlood_Crystal.GainBuf<BattleUnitBuf_HardBlood_Crystal>(_owner, 1);
                        }
                    }
                }
                public override void OnStartBattle()
                {
                    if (BattleUnitBuf_HardBlood_Crystal.GetBufStack<BattleUnitBuf_HardBlood_Crystal>(_owner) >= 6)
                    {
                        DiceCardXmlInfo cardItem = ItemXmlDataList.instance.GetCardItem(MyId.Card_经典反击书页, false);
                        List<BattleDiceBehavior> list = new List<BattleDiceBehavior>();
                        BattleDiceBehavior battleDiceBehavior = new BattleDiceBehavior();
                        battleDiceBehavior.behaviourInCard = cardItem.DiceBehaviourList[1].Copy();
                        battleDiceBehavior.SetIndex(1);
                        list.Add(battleDiceBehavior);
                        this._owner.cardSlotDetail.keepCard.AddBehaviours(cardItem, list);
                    }

                }

                public BattleUnitBuf_HardBloodArt_BloodUmbrella(BattleUnitModel model) : base(model)
                {
                    typeof(BattleUnitBuf).GetField("_bufIcon", AccessTools.all).SetValue(this, TKS_BloodFiend_Initializer.ArtWorks["敌方血伞"]);
                }
            }
            //血镰
            //目标每有2层流血便使自身斩击骰子造成的伤害增加10%(至多50%)
            //首次进入时使自身获得1层"汹涌的血潮"
            public class BattleUnitBuf_HardBloodArt_BloodSickle : BattleUnitBuf_HardBloodArt
            {
                protected override string keywordId => "BattleUnitBuf_Sickle";
                public override List<LorId> BloodArtFinalCard => new List<LorId>() { MyId.Card_堂埃尤尔派硬血术3式_血镰_1};
                public override List<LorId> BloodArtCards => new List<LorId>() { MyId.Card_镰刃截断, MyId.Card_巨镰纵切 };

                public override void AfterGetOrAddBuf()
                {
                    var Passive02 =  _owner.passiveDetail.PassiveList.Find(x => x is PassiveAbility_DonEyuil_02) as PassiveAbility_DonEyuil_02;
                    if(Passive02.HasEnterBloodSickleArt == false)
                    {
                        Passive02.HasEnterBloodSickleArt = true;
                        BattleUnitBuf_BloodTide.GainBuf<BattleUnitBuf_BloodTide>(_owner, 1);
                    }
                }
                public override void OnRollDice(BattleDiceBehavior behavior)
                {
                    if (behavior != null && behavior.card != null && behavior.card.target != null && behavior.Detail == BehaviourDetail.Slash)
                    {
                        behavior.ApplyDiceStatBonus(new DiceStatBonus()
                        {
                            dmgRate = 10 * Math.Min(5, (behavior.card.target.bufListDetail.GetKewordBufStack(KeywordBuf.Bleeding) / 2))
                        });
                    }
                }
                public BattleUnitBuf_HardBloodArt_BloodSickle(BattleUnitModel model) : base(model)
                {
                    typeof(BattleUnitBuf).GetField("_bufIcon", AccessTools.all).SetValue(this, TKS_BloodFiend_Initializer.ArtWorks["敌方血镰"]);
                }
            }
            //血甲
            //自身护盾减少时将视作受到等量"流血"伤害
            //每幕结束时自身每有一颗未被使用的防御型骰子便使自身获得10点护盾
            //自身被命中时对命中者施加2-3层"流血"
            public class BattleUnitBuf_HardBloodArt_BloodShield : BattleUnitBuf_HardBloodArt
            {
                protected override string keywordId => "BattleUnitBuf_Armour";
                public override void OnRoundEnd()
                {
                    foreach (DiceBehaviour diceBehaviour in this._owner.cardSlotDetail.keepCard.GetDiceBehaviourXmlList())
                    {
                        if (diceBehaviour.Type == BehaviourType.Def)
                        {
                            BattleUnitBuf_BloodShield.GainBuf<BattleUnitBuf_BloodShield>(_owner, 10);
                        }
                    }
                }
                public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
                {
                    if(atkDice != null && atkDice.card != null && atkDice.card.owner != null)
                    {
                        atkDice.card.owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Bleeding, RandomUtil.Range(2, 3));
                    }
                }
                public BattleUnitBuf_HardBloodArt_BloodShield(BattleUnitModel model) : base(model)
                {
                    typeof(BattleUnitBuf).GetField("_bufIcon", AccessTools.all).SetValue(this, TKS_BloodFiend_Initializer.ArtWorks["敌方血甲"]);
                }
            }
        }
        //堂埃尤尔派硬血术

        public override string debugDesc => "自身将定期切换不同的硬血术状态并获得不同效果与书页 命中时对目标施加2层流血";
        public override void OnSucceedAreaAttack(BattleDiceBehavior behavior, BattleUnitModel target)
        {
            target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, 2, owner);
        }
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            if(behavior != null && behavior.card !=null && behavior.card.target != null)
            {
                behavior.card.target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding,2,owner);
            }
        }

        public HardBloodArtPair CurrentArtPair;
        public HardBloodArtPair SelectHardBloodArt(HardBloodArtPair LatestArtPair,bool WithOutShield = false)
        {
            BattleUnitBuf_HardBloodArt RandomHardBloodArt()
            {
                switch (RandomUtil.SelectOne(new List<string>() { "S", "L", "SS","SC","U","SI" }))
                {
                    case "S":
                        return BattleUnitBuf_HardBloodArt.GetOrAddBuf<BattleUnitBuf_HardBloodArt.BattleUnitBuf_HardBloodArt_BloodSword>(owner);
                    case "L":
                        return BattleUnitBuf_HardBloodArt.GetOrAddBuf<BattleUnitBuf_HardBloodArt.BattleUnitBuf_HardBloodArt_BloodLance>(owner);
                    case "SS":
                        return BattleUnitBuf_HardBloodArt.GetOrAddBuf<BattleUnitBuf_HardBloodArt.BattleUnitBuf_HardBloodArt_DoubleSwords>(owner);
                    case "SC":
                        return BattleUnitBuf_HardBloodArt.GetOrAddBuf<BattleUnitBuf_HardBloodArt.BattleUnitBuf_HardBloodArt_BloodScourge>(owner);
                    case "U":
                        return BattleUnitBuf_HardBloodArt.GetOrAddBuf<BattleUnitBuf_HardBloodArt.BattleUnitBuf_HardBloodArt_BloodUmbrella>(owner);
                    case "SI":
                        return BattleUnitBuf_HardBloodArt.GetOrAddBuf<BattleUnitBuf_HardBloodArt.BattleUnitBuf_HardBloodArt_BloodSickle>(owner);
                    default:
                        return null;
                }
                //Type BloodArt = RandomUtil.SelectOne(typeof(BattleUnitBuf_HardBloodArt.BattleUnitBuf_HardBloodArt_BloodSword), typeof(BattleUnitBuf_HardBloodArt.BattleUnitBuf_HardBloodArt_BloodLance), typeof(BattleUnitBuf_HardBloodArt.BattleUnitBuf_HardBloodArt_DoubleSwords), typeof(BattleUnitBuf_HardBloodArt.BattleUnitBuf_HardBloodArt_BloodBlade), typeof(BattleUnitBuf_HardBloodArt.BattleUnitBuf_HardBloodArt_BloodScourge), typeof(BattleUnitBuf_HardBloodArt.BattleUnitBuf_HardBloodArt_BloodUmbrella), typeof(BattleUnitBuf_HardBloodArt.BattleUnitBuf_HardBloodArt_BloodSickle));
                //return AccessTools.Method(typeof(BattleUnitBuf_HardBloodArt), "GetOrAddBuf").MakeGenericMethod(BloodArt).Invoke(null, new object[] { owner }) as BattleUnitBuf_HardBloodArt;
            }
            if(LatestArtPair != null)
            {
                var ExpiredArtPair = LatestArtPair.Expire();
                if (ExpiredArtPair != null) { return ExpiredArtPair; }
            }
            owner.bufListDetail.GetActivatedBufList().FindAll(x => x is BattleUnitBuf_HardBloodArt).Do(x => x.Destroy());
            if (WithOutShield == false && Singleton<StageController>.Instance.RoundTurn >= 2 && RandomUtil.valueForProb < 1f )
            {
                return new HardBloodArtPair(HardBloodArtPair.HardBloodArtCombo.Sheild, BattleUnitBuf_HardBloodArt.GetOrAddBuf<BattleUnitBuf_HardBloodArt.BattleUnitBuf_HardBloodArt_BloodShield>(owner));
            }
            switch(RandomUtil.SelectOne(new List<HardBloodArtCombo>() { HardBloodArtCombo.Sword_Lance,HardBloodArtCombo.Kinfe_Double,HardBloodArtCombo.Sickle_Sword,HardBloodArtCombo.Bow_,HardBloodArtCombo.Scourage_Umbre}))
            {
                case HardBloodArtCombo.Sword_Lance:
                    return new HardBloodArtPair(HardBloodArtPair.HardBloodArtCombo.Sword_Lance, BattleUnitBuf_HardBloodArt.GetOrAddBuf<BattleUnitBuf_HardBloodArt.BattleUnitBuf_HardBloodArt_BloodSword>(owner), BattleUnitBuf_HardBloodArt.GetOrAddBuf<BattleUnitBuf_HardBloodArt.BattleUnitBuf_HardBloodArt_BloodLance>(owner));
                case HardBloodArtCombo.Kinfe_Double:
                    return new HardBloodArtPair(HardBloodArtPair.HardBloodArtCombo.Kinfe_Double, BattleUnitBuf_HardBloodArt.GetOrAddBuf<BattleUnitBuf_HardBloodArt.BattleUnitBuf_HardBloodArt_BloodBlade>(owner), BattleUnitBuf_HardBloodArt.GetOrAddBuf<BattleUnitBuf_HardBloodArt.BattleUnitBuf_HardBloodArt_DoubleSwords>(owner));
                case HardBloodArtCombo.Sickle_Sword:
                    return new HardBloodArtPair(HardBloodArtPair.HardBloodArtCombo.Sickle_Sword, BattleUnitBuf_HardBloodArt.GetOrAddBuf<BattleUnitBuf_HardBloodArt.BattleUnitBuf_HardBloodArt_BloodSickle>(owner), BattleUnitBuf_HardBloodArt.GetOrAddBuf<BattleUnitBuf_HardBloodArt.BattleUnitBuf_HardBloodArt_BloodSword>(owner));
                case HardBloodArtCombo.Bow_:
                    return new HardBloodArtPair(HardBloodArtPair.HardBloodArtCombo.Bow_, BattleUnitBuf_HardBloodArt.GetOrAddBuf<BattleUnitBuf_HardBloodArt.BattleUnitBuf_HardBloodArt_BloodBow>(owner),RandomHardBloodArt());
                case HardBloodArtCombo.Scourage_Umbre:
                    return new HardBloodArtPair(HardBloodArtPair.HardBloodArtCombo.Scourage_Umbre, BattleUnitBuf_HardBloodArt.GetOrAddBuf<BattleUnitBuf_HardBloodArt.BattleUnitBuf_HardBloodArt_BloodScourge>(owner), BattleUnitBuf_HardBloodArt.GetOrAddBuf<BattleUnitBuf_HardBloodArt.BattleUnitBuf_HardBloodArt_BloodUmbrella>(owner));
                default:
                    return null;
            }
        }
        public class HardBloodArtPair
        {
            public enum HardBloodArtCombo
            {
                Sword_Lance,
                Kinfe_Double,
                Sickle_Sword,
                Bow_,
                Scourage_Umbre,
                Sheild,
                Sheild2
            }
            public HardBloodArtCombo ComboType;
            public List<PassiveAbility_DonEyuil_02.BattleUnitBuf_HardBloodArt> Arts = new List<PassiveAbility_DonEyuil_02.BattleUnitBuf_HardBloodArt>() { };

            public HardBloodArtPair Expire()
            {
                if(ComboType == HardBloodArtCombo.Sheild)
                {
                    ComboType = HardBloodArtCombo.Sheild2;
                    return this;
                }
                return null;
            }
            public List<LorId> GetComboCards()
            {
                var CardList = new List<LorId>() { };
                Arts.Do(x => CardList.AddRange(x.BloodArtCards));
                return CardList;
            }
            public List<LorId> GetComboFinalCards()
            {
                var CardList = new List<LorId>() { };
                Arts.Do(x => CardList.AddRange(x.BloodArtFinalCard));
                return CardList;
            }
            public HardBloodArtPair(HardBloodArtCombo Combo,params BattleUnitBuf_HardBloodArt[] ArtBufs)
            {
                ComboType = Combo;
                Arts = ArtBufs.ToList();
            }
        }
    }
    public class PassiveAbility_DonEyuil_03 : PassiveAbilityBase
    {
        //埃尤尔之血
        public override string debugDesc => "拼点时目标与自身每有2层流血便使自身所有骰子威力+1(至多+3)\r\n命中目标时恢复等同于自身骰子最终值的体力";

        public override void OnStartParrying(BattlePlayingCardDataInUnitModel card)
        {
            card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus()
            {
                power = Math.Min((owner.bufListDetail.GetKewordBufStack(KeywordBuf.Bleeding) + card.target.bufListDetail.GetKewordBufStack(KeywordBuf.Bleeding)) / 2  ,3)
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

        public override void OnWaveStart()
        {
            BattleObjectManager.instance.GetAliveList(Faction.Player).Do(x => x.bufListDetail.AddBuf(new BattleUnitBuf_RunningTowardDream()));
        }
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
    public class PassiveAbility_DonEyuil_06 : PassiveAbilityBase
    {
        //萦绕家族的疾病
        public override string debugDesc => "使所有敌方角色获得\"席卷而来的饥饿\"";
        public override void OnCreated()
        {
            BattleObjectManager.instance.GetAliveList_opponent(owner.faction).Do(x => BattleUnitBuf_FloodOfHunger.GetOrAddBuf<BattleUnitBuf_FloodOfHunger>(x));
            this.name = TKS_BloodFiend_Initializer.GetPassiveName(6);
            this.desc = TKS_BloodFiend_Initializer.GetPassiveDesc(6);
        }
    }
    public class PassiveAbility_DonEyuil_07 : PassiveAbilityBase
    {
        //若家人也能找到梦想
        public override string debugDesc => "拥有觉醒书页的角色获得的正面情感翻倍\r\n场上角色解除\"席卷而来的饥饿\"时将永久获得1层\"强壮\"与3层\"振奋\"";
        public override void OnCreated()
        {
            BattleObjectManager.instance.GetAliveList_opponent(owner.faction).Do(x => BattleUnitBuf_FloodOfHunger.GetOrAddBuf<BattleUnitBuf_MayYouFindDream>(x));
            this.name = TKS_BloodFiend_Initializer.GetPassiveName(7);
            this.desc = TKS_BloodFiend_Initializer.GetPassiveDesc(7);
        }
        public override void OnDestroyed()
        {
            BattleUnitBuf_MayYouFindDream.RemoveBuf<BattleUnitBuf_MayYouFindDream>(owner);
        }

        public class BattleUnitBuf_MayYouFindDream : BattleUnitBuf_Don_Eyuil
        {
            public BattleUnitBuf_MayYouFindDream(BattleUnitModel model):base(model) { }
            public override void BeforeAddEmotionCoin(EmotionCoinType CoinType, ref int Count)
            {
                if (CoinType == EmotionCoinType.Positive && _owner.emotionDetail.GetSelectedCardList().Find(x => x.XmlInfo.State == MentalState.Positive) != null)
                {
                    Count *= 2;
                }
            }
        }
    }
    public class PassiveAbility_DonEyuil_08 : PassiveAbilityBase
    {
        //无法舍弃的责任
        public override string debugDesc => "每幕开始时获得等同于场上拥有\"席卷而来的饥饿\"角色数的\"血铠\"\r\n若场上仍有角色处于\"席卷而来的饥饿\"状态则自身无法陷入混乱\r\n护盾消耗完后一幕移除本阶段被动并获得新的被动于行动逻辑\r\n体力无法低于400";
        public override void OnCreated()
        {
            this.name = TKS_BloodFiend_Initializer.GetPassiveName(8);
            this.desc = TKS_BloodFiend_Initializer.GetPassiveDesc(8);
        }
        public override void OnRoundStartAfter()
        {
            BattleUnitBuf_BloodArmor.GainBuf<BattleUnitBuf_BloodArmor>(owner, BattleUnitBuf_FloodOfHunger.GetAllUnitWithBuf<BattleUnitBuf_FloodOfHunger>().Count);
        }
        public override bool isStraighten => BattleUnitBuf_FloodOfHunger.GetAllUnitWithBuf<BattleUnitBuf_FloodOfHunger>().Count > 0; 

    }

}
