using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;

namespace Project.Web.Mvc4.Areas.PayrollSystem.EventHandlers
{
    public class DeductionCardEventHandlers : ViewModel
    {
        //
        // GET: /PayrollSystem/DeductionCardEventHandlers/

       public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.Views[0].EditHandler = "DeductionCard_EditHandler";
            model.ViewModelTypeFullName = typeof(DeductionCardEventHandlers).FullName;
        }

    }
}
