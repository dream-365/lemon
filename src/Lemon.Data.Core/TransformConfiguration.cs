using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace Lemon.Data.Core
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

        public int? BoundedCapacity { get; set; }

        public void RegisterServcie<TService, TImplement>() where TImplement : TService where TService : class
        {
            _container.Register(Component.For<TService>().ImplementedBy<TImplement>());
        }

        public void UseDefaultSevices()
        {
        }

        internal WindsorContainer Container {
            get { return _container; }
        }
    }
}
