using Syncfusion.RDL.DOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reporting.RDL
{
    public class RdlDataSource
    {
        private string _connectionString;
        public RdlDataSource(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Syncfusion.RDL.DOM.DataSource Create()
        {
            var dataSource = new Syncfusion.RDL.DOM.DataSource();
            dataSource.Name = "DataSource1";
            var connectionProperties = new ConnectionProperties();
            connectionProperties.DataProvider = "SQL";
            dataSource.ConnectionProperties = connectionProperties;
            dataSource.ConnectionProperties.ConnectString = _connectionString;
            dataSource.ConnectionProperties.IntegratedSecurity = true;

            return dataSource;
        }
    }
}
