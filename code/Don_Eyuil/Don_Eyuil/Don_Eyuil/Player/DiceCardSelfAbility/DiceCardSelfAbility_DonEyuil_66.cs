using Don_Eyuil.Don_Eyuil.DiceCardSelfAbility;
using Don_Eyuil.Don_Eyuil.Player.Buff;

namespace Don_Eyuil.Don_Eyuil.Player.DiceCardSelfAbility
{
    public class DiceCardSelfAbility_DonEyuil_66 : DiceCardSelfAbilityBase
    {
        public static string Desc = "自身每激活了1张[硬血术]书页便使本书页费用-1\r\n本书页将命中全体敌方角色\r\n";
        public override int GetCostAdder(BattleUnitModel unit, BattleDiceCardModel self)
        {
            return -BattleUnitBuf_Don_Eyuil.GetBuf<BattleUnitBuf_HardBlood>(unit).ActivatedNum;
        }
        public override void OnRollDice(BattleDiceBehavior behavior)
        {
            foreach (var item in BattleObjectManager.instance.GetAliveList_opponent(owner.faction))
            {
                DiceCardSelfAbility_DonEyuil_01.GiveDamageForSubTarget(behavior, item);
            }
        }
    }
}
