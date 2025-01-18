using Don_Eyuil.Buff;
using Don_Eyuil.Don_Eyuil.Buff;

namespace Don_Eyuil.Don_Eyuil.Player.DiceCardSelfAbility
{
    public class DiceCardSelfAbility_DonEyuil_62 : DiceCardSelfAbilityBase
    {
        public static string Desc = "本书页仅限自身设置的所有硬血术书页均已被激活后使用\r\n自身每设置一张硬血术书页便使本书页造成的伤害与混乱伤害增加30%\r\n";
        public override bool OnChooseCard(BattleUnitModel owner)
        {
            return owner.bufListDetail.HasBuf<BattleUnitBuf_Sword>() && owner.bufListDetail.HasBuf<BattleUnitBuf_Lance>() && owner.bufListDetail.HasBuf<BattleUnitBuf_Sickle>() && owner.bufListDetail.HasBuf<BattleUnitBuf_Blade>() && owner.bufListDetail.HasBuf<BattleUnitBuf_DoubleSwords>() && owner.bufListDetail.HasBuf<BattleUnitBuf_Armour>() && owner.bufListDetail.HasBuf<BattleUnitBuf_Bow>() && owner.bufListDetail.HasBuf<BattleUnitBuf_Scourge>() && owner.bufListDetail.HasBuf<BattleUnitBuf_Umbrella>();
        }
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            var temp = owner.UnitData.unitData.GetDeckForBattle(1).FindAll(x => x.Name.Contains("堂埃尤尔派硬血术")).Count;
            behavior.ApplyDiceStatBonus(new DiceStatBonus() { dmgRate = 30 * temp, breakRate = 30 * temp });
        }
    }
}
