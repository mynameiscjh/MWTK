using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;

namespace Don_Eyuil.WhiteMoon_Sparkle.Player.Buff
{
    public class BattleUnitBuf_Sword : BattleUnitBuf_Don_Eyuil
    {
        public static string Desc =
            "月之剑（主）[中立buff]:\r\n所有骰子威力+1\r\n每一幕开始时赋予手中所有书页4种月相标记(以残月>弦月>凸月>满月的顺序施加 若已拥有标记则刷新标记)\r\n若自身以正确顺序使用标记书页 则推进月相变化(正确顺序为残月>弦月>凸月>满月  根据速度骰子从左到右判断使用顺序 从残月开始计算)\r\n根据本幕推进到的月相结果使下一幕获得对应月相buff\r\n" +
            "\r\n月之剑（副）[中立buff]:\r\n无视自身的伤害降低效果与目标不高于30%或100的减伤效果(不包括抗性)\r\n" +
            "\r\n月之剑+（强化主）[中立buff]:\r\n所有骰子威力+2\r\n速度骰子最小值+2 \r\n每一幕开始时赋予手中所有书页4种月相标记（以残月>弦月>凸月>满月的顺序施加 若已拥有标记则刷新标记）\r\n若自身以正确顺序使用标记书页 则推进月相变化（正确顺序为残月>弦月>凸月>满月  根据速度骰子从左到右判断使用顺序 从残月开始计算）\r\n根据本幕推进到的月相结果使下一幕获得对应月相buff\r\n" +
            "\r\n月之剑+（强化副）[中立buff]:\r\n无视自身的伤害降低效果与目标不高于99%或100的减伤效果（不包括抗性）\r\n若目标无减伤效果则额外触发一次命中时效果(每幕至多触发1次)\r\n" +
            "\r\n月之剑（主副同用额外效果）[中立buff]:\r\n速度骰子+1\r\n每幕首颗速度骰子中使用的书页下一幕抽回至手中（不对ego书页生效）\r\n";

        public bool IsIntensify = false;

        [HarmonyPatch(typeof(BattleDiceBehavior), "GiveDamage")]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> BattleDiceBehavior_GiveDamage_Tran(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
            for (int i = 0; i < codes.Count; i++)
            {
                if (codes[i].opcode == OpCodes.Ldloc_S && codes[i].ToString().Contains("13") && codes[i + 1].opcode == OpCodes.Ldc_I4_0 && codes[i + 2].opcode == OpCodes.Ldarg_0 && codes[i + 3].opcode == OpCodes.Call && codes[i + 3].operand.ToString().Contains("get_owner") && codes[i + 4].opcode == OpCodes.Ldc_I4_0 && codes[i + 5].Calls(AccessTools.Method(typeof(BattleUnitModel), "TakeDamage")))
                {
                    codes.InsertRange(i + 1, new List<CodeInstruction>
                    {
                        new CodeInstruction(OpCodes.Ldarg_0),
                        CodeInstruction.Call(typeof(BattleUnitBuf_Sword), "ChangeDamage")
                    });
                    break;
                }
            }
            return codes.AsEnumerable();
        }

        public static bool fl_ChangeDamage = false;

        public static int ChangeDamage(int oldDmg, BattleDiceBehavior behavior)
        {
            if (behavior == null || behavior.card.target == null || !behavior.card.owner.bufListDetail.HasBuf<BattleUnitBuf_Sword>())
            {
                return oldDmg;
            }
            var buf = BattleUnitBuf_Don_Eyuil.GetBuf<BattleUnitBuf_Sword>(behavior.card.owner);
            var atkResist = behavior.card.target.GetResistHP(behavior.Detail);
            var dmg = (int)(behavior.DiceResultValue * BookModel.GetResistRate(atkResist));
            if (dmg < oldDmg)
            {
                return oldDmg;
            }
            if (BattleUnitBuf_Sparkle.Instance.PrimaryWeapon == buf && !buf.IsIntensify)
            {
                if (dmg * 0.7 >= oldDmg)
                {
                    return dmg;
                }
                if (dmg - 100 >= oldDmg)
                {
                    return dmg;
                }
            }
            if (BattleUnitBuf_Sparkle.Instance.PrimaryWeapon == buf && buf.IsIntensify)
            {
                if (!fl_ChangeDamage)
                {
                    behavior.owner.OnSucceedAttack(behavior);
                    fl_ChangeDamage = true;
                }

                if (dmg * 0.99 >= oldDmg)
                {
                    return dmg;
                }
                if (dmg - 100 >= oldDmg)
                {
                    return dmg;
                }
            }
            return oldDmg;
        }

