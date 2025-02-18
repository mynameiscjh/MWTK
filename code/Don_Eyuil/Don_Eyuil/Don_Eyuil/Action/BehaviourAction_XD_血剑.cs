using System.Collections.Generic;
using static Don_Eyuil.TKS_BloodFiend_Initializer.TKS_EnumExtension;
using static RencounterManager;

namespace Don_Eyuil.Don_Eyuil.Action
{
    public class BehaviourAction_XD_血剑_1 : BehaviourActionBase
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

            new MovingAction(ActionDetail.Slash, CharMoveState.Stop, delay: 0.3f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_opponent);

            new MovingAction(SMotionExtension.TKS_BL_S16, CharMoveState.Stop, delay: 0.3f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_self);

            new MovingAction(ActionDetail.Damaged, CharMoveState.MoveBack, 3, false, 0.3f, 3).SetEffectTiming_Tool(EffectTiming.PRE, EffectTiming.NOT_PRINT, EffectTiming.NOT_PRINT).Add(list_opponent);

            new MovingAction(ActionDetail.Slash, CharMoveState.MoveBack, 3, false, 0.3f, 3).SetEffectTiming_Tool(EffectTiming.PRE, EffectTiming.NOT_PRINT, EffectTiming.NOT_PRINT).Add(list_self);

            new MovingAction(ActionDetail.Default, CharMoveState.Stop, delay: 0.6f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_opponent);

            new MovingAction(SMotionExtension.TKS_BL_S17, CharMoveState.Stop, delay: 0.3f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_self);

            new MovingAction(SMotionExtension.TKS_BL_S18, CharMoveState.Stop, delay: 0.3f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_self);

            new MovingAction(ActionDetail.Damaged, CharMoveState.Stop, delay: 0.3f).SetEffectTiming_Tool(EffectTiming.PRE, EffectTiming.WITHOUT_DMGTEXT, EffectTiming.WITHOUT_DMGTEXT).Add(list_opponent);

            new MovingAction(SMotionExtension.TKS_BL_S19, CharMoveState.MoveForward, 8, false, 0.3f, 3).SetEffectTiming_Tool(EffectTiming.PRE, EffectTiming.WITHOUT_DMGTEXT, EffectTiming.WITHOUT_DMGTEXT).Add(list_self);

            new MovingAction(ActionDetail.Damaged, CharMoveState.Stop, delay: 0.3f).SetEffectTiming_Tool(EffectTiming.PRE, EffectTiming.WITHOUT_DMGTEXT, EffectTiming.WITHOUT_DMGTEXT).Add(list_opponent);

            new MovingAction(SMotionExtension.TKS_BL_S20, CharMoveState.Stop, 6, false, 0.3f, 3).SetEffectTiming_Tool(EffectTiming.PRE, EffectTiming.WITHOUT_DMGTEXT, EffectTiming.WITHOUT_DMGTEXT).Add(list_self);

            new MovingAction(ActionDetail.Damaged, CharMoveState.Stop, delay: 0.3f).SetEffectTiming_Tool(EffectTiming.PRE, EffectTiming.WITHOUT_DMGTEXT, EffectTiming.WITHOUT_DMGTEXT).Add(list_opponent);

            new MovingAction(SMotionExtension.TKS_BL_S21, CharMoveState.Stop, 6, false, 0.3f, 3).SetEffectTiming_Tool(EffectTiming.PRE, EffectTiming.WITHOUT_DMGTEXT, EffectTiming.WITHOUT_DMGTEXT).Add(list_self);

            new MovingAction(ActionDetail.Damaged, CharMoveState.Stop, delay: 1f).SetEffectTiming_Tool(EffectTiming.PRE, EffectTiming.WITHOUT_DMGTEXT, EffectTiming.WITHOUT_DMGTEXT).Add(list_opponent);

            new MovingAction(SMotionExtension.TKS_BL_S22, CharMoveState.Stop, 6, false, 1.7f, 3).SetEffectTiming_Tool(EffectTiming.PRE, EffectTiming.WITHOUT_DMGTEXT, EffectTiming.WITHOUT_DMGTEXT).SetCustomEffectRes("Kali_SpecialVertAtk").Add(list_self);

            new MovingAction(ActionDetail.Damaged, CharMoveState.Stop, 10, false, 1.0f, 5).SetEffectTiming_Tool(EffectTiming.POST, EffectTiming.POST, EffectTiming.POST).Add(list_opponent);

            new MovingAction(SMotionExtension.TKS_BL_S23, CharMoveState.Stop, 6, false, 1.0f, 3).SetEffectTiming_Tool(EffectTiming.POST, EffectTiming.POST, EffectTiming.POST).Add(list_self);

            new MovingAction(ActionDetail.Damaged, CharMoveState.MoveBack, 10, false, 0.5f, 5).SetEffectTiming_Tool(EffectTiming.POST, EffectTiming.POST, EffectTiming.POST).Add(list_opponent);

            opponent.infoList = list_opponent;
            return list_self;
        }
    }
}
