using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using static Don_Eyuil.TKS_BloodFiend_Initializer.TKS_EnumExtension;

namespace Don_Eyuil.Don_Eyuil.Action
{
    public class BehaviourAction_XD_辉耀血光_1 : BehaviourActionBase_Don_Eyuil
    {
        public override void GetMovingAction_DonEyuil(ref List<RencounterManager.MovingAction> LS, ref List<RencounterManager.MovingAction> LO, ref RencounterManager.ActionAfterBehaviour self, ref RencounterManager.ActionAfterBehaviour opponent)
        {

            Start(LS, SMotionExtension.TKS_BL_S134, CharMoveState.Stop, delay: 0.3f)
            .SetEffectTiming(EffectTiming.PRE, EffectTiming.NONE, EffectTiming.NONE)

                .Next(LO, ActionDetail.Damaged, CharMoveState.Stop, delay: 0.6f)
                .SetEffectTiming(EffectTiming.PRE, EffectTiming.NONE, EffectTiming.NONE)

                .Start(LS, SMotionExtension.TKS_BL_S128, CharMoveState.MoveForward, 2, false, 0.6f, 2)
                .SetEffectTiming(EffectTiming.PRE, EffectTiming.NONE, EffectTiming.NONE)

                .Next(LO, ActionDetail.Damaged, CharMoveState.Custom, delay: 0.3f)
                .WithDOTWeen(opponent.view.transform.DOMove(new Vector3(10 * (opponent.view.model.direction == Direction.RIGHT ? -1 : 1), 10), 0.3f).SetRelative().SetEase(Ease.OutCubic))
                .SetEffectTiming(EffectTiming.PRE, EffectTiming.NONE, EffectTiming.NONE)

                .Next(LS, SMotionExtension.TKS_BL_S132, CharMoveState.Custom, delay: 0.3f)
                .WithDOTWeen(self.view.transform.DOMove(opponent.view.WorldPosition + new Vector3(10 * (opponent.view.model.direction == Direction.RIGHT ? -1 : 1), 10), 0.1f).SetEase(Ease.OutCubic))
                .SetEffectTiming(EffectTiming.POST, EffectTiming.POST, EffectTiming.POST)

                .Next(LO, ActionDetail.Damaged, CharMoveState.Stop, delay: 0.3f)
                .SetEffectTiming(EffectTiming.PRE, EffectTiming.NONE, EffectTiming.NONE)

                .Next(LO, ActionDetail.Damaged, CharMoveState.Custom, delay: 0.3f)
                .WithDOTWeen(opponent.view.transform.DOMove(new Vector3(Mathf.Sin(Mathf.Deg2Rad * 25) * 10 * (opponent.view.model.direction == Direction.RIGHT ? -1 : 1), -10), 0.3f).SetRelative().SetEase(Ease.InCubic))
                .SetEffectTiming(EffectTiming.POST, EffectTiming.POST, EffectTiming.POST)

                .Next(LS, SMotionExtension.TKS_BL_S132, CharMoveState.Custom, delay: 0.3f)
                .WithDOTWeen(self.view.transform.DOMove(new Vector3(Mathf.Sin(Mathf.Deg2Rad * 40) * 10 * (opponent.view.model.direction == Direction.RIGHT ? -1 : 1), -10), 0.3f).SetRelative().SetEase(Ease.InCubic))
                .SetEffectTiming(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE)
            .Finish();
        }
    }

    public class BehaviourAction_XD_辉耀血光_2 : BehaviourActionBase_Don_Eyuil
    {
        public override void GetMovingAction_DonEyuil(ref List<RencounterManager.MovingAction> LS, ref List<RencounterManager.MovingAction> LO, ref RencounterManager.ActionAfterBehaviour self, ref RencounterManager.ActionAfterBehaviour opponent)
        {
            Start(LS, SMotionExtension.TKS_BL_S44, CharMoveState.MoveForward, 15, false, 0.3f, 5).SetEffectTiming(EffectTiming.PRE, EffectTiming.PRE, EffectTiming.PRE)

                .Next(LO, ActionDetail.Damaged, CharMoveState.Stop, delay: 0.3f).SetEffectTiming(EffectTiming.PRE, EffectTiming.PRE, EffectTiming.PRE)

            .Finish();
        }
    }
}
