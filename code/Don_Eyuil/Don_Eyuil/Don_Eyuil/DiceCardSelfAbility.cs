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
using TMPro;
namespace Don_Eyuil
{
    public class DiceCardSelfAbility_DonEyuil_01 : DiceCardSelfAbilityBase
    {
        public static string Desc = "本书页将额外命中两名敌方角色";
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            //Debug.LogError("AAAAAAAAAAAAAAAAAAAAAAAAAAAA" + behavior.card.target.Book.Name);
            behavior.GiveDamage_SubTarget(card.target, 2);

            /*if(!behavior.HasFlag(TKS_BloodFiend_Initializer.TKS_EnumExtension.DiceFlagExtension.HasGivenDamage_SubTarget))
            {
                var AliveList = BattleObjectManager.instance.GetAliveList_opponent(owner.faction);
                AliveList.Remove(card.target);
                var list = new List<string>(){ };
                AliveList.Do(x => list.Add(x.Book.Name));
                Debug.LogError(String.Join(",",list));
                behavior.GiveDamage_SubTarget(MyTools.TKSRandomUtil(AliveList.ToList(), 2, false, false).ToArray());
            }*/
            //behavior.GiveDamage_SubTarget(MyTools.TKSRandomUtil(BattleObjectManager.instance.GetAliveList_opponent(owner.faction).Except(new List<BattleUnitModel>() { card.target }).ToList(), 2, false, false).ToArray());
            //MyTools.TKSRandomUtil(BattleObjectManager.instance.GetAliveList_opponent(owner.faction), 2, false, false).DoIf(x => x != card.target, y => behavior.GiveDamage_SubTarget(y));
        }
        public override void OnRollDice(BattleDiceBehavior behavior)
        {
            //Singleton<StageController>.Instance.dontUseUILog = true;

            //Singleton<StageController>.Instance.dontUseUILog = false;
        }
    }
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
        
        public override void OnStartBattle()
        {
            BattleUnitBuf_AntiBleeding.GainBuf<BattleUnitBuf_AntiBleeding>(card.target, 1,BufReadyType.NextRound);
        }
        public class BattleUnitBuf_AntiBleeding : BattleUnitBuf_Don_Eyuil
        {
            public override void OnRoundEnd()
            {
                this.Destroy();
            }
            public BattleUnitBuf_AntiBleeding(BattleUnitModel model) : base(model){ }

            [HarmonyPatch(typeof(BattleUnitBufListDetail), "AddKeywordBufThisRoundByEtc")]
            [HarmonyPatch(typeof(BattleUnitBufListDetail), "AddKeywordBufByEtc")]
            [HarmonyPatch(typeof(BattleUnitBufListDetail), "AddKeywordBufThisRoundByCard")]
            [HarmonyPatch(typeof(BattleUnitBufListDetail), "AddKeywordBufByCard")]
            [HarmonyPatch(typeof(BattleUnitBufListDetail), "AddKeywordBufNextNextByCard")]
            [HarmonyPrefix]
            public static bool BattleUnitBufListDetail_AddKeywordBuf_Pre(BattleUnitBufListDetail __instance, KeywordBuf bufType) => !(bufType == KeywordBuf.Bleeding && __instance.GetFieldValue<BattleUnitModel>("_self").bufListDetail.HasBuf<BattleUnitBuf_AntiBleeding>());
        }
    }
    //群体书页
    public class DiceCardSelfAbility_DonEyuil_22 : DiceCardSelfAbilityBase
    {
        public static string Desc = "本书页命中目标时将使自身获得10点护盾\r\n[使用后]结束本幕";
        public override void OnSucceedAreaAttack(BattleUnitModel target)
        {
            if(target != null)
            {
                BattleUnitBuf_BloodShield.GainBuf<BattleUnitBuf_BloodShield>(owner, 10);
            }
            
        }
        public override void OnEndAreaAttack()
        {

            // Singleton<StageController>.Instance.BattleEndForcelyNotRound();
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
            //Singleton<StageController>.Instance.InvokeMethod("set_phase", StageController.StagePhase.RoundEndPhase);
            //Singleton<StageController>.Instance.InvokeMethod("RoundEndPhase", Time.deltaTime);
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
    public class DiceCardSelfAbility_DonEyuil_32 : DiceCardSelfAbilityBase
    {
        public static string Desc = "本书页将同时命中所有敌方角色\r\n拼点失败时本书页依旧将击中目标但只施加[流血]\r\n";
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            //Singleton<StageController>.Instance.dontUseUILog = true;
            //BattleObjectManager.instance.GetAliveList_opponent(owner.faction).DoIf(y => y != card.target, x => behavior.GiveDamage_SubTarget(x));
            //Singleton<StageController>.Instance.dontUseUILog = false;
        }
    }
    public class DiceCardSelfAbility_DonEyuil_34 : DiceCardSelfAbilityBase
    {
        public static string Desc = "[使用时]这一幕及下两幕对自身施加8层[流血]";
        public override void OnUseCard()
        {
            owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Bleeding, 8);
            owner.bufListDetail.AddKeywordBufNextNextByCard(KeywordBuf.Bleeding, 8);
        }
    }
    public class DiceCardSelfAbility_DonEyuil_35 : DiceCardSelfAbilityBase
    {
        public static string Desc = "[战斗开始]这一幕拼点失败时对自身施加2层[流血](至多3次)";

        public override void OnStartBattle()
        {
            BattleUnitBuf_LoseParryingSelfBleeding.GainBuf<BattleUnitBuf_LoseParryingSelfBleeding>(owner, 1);
        }
        public class BattleUnitBuf_LoseParryingSelfBleeding : BattleUnitBuf_Don_Eyuil
        {
            int TriggeredCount = 0;
            public override void OnLoseParrying(BattleDiceBehavior behavior)
            {
                if(TriggeredCount < 3)//0 1 2
                {
                    TriggeredCount++;//1 2 3
                    _owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Bleeding, 2);
                }
            }
            public override void OnRoundEnd()
            {
                this.Destroy();
            }
            public BattleUnitBuf_LoseParryingSelfBleeding(BattleUnitModel model) : base(model) { }
        }

    }
    public class DiceCardSelfAbility_DonEyuil_36 : DiceCardSelfAbilityBase
    {
        public static string Desc = "[战斗开始]这一幕击中目标时对自身施加1层[虚弱](至多2次";
        public override void OnStartBattle()
        {
            BattleUnitBuf_AtkSelfWeak.GainBuf<BattleUnitBuf_AtkSelfWeak>(owner, 1);
        }
        public class BattleUnitBuf_AtkSelfWeak : BattleUnitBuf_Don_Eyuil
        {
            int TriggeredCount = 0;
            public override void OnSuccessAttack(BattleDiceBehavior behavior)
            {
                if (TriggeredCount < 2)//0 1  
                {
                    TriggeredCount++;//1 2  
                    _owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Weak, 1);
                }
            }
            public override void OnRoundEnd()
            {
                this.Destroy();
            }
            public BattleUnitBuf_AtkSelfWeak(BattleUnitModel model) : base(model) { }
        }
    }
    public class DiceCardSelfAbility_DonEyuil_38 : DiceCardSelfAbilityBase
    {
        public static string Desc = "[战斗开始]这一幕拼点失败时使自身获得1层[强壮]";
        public override void OnStartBattle()
        {
            BattleUnitBuf_LoseParryingStrong.GainBuf<BattleUnitBuf_LoseParryingStrong>(owner, 1);
        }
        public class BattleUnitBuf_LoseParryingStrong : BattleUnitBuf_Don_Eyuil
        {
            public override void OnLoseParrying(BattleDiceBehavior behavior)
            {
                _owner.bufListDetail.AddKeywordBufByCard(KeywordBuf.Strength, 1,_owner);
            }
            public override void OnRoundEnd()
            {
                this.Destroy();
            }
            public BattleUnitBuf_LoseParryingStrong(BattleUnitModel model) : base(model) { }
        }
    }
    public class DiceCardSelfAbility_DonEyuil_39 : DiceCardSelfAbilityBase
    {
        public static string Desc = "本书页命中时将恢复等同于伤害量25%的体力若这一幕中自身已经累积承受10点流血伤害则改为50%\r\n本书页恢复溢出的体力将转化为等量护盾";
        public class BattleUnitBuf_HardBloodBleedingAddition : BattleUnitBuf_Don_Eyuil
        {
            public int BleedingDmgTotal = 0;
            public BattleUnitBuf_HardBloodBleedingAddition(BattleUnitModel model) : base(model) { }
            public override void OnSuccessAttack(BattleDiceBehavior behavior)
            {
                if(behavior != null && behavior.card != null && behavior.card.card.XmlData.Script == "DonEyuil_39")
                {
                    int RecoverHpNum = (int)(behavior.DiceResultDamage * BleedingDmgTotal >= 10 ? 0.5 : 0.25);
                    if(_owner.hp + RecoverHpNum > _owner.MaxHp)
                    {
                        BattleUnitBuf_BloodShield.GainBuf<BattleUnitBuf_BloodShield>(_owner,(int) _owner.hp + RecoverHpNum - _owner.MaxHp);
                        RecoverHpNum = _owner.MaxHp - (int)_owner.hp;
                    }
                    _owner.RecoverHP(RecoverHpNum);
                }
            }
            public override void AfterTakeBleedingDamage(int Dmg)
            {
                BleedingDmgTotal += Dmg;
            }
        }
        public override void OnEndBattle()
        {
            BattleUnitBuf_HardBloodBleedingAddition.RemoveBuf<BattleUnitBuf_HardBloodBleedingAddition>(owner);
        }

        public override void OnStartBattle()
        {
            BattleUnitBuf_HardBloodBleedingAddition.GetOrAddBuf<BattleUnitBuf_HardBloodBleedingAddition>(owner);
        }
    }
    public class DiceCardSelfAbility_DonEyuil_43 : DiceCardSelfAbilityBase
    {
        public static string Desc = "[战斗开始]对自身与2名敌方角色施加1层[强壮]于[振奋]";
        public override void OnStartBattle()
        {
            owner.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Strength, 1,owner);
            MyTools.TKSRandomUtil(BattleObjectManager.instance.GetAliveList_opponent(owner.faction), 2, false, false).Do(x => x.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.BreakProtection, 1, owner));
        }
    }
    public class DiceCardSelfAbility_DonEyuil_44 : DiceCardSelfAbilityBase
    {
        public static string Desc = "本书页将命中全体敌方角色";
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            behavior.GiveDamage_SubTarget(card.target, -1);
           // BattleObjectManager.instance.GetAliveList_opponent(owner.faction).DoIf(y => y != card.target, x => behavior.GiveDamage_SubTarget(x));
        }
        public override void OnRollDice(BattleDiceBehavior behavior)
        {
            //Singleton<StageController>.Instance.dontUseUILog = true;

            //Singleton<StageController>.Instance.dontUseUILog = false;
        }
    }

    public class DiceCardSelfAbility_DonEyuil_46 : DiceCardSelfAbilityBase
    {
        public static string Desc = "[使用时]若自身应用了[血鞭]则使本书页施加[流血]时同时施加[血晶荆棘]";
    }
    public class DiceCardSelfAbility_DonEyuil_50 : DiceCardSelfAbilityBase
    {
        public static string Desc = "场上每有一名处于共鸣状态的敌方角色便使本书页造成的伤害减少25%\r\n[战斗开始]清除所有的[凝结的情感]\r\n[使用后]结束本幕";
        public override void BeforeGiveDamage(BattleDiceBehavior behavior)
        {
            var Count = BattleUnitBuf_Resonance.GetAllUnitWithBuf<BattleUnitBuf_Resonance>(Faction.Player).Count * 25;
            if(Count >0)
            {
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    dmgRate = -Count ,
                    breakRate = -Count
                });
            }
        }
        public override void OnStartBattle()
        {
            BattleObjectManager.instance.GetAliveList().DoIf(x => x.passiveDetail.HasPassive<PassiveAbility_DonEyuil_10>(), y => y.Die());
        }
        public override void OnEndAreaAttack()
        {

            // Singleton<StageController>.Instance.BattleEndForcelyNotRound();
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
        }
    }
    public class DiceCardSelfAbility_DonEyuil_51 : DiceCardSelfAbilityBase
    {
        public static string Desc = "[战斗开始]将场上所有共鸣效果转移至目标并对目标施加[光荣的决斗]同时使所有其余敌方角色无法行动";
        public override void OnStartBattle()
        {
            if(card.target != null)
            {
                /*BattleUnitBuf_Resonance.GetAllBufOnField<BattleUnitBuf_Resonance>().DoIf(y => y.owner!= card.target,x =>
                {
                    if (x.GetType().Name.Contains("BrightDream")) { BattleUnitBuf_Resonance.GetOrAddBuf<BattleUnitBuf_Resonance.BattleUnitBuf_Resonance_BrightDream>(card.target); }
                    if (x.GetType().Name.Contains("GreatHope")) { BattleUnitBuf_Resonance.GetOrAddBuf<BattleUnitBuf_Resonance.BattleUnitBuf_Resonance_GreatHope>(card.target); }
                    if (x.GetType().Name.Contains("BoreResponsibility")) { BattleUnitBuf_Resonance.GetOrAddBuf<BattleUnitBuf_Resonance.BattleUnitBuf_Resonance_BoreResponsibility>(card.target); }
                    if (x.GetType().Name.Contains("MutualUnderstanding")) { BattleUnitBuf_Resonance.GetOrAddBuf<BattleUnitBuf_Resonance.BattleUnitBuf_Resonance_MutualUnderstanding>(card.target); }
                    x.Destroy();
                });*/
                BattleObjectManager.instance.GetAliveList_opponent(owner.faction).DoIf(x => x != card.target, y =>
                {
                    y.bufListDetail.GetActivatedBufList().DoIf(bx => bx is BattleUnitBuf_Resonance && !bx.IsDestroyed(), by =>
                    {
                        AccessTools.Method(by.GetType(), "GetOrAddBuf").MakeGenericMethod(by.GetType()).Invoke(null, new object[]
                        {
                            card.target
                        });
                        //card.target.bufListDetail.AddBuf(by);
                        by.Destroy();
                    });
                    BattleUnitBuf_DuelStun.GetOrAddBuf<BattleUnitBuf_DuelStun>(y);
                });
            }
            BattleUnitBuf_GloriousDuel.GetOrAddBuf<BattleUnitBuf_GloriousDuel>(card.target,BufReadyType.NextRound);
        }
        public class BattleUnitBuf_DuelStun : BattleUnitBuf_Don_Eyuil
        {
            public override bool IsTargetable() => false;
            public BattleUnitBuf_DuelStun(BattleUnitModel model) : base(model) { }
            public override bool IsActionable()
            {
                return base.IsDestroyed();
            }
            public override int SpeedDiceBreakedAdder()
            {
                return 10;
            }

        }
    }
    public class DiceCardSelfAbility_DonEyuil_52: DiceCardSelfAbilityBase
    {
        public static string Desc = "[使用时]若目标带有[璀璨的梦想]则使本书页所有骰子最大值与最小值-10";
        public override void OnUseCard()
        {
            if(card.target != null && BattleUnitBuf_Resonance.GetBuf<BattleUnitBuf_Resonance.BattleUnitBuf_Resonance_BrightDream>(card.target) != null)
            {
                card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus() { min = -10, max = -10 });
            }    
        }
    }
    public class DiceCardSelfAbility_DonEyuil_53 : DiceCardSelfAbilityBase
    {
        public static string Desc = "[使用时]若目标带有[美好的希望]则使本书页所有骰子最大值与最小值-20";
        public override void OnUseCard()
        {
            if (card.target != null && BattleUnitBuf_Resonance.GetBuf<BattleUnitBuf_Resonance.BattleUnitBuf_Resonance_GreatHope>(card.target) != null)
            {
                card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus() { min = -20, max = -20 });
            }
        }
    }
    public class DiceCardSelfAbility_DonEyuil_54 : DiceCardSelfAbilityBase
    {
        public static string Desc = "[使用时]若目标带有[背负的责任]则使本书页所有骰子最大值与最小值-20";
        public override void OnUseCard()
        {
            if (card.target != null && BattleUnitBuf_Resonance.GetBuf<BattleUnitBuf_Resonance.BattleUnitBuf_Resonance_BoreResponsibility>(card.target) != null)
            {
                card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus() { min = -20, max = -20 });
            }
        }
    }
    public class DiceCardSelfAbility_DonEyuil_55 : DiceCardSelfAbilityBase
    {
        public static string Desc = "[使用时]若目标带有[互相的理解]则使本书页所有骰子最大值与最小值-10";
        public override void OnUseCard()
        {
            if (card.target != null && BattleUnitBuf_Resonance.GetBuf<BattleUnitBuf_Resonance.BattleUnitBuf_Resonance_MutualUnderstanding>(card.target) != null)
            {
                card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus() { min = -10, max = -10 });
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
    public class DiceCardSelfAbility_DonEyuil_79 : DiceCardSelfAbilityBase
    {
        public static string Desc = "[使用时]这一幕对自身施加5层流血";
        public override void OnUseCard()
        {
            owner.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, 5, owner);
        }
    }
}
