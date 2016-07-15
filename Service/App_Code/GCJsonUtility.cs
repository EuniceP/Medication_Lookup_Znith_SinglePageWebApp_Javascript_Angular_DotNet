using System;
using System.IO;
using System.Text;
using System.Data.Common;
using Newtonsoft.Json;
namespace JSON_Utility
{
    public enum JsonValueType
    {
        INT, PCT, FLOAT, MONEY, TEXT, NUMBER_WITH_DECIMAL
    }
    public class GCJsonUtility
    {
        public static object DBValueToJsonValString(JsonValueType type, object db_val)
        {
            string val = string.Empty;
            if (db_val != DBNull.Value)
            {
                switch (type)
                {
                    case JsonValueType.INT:
                        val = string.Format("{0:#,##0}", db_val);
                        break;
                    case JsonValueType.PCT:
                        val = string.Format("{0:0.00%}", db_val);
                        break;
                    case JsonValueType.FLOAT:
                        val = string.Format("{0:0.00}", db_val);
                        break;
                    case JsonValueType.MONEY:
                        val = string.Format("{0:$#,##0}", db_val);
                        break;
                    case JsonValueType.NUMBER_WITH_DECIMAL:
                        val = string.Format("{0:#,##0.00}", db_val);
                        break;
                    case JsonValueType.TEXT:
                        val = db_val.ToString();
                        break;
                }
            }
            return val;
        }
        public static object DBValueToString(string db_type, object db_val)
        {
            object val = db_val;
            if (db_val != DBNull.Value)
            {
                switch (db_type)
                {
                    case "System.DateTime":
                        val = Convert.ToDateTime(db_val).ToString("MM/dd/yyyy");
                        break;
                    case "System.Double":
                    case "System.Decimal":
                        val = DBValueToJsonValString(JsonValueType.NUMBER_WITH_DECIMAL, val);
                        break;
                    case "System.Int32":
                        val = DBValueToJsonValString(JsonValueType.INT, val);
                        break;
                }
            }
            return val;
        }

        public static string DataReaderToJsonObjectRawData(DbDataReader reader)
        {
            return DataReaderToJsonObjectRawData(reader, false);
        }

