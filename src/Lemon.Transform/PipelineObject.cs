namespace Lemon.Transform
{
    public abstract class PipelineObject
    {
        private PipelineContext _context;

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
