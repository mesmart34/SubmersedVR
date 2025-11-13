using System.Reflection;
using HarmonyLib;

namespace SubmersedVR.Patches;

[HarmonyPatch(typeof(uGUI_CraftingMenu))]
public static class CraftingMenuUseControllerButtons
{
    public static MethodBase TargetMethod()
    {
        var type = typeof(uGUI_CraftingMenu);
        return AccessTools.FirstMethod(type, method => method.Name.Contains("OnPointerClick"));
    }

    static bool Prefix(ref bool __result, uGUI_CraftingMenu __instance, uGUI_ItemIcon icon, int button)
    {
        Mod.logger.LogInfo($"uGUI_CraftingMenu OnPointerClick called {button} ");
        if (__instance.interactable)
        {
            var node = __instance.GetNode(icon);
            switch (button)
            {
                case 0: // uGUI.button0 => UISubmit
                    __instance.Action(node);
                    __result = true;
                    break;
                case 1: // uGUI.button1 => UICancel
                    __instance.Deselect();
                    __result = true;
                    break;
                case 2: // uGUI.button2 => UIClear => Pinning
                    if (node.action == TreeAction.Craft)
                    {
                        var techType = node.techType;
                        if (CrafterLogic.IsCraftRecipeUnlocked(techType))
                        {
                            PinManager.TogglePin(techType);
                        }
                    }
                    __result = true;
                    break;
                default:
                    __result = false;
                    break;
            }
        }
        return false;
    }
}