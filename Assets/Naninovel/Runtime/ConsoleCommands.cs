﻿// Copyright 2017-2019 Elringus (Artyom Sovetnikov). All Rights Reserved.

using UnityCommon;
using UnityConsole;
using UnityEngine;

namespace Naninovel
{
    public static class ConsoleCommands
    {
        [ConsoleCommand("nav")]
        public static void ToggleScriptNavigator () => Engine.GetService<ScriptManager>()?.ToggleNavigator();

        [ConsoleCommand("debug")]
        public static void ToggleDebugInfo () => UI.DebugInfoGUI.Toggle();

        #if UNITY_GOOGLE_DRIVE_AVAILABLE
        [ConsoleCommand("purge")]
        public static void PurgeCache ()
        {
            var manager = Engine.GetService<ResourceProviderManager>();
            if (manager is null) { Debug.LogError("Failed to retrieve provider manager."); return; }
            var googleDriveProvider = manager.GetProvider(ResourceProviderType.GoogleDrive) as UnityCommon.GoogleDriveResourceProvider;
            if (googleDriveProvider is null) { Debug.LogError("Failed to retrieve google drive provider."); return; }
            googleDriveProvider.PurgeCache();
        }
        #endif

        [ConsoleCommand]
        public static void Play () => Engine.GetService<ScriptPlayer>()?.Play();

        [ConsoleCommand]
        public static void PlayScript (string name) => Engine.GetService<ScriptPlayer>()?.PreloadAndPlayAsync(name);

        [ConsoleCommand]
        public static void Stop () => Engine.GetService<ScriptPlayer>()?.Stop();

        [ConsoleCommand]
        public static async void Rewind (int line)
        {
            line = Mathf.Clamp(line, 1, int.MaxValue);
            await Engine.GetService<ScriptPlayer>()?.RewindAsync(line - 1);
            Engine.GetService<ScriptPlayer>()?.Play();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void SetupDevelopmentConsole ()
        {
            var config = Configuration.LoadOrDefault<EngineConfiguration>();

            ConsoleGUI.AutoInitialize = config.EnableDevelopmentConsole;
            ConsoleGUI.ToggleKey = config.ToggleConsoleKey;

            // Process input starting with `@` as naninovel commands.
            InputPreprocessor.AddPreprocessor(ProcessActionInput);
        }

        private static string ProcessActionInput (string input)
        {
            if (input is null || !input.StartsWithFast(CommandScriptLine.IdentifierLiteral)) return input;

            var scriptLine = new CommandScriptLine(string.Empty, 0, input, null, false);
            if (scriptLine is null) return null;

            var command = Commands.Command.FromScriptLine(scriptLine);
            if (command is null) return null;

            if (command.ShouldExecute)
                command.ExecuteAsync();
            return null;
        }
    }
}