        public class BattleDiceCardBuf_Moon : BattleDiceCardBuf
        {
            public BattleDiceCardBuf_Moon()
            {
                this._stack = 0;
                this.SetFieldValue<bool>("_iconInit", true);
                this.SetFieldValue<Sprite>("_bufIcon", TKS_BloodFiend_Initializer.ArtWorks[$"Moon{this._stack}"]);
            }

            public BattleDiceCardBuf_Moon(int v)
            {
                this._stack = v;
                this.SetFieldValue<bool>("_iconInit", true);
                this.SetFieldValue<Sprite>("_bufIcon", TKS_BloodFiend_Initializer.ArtWorks[$"Moon{this._stack}"]);
            }

            public override void OnRoundEnd()
            {
                Destroy();
            }

            public void Change(int v)
            {
                this._stack = v;
                this.SetFieldValue<Sprite>("_bufIcon", TKS_BloodFiend_Initializer.ArtWorks[$"Moon{this._stack}"]);
            }

            public static void ChangeMoon(BattleUnitModel owner, int v)
            {
                owner.bufListDetail.RemoveBufAll(typeof(BattleUnitBuf_NewMoon));
                owner.bufListDetail.RemoveBufAll(typeof(BattleUnitBuf_QuarterMoon));
                owner.bufListDetail.RemoveBufAll(typeof(BattleUnitBuf_GibbousMoon));
                owner.bufListDetail.RemoveBufAll(typeof(BattleUnitBuf_FullMoon));
                switch (v)
                {
                    case 0:
                        BattleUnitBuf_Don_Eyuil.GainBuf<BattleUnitBuf_NewMoon>(owner, 1);
                        break;
                    case 1:
                        BattleUnitBuf_Don_Eyuil.GainBuf<BattleUnitBuf_QuarterMoon>(owner, 1);
                        break;
                    case 2:
                        BattleUnitBuf_Don_Eyuil.GainBuf<BattleUnitBuf_GibbousMoon>(owner, 1);
                        break;
                    case 3:
                        BattleUnitBuf_Don_Eyuil.GainBuf<BattleUnitBuf_FullMoon>(owner, 1);
                        break;
                    default:
                        break;
                }
            }

            public class BattleUnitBuf_NewMoon : BattleUnitBuf_Don_Eyuil
            {
                public static string Desc = "这一幕自身命中目标时使自身与体力最低的1名我方角色恢复2点体力与混乱抗性";

                public override void OnSuccessAttack(BattleDiceBehavior behavior)
                {
                    var temp = BattleObjectManager.instance.GetAliveList(owner.faction);
                    temp.Sort((x, y) => (int)(x.hp - y.hp));
                    temp.First().RecoverHP(2);
                    temp.First().RecoverBreakLife(2);
                    _owner.RecoverHP(2);
                    _owner.RecoverBreakLife(2);
                }

                public BattleUnitBuf_NewMoon(BattleUnitModel model) : base(model)
                {
                }
            }

            public class BattleUnitBuf_QuarterMoon : BattleUnitBuf_Don_Eyuil
            {
                public static string Desc = "自身获得的情感硬币数+1 获得的情感硬币溢出时使所有友方角色获得相同的情感硬币";

