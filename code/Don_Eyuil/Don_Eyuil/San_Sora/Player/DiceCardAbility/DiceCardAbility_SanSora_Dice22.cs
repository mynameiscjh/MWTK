using Don_Eyuil.San_Sora.Player.Buff;

namespace Don_Eyuil.San_Sora.Player.DiceCardAbility
{
    public class DiceCardAbility_SanSora_Dice22 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]获得2层[血羽]";
        public override void OnSucceedAttack()
        {
            BattleUnitBuf_Don_Eyuil.GainBuf<BattleUnitBuf_Feather>(owner, 2);
        }
    }
}
