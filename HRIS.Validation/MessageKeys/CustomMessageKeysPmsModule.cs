//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//*******company name: souccar for electronic industries*******//
//project manager:
//supervisor:
//author: Ammar Alziebak
//description:
//start date:
//end date:
//last update:
//update by:
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Validation.MessageKeys
{
    public static class CustomMessageKeysPmsModule
    {
        public const string TotalSumWeight = "TotalSumWeightLessthen100";
        public const string ResourceGroupName = "CustomMessageKeysPmsModule";
        public const string totalWeightRange = "TotalWeightRangeBetween0-100";

        public static string GetFullKey(string key)
        {
            return ResourceGroupName + "_" + key;
        }
    }
}
