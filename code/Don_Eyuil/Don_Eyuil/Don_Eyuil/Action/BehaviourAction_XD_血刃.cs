using System;
using System.Collections.Generic;
using UnityEngine;
using static Don_Eyuil.TKS_BloodFiend_Initializer.TKS_EnumExtension;
using static RencounterManager;

namespace Don_Eyuil.Don_Eyuil.Action
{
    public class BehaviourAction_XD_血刃_1 : BehaviourActionBase
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

            list_opponent.Add(new RencounterManager.MovingAction(ActionDetail.Default, CharMoveState.Stop, delay: 0.5f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE));

            list_self.Add(new RencounterManager.MovingAction(SMotionExtension.TKS_BL_S54, CharMoveState.Stop, delay: 0.25f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE));
            list_self.Add(new RencounterManager.MovingAction(SMotionExtension.TKS_BL_S55, CharMoveState.Stop, delay: 0.25f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE));

            list_opponent.Add(new RencounterManager.MovingAction(ActionDetail.Damaged, CharMoveState.Stop, 1, delay: 0.25f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE));

            list_self.Add(new RencounterManager.MovingAction(SMotionExtension.TKS_BL_S56, CharMoveState.MoveForward, 5, true, 0.25f, 5).SetEffectTiming_Tool(EffectTiming.POST, EffectTiming.POST, EffectTiming.POST));

            list_opponent.Add(new RencounterManager.MovingAction(ActionDetail.Damaged, CharMoveState.MoveBack, 1, delay: 0.25f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE));

            list_self.Add(new RencounterManager.MovingAction(SMotionExtension.TKS_BL_S57, CharMoveState.MoveBack, 2, true, 0.25f, 2).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE));
            list_self.Add(new RencounterManager.MovingAction(SMotionExtension.TKS_BL_S58, CharMoveState.Stop, delay: 0.25f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE));
            opponent.infoList = list_opponent;
            return list_self;
        }
    }

    public class BehaviourAction_XD_血刃_2 : BehaviourActionBase
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

            list_opponent.Add(new RencounterManager.MovingAction(ActionDetail.Slash, CharMoveState.Stop, delay: 0.75f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE));
            var ownerView = self.view;
            var oldPos = ownerView.WorldPosition;
            list_self.Add(new RencounterManager.MovingAction(SMotionExtension.TKS_BL_S59, CharMoveState.Custom, delay: 0.75f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE).SetCustomMoving_Tool((float deltaTime, float elapsedTime) =>
            {
                Vector3 b = (oldPos + new Vector3(10 * (ownerView.model.direction == Direction.LEFT ? 1 : -1), 0) - oldPos).normalized * SingletonBehavior<HexagonalMapManager>.Instance.tileSize * 2f;
                Vector3 viewPos = ownerView.WorldPosition + b;
                if (SingletonBehavior<HexagonalMapManager>.Instance.IsWall(viewPos) != HexagonalMapManager.WallDirection.NONE)
                {
                    return true;
                }
                var t = elapsedTime / 0.75f;
                ownerView.WorldPosition = new Vector3(10 * t * (ownerView.model.direction == Direction.LEFT ? 1 : -1), 10 * t - 10 * t * t, 0) + oldPos;

                if (Math.Abs(t - 0) <= 0.1f)
                {
                    ownerView.charAppearance.ChangeMotion(SMotionExtension.TKS_BL_S59);
                    Debug.Log($"SMotionExtension.TKS_BL_S59");
                }
                if ((Math.Abs(t - 0.33f) <= 0.1f))
                {
                    ownerView.charAppearance.ChangeMotion(SMotionExtension.TKS_BL_S60);
                    Debug.Log($"SMotionExtension.TKS_BL_S60");
                }
                if ((Math.Abs(t - 0.66f) <= 0.1f))
                {
                    ownerView.charAppearance.ChangeMotion(SMotionExtension.TKS_BL_S61);
                    Debug.Log($"SMotionExtension.TKS_BL_S61");
                }

                return Vector3.Distance(ownerView.WorldPosition, oldPos + new Vector3(10 * (ownerView.model.direction == Direction.LEFT ? 1 : -1), 0)) < 0.1f;
            }));

            list_opponent.Add(new RencounterManager.MovingAction(ActionDetail.Default, CharMoveState.Stop, 1, delay: 0.5f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE));

            list_self.Add(new RencounterManager.MovingAction(SMotionExtension.TKS_BL_S62, CharMoveState.Stop, delay: 0.5f).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE));

            list_opponent.Add(new RencounterManager.MovingAction(ActionDetail.Damaged, CharMoveState.Stop, 1, false, delay: 0.5f).SetEffectTiming_Tool(EffectTiming.POST, EffectTiming.POST, EffectTiming.POST));

            list_self.Add(new RencounterManager.MovingAction(SMotionExtension.TKS_BL_S63, CharMoveState.MoveForward, 25, false, 0.5f, 12).SetEffectTiming_Tool(EffectTiming.POST, EffectTiming.POST, EffectTiming.POST));

            list_self.Add(new RencounterManager.MovingAction(SMotionExtension.TKS_BL_S64, CharMoveState.Stop, updateDir: false).SetEffectTiming_Tool(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE));

            opponent.infoList = list_opponent;
            return list_self;
        }
    }

    public static class TempTool
    {
        public static RencounterManager.MovingAction SetEffectTiming_Tool(this RencounterManager.MovingAction moving, EffectTiming atk, EffectTiming recover, EffectTiming damaged)
        {
            moving.atkEffectTiming = atk;
            moving.recoverEffectTiming = recover;
            moving.damagedEffectTiming = damaged;
            return moving;
        }

        public static RencounterManager.MovingAction SetCustomMoving_Tool(this RencounterManager.MovingAction moving, RencounterManager.MovingAction.MoveCustomEvent m)
        {
            moving.SetCustomMoving(m);
            return moving;
        }
        public static RencounterManager.MovingAction SetCustomMoving_Tool(this RencounterManager.MovingAction moving, RencounterManager.MovingAction.MoveCustomEventWithElapsed m)
        {
            moving.SetCustomMoving(m);
            return moving;
        }

        public static void Add(this RencounterManager.MovingAction moving, List<RencounterManager.MovingAction> list)
        {
            list.Add(moving);
        }

        public static MovingAction SetCustomEffectRes(this RencounterManager.MovingAction moving, string customEffectRes)
        {
            moving.customEffectRes = customEffectRes;
            return moving;
        }
    }
}
