using Don_Eyuil.Don_Eyuil.Player.PassiveAbility;
using Don_Eyuil.San_Sora;
using Don_Eyuil.San_Sora.Player.PassiveAbility;
using Don_Eyuil.WhiteMoon_Sparkle.Player.Buff;
using Don_Eyuil.WhiteMoon_Sparkle.Player.PassiveAbility;
using EnumExtenderV2;
using HarmonyLib;
using LOR_XML;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Xml;
//using Workshop;
using System.Xml.Serialization;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;
using static Don_Eyuil.WhiteMoon_Sparkle.Player.PassiveAbility.PassiveAbility_WhiteMoonSparkle_16;
using Debug = UnityEngine.Debug;
using File = System.IO.File;
namespace Don_Eyuil
{
    [HarmonyPatch]
    public static class TKS_BloodFiend_PatchMethods_StoryFerrisWheel
    {
        public static bool isInit = false;
        public static GameObject Icons_FerrisWheel = null;
        public static GameObject Phase_FerrisWheel = null;
        public static string Desc;
        public static bool IsInFerrisWheel = false;
        public static GameObject icons = null;
        [HarmonyPatch(typeof(UIStoryProgressPanel), "SetStoryLine")]
        [HarmonyPostfix]
        public static void UIStoryProgressPanel_SetStoryLine_Post(UIStoryProgressPanel __instance)
        {
            if (isInit)
            {
                return;
            }
            var list = __instance.GetFieldValue<List<UIStoryProgressIconSlot>>("iconList");
            var temp = list.Find((UIStoryProgressIconSlot x) => x.currentStory == UIStoryLine.HanaAssociation);
            icons = temp.transform.parent.parent.gameObject;
            Icons_FerrisWheel = UnityEngine.Object.Instantiate(icons, icons.transform.parent);
            Icons_FerrisWheel.name = "..D";
            for (int i = 0; i < Icons_FerrisWheel.transform.childCount; i++)
            {
                if (i == 3)
                {
                    Phase_FerrisWheel = Icons_FerrisWheel.transform.GetChild(3).gameObject;
                    continue;
                }
                UnityEngine.Object.Destroy(Icons_FerrisWheel.transform.GetChild(i).gameObject);
            }
            Phase_FerrisWheel.name = "TK";
            foreach (Transform child in Phase_FerrisWheel.GetComponentsInChildren<Transform>())
            {
                if (child.gameObject == Phase_FerrisWheel)
                {
                    continue;
                }
                UnityEngine.Object.Destroy(child.gameObject);
            }
            Vector3 降低可读性的魔法数字2 = new Vector3(0, -50, 0);

            GameObject point = new GameObject("point");
            point.transform.parent = Phase_FerrisWheel.transform;
            point.transform.localPosition = new Vector3(853.7309f, 7736f + 1583.335f - 400f, 0f) + 降低可读性的魔法数字2;
            point.AddComponent<RedLine>();
            point.transform.localScale = new Vector3(15f, 15f, 0.8f);
            point.AddComponent<Image>().sprite = TKS_BloodFiend_Initializer.ArtWorks["特别大的摩天轮"];

            GameObject ferrisWheel = new GameObject("摩天轮");
            ferrisWheel.transform.parent = icons.transform;
            ferrisWheel.transform.localPosition = new Vector3(652.9309f, 7871.67f - 400f + 200f, 0) + 降低可读性的魔法数字2;
            Desc = "200是应为好看";
            Desc = "400 和 降低可读性的魔法数字2都是因为月亮计划会让坐标上移所以需要减回来";
            var image = ferrisWheel.AddComponent<Image>();//7871.67
            image.sprite = TKS_BloodFiend_Initializer.ArtWorks["摩天轮_BIG"];
            var button = ferrisWheel.AddComponent<Button>();
            button.targetGraphic = image;
            button.onClick.AddListener(new UnityEngine.Events.UnityAction(() =>
            {
                icons.SetActive(false);
                icons.transform.parent.GetChild(0).gameObject.SetActive(false);
                Icons_FerrisWheel.SetActive(true);
                IsInFerrisWheel = true;
            }));

            GameObject ferrisWheel_back = new GameObject("摩天轮_back");
            ferrisWheel_back.transform.parent = Icons_FerrisWheel.transform;
            ferrisWheel_back.transform.localPosition = new Vector3(652.9309f, 7871.67f - 400f + 200f, 0) + 降低可读性的魔法数字2;
            var image_back = ferrisWheel_back.AddComponent<Image>();
            image_back.sprite = TKS_BloodFiend_Initializer.ArtWorks["摩天轮_BIG"];
            var button_back = ferrisWheel_back.AddComponent<Button>();
            button_back.targetGraphic = image_back;
            button_back.onClick.AddListener(new UnityEngine.Events.UnityAction(() =>
            {
                icons.SetActive(true);
                icons.transform.parent.GetChild(0).gameObject.SetActive(true);
                Icons_FerrisWheel.SetActive(false);
                IsInFerrisWheel = false;
            }));

            //-879.5304 -9054.552 0
#if false
            GameObject 箭头 = new GameObject("箭头");
            箭头.transform.parent = Icons_FerrisWheel.transform;
            箭头.AddComponent<Image>().sprite = TKS_BloodFiend_Initializer.ArtWorks["..D"];
            箭头.transform.localPosition = new Vector3(652.9309f, -715f, 0);
            var 箭头_button = 箭头.AddComponent<Button>();
            箭头_button.targetGraphic = 箭头.GetComponent<Image>();
            箭头_button.onClick.AddListener(new UnityEngine.Events.UnityAction(() => Icons_FerrisWheel.transform.parent.localPosition = new Vector3(-879.5304f, -9054.552f, 0)));
#endif
            Vector3 降低可读性的魔法数字 = new Vector3(0f, 140f, 0f);

            UISpriteDataManager.instance.GetFieldValue<Dictionary<string, UIIconManager.IconSet>>("StoryIconDic").Add("TEST", new UIIconManager.IconSet
            {
                icon = TKS_BloodFiend_Initializer.ArtWorks["..D"],
                iconGlow = TKS_BloodFiend_Initializer.ArtWorks["..D"],
                colorGlow = new Color(1, 1, 1, 1),
                color = new Color(1, 1, 1, 1),
                type = ""
            });
            UISpriteDataManager.instance.GetFieldValue<Dictionary<string, UIIconManager.IconSet>>("StoryIconDic").Add("SanSora", new UIIconManager.IconSet
            {
                icon = TKS_BloodFiend_Initializer.ArtWorks["tk"],
                iconGlow = TKS_BloodFiend_Initializer.ArtWorks["tk"],
                colorGlow = new Color(1, 1, 1, 1),
                color = new Color(1, 1, 1, 1),
                type = ""
            });

            UISpriteDataManager.instance.GetFieldValue<Dictionary<string, UIIconManager.IconSet>>("StoryIconDic").Add("Sparkle", new UIIconManager.IconSet
            {
                icon = TKS_BloodFiend_Initializer.ArtWorks["xy"],
                iconGlow = TKS_BloodFiend_Initializer.ArtWorks["xy"],
                colorGlow = new Color(1, 1, 1, 1),
                color = new Color(1, 1, 1, 1),
                type = ""
            });

            void Func(int r, LorId id)
            {
                UIStoryProgressIconSlot testS = UnityEngine.Object.Instantiate(temp, Phase_FerrisWheel.transform);
                testS.name = "139";
                testS.currentStory = UIStoryLine.HanaAssociation;
                testS.Initialized(__instance);
                testS.transform.localPosition = new Vector3(852.9309f, 7585f + 1583.335f - 400f, 0) + 降低可读性的魔法数字2;
                testS.SetSlotData(new List<StageClassInfo>()
                {
                    Singleton<StageClassInfoList>.Instance.GetData(id)
                });
                var 挂钩 = new GameObject($"挂钩 {r}");
                挂钩.transform.parent = testS.transform.GetChild(1).GetChild(1);
                挂钩.transform.localPosition = Vector3.zero;
                var image_挂钩 = 挂钩.AddComponent<Image>();
                image_挂钩.sprite = TKS_BloodFiend_Initializer.ArtWorks["挂钩"];
                image_挂钩.raycastTarget = false;
                testS.gameObject.AddComponent<Roll>().Init(new Vector3(852.9309f, 7585f + 1583.335f - 400f, 0) + 降低可读性的魔法数字2, 600, r * 5);
                testS.gameObject.SetActive(true);
            }

            Func(0, MyId.Stage_桑空);
            Func(45, MyId.Stage_测试);
            Func(90, MyId.Stage_测试);
            Func(135, MyId.Stage_测试);
            Func(180, MyId.Stage_白月);
            Func(45 + 180, MyId.Stage_测试);
            Func(90 + 180, MyId.Stage_测试);
            Func(135 + 180, MyId.Stage_测试);


            GameObject 底座 = new GameObject("摩天轮底座");
            底座.transform.parent = Phase_FerrisWheel.transform;
            底座.transform.localPosition = new Vector3(853.7309f, 7606f + 1583.335f - 400f, 0f) + 降低可读性的魔法数字2;//853.7309 8739.335 0
            底座.transform.localScale = new Vector3(16.25f, 15.25f, 0.8713f);
            var A = 底座.AddComponent<Image>();
            A.sprite = TKS_BloodFiend_Initializer.ArtWorks["底座"];
            A.raycastTarget = false;

            var Don_Eyuil = UnityEngine.Object.Instantiate(temp, Phase_FerrisWheel.transform);
            Don_Eyuil.name = "Don_Eyuil";
            Don_Eyuil.currentStory = UIStoryLine.BlackSilence;
            Don_Eyuil.Initialized(__instance);
            Don_Eyuil.transform.localPosition = new Vector3(852.9309f, 7585f + 1583.335f - 400f + 50, 0) + 降低可读性的魔法数字2;
            Desc = "50是因为不在中间";
            UISpriteDataManager.instance.GetFieldValue<Dictionary<string, UIIconManager.IconSet>>("StoryIconDic").Add("Don_Eyuil", new UIIconManager.IconSet
            {
                icon = TKS_BloodFiend_Initializer.ArtWorks["Don_Eyuil"],
                iconGlow = TKS_BloodFiend_Initializer.ArtWorks["Don_Eyuil"],
                colorGlow = new Color(1, 1, 1, 1),
                color = new Color(1, 1, 1, 1),
                type = ""
            });
            Don_Eyuil.SetSlotData(new List<StageClassInfo>()
            {
                Singleton<StageClassInfoList>.Instance.GetData(MyId.Stage_埃尤尔)
            });

            UIStoryProgressIconSlot B = UnityEngine.Object.Instantiate(temp, Phase_FerrisWheel.transform);
            B.name = "B..D";
            B.currentStory = UIStoryLine.HanaAssociation;
            B.Initialized(__instance);
            B.transform.localPosition = new Vector3(1052.931f, 7938.335f, 0f);
            B.SetSlotData(new List<StageClassInfo>()
                {
                    Singleton<StageClassInfoList>.Instance.GetData(MyId.Stage_测试)
                });
            B.gameObject.SetActive(true);

            Icons_FerrisWheel.SetActive(false);

            isInit = true;

        }

