using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lemon.Data.IO;
using Lemon.Data.Core;

namespace LemonDemo
{
    public class DataCompare
    {
        public class CompareObserver : ICompareObserver<Message>
        {
            public void OnAdd(Message message)
            {
                Console.WriteLine("[ADD]-" + Newtonsoft.Json.JsonConvert.SerializeObject(message));
            }

            public void OnChange(Message previous, Message current)
            {
                
            }

            public void OnDelete(Message message)
            {
                Console.WriteLine("[DEL]-" + Newtonsoft.Json.JsonConvert.SerializeObject(message));
            }
        }

        public static void Run()
        {
            var ds1 = new DataSet<Message>(new Messages(10000000000000000).AsEnumerable());
            var ds2 = new DataSet<Message>(new Messages(10000000000000000).AsEnumerable());

            var engine = new DataSetCompareEngine();

             var exe = engine.Compare(ds1, ds2, new CompareOptions {
                 PrimaryKey = "Id",
                 ColumnsToCompare = new string[] { "Name" } });

            exe.Observer = new CompareObserver();

            var startTime = DateTime.Now;

            exe.RunAsync().Wait();

            var endTime = DateTime.Now;

            Console.WriteLine("Duration: {0}", endTime - startTime);
        }
    }
}
