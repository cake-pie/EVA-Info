using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EvaInfo
{
    public sealed class EvaInfoPane : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        ProtoCrewMember pcm;

        internal EvaInfoPane(ProtoCrewMember pcm, RectTransform panelRoot)
        {
            this.pcm = pcm;

            EvaInfoSettings settings = EvaInfoAddon.Instance.settings;

            GameObject iconPaneGO = new GameObject("iconPane");
            RectTransform iconPaneRT = iconPaneGO.AddComponent<RectTransform>();
            Image iconPaneBG = iconPaneGO.AddComponent<Image>();
            iconPaneBG.sprite = HighLogic.UISkin.window.active.background;
            iconPaneRT.SetParent(panelRoot, false);
            iconPaneRT.anchorMin = Vector2.zero;
            iconPaneRT.anchorMax = Vector2.zero;
            iconPaneRT.pivot = new Vector2(0.5f, 0);
            iconPaneRT.sizeDelta = Vector2.one * settings.iconPaneSize;
            iconPaneRT.anchoredPosition = new Vector2(0, settings.BottomRankOffset);

            GameObject iconGO = CTIWrapper.CTI.getTrait(pcm.trait).makeGameObject();
            RectTransform iconRT = iconGO.GetComponent<RectTransform>();
            iconRT.SetParent(iconPaneRT, false);
            iconRT.anchorMin = Vector2.one * 0.5f;
            iconRT.anchorMax = Vector2.one * 0.5f;
            iconRT.pivot = Vector2.one * 0.5f;
            iconRT.sizeDelta = Vector2.one * settings.iconSize;
            iconRT.anchoredPosition = Vector2.zero;

            // TODO only show if using experience / settings.showSuperfluousRank
            GameObject rankGO = new GameObject("rankPane");
            RectTransform rankRT = rankGO.AddComponent<RectTransform>();
            Image rankIMG = rankGO.AddComponent<Image>();
            rankIMG.sprite = settings.RankSprite(pcm.experienceLevel);
            rankRT.SetParent(iconPaneRT, false);
            rankRT.anchorMin = Vector2.one * 0.5f;
            rankRT.anchorMax = Vector2.one * 0.5f;
            // TODO use settings
            //settings.rankPosition;
            //settings.rankPlacement;
            rankRT.pivot = new Vector2(0.5f, 0);
            rankRT.sizeDelta = new Vector2(settings.iconSize, settings.RankAspectRatio * settings.iconSize);
            rankRT.anchoredPosition = new Vector2(0, settings.iconPaneSize/2);
        }

        internal void updateName(){
        }

        internal void updateLevel(){
        }

        public void OnPointerEnter(PointerEventData pointerEventData)
        {
        }

        public void OnPointerExit(PointerEventData pointerEventData)
        {
        }

        public void OnPointerClick(PointerEventData pointerEventData)
        {
        }
    }
}
