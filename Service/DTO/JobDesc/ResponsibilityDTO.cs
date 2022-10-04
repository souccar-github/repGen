using System.Collections.Generic;
using System.Text;
using HRIS.Domain.JobDesc.ValueObjects;

namespace Service.DTO.JobDesc
{
    public class ResponsibilityDTO
    {
        public ResponsibilityDTO(Responsibility responsibility)
        {
            Description = responsibility.Description;
            Weight = responsibility.Weight;
            ResponsibilityKpis = ConvertKpisToString(responsibility.ResponsibilityKpis);
        }

        public string Description { get; set; }
        public float Weight { get; set; }
        public string ResponsibilityKpis { get; set; }

        private static string ConvertKpisToString(IEnumerable<ResponsibilityKpi> responsibilityKpis)
        {
            var builder = new StringBuilder();
            foreach (ResponsibilityKpi responsibilityKpi in responsibilityKpis)
                builder.AppendLine(string.Format("{0},{1}", responsibilityKpi.Description, responsibilityKpi.Value));
            return builder.ToString();
        }
    }
}