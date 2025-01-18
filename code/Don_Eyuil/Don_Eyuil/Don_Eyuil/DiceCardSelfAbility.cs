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
using Don_Eyuil.Don_Eyuil.DiceCardSelfAbility;
namespace Don_Eyuil
{
    public class DiceCardSelfAbility_DonEyuil_03 : DiceCardSelfAbilityBase
    {
        public static string Desc = "[使用时]消耗自身所有[硬血结晶]每消耗1层便使本书页施加的[流血]层数+1";
        public class BattleUnitBuf_HardBloodBleedingAddition : BattleUnitBuf_Don_Eyuil
        {
            public int AdditionCount = 0;
            public BattleUnitBuf_HardBloodBleedingAddition(BattleUnitModel model) : base(model) { }
            public override int OnGiveKeywordBufByCard(BattleUnitBuf cardBuf, int stack, BattleUnitModel target)
            {
                if(cardBuf.bufType == KeywordBuf.Bleeding)
                {
                    return AdditionCount;
                }
                return base.OnGiveKeywordBufByCard(cardBuf,stack,target);
            }
        }
        public override void OnEndBattle()
        {
            BattleUnitBuf_HardBloodBleedingAddition.RemoveBuf<BattleUnitBuf_HardBloodBleedingAddition>(owner);
        }
        public override void OnUseCard()
        {
            BattleUnitBuf_HardBloodBleedingAddition.GetOrAddBuf<BattleUnitBuf_HardBloodBleedingAddition>(owner).AdditionCount = BattleUnitBuf_HardBlood_Crystal.GetBufStack<BattleUnitBuf_HardBlood_Crystal>(owner);
            BattleUnitBuf_HardBlood_Crystal.RemoveBuf<BattleUnitBuf_HardBlood_Crystal>(owner);
        }
    }
    public class DiceCardSelfAbility_DonEyuil_04 : DiceCardSelfAbilityBase
    {
        public static string Desc = "[使用时]若目标[流血]层数不低于4层则在下一幕获得2层[迅捷]";
        public override void OnUseCard()
        {
            if(card.target != null && card.target.bufListDetail.GetKewordBufStack(KeywordBuf.Bleeding) >= 4)
            {
                owner.bufListDetail.AddKeywordBufByCard(KeywordBuf.Quickness, 2, owner);
            }
        }
    }
    public class DiceCardSelfAbility_DonEyuil_05 : DiceCardSelfAbilityBase
    {
        public static string Desc = "[使用时]若自身速度不低于6则使本书页所有骰子威力+2";
        public override void OnUseCard()
        {
            if(card.speedDiceResultValue >= 6)
            {
                card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus()
                {
                    power = 2
                });
            }
        }
    }
    public class DiceCardSelfAbility_DonEyuil_06 : DiceCardSelfAbilityBase
    {
        public static string Desc = "[使用时]自身速度每高于目标2点便时本书页施加的流血层数+1(至多+2)\r\n若以本书页击杀目标则对所有敌方角色施加9层[流血]";
        public class BattleUnitBuf_HardBloodBleedingAddition : BattleUnitBuf_Don_Eyuil
        {
            public int AdditionCount = 0;
            public BattleUnitBuf_HardBloodBleedingAddition(BattleUnitModel model) : base(model) { }
            public override int OnGiveKeywordBufByCard(BattleUnitBuf cardBuf, int stack, BattleUnitModel target)
            {
                if (cardBuf.bufType == KeywordBuf.Bleeding)
                {
                    return AdditionCount;
                }
                return base.OnGiveKeywordBufByCard(cardBuf, stack, target);
            }
        }
        public override void OnEndBattle()
        {
            if (this.card.target != null && this.card.target.IsDead())
            {
                BattleObjectManager.instance.GetAliveList_opponent(owner.faction).Do(x => x.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Bleeding, 5));
            }
            BattleUnitBuf_HardBloodBleedingAddition.RemoveBuf<BattleUnitBuf_HardBloodBleedingAddition>(owner);
        }
        public override void OnUseCard()
        {
            //if ((behavior.card.speedDiceResultValue - behavior.card.target.GetSpeedDiceResult(behavior.card.targetSlotOrder).value) >= 2)
            if(card.target != null)
            {
                BattleUnitBuf_HardBloodBleedingAddition.GetOrAddBuf<BattleUnitBuf_HardBloodBleedingAddition>(owner).AdditionCount = Math.Min(2, Math.Max(0, (card.speedDiceResultValue - card.target.GetSpeedDiceResult(card.targetSlotOrder).value) / 2));
            }
        }
    }
    public class DiceCardSelfAbility_DonEyuil_09 : DiceCardSelfAbilityBase
    {
        public static string Desc = "[使用时] 若目标[流血] 层数不低于4层则使本书页所有骰子威力+3";

        public override void OnUseCard()
        {
            if(card.target != null && card.target.bufListDetail.GetKewordBufStack(KeywordBuf.Bleeding) >= 4)
            {
                card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus() { power = 3 } );
            }
        }
    }
    public class DiceCardSelfAbility_DonEyuil_11 : DiceCardSelfAbilityBase
    {
        public static string Desc = "本书页命中时将使本书页施加的[流血]层数+1";
        public class BattleUnitBuf_HardBloodBleedingAddition : BattleUnitBuf_Don_Eyuil
        {
            public int AdditionCount = 0;
            public int BleedingTotal = 0;
            public BattleUnitBuf_HardBloodBleedingAddition(BattleUnitModel model) : base(model) { }
            public override int OnGiveKeywordBufByCard(BattleUnitBuf cardBuf, int stack, BattleUnitModel target)
            {
                if (cardBuf.bufType == KeywordBuf.Bleeding)
                {
                    BleedingTotal += AdditionCount + stack;
                    return AdditionCount;
                }
                return base.OnGiveKeywordBufByCard(cardBuf, stack, target);
            }
        }
        public override void OnEndBattle()
        {
            BattleUnitBuf_HardBloodBleedingAddition.RemoveBuf<BattleUnitBuf_HardBloodBleedingAddition>(owner);
        }
        public override void OnSucceedAttack()
        {
            BattleUnitBuf_HardBloodBleedingAddition.GetOrAddBuf<BattleUnitBuf_HardBloodBleedingAddition>(owner).AdditionCount = 1;
        }
    }
    public class DiceCardSelfAbility_DonEyuil_14 : DiceCardSelfAbilityBase
    {
        public class BattleUnitBuf_HardBloodBleedingAddition : BattleUnitBuf_Don_Eyuil
        {
            public BattleUnitBuf_HardBloodBleedingAddition(BattleUnitModel model) : base(model) { }
            public override int GetMultiplierOnGiveKeywordBufByCard(BattleUnitBuf cardBuf, BattleUnitModel target)
            {
                if(cardBuf.bufType == KeywordBuf.Bleeding)
                {
                    return 2;
                }
                return base.GetMultiplierOnGiveKeywordBufByCard(cardBuf, target);
            }
        }
        public static string Desc = "[使用时]若目标没有[流血]则本书页施加的[流血]层数翻倍";

        public override void OnEndBattle()
        {
            BattleUnitBuf_HardBloodBleedingAddition.RemoveBuf<BattleUnitBuf_HardBloodBleedingAddition>(owner);
        }
        public override void OnUseCard()
        {
            if(card.target != null && card.target.bufListDetail.GetKewordBufStack(KeywordBuf.Bleeding) > 0)
            {
                BattleUnitBuf_HardBloodBleedingAddition.GetOrAddBuf<BattleUnitBuf_HardBloodBleedingAddition>(owner);
            }
        }
    }
    public class DiceCardSelfAbility_DonEyuil_21 : DiceCardSelfAbilityBase
    {
        public static string Desc = "[战斗开始]下一幕使目标不会被施加[流血]";
        public class BattleUnitBuf_AntiBleeding : BattleUnitBuf_Don_Eyuil
        {
            public BattleUnitBuf_AntiBleeding(BattleUnitModel model) : base(model) { }
            public override void BeforeAddKeywordBuf(KeywordBuf BufType, ref int Stack)
            {
                if(BufType == KeywordBuf.Bleeding) { stack = 0; }
            }
        }
    }
    //群体书页
    public class DiceCardSelfAbility_DonEyuil_22 : DiceCardSelfAbilityBase
    {
        public static string Desc = "本书页命中目标时将使自身获得10点护盾\r\n[使用后]结束本幕";
        public override void OnSucceedAreaAttack(BattleUnitModel target)
        {
            
        }
        public override void OnEndAreaAttack()
        {
            Singleton<StageController>.Instance.RoundEndForcely();
        }
    }
    public class DiceCardSelfAbility_DonEyuil_23 : DiceCardSelfAbilityBase
    {
        public static string Desc = "[战斗开始]使自身获得3层[振奋]";
        public override void OnStartBattle()
        {
            owner.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.BreakProtection, 3, owner);
        }
    }
    public class DiceCardSelfAbility_DonEyuil_24 : DiceCardSelfAbilityBase
    {
        public static string Desc = "[使用时]若目标[流血]层数不低于3层则使本书页所有骰子最小值+3";

        public override void OnUseCard()
        {
            if(card.target != null && card.target.bufListDetail.GetKewordBufStack(KeywordBuf.Bleeding) >= 3)
            {
                card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus() { min = 3 });
            }
        }
    }


    public class DiceCardSelfAbility_DonEyuil_77 : DiceCardSelfAbilityBase
    {
        public static string Desc = "[使用时]这一幕对自身施加3层流血";
        public int winCount = 0;
        public override void OnUseCard()
        {
            winCount = 0;
            owner.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, 3, owner);
        }
        public override void OnWinParryingAtk()
        {
            winCount++;
        }
        public override void OnWinParryingDef()
        {
            winCount++;
        }
    }
}
