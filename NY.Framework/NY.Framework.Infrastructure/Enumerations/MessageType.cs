using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.Infrastructure.Enumerations
{
    public enum MessageType
    {
        Closing_Message=1,
        Invalid,
        Invalid_Because_Campaign_Has_been_Closed,
        Main_Feedback_SMS,
        Valid_Reply
    }
}
