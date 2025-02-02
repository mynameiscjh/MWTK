using Don_Eyuil.San_Sora.Player.Buff;

namespace Don_Eyuil.San_Sora.Player.DiceCardSelfAbility
{
    public class DiceCardSelfAbility_SanSora_13 : DiceCardSelfAbilityBase
    {
        public static string Desc = "[使用时]抽取1张书页并获得5层[血羽]";

        public override void OnUseCard()
        {
            owner.allyCardDetail.DrawCards(1);
            BattleUnitBuf_Don_Eyuil.GainBuf<BattleUnitBuf_Feather>(owner, 5);
        }
    }
}
