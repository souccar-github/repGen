using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRIS.Domain.AttendanceSystem.Entities;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.PayrollSystem.Entities;
using HRIS.Domain.Personnel.RootEntities;
using Project.Web.Mvc4.Areas.Personnel.Models;
using NHibernate.Hql.Ast.ANTLR;
using Project.Web.Mvc4.Areas.EmployeeRelationServices.Models;
using Project.Web.Mvc4.Models.Navigation;
using HRIS.Domain.Personnel.Entities;
using HRIS.Domain.Recruitment.RootEntities;
using HRIS.Domain.Personnel.Configurations;
using Project.Web.Mvc4.Factories;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Areas.Personnel.Models;
using Project.Web.Mvc4.Helpers;

public class PersonnelAdjustment : ModelAdjustment
{

    private static Dictionary<string, ViewModel> parent = new Dictionary<string, ViewModel>();
    public override void AdjustModule(Module module)
    {
        module.Aggregates.SingleOrDefault(x => x.TypeFullName == (typeof(EmployeeCard).FullName))
                .Details.SingleOrDefault(x => x.TypeFullName == (typeof(EmployeeLoan).FullName)).Details = DetailFactory.Create(typeof(EmployeeLoan));

        if (module.ModuleId.Equals(ModulesNames.Personnel))
        {
            var employeeDetails = new List<string>()
                {
                    "Children","Spouse","Dependents","Experiences","Educations","Trainings","Skills","Languages",
                    "Certifications","MilitaryService","Passports","DrivingLicense","Convictions","Residencies","Positions","Attachments"
                };
            module.Aggregates = module.Aggregates.Where(x => x.TypeFullName != typeof(Applicant).FullName).ToList();
            module.Aggregates.SingleOrDefault(x => x.TypeFullName == (typeof(Employee).FullName))
                .Details = module.Aggregates.SingleOrDefault(x => x.TypeFullName == (typeof(Employee).FullName))
                    .Details.Where(x => employeeDetails.Contains(x.DetailId))
                    .ToList();

            var employeeCardDetails = new List<string>()
                {
                    "EmployeeCustodies","PrimaryEmployeeBenefits","PrimaryEmployeeDeductions","EmployeeLoans","BankingInformations","TemporaryWorkshops"
                };

            module.Aggregates.SingleOrDefault(x => x.TypeFullName == (typeof(EmployeeCard).FullName))
                .Details = module.Aggregates.SingleOrDefault(x => x.TypeFullName == (typeof(EmployeeCard).FullName))
                    .Details.Where(x => employeeCardDetails.Contains(x.DetailId))
                    .ToList();
        }

        module.Dashboards.Add(new Dashboard()
        {
            Title = GlobalResource.Dashboard,
            Controller = "Personnel/Dashboard",
            Action = "PersonnelDashboard",
            DashboardId = "PersonnelDashboard",
            SecurityId = "PersonnelDashboard"
        });
    }


    public override ViewModel AdjustGridModel(string type)
    {
      
        if (parent.Count == 0)
        {
            parent.Add("Attachment", new AttachmentViewModel());
            parent.Add("MilitaryService", new MilitaryServiceViewModel());
            parent.Add("Passport", new PassportViewModel());
            parent.Add("Residency", new ResidencieViewModel());
            parent.Add("Spouse", new SpouseViewModel());
            parent.Add("Language", new LanguageViewModel());
            parent.Add("Certification", new CertificationViewModel());
            parent.Add("Conviction", new ConvictionViewModel());
            parent.Add("HealthInsuranceTypes", new DefineHealthInsuranceViewModel());
            parent.Add("Dependent", new DependentViewModel());
            parent.Add("DrivingLicense", new DrivingLicenseViewModel());
            parent.Add("EmployeeCustodie", new EmployeeCustodieViewModel());
            parent.Add("Experience", new ExperienceViewModel());
            parent.Add("JobRelatedInfo", new JobRelatedInfoViewModel());
            parent.Add("Skill", new SkillViewModel());
            parent.Add("Child", new ChildViewModel());
            parent.Add("EmployeeCodeSetting", new EmployeeCodeSettingViewModel());
            parent.Add("Employee", new Project.Web.Mvc4.Areas.Personnel.Models.EmployeeViewModel());

        }
        try
        {
            return parent[type];
        }
        catch
        {

            return new ViewModel();
        }

    }
}