using System.Collections.Generic;
using UnityEngine;
using static Don_Eyuil.TKS_BloodFiend_Initializer.TKS_EnumExtension;
using static RencounterManager;

namespace Don_Eyuil.Don_Eyuil.Action
{
    public class BehaviourAction_XD_血伞_1 : BehaviourActionBase
    {
        public override List<RencounterManager.MovingAction> GetMovingAction(ref RencounterManager.ActionAfterBehaviour self, ref RencounterManager.ActionAfterBehaviour opponent)
        {
            if (self.result != Result.Win)
            {
                return new List<MovingAction>();
            }

            opponent.infoList.Clear();
            var list_self = new List<RencounterManager.MovingAction>();
            var list_opponent = new List<RencounterManager.MovingAction>();

            new MovingAction(ActionDetail.Penetrate, CharMoveState.Stop, delay: 0.3f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_opponent);

            new MovingAction(SMotionExtension.TKS_BL_S86, CharMoveState.Stop, delay: 0.3f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_self);

            new MovingAction(ActionDetail.Damaged, CharMoveState.MoveBack, 5, false, 0.3f, 3).SetEffectTiming_Tool(EffectTiming.PRE, EffectTiming.NONE, EffectTiming.NONE).Add(list_opponent);

            new MovingAction(SMotionExtension.TKS_BL_S87, CharMoveState.MoveBack, 2, false, 0.3f, 3).SetEffectTiming_Tool(EffectTiming.PRE, EffectTiming.NONE, EffectTiming.NONE).Add(list_self);

            new MovingAction(SMotionExtension.TKS_BL_S88, CharMoveState.Stop, delay: 0.3f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_self);

            var selfView = self.view;
            var opponentView = opponent.view;
            var v = Vector3.zero;
            new MovingAction(SMotionExtension.TKS_BL_S89, CharMoveState.Custom, delay: 0.3f).SetCustomMoving_Tool((deltaTime, time) =>
            {
                selfView.WorldPosition = Vector3.SmoothDamp(selfView.WorldPosition, opponentView.WorldPosition + new Vector3(4, 5), ref v, 0.1f);

                if (Vector3.Distance(selfView.WorldPosition, opponentView.WorldPosition + new Vector3(4, 5)) <= 0.1f)
                {
                    //selfView.LockPosY(true);
                    return true;
                }

                return false;
            }).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_self);

            for (int i = 0; i < 5; i++)
            {
                new MovingAction(SMotionExtension.TKS_BL_S90, CharMoveState.Stop, delay: 0.1f).SetEffectTiming_Tool(EffectTiming.PRE, EffectTiming.NONE, EffectTiming.NONE).Add(list_self);

                new MovingAction(SMotionExtension.TKS_BL_S91, CharMoveState.Stop, delay: 0.1f).SetEffectTiming_Tool(EffectTiming.PRE, EffectTiming.NONE, EffectTiming.NONE).Add(list_self);
            }

            Vector3 v2 = new Vector3(0, 20, 0);
            new MovingAction(SMotionExtension.TKS_BL_S92, CharMoveState.Custom, delay: 0.3f).SetCustomMoving_Tool((deltaTime, time) =>
            {
                selfView.WorldPosition = Vector3.SmoothDamp(selfView.WorldPosition, opponentView.WorldPosition + new Vector3(10, 0), ref v2, 0.4f);

                if (Vector3.Distance(selfView.WorldPosition, opponentView.WorldPosition + new Vector3(10, 0)) <= 0.1f)
                {
                    //selfView.LockPosY(true);
                    return true;
                }

                return false;
            }).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_self);

            new MovingAction(SMotionExtension.TKS_BL_S93, CharMoveState.Stop, delay: 0.3f).SetEffectTiming_Tool(EffectTiming.POST, EffectTiming.POST, EffectTiming.POST).Add(list_self);

            opponent.infoList = list_opponent;
            return list_self;
        }
    }
}
