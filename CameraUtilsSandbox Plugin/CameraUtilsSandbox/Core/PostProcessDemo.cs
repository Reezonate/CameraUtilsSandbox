using System;
using CameraUtils.Core;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering;
using Zenject;

namespace CameraUtilsSandbox {
    [UsedImplicitly]
    public class PostProcessDemo : IInitializable, IDisposable {
        private CommandBuffer _commandBuffer;

        public void Initialize() {
            _commandBuffer = new CommandBuffer();
            _commandBuffer.DrawMesh(BundleLoader.PostProcessQuad, Matrix4x4.identity, BundleLoader.InvertPostProcessMaterial);
            CamerasManager.RegisterCommandBuffer(CameraEvent.AfterImageEffects, _commandBuffer, CameraFlags.HMD, CameraFlags.Mirror);
        }

        public void Dispose() {
            CamerasManager.UnRegisterCommandBuffer(_commandBuffer);
            _commandBuffer.Release();
        }
    }
}