using System.Collections.Generic;
using static BattleFarAreaPlayManager;
using static Don_Eyuil.TKS_BloodFiend_Initializer.TKS_EnumExtension;

namespace Don_Eyuil.Don_Eyuil.Action
{
    public class BehaviourAction_XD_血甲_1 : BehaviourActionBase
    {
        public override FarAreaEffect SetFarAreaAtkEffect(BattleUnitModel self)
        {
            var temp = self.view.gameObject.AddComponent<FarAreaEffect_XD_血甲_1>();
            temp.Init(self);
            return temp;
        }

        public class FarAreaEffect_XD_血甲_1 : FarAreaEffect
        {

            public override void Init(BattleUnitModel self, params object[] args)
            {
                base.Init(self, args);
                this.isRunning = false;
                self.view.charAppearance.ChangeMotion(ActionDetail.Aim);
                this.state = FarAreaEffect.EffectState.None;
            }

            public override bool HasIndependentAction => true;

            float time = 0;
            public override bool ActionPhase(float deltaTime, BattleUnitModel attacker, List<VictimInfo> victims, ref List<VictimInfo> defenseVictims)
            {

                time += deltaTime;

                if (time < 0.1f)
                {
                    attacker.view.charAppearance.ChangeMotion(ActionDetail.Aim);
                    return false;
                }

                if (time < 0.2f)
                {
                    attacker.view.charAppearance.ChangeMotion(SMotionExtension.TKS_BL_S75);
                    return false;
                }
                isRunning = false;

                foreach (var item in victims)
                {
                    attacker.currentDiceAction.currentBehavior.GiveDamage(item.unitModel);
                }

                return true;
            }
        }
    }


    public class BehaviourAction_XD_血甲_2_3 : BehaviourActionBase
    {
        public override FarAreaEffect SetFarAreaAtkEffect(BattleUnitModel self)
        {
            var temp = self.view.gameObject.AddComponent<FarAreaEffect_XD_血甲_2>();
            temp.Init(self);
            return temp;
        }

        public class FarAreaEffect_XD_血甲_2 : FarAreaEffect
        {

            public override void Init(BattleUnitModel self, params object[] args)
            {
                base.Init(self, args);
                this.isRunning = false;
                self.view.charAppearance.ChangeMotion(SMotionExtension.TKS_BL_S75);
                this.state = FarAreaEffect.EffectState.None;
            }

            public override bool HasIndependentAction => true;

            float time = 0;
            public override bool ActionPhase(float deltaTime, BattleUnitModel attacker, List<VictimInfo> victims, ref List<VictimInfo> defenseVictims)
            {

                time += deltaTime;

                if (time < 0.2f)
                {
                    attacker.view.charAppearance.ChangeMotion(SMotionExtension.TKS_BL_S75);
                    return false;
                }
                isRunning = false;

                foreach (var item in victims)
                {
                    attacker.currentDiceAction.currentBehavior.GiveDamage(item.unitModel);
                }

                return true;
            }
        }
    }
}
