using Souccar.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Souccar.Core.Extensions;

namespace Project.Web.Mvc4.Helpers.Resource
{
    public class EmployeeRelationServicesLocalizationHelper
    {
        public const string ResourceGroupName = "EmployeeRelationServicesModule";
        public const string ExitInterview = "ExitInterview";
        public const string RequiredHours = "RequiredHours";
        public const string RequiredDays = "RequiredDays";
        public const string EmployeeName = "EmployeeName";
        public const string LeaveType = "LeaveType";
        public const string RecordDate = "RecordDate";
        public const string RecordTime = "RecordTime";
        public const string LogType = "LogType";
        public const string Entrance = "Entrance";
        public const string Exit = "Exit";
        public const string Note = "Note";
        public const string RequestDate = "RequestDate";
        public const string LeaveDuration = "LeaveDuration";
        public const string LeaveStartDate = "LeaveStartDate";
        public const string LeaveEndDate = "LeaveEndDate";
        public const string LeaveCause = "LeaveCause";
        public const string Description = "Description";
        public const string ApproveLeave = "ApproveLeave";
        public const string Balance = "Balance";
        public const string BalanceInformation = "BalanceInformation";
        public const string Granted = "Granted";
        public const string Remain = "Remain";
        public const string PreviousLeavesCount = "PreviousLeavesCount";
        public const string MonthlyBalance = "MonthlyBalance";
        public const string MonthlyGranted = "MonthlyGranted";
        public const string MonthlyRemain = "MonthlyRemain";
        public const string MaximumNumber = "MaximumNumber";
        public const string AddLeaveTemplateMasterAndLeaveTemplateDetail = "AddLeaveTemplateMasterAndLeaveTemplateDetail";

        //UnpaidLeaveRequest
        public const string Calculated = "Calculated";
        public const string Uncalculated = "Uncalculated";

        //HealthyLeaveRequest
        public const string ConsecutiveBalance = "ConsecutiveBalance";
        public const string SeparateBalance = "SeparateBalance";
        public const string ConsecutiveGranted = "ConsecutiveGranted";
        public const string SeparateGranted = "SeparateGranted";
        public const string ConsecutiveRemain = "ConsecutiveRemain";
        public const string SeparateRemain = "SeparateRemain";

        //HourlyLeaveRequest
        public const string BalancePerDay = "BalancePerDay";
        public const string BalancePerMonth = "BalancePerMonth";
        public const string GrantedPerDay = "GrantedPerDay";
        public const string GrantedPerMonth = "GrantedPerMonth";
        public const string RemainPerDay = "RemainPerDay";
        public const string RemainPerMonth = "RemainPerMonth";
        public const string TotalHoursForFullTimeDay = "TotalHoursForFullTimeDay";

        //MaternityLeaveRequest
        public const string FirstChildBalance = "FirstChildBalance";
        public const string SecondChildBalance = "SecondChildBalance";
        public const string ThirdChildBalance = "ThirdChildBalance";
        public const string DeathPeriodPercentage = "DeathPeriodPercentage";
        public const string Attachments = "Attachments";
        public const string Primary = "Primary";


        public const string WorkflowDescriptionForLeaveRequest = "WorkflowDescriptionForLeaveRequest";
        //LeaveApproval
        public const string LeaveApprovalBody = "YouHaveLeaveRequestFor";
        public const string LeaveApprovalSupject = "YouHaveLeaveRequestFor";
        //change balance of leave setting

        public const string YourBalanceLeaves = "YourBalanceLeaves";
        public const string HaveIncreasedTo = "HaveIncreasedTo";

        public const string LeaveCancel = "LeaveCancel";
        public const string LeaveDecrease = "LeaveDecrease";

        public const string StartingDate = "StartingDate";
        public const string CentralAgencyAgreementDate = "CentralAgencyAgreementDate";
        public const string CancellationCause = "CancellationCause";
        public const string DeductionCause = "DeductionCause";

