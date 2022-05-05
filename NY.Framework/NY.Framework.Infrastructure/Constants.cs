using Microsoft.Extensions.Configuration;
using NY.Framework.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NY.Framework.Infrastructure
{
    public static class Constants
    {
        public const string SYSTEM_ADMINISTRATOR_ROLE = "SYSTEM_ADMIN";
        public const string ANONYMOUS_ROLE = "ANONYMOUS";

        public const string UserNamePasswordMessage = "Invalid User Name Or Password.";

        public const string NoData = ".  .  .  .  .  No Data  .  .  .  .  .";

        public static int OoredooUnicodeCharCount =Convert.ToInt32(ConfigManager.AppSetting["OperatorConfig:OoredooUnicodeCharCount"]);
        public static int OoredooUnicodeCharCountOver70 =Convert.ToInt32(ConfigManager.AppSetting["OperatorConfig:OoredooUnicodeCharCountOver70"]);
        public static int OoredooEngCharCount =Convert.ToInt32(ConfigManager.AppSetting["OperatorConfig:OoredooEngCharCount"]);
        public static int OoredooEngCharCountOver160 =Convert.ToInt32(ConfigManager.AppSetting["OperatorConfig:OoredooEngCharCountOver160"]);

        public static int MytelUnicodeCharCount =Convert.ToInt32(ConfigManager.AppSetting["OperatorConfig:MytelUnicodeCharCount"]);
        public static int MytelUnicodeCharCountOver70 = Convert.ToInt32(ConfigManager.AppSetting["OperatorConfig:MytelUnicodeCharCountOver70"]);
        public static int MytelEngCharCount =Convert.ToInt32(ConfigManager.AppSetting["OperatorConfig:MytelEngCharCount"]);
        public static int MytelEngCharCountOver160 = Convert.ToInt32(ConfigManager.AppSetting["OperatorConfig:MytelEngCharCountOver160"]);

        public static int TelenorUnicodeCharCount =Convert.ToInt32(ConfigManager.AppSetting["OperatorConfig:TelenorUnicodeCharCount"]);
        public static int TelenorUnicodeCharCountOver70 = Convert.ToInt32(ConfigManager.AppSetting["OperatorConfig:TelenorUnicodeCharCountOver70"]);
        public static int TelenorEngCharCount =Convert.ToInt32(ConfigManager.AppSetting["OperatorConfig:TelenorEngCharCount"]);
        public static int TelenorEngCharCountOver140 = Convert.ToInt32(ConfigManager.AppSetting["OperatorConfig:TelenorEngCharCountOver140"]);

        public static int MptUnicodeCharCount =Convert.ToInt32(ConfigManager.AppSetting["OperatorConfig:MptUnicodeCharCount"]);
        public static int MptEngCharCount = Convert.ToInt32(ConfigManager.AppSetting["OperatorConfig:MptEngCharCount"]);
        public static int MptUnicodeCharCountOver70 = Convert.ToInt32(ConfigManager.AppSetting["OperatorConfig:MptUnicodeCharCountOver70"]);
        public static int MptEngCharCountOver140 = Convert.ToInt32(ConfigManager.AppSetting["OperatorConfig:MptEngCharCountOver140"]);


        public static string FilePath = ConfigManager.AppSetting["FilePathConfiguration:ReportTemplateFilePath"];
        public static string MytelApiUrl = ConfigManager.AppSetting["OperatorConfig:MytelUrl"];
        public static string OoredooApiUrl = ConfigManager.AppSetting["OperatorConfig:OoredooUrl"];
        public static string MptTelenorApiUrl = ConfigManager.AppSetting["OperatorConfig:MptTelenorUrl"]; 
        public static string MytelUserName = ConfigManager.AppSetting["OperatorConfig:MytelUserName"];
        public static string MytelPassword = ConfigManager.AppSetting["OperatorConfig:MytelPassword"];
        public static string MptUserName = ConfigManager.AppSetting["OperatorConfig:MptUserName"];
        public static string MptPassword = ConfigManager.AppSetting["OperatorConfig:MptPassword"];
        public static string TelenorUserName = ConfigManager.AppSetting["OperatorConfig:TelenorUserName"];
        public static string TelenorPassword = ConfigManager.AppSetting["OperatorConfig:TelenorPassword"];

        public static string OoredooApiKey = ConfigManager.AppSetting["OperatorConfig:OoredooApiKey"];
        public static string AttachFile = ConfigManager.AppSetting["FilePathConfiguration:AttachFilePath"];
        public static string UploadFile = ConfigManager.AppSetting["FilePathConfiguration:UploadFilePath"];
        public static string MinistryLogoPath = ConfigManager.AppSetting["FilePathConfiguration:LoGoPath"];

        public const string AccessedDenised = "Permission မရှိပါ။";
        public const string UnAuthorize = "401";
        public const string Forbideen = "4O3 Forbideen";
        public const string ServerError = "500 Internal Server Error";
       
        #region Save & Delete Message

        public const string SAVE_SUCCESS_MESSAGE = "အချက်အလက်များကို သိမ်းဆည်းပြီးပါပြီ။";
        public const string FILE_SAVE_SUCCESS_MESSAGE = "ဖိုင်သိမ်းဆည်းပြီးပါပြီ။";
        public const string DELETE_SUCCESS_MESSAGE = "အချက်အလက်များကို ဖျက်ပြီးပါပြီ။";
        public const string DELETE_FAIL_MESSAGE = "DELETE_FAIL_MESSAGE";
        public const string DELETE_FAIL_MESSAGEForUser = "DELETE_FAIL_MESSAGEForUser";

        #endregion

        #region Name Must Be Unique

        public const string SubMenuNameMustBeUnique = "Sub Menu အမည်တူနေပါသည်။";
        public const string MinistryNameMustBeUnique = "ဝန်ကြီးဌာနအမည်တူနေပါသည်။";
        public const string CategoryNameMustBeUnique = "ဝန်ဆောင်မှုအမျိုးအစားတူနေပါသည်။";

        public const string LocationNameMustBeUnique = "Locationအမည်တူနေပါသည်။";
        public const string LocationCodeMustBeUnique = "Location Codeတူနေပါသည်။";


    
        public const string CampaignNameMustBeUnique = "ကမ်ပိန်းအမည်တူနေပါသည်။";

        public const string DepartmentNameMustBeUnique = "ဌာနအမည်တူနေပါသည်။";
        public const string GroupNameMustBeUnique = "အုပ်စုအမည်တူနေပါသည်။";
        public const string GroupMemberNameMustBeUnique = "အမည်တူနေပါသည်။";

        public const string UserNameMustBeUnique = "User Name မထပ်ရပါ။";
        public const string ServiceNameMustBeUnique = "Service Name မထပ်ရပါ။";

        #endregion

        #region Delete Confirmation Messages
        // for all
        public const string DeleteConfirmationSMSForAll = "အချက်အလက်များပျက်သွားပါမည်။ ဖျက်မှာသေချာပါသလား?";
        #endregion

        

        public static string ForeignKeyConstraint = "တခြားဆက်စပ်နေသောrecords များကိုအရင်ဖျက်ရန်လိုအပ်ပါသည်။";
                
    }
}
