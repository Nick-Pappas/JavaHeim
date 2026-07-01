/**
 * v1
 */
using HarmonyLib;
using System.Collections;
using System.Reflection;
using TMPro;

namespace JavaHeim
{
    /**
     * Enterprise Service for the modification of non-player entity nameplates.
     * Uses reflection to navigate the private internal structures of the EnemyHud.
     * Does not work on vampiric stuff thus.
     */
    [HarmonyPatch(typeof(EnemyHud))]
    public static class EnemyHudPatches
    {
        /**
         * Cached reflection info for the private m_name field in the HudData class.
         */
        private static FieldInfo? _nameFieldInfo;

        /**
         * Postfix hook to catch creature names (Greylings, Bosses, Thingamajigies, etc.) as they are updated.
         * 
         * @param ___m_huds The private dictionary containing active enemy HUD data.
         */
        [HarmonyPatch("UpdateHuds")]
        [HarmonyPostfix]
        public static void UpdateHuds_Postfix(IDictionary ___m_huds)
        {
            if (___m_huds == null)
            {
                return;
            }

            foreach (object hudDataP in ___m_huds.Values)
            {
                if (hudDataP == null)
                {
                    continue;
                }

                /**
                 * Initialize the field reflector if it hasn't been cached yet xD
                 */
                if (_nameFieldInfo == null)
                {
                    _nameFieldInfo = hudDataP.GetType().GetField("m_name", BindingFlags.Public | BindingFlags.Instance);
                }

                /**
                 * The Legend
                 */
                TextMeshProUGUI? nameLabel = _nameFieldInfo?.GetValue(hudDataP) as TextMeshProUGUI;

                if (nameLabel != null && !string.IsNullOrEmpty(nameLabel.text))
                {
                    nameLabel.text = JavaHeimStringManipulationService.GetCursedJavaStringFromText(nameLabel.text);
                }
            }
        }
    }
}