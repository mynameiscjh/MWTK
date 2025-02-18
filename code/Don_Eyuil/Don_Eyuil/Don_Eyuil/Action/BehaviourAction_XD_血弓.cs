using System.Collections.Generic;
using static Don_Eyuil.TKS_BloodFiend_Initializer.TKS_EnumExtension;
using static RencounterManager;

namespace Don_Eyuil.Don_Eyuil.Action
{
    public class BehaviourAction_XD_血弓_1 : BehaviourActionBase
    {
        public override List<RencounterManager.MovingAction> GetMovingAction(ref RencounterManager.ActionAfterBehaviour self, ref RencounterManager.ActionAfterBehaviour opponent)
        {
            if (self.result != Result.Win)
            {
                return new List<MovingAction>();
            }

            opponent.infoList.Clear();
            var list_self = new List<MovingAction>();
            var list_opponent = new List<MovingAction>();

            new MovingAction(ActionDetail.Default, CharMoveState.Stop, delay: 0.3f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_opponent);

            new MovingAction(SMotionExtension.TKS_BL_S5, CharMoveState.Stop, 1, true, 0.15f, 2).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_self);
            new MovingAction(SMotionExtension.TKS_BL_S6, CharMoveState.Stop, 1, true, 0.15f, 2).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_self);

            new MovingAction(ActionDetail.Default, CharMoveState.Stop, 5, true, 0.2f, 3).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_opponent);

            new MovingAction(SMotionExtension.TKS_BL_S8, CharMoveState.MoveBack, 1, true, 0.2f, 2).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_self);

            new MovingAction(ActionDetail.Damaged, CharMoveState.MoveBack, 5, true, 0.2f, 3).SetEffectTiming_Tool(EffectTiming.PRE, EffectTiming.PRE, EffectTiming.PRE).Add(list_opponent);

            new MovingAction(SMotionExtension.TKS_BL_S8, CharMoveState.Stop, 1, true, 0.2f, 2).SetEffectTiming_Tool(EffectTiming.PRE, EffectTiming.PRE, EffectTiming.PRE).Add(list_self);

            opponent.infoList = list_opponent;
            return list_self;
        }
    }

    public class BehaviourAction_XD_血弓_2 : BehaviourActionBase
    {

        public override List<RencounterManager.MovingAction> GetMovingAction(ref RencounterManager.ActionAfterBehaviour self, ref RencounterManager.ActionAfterBehaviour opponent)
        {
            if (self.result != Result.Win)
            {
                return new List<MovingAction>();
            }

            opponent.infoList.Clear();
            var list_self = new List<MovingAction>();
            var list_opponent = new List<MovingAction>();

            if (!self.behaviourResultData.behaviour.HasFlag(DiceFlagExtension.HasGivenDamage))
            {
                new MovingAction(ActionDetail.Default, CharMoveState.Stop, delay: 0.3f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_opponent);

                new MovingAction(SMotionExtension.TKS_BL_S5, CharMoveState.Stop, 1, true, 0.15f, 2).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_self);
                new MovingAction(SMotionExtension.TKS_BL_S6, CharMoveState.Stop, 1, true, 0.15f, 2).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_self);
            }
            else
            {
                new MovingAction(ActionDetail.Default, CharMoveState.Stop, delay: 0.1f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_opponent);

                new MovingAction(SMotionExtension.TKS_BL_S5, CharMoveState.Stop, 1, true, 0.05f, 2).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_self);
                new MovingAction(SMotionExtension.TKS_BL_S6, CharMoveState.Stop, 1, true, 0.05f, 2).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_self);
            }



            new MovingAction(ActionDetail.Default, CharMoveState.Stop, 5, true, 0.05f, 3).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_opponent);

            new MovingAction(SMotionExtension.TKS_BL_S8, CharMoveState.MoveBack, 1, true, 0.05f, 2).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_self);

            new MovingAction(ActionDetail.Damaged, CharMoveState.MoveBack, 5, true, 0.05f, 3).SetEffectTiming_Tool(EffectTiming.PRE, EffectTiming.PRE, EffectTiming.PRE).Add(list_opponent);

            new MovingAction(SMotionExtension.TKS_BL_S8, CharMoveState.Stop, 1, true, 0.05f, 2).SetEffectTiming_Tool(EffectTiming.PRE, EffectTiming.PRE, EffectTiming.PRE).Add(list_self);

            if (!self.behaviourResultData.behaviour.HasFlag(DiceFlagExtension.HasGivenDamage))
            {
                self.behaviourResultData.behaviour.AddFlag(DiceFlagExtension.HasGivenDamage);
            }

            opponent.infoList = list_opponent;
            return list_self;
        }
    }

    public class BehaviourAction_XD_血弓_3 : BehaviourActionBase
    {
        public override List<RencounterManager.MovingAction> GetMovingAction(ref RencounterManager.ActionAfterBehaviour self, ref RencounterManager.ActionAfterBehaviour opponent)
        {
            if (self.result != Result.Win)
            {
                return new List<MovingAction>();
            }

            opponent.infoList.Clear();
            var list_self = new List<MovingAction>();
            var list_opponent = new List<MovingAction>();

            new MovingAction(ActionDetail.Default, CharMoveState.Stop, delay: 0.4f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_opponent);

            new MovingAction(SMotionExtension.TKS_BL_S9, CharMoveState.Stop, delay: 0.05f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_self);

            new MovingAction(SMotionExtension.TKS_BL_S10, CharMoveState.Stop, delay: 0.05f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_self);

            new MovingAction(SMotionExtension.TKS_BL_S11, CharMoveState.Stop, delay: 0.05f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_self);

            new MovingAction(SMotionExtension.TKS_BL_S12, CharMoveState.Stop, delay: 0.05f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_self);

            new MovingAction(SMotionExtension.TKS_BL_S13, CharMoveState.Stop, delay: 0.2f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_self);

            new MovingAction(ActionDetail.Damaged, CharMoveState.MoveBack, 5, false, 0.1f, 3).SetEffectTiming_Tool(EffectTiming.POST, EffectTiming.POST, EffectTiming.POST).Add(list_opponent);

            new MovingAction(SMotionExtension.TKS_BL_S14, CharMoveState.MoveBack, 5, false, 0.1f, 3).SetEffectTiming_Tool(EffectTiming.PRE, EffectTiming.PRE, EffectTiming.PRE).Add(list_self);

            new MovingAction(ActionDetail.Damaged, CharMoveState.Stop, delay: 0.1f).SetEffectTiming_Tool(EffectTiming.PRE, EffectTiming.PRE, EffectTiming.PRE).Add(list_opponent);

            new MovingAction(SMotionExtension.TKS_BL_S15, CharMoveState.Stop, delay: 0.1f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_self);

            opponent.infoList = list_opponent;
            return list_self;
        }
    }
}
