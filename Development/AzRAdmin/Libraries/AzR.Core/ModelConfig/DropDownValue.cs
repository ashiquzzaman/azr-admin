using System;
using System.Collections.Generic;
using System.Linq;

namespace AzR.Core.ModelConfig
{
    public static class DropDownValue
    {
        public static readonly List<string> BloodgroupList = new List<string>
        {
            "A+",
            "B+",
            "O+",
            "AB+",
            "A-",
            "B-",
            "O-",
            "AB-"
        };

        public static readonly List<string> ReligionList = new List<string>
        {
            "Islam",
            "Hindu"
        };

        public static readonly List<string> GenderList = new List<string>
        {
            "Male",
            "Female"
        };
        public static readonly List<string> CategoryTypeList = new List<string>
        {
            "POST",
            "TODO"
        };

        public static readonly List<int> PassingYearList = Enumerable.Range(1971, DateTime.UtcNow.Year - 1971 + 1).ToList().OrderByDescending(v => v).ToList();

        public static readonly List<string> MaritalStatusList = new List<string>
        {
            "Unmarried",
            "Married"
        };


        public static readonly List<string> InfoTypeList = new List<string>
        {   "Notice",
            "News",
            "User Guide",
            "Documentation",
            "About",
            "Contact",
            "Address",
            "Feature"
        };
        public static readonly List<string> FaqTypeList = new List<string>
        {
            "Help",
            "Faqs",
            "Question",
            "Feedback",
            "Report",
            "Answer"
        };
        public static readonly List<string> AccessPropertyList = new List<string>
        {
            "Public",
            "Privete"
        };

        public static readonly List<string> UserList = new List<string>
        {
            "Employee",
            "Party",
            "User"
        };
        public static readonly List<string> InstituteList = new List<string>
        {
            "Owner",
            "Developer",
            "Others"
        };

        public static readonly List<string> CategoryList = new List<string>
        {
            "Menu",
            "Product",
        };

        public static readonly List<string> GlobalSettingList = new List<string>
        {
            "DEFAULT",
            "BOOLEAN",
        };
        public static readonly List<string> LayoutList = new List<string>
        {
            "ADMIN",
            "PUBLIC",
        };

        public static readonly List<string> ModuleList = new List<string>
        {
            "GENERAL",
            "MENUMODULE",
        };
        public static readonly List<string> NotifyUserTypeList = new List<string>
        {
            "MANAGER",
            "CHEF",
            "ADMIN"
        };

        public static readonly List<string> HeadTypeList = new List<string> { "EXPENSE", "REVENUE", "ASSET", "LIABILITY" };
        public static readonly List<string> HeadPaymentModeList = new List<string> { "Cash,Bank", "N/A", "Cash", "Bank" };
        public static readonly List<string> BankAccTypeList = new List<string> { "SAVING ACCOUNT", "CURRENT ACCOUNT", "CC ACCOUNT", "LOAN ACCOUNT" };
        public static readonly List<string> PaymentMode = new List<string> { "CASH", "CARD", "CHEQUE", "DUE", "DUE&CASH" };
        public static readonly List<string> TransferList = new List<string> { "GROSS PROFIT", "NET PROFIT", "BALANCE SHEET" };
    }
}
