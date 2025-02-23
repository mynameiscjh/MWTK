using System.Collections.Generic;
using static Don_Eyuil.TKS_BloodFiend_Initializer.TKS_EnumExtension;
using static RencounterManager;

namespace Don_Eyuil.Don_Eyuil.Action
{
    public class BehaviourAction_XD_血镰_1 : BehaviourActionBase
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

            new MovingAction(ActionDetail.Penetrate, CharMoveState.MoveForward, 3, false, 0.2f, 3).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_opponent);

            new MovingAction(SMotionExtension.TKS_BL_S34, CharMoveState.Stop, delay: 0.2f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_self);

            new MovingAction(ActionDetail.Damaged, CharMoveState.MoveBack, 5, false, 0.2f, 3).SetEffectTiming_Tool(EffectTiming.PRE, EffectTiming.NONE, EffectTiming.NONE).Add(list_opponent);

            new MovingAction(SMotionExtension.TKS_BL_S35, CharMoveState.MoveBack, 2, false, 0.2f, 0.5f).SetEffectTiming_Tool(EffectTiming.PRE, EffectTiming.NONE, EffectTiming.NONE).Add(list_self);

            for (int i = 0; i < 2; i++)
            {
                new MovingAction(SMotionExtension.TKS_BL_S36, CharMoveState.Stop, delay: 0.05f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_self);
                new MovingAction(SMotionExtension.TKS_BL_S37, CharMoveState.Stop, delay: 0.05f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_self);
                new MovingAction(SMotionExtension.TKS_BL_S38, CharMoveState.Stop, delay: 0.05f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_self);
            }

            for (int i = 0; i < 2; i++)
            {
                new MovingAction(SMotionExtension.TKS_BL_S39, CharMoveState.Stop, delay: 0.05f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_self);
                new MovingAction(SMotionExtension.TKS_BL_S40, CharMoveState.Stop, delay: 0.05f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_self);
                new MovingAction(SMotionExtension.TKS_BL_S41, CharMoveState.Stop, delay: 0.05f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_self);
            }

            new MovingAction(SMotionExtension.TKS_BL_S42, CharMoveState.Stop, delay: 0.2f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_self);

            new MovingAction(SMotionExtension.TKS_BL_S43, CharMoveState.Stop, delay: 0.2f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_self);

            new MovingAction(SMotionExtension.TKS_BL_S44, CharMoveState.MoveForward, 17, false, 0.2f, 8).SetEffectTiming_Tool(EffectTiming.PRE, EffectTiming.NONE, EffectTiming.NONE).Add(list_self);

            new MovingAction(SMotionExtension.TKS_BL_S45, CharMoveState.Stop, delay: 0.1f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_self);

            new MovingAction(SMotionExtension.TKS_BL_S45, CharMoveState.MoveForward, 10, false, 0.1f, 5).SetEffectTiming_Tool(EffectTiming.PRE, EffectTiming.NONE, EffectTiming.NONE).Add(list_self);

            new MovingAction(SMotionExtension.TKS_BL_S46, CharMoveState.Stop, delay: 0.1f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_self);

            new MovingAction(SMotionExtension.TKS_BL_S46, CharMoveState.MoveForward, 10, false, 0.1f, 5).SetEffectTiming_Tool(EffectTiming.PRE, EffectTiming.NONE, EffectTiming.NONE).Add(list_self);

            new MovingAction(SMotionExtension.TKS_BL_S47, CharMoveState.Stop, delay: 0.1f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_self);

            new MovingAction(SMotionExtension.TKS_BL_S47, CharMoveState.MoveForward, 13, false, 0.1f, 7).SetEffectTiming_Tool(EffectTiming.POST, EffectTiming.POST, EffectTiming.POST).Add(list_self);

            opponent.infoList = list_opponent;
            return list_self;
        }
    }

    public class BehaviourAction_XD_血镰_2 : BehaviourActionBase
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

            var random = RandomUtil.Range(0, 3);
            //var random = self.behaviourResultData.behaviour._flags.FindAll(x => x == DiceFlagExtension.HasGivenDamage).Count;

            switch (random)
            {
                case 0:

                    new MovingAction(ActionDetail.Damaged, CharMoveState.Stop, delay: 1f).SetEffectTiming_Tool(EffectTiming.POST, EffectTiming.POST, EffectTiming.POST).Add(list_opponent);

                    new MovingAction(SMotionExtension.TKS_BL_S48, CharMoveState.Stop, delay: 1f).SetEffectTiming_Tool(EffectTiming.POST, EffectTiming.POST, EffectTiming.POST).Add(list_self);

                    break;
                case 1:

                    new MovingAction(ActionDetail.Damaged, CharMoveState.Stop, delay: 1f).SetEffectTiming_Tool(EffectTiming.POST, EffectTiming.POST, EffectTiming.POST).Add(list_opponent);

                    new MovingAction(SMotionExtension.TKS_BL_S49, CharMoveState.Stop, delay: 1f).SetEffectTiming_Tool(EffectTiming.POST, EffectTiming.POST, EffectTiming.POST).Add(list_self);

                    break;
                case 2:

                    new MovingAction(ActionDetail.Damaged, CharMoveState.Stop, delay: 1f).SetEffectTiming_Tool(EffectTiming.POST, EffectTiming.POST, EffectTiming.POST).Add(list_opponent);

                    new MovingAction(SMotionExtension.TKS_BL_S50, CharMoveState.MoveForward, 13, true, 0.1f, 7).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_self);

                    new MovingAction(SMotionExtension.TKS_BL_S50, CharMoveState.Stop, delay: 0.9f).SetEffectTiming_Tool(EffectTiming.POST, EffectTiming.POST, EffectTiming.POST).Add(list_self);

                    break;
                case 3:

                    new MovingAction(ActionDetail.Damaged, CharMoveState.Stop, delay: 1f).SetEffectTiming_Tool(EffectTiming.POST, EffectTiming.POST, EffectTiming.POST).Add(list_opponent);

                    new MovingAction(SMotionExtension.TKS_BL_S51, CharMoveState.Stop, delay: 1f).SetEffectTiming_Tool(EffectTiming.POST, EffectTiming.POST, EffectTiming.POST).Add(list_self);

                    break;
                default:
                    break;
            }

            //self.behaviourResultData.behaviour._flags.Add(DiceFlagExtension.HasGivenDamage);

            opponent.infoList = list_opponent;
            return list_self;
        }
    }

    public class BehaviourAction_XD_血镰_3 : BehaviourActionBase
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

            new MovingAction(ActionDetail.Damaged, CharMoveState.Stop, delay: 0.3f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_opponent);

            new MovingAction(SMotionExtension.TKS_BL_S52, CharMoveState.Stop, delay: 0.3f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_self);

            new MovingAction(ActionDetail.Damaged, CharMoveState.MoveBack, 10, false, 0.3f, 6).SetEffectTiming_Tool(EffectTiming.PRE, EffectTiming.PRE, EffectTiming.PRE).Add(list_opponent);

            new MovingAction(SMotionExtension.TKS_BL_S53, CharMoveState.Stop, delay: 0.3f).SetEffectTiming_Tool(EffectTiming.PRE, EffectTiming.PRE, EffectTiming.PRE).Add(list_self);

            opponent.infoList = list_opponent;
            return list_self;
        }
    }
}
