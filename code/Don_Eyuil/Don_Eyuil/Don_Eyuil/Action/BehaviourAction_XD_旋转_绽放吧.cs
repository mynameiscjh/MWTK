using System.Collections.Generic;
using UnityEngine;
using static Don_Eyuil.TKS_BloodFiend_Initializer.TKS_EnumExtension;
using static RencounterManager;

namespace Don_Eyuil.Don_Eyuil.Action
{
    public class BehaviourAction_XD_旋转_绽放吧_1 : BehaviourActionBase
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

            new MovingAction(SMotionExtension.TKS_BL_S104, CharMoveState.Stop, delay: 0.3f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_self);

            new MovingAction(ActionDetail.Damaged, CharMoveState.MoveBack, 5, false, 0.3f, 3).SetEffectTiming_Tool(EffectTiming.PRE, EffectTiming.PRE, EffectTiming.PRE).Add(list_opponent);

            new MovingAction(SMotionExtension.TKS_BL_S105, CharMoveState.Stop, delay: 0.3f).SetEffectTiming_Tool(EffectTiming.PRE, EffectTiming.PRE, EffectTiming.PRE).Add(list_self);

            opponent.infoList = list_opponent;
            return list_self;
        }
    }

    public class BehaviourAction_XD_旋转_绽放吧_2 : BehaviourActionBase
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

            new MovingAction(ActionDetail.Damaged, CharMoveState.Stop, delay: 0.5f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_opponent);

            new MovingAction(SMotionExtension.TKS_BL_S106, CharMoveState.Stop, delay: 0.5f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_self);

            new MovingAction(ActionDetail.Damaged, CharMoveState.Stop, delay: 0.5f).SetEffectTiming_Tool(EffectTiming.PRE, EffectTiming.PRE, EffectTiming.PRE).Add(list_opponent);

            new MovingAction(SMotionExtension.TKS_BL_S107, CharMoveState.MoveForward, 13, false, 0.5f, 5).SetEffectTiming_Tool(EffectTiming.PRE, EffectTiming.PRE, EffectTiming.PRE).Add(list_self);

            opponent.infoList = list_opponent;
            return list_self;
        }
    }

    public class BehaviourAction_XD_旋转_绽放吧_3 : BehaviourActionBase
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

            //new MovingAction(ActionDetail.Damaged, CharMoveState.Stop, updateDir: false, delay: 0.5f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_opponent);

            //new MovingAction(SMotionExtension.TKS_BL_S107, CharMoveState.MoveForward, 13, false, 0.5f, 5).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_self);

            var selfView = self.view;
            var opponentView = opponent.view;
            var v = Vector3.zero;

            new MovingAction(ActionDetail.Damaged, CharMoveState.Custom, updateDir: false, delay: 0.5f).SetCustomMoving_Tool((deltaTime, time) =>
            {
                opponentView.KnockDown(true);
                opponentView.WorldPosition = Vector3.SmoothDamp(opponentView.WorldPosition, selfView.WorldPosition + new Vector3(3, 10), ref v, 0.1f);
                return Vector3.Distance(opponentView.WorldPosition, selfView.WorldPosition + new Vector3(3, 10)) <= 0.1f;
            }).SetEffectTiming_Tool(EffectTiming.PRE, EffectTiming.NONE, EffectTiming.NONE).Add(list_opponent);

            new MovingAction(SMotionExtension.TKS_BL_S109, CharMoveState.Stop, updateDir: true, delay: 0.5f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_self);

            for (int i = 0; i < 5; i++)
            {
                new MovingAction(ActionDetail.Damaged, CharMoveState.KnockDown, updateDir: false, delay: 0.1f).SetEffectTiming_Tool(EffectTiming.PRE, EffectTiming.NONE, EffectTiming.NONE).Add(list_opponent);

                new MovingAction(SMotionExtension.TKS_BL_S110, CharMoveState.Stop, updateDir: true, delay: 0.05f).SetEffectTiming_Tool(EffectTiming.PRE, EffectTiming.NONE, EffectTiming.NONE).Add(list_self);

                new MovingAction(SMotionExtension.TKS_BL_S111, CharMoveState.Stop, updateDir: true, delay: 0.05f).SetEffectTiming_Tool(EffectTiming.PRE, EffectTiming.NONE, EffectTiming.NONE).Add(list_self);
            }

            new MovingAction(ActionDetail.Damaged, CharMoveState.KnockDown, updateDir: false, delay: 0.4f).SetEffectTiming_Tool(EffectTiming.PRE, EffectTiming.NONE, EffectTiming.NONE).Add(list_opponent);

            new MovingAction(SMotionExtension.TKS_BL_S110, CharMoveState.Stop, updateDir: true, delay: 0.2f).SetEffectTiming_Tool(EffectTiming.PRE, EffectTiming.NONE, EffectTiming.NONE).Add(list_self);

            new MovingAction(SMotionExtension.TKS_BL_S111, CharMoveState.Stop, updateDir: true, delay: 0.2f).SetEffectTiming_Tool(EffectTiming.PRE, EffectTiming.PRE, EffectTiming.PRE).Add(list_self);

            opponent.infoList = list_opponent;
            return list_self;
        }
    }
}
