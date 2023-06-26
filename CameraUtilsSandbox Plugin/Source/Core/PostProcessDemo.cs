using System;
using CameraUtils.Core;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering;
using Zenject;

//InvertPostProcess shader: https://github.com/Reezonate/CameraUtilsSandbox/blob/master/CameraUtilsSandbox%20Bundle/Assets/CameraUtilsSandbox/3_Shaders/InvertPostProcess.shader

namespace CameraUtilsSandbox {
    [UsedImplicitly]
    public class PostProcessDemo : IInitializable, IDisposable, ICameraEffect {
        #region Init / Dispose

        private CommandBuffer _commandBuffer;

        public void Initialize() {
            _commandBuffer = new CommandBuffer();
            _commandBuffer.DrawMesh(BundleLoader.PostProcessQuad, Matrix4x4.identity, BundleLoader.InvertPostProcessMaterial);
            CamerasManager.RegisterCameraEffect(this);
        }

        public void Dispose() {
            CamerasManager.UnRegisterCameraEffect(this);
            _commandBuffer.Release();
        }

        #endregion

        #region ICameraEffect

        public bool IsSuitableForCamera(RegisteredCamera registeredCamera) {
            return !registeredCamera.CameraFlags.HasFlag(CameraFlags.Mirror) && !registeredCamera.CameraFlags.HasFlag(CameraFlags.HMD);
        }

        public void HandleAddedToCamera(RegisteredCamera registeredCamera) {
            registeredCamera.Camera.AddCommandBuffer(CameraEvent.AfterImageEffects, _commandBuffer);
        }

        public void HandleRemovedFromCamera(RegisteredCamera registeredCamera) {
            registeredCamera.Camera.RemoveCommandBuffer(CameraEvent.AfterImageEffects, _commandBuffer);
        }

        #endregion
    }
}