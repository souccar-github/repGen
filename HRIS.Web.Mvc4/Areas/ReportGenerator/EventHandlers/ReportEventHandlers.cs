using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.ReportGenerator.Domain.QueryBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Web.Mvc4.Areas.ReportGenerator.EventHandlers
{
    public class ReportEventHandlers : ViewModel
    {
 
    public int Id { get; set; }
    public virtual IList<SectionQueryTreeViewModel> Items { get; set; }

    public ReportEventHandlers()
    {
        Items = new List<SectionQueryTreeViewModel>();
    }

    public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
    {
            //Show Windows with Two Columns
            model.Views[0].ShowTwoColumns = true;

            //  model.Views[0].EditHandler = "QueryTreeEditHandler";
            //model.Views[0].ViewHandler = "QueryTreeViewHandler";
            //Call Event Handlers
            model.ViewModelTypeFullName = typeof(ReportEventHandlers).FullName;
            model.Views[0].EditHandler = "QueryTreeEditHandler";
            model.Views[0].ViewHandler = "QueryTreeViewHandler";
        }

  

}



public class SectionQueryTreeViewModel
    {
    public SectionQueryTreeViewModel()
    {
      
        Nodes = new List<SectionQueryTreeViewModel>();
        Leaves = new List<SectionLeaveTreeViewModel>();
        AggregateFilters = new List<SectionAggregateFiltersViewModel>();
        //AggregateOperations = new List<SectionAggregateOperationsViewModel>();
        }


        public int Id { get; set; }

        public virtual Report Report { set; get; }

        public virtual Type Type { get; set; }
  
        public virtual Type DefiningType { get; set; }

       
        public virtual IList<SectionQueryTreeViewModel> Nodes { get; set; }
        public virtual IList<SectionLeaveTreeViewModel> Leaves { get; set; }

        public virtual string FullClassPath { get; set; }


        public virtual string FullClassName { get; set; }

        public virtual int SelectOrder { get; set; }

    public virtual IList<SectionAggregateFiltersViewModel> AggregateFilters { get; set; }
    //public virtual IList<SectionAggregateOperationsViewModel> AggregateOperations { get; set; }


    }



    public class SectionLeaveTreeViewModel
    {
        public virtual Type PropertyType { get; set; }

      
        public virtual bool IsPrimitive { get; set; }

      
        public virtual Type ParentType { get; set; }

     
        public virtual Type DefiningType { get; set; }

      
        public virtual string PropertyFullPath { get; set; }

     
        public virtual int Selected { get; set; }

       
        public virtual bool IsReference { get; set; }

        public virtual SortDescriptor SortDescriptor { get; set; }

      
        public virtual GroupDescriptor GroupDescriptor { get; set; }

        //public virtual IList<FilterDescriptor> FilterDescriptors { get; set; }

        public virtual string PropertyName { get; set; }

    }
    public class SectionAggregateFiltersViewModel
    {
     


    }
    //public class SectionAggregateOperationsViewModel
    //{
      


    //}

}