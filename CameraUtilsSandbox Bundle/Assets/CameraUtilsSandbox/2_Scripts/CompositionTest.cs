using UnityEngine;

namespace CameraOverlayTest {
    public class CompositionTest : MonoBehaviour {
        public CompositionImageEffect imageEffect;

        public Camera sideCam1;
        public Rect sideCam1Rect;

        public Camera sideCam2;
        public Rect sideCam2Rect;

        public void Update() {
            imageEffect.SetCameraPosition(sideCam1, sideCam1Rect);
            imageEffect.SetCameraPosition(sideCam2, sideCam2Rect);
        }
    }
}