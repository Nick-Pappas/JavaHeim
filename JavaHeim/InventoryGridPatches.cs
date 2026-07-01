/**
 * v1
 */
using HarmonyLib;
using UnityEngine;

namespace JavaHeim
{
    /**
     * Intercepts the Inventory UI grid to provide cursed Java tooltips for Blaxxun.
     */
    [HarmonyPatch(typeof(InventoryGrid))]
    public static class InventoryGridPatches
    {
        /**
         * Prefix hook to override the vanilla tooltip creation logic.
         * 
         * @param __instance The active InventoryGrid instance.
         * @param item The item data being inspected.
         * @param tooltip The UI tooltip component to be modified.
         * @return Always returns false to prevent the vanilla tooltip from being set since the vanilla tooltip is never a Java tooltip :`(.
         */
        [HarmonyPatch("CreateItemTooltip")]
        [HarmonyPrefix]
        public static bool CreateItemTooltip_Prefix(InventoryGrid __instance, ItemDrop.ItemData item, UITooltip tooltip)
        {
            /**
             * The Legend
             */
            string cursedString = JavaHeimStringManipulationService.GetCursedJavaString(item);
            string tooltipText = item.GetTooltip(-1);
            RectTransform anchor = __instance.m_tooltipAnchor;

            tooltip.Set(cursedString, tooltipText, anchor, default(Vector2));

            return false;
        }
    }
}