using Lemon.Data.Core;
using System.Data.SqlClient;
using Dapper;
using System;
using System.Collections.Generic;

namespace Lemon.IO
{
    public enum WriteMode
    {
        Update = 0,
        Insert = 1,
        Upsert = 2
    }

    public class SqlDataWriter<T> : IDataWriter<T>
    {
        private const int TIMEOUT_MAX_TRY_TIMES = 10;

        private readonly string _connectionString;

        private readonly string _sql;

        public SqlDataWriter(string connectionString, string name, WriteMode mode = WriteMode.Upsert)
        {
            _connectionString = connectionString;

            var schema = Util.BuildSchemaFromType(typeof(T));

            schema.Name = name;

            if(mode == WriteMode.Upsert)
            {
                _sql = Util.BuildInsertSql(schema);
            }else if(mode == WriteMode.Insert)
            {
                _sql = Util.BuildInsertSql(schema, false);
            }else if(mode == WriteMode.Update)
            {
                _sql = Util.BuidUpdateSql(schema);
            }else
            {
                throw new Exception("not supported write mode: " + mode);
            }
        }

        public void Dispose()
        {
        }

        private void InternalWrite(IEnumerable<T> records)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var trans = connection.BeginTransaction())
                {
                    try
                    {
                        foreach (var record in records)
                        {
                            connection.Execute(_sql, record, trans);
                        }

                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw ex;
                    }
                }
            }

        }

        private void InternalWrite(T record)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(_sql, record);
            }
        }

        public void Write(IEnumerable<T> records)
        {
            for (int i = 0; i < TIMEOUT_MAX_TRY_TIMES; i++)
            {
                try
                {
                    InternalWrite(records);

                    return;
                }
                catch (SqlException ex)
                {
                    if (ex.Number == -2)
                    {
                        LogService.Default.Info("sql timeout try times: " + i);
                        continue;
                    }
                }
            }
        }

        public void Write(T record)
        {
            for (int i = 0; i < TIMEOUT_MAX_TRY_TIMES; i++)
            {
                try
                {
                    InternalWrite(record);

                    return;
                }
                catch (SqlException ex)
                {
                    if(ex.Number == -2)
                    {
                        LogService.Default.Info("sql timeout try times: " + i);
                                 
                        continue;
                    }

                    throw ex;
                }
            }

            throw new Exception("maxium try times reached");
        }
    }
}
