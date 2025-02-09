using Don_Eyuil.WhiteMoon_Sparkle.Player.Buff;

namespace Don_Eyuil.WhiteMoon_Sparkle.Player.DiceCardAbility
{
    public class DiceCardAbility_WhiteMoonSparkle_Dice16 : DiceCardAbilityBase
    {
        public static string Desc = "若自身当前应用的副武器为千斤弓则本骰子额外命中2命目标";

        public override void OnSucceedAttack(BattleUnitModel target)
        {
            if (BattleUnitBuf_Sparkle.Instance.SubWeapons.Exists(x => x.GetType() == typeof(BattleUnitBuf_Bow)))
            {
                behavior.GiveDamage_SubTarget(target, 2);
            }
        }
    }
}
