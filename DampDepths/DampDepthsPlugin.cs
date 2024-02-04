using BepInEx;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

namespace DampDepths
{
    [BepInPlugin(guid, modName, version)]
    public class DampDepthsPlugin : BaseUnityPlugin
    {
        public const string guid = "com." + teamName + "." + modName;
        public const string teamName = "RiskOfBrainrot";
        public const string modName = "DampDepths";
        public const string version = "1.0.0";

        public static PluginInfo PInfo { get; private set; }

        public void Awake()
        {
            PInfo = Info;

            SceneManager.activeSceneChanged += OnSceneChanged;
        }

        private void OnSceneChanged(Scene oldScene, Scene newScene)
        {
            if(newScene.name.ToLower() == "dampcavesimple")
            {
                GameObject[] rootObjects = newScene.GetRootGameObjects();
                GameObject skyboxHolder = Array.Find(rootObjects, x => x.name == "HOLDER: Lighting, PP, Wind, Misc");
                if(skyboxHolder)
                {
                    Transform tunnelDCPP = skyboxHolder.transform.Find("DCPPInTunnels");
                    if(tunnelDCPP != null)
                    {
                        if (tunnelDCPP.TryGetComponent(out PostProcessVolume ppvTunnel))
                        {
                            ppvTunnel.sharedProfile = Addressables.LoadAssetAsync<PostProcessProfile>("RoR2/Base/title/ppSceneDampcave.asset").WaitForCompletion();
                            if (ppvTunnel.profile.TryGetSettings(out RampFog rampFog))
                            {
                                rampFog.fogPower.Override(0.49f);
                                rampFog.fogOne.Override(0.154f);
                            }
                        }
                    }
                }

                if(SceneInfo.instance.TryGetComponent(out PostProcessVolume ppv))
                {
                    ppv.sharedProfile = Addressables.LoadAssetAsync<PostProcessProfile>("RoR2/Base/title/ppSceneDampcave.asset").WaitForCompletion();
                    if (ppv.profile.TryGetSettings(out RampFog rampFog))
                    {
                        rampFog.fogPower.Override(1f);
                        rampFog.fogOne.Override(0.3f);
                    }
                }
            }
        }
    }
}
