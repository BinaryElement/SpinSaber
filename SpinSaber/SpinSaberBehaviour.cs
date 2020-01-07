using SpinSaber.Swingers;
using SpinSaber.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SpinSaber {

    class SpinSaberBehaviour : MonoBehaviour {

        PlayerController playerController;
        SpinConfig config;

        public bool IsPaused {
            get { return playerController.disableSabers; }
        }

        Swinger currentSwinger = null;
        public Swinger CurrentSwinger {
            get { return currentSwinger; }
            set {
                currentSwinger = value;
                currentSwinger.startRot = playerController.transform.root.rotation.eulerAngles;
            }
        }

        void Start() {
            Console.WriteLine("[SpinSaber] SpinSaberBehaviour initialized!");
            playerController = FindObjectOfType<PlayerController>();
            if (playerController == null) {
                Console.WriteLine("[SpinSaber] Failed to find PlayerController on SpinSaberBehaviour load!");
                enabled = false;
                return;
            } else {
                Console.WriteLine("[SpinSaber] Found PlayerController!");

                config = Plugin.LoadedConfig;
                if (config == null) {
                    Console.WriteLine("[SpinSaber] No valid selected SpinConfig!");
                    enabled = false;
                    return;
                }

                StartCoroutine(SpinnyBoi());
            }
        }

        IEnumerator BlindBoi() {
            Camera mainCam = Camera.main;
            float origFarPlane = mainCam.farClipPlane;
            float newFarPlane = mainCam.nearClipPlane + 0.01f;
            float blindTime = SpinSaberUI.blindTime;
            float visibleTime = SpinSaberUI.visibleTime;
            Console.WriteLine("[SpinSaber] Far plane prev: " + origFarPlane);
            float timeMult = SpinSaberUI.timeMult;

            bool currentlyBlind = false;
            float t = 0;
            while (true) {
                yield return null;
                t -= Time.deltaTime * timeMult;
                if (t <= 0) {
                    if (currentlyBlind) {
                        t = visibleTime;
                        mainCam.farClipPlane = origFarPlane;
                        currentlyBlind = false;
                    } else {
                        t = blindTime;
                        mainCam.farClipPlane = newFarPlane;
                        currentlyBlind = true;
                    }
                }
            }

            //Camera.main.farClipPlane = newFarPlane;
        }

        IEnumerator SpinnyBoi() {
            if (SpinSaberUI.blindTime > 0f && SpinSaberUI.visibleTime > 0f) {
                StartCoroutine(BlindBoi());
            }
            float t = 0;
            List<Swinger> swingers = config.SetupSwingers();
            if (swingers.Count < 1) {
                Console.WriteLine("[SpinSaber] Zero swingers found!  This party is weak!");
                enabled = false;
                yield break;
            }
            Console.WriteLine("[SpinSaber] Set up the swingers!");
            float timeMult = SpinSaberUI.timeMult;

            while (true) {
                yield return null;
                if (IsPaused) {
                    //playerController.transform.root.rotation = Quaternion.identity;
                } else {
                    t += Time.deltaTime * timeMult;

                    Swinger swinger = null;
                    for (int i = 0; i < swingers.Count; i++) {
                        if (swingers[i].startTime < t && swingers[i].endTime > t) {
                            swinger = swingers[i];
                            break;
                        }
                    }
                    if (swinger == null) {
                        swinger = swingers[0];
                        t = 0;
                    }

                    playerController.transform.root.rotation = swinger.GetRotation((t - swinger.startTime) / (swinger.endTime - swinger.startTime)); //Convert to a 0-1 number
                }
            }

            /*while (true) {
                yield return null;
                if (IsPaused) {
                    rot = Vector3.zero;
                } else {
                    rot = playerController.transform.root.rotation.eulerAngles;
                }
                rot.x += Plugin.xRot * Time.deltaTime;
                rot.y += Plugin.yRot * Time.deltaTime;
                rot.z += Plugin.zRot * Time.deltaTime;
                playerController.transform.root.rotation = Quaternion.Euler(rot);
            }*/
        }

    }

}