        //MsgShouldDefineWorkFlowSettingAndWorkFlowSettingLeaveBinder
        public const string MsgShouldDefineWorkFlowSettingAndWorkFlowSettingLeaveBinder = "MsgShouldDefineWorkFlowSettingAndWorkFlowSettingLeaveBinder";
        //MsgLeaveNotApprovedUntilNow
        public const string MsgLeaveNotApprovedUntilNow = "MsgLeaveNotApprovedUntilNow";
        //MsgLeaveIsAlreadyCanceled
        public const string MsgLeaveIsAlreadyCanceled = "MsgLeaveIsAlreadyCanceled";
        //MsgStartingDateIsRequired
        public const string MsgStartingDateIsRequired = "MsgStartingDateIsRequired";
        //MsgRequiredDaysIsRequired
        public const string MsgRequiredDaysIsRequired = "MsgRequiredDaysIsRequired";
        //MsgRequiredHoursIsRequired
        public const string MsgRequiredHoursIsRequired = "MsgRequiredHoursIsRequired";
        //MsgCancellationCauseIsRequired
        public const string MsgCancellationCauseIsRequired = "MsgCancellationCauseIsRequired";
        //MsgDeductionCauseIsRequired
        public const string MsgDeductionCauseIsRequired = "MsgDeductionCauseIsRequired";
        //لايمكن أخذ اجازة صحية متصلة , رصيدك السنوي قد نفذ
        public const string MsgYourYearlyConsecutiveBalanceIsnotEnoughCannotTakeConsecutiveHealthyLeave = "MsgYourYearlyConsecutiveBalanceIsnotEnoughCannotTakeConsecutiveHealthyLeave";
        //لايمكن أخذ اجازة صحية منفصلة , رصيدك السنوي قد نفذ
        public const string MsgYourYearlySeperateBalanceIsnotEnoughCannotTakeSeperateHealthyLeave = "MsgYourYearlySeperateBalanceIsnotEnoughCannotTakeSeperateHealthyLeave";
        //لايمكن أخذ اجازة صحية متصلة , رصيدك خلال الخمس سنوات قد نفذ
        public const string MsgYourBalanceDuringFiveYearsIsnotEnoughCannotTakeConsecutiveHealthyLeave = "MsgYourBalanceDuringFiveYearsIsnotEnoughCannotTakeConsecutiveHealthyLeave";
        //لايمكن أخذ اجازة صحية منفصلة , رصيدك خلال الخمس سنوات قد نفذ
        public const string MsgYourBalanceDuringFiveYearsIsnotEnoughCannotTakeSeperateHealthyLeave = "MsgYourBalanceDuringFiveYearsIsnotEnoughCannotTakeSeperateHealthyLeave";
        //Start date of this leave is a holiday, please try again...
        public const string MsgStartDateOfThisLeaveIsHoliday = "MsgStartDateOfThisLeaveIsHoliday";
        //There is a leave with a same date, please try again...
        public const string MsgThereIsLeaveWithSameDate = "MsgThereIsLeaveWithSameDate";
        //The difference between request date and start date is smaller than number of interval days in an administrative leave setting, please try again...
        public const string MsgDifferenceBetweenRequestDateAndStartDateSmallerThanIntervalDays = "MsgDifferenceBetweenRequestDateAndStartDateSmallerThanIntervalDays";
        public const string MsgEmployeeCardNotExist = "MsgEmployeeCardNotExist";
        public const string MsgLeaveSettingNotExist = "MsgLeaveSettingNotExist";
        public const string MsgDisciplinarySettingNotExist = "MsgDisciplinarySettingNotExist";
        public const string MsgGeneralSettingNotExist = "MsgGeneralSettingNotExist";
        public const string MsgRewardSettingNotExist = "MsgRewardSettingNotExist";
        //You can't add this leave beacuase Request Date is greater than Start Date, please try again...
        public const string MsgRequestDateIsGreaterThanStartDate = "MsgRequestDateIsGreaterThanStartDate";
        //You can't add this leave beacuase Required Days is greater than remain balance, please try again...
        public const string MsgRequiredDaysIsGreaterThanRemainBalance = "MsgRequiredDaysIsGreaterThanRemainBalance";
        //Employee Age Is Required
        public const string MsgEmployeeAgeIsRequired = "MsgEmployeeAgeIsRequired";
        //Minimum Years Is Required
        public const string MsgMinimumYearsIsRequired = "MsgMinimumYearsIsRequired";
        //Maximum Years Is Required
        public const string MsgMaximumYearsIsRequired = "MsgMaximumYearsIsRequired";
        //Forced Due Balance Must Be Less Than Due Balance
        public const string MsgForcedDueBalanceMustBeLessThanDueBalance = "MsgForcedDueBalanceMustBeLessThanDueBalance";
        //TheEmployeeWhoYouSelectIsResignedAndYouCanNotToAddRecycleLeaveToHim
        public const string TheEmployeeWhoYouHaveSelectedIsResignedOrTerminated = "TheEmployeeWhoYouHaveSelectedIsResignedOrTerminated";
        //TheEmployeeStartWorkingDateIsGraterThanYear
        public const string TheEmployeeStartWorkingDateIsGraterThanYear = "TheEmployeeStartWorkingDateIsGraterThanYear";
        //Forced Due Balance For Mothers Must Be Less Than Due Balance
        public const string MsgForcedDueBalanceForMothersMustBeLessThanDueBalance = "MsgForcedDueBalanceForMothersMustBeLessThanDueBalance";
        //This is not a valid year of services, please try again...
        public const string MsgThisIsNotValidYearOfServices = "MsgThisIsNotValidYearOfServices";
        //You can't beacuase Start Date is greater than today, please try again...
        public const string MsgStartDateisGreaterThanToday = "MsgStartDateisGreaterThanToday";
        //This is not a valid holiday date, please try again...
        public const string MsgThisIsNotValidHolidayDate = "MsgThisIsNotValidHolidayDate";
        public const string MsgThisHolidyAlreadyExists = "MsgThisHolidyAlreadyExists";
        //You can't add more setting of this leave , it should be once...
        public const string MsgCannotAddMoreSettingOfThisLeaveItShouldBeOnce = "MsgCannotAddMoreSettingOfThisLeaveItShouldBeOnce";
        public const string MsgShouldAddSettingForThisLeave = "MsgShouldAddSettingForThisLeave";
        //This is not a valid fixed holiday because the count of all holiday days will be greater than the count of year days, please try again...
        public const string MsgHolidayDaysGreaterThanCountOfYearDays = "MsgHolidayDaysGreaterThanCountOfYearDays";
        //You don't have an administrative leave so this leave will record as unpaid leave, if you agree press save.
        public const string MsgYouDontHaveAdministrativeLeaves = "MsgYouDontHaveAdministrativeLeaves";
        //You can't add this leave beacuase Required Hours is greater than remain hours (" + remainHours  + "), please try again...
        public const string MsgRequiredHoursGreaterThanRemainHours = "MsgRequiredHoursGreaterThanRemainHours";
        //You can't add this leave beacuase Required Hours is greater than remain hours, please try again...
        public const string MsgRequiredHoursIsGreaterThanRemainHours = "MsgRequiredHoursIsGreaterThanRemainHours";
        //You can't add this leave beacuase Required Hours is greater than remain balance per month, please try again...
        public const string MsgRequiredHoursIsGreaterThanRemainHoursPerMonth = "MsgRequiredHoursIsGreaterThanRemainHoursPerMonth";
        //"You can't take a leave, your employment period is less than " + leaveSetting.EmploymentPeriodGreaterThan,
        public const string MsgCannotTakeLeaveYourEmploymentPeriodIsLessThanLeaveSetting = "MsgCannotTakeLeaveYourEmploymentPeriodIsLessThanLeaveSetting";
        //Request Date Year Is Not Same Current Year
        public const string MsgRequestDateYearIsNotSameCurrentYear = "MsgRequestDateYearIsNotSameCurrentYear";
        //Start Date Year Is Not Same Current Year
        public const string MsgStartDateYearIsNotSameCurrentYear = "MsgStartDateYearIsNotSameCurrentYear";
        //MsgEndDateYearIsNotSameCurrentYear
        public const string MsgEndDateYearIsNotSameCurrentYear = "MsgEndDateYearIsNotSameCurrentYear";
        //You Are Allowed To Have Only One Pilgrimage Leave
        public const string MsgYouAreAllowedToHaveOnlyOnePilgrimageLeave = "MsgYouAreAllowedToHaveOnlyOnePilgrimageLeave";
        //MsgThisLeaveAllowedJustForThreeChildren
        public const string MsgThisLeaveAllowedJustForThreeChildren = "MsgThisLeaveAllowedJustForThreeChildren";
        //This Leave Just For Married Women
        public const string MsgThisLeaveJustForMarriedWomen = "MsgThisLeaveJustForMarriedWomen";
        //MsgThisLeaveAlreadyTakenForThisChild
        public const string MsgThisLeaveAlreadyTakenForThisChild = "MsgThisLeaveAlreadyTakenForThisChild";
        //This Is Not a Regular Date
        public const string MsgThisIsNotRegularDate = "MsgThisIsNotRegularDate";
        //Should Define How Many Days For Additional Maternity In Maternity Leave Setting
        public const string MsgShouldDefineHowManyDaysForAdditionalMaternityInMaternityLeaveSetting = "MsgShouldDefineHowManyDaysForAdditionalMaternityInMaternityLeaveSetting";
        //MsgCannotAddMoreAdministrativeProfile
        public const string MsgCannotAddMoreAdministrativeProfile = "MsgCannotAddMoreAdministrativeProfile";
        //MsgCannotAddMoreRecycleForThisLeaveSettingInYear{0}
        public const string MsgCannotAddMoreRecycleForThisLeaveSettingInYear = "MsgCannotAddMoreRecycleForThisLeaveSettingInYear";
        public const string YouMustDeleteRecycleLeaveBalanceFromNewestToOldest = "YouMustDeleteRecycleLeaveBalanceFromNewestToOldest";
        //MsgYouCanNotTakeLeaveBecauseEmployee{0}IsNotOnHeadOfHisWork
        public const string MsgYouCanNotTakeLeaveBecauseEmployeeIsNotOnHeadOfHisWork = "MsgYouCanNotTakeLeaveBecauseEmployeeIsNotOnHeadOfHisWork";
        //MsgYouCanNotRecycleThisLeaveBecauseRoundPercentageIsZero
        public const string MsgYouCanNotRecycleThisLeaveBecauseRoundPercentageIsZero = "MsgYouCanNotRecycleThisLeaveBecauseRoundPercentageIsZero";
        //MsgStartDateIsRequired
        public const string MsgStartDateIsRequired = "MsgStartDateIsRequired";
        //MsgPositionSeparationDateIsRequired
        public const string MsgPositionSeparationDateIsRequired = "MsgPositionSeparationDateIsRequired";
        //MsgPositionJoiningDateIsRequired
        public const string MsgPositionJoiningDateIsRequired = "MsgPositionJoiningDateIsRequired";
        //MsgDisciplinaryDateIsRequired
        public const string MsgDisciplinaryDateIsRequired = "MsgDisciplinaryDateIsRequired";
        //MsgRewardDateIsRequired
        public const string MsgRewardDateIsRequired = "MsgRewardDateIsRequired";
        //MsgEndDateIsRequired
        public const string MsgEndDateIsRequired = "MsgEndDateIsRequired";
        //MsgRequestDateIsRequired
        public const string MsgRequestDateIsRequired = "MsgRequestDateIsRequired";
        //MsgEndDateShouldBeGreaterThanOrEqualStartDate
        public const string MsgEndDateShouldBeGreaterThanOrEqualStartDate = "MsgEndDateShouldBeGreaterThanOrEqualStartDate";
        //MsgFromTimeIsRequired
        public const string MsgFromTimeIsRequired = "MsgFromTimeIsRequired";
        //MsgToTimeIsRequired
        public const string MsgToTimeIsRequired = "MsgToTimeIsRequired";
        //MsgToTimeShouldBeGreaterThanFromTime
        public const string MsgToTimeShouldBeGreaterThanFromTime = "MsgToTimeShouldBeGreaterThanFromTime";
        //MsgRequiredHours{0}IsGreaterThanAllowedHoursPerDay{1}
        public const string MsgRequiredHoursIsGreaterThanAllowedHoursPerDay = "MsgRequiredHoursIsGreaterThanAllowedHoursPerDay";
        //MsgSorryYouPassedMaximumNumberForThisLeave
        public const string MsgSorryYouPassedMaximumNumberForThisLeave = "MsgSorryYouPassedMaximumNumberForThisLeave";
        //MsgMonthlyBalanceShouldBeGreaterThanZero
        public const string MsgMonthlyBalanceShouldBeGreaterThanZero = "MsgMonthlyBalanceShouldBeGreaterThanZero";
        public const string MsgThereIsBalanceSliceInSameRange = "MsgThereIsBalanceSliceInSameRange";
        public const string MsgThereIsPaidSliceInSameRange = "MsgThereIsPaidSliceInSameRange";
        //MsgMaximumNumberShouldBeGreaterThanZero
        public const string MsgMaximumNumberShouldBeGreaterThanZero = "MsgMaximumNumberShouldBeGreaterThanZero";
        //MsgMaximumHoursPerDayShouldBeGreaterThanZero
        public const string MsgMaximumHoursPerDayShouldBeGreaterThanZero = "MsgMaximumHoursPerDayShouldBeGreaterThanZero";
        //MsgHoursEquivalentToOneLeaveDayShouldBeGreaterThanZero
        public const string MsgHoursEquivalentToOneLeaveDayShouldBeGreaterThanZero = "MsgHoursEquivalentToOneLeaveDayShouldBeGreaterThanZero";
        public const string MsgYouDoNotHaveEnoughBalanceTheRemainDaysIsGreaterThanTheRequiredDays = "MsgYouDoNotHaveEnoughBalanceTheRemainDaysIsGreaterThanTheRequiredDays";
        //MsgMaximumHoursPerDayShouldBeSmallerThanHoursEquivalentToOneLeaveDay
        public const string MsgMaximumHoursPerDayShouldBeSmallerThanHoursEquivalentToOneLeaveDay = "MsgMaximumHoursPerDayShouldBeSmallerThanHoursEquivalentToOneLeaveDay";
        //MsgYouDoNotHaveEnoughBalanceTheRemainDaysIs{0}AndTheRequiredDaysIs{1}
        public const string MsgYouDoNotHaveEnoughBalanceTheRemainDaysIsAndTheRequiredDaysIs = "MsgYouDoNotHaveEnoughBalanceTheRemainDaysIsAndTheRequiredDaysIs";
        public const string MsgPleaseSeprateTheLeaveInTwoLeavesEveryOneInDifferentYear = "PleaseSeprateTheLeaveInTwoLeavesEveryOneInDifferentYear";
        //MsgYouDoNotHaveEnoughMonthlyBalanceTheRemainMonthlyDaysIs{0}AndTheRequiredDaysIs{1}
        public const string MsgYouDoNotHaveEnoughMonthlyBalanceTheRemainMonthlyDaysIsAndTheRequiredDaysIs = "MsgYouDoNotHaveEnoughMonthlyBalanceTheRemainMonthlyDaysIsAndTheRequiredDaysIs";
        public const string YouHaveExceededMaximumHoursPerDay = "YouHaveExceededMaximumHoursPerDay";

