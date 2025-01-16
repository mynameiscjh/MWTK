using HarmonyLib;

namespace Don_Eyuil
{
    public class ModInit : ModInitializer
    {
        public static string id = "Don_Eyuil";

        public override void OnInitializeMod()
        {
            base.OnInitializeMod();
            AddPatch();
        }

        public static void AddPatch()
        {
            Harmony harmony = new Harmony("139");
            harmony.PatchAll(typeof(EmotionEgoXmlInfo_Mod));
        }
    }

    public class EmotionEgoXmlInfo_Mod : EmotionEgoXmlInfo
    {
        public string packageId = "";

        [HarmonyPatch(typeof(EmotionEgoXmlInfo), "get_CardId")]
        [HarmonyPostfix]
        public static void EmotionEgoXmlInfo_get_CardId_Post(EmotionEgoXmlInfo __instance, LorId __result)
        {
            if (__instance is EmotionEgoXmlInfo_Mod)
            {
                __result = new LorId((__instance as EmotionEgoXmlInfo_Mod).packageId, __instance.id);
            }
        }
    }

}
