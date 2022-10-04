using Project.Web.Mvc4.Areas.OrganizationChart.Models;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Web.Mvc4.Areas.OrganizationChart.Models
{
    public class NodeViewModel : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type,
           RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof (NodeViewModel).FullName;
            model.ActionListHandler = "Node_ActionListHandler";
            model.Views[0].ViewHandler = "NodeViewHandler";
            model.ToolbarCommands.RemoveAt(0);
        }



    }
}