                public override void BeforeAddEmotionCoin(EmotionCoinType CoinType, ref int Count)
                {
                    Count++;
                    if (_owner.emotionDetail.AllEmotionCoins.Count >= _owner.emotionDetail.MaximumCoinNumber)
                    {
                        BattleObjectManager.instance.GetAliveList(_owner.faction).Where(x => x.Book.BookId != MyId.Book_堂_埃尤尔之页 && x != _owner).Do(x => x.emotionDetail.CreateEmotionCoin(CoinType, 1));
                    }
                }

                public BattleUnitBuf_QuarterMoon(BattleUnitModel model) : base(model)
                {
                }
            }

            public class BattleUnitBuf_GibbousMoon : BattleUnitBuf_Don_Eyuil
            {
                public static string Desc = "自身命中时造成的伤害减少至0%但命中时追加点数80%的追加伤害\r\n自身可无视速度拉取指向体力最低友方角色的书页\r\n";

                [HarmonyPatch(typeof(BattleUnitModel), "CanChangeAttackTarget")]
                [HarmonyPostfix]
                public static void BattleUnitModel_CanChangeAttackTarget_Post(BattleUnitModel target, int targetIndex, BattleUnitModel __instance, ref bool __result)
                {
                    if (target.cardSlotDetail.cardAry[targetIndex] == null)
                    {
                        return;
                    }

                    if (__instance.Book.BookId == MyId.Book_小耀之页 && (GetBuf<BattleUnitBuf_GibbousMoon>(__instance) != null || GetBuf<BattleUnitBuf_FullMoon>(__instance) != null))
                    {
                        var temp = BattleObjectManager.instance.GetAliveList(__instance.faction);
                        temp.Sort((x, y) => (int)(x.hp - y.hp));
                        if (temp[0] == null)
                        {
                            return;
                        }
                        if (target.cardSlotDetail.cardAry[targetIndex].target.hp == temp[0].hp)
                        {
                            __result = true;
                        }
                    }
                }

                public override void BeforeGiveDamage(BattleDiceBehavior behavior)
                {
                    behavior.ApplyDiceStatBonus(new DiceStatBonus() { dmgRate = -100 });
                }

                public override void OnSuccessAttack(BattleDiceBehavior behavior)
                {
                    behavior.card.target.TakeDamage((int)(behavior.DiceResultValue * 0.8));
                }

                public BattleUnitBuf_GibbousMoon(BattleUnitModel model) : base(model)
                {
                }
            }

            public class BattleUnitBuf_FullMoon : BattleUnitBuf_Don_Eyuil
            {
                public static string Desc = "这一幕自身命中目标时使自身与体力最低的1名我方角色恢复2点体力与混乱抗性\r\n自身获得的情感硬币数+1 获得的情感硬币溢出时使所有友方角色获得相同的情感硬币 以此方法获得3枚正面/负面情感硬币的友方角色获得1层强壮/忍耐\r\n自身命中时造成的伤害减少至20%但命中时追加点数80%的追加伤害\r\n自身可无视速度拉取指向友方角色的书页\r\n";

                public Dictionary<BattleUnitModel, int[]> dic = new Dictionary<BattleUnitModel, int[]>();

                public override void BeforeAddEmotionCoin(EmotionCoinType CoinType, ref int Count)
                {
                    Count++;
                    if (_owner.emotionDetail.AllEmotionCoins.Count >= _owner.emotionDetail.MaximumCoinNumber)
                    {
                        BattleObjectManager.instance.GetAliveList(_owner.faction).Where(x => x.Book.BookId != MyId.Book_堂_埃尤尔之页 && x != _owner).Do(x =>
                        {
                            x.emotionDetail.CreateEmotionCoin(CoinType, 1);
                            if (dic.ContainsKey(x))
                            {
                                switch (CoinType)
                                {
                                    case EmotionCoinType.Positive:
                                        dic[x][0]++;
                                        break;
                                    case EmotionCoinType.Negative:
                                        dic[x][1]++;
                                        break;
                                }
                            }
                            else
                            {
                                dic.Add(x, new int[2] { CoinType == EmotionCoinType.Positive ? 1 : 0, CoinType == EmotionCoinType.Negative ? 1 : 0 });
                            }
                            if (dic[x][0] >= 3)
                            {
                                x.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Strength, 1);
                            }
                            if (dic[x][1] >= 3)
                            {
                                x.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Endurance, 1);
                            }
                        });
                    }
                }

