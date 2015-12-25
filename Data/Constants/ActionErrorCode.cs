﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Constants
{
    public enum ActionErrorCode
    {
        [Description("The terminal was passed a request that required a connection string, and it was not found.")]
        SQL_SERVER_CONNECTION_STRING_MISSING = 10000,
        [Description("The terminal was unable to connect with the provided database connection string.")]
        SQL_SERVER_CONNECTION_FAILED,
        PAYLOAD_DATA_MISSING,
        PAYLOAD_DATA_INVALID,
        NO_AUTH_TOKEN_PROVIDED,
        DESIGN_TIME_DATA_MISSING
    }
}
