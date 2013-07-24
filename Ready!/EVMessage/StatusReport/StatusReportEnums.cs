using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EVMessage.StatusReport
{
    public enum ReportType
    {
        NULL,
        MessageStatus,
        Errors
    }

    public enum ReportStatusCode
    {
        NULL,
        MAReceived,
        MAReceivedErrors,
        MAValidationSuccessful,
        MAValidationFailed,
        MASentToEMA,
        ACKReceivedFromEMA
    }

    public enum MAValidationType
    {
        NULL,
        XMLValidation,
        XMLDataValidation,
        EMABussinessRulesValidation
    }
}
