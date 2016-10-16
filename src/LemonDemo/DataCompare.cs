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
            var connectionString = "{connection string}";

            var sqlDataReader1 = new SqlDataReader<Message>(connectionString, "directory_objects", "Id != '0142ebdf-e6fe-4906-993a-e995a76da4cf'");

            var sqlDataReader2 = new SqlDataReader<Message>(connectionString, "directory_objects", "Id != '0134e450-dbf0-447c-8698-5f395f002454'");

            var ds1 = new DataSet<Message>(sqlDataReader1);
            var ds2 = new DataSet<Message>(sqlDataReader2);

            var engine = new DataSetCompareEngine();

             var exe = engine.Compare(ds1, ds2, new CompareOptions { PrimaryKey = "Id" });

            exe.Observer = new CompareObserver();

            var startTime = DateTime.Now;

            exe.RunAsync().Wait();

            var endTime = DateTime.Now;

            Console.WriteLine("Duration: {0}", endTime - startTime);
        }
    }
}
