using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace Lemon.Transform
{
    public class TransformConfiguration    {

        private WindsorContainer _container = new WindsorContainer();

        private bool _debug = false;

        public bool Debug {
            get {
                return _debug;
            }
            set
            {
                _debug = value;
            }
        }

        public void RegisterServcie<TService, TImplement>() where TImplement : TService where TService : class
        {
            _container.Register(Component.For<TService>().ImplementedBy<TImplement>());
        }

        public void RegisterDataInput<TDataInput>(string name) where TDataInput : AbstractDataInput
        {
            _container.Register(Component.For<AbstractDataInput>().Named(name + "_input")
                .LifeStyle
                .Transient
                .ImplementedBy<TDataInput>());

        }

        public void RegisterDataOutput<TDataOutput>(string name) where TDataOutput : AbstractDataOutput
        {
            _container.Register(Component.For<AbstractDataOutput>().Named(name + "_output")
                .LifeStyle
                .Transient
                .ImplementedBy<TDataOutput>());
        }

        public void UseDefaultSevices()
        {
            RegisterDataInput<MongoDataInput>("mongo");
            RegisterDataOutput<MongoDataOutput>("mongo");
            RegisterDataInput<SqlServerDataInput>("Microsoft.SqlServer");
            RegisterDataOutput<SqlServerDataOutput>("Microsoft.SqlServer");
            RegisterDataInput<JsonFileDataInput>("json");
            RegisterServcie<IDataSourcesRepository, JsonDataSourcesRepository>();
        }

        internal WindsorContainer Container {
            get { return _container; }
        }
    }
}
