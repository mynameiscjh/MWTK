using Don_Eyuil.Don_Eyuil.Player.Buff;

namespace Don_Eyuil.Don_Eyuil.Player.DiceCardSelfAbility
{
    public class DiceCardSelfAbility_DonEyuil_66 : DiceCardSelfAbilityBase
    {
        public static string Desc = "自身每激活了1张[硬血术]书页便使本书页费用-1\r\n本书页将命中全体敌方角色\r\n";
        public override string[] Keywords => new string[] { "DonEyuil" };
        public override int GetCostAdder(BattleUnitModel unit, BattleDiceCardModel self)
        {
            var buf = BattleUnitBuf_Don_Eyuil.GetBuf<BattleUnitBuf_HardBlood>(unit);
            if (buf == null)
            {
                return 0;
            }
            return -buf.ActivatedNum;
        }
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            behavior.GiveDamage_SubTarget(card.target, -1);
            //BattleObjectManager.instance.GetAliveList_opponent(owner.faction).DoIf(y => y != card.target, x => behavior.GiveDamage_SubTarget(x));
        }
        public override void OnRollDice(BattleDiceBehavior behavior)
        {
            //Singleton<StageController>.Instance.dontUseUILog = true;
            //BattleObjectManager.instance.GetAliveList_opponent(owner.faction).DoIf(y => y != card.target, x => behavior.GiveDamage(x));
            //Singleton<StageController>.Instance.dontUseUILog = false;

        }
    }
}
