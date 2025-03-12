using System.Collections.Generic;
using UnityEngine;
using static Don_Eyuil.TKS_BloodFiend_Initializer.TKS_EnumExtension;
using static RencounterManager;
using DG.Tweening;

namespace Don_Eyuil.Don_Eyuil.Action
{
    public class BehaviourAction_XD_血之宝库_1 : BehaviourActionBase_Don_Eyuil
    {
        public override void GetMovingAction_DonEyuil(ref List<MovingAction> LS, ref List<MovingAction> LO, ref ActionAfterBehaviour self, ref ActionAfterBehaviour opponent)
        {
            Start(LO,ActionDetail.Default, CharMoveState.Stop, delay: 0.4f).SetEffectTiming(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE)
            .Next(LS, ActionDetail.Aim, CharMoveState.Stop, delay: 0.4f).SetEffectTiming(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE)
            .Start(LO, ActionDetail.Damaged, CharMoveState.Stop, delay: 0.4f).SetEffectTiming(EffectTiming.POST, EffectTiming.POST, EffectTiming.POST)
            .Next(LS, ActionDetail.Fire, CharMoveState.Stop, delay: 0.4f).SetEffectTiming(EffectTiming.POST, EffectTiming.POST, EffectTiming.POST)
            .Finish();
        }
    }
    public class BehaviourAction_XD_血之宝库_2 : BehaviourActionBase_Don_Eyuil
    {
        public override void GetMovingAction_DonEyuil(ref List<MovingAction> LS, ref List<MovingAction> LO, ref ActionAfterBehaviour self, ref ActionAfterBehaviour opponent)
        {
            Start(LO, ActionDetail.Default, CharMoveState.Stop, delay: 0.4f).SetEffectTiming(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE)
            .Next(LS, SMotionExtension.TKS_BL_S1, CharMoveState.Stop, delay: 0.4f).SetEffectTiming(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE)
            .Start(LO, ActionDetail.Damaged, CharMoveState.Stop, delay: 0.4f).SetEffectTiming(EffectTiming.POST, EffectTiming.POST, EffectTiming.POST)
            .Next(LS, SMotionExtension.TKS_BL_S2, CharMoveState.Stop, delay: 0.4f).SetEffectTiming(EffectTiming.POST, EffectTiming.POST, EffectTiming.POST)
            .Finish();
        }
    }
    public class BehaviourAction_XD_血之宝库_3 : BehaviourActionBase_Don_Eyuil
    {
        public override void GetMovingAction_DonEyuil(ref List<MovingAction> LS, ref List<MovingAction> LO, ref ActionAfterBehaviour self, ref ActionAfterBehaviour opponent)
        {

            /*
            --new MovingAction(ActionDetail.Default, CharMoveState.Stop, delay: 0.4f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_opponent);
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

            }).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).Add(list_self);*/


            Start(LO, ActionDetail.Default, CharMoveState.Stop, delay: 0.4f).SetEffectTiming(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE)
            .Next(LS, SMotionExtension.TKS_BL_S3, CharMoveState.Custom, 5, false, 0.8f, 3).WithCustomMoving((deltaTime, elapsedTime) =>
                {
                    Debug.LogError("aaaaaaa:" + elapsedTime);
                    return elapsedTime>=0.5f;
                }, (deltaTime, elapsedTime) =>
                {
                    Debug.LogError("bbbbbbb:" + elapsedTime);
                    return elapsedTime >= 0.75f;
                }).SetEffectTiming(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE)
                .WithCustomMoving((deltaTime, elapsedTime) =>
                {
                    Debug.LogError("ccccccc:" + elapsedTime);
                    return elapsedTime >= 0.3f;
                })
            .Start(LO, ActionDetail.Damaged, CharMoveState.Stop, delay: 0.4f).SetEffectTiming(EffectTiming.POST, EffectTiming.POST, EffectTiming.POST)
            .Next(LS, SMotionExtension.TKS_BL_S3, CharMoveState.Custom, 5, false, 0.8f, 3).SetEffectTiming(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE)
            .Finish();
        }
    }
}
