using System.Collections.Generic;
using CameraUtilsSandbox;
using UnityEngine;

namespace CameraOverlayTest {
    [DisallowMultipleComponent]
    public class CompositionImageEffect : MonoBehaviour {
        #region Serialized

        [SerializeField]
        public CompositionManagerSO compositionManagerSo;

        #endregion

        #region Cameras

        private readonly Dictionary<Camera, OverlayCamera> _cameras = new Dictionary<Camera, OverlayCamera>();

        public void SetCameraPosition(Camera camera, Rect normalizedScreenRect) {
            var screenRectV4 = new Vector4(normalizedScreenRect.x, normalizedScreenRect.y, normalizedScreenRect.width, normalizedScreenRect.height);
            if (_cameras.ContainsKey(camera) && _cameras[camera].NormalizedScreenRect == screenRectV4) return;

            var overlayCamera = new OverlayCamera(camera, screenRectV4);
            _cameras[camera] = overlayCamera;
            MarkIsDirty();
        }

        public void RemoveCamera(Camera camera) {
            if (!_cameras.ContainsKey(camera)) return;
            _cameras.Remove(camera);
            MarkIsDirty();
        }

        #endregion

        #region Events

        private void OnRenderImage(RenderTexture src, RenderTexture dest) {
            UpdateComposition(src.width, src.height);
            _composition.Render(src, dest);
        }

        private void OnDestroy() {
            Dispose();
        }

        #endregion

        #region Composition

        private Composition _composition;
        private bool _isDirty = true;

        private void UpdateComposition(int screenWidth, int screenHeight) {
            if (!_isDirty && _composition != null && _composition.VerifyScreenSize(screenWidth, screenHeight)) return;
            Dispose();
            _composition = compositionManagerSo.CreateComposition(_cameras.Values, screenWidth, screenHeight);
            _isDirty = false;
        }

        public void MarkIsDirty() {
            _isDirty = true;
        }

        private void Dispose() {
            _composition?.Dispose();
        }

        #endregion
    }
}