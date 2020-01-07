using IPA;
using SpinSaber.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpinSaber {

    public class Plugin : IBeatSaberPlugin {

        public string Name => "SpinSaber";
        public string Version => "2.0.1";

        public static bool SpinSaberEnabled {
            get { return loadedConfigIndex == -1; }
        }

        public static int loadedConfigIndex = 0;
        public static List<SpinConfig> loadedConfigs = new List<SpinConfig>();

        public static SpinConfig LoadedConfig {
            get {
                if (loadedConfigIndex == -1 || loadedConfigs.Count <= loadedConfigIndex) return null;
                else return loadedConfigs[loadedConfigIndex];
            }
        }

        public void OnActiveSceneChanged(Scene prevScene, Scene nextScene) {
            if (prevScene.name != "GameCore" && nextScene.name == "GameCore") {
                GameObject spinnerObj = new GameObject("SpinSaberBehaviour");
                spinnerObj.AddComponent<SpinSaberBehaviour>();
            }
        }

        public void OnApplicationQuit() {
            //SceneManager.activeSceneChanged -= SceneManagerOnActiveSceneChanged;
            //SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
        }

        public void OnApplicationStart() {
            Console.WriteLine("[SpinSaber] Initialized!");
            //SerializeExampleConfigs();
            ReloadSettings();

            //SceneManager.activeSceneChanged += SceneManagerOnActiveSceneChanged;
            //SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        }

        public void OnFixedUpdate() {
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode) {
            if (scene.name == "MenuCore") {
                try {
                    SpinSaberUI.InitializeGameplayMenu();

                } catch (Exception) {
                }
            }
        }

        public void OnSceneUnloaded(Scene scene) {
        }

        public void OnUpdate() {
            if (Input.GetKeyDown(KeyCode.Alpha9)) {
                ReloadSettings();
            }
        }

        public void ReloadSettings() {
            loadedConfigs = new List<SpinConfig>();

            LoadConfigsFromXML(ref loadedConfigs);

            Console.WriteLine("[SpinSaber] Loaded Settings!");
        }

        public void LoadConfigsFromXML(ref List<SpinConfig> configs) {

            string folder = Environment.CurrentDirectory.Replace('\\', '/') + "/UserData/SpinSaber/";
            Directory.CreateDirectory(folder);
            IEnumerable<string> fileEnum = Directory.EnumerateFiles(folder, "*.xml");
            foreach (string file in Directory.EnumerateFiles(folder, "*.xml")) {
                SpinConfig config;
                XmlSerializer ser = new XmlSerializer(typeof(SpinConfig));
                using (FileStream fileStream = new FileStream(file, FileMode.Open)) {
                    config = (SpinConfig)ser.Deserialize(fileStream);
                }
                if (config == null) {
                    Console.WriteLine("[SpinSaber] Invalid config at " + file);
                    continue;
                }
                config.Validate();
                bool duplicate = false;
                for (int i = 0; i < loadedConfigs.Count; i++) {
                    if (loadedConfigs[i].name == config.name) { duplicate = true; break; }
                }
                if (!duplicate) configs.Add(config);
            }

            if (configs.Count <= 0) {
                foreach (SpinConfig config in SerializeExampleConfigs()) {
                    configs.Add(config);
                }
            }
        }

        public SpinConfig[] SerializeExampleConfigs() {

            SpinConfig[] examples = new SpinConfig[3];

            examples[0] = new SpinConfig();
            examples[0] = new SpinConfig();
            examples[0].name = "SpinExample";
            examples[0].bpmFactor = 0;
            examples[0].periods = new SpinConfig.SpinConfigPeriod[] {
                new SpinConfig.SpinConfigPeriod(24, Swingers.Swinger.Type.NONE, 0, new Vector3(0, 0, 0), new Vector3(0, 360, 0)) //15° per second
            };

            examples[1] = new SpinConfig();
            examples[1].name = "SwingExample";
            examples[1].bpmFactor = 0;
            examples[1].periods = new SpinConfig.SpinConfigPeriod[] {
                new SpinConfig.SpinConfigPeriod(5, Swingers.Swinger.Type.EASE_IN_OUT_QUADRATIC, 0, new Vector3(0, -45, 0), new Vector3(0, 45, 0)),
                new SpinConfig.SpinConfigPeriod(5, Swingers.Swinger.Type.EASE_IN_OUT_QUADRATIC, 0, new Vector3(0, 45, 0), new Vector3(0, -45, 0))
            };
            loadedConfigs.Add(examples[1]);

            examples[2] = new SpinConfig();
            examples[2].name = "SwingWaitEx";
            examples[2].bpmFactor = 0;
            examples[2].periods = new SpinConfig.SpinConfigPeriod[] {
                new SpinConfig.SpinConfigPeriod(5, Swingers.Swinger.Type.EASE_IN_OUT_QUADRATIC, 0, new Vector3(0, -45, 0), new Vector3(0, 45, 0)),
                new SpinConfig.SpinConfigPeriod(2, Swingers.Swinger.Type.NONE, 0, new Vector3(0, 45, 0), new Vector3(0, 45, 0)),
                new SpinConfig.SpinConfigPeriod(5, Swingers.Swinger.Type.EASE_IN_OUT_QUADRATIC, 0, new Vector3(0, 45, 0), new Vector3(0, -45, 0)),
                new SpinConfig.SpinConfigPeriod(2, Swingers.Swinger.Type.NONE, 0, new Vector3(0, -45, 0), new Vector3(0, -45, 0)),
            };
            loadedConfigs.Add(examples[2]);

            foreach (SpinConfig config in examples) {
                string filePath = Environment.CurrentDirectory.Replace('\\', '/') + "/UserData/SpinSaber/" + config.name + ".xml";

                if (File.Exists(filePath)) continue; //File.Delete(filePath);
                using (StreamWriter w = File.AppendText(filePath)) {
                    XmlSerializer ser = new XmlSerializer(config.GetType());
                    ser.Serialize(w, config);
                }
            }

            return examples;
        }

    }

}

