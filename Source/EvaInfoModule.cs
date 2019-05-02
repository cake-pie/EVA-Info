using UnityEngine;
using UnityEngine.UI;

namespace EvaInfo
{
    public sealed class EvaInfoModule : PartModule
    {
        private const float HelmetRadius = 0.27f;

        private ProtoCrewMember pcm = null;
        private Transform headTransform;
        private bool isInFrame = true;

        private Canvas canvas;
        private Camera camera;
        private RectTransform panelRootTransform;
        private EvaInfoPane infoPane;

        public override void OnLoad(ConfigNode node)
        {
            // Sanity check: this module only applies to KerbalEVAs
            if (!part.Modules.Contains<KerbalEVA>())
            {
                EvaInfoAddon.Log($"{part.name} is not a KerbalEVA! Removing module.");
                part.RemoveModule(this);
            }

            // Safety check: Community Trait Icons is available
            if (HighLogic.LoadedSceneIsFlight && !EvaInfoAddon.foundCTI)
            {
                EvaInfoAddon.Log("Unable to proceed without Community Trait Icons; removing module.");
                part.RemoveModule(this);
            }
        }

        public override void OnStart(StartState state)
        {
            if (!HighLogic.LoadedSceneIsFlight)
                return;

            int crewCount = vessel.GetCrewCount();
            if (crewCount < 1)
            {
                EvaInfoAddon.Log($"EVA Kerbal vessel {vessel.name} has {crewCount} crew. Perhaps this kerbal is dead inside?");
                EvaInfoAddon.Log("Unable to proceed; removing module.");
                part.RemoveModule(this);
                return;
            }
            if (crewCount > 1)
            {
                EvaInfoAddon.Log($"EVA Kerbal vessel {vessel.name} has {crewCount} crew. Perhaps this kerbal has multiple personality disorder?");
                EvaInfoAddon.Log("Proceeding using dominant personality (the first one in the list).");
            }

            this.pcm = vessel.GetVesselCrew()[0];
            headTransform = this.transform.Find("helmetAndHeadCollider");

            SetupPane();
            if (EvaInfoAddon.ShowInfo)
                PositionPane();
            else
                UpdateVisibility();

            GameEvents.onKerbalNameChanged.Add(OnKerbalNameChanged);
            GameEvents.onKerbalLevelUp.Add(OnKerbalLevelUp);
            EvaInfoAddon.onToggleInfo.Add(UpdateVisibility);
        }

        private void OnDestroy()
        {
            GameEvents.onKerbalNameChanged.Remove(OnKerbalNameChanged);
            GameEvents.onKerbalLevelUp.Remove(OnKerbalLevelUp);
            EvaInfoAddon.onToggleInfo.Remove(UpdateVisibility);

            CleanupPane();
        }

        private void LateUpdate()
        {
            if (EvaInfoAddon.ShowInfo)
                PositionPane();
        }

        private void UpdateVisibility()
        {
            canvas.enabled = EvaInfoAddon.ShowInfo && isInFrame;
        }

        private void SetupPane()
        {
            // canvas
            GameObject canvasGO = new GameObject($"EvaInfoCanvas ({pcm.name})");
            canvasGO.layer = LayerMask.NameToLayer("Default"); // Important! camera culls UI layer
            Transform canvasTransform = canvasGO.AddComponent<RectTransform>();
            canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.pixelPerfect = true;
            camera = FlightCamera.fetch.mainCamera;
            canvas.worldCamera = camera;
            canvas.planeDistance = camera.nearClipPlane;
            CanvasScaler canvasScalar = canvasGO.AddComponent<CanvasScaler>();
            canvasScalar.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
            canvasGO.AddComponent<GraphicRaycaster>();

            // panel root
            panelRootTransform = new GameObject("EvaInfoRoot").AddComponent<RectTransform>();
            panelRootTransform.SetParent(canvasTransform, false);
            panelRootTransform.anchorMin = Vector2.zero;
            panelRootTransform.anchorMax = Vector2.zero;
            panelRootTransform.pivot = Vector2.one/2;
            panelRootTransform.sizeDelta = Vector2.zero;

            infoPane = new EvaInfoPane(pcm, panelRootTransform);
        }

        private void PositionPane()
        {
            bool newInFrame = false;
            EvaInfoSettings settings = EvaInfoAddon.Instance.settings;
            float distance = Vector3.Dot((headTransform.position - camera.transform.position), camera.transform.forward);
            if (distance >= settings.nearClipDistance && distance <= settings.farClipDistance)
            {
                Vector2 screenPoint = (Vector2) camera.WorldToScreenPoint(headTransform.position);
                float helmetHeight = HelmetRadius * Screen.height / (2.0f * distance * Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad));
                screenPoint.y += helmetHeight;
                if (TestInFrame(screenPoint))
                {
                    float scale = 1.0f;
                    if (settings.scaleWithDistance && distance > settings.scaleStartDistance)
                        scale = ( (settings.farClipDistance - distance) / (settings.farClipDistance - settings.scaleStartDistance) * (1.0f - settings.scaleAtFarClip) ) + settings.scaleAtFarClip;
                    screenPoint.y += scale * (settings.OffsetHeight + settings.BottomRankOffset);

                    newInFrame = true;
                    canvas.planeDistance = distance;
                    panelRootTransform.localScale = scale * Vector3.one;
                    panelRootTransform.anchoredPosition = screenPoint;
                }
            }
            if (isInFrame != newInFrame)
            {
                isInFrame = newInFrame;
                UpdateVisibility();
            }
        }

        // lax test for whether screenpoint is a position where info pane would be at least partially visible
        private bool TestInFrame(Vector2 screenpoint)
        {
            float paneSize = EvaInfoAddon.Instance.settings.iconPaneSize;
            if (screenpoint.x < -paneSize/2 || screenpoint.x > (Screen.width + paneSize/2))
                return false;
            if (screenpoint.y < -(EvaInfoAddon.Instance.settings.OffsetHeight + paneSize) || screenpoint.y > Screen.height)
                return false;
            return true;
        }

        private void CleanupPane()
        {
            if (canvas != null) Destroy(canvas.gameObject);
            canvas = null;
            camera = null;
            panelRootTransform = null;
            infoPane = null;
        }

        private void OnKerbalNameChanged(ProtoCrewMember kerbal, string oldName, string newName)
        {
            if (kerbal == this.pcm)
                infoPane.updateName();
        }

        private void OnKerbalLevelUp(ProtoCrewMember kerbal)
        {
            if (kerbal == this.pcm)
                infoPane.updateLevel();
        }
    }
}
