namespace JSON_Utility
{
    public class GCStoredProcedure
    {
        public class Get_Pharma_Groups
        {
            public const string SP_NAME = "dbo.usp_PHARMA_LOOKUP";
            public class Action
            {
                //public const string ASSIGN_ALERT = "ASSIGN_ALERT";
                //public const string RESET_ASSIGNED_ALERT = "RESET_ASSIGNED_ALERT";
                //public const string COMPLETE_ALERT = "COMPLETE_ALERT";
                //public const string RESET_COMPLETE_ALERT = "RESET_COMPLETE_ALERT";
                //public const string MARK_NO_ACTION = "MARK_NO_ACTION";
            }
            public class GCParams
            {
                public const string Action= "@Action";
            }
        }
        public class Get_Pharma_Classes
        {
            public const string SP_NAME = "dbo.usp_PHARMA_LOOKUP";
            public class Action
            { 
            }
            public class GCParams
            {
                public const string Action = "@Action";
                public const string Group_Code = "@GROUP_CODE";
            }
        }
        public class Get_Pharma_Price_History
        {
            public const string SP_NAME = "dbo.usp_PHARMA_LOOKUP";
            public class Action
            {
            }
            public class GCParams
            {
                public const string Action = "@Action";
                public const string NDC = "@NDC";
            }
        } 
        public class Get_Pharma_Manufacturer
        {
            public const string SP_NAME = "dbo.usp_PHARMA_LOOKUP";
            public class Action
            {
            }
            public class GCParams
            {
                public const string Action = "@Action";
            }
        }

        
        public class Get_AlertType
        {
            public const string SP_NAME = "dbo.usp_GET_ALERT_TYPE";
            public class Action
            {
            }
            public class GCParams
            {
                public const string ALERT_TYPE_ID = "@ALERT_TYPE_ID";
            }
        }

        public class Get_AlertList
        {
            public const string SP_NAME = "dbo.usp_GET_ALERT_LIST";
            public class Action
            {
            }
            public class GCParams
            {
                public const string ALERT_TYPE_ID = "@ALERT_TYPE_ID";
                public const string USER_ROLE = "@USER_ROLE";
                public const string LOG_IN_NAME = "@LOG_IN_NAME";
                public const string DATE_FROM = "@DATE_FROM";
                public const string DATE_THRU = "@DATE_THRU";
                public const string STATUS = "@STATUS";
                public const string ASSIGNED_TO_USER_ID = "@ASSIGNED_TO_USER_ID";
                public const string COMPLETED_BY_USER_ID = "@COMPLETED_BY_USER_ID";
                public const string EMPLOYEE = "@EMPLOYEE";
            }
        }

        public class Get_UserInfo
        {
            public const string SP_NAME = "dbo.usp_GET_USER_INFO";
            public class GCParams
            {
                public const string LOG_IN_NAME = "@LOG_IN_NAME";
            }
            public class GCOutputParams
            {
                public const string KNOWN_AS = "@KNOWN_AS";
                public const string SERVICE_LOCATION_CD = "@SERVICE_LOCATION_CD";
            }
        }

        public class Get_UserRole
        {
            public const string SP_NAME = "dbo.usp_GET_USER_ROLE";
            public class GCParams
            {
                public const string LOG_IN_NAME = "@LOG_IN_NAME";
            }
        }

        public class Get_UserDirectReportFullNameList
        {
            public const string SP_NAME = "dbo.usp_GET_USER_DIRECT_REPORT_FULLNAME";
            public class GCParams
            {
                public const string LOG_IN_NAME = "@LOG_IN_NAME";
            }
        }

        public class Get_UserList
        {
            public const string SP_NAME = "dbo.usp_GET_USER_LIST";
            public class GCParams
            {
                public const string USER_ROLE = "@USER_ROLE";
                public const string SERVICE_LOCATION_CD = "@SERVICE_LOCATION_CD";
            }
        }
    }
}