        //MsgYouCanNotCheckHasMonthlyBalanceAndIsIndivisibleTogether
        public const string MsgYouCanNotCheckHasMonthlyBalanceAndIsIndivisibleTogether = "MsgYouCanNotCheckHasMonthlyBalanceAndIsIndivisibleTogether";
        //MsgYouCanNotCheckIsIndivisibleAndIsDivisibleToHoursTogether
        public const string MsgYouCanNotCheckIsIndivisibleAndIsDivisibleToHoursTogether = "MsgYouCanNotCheckIsIndivisibleAndIsDivisibleToHoursTogether";
        //MsgCannotAddThisLeaveAgainItShouldBeOnce
        public const string MsgCannotAddThisLeaveAgainItShouldBeOnce = "MsgCannotAddThisLeaveAgainItShouldBeOnce";

        public const string DisciplinaryRequest = "DisciplinaryRequest";
        public const string TerminationRequest = "TerminationRequest";
        public const string ResignationRequest = "ResignationRequest";
        public const string PromotionRequest = "PromotionRequest";
        public const string FinancialPromotionRequest = "FinancialPromotionRequest";
        public const string RewardRequest = "RewardRequest";
        public const string EmployeeLeaveRequest = "EmployeeLeaveRequest";
        public const string EntranceExitRequest = "EntranceExitRequest";
        public const string MissionRequest = "MissionRequest";
        public const string HourlyMission = "HourlyMission";
        public const string TravelMission = "TravelMission";

