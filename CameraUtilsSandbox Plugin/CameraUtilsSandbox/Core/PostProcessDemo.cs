using System;
using CameraUtils.Core;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering;
using Zenject;

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
            _commandBuffer.Release();
            CamerasManager.UnRegisterCameraEffect(this);
        }

        #endregion

        #region ICameraEffect

        public bool IsSuitableForCamera(RegisteredCamera registeredCamera) {
            return registeredCamera.CameraFlags.HasFlag(CameraFlags.FirstPerson) && !registeredCamera.CameraFlags.HasFlag(CameraFlags.Mirror);
        }

        public void OnAddedToCamera(RegisteredCamera registeredCamera) {
            registeredCamera.Camera.AddCommandBuffer(CameraEvent.AfterImageEffects, _commandBuffer);
        }

        public void OnRemovedFromCamera(RegisteredCamera registeredCamera) {
            registeredCamera.Camera.RemoveCommandBuffer(CameraEvent.AfterImageEffects, _commandBuffer);
        }

        #endregion
    }
}