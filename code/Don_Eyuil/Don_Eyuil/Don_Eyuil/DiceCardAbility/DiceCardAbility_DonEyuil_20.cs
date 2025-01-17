using Don_Eyuil.Don_Eyuil.Buff;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace Don_Eyuil.Don_Eyuil.DiceCardAbility
{
    public class DiceCardAbility_DonEyuil_20 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]恢复等同于造成伤害量的体力溢出部分将转化为等量护盾";

        public static void AfterGiveDamage(BattleDiceBehavior behavior, int dmg)
        {
            if (!behavior.abilityList.Exists(x => x is DiceCardAbility_DonEyuil_20))
            {
                return;
            }
            if (behavior.owner.hp + dmg <= behavior.owner.MaxHp)
            {
                behavior.owner.RecoverHP(dmg);
            }
            else
            {
                behavior.owner.RecoverHP(behavior.owner.MaxHp - (int)behavior.owner.hp);
                int temp = dmg - (behavior.owner.MaxHp - (int)behavior.owner.hp);
                BattleUnitBuf_PhysicalShield.AddBuf(behavior.owner, temp);
            }
        }

        [HarmonyPatch(typeof(BattleDiceBehavior), "GiveDamage")]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> BattleDiceBehavior_GiveDamage_Tran(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
            int index = codes.FindIndex(x => x.operand.ToString().Contains("::TakeDamage") && x.opcode == OpCodes.Callvirt);
            codes.InsertRange(index + 1, new List<CodeInstruction>()
            {
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldloca_S, 13),
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(DiceCardAbility_DonEyuil_20), "AfterGiveDamage"))
            });
            return codes.AsEnumerable();
        }
    }
}
