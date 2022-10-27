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

            var filters = new List<string>();
            filters = GetQueryFilters(filters,queryTree);
            if (filters.Count > 0)
            {
                strBuilder.Append(" WHERE ");
            }
            foreach (var filter in filters)
            {
                strBuilder.Append(filter);
            }

            return strBuilder.ToString();
        }

        private List<string> GetQueryFields(List<string> fields, QueryTree queryTree, QueryTree parent = null)
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
                var foreignKey = queryTree.Type.GetProperties().Any(x => x.PropertyType.Name == parentName);
                if (foreignKey)
                {
                    tables.Add($" LEFT JOIN [{name}] ON [{parentName}].[Id] = [{name}].[{parentName}_id] ");
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
                GetTablesWithRelations(tables, supQueryTree, queryTree);
            }

            return tables;
        }

        private string GetOperatorChar(FilterDescriptor filter)
        {
            if (filter.FilterOperator == FilterOperator.IsGreaterThan)
                return ">";
            else if (filter.FilterOperator == FilterOperator.IsGreaterThanOrEqualTo)
                return ">=";
            else if (filter.FilterOperator == FilterOperator.IsLessThan)
                return "<";
            else if (filter.FilterOperator == FilterOperator.IsLessThanOrEqualTo)
                return "<=";
            else if (filter.FilterOperator == FilterOperator.IsEqualTo)
                return "=";
            else if (filter.FilterOperator == FilterOperator.IsNotEqualTo)
                return "!=";
            else
                return "like";
        }

        private List<string> GetQueryFilters(List<string> filters, QueryTree queryTree, int counter = 0)
        {
            var name = queryTree.GetTableName();
            foreach(var leave in queryTree.Leaves.Where(x => x.IsSelected))
            {
                if(leave.FilterDescriptors.Count > 0)
                {
                    if (counter > 0)
                    {
                        filters.Add(" AND ");
                    }
                    foreach (var filter in leave.FilterDescriptors)
                    {
                        var op = GetOperatorChar(filter);
                       
                        if (filter.FilterOperator == FilterOperator.Contains || filter.FilterOperator == FilterOperator.StartsWith || filter.FilterOperator == FilterOperator.EndsWith)
                        {
                            if(filter.FilterOperator == FilterOperator.Contains)
                            {
                                filters.Add($" [{name}].[{leave.PropertyName}] {op} N'%{filter.Value}%' ");
                            }
                            else if (filter.FilterOperator == FilterOperator.StartsWith)
                            {
                                filters.Add($" [{name}].[{leave.PropertyName}] {op} N'{filter.Value}%' ");
                            }
                            else if (filter.FilterOperator == FilterOperator.EndsWith)
                            {
                                filters.Add($" [{name}].[{leave.PropertyName}] {op} N'%{filter.Value}' ");
                            }
                        }
                        else
                        {
                            filters.Add($" [{name}].[{leave.PropertyName}] {op} N'{filter.Value}' ");
                        }
                        counter++;
                    }
                }
            }
            foreach(var node in queryTree.Nodes.Where(x => x.HasSelectedFields))
            {
                GetQueryFilters(filters, node,counter);
            }
            return filters;
        }

    }
}
