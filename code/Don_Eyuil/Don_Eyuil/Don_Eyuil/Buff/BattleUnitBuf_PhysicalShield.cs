using BattleCharacterProfile;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static BattleCharacterProfile.BattleCharacterProfileUI;

namespace Don_Eyuil.Don_Eyuil.Buff
{
    public class BattleUnitBuf_PhysicalShield : BattleUnitBuf
    {

        public static void AddBuf(BattleUnitModel unit, int stack)
        {
            if (unit.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_PhysicalShield) is BattleUnitBuf_PhysicalShield BattleUnitBuf_PhysicalShield)
            {
                BattleUnitBuf_PhysicalShield.stack += stack;
                if (BattleUnitBuf_PhysicalShield.stack <= 0)
                {
                    BattleUnitBuf_PhysicalShield.stack = 0;
                }
            }
            else
            {
                BattleUnitBuf_PhysicalShield = new BattleUnitBuf_PhysicalShield
                {
                    stack = stack
                };
                unit.bufListDetail.AddBuf(BattleUnitBuf_PhysicalShield);
            }

        }
        public void ReduceShield(int num)
        {
            this.stack -= num;
            if (this.stack <= 0)
            {
                this.stack = 0;
            }
        }
        public static int GetBuf(BattleUnitModel unit)
        {
            if (unit.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_PhysicalShield) is BattleUnitBuf_PhysicalShield BattleUnitBuf_PhysicalShield)
            {
                return BattleUnitBuf_PhysicalShield.stack;
            }
            return 0;
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
            bool flag = this.shieldBarGameObject.GetComponent<Image>() != null;
            if (flag)
            {
                this.shieldBar = this.shieldBarGameObject.GetComponent<Image>();
                this.shieldBar.color = this.shieldColor;
            }
            bool flag2 = this.shieldTextGameObject.GetComponent<TextMeshProUGUI>() != null;
            if (flag2)
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
            BattleCharacterProfileUI profileUI = SingletonBehavior<BattleManagerUI>.Instance.ui_unitListInfoSummary.GetProfileUI(this._owner);
            if (profileUI != null)
            {
                this.shieldBarUIGameObject1 = UnityEngine.Object.Instantiate<GameObject>(profileUI.GetFieldValue<HpBar>("hpBar").img.transform.gameObject, profileUI.GetFieldValue<HpBar>("hpBar").img.transform.parent.transform);
                this.shieldBarUIGameObject2 = UnityEngine.Object.Instantiate<GameObject>(profileUI.GetFieldValue<HpBar>("img_damagedHp").img.transform.gameObject, profileUI.GetFieldValue<HpBar>("img_damagedHp").img.transform.parent.transform);
                this.shieldBarUIGameObject3 = UnityEngine.Object.Instantiate<GameObject>(profileUI.GetFieldValue<HpBar>("img_healedHp").img.transform.gameObject, profileUI.GetFieldValue<HpBar>("img_healedHp").img.transform.parent.transform);
                this.shieldTextUIGameObject = UnityEngine.Object.Instantiate<GameObject>(profileUI.GetFieldValue<Text>("txt_hp").transform.gameObject, profileUI.GetFieldValue<Text>("txt_hp").transform.parent.transform);
                this.shieldBarUIGameObject1.name = "ShieldUI1";
                this.shieldBarUIGameObject2.name = "ShieldUI2";
                this.shieldBarUIGameObject3.name = "ShieldUI3";
                this.shieldTextUIGameObject.name = "ShieldUI4";
            }
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
            this.currentShield = 0f;
        }

        public void SetShield(int targetstack)
        {
            bool flag = this.shieldBarGameObject == null && this.shieldTextGameObject == null;
            if (flag)
            {
                this.CreateUI1();
            }
            bool activeInHierarchy = this._owner.view.unitBottomStatUI.gameObject.activeInHierarchy;
            if (activeInHierarchy)
            {
                bool flag2 = targetstack <= 0;
                if (flag2)
                {
                    this.shieldText.text = string.Empty;
                }
                else
                {
                    this.shieldText.text = targetstack.ToString();
                }
                this._owner.view.unitBottomStatUI.StartCoroutine(this.ShieldBarAnimationRoutine(targetstack));
            }
            bool flag3 = this.shieldBarUIGameObject1 == null && this.shieldBarUIGameObject2 == null && this.shieldBarUIGameObject3 == null && this.shieldTextUIGameObject == null;
            if (flag3)
            {
                this.CreateUI2();
            }
            BattleCharacterProfileUI profileUI = SingletonBehavior<BattleManagerUI>.Instance.ui_unitListInfoSummary.GetProfileUI(this._owner);
            bool flag4 = profileUI != null;
            if (flag4)
            {
                bool flag5 = (float)targetstack > this.currentShield;
                if (flag5)
                {
                    profileUI.StartCoroutine(this.UpdateShieldBar(this.shieldBarUI, this.img_healedShield, (float)targetstack, this.img_healedShield));
                }
                else
                {
                    profileUI.StartCoroutine(this.UpdateShieldBar(this.img_damagedShield, this.shieldBarUI, (float)targetstack, this.img_damagedShield));
                }
                bool flag6 = (float)targetstack != this.currentShield;
                if (flag6)
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
            bool flag = this.currentShield <= 0f;
            if (flag)
            {
                this.txt_shield.text = string.Empty;
            }
            yield break;
        }


        public Color shieldColor = Color.blue;

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
    }

}
