using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Lemon.Transform
{
    public class DefaultTransformEngine : CoreDocumentTransformEngine, ITransformEngine
    {
        private DefaultConnectionStringProvider _connectionStringProvider = new DefaultConnectionStringProvider();

        private IDocumentTransformProvider _transformProvider;

        private IDictionary<string, DataReaderRegistration> _dataReaderRegistrations;

        private IDictionary<string, DataWriterRegistration> _dataWriterRegistrations;

        public DefaultTransformEngine(IDocumentTransformProvider provider)
        {
            _transformProvider = provider;

            _dataReaderRegistrations = new Dictionary<string, DataReaderRegistration>();

            _dataWriterRegistrations = new Dictionary<string, DataWriterRegistration>();

            RegisterDataReader(new DataReaderRegistration { Name = "mongo", CreateNew = CreateMongoDataReader });
            RegisterDataReader(new DataReaderRegistration { Name = "mssql", CreateNew = CreateSqlDataReader });
            RegisterDataReader(new DataReaderRegistration { Name = "json", CreateNew = CreateJsonFileDataReader });

            RegisterDataWriter(new DataWriterRegistration { Name = "mongo", CreateNew = CreateMongoDataWritter });
            RegisterDataWriter(new DataWriterRegistration { Name = "mssql", CreateNew = CreateSqlDataWritter });
        }

        public void RegisterDataReader(DataReaderRegistration registration)
        {
            _dataReaderRegistrations.Add(registration.Name, registration);
        }

        public void RegisterDataWriter(DataWriterRegistration registration)
        {
            _dataWriterRegistrations.Add(registration.Name, registration);
        }

        public void Execute(TransformActionDefinition transformActionDefinition)
        {
            var transformer = _transformProvider.Get(transformActionDefinition.Transformer);

            ITransformDataReader reader = _dataReaderRegistrations[transformActionDefinition.Source.SourceType].CreateNew.Invoke(transformActionDefinition.Source);

            ITransformDataWritter writter = _dataWriterRegistrations[transformActionDefinition.Target.TargetType].CreateNew.Invoke(transformActionDefinition.Target);

            Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine("start transform action: [{0}]", transformActionDefinition.Transformer);

            var transformObject = new TransformObject
            {
                DataReader = reader,
                DataWritter = writter,
                TransformColumnDefinitions = transformer.GetTransformColumnDefinitions(),
                CalculationColumnDefinition = transformer.GetCalculationColumnDefinitions()
            };

            Execute(transformObject);

            Console.WriteLine();
        }


        private static ITransformDataReader CreateJsonFileDataReader(DataSource source)
        {
            return new JsonFileDataReader(source.Connection, source.PrimaryKey);
        }

        private ITransformDataReader CreateSqlDataReader(DataSource source)
        {
            var temp = source.Connection.Split('.');

            var logicDbName = temp[0];

            var queryName = temp[1];

            var connection = new SqlConnection(_connectionStringProvider.GetConnectionString(logicDbName));

            var sql = SqlNamedQueryProvider.Instance.Get(queryName);

            IDictionary<string, object> parameters = Newtonsoft.Json.JsonConvert.DeserializeObject<IDictionary<string, object>>(source.Filter);

            var reader = new SqlTransformDataReader(connection, sql, parameters);

            reader.SetPrimaryKey(source.PrimaryKey);

            return reader;
        }

        private ITransformDataReader CreateMongoDataReader(DataSource source)
        {
            var collection = GetCollection(source.Connection);

            return new MongoDataReader(collection, source.Filter);
        }

        private ITransformDataWritter CreateSqlDataWritter(DataTarget target)
        {
            var temp = target.Connection.Split('.');

            var logicDbName = temp[0];

            var tableName = temp[1];

            var connection = new SqlConnection(_connectionStringProvider.GetConnectionString(logicDbName));

            return new SqlDataWritter(connection, tableName, target.PrimaryKey);
        }

        private ITransformDataWritter CreateMongoDataWritter(DataTarget target)
        {
            var collection = GetCollection(target.Connection);

            return new MongoDataWritter(collection);
        }

        protected IMongoCollection<BsonDocument> GetCollection(string exp)
        {
            var temp = exp.Split('.');

            var databaseName = temp[0];

            var collectionName = temp[1];

            var client = new MongoClient(_connectionStringProvider.GetConnectionString("mongo:" + databaseName));

            var database = client.GetDatabase(databaseName);

            var collection = database.GetCollection<BsonDocument>(collectionName);

            return collection;
        }
    }
}
