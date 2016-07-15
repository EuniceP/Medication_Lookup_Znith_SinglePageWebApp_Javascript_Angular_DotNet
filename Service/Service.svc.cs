using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceModel.Activation;
using System.Text;
using System.Data.Common;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.IO;
using SQL;
using JSON_Utility;
using ErrorHandler;
using Microsoft.VisualBasic;

namespace Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service.svc or Service.svc.cs at the Solution Explorer and start debugging.
   
    public class Service : IService
    {
        public class GCStoredProcedure
        {
            public const string SP_NAME = "usp_PHARMA_LOOKUP";
            public class GCAction
            {
                public const string GET_PHARMA_GROUP = "GET_PHARMA_GROUP";
                public const string GET_PHARMA_CLASS = "GET_PHARMA_CLASS";
                public const string GET_PHARMA_GROUPCLASS = "GET_PHARMA_GROUPCLASS";
                public const string GET_PHARMA_MANUFACTURER = "GET_PHARMA_MANUFACTURER";
                public const string GET_PHARMA_PRICE_HISTORY = "GET_PHARMA_PRICE_HISTORY";
                public const string GET_PHARMA_INGREDIENT = "GET_PHARMA_INGREDIENT";
                public const string GET_PHARMA_GENERICNAMES = "GET_PHARMA_GENERICNAMES";
                public const string GET_PHARMA_PRODUCT = "GET_PHARMA_PRODUCT";
                public const string GET_PHARMA_RESULT = "GET_PHARMA_RESULT";
            }
            public class GCParam
            {
                public const string Action = "@ACTION";
                public const string Group_Code = "@GROUP_CODE";
                public const string Class_Code = "@CLASS_CODE";
                public const string NDC = "@NDC";
                public const string GPI = "@GPI";
                public const string Product = "@PRODUCT";
                public const string Manufacturer = "@MANUFACTURER";
                public const string GroupClass_Name = "@GROUPCLASS_NAME";
            }
        }
        
        Stream IService.GET_PHARMA_GROUP()
        {
            Stream stream = null;
            using (SqlCommand cmd = SQL_Helper.GetDB_Command(GCStoredProcedure.SP_NAME))
            {
                try
                {
                    cmd.Parameters.Add(SQL_Helper.Get_Param(GCStoredProcedure.GCParam.Action, GCStoredProcedure.GCAction.GET_PHARMA_GROUP));
                    cmd.Connection.Open();
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        stream = GCJsonUtility.DataReaderToJsonArray(reader);
                    }
                    cmd.Connection.Close();
                }
                catch (Exception ex)
                {
                    GCErrorHandler.ReportError(ex);
                    throw ex;
                }
                finally
                {
                    if (cmd.Connection.State != System.Data.ConnectionState.Closed)
                    {
                        cmd.Connection.Close();
                    }
                }
            }
            return stream;
        }
        Stream IService.GET_PHARMA_GROUPCLASS()
        {
            Stream stream = null;
            using (SqlCommand cmd = SQL_Helper.GetDB_Command(GCStoredProcedure.SP_NAME))
            {
                try
                {
                    cmd.Parameters.Add(SQL_Helper.Get_Param(GCStoredProcedure.GCParam.Action, GCStoredProcedure.GCAction.GET_PHARMA_GROUPCLASS));
                    cmd.Connection.Open();
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        stream = GCJsonUtility.DataReaderToJsonArray(reader);
                    }
                    cmd.Connection.Close();
                }
                catch (Exception ex)
                {
                    GCErrorHandler.ReportError(ex);
                    throw ex;
                }
                finally
                {
                    if (cmd.Connection.State != System.Data.ConnectionState.Closed)
                    {
                        cmd.Connection.Close();
                    }
                }
            }
            return stream;
        }
        Stream IService.GET_PHARMA_CLASS(string Group_Code)
        {
            Stream stream = null;
            using (SqlCommand cmd = SQL_Helper.GetDB_Command(GCStoredProcedure.SP_NAME))
            {
                try
                {
                    cmd.Parameters.Add(SQL_Helper.Get_Param(GCStoredProcedure.GCParam.Action, GCStoredProcedure.GCAction.GET_PHARMA_CLASS));
                    
                    if (!string.IsNullOrEmpty(Group_Code))
                    {
                        cmd.Parameters.Add(SQL_Helper.Get_Param(GCStoredProcedure.GCParam.Group_Code, Group_Code));
                    }
                    cmd.Connection.Open();
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        stream = GCJsonUtility.DataReaderToJsonArray(reader);
                    }
                    cmd.Connection.Close();
                }
                catch (Exception ex)
                {
                    GCErrorHandler.ReportError(ex);
                    throw ex;
                }
                finally
                {
                    if (cmd.Connection.State != System.Data.ConnectionState.Closed)
                    {
                        cmd.Connection.Close();
                    }
                }
            }
            return stream;

        }

        Stream IService.GET_PHARMA_PRICE_HISTORY(string NDC)
        {
            Stream stream = null;
            using (SqlCommand cmd = SQL_Helper.GetDB_Command(GCStoredProcedure.SP_NAME))
            {
                try
                {
                    cmd.Parameters.Add(SQL_Helper.Get_Param(GCStoredProcedure.GCParam.Action, GCStoredProcedure.GCAction.GET_PHARMA_PRICE_HISTORY));

                    if (!string.IsNullOrEmpty(NDC))
                    {
                        cmd.Parameters.Add(SQL_Helper.Get_Param(GCStoredProcedure.GCParam.NDC, NDC));
                    }
                    cmd.Connection.Open();
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        stream = GCJsonUtility.DataReaderToJsonArray(reader);
                    }
                    cmd.Connection.Close();
                }
                catch (Exception ex)
                {
                    GCErrorHandler.ReportError(ex);
                    throw ex;
                }
                finally
                {
                    if (cmd.Connection.State != System.Data.ConnectionState.Closed)
                    {
                        cmd.Connection.Close();
                    }
                }
            }
            return stream;
        }

        Stream IService.GET_PHARMA_INGREDIENT(string NDC)
        {
            Stream stream = null;
            using (SqlCommand cmd = SQL_Helper.GetDB_Command(GCStoredProcedure.SP_NAME))
            {
                try
                {
                    cmd.Parameters.Add(SQL_Helper.Get_Param(GCStoredProcedure.GCParam.Action, GCStoredProcedure.GCAction.GET_PHARMA_INGREDIENT));

                    if (!string.IsNullOrEmpty(NDC))
                    {
                        cmd.Parameters.Add(SQL_Helper.Get_Param(GCStoredProcedure.GCParam.NDC, NDC));
                    }
                    cmd.Connection.Open();
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        stream = GCJsonUtility.DataReaderToJsonArray(reader);
                    }
                    cmd.Connection.Close();
                }
                catch (Exception ex)
                {
                    GCErrorHandler.ReportError(ex);
                    throw ex;
                }
                finally
                {
                    if (cmd.Connection.State != System.Data.ConnectionState.Closed)
                    {
                        cmd.Connection.Close();
                    }
                }
            }
            return stream;
        }

        Stream IService.GET_PHARMA_MANUFACTURER()
        {
            Stream stream = null;
            using (SqlCommand cmd = SQL_Helper.GetDB_Command(GCStoredProcedure.SP_NAME))
            {
                try
                {
                    cmd.Parameters.Add(SQL_Helper.Get_Param(GCStoredProcedure.GCParam.Action, GCStoredProcedure.GCAction.GET_PHARMA_MANUFACTURER));
                    cmd.Connection.Open();
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        stream = GCJsonUtility.DataReaderToJsonArray(reader);
                    }
                    cmd.Connection.Close();
                }
                catch (Exception ex)
                {
                    GCErrorHandler.ReportError(ex);
                    throw ex;
                }
                finally
                {
                    if (cmd.Connection.State != System.Data.ConnectionState.Closed)
                    {
                        cmd.Connection.Close();
                    }
                }
            }
            return stream;

        }

        Stream IService.GET_PHARMA_GENERICNAMES(string Group_Code, string Class_Code, string NDC, string GPI, string Product, string Manufacturer, string GroupClass_Name)
        {

            Stream stream = null;
            using (SqlCommand cmd = SQL_Helper.GetDB_Command(GCStoredProcedure.SP_NAME))
            {
                try
                {
                    cmd.Parameters.Add(SQL_Helper.Get_Param(GCStoredProcedure.GCParam.Action, GCStoredProcedure.GCAction.GET_PHARMA_GENERICNAMES));
                    if (!string.IsNullOrEmpty(NDC))
                    {
                        cmd.Parameters.Add(SQL_Helper.Get_Param(GCStoredProcedure.GCParam.NDC, NDC));
                    }
                    else if (!string.IsNullOrEmpty(GPI))
                    {
                        cmd.Parameters.Add(SQL_Helper.Get_Param(GCStoredProcedure.GCParam.GPI, GPI));
                    }
                    else if (!string.IsNullOrEmpty(Product))
                    {
                        cmd.Parameters.Add(SQL_Helper.Get_Param(GCStoredProcedure.GCParam.Product, Product));
                    }
                    else if (!string.IsNullOrEmpty(Manufacturer))
                    {
                        cmd.Parameters.Add(SQL_Helper.Get_Param(GCStoredProcedure.GCParam.Manufacturer, Manufacturer));
                    }
                    else if (!string.IsNullOrEmpty(GroupClass_Name))
                    {
                        cmd.Parameters.Add(SQL_Helper.Get_Param(GCStoredProcedure.GCParam.GroupClass_Name, GroupClass_Name));
                    }
                    else if (!string.IsNullOrEmpty(Class_Code))
                    {
                        cmd.Parameters.Add(SQL_Helper.Get_Param(GCStoredProcedure.GCParam.Class_Code, Class_Code));
                    }
                    else if (!string.IsNullOrEmpty(Group_Code))
                    {
                        cmd.Parameters.Add(SQL_Helper.Get_Param(GCStoredProcedure.GCParam.Group_Code, Group_Code));
                    };


                    cmd.Connection.Open();
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        stream = GCJsonUtility.DataReaderToJsonArray(reader);
                    }
                    cmd.Connection.Close();
                }
                catch (Exception ex)
                {
                    GCErrorHandler.ReportError(ex);
                    throw ex;
                }
                finally
                {
                    if (cmd.Connection.State != System.Data.ConnectionState.Closed)
                    {
                        cmd.Connection.Close();
                    }
                }
            }
            return stream;
        }

        Stream IService.GET_PHARMA_RESULT(string Group_Code, string Class_Code, string NDC, string GPI, string Product, string Manufacturer, string GroupClass_Name)  
        {
            
            Stream stream = null;
            using (SqlCommand cmd = SQL_Helper.GetDB_Command(GCStoredProcedure.SP_NAME))
            {
                try
                {
                    cmd.Parameters.Add(SQL_Helper.Get_Param(GCStoredProcedure.GCParam.Action, GCStoredProcedure.GCAction.GET_PHARMA_RESULT));
                    if (!string.IsNullOrEmpty(NDC))
                    {
                        cmd.Parameters.Add(SQL_Helper.Get_Param(GCStoredProcedure.GCParam.NDC, NDC));
                    }
                    else if (!string.IsNullOrEmpty(GPI))
                    {
                        cmd.Parameters.Add(SQL_Helper.Get_Param(GCStoredProcedure.GCParam.GPI, GPI));
                    }
                    else if (!string.IsNullOrEmpty(Product))
                    {
                        cmd.Parameters.Add(SQL_Helper.Get_Param(GCStoredProcedure.GCParam.Product, Product));
                    }
                    else if (!string.IsNullOrEmpty(Manufacturer))
                    {
                        cmd.Parameters.Add(SQL_Helper.Get_Param(GCStoredProcedure.GCParam.Manufacturer, Manufacturer));
                    }
                    else if (!string.IsNullOrEmpty(GroupClass_Name))
                    {
                        cmd.Parameters.Add(SQL_Helper.Get_Param(GCStoredProcedure.GCParam.GroupClass_Name, GroupClass_Name));
                    }
                    else if (!string.IsNullOrEmpty(Class_Code))
                    {
                        cmd.Parameters.Add(SQL_Helper.Get_Param(GCStoredProcedure.GCParam.Class_Code, Class_Code));
                    } 
                    else if (!string.IsNullOrEmpty(Group_Code))
                    {
                        cmd.Parameters.Add(SQL_Helper.Get_Param(GCStoredProcedure.GCParam.Group_Code, Group_Code));
                    }; 
              
 
                    cmd.Connection.Open();
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        stream = GCJsonUtility.DataReaderToJsonArray(reader);
                    }
                    cmd.Connection.Close();
                }
                catch (Exception ex)
                {
                    GCErrorHandler.ReportError(ex);                    
                    throw ex;
                }
                finally
                {
                    if (cmd.Connection.State != System.Data.ConnectionState.Closed)
                    {
                        cmd.Connection.Close();
                    }
                }
            }
            return stream;
        }

    }
}
