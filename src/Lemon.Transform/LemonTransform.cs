using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace Lemon.Transform
{
    public class LemonTransform    {
        private static WindsorContainer _container = new WindsorContainer();

        private static PackageContainer _packageContainer = new PackageContainer();

        public static void RegisterServcie<TService, TImplement>() where TImplement : TService where TService : class
        {
            _container.Register(Component.For<TService>().ImplementedBy<TImplement>());
        }

        public static void RegisterDataInput<TDataInput>(string name) where TDataInput : IDataInput
        {
            _container.Register(Component.For<IDataInput>().Named(name + "_input")
                       .ImplementedBy<TDataInput>());
        }

        public static void RegisterDataOutput<TDataOutput>(string name) where TDataOutput : IDataOutput
        {
            _container.Register(Component.For<IDataOutput>().Named(name + "_output")
                .ImplementedBy<TDataOutput>());
        }

        public static void InstallPackage<TPackageInstallation>(string name) where TPackageInstallation : PackageInstallation
        {
            _packageContainer.InstallPackage<TPackageInstallation>(name);
        }

        public static void UseDefaultSevices()
        {
            RegisterDataInput<MongoDataInput>("mongo");
            RegisterDataOutput<MongoDataOutput>("mongo");
            RegisterServcie<IDataStoreService, JsonDataStoreService>();
        }

        internal static WindsorContainer Container {
            get { return _container; }
        }

        internal static PackageContainer PackageContainer
        {
            get { return _packageContainer; }
        }
    }
}
