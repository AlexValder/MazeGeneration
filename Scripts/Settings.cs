using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Godot;
using Serilog;
using File = System.IO.File;

namespace Demonomania.Scripts {
    internal static class Settings {
        private const string CONFIG_FILE = "config.ini";
        private static readonly ConfigFile s_config = new ConfigFile();

#region Setting Properties
    #region Graphics
        public static bool WindowMaximized {
            get => GetValue<bool>(Section.Graphics, "window_maximized");
            set => SetValue(Section.Graphics, "window_maximized", value);
        }

    #endregion
#endregion

        static Settings() {
            if (!File.Exists(CONFIG_FILE)) {
                using (_ = File.CreateText(CONFIG_FILE)) {}

                CreateDefault();
                return;
            }

            var result = s_config.Load(CONFIG_FILE);
            if (result != Error.Ok) {
                Log.Logger.Error("Failed to load config file. Error: {Error}", result);
            }
        }

        private static void CreateDefault() {
            foreach (var section in Defaults) {
                foreach (var key in section.Value) {
                    s_config.SetValue(section.Key.AsString(), key.Key, key.Value);
                }
            }
            Save();
        }

        private static void Save() {
            var result = s_config.Save(CONFIG_FILE);
            if (result != Error.Ok) {
                Log.Logger.Error("Failed to save config file");
            }
        }

        private static T GetValue<T>(Section section, string field) {
            var value = s_config.GetValue(section.AsString(), field);
            return value is T typedValue ? typedValue : default;
        }
        private static void SetValue<T>(Section section, string field, T value) {
            s_config.SetValue(section.AsString(), field, value);
        }

        private static readonly ImmutableDictionary<Section, ImmutableDictionary<string, object>> Defaults =
            new Dictionary<Section, ImmutableDictionary<string, object>> {
                [Section.Graphics] = new Dictionary<string, object> {
                    ["window_maximized"] = false,
                }.ToImmutableDictionary(),
            }.ToImmutableDictionary();

        private enum Section {
            Graphics,
        }

        private static string AsString(this Section value) {
            switch (value) {
                case Section.Graphics: return "graphics";
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, null);
            }
        }
    }
}
