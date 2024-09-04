using System.Data;

namespace SSGM_ELVISPRIME_COMMON.Query.MyMenu
{
    public class Console_Query
    {
        string sqlstr;

        /// <summary>
        /// Console - 창고 업무 조회
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string fnGetConsoleData_Query(DataRow dr)
        {
            string sqlstr = "";

            sqlstr += " SELECT * ";
            sqlstr += "   FROM (  SELECT ROWNUM AS RNUM, ";
            sqlstr += "                  FLOOR ( (ROWNUM - 1) / 10 + 1) AS PAGE, ";
            sqlstr += "                  COUNT (*) OVER () AS TOTCNT, ";
            sqlstr += "                  A.* ";
            sqlstr += "	FROM (SELECT MAX(D.ACT_CUST_NM) AS ACT_CUST_NM, ";
            sqlstr += "                           MAX(D.GR_NO) AS GR_NO, ";
            sqlstr += "                           MAX(M.POD_CD)AS POD_CD, ";
            sqlstr += "                           MAX(M.POD_NM) AS POD_NM, ";
            sqlstr += "                           MAX(D.GR_YMD) AS GR_YMD, ";
            sqlstr += "                           MAX(D.MARK) AS MARK, ";
            sqlstr += "                           SUM(D.ACT_QTY) AS ACT_QTY, ";
            sqlstr += "                           SUM(D.BK_GRS_WGT) AS ACT_GRS_WGT, ";
            sqlstr += "                           SUM(D.ACT_MSRMT) AS ACT_MSRMT, ";
            sqlstr += "                           MAX(D.WH_CD) AS WH_CD, ";
            sqlstr += "                           D.BK_NO AS BK_NO, ";
            sqlstr += "                           MAX(D.BK_SEQ) AS BK_SEQ, ";
            sqlstr += "                           MAX(D.ACT_GR_CUST) AS ACT_GR_CUST ";
            sqlstr += "                      FROM CFS_BK_DTL D ";
            sqlstr += "                           JOIN CFS_BK_MST M ";
            sqlstr += "                              ON D.WH_CD = M.WH_CD AND D.BK_NO = M.BK_NO ";
            sqlstr += "                      WHERE 1=1 ";
            sqlstr += "    AND D.GR_YMD BETWEEN '" + dr["ETD"] + "' AND '" + dr["ETA"] + "' ";
             

            if (dr["USR_TYPE"].ToString() != "M")
            {
                sqlstr += "   AND M.SHP_CD = '" + dr["CUST_CD"].ToString() + "'";
            }

            if (dr["BK_NO"].ToString() != "")
            {
                sqlstr += "    AND D.BK_NO  LIKE '%" + dr["BK_NO"] + "%' ";
            }

            if(dr["POD_CD"].ToString() != "")
            {
                sqlstr += " AND (M.POD_CD = '" + dr["POD_CD"] + "' OR ((SELECT LOC_NM FROM MDM_PORT_MST WHERE LOC_CD = M.POD_CD) = UPPER('" + dr["POD"] + "'))) ";
            }

            sqlstr += "    GROUP BY D.BK_NO ";
            //if (dr["POD_CD"].ToString() == "")
            //{
            //    sqlstr += "    AND ( (UPPER('" + dr["POD"] + "') IS NULL and 1 = 1 ) or (UPPER('" + dr["POD"] + "') IS NOT NULL and M.POD_CD LIKE UPPER('%" + dr["POD"] + "%')) or (UPPER('" + dr["POD"] + "') IS NOT NULL and (SELECT LOC_NM FROM MDM_PORT_MST WHERE LOC_CD = M.POD_CD) LIKE UPPER('%" + dr["POD"] + "%') ) )";
            //}
            //else
            //{
            //    sqlstr += "    AND ( (UPPER('" + dr["POD"] + "') IS NULL and 1 = 1 ) or (UPPER('" + dr["POD"] + "') IS NOT NULL and M.POD_CD LIKE UPPER('%" + dr["POD_CD"] + "%') ) or (UPPER('" + dr["POD"] + "') IS NOT NULL and (SELECT LOC_NM FROM MDM_PORT_MST WHERE LOC_CD = M.POD_CD) LIKE UPPER('%" + dr["POD"] + "%') ) )";
            //}

            if (dr["ID"].ToString() != "")
            {
                sqlstr += " ORDER BY " + dr["ID"] + " " + dr["ORDER"] + "";
            }
            else
            {
                sqlstr += " ORDER BY GR_YMD DESC ";
            }

            sqlstr += "                   ) A ";
            sqlstr += "         ORDER BY RNUM ASC) ";
            sqlstr += "  WHERE PAGE = " + dr["PAGE"];
            
            return sqlstr;
        }

        /// <summary>
        /// Console - 파일 리스트 가져오기
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string fnLayerSetFileList_Query(DataRow dr)
        {
            string sqlstr = "";
            sqlstr += "  SELECT * FROM COM_DOC_MST  ";
            sqlstr += "        WHERE MNGT_NO = '" + dr["MNGT_NO"] + "' ";
            sqlstr += "        AND DOC_TYPE = 'ETCH'";
            return sqlstr;
        }        
    }
}
