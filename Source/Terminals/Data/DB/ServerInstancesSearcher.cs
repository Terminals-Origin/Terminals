using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Sql;
using System.Linq;
using System.Threading.Tasks;

namespace Terminals.Data.DB
{
    internal class ServerInstancesSearcher
    {
        internal const string SERVER_NAME_COLUMN = "ServerName";

        internal const string INSTANCE_NAME_COLUMN = "InstanceName";

        private readonly Func<DataTable> findInstances = SqlDataSourceEnumerator.Instance.GetDataSources;

        /// <summary>
        /// Creates new instance of Sql server instances search engine.
        /// </summary>
        /// <param name="findInstaces">Optional method, which can provide the datatable filled with server instances</param>
        public ServerInstancesSearcher(Func<DataTable> findInstaces = null)
        {
            if (findInstaces != null)
                this.findInstances = findInstaces;
        }

        /// <summary>
        /// Finds all available MS SQL server instances in form %Server%\%Instance%.
        /// Returns nenver null collection of found items. Returns task, because it can take long time.
        /// </summary>
        internal Task<List<string>> FindSqlServerInstancesAsync()
        {
            return Task<List<string>>.Factory.StartNew(FindSqlServerInstances);
        }

        private List<string> FindSqlServerInstances()
        {
            DataTable instancesTable = this.findInstances();
            
            if (instancesTable == null)
                return new List<string>();

            return instancesTable.Rows.OfType<DataRow>()
                                      .Select(ToFullInstanceName)
                                      .ToList();
        }

        private static string ToFullInstanceName(DataRow row)
        {
            string serverName = row[SERVER_NAME_COLUMN].ToString();
            string instanceName = row[INSTANCE_NAME_COLUMN].ToString();
            return FormatSqlInstanceDisplayName(serverName, instanceName);
        }

        private static string FormatSqlInstanceDisplayName(string serverName, string instanceName)
        {
            if (string.IsNullOrEmpty(instanceName))
                return serverName;

            return serverName + "\\" + instanceName;
        }
    }
}