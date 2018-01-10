using System.Collections.Generic;

namespace AzR.Core.ModelConfig
{
    public class RadioButtonHelper
    {
        public static readonly Dictionary<string, bool> YesNo = new Dictionary<string, bool>
        {
            { "Yes",true},
             { "No",false}
        };
        public static readonly Dictionary<string, bool> OrgUser = new Dictionary<string, bool>
        {
            { "Org",true},
             { "User",false}
        };

        public static readonly Dictionary<string, bool> ActiveInActive = new Dictionary<string, bool>
        {
            {"Active", true},
            {"InActive", false}
        };
        public static readonly Dictionary<string, bool> FixedPercentage = new Dictionary<string, bool>
        {
            {"Fixed", true},
            {"Percentage", false}
        };

    }
}
