using UnityEngine;

namespace CameraOverlayTest {
    public struct OverlayCamera {
        public readonly Camera Camera;
        public readonly Vector4 NormalizedScreenRect;

        public OverlayCamera(Camera camera, Vector4 normalizedScreenRect) {
            Camera = camera;
            NormalizedScreenRect = normalizedScreenRect;
        }
    }
}