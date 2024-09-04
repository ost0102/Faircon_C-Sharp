using System;
using System.Data;

namespace SSGM_ELVISPRIME_COMMON.Query.Member
{
    class Member_Query
    {
        string sqlstr;

        public string isCheckID_Query(DataRow dr)
        {

            sqlstr = "";
            sqlstr += " SELECT USR_ID ";
            sqlstr += "        , APV_YN";
            sqlstr += " FROM MDM_EXT_USR_MST ";
            sqlstr += " WHERE 1=1 ";
            sqlstr += " AND UPPER(USR_ID) = UPPER('" + dr["USR_ID"] + "') ";

            return sqlstr;
        }

        public string isCheckEmail_Query(DataRow dr)
        {

            sqlstr = "";
            sqlstr += " SELECT EMAIL ";
            sqlstr += " FROM MDM_EXT_USR_MST ";
            sqlstr += " WHERE 1=1 ";
            sqlstr += " AND UPPER(EMAIL) = UPPER('" + dr["EMAIL"] + "') ";

            return sqlstr;
        }

        public string isCheckCRN_Query(DataRow dr)
        {

            sqlstr = "";
            sqlstr += " SELECT  ";
            sqlstr += " CUST_CD ,  ";
            sqlstr += " CUST_NM  ";
            sqlstr += " FROM MDM_CUST_MST ";
            sqlstr += " WHERE 1=1 ";
            sqlstr += " AND CRN = '" + dr["CRN"] + "' ";

            return sqlstr;
        }

        public string GetOfficeCode_Query()
        {
            sqlstr = "";
            sqlstr += " SELECT OFFICE_CD FROM MDM_OFFICE_MST WHERE CTRY_CD ='KR' AND ORG_LVL = 'LB' ";

            return sqlstr;
        }

        public string InsertRegister_Query(DataRow dr)
        {
            sqlstr = "";
            sqlstr += " INSERT INTO MDM_EXT_USR_MST ";
            sqlstr += "      (USR_ID , LOC_NM , DEF_OFFICE_CD , CUST_CD , CUST_NM , PSWD , CHAR_PSWD , HP_NO , EMAIL , USE_YN , INS_USR , INS_YMD , INS_HM , UPD_USR , UPD_YMD , UPD_HM , USR_TYPE , CRN , EMAIL_YN)   ";
            sqlstr += "      VALUES (";
            sqlstr += "        '" + dr["USR_ID"] + "'";
            sqlstr += "      , '" + dr["LOC_NM"] + "'";
            sqlstr += "      , '" + dr["OFFICE_CD"] + "'";
            sqlstr += "      , '" + dr["CUST_CD"] + "'";
            sqlstr += "      , '" + dr["CUST_NM"] + "'";
            sqlstr += "      , '" + YJIT.Utils.StringUtils.Md5Hash((string)dr["PSWD"]) + "'";            
            sqlstr += "      , '" + dr["CHAR_PSWD"] + "'";
            sqlstr += "      , '" + dr["HP_NO"] + "'";
            sqlstr += "      , '" + dr["EMAIL"] + "'";
            sqlstr += "      , '" + dr["USE_YN"] + "'";
            sqlstr += "      , (SELECT TO_CHAR(SYSDATE,'YYYYMM') || LPAD(TO_NUMBER(NVL(MAX(SUBSTR(USR_ID,9)),0)) + 1,4,0) ";
            sqlstr += "     FROM MDM_EXT_USR_MST  WHERE USR_ID LIKE TO_CHAR(SYSDATE,'YYYYMM')|| '%' )";
            sqlstr += "      , '" + DateTime.Now.ToString("yyyyMMdd") + "'";
            sqlstr += "      , '" + DateTime.Now.ToString("HHmmss") + "'";
            sqlstr += "      , (SELECT TO_CHAR(SYSDATE,'YYYYMM') || LPAD(TO_NUMBER(NVL(MAX(SUBSTR(USR_ID,9)),0)) + 1,4,0) ";
            sqlstr += "     FROM MDM_EXT_USR_MST  WHERE USR_ID LIKE TO_CHAR(SYSDATE,'YYYYMM')|| '%' )";
            sqlstr += "      , '" + DateTime.Now.ToString("yyyyMMdd") + "'";
            sqlstr += "      , '" + DateTime.Now.ToString("HHmmss") + "'";
            sqlstr += "      , '" + dr["USR_TYPE"] + "'";
            sqlstr += "      , '" + dr["CRN"] + "'";            
            sqlstr += "      , '" + dr["EMAIL_YN"] + "')";

            return sqlstr;
        }

        public string GetRegister_Query(DataRow dr)
        {

            sqlstr = "";
            sqlstr += " SELECT ";
            sqlstr += " A.EMAIL AS USR_ID , ";
            sqlstr += " A.USR_ID AS LOC_CD , ";
            sqlstr += " A.LOC_NM , ";
            sqlstr += " A.HP_NO , ";
            sqlstr += " A.INS_YMD , ";
            sqlstr += " A.INS_HM , ";
            sqlstr += " A.USR_TYPE, ";
            sqlstr += " A.CRN, ";
            sqlstr += " A.APP_KEY,";
            sqlstr += " A.APV_YN,";
            sqlstr += " A.CUST_CD,";
            sqlstr += " A.CUST_NM";
            sqlstr += " FROM MDM_EXT_USR_MST A ";
            sqlstr += " WHERE A.USR_ID = '" + dr["USR_ID"] + "' ";

            return sqlstr;
        }

        public string GetAdminList_Query(DataRow dr)
        {
            sqlstr = "";
            sqlstr += " SELECT DISTINCT * ";
            sqlstr += " FROM PRM_MAIL_INFO ";
            sqlstr += " WHERE 1=1 AND ";
            sqlstr += " DOMAIN = '" + dr["DOMAIN"] + "' AND  ";
            sqlstr += " (MAIL_TYPE = 'ALL' OR MAIL_TYPE = 'USER') ";

            return sqlstr;
        }
    }
}
