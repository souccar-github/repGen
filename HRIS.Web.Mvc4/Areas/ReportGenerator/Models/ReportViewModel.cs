using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Souccar.ReportGenerator.Domain.QueryBuilder;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;
using System.Web.Script.Serialization;
using Souccar.Infrastructure.Core;
using HRIS.Validation.MessageKeys;
using Project.Web.Mvc4.Helpers.Resource;

namespace Project.Web.Mvc4.Areas.ReportGenerator.Models
{
    public class ReportViewModel:ViewModel
    {

        public int Id { get; set; }
        //public virtual IList<SectionQueryTreeViewModel> Items { get; set; }

        public ReportViewModel()
        {
           // Items = new List<SectionQueryTreeViewModel>();
        }

        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
             requestInformation.NavigationInfo.Next.Clear();

            //Show Windows with Two Columns
             model.Views[0].ShowTwoColumns = true;

            //model.Views[0].EditHandler = "QueryTreeEditHandler";
           //model.Views[0].ViewHandler = "QueryTreeViewHandler";
            //Call Event Handlers
            model.ViewModelTypeFullName = typeof(ReportViewModel).FullName;
            model.Views[0].EditHandler = "QueryTreeEditHandler";
            model.Views[0].ViewHandler = "QueryTreeViewHandler";
        }
        //public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        //{
        //    var report = entity as Report;

        //    var theReportNameIsExist = ServiceFactory.ORMService.All<Report>().
        //        Any(x => (x.Name == report.Name && x.Id != report.Id)|| (x.Name == null && x.Id != report.Id))  ;

        //    if (theReportNameIsExist)
        //    {
        //        validationResults.Add(
        //            new ValidationResult()
        //            {
        //                Message = ReportLocalizationHelper.GetResource(ReportLocalizationHelper.ReportNameAlreadyExists),
        //                Property = typeof(Report).GetProperty("Name")

        //            });

        //   }
        //}
        public override void BeforeInsert(RequestInformation requestInformation, Entity entity,
         string customInformation = null)
        {
            this.PreventDefault = true;
        }


    }



    //public class SectionQueryTreeViewModel
    //{
    //    public SectionQueryTreeViewModel()
    //    {

    //        Nodes = new List<SectionQueryTreeViewModel>();
    //        Leaves = new List<SectionLeaveTreeViewModel>();
    //        AggregateFilters = new List<SectionAggregateFiltersViewModel>();
    //        //AggregateOperations = new List<SectionAggregateOperationsViewModel>();
    //    }


    //    public int Id { get; set; }

    //    public virtual Report Report { set; get; }

    //    public virtual Type Type { get; set; }

    //    public virtual Type DefiningType { get; set; }


    //    public virtual IList<SectionQueryTreeViewModel> Nodes { get; set; }
    //    public virtual IList<SectionLeaveTreeViewModel> Leaves { get; set; }

    //    public virtual string FullClassPath { get; set; }


    //    public virtual string FullClassName { get; set; }

    //    public virtual int SelectOrder { get; set; }

    //    public virtual IList<SectionAggregateFiltersViewModel> AggregateFilters { get; set; }
    //    //public virtual IList<SectionAggregateOperationsViewModel> AggregateOperations { get; set; }


    //}



    //public class SectionLeaveTreeViewModel
    //{
    //    public virtual Type PropertyType { get; set; }


    //    public virtual bool IsPrimitive { get; set; }


    //    public virtual Type ParentType { get; set; }


    //    public virtual Type DefiningType { get; set; }


    //    public virtual string PropertyFullPath { get; set; }


    //    public virtual int Selected { get; set; }


    //    public virtual bool IsReference { get; set; }

    //    public virtual SortDescriptor SortDescriptor { get; set; }


    //    public virtual GroupDescriptor GroupDescriptor { get; set; }

    //    //public virtual IList<FilterDescriptor> FilterDescriptors { get; set; }

    //    public virtual string PropertyName { get; set; }

    //}
    //public class SectionAggregateFiltersViewModel
    //{



    //}
    //public class SectionAggregateOperationsViewModel
    //{



    //}

}