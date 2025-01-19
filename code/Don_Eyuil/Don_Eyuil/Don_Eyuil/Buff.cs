using BattleCharacterProfile;
using Don_Eyuil.Don_Eyuil.Buff;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using static BattleCharacterProfile.BattleCharacterProfileUI;
using UnityEngine.UI;

namespace Don_Eyuil
{
    //硬血结晶
    public class BattleUnitBuf_HardBlood_Crystal: BattleUnitBuf_Don_Eyuil
    {
        protected override string keywordId => "BattleUnitBuf_BleedCrystal";
        //至多30层
        //可配合硬血术效果
        public override int GetMaxStack() => 30;
        public BattleUnitBuf_HardBlood_Crystal(BattleUnitModel model) : base(model)
        {
            typeof(BattleUnitBuf).GetField("_bufIcon", AccessTools.all).SetValue(this,TKS_BloodFiend_Initializer.ArtWorks["硬血结晶"]);
            typeof(BattleUnitBuf).GetField("_iconInit", AccessTools.all).SetValue(this, true);
            this.stack = 0;
        }
    }
    //无法凝结的血
    public class BattleUnitBuf_UncondensableBlood : BattleUnitBuf_Don_Eyuil
    {
        protected override string keywordId => "BattleUnitBuf_Flow";
        //自身流血无法低于2+x
        public override void OnRoundEnd()
        {
            this.Destroy();
        }

        public static void UncodensableBloodCheck(BattleUnitBuf BleedingBuf)
        {
            var owner = BleedingBuf.GetFieldValue<BattleUnitModel>("_owner");
            if (owner != null && BattleUnitBuf_UncondensableBlood.GetBufStack<BattleUnitBuf_UncondensableBlood>(owner) > 0)
            {
                BleedingBuf.stack = Math.Max(BleedingBuf.stack, 2 + BattleUnitBuf_UncondensableBlood.GetBufStack<BattleUnitBuf_UncondensableBlood>(owner));
            }
        }

