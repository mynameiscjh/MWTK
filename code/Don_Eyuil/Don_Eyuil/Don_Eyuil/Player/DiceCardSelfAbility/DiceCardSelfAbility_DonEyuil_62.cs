using Don_Eyuil.Don_Eyuil.Player.Buff;
using Don_Eyuil.PassiveAbility;

namespace Don_Eyuil.Don_Eyuil.Player.DiceCardSelfAbility
{
    public class DiceCardSelfAbility_DonEyuil_62 : DiceCardSelfAbilityBase
    {
        public static string Desc = "本书页仅限自身设置的所有硬血术书页均已被激活后使用\r\n自身每设置一张硬血术书页便使本书页造成的伤害与混乱伤害增加30%\r\n";
        public override bool OnChooseCard(BattleUnitModel owner)
        {
            var temp = BattleUnitBuf_Don_Eyuil.GetBuf<BattleUnitBuf_HardBlood>(owner);
            return owner.UnitData.unitData.GetDeckForBattle(1).FindAll(x => PassiveAbility_DonEyuil_15.HardBloodCards.Contains(x.id)).Count == (temp == null ? 0 : temp.ActivatedNum);
        }
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            var temp = owner.UnitData.unitData.GetDeckForBattle(1).FindAll(x => PassiveAbility_DonEyuil_15.HardBloodCards.Contains(x.id)).Count;
            behavior.ApplyDiceStatBonus(new DiceStatBonus() { dmgRate = 30 * temp, breakRate = 30 * temp });
        }
    }
}
