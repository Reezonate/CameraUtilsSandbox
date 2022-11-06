using System;
using UnityEngine;
using UnityEngine.Rendering;
using Object = UnityEngine.Object;

namespace CameraOverlayTest {
    public class Composition : IDisposable {
        #region Constructor

        private readonly Material _compositionMaterial;
        private readonly Camera[] _cameras;
        private readonly RenderTexture[] _targetTextures;
        private readonly Material[] _clearMaterials;
        private readonly CommandBuffer[] _clearCommandBuffers;
        private readonly int _screenWidth;
        private readonly int _screenHeight;
        private readonly int _camerasCount;

        public Composition(
            Material compositionMaterial,
            Camera[] cameras,
            RenderTexture[] targetTextures,
            Material[] clearMaterials,
            CommandBuffer[] clearCommandBuffers,
            int screenWidth,
            int screenHeight
        ) {
            _cameras = cameras;
            _compositionMaterial = compositionMaterial;
            _targetTextures = targetTextures;
            _clearMaterials = clearMaterials;
            _clearCommandBuffers = clearCommandBuffers;
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;
            _camerasCount = _cameras.Length;
        }

        #endregion

        #region Render

        public void Render(RenderTexture src, RenderTexture dest) {
            if (_camerasCount == 0) {
                Graphics.Blit(src, dest);
                return;
            }
            
            for (var i = 0; i < _camerasCount; i++) {
                _clearMaterials[i].mainTexture = src;
                _cameras[i].Render();
            }

            Graphics.Blit(src, dest, _compositionMaterial);
        }

        #endregion

        #region VerifyScreenSize

        public bool VerifyScreenSize(int screenWidth, int screenHeight) {
            return screenWidth == _screenWidth && screenHeight == _screenHeight;
        }

        #endregion

        #region Dispose

        public void Dispose() {
            Object.Destroy(_compositionMaterial);
            for (var i = 0; i < _camerasCount; i++) {
                _targetTextures[i].Release();
                _clearCommandBuffers[i].Release();
                Object.Destroy(_clearMaterials[i]);
            }
        }

        #endregion
    }
}