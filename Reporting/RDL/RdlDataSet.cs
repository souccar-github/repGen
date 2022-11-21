using Reporting.Extensions;
using Souccar.Domain.DomainModel;
using Souccar.NHibernate;
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
        private Syncfusion.RDL.DOM.DataSets _dataSets;
        private QueryTree _queryTree;

        public RdlDataSet(
            Syncfusion.RDL.DOM.DataSource dataSource,
            QueryTree queryTree)
        {
            _dataSets = new Syncfusion.RDL.DOM.DataSets();
            _dataSource = dataSource;
            _queryTree = queryTree;
        }
        public Syncfusion.RDL.DOM.DataSets Create()
        {
            CreateMainDataSet();
            CreateValidValuesDataSets(_queryTree);
            //CreateValidValuesDataSet("FirstName");
            // Create parameters data set
            return _dataSets;
        }

        private void CreateMainDataSet()
        {
            var dataSet = new Syncfusion.RDL.DOM.DataSet();
            dataSet.Name = "MainDataSet";
            dataSet.Query = GetQuery();
            dataSet.Fields = GetFields();

            _dataSets.Add(dataSet);
        }
        private void CreateValidValuesDataSet(string PropName)
        {
            var dataSet = new Syncfusion.RDL.DOM.DataSet();
            dataSet.Name = $"{PropName}DataSet";
            dataSet.Query = GetQueryForIndex(PropName);
            dataSet.Fields = GetFieldsForIndex();

            _dataSets.Add(dataSet);
        }


        private Syncfusion.RDL.DOM.Query GetQueryForIndex(string PropName)
        {
            var query = new Syncfusion.RDL.DOM.Query();
            query.DataSourceName = _dataSource.Name;
            //Parans
            //  var queryParameters = new Syncfusion.RDL.DOM.QueryParameters();
            // GenerateQueryParameters(queryParameters, _queryTree);
            //  query.QueryParameters = new QueryParameters();
            //  query.QueryParameters.AddRange(queryParameters);
            query.CommandType = CommandType.Text;
            query.CommandText = QueryBuilderForIndex(_queryTree, PropName);
            query.Timeout = 30;


            return query;
        }


        public bool IsIndex(System.Type type)
        {
            return type.GetInterfaces().Any(inter => inter == typeof(IAggregateRoot)) &&
                   type.GetInterfaces().Any(inter => inter == typeof(IIndex));
        }


        private void CreateValidValuesDataSets(QueryTree queryTree)
        {
            foreach (var leave in queryTree.Leaves.Where(x => x.IsSelected))
            {
                if (leave.FilterDescriptors.Count > 0)
                {

                    var type = queryTree.Type;
                    var propInfo = type.GetProperty(leave.PropertyName);

                    if (IsIndex(propInfo.PropertyType))
                    {
                        CreateValidValuesDataSet(propInfo.Name);
                    }
                }
            }
            foreach (var node in queryTree.Nodes.Where(x => x.HasSelectedFields))
            {
                CreateValidValuesDataSets(node);
            }
        }

        private Syncfusion.RDL.DOM.Query GetValidValuesQuery()
        {
            var query = new Syncfusion.RDL.DOM.Query();
            query.DataSourceName = _dataSource.Name;
            //Params
            var queryParameters = new Syncfusion.RDL.DOM.QueryParameters();
            query.CommandType = CommandType.Text;

            var strBuilder = new StringBuilder();
            strBuilder.Append("SELECT DISTINCT ");

            //Fields
            var fields = new List<string>();
            fields = GetQueryFields(fields, _queryTree);
            foreach (var field in fields)
            {
                strBuilder.Append(field);
            }

            strBuilder.Append(" FROM ");

            //Tables
            var tables = new List<string>();
            tables = GetTablesWithRelations(tables, _queryTree);
            foreach (var table in tables)
            {
                strBuilder.Append(table);
            }

            query.CommandText = strBuilder.ToString();
            query.Timeout = 30;

            return query;
        }

        private Fields GetValidValuesFields()
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

        private Syncfusion.RDL.DOM.Query GetQuery()
        {
            var query = new Syncfusion.RDL.DOM.Query();
            query.DataSourceName = _dataSource.Name;
            //Parans
            var queryParameters = new Syncfusion.RDL.DOM.QueryParameters();
            GenerateQueryParameters(queryParameters, _queryTree);
            query.QueryParameters = new QueryParameters();
            query.QueryParameters.AddRange(queryParameters);
            query.CommandType = CommandType.Text;
            query.CommandText = BuilderQuery(_queryTree);
            query.Timeout = 30;


            return query;
        }

        private void GenerateQueryParameters(QueryParameters queryParameters, QueryTree _queryTree)
        {
            var parameters = new List<string>();
            parameters = GetParameters(parameters, _queryTree);

            foreach (var parameter in parameters)
            {
                QueryParameter par = new QueryParameter();
                par.Name = parameter;
                par.Value = $"=Parameters!{parameter.Replace('@', ' ').Trim()}.Value";
                queryParameters.Add(par);
            }

        }


        private Fields GetFieldsForIndex()
        {
            var fields = new Syncfusion.RDL.DOM.Fields()
            {
                new Field
                {
                    Name = "Id",
                    DataField = "Id"
                },
                new Field
                {
                    Name = "Name",
                    DataField = "Name"
                },
                new Field
                {
                    Name = "ValueOrder",
                    DataField = "ValueOrder"
                }
            };
            return fields;
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
            // var name = queryTree.GetTableName();
            var name = GetTableName(queryTree.Type);
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

        private string QueryBuilderForIndex(QueryTree queryTree, string propName)
        {
            var strBuilder = new StringBuilder();
            strBuilder.Append("SELECT DISTINCT ");
            var fields = new List<string>()
            {
                "[Id],",
                "[Name],",
                "[ValueOrder]"
            };
            foreach (var field in fields)
            {
                strBuilder.Append(field);
            }
            strBuilder.Append(" FROM ");

            //Tables

            var tables = new List<string>();


            GetIndexTables(tables, queryTree, propName);


            foreach (var table in tables)
            {
                strBuilder.Append(table);
            }
            return strBuilder.ToString();
        }

        private List<string> GetIndexTables(List<string> tables, QueryTree queryTree, string propName)
        {
            foreach (var leave in queryTree.Leaves.Where(x => x.IsSelected))
            {
                if (leave.FilterDescriptors.Count > 0 && leave.PropertyName == propName)
                {

                    var type = queryTree.Type;
                    var propInfo = type.GetProperty(leave.PropertyName);
                    var name = GetTableName(propInfo.PropertyType);
                    if (IsIndex(propInfo.PropertyType))
                    {
                        tables.Add(name);
                    }
                }
            }
            foreach (var node in queryTree.Nodes.Where(x => x.HasSelectedFields))
            {
                GetIndexTables(tables, node, propName);
            }
            return tables;
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

            var parameters = new List<string>();
            parameters = GetParameters(parameters, queryTree);

            //Function To Add QueryParameter

            RdlParameter rdlParameter = new RdlParameter(queryTree);
            ReportParameters RepParams = rdlParameter.Create(queryTree);

            var filters = new List<string>();
            // filters = GetQueryFilters(filters,queryTree);
            filters = GetQueryFiltersbyParameter(filters, queryTree);
            if (filters.Count > 0)
            {
                strBuilder.Append(" WHERE ");
            }
            foreach (var filter in filters)
            {
                strBuilder.Append(filter);
            }
            //
            var sorts = new List<string>();

            sorts = GetSorts(queryTree, sorts);
            if (sorts.Count > 0)
            {
                strBuilder.Append(" order by");
            }

            foreach (var sort in sorts)
            {
                strBuilder.Append(sort);
            }

            return strBuilder.ToString();
        }

        private List<string> GetQueryFields(List<string> fields, QueryTree queryTree, QueryTree parent = null)
        {
            // var name = queryTree.GetTableName();
            var name = GetTableName(queryTree.Type);
            var comma = "";
            if (parent != null)
                comma = ",";
            //Id
            fields.Add($"{comma} [{name}].[Id] AS {name}Id");

            var leafs = queryTree.Leaves.Where(x => x.IsSelected).ToList();
            for (var i = 0; i < leafs.Count; i++)
            {

                var type = queryTree.Type;
                var propInfo = type.GetProperty(leafs[i].PropertyName);
                if (IsIndex(propInfo.PropertyType))
                {
                    fields.Add($" , [{GetTableName(propInfo.PropertyType)}].[Name] AS {name}{leafs[i].DisplayName}");
                }

                else
                {
                    fields.Add($" , [{name}].[{leafs[i].DisplayName}] AS {name}{leafs[i].DisplayName}");
                }
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
            //var name = queryTree.GetTableName();
            var name = GetTableName(queryTree.Type);
            if (parent != null)
            {
                var parentName = GetTableName(parent.Type);
                //  var foreignKey = queryTree.Type.GetProperties().Any(x => x.PropertyType.Name == parent.GetTableName());
                var foreignKey = queryTree.Type.GetProperties().Any(x => x.PropertyType.Name == GetTableName(parent.Type));
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
            var leafs = queryTree.Leaves.Where(x => x.IsSelected).ToList();
            foreach (var leaf in leafs)
            {
                var type = queryTree.Type;
                var propInfo = type.GetProperty(leaf.PropertyName);
                if (IsIndex(propInfo.PropertyType))
                {
                    string newName = GetTableName(propInfo.PropertyType);
                    var newparentName = GetTableName(queryTree.Type);
                    if (parent != null)
                    {
                        newparentName = GetTableName(queryTree.Type);
                    }
                    tables.Add($" INNER JOIN [{newName}] ON [{newName}].[Id] = [{newparentName}].[{propInfo.Name}_id] ");
                }
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

        private string GetSortChar(SortDescriptor sort)
        {
            if (sort.SortDirection == ListSortDirection.Ascending)
                return "Asc";
            else
                return "Desc";
        }
        private List<string> GetQueryFilters(List<string> filters, QueryTree queryTree, int counter = 0)
        {
            // var name = queryTree.GetTableName();
            var name = GetTableName(queryTree.Type);
            foreach (var leave in queryTree.Leaves.Where(x => x.IsSelected))
            {
                if (leave.FilterDescriptors.Count > 0)
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
                            if (filter.FilterOperator == FilterOperator.Contains)
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
            foreach (var node in queryTree.Nodes.Where(x => x.HasSelectedFields))
            {
                GetQueryFilters(filters, node, counter);
            }
            return filters;
        }

        private List<string> GetParameters(List<string> parameters, QueryTree queryTree)
        {
            // var name = queryTree.GetTableName();
            foreach (var leave in queryTree.Leaves.Where(x => x.IsSelected))
            {
                if (leave.FilterDescriptors.Count > 0)
                {
                    foreach (var filter in leave.FilterDescriptors)
                    {
                        parameters.Add("@" + leave.PropertyName);
                    }
                }
            }
            foreach (var node in queryTree.Nodes.Where(x => x.HasSelectedFields))
            {
                GetParameters(parameters, node);
            }
            return parameters;
        }
        private List<string> GetSorts(QueryTree queryTree, List<string> sorts, int counter = 0)
        {
            var name = GetTableName(queryTree.Type);

            foreach (var leave in queryTree.Leaves.Where(x => x.IsSelected && x.IsSorted))
            {

                if (counter > 0)
                {
                    sorts.Add(" , ");
                }

                var sortchar = GetSortChar(leave.SortDescriptor);
                sorts.Add($" [{name}].[{leave.PropertyName}] {sortchar}");
                counter++;
            }
            foreach (var node in queryTree.Nodes.Where(x => x.HasSelectedFields))
            {
                GetSorts(node, sorts, counter);
            }
            return sorts;
        }
        private List<string> GetQueryFiltersbyParameter(List<string> filters, QueryTree queryTree, int counter = 0)
        {
            //var name = queryTree.GetTableName();
            var name = GetTableName(queryTree.Type);
            foreach (var leave in queryTree.Leaves.Where(x => x.IsSelected))
            {
                if (leave.FilterDescriptors.Count > 0)
                {
                    if (counter > 0)
                    {
                        filters.Add(" AND ");
                    }
                    foreach (var filter in leave.FilterDescriptors)
                    {
                        var op = GetOperatorChar(filter);

                        var type = queryTree.Type;
                        var propInfo = type.GetProperty(leave.PropertyName);
                        if (IsIndex(propInfo.PropertyType))
                        {
                            if (filter.FilterOperator == FilterOperator.Contains || filter.FilterOperator == FilterOperator.StartsWith || filter.FilterOperator == FilterOperator.EndsWith)
                            {
                                if (filter.FilterOperator == FilterOperator.Contains)
                                {
                                    filters.Add($" [{name}].[{leave.PropertyName}_Id] {op} N'%' + @{leave.PropertyName} + '%' ");
                                }
                                else if (filter.FilterOperator == FilterOperator.StartsWith)
                                {
                                    filters.Add($" [{name}].[{leave.PropertyName}_Id] {op}  @{leave.PropertyName} + '%' ");
                                }
                                else if (filter.FilterOperator == FilterOperator.EndsWith)
                                {
                                    filters.Add($" [{name}].[{leave.PropertyName}_Id] {op} N'%' + @{leave.PropertyName} ");
                                }
                            }
                            else
                            {
                                filters.Add($" [{name}].[{leave.PropertyName}_Id] in (@{leave.PropertyName}) ");
                            }
                            counter++;
                        }
                        else if (propInfo.PropertyType.IsEnum)
                        {
                            filters.Add($" [{name}].[{leave.PropertyName}] in (@{leave.PropertyName}) ");
                            counter++;
                        }
                        else
                        {
                            if (filter.FilterOperator == FilterOperator.Contains || filter.FilterOperator == FilterOperator.StartsWith || filter.FilterOperator == FilterOperator.EndsWith)
                            {
                                if (filter.FilterOperator == FilterOperator.Contains)
                                {
                                    filters.Add($" [{name}].[{leave.PropertyName}] {op} N'%' + @{leave.PropertyName} + '%' ");
                                }
                                else if (filter.FilterOperator == FilterOperator.StartsWith)
                                {
                                    filters.Add($" [{name}].[{leave.PropertyName}] {op}  @{leave.PropertyName} + '%' ");
                                }
                                else if (filter.FilterOperator == FilterOperator.EndsWith)
                                {
                                    filters.Add($" [{name}].[{leave.PropertyName}] {op} N'%' + @{leave.PropertyName} ");
                                }
                            }
                            else
                            {
                                filters.Add($" [{name}].[{leave.PropertyName}] {op} @{leave.PropertyName} ");
                            }
                            counter++;
                        }


                    }
                }
            }
            foreach (var node in queryTree.Nodes.Where(x => x.HasSelectedFields))
            {
                GetQueryFiltersbyParameter(filters, node, counter);
            }
            return filters;
        }

        public string GetTableName(System.Type entityType)
        {
            if (entityType.Name == "Level")
            {

            }
            var persisterEntity = NHibernateSession.Current.SessionFactory.GetClassMetadata(entityType) as NHibernate.Persister.Entity.AbstractEntityPersister;
            if (persisterEntity == null)
                return string.Empty;

            var tableName = persisterEntity.TableName.Replace("[", "").Replace("]", "");
            return tableName;
            //var txt = $"{entityType.Name}Map";
            //var mapAssembly = Assembly.GetAssembly(typeof(EmployeeMap));
            //var mapType = mapAssembly.GetType(txt);
            //var list = mapType.GetProperties();
            //return "";
        }

    }
}
