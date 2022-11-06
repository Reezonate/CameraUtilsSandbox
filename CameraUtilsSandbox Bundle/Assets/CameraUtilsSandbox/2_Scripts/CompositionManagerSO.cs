using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace CameraOverlayTest {
    [CreateAssetMenu(fileName = "CompositionManager", menuName = "Composition")]
    public class CompositionManagerSO : ScriptableObject {
        #region Properties

        private const int MaxCameras = 4;

        private static readonly int ScreenRectPropertyId = Shader.PropertyToID("_ScreenRect");
        private static readonly int[] CamTexProperties = new int[MaxCameras];
        private static readonly int[] CamRectProperties = new int[MaxCameras];

        static CompositionManagerSO() {
            for (var i = 0; i < MaxCameras; i++) {
                CamTexProperties[i] = Shader.PropertyToID($"_Cam{i}Tex");
                CamRectProperties[i] = Shader.PropertyToID($"_Cam{i}Rect");
            }
        }

        public Material compositionMaterial1Cam;
        public Material compositionMaterial2Cams;
        public Material compositionMaterial3Cams;
        public Material compositionMaterial4Cams;
        public Material clearMaterial;
        public Mesh postProcessQuad;

        private Material GetCompositionMaterial(int camerasCount) {
            switch (camerasCount) {
                case 1: return compositionMaterial1Cam;
                case 2: return compositionMaterial2Cams;
                case 3: return compositionMaterial3Cams;
                case 4: return compositionMaterial4Cams;
                default: throw new ArgumentOutOfRangeException(nameof(camerasCount));
            }
        }

        #endregion

        #region CreateComposition

        public Composition CreateComposition(
            IReadOnlyCollection<OverlayCamera> cameras,
            int screenWidth,
            int screenHeight
        ) {
            var compositionMaterial = Instantiate(GetCompositionMaterial(cameras.Count));
            var targetTextures = new RenderTexture[cameras.Count];
            var clearCommandBuffers = new CommandBuffer[cameras.Count];
            var clearMaterials = new Material[cameras.Count];
            var cams = new Camera[cameras.Count];

            var i = 0;
            foreach (var overlayCamera in cameras) {
                AdjustRectangle(
                    overlayCamera.NormalizedScreenRect, screenWidth, screenHeight,
                    out var pixelAdjustedRect, out var textureWidth, out var textureHeight
                );
                cams[i] = overlayCamera.Camera;
                targetTextures[i] = CreateCameraTargetTexture(textureWidth, textureHeight);
                clearMaterials[i] = CreateClearMaterial(pixelAdjustedRect);
                clearCommandBuffers[i] = CreateClearCommandBuffer(clearMaterials[i]);

                overlayCamera.Camera.enabled = false;
                overlayCamera.Camera.targetTexture = targetTextures[i];
                overlayCamera.Camera.AddCommandBuffer(CameraEvent.BeforeForwardOpaque, clearCommandBuffers[i]);

                compositionMaterial.SetTexture(CamTexProperties[i], targetTextures[i]);
                compositionMaterial.SetVector(CamRectProperties[i], pixelAdjustedRect);

                i += 1;
            }

            return new Composition(compositionMaterial, cams, targetTextures, clearMaterials, clearCommandBuffers, screenWidth, screenHeight);
        }

        #endregion

        #region Utils

        private static void AdjustRectangle(
            Vector4 rawRect, int screenWidth, int screenHeight,
            out Vector4 pixelAdjustedRect, out int textureWidth, out int textureHeight
        ) {
            var x = Mathf.RoundToInt(screenWidth * rawRect.x);
            var y = Mathf.RoundToInt(screenHeight * rawRect.y);
            textureWidth = Mathf.RoundToInt(screenWidth * rawRect.z);
            textureHeight = Mathf.RoundToInt(screenHeight * rawRect.w);
            pixelAdjustedRect = new Vector4(
                (float)x / screenWidth,
                (float)y / screenHeight,
                (float)textureWidth / screenWidth,
                (float)textureHeight / screenHeight
            );
        }

        private static RenderTexture CreateCameraTargetTexture(int textureWidth, int textureHeight) {
            var texture = new RenderTexture(textureWidth, textureHeight, 16);
            texture.Create();
            return texture;
        }

        private Material CreateClearMaterial(Vector4 screenRectangle) {
            var material = Instantiate(clearMaterial);
            material.SetVector(ScreenRectPropertyId, screenRectangle);
            return material;
        }

        private CommandBuffer CreateClearCommandBuffer(Material clearMaterialInstance) {
            var commandBuffer = new CommandBuffer();
            commandBuffer.DrawMesh(postProcessQuad, Matrix4x4.identity, clearMaterialInstance);
            return commandBuffer;
        }

        #endregion
    }
}