using System;
using System.Data;

namespace SSGM_ELVISPRIME_COMMON.Query.Member
{
    class Find_Query
    {
        string sqlstr;

        public string FindID_Query(DataRow dr)
        {
            sqlstr = "";
            sqlstr += " SELECT ";
            sqlstr += " USR_ID ";
            sqlstr += " FROM ";
            sqlstr += " MDM_EXT_USR_MST ";
            sqlstr += " WHERE 1=1 AND ";
            sqlstr += " TRIM(LOC_NM) = '" + dr["LOC_NM"] + "'";
            sqlstr += " AND TRIM(HP_NO) = '" + dr["HP_NO"] + "' ";

            return sqlstr;
        }

        public string GetAuthNum_Query(DataRow dr)
        {
            sqlstr = "";
            sqlstr += " SELECT ";
            sqlstr += " USR_ID,";
            sqlstr += " EMAIL,";
            sqlstr += " PSWD,";
            sqlstr += " APV_YN,";
            sqlstr += " (SELECT CD_NM FROM MDM_COM_CODE WHERE GRP_CD = 'R02' AND OPT_ITEM1 = 'Y') AS SEND_EMAIL";
            sqlstr += " FROM ";
            sqlstr += " MDM_EXT_USR_MST ";
            sqlstr += " WHERE 1=1 AND ";
            sqlstr += " TRIM(LOC_NM) = '" + dr["LOC_NM"] + "' AND ";
            sqlstr += " TRIM(USR_ID) = '" + dr["USR_ID"] + "'";

            return sqlstr;
        }

        public string SetAuthNum_Query(DataRow dr, string ConfirmKey)
        {
            sqlstr = "";
            sqlstr += " UPDATE ";
            sqlstr += " MDM_EXT_USR_MST ";
            sqlstr += " SET ";
            sqlstr += " APP_KEY = '" + ConfirmKey + "' , ";
            sqlstr += " UPD_USR = '"+dr["USR_ID"]+"' ,";		
            sqlstr += " UPD_YMD = '" + DateTime.Now.ToString("yyyyMMdd") + "' , ";
            sqlstr += " UPD_HM = '" + DateTime.Now.ToString("HHmmss") + "'";
            sqlstr += " WHERE 1=1 AND ";
            sqlstr += " USR_ID = '" + dr["USR_ID"] + "'";

            return sqlstr;
        }

        public string FindPW_Query(DataRow dr)
        {
            sqlstr = "";

            sqlstr += " SELECT ";
            sqlstr += " USR_ID ";
            sqlstr += " ,EMAIL ";
            sqlstr += " ,(SELECT CD_NM FROM MDM_COM_CODE WHERE GRP_CD = 'R02' AND OPT_ITEM1 = 'Y') AS SEND_EMAIL ";
            sqlstr += " FROM MDM_EXT_USR_MST ";
            sqlstr += " WHERE 1=1  ";
            sqlstr += " AND LOC_NM = '" + dr["LOC_NM"] + "' ";
            sqlstr += " AND USR_ID = '" + dr["USR_ID"] + "' ";
            sqlstr += " AND APP_KEY = '" + dr["KEY"] + "'";

            return sqlstr;
        }

        /// <summary>
        /// 새로운 비밀번호 세팅
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="strPSWD"></param>
        /// <param name="hiddenPSWD"></param>
        /// <returns></returns>
        public string SetNewPW_Query(DataRow dr, string strPSWD, string hiddenPSWD)
        {

            sqlstr = "";
            sqlstr += " UPDATE ";
            sqlstr += " MDM_EXT_USR_MST ";
            sqlstr += " SET ";
            sqlstr += " PSWD = '" + YJIT.Utils.StringUtils.Md5Hash(strPSWD) + "' ,";
            sqlstr += " CHAR_PSWD = '" + hiddenPSWD + "' ,";
            sqlstr += " UPD_USR = '" + dr["USR_ID"] + "' ,";
            sqlstr += " UPD_YMD = '" + DateTime.Now.ToString("yyyyMMdd") + "' ,";
            sqlstr += " UPD_HM = '" + DateTime.Now.ToString("HHmmss") + "' ";
            sqlstr += " WHERE 1=1 AND ";
            sqlstr += " USR_ID = '" + dr["USR_ID"] + "'";

            return sqlstr;
        }

    }
}
