using BattleCharacterProfile;
using Don_Eyuil.Don_Eyuil.Player.Buff;
using Don_Eyuil.San_Sora.Player.Buff;
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
    public class DiceCardSelfAbility_SanSora_01 : DiceCardSelfAbilityBase
    {
        public static string Desc = "自身每有10层[血羽]便使本书页所有骰子造成的伤害增加20%";
        public override void BeforeGiveDamage(BattleDiceBehavior behavior)
        {
            behavior.ApplyDiceStatBonus(new DiceStatBonus() { dmgRate = (BattleUnitBuf_Feather.GetBufStack<BattleUnitBuf_Feather>(owner) / 10) * 20 });
        }
    }
    public class DiceCardSelfAbility_SanSora_02 : DiceCardSelfAbilityBase
    {
        public static string Desc = "[使用时]若目标未拥有[流血]则使本书页所有骰子威力+3";
        public override void OnUseCard()
        {
            if(card != null && card.target != null && card.target.bufListDetail.GetKewordBufStack(KeywordBuf.Bleeding) <= 0)
            {
                card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus() { power = 3 });
            }
        }
    }
    public class DiceCardSelfAbility_SanSora_03 : DiceCardSelfAbilityBase
    {
        public static string Desc = "[拼点开始]自身与目标每有2层[流血]便使本书页所有骰子威力+1(至多+4)";
        public override void OnStartParrying()
        {
            card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus()
            {
                power = Math.Min(4, (owner.bufListDetail.GetKewordBufStack(KeywordBuf.Bleeding) + card.target?.bufListDetail.GetKewordBufStack(KeywordBuf.Bleeding) ?? 0) / 2)
            });
        }
    }
    public class DiceCardSelfAbility_SanSora_04 : DiceCardSelfAbilityBase
    {
        public static string Desc = "[战斗开始]这一幕对自身施加3层[流血]";
        public override void OnStartBattle()
        {
            owner.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Bleeding, 3);
        }
    }
    public class DiceCardSelfAbility_SanSora_05 : DiceCardSelfAbilityBase
    {
        public static string Desc = "[使用时]自身每有10层[血羽]便使本书页所有骰子威力+1\r\n本书页击中目标使将获得10点护盾\r\n[使用后]结束本幕并使自身在下一幕获得[摇曳]";
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            behavior.ApplyDiceStatBonus(new DiceStatBonus()
            {
                power = BattleUnitBuf_Feather.GetBufStack<BattleUnitBuf_Feather>(owner) / 10
            });
        }
        public override void OnSucceedAreaAttack(BattleUnitModel target)
        {
            if(target != null)
            {
                BattleUnitBuf_BloodShield.GainBuf<BattleUnitBuf_BloodShield>(owner, 10);
            }
        }
        public override void OnEndBattle()
        {
            BattleObjectManager.instance.GetAliveList().Do(target =>
            {
                List<BattlePlayingCardDataInUnitModel> list = new List<BattlePlayingCardDataInUnitModel>();
                for (int i = 0; i < target.cardSlotDetail.cardAry.Count; i++)
                {
                    BattlePlayingCardDataInUnitModel battlePlayingCardDataInUnitModel = target.cardSlotDetail.cardAry[i];
                    if (battlePlayingCardDataInUnitModel != null && !battlePlayingCardDataInUnitModel.isDestroyed && battlePlayingCardDataInUnitModel.GetDiceBehaviorList().Count > 0)
                    {
                        battlePlayingCardDataInUnitModel.DestroyPlayingCard();
                    }
                }
            });
            BattleUnitBuf_SanFlicker_Enemy.GainBuf<BattleUnitBuf_SanFlicker_Enemy>(owner, 1);
        }
    }
    public class DiceCardSelfAbility_SanSora_06 : DiceCardSelfAbilityBase
    {
        public static string Desc = "[使用时]消耗至多15层[血羽]每消耗3层便使本书页所有骰子最大值+1";
        public override void OnUseCard()
        {
            card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus()
            {
                max = Math.Min(5, BattleUnitBuf_Feather.GetBufStack<BattleUnitBuf_Feather>(owner) / 3)
            }) ;
            BattleUnitBuf_Feather.GainBuf<BattleUnitBuf_Feather>(owner, -15);
        }
    }
    public class DiceCardSelfAbility_SanSora_14 : DiceCardSelfAbilityBase
    {
        public static string Desc = "[使用时]消耗8层[血羽]使本书页所有骰子威力+2";
        public override void OnUseCard()
        {
            if(BattleUnitBuf_Feather.UseBuf<BattleUnitBuf_Feather>(owner,8))
            {
                card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus() { power = 2 });
            }
        }
    }
    public class DiceCardSelfAbility_SanSora_15 : DiceCardSelfAbilityBase
    {
        public static string Desc = "[我无法视而不见!]\r\n[战斗开始]对自身施加3层[流血]并使这一幕自身对敌方角色施加的[流血]层数+1";
        public class BattleUnitBuf_HardBloodBleedingAddition : BattleUnitBuf_Don_Eyuil
        {
            public BattleUnitBuf_HardBloodBleedingAddition(BattleUnitModel model) : base(model) { }
            public override int OnGiveKeywordBufByCard(BattleUnitBuf cardBuf, int stack, BattleUnitModel target)
            {
                return cardBuf.bufType == KeywordBuf.Bleeding ? 1 : 0;
            }
            public override void OnRoundEnd()
            {
                this.Destroy();
            }
        }
        public override void OnStartBattle()
        {
            owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Bleeding, 3);
            BattleUnitBuf_HardBloodBleedingAddition.GainBuf<BattleUnitBuf_HardBloodBleedingAddition>(owner,1);
        }
    }
    //群体书页
    public class DiceCardSelfAbility_SanSora_16 : DiceCardSelfAbilityBase
    {
        public static string Desc = "[便能达成那理想中的共存吧...]\r\n[战斗开始]若场上的[流血]总数不低于20则使自身在这一幕中获得4层[虚弱]";
        public override void OnStartBattleAfterCreateBehaviour()
        {
            int BleedingTotal = 0;
            BattleObjectManager.instance.GetAliveList().Do(x => BleedingTotal += x.bufListDetail.GetKewordBufStack(KeywordBuf.Bleeding));
            if(BleedingTotal >= 20)
            {
                owner.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Weak, 4);
            }
        }
    }
    public class DiceCardSelfAbility_SanSora_17 : DiceCardSelfAbilityBase
    {
        public static string Desc = "[我坚信共存与梦想终会实现]";
    }
    public class DiceCardSelfAbility_SanSora_18 : DiceCardSelfAbilityBase
    {
        public static string Desc = "[我将一刻不停的向之飞翔!!]";
        public override void OnEndBattle()
        {
            BattleObjectManager.instance.GetAliveList(Faction.Enemy).Do(x => x.DieFake());
            Singleton<StageController>.Instance.CheckEndBattle();
        }
    }

}