        [HarmonyPatch(typeof(StageClassInfo), "currentState", MethodType.Getter)]
        [HarmonyPostfix]
        public static void StageClassInfo_currentState_Post(StageClassInfo __instance, ref StoryState __result)
        {
            if (__instance.id == MyId.Stage_测试)
            {
                __result = StoryState.Close;
            }
        }

        [HarmonyPatch(typeof(UIStoryProgressPanel), "OpenInit")]
        [HarmonyPostfix]
        public static void UIStoryProgressPanel_OpenInit_Post()
        {
            if (IsInFerrisWheel)
            {
                icons.SetActive(true);
                icons.transform.parent.GetChild(0).gameObject.SetActive(true);
                Icons_FerrisWheel.SetActive(false);
                IsInFerrisWheel = false;
            }
        }

        public class Roll : MonoBehaviour
        {
            public Vector2 point; // 旋转中心的坐标
            public float R;

            public void Init(Vector2 point, float r, int index = 0)
            {
                this.point = point;
                this.R = r;
                this.index = index;
            }

            public List<Vector2> points = new List<Vector2>();
            public int index = 0;
            void Start()
            {
                float count = 1800;
                float a = 360 / count;
                for (int i = 0; i < count; i++)
                {
                    points.Add(new Vector2(Mathf.Cos(a * i * Mathf.Deg2Rad) * R + point.x, Mathf.Sin(a * i * Mathf.Deg2Rad) * R + point.y));
                }
                //StartCoroutine(GetEnumerator());
            }
            float time = 0.05f;
            void Update()
            {
                time -= Time.deltaTime;
                if (time > 0)
                {
                    return;
                }
                transform.localPosition = points[index];
                index++;
                index %= points.Count;
                time = 0.05f;
            }
        }

        public class RedLine : MonoBehaviour
        {
            float time = 0.05f;
            float temp = 0f;
            void Update()
            {
                time -= Time.deltaTime;
                if (time > 0)
                {
                    return;
                }
                transform.localRotation = Quaternion.Euler(0, 0, temp);
                temp += 0.2f;
                time = 0.05f;



            }
        }

        public static List<string> Stages = new List<string>()
        {
            "Don_Eyuil",
            "SanSora"
        };

