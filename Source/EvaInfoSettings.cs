namespace EvaInfo
{
    public class EvaInfoSettings
    {
        // sizing
        internal int iconPaneSize = 34;
        internal int iconSize = 28;

        // offset of pane base from "top" of helmet as proportion of iconPaneSize
        internal float offsetFactor = 0.5f;
        internal float OffsetHeight {
            get { return offsetFactor * iconPaneSize; }
        }

        // near/far cutoff distance for showing pane
        internal float nearClipDistance = 1.0f;
        internal float farClipDistance = 100f;

        // shrink with increasing distance
        // - shrink start distance
        // - % of full size when at far cutoff
        internal bool scaleWithDistance = false;
        internal float scaleStartDistance = 60f;
        internal float scaleAtFarClip = 0.6f;

        #region rank
        // rank position
        internal enum RankPosition
        {
            TOP = 1,
            LEFT = 2,
            RIGHT = 3,
            BOTTOM = 4
        }
        internal RankPosition rankPosition = RankPosition.TOP;

        // rank placement
        internal enum RankPlacement
        {
            INSIDE = 1, // overlaid on top of trait icon
            OUTSIDE = 2
        }
        internal RankPlacement rankPlacement = RankPlacement.OUTSIDE;

        // rank graphic - star, chevron, etc
        // TODO

        // offset required to compensate for rank graphic placed bottom outside
        internal int BottomRankOffset {
            get { return (rankPosition == RankPosition.BOTTOM && rankPlacement == RankPlacement.OUTSIDE) ? 10 : 0; }
        }

        // force show rank when ExperienceSystem off
        internal bool showSuperfluousRank = false;
        #endregion

        // prefer blizzy toolbar

        internal EvaInfoSettings CopyOf()
        {
            return (EvaInfoSettings) this.MemberwiseClone();
        }

        internal void Load(ConfigNode node)
        {
        }

        internal void Save(ConfigNode node)
        {
        }

        internal static EvaInfoSettings FetchPreset()
        {
            EvaInfoSettings result = new EvaInfoSettings();
            //result.Load(presetNode);
            return result;
        }

        internal void SaveAsPreset()
        {
            //Save(presetNode);
        }

        internal static void DeletePreset()
        {
        }

        internal static EvaInfoSettings FetchDefault()
        {
            return new EvaInfoSettings();
        }

        internal static EvaInfoSettings FetchPresetOrDefault()
        {
            //if preset exists
            //    return FetchPreset()
            //else
            return FetchDefault();
        }
    }
}

