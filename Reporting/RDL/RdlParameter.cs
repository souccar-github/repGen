using HRIS.Domain.Personnel.RootEntities;
using Souccar.ReportGenerator.Domain.QueryBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Souccar.Infrastructure.Extenstions;
using System.Reflection;

namespace Reporting.RDL
{
    public class RdlParameter
    {
        private Syncfusion.RDL.DOM.ReportParameters _reportParameters;
        private Syncfusion.RDL.DOM.ReportParameter parameter;
        private QueryTree _queryTree;
        public RdlParameter(QueryTree queryTree)
        {
           
            _queryTree = queryTree;
            _reportParameters = new Syncfusion.RDL.DOM.ReportParameters();
        }

        //public Syncfusion.RDL.DOM.ReportParameters Create()
        //{
        //    _reportParameters = new Syncfusion.RDL.DOM.ReportParameters();

        //    foreach (var leave in _queryTree.Leaves.Where(x => x.IsSelected))
        //    {
        //        if (leave.FilterDescriptors.Count > 0)
        //        {
        //            foreach (var filter in leave.FilterDescriptors)
        //            {
        //                var type = _queryTree.Type;
        //                var propInfo = type.GetProperty(leave.PropertyName);
        //                parameter.Name = propInfo.Name;
        //                parameter.Prompt = leave.DisplayName;
        //                //Method to get Data type from Type
        //                parameter.DataType = Syncfusion.RDL.DOM.DataTypes.String;
        //                parameter.ValidValues = GetValidValues(propInfo);
        //                parameter.DefaultValue = GetDefaultValue(propInfo);
        //                _reportParameters.Add(parameter);
        //            }
        //        }
        //    }
        //    foreach (var node in _queryTree.Nodes.Where(x => x.HasSelectedFields))
        //    {
        //        Create();
        //    }
        //    return _reportParameters;
        //}


        public Syncfusion.RDL.DOM.ReportParameters Create(QueryTree queryTree)
        {
            foreach (var leave in queryTree.Leaves.Where(x => x.IsSelected))
            {
                if (leave.FilterDescriptors.Count > 0)
                {
                        parameter = new Syncfusion.RDL.DOM.ReportParameter();
                        var type = leave.PropertyType;
                        var propInfo = type.Name;
                        parameter.Name = leave.PropertyName;
                        parameter.Prompt = leave.DisplayName;
                    //Method to get Data type from Type
                   // parameter.DataType = GetParameterDataType(propInfo);
                    // parameter.DataType = Syncfusion.RDL.DOM.DataTypes.String;
                    //parameter.ValidValues = GetValidValues(propInfo);
                    //  parameter.DefaultValue = GetDefaultValue(propInfo);
                    _reportParameters.Add(parameter);
                }
            }
            foreach (var node in queryTree.Nodes.Where(x => x.HasSelectedFields))
            {
                Create(node);
            }
            return _reportParameters;
        }
        //private Syncfusion.RDL.DOM.DataTypes GetParameterDataType(string propertyInfo)
        //{
        //    if(propertyInfo == Syncfusion.RDL.DOM.DataTypes.String.ToString())
        //    {
        //        return Syncfusion.RDL.DOM.DataTypes.String;
        //    }
        //    else if(propertyInfo == Syncfusion.RDL.DOM.DataTypes.Boolean.ToString())
        //    {
        //        return Syncfusion.RDL.DOM.DataTypes.Boolean;
        //    }
           
        //}

        private Syncfusion.RDL.DOM.ReportParameter GetParameterName(QueryTree queryTree)
        {
            var name = parameter.Name;
            foreach (var leave in queryTree.Leaves.Where(x => x.IsSelected))
            {
                if (leave.FilterDescriptors.Count > 0)
                {
                    name = leave.DisplayName;
                }
                foreach (var node in queryTree.Nodes.Where(x => x.HasSelectedFields))
                {
                    GetParameterName(node);
                }
            }
            return null;
        }
        private Syncfusion.RDL.DOM.DefaultValue GetDefaultValue(PropertyInfo propertyInfo)
        {
            var defaultValue = new Syncfusion.RDL.DOM.DefaultValue();
            defaultValue.Values.Add("111");
            return defaultValue;
        }

        private Syncfusion.RDL.DOM.ValidValues GetValidValues(PropertyInfo propInfo)
        {
            var validValue = new Syncfusion.RDL.DOM.ValidValues();
            var dataSetReference = new Syncfusion.RDL.DOM.DataSetReference();
            dataSetReference.DataSetName = $"{propInfo.Name}DataSet";
            dataSetReference.ValueField = "Id";
            dataSetReference.LabelField = "Id";
            validValue.DataSetReference = dataSetReference;

            return validValue;
        }

        private Syncfusion.RDL.DOM.DataTypes GetType(PropertyInfo propertyInfo)
        {
            if(propertyInfo.PropertyType == typeof(String))
            {
                return Syncfusion.RDL.DOM.DataTypes.String;
            }

            return Syncfusion.RDL.DOM.DataTypes.String;
        }
    }
}
