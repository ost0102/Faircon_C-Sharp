using System.Data;

namespace SSGM_ELVISPRIME_COMMON.Query.Main
{
    class Schedule_Query
    {
        string sqlstr;

        /// <summary>
        /// 스케줄 데이터 가지고 오기 (해운)
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string GetSEAScheduleData_Query(DataRow dr)
        {
            sqlstr = "";

            sqlstr += " SELECT * ";
            sqlstr += "   FROM (SELECT ROWNUM AS RNUM, ";
            sqlstr += "                FLOOR ( (ROWNUM - 1) / 10 + 1) AS PAGE, ";
            sqlstr += "                COUNT (*) OVER () AS TOTCNT, ";
            sqlstr += "                TOTAL.*, ";
            sqlstr += "                CASE WHEN TOTAL.POL_CD LIKE 'KR%' THEN 'E' WHEN TOTAL.POL_CD NOT LIKE 'KR%' THEN 'I' END AS BOUND ";
            sqlstr += "           FROM (  SELECT A.LINE_CD, ";
            sqlstr += "                          A.SCH_NO, ";
            sqlstr += "                          A.VSL, ";
            sqlstr += "                          A.POL_CD, ";
            sqlstr += "                          (SELECT LOC_NM FROM MDM_PORT_MST WHERE LOC_CD = '" + dr["POL_CD"].ToString() + "') AS POL_TRMN, ";
            sqlstr += "                          A.POD_CD, ";
            sqlstr += "                          (SELECT LOC_NM FROM MDM_PORT_MST WHERE LOC_CD = '" + dr["POD_CD"].ToString() + "') AS POD_TRMN, ";
            sqlstr += "                          A.ETD, ";
            sqlstr += "                          A.ETA, ";
            sqlstr += "                          A.DOC_CLOSE_YMD, ";
            sqlstr += "                          SUBSTR(A.DOC_CLOSE_HM,0,4) AS DOC_CLOSE_HM, ";
            sqlstr += "                          A.CARGO_CLOSE_YMD, ";
            sqlstr += "                          SUBSTR(A.CARGO_CLOSE_HM,0,4) AS CARGO_CLOSE_HM, ";
            sqlstr += "                          A.TRANSIT_TIME, ";
            sqlstr += "                          A.TRANSIT_TIME_NM, ";
            sqlstr += "                          A.TS_CNT, ";
            sqlstr += "                          A.RMK, ";
            sqlstr += "                          CASE WHEN A.CNTR_TYPE = 'F' THEN 'FCL' WHEN A.CNTR_TYPE = 'L' THEN 'LCL' WHEN A.CNTR_TYPE = 'B' THEN 'BULK' END AS CNTR_TYPE, ";
            sqlstr += "                          A.LINE_TYPE, ";
            sqlstr += "                          A.POL_TML_NM, ";
            sqlstr += "                          A.SCH_PIC, ";
            sqlstr += "                          A.REQ_SVC, ";
            sqlstr += "                          A.VSL || ' ' || A.VOY AS VSL_VOY, ";
            sqlstr += "                          B.IMG_PATH ";
            sqlstr += "                          ,TO_CHAR (TO_DATE (A.DOC_CLOSE_YMD) - 1, 'YYYYMMDD') AS PREV_CLOSE";
            sqlstr += "                         , TO_CHAR (TO_DATE (DOC_CLOSE_YMD), 'YYYYMMDD') || TO_CHAR (TO_DATE (LPAD(DOC_CLOSE_HM,'4','0'), 'hh24miss'), 'hh24mi') AS DOC_CLOSE  ";
            sqlstr += "                    FROM PRM_SCH_MST A ";
            sqlstr += "                         INNER JOIN MDM_CARR_MST CARR ";
            sqlstr += "                            ON A.LINE_CD = CARR.CARR_CD ";
            sqlstr += "                         LEFT OUTER JOIN MDM_LINE_IMG B ";
            sqlstr += "                            ON CARR.SCAC_CD = B.LINE_CD ";
            sqlstr += "                               OR CARR.EDI_CD = B.LINE_CD ";
            sqlstr += "                         WHERE 1=1 ";
            sqlstr += "                            AND ( ('" + dr["REQ_SVC"] + "' IS NULL and 1 = 1 ) or ('" + dr["REQ_SVC"] + "'  = 'ALL' and 1 = 1 ) or ('" + dr["REQ_SVC"] + "' IS NOT NULL and A.REQ_SVC = '" + dr["REQ_SVC"] + "' ) ) ";
            sqlstr += "                            AND ETD >= '" + dr["ETD_START"].ToString() + "'";

            sqlstr += "    AND A.WEB_FLAG = 'Y' ";
            sqlstr += "    AND A.LINE_TYPE = '" + dr["LINE_TYPE"] + "'";

            if (dr["CNTR_TYPE"].ToString() != "")
            {
                sqlstr += "    AND A.CNTR_TYPE = '" + dr["CNTR_TYPE"] + "'";
            }

            if (dr["POL_CD"].ToString() == "")
            {
                sqlstr += "    AND ( (UPPER('" + dr["POL"] + "') IS NULL and 1 = 1 ) or (UPPER('" + dr["POL"] + "') IS NOT NULL and A.POL_CD LIKE UPPER('%" + dr["POL"] + "%')) or (UPPER('" + dr["POL"] + "') IS NOT NULL and (SELECT LOC_NM FROM MDM_PORT_MST WHERE LOC_CD = A.POL_CD) LIKE UPPER('%" + dr["POL"] + "%') ) )";
            }
            else
            {
                sqlstr += "    AND ( (UPPER('" + dr["POL"] + "') IS NULL and 1 = 1 ) or (UPPER('" + dr["POL"] + "') IS NOT NULL and A.POL_CD LIKE UPPER('%" + dr["POL_CD"] + "%') ) or (UPPER('" + dr["POL"] + "') IS NOT NULL and (SELECT LOC_NM FROM MDM_PORT_MST WHERE LOC_CD = A.POL_CD) LIKE UPPER('%" + dr["POL"] + "%') ) )";
            }

            if (dr["POD_CD"].ToString() == "")
            {
                sqlstr += "    AND ( (UPPER('" + dr["POD"] + "') IS NULL and 1 = 1 ) or (UPPER('" + dr["POD"] + "') IS NOT NULL and A.POD_CD LIKE UPPER('%" + dr["POD"] + "%') ) or (UPPER('" + dr["POD"] + "') IS NOT NULL and (SELECT LOC_NM FROM MDM_PORT_MST WHERE LOC_CD = A.POD_CD) LIKE UPPER('%" + dr["POD"] + "%') ) )";
            }
            else
            {
                sqlstr += "    AND ( (UPPER('" + dr["POD"] + "') IS NULL and 1 = 1 ) or (UPPER('" + dr["POD"] + "') IS NOT NULL and A.POD_CD LIKE UPPER('%" + dr["POD_CD"] + "%') ) or (UPPER('" + dr["POD"] + "') IS NOT NULL and (SELECT LOC_NM FROM MDM_PORT_MST WHERE LOC_CD = A.POD_CD) LIKE UPPER('%" + dr["POD"] + "%') ) )";
            }

            if (dr["ORDERBY"].ToString() != "")
            {
                sqlstr += "ORDER BY " + dr["ORDERBY"] + " " + dr["SORT"] + "";
            }
            else
            {
                sqlstr += "ORDER BY ETA";
            }

            sqlstr += "                 ) TOTAL ";
            sqlstr += " ) ";

            sqlstr += "  WHERE PAGE = '" + dr["PAGE"].ToString() + "' ";

            return sqlstr;
        }

