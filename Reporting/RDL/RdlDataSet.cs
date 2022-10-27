using Reporting.Extensions;
using Souccar.ReportGenerator.Domain.QueryBuilder;
using Syncfusion.RDL.DOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reporting.RDL
{
    public class RdlDataSet
    {
        private Syncfusion.RDL.DOM.DataSource _dataSource;
        private Syncfusion.RDL.DOM.DataSet _dataSet;
        private QueryTree _queryTree;

        public RdlDataSet(
            Syncfusion.RDL.DOM.DataSource dataSource,
            QueryTree queryTree)
        {
            _dataSet = new Syncfusion.RDL.DOM.DataSet();
            _dataSource = dataSource;
            _queryTree = queryTree;
        }
        public Syncfusion.RDL.DOM.DataSet Create()
        {
            _dataSet.Name = "DataSet1";
            _dataSet.Query = GetQuery();
            _dataSet.Fields = GetFields();

            return _dataSet;
        }

        private Syncfusion.RDL.DOM.Query GetQuery()
        {
            var query = new Syncfusion.RDL.DOM.Query();
            query.DataSourceName = _dataSource.Name;
            query.CommandType = CommandType.Text;
            query.CommandText = BuilderQuery(_queryTree);
            query.Timeout = 30;

            return query;
        }
        private Fields GetFields()
        {
            var fields = new Syncfusion.RDL.DOM.Fields();

            var fieldsNames = GetFieldsNames(new List<string>(), _queryTree);
            foreach (var fieldName in fieldsNames)
            {
                var field = new Syncfusion.RDL.DOM.Field();
                field.Name = fieldName;
                field.DataField = fieldName;
                fields.Add(field);
            }
            return fields;
        }

        private List<string> GetFieldsNames(List<string> fields, QueryTree queryTree)
        {
            var name = queryTree.GetTableName();
            //Id field
            fields.Add($"{name}Id");

            var leafs = queryTree.Leaves.Where(x => x.IsSelected).ToList();
            for (var i = 0; i < leafs.Count; i++)
            {
                fields.Add($"{name}{leafs[i].DisplayName}");
            }

            foreach (QueryTree supQueryTree in queryTree.Nodes.Where(x => x.HasSelectedFields))
            {
                //Edit Walaa 26
                //if (queryTree.Type.GetProperty(supQueryTree.DisplayName)
                //   .PropertyType
                //   .GetInterface("IEnumerable") != null)
                //{
                    GetFieldsNames(fields, supQueryTree);
                //} 
                //end
            }

            return fields;
        }

        private string BuilderQuery(QueryTree queryTree)
        {
            var strBuilder = new StringBuilder();
            strBuilder.Append("SELECT DISTINCT ");

            //Fields
            var fields = new List<string>();
            fields = GetQueryFields(fields, queryTree);
            foreach (var field in fields)
            {
                strBuilder.Append(field);
            }

            strBuilder.Append(" FROM ");

            //Tables
            var tables = new List<string>();
            tables = GetTablesWithRelations(tables, queryTree);
            foreach (var table in tables)
            {
                strBuilder.Append(table);
            }

            return strBuilder.ToString();
        }

        private List<string> GetQueryFields(List<string> fields, QueryTree queryTree , QueryTree parent = null)
        {
            var name = queryTree.GetTableName(); ;
            var comma = "";
            if (parent != null)
                comma = ",";
            //Id
            fields.Add($"{comma} [{name}].[Id] AS {name}Id");

            var leafs = queryTree.Leaves.Where(x => x.IsSelected).ToList();
            for (var i = 0; i < leafs.Count; i++)
            {
                fields.Add($" , [{name}].[{leafs[i].DisplayName}] AS {name}{leafs[i].DisplayName}");
            }

            foreach (QueryTree supQueryTree in queryTree.Nodes.Where(x => x.HasSelectedFields))
            {
                //if (queryTree.Type.GetProperty(supQueryTree.DisplayName)
                //   .PropertyType
                //   .GetInterface("IEnumerable") != null)
                //{
                    GetQueryFields(fields, supQueryTree, queryTree);
                //}
            }

            return fields;
        }

        private List<string> GetTablesWithRelations(List<string> tables, QueryTree queryTree, QueryTree parent = null)
        {
            var name = queryTree.GetTableName();
            if (parent != null)
            {
                var parentName = parent.GetTableName();
                //Edit Walaa 
                var foreignKey = queryTree.Type.GetProperties().Any(x => x.PropertyType.Name == parentName); 
                if (foreignKey)
                { 
                tables.Add($" LEFT JOIN [{name}] ON [{parentName}].[Id] = [{name}].[{parentName}_id] ");
                    // في حال كان الربط One to many
                    //  tables.Add($" LEFT JOIN [{name}] ON [{parentName}].[{name}_Id] = [{name}].[Id]");
                }
                else
                {
                    tables.Add($" LEFT JOIN [{name}] ON [{parentName}].[{name}_Id] = [{name}].[Id] ");
                }
            }
            else
            {
                tables.Add($"{name}");
            }

            foreach (QueryTree supQueryTree in queryTree.Nodes.Where(x => x.HasSelectedFields))
            {
                //if (queryTree.Type.GetProperty(supQueryTree.DisplayName)
                //   .PropertyType
                //   .GetInterface("IEnumerable") != null)
                //{
                    GetTablesWithRelations(tables, supQueryTree, queryTree);
                //}
            }

            return tables;
        }
    }
}
