using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

[HarmonyPatch(typeof(publisherScript), "CreateNewGame2")]
public static class CreateNewGame2_ProjectType_Transpiler
{
    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        bool injected = false;
        foreach (var instr in instructions)
        {
            if (!injected && instr.opcode == OpCodes.Switch)
            {
                yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(SubsidiaryTeamsPlugin), "OverrideProjectNum"));
                injected = true;
            }
            yield return instr;
        }
    }
}
