using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using System.Collections.Generic;

namespace Lemon.Data.Core
{
    public class ServicesInstaller
    {
        private readonly IWindsorContainer _container;
        private static ServicesInstaller _current;

        public static ServicesInstaller Current { get {
                if(_current == null)
                {
                    lock(typeof(ServicesInstaller))
                    {
                        _current = new ServicesInstaller();
                    }      
                }
                return _current;
            }
        }

        public IWindsorContainer Container { get { return _container; } }

        private ServicesInstaller()
        {
            _container = new WindsorContainer().Install(FromAssembly.This());
            InstallBuildInServices();
        }

        public void Install<TService, TImpl>()
        {
            _container.Register(Component
                 .For(typeof(TService))
                 .ImplementedBy(typeof(TImpl))
                 .LifestylePerThread());
        }

        private void InstallBuildInServices()
        {
            _container.Register(Component
             .For(typeof(IEqualityComparer<>))
             .ImplementedBy(typeof(FieldsEqualityComparer<>))
             .LifestylePerThread());

            _container.Register(Component
             .For(typeof(IComparer<>))
             .ImplementedBy(typeof(PrimaryKeyComparer<>))
             .LifestylePerThread());
        }
    }
}
