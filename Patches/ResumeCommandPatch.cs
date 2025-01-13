using HarmonyLib;
using StardewValley;
using StardewValley.Menus;
using UniversalPauseCommand.Messages;

namespace UniversalPauseCommand.Patches
{
    public static class ResumeCommandPatch
    {
        public static void ApplyPatch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(ChatCommands.DefaultHandlers), "Resume"),
                prefix: new HarmonyMethod(typeof(ResumeCommandPatch), nameof(Prefix))
            );
        }

        public static bool Prefix(string[] command, ChatBox chat)
        {
            var i18n = ModEntry.StaticHelper.Translation;

            if (Game1.netWorldState.Value.IsPaused)
            {
                Game1.netWorldState.Value.IsPaused = false;
                var playerName = Game1.player.Name;
                var message = i18n.Get("resumed_message", new { playerName });
                MessageManager.SendGlobalInfoMessage(message);
                return false;
            }

            return false;
        }
    }
}