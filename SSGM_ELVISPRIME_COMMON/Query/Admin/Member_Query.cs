using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;

namespace SSGM_ELVISPRIME_COMMON.Query.Admin
{
    public class Member_Query
    {   
        string sqlstr;

        private string memberKey = ConfigurationManager.AppSettings["memberKey"].ToString();

        /// <summary>
        /// 관리자 관리 - 검색
        /// </summary>
        /// <returns></returns>
        public string SearchMember_Query(DataRow dr)
        {
            sqlstr = "";

            sqlstr += " SELECT * ";
            sqlstr += "   FROM (SELECT ROWNUM AS RNUM, ";
            sqlstr += "                FLOOR ( (ROWNUM - 1) / 10 + 1) AS PAGE, ";
            sqlstr += "                COUNT (*) OVER () AS TOTCNT, ";
            sqlstr += "                TOTAL.* ";
            sqlstr += "           FROM (SELECT MEMB_NO, ";
            sqlstr += "                        M_ID, ";
            sqlstr += "                        \"LEVEL\" AS LVL, ";
            sqlstr += "                        AUTH_LEVEL, ";
            sqlstr += "                        STATUS, ";
            sqlstr += "                        CryptString.decrypt (PASSWORD, '"+ memberKey+"') ";
            sqlstr += "                           AS PWD, ";
            sqlstr += "                        \"NAME\" AS M_NAME, ";
            sqlstr += "                        MOBILE, ";
            sqlstr += "                        SUBSTR (NVL (REGDT, ''), 0, 10) AS REGDT, ";
            sqlstr += "                        LAST_LOGIN ";
            sqlstr += "                   FROM MEMBER ";
            sqlstr += "                  WHERE NVL (DEL_FLAG, 'n') = 'n'  ";

            if (dr["SEARCH_TYPE"].ToString() != "ALL")
            {
                if (dr["SEARCH_TYPE"].ToString() == "ID")
                {
                    sqlstr += "                  AND M_ID LIKE '%" + dr["SEARCH_DATA"].ToString() + "%' ";
                }
                else if (dr["SEARCH_TYPE"].ToString() == "NAME")
                {
                    sqlstr += "                  AND NAME LIKE '%" + dr["SEARCH_DATA"].ToString() + "%' ";
                }
                else if (dr["SEARCH_TYPE"].ToString() == "PHONE")
                {
                    sqlstr += "                  AND MOBILE LIKE '%" + dr["SEARCH_DATA"].ToString() + "%' ";
                }
            }
            else
            {
                if (dr["SEARCH_TYPE"].ToString() != "")
                {
                    sqlstr += "                  AND (M_ID LIKE '%" + dr["SEARCH_DATA"].ToString() + "%' ";

                    sqlstr += "                  OR NAME LIKE '%" + dr["SEARCH_DATA"].ToString() + "%' ";

                    sqlstr += "                  OR MOBILE LIKE '%" + dr["SEARCH_DATA"].ToString() + "%')";

                }
            }

            sqlstr += "                  ) TOTAL) ";
            sqlstr += "  WHERE PAGE = '"+ dr["PAGE"].ToString()+"' ";
   
            return sqlstr;
        }

        /// <summary>
        /// 관리자 관리 - 아이디 중복 체크
        /// </summary>
        /// <returns></returns>
        public string CheckIDMember_Query(DataRow dr)
        {
            sqlstr = "";

            sqlstr += " SELECT ";
            sqlstr += "        M_ID ";
            sqlstr += "   FROM MEMBER ";
            sqlstr += "  WHERE 1 = 1  ";
            sqlstr += "     AND M_ID = '" + dr["USR_ID"].ToString() + "'  ";

            return sqlstr;
        }

        /// <summary>
        /// 관리자 관리 - 등록 쿼리
        /// </summary>
        /// <returns></returns>
        public string InsertMember_Query(DataRow dr)
        {
            sqlstr = "";

            sqlstr += " INSERT INTO MEMBER VALUES ( ";
            sqlstr += " (SELECT NVL(MAX(MEMB_NO), 0) + 1 FROM MEMBER) ";
            sqlstr += " , '" + dr["M_ID"].ToString() + "' ";
            sqlstr += " , 50 ";   //level
            sqlstr += " , '' ";   // auth_level
            sqlstr += " , '' ";   // status
            sqlstr += " , CryptString.encrypt('" + dr["PASSWORD"].ToString() + "', '" + memberKey + "') ";
            sqlstr += " , '" + dr["NAME"].ToString() + "' ";
            sqlstr += " , '" + dr["MOBILE1"].ToString() + "-" + dr["MOBILE2"].ToString() + "-" + dr["MOBILE3"].ToString() + "' ";
            sqlstr += " , TO_CHAR(SYSDATE, 'YYYY-MM-DD hh24:mi:ss') ";
            sqlstr += " , '' ";   // last_login
            sqlstr += " , '' ";   // last_loginIP
            sqlstr += " , 0 ";   // cnt_login
            sqlstr += " , '' ";   // deldate
            sqlstr += " , '' ";   // drop_comment
            sqlstr += " , '' ";   // del_flag
            sqlstr += " ) ";

            return sqlstr;
        }

        /// <summary>
        /// 관리자 관리 - 관리자 관리 수정 검색
        /// </summary>
        /// <returns></returns>
        public string SearchMemberModify_Query(string UserID)
        {
            sqlstr = "";

            sqlstr += " SELECT MEMB_NO ";
            sqlstr += "         , M_ID ";
            sqlstr += "         , \"LEVEL\" AS LVL ";
            sqlstr += "         , AUTH_LEVEL ";
            sqlstr += "         , STATUS ";            
            sqlstr += "         , \"NAME\" AS M_NAME ";
            sqlstr += "         , MOBILE ";
            sqlstr += "         , SUBSTR(NVL(REGDT, ''), 0, 10) AS REGDT ";
            sqlstr += "         , LAST_LOGIN ";
            sqlstr += "  FROM MEMBER ";
            sqlstr += " WHERE MEMB_NO = " + UserID + " ";

            return sqlstr;
        }

        /// <summary>
        /// 관리자 관리 - 등록 쿼리
        /// </summary>
        /// <returns></returns>
        public string DeleteMember_Query(DataRow dr)
        {
            sqlstr = "";

            sqlstr += " DELETE FROM MEMBER ";
            sqlstr += " WHERE MEMB_NO = '" + dr["MEMB_NO"].ToString() + "' ";

            return sqlstr;
        }

        /// <summary>
        /// 관리자 관리 - 등록 쿼리
        /// </summary>
        /// <returns></returns>
        public string ModifyMember_Query(DataRow dr)
        {
            sqlstr = "";

            sqlstr += " UPDATE MEMBER ";
            sqlstr += " SET ";
            sqlstr += "         \"NAME\" = '" + dr["NAME"].ToString() + "' ";
            if (dr["PASSWORD"].ToString() != "")
            {
                sqlstr += "         , PASSWORD = CryptString.encrypt('" + dr["PASSWORD"].ToString() + "', '" + memberKey + "') ";
            }   
            sqlstr += "         , MOBILE = '" + dr["MOBILE1"].ToString() + "-" + dr["MOBILE2"].ToString() + "-" + dr["MOBILE3"].ToString() + "' ";
            sqlstr += "         , REGDT = TO_CHAR(SYSDATE, 'YYYY-MM-DD hh24:mi:ss') ";
            sqlstr += " WHERE MEMB_NO = '" + dr["MEMB_NO"].ToString() + "' ";            

            return sqlstr;
        }
    }
}