        [HarmonyPatch(typeof(BattleUnitBuf_bleeding), "AfterDiceAction")]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> BattleUnitBuf_bleeding_AfterDiceAction_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
            codes.InsertRange(codes.Count - 2, new List<CodeInstruction>()
            {
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Call,AccessTools.Method(typeof(BattleUnitBuf_UncondensableBlood),"UncodensableBloodCheck"))
            });
            return codes.AsEnumerable<CodeInstruction>();
        }

        public BattleUnitBuf_UncondensableBlood(BattleUnitModel model) : base(model)
        {
            typeof(BattleUnitBuf).GetField("_bufIcon", AccessTools.all).SetValue(this, TKS_BloodFiend_Initializer.ArtWorks["无法凝结的血"]);
            typeof(BattleUnitBuf).GetField("_iconInit", AccessTools.all).SetValue(this, true);
            this.stack = 0;
        }
    }
    //热血尖枪
    public class BattleUnitBuf_WarmBloodLance : BattleUnitBuf_Don_Eyuil
    {
        protected override string keywordId => "BattleUnitBuf_Rifle";
        //自身这一幕施加的"流血"翻倍
        public override int GetMultiplierOnGiveKeywordBufByCard(BattleUnitBuf cardBuf, BattleUnitModel target)
        {
            return cardBuf.bufType == KeywordBuf.Bleeding ? 2 : 1;
        }
        public BattleUnitBuf_WarmBloodLance(BattleUnitModel model) : base(model)
        {
            typeof(BattleUnitBuf).GetField("_bufIcon", AccessTools.all).SetValue(this, TKS_BloodFiend_Initializer.ArtWorks["热血尖枪"]);
            typeof(BattleUnitBuf).GetField("_iconInit", AccessTools.all).SetValue(this, true);
            this.stack = 0;
        }

        public override void OnRoundEnd()
        {
            this.Destroy();
        }
    }
    //深度创痕
    public class BattleUnitBuf_DeepWound : BattleUnitBuf_Don_Eyuil
    {
        protected override string keywordId => "BattleUnitBuf_DeepWound";

        public static string Desc = "这一幕受到的\"流血\"伤害增加50%";
        public BattleUnitBuf_DeepWound(BattleUnitModel model) : base(model)
        {
            typeof(BattleUnitBuf).GetField("_bufIcon", AccessTools.all).SetValue(this, TKS_BloodFiend_Initializer.ArtWorks["深度创痕"]);
            typeof(BattleUnitBuf).GetField("_iconInit", AccessTools.all).SetValue(this, true);
            this.stack = 0;
        }
        public override void OnRoundEnd()
        {
            this.Destroy();
        }
        public override float DmgFactor(int dmg, DamageType type = DamageType.ETC, KeywordBuf keyword = KeywordBuf.None)
        {
            if (keyword == KeywordBuf.Bleeding)
            {
                return 1.5f;
            }
            return base.DmgFactor(dmg, type, keyword);
        }
    }
    //血晶荆棘
    public class BattleUnitBuf_BloodCrystalThorn : BattleUnitBuf_Don_Eyuil
    {
        protected override string keywordId => "BattleUnitBuf_Thistles";
        public static string Desc = "投掷骰子时使自身在下一幕中获得1层[流血](每幕至多触发x次) 自身速度降低x/2 每幕结束时层数减半";
        public BattleUnitBuf_BloodCrystalThorn(BattleUnitModel model) : base(model)
        {
            typeof(BattleUnitBuf).GetField("_bufIcon", AccessTools.all).SetValue(this, TKS_BloodFiend_Initializer.ArtWorks["血晶荆棘"]);
            typeof(BattleUnitBuf).GetField("_iconInit", AccessTools.all).SetValue(this, true);
            this.stack = 0;
        }
        public int TriggeredOnRollDiceCount = 0;
        public override void OnRollDice(BattleDiceBehavior behavior)
        {
            TriggeredOnRollDiceCount++;
            if(TriggeredOnRollDiceCount <= this.stack)
            {
                _owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Bleeding, 1);
            }
        }
        public override int GetSpeedDiceAdder(int speedDiceResult)
        {
            return -this.stack / 2;
        }
        public override void OnRoundEnd()
        {
            this.stack /= 2;
            if (this.stack <= 0)
            {
                this.Destroy();
            }
            TriggeredOnRollDiceCount = 0;
        }
    }
    //汹涌的血潮(不衰减）
    public class BattleUnitBuf_BloodTide : BattleUnitBuf_Don_Eyuil
    {
        protected override string keywordId => "BattleUnitBuf_Tidewater";
        public static string Desc = "所有敌方角色被施加\"流血\"时层数+x\r\n自身对处于流血状态的敌方角色造成的伤害与混乱伤害x×10%";
        public BattleUnitBuf_BloodTide(BattleUnitModel model) : base(model)
        {
            typeof(BattleUnitBuf).GetField("_bufIcon", AccessTools.all).SetValue(this, TKS_BloodFiend_Initializer.ArtWorks["汹涌的血潮"]);
            typeof(BattleUnitBuf).GetField("_iconInit", AccessTools.all).SetValue(this, true);
            this.stack = 0;
        }
        public override void BeforeOtherUnitAddKeywordBuf(KeywordBuf BufType, BattleUnitModel Target, ref int Stack)
        {
            if(bufType == KeywordBuf.Bleeding && Target.faction != _owner.faction )
            {
                Stack += this.stack;
            }
        }
        public override void BeforeGiveDamage(BattleDiceBehavior behavior)
        {
            if(behavior != null && behavior.card!=null && behavior.card.target != null)
            {
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    dmgRate = 10 * stack,
                    breakRate = 10 * stack,
                });
            }
        }
    }
    //席卷而来的饥饿
    public class BattleUnitBuf_FloodOfHunger : BattleUnitBuf_Don_Eyuil
    {
        protected override string keywordId => "BattleUnitBuf_FloodOfHunger";
        public static string Desc = "击中\"流血\"不低于3的目标时恢复2点体力\r\n若一幕中没有恢复体力则失去20%的混乱抗性并获得1层\"虚弱\"\r\n获得15点正面情感后移除本效果";
        //P07:场上角色解除\"席卷而来的饥饿\"时将永久获得1层\"强壮\"与3层\"振奋\"";
        public BattleUnitBuf_FloodOfHunger(BattleUnitModel model) : base(model)
        {
            typeof(BattleUnitBuf).GetField("_bufIcon", AccessTools.all).SetValue(this, TKS_BloodFiend_Initializer.ArtWorks["席卷而来的饥饿"]);
            typeof(BattleUnitBuf).GetField("_iconInit", AccessTools.all).SetValue(this, true);
            this.stack = 0;
        }
        public bool HasTriggered = false;
        public int TotalEmotionCoinNum = 0;
        public class BattleUnitBuf_InfinityStrongNBreakProtection : BattleUnitBuf
        {
            public override void OnRoundEnd()
            {
                _owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Strength, 1);
                _owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.BreakProtection,3);
            }
        }
        public override void BeforeAddEmotionCoin(EmotionCoinType CoinType, ref int Count)
        {
            TotalEmotionCoinNum += CoinType == EmotionCoinType.Positive ? Count : 0;
            if(TotalEmotionCoinNum >= 15)
            {
                if(_owner.passiveDetail.HasPassive<PassiveAbility_DonEyuil_07>())
                {
                    _owner.bufListDetail.AddBuf(new BattleUnitBuf_InfinityStrongNBreakProtection());
                }
                this.Destroy();
            }
        }
        public override void OnRoundEnd()
        {
            if(HasTriggered == false)
            {
                _owner.TakeBreakDamage((int)(this._owner.breakDetail.GetDefaultBreakGauge() * 0.2),DamageType.ETC);
                _owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Weak, 1);
                return;
            }
            HasTriggered = false;
        }
        public override void OnSuccessAttack(BattleDiceBehavior behavior)
        {
            if(behavior != null && behavior.card != null && behavior.card.target != null && behavior.card.target.bufListDetail.GetKewordBufStack(KeywordBuf.Bleeding) >= 3)
            {
                _owner.RecoverHP(2);
            }
        }

        public override void AfterRecoverHp(int v)
        {
            HasTriggered = true;
        }

    }
    //血铠
    public class BattleUnitBuf_BloodArmor : BattleUnitBuf_Don_Eyuil
    {
        protected override string keywordId => "BattleUnitBuf_BloodArmor";
        public static string Desc = "这一幕中受到的伤害与混乱伤害减少{0}×20%";
        public BattleUnitBuf_BloodArmor(BattleUnitModel model) : base(model)
        {
            typeof(BattleUnitBuf).GetField("_bufIcon", AccessTools.all).SetValue(this, TKS_BloodFiend_Initializer.ArtWorks["敌方血甲"]);
            typeof(BattleUnitBuf).GetField("_iconInit", AccessTools.all).SetValue(this, true);
            this.stack = 0;
        }
        public override int GetBreakDamageReductionRate()
        {
            return 20 * stack;
        }
        public override int GetDamageReductionRate()
        {
            return 20 * stack;
        }
    }
    //血甲护盾
    public class BattleUnitBuf_BloodShield : BattleUnitBuf_Don_Eyuil
    {
        public void ReduceShield(int num)
        {
            this.stack -= num;
            if (this.stack <= 0)
            {
                this.stack = 0;
            }
            if (_owner.bufListDetail.HasBuf<BattleUnitBuf_Armour>() || _owner.bufListDetail.HasBuf<PassiveAbility_DonEyuil_02.BattleUnitBuf_HardBloodArt.BattleUnitBuf_HardBloodArt_BloodShield>())
            {
                BattleUnitBuf_Don_Eyuil.OnTakeBleedingDamagePatch.Trigger_BleedingDmg_After(_owner, num, KeywordBuf.Bleeding);
            }

        }
        private void DestroyUI()
        {
            UnityEngine.Object.DestroyImmediate(this.shieldBarGameObject);
            UnityEngine.Object.DestroyImmediate(this.shieldTextGameObject);
        }

        public override void Destroy()
        {
            this.DestroyUI();
        }

        private void CreateUI1()
        {
            this.shieldBarGameObject = UnityEngine.Object.Instantiate<GameObject>(this._owner.view.unitBottomStatUI.GetFieldValue<Image>("hpBar").transform.gameObject, this._owner.view.unitBottomStatUI.GetFieldValue<Image>("hpBar").transform.parent.transform);
            this.shieldTextGameObject = UnityEngine.Object.Instantiate<GameObject>(this._owner.view.unitBottomStatUI.GetFieldValue<TextMeshProUGUI>("_txtHp").transform.gameObject, this._owner.view.unitBottomStatUI.GetFieldValue<TextMeshProUGUI>("_txtHp").transform.parent.transform);
            if (this.shieldBarGameObject.GetComponent<Image>() != null)
            {
                this.shieldBar = this.shieldBarGameObject.GetComponent<Image>();
                this.shieldBar.color = this.shieldColor;
            }
            if (this.shieldTextGameObject.GetComponent<TextMeshProUGUI>() != null)
            {
                this.shieldTextGameObject.transform.localPosition += new Vector3(0f, -50f, 0f);
                this.shieldText = this.shieldTextGameObject.GetComponent<TextMeshProUGUI>();
                this.shieldText.color = this.shieldColor;
                this.shieldText.text = string.Empty;
            }
        }

        private void CreateUI2()
        {
            this.currentShieldValue = 0f;
            this.currentShield = 0f;
            BattleCharacterProfileUI profileUI = SingletonBehavior<BattleManagerUI>.Instance.ui_unitListInfoSummary.GetProfileUI(this._owner);
            if (profileUI != null)
            {
                this.shieldBarUIGameObject1 = UnityEngine.Object.Instantiate<GameObject>(profileUI.GetFieldValue<HpBar>("hpBar").img.transform.gameObject, profileUI.GetFieldValue<HpBar>("hpBar").img.transform.parent.transform);
                this.shieldBarUIGameObject2 = UnityEngine.Object.Instantiate<GameObject>(profileUI.GetFieldValue<HpBar>("img_damagedHp").img.transform.gameObject, profileUI.GetFieldValue<HpBar>("img_damagedHp").img.transform.parent.transform);
                this.shieldBarUIGameObject3 = UnityEngine.Object.Instantiate<GameObject>(profileUI.GetFieldValue<HpBar>("img_healedHp").img.transform.gameObject, profileUI.GetFieldValue<HpBar>("img_healedHp").img.transform.parent.transform);
                this.shieldTextUIGameObject = UnityEngine.Object.Instantiate<GameObject>(profileUI.GetFieldValue<Text>("txt_hp").transform.gameObject, profileUI.GetFieldValue<Text>("txt_hp").transform.parent.transform);
                this.shieldBarUIGameObject1.name = "BloodShieldUI1";
                this.shieldBarUIGameObject2.name = "BloodShieldUI2";
                this.shieldBarUIGameObject3.name = "BloodShieldUI3";
                this.shieldTextUIGameObject.name = "BloodShieldUI4";
                Color color = this.shieldColor;
                if (shieldBarUIGameObject1 != null && this.shieldBarUIGameObject1.GetComponent<Image>() != null)
                {
                    this.shieldBarUIGameObject1.transform.localPosition += new Vector3(17f, 30f, 0f);
                    this.shieldBarUI = this.shieldBarUIGameObject1.GetComponent<Image>();
                    this.shieldBarUI.color = color;
                }
                if (shieldBarUIGameObject2 != null && this.shieldBarUIGameObject2.GetComponent<Image>() != null)
                {
                    this.shieldBarUIGameObject2.transform.localPosition += new Vector3(17f, 30f, 0f);
                    this.img_damagedShield = this.shieldBarUIGameObject2.GetComponent<Image>();
                    this.img_damagedShield.color = color;
                }
                if (shieldBarUIGameObject3 != null && this.shieldBarUIGameObject3.GetComponent<Image>() != null)
                {
                    this.shieldBarUIGameObject3.transform.localPosition += new Vector3(17f, 30f, 0f);
                    this.img_healedShield = this.shieldBarUIGameObject3.GetComponent<Image>();
                    this.img_healedShield.color = color;
                }
                if (shieldTextUIGameObject != null && this.shieldTextUIGameObject.GetComponent<Text>() != null)
                {
                    this.shieldTextUIGameObject.transform.localPosition += new Vector3(-20f, 90f, 0f);
                    this.shieldTextUIGameObject.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                    this.txt_shield = this.shieldTextUIGameObject.GetComponent<Text>();
                    this.txt_shield.color = color;
                    this.txt_shield.text = string.Empty;
                }
            }

        }

        public void ChangeColor(Color color)
        {
            this.shieldColor = color;
            this.shieldBar.color = this.shieldColor;
            this.shieldText.color = this.shieldColor;
        }

        public void SetShield(int targetstack)
        {
            if (this.shieldBarGameObject == null && this.shieldTextGameObject == null)
            {
                this.CreateUI1();
            }
            bool activeInHierarchy = this._owner.view.unitBottomStatUI.gameObject.activeInHierarchy;
            if (activeInHierarchy)
            {
                this.shieldText.text = targetstack <= 0 ? string.Empty: targetstack.ToString();
                this._owner.view.unitBottomStatUI.StartCoroutine(this.ShieldBarAnimationRoutine(targetstack));
            }
            if (this.shieldBarUIGameObject1 == null && this.shieldBarUIGameObject2 == null && this.shieldBarUIGameObject3 == null && this.shieldTextUIGameObject == null)
            {
                this.CreateUI2();
            }
            BattleCharacterProfileUI profileUI = SingletonBehavior<BattleManagerUI>.Instance.ui_unitListInfoSummary.GetProfileUI(this._owner);
            if (profileUI != null)
            {
                if ((float)targetstack > this.currentShield)
                {
                    profileUI.StartCoroutine(this.UpdateShieldBar(this.shieldBarUI, this.img_healedShield, (float)targetstack, this.img_healedShield));
                }
                else
                {
                    profileUI.StartCoroutine(this.UpdateShieldBar(this.img_damagedShield, this.shieldBarUI, (float)targetstack, this.img_damagedShield));
                }
                if ((float)targetstack != this.currentShield)
                {
                    profileUI.StartCoroutine(this.UpdateShieldNum(this.currentShield, (float)targetstack));
                }
            }
        }

        public IEnumerator ShieldBarAnimationRoutine(int targetstack)
        {
            while (Mathf.Abs((float)targetstack - this.currentShieldValue) > Mathf.Epsilon)
            {
                this.currentShieldValue = Mathf.Lerp(this.currentShieldValue, (float)targetstack, Time.deltaTime);
                float num = this.currentShieldValue / (float)this._owner.MaxHp;
                float z = 90f * (1f - num);
                this.shieldBar.transform.localRotation = Quaternion.Euler(0f, 0f, z);
                yield return null;
            }
            yield break;
        }

        private IEnumerator UpdateShieldBar(Image src, Image dst, float newShield, Image bar)
        {
            Color c = bar.color;
            c.a = 1f;
            bar.color = c;
            float t = newShield / (float)this._owner.MaxHp;
            float x = Mathf.Lerp(-550f, 0f, t);
            Vector3 dstPos = dst.transform.localPosition;
            dstPos.x = x;
            dst.transform.localPosition = dstPos;
            float e = 0f;
            Vector3 srcPos = src.transform.localPosition;
            while (e < 1f)
            {
                e += Time.deltaTime;
                src.transform.localPosition = Vector3.Lerp(srcPos, dstPos, e);
                c.a = 1f - e;
                bar.color = c;
                yield return YieldCache.waitFrame;
            }
            c.a = 0f;
            bar.color = c;
            yield break;
        }

        private IEnumerator UpdateShieldNum(float curShield, float newShield)
        {
            float e = 0f;
            while (e < 1f)
            {
                e += Time.deltaTime;
                float num = Mathf.Lerp(curShield, newShield, e);
                this.txt_shield.text = ((int)num).ToString();
                yield return YieldCache.waitFrame;
            }
            this.currentShield = newShield;
            if (this.currentShield <= 0f)
            {
                this.txt_shield.text = string.Empty;
            }
            yield break;
        }


        public Color shieldColor = new Color(0.68f,0,0,1);

        public GameObject shieldBarGameObject;

        public GameObject shieldTextGameObject;

        public Image shieldBar;

        public TextMeshProUGUI shieldText;

        public float currentShieldValue;

        public float currentShield;

        public GameObject shieldBarUIGameObject1;

        public GameObject shieldBarUIGameObject2;

        public GameObject shieldBarUIGameObject3;

        public GameObject shieldTextUIGameObject;

        public Image shieldBarUI;

        public Image img_damagedShield;

        public Image img_healedShield;

        public Text txt_shield;

        [HarmonyPatch(typeof(BattleCharacterProfileUI), "Initialize")]
        [HarmonyPostfix]
        public static void BattleCharacterProfileUI_Initialize_Post(BattleCharacterProfileUI __instance)
        {
            Transform[] componentsInChildren = __instance.gameObject.GetComponentsInChildren<Transform>(true);
            for (int i = 0; i < componentsInChildren.Length; i++)
            {
                if (componentsInChildren[i].gameObject.name.Contains("BloodShieldUI4"))
                {
                    UnityEngine.Object.Destroy(componentsInChildren[i].gameObject);
                }
            }
        }
        public BattleUnitBuf_BloodShield(BattleUnitModel model) : base(model) { stack = 0; }
        [HarmonyPatch(typeof(BattleUnitBottomStatUI), "SetBufs")]
        [HarmonyPostfix]
        public static void BattleUnitBottomStatUI_SetBufs_Post(BattleBufUIDataList bufDataList)
        {
            BattleBufUIData battleBufUIData = bufDataList.bufActivatedList.Find((BattleBufUIData x) => x.buf is BattleUnitBuf_BloodShield);
            if (battleBufUIData != null)
            {
                ((BattleUnitBuf_BloodShield)battleBufUIData.buf).SetShield(battleBufUIData.stack);
            }
        }

        public static void ShieldReduce(BattleUnitModel target, ref int dmg)
        {
            if (BattleUnitBuf_BloodShield.GetBufStack<BattleUnitBuf_BloodShield>(target) > 0)
            {
                int ShieldReduceStack = Math.Min(dmg, BattleUnitBuf_BloodShield.GetBufStack<BattleUnitBuf_BloodShield>(target));
                dmg -= ShieldReduceStack;
                BattleUnitBuf_BloodShield.GetBuf<BattleUnitBuf_BloodShield>(target).ReduceShield(ShieldReduceStack);
            }
        }

        [HarmonyPatch(typeof(BattleUnitModel), "TakeDamage")]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> BattleUnitModel_TakeDamage_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> list = instructions.ToList<CodeInstruction>();
            MethodInfo method = AccessTools.Method(typeof(BattleUnitModel), "IsImmuneDmg", new Type[]
            {
                typeof(DamageType),
                typeof(KeywordBuf)
            }, null);
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Calls(method))
                {
                    while (i < list.Count)
                    {
                        Label? label;
                        if (list[i].Branches(out label))
                        {
                            int index = list.FindIndex((CodeInstruction code) => code.labels.Contains(label.Value));
                            list.InsertRange(index, new CodeInstruction[]
                            {
                                new CodeInstruction(OpCodes.Nop, null).MoveLabelsFrom(list[index]),
                                new CodeInstruction(OpCodes.Ldarg_0, null),
                                new CodeInstruction(OpCodes.Ldloca, 1),
                                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(BattleUnitBuf_BloodShield), "ShieldReduce", null, null))
                            });
                            break;
                        }
                        i++;
                    }
                    break;
                }
            }
            return list;
        }
    }


}
