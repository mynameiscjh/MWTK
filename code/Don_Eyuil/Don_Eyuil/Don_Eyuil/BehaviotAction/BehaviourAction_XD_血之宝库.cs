using System.Collections.Generic;
using UnityEngine;
using static Don_Eyuil.TKS_BloodFiend_Initializer.TKS_EnumExtension;
using static RencounterManager;

namespace Don_Eyuil.Don_Eyuil.Action
{
    public class BehaviourAction_XD_血之宝库a_1 : BehaviourActionBase_Don_Eyuil
    {
        public override void GetMovingAction_DonEyuil(ref List<MovingAction> LS, ref List<MovingAction> LO, ref ActionAfterBehaviour self, ref ActionAfterBehaviour opponent)
        {
            /*            
            new MovingAction(ActionDetail.Default, CharMoveState.Stop, delay: 0.4f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_opponent);
            new MovingAction(ActionDetail.Aim, CharMoveState.Stop, delay: 0.4f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_self);
            new MovingAction(ActionDetail.Damaged, CharMoveState.Stop, delay: 0.4f).SetEffectTiming_Tool(EffectTiming.POST, EffectTiming.POST, EffectTiming.POST).Add(list_opponent);
            new MovingAction(ActionDetail.Fire, CharMoveState.Stop, delay: 0.4f).SetEffectTiming_Tool(EffectTiming.POST, EffectTiming.POST, EffectTiming.POST).Add(list_self);
            */
            Start(LO,ActionDetail.Default, CharMoveState.Stop, delay: 0.4f).SetEffectTiming(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE)
            .Next(LS, ActionDetail.Aim, CharMoveState.Stop, delay: 0.4f).SetEffectTiming(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE)
            .Start(LO, ActionDetail.Damaged, CharMoveState.Stop, delay: 0.4f).SetEffectTiming(EffectTiming.POST, EffectTiming.POST, EffectTiming.POST)
            .Next(LS, ActionDetail.Fire, CharMoveState.Stop, delay: 0.4f).SetEffectTiming(EffectTiming.POST, EffectTiming.POST, EffectTiming.POST)
            .Finish();

        }
    }
    public class BehaviourAction_XD_血之宝库a_2 : BehaviourActionBase_Don_Eyuil
    {
        public override void GetMovingAction_DonEyuil(ref List<MovingAction> LS, ref List<MovingAction> LO, ref ActionAfterBehaviour self, ref ActionAfterBehaviour opponent)
        {
            /*            
            new MovingAction(ActionDetail.Default, CharMoveState.Stop, delay: 0.4f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_opponent);
            new MovingAction(SMotionExtension.TKS_BL_S1, CharMoveState.Stop, delay: 0.4f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_self);
            new MovingAction(ActionDetail.Damaged, CharMoveState.Stop, delay: 0.4f).SetEffectTiming_Tool(EffectTiming.POST, EffectTiming.POST, EffectTiming.POST).Add(list_opponent);
            new MovingAction(SMotionExtension.TKS_BL_S2, CharMoveState.Stop, delay: 0.4f).SetEffectTiming_Tool(EffectTiming.POST, EffectTiming.POST, EffectTiming.POST).Add(list_self);
            */
            Start(LO, ActionDetail.Default, CharMoveState.Stop, delay: 0.4f).SetEffectTiming(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE)
            .Next(LS, SMotionExtension.TKS_BL_S1, CharMoveState.Stop, delay: 0.4f).SetEffectTiming(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE)
            .Start(LO, ActionDetail.Damaged, CharMoveState.Stop, delay: 0.4f).SetEffectTiming(EffectTiming.POST, EffectTiming.POST, EffectTiming.POST)
            .Next(LS, SMotionExtension.TKS_BL_S2, CharMoveState.Stop, delay: 0.4f).SetEffectTiming(EffectTiming.POST, EffectTiming.POST, EffectTiming.POST)
            .Finish();

        }
    }
}
