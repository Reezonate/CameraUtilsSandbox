using JetBrains.Annotations;
using Zenject;

namespace CameraUtilsSandbox.Installers {
    [UsedImplicitly]
    public class OnAppInstaller : Installer<OnAppInstaller> {
        public override void InstallBindings() {
            Container.Bind<MonkeysDemo>().FromNewComponentOnRoot().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<PostProcessDemo>().AsSingle();
        }
    }
}