        /// <summary>
        /// Liner 정보 가지고 오기 (Carr)
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string GetSEALinerData_Query(DataRow dr)
        {
            sqlstr = "";

            sqlstr += " SELECT CARR.CARR_CD , CARR.CARR_NM ";
            sqlstr += " FROM PRM_SCH_MST A INNER JOIN MDM_CARR_MST CARR ON A.LINE_CD = CARR.CARR_CD ";
            sqlstr += " WHERE 1 = 1  ";

            sqlstr += "    AND A.LINE_TYPE = '" + dr["LINE_TYPE"] + "'";

            if (dr["CNTR_TYPE"].ToString() != "")
            {
                sqlstr += "    AND A.CNTR_TYPE = '" + dr["CNTR_TYPE"] + "'";
            }

            sqlstr += " AND CARR.CARR_NM IS NOT NULL ";
            sqlstr += "        AND ( ('" + dr["REQ_SVC"] + "' IS NULL and 1 = 1 ) or ('" + dr["REQ_SVC"] + "'  = 'ALL' and 1 = 1 ) or ('" + dr["REQ_SVC"] + "' IS NOT NULL and A.REQ_SVC = '" + dr["REQ_SVC"] + "' ) ) ";            
            sqlstr += "    AND A.WEB_FLAG = 'Y' ";
            sqlstr += "        AND ETD >= '" + dr["ETD_START"].ToString() + "'";

            if (dr["POL_CD"].ToString() == "")
            {
                sqlstr += "    AND ( (UPPER('" + dr["POL"] + "') IS NULL and 1 = 1 ) or (UPPER('" + dr["POL"] + "') IS NOT NULL and A.POL_CD LIKE UPPER('%" + dr["POL"] + "%')) or (UPPER('" + dr["POL"] + "') IS NOT NULL and (SELECT LOC_NM FROM MDM_PORT_MST WHERE LOC_CD = A.POL_CD) LIKE UPPER('%" + dr["POL"] + "%') ) )";
            }
            else
            {
                sqlstr += "    AND ( (UPPER('" + dr["POL"] + "') IS NULL and 1 = 1 ) or (UPPER('" + dr["POL"] + "') IS NOT NULL and A.POL_CD LIKE UPPER('%" + dr["POL_CD"] + "%') ) or (UPPER('" + dr["POL"] + "') IS NOT NULL and (SELECT LOC_NM FROM MDM_PORT_MST WHERE LOC_CD = A.POL_CD) LIKE UPPER('%" + dr["POL"] + "%') ) )";
            }

            if (dr["POD_CD"].ToString() == "")
            {
                sqlstr += "    AND ( (UPPER('" + dr["POD"] + "') IS NULL and 1 = 1 ) or (UPPER('" + dr["POD"] + "') IS NOT NULL and A.POD_CD LIKE UPPER('%" + dr["POD"] + "%') ) or (UPPER('" + dr["POD"] + "') IS NOT NULL and (SELECT LOC_NM FROM MDM_PORT_MST WHERE LOC_CD = A.POD_CD) LIKE UPPER('%" + dr["POD"] + "%') ) )";
            }
            else
            {
                sqlstr += "    AND ( (UPPER('" + dr["POD"] + "') IS NULL and 1 = 1 ) or (UPPER('" + dr["POD"] + "') IS NOT NULL and A.POD_CD LIKE UPPER('%" + dr["POD_CD"] + "%') ) or (UPPER('" + dr["POD"] + "') IS NOT NULL and (SELECT LOC_NM FROM MDM_PORT_MST WHERE LOC_CD = A.POD_CD) LIKE UPPER('%" + dr["POD"] + "%') ) )";
            }

            sqlstr += " GROUP BY CARR.CARR_CD , CARR.CARR_NM ";

            return sqlstr;
        }

