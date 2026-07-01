/**
 * v1
 */
using HarmonyLib;

namespace JavaHeim
{
    /**
     * Intercepts individual ItemDrop objects to ensure ground item names 
     * are properly formatted for internal engine consumption yum yum.
     */
    [HarmonyPatch(typeof(ItemDrop))]
    public static class ItemDropPatches
    {
        /**
         * Overrides the basic hover name return value with something appropriate for the occasion.
         * 
         * @param __instance The active ItemDrop instance.
         * @param __result The referenced string result to be modified.
         * @return Always returns false to skip the vanilla name generation because it is stupid.
         */
        [HarmonyPatch("GetHoverName")]
        [HarmonyPrefix]
        public static bool GetHoverName_Prefix(ItemDrop __instance, ref string __result)
        {
            /**
             * The Legend
             */
            ItemDrop.ItemData itemData = __instance.m_itemData;
            __result = JavaHeimStringManipulationService.GetCursedJavaString(itemData);

            return false;
        }

        /**
         * Ensures ground item text is replaced correctly in complex localized hover strings. 
         * 
         * @param __instance The active ItemDrop instance.
         * @param __result The referenced string result to be modified.
         */
        [HarmonyPatch("GetHoverText")]
        [HarmonyPostfix]
        public static void GetHoverText_Postfix(ItemDrop __instance, ref string __result)
        {
            /**
             * The Legend
             */
            ItemDrop.ItemData itemData = __instance.m_itemData;
            string rawName = itemData.m_shared.m_name;
            string localizedName = Localization.instance.Localize(rawName);
            string cursedString = JavaHeimStringManipulationService.GetCursedJavaString(itemData);

            __result = __result.Replace(localizedName, cursedString);
        }
    }
}