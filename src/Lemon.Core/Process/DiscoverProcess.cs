using Lemon.Core.Config;
using Lemon.Core.Discover;
using Lemon.Core.Model;
using MongoDB.Bson;
using System;

namespace Lemon.Core
{
    public class DiscoverProcess
    {
        private IBuildIndexProvider _buildIndexProvider;

        private IMessageQueueProvider _messageQueueProvider;

        private JsonConfigrationManager<DiscoverSetting> _configrationManager;

        private Action<BsonDocument> _action;

        public string DispatchQueueName { get; set; }

        public void SetBuildIndexProivder(IBuildIndexProvider provider)
        {
            _buildIndexProvider = provider;
        }

        public void SetMessageQueueProvider(IMessageQueueProvider provider)
        {
            _messageQueueProvider = provider;
        }

        public void LoadSettings(string path)
        {
            _configrationManager = new JsonConfigrationManager<DiscoverSetting>(path);
        }

        public void OnNew(Action<BsonDocument> action)
        {
            _action = action;
        }

        public void Start(string name, string collection = null, string handler = null)
        {
            var setting = _configrationManager.GetSetting(name);

            var discoverObject = new DiscoverObject(setting);

            discoverObject.Builder = _buildIndexProvider.Get(setting.IndexBuilder);

            IMessageQueue dispatchQueue = _messageQueueProvider != null ? _messageQueueProvider.Get(DispatchQueueName, true) : null;

            var task = discoverObject.ForEachAsync((item) => {

                if(dispatchQueue != null)
                {
                    var message = new DownloadContentMessageBody
                    {
                        Url = item.GetValue("uri").AsString
                    };

                    message.Context.Add("saveTo", collection);

                    message.Context.Add("handler", handler);

                    dispatchQueue.Send(message);

                    Console.WriteLine("[Dispatch]");
                }

                if (_action != null)
                {
                    try
                    {
                        _action.Invoke(item);
                    }catch(System.Exception ex)
                    {
                        // Ignore the exception caused by external invoke
                    }
                }
            });

            task.Wait();
        }
    }
}
