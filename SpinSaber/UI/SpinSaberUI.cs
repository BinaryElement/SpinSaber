using CustomUI.GameplaySettings;
using CustomUI.MenuButton;
using CustomUI.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SpinSaber.UI {

    class SpinSaberUI {

        public static bool blindingOptionsEnabled = false;
        public static float blindTime = -1f;
        public static float visibleTime = -1f;

        public static float lastOptionSelected = -1f;
        public static float timeMult = 1f;

        public static void InitializeGameplayMenu() {

            blindingOptionsEnabled = false;//ModPrefs.GetBool("SpinSaber", "BlindMe", false);

            Console.WriteLine("[SpinSaber] Initializing GameplayMenu Options...");

            GameplaySettingsUI.CreateSubmenuOption(GameplaySettingsPanels.PlayerSettingsLeft, "SpinSaber", "MainMenu", "SSM", "Select SpinSaber settings");

            /*ToggleOption spinSaberEnabledToggle = GameplaySettingsUI.CreateToggleOption(GameplaySettingsPanels.PlayerSettingsLeft, "Spin Enabled", "SSM", "Enable/Disable SpinSaber.");
            spinSaberEnabledToggle.GetValue = Plugin.spinSaberEnabled;
            spinSaberEnabledToggle.OnToggle += SpinSaberEnabledToggled;*/
            
            GameplaySettingsUI.CreateSubmenuOption(GameplaySettingsPanels.PlayerSettingsLeft, "Profile", "SSM", "SSMPROFILE", "Select SpinSaber profile");

            MultiSelectOption spinSaberConfigSelect = GameplaySettingsUI.CreateListOption(GameplaySettingsPanels.PlayerSettingsLeft, "SpinConfig", "SSMPROFILE", "Select your preferred spin config.");
            spinSaberConfigSelect.AddOption(-1, "Disabled");
            if (Plugin.loadedConfigs.Count <= 0) {
                //spinSaberConfigSelect.AddOption(0, "No Configs Found");
            } else {
                for (int i = 0; i < Plugin.loadedConfigs.Count; i++) {
                    spinSaberConfigSelect.AddOption(i, Plugin.loadedConfigs[i].name);
                }
            }
            spinSaberConfigSelect.OnChange += SpinSaberConfigSelected;
            spinSaberConfigSelect.GetValue = () => lastOptionSelected;

            GameplaySettingsUI.CreateSubmenuOption(GameplaySettingsPanels.PlayerSettingsLeft, "Time", "SSM", "SSMTIME", "Select Time Scale");

            MultiSelectOption spinSaberTimeMult = GameplaySettingsUI.CreateListOption(GameplaySettingsPanels.PlayerSettingsLeft, "Time Rate", "SSMTIME", "Multiply the speed of your selected config.");
            for (int i = 1; i <= 9; i++) {
                spinSaberTimeMult.AddOption(i/10f, "0."+i+"x");
            }
            spinSaberTimeMult.AddOption(1, "Default");
            for (int i = 1; i <= 9; i++) {
                spinSaberTimeMult.AddOption(1 + (i / 10f), "1." + i + "x");
            }
            spinSaberTimeMult.OnChange += SpinSaberTimeSelected;
            spinSaberTimeMult.GetValue = () => timeMult;

            if (blindingOptionsEnabled) {

                MultiSelectOption blindTimeMult = GameplaySettingsUI.CreateListOption(GameplaySettingsPanels.PlayerSettingsLeft, "Blind Time", "SSM", "Duration of being blinded.");
                blindTimeMult.AddOption(-1f, "Disabled");
                for (int i = 0; i <= 9; i++) {
                    for (int j = 0; j <= 9; j++) {
                        if (i == 0 && j == 0) continue;
                        blindTimeMult.AddOption(i + (j / 10f), i+"." + j + "s");
                    }
                }
                blindTimeMult.AddOption(10, "10.0s");
                blindTimeMult.OnChange += BlindTimeSelected;
                blindTimeMult.GetValue = () => blindTime;

                MultiSelectOption visibleTimeMult = GameplaySettingsUI.CreateListOption(GameplaySettingsPanels.PlayerSettingsLeft, "Visible Time", "SSM", "Duration of visibility.");
                visibleTimeMult.AddOption(-1f, "Disabled");
                for (int i = 0; i <= 9; i++) {
                    for (int j = 0; j <= 9; j++) {
                        if (i == 0 && j == 0) continue;
                        visibleTimeMult.AddOption(i + (j / 10f), i + "." + j + "s");
                    }
                }
                visibleTimeMult.AddOption(10, "10.0s");
                visibleTimeMult.OnChange += VisibleTimeSelected;
                visibleTimeMult.GetValue = () => visibleTime;

            }

        }

        private static void SpinSaberConfigSelected(float f) {
            if (f == -1) {
                Plugin.loadedConfigIndex = -1;
            } else {
                Plugin.loadedConfigIndex = Mathf.RoundToInt(f);
            }
            lastOptionSelected = f;
        }

        private static void SpinSaberTimeSelected(float f) {
            timeMult = f;
        }

        private static void BlindTimeSelected(float f) {
            Console.WriteLine("[SpinSaber] Blind Time : "+f);
            blindTime = f;
        }

        private static void VisibleTimeSelected(float f) {
            Console.WriteLine("[SpinSaber] Visible Time : " + f);
            visibleTime = f;
        }

    }

}
