using HRIS.Mapping.Personnel.RootEntities;
using Souccar.NHibernate;
using Souccar.ReportGenerator.Domain.QueryBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Reporting.RDL
{
    public class RdlQueryBuilder
    {
        public string Query { get; set; }
        public RdlQueryBuilder(QueryTree queryTree)
        {
            Query = Builder(queryTree);
        }

        private string Builder(QueryTree queryTree)
        {
            var strBuilder = new StringBuilder();
            strBuilder.Append("SELECT DISTINCT ");

            //Fields
            var fields = new List<string>();
            fields = GetFieldsNames(fields, queryTree);
            foreach (var field in fields)
            {
                strBuilder.Append(field);
            }

            //Tables
            var tables = new List<string>();
            tables = GetTablesWithRelations(tables, queryTree);
            foreach (var table in tables)
            {
                strBuilder.Append(table);
            }

            return strBuilder.ToString();
        }

        private List<string> GetFieldsNames(List<string> fields, QueryTree queryTree)
        {
            var leafs = queryTree.Leaves.Where(x => x.IsSelected).ToList();
            for (var i = 0; i < leafs.Count; i++)
            {
                string comma = "";
                if (i != 0)
                    comma = ",";

                fields.Add($" {comma} [{leafs[i].DisplayName}].[{leafs[i].DisplayName}] AS {leafs[i].DisplayName}{leafs[i].DisplayName}");
            }

            foreach (QueryTree supQueryTree in queryTree.Nodes.Where(x => x.HasSelectedFields))
            {
                if (queryTree.Type.GetProperty(supQueryTree.DisplayName)
                   .PropertyType
                   .GetInterface("IEnumerable") != null)
                {
                    GetFieldsNames(fields, supQueryTree);
                }
            }

            return fields;
        }

        private List<string> GetTablesWithRelations(List<string> tables, QueryTree queryTree, QueryTree parent = null)
        {
            if(parent != null)
            {
                tables.Add($"[{queryTree.DisplayName}] ON [{parent.DisplayName}].[Id] = [{queryTree.DisplayName}].[{parent.DisplayName}_id] ");
            }
            else
            {
                
                tables.Add($"{queryTree.DisplayName}");
            }

            foreach (QueryTree supQueryTree in queryTree.Nodes.Where(x => x.HasSelectedFields))
            {
                if (queryTree.Type.GetProperty(supQueryTree.DisplayName)
                   .PropertyType
                   .GetInterface("IEnumerable") != null)
                {
                    GetTablesWithRelations(tables, supQueryTree, parent);
                }
            }

            return tables;
        }

        
    }
}