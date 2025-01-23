using Don_Eyuil.Don_Eyuil.Buff;
using Don_Eyuil.Don_Eyuil.DiceCardSelfAbility;
using HarmonyLib;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using static Don_Eyuil.Don_Eyuil.Player.DiceCardSelfAbility.DiceCardSelfAbility_DonEyuil_69;

namespace Don_Eyuil
{
    public class DiceCardAbility_DonEyuil_Testify : DiceCardAbilityBase
    {
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            target.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.WarpCharge, 1);
        }
    }
    public class DiceCardAbility_DonEyuil_02 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]对双方施加3层[流血]";
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            owner.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, 3, owner);
            if(target != null)
            {
                target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, 3, owner);
            }

        }
    }
    public class DiceCardAbility_DonEyuil_07 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]对目标下两幕施加3层[无法凝结的血]";
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            BattleUnitBuf_UncondensableBlood.GainBuf<BattleUnitBuf_UncondensableBlood>(target, 3,BufReadyType.NextRound);
            BattleUnitBuf_UncondensableBlood.GainBuf<BattleUnitBuf_UncondensableBlood>(target, 3, BufReadyType.NextNextRound);
        }
    }
    public class DiceCardAbility_DonEyuil_08 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]对目标施加1层[流血](重复触发自身激活的硬血术书页次)";
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            var P02 = owner.passiveDetail.PassiveList.Find(x => x is PassiveAbility_DonEyuil_02) as PassiveAbility_DonEyuil_02;
            if(P02 != null)
            {
                if(P02.CurrentArtPair != null &&  P02.CurrentArtPair.Arts != null && P02.CurrentArtPair.Arts.Count > 0)
                {
                    P02.CurrentArtPair.Arts.Do(x => target.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Bleeding, 1));
                }
            }
        }
    }
    public class DiceCardAbility_DonEyuil_10 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]获得4层[结晶硬血]";
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            BattleUnitBuf_HardBlood_Crystal.GainBuf<BattleUnitBuf_HardBlood_Crystal>(owner, 4);
        }
    }
    public class DiceCardAbility_DonEyuil_12 : DiceCardAbilityBase
    {
        public static string Desc = "本骰子将重复投掷2次自身每有10层[结晶硬血]便使该次数+1\r\n[命中时]对目标施加1层[流血]";

        public override void AfterAction()
        {
            if (!base.owner.IsBreakLifeZero() && this._repeatCount < 1 + (BattleUnitBuf_HardBlood_Crystal.GetBufStack<BattleUnitBuf_HardBlood_Crystal>(owner) / 10))
            {
                this._repeatCount++;
                base.ActivateBonusAttackDice();
            }
        }
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            if(target != null)
            {
                target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, 1, owner);
            }
        }
        private int _repeatCount;
    }
    public class DiceCardAbility_DonEyuil_13 : DiceCardAbilityBase
    {
        public static string Desc = "本书页每施加1层流血便使本骰子威力+1";
        public override void BeforeRollDice()
        {
            var AdditionBuf = DiceCardSelfAbility_DonEyuil_11.BattleUnitBuf_HardBloodBleedingAddition.GetBuf<DiceCardSelfAbility_DonEyuil_11.BattleUnitBuf_HardBloodBleedingAddition>(owner);
            if (behavior != null && AdditionBuf != null && AdditionBuf.BleedingTotal > 0)
            {
                behavior.ApplyDiceStatBonus(new DiceStatBonus() { power = AdditionBuf.BleedingTotal });
            }
        }
    }
    public class DiceCardAbility_DonEyuil_15 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]下两幕对目标施加4层[流血]";
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            if(target != null)
            {
                target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, 4, owner);
                target.bufListDetail.AddKeywordBufNextNextByCard(KeywordBuf.Bleeding, 4, owner);
            }
        }
    }
    public class DiceCardAbility_DonEyuil_16 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]对所有敌方角色施加共计15层[流血]";
        public override void OnSucceedAttack()
        {
            List<int> BleedingCount = new List<int>() {0};
            var AliveList = BattleObjectManager.instance.GetAliveList_opponent(owner.faction);
            for (int i =0;i< AliveList.Count - 1;i++)
            {
                BleedingCount.Add(RandomUtil.Range(0, 15));
            }
            BleedingCount.Add(15);BleedingCount.Sort();
            for(int i = 0;i<BleedingCount.Count-1;i++)
            {
                BleedingCount[i] = BleedingCount[i + 1] - BleedingCount[i];
            }
            BleedingCount.RemoveAt(BleedingCount.Count - 1);
            for (int i = 0; i < AliveList.Count - 1; i++)
            {
                if (AliveList[i] != null)
                {
                    AliveList[i].bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, BleedingCount[i], owner);
                }
            }
        }
    }
    public class DiceCardAbility_DonEyuil_17 : DiceCardAbilityBase
    {
        public static string Desc = "[拼点胜利]下一幕对双方施加1层[流血]";

        public override void OnWinParrying()
        {
            owner.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Bleeding, 1, owner);
            behavior.card.target.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Bleeding, 1, owner);
        }
    }
    public class DiceCardAbility_DonEyuil_18 : DiceCardAbilityBase
    {
        public static string Desc = "[拼点胜利]这一幕对双方施加3层[流血]";

        public override void OnWinParrying()
        {
            owner.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Bleeding, 3, owner);
            behavior.card.target.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Bleeding, 3, owner);
        }
    }
    public class DiceCardAbility_DonEyuil_19 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]重复触发本书页拼点胜利次自身[流血]";

        public override void OnSucceedAttack()
        {
            if (this.card.cardAbility is DiceCardSelfAbility_DonEyuil_77)
            {
                var ability = this.card.cardAbility as DiceCardSelfAbility_DonEyuil_77;
                var buf = owner.bufListDetail.GetActivatedBufList().Find(x => x.bufType == KeywordBuf.Bleeding);
                if (buf == null)
                {
                    return;
                }
                for (int i = 0; i < ability.winCount; i++)
                {
                    buf.AfterDiceAction(behavior);
                }
            }
        }
    }
    public class DiceCardAbility_DonEyuil_20 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]恢复等同于造成伤害量的体力溢出部分将转化为等量护盾";

        public static int AfterGiveDamage(BattleDiceBehavior behavior, int dmg)
        {
            if(behavior != null && behavior.abilityList.Exists(x => x is DiceCardAbility_DonEyuil_20) && behavior.owner != null)
            {
                int losthp = behavior.owner.MaxHp - (int)behavior.owner.hp;
                behavior.owner.RecoverHP(Math.Min(losthp, dmg));
                if(dmg - losthp > 0)
                {
                    //BattleUnitBuf_PhysicalShield.AddBuf(behavior.owner, dmg - losthp);
                    BattleUnitBuf_BloodShield.GainBuf<BattleUnitBuf_BloodShield>(behavior.owner, dmg);
                }
            }
            return dmg;
        }
        [HarmonyPatch(typeof(BattleDiceBehavior), "GiveDamage")]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> BattleDiceBehavior_GiveDamage_Tran(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
            int index = codes.FindIndex(x => x.opcode == OpCodes.Callvirt && x.operand.ToString().Contains("TakeDamage"));
            codes.InsertRange(index + 1, new List<CodeInstruction>()
            {
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldloc_S, 13),
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(DiceCardAbility_DonEyuil_20), "AfterGiveDamage"))
            });
            return codes.AsEnumerable();
        }
        
    }
    public class DiceCardAbility_DonEyuil_25 : DiceCardAbilityBase
    {
        public static string Desc = "若本骰子基础值不低于9则使本骰子重复投掷一次(至多3次)";
        public override void AfterAction()
        {
            if (!base.owner.IsBreakLifeZero() && this._repeatCount < 3 && behavior.DiceVanillaValue >= 9)
            {
                this._repeatCount++;
                base.ActivateBonusAttackDice();
            }
        }
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            if (target != null)
            {
                target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, 1, owner);
            }
        }
        private int _repeatCount;
    }
    public class DiceCardAbility_DonEyuil_26 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]这一幕对目标施加3层[流血]";
        public override void OnSucceedAttack(BattleUnitModel target) {target.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Bleeding,3,owner);}
    }
    public class DiceCardAbility_DonEyuil_27 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]消耗目标3层[流血]层流血并使本骰子重复投掷1次(至多4次)";
        public override void OnSucceedAttack(BattleUnitModel target) 
        {
            if (target.bufListDetail.GetKewordBufStack(KeywordBuf.Bleeding) >= 3 && !base.owner.IsBreakLifeZero() && this._repeatCount < 4)
            {
                this._repeatCount++;
                base.ActivateBonusAttackDice();
                target.bufListDetail.GetActivatedBuf(KeywordBuf.Bleeding).stack -=3;
            }
        }
        private int _repeatCount;
    }
    public class DiceCardAbility_DonEyuil_28 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]追加本书页击中目标次数×2的混乱伤害";
        public override void OnSucceedAttack()
        {
            var temp = card.cardAbility as DiceCardSelfAbility_DmgCount;
            if (temp != null)
            {
                card.target.TakeBreakDamage(temp.count * 2, attacker: owner);
            }
        }
    }
    public class DiceCardAbility_DonEyuil_29 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]对目标施加等同于目标[流血]层数的[血晶荆棘]";
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            BattleUnitBuf_BloodCrystalThorn.GainBuf<BattleUnitBuf_BloodCrystalThorn>(target, target.bufListDetail.GetKewordBufStack(KeywordBuf.Bleeding));
        }
    }
    public class DiceCardAbility_DonEyuil_30 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]对目标施加等同于目标[流血]层数的[血晶荆棘]";
        public override void OnSucceedAreaAttack(BattleUnitModel target)
        {
            BattleUnitBuf_BloodCrystalThorn.GainBuf<BattleUnitBuf_BloodCrystalThorn>(target, target.bufListDetail.GetKewordBufStack(KeywordBuf.Bleeding));
        }
    }
    public class DiceCardAbility_DonEyuil_31 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]将自身的[流血]转移至目标";
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            target.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Bleeding, owner.bufListDetail.GetKewordBufStack(KeywordBuf.Bleeding));
            owner.bufListDetail.RemoveBufAll(KeywordBuf.Bleeding);
        }
    }
    public class DiceCardAbility_DonEyuil_33 : RedDiceCardAbility
    {
        public static string Desc = "[拼点胜利]摧毁目标书页所有骰子[命中时]施加2层[流血](重复触发3次)";

        public override void OnWinParrying()
        {
            if(card.target != null && card.target.currentDiceAction != null)
            {
                card.target.currentDiceAction.DestroyDice(DiceMatch.AllDice);
            }

        }
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            for (int i = 0; i < 3; i++)
            {
                target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, 2, owner);
            }
        }
    }
    public class DiceCardAbility_DonEyuil_37 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]下一幕对自身施加3层[流血]";
        public override void OnSucceedAttack()
        {
            owner.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, 3,owner);
        }
    }
    public class DiceCardAbility_DonEyuil_41 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]使目标这一幕获得[流血]时层数+1";
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            BattleUnitBuf_HardBloodBleedingAddition.GainBuf<BattleUnitBuf_HardBloodBleedingAddition>(target, 1);
        }
        public class BattleUnitBuf_HardBloodBleedingAddition : BattleUnitBuf_Don_Eyuil
        {
            public BattleUnitBuf_HardBloodBleedingAddition(BattleUnitModel model) : base(model) { }
            public override void OnRoundEnd()
            {
                this.Destroy();
            }
            public override void BeforeAddKeywordBuf(KeywordBuf BufType, ref int Stack)
            {
                if(BufType == KeywordBuf.Bleeding)
                {
                    Stack += 1;
                }
            }
        }
    }
    public class DiceCardAbility_DonEyuil_42 : DiceCardAbilityBase
    {
        public static string Desc = "[拼点胜利]对目标施加3层[流血]与[血晶荆棘]";
        public override void OnWinParrying()
        {
            if (card != null && card.target != null)
            {
                card.target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, 3, owner);
                BattleUnitBuf_BloodCrystalThorn.GainBuf<BattleUnitBuf_BloodCrystalThorn>(card.target, 3);
            }
        }
    }
    public class DiceCardAbility_DonEyuil_45 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]使自身获得5层[硬血结晶]";
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            BattleUnitBuf_HardBlood_Crystal.GainBuf<BattleUnitBuf_HardBlood_Crystal>(owner, 5);
        }
    }
    public class DiceCardAbility_DonEyuil_47 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]这一幕与下一幕施加4层[流血]";
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            target.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Bleeding, 4, owner);
            target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, 4, owner);
        }
    }
    public class DiceCardAbility_DonEyuil_48 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]消耗3层[结晶硬血]并施加3层[无法凝结的血]";
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            if (BattleUnitBuf_HardBlood_Crystal.UseBuf<BattleUnitBuf_HardBlood_Crystal>(target, 3))
            {
                BattleUnitBuf_UncondensableBlood.GainBuf<BattleUnitBuf_UncondensableBlood>(target, 3);
            }
        }
    }
    public class DiceCardAbility_DonEyuil_49 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]重复触发目标5次[流血]";
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            for (int i = 0; i < 5; i++)
            {
                if (target.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_bleeding) == null)
                {
                    return;
                }
                target.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_bleeding).AfterDiceAction(behavior);
            }
            BattleUnitBuf_Don_Eyuil.GetBuf<BattleUnitBuf_AddThistles>(owner)?.Destroy();
        }
    }
    public class DiceCardAbility_DonEyuil_78 : DiceCardAbilityBase
    {
        public static string Desc = "[拼点胜利]对目标施加1层[流血]";
        public override void OnWinParrying()
        {
            if(card.target!=null)
            {
                card.target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, 1, owner);
            }
        }
    }
    public class DiceCardAbility_DonEyuil_80 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]对目标施加2层[流血]";
        public override void OnSucceedAreaAttack(BattleUnitModel target)
        {
            target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, 2, owner);
        }
    }
    public class DiceCardAbility_DonEyuil_81 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]对目标施加8层[流血]与1层[虚弱]";
        public override void OnSucceedAreaAttack(BattleUnitModel target)
        {
            target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, 8, owner);
            target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Weak, 1, owner);
        }
    }
}