        public const string DisciplinarySetting = "DisciplinarySetting";
        public const string DisciplinaryDate = "DisciplinaryDate";
        public const string DisciplinaryReason = "DisciplinaryReason";
        public const string Comment = "Comment";
        public const string DisciplinarySettingName = "DisciplinarySettingName";

        public const string RewardSetting = "RewardSetting";
        public const string RewardDate = "RewardDate";
        public const string RewardReason = "RewardReason";
        public const string RewardSettingName = "RewardSettingName";

        public const string LeaveSetting = "LeaveSetting";
        public const string StartDate = "StartDate";
        public const string EndDate = "EndDate";
        public const string IsHourlyLeave = "IsHourlyLeave";
        public const string IsHourlyMission = "IsHourlyMission";
        public const string FromTime = "FromTime";
        public const string ToTime = "ToTime";
        public const string SpentDays = "SpentDays";
        public const string LeaveReason = "LeaveReason";

        public const string LastWorkingDate = "LastWorkingDate";
        public const string TerminationReason = "TerminationReason";

        public const string ResignationReason = "ResignationReason";
        public const string NoticeStartDate = "NoticeStartDate";
        public const string NoticeEndDate = "NoticeEndDate";

        public const string PositionSeparationDate = "PositionSeparationDate";
        public const string PositionJoiningDate = "PositionJoiningDate";
        public const string PromotionReason = "PromotionReason";
        public const string NewPosition = "NewPosition";
        public const string NewJobTitle = "NewJobTitle";
        public const string Position = "Position";
        public const string JobTitle = "JobTitle";
        public const string CurrentPosition = "CurrentPosition";

