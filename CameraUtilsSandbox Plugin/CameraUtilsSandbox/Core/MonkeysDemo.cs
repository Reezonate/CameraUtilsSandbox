using CameraUtils.Core;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

namespace CameraUtilsSandbox {
    [UsedImplicitly]
    public class MonkeysDemo : MonoBehaviour {
        #region Awake

        private const float Direction = Mathf.PI / 2;
        private const float Sector = Mathf.PI * 1.4f;
        private const float AngleFrom = Direction + Sector / 2;

        private const float Height = 0.1f;
        private const float Radius = 1.0f;

        public void Awake() {
            for (var i = 0; i < 32; i++) {
                var t = (float)i / 31;
                var angle = AngleFrom - Sector * t;

                var pos = new Vector3(Radius * Mathf.Cos(angle), Height, Radius * Mathf.Sin(angle));
                var rot = Quaternion.Euler(45, 90 - angle * Mathf.Rad2Deg, 0);

                var monkey = CreateMonkey((VisibilityLayer)i);
                monkey.transform.SetPositionAndRotation(pos, rot);
            }
        }

        #endregion

        #region CreateMonkey

        private GameObject CreateMonkey(VisibilityLayer visibilityLayer) {
            var rootObject = new GameObject("TestMonkey");
            var rootTransform = rootObject.transform;
            rootTransform.SetParent(transform, false);
            rootTransform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

            var labelYOffset = 0.12f + 0.06f * ((int)visibilityLayer % 2);

            var labelObject = new GameObject("Label");
            labelObject.transform.SetParent(rootTransform, false);
            labelObject.transform.localPosition = new Vector3(0, labelYOffset, 0);
            var text = labelObject.AddComponent<TextMeshPro>();
            text.text = $"[{(int)visibilityLayer}, {visibilityLayer}]";
            text.alignment = TextAlignmentOptions.Center;
            text.fontSize = 0.5f;

            var monkeyObject = Instantiate(BundleLoader.MonkeyPrefab, rootTransform, false);

            labelObject.SetLayer(VisibilityLayer.AlwaysVisibleAndReflected);
            monkeyObject.SetLayer(visibilityLayer);
            return rootObject;
        }

        #endregion
    }
}