﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevExpress.XtraCharts.Native;
using HRIS.Domain.Workflow;
using Project.Web.Mvc4.Extensions;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.Workflow.RootEntities;
using Souccar.Infrastructure.Core;
using Project.Web.Mvc4.Factories;
using Souccar.Core.Extensions;

namespace Project.Web.Mvc4.Areas.Workflow.Models
{
    public class WorkflowItemsViewModel : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {

            model.ViewModelTypeFullName = typeof (WorkflowItemsViewModel).FullName;
            var editLocalizationName = ServiceFactory.LocalizationService.GetResource(GridModelLocalizationConst.ResourceGroupName + "_" + GridModelLocalizationConst.Edit) ?? GridModelLocalizationConst.Edit.ToCapitalLetters();
            var deleteLocalizationName = ServiceFactory.LocalizationService.GetResource(GridModelLocalizationConst.ResourceGroupName + "_" + GridModelLocalizationConst.Delete) ?? GridModelLocalizationConst.Delete.ToCapitalLetters();
            if (model.ToolbarCommands.Any(x => x.Name == BuiltinCommand.Create.ToString().ToLower()))
                model.ToolbarCommands.Remove(model.ToolbarCommands.FirstOrDefault(x => x.Name == BuiltinCommand.Create.ToString().ToLower()));
            if (model.ActionList.Commands.Any(x => x.Name == editLocalizationName))
                model.ActionList.Commands.Remove(model.ActionList.Commands.FirstOrDefault(x => x.Name == editLocalizationName));
            if (model.ActionList.Commands.Any(x => x.Name == deleteLocalizationName))
                model.ActionList.Commands.Remove(model.ActionList.Commands.FirstOrDefault(x => x.Name == deleteLocalizationName));
        }

        //public override void AfterRead(RequestInformation requestInformation, DataSourceResult result, int pageSize = 10,
        //    int skip = 0)
        //{
        //    var IsTrue = true;
        //    var allBeforNode = ((IQueryable<WorkflowItems>) result.Data).ToList();
        //    var currentworkitem = ServiceFactory.ORMService.All<WorkflowItems>().Select(x => x.Workitem.Id).ToList();
        //    while (IsTrue)
        //    {
        //        var _skip = 0;
        //        var all = ServiceFactory.ORMService.All<WorkflowItem>().Where(x => !currentworkitem.Contains(x.Id)).Skip(_skip*200).Take(200);
        //        var count = all.ToList().Count;
        //        if (count != 200)
        //            IsTrue = false;
        //        foreach (var workflowItem in all)
        //        {
        //            var newworkitem = new WorkflowItems()
        //            {
        //                Workitem = workflowItem
        //            };

        //            newworkitem.Save();
        //            allBeforNode.Add(newworkitem);
        //        }
        //        _skip++;
        //    }




        //    result.Data = allBeforNode.Skip<WorkflowItems>(skip).Take<WorkflowItems>(pageSize).AsQueryable();
        //    result.Total = allBeforNode.Count();
        //}
    }
}
