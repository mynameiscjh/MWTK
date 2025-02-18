using System.Collections.Generic;
using static BattleFarAreaPlayManager;
using static Don_Eyuil.TKS_BloodFiend_Initializer.TKS_EnumExtension;

namespace Don_Eyuil.Don_Eyuil.Action
{
    public class BehaviourAction_XD_血鞭_1 : BehaviourActionBase
    {
        public override FarAreaEffect SetFarAreaAtkEffect(BattleUnitModel self)
        {
            var temp = self.view.gameObject.AddComponent<FarAreaEffect_XD_血鞭_1>();
            temp.Init(self);
            return temp;
        }

        public class FarAreaEffect_XD_血鞭_1 : FarAreaEffect
        {

            public override void Init(BattleUnitModel self, params object[] args)
            {
                base.Init(self, args);
                this.isRunning = false;
                self.view.charAppearance.ChangeMotion(SMotionExtension.TKS_BL_S77);
                this.state = FarAreaEffect.EffectState.None;
            }

            public override bool HasIndependentAction => true;

            float time = 0;

            List<ActionDetail> list = new List<ActionDetail>()
            {
                SMotionExtension.TKS_BL_S77,
                SMotionExtension.TKS_BL_S78,
                SMotionExtension.TKS_BL_S79,
                SMotionExtension.TKS_BL_S80,
                SMotionExtension.TKS_BL_S81,
                SMotionExtension.TKS_BL_S82,
                SMotionExtension.TKS_BL_S83,
                SMotionExtension.TKS_BL_S84,
                SMotionExtension.TKS_BL_S85,
            };

            public override bool ActionPhase(float deltaTime, BattleUnitModel attacker, List<VictimInfo> victims, ref List<VictimInfo> defenseVictims)
            {

                time += deltaTime;

                if (time > 0.3f)
                {
                    attacker.view.charAppearance.ChangeMotion(list[0]);
                    list.RemoveAt(0);
                    time = 0;
                    return false;
                }

                if (list.Count > 0)
                {
                    return false;
                }

                foreach (var item in victims)
                {
                    attacker.currentDiceAction.currentBehavior.GiveDamage(item.unitModel);
                }

                return true;
            }
        }
    }
}
