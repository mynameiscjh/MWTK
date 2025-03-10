using System.Collections.Generic;
using static Don_Eyuil.TKS_BloodFiend_Initializer.TKS_EnumExtension;

namespace Don_Eyuil.Don_Eyuil.Action
{
    public class BehaviourAction_XD_流淌于心_2 : BehaviourActionBase_Don_Eyuil
    {
        public override void GetMovingAction_DonEyuil(ref List<RencounterManager.MovingAction> LS, ref List<RencounterManager.MovingAction> LO, ref RencounterManager.ActionAfterBehaviour self, ref RencounterManager.ActionAfterBehaviour opponent)
        {
            Start(LS, SMotionExtension.TKS_BL_S113, CharMoveState.Stop, delay: 0.3f).SetEffectTiming(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Next(LO, ActionDetail.Damaged, CharMoveState.Stop, delay: 0.3f).SetEffectTiming(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Start(LS, SMotionExtension.TKS_BL_S112, CharMoveState.Stop, delay: 0.3f).SetEffectTiming(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Next(LO, ActionDetail.Damaged, CharMoveState.Stop, delay: 0.3f).SetEffectTiming(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Start(LS, SMotionExtension.TKS_BL_S138, CharMoveState.Stop, delay: 0.3f).SetEffectTiming(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Next(LO, ActionDetail.Damaged, CharMoveState.Stop, delay: 0.3f).SetEffectTiming(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Start(LS, SMotionExtension.TKS_BL_S139, CharMoveState.Stop, delay: 0.3f).SetEffectTiming(EffectTiming.PRE, EffectTiming.PRE, EffectTiming.PRE).Next(LO, ActionDetail.Damaged, CharMoveState.Stop, delay: 0.3f).SetEffectTiming(EffectTiming.PRE, EffectTiming.PRE, EffectTiming.PRE).Finish();
        }
    }
}
