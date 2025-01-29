using LOR_BattleUnit_UI;
using System.Collections.Generic;

namespace Don_Eyuil.San_Sora.Player.Buff
{
    public class BattleUnitBuf_Blade : BattleUnitBuf_SanHardBlood
    {
        public static string Desc = "本速度骰子使用书页期间若自身”血羽”与”结晶硬血”层数之和不低于30则与目标拼点时目标受到的”流血”伤害将扩散至两名敌方角色";

        public BattleUnitBuf_Blade(SpeedDiceUI dice) : base(dice)
        {
            //dice.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = TKS_BloodFiend_Initializer.ArtWorks["血刃骰子"];
        }

        public override void AfterOtherUnitTakeBleedingDamage(BattleUnitModel Unit, int Dmg)
        {
            if (Unit == null || Unit.currentDiceAction == null || Unit.currentDiceAction.currentBehavior == null || Unit.currentDiceAction.currentBehavior.TargetDice == null) { return; }
            if (Unit == Card.target && Unit.currentDiceAction.currentBehavior.TargetDice.card == Card)
            {
                var list = new List<BattleUnitModel>(BattleObjectManager.instance.GetAliveList_opponent(_owner.faction));
                for (int i = 0; i < 2; i++)
                {
                    if (list.Count <= 0)
                    {
                        return;
                    }
                    var temp = RandomUtil.SelectOne(list);
                    list.Remove(temp);
                    temp.TakeDamage(Dmg, DamageType.Buf, null, KeywordBuf.Bleeding);
                    OnTakeBleedingDamagePatch.Trigger_BleedingDmg_After(temp, Dmg, KeywordBuf.Bleeding);
                }
            }
        }
    }
}
