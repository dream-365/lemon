using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using MongoDB.Bson;
using System.Collections.Generic;

namespace Lemon.Core
{
    public class ServicesInstaller
    {
        private IWindsorContainer _container;
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
            Build();
        }

        public void Rebuild()
        {
            Build();
        }

        private void Build()
        {
            _container = new WindsorContainer().Install(FromAssembly.This());
            InstallBuildInServices();
        }

        public void Install<TService, TImpl>()
        {
            _container.Register(Component
                 .For(typeof(TService))
                 .ImplementedBy(typeof(TImpl))
                 .LifeStyle.Transient);
        }

        private void InstallBuildInServices()
        {
            _container.Register(Component
             .For(typeof(IEqualityComparer<>))
             .ImplementedBy(typeof(FieldsEqualityComparer<>))
             .LifeStyle.Transient);

            _container.Register(Component
             .For(typeof(IComparer<>))
             .ImplementedBy(typeof(PrimaryKeyComparer<>))
             .LifeStyle.Transient);

            Install<IEqualityComparer<BsonDocument>, BsonFieldsEqualityComparer>();
            Install<IComparer<BsonDocument>, BsonPrimaryKeyComparer>();
        }
    }
}
