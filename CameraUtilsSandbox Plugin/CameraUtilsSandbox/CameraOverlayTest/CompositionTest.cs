using System;
using CameraUtils.Core;
using CameraUtilsSandbox;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace CameraOverlayTest;

[UsedImplicitly]
public class CompositionTest : IInitializable, IDisposable, ICameraEffect {
    #region MyRegion

    public void Initialize() {
        CamerasManager.RegisterCameraEffect(this);
    }

    public void Dispose() {
        CamerasManager.UnRegisterCameraEffect(this);
    }

    public bool IsSuitableForCamera(RegisteredCamera registeredCamera) {
        return registeredCamera.Camera.name switch {
            "MainCamera" => true,
            "RecorderCamera" => true,
            _ => false
        };
    }

    #endregion

    #region Logic

    private static readonly Vector3 SideCamPos = new(-3.0f, 1.2f, -1.0f);
    private static readonly Vector3 SideCamLookAt = new(0.0f, 1.0f, 1.0f);
    private static readonly Rect SideCamScreenRect = new(0.0f, 0.0f, 0.4f, 0.4f);
    
    private static readonly Vector3 BackCamPos = new(0.0f, 1.4f, -4.0f);
    private static readonly Vector3 BackCamLookAt = new(0.0f, 1.0f, 0.0f);
    private static readonly Rect BackCamScreenRect = new(0.6f, 0.0f, 0.4f, 0.4f);

    private CompositionImageEffect _imageEffect;
    private Camera _sideCamera;
    private Camera _backCamera;

    public void HandleAddedToCamera(RegisteredCamera registeredCamera) {
        _sideCamera = Object.Instantiate(registeredCamera.Camera);
        var cullingMask = _sideCamera.cullingMask;
        cullingMask &= ~(1 << (int)VisibilityLayer.Environment);
        cullingMask &= ~(1 << (int)VisibilityLayer.Skybox);
        _sideCamera.cullingMask = cullingMask;
        _sideCamera.enabled = false;
        _sideCamera.transform.position = SideCamPos;
        _sideCamera.transform.LookAt(SideCamLookAt);

        _backCamera = Object.Instantiate(_sideCamera);
        _backCamera.transform.position = BackCamPos;
        _backCamera.transform.LookAt(BackCamLookAt);

        _imageEffect = registeredCamera.Camera.gameObject.AddComponent<CompositionImageEffect>();
        _imageEffect.compositionManagerSo = BundleLoader.CompositionManager;
        _imageEffect.SetCameraPosition(_sideCamera, SideCamScreenRect);
        _imageEffect.SetCameraPosition(_backCamera, BackCamScreenRect);
    }

    public void HandleRemovedFromCamera(RegisteredCamera registeredCamera) { }

    #endregion
}