        /// <summary>
        /// Liner 체크된 해운 스케줄 가져오기
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string GetSEAChkSchedule_Query(DataRow dr)
        {
            sqlstr = "";

            sqlstr += " SELECT * ";
            sqlstr += "   FROM (SELECT ROWNUM AS RNUM, ";
            sqlstr += "                FLOOR ( (ROWNUM - 1) / 10 + 1) AS PAGE, ";
            sqlstr += "                COUNT (*) OVER () AS TOTCNT, ";
            sqlstr += "                TOTAL.*, ";
            sqlstr += "                CASE WHEN TOTAL.POL_CD LIKE 'KR%' THEN 'E' WHEN TOTAL.POL_CD NOT LIKE 'KR%' THEN 'I' END AS BOUND "; 
            sqlstr += "           FROM (  SELECT A.LINE_CD, ";
            sqlstr += "                          A.SCH_NO, ";
            sqlstr += "                          A.VSL, ";
            sqlstr += "                          A.POL_CD, ";
            sqlstr += "                          (SELECT LOC_NM ";
            sqlstr += "                             FROM MDM_PORT_MST ";
            sqlstr += "                            WHERE LOC_CD = '" + dr["POL_CD"].ToString() + "') ";
            sqlstr += "                             AS POL_TRMN, ";
            sqlstr += "                          A.POD_CD, ";
            sqlstr += "                          (SELECT LOC_NM ";
            sqlstr += "                             FROM MDM_PORT_MST ";
            sqlstr += "                            WHERE LOC_CD = '" + dr["POD_CD"].ToString() + "') ";
            sqlstr += "                             AS POD_TRMN, ";
            sqlstr += "                          A.ETD, ";
            sqlstr += "                          A.ETA, ";
            sqlstr += "                          A.DOC_CLOSE_YMD, ";
            sqlstr += "                          SUBSTR(A.DOC_CLOSE_HM,0,4) AS DOC_CLOSE_HM, ";
            sqlstr += "                          A.CARGO_CLOSE_YMD, ";
            sqlstr += "                          SUBSTR(A.CARGO_CLOSE_HM,0,4) AS CARGO_CLOSE_HM, ";
            sqlstr += "                          A.TRANSIT_TIME, ";
            sqlstr += "                          A.TRANSIT_TIME_NM, ";
            sqlstr += "                          A.TS_CNT, ";
            sqlstr += "                          A.RMK, ";
            sqlstr += "                          CASE WHEN A.CNTR_TYPE = 'F' THEN 'FCL' WHEN A.CNTR_TYPE = 'L' THEN 'LCL' WHEN A.CNTR_TYPE = 'B' THEN 'BULK' END AS CNTR_TYPE, ";
            sqlstr += "                          A.LINE_TYPE, ";
            sqlstr += "                          A.POL_TML_NM, ";
            sqlstr += "                          A.SCH_PIC, ";
            sqlstr += "                          A.REQ_SVC, ";
            sqlstr += "                          A.VSL || ' ' || A.VOY AS VSL_VOY, ";
            sqlstr += "                          B.IMG_PATH ";
            sqlstr += "                          ,TO_CHAR (TO_DATE (A.DOC_CLOSE_YMD) - 1, 'YYYYMMDD') AS PREV_CLOSE";
            sqlstr += "                         , TO_CHAR (TO_DATE (DOC_CLOSE_YMD), 'YYYYMMDD') || TO_CHAR (TO_DATE (LPAD(DOC_CLOSE_HM,'4','0'), 'hh24miss'), 'hh24mi') AS DOC_CLOSE  ";
            sqlstr += "                    FROM PRM_SCH_MST A ";
            sqlstr += "                         INNER JOIN MDM_CARR_MST CARR ";
            sqlstr += "                            ON A.LINE_CD = CARR.CARR_CD ";
            sqlstr += "                         LEFT OUTER JOIN MDM_LINE_IMG B ";
            sqlstr += "                            ON CARR.SCAC_CD = B.LINE_CD ";
            sqlstr += "                               OR CARR.EDI_CD = B.LINE_CD ";
            sqlstr += "         WHERE 1=1 ";

            sqlstr += "    AND A.WEB_FLAG = 'Y' ";
            sqlstr += "    AND A.LINE_TYPE = '" + dr["LINE_TYPE"] + "'";

            if (dr["CNTR_TYPE"].ToString() != "")
            {
                sqlstr += "    AND A.CNTR_TYPE = '" + dr["CNTR_TYPE"] + "'";
            }

            sqlstr += "        AND ( ('" + dr["REQ_SVC"] + "' IS NULL and 1 = 1 ) or ('" + dr["REQ_SVC"] + "'  = 'ALL' and 1 = 1 ) or ('" + dr["REQ_SVC"] + "' IS NOT NULL and A.REQ_SVC = '" + dr["REQ_SVC"] + "' ) ) ";           
            sqlstr += "        AND ETD >= '" + dr["ETD_START"].ToString() + "' ";

            if (dr["POL_CD"].ToString() == "")
            {
                sqlstr += "    AND ( (UPPER('" + dr["POL"] + "') IS NULL and 1 = 1 ) or (UPPER('" + dr["POL"] + "') IS NOT NULL and A.POL_CD LIKE UPPER('%" + dr["POL"] + "%')) or (UPPER('" + dr["POL"] + "') IS NOT NULL and (SELECT LOC_NM FROM MDM_PORT_MST WHERE LOC_CD = A.POL_CD) LIKE UPPER('%" + dr["POL"] + "%') ) )";
            }
            else
            {
                sqlstr += "    AND ( (UPPER('" + dr["POL"] + "') IS NULL and 1 = 1 ) or (UPPER('" + dr["POL"] + "') IS NOT NULL and A.POL_CD LIKE UPPER('%" + dr["POL_CD"] + "%') ) or (UPPER('" + dr["POL"] + "') IS NOT NULL and (SELECT LOC_NM FROM MDM_PORT_MST WHERE LOC_CD = A.POL_CD) LIKE UPPER('%" + dr["POL"] + "%') ) )";
            }

            if (dr["POD_CD"].ToString() == "")
            {
                sqlstr += "    AND ( (UPPER('" + dr["POD"] + "') IS NULL and 1 = 1 ) or (UPPER('" + dr["POD"] + "') IS NOT NULL and A.POD_CD LIKE UPPER('%" + dr["POD"] + "%') ) or (UPPER('" + dr["POD"] + "') IS NOT NULL and (SELECT LOC_NM FROM MDM_PORT_MST WHERE LOC_CD = A.POD_CD) LIKE UPPER('%" + dr["POD"] + "%') ) )";
            }
            else
            {
                sqlstr += "    AND ( (UPPER('" + dr["POD"] + "') IS NULL and 1 = 1 ) or (UPPER('" + dr["POD"] + "') IS NOT NULL and A.POD_CD LIKE UPPER('%" + dr["POD_CD"] + "%') ) or (UPPER('" + dr["POD"] + "') IS NOT NULL and (SELECT LOC_NM FROM MDM_PORT_MST WHERE LOC_CD = A.POD_CD) LIKE UPPER('%" + dr["POD"] + "%') ) )";
            }

            if (dr["LINE_CD"].ToString() == "All")
            {
                sqlstr += " ";
            }
            else
            {
                sqlstr += " AND A.LINE_CD IN (" + dr["LINE_CD"].ToString() + ")";
            }

            if (dr["TS"].ToString() == "N")
            {
                sqlstr += " AND A.TS_CNT IS NULL ";
            }
            else if (dr["TS"].ToString() == "T")
            {
                sqlstr += " AND A.TS_CNT = '0'";
            }
            else if (dr["TS"].ToString() == "D")
            {
                sqlstr += " AND A.TS_CNT = '1'";
            }

            if (dr["ORDERBY"].ToString() != "")
            {
                sqlstr += " ORDER BY " + dr["ORDERBY"] + " " + dr["SORT"] + "";
            }
            else
            {
                sqlstr += " ORDER BY ETA";
            }

            sqlstr += "                 ) TOTAL ";
            sqlstr += " ) ";
            sqlstr += "  WHERE PAGE = '" + dr["PAGE"].ToString() + "' ";

            return sqlstr;
        }