        [HarmonyPatch(typeof(UIInvitationPanel), "GetTheBlueReverberationPrimaryStage")]
        [HarmonyPostfix]
        public static void UIInvitationPanel_GetTheBlueReverberationPrimaryStage_Post(UIInvitationPanel __instance, ref UIStoryLine __result)
        {

            if (__instance == null || __instance.CurrentStage == null || __instance.CurrentStage.storyType == null || !Stages.Exists(x => x == __instance.CurrentStage.storyType))
            {
                return;
            }

            __result = UIStoryLine.BlackSilence;
        }
        [HarmonyPatch(typeof(UIInvitationRightMainPanel), "SetInvBookApplyState")]
        [HarmonyPostfix]
        public static void UIInvitationRightMainPanel_SetInvBookApplyState_Post(InvitationApply_State state, UIInvitationPanel ___invPanel, ref Image ___img_endcontents_content)
        {
            var temp = ___invPanel.CurrentStage;
            if (temp == null || temp.storyType == null)
            {
                return;
            }

            if (state == InvitationApply_State.BlackSilence && Stages.Exists(x => x == temp.storyType))
            {
                ___img_endcontents_content.sprite = TKS_BloodFiend_Initializer.ArtWorks["..D"];
            }
        }
        [HarmonyPatch(typeof(UIInvitationRightMainPanel), "OnClickSendButtonForBlue")]
        [HarmonyPrefix]
        public static bool UIInvitationRightMainPanel_OnClickSendButtonForBlue_Pre(UIInvitationRightMainPanel __instance, UIInvitationPanel ___invPanel)
        {
            StageClassInfo currentStage = ___invPanel.CurrentStage;

            if (currentStage == null || currentStage.storyType == null || !Stages.Exists(x => x == currentStage.storyType))
            {
                return true;
            }

            if (__instance.currentinvState == InvitationApply_State.BlackSilence)
            {
                UIAlarmPopup.instance.SetAlarmTextForBlue(UIAlarmType.StartTheBlueBattlePrimary, (bool yesno) =>
                {
                    if (yesno)
                    {
                        StageClassInfo stageClassInfo = Singleton<StageClassInfoList>.Instance.GetAllDataList().Find((StageClassInfo x) => x.storyType == currentStage.storyType);
                        Singleton<LibraryQuestManager>.Instance.OnSendInvitation(stageClassInfo.id);
                        UI.UIController.Instance.PrepareBattle(stageClassInfo, new List<DropBookXmlInfo>());
                        UISoundManager.instance.PlayEffectSound(UISoundType.Ui_Invite);


                    }
                }, "伟大摩天轮", UIStoryLine.BlackSilence);
                return false;
            }
            return true;

        }

    }
    [HarmonyPatch]
    public class TKS_BloodFiend_PatchMethods_Testify
    {
        [HarmonyPatch]
        public class VersionPatch
        {
            public static MethodBase TargetMethod()
            {
                return AccessTools.Method(typeof(VersionViewer), "Start");
            }
            public unsafe static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator ILcodegenerator)
            {
                List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
                var Local = ILcodegenerator.DeclareLocal(typeof(string));
                for (int i = 1; i < codes.Count; i++)
                {
                    if (codes[i].opcode == OpCodes.Ldfld && codes[i].operand.ToString().Contains("ver"))
                    {
                        codes.InsertRange(i + 1, new List<CodeInstruction>()
                        {
                            new CodeInstruction(OpCodes.Stloc, Local.LocalIndex),
                            new CodeInstruction(OpCodes.Ldloca_S,Local.LocalIndex),
                            new CodeInstruction(OpCodes.Conv_U),
                            new CodeInstruction(OpCodes.Nop).CallInternalDelegate<PatchTools.UnmanagedDelegateTypes.UnmanagedDelegate_1<string>>((string* x) =>{
                                (*x) = "当前血魔Mod测试版版本id:" + TKS_BloodFiend_Initializer.Version;
                                //Debug.LogError(typeof(VersionPatch).GetInternalDelegate()?.Method.DeclaringType.Name);
                                //Debug.LogError(*x +String.Join(",", PatchTools.InternalDelegateCache.Keys));
                            }),
                            new CodeInstruction(OpCodes.Ldloc,Local.LocalIndex),
                        });
                    }
                }
                return codes.AsEnumerable<CodeInstruction>();
            }
        }
    }



    [HarmonyPatch]
    public class TKS_BloodFiend_PatchMethods_CustomCharacterSkin
    {
        public static Workshop.WorkshopAppearanceInfo LoadCustomAppearanceSMotion(string path)
        {
            bool LoadCustomAppearanceInfoSMotion(string rootPath, string xml, ref Workshop.WorkshopAppearanceInfo __result)
            {
                if (__result == null)
                {
                    return false;
                }
                if (string.IsNullOrEmpty(xml))
                {
                    return false;
                }
                StreamReader streamReader = new StreamReader(xml);
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(streamReader.ReadToEnd());
                XmlNode xmlNode = xmlDocument.SelectSingleNode("ModInfo");
                xmlNode.SelectSingleNode("FaceInfo");
                XmlNode xmlNode2 = xmlNode.SelectSingleNode("ClothInfo");
                if (xmlNode2 != null)
                {
                    __result.isClothCustom = true;
                    string innerText = xmlNode2.SelectSingleNode("Name").InnerText;
                    if (!string.IsNullOrEmpty(innerText))
                    {
                        __result.bookName = innerText;
                    }
                    foreach (ActionDetail actionDetail in TKS_BloodFiend_Initializer.TKS_EnumExtension.SMotionExtension.GetExtensionEnum())
                    {
                        string text = actionDetail.ToString();
                        XmlNode xmlNode3 = xmlNode2.SelectSingleNode(text);
                        if (xmlNode3 == null)
                        {
                            text = "Penetrate";
                            xmlNode3 = xmlNode2.SelectSingleNode(text);
                        }
                        if (xmlNode3 != null)
                        {
                            string text2 = rootPath + "/ClothCustom/" + text + ".png";
                            string text3 = rootPath + "/ClothCustom/" + text + "_front.png";
                            XmlNode xmlNode4 = xmlNode3.SelectSingleNode("Pivot");
                            XmlNode namedItem = xmlNode4.Attributes.GetNamedItem("pivot_x");
                            XmlNode namedItem2 = xmlNode4.Attributes.GetNamedItem("pivot_y");
                            XmlNode xmlNode5 = xmlNode3.SelectSingleNode("Head");
                            XmlNode namedItem3 = xmlNode5.Attributes.GetNamedItem("head_x");
                            XmlNode namedItem4 = xmlNode5.Attributes.GetNamedItem("head_y");
                            XmlNode namedItem5 = xmlNode5.Attributes.GetNamedItem("rotation");
                            XmlNode xmlNode6 = xmlNode3.SelectSingleNode("Direction");
                            XmlNode namedItem6 = xmlNode5.Attributes.GetNamedItem("head_enable");
                            float num = float.Parse(namedItem.InnerText);
                            float num2 = float.Parse(namedItem2.InnerText);
                            float num3 = float.Parse(namedItem3.InnerText);
                            float num4 = float.Parse(namedItem4.InnerText);
                            float headRotation = float.Parse(namedItem5.InnerText);
                            bool headEnabled = true;
                            if (namedItem6 != null)
                            {
                                bool.TryParse(namedItem6.InnerText, out headEnabled);
                            }
                            Vector2 pivotPos = new Vector2((num + 512f) / 1024f, (num2 + 512f) / 1024f);
                            Vector2 headPos = new Vector2(num3 / 100f, num4 / 100f);
                            bool hasFrontSprite = false;
                            string text4 = text2;
                            string frontSpritePath = text3;
                            bool hasSpriteFile = false;
                            bool hasFrontSpriteFile = false;
                            if (File.Exists(text2))
                            {
                                hasSpriteFile = true;
                            }
                            if (File.Exists(text3))
                            {
                                hasFrontSprite = true;
                                hasFrontSpriteFile = true;
                            }
                            CharacterMotion.MotionDirection direction = CharacterMotion.MotionDirection.FrontView;
                            if (xmlNode6.InnerText == "Side")
                            {
                                direction = CharacterMotion.MotionDirection.SideView;
                            }
                            Workshop.ClothCustomizeData value = new Workshop.ClothCustomizeData
                            {
                                spritePath = text4,
                                frontSpritePath = frontSpritePath,
                                hasFrontSprite = hasFrontSprite,
                                pivotPos = pivotPos,
                                headPos = headPos,
                                headRotation = headRotation,
                                direction = direction,
                                headEnabled = headEnabled,
                                hasFrontSpriteFile = hasFrontSpriteFile,
                                hasSpriteFile = hasSpriteFile
                            };
                            if (text4 != null)
                            {
                                __result.clothCustomInfo.Add(actionDetail, value);
                            }
                        }
                    }
                }
                return true;
            }
            Workshop.WorkshopAppearanceInfo result = new Workshop.WorkshopAppearanceInfo();
            string xmlinfo = Path.Combine(path, "ModInfo.xml");
            if (File.Exists(xmlinfo) && LoadCustomAppearanceInfoSMotion(path, xmlinfo, ref result))
            {
                return result;
            }
            return null;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Workshop.WorkshopSkinDataSetter), "SetData", argumentTypes: new Type[1] { typeof(Workshop.WorkshopSkinData) })]
        public static void WorkshopSkinDataSetter_SetData_Prefix(Workshop.WorkshopSkinDataSetter __instance, Workshop.WorkshopSkinData data)
        {
            CharacterMotion CopyCharacterMotion(CharacterAppearance apprearance, ActionDetail detail)
            {
                GameObject gameObject = UnityEngine.Object.Instantiate(apprearance._motionList[0].gameObject, apprearance._motionList[0].transform.parent);
                gameObject.name = "Custom_" + detail;
                CharacterMotion component = gameObject.GetComponent<CharacterMotion>();
                component.actionDetail = detail;
                component.motionSpriteSet.Clear();
                component.motionSpriteSet.Add(new SpriteSet(component.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>(), CharacterAppearanceType.Body));
                component.motionSpriteSet.Add(new SpriteSet(component.transform.GetChild(0).GetChild(0).gameObject.GetComponent<SpriteRenderer>(), CharacterAppearanceType.Head));
                component.motionSpriteSet.Add(new SpriteSet(component.transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>(), CharacterAppearanceType.Body));
                return component;
            }

            if (data.contentFolderIdx == TKS_BloodFiend_Initializer.packageId)
            {
                foreach (KeyValuePair<ActionDetail, Workshop.ClothCustomizeData> keyValuePair in data.dic)
                {
                    //Debug.LogError("Actiondetail" + keyValuePair.Key);
                    CharacterMotion characterMotion = __instance.Appearance.GetCharacterMotion(keyValuePair.Key);
                    if (characterMotion == null)
                    {
                        characterMotion = CopyCharacterMotion(__instance.Appearance, keyValuePair.Key);
                        __instance.Appearance._motionList.Add(characterMotion);
                        __instance.Appearance.GetType().GetField("_initialized", AccessTools.all).SetValue(__instance.Appearance, false);
                    }
                }
                if (!(bool)__instance.Appearance.GetType().GetField("_initialized", AccessTools.all).GetValue(__instance.Appearance))
                {
                    __instance.Appearance.Initialize("");
                }
            }

        }
    }
    [HarmonyPatch]
    public class TKS_BloodFiend_PatchMethods_PassiveUI
    {

        [HarmonyPatch(typeof(UILibrarianEquipInfoSlot), "SetData")]
        [HarmonyPostfix]
        public static void UILibrarianEquipInfoSlot_SetData_Post(BookPassiveInfo passive, Image ___Frame, TextMeshProUGUI ___txt_cost)
        {
            void CreateFerrisWheelIcon(int id)
            {
                void InitializeFerrisWheelIcon()
                {
                    ___txt_cost.text = "";
                    GameObject gameObject = new GameObject($"TKS_FerrisWheelCostIcon");
                    gameObject.transform.parent = ___txt_cost.transform;
                    gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
                    gameObject.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                    gameObject.AddComponent<Image>().sprite = TKS_BloodFiend_Initializer.ArtWorks[$"FWC_{id}"];
                }
                foreach (Transform ChildObject in ___txt_cost.transform)
                {
                    if (ChildObject.name == "TKS_FerrisWheelCostIcon")
                    {
                        ChildObject.gameObject.SetActive(false);
                        GameObject.Destroy(ChildObject.gameObject);
                    }
                }
                if (passive.passive.id.packageId == TKS_BloodFiend_Initializer.packageId)
                {
                    switch (passive.passive.id.id)
                    {
                        case 1:
                        case 18:
                            InitializeFerrisWheelIcon();
                            return;
                    }
                }
            }
            if (passive != null)
            {
                CreateFerrisWheelIcon(passive.passive.id.id);
            }
        }
    }
    public class TKS_BloodFiend_Initializer : ModInitializer
    {
        public static string Version = "v2.1";
        public static string packageId = "Don_Eyuil";
        public static Dictionary<string, Sprite> ArtWorks = new Dictionary<string, Sprite>();
        public static string language;
        public class TKS_EnumExtension
        {
            public class TKS_EnumExtender<T> where T : struct, Enum
            {
                public static T2 InsertEnumValue<T2>(string name) where T2 : struct, Enum
                {
                    T2 t;
                    if (EnumExtender.TryGetValueOf<T2>(name, out t) || (EnumExtender.TryFindUnnamedValue<T2>(new T2?(default(T2)), null, false, out t) && EnumExtender.TryAddName<T2>(name, t, false)))
                    {
                        return t;
                    }
                    return default(T2);
                }
                public static void ExtendEnum(Type Extension)
                {
                    Type EnumType = Extension.BaseType.GenericTypeArguments[0];
                    Extension.GetProperties(BindingFlags.Static | BindingFlags.Public).DoIf(x => x.PropertyType == EnumType,
                        act => act.SetValue(null, AccessTools.Method(Extension, "InsertEnumValue").MakeGenericMethod(EnumType).Invoke(null, new object[]
                        {
                            act.Name
                        }
                    )));
                }
                public static List<T> GetExtensionEnum()
                {
                    //Type EnumType = Extension.BaseType.GenericTypeArguments[0];
                    List<T> Enums = new List<T>() { };
                    typeof(TKS_EnumExtension).GetNestedTypes().DoIf(x => !x.IsGenericType && x.BaseType.GenericTypeArguments.Count() > 0 && x.BaseType.GenericTypeArguments[0] == typeof(T), (Type act) =>
                    {
                        act.GetProperties(BindingFlags.Static | BindingFlags.Public).DoIf(y => y.PropertyType == act.BaseType.GenericTypeArguments[0], x => Enums.Add((T)x.GetValue(act)));
                    });
                    return Enums;
                }
            }
            public class SMotionExtension : TKS_EnumExtender<ActionDetail>
            {
                public static ActionDetail TKS_BL_S1 { get; internal set; }
                public static ActionDetail TKS_BL_S2 { get; internal set; }
                public static ActionDetail TKS_BL_S3 { get; internal set; }
                public static ActionDetail TKS_BL_S4 { get; internal set; }
                public static ActionDetail TKS_BL_S5 { get; internal set; }
                public static ActionDetail TKS_BL_S6 { get; internal set; }
                public static ActionDetail TKS_BL_S7 { get; internal set; }
                public static ActionDetail TKS_BL_S8 { get; internal set; }
                public static ActionDetail TKS_BL_S9 { get; internal set; }
                public static ActionDetail TKS_BL_S10 { get; internal set; }
                public static ActionDetail TKS_BL_S11 { get; internal set; }
                public static ActionDetail TKS_BL_S12 { get; internal set; }
                public static ActionDetail TKS_BL_S13 { get; internal set; }
                public static ActionDetail TKS_BL_S14 { get; internal set; }
                public static ActionDetail TKS_BL_S15 { get; internal set; }
                public static ActionDetail TKS_BL_S16 { get; internal set; }
                public static ActionDetail TKS_BL_S17 { get; internal set; }
                public static ActionDetail TKS_BL_S18 { get; internal set; }
                public static ActionDetail TKS_BL_S19 { get; internal set; }
                public static ActionDetail TKS_BL_S20 { get; internal set; }
                public static ActionDetail TKS_BL_S21 { get; internal set; }
                public static ActionDetail TKS_BL_S22 { get; internal set; }
                public static ActionDetail TKS_BL_S23 { get; internal set; }
                public static ActionDetail TKS_BL_S24 { get; internal set; }
                public static ActionDetail TKS_BL_S25 { get; internal set; }
                public static ActionDetail TKS_BL_S26 { get; internal set; }
                public static ActionDetail TKS_BL_S27 { get; internal set; }
                public static ActionDetail TKS_BL_S28 { get; internal set; }
                public static ActionDetail TKS_BL_S29 { get; internal set; }
                public static ActionDetail TKS_BL_S30 { get; internal set; }
                public static ActionDetail TKS_BL_S31 { get; internal set; }
                public static ActionDetail TKS_BL_S32 { get; internal set; }
                public static ActionDetail TKS_BL_S33 { get; internal set; }
                public static ActionDetail TKS_BL_S34 { get; internal set; }
                public static ActionDetail TKS_BL_S35 { get; internal set; }
                public static ActionDetail TKS_BL_S36 { get; internal set; }
                public static ActionDetail TKS_BL_S37 { get; internal set; }
                public static ActionDetail TKS_BL_S38 { get; internal set; }
                public static ActionDetail TKS_BL_S39 { get; internal set; }
                public static ActionDetail TKS_BL_S40 { get; internal set; }
                public static ActionDetail TKS_BL_S41 { get; internal set; }
                public static ActionDetail TKS_BL_S42 { get; internal set; }
                public static ActionDetail TKS_BL_S43 { get; internal set; }
                public static ActionDetail TKS_BL_S44 { get; internal set; }
                public static ActionDetail TKS_BL_S45 { get; internal set; }
                public static ActionDetail TKS_BL_S46 { get; internal set; }
                public static ActionDetail TKS_BL_S47 { get; internal set; }
                public static ActionDetail TKS_BL_S48 { get; internal set; }
                public static ActionDetail TKS_BL_S49 { get; internal set; }
                public static ActionDetail TKS_BL_S50 { get; internal set; }
                public static ActionDetail TKS_BL_S51 { get; internal set; }
                public static ActionDetail TKS_BL_S52 { get; internal set; }
                public static ActionDetail TKS_BL_S53 { get; internal set; }
                public static ActionDetail TKS_BL_S54 { get; internal set; }
                public static ActionDetail TKS_BL_S55 { get; internal set; }
                public static ActionDetail TKS_BL_S56 { get; internal set; }
                public static ActionDetail TKS_BL_S57 { get; internal set; }
                public static ActionDetail TKS_BL_S58 { get; internal set; }
                public static ActionDetail TKS_BL_S59 { get; internal set; }
                public static ActionDetail TKS_BL_S60 { get; internal set; }
                public static ActionDetail TKS_BL_S61 { get; internal set; }
                public static ActionDetail TKS_BL_S62 { get; internal set; }
                public static ActionDetail TKS_BL_S63 { get; internal set; }
                public static ActionDetail TKS_BL_S64 { get; internal set; }
                public static ActionDetail TKS_BL_S65 { get; internal set; }
                public static ActionDetail TKS_BL_S66 { get; internal set; }
                public static ActionDetail TKS_BL_S67 { get; internal set; }
                public static ActionDetail TKS_BL_S68 { get; internal set; }
                public static ActionDetail TKS_BL_S69 { get; internal set; }
                public static ActionDetail TKS_BL_S70 { get; internal set; }
                public static ActionDetail TKS_BL_S71 { get; internal set; }
                public static ActionDetail TKS_BL_S72 { get; internal set; }
                public static ActionDetail TKS_BL_S73 { get; internal set; }
                public static ActionDetail TKS_BL_S74 { get; internal set; }
                public static ActionDetail TKS_BL_S75 { get; internal set; }
                public static ActionDetail TKS_BL_S76 { get; internal set; }
                public static ActionDetail TKS_BL_S77 { get; internal set; }
                public static ActionDetail TKS_BL_S78 { get; internal set; }
                public static ActionDetail TKS_BL_S79 { get; internal set; }
                public static ActionDetail TKS_BL_S80 { get; internal set; }
                public static ActionDetail TKS_BL_S81 { get; internal set; }
                public static ActionDetail TKS_BL_S82 { get; internal set; }
                public static ActionDetail TKS_BL_S83 { get; internal set; }
                public static ActionDetail TKS_BL_S84 { get; internal set; }
                public static ActionDetail TKS_BL_S85 { get; internal set; }
                public static ActionDetail TKS_BL_S86 { get; internal set; }
                public static ActionDetail TKS_BL_S87 { get; internal set; }
                public static ActionDetail TKS_BL_S88 { get; internal set; }
                public static ActionDetail TKS_BL_S89 { get; internal set; }
                public static ActionDetail TKS_BL_S90 { get; internal set; }
                public static ActionDetail TKS_BL_S91 { get; internal set; }
                public static ActionDetail TKS_BL_S92 { get; internal set; }
                public static ActionDetail TKS_BL_S93 { get; internal set; }
                public static ActionDetail TKS_BL_S94 { get; internal set; }
                public static ActionDetail TKS_BL_S95 { get; internal set; }
                public static ActionDetail TKS_BL_S96 { get; internal set; }
                public static ActionDetail TKS_BL_S97 { get; internal set; }
                public static ActionDetail TKS_BL_S98 { get; internal set; }
                public static ActionDetail TKS_BL_S99 { get; internal set; }
                public static ActionDetail TKS_BL_S100 { get; internal set; }
                public static ActionDetail TKS_BL_S101 { get; internal set; }
                public static ActionDetail TKS_BL_S102 { get; internal set; }
                public static ActionDetail TKS_BL_S103 { get; internal set; }
            }
            public class DiceFlagExtension : TKS_EnumExtender<DiceFlag>
            {
                public static DiceFlag HasGivenDamage_SubTarget { get; internal set; }
                public static DiceFlag HasGivenDamage_BattleUnitBuf_Year { get; internal set; }
                public static DiceFlag HasGivenDamage { get; internal set; }
            }
        }
        public static string GetPassiveName(int id)
        {
            return Singleton<PassiveXmlList>.Instance.GetDataAll().Find((PassiveXmlInfo x) => x.id == new LorId(packageId, id)).name;
        }
        public static string GetPassiveDesc(int id)
        {
            return Singleton<PassiveXmlList>.Instance.GetDataAll().Find((PassiveXmlInfo x) => x.id == new LorId(packageId, id)).desc;
        }
        private static FileInfo[] SafeGetFiles(string path)
        {
            try
            {
                return new DirectoryInfo(path).GetFiles();
            }
            catch (Exception ex)
            {
                Debug.LogErrorFormat("DONEYUILLOADERROR:", new object[]
                {
                    path,
                    ex
                });
            }
            return Array.Empty<FileInfo>();
        }

        public static void DonEyuilLoad(string DllPath)
        {
            void LoadLocalize(string LocalizeKey)
            {
                void AddLocalize_EffectTexts()
                {
                    Dictionary<string, BattleEffectText> dictionary = typeof(BattleEffectTextsXmlList).GetField("_dictionary", AccessTools.all).GetValue(Singleton<BattleEffectTextsXmlList>.Instance) as Dictionary<string, BattleEffectText>;
                    FileInfo[] files = TKS_BloodFiend_Initializer.SafeGetFiles(DllPath + "/Localize/" + TKS_BloodFiend_Initializer.language + "/" + LocalizeKey + "/EffectTexts");
                    for (int i = 0; i < files.Length; i++)
                    {
                        using (StringReader stringReader = new StringReader(File.ReadAllText(files[i].FullName)))
                        {
                            BattleEffectTextRoot battleEffectTextRoot = (BattleEffectTextRoot)new XmlSerializer(typeof(BattleEffectTextRoot)).Deserialize(stringReader);
                            for (int j = 0; j < battleEffectTextRoot.effectTextList.Count; j++)
                            {
                                BattleEffectText battleEffectText = battleEffectTextRoot.effectTextList[j];
                                dictionary.Add(battleEffectText.ID, battleEffectText);
                            }
                        }
                    }
                }

                void LoadLocalize_BattleCardAbilities()
                {
                    FileInfo[] array = TKS_BloodFiend_Initializer.SafeGetFiles(DllPath + "/Localize/" + TKS_BloodFiend_Initializer.language + "/" + LocalizeKey + "/BattleCardAbilities");
                    for (int i = 0; i < array.Length; i++)
                    {
                        using (StringReader stringReader = new StringReader(File.ReadAllText(array[i].FullName)))
                        {
                            foreach (BattleCardAbilityDesc battleCardAbilityDesc in ((BattleCardAbilityDescRoot)new XmlSerializer(typeof(BattleCardAbilityDescRoot)).Deserialize(stringReader)).cardDescList)
                            {
                                try
                                {
                                    Singleton<BattleCardAbilityDescXmlList>.Instance.GetData(battleCardAbilityDesc.id).desc = battleCardAbilityDesc.desc;
                                }
                                catch (Exception ex)
                                {
                                    Debug.LogErrorFormat("Failed Load Localize:{0}\nCause:{1}", new object[]
                                    {
                                        battleCardAbilityDesc.id,
                                        ex
                                    });
                                }
                            }
                        }
                    }
                }
                void AddLocalize_Etc()
                {
                    foreach (FileInfo fileInfo in TKS_BloodFiend_Initializer.SafeGetFiles(DllPath + "/Localize/" + TKS_BloodFiend_Initializer.language + "/" + LocalizeKey + "/etc"))
                    {
                        XmlDocument xmlDocument = new XmlDocument();
                        xmlDocument.LoadXml(File.ReadAllText(fileInfo.FullName));
                        foreach (object obj in xmlDocument.SelectNodes("localize/text"))
                        {
                            try
                            {
                                XmlNode xmlNode = (XmlNode)obj;
                                if (xmlNode.Attributes.GetNamedItem("id") != null)
                                {
                                    TextDataModel.textDic[xmlNode.Attributes.GetNamedItem("id").InnerText] = xmlNode.InnerText;
                                }
                            }
                            catch (Exception ex)
                            {
                                Debug.LogErrorFormat("Failed Load Localize:{0}\nCause:{1}", new object[]
                                {
                                    obj,
                                    ex
                                });
                            }
                        }
                    }
                }
                LoadLocalize_BattleCardAbilities();
                AddLocalize_EffectTexts();
                AddLocalize_Etc();
            }
            void LoadCustomSkin(string path)
            {
                try
                {
                    //string text = Path.Combine(path, "..", "Resource\\CharacterSkin");
                    if (Directory.Exists(path))
                    {
                        string[] directories5 = Directory.GetDirectories(path);
                        for (int i = 0; i < directories5.Length; i++)
                        {
                            string[] array5 = directories5[i].Split('\\');
                            Workshop.WorkshopSkinData workshopBookSkinData = Singleton<CustomizingBookSkinLoader>.Instance.GetWorkshopBookSkinData(packageId, array5[array5.Length - 1]);
                            if (workshopBookSkinData != null && (workshopBookSkinData.dataName == "Don_Eyuil") || (workshopBookSkinData.dataName == "San_Sora"))
                            {
                                foreach (KeyValuePair<ActionDetail, Workshop.ClothCustomizeData> keyValuePair in TKS_BloodFiend_PatchMethods_CustomCharacterSkin.LoadCustomAppearanceSMotion(directories5[i]).clothCustomInfo)
                                {
                                    workshopBookSkinData.dic[keyValuePair.Key] = keyValuePair.Value;
                                }
                            }
                        }
                    }
                }
                catch (Exception _)
                {
                    Debug.LogError("LoadError" + _.Message);
                }
            }
            void LoadArtWorks(DirectoryInfo dir)
            {
                if (dir.GetDirectories().Length != 0)
                {
                    DirectoryInfo[] directories = dir.GetDirectories();
                    for (int i = 0; i < directories.Length; i++)
                    {
                        LoadArtWorks(directories[i]);
                    }
                }
                foreach (System.IO.FileInfo fileInfo in dir.GetFiles())
                {
                    Texture2D texture2D = new Texture2D(2, 2);
                    texture2D.LoadImage(File.ReadAllBytes(fileInfo.FullName));
                    Sprite value = Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0f, 0f));
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileInfo.FullName);
                    TKS_BloodFiend_Initializer.ArtWorks[fileNameWithoutExtension] = value;
                }
            }

            LoadCustomSkin(Path.Combine(DllPath, "..", "Resource\\CharacterSkin"));
            LoadArtWorks(new DirectoryInfo(DllPath + "/ArtWork"));
            LoadLocalize("Don_Eyuil");
            LoadLocalize("San_Sora");
            LoadLocalize("WhiteMoon_Sparkle");
        }
        public override void OnInitializeMod()
        {
            TKS_BloodFiend_Initializer.language = GlobalGameManager.Instance.CurrentOption.language;

            Harmony harmony = new Harmony(packageId);
            harmony.PatchAll();
            //Extra扩展Patch---------------------------------------------------------//
            harmony.PatchAll(typeof(EmotionEgoXmlInfo_Mod));
            harmony.PatchAll(typeof(TKS_BloodFiend_PatchMethods_CustomCharacterSkin));
            harmony.PatchAll(typeof(TKS_BloodFiend_PatchMethods_PassiveUI));
            harmony.PatchAll(typeof(TKS_BloodFiend_PatchMethods_StoryFerrisWheel));
            //harmony.PatchAll(typeof(TKS_BloodFiend_PatchMethods_Testify.TransBehavior_AtkVSDfnPatch));
            //-----------------------------------------------------------------------//

            //Buff基类时点Patch------------------------------------------------------//
            harmony.PatchAll(typeof(BattleUnitBuf_Don_Eyuil));
            harmony.PatchAll(typeof(BattleUnitBuf_Don_Eyuil.OnTakeBleedingDamagePatch));
            harmony.PatchAll(typeof(BattleUnitBuf_Don_Eyuil.OnStartBattlePatch));
            harmony.PatchAll(typeof(BattleUnitBuf_Don_Eyuil.BeforeAddKeywordBufPatch));
            harmony.PatchAll(typeof(BattleUnitBuf_Don_Eyuil.AfterAddKeywordBufPatch));
            harmony.PatchAll(typeof(BattleUnitBuf_Don_Eyuil.BeforeAddEmotionCoinPatch));
            harmony.PatchAll(typeof(BattleUnitBuf_Don_Eyuil.BeforeRecoverHpPatch));
            harmony.PatchAll(typeof(BattleUnitBuf_Don_Eyuil.BeforeRecoverPlayPointPatch));
            harmony.PatchAll(typeof(BattleUnitBuf_Don_Eyuil.CanForcelyAggroPatch));
            //harmony.PatchAll(typeof(BattleUnitBuf_Don_Eyuil.AfterApplyEnemyCardPatch));
            //-----------------------------------------------------------------------//

            //被动基类时点Patch------------------------------------------------------//
            harmony.PatchAll(typeof(PassiveAbilityBase_Don_Eyuil));
            harmony.PatchAll(typeof(PassiveAbilityBase_Don_Eyuil.OnStartBattleTheLastPatch));
            harmony.PatchAll(typeof(PassiveAbilityBase_Don_Eyuil.AfterApplyEnemyCardPatch));
            //-----------------------------------------------------------------------//

            //硬血术选卡扩展Patch----------------------------------------------------//
            harmony.PatchAll(typeof(HardBloodCards));
            //-----------------------------------------------------------------------//

            //存存币基类扩展Patch----------------------------------------------------//
            harmony.PatchAll(typeof(RedDiceCardAbility));
            //-----------------------------------------------------------------------//

            //被动效果Patch----------------------------------------------------------//
            harmony.PatchAll(typeof(PassiveAbility_DonEyuil_15));
            harmony.PatchAll(typeof(PassiveAbility_SanSora_10));
            harmony.PatchAll(typeof(PassiveAbility_SanSora_08));
            harmony.PatchAll(typeof(PassiveAbility_WhiteMoonSparkle_14));
            harmony.PatchAll(typeof(PassiveAbility_WhiteMoonSparkle_16));
            //-----------------------------------------------------------------------//

            //Buff效果Patch----------------------------------------------------------//
            harmony.PatchAll(typeof(BattleUnitBuf_UncondensableBlood));
            harmony.PatchAll(typeof(BattleUnitBuf_BloodShield));
            harmony.PatchAll(typeof(BattleUnitBuf_Know));
            harmony.PatchAll(typeof(BattleUnitBuf_Sword));
            harmony.PatchAll(typeof(BattleUnitBuf_Year.BattleDiceCardBuf_TransDice.DiceTransformPatch));
            //-----------------------------------------------------------------------//

            //骰子效果Patch----------------------------------------------------------//
            harmony.PatchAll(typeof(DiceCardAbility_DonEyuil_20));
            harmony.PatchAll(typeof(DiceCardAbility_不承受反震伤害));
            //-----------------------------------------------------------------------//

            //书页效果Patch----------------------------------------------------------//
            harmony.PatchAll(typeof(DiceCardSelfAbility_DonEyuil_21.BattleUnitBuf_AntiBleeding));
            //-----------------------------------------------------------------------//

            //测试Patch----------------------------------------------------------//
            harmony.PatchAll(typeof(TKS_BloodFiend_PatchMethods_Testify));
            //-----------------------------------------------------------------------//



            //typeof(TKS_EnumExtension).GetNestedTypes().DoIf(x => !x.IsGenericType, act => TKS_EnumExtension.ExtendEnum(act));

            TKS_EnumExtension.SMotionExtension.ExtendEnum(typeof(TKS_EnumExtension.SMotionExtension));
            TKS_EnumExtension.DiceFlagExtension.ExtendEnum(typeof(TKS_EnumExtension.DiceFlagExtension));
            // Debug.LogError(String.Join(".", Enum.GetNames(typeof(ActionDetail))));
            DonEyuilLoad(Path.GetDirectoryName(Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().CodeBase).Path)));

        }
    }



    public class MyId
    {
        public static LorId 未实现id = MyTools.Create(0);
        public static LorId Card_血之宝库_1 = MyTools.Create(1);
        public static LorId Card_血剑斩击 = MyTools.Create(2);
        public static LorId Card_凝血化锋_1 = MyTools.Create(3);
        public static LorId Card_剑刃截断 = MyTools.Create(4);
        public static LorId Card_堂埃尤尔派硬血术1式_血剑_1 = MyTools.Create(5);
        public static LorId Card_高速穿刺 = MyTools.Create(6);
        public static LorId Card_长枪冲锋 = MyTools.Create(7);
        public static LorId Card_堂埃尤尔派硬血术2式_血枪_1 = MyTools.Create(8);
        public static LorId Card_镰刃截断 = MyTools.Create(9);
        public static LorId Card_巨镰纵切 = MyTools.Create(10);
        public static LorId Card_堂埃尤尔派硬血术3式_血镰_1 = MyTools.Create(11);
        public static LorId Card_血刃割裂 = MyTools.Create(12);
        public static LorId Card_血刃环切 = MyTools.Create(13);
        public static LorId Card_堂埃尤尔派硬血术4式_血刃_1 = MyTools.Create(14);
        public static LorId Card_迅捷剑击 = MyTools.Create(15);
        public static LorId Card_堂埃尤尔派硬血术5式_双剑_1 = MyTools.Create(16);
        public static LorId Card_血液凝结 = MyTools.Create(17);
        public static LorId Card_堂埃尤尔派硬血术6式_血甲_1 = MyTools.Create(18);
        public static LorId Card_血之壁垒 = MyTools.Create(19);
        public static LorId Card_硬血化铠 = MyTools.Create(20);
        public static LorId Card_穿云血箭 = MyTools.Create(21);
        public static LorId Card_血箭连射 = MyTools.Create(22);
        public static LorId Card_堂埃尤尔派硬血术7式_血弓_1 = MyTools.Create(23);
        public static LorId Card_血鞭抽打 = MyTools.Create(24);
        public static LorId Card_堂埃尤尔派硬血术8式_血鞭_1 = MyTools.Create(25);
        public static LorId Card_血伞挥打_1 = MyTools.Create(26);
        public static LorId Card_堂埃尤尔派硬血术9式_血伞_1 = MyTools.Create(27);
        public static LorId Card_为仍在饥渴中的家人设下的晚宴 = MyTools.Create(28);
        public static LorId Card_必须担负的责任 = MyTools.Create(29);
        public static LorId Card_这绝非理想中的共存 = MyTools.Create(30);
        public static LorId Card_但我有亲族_家人_不能视而不见 = MyTools.Create(31);
        public static LorId Card_凝血化锋_2 = MyTools.Create(32);
        public static LorId Card_纵血为刃_1 = MyTools.Create(33);
        public static LorId Card_硬血截断_1 = MyTools.Create(34);
        public static LorId Card_血如泉涌_1 = MyTools.Create(35);
        public static LorId Card_梦之冒险_1 = MyTools.Create(36);
        public static LorId Card_冲锋_驽骍难得_1 = MyTools.Create(37);
        public static LorId Card_旋转_绽放把_1 = MyTools.Create(38);
        public static LorId Card_堂埃尤尔派硬血术终式_La_Sangre_1 = MyTools.Create(39);
        public static LorId Card_便以决斗作为这场战斗的结尾吧 = MyTools.Create(40);
        public static LorId Card_你是否心怀梦想_无所畏惧 = MyTools.Create(41);
        public static LorId Card_你是否相信希望_憧憬未来 = MyTools.Create(42);
        public static LorId Card_你是否肩负责任_永不抛弃 = MyTools.Create(43);
        public static LorId Card_你是否心怀理解_尊重他人 = MyTools.Create(44);
        public static LorId Card_堂埃尤尔派硬血术1式_血剑_2 = MyTools.Create(45);
        public static LorId Card_堂埃尤尔派硬血术2式_血枪_2 = MyTools.Create(46);
        public static LorId Card_堂埃尤尔派硬血术3式_血镰_2 = MyTools.Create(47);
        public static LorId Card_堂埃尤尔派硬血术4式_血刃_2 = MyTools.Create(48);
        public static LorId Card_堂埃尤尔派硬血术5式_双剑_2 = MyTools.Create(49);
        public static LorId Card_堂埃尤尔派硬血术6式_血甲_2 = MyTools.Create(50);
        public static LorId Card_堂埃尤尔派硬血术7式_血弓_2 = MyTools.Create(51);
        public static LorId Card_堂埃尤尔派硬血术8式_血鞭_2 = MyTools.Create(52);
        public static LorId Card_堂埃尤尔派硬血术9式_血伞_2 = MyTools.Create(53);
        public static LorId Card_堂埃尤尔派硬血术终式_La_Sangre_2 = MyTools.Create(54);
        public static LorId Card_血之宝库_2 = MyTools.Create(55);
        public static LorId Card_冲锋_驽骍难得_2 = MyTools.Create(56);
        public static LorId Card_血伞挥打_2 = MyTools.Create(57);
        public static LorId Card_旋转_绽放把_2 = MyTools.Create(58);
        public static LorId Card_凝血化锋_3 = MyTools.Create(59);
        public static LorId Card_纵血为刃_2 = MyTools.Create(60);
        public static LorId Card_硬血截断_2 = MyTools.Create(61);
        public static LorId Card_血如泉涌_2 = MyTools.Create(62);
        public static LorId Card_梦之冒险_2 = MyTools.Create(63);
        public static LorId Card_经典反击书页 = MyTools.Create(64);
        public static LorId Card_双剑反击闪避书页 = MyTools.Create(65);
        public static LorId Card_血伞反击 = MyTools.Create(66);
        public static LorId Card_若能摆脱这可怖的疾病 = MyTools.Create(67);

        public static LorId Card_致伤 = MyTools.Create(77);
        public static LorId Card_摧垮 = MyTools.Create(78);
        public static LorId Card_伴血猛袭 = MyTools.Create(79);
        public static LorId Card_释血化刃 = MyTools.Create(80);
        public static LorId Card_冲锋截断 = MyTools.Create(81);
        public static LorId Card_桑空派变体硬血术6式_血甲 = MyTools.Create(82);
        public static LorId Card_硬血为铠 = MyTools.Create(83);
        public static LorId Card_利血贯穿 = MyTools.Create(84);
        public static LorId Card_血刃剔除 = MyTools.Create(124);
        public static LorId Card_受苦的亲族正在增多 = MyTools.Create(125);
        public static LorId Card_若能摆脱这可怖的疾病_2 = MyTools.Create(126);
        public static LorId Card_翱翔向梦 = MyTools.Create(127);
        public static LorId Card_桑空派变体硬血术终式_La_Sangre = MyTools.Create(128);
        public static LorId Card_直到触及到那梦想 = MyTools.Create(129);
        public static LorId Card_Desc_桑空派变体硬血术1式_血剑 = MyTools.Create(85);
        public static LorId Card_Desc_桑空派变体硬血术2式_血枪 = MyTools.Create(86);
        public static LorId Card_Desc_桑空派变体硬血术3式_血镰 = MyTools.Create(87);
        public static LorId Card_Desc_桑空派变体硬血术4式_血刃 = MyTools.Create(88);
        public static LorId Card_Desc_桑空派变体硬血术5式_双剑 = MyTools.Create(89);
        public static LorId Card_Desc_桑空派变体硬血术6式_血甲 = MyTools.Create(90);
        public static LorId Card_Desc_桑空派变体硬血术7式_血弓 = MyTools.Create(91);
        public static LorId Card_Desc_桑空派变体硬血术8式_血鞭 = MyTools.Create(92);
        public static LorId Card_桑空派变体硬血术终式_La_Sangre_2 = MyTools.Create(94);
        public static LorId Card_释血化刃_2 = MyTools.Create(95);
        public static LorId Card_冲锋截断_2 = MyTools.Create(96);
        public static LorId Card_硬血为铠_2 = MyTools.Create(97);
        public static LorId Card_利血贯穿_2 = MyTools.Create(98);
        public static LorId Card_血刃剔除_2 = MyTools.Create(99);
        public static LorId Card_翱翔向梦_2 = MyTools.Create(100);



        public static LorId Card_一如既往_埃尤尔 = MyTools.Create(102);
        public static LorId Card_一如既往_小耀 = MyTools.Create(101);
        public static LorId Card_所护之物_泉之龙_秋之莲 = MyTools.Create(106);
        public static LorId Card_所护之物_千斤弓 = MyTools.Create(107);
        public static LorId Card_所护之物_月之剑 = MyTools.Create(108);
        public static LorId Card_所见之梦_泉之龙_秋之莲 = MyTools.Create(109);
        public static LorId Card_所见之梦_千斤弓 = MyTools.Create(110);
        public static LorId Card_所见之梦_月之剑 = MyTools.Create(111);
        public static LorId Card_传承之梦_泉之龙_秋之莲 = MyTools.Create(113);
        public static LorId Card_传承之梦_千斤弓 = MyTools.Create(114);
        public static LorId Card_传承之梦_月之剑 = MyTools.Create(115);
        public static LorId Card_月下终曲 = MyTools.Create(116);
        public static LorId Card_Desc_泉之龙_秋之莲 = MyTools.Create(130);
        public static LorId Card_Desc_千斤弓 = MyTools.Create(131);
        public static LorId Card_Desc_月之剑 = MyTools.Create(132);
        public static LorId Card_反击通用书页 = MyTools.Create(133);

        public static LorId Book_堂_埃尤尔之页 = MyTools.Create(10000001);
        public static LorId Book_桑空之页 = MyTools.Create(10000002);
        public static LorId Book_小耀之页 = MyTools.Create(10000003);
        public static LorId Stage_埃尤尔 = MyTools.Create(1);
        public static LorId Stage_桑空 = MyTools.Create(2);
        public static LorId Stage_白月 = MyTools.Create(3);
        public static LorId Stage_测试 = MyTools.Create(881506);
        public static ulong User_漠北九月 = 76561198941514651;
        public static ulong User_小D = 76561199079466854;
        public static ulong User_天空 = 76561198877012566;
        public static ulong User_139 = 76561198995229429;
        public static ulong User_回声 = 1554789556;


        public static List<LorId> Books_拉曼查乐园的血魔 = new List<LorId>()
        {
            Book_堂_埃尤尔之页,
            Book_桑空之页
        };

        /// <summary>
        /// 通过给定的命名空间返回与之对应的核心书页ID
        /// </summary>
        /// <param name="NameSpace">给定的命名空间</param>
        /// <returns>返回参数的存储形式为(玩家核心ID,来宾核心ID)</returns>
        public static (LorId, LorId) Mapping_Books_命名空间与核心书页映射(string NameSpace)
        {
            return NameSpace.Contains("San_Sora") ? (MyTools.Create(10000002), MyTools.Create(8)) :
                NameSpace.Contains("Don_Eyuil") ? (MyTools.Create(10000001), MyTools.Create(1)) :
                (null, null);
        }
    }

}
