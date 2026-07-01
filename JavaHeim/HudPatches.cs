/**
 * v1
 */
using HarmonyLib;
using TMPro;

namespace JavaHeim
{
    /**
     * Intercepts the Head-Up Display (HUD) update cycle. VERY performant.
     * Redirects crosshair hover text to the Java manipulation service which is also uber performant.
     */
    [HarmonyPatch(typeof(Hud))]
    public static class HudPatches
    {
        /**
         * Postfix hook on UpdateCrosshair to transform the central hover label.
         * 
         * @param __instance The active Hud instance being patched.
         */
        [HarmonyPatch("UpdateCrosshair")]
        [HarmonyPostfix]
        public static void UpdateCrosshair_Postfix(Hud __instance)
        {
            /**
             * The Legend
             */
            TextMeshProUGUI label = __instance.m_hoverName;
            string currentText = label.text;

            if (string.IsNullOrEmpty(currentText))
            {
                return;
            }

            label.text = JavaHeimStringManipulationService.GetCursedJavaStringFromText(currentText);
        }
    }
}