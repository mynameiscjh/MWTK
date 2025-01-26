using Don_Eyuil.San_Sora.Player.Buff;

namespace Don_Eyuil.San_Sora.Player.PassiveAbility
{
    public class PassiveAbility_SanSora_11 : PassiveAbilityBase_Don_Eyuil
    {
        public override string debugDesc => "命中目标时若自身速度高于目标则使自身获得2层”血羽”并恢复速度差值的体力(至多4点)\r\n自身恢复体力时若溢出则随机使一名友方角色获得1点正面情感(每幕至多触发3次)";

        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            if (behavior.card.speedDiceResultValue > behavior.TargetDice.card.speedDiceResultValue)
            {
                BattleUnitBuf_Don_Eyuil.GainBuf<BattleUnitBuf_Feather>(owner, 2);
                owner.RecoverHP(behavior.card.speedDiceResultValue - behavior.TargetDice.card.speedDiceResultValue);
            }
        }
        int fl = 0;

        public override void OnRoundStart()
        {
            fl = 0;
        }

        public override void BeforeRecoverHP(ref int v)
        {
            base.BeforeRecoverHP(ref v);
            if (v + owner.hp > owner.MaxHp && fl < 3)
            {
                RandomUtil.SelectOne(BattleObjectManager.instance.GetAliveList(owner.faction)).emotionDetail.CreateEmotionCoin(EmotionCoinType.Positive);
                fl++;
            }
        }
    }
}
