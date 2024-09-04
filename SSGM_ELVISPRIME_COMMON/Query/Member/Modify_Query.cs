using System;
using System.Data;

namespace SSGM_ELVISPRIME_COMMON.Query.Member
{
    class Modify_Query
    {
        string sqlstr;

        /// <summary>
        /// 회원정보수정 - 회원정보 가져오기
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string GetModifyInfo_Query(DataRow dr)
        {

            sqlstr = "";

            sqlstr += "  SELECT USR_ID, ";
            sqlstr += "         EMAIL, ";
            sqlstr += "         LOC_NM, ";
            sqlstr += "         HP_NO, ";
            sqlstr += "         CUST_CD, ";
            sqlstr += "         CRN, ";
            sqlstr += "         CUST_NM ";
            sqlstr += "    FROM MDM_EXT_USR_MST ";
            sqlstr += "    WHERE 1=1 ";
            sqlstr += "  	AND USR_ID = '" + dr["USR_ID"] + "' ";
            sqlstr += "  	AND EMAIL = '" + dr["EMAIL"] + "' ";

            return sqlstr;
        }

        /// <summary>
        /// 회원정보수정 - 비밀번호 체크
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string ChkNowPSWD_Query(DataRow dr)
        {

            sqlstr = "";

            sqlstr += "  SELECT PSWD ";
            sqlstr += "    FROM MDM_EXT_USR_MST ";
            sqlstr += "    WHERE 1=1 ";
            sqlstr += "  	AND USR_ID = '" + dr["USR_ID"] + "' ";
            sqlstr += "  	AND EMAIL = '" + dr["EMAIL"] + "' ";

            return sqlstr;
        }

        /// <summary>
        /// 회원정보수정 - 회원정보 저장 하기 
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string ModifySave_Query(DataRow dr)
        {

            sqlstr = "";

            sqlstr = "";
            sqlstr += " UPDATE MDM_EXT_USR_MST SET ";
            if (dr["NEW_PSWD"].ToString() != "")
            {
                sqlstr += " PSWD = '" + YJIT.Utils.StringUtils.Md5Hash((string)dr["NEW_PSWD"]) + "' , ";
                sqlstr += " APP_KEY = '' , ";
            }
            sqlstr += " LOC_NM = '" + dr["LOC_NM"] + "' , ";
            sqlstr += " HP_NO = '" + dr["HP_NO"] + "' ";
            sqlstr += " WHERE ";
            sqlstr += " USR_ID = '" + dr["USR_ID"] + "'  ";
            sqlstr += " AND EMAIL = '" + dr["EMAIL"] + "'  ";
            sqlstr += " AND PSWD = '" + YJIT.Utils.StringUtils.Md5Hash((string)dr["NOW_PSWD"]) + "' ";

            return sqlstr;
        }
    }
}
