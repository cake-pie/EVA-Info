using UnityEngine;
using KSP.UI.Screens;

namespace EvaInfo
{
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public sealed class EvaInfoAddon : MonoBehaviour
    {
        internal static EvaInfoAddon Instance;
        internal static bool foundCTI = true;

        internal static EventVoid onToggleInfo = new EventVoid("onToggleInfo");
        private static bool _showInfo;
        internal static bool ShowInfo {
            get { return _showInfo; }
            private set {
                if (value == _showInfo) return;
                _showInfo = value;
                onToggleInfo.Fire();
            }
        }

        private Texture toolbarBtnOn, toolbarBtnOff;
        private ApplicationLauncherButton stockButton;

        private void Start()
        {
            if (Instance != null || !foundCTI)
            {
                // Reloading of GameDatabase causes another copy of addon to spawn at next opportunity. Suppress it.
                // see: https://forum.kerbalspaceprogram.com/index.php?/topic/7542-x/&do=findComment&comment=3574980
                Destroy(gameObject);
                return;
            }

            foundCTI = CTIWrapper.initCTIWrapper() && CTIWrapper.CTI.Loaded;
            if (!foundCTI)
            {
                Log("ERROR: Community Trait Icons was not found!");
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            toolbarBtnOn = GameDatabase.Instance.GetTexture("EvaInfo/icons/button/icon-48-on", false);
            toolbarBtnOff = GameDatabase.Instance.GetTexture("EvaInfo/icons/button/icon-48-off", false);
            GameEvents.onGUIApplicationLauncherReady.Add(OnAppLauncherReady);

            GameEvents.onGameSceneSwitchRequested.Add(OnGameSceneSwitchRequested);
        }

        private void OnDestroy()
        {
            GameEvents.onGUIApplicationLauncherReady.Remove(OnAppLauncherReady);
            GameEvents.onGUIApplicationLauncherUnreadifying.Remove(OnAppLauncherUnreadifying);
            GameEvents.onGameSceneSwitchRequested.Remove(OnGameSceneSwitchRequested);
            Instance = null;
        }

        private void OnAppLauncherReady()
        {
            GameEvents.onGUIApplicationLauncherReady.Remove(OnAppLauncherReady);
            GameEvents.onGUIApplicationLauncherUnreadifying.Add(OnAppLauncherUnreadifying);

            switch (HighLogic.LoadedScene)
            {
            case GameScenes.FLIGHT:
                stockButton = ApplicationLauncher.Instance.AddModApplication(
                    (delegate {
                        ShowInfo = true;
                        stockButton.SetTexture(toolbarBtnOn);
                    }),
                    (delegate {
                        ShowInfo = false;
                        stockButton.SetTexture(toolbarBtnOff);
                    }),
                    null, null, // TODO tooltip?
                    null, null,
                    ApplicationLauncher.AppScenes.FLIGHT,
                    toolbarBtnOff
                );
                break;
            }
        }

        private void OnAppLauncherUnreadifying(GameScenes scene)
        {
            if (stockButton != null)
            {
                ApplicationLauncher.Instance.RemoveModApplication(stockButton);
                Destroy(stockButton.gameObject);
                stockButton = null;
            }
            GameEvents.onGUIApplicationLauncherUnreadifying.Remove(OnAppLauncherUnreadifying);
            GameEvents.onGUIApplicationLauncherReady.Add(OnAppLauncherReady);
        }

        private void OnGameSceneSwitchRequested(GameEvents.FromToAction<GameScenes,GameScenes> data)
        {
            switch (data.from)
            {
                case GameScenes.FLIGHT:
                    ShowInfo = false;
                    break;
            }
        }

        internal static void Log(string s)
        {
            Debug.Log("[EvaInfo] " + s);
        }
    }
}
