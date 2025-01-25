using Don_Eyuil.Don_Eyuil.Player.Buff;
using System;

namespace Don_Eyuil.Don_Eyuil.Player.DiceCardSelfAbility
{
    public class DiceCardSelfAbility_DonEyuil_65 : DiceCardSelfAbilityBase
    {
        public static string Desc = "[使用时]抽取1张书页\r\n自身每激活了1张[硬血术]书页便使本书页额外命中一名目标(至多3名)\r\n";
        public override string[] Keywords => new string[] { "DonEyuil" };
        public override void OnUseCard()
        {
            owner.allyCardDetail.DrawCards(1);
        }
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            if (BattleUnitBuf_Don_Eyuil.GetBuf<BattleUnitBuf_HardBlood>(owner) == null)
            {
                return;
            }

            behavior.GiveDamage_SubTarget(card.target, Math.Min(3, BattleUnitBuf_Don_Eyuil.GetBuf<BattleUnitBuf_HardBlood>(owner).ActivatedNum));
            /*
            var temp = new List<BattleUnitModel>(BattleObjectManager.instance.GetAliveList_opponent(owner.faction));
            for (int i = 0; i < Math.Min(3, BattleUnitBuf_Don_Eyuil.GetBuf<BattleUnitBuf_HardBlood>(owner).ActivatedNum); i++)
            {
                if (temp.Count <= 0)
                {
                    return;
                }
                var luckyDog = RandomUtil.SelectOne(temp);
                temp.Remove(luckyDog);
                //Singleton<StageController>.Instance.dontUseUILog = true;
                behavior.GiveDamage_SubTarget(luckyDog);
                //Singleton<StageController>.Instance.dontUseUILog = false;
            }*/
        }
        public override void BeforeGiveDamage(BattleDiceBehavior behavior)
        {

        }
    }
}
