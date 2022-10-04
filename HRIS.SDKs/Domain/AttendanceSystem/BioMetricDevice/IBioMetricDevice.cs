using System.Collections.Generic;

namespace HRIS.SDKs.Domain.AttendanceSystem.BioMetricDevice
{
    public interface IBioMetricDevice
    {
        int Handle { get; set; }
        void Connect(string ipAddress, int port);
        List<BioMetricRecordData> GetRecordsData();
        void ClearRecordsData();
        int GetRecordsCount();
        BioMetricRecordType GetBioMetricRecordType(ushort deviceType);
    }
}