        /// <summary>
        /// 스케줄 데이터 가지고 오기 (항공)
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string GetAIRScheduleData_Query(DataRow dr)
        {
            sqlstr = "";

            sqlstr += " SELECT * ";
            sqlstr += "   FROM (SELECT ROWNUM AS RNUM, ";
            sqlstr += "                FLOOR ( (ROWNUM - 1) / 10 + 1) AS PAGE, ";
            sqlstr += "                COUNT (*) OVER () AS TOTCNT, ";
            sqlstr += "                TOTAL.*, ";
            sqlstr += "                CASE WHEN (SELECT LOC_CD FROM MDM_PORT_MST WHERE CTRY_CD = 'KR' AND LOC_TYPE = 'A' AND LOC_CD = TOTAL.POL_CD) IS NOT NULL THEN 'E' ELSE 'I' END AS BOUND "; //TWKIM 20210720 수출입 확인
            sqlstr += "           FROM (  SELECT A.LINE_CD, ";
            sqlstr += "                          A.SCH_NO, ";
            sqlstr += "                          A.VSL, ";
            sqlstr += "                          A.POL_CD, ";
            sqlstr += "                          (SELECT LOC_NM FROM MDM_PORT_MST WHERE LOC_CD = '" + dr["POL_CD"].ToString() + "') AS POL_TRMN, ";
            sqlstr += "                          A.POD_CD, ";
            sqlstr += "                          (SELECT LOC_NM FROM MDM_PORT_MST WHERE LOC_CD = '" + dr["POD_CD"].ToString() + "') AS POD_TRMN, ";
            sqlstr += "                          A.ETD, ";
            sqlstr += "                          A.ETD_HM, ";            
            sqlstr += "                          A.ETA, ";
            sqlstr += "                          A.ETA_HM, ";            
            sqlstr += "                          A.DOC_CLOSE_YMD, ";
            sqlstr += "                          SUBSTR(A.DOC_CLOSE_HM,0,4) AS DOC_CLOSE_HM, ";
            sqlstr += "                          A.CARGO_CLOSE_YMD, ";
            sqlstr += "                          SUBSTR(A.CARGO_CLOSE_HM,0,4) AS CARGO_CLOSE_HM, ";
            sqlstr += "                          A.TRANSIT_TIME, ";
            sqlstr += "                          A.TRANSIT_TIME_NM, ";
            sqlstr += "                          A.TS_CNT, ";
            sqlstr += "                          A.RMK, ";
            sqlstr += "                          CASE WHEN A.CNTR_TYPE = 'F' THEN 'FCL' WHEN A.CNTR_TYPE = 'L' THEN 'LCL' WHEN A.CNTR_TYPE = 'B' THEN 'BULK' END AS CNTR_TYPE, ";
            sqlstr += "                          A.LINE_TYPE, ";
            sqlstr += "                          A.POL_TML_NM, ";
            sqlstr += "                          A.SCH_PIC, ";
            sqlstr += "                          A.REQ_SVC, ";
            sqlstr += "                          A.VSL || ' ' || A.VOY AS VSL_VOY, ";
            sqlstr += "                          B.IMG_PATH ";
            sqlstr += "                          ,TO_CHAR (TO_DATE (A.DOC_CLOSE_YMD) - 1, 'YYYYMMDD') AS PREV_CLOSE";
            sqlstr += "                         , TO_CHAR (TO_DATE (DOC_CLOSE_YMD), 'YYYYMMDD') || TO_CHAR (TO_DATE (LPAD(DOC_CLOSE_HM,'4','0'), 'hh24miss'), 'hh24mi') AS DOC_CLOSE  ";
            sqlstr += "                    FROM PRM_SCH_MST A ";
            sqlstr += "                         INNER JOIN MDM_CARR_MST CARR ";
            sqlstr += "                            ON A.LINE_CD = CARR.CARR_CD ";
            sqlstr += "                         LEFT OUTER JOIN MDM_LINE_IMG B ";
            sqlstr += "                            ON A.LINE_CD = B.LINE_CD ";
            sqlstr += "                         WHERE 1=1 ";
            sqlstr += "                            AND ( ('" + dr["REQ_SVC"] + "' IS NULL and 1 = 1 ) or ('" + dr["REQ_SVC"] + "'  = 'ALL' and 1 = 1 ) or ('" + dr["REQ_SVC"] + "' IS NOT NULL and A.REQ_SVC = '" + dr["REQ_SVC"] + "' ) ) ";
            sqlstr += "                            AND ETD >= '" + dr["ETD_START"].ToString() + "'";
            sqlstr += "    AND A.WEB_FLAG = 'Y' "; 
            if (dr["POL_CD"].ToString() == "")
            {
                sqlstr += "    AND ( (UPPER('" + dr["POL"] + "') IS NULL and 1 = 1 ) or (UPPER('" + dr["POL"] + "') IS NOT NULL and A.POL_CD LIKE UPPER('%" + dr["POL"] + "%')) or (UPPER('" + dr["POL"] + "') IS NOT NULL and (SELECT LOC_NM FROM MDM_PORT_MST WHERE LOC_CD = A.POL_CD) LIKE UPPER('%" + dr["POL"] + "%') ) )";
            }
            else
            {
                sqlstr += "    AND ( (UPPER('" + dr["POL"] + "') IS NULL and 1 = 1 ) or (UPPER('" + dr["POL"] + "') IS NOT NULL and A.POL_CD LIKE UPPER('%" + dr["POL_CD"] + "%') ) or (UPPER('" + dr["POL"] + "') IS NOT NULL and (SELECT LOC_NM FROM MDM_PORT_MST WHERE LOC_CD = A.POL_CD) LIKE UPPER('%" + dr["POL"] + "%') ) )";
            }

            if (dr["POD_CD"].ToString() == "")
            {
                sqlstr += "    AND ( (UPPER('" + dr["POD"] + "') IS NULL and 1 = 1 ) or (UPPER('" + dr["POD"] + "') IS NOT NULL and A.POD_CD LIKE UPPER('%" + dr["POD"] + "%') ) or (UPPER('" + dr["POD"] + "') IS NOT NULL and (SELECT LOC_NM FROM MDM_PORT_MST WHERE LOC_CD = A.POD_CD) LIKE UPPER('%" + dr["POD"] + "%') ) )";
            }
            else
            {
                sqlstr += "    AND ( (UPPER('" + dr["POD"] + "') IS NULL and 1 = 1 ) or (UPPER('" + dr["POD"] + "') IS NOT NULL and A.POD_CD LIKE UPPER('%" + dr["POD_CD"] + "%') ) or (UPPER('" + dr["POD"] + "') IS NOT NULL and (SELECT LOC_NM FROM MDM_PORT_MST WHERE LOC_CD = A.POD_CD) LIKE UPPER('%" + dr["POD"] + "%') ) )";
            }

            if (dr["ORDERBY"].ToString() != "")
            {
                sqlstr += "ORDER BY " + dr["ORDERBY"] + " " + dr["SORT"] + "";
            }
            else
            {
                sqlstr += "ORDER BY ETA";
            }

            sqlstr += "                 ) TOTAL ";      
            sqlstr += " ) ";
            sqlstr += "  WHERE PAGE = '" + dr["PAGE"].ToString() + "' ";

            return sqlstr;
        }