        public const string IsPercentage = "IsPercentage";
        public const string FixedValue = "FixedValue";
        public const string Percentage = "Percentage";
        public const string FinancialPromotionReason = "FinancialPromotionReason";

        public const string DisciplinaryNotifyBody = "YouHaveDisciplinaryRequestFor";
        public const string DisciplinaryNotifySubject = "DisciplinaryNotifySubject";

        public const string DisciplinaryApprovalBody = "DisciplinaryApprovalBody";
        public const string DisciplinaryApprovalSubjectFor = "DisciplinaryApprovalSubjectFor";

        public const string RewardApprovalBody = "YouHaveRewardRequestFor";
        public const string RewardApprovalSubjectFor = "RewardApprovalSubjectFor";

        public const string TerminationApprovalBody = "YouHaveTerminationRequestFor";
        public const string TerminationApprovalSubjectFor = "TerminationApprovalSubjectFor";

        public const string ResignationApprovalBody = "YouHaveResignationRequestFor";
        public const string ResignationApprovalSubjectFor = "ResignationApprovalSubjectFor";

        public const string PromotionApprovalBody = "YouHavePromotionRequestFor";
        public const string PromotionApprovalSubjectFor = "PromotionApprovalSubjectFor";

        public const string FinancialPromotionApprovalBody = "YouHaveFinancialPromotionRequestFor";
        public const string FinancialPromotionApprovalSubjectFor = "FinancialPromotionApprovalSubjectFor";
        public const string TotalWeightGreaterThan100 = "TotalWeightGreaterThan100";