        public static string DataReaderToJsonObjectRawData(DbDataReader reader, bool check_db_type)
        {
            if (reader != null && reader.HasRows)
            {
                using (StringWriter sw = new StringWriter())
                {
                    using (JsonTextWriter tw = new JsonTextWriter(sw))
                    {
                        tw.WriteStartObject();
                        while (reader.Read())
                        {
                            WritePropValueJson(reader, tw, check_db_type);
                        }
                        reader.Close();
                        tw.WriteEndObject();
                        tw.Close();
                    }
                    sw.Close();
                    return sw.ToString();
                }
            }
            else
            {
                if (reader != null && !reader.IsClosed)
                {
                    reader.Close();
                }
                return "{}";
            }
        }
        public static Stream DataReaderToJsonObject(DbDataReader reader)
        {
            return DataReaderToJsonObject(reader, false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="check_db_type">If true, format json string based on data type</param>
        /// <returns></returns>
        public static Stream DataReaderToJsonObject(DbDataReader reader, bool check_db_type)
        {
            if (reader != null && reader.HasRows)
            {
                using (StringWriter sw = new StringWriter())
                {
                    using (JsonTextWriter tw = new JsonTextWriter(sw))
                    {
                        tw.WriteStartObject();
                        while (reader.Read())
                        {
                            WritePropValueJson(reader, tw, check_db_type);
                        }
                        reader.Close();
                        tw.WriteEndObject();
                        tw.Close();
                    }
                    sw.Close();
                    return WrapCallback(sw.ToString());
                }
            }
            else
            {
                if (reader != null && !reader.IsClosed)
                {
                    reader.Close();
                }
                return WrapCallback("{}");
            }

        }

        public static Stream DataReaderToJsonArrayObject(DbDataReader reader, string col_name, string array_col_name)
        {
            return DataReaderToJsonArrayObject(reader, col_name, new string[] { array_col_name }, null, false, null);
        }
        public static Stream DataReaderToJsonArrayObject(DbDataReader reader, string col_name, string array_col_name, bool forceEmptyReaderToCreateObject)
        {
            return DataReaderToJsonArrayObject(reader, col_name, new string[] { array_col_name }, null, forceEmptyReaderToCreateObject, null);
        }
        public static Stream DataReaderToJsonArrayObject(DbDataReader reader, string col_name, string[] array_col_name)
        {
            return DataReaderToJsonArrayObject(reader, col_name, array_col_name, null, false, null);
        }
        public static Stream DataReaderToJsonArrayObject(DbDataReader reader, string col_name, string[] array_col_name, string[] array_same_level)
        {
            return DataReaderToJsonArrayObject(reader, col_name, array_col_name, array_same_level, false, null);
        }
        public static Stream DataReaderToJsonArrayObject(DbDataReader reader, string col_name, string[] array_col_name, string[] array_same_level, bool forceEmptyReaderToCreateObject)
        {
            return DataReaderToJsonArrayObject(reader, col_name, array_col_name, array_same_level, forceEmptyReaderToCreateObject, null);
        }
        /// <summary>
        /// Create array of object assuming using column name to be an object name, the rest will be under data array object 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        /// 
        public static Stream DataReaderToJsonArrayObject(DbDataReader reader, string col_name, string[] array_col_name, string [] array_same_level, bool forceEmptyReaderToCreateObject, string [] true_false_columns)
        {
            Stream jsonStream = null;
            if (reader != null)
            {
                using (StringWriter sw = new StringWriter())
                {
                    using (JsonTextWriter tw = new JsonTextWriter(sw))
                    {
                        string last_key = string.Empty;
                        int array_col_idx = 0;
                        tw.WriteStartArray();
                        do
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    if (string.Compare(last_key, reader[col_name].ToString(), true) != 0)
                                    {
                                        if (!string.IsNullOrEmpty(last_key))
                                        {
                                            tw.WriteEndArray();
                                            tw.WriteEndObject();
                                        }
                                        tw.WriteStartObject();
                                        tw.WritePropertyName(col_name.ToLower());
                                        tw.WriteValue(reader[col_name]);
                                        if(array_same_level != null && array_same_level.Length > 0) 
                                        {
                                            foreach (string col_same_level in array_same_level)
                                            {
                                                tw.WritePropertyName(col_same_level.ToLower());
                                                tw.WriteValue(reader[col_same_level]);
                                            }
                                        }
                                        if (array_col_name != null && !string.IsNullOrEmpty(array_col_name[array_col_idx]))
                                        {
                                            tw.WritePropertyName(array_col_name[array_col_idx]);
                                        }
                                        else
                                        {
                                            tw.WritePropertyName("data");
                                        }
                                        tw.WriteStartArray();
                                    }
                                    tw.WriteStartObject();
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        string fieldName = reader.GetName(i);
                                        bool skipField = false;
                                        if (string.Compare(fieldName, col_name, true) == 0)
                                        {
                                            skipField = true;
                                        }
                                        else if (array_same_level != null && array_same_level.Length > 0)
                                        {
                                            foreach (string col_same_level in array_same_level)
                                            {
                                                if (string.Compare(fieldName, col_same_level, true) == 0)
                                                {
                                                    skipField = true;
                                                    break;
                                                }
                                            }
                                        }

                                        if (!skipField)
                                        {
                                            tw.WritePropertyName(fieldName.ToLower());
                                            if (true_false_columns != null)
                                            {
                                                bool is_true_false_column = false;
                                                foreach (string true_false_col in true_false_columns)
                                                {
                                                    if (string.Compare(fieldName, true_false_col, true) == 0)
                                                    {
                                                        is_true_false_column = true;
                                                        break;
                                                    }
                                                }
                                                if (is_true_false_column)
                                                {
                                                    tw.WriteValue(Convert.ToBoolean(reader[i]));
                                                }
                                                else
                                                {
                                                    tw.WriteValue(reader[i]);
                                                }
                                            }
                                            else
                                            {
                                                tw.WriteValue(reader[i]);
                                            }
                                        }
                                    }
                                    last_key = reader[col_name].ToString();
                                    tw.WriteEndObject();
                                }
                            }
                            else
                            {
                                if (forceEmptyReaderToCreateObject)
                                {
                                    tw.WriteStartObject();
                                    tw.WritePropertyName(col_name.ToLower());
                                    if (array_col_name != null)
                                    {
                                        tw.WriteValue(array_col_name[array_col_idx]);
                                    }
                                    if (array_col_name != null && !string.IsNullOrEmpty(array_col_name[array_col_idx]))
                                    {
                                        tw.WritePropertyName(array_col_name[array_col_idx]);
                                    }
                                    else
                                    {
                                        tw.WritePropertyName("data");
                                    }
                                    tw.WriteStartArray();
                                    tw.WriteEndArray();
                                    tw.WriteEndObject();
                                }
                            }
                            if (!string.IsNullOrEmpty(last_key))
                            {
                                tw.WriteEndArray();
                                tw.WriteEndObject();
                            }
                            array_col_idx = (array_col_idx + 1) <= (array_col_name == null ? 0 : array_col_name.Length - 1) ? (array_col_idx + 1) : (array_col_name == null ? 0 : array_col_name.Length - 1);
                            last_key = string.Empty;
                        } while (reader.NextResult());
                        reader.Close();
                        tw.WriteEndArray();
                        tw.Close();
                    }
                    sw.Close();
                    jsonStream = WrapCallback(sw.ToString());
                }
            }
            else
            {
                jsonStream = WrapCallback("[]");
            }
            return jsonStream;
        }

        public static string DataReaderToJsonArrayRawData(DbDataReader reader)
        {
            return DataReaderToJsonArrayRawData(reader, false);
        }

        public static string DataReaderToJsonArrayRawData(DbDataReader reader, bool check_db_type)
        {
            if (reader != null && reader.HasRows)
            {
                using (StringWriter sw = new StringWriter())
                {
                    using (JsonTextWriter tw = new JsonTextWriter(sw))
                    {
                        tw.WriteStartArray();
                        while (reader.Read())
                        {
                            tw.WriteStartObject();
                            WritePropValueJson(reader, tw, check_db_type);
                            tw.WriteEndObject();
                        }
                        tw.WriteEndArray();
                        tw.Close();
                    }
                    sw.Close();
                    return sw.ToString();
                }
            }
            else
            {
                return "[]";
            }
        }

        public static Stream DataReaderToJsonArray(DbDataReader reader, string[] obj_array_name_list)
        {
            Stream stream = null;
            using (StringWriter sw = new StringWriter())
            {
                using (JsonTextWriter tw = new JsonTextWriter(sw))
                {
                    tw.WriteStartObject();
                    foreach (string obj_name in obj_array_name_list)
                    {
                        tw.WritePropertyName(obj_name);
                        tw.WriteRawValue(DataReaderToJsonArrayRawData(reader));
                        reader.NextResult();
                    }
                    reader.Close();
                    tw.WriteEndObject();
                    tw.Close();
                }
                stream = WrapCallback(sw.ToString());
            }
            return stream;
        }

        public static Stream DataReaderToJsonArray(DbDataReader reader)
        {
            return DataReaderToJsonArray(reader, false);
        }

        public static Stream DataReaderToJsonArray(DbDataReader reader, bool check_db_type)
        {
            Stream stream = null;
            if (reader != null && reader.HasRows)
            {
                stream = WrapCallback(DataReaderToJsonArrayRawData(reader, check_db_type));
                reader.Close();
            }
            else
            {
                if (reader != null && !reader.IsClosed)
                {
                    reader.Close();
                }
                stream = WrapCallback("[]");
            }
            return stream;
        }
        public static void WritePropValueJson(DbDataReader reader, JsonTextWriter tw, bool check_db_type)
        {
            int fieldCnt = reader.FieldCount;
            for (int i = 0; i < fieldCnt; i++)
            {
                tw.WritePropertyName(reader.GetName(i).ToLower());
                if (reader[i] == DBNull.Value)
                {
                    tw.WriteNull();
                }
                else
                {
                    if (check_db_type)
                    {
                        tw.WriteValue(DBValueToString(reader.GetFieldType(i).ToString(), reader[i]));
                    }
                    else
                    {
                        tw.WriteValue(reader[i]);
                    }
                }

            }
        }

        public static Stream PostConfirmation(DbDataReader reader)
        {
            string err_msg = string.Empty;
            if (reader != null)
            {
                while (reader.Read())
                {
                    if (string.Equals("ERROR_MSG", reader.GetName(0), StringComparison.OrdinalIgnoreCase))
                    {
                        err_msg = reader[0].ToString();
                    }
                }
                reader.Close();
            }
            return PostConfirmation(err_msg);
        }

        public static Stream PostConfirmation(string err_msg)
        {
            Stream result = null;
            using (StringWriter sw = new StringWriter())
            {
                using (JsonTextWriter tw = new JsonTextWriter(sw))
                {
                    tw.WriteStartObject();
                    tw.WritePropertyName("success");
                    tw.WriteValue(string.IsNullOrEmpty(err_msg));
                    tw.WritePropertyName("error_msg");
                    tw.WriteValue(err_msg);
                    tw.WriteEndObject();
                    tw.Close();
                }
                System.ServiceModel.Web.WebOperationContext ctx = System.ServiceModel.Web.WebOperationContext.Current;
                ctx.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                result = new MemoryStream(Encoding.UTF8.GetBytes(sw.ToString()));
                sw.Close();
            }
            return result;
        }

        public static Stream WrapCallback(StringBuilder str)
        {
            System.ServiceModel.Web.WebOperationContext ctx = System.ServiceModel.Web.WebOperationContext.Current;
            System.Collections.Specialized.NameValueCollection query = ctx.IncomingRequest.UriTemplateMatch.QueryParameters;
            if (!string.IsNullOrEmpty(query.Get("callback")))
            {
                ctx.OutgoingResponse.ContentType = "application/x-javascript";
                str.Insert(0, string.Format("{0}(", query.Get("callback")));
                str.Append(");");
            }
            else
            {
                ctx.OutgoingResponse.ContentType = "application/json; charset=utf-8";
            }
            return new MemoryStream(Encoding.UTF8.GetBytes(str.ToString()));
        }

        public static Stream WrapCallback(string json_string)
        {
            System.ServiceModel.Web.WebOperationContext ctx = System.ServiceModel.Web.WebOperationContext.Current;
            System.Collections.Specialized.NameValueCollection query = ctx.IncomingRequest.UriTemplateMatch.QueryParameters;
            if (!string.IsNullOrEmpty(query.Get("callback")))
            {
                ctx.OutgoingResponse.ContentType = "application/x-javascript";
                return new MemoryStream(Encoding.UTF8.GetBytes(string.Format("{0}({1});", query.Get("callback"), json_string)));
            }
            else
            {
                ctx.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                return new MemoryStream(Encoding.UTF8.GetBytes(json_string));
            }
        }
    }
}