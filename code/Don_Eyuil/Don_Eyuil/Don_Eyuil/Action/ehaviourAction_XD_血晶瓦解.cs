using System.Collections.Generic;
using static Don_Eyuil.TKS_BloodFiend_Initializer.TKS_EnumExtension;

namespace Don_Eyuil.Don_Eyuil.Action
{
    class BehaviourAction_XD_血晶瓦解_1 : BehaviourActionBase_Don_Eyuil
    {

        public override void GetMovingAction_DonEyuil(ref List<RencounterManager.MovingAction> LS, ref List<RencounterManager.MovingAction> LO, ref RencounterManager.ActionAfterBehaviour self, ref RencounterManager.ActionAfterBehaviour opponent)
        {
            Start(LO, ActionDetail.Damaged, CharMoveState.Stop, delay: 0.3f).SetEffectTiming(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE)

            .Next(LS, SMotionExtension.TKS_BL_S129, CharMoveState.Stop, delay: 0.3f)
            .SetEffectTiming(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE)

            .Next(LO, ActionDetail.Damaged, CharMoveState.Stop, delay: 0.3f)
            .SetEffectTiming(EffectTiming.PRE, EffectTiming.PRE, EffectTiming.PRE)

            .Next(LS, SMotionExtension.TKS_BL_S130, CharMoveState.MoveForward, 20, false, 0.3f, 10)
            .SetEffectTiming(EffectTiming.PRE, EffectTiming.PRE, EffectTiming.PRE)

            .Finish();
        }

    }

    class BehaviourAction_XD_血晶瓦解_2 : BehaviourActionBase_Don_Eyuil
    {

        public override void GetMovingAction_DonEyuil(ref List<RencounterManager.MovingAction> LS, ref List<RencounterManager.MovingAction> LO, ref RencounterManager.ActionAfterBehaviour self, ref RencounterManager.ActionAfterBehaviour opponent)
        {
            Start(LO, ActionDetail.Damaged, CharMoveState.Stop, delay: 0.3f)
            .SetEffectTiming(EffectTiming.PRE, EffectTiming.PRE, EffectTiming.PRE)

            .Next(LS, SMotionExtension.TKS_BL_S135, CharMoveState.Stop, delay: 0.3f)
            .SetEffectTiming(EffectTiming.PRE, EffectTiming.PRE, EffectTiming.PRE)

            .Finish();
        }

    }
}