                public override void BeforeGiveDamage(BattleDiceBehavior behavior)
                {
                    behavior.ApplyDiceStatBonus(new DiceStatBonus() { dmgRate = -80 });
                }

                public override void OnSuccessAttack(BattleDiceBehavior behavior)
                {
                    behavior.card.target.TakeDamage((int)(behavior.DiceResultValue * 0.8));

                    var temp = BattleObjectManager.instance.GetAliveList(owner.faction);
                    temp.Sort((x, y) => (int)(x.hp - y.hp));
                    temp.First().RecoverHP(2);
                    temp.First().RecoverBreakLife(2);
                    _owner.RecoverHP(2);
                    _owner.RecoverBreakLife(2);
                }

                public BattleUnitBuf_FullMoon(BattleUnitModel model) : base(model)
                {
                }
            }
        }

        public override int SpeedDiceNumAdder()
        {
            if (BattleUnitBuf_Sparkle.Instance.PrimaryWeapon == this && BattleUnitBuf_Sparkle.Instance.SubWeapon == this)
            {
                return 1;
            }

            return base.SpeedDiceNumAdder();
        }


        public override void OnRoundStart()
        {
            if (BattleUnitBuf_Sparkle.Instance.PrimaryWeapon == this && IsIntensify)
            {
                _owner.Book.SetSpeedDiceMin(_owner.Book.ClassInfo.EquipEffect.SpeedMin + 2);
            }
            if (BattleUnitBuf_Sparkle.Instance.PrimaryWeapon == this)
            {
                foreach (var item in _owner.allyCardDetail.GetHand())
                {
                    item.AddBuf(new BattleDiceCardBuf_Moon(Random.Range(0, 3)));
                }
            }
            fl_ChangeDamage = false;
        }

        public override void OnStartBattle()
        {
            if (BattleUnitBuf_Sparkle.Instance.PrimaryWeapon == this)
            {
                int currentStage = -1;
                foreach (var item in _owner.cardSlotDetail.cardAry)
                {
                    if (item.card.HasBuf<BattleDiceCardBuf_Moon>())
                    {
                        var buf = item.card.GetBufList().Find(x => x.GetType() == typeof(BattleDiceCardBuf_Moon)) as BattleDiceCardBuf_Moon;
                        if (currentStage + 1 == buf.Stack)
                        {
                            currentStage++;
                        }
                        else
                        {
                            currentStage = -1;
                        }
                    }
                }
                if (currentStage == -1)
                {
                    currentStage = 0;
                }
                BattleDiceCardBuf_Moon.ChangeMoon(_owner, currentStage);
            }
        }

        public override void OnRoundEnd()
        {
            if (BattleUnitBuf_Sparkle.Instance.PrimaryWeapon == this && BattleUnitBuf_Sparkle.Instance.SubWeapon == this)
            {
                if (_owner.cardSlotDetail.cardAry[0] == null)
                {
                    return;
                }
                _owner.allyCardDetail.DrawCardsAllSpecific(_owner.cardSlotDetail.cardAry[0].card.GetID());
            }
        }

        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            if (BattleUnitBuf_Sparkle.Instance.PrimaryWeapon == this)
            {
                behavior.ApplyDiceStatBonus(new DiceStatBonus() { power = IsIntensify ? 2 : 1 });
            }
        }

        public BattleUnitBuf_Sword(BattleUnitModel model) : base(model)
        {
        }
    }
}
