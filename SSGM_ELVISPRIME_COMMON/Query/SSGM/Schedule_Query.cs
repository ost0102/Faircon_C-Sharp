using System;
using System.Data;

namespace SSGM_ELVISPRIME_COMMON.Query.SSGM
{
    class Schedule_Query
    {
        string sqlstr;

        public string GetArrivePort_Query(DataRow dr)
        {
            sqlstr = "";
            sqlstr += "  SELECT PORT_NO , START_PORT, BOUND_PORT, LOC_CD, LOC_NM, USE_YN,INS_YMD,SEQ ";
            sqlstr += "  FROM PORT_MST ";
            sqlstr += "  WHERE START_PORT = '" + dr["START_PORT"] + "' ";
            sqlstr += "  AND USE_YN = 'Y' ";
            sqlstr += "  ORDER BY SEQ ,INS_YMD, INS_HM ASC ";


            return sqlstr;
        }
        public string GetArrivePort_BKG_Query(DataRow dr)
        {
            sqlstr = "";
            sqlstr += "  SELECT PORT_NO , START_PORT, BOUND_PORT, LOC_CD, LOC_NM, USE_YN,INS_YMD,SEQ ";
            sqlstr += "  FROM PORT_MST ";
            sqlstr += "  WHERE START_PORT = '" + dr["START_PORT"] + "' ";
            sqlstr += "  AND USE_YN = 'Y' ";
            sqlstr += "  AND BKG_YN = 'Y' ";
            sqlstr += "  ORDER BY SEQ ,INS_YMD, INS_HM ASC ";


            return sqlstr;
        }
        
        public string GeWareArrivePort_Query(DataRow dr)
        {
            sqlstr = "";
            sqlstr += "          SELECT LOC_CD";
            sqlstr += "                    , LOC_NM";
            sqlstr += "                    , (SELECT MAX(BOUND_PORT) FROM PORT_MST WHERE LOC_CD = A.LOC_CD) AS BOUND_PORT";
            sqlstr += "                    , MIN(SEQ) AS SEQ";
            sqlstr += "                    , INS_YMD";
            sqlstr += "                    , MIN(INS_HM) AS INS_HM";
            sqlstr += "          FROM PORT_MST A";
            sqlstr += "          GROUP BY  LOC_CD , LOC_NM , INS_YMD";
            sqlstr += "          ORDER BY SEQ , INS_YMD , INS_HM ";

            return sqlstr;
        }



        

