using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace Lemon.Transform
{
    public class PackageContainer
    {
        private WindsorContainer _container = new WindsorContainer();

        public void InstallPackage<TPackageInstallation>(string name) where TPackageInstallation : PackageInstallation
        {
            _container.Register(
                Component.For<PackageInstallation>()
                         .Named(name)
                         .ImplementedBy<TPackageInstallation>());
        }

        public TransformPackage Resove(string name)
        {
            var installation = _container.Resolve<PackageInstallation>(name);

            return installation.Create();
        }
    }
}
