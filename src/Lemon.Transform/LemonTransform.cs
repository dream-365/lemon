using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace Lemon.Transform
{
    public class LemonTransform    {
        private static WindsorContainer _container = new WindsorContainer();

        public static void RegisterServcie<TService, TImplement>() where TImplement : TService where TService : class
        {
            _container.Register(Component.For<TService>().ImplementedBy<TImplement>());
        }

        public static void RegisterDataInput<TDataInput>(string name) where TDataInput : AbstractDataInput
        {
            _container.Register(Component.For<AbstractDataInput>().Named(name + "_input")
                .LifeStyle
                .Transient
                .ImplementedBy<TDataInput>());

        }

        public static void RegisterDataOutput<TDataOutput>(string name) where TDataOutput : AbstractDataOutput
        {
            _container.Register(Component.For<AbstractDataOutput>().Named(name + "_output")
                .LifeStyle
                .Transient
                .ImplementedBy<TDataOutput>());
        }

        public static void UseDefaultSevices()
        {
            RegisterDataInput<MongoDataInput>("mongo");
            RegisterDataOutput<MongoDataOutput>("mongo");
            RegisterDataInput<SqlServerDataInput>("mssql");
            RegisterDataOutput<SqlServerDataOutput>("mssql");
            RegisterDataInput<JsonFileDataInput>("json");
            RegisterServcie<IDataStoreService, JsonDataStoreService>();
        }

        internal static WindsorContainer Container {
            get { return _container; }
        }
    }
}