        /// <summary>
        /// Liner 정보 가지고 오기 (Carr)
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string GetAIRLinerData_Query(DataRow dr)
        {
            sqlstr = "";

            sqlstr += " SELECT CARR.CARR_CD , CARR.CARR_NM ";
            sqlstr += " FROM PRM_SCH_MST A INNER JOIN MDM_CARR_MST CARR ON A.LINE_CD = CARR.CARR_CD ";
            sqlstr += " WHERE 1 = 1  ";
            sqlstr += " AND CARR.CARR_NM IS NOT NULL ";
            sqlstr += "        AND ( ('" + dr["REQ_SVC"] + "' IS NULL and 1 = 1 ) or ('" + dr["REQ_SVC"] + "'  = 'ALL' and 1 = 1 ) or ('" + dr["REQ_SVC"] + "' IS NOT NULL and A.REQ_SVC = '" + dr["REQ_SVC"] + "' ) ) ";            
            sqlstr += "        AND ETD >= '" + dr["ETD_START"].ToString() + "' ";
            sqlstr += "    AND A.WEB_FLAG = 'Y' "; 

            if (dr["POL_CD"].ToString() == "")
            {
                sqlstr += "    AND ( (UPPER('" + dr["POL"] + "') IS NULL and 1 = 1 ) or (UPPER('" + dr["POL"] + "') IS NOT NULL and A.POL_CD LIKE UPPER('%" + dr["POL"] + "%')) or (UPPER('" + dr["POL"] + "') IS NOT NULL and (SELECT LOC_NM FROM MDM_PORT_MST WHERE LOC_CD = A.POL_CD) LIKE UPPER('%" + dr["POL"] + "%') ) )";
            }
            else
            {
                sqlstr += "    AND ( (UPPER('" + dr["POL"] + "') IS NULL and 1 = 1 ) or (UPPER('" + dr["POL"] + "') IS NOT NULL and A.POL_CD LIKE UPPER('%" + dr["POL_CD"] + "%') ) or (UPPER('" + dr["POL"] + "') IS NOT NULL and (SELECT LOC_NM FROM MDM_PORT_MST WHERE LOC_CD = A.POL_CD) LIKE UPPER('%" + dr["POL"] + "%') ) )";
            }

            if (dr["POD_CD"].ToString() == "")
            {
                sqlstr += "    AND ( (UPPER('" + dr["POD"] + "') IS NULL and 1 = 1 ) or (UPPER('" + dr["POD"] + "') IS NOT NULL and A.POD_CD LIKE UPPER('%" + dr["POD"] + "%') ) or (UPPER('" + dr["POD"] + "') IS NOT NULL and (SELECT LOC_NM FROM MDM_PORT_MST WHERE LOC_CD = A.POD_CD) LIKE UPPER('%" + dr["POD"] + "%') ) )";
            }
            else
            {
                sqlstr += "    AND ( (UPPER('" + dr["POD"] + "') IS NULL and 1 = 1 ) or (UPPER('" + dr["POD"] + "') IS NOT NULL and A.POD_CD LIKE UPPER('%" + dr["POD_CD"] + "%') ) or (UPPER('" + dr["POD"] + "') IS NOT NULL and (SELECT LOC_NM FROM MDM_PORT_MST WHERE LOC_CD = A.POD_CD) LIKE UPPER('%" + dr["POD"] + "%') ) )";
            }

            sqlstr += " GROUP BY CARR.CARR_CD , CARR.CARR_NM ";

            return sqlstr;
        }

