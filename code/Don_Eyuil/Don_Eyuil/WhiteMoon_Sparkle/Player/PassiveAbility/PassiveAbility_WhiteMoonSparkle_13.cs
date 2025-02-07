using Don_Eyuil.WhiteMoon_Sparkle.Player.Buff;

namespace Don_Eyuil.WhiteMoon_Sparkle.Player.PassiveAbility
{
    public class PassiveAbility_WhiteMoonSparkle_13 : PassiveAbilityBase_Don_Eyuil
    {
        public static string Desc = "自身拥有三种武器\r\n开启舞台时可各选择一种主武器和副武器(可重复)若选择了同种武器则获得附加效果(每幕可在战斗准备阶段更改1次)\r\n自身情感等级达到2/4级时可额外选择1种武器强化/额外应用一种副武器\r\n场上每有3名敌方角色死亡则可额外选择一次上述效果 若友方角色死亡同样同样可以选择一次上述效果\r\n";

        public override void OnWaveStart()
        {
            BattleUnitBuf_Sparkle.Instance.SelectPrimaryWeapon();
            BattleUnitBuf_Sparkle.Instance.SelectSubWeapon();
        }

        bool fl2 = false, fl4 = false;

        public override void OnRoundStart()
        {
            if ((owner.emotionDetail.EmotionLevel == 2 && !fl2) || (owner.emotionDetail.EmotionLevel == 4 && !fl4))
            {
                if (owner.emotionDetail.EmotionLevel == 2)
                {
                    fl2 = true;
                }
                if (owner.emotionDetail.EmotionLevel == 4)
                {
                    fl4 = true;
                }
                BattleUnitBuf_Sparkle.Instance.SelectSubWeapon();
            }
        }
        int count = 0;
        public override void OnDieOtherUnit(BattleUnitModel unit)
        {
            if (unit.faction == Faction.Enemy)
            {
                count++;
            }
            if (unit.faction == Faction.Player)
            {
                BattleUnitBuf_Sparkle.Instance.SelectSubWeapon();
            }
            if (count % 3 == 0 && count > 0)
            {
                BattleUnitBuf_Sparkle.Instance.SelectSubWeapon();
            }
        }

    }
}
