namespace Don_Eyuil.WhiteMoon_Sparkle.Player.DiceCardAbility
{
    public class DiceCardAbility_WhiteMoonSparkle_Dice34 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]使本书页剩余骰子额外命中1名敌方角色";

        public class DiceCardAbility_骰子额外命中1名敌方角色 : DiceCardAbilityBase
        {
            public override void OnSucceedAttack(BattleUnitModel target)
            {
                behavior.GiveDamage_SubTarget(target, 1);
            }
        }

        public override void OnSucceedAttack()
        {
            card.ApplyDiceAbility(DiceMatch.AllDice, new DiceCardAbility_骰子额外命中1名敌方角色());
        }
    }
}
