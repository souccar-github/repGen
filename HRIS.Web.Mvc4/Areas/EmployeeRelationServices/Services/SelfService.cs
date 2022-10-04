using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.Global.Enums;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using NHibernate.Mapping;
using Souccar.Domain.DomainModel;
using Souccar.Infrastructure.Core;
using Project.Web.Mvc4.Extensions;

namespace Project.Web.Mvc4.Areas.EmployeeRelationServices.Services
{
    public static class SelfService
    {
        public static void Transfer(EmployeeTransfer transfer, EmployeeCard employeeCard)
        {
            var currentUser=UserExtensions.CurrentUser;
            transfer.Creator = currentUser;
            transfer.CreationDate = DateTime.Now;
            transfer.SourcePosition.AddPositionStatus(HRIS.Domain.JobDescription.Enum.PositionStatusType.Vacant);
            transfer.SourcePosition.JobDescription.JobTitle.Vacancies++;
            transfer.DestinationPosition.AddPositionStatus(HRIS.Domain.JobDescription.Enum.PositionStatusType.Assigned);
            transfer.DestinationPosition.JobDescription.JobTitle.Vacancies--;
            employeeCard.AddEmployeeTransfer(transfer);
            var ep = transfer.SourcePosition.AssigningEmployeeToPosition;
            ep.Position = transfer.DestinationPosition;
            transfer.DestinationPosition.AssigningEmployeeToPosition = ep;
            transfer.SourcePosition.AssigningEmployeeToPosition = null;
            ep.CreationDate = DateTime.Now;

            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>(){transfer,ep},currentUser);
        }
        public static void EndJobSecondPosition(EndingSecondaryPositionEmployee endSecondaryPosition, EmployeeCard employeeCard)
        {
            var currentUser = UserExtensions.CurrentUser;
            endSecondaryPosition.Creator = currentUser;
            var positions = endSecondaryPosition.EmployeeCard.Employee.Positions;
            var position = endSecondaryPosition.Position.AssigningEmployeeToPosition;
            position.Position.AddPositionStatus(HRIS.Domain.JobDescription.Enum.PositionStatusType.Vacant);
            position.Position.JobDescription.JobTitle.Vacancies++;
            var assignment =
                    ServiceFactory.ORMService.All<Assignment>()
                        .SingleOrDefault(x => x.AssigningEmployeeToPosition == position);
            position.Position.AssigningEmployeeToPosition = null;

            if (assignment != null)
                assignment.AssigningEmployeeToPosition = null;
            position.Position = null;
            positions.Remove(position);
            employeeCard.AddEndingSecondaryPosition(endSecondaryPosition);

            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { endSecondaryPosition.EmployeeCard.Employee, position, assignment }, currentUser);
        }
        
        public static void Promotion(EmployeePromotion promotion,EmployeeCard employeeCard)
        {
            var currentUser = UserExtensions.CurrentUser;
            promotion.Creator = currentUser;
            promotion.CreationDate = DateTime.Now;
            employeeCard.AddEmployeePromotion(promotion);

            var primaryPosition = employeeCard.Employee.Positions.SingleOrDefault(x => x.IsPrimary);

            promotion.Position.AddPositionStatus(HRIS.Domain.JobDescription.Enum.PositionStatusType.Assigned);
            promotion.Position.JobDescription.JobTitle.Vacancies--;

            if (primaryPosition != null)
            {
                primaryPosition.Position.AddPositionStatus(HRIS.Domain.JobDescription.Enum.PositionStatusType.Vacant);
                primaryPosition.Position.JobDescription.JobTitle.Vacancies++;
            }

            var ep=primaryPosition.Position.AssigningEmployeeToPosition;
            primaryPosition.Position.AssigningEmployeeToPosition = null;
            ep.Position = promotion.Position;
            promotion.Position.AssigningEmployeeToPosition = ep;
            ep.CreationDate = DateTime.Now;
            promotion.PromotionStatus = Status.Approved;
            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { employeeCard, ep }, currentUser);
        }
    }
}