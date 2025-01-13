using HarmonyLib;
using StardewModdingAPI;
using UniversalPauseCommand.Messages;
using UniversalPauseCommand.Patches;

namespace UniversalPauseCommand
{
    internal sealed class ModEntry : Mod
    {
        public static IModHelper StaticHelper = null!;

        public override void Entry(IModHelper helper)
        {
            StaticHelper = helper;

            var harmony = new Harmony(ModManifest.UniqueID);
            PauseCommandPatch.ApplyPatch(harmony);
            ResumeCommandPatch.ApplyPatch(harmony);

            helper.Events.Multiplayer.ModMessageReceived += MessageManager.HandleMessage;
        }
    }
}