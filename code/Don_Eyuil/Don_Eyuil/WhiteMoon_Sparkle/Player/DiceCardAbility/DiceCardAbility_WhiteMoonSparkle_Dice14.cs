using HarmonyLib;

namespace Don_Eyuil.WhiteMoon_Sparkle.Player.DiceCardAbility
{
    public class DiceCardAbility_WhiteMoonSparkle_Dice14 : DiceCardAbilityBase
    {
        public static string Desc = "本书页使用期间目标每承受10点非命中伤害便使本书页第2颗骰子重复投掷1次(至多3次)";

        [HarmonyPatch(typeof(BattleUnitModel), "OnStartBattle")]
        [HarmonyPostfix]
        public static void BattleUnitModel_OnStartBattle_Post(BattleUnitModel __instance)
        {
            foreach (var item in __instance.cardSlotDetail.cardAry)
            {
                foreach (var dice in item.cardBehaviorQueue)
                {
                    if (dice.abilityList.Exists(x => x.GetType() == typeof(DiceCardAbility_WhiteMoonSparkle_Dice14)))
                    {
                        BattleUnitBuf_Don_Eyuil.GainBuf<BattleUnitBuf_用来记录伤害的buff>(item.target, 1);
                    }
                }
            }
        }

        int count = 0;

        public override void OnRollDice()
        {
            var buf = BattleUnitBuf_Don_Eyuil.GetBuf<BattleUnitBuf_用来记录伤害的buff>(card.target);
            if (buf != null && buf.count >= 10 && count < 3)
            {
                buf.count -= 10;
                count++;
                behavior.isBonusAttack = true;
            }
        }

        public class BattleUnitBuf_用来记录伤害的buff : BattleUnitBuf_Don_Eyuil
        {
            public int count = 0;

            public BattleUnitBuf_用来记录伤害的buff(BattleUnitModel model) : base(model)
            {
            }

            public override float DmgFactor(int dmg, DamageType type = DamageType.ETC, KeywordBuf keyword = KeywordBuf.None)
            {
                if (type != DamageType.Attack)
                {
                    count += dmg;
                }

                return base.DmgFactor(dmg, type, keyword);
            }

            public override void OnRoundEnd()
            {
                Destroy();
            }
        }
    }
}