        public string GetSchData_Query(DataRow dr)
        {
            sqlstr = "";

            sqlstr = "  SELECT * FROM (";
            sqlstr += "  SELECT  ROWNUM AS RNUM";
            sqlstr += " , FLOOR ( (ROWNUM - 1) / 10 + 1) AS PAGE ";
            sqlstr += " , COUNT (*) OVER () AS TOTCNT	";
            sqlstr += " , SCH_NO	";
            sqlstr += " , LINE_CD	";
            sqlstr += " , LINE_NM	";
            sqlstr += " , IMG_PATH	";
            sqlstr += " , REQ_SVC	";
            sqlstr += " , OFFICE_CD	";
            sqlstr += " , VSL_VOY	";
            sqlstr += " , VSL";
            sqlstr += " , VOY";
            sqlstr += " , SCH_PIC	";
            sqlstr += " , POL_CD	";
            sqlstr += " , POL_NM	";
            sqlstr += " , POD_CD	";
            sqlstr += " , POD_NM	";
            sqlstr += " , POL_TML_NM	";
            sqlstr += " , ETD	";
            sqlstr += " , ETD_HM	";
            sqlstr += " , ETA	";
            sqlstr += " , ETA_HM	";
            sqlstr += " , DOC_CLOSE_YMD ";
            sqlstr += " , DOC_CLOSE_HM";
            sqlstr += " , TS_CNT	";
            sqlstr += " , REPLACE(RMK,CHR(13) || CHR(10) , '<BR>') AS RMK	";
            sqlstr += " , TRANSIT_TIME	";
            sqlstr += " , TRANSIT_TIME_NM	";
            sqlstr += " , TS_DTL	";
            sqlstr += " , TS_COUNT	";
            sqlstr += " , CARGO_CLOSE_YMD ";
            sqlstr += " , CARGO_CLOSE_HM ";
            sqlstr += " , CNTR_TYPE ";
            sqlstr += " , PREV_CLOSE ";
            sqlstr += " , EX_IM_TYPE ";
            sqlstr += " , BKG_YN ";
            sqlstr += " , USE_YN ";
            sqlstr += "         FROM(";
            sqlstr += "  SELECT SCH_NO";
            sqlstr += "           , OFFICE_CD";
            sqlstr += "           , REQ_SVC";
            sqlstr += "           , A.LINE_CD";
            sqlstr += "           , IMG_PATH";
            sqlstr += "           , (SELECT CARR_NM";
            sqlstr += "               FROM MDM_CARR_MST";
            sqlstr += "              WHERE CARR_CD = A.LINE_CD)";
            sqlstr += "              AS LINE_NM";
            sqlstr += "           , VSL || ' ' ||VOY AS VSL_VOY";
            sqlstr += "           , VSL";
            sqlstr += "           , VOY";
            sqlstr += "           , POL_CD";
            sqlstr += "           , (SELECT LOC_NM";
            sqlstr += "               FROM MDM_PORT_MST";
            sqlstr += "             WHERE LOC_CD = POL_CD)";
            sqlstr += "              AS POL_NM";
            sqlstr += "            , POL_TML_NM";
            sqlstr += "            , POD_CD";
            sqlstr += "            , (SELECT LOC_NM";
            sqlstr += "                FROM MDM_PORT_MST";
            sqlstr += "              WHERE LOC_CD = POD_CD)";
            sqlstr += "               AS POD_NM";
            sqlstr += "            , ETD";
            sqlstr += "            , ETD_HM ";
            sqlstr += "            , ETA";
            sqlstr += "            , ETA_HM";
            sqlstr += "            , A.DOC_CLOSE_YMD ";
            sqlstr += "            , SUBSTR(A.DOC_CLOSE_HM,0,4) AS DOC_CLOSE_HM ";
            sqlstr += "            , A.CARGO_CLOSE_YMD ";
            sqlstr += "            , SUBSTR(A.CARGO_CLOSE_HM,0,4) AS CARGO_CLOSE_HM ";
            sqlstr += "            , TO_CHAR (TO_DATE (DOC_CLOSE_YMD) - 1, 'YYYYMMDD') AS PREV_CLOSE";
            sqlstr += "            , TS_CNT ";
            sqlstr += "            , TRANSIT_TIME";
            sqlstr += "            , TRANSIT_TIME_NM";
            sqlstr += "            , SCH_PIC";
            sqlstr += "            , (SELECT BKG_YN FROM PORT_MST@DL_SSGM WHERE LOC_CD = A.POD_CD AND START_PORT = A.POL_CD) AS BKG_YN";
            sqlstr += "            , (SELECT USE_YN FROM PORT_MST@DL_SSGM WHERE LOC_CD = A.POD_CD AND START_PORT = A.POL_CD) AS USE_YN";
            sqlstr += "            , A.RMK";
            sqlstr += "            , A.LINE_TYPE";
            sqlstr += "            , ( SELECT CNTN_GRP_CD FROM MDM_PORT_MST  WHERE LOC_CD= A.POD_CD) AS CNTN_GRP_CD ";
            sqlstr += "            , CASE WHEN TS_CNT =  0 THEN (SELECT MAX(SEQ) - 1 FROM PRM_SCH_TS WHERE SCH_NO = A.SCH_NO) ELSE 0 END AS TS_COUNT ";
            sqlstr += "            , CASE WHEN TS_CNT = '0'THEN(SELECT LISTAGG(POL_CD, '-') WITHIN GROUP (ORDER BY SEQ) AS POL_CD FROM PRM_SCH_TS WHERE SCH_NO = A.SCH_NO)|| '-' || A.POD_CD END AS TS_DTL ";
            sqlstr += "            , CASE WHEN CNTR_TYPE = 'F' THEN 'FCL' WHEN CNTR_TYPE = 'L' THEN 'LCL' WHEN A.CNTR_TYPE = 'C' THEN 'CONSOLE' END AS CNTR_TYPE  ";
            sqlstr += "            , A.EX_IM_TYPE  ";
            sqlstr += "                    FROM PRM_SCH_MST A ";
            sqlstr += "                         INNER JOIN MDM_CARR_MST CARR ";
            sqlstr += "                            ON A.LINE_CD = CARR.CARR_CD ";
            sqlstr += "                         LEFT OUTER JOIN MDM_LINE_IMG B ";
            sqlstr += "                            ON CARR.SCAC_CD = B.LINE_CD ";
            sqlstr += "                               OR CARR.EDI_CD = B.LINE_CD ";
            sqlstr += "   WHERE 1 = 1 ";

            sqlstr += "    AND A.WEB_FLAG = 'Y' ";
            sqlstr += "    AND A.ETD >=  '" + dr["ETD_START"] + "'";

            sqlstr += "    AND ( ('" + dr["REQ_SVC"] + "' IS NULL and 1 = 1 ) or ('" + dr["REQ_SVC"] + "'  = 'ALL' and 1 = 1 ) or ('" + dr["REQ_SVC"] + "' IS NOT NULL and A.REQ_SVC = '" + dr["REQ_SVC"] + "' ) ) ";            

            if (dr["CNTR_TYPE"].ToString() != "")
            {
                if(dr["CNTR_TYPE"].ToString() == "F")
                {
                    sqlstr += "    AND A.CNTR_TYPE = '" + dr["CNTR_TYPE"] + "'";
                }
                else if (dr["CNTR_TYPE"].ToString() == "L")
                {
                    sqlstr += "    AND (A.CNTR_TYPE = 'L' OR A.CNTR_TYPE = 'C')";
                }   
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

            if (dr["ID"].ToString() != "")
            {
                sqlstr += "ORDER BY " + dr["ID"] + " " + dr["ORDER"] + "";
            }
            else
                sqlstr += "ORDER BY ETA";
            sqlstr += " )A ";
            sqlstr += ")WHERE PAGE = " + dr["PAGE"];

            return sqlstr;
        }
    }
}
