#region

using System;
using AutoMapper;
using HRIS.Domain.OrgChart.Entities;
using HRIS.Domain.OrgChart.ValueObjects;
using HRIS.Domain.OrgChart.ValueObjects.AssignedGrade;

#endregion

namespace Service.OrgChart
{
    public class GradeHelpers
    {
        //public  void CloneGradeToPosition(int gardeId, int positionId, int nodeId)
        //{
        //    var gradeService = new EntityService<Grade>();
        //    var nodeService = new EntityService<Node>();

        //    Grade grade = gradeService.GetById(gardeId);
        //    Node node = nodeService.GetById(nodeId);
        //    Position position = node.Positions.Single(i => i.Id == positionId);

        //    CloneGradeToPosition(grade,position);
        //}

        public static void CloneGradeToPosition(int gardeId, Position position)
        {
            var gradeService = new EntityService<Grade>();
            Grade grade = gradeService.GetById(gardeId);
            CloneGradeToPosition(grade, position);
        }

        public static void CloneGradeToPosition(int gardeId, int positionId)
        {
            var gradeService = new EntityService<Grade>();
            var positionService = new EntityService<Position>();
            Grade grade = gradeService.GetById(gardeId);
            Position position = positionService.GetById(positionId);

            CloneGradeToPosition(grade, position);
        }

        public static void CloneGradeToPosition(Grade grade, Position position)
        {
            if (position.IsTransient())
            {
                throw new ArgumentException("Position can not be transient");
            }
            PositionGrade positionGrade = CreateGradePositionFromGrad(grade);
            position.AddGrade(positionGrade);
            var positionService = new EntityService<Position>();
            positionService.Update(position);
        }

        public static PositionGrade CreateGradePositionFromGrad(Grade grade)
        {
            if (grade == null)
            {
                throw new ArgumentNullException("Grade Can't be null");
            }
            if (grade.IsTransient())
            {
                throw new ArgumentException("Grade can not be transient");
            }

            var positionGrade = new PositionGrade();
            Mapper.CreateMap<CashBenefit, AssignedCashBenefit>().ForMember(dto => dto.Id, opt => opt.Ignore()).
                ForMember(dto => dto.ActiveDate, opt => opt.Ignore()).
                ForMember(dto => dto.InactiveDate, opt => opt.Ignore()).
                ForMember(dto => dto.Status, opt => opt.Ignore());
            Mapper.CreateMap<CashDeduction, AssignedCashDeduction>().ForMember(dto => dto.Id, opt => opt.Ignore()).
                ForMember(dto => dto.ActiveDate, opt => opt.Ignore()).
                ForMember(dto => dto.InactiveDate, opt => opt.Ignore()).
                ForMember(dto => dto.Status, opt => opt.Ignore());
            Mapper.CreateMap<NonCashBenefit, AssignedNonCashBenefit>().ForMember(dto => dto.Id, opt => opt.Ignore())
                .
                ForMember(dto => dto.ActiveDate, opt => opt.Ignore()).
                ForMember(dto => dto.InactiveDate, opt => opt.Ignore()).
                ForMember(dto => dto.Status, opt => opt.Ignore());

            Mapper.CreateMap<Insurance, AssignedInsurance>().ForMember(dto => dto.Id, opt => opt.Ignore()).
                ForMember(dto => dto.ActiveDate, opt => opt.Ignore()).
                ForMember(dto => dto.ExpiryDate, opt => opt.Ignore()).
                ForMember(dto => dto.InsuranceNo, opt => opt.Ignore());

            Mapper.CreateMap<Asset, AssignedAsset>().ForMember(dto => dto.Id, opt => opt.Ignore());
            Mapper.CreateMap<Grade, PositionGrade>().ForMember(dto => dto.Id, opt => opt.Ignore())
                .ForMember(dto => dto.GradeId, opt => opt.Ignore())
                .ForMember(dto => dto.Position, opt => opt.Ignore())
                .ForMember(dto => dto.FromDate, opt => opt.Ignore())
                .ForMember(dto => dto.ExpireDate, opt => opt.Ignore())
                .ForMember(dto => dto.Comment, opt => opt.Ignore());
            positionGrade.GradeId = grade.Id;
            Mapper.AssertConfigurationIsValid();
            Mapper.DynamicMap(grade, positionGrade);

            #region Dates

            foreach (AssignedCashBenefit assignedCashBenefit in positionGrade.CashBenefits)
            {
                assignedCashBenefit.ActiveDate = DateTime.Today;
                assignedCashBenefit.InactiveDate = DateTime.Today;
            }

            foreach (AssignedCashDeduction assignedCashDeduction in positionGrade.CashDeductions)
            {
                assignedCashDeduction.ActiveDate = DateTime.Today;
                assignedCashDeduction.InactiveDate = DateTime.Today;
            }

            foreach (AssignedNonCashBenefit assignedNonCashBenefit in positionGrade.NonCashBenefits)
            {
                assignedNonCashBenefit.ActiveDate = DateTime.Today;
                assignedNonCashBenefit.InactiveDate = DateTime.Today;
            }

            foreach (AssignedInsurance assignedInsurance in positionGrade.Insurances)
            {
                assignedInsurance.ActiveDate = DateTime.Today;
                assignedInsurance.ExpiryDate = DateTime.Today;
            }

            foreach (AssignedAsset assignedAsset in positionGrade.Assets)
            {
                assignedAsset.PurchaseDate = DateTime.Today;
            }

            #endregion

            return positionGrade;
        }

        public static PositionGrade CreateGradePositionFromGrad(int gradeId)
        {
            if (gradeId == 0)
            {
                throw new ArgumentNullException("Grade Id Can't be transient");
            }
            var gradeService = new EntityService<Grade>();
            Grade grade = gradeService.LoadById(gradeId);
            return CreateGradePositionFromGrad(grade);
        }
    }
}