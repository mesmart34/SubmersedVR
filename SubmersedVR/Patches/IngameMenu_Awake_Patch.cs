using HarmonyLib;
using TMPro;
using UnityEngine.UI;
using static UnityEngine.Object;

namespace SubmersedVR.Patches;

// Makes the ingame menu spawn infront of you in vr
[HarmonyPatch(typeof(IngameMenu), nameof(IngameMenu.Awake))]
internal static class IngameMenu_Awake_Patch
{
    private static Button _recenterVRButton;
    private const string RecenterVRButtonText = "RecenterVR";
    
    internal static void Postfix(IngameMenu __instance)
    {
        var scalar = __instance.GetComponent<uGUI_CanvasScaler>();
        scalar.vrMode = uGUI_CanvasScaler.Mode.Static;
        
        if (!__instance || _recenterVRButton != null)
        {
            return;
        }
        
        //Clone the quitToMainMenuButton and update it
        var menuButton = __instance.quitToMainMenuButton.transform.parent.GetChild(0).gameObject.GetComponent<Button>();
        _recenterVRButton = Instantiate(menuButton, __instance.quitToMainMenuButton.transform.parent);
        _recenterVRButton.transform.SetSiblingIndex(1);//put the button in the second position in the menu
        _recenterVRButton.name = RecenterVRButtonText;
        _recenterVRButton.GetComponentInChildren<TextMeshProUGUI>().SetText(RecenterVRButtonText);
        _recenterVRButton.onClick.RemoveAllListeners();//remove cloned listeners
        //add new listener
        
        _recenterVRButton.onClick.AddListener(VRUtil.Recenter);
    }
}