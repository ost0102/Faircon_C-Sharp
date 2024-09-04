using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SSGM_ELVISPRIME_COMMON.Query.Admin
{
    public class Port_Query
    {
        string sqlstr;

        /// <summary>
        /// 관리자 관리 - 검색
        /// </summary>
        /// <returns></returns>
        public string SearchPort_Query(DataRow dr)
        {
            sqlstr = "";

            sqlstr += " SELECT * ";
            sqlstr += "   FROM (SELECT ROWNUM AS RNUM, ";
            sqlstr += "                FLOOR ( (ROWNUM - 1) / 10 + 1) AS PAGE, ";
            sqlstr += "                COUNT (*) OVER () AS TOTCNT, ";
            sqlstr += "                TOTAL.* ";
            sqlstr += "           FROM (SELECT PORT_NO, ";
            sqlstr += "                        START_PORT, ";
            sqlstr += "                        BOUND_PORT, ";
            sqlstr += "                        LOC_NM, ";
            sqlstr += "                        LOC_CD, ";
            sqlstr += "                        USE_YN, ";
            sqlstr += "                        BKG_YN, ";
            sqlstr += "                        INS_USR, ";
            sqlstr += "                        INS_YMD ";
            sqlstr += "                   FROM PORT_MST ";
            sqlstr += "                   WHERE 1=1 ";
            if (dr["SEARCH_TYPE"].ToString() != "ALL")
            {
                if (dr["SEARCH_TYPE"].ToString() == "START_PORT")
                {
                    sqlstr += "                  AND START_PORT LIKE UPPER('%" + dr["SEARCH_DATA"].ToString() + "%') ";
                }
                else if (dr["SEARCH_TYPE"].ToString() == "BOUND_PORT")
                {
                    sqlstr += "                  AND BOUND_PORT LIKE UPPER('%" + dr["SEARCH_DATA"].ToString() + "%') ";
                }
                else if (dr["SEARCH_TYPE"].ToString() == "LOC_NM")
                {
                    sqlstr += "                  AND LOC_NM LIKE UPPER('%" + dr["SEARCH_DATA"].ToString() + "%') ";
                }
                else if (dr["SEARCH_TYPE"].ToString() == "LOC_CD")
                {
                    sqlstr += "                  AND LOC_CD LIKE UPPER('%" + dr["SEARCH_DATA"].ToString() + "%') ";
                }
                else if (dr["SEARCH_TYPE"].ToString() == "USE_YN")
                {
                    sqlstr += "                  AND USE_YN LIKE UPPER('%" + dr["SEARCH_DATA"].ToString() + "%') ";
                }
                else if (dr["SEARCH_TYPE"].ToString() == "BKG_YN")
                {
                    sqlstr += "                  AND BKG_YN LIKE UPPER('%" + dr["SEARCH_DATA"].ToString() + "%') ";
                }
            }
            else
            {
                if (dr["SEARCH_TYPE"].ToString() != "")
                {
                    sqlstr += "                  AND (LOC_NM LIKE UPPER('%" + dr["SEARCH_DATA"].ToString() + "%') ";

                    sqlstr += "                  OR START_PORT LIKE UPPER('%" + dr["SEARCH_DATA"].ToString() + "%') ";

                    sqlstr += "                  OR LOC_CD LIKE UPPER('%" + dr["SEARCH_DATA"].ToString() + "%') ";

                    sqlstr += "                  OR USE_YN LIKE UPPER('%" + dr["SEARCH_DATA"].ToString() + "%') ";

                    sqlstr += "                  OR BKG_YN LIKE UPPER('%" + dr["SEARCH_DATA"].ToString() + "%') ";

                    sqlstr += "                  OR BOUND_PORT LIKE UPPER('%" + dr["SEARCH_DATA"].ToString() + "%'))";

                }
            }

            sqlstr += "  ORDER BY INS_YMD DESC , INS_HM DESC  ) TOTAL) ";
            sqlstr += "  WHERE PAGE = '" + dr["PAGE"].ToString() + "' ";
            return sqlstr;
        }

        /// <summary>
        /// 관리자 관리 - 아이디 중복 체크
        /// </summary>
        /// <returns></returns>
        public string CheckPort_Query(DataRow dr)
        {
            sqlstr = "";

            sqlstr += " SELECT ";
            sqlstr += "        LOC_CD ";
            sqlstr += "   FROM PORT_MST ";
            sqlstr += "  WHERE 1 = 1  ";
            sqlstr += "     AND LOC_CD = '" + dr["LOC_CD"].ToString() + "'  ";
            sqlstr += "     AND START_PORT = '" + dr["START_PORT"].ToString() + "'  ";

            return sqlstr;
        }

        /// <summary>
        /// 포트 관리 - 등록 쿼리
        /// </summary>
        /// <returns></returns> 
        public string InsertPort_Query(DataRow dr)
        {
            sqlstr = "";

            sqlstr += " INSERT INTO PORT_MST VALUES( ";
            sqlstr += " (SELECT NVL(MAX(PORT_NO), 0) + 1 FROM PORT_MST) ";
            sqlstr += " , '" + dr["START_PORT"].ToString() + "' ";
            sqlstr += " ,'" + dr["BOUND_PORT"].ToString() + "' ";
            sqlstr += " ,'" + dr["LOC_CD"].ToString() + "' ";
            sqlstr += " ,'" + dr["LOC_NM"].ToString() + "' ";
            sqlstr += " ,'" + dr["USE_YN"].ToString() + "' ";
            sqlstr += " ,'" + dr["INS_USR"].ToString() + "' ";
            sqlstr += " ,TO_CHAR(SYSDATE,'YYYYMMDD') ";
            sqlstr += " ,TO_CHAR(SYSDATE,'HH24MISS') ";
            sqlstr += " ,'" + dr["INS_USR"].ToString() + "' ";
            sqlstr += " ,TO_CHAR(SYSDATE,'YYYYMMDD') ";
            sqlstr += " ,TO_CHAR(SYSDATE,'HH24MISS') ";
            sqlstr += " ,(SELECT SEQ FROM PORT_MST WHERE BOUND_PORT = '" + dr["BOUND_PORT"].ToString() + "' AND START_PORT = '" + dr["START_PORT"].ToString() + "' AND ROWNUM = 1) ";
            sqlstr += " ,'" + dr["BKG_YN"].ToString() + "') ";

            return sqlstr;
        }

        /// <summary>
        /// 관리자 관리 - 관리자 관리 수정 검색
        /// </summary>
        /// <returns></returns>
        public string SearchPortModify_Query(string PortNo)
        {
            sqlstr = "";

            sqlstr += " SELECT *";
            sqlstr += "  FROM PORT_MST ";
            sqlstr += " WHERE PORT_NO = " + PortNo + " ";

            return sqlstr;
        }

        /// <summary>
        /// 주소 (시,도) 가져오기
        /// </summary>
        /// <returns></returns>
        public string SetStartPort_Query()
        {
            sqlstr = "";

            sqlstr += " SELECT START_PORT ";
            sqlstr += "     FROM PORT_MST ";

            return sqlstr;
        }


        /// <summary>
        /// 주소 (시,도) 가져오기
        /// </summary>
        /// <returns></returns>
        public string SetBoundPort_Query()
        {
            sqlstr = "";

            sqlstr += " SELECT BOUND_PORT ";
            sqlstr += "     FROM PORT_MST ";

            return sqlstr;
        }

        /// <summary>
        /// 주소 (시,도) 가져오기
        /// </summary>
        /// <returns></returns>
        public string SetUsePort_Query()
        {
            sqlstr = "";

            sqlstr += " SELECT USE_YN ";
            sqlstr += "     FROM PORT_MST ";

            return sqlstr;
        }
        /// <summary>
        /// 관리자 관리 - 등록 쿼리
        /// </summary>
        /// <returns></returns>
        public string DeletePort_Query(DataRow dr)
        {
            sqlstr = "";

            sqlstr += " DELETE FROM PORT_MST ";
            sqlstr += " WHERE PORT_NO = '" + dr["PORT_NO"].ToString() + "' ";

            return sqlstr;
        }

        /// <summary>
        /// 관리자 관리 - 등록 쿼리
        /// </summary>
        /// <returns></returns>
        public string ModifyPort_Query(DataRow dr)
        {
            sqlstr = "";

            sqlstr += " UPDATE PORT_MST ";
            sqlstr += " SET ";
            sqlstr += "         START_PORT = '" + dr["START_PORT"].ToString() + "' ";
            sqlstr += ",         BOUND_PORT = '" + dr["BOUND_PORT"].ToString() + "' ";
            sqlstr += ",         LOC_CD = '" + dr["LOC_CD"].ToString() + "' ";
            sqlstr += ",         LOC_NM = '" + dr["LOC_NM"].ToString() + "' ";
            sqlstr += ",         USE_YN = '" + dr["USE_YN"].ToString() + "' ";
            sqlstr += ",         SEQ = (SELECT SEQ FROM PORT_MST WHERE BOUND_PORT = '" + dr["BOUND_PORT"].ToString() + "' AND START_PORT = '" + dr["START_PORT"].ToString() + "' AND ROWNUM = 1) ";
            sqlstr += ",         BKG_YN = '" + dr["BKG_YN"].ToString() + "' ";
            sqlstr += " WHERE PORT_NO = '" + dr["PORT_NO"].ToString() + "' ";

            return sqlstr;
        }
    }
}