using UnityEngine;

namespace EvaInfo
{
    public class EvaInfoSettings
    {
        // sizing
        internal int iconPaneSize = 34;
        internal int iconSize = 28;

        // offset of pane base from "top" of helmet as proportion of iconPaneSize
        internal float offsetFactor = 0.35f;
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
        private string rankGraphic = "EvaInfo/icons/rank/horizontal/starsH02";
        internal string RankGraphic {
            get { return rankGraphic; }
            set {
                Texture2D tex = GameDatabase.Instance.GetTexture(value, false);
                if (tex == null) return;
                rankGraphic = value;
                rankTexture = tex;
            }
        }
        private Texture2D rankTexture;
        internal Texture2D RankTexture {
            get {
                if (rankTexture == null)
                    rankTexture = GameDatabase.Instance.GetTexture(rankGraphic, false);
                return rankTexture;
            }
        }
        internal float RankAspectRatio {
            get {
                if (RankTexture == null) return 0;
                switch (rankPosition)
                {
                    case RankPosition.TOP:
                    case RankPosition.BOTTOM:
                        return (RankTexture.height/6f) / RankTexture.width;
                    case RankPosition.LEFT:
                    case RankPosition.RIGHT:
                        return (RankTexture.width/6f) / RankTexture.height;
                    default:
                        return 0;
                }
            }
        }
        internal Sprite RankSprite(int level)
        {
            if (RankTexture == null) return null;
            float spriteW = 0;
            float spriteH = 0;
            switch (rankPosition)
            {
                case RankPosition.TOP:
                case RankPosition.BOTTOM:
                    spriteW = RankTexture.width;
                    spriteH = RankTexture.height / 6f;
                    return Sprite.Create(RankTexture,
                        new Rect(0, level * spriteH, spriteW, spriteH),
                        Vector2.one * 0.5f, 100f, 0,
                        SpriteMeshType.FullRect);
                case RankPosition.LEFT:
                case RankPosition.RIGHT:
                    spriteW = RankTexture.width / 6f;
                    spriteH = RankTexture.height;
                    return Sprite.Create(RankTexture,
                        new Rect(level * spriteW, 0, spriteW, spriteH),
                        Vector2.one * 0.5f, 100f, 0,
                        meshType: SpriteMeshType.FullRect);
                default:
                return null;
            }
        }

        // offset required to compensate for rank graphic placed bottom outside
        internal float BottomRankOffset {
            get { return (rankPosition == RankPosition.BOTTOM && rankPlacement == RankPlacement.OUTSIDE) ? (iconSize * RankAspectRatio) : 0; }
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

