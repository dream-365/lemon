using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Timers;
using Lemon.Transform.Models;

namespace Lemon.Transform
{
    public enum PipelineStatus
    {
        New = 0,
        Running = 1,
        Finished = 2,
        Failed = 3
    }

    public abstract class DataFlowPipeline
    {
        private ProgressIndicator _progressIndicator;

        private IList<Task> _compltetions;

        protected event Action OnStart;

        protected event Action OnComplete;

        private Timer _timer = new Timer(1000);

        private PipelineStatus _status;

        private ConnectionNode _rootNode;

        protected ConnectionNode RootNode { get { return _rootNode; } }

        public DataFlowPipeline()
        {
            _status = PipelineStatus.New;

            _compltetions = new List<Task>();

            _progressIndicator = new ProgressIndicator();

            OnStart += DataFlowPipeline_OnStart;

            OnComplete += DataFlowPipeline_OnComplete;

            _timer.AutoReset = true;

            _timer.Elapsed += TimeIntervalCallback; ;
        }

        private void TimeIntervalCallback(object sender, ElapsedEventArgs e)
        {
            OnProgressChange(GetState());
        }

        protected virtual void OnProgressChange(IEnumerable<ProgressStateItem> progress)
        {

        }

        public IEnumerable<ProgressStateItem> GetState()
        {
            IList<ProgressStateItem> items = new List<ProgressStateItem>();

            foreach(var kv in _progressIndicator.GetAllProgress())
            {
                var temp = kv.Key.Split('.');

                var item = new ProgressStateItem
                {
                    ActionName = temp[0],

                    PortName = temp.Length > 1 ? temp[1] : "default",

                    Count = kv.Value
                };

                items.Add(item);
            }

            return items;
        }

        private void DataFlowPipeline_OnComplete()
        {
            _timer.Stop();

            OnProgressChange(GetState());
        }

        private void DataFlowPipeline_OnStart()
        {
            _timer.Start();
        }

        /// <summary>
        /// waits the objects for completes
        /// </summary>
        /// <param name="objects"></param>
        public void Waits(params LinkObject [] objects)
        {
            foreach(var obj in objects)
            {
                _compltetions.Add(obj.Compltetion);
            } 
        }

        protected abstract AbstractDataInput OnCreate(PipelineContext context);

        public PipelineStatus Run(IDictionary<string, object> namedParameters = null)
        {
            try
            {
                if (_status == PipelineStatus.Running)
                {
                    return _status;
                }

                LogService.Default.Info("data pipeline is running");

                _status = PipelineStatus.Running;

                _progressIndicator.Clear();

                var entry = OnCreate(new PipelineContext(_progressIndicator, namedParameters));

                _rootNode = entry.Node;

                if (OnStart != null)
                {
                    OnStart();
                }

                entry.Start(namedParameters);

                LogService.Default.Info("wait the pipeline for comptetion");

                Task.WaitAll(_compltetions.ToArray());

                _compltetions.Clear();

                _status = PipelineStatus.Finished;

                if (OnComplete != null)
                {
                    OnComplete();
                }

                LogService.Default.Info("pipeline completed!");
            }
            catch (Exception ex)
            {
                _status = PipelineStatus.Failed;

                LogService.Default.Error(string.Format("pipeline {0} is failed", GetType().Name), ex);
            }

            return _status;
        }

        public Task<PipelineStatus> RunAsync(IDictionary<string, object> namedParameters = null)
        {
            return Task.Run(() =>
            {
                return Run(namedParameters);                
            });
        }

        [Obsolete]
        public IEnumerable<KeyValuePair<string, long>> GetAllProgress()
        {
            return _progressIndicator.GetAllProgress();
        }
    }
}
