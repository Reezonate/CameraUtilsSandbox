using CameraUtilsSandbox.Installers;
using IPA;
using JetBrains.Annotations;
using SiraUtil.Zenject;
using IPALogger = IPA.Logging.Logger;

namespace CameraUtilsSandbox {
    [Plugin(RuntimeOptions.SingleStartInit)]
    [UsedImplicitly]
    public class Plugin {
        #region Init

        internal static IPALogger Log { get; private set; }

        [Init]
        public Plugin(IPALogger logger, Zenjector zenjector) {
            Log = logger;
            BundleLoader.Initialize();
            zenjector.Install<OnAppInstaller>(Location.App);
        }

        #endregion

        #region OnApplicationStart

        [OnStart]
        [UsedImplicitly]
        public void OnApplicationStart() { }

        #endregion

        #region OnApplicationQuit

        [OnExit]
        [UsedImplicitly]
        public void OnApplicationQuit() { }

        #endregion
    }
}