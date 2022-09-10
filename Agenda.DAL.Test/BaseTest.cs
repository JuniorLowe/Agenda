using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace Agenda.DAL.Test
{

    [TestFixture]
    public class BaseTest
    {

        private string _script;
        private string _conn;
        private string _catalogTest;

        public BaseTest()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();


            _script = @"DBAgendaTest_Create.sql";
            _conn = configuration.GetConnectionString("connSetupTest");
            _catalogTest = configuration.GetConnectionString("providerName");

        }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            CreateDbTest();
        }
        [OneTimeTearDown]
        public void OnTimeTearDown()
        {
            DeleteDBTest();
        }

        private void CreateDbTest()
        {           
            using (var conn = new SqlConnection(_conn))
            {
                conn.Open();
                var scriptSql = File
                    .ReadAllText($@"{AppDomain.CurrentDomain.SetupInformation.ApplicationBase}\{_script}")
                    .Replace("$(DefaultDataPath)", $@"{AppDomain.CurrentDomain.SetupInformation.ApplicationBase}")
                    .Replace("$(DefaultLogPath)", $@"{AppDomain.CurrentDomain.SetupInformation.ApplicationBase}")
                    .Replace("$(DefaultFilePrefix)", _catalogTest)
                    .Replace("$(DatabaseName)", _catalogTest)
                    .Replace("WITH (DATA_COMPRESSION = PAGE)", String.Empty)
                    .Replace("SET NOEXEC ON", String.Empty)
                    .Replace("GO\r\n", "|");

                ExecuteScriptSQL(conn, scriptSql);
            }
        }

        private void ExecuteScriptSQL(SqlConnection conn, string scriptSql)
        {
            using (var cmd = conn.CreateCommand())
            {                
                foreach (var sql in scriptSql.Split("|"))
                {
                    cmd.CommandText = sql;
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {

                        Console.WriteLine(sql);
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

        private void DeleteDBTest()
        {
            using (var conn = new SqlConnection(_conn))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {                    
                    try
                    {                        
                        cmd.CommandText = $@"USE [master];
                        DECLARE @kill varchar(8000) = '';
                        SELECT @kill = @kill + 'Kill ' + CONVERT(varchar(5), session_id) + ';'
                        FROM  sys.dm_exec_sessions
                        WHERE database_id = db_id('{_catalogTest}') 
                        EXEC(@kill);";
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = $"DROP DATABASE {_catalogTest}";
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

    }
}
