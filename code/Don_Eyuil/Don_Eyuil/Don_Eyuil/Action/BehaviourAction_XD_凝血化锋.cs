using System.Collections.Generic;
using static Don_Eyuil.TKS_BloodFiend_Initializer.TKS_EnumExtension;
using static RencounterManager;

namespace Don_Eyuil.Don_Eyuil.Action
{
    public class BehaviourAction_XD_凝血化锋_1 : BehaviourActionBase
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

            new MovingAction(SMotionExtension.TKS_BL_S113, CharMoveState.Stop, delay: 0.3f).SetEffectTiming_Tool(EffectTiming.PRE, EffectTiming.WITHOUT_DMGTEXT, EffectTiming.WITHOUT_DMGTEXT).Add(list_self);

            new MovingAction(ActionDetail.Damaged, CharMoveState.MoveBack, 5, false, 0.3f, 3).SetEffectTiming_Tool(EffectTiming.PRE, EffectTiming.WITHOUT_DMGTEXT, EffectTiming.WITHOUT_DMGTEXT).Add(list_opponent);

            new MovingAction(ActionDetail.Aim, CharMoveState.Stop, delay: 0.3f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_self);

            new MovingAction(ActionDetail.Damaged, CharMoveState.Stop, delay: 0.3f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_opponent);

            new MovingAction(SMotionExtension.TKS_BL_S75, CharMoveState.Stop, delay: 0.3f).SetEffectTiming_Tool(EffectTiming.POST, EffectTiming.POST, EffectTiming.POST).Add(list_self);

            new MovingAction(ActionDetail.Damaged, CharMoveState.Stop, delay: 0.3f).SetEffectTiming_Tool(EffectTiming.POST, EffectTiming.POST, EffectTiming.POST).Add(list_opponent);

            new MovingAction(SMotionExtension.TKS_BL_S54, CharMoveState.Stop, delay: 0.3f).SetEffectTiming_Tool(EffectTiming.POST, EffectTiming.POST, EffectTiming.POST).Add(list_self);

            new MovingAction(ActionDetail.Damaged, CharMoveState.Stop, delay: 0.3f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_opponent);

            opponent.infoList = list_opponent;
            return list_self;
        }
    }

    public class BehaviourAction_XD_凝血化锋_2 : BehaviourActionBase
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

            new MovingAction(SMotionExtension.TKS_BL_S54, CharMoveState.Stop, delay: 0.3f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_self);

            new MovingAction(ActionDetail.Default, CharMoveState.Stop, delay: 0.3f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_opponent);

            new MovingAction(SMotionExtension.TKS_BL_S55, CharMoveState.Stop, delay: 0.3f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_self);

            new MovingAction(ActionDetail.Default, CharMoveState.Stop, delay: 0.3f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_opponent);

            new MovingAction(SMotionExtension.TKS_BL_S54, CharMoveState.MoveForward, 15, false, 0.3f, 7).SetEffectTiming_Tool(EffectTiming.PRE, EffectTiming.PRE, EffectTiming.PRE).Add(list_self);

            new MovingAction(ActionDetail.Damaged, CharMoveState.Stop, delay: 0.3f).SetEffectTiming_Tool(EffectTiming.PRE, EffectTiming.PRE, EffectTiming.PRE).Add(list_opponent);

            opponent.infoList = list_opponent;
            return list_self;
        }
    }
}
