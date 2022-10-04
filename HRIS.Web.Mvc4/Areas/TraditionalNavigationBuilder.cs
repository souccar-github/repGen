using DevExpress.Web;
using System.Web.Mvc;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.Personnel.RootEntities;
using Project.Web.Mvc4.Factories;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.Navigation;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Project.Web.Mvc4.ProjectModels;
using System;

namespace Project.Web.Mvc4.Areas
{
    public class TraditionalNavigationBuilder : NavigationBuilder
    {

        private IList<NavigationTab> tab = new List<NavigationTab>();
        private int _order = 1;
        public override void BuildDomainTab()
        {
            var domainAssembly = typeof(Employee).Assembly;
            var modules = new List<KeyValuePair<Assembly, string>>
            {
                new KeyValuePair<Assembly, string>(domainAssembly, ModulesNames.Personnel),
                new KeyValuePair<Assembly, string>(domainAssembly, ModulesNames.OrganizationChart),
                new KeyValuePair<Assembly, string>(domainAssembly, ModulesNames.Grade),
                new KeyValuePair<Assembly, string>(domainAssembly, ModulesNames.JobDescription)
            };
            tab.Add(NavigationTabFactory.Create(modules, NavigationTabName.Administrative, _order++));
            modules = new List<KeyValuePair<Assembly, string>>
            {
                new KeyValuePair<Assembly, string>(domainAssembly, ModulesNames.PayrollSystem),
                new KeyValuePair<Assembly, string>(domainAssembly, ModulesNames.AttendanceSystem),
                new KeyValuePair<Assembly, string>(domainAssembly, ModulesNames.EmployeeRelationServices),
                new KeyValuePair<Assembly, string>(domainAssembly, ModulesNames.Workflow)
            };
            tab.Add(NavigationTabFactory.Create(modules, NavigationTabName.Operational, _order++));
            modules = new List<KeyValuePair<Assembly, string>>
            {
                new KeyValuePair<Assembly, string>(domainAssembly, ModulesNames.PMS),
                new KeyValuePair<Assembly, string>(domainAssembly, ModulesNames.Objective),
                new KeyValuePair<Assembly, string>(domainAssembly, ModulesNames.Training),
                new KeyValuePair<Assembly, string>(domainAssembly, ModulesNames.Recruitment)
            };
            tab.Add(NavigationTabFactory.Create(modules, NavigationTabName.Strategic, _order++));

        }
        public override void BuildLocalizationTab()
        {
            var localizationAssembly = typeof(Souccar.Domain.Localization.Language).Assembly;
            var reportGeneratorAssembly = typeof(Souccar.ReportGenerator.Domain.QueryBuilder.Report).Assembly;
            var modules = new List<KeyValuePair<Assembly, string>>
            {
                new KeyValuePair<Assembly, string>(localizationAssembly, ModulesNames.Localization),
                new KeyValuePair<Assembly, string>(localizationAssembly, ModulesNames.Security),
                new KeyValuePair<Assembly, string>(localizationAssembly, ModulesNames.Reporting),
                new KeyValuePair<Assembly, string>(localizationAssembly, ModulesNames.Audit),
                new KeyValuePair<Assembly, string>(reportGeneratorAssembly, ModulesNames.ReportGenerator)
            };
            tab.Add(NavigationTabFactory.Create(modules, NavigationTabName.System, _order++));
        }
        //public override void BuildReportTab()
        //{
        //    var reportGeneratorAssembly = typeof(QueryTree).Assembly;

        //    tab.FirstOrDefault(x => x.Name == NavigationTabName.System).Modules.Add(ModuleFactory.Create(reportGeneratorAssembly, ModulesNames.ReportGenerator));
        //}
        public override string BuildTabDesign()
        {
            return "~/Views/CustomUI/MaestroUI.cshtml"; ;
        }
        public override string GetStyle()
        {
            return "maestro-style 1";
        }
        public override IList<NavigationTab> GetNavigationTab()
        {
            return tab;
        }

    }
}