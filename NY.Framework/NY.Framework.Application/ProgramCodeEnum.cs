using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NY.Framework.Application
{
    public enum ProgramCodeEnum
    {
        Dashboard=1,
        DATAS,                  //အချက်အလက်များ (Data)
        DATA,                   //စုစည်းထားသည့် Data များ
        DATA_ENTRY,             //Data အသစ်ထည့်ရန်
        DATA_UPLOAD,            //Data upload via EXCEL file
        SERVICE_VIEW,           //ဝန်ဆောင်မှုအမျိုးအစားများ
        SENDING_SMS,            //SMS များပေးပို့ခြင်း
        MOBILE_GROUP_ENTRY,     //ဖုန်းနံပါတ်အုပ်စုအသစ်ထည့်ရန်
        MOBILE_GROUP_LIST,      //ဖုန်းနံပါတ်အုပ်စုအားလုံးကြည့်ရန်
        CAMPAIGN_ENTRY,         //ကမ်ပိန်း Campaign အသစ်စတင်ရန်
        CAMPAIGN_LIST,          //ကမ်ပိန်း Campaign အားလုံးကြည့်ရန်
        INBOX_OUTBOX,           //ဝင်စာ/ထွက်စာ
        CATEGORIZING,           //ပြည်သူ့တုန့်ပြန်ချက်များကို အုပ်စုခွဲခြင်း
        UNCATEGORIZED_SMS,      //အုပ်စုမခွဲရသေးသော SMS များ
        CATEGORIZED_SMS,        //အုပ်စုခွဲပြီးသော SMS များ
        MANAGE_CATEGORY,        //တုန့်ပြန်မှုများကို အုပ်စုခွဲခြင်း
        KPIS,                   //KPIs
        CATEGORIZATION_STATS,   //Categorization Stats
        RESPONSE_STATS,         //Response Stats
        ACCOUNT_MANAGEMENT,     //Account Management
        ACCOUNT,                //Accounts
        DATA_MANAGEMENT,        //Data Management
        DATA_PORTAL,            //Data Portal
        MINISTRY,               //Ministry
        DEPARTMENT,             //Department
        SERVICES,               //Services
        CATEGORY,               //Category
        LOCATION,               //Location
        SECURITY,               //Security
        ACCESS_LOGS,            //Access Logs
        CHANGE_PASSWORD,        //Password ပြောင်းရန်
        USER,
        SERVICE,
        MONTHLYCAMPAIGNCOUNT
    }
}
