using LOR_BattleUnit_UI;
using UnityEngine.UI;

namespace Don_Eyuil.San_Sora.Player.Buff
{
    public class BattleUnitBuf_Armour : BattleUnitBuf_SanHardBlood
    {
        public static string Desc = "自身”血羽”不低于10层时本速度骰子恢复体力溢出时将转化为等量护盾\r\n自身拥有护盾时使自身所有防御型骰子威力+2";

        public BattleUnitBuf_Armour(SpeedDiceUI dice) : base(dice)
        {
            dice.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = TKS_BloodFiend_Initializer.ArtWorks["这里要放图片哦"];
        }

        public override void BeforeRecoverHp(int v)
        {
            if (_owner.currentDiceAction == Card && _owner.hp + v > _owner.MaxHp)
            {
                GainBuf<BattleUnitBuf_BloodShield>(_owner, (int)_owner.hp + v - _owner.MaxHp);
            }
        }

        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            if (_owner.bufListDetail.HasBuf<BattleUnitBuf_BloodShield>() && behavior.Type == LOR_DiceSystem.BehaviourType.Def)
            {
                behavior.ApplyDiceStatBonus(new DiceStatBonus() { power = 2 });
            }
        }
    }
}
