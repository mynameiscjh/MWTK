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
using static Don_Eyuil.BattleUnitBuf_Don_Eyuil;
using LOR_BattleUnit_UI;
using static Don_Eyuil.San_Sora.PassiveAbility_SanSora_06;
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
        public static int Phase1Round = 0;
        public static int Phase2Round = 0;
        public static int Phase3Round = 0;

        public int AddedBleedingCount = 0;
        public override void OnWaveStart()
        {
            Phase = 1; Phase1Round = 0; Phase2Round = 0; Phase3Round = 0; AddedBleedingCount = 0;
        }
        public override int GetMinHp()
        {
            switch (Phase)
            {
                case 1: return 500;
                case 2: if (AddedBleedingCount >= 100) { return base.GetMinHp(); } return 300;
                case 3: return 10;
                default: return base.GetMinHp();
            }
        }
        public override bool isStraighten => Phase == 3;
        public override bool isTargetable => !(Phase == 3 && Phase3Round >= 4);
        public override int GetSpeedDiceAdder(int speedDiceResult)
        {
            return (Phase == 3 && Phase3Round >= 4)? 99: base.GetSpeedDiceAdder(speedDiceResult);
        }
        public override int SpeedDiceNumAdder()
        {
            int EmotionOffest = (owner.emotionDetail.EmotionLevel >= 4) ? -1 : 0;
            if(Phase == 1)
            {
                if(Phase1Round == 3) { return 0 + EmotionOffest; }
                if(Phase1Round == 4) { return 3 + EmotionOffest; }
                return 3 + Math.Min(2, Singleton<StageController>.Instance.RoundTurn - 1) + EmotionOffest;
            }
            else if(Phase == 2)
            {
                switch(Phase2Round)
                {
                    case 1:return 6 + EmotionOffest;
                    case 2: return 5 + EmotionOffest;
                    case 3: return 5 + EmotionOffest;
                    case 4: return 3 + EmotionOffest;
                }
            }
            else if(Phase == 3)
            {
                return 6;
            }
            return base.SpeedDiceNumAdder();
        }
        public override void OnAfterRollSpeedDice()
        {
            List<string> GetHardBloodBufId()
            {
                var IDList = new List<string>() { "Blade", "Bow", "DoubleSwords", "Lance", "Scourge", "Sickle", "Sword" };
                if (Phase1Round == 3 || Phase1Round == 4) { return new List<string>() { "Armour",null }; }
                return MyTools.TKSRandomUtil(IDList, 2, false, false);
            }
            void AddDiceHardBloodBuf(SpeedDiceUI dice,string BufId)
            {
                switch (BufId)
                {
                    case "Blade":BattleUnitBuf_Blade.GainBuf<BattleUnitBuf_Blade>(dice);return;
                    case "Bow": BattleUnitBuf_Bow.GainBuf<BattleUnitBuf_Bow>(dice); return;
                    case "DoubleSwords": BattleUnitBuf_DoubleSwords.GainBuf<BattleUnitBuf_DoubleSwords>(dice); return;
                    case "Lance": BattleUnitBuf_Lance.GainBuf<BattleUnitBuf_Lance>(dice); return;
                    case "Scourge": BattleUnitBuf_Scourge.GainBuf<BattleUnitBuf_Scourge>(dice); return;
                    case "Sickle": BattleUnitBuf_Sickle.GainBuf<BattleUnitBuf_Sickle>(dice); return;
                    case "Sword": BattleUnitBuf_Sword.GainBuf<BattleUnitBuf_Sword>(dice); return;
                    case "Armour": BattleUnitBuf_Armour.GainBuf<BattleUnitBuf_Armour>(dice); return;
                }
            }
            var SpeedDices = owner.view.speedDiceSetterUI.GetFieldValue<List<SpeedDiceUI>>("_speedDices");
            var HardBloodList =  GetHardBloodBufId();
            //一半/一半的施加硬血术 如果有奇数就随机加
            SpeedDices.DivisibleSkip((SpeedDices.Count / 2) * 2).SplitDivisibleIEnumerable().Do((Dice1) => 
            {
                Dice1?.Do(x => AddDiceHardBloodBuf(x, RandomUtil.SelectOne(HardBloodList)));
            },(Dice2) =>
            {
                Dice2.DivisibleSkipWhile((x, index) => index <= SpeedDices.Count / 2).Do(x =>
                {
                    AddDiceHardBloodBuf(x.Item2, HardBloodList[0]);
                    AddDiceHardBloodBuf(x.Item1, HardBloodList.Count > 1 ? HardBloodList[0] : HardBloodList[1]);
                });
            });

        }
        public override void OnRoundEndTheLast()
        {
            if(Phase == 1 && BattleUnitBuf_BloodShield.GetBufStack<BattleUnitBuf_BloodShield>(owner) <= 0)
            {
                Phase = 2;
                owner.passiveDetail.AddPassive(MyTools.Create(23));
                owner.passiveDetail.AddPassive(MyTools.Create(24));
                owner.bufListDetail.GetActivatedBufList().DoIf(x => x.positiveType == BufPositiveType.Negative, y => y.Destroy());
                if (this.owner.turnState == BattleUnitTurnState.BREAK)
                {
                    this.owner.turnState = BattleUnitTurnState.WAIT_CARD;
                }
                this.owner.breakDetail.nextTurnBreak = false;
                this.owner.breakDetail.RecoverBreakLife(1, false);
                this.owner.breakDetail.RecoverBreak(this.owner.breakDetail.GetDefaultBreakGauge());
                AddedBleedingCount = 0;
            }
            if (Phase == 2 && AddedBleedingCount >= 100)
            {
                Phase = 3;
                owner.bufListDetail.GetActivatedBufList().DoIf(x => x.positiveType == BufPositiveType.Negative, y => y.Destroy());
                if (this.owner.turnState == BattleUnitTurnState.BREAK)
                {
                    this.owner.turnState = BattleUnitTurnState.WAIT_CARD;
                }
                this.owner.breakDetail.nextTurnBreak = false;
                this.owner.breakDetail.RecoverBreakLife(1, false);
                this.owner.breakDetail.RecoverBreak(this.owner.breakDetail.GetDefaultBreakGauge());
                owner.passiveDetail.DestroyPassive(owner.passiveDetail.PassiveList.Find(x => x is PassiveAbility_SanSora_06));
                owner.passiveDetail.DestroyPassive(owner.passiveDetail.PassiveList.Find(x => x is PassiveAbility_SanSora_07));
                owner.passiveDetail.AddPassive(MyTools.Create(25));
                owner.passiveDetail.AddPassive(MyTools.Create(26));
                AddedBleedingCount = 0;
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
        public override void OnRoundStart()
        {
            owner.allyCardDetail.ExhaustAllCardsInHand();
            int i = this.owner.Book.GetSpeedDiceRule(this.owner).diceNum - this.owner.Book.GetSpeedDiceRule(this.owner).breakedNum;
            if(Phase == 1)
            {
                //第一幕: 摧垮 致伤 致伤 释血化刃 剩余的速度骰子随机填入摧垮 致伤 释血化刃
                //第二幕:利血贯穿 伴血猛袭 伴血猛袭 冲锋截断 摧垮 摧垮 剩余的速度骰子随机填入摧垮 致伤 释血化刃
                //第三幕: 速度骰子变为1颗固定为血甲并固定使用桑空派变体硬血术6式 - 血甲
                //第四幕：速度骰子变为4颗且所有骰子为血甲固定使用硬血为铠
                //循环
                if (Phase1Round > 3)//0 1 2 3 Pass 4 -> 0
                {
                    Phase1Round = 0;
                }
                Phase1Round++;//0 1 2 3 -> 1 2 3 4
                switch(Phase1Round)
                {
                    case 1:
                        AddNewCard(MyId.Card_摧垮, 999);
                        AddNewCard(MyId.Card_致伤, 999);
                        AddNewCard(MyId.Card_致伤, 999);
                        AddNewCard(MyId.Card_释血化刃, 999);
                        i -= 4;
                        break;
                    case 2:
                        AddNewCard(MyId.Card_利血贯穿, 999);
                        AddNewCard(MyId.Card_伴血猛袭, 999);
                        AddNewCard(MyId.Card_伴血猛袭, 999);
                        AddNewCard(MyId.Card_冲锋截断, 999);
                        AddNewCard(MyId.Card_摧垮, 999);
                        AddNewCard(MyId.Card_摧垮, 999);
                        i -= 6;
                        break;
                    case 3:
                        AddNewCard(MyId.Card_桑空派变体硬血术6式_血甲, 999);
                        i = -1;
                        break;
                    case 4:
                        AddNewCard(MyId.Card_硬血为铠, 999);
                        AddNewCard(MyId.Card_硬血为铠, 999);
                        AddNewCard(MyId.Card_硬血为铠, 999);
                        AddNewCard(MyId.Card_硬血为铠, 999);
                        i = -1;
                        break;
                }
                while (i > 0)
                {
                    AddNewCard(RandomUtil.SelectOne(new List<LorId>() { MyId.Card_摧垮, MyId.Card_致伤, MyId.Card_释血化刃 }), 500);
                    i--;
                }
            }
            else if (Phase == 2)
            {
                //第一幕:速度骰子变为7颗 受苦的亲族正在增多 利血贯穿 利血贯穿 伴血猛袭 冲锋截断 血刃剔除 剩余的速度骰子随机填入伴血猛袭与摧垮
                //第二幕：速度骰子变为6颗 利血贯穿 冲锋截断 血刃剔除 血刃剔除 释血化刃 释血化刃
                //第三幕：速度骰子变为6颗 冲锋截断 冲锋截断 伴血猛袭 剩余的速度骰子随机填入伴血猛袭与摧垮
                //第四幕:速度骰子变为4颗 若能摆脱这可怖的疾病...血刃剔除 血刃剔除 血刃剔除
                //循环
                if (Phase2Round > 3)//0 1 2 3 Pass 4 -> 0
                {
                    Phase2Round = 0;
                }
                Phase2Round++;//0 1 2 3 -> 1 2 3 4
                switch (Phase2Round)
                {
                    case 1:
                        AddNewCard(MyId.Card_受苦的亲族正在增多, 999);
                        AddNewCard(MyId.Card_利血贯穿, 999);
                        AddNewCard(MyId.Card_利血贯穿, 999);
                        AddNewCard(MyId.Card_伴血猛袭, 999);
                        AddNewCard(MyId.Card_冲锋截断, 999);
                        AddNewCard(MyId.Card_血刃剔除, 999);
                        i -= 6;
                        break;
                    case 2:
                        AddNewCard(MyId.Card_利血贯穿, 999);
                        AddNewCard(MyId.Card_冲锋截断, 999);
                        AddNewCard(MyId.Card_血刃剔除, 999);
                        AddNewCard(MyId.Card_血刃剔除, 999);
                        AddNewCard(MyId.Card_释血化刃, 999);
                        AddNewCard(MyId.Card_释血化刃, 999);
                        i = -1;
                        break;
                    case 3:
                        AddNewCard(MyId.Card_冲锋截断, 999);
                        AddNewCard(MyId.Card_冲锋截断, 999);
                        AddNewCard(MyId.Card_伴血猛袭, 999);
                        i -= 3;
                        break;
                    case 4:
                        AddNewCard(MyId.Card_若能摆脱这可怖的疾病, 999);
                        AddNewCard(MyId.Card_血刃剔除, 999);
                        AddNewCard(MyId.Card_血刃剔除, 999);
                        AddNewCard(MyId.Card_血刃剔除, 999);
                        i = -1;
                        break;
                }
                while (i > 0)
                {
                    AddNewCard(RandomUtil.SelectOne(new List<LorId>() { MyId.Card_摧垮, MyId.Card_伴血猛袭}), 500);
                    i--;
                }
            }
            else if (Phase == 3)
            {
                //每幕固定使用1-2张翱翔向梦其余骰子随机填入血刃剔除 伴血猛袭 冲锋截断 释血化刃 利血贯穿
                //第二幕固定使用一张桑空派变体硬血术终式 - La Sangre
                //第四幕速度骰子固定为1颗速度改为99且无法选中 使用 直到触及到那梦想!后司书接待胜利)*/
                if (Phase3Round > 3)//0 1 2 3 Pass 4 -> 0
                {
                    Phase3Round = 0;
                }
                Phase3Round++;//0 1 2 3 -> 1 2 3 4
                if(Phase3Round == 2)
                {
                    AddNewCard(MyId.Card_桑空派变体硬血术终式_La_Sangre, 999);
                }
                if (Phase3Round == 4)
                {
                    AddNewCard(MyId.Card_直到触及到那梦想, 999);
                    return;
                }
                MyTools.TKSRandomUtil(new List<LorId>() { MyId.Card_翱翔向梦, MyId.Card_翱翔向梦 }, RandomUtil.Range(1, 2), true, false).Do(x =>
                {
                    AddNewCard(x, 900);
                    i--;
                });
                while (i > 0)
                {
                    AddNewCard(RandomUtil.SelectOne(new List<LorId>() { MyId.Card_血刃剔除, MyId.Card_伴血猛袭, MyId.Card_冲锋截断, MyId.Card_释血化刃, MyId.Card_利血贯穿 }), 500);
                    i--;
                }
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
    public class PassiveAbility_SanSora_05 : PassiveAbilityBase
    {
        //正在进行的冒险与重燃的热血
        public override string debugDesc => "自身每拥有50点护盾便使自身受到的伤害与混乱伤害减少5%(至多30%)\r\n每幕开始时获得(自身失去护盾量/25)层”血羽”";
        public class BattleUnitBuf_SanSoraDmgRedu : BattleUnitBuf_Don_Eyuil
        {
            public int LostShieldCount = 0;
            public BattleUnitBuf_SanSoraDmgRedu(BattleUnitModel model) : base(model)
            {
                this.stack = 0;
            }
            public override int GetBreakDamageReductionRate()
            {
                return Math.Min(5 * (BattleUnitBuf_BloodShield.GetBufStack<BattleUnitBuf_BloodShield>(owner) / 50),30);
            }
            public override int GetDamageReductionRate()
            {
                return Math.Min(5 * (BattleUnitBuf_BloodShield.GetBufStack<BattleUnitBuf_BloodShield>(owner) / 50), 30);
            }
            public override void OnGainEyuilBufStack(BattleUnitBuf_Don_Eyuil Buff, ref int stack)
            {
                if(Buff is BattleUnitBuf_BloodShield && stack < 0)
                {
                    LostShieldCount += Math.Abs(stack);
                }
            }
            public override void OnRoundStartAfter()
            {
                BattleUnitBuf_Feather.GainBuf<BattleUnitBuf_Feather>(owner, LostShieldCount / 25);
            }
        }
        public override void OnCreated()
        {
            BattleUnitBuf_SanSoraDmgRedu.GetOrAddBuf<BattleUnitBuf_SanSoraDmgRedu>(owner);
        }
        public override void OnDestroyed()
        {
            BattleUnitBuf_SanSoraDmgRedu.RemoveBuf<BattleUnitBuf_SanSoraDmgRedu>(owner);
        }
    }
    public class PassiveAbility_SanSora_06 : PassiveAbilityBase
    {
        //亲族们仍在苦难之中!
        public override string debugDesc => "自身拼点失败时将对自身施加2层”流血”\r\n自身累计施加100层”流血”前体力无法低于300\r\n条件达成后下一幕改变行动逻辑并获得新的被动";
        public override void OnLoseParrying(BattleDiceBehavior behavior)
        {
            owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Bleeding, 2);
        }
        public class BattleUnitBuf_SanSoraBleedingCount : BattleUnitBuf_Don_Eyuil
        {
            public BattleUnitBuf_SanSoraBleedingCount(BattleUnitModel model) : base(model)
            {
                this.stack = 0;
            }
            public override void AfterAddKeywordBuf(BattleUnitModel Adder, KeywordBuf BufType, ref int Stack)
            {
                PassiveAbility_SanSora_01 Passive = null;
                if (BufType == KeywordBuf.Bleeding && (Passive = Adder.passiveDetail.PassiveList.Find(x => x is PassiveAbility_SanSora_01) as PassiveAbility_SanSora_01) != null)
                {
                    Passive.AddedBleedingCount += Stack;
                }
            }
        }
        public override void OnCreated()
        {
            BattleObjectManager.instance.GetAliveList().Do(x => BattleUnitBuf_SanSoraBleedingCount.GetOrAddBuf<BattleUnitBuf_SanSoraBleedingCount>(x));
            this.name = TKS_BloodFiend_Initializer.GetPassiveName(23);
            this.desc = TKS_BloodFiend_Initializer.GetPassiveDesc(23);
        }
        public override void OnDestroyed()
        {
            BattleObjectManager.instance.GetAliveList().Do(x => BattleUnitBuf_SanSoraBleedingCount.RemoveBuf<BattleUnitBuf_SanSoraBleedingCount>(x));
        }
    }
    public class PassiveAbility_SanSora_07 : PassiveAbilityBase
    {
        //若能实现那理想中的共存
        public override string debugDesc => "敌方角色死亡时将在这一幕中对自身施加10层”流血”与2层”虚弱”\r\n自身所有骰子与招架型外的骰子拼点时威力+2";
        public override void OnDieOtherUnit(BattleUnitModel unit)
        {
            if(unit.faction == Faction.Enemy)
            {
                owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Bleeding, 10);
                owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Weak, 2);
            }
        }
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            if(behavior != null && behavior.TargetDice != null && behavior.TargetDice.Detail != BehaviourDetail.Guard)
            {
                behavior.ApplyDiceStatBonus(new DiceStatBonus() { power = 2 });
            }
        }
        public override void OnCreated()
        {
            this.name = TKS_BloodFiend_Initializer.GetPassiveName(24);
            this.desc = TKS_BloodFiend_Initializer.GetPassiveDesc(24);
        }
    }
    public class PassiveAbility_SanSora_08 : PassiveAbilityBase
    {
        //激荡的心情与永不终结之梦
        public override string debugDesc => "自身拼点失败使改为获得正面情感\r\n每幕中场上所有角色每获得10点正面情感便使其下幕所有骰子威力+1";
        public class BattleUnitBuf_SanSoraDmgRedu : BattleUnitBuf_Don_Eyuil
        {
            public int GetEmotionCoinCount = 0;
            public BattleUnitBuf_SanSoraDmgRedu(BattleUnitModel model) : base(model)
            {
                this.stack = 0;
            }
            public class BattleUnitBuf_SanSoraPowerUp : BattleUnitBuf_Don_Eyuil
            {
                public BattleUnitBuf_SanSoraPowerUp(BattleUnitModel model) : base(model)
                {
                    this.stack = 0;
                }
                public override void BeforeRollDice(BattleDiceBehavior behavior)
                {
                    behavior.ApplyDiceStatBonus(new DiceStatBonus() { power = stack });
                }

                public override void OnRoundEnd()
                {
                    this.Destroy();
                }
            }
            public override void OnRoundEnd()
            {
                BattleUnitBuf_SanSoraPowerUp.GainBuf<BattleUnitBuf_SanSoraPowerUp>(_owner,GetEmotionCoinCount / 10, BufReadyType.NextRound);
                GetEmotionCoinCount = 0;
            }
            public override void BeforeAddEmotionCoin(EmotionCoinType CoinType, ref int Count)
            {
                GetEmotionCoinCount += CoinType == EmotionCoinType.Positive ? Count : 0;
            }
        }
        public override void OnCreated()
        {
            BattleObjectManager.instance.GetAliveList().Do(x => BattleUnitBuf_SanSoraDmgRedu.GetOrAddBuf<BattleUnitBuf_SanSoraDmgRedu>(x));
            this.name = TKS_BloodFiend_Initializer.GetPassiveName(25);
            this.desc = TKS_BloodFiend_Initializer.GetPassiveDesc(25);
        }
        public override void OnDestroyed()
        {
            BattleObjectManager.instance.GetAliveList().Do(x => BattleUnitBuf_SanSoraDmgRedu.RemoveBuf<BattleUnitBuf_SanSoraDmgRedu>(x));
        }

        [HarmonyPatch(typeof(BattleParryingManager), "Decision")]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> BattleParryingManager_Decision_Tran(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
            for (int i = 1; i < codes.Count; i++)
            {
                /*if (codes[i].opcode == OpCodes.Ldfld && codes[i].operand == AccessTools.Field(typeof(BattleParryingManager), "_currentLoserTeam") &&
                    codes[i + 1].opcode == OpCodes.Ldfld && codes[i + 1].operand == AccessTools.Field(typeof(BattleParryingManager.ParryingTeam), "unit") &&
                    codes[i + 2].Calls(AccessTools.Method(typeof(BattleUnitModel), "get_emotionDetail")) &&
                    codes[i + 3].opcode == OpCodes.Ldc_I4_1 &&
                    codes[i + 4].opcode == OpCodes.Ldloc_2 &&
                    codes[i + 5].Calls(AccessTools.Method(typeof(BattleUnitEmotionDetail), "CreateEmotionCoin")))
                {
                }*/
                if (codes[i].opcode == OpCodes.Ldc_I4_1  && codes[i + 1].opcode == OpCodes.Ldloc_2 && codes[i + 2].Calls(AccessTools.Method(typeof(BattleUnitEmotionDetail), "CreateEmotionCoin")))
                {
                    codes.InsertRange(i + 1,new List<CodeInstruction>()
                    {
                        new CodeInstruction(OpCodes.Ldarg_0),
                        new CodeInstruction(OpCodes.Ldfld,AccessTools.Field(typeof(BattleParryingManager), "_currentLoserTeam")),
                        new CodeInstruction(OpCodes.Ldfld,AccessTools.Field(typeof(BattleParryingManager.ParryingTeam), "unit")),
                        new CodeInstruction(OpCodes.Call).CallInternalDelegate<PatchTools.UnmanagedDelegateTypes.UnmanagedDelegateWithRet<EmotionCoinType,EmotionCoinType,BattleUnitModel>>((EmotionCoinType CoinType,BattleUnitModel Unit)=>
                        {
                            if(Unit.passiveDetail.HasPassive<PassiveAbility_SanSora_08>() && CoinType == EmotionCoinType.Negative)
                            {
                                return EmotionCoinType.Positive;
                            }
                            return CoinType;
                        })
                    });
                }
            }
            return codes.AsEnumerable();
        }
    }
    public class PassiveAbility_SanSora_09 : PassiveAbilityBase
    {
        //昂首阔步迈向你我的明天
        public override string debugDesc => "体力无法低于10且自身不会陷入混乱\r\n获得本被动后每过一幕便使自身所有骰子威力+1\r\n4幕后若仍有司书存活则司书方获得胜利";
        public class BattleUnitBuf_SanSoraDmgRedu : BattleUnitBuf_Don_Eyuil
        {
            public int PowerUpCount = 0;
            public BattleUnitBuf_SanSoraDmgRedu(BattleUnitModel model) : base(model)
            {
                PowerUpCount = 0;
            }
            public override void OnRoundEnd()
            {
                PowerUpCount++;
            }
            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                behavior.ApplyDiceStatBonus(new DiceStatBonus() { power =  PowerUpCount });
            }
        }
        public override void OnCreated()
        {
            BattleUnitBuf_SanSoraDmgRedu.GetOrAddBuf<BattleUnitBuf_SanSoraDmgRedu>(owner);
            this.name = TKS_BloodFiend_Initializer.GetPassiveName(26);
            this.desc = TKS_BloodFiend_Initializer.GetPassiveDesc(26);
        }
        public override void OnDestroyed()
        {
            BattleUnitBuf_SanSoraDmgRedu.RemoveBuf<BattleUnitBuf_SanSoraDmgRedu>(owner);
        }
    }
}
