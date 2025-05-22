using System;
using System.Data;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Serialization;
using System.Data.SqlClient;
using VSW.Lib.Global;

namespace VSW.Website.Tools
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class Copy : System.Web.Services.WebService
    {
        public HeaderUserAuth UserInfo = null;
        public HeaderError ErrorInfo = null;

        private bool IsValid()
        {
            if (UserInfo == null) 
                return false;

            return (UserInfo.LoginName.Trim().ToUpper() == VSW.Core.Global.Config.GetValue("Mod.WebServiceKey").ToString().Trim().ToUpper());
        }

        [WebMethod, SoapHeader("UserInfo", Direction = SoapHeaderDirection.In)]
        public void Clear_Cache()
        {
            if (!this.IsValid())
                return;

            VSW.Core.Web.Cache.Clear();
        }

        [WebMethod, SoapHeader("UserInfo", Direction = SoapHeaderDirection.In)]
        public int SQLCmd_ExecuteNonQuery(string sSQL)
        {
            if (!this.IsValid())
            {
                return -1;
            }
            return VSW.Core.Data.SQLCmd.ExecuteNonQuery(VSW.Core.Data.ConnectionString.Default, sSQL);
        }

        [WebMethod, SoapHeader("UserInfo", Direction = SoapHeaderDirection.In)]
        public bool SQLCmd_ExecuteScalar_ToBool(string sSQL)
        {
            if (!this.IsValid())
                return false;

            return VSW.Core.Data.SQLCmd.ExecuteScalar(VSW.Core.Data.ConnectionString.Default, sSQL).ToBool();
        }

        [WebMethod, SoapHeader("UserInfo", Direction = SoapHeaderDirection.In)]
        public DataTable SQL_GetTable(string sTable)
        {
            if (!this.IsValid())
                return null;

            return VSW.Core.Data.SQLCmd.ExecuteDataTable(VSW.Core.Data.ConnectionString.Default, 
                "SELECT * FROM " + sTable);
        }

        [WebMethod, SoapHeader("UserInfo", Direction = SoapHeaderDirection.In)]
        public DataTable SQL_GetTablePage(string sTable, 
            string sMasterKey, 
            int PageIndex, 
            int PageSize, 
            out int TotalRecord)
        {
            TotalRecord = 0;

            if (!this.IsValid())
                return null;

            return VSW.Core.Data.SQL.GetTableWithPage(VSW.Core.Data.ConnectionString.Default, 
                sTable, 
                sMasterKey, 
                "*", 
                string.Empty, 
                sMasterKey + " DESC", 
                PageSize, 
                PageIndex, 
                ref TotalRecord);
        }

        [WebMethod, SoapHeader("UserInfo", Direction = SoapHeaderDirection.In)]
        public DataTable SQLCmd_GetTable(string sSQL)
        {
            if (!this.IsValid())
                return null;

            return VSW.Core.Data.SQLCmd.ExecuteDataTable(VSW.Core.Data.ConnectionString.Default, sSQL);
        }

        [WebMethod, SoapHeader("UserInfo", Direction = SoapHeaderDirection.In)]
        public int SQL_InsertRecord(string sTable, DataTable dt, bool identity)
        {
            if (!this.IsValid())
                return -1;

            SqlCommand sQLCmd = new SqlCommand();
            SqlParametersByDataRow(sQLCmd, dt.Rows[0], "");
            if (sQLCmd.Parameters.Count == 0)
                return -1;

            string _sSQL = GetInsertText(sQLCmd);

            if (!identity)
                _sSQL = string.Format("INSERT INTO [{0}]({1}) VALUES({2});", sTable, _sSQL.Replace("@", "[").Replace(",", "],") + "]", _sSQL);
            else
                _sSQL = "SET IDENTITY_INSERT [" + sTable + "] ON; " + string.Format("INSERT INTO [{0}]({1}) VALUES({2});", sTable, _sSQL.Replace("@", "[").Replace(",", "],") + "]", _sSQL) + " SET IDENTITY_INSERT [" + sTable + "] OFF;";

            object[] _params = new object[2 * sQLCmd.Parameters.Count];
            for (int i = 0; i < sQLCmd.Parameters.Count; i++)
            {
                _params[2 * i] = sQLCmd.Parameters[i].ParameterName;
                _params[2 * i + 1] = sQLCmd.Parameters[i].Value;
            }

            return VSW.Core.Data.SQLCmd.ExecuteNonQuery(VSW.Core.Data.ConnectionString.Default, _sSQL, _params);
        }
        
        private void AddParameter(SqlCommand sQLCmd, string FieldName, object Value)
        {
            FieldName = (FieldName.IndexOf("@") == 0) ? FieldName : ("@" + FieldName);
            sQLCmd.Parameters.Add(new SqlParameter(FieldName.Trim(), Value));
        }

        private void SqlParametersByDataRow(SqlCommand sQLCmd, DataRow Row, string ExceptFieldName)
        {
            DataTable table = Row.Table;
            string fieldName = string.Empty;
            foreach (DataColumn column in table.Columns)
            {
                fieldName = column.ColumnName;
                if (!(ExceptFieldName.ToUpper() == fieldName.ToUpper()) && ((Row[fieldName] != DBNull.Value)))
                {
                    AddParameter(sQLCmd, fieldName, Row[fieldName]);
                }
            }
        }

        private string GetInsertText(SqlCommand sQLCmd)
        {
            string sSQL = string.Empty;
            for (int i = 0; i < sQLCmd.Parameters.Count; i++)
            {
                sSQL = sSQL + ((i == 0) ? "" : ", ") + sQLCmd.Parameters[i].ParameterName;
            }
            return sSQL;
        }

    }

    public class HeaderUserAuth : SoapHeader
    {
        public string LoginName = string.Empty;
        public string Password = string.Empty;
    }

    public class HeaderError : SoapHeader
    {
        public int Code = -1;
        public string Message = String.Empty;
    }
}
