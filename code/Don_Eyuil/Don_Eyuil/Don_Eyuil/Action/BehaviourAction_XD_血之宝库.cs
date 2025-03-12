using System.Collections.Generic;
using UnityEngine;
using static Don_Eyuil.TKS_BloodFiend_Initializer.TKS_EnumExtension;
using static RencounterManager;

namespace Don_Eyuil.Don_Eyuil.Action
{
    /*public class BehaviourAction_XD_血之宝库_1 : BehaviourActionBase
    {
        public override List<MovingAction> GetMovingAction(ref ActionAfterBehaviour self, ref ActionAfterBehaviour opponent)
        {
            if (self.result != Result.Win)
            {
                return new List<MovingAction>();
            }

            opponent.infoList.Clear();
            var list_self = new List<MovingAction>();
            var list_opponent = new List<MovingAction>();

            new MovingAction(ActionDetail.Default, CharMoveState.Stop, delay: 0.4f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_opponent);

            new MovingAction(ActionDetail.Aim, CharMoveState.Stop, delay: 0.4f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_self);

            new MovingAction(ActionDetail.Damaged, CharMoveState.Stop, delay: 0.4f).SetEffectTiming_Tool(EffectTiming.POST, EffectTiming.POST, EffectTiming.POST).Add(list_opponent);

            new MovingAction(ActionDetail.Fire, CharMoveState.Stop, delay: 0.4f).SetEffectTiming_Tool(EffectTiming.POST, EffectTiming.POST, EffectTiming.POST).Add(list_self);

            opponent.infoList = list_opponent;
            return list_self;
        }
    }

    public class BehaviourAction_XD_血之宝库_2 : BehaviourActionBase
    {
        public override List<MovingAction> GetMovingAction(ref ActionAfterBehaviour self, ref ActionAfterBehaviour opponent)
        {
            if (self.result != Result.Win)
            {
                return new List<MovingAction>();
            }

            opponent.infoList.Clear();
            var list_self = new List<MovingAction>();
            var list_opponent = new List<MovingAction>();

            new MovingAction(ActionDetail.Default, CharMoveState.Stop, delay: 0.4f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_opponent);

            new MovingAction(SMotionExtension.TKS_BL_S1, CharMoveState.Stop, delay: 0.4f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_self);

            new MovingAction(ActionDetail.Damaged, CharMoveState.Stop, delay: 0.4f).SetEffectTiming_Tool(EffectTiming.POST, EffectTiming.POST, EffectTiming.POST).Add(list_opponent);

            new MovingAction(SMotionExtension.TKS_BL_S2, CharMoveState.Stop, delay: 0.4f).SetEffectTiming_Tool(EffectTiming.POST, EffectTiming.POST, EffectTiming.POST).Add(list_self);

            opponent.infoList = list_opponent;
            return list_self;
        }
    }*/

    public class BehaviourAction_XD_血之宝库A_3 : BehaviourActionBase
    {
        public override unsafe List<MovingAction> GetMovingAction(ref ActionAfterBehaviour self, ref ActionAfterBehaviour opponent)
        {
            if (self.result != Result.Win)
            {
                return new List<MovingAction>();
            }

            opponent.infoList.Clear();
            var list_self = new List<MovingAction>();
            var list_opponent = new List<MovingAction>();

            var view = self.view;

            new MovingAction(ActionDetail.Default, CharMoveState.Stop, delay: 0.4f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_opponent);

            var oldPos = view.WorldPosition;

            new MovingAction(SMotionExtension.TKS_BL_S3, CharMoveState.Custom, 5, false, 0.8f, 3).SetCustomMoving_Tool((deltaTime, elapsedTime) =>
            {

                if (elapsedTime >= 0.8f)
                {
                    return true;
                }

                view.WorldPosition += new Vector3(2 * (view.model.direction == Direction.LEFT ? 1 : -1), 12) * deltaTime;
                return false;

            }).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_self);

            new MovingAction(ActionDetail.Damaged, CharMoveState.Stop, delay: 0.4f).SetEffectTiming_Tool(EffectTiming.POST, EffectTiming.POST, EffectTiming.POST).Add(list_opponent);

            new MovingAction(SMotionExtension.TKS_BL_S4, CharMoveState.Stop, delay: 0.8f).SetEffectTiming_Tool(EffectTiming.POST, EffectTiming.POST, EffectTiming.POST).Add(list_self);

            var dis = view.WorldPosition.y - oldPos.y;

            new MovingAction(SMotionExtension.TKS_BL_S4, CharMoveState.Custom, 5, false, 0f, 3).SetCustomMoving_Tool((deltaTime, elapsedTime) =>
            {

                if ((view.WorldPosition.y - oldPos.y) / dis <= 0.5f)
                {
                    return true;
                }

                view.WorldPosition += new Vector3(0, -24) * deltaTime;
                return false;

            }).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_self);

            new MovingAction(ActionDetail.Default, CharMoveState.Custom, 5, false, 0f, 3).SetCustomMoving_Tool((deltaTime, elapsedTime) =>
            {

                if ((view.WorldPosition.y - oldPos.y) / dis <= 0f)
                {
                    return true;
                }

                view.WorldPosition += new Vector3(0, -24) * deltaTime;
                return false;

            }).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_self);


            opponent.infoList = list_opponent;
            return list_self;
        }
    }
}
