using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRIS.Domain.AttendanceSystem.Enums;
using HRIS.Domain.AttendanceSystem.RootEntities;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.SDKs.Domain.AttendanceSystem.BioMetricDevice;
using  Project.Web.Mvc4.App_Start;
using Souccar.Core.Extensions;
using  Project.Web.Mvc4.Extensions;
using Souccar.Infrastructure.Services.Sys;
using Souccar.Infrastructure.Extenstions;
using  Project.Web.Mvc4.Areas.AttendanceSystem.Services;
using  Project.Web.Mvc4.Helpers.DomainExtensions;
using Souccar.Domain.DomainModel;
using  Project.Web.Mvc4.Areas.EmployeeRelationServices.Helper;
using Souccar.Infrastructure.Core;
using HRIS.Domain.AttendanceSystem.Configurations;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace Project.Web.Mvc4.Areas.AttendanceSystem.Controllers
{
    public class BioMetricInteractionController : Controller
    {
        public ActionResult Index()
        {
            return PartialView();
        }

        //  اعادة كافة الاجهزة التي يدعمها النظام
        [HttpPost]
        public ActionResult GetSupportedBioMetricDevices()
        {
            var bioMetricDevices = typeof(BioMetricSetting).GetAll<BioMetricSetting>().Where(x => x.IsActive);
            var result = new ArrayList();

            foreach (var item in bioMetricDevices)
            {
                var temp = new Dictionary<string, object>();
                temp["Id"] = item.Id;
                temp["Name"] = item.Name;
                result.Add(temp);
            }
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }


        //  تحديث كافة الاجهزة التي يدعمها النظام
        [HttpPost]
        public ActionResult SyncSupportedBioMetricDevices()
        {
            try
            {
                var supportedDevicesKeys = BioMetricService.GetSupportedDevicesKeys();
                var currentDevicesList = typeof(BioMetricDevice).GetAll<BioMetricDevice>().ToList();
                var deletedDevicesList = currentDevicesList.Where(x => supportedDevicesKeys.All(y => x.DeviceTypeFullName != y));
                var addedDevicesList = supportedDevicesKeys.Where(x => currentDevicesList.All(y => y.DeviceTypeFullName != x));

                foreach (var deletedDevice in deletedDevicesList)
                {
                    var device = deletedDevice;
                    var usedDevices = typeof(BioMetricSetting).GetAll<BioMetricSetting>().Where(x => x.BioMetricDevice.Id == device.Id);
                    foreach (var usedDevice in usedDevices)
                    {
                        usedDevice.Delete();
                    }
                    deletedDevice.Delete();
                }

                foreach (var newDevice in addedDevicesList)
                {
                    var bioMetricDevice = new BioMetricDevice
                    {
                        DeviceTypeFullName = newDevice,
                        Name = newDevice.Split('.').Last()
                    };
                    bioMetricDevice.Save();
                }

                return Json(new
                {
                    Success = true,
                    Msg = Helpers.GlobalResource.DoneMessage
                });
            }
            catch
            {
                return Json(new
                {
                    Success = false,
                    Msg = Helpers.GlobalResource.FailMessage
                });
            }
        }


        // تنفيذ العمليات على جهاز البصمة
        [HttpPost]
        public ActionResult PerformBioMetricInteraction(int bioMetricDeviceId, bool transferDataFromBioMetric, bool clearDataFromBioMetricTitle)
        {
            try
            {
                var bioMetricSetting = (BioMetricSetting)typeof(BioMetricSetting).GetById(bioMetricDeviceId);

                var bioMetricDevice = BioMetricService.GetDevice(bioMetricSetting.BioMetricDevice.DeviceTypeFullName);
                bioMetricDevice.Connect(bioMetricSetting.IpAddress, bioMetricSetting.Port);
                var employeeAttendanceCards = typeof(EmployeeCard).GetAll<EmployeeCard>();
                if (transferDataFromBioMetric)
                {
                    var data = bioMetricDevice.GetRecordsData();
                    foreach (var bioMetricRecordData in data)
                    {
                        var empAttendanceCard = employeeAttendanceCards.FirstOrDefault(x => x.EmployeeMachineCode == bioMetricRecordData.UserDeviceId.ToString());
                        if (empAttendanceCard == null)
                        {
                            continue; // todo Mhd Alsaadi: للتأكد هل سيتم تجاهل الريكورد الذي لم نجد له مقابل ببطاقات الدوام سواء لعدم وجود البطاقة او لعدم وجود كود الجهاز
                        }
                        var errorType = ErrorType.None;// GetEntranceExitRecordErrorType(bioMetricRecordData, data);

                        if (!AttendanceService.CheckEntranceExitRecordDuplicate(empAttendanceCard.Employee, bioMetricRecordData.DateTime, InsertSource.Machine, ConvertBioMetricRecordTypeToMaestroRecordType(bioMetricRecordData.RecordType), bioMetricSetting.IgnorePeriod))
                        {
                            var entranceExitRecord = new EntranceExitRecord();
                            var currentUser = UserExtensions.CurrentUser;
                            var entities = new List<IAggregateRoot>();
                            var fingerprintTransferredData = new FingerprintTransferredData();

                            entranceExitRecord.Employee = empAttendanceCard.Employee;
                            //entranceExitRecord.ErrorMessage = "";
                            entranceExitRecord.ErrorType = errorType;
                            entranceExitRecord.InsertSource = InsertSource.Machine;
                            entranceExitRecord.LogDateTime = bioMetricRecordData.DateTime;
                            entranceExitRecord.LogTime = new DateTime(2000,1,1, bioMetricRecordData.DateTime.Hour,bioMetricRecordData.DateTime.Minute, bioMetricRecordData.DateTime.Second);
                            entranceExitRecord.LogDate = new DateTime(bioMetricRecordData.DateTime.Year,bioMetricRecordData.DateTime.Month,bioMetricRecordData.DateTime.Day,0,0,0);
                            entranceExitRecord.LogType = ConvertBioMetricRecordTypeToMaestroRecordType(bioMetricRecordData.RecordType);
                            entranceExitRecord.Note = "";
                            //entranceExitRecord.Status = errorType == ErrorType.None ? EntranceExitStatus.Ok : EntranceExitStatus.Error;
                            entranceExitRecord.UpdateReason = "";

                            entities.Add(entranceExitRecord);

                            fingerprintTransferredData.EntranceExitRecord = entranceExitRecord;
                            fingerprintTransferredData.LogDateTime = bioMetricRecordData.DateTime;
                            fingerprintTransferredData.LogType = ConvertBioMetricRecordTypeToMaestroRecordType(bioMetricRecordData.RecordType);
                            fingerprintTransferredData.Employee = empAttendanceCard.Employee;

                            entities.Add(fingerprintTransferredData);

                            ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser);
                        }
                    }
                }

                if (clearDataFromBioMetricTitle)
                {
                    bioMetricDevice.ClearRecordsData(); 
                    var fingerprintTransferredDatas = ServiceFactory.ORMService.All<FingerprintTransferredData>();
                    foreach (var fingerprintTransferredData in fingerprintTransferredDatas)
                    {
                        fingerprintTransferredData.Delete();
                    }
                }
            }
            catch
            {
                return Json(new
                {
                    Success = false,
                    Msg = Helpers.GlobalResource.FailMessage
                });
            }
            return Json(new
            {
                Success = true,
                Msg = Helpers.GlobalResource.DoneMessage
            });
        }

        private LogType ConvertBioMetricRecordTypeToMaestroRecordType(BioMetricRecordType bioMetricRecordType)
        {
            if (bioMetricRecordType == BioMetricRecordType.In || bioMetricRecordType == BioMetricRecordType.InMission || bioMetricRecordType == BioMetricRecordType.NotSupported)
            {
                return LogType.Entrance;
            }
            if (bioMetricRecordType == BioMetricRecordType.Out || bioMetricRecordType == BioMetricRecordType.OutMission)
            {
                return LogType.Exit;
            }
            return LogType.Entrance;
        }


        public static ErrorType GetEntranceExitRecordErrorType(BioMetricRecordData record, List<BioMetricRecordData> recordsData)
        {
            //قائمة بسجلات الدخول و الخروج لليوم المدخل للموظف
            List<BioMetricRecordData> recordsInThisDay = new List<BioMetricRecordData>();

            foreach (var recordData in recordsData)
            {
                if (recordData.DateTime.Year == record.DateTime.Year
                    && recordData.DateTime.Month == record.DateTime.Month
                    && recordData.DateTime.Day == record.DateTime.Day
                    && recordData.UserDeviceId == record.UserDeviceId)
                {
                    recordsInThisDay.Add(recordData);
                }
            }
            if (recordsInThisDay.Count != 0)
            {
                var recordsInThisDayRecurrence = 0;
                var recordRecurrence = 0;

                //لمعرفة ترتيب التسجيلة في القائمة 
                foreach (var recordInThisDay in recordsInThisDay)
                {
                    if (recordInThisDay.DateTime == record.DateTime)
                    {
                        recordRecurrence = recordsInThisDayRecurrence;
                    }
                    recordsInThisDayRecurrence++;
                }

                // في حال لا تحوي القائمة الا على تسجيلة وحيدة
                if (recordsInThisDay.Count() == 1)
                {
                    if (recordsInThisDay[0].RecordType == BioMetricRecordType.In
                    || recordsInThisDay[0].RecordType == BioMetricRecordType.InMission)
                        return ErrorType.EntranceWithoutExit;
                    if (recordsInThisDay[0].RecordType == BioMetricRecordType.Out
                    || recordsInThisDay[0].RecordType == BioMetricRecordType.OutMission)
                        return ErrorType.ExitWithoutEntrance;
                }

                //في حال كانت التسجيلة في بداية القائمة
                if (recordRecurrence == 0
                    && (recordsInThisDay[recordRecurrence].RecordType == BioMetricRecordType.Out
                    || recordsInThisDay[recordRecurrence].RecordType == BioMetricRecordType.OutMission))
                    return ErrorType.ExitWithoutEntrance;

                //في حال كانت التسجيلة في نهاية القائمة
                if (recordRecurrence == recordsInThisDay.Count
                    && (recordsInThisDay[recordRecurrence].RecordType == BioMetricRecordType.In
                    || recordsInThisDay[recordRecurrence].RecordType == BioMetricRecordType.InMission))
                    return ErrorType.EntranceWithoutExit;

                //في حال كانت التسجيلة في منتصف القائمة و التسجيلة دخول
                if (recordRecurrence > 0
                    && recordRecurrence < recordsInThisDay.Count
                    && (recordsInThisDay[recordRecurrence].RecordType == BioMetricRecordType.In
                    || recordsInThisDay[recordRecurrence].RecordType == BioMetricRecordType.InMission))
                {
                    if (recordsInThisDay[recordRecurrence - 1].RecordType == BioMetricRecordType.In
                    || recordsInThisDay[recordRecurrence - 1].RecordType == BioMetricRecordType.InMission)
                    {
                        return ErrorType.MultipleEntrance;
                    }
                    if (recordRecurrence + 1 < recordsInThisDay.Count())
                    {
                        if (recordsInThisDay[recordRecurrence + 1].RecordType == BioMetricRecordType.In
                        || recordsInThisDay[recordRecurrence + 1].RecordType == BioMetricRecordType.InMission)
                        {
                            return ErrorType.EntranceWithoutExit;
                        }

                        if ((recordsInThisDay[recordRecurrence + 1].RecordType == BioMetricRecordType.Out
                        || recordsInThisDay[recordRecurrence + 1].RecordType == BioMetricRecordType.OutMission)
                            && recordsInThisDay[recordRecurrence].DateTime > recordsInThisDay[recordRecurrence + 1].DateTime)
                        {
                            return ErrorType.EntranceTimeGreaterThanExitTime;
                        }
                    }
                }

                //في حال كانت التسجيلة في منتصف القائمة و خروج
                if (recordRecurrence > 0
                    && recordRecurrence < recordsInThisDay.Count
                    && (recordsInThisDay[recordRecurrence].RecordType == BioMetricRecordType.Out
                    || recordsInThisDay[recordRecurrence].RecordType == BioMetricRecordType.OutMission))
                {
                    if (recordsInThisDay[recordRecurrence - 1].RecordType == BioMetricRecordType.Out
                    || recordsInThisDay[recordRecurrence - 1].RecordType == BioMetricRecordType.OutMission)
                    {
                        return ErrorType.MultipleExit;
                    }
                    if (recordRecurrence + 1 < recordsInThisDay.Count())
                    {
                        if (recordsInThisDay[recordRecurrence + 1].RecordType == BioMetricRecordType.Out
                        || recordsInThisDay[recordRecurrence + 1].RecordType == BioMetricRecordType.OutMission)
                        {
                            return ErrorType.ExitWithoutEntrance;
                        }
                    }
                }

            }
            return ErrorType.None;
        }


        // فحص الاتصال بالالة
        [HttpPost]
        public ActionResult CheckDeviceStatus(int bioMetricDeviceId)
        {
            var msg = "Checked! ,";            
            var bioMetricSetting = (BioMetricSetting)typeof(BioMetricSetting).GetById(bioMetricDeviceId);
            var bioMetricDevice = BioMetricService.GetDevice((bioMetricSetting.BioMetricDevice.DeviceTypeFullName).ToString());
            var ipAddress = bioMetricSetting.IpAddress;
            var port = bioMetricSetting.Port;
            var Success = true;

            //Device support
            if (bioMetricDevice != null)
            {
                msg += "This device is supported, ";
                Success = false;
            }
            if (bioMetricDevice == null)
            {
                msg += "This device is Not supported, ";
                Success = false;
            }
            //Ping
            try
            {
                TcpClient client = new TcpClient(ipAddress, port);
                msg += "Ping Success, ";
            }
            catch (Exception ex)
            {
                msg += "Ping Failed, ";
                Success = false;
            }
            //connect
            if (bioMetricDevice != null)
            {
                try
                {
                    bioMetricDevice.Connect(ipAddress, port);
                    msg += "Connect Success, ";
                }
                catch (Exception)
                {
                    msg += "Connect Failed, ";
                    Success = false;
               
                }
            }


            ////BSSDK
            //var result = BSSDK.BS_InitSDK();
            //if (result != BSSDK.BS_SUCCESS)
            //{
            //    msg += "BSSDK.BS_InitSDK() Failed, ";
            //    Success = false;
            //}

            //var handle = 0;
            //result = BSSDK.BS_OpenSocket(ipAddress, port, ref handle);
            //if (result != BSSDK.BS_SUCCESS)
            //{
            //    msg += "BSSDK.BS_InitSDK() Failed, ";
            //    Success = false;
            //}

         
            return Json(new
            {
                Success = Success,
                Msg = msg
            });
        }
    }    
  
}