        public const string TheEmployeeWhoYouHaveSelectedIsNew = "TheEmployeeWhoYouHaveSelectedIsNew";
        public const string EmployeeRelationServicesDashboard = "EmployeeRelationServicesDashboard";
        public const string EmployeeLifeCycle = "EmployeeLifeCycle";
        public const string AdministrativeLevelDashboardForEmployeeRelationsServices =
            "AdministrativeLevelDashboardForEmployeeRelationsServices";

        public const string EmployeeLeavesPerMonth = "EmployeeLeavesPerMonth";
        public const string EmployeeLeavesPerYear = "EmployeeLeavesPerYear";
        public const string LeavesPerYear = "LeavesPerYear";

        public const string Leaves = "Leaves";
        public const string Taken = "Taken";
        public const string LeavesCount = "LeavesCount";
        public const string EntranceExitRecordRequestApprovalBody = "EntranceExitRecordRequestApprovalBody";
        public const string MissionRequestApprovalBody = "MissionRequestApprovalBody";
        public const string MissionRequestApprovalSupject = "MissionRequestApprovalSupject";
        public const string EntranceExitRecordRequestApprovalSupject = "EntranceExitRecordRequestApprovalSupject";
        public const string EntranceExitRecordRequest = "EntranceExitRecordRequest";
        public const string MsgWorkFlowSettingsNotExist = "MsgWorkFlowSettingsNotExist";
        public const string EntranceExitRecordAlreadyExist = "EntranceExitRecordAlreadyExist";
        public const string MissionAlreadyExistInTheSamePeriod = "MissionAlreadyExistInTheSamePeriod";
        public const string MsgEmployeeIsNotOnHeadOfHisWork = "MsgEmployeeIsNotOnHeadOfHisWork";
        public const string MissionEndTimeMustBeGreaterThanStartTime = "MissionEndTimeMustBeGreaterThanStartTime";
        public const string MissionEndDateMustBeGreaterThanStartDate = "MissionEndDateMustBeGreaterThanStartDate";


        public static string GetResource(string key)
        {
            var result = ServiceFactory.LocalizationService.GetResource(ResourceGroupName + "_" + key);
            return string.IsNullOrEmpty(result) ? key.ToCapitalLetters() : result;
        }

        public static string GetResource(string key,int locale)
        {
            var result = ServiceFactory.LocalizationService.GetResource(ResourceGroupName + "_" + key);
            return string.IsNullOrEmpty(result) ? key.ToCapitalLetters() : result;
        }
    }
}