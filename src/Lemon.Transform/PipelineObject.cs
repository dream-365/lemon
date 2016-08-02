namespace Lemon.Transform
{
    public abstract class PipelineObject
    {
        private ConnectionNode _node;

        private PipelineContext _context;

        /// <summary>
        /// create the node if it does not exist
        /// </summary>
        public ConnectionNode Node {
            get {
                lock(this)
                {
                   if(_node == null)
                    {
                        _node = new ConnectionNode(Name);
                    }

                    return _node;
                }
            }
        }

        /// <summary>
        /// create the context if it does not exist
        /// </summary>
        public PipelineContext Context
        {
            get
            {
                lock (this)
                {
                    if (_context == null)
                    {
                        _context = new PipelineContext(new ProgressIndicator());
                    }
                }

                return _context;
            }

            set
            {
                _context = value;
            }
        }

        private string _name;

        public string Name
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_name))
                {
                    lock (this)
                    {
                        _name = this.GetType().Name + "_" + GetHashCode();
                    }
                }

                return _name;
            }

            set
            {
                _name = value;
            }
        }
    }
}
