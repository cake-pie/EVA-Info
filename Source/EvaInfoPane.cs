using UnityEngine;
using UnityEngine.EventSystems;

namespace EvaInfo
{
    public sealed class EvaInfoPane : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        ProtoCrewMember pcm;

        internal EvaInfoPane(ProtoCrewMember pcm, RectTransform panelRoot)
        {
            this.pcm = pcm;
            //pcm.name;
            //pcm.experienceLevel;
            GameObject traitIcon = CTIWrapper.CTI.getTrait(pcm.trait).makeGameObject();
            RectTransform rt = traitIcon.GetComponent<RectTransform>();
            rt.SetParent(panelRoot, false);
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.zero;
            rt.pivot = new Vector2(0.5f, 0);
            rt.sizeDelta = 30 * Vector2.one;
            rt.anchoredPosition = Vector2.zero;
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
