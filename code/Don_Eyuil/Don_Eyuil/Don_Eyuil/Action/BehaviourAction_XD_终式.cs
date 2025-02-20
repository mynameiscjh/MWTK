using System.Collections.Generic;
using UnityEngine;
using static Don_Eyuil.TKS_BloodFiend_Initializer.TKS_EnumExtension;

namespace Don_Eyuil.Don_Eyuil.Action
{
    public class BehaviourAction_XD_终式_1 : BehaviourActionBase
    {
        public override FarAreaEffect SetFarAreaAtkEffect(BattleUnitModel self)
        {
            var temp = self.view.gameObject.AddComponent<FarAreaEffect_XD_终式_1>();
            temp.Init(self);
            return temp;
        }

        public class FarAreaEffect_XD_终式_1 : FarAreaEffect
        {

            public override bool HasIndependentAction => true;

            public override void Init(BattleUnitModel self, params object[] args)
            {
                base.Init(self, args);
                this.isRunning = false;
                self.view.charAppearance.ChangeMotion(SMotionExtension.TKS_BL_S94);
                this.state = FarAreaEffect.EffectState.None;
                oldPos = self.view.WorldPosition;

            }
            Vector3 oldPos;
            public float time = 0;
            public override bool ActionPhase(float deltaTime, BattleUnitModel attacker, List<BattleFarAreaPlayManager.VictimInfo> victims, ref List<BattleFarAreaPlayManager.VictimInfo> defenseVictims)
            {

                if (attacker.view.WorldPosition.y < oldPos.y + 5 && attacker.view.WorldPosition.y > oldPos.y && attacker.view.charAppearance.GetCurrentMotion().actionDetail != SMotionExtension.TKS_BL_S94)
                {
                    attacker.view.charAppearance.ChangeMotion(SMotionExtension.TKS_BL_S94);
                }

                if (attacker.view.WorldPosition.y < oldPos.y + 10 && attacker.view.WorldPosition.y > oldPos.y + 5 && attacker.view.charAppearance.GetCurrentMotion().actionDetail != SMotionExtension.TKS_BL_S95)
                {
                    attacker.view.charAppearance.ChangeMotion(SMotionExtension.TKS_BL_S95);
                }

                if (attacker.view.WorldPosition.y < oldPos.y + 15 && attacker.view.WorldPosition.y > oldPos.y + 10 && attacker.view.charAppearance.GetCurrentMotion().actionDetail != SMotionExtension.TKS_BL_S96)
                {
                    attacker.view.charAppearance.ChangeMotion(SMotionExtension.TKS_BL_S96);
                }

                if (attacker.view.WorldPosition.y < oldPos.y + 20)
                {
                    attacker.view.WorldPosition += new Vector3(0, 15f) * deltaTime;
                    return false;
                }

                if (attacker.view.WorldPosition.y >= oldPos.y + 20)
                {
                    time += deltaTime;
                    if (time >= 0.5f)
                    {
                        if (attacker.view.charAppearance.GetCurrentMotion().actionDetail == SMotionExtension.TKS_BL_S98)
                        {
                            foreach (var item in victims)
                            {
                                attacker.currentDiceAction.currentBehavior.GiveDamage(item.unitModel);
                            }
                            attacker.view.LockPosY(true);
                            return true;
                        }
                        if (attacker.view.charAppearance.GetCurrentMotion().actionDetail == SMotionExtension.TKS_BL_S97)
                        {
                            attacker.view.charAppearance.ChangeMotion(SMotionExtension.TKS_BL_S98);
                        }
                        if (attacker.view.charAppearance.GetCurrentMotion().actionDetail == SMotionExtension.TKS_BL_S96)
                        {
                            attacker.view.charAppearance.ChangeMotion(SMotionExtension.TKS_BL_S97);
                        }

                        time = 0;
                    }
                }

                return false;
            }
        }
    }

    public class BehaviourAction_XD_终式_2 : BehaviourActionBase
    {
        public override FarAreaEffect SetFarAreaAtkEffect(BattleUnitModel self)
        {
            var temp = self.view.gameObject.AddComponent<FarAreaEffect_XD_终式_2>();
            temp.Init(self);
            return temp;
        }

        public class FarAreaEffect_XD_终式_2 : FarAreaEffect
        {

            public override bool HasIndependentAction => true;

            public override void Init(BattleUnitModel self, params object[] args)
            {
                base.Init(self, args);
                self.view.LockPosY(false);
                this.isRunning = false;
                self.view.charAppearance.ChangeMotion(SMotionExtension.TKS_BL_S99);
                this.state = FarAreaEffect.EffectState.None;
                oldPos = self.view.WorldPosition;

            }
            Vector3 oldPos;
            Vector3 tempPos;
            public float time = 0;
            int count = 0;
            Vector3 temp_1 = Vector3.zero;
            Vector3 temp_2 = Vector3.zero;
            bool fl = false;
            int stat = 0;
            public override bool ActionPhase(float deltaTime, BattleUnitModel attacker, List<BattleFarAreaPlayManager.VictimInfo> victims, ref List<BattleFarAreaPlayManager.VictimInfo> defenseVictims)
            {

                time += deltaTime;
                if (stat == 0)
                {
                    if (count <= 10)
                    {
                        if (time >= 0.2f)
                        {
                            count++;
                            time = 0;
                        }

                        if (count % 2 == 0)
                        {
                            attacker.view.charAppearance.ChangeMotion(SMotionExtension.TKS_BL_S99);
                        }

                        if (count % 2 == 1)
                        {
                            attacker.view.charAppearance.ChangeMotion(SMotionExtension.TKS_BL_S100);
                        }

                        return false;
                    }
                    stat = 1;
                }

                if (stat == 1)
                {
                    if (time <= 1f && attacker.view.charAppearance.GetCurrentMotion().actionDetail != SMotionExtension.TKS_BL_S102)
                    {
                        attacker.view.charAppearance.ChangeMotion(SMotionExtension.TKS_BL_S101);
                        return false;
                    }
                    attacker.view.charAppearance.ChangeMotion(SMotionExtension.TKS_BL_S102);
                    stat = 2;
                }

                if (stat == 2)
                {
                    attacker.view.WorldPosition = Vector3.SmoothDamp(attacker.view.WorldPosition, new Vector3(-20, 0, 0), ref temp_1, 0.1f);
                    if (Vector3.Distance(attacker.view.WorldPosition, new Vector3(-20, 0, 0)) <= 1)
                    {
                        attacker.view.charAppearance.ChangeMotion(SMotionExtension.TKS_BL_S103);
                        time = 0;
                        foreach (var item in victims)
                        {
                            attacker.currentDiceAction.currentBehavior.GiveDamage(item.unitModel);
                        }
                        stat = 3;
                    }
                }

                if (stat == 3)
                {
                    attacker.view.WorldPosition = Vector3.SmoothDamp(attacker.view.WorldPosition, new Vector3(-10, 0, 0), ref temp_1, 0.1f);
                    if (Vector3.Distance(attacker.view.WorldPosition, new Vector3(-10, 0, 0)) <= 1)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
    }
}
