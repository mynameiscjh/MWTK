using Don_Eyuil.San_Sora.Player.Buff;

namespace Don_Eyuil.San_Sora.Player.DiceCardSelfAbility
{
    public class DiceCardSelfAbility_SanSora_07 : DiceCardSelfAbilityBase
    {
        public static string Desc = "自身当前每有一张已经生效的硬血术书页便使本书页费用-1 自身每消耗3层”血羽”或”结晶硬血”便使本书页获得1点冷却";

        public override int GetCostAdder(BattleUnitModel unit, BattleDiceCardModel self)
        {
            var buf = BattleUnitBuf_SanSora.GetBuf<BattleUnitBuf_SanSora>(unit);

            if (buf == null) { return 0; }
            int temp = 0;
            temp += buf.Sword != null ? 1 : 0;
            temp += buf.Lance != null ? 1 : 0;
            temp += buf.Blade != null ? 1 : 0;
            temp += buf.Bow != null ? 1 : 0;
            temp += buf.DoubleSwords != null ? 1 : 0;
            temp += buf.Scourge != null ? 1 : 0;
            temp += buf.Sickle != null ? 1 : 0;
            temp += buf.Armour != null ? 1 : 0;
            return -temp;
        }

    }
}
