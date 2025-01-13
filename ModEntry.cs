using HarmonyLib;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using UniversalPauseCommand.Config;
using UniversalPauseCommand.Messages;
using UniversalPauseCommand.Patches;

namespace UniversalPauseCommand
{
    internal sealed class ModEntry : Mod
    {
        public static IModHelper StaticHelper = null!;
        public static ModConfig Config = null!;

        public override void Entry(IModHelper helper)
        {
            StaticHelper = helper;

            var harmony = new Harmony(ModManifest.UniqueID);
            PauseCommandPatch.ApplyPatch(harmony);
            ResumeCommandPatch.ApplyPatch(harmony);

            Config = helper.ReadConfig<ModConfig>();
            helper.Events.Multiplayer.ModMessageReceived += MessageManager.HandleMessage;
            helper.Events.Input.ButtonPressed += OnButtonPressed;
            helper.Events.GameLoop.GameLaunched += OnGameLaunched;
        }

        private void OnGameLaunched(object? sender, GameLaunchedEventArgs e)
        {
            if (StaticHelper.ModRegistry.IsLoaded("spacechase0.GenericModConfigMenu"))
                SetupGenericModConfigMenu();
        }

        private void SetupGenericModConfigMenu()
        {
            var gmcmApi = StaticHelper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            if (gmcmApi == null)
                return;

            gmcmApi.Register(
                mod: ModManifest,
                reset: () => Config = new ModConfig(),
                save: () => StaticHelper.WriteConfig(Config),
                titleScreenOnly: false
            );

            var i18n = StaticHelper.Translation;

            gmcmApi.AddKeybind(
                mod: ModManifest,
                name: () => i18n.Get("config_option_name"),
                tooltip: () => i18n.Get("config_option_tooltip"),
                getValue: () => Config.PauseKey,
                setValue: value => Config.PauseKey = value
            );
        }

        private void OnButtonPressed(object? sender, ButtonPressedEventArgs e)
        {
            if (e.Button == Config.PauseKey && Context.IsWorldReady)
            {
                PauseCommandPatch.Pause();
            }
        }
    }
}