        /// <summary>
        /// Liner 체크된 항공 스케줄 가져오기
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string GetAIRChkSchedule_Query(DataRow dr)
        {
            sqlstr = "";

            sqlstr += " SELECT * ";
            sqlstr += "   FROM (SELECT ROWNUM AS RNUM, ";
            sqlstr += "                FLOOR ( (ROWNUM - 1) / 10 + 1) AS PAGE, ";
            sqlstr += "                COUNT (*) OVER () AS TOTCNT, ";
            sqlstr += "                TOTAL.*, ";
            sqlstr += "                CASE WHEN (SELECT LOC_CD FROM MDM_PORT_MST WHERE CTRY_CD = 'KR' AND LOC_TYPE = 'A' AND LOC_CD = TOTAL.POL_CD) IS NOT NULL THEN 'E' ELSE 'I' END AS BOUND "; //TWKIM 20210720 수출입 확인
            sqlstr += "           FROM (  SELECT A.LINE_CD, ";
            sqlstr += "                          A.SCH_NO, ";
            sqlstr += "                          A.VSL, ";
            sqlstr += "                          A.POL_CD, ";
            sqlstr += "                          (SELECT LOC_NM ";
            sqlstr += "                             FROM MDM_PORT_MST ";
            sqlstr += "                            WHERE LOC_CD = '" + dr["POL_CD"].ToString() + "') ";
            sqlstr += "                             AS POL_TRMN, ";
            sqlstr += "                          A.POD_CD, ";
            sqlstr += "                          (SELECT LOC_NM ";
            sqlstr += "                             FROM MDM_PORT_MST ";
            sqlstr += "                            WHERE LOC_CD = '" + dr["POD_CD"].ToString() + "') ";
            sqlstr += "                             AS POD_TRMN, ";
            sqlstr += "                          A.ETD, ";
            sqlstr += "                          A.ETD_HM, ";            
            sqlstr += "                          A.ETA, ";
            sqlstr += "                          A.ETA_HM, ";            
            sqlstr += "                          A.DOC_CLOSE_YMD, ";
            sqlstr += "                          SUBSTR(A.DOC_CLOSE_HM,0,4) AS DOC_CLOSE_HM, ";
            sqlstr += "                          A.CARGO_CLOSE_YMD, ";
            sqlstr += "                          SUBSTR(A.CARGO_CLOSE_HM,0,4) AS CARGO_CLOSE_HM, ";
            sqlstr += "                          A.TRANSIT_TIME, ";
            sqlstr += "                          A.TRANSIT_TIME_NM, ";
            sqlstr += "                          A.TS_CNT, ";
            sqlstr += "                          A.RMK, ";
            sqlstr += "                          CASE WHEN A.CNTR_TYPE = 'F' THEN 'FCL' WHEN A.CNTR_TYPE = 'L' THEN 'LCL' WHEN A.CNTR_TYPE = 'B' THEN 'BULK' END AS CNTR_TYPE, ";
            sqlstr += "                          A.LINE_TYPE, ";
            sqlstr += "                          A.POL_TML_NM, ";
            sqlstr += "                          A.SCH_PIC, ";
            sqlstr += "                          A.REQ_SVC, ";
            sqlstr += "                          A.VSL || ' ' || A.VOY AS VSL_VOY, ";
            sqlstr += "                          B.IMG_PATH ";
            sqlstr += "                          ,TO_CHAR (TO_DATE (A.DOC_CLOSE_YMD) - 1, 'YYYYMMDD') AS PREV_CLOSE";
            sqlstr += "                         , TO_CHAR (TO_DATE (DOC_CLOSE_YMD), 'YYYYMMDD') || TO_CHAR (TO_DATE (LPAD(DOC_CLOSE_HM,'4','0'), 'hh24miss'), 'hh24mi') AS DOC_CLOSE  ";
            sqlstr += "                    FROM PRM_SCH_MST A ";
            sqlstr += "                         INNER JOIN MDM_CARR_MST CARR ";
            sqlstr += "                            ON A.LINE_CD = CARR.CARR_CD ";
            sqlstr += "                         LEFT OUTER JOIN MDM_LINE_IMG B ";
            sqlstr += "                            ON A.LINE_CD = B.LINE_CD ";
            sqlstr += "         WHERE 1=1 ";
            sqlstr += "        AND ( ('" + dr["REQ_SVC"] + "' IS NULL and 1 = 1 ) or ('" + dr["REQ_SVC"] + "'  = 'ALL' and 1 = 1 ) or ('" + dr["REQ_SVC"] + "' IS NOT NULL and A.REQ_SVC = '" + dr["REQ_SVC"] + "' ) ) ";            
            sqlstr += "        AND ETD >= '" + dr["ETD_START"].ToString() + "' ";
            sqlstr += "    AND A.WEB_FLAG = 'Y' ";

            if (dr["POL_CD"].ToString() == "")
            {
                sqlstr += "    AND ( (UPPER('" + dr["POL"] + "') IS NULL and 1 = 1 ) or (UPPER('" + dr["POL"] + "') IS NOT NULL and A.POL_CD LIKE UPPER('%" + dr["POL"] + "%')) or (UPPER('" + dr["POL"] + "') IS NOT NULL and (SELECT LOC_NM FROM MDM_PORT_MST WHERE LOC_CD = A.POL_CD) LIKE UPPER('%" + dr["POL"] + "%') ) )";
            }
            else
            {
                sqlstr += "    AND ( (UPPER('" + dr["POL"] + "') IS NULL and 1 = 1 ) or (UPPER('" + dr["POL"] + "') IS NOT NULL and A.POL_CD LIKE UPPER('%" + dr["POL_CD"] + "%') ) or (UPPER('" + dr["POL"] + "') IS NOT NULL and (SELECT LOC_NM FROM MDM_PORT_MST WHERE LOC_CD = A.POL_CD) LIKE UPPER('%" + dr["POL"] + "%') ) )";
            }

            if (dr["POD_CD"].ToString() == "")
            {
                sqlstr += "    AND ( (UPPER('" + dr["POD"] + "') IS NULL and 1 = 1 ) or (UPPER('" + dr["POD"] + "') IS NOT NULL and A.POD_CD LIKE UPPER('%" + dr["POD"] + "%') ) or (UPPER('" + dr["POD"] + "') IS NOT NULL and (SELECT LOC_NM FROM MDM_PORT_MST WHERE LOC_CD = A.POD_CD) LIKE UPPER('%" + dr["POD"] + "%') ) )";
            }
            else
            {
                sqlstr += "    AND ( (UPPER('" + dr["POD"] + "') IS NULL and 1 = 1 ) or (UPPER('" + dr["POD"] + "') IS NOT NULL and A.POD_CD LIKE UPPER('%" + dr["POD_CD"] + "%') ) or (UPPER('" + dr["POD"] + "') IS NOT NULL and (SELECT LOC_NM FROM MDM_PORT_MST WHERE LOC_CD = A.POD_CD) LIKE UPPER('%" + dr["POD"] + "%') ) )";
            }

            if (dr["LINE_CD"].ToString() == "All")
            {
                sqlstr += " ";
            }
            else
            {
                sqlstr += " AND A.LINE_CD IN (" + dr["LINE_CD"].ToString() + ")";
            }

            if (dr["TS"].ToString() == "N")
            {
                sqlstr += " AND A.TS_CNT IS NULL ";
            }
            else if (dr["TS"].ToString() == "T")
            {
                sqlstr += " AND A.TS_CNT = '0'";
            }
            else if (dr["TS"].ToString() == "D")
            {
                sqlstr += " AND A.TS_CNT = '1'";
            }

            if (dr["ORDERBY"].ToString() != "")
            {
                sqlstr += " ORDER BY " + dr["ORDERBY"] + " " + dr["SORT"] + "";
            }
            else
            {
                sqlstr += " ORDER BY ETA";
            }

            sqlstr += "                 ) TOTAL ";
            sqlstr += " ) ";

            sqlstr += "  WHERE PAGE = '" + dr["PAGE"].ToString() + "' ";

            return sqlstr;
        }
    }
}
