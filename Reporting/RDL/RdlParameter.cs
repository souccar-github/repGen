using HRIS.Domain.Personnel.RootEntities;
using Souccar.ReportGenerator.Domain.QueryBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Souccar.Infrastructure.Extenstions;
using System.Reflection;
using Syncfusion.RDL.DOM;
using Souccar.Domain.DomainModel;

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


        public bool IsIndex(System.Type type)
        {
            return type.GetInterfaces().Any(inter => inter == typeof(IAggregateRoot)) &&
                   type.GetInterfaces().Any(inter => inter == typeof(IIndex));
        }

        public Syncfusion.RDL.DOM.ReportParameters Create(QueryTree queryTree)
        {
            foreach (var leave in queryTree.Leaves.Where(x => x.IsSelected))
            {
                if (leave.FilterDescriptors.Count > 0)
                {
                        parameter = new Syncfusion.RDL.DOM.ReportParameter();

                        var type = queryTree.Type;
                        var propInfo = type.GetProperty(leave.PropertyName);
                    parameter.Name = leave.PropertyName;
                    parameter.Prompt = leave.DisplayName;
                    parameter.DataType = GetTypes(propInfo);

                    if (IsIndex(propInfo.PropertyType))
                    {
                        parameter.ValidValues = GetIndexValidValues(propInfo);
                    }
                    else
                    {
                        parameter.ValidValues = GetValidValues(propInfo);
                    }
                  
                   // parameter.ValidValues = GetValidValues(propInfo);
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

        //private Syncfusion.RDL.DOM.ReportParameter GetParameterName(QueryTree queryTree)
        //{
        //    var name = parameter.Name;
        //    foreach (var leave in queryTree.Leaves.Where(x => x.IsSelected))
        //    {
        //        if (leave.FilterDescriptors.Count > 0)
        //        {
        //            name = leave.DisplayName;
        //        }
        //        foreach (var node in queryTree.Nodes.Where(x => x.HasSelectedFields))
        //        {
        //            GetParameterName(node);
        //        }
        //    }
        //    return null;
        //}


        //private Syncfusion.RDL.DOM.DefaultValue GetDefaultValue(PropertyInfo propertyInfo)
        //{
        //    var defaultValue = new Syncfusion.RDL.DOM.DefaultValue();
        //    defaultValue.Values.Add("111");
        //    return defaultValue;
        //}

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
        private Syncfusion.RDL.DOM.ValidValues GetIndexValidValues(PropertyInfo propInfo)
        {
            var IndexvalidValue = new Syncfusion.RDL.DOM.ValidValues();
            var dataSetReference = new Syncfusion.RDL.DOM.DataSetReference();
            dataSetReference.DataSetName = $"{propInfo.Name}DataSet";
            dataSetReference.ValueField = "Id";
            dataSetReference.LabelField = "Name";
            IndexvalidValue.DataSetReference = dataSetReference;

            return IndexvalidValue;
        }

        private Syncfusion.RDL.DOM.DataTypes GetTypes(PropertyInfo Leave)
        {
            if(Leave == null)
            {
                return Syncfusion.RDL.DOM.DataTypes.String;
            }
            var datatype = Leave.PropertyType;
            DataTypes propType;
            if (datatype == typeof(Boolean))
            {
                propType = Syncfusion.RDL.DOM.DataTypes.Boolean;
            }
            else if (datatype == typeof(DateTime))
            {
                propType = Syncfusion.RDL.DOM.DataTypes.DateTime;
            }
            else if (datatype == typeof(int))
            {
                propType = Syncfusion.RDL.DOM.DataTypes.Integer;
            }
            else if(datatype == typeof(float))
            {
                propType = Syncfusion.RDL.DOM.DataTypes.Float;
            }
            else
            {
                propType = Syncfusion.RDL.DOM.DataTypes.String;
            }

            return propType;
        }
    }
}
