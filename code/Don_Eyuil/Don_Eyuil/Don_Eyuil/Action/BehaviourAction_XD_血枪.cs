using System.Collections.Generic;
using UnityEngine;
using static Don_Eyuil.TKS_BloodFiend_Initializer.TKS_EnumExtension;
using static RencounterManager;

namespace Don_Eyuil.Don_Eyuil.Action
{
    public class BehaviourAction_XD_血枪_1 : BehaviourActionBase
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

            new MovingAction(ActionDetail.Default, CharMoveState.Stop, delay: 0.45f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_opponent);

            new MovingAction(SMotionExtension.TKS_BL_S24, CharMoveState.Stop, delay: 0.15f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_self);

            new MovingAction(SMotionExtension.TKS_BL_S25, CharMoveState.Stop, delay: 0.15f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_self);

            new MovingAction(SMotionExtension.TKS_BL_S26, CharMoveState.Stop, delay: 0.15f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_self);

            new MovingAction(ActionDetail.Damaged, CharMoveState.Stop, delay: 0.3f).SetEffectTiming_Tool(EffectTiming.PRE, EffectTiming.PRE, EffectTiming.PRE).Add(list_opponent);

            new MovingAction(SMotionExtension.TKS_BL_S27, CharMoveState.Stop, delay: 0.3f).SetEffectTiming_Tool(EffectTiming.PRE, EffectTiming.PRE, EffectTiming.PRE).Add(list_self);

            opponent.infoList = list_opponent;
            return list_self;
        }
    }

    public class BehaviourAction_XD_血枪_2 : BehaviourActionBase
    {

        public override List<MovingAction> GetMovingAction(ref ActionAfterBehaviour self, ref ActionAfterBehaviour opponent)
        {
            if (self.result != Result.Win)
            {
                return new List<MovingAction>();
            }
            var selfView = self.view;
            var oldPos = selfView.WorldPosition;

            opponent.infoList.Clear();
            var list_self = new List<MovingAction>();
            var list_opponent = new List<MovingAction>();

            new MovingAction(ActionDetail.Default, CharMoveState.Stop, delay: 0.5f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_opponent);

            new MovingAction(SMotionExtension.TKS_BL_S28, CharMoveState.Custom, delay: 0f).SetCustomMoving_Tool((float deltaTime, float elapsedTime) =>
            {
                if (selfView.WorldPosition.y >= oldPos.y + 10)
                {
                    return true;
                }
                selfView.WorldPosition += new Vector3(4, 10) * 4 * deltaTime;
                return false;
            }).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_self);

            new MovingAction(ActionDetail.Default, CharMoveState.Stop, delay: 0.5f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_opponent);

            new MovingAction(SMotionExtension.TKS_BL_S29, CharMoveState.Custom, delay: 0f).SetCustomMoving_Tool((float deltaTime, float elapsedTime) =>
            {
                if (selfView.WorldPosition.y <= oldPos.y)
                {
                    return true;
                }
                selfView.WorldPosition += new Vector3(-4, -10) * 4 * deltaTime;
                return false;
            }).SetEffectTiming_Tool(EffectTiming.POST, EffectTiming.POST, EffectTiming.POST).Add(list_self);

            opponent.infoList = list_opponent;
            return list_self;
        }
    }
}
