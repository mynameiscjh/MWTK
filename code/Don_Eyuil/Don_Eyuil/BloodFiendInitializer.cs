using Don_Eyuil.PassiveAbility;
using EnumExtenderV2;
using HarmonyLib;
using LOR_DiceSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using UnityEngine;
using Workshop;

namespace Don_Eyuil
{






    [HarmonyPatch]
    public class TKS_BloodFiend_PatchMethods_CustomCharacterSkin
    {
        public static WorkshopAppearanceInfo LoadCustomAppearanceSMotion(string path)
        {
            bool LoadCustomAppearanceInfoSMotion(string rootPath, string xml, ref WorkshopAppearanceInfo __result)
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
                            ClothCustomizeData value = new ClothCustomizeData
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
            WorkshopAppearanceInfo result = new WorkshopAppearanceInfo();
            string xmlinfo = Path.Combine(path, "ModInfo.xml");
            if (File.Exists(xmlinfo) && LoadCustomAppearanceInfoSMotion(path, xmlinfo, ref result))
            {
                return result;
            }
            return null;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(WorkshopSkinDataSetter), "SetData", argumentTypes: new Type[1] { typeof(WorkshopSkinData) })]
        public static void WorkshopSkinDataSetter_SetData_Prefix(WorkshopSkinDataSetter __instance, WorkshopSkinData data)
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
                foreach (KeyValuePair<ActionDetail, ClothCustomizeData> keyValuePair in data.dic)
                {
                    Debug.LogError("Actiondetail" + keyValuePair.Key);
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
    public class TKS_BloodFiend_Initializer : ModInitializer
    {
        public static string packageId = "Don_Eyuil";

        public static Dictionary<string, Sprite> ArtWorks = new Dictionary<string, Sprite>();

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
            }
        }
        public static void DonEyuilLoad(string DllPath)
        {

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
                            WorkshopSkinData workshopBookSkinData = Singleton<CustomizingBookSkinLoader>.Instance.GetWorkshopBookSkinData(packageId, array5[array5.Length - 1]);
                            if (workshopBookSkinData != null && workshopBookSkinData.dataName == "Don_Eyuil")
                            {
                                foreach (KeyValuePair<ActionDetail, ClothCustomizeData> keyValuePair in TKS_BloodFiend_PatchMethods_CustomCharacterSkin.LoadCustomAppearanceSMotion(directories5[i]).clothCustomInfo)
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
        }

        public override void OnInitializeMod()
        {
            Harmony harmony = new Harmony(packageId);
            harmony.PatchAll();
            harmony.PatchAll(typeof(EmotionEgoXmlInfo_Mod));
            harmony.PatchAll(typeof(BattleUnitBuf_Don_Eyuil.OnTakeBleedingDamagePatch));
            harmony.PatchAll(typeof(BattleUnitBuf_Don_Eyuil.OnStartBattlePatch));
            harmony.PatchAll(typeof(BattleUnitBuf_Don_Eyuil.BeforeAddKeywordBufPatch));
            harmony.PatchAll(typeof(BattleUnitBuf_UncondensableBlood));
            harmony.PatchAll(typeof(PassiveAbility_DonEyuil_15));
            harmony.PatchAll(typeof(RedDiceCardAbility));
            //typeof(TKS_EnumExtension).GetNestedTypes().DoIf(x => !x.IsGenericType, act => TKS_EnumExtension.ExtendEnum(act));
            TKS_EnumExtension.SMotionExtension.ExtendEnum(typeof(TKS_EnumExtension.SMotionExtension));
            Debug.LogError(String.Join(".", Enum.GetNames(typeof(ActionDetail))));
            DonEyuilLoad(Path.GetDirectoryName(Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().CodeBase).Path)));

        }
    }
    public class EmotionEgoXmlInfo_Mod : EmotionEgoXmlInfo
    {
        public string packageId = "";

        public EmotionEgoXmlInfo_Mod(LorId id)
        {
            this.packageId = id.packageId;
            this._CardId = id.id;
        }
        public EmotionEgoXmlInfo_Mod()
        {

        }
        [HarmonyPatch(typeof(EmotionEgoXmlInfo), "get_CardId")]
        [HarmonyPostfix]
        public static void EmotionEgoXmlInfo_get_CardId_Post(EmotionEgoXmlInfo __instance, ref LorId __result)
        {
            if (__instance is EmotionEgoXmlInfo_Mod)
            {
                __result = new LorId((__instance as EmotionEgoXmlInfo_Mod).packageId, __instance._CardId);

                if (!ItemXmlDataList.instance.GetFieldValue<Dictionary<LorId, DiceCardXmlInfo>>("_cardInfoTable").TryGetValue(__result, out var v))
                {
                    var card = ItemXmlDataList.instance.GetCardItem(__result);
                    ItemXmlDataList.instance.GetFieldValue<Dictionary<LorId, DiceCardXmlInfo>>("_cardInfoTable").Add(__result, card);
                }
            }
        }
    }

    public class MyId
    {
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
        public static LorId Book_堂_埃尤尔之页 = MyTools.Create(10000001);
    }

}
