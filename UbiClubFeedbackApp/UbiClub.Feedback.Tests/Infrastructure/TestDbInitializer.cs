using System;
using System.Data;
using System.IO;
using Microsoft.SqlServer.Dac;

namespace UbiClub.Feedback.Tests.Infrastructure
{
    public class TestDbInitializer
    {
        private static readonly object _lock = new object();
        private static bool _databaseInitialized;
        internal static bool DatabaseInitialized => _databaseInitialized;

        private const string DacpacFileName = "UbiClub.dacpac";
        private const string OriginalDbName = "UbiClub.Db";
        internal const string TargetDbName = "UbiClub.Test";
        private const string MasterDbName = "master";

        private const string ConnectionStringTemplate =
            "Data Source=(LocalDB)\\MSSQLLocalDB;Integrated Security=true;Database={0}";
        internal string ConnectionString => string.Format(ConnectionStringTemplate, TargetDbName);
        private string MasterConnectionString => string.Format(ConnectionStringTemplate, MasterDbName);
        
        /// <summary>
        /// Create Test DB & Seed
        /// </summary>
        public void Init()
        {
            lock (_lock)
            {
                if (!_databaseInitialized)
                {
                    var dacOptions = new DacDeployOptions
                    {
                        CreateNewDatabase = true
                    };

                    var svc = new DacServices(MasterConnectionString);

                    svc.Deploy( DacPackage.Load(DacpacFullPath),
                            TargetDbName,true,options: dacOptions);
                    _databaseInitialized = true;
                }
            }

        }

        private string DacpacFullPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DacpacFileName);
    }
}