namespace EvaInfo
{
    [KSPScenario(ScenarioCreationOptions.AddToAllGames, GameScenes.SPACECENTER, GameScenes.FLIGHT)]
    public class EvaInfoScenario : ScenarioModule
    {
        public override void OnLoad(ConfigNode node)
        {
            //if new game (empty node)
            EvaInfoAddon.Instance.settings = EvaInfoSettings.FetchPresetOrDefault();
            //else
            //    EvaInfoAddon.Instance.Settings = EvaInfoAddon.Instance.Settings ?? new EvaInfoSettings();
            //    EvaInfoAddon.Instance.Settings.Load(node);
        }

        public override void OnSave(ConfigNode node)
        {
            EvaInfoAddon.Instance.settings.Save(node);
        }
    }
}
