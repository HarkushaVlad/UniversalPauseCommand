using HarmonyLib;
using StardewValley;
using StardewValley.Menus;
using UniversalPauseCommand.Messages;

namespace UniversalPauseCommand.Patches
{
    public static class PauseCommandPatch
    {
        public static void ApplyPatch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(ChatCommands.DefaultHandlers), "Pause"),
                prefix: new HarmonyMethod(typeof(PauseCommandPatch), nameof(Prefix))
            );
        }

        public static bool Prefix(string[] command, ChatBox chat)
        {
            return Pause();
        }

        public static bool Pause()
        {
            var i18n = ModEntry.StaticHelper.Translation;

            var isPaused = Game1.netWorldState.Value.IsPaused;
            Game1.netWorldState.Value.IsPaused = !isPaused;

            var playerName = Game1.player.Name;

            var message = isPaused
                ? i18n.Get("resumed_message", new { playerName })
                : i18n.Get("paused_message", new { playerName });

            MessageManager.SendGlobalInfoMessage(message);

            return false;
        }
    }
}