using System;
using System.Reflection;
using CameraOverlayTest;
using UnityEngine;

namespace CameraUtilsSandbox {
    public static class BundleLoader {
        #region Initialize

        private const string BundlePath = "CameraUtilsSandbox.AssetBundle.asset_bundle";
        private static bool _ready;

        public static void Initialize() {
            if (_ready) return;

            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(BundlePath);
            var localAssetBundle = AssetBundle.LoadFromStream(stream);

            if (localAssetBundle == null) {
                throw new Exception("AssetBundle load error!");
            }

            LoadAssets(localAssetBundle);

            localAssetBundle.Unload(false);
            _ready = true;
        }

        #endregion

        #region Assets

        public static GameObject MonkeyPrefab;
        public static Mesh PostProcessQuad;
        public static Material InvertPostProcessMaterial;
        public static CompositionManagerSO CompositionManager;

        private static void LoadAssets(AssetBundle assetBundle) {
            MonkeyPrefab = assetBundle.LoadAsset<GameObject>("MonkeyPrefab");
            PostProcessQuad = assetBundle.LoadAsset<Mesh>("PostProcessQuad");
            InvertPostProcessMaterial = assetBundle.LoadAsset<Material>("InvertPostProcess");
            CompositionManager = assetBundle.LoadAsset<CompositionManagerSO>("CompositionManager");
        }

        #endregion
    }
}