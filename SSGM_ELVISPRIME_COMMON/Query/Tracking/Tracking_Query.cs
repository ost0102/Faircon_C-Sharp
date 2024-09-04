using System.Data;

namespace SSGM_ELVISPRIME_COMMON.Query.Tracking
{
    class Tracking_Query
    {
        string sqlstr;

        /// <summary>
        /// 화물추적 MST 쿼리
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string Query_GetTrackMstList(DataRow dr)
        {
            sqlstr = "";

            //sqlstr += "	SELECT A.HBL_NO		";
            //sqlstr += "          , A.CNTR_NO		";
            //sqlstr += "          , A.MBL_NO		";
            //sqlstr += "          , (SELECT CD_NM FROM MDM_COM_CODE WHERE COMN_CD = A.NOW_EVENT_CD AND GRP_CD = CASE WHEN A.REQ_SVC = 'SEA' THEN 'B30' WHEN A.REQ_SVC = 'AIR' THEN 'A30' END) AS NOW_EVENT_NM		";
            //sqlstr += "          , ACT_LOC_NM	";
            //sqlstr += "          , (SELECT CASE WHEN CTRY_CD LIKE 'KR' THEN 'E' ELSE 'I' END FROM MDM_PORT_MST WHERE LOC_CD = A.POL_CD) AS EX_IM_TYPE 		";
            //sqlstr += "          , A.REQ_SVC 		";
            //sqlstr += "    FROM(  		";
            //sqlstr += "    SELECT A.HBL_NO		";
            //sqlstr += "             , A.CNTR_NO		";
            //sqlstr += "             , MAX(C.MBL_NO) AS MBL_NO		";
            //sqlstr += "             , MAX(NOW_EVENT_CD) AS NOW_EVENT_CD		";
            //sqlstr += "             , MAX(LAST_EVENT_CD) AS LAST_EVENT_CD   ";
            //sqlstr += "             , MAX(B.ACT_LOC_CD) AS ACT_LOC_CD		";
            //sqlstr += "             , MAX(B.ACT_LOC_NM) AS ACT_LOC_NM		";
            //sqlstr += "             , MAX(C.REQ_SVC) AS REQ_SVC		";
            //sqlstr += "             , MAX(C.POL_CD) AS POL_CD		";
            //sqlstr += "       FROM FMS_TRK_MST A		";
            //sqlstr += "                LEFT OUTER JOIN FMS_TRK_DTL B ON A.HBL_NO = B.HBL_NO		";
            //sqlstr += "                LEFT OUTER JOIN FMS_HBL_MST C ON A.HBL_NO = C.HBL_NO		";
            //sqlstr += "                WHERE 1 = 1		";
            //
            //if (dr["HBL_NO"].ToString() != "" && dr["CNTR_NO"].ToString() != "")
            //{
            //    sqlstr += "     AND ((A.HBL_NO = '" + dr["HBL_NO"] + "' OR C.MBL_NO = '" + dr["HBL_NO"] + "') AND A.CNTR_NO = '" + dr["CNTR_NO"] + "')		";
            //}
            //else if (dr["HBL_NO"].ToString() != "")
            //{
            //    sqlstr += "     AND (A.HBL_NO = '" + dr["HBL_NO"] + "' OR C.MBL_NO = '" + dr["HBL_NO"] + "')		";
            //}
            //else if (dr["CNTR_NO"].ToString() != "")
            //{
            //    sqlstr += "      AND (A.CNTR_NO = '" + dr["CNTR_NO"] + "')		";
            //}
            //else
            //{
            //    sqlstr += "     AND (A.HBL_NO = '" + dr["HBL_NO"] + "' OR A.CNTR_NO = '" + dr["HBL_NO"] + "' OR C.MBL_NO = '" + dr["HBL_NO"] + "')		";
            //}
            //
            //sqlstr += "                GROUP BY A.HBL_NO,A.CNTR_NO		";
            //sqlstr += "	 )A		";

            return sqlstr;
        }

        /// <summary>
        /// HBL_NO CheckBL 확인
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string Query_fnIsCheckBL(DataRow dr)
        {
            sqlstr = "";

            sqlstr += " SELECT A.HBL_NO, ";
            sqlstr += "        A.MBL_NO, ";
            sqlstr += "        A.REQ_SVC, ";
            sqlstr += "        (SELECT SCAC_CD FROM MDM_CARR_MST WHERE CARR_CD = A.LINE_CD) AS SCAC_CD, ";
            sqlstr += "        H.CHKBL_YN, ";
            sqlstr += "        (SELECT SUBSTR(KEY_CD,1,INSTR(KEY_CD,'|')-1) FROM MDM_OFFICE_CONFIG WHERE OFFICE_CD = '" + dr["OFFICE_CD"] + "' AND ITEM_CD = '112') AS TOKEN, ";
            sqlstr += "        (SELECT SUBSTR(KEY_CD,INSTR(KEY_CD,'|')+1) FROM MDM_OFFICE_CONFIG WHERE OFFICE_CD = '" + dr["OFFICE_CD"] + "' AND ITEM_CD = '112') AS EXPIREDDT, ";// 토큰 만료 시간 추가 
            sqlstr += "        (SELECT ID FROM MDM_OFFICE_EDI WHERE OFFICE_CD = '" + dr["OFFICE_CD"] + "' AND EDI_SYS_CD = 'SVTG') AS ID, ";
            sqlstr += "        (SELECT PWD FROM MDM_OFFICE_EDI WHERE OFFICE_CD = '" + dr["OFFICE_CD"] + "' AND EDI_SYS_CD = 'SVTG') AS PWD ";
            sqlstr += "   FROM FMS_HBL_MST A ";
            sqlstr += "        INNER JOIN FMS_TRK_MST C ";
            sqlstr += "           ON A.HBL_NO = C.HBL_NO ";
            sqlstr += "        INNER JOIN FMS_HBL_OTH H ";
            sqlstr += "           ON A.HBL_NO = H.HBL_NO ";
            sqlstr += "  WHERE 1 = 1 ";
            sqlstr += "        AND (A.HBL_NO = '" + dr["HBL_NO"] + "' OR C.CNTR_NO = '" + dr["CNTR_NO"] + "' OR A.MBL_NO = '" + dr["HBL_NO"] + "') ";

            return sqlstr;
        }

        /// <summary>
        /// 화물추적 DTL 쿼리
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string Query_GetTrackDTL(DataRow dr)
        {
            sqlstr = "";

            sqlstr += " SELECT EVENT_CD	";
            sqlstr += "          ,  EVENT_NM";
            sqlstr += "          ,  ACT_LOC_CD";
            sqlstr += "          ,  TO_CHAR(TO_DATE(ACT_YMD),'YYYY.MM.DD') AS ACT_YMD ";
            sqlstr += "          ,  TO_CHAR(TO_DATE(ACT_HM,'HH24MISS'),'HH24:MI') AS ACT_HM";
            sqlstr += "          ,  TO_CHAR(TO_DATE(EST_YMD),'YYYY.MM.DD') AS EST_YMD ";
            sqlstr += "          ,  TO_CHAR(TO_DATE(EST_HM,'HH24MISS'),'HH24:MI') AS EST_HM";
            sqlstr += "          ,  ACT_LOC_NM";
            sqlstr += "          , EVENT_STATUS";
            sqlstr += "          , EX_IM_TYPE";
            sqlstr += "          , REQ_SVC";
            sqlstr += "          FROM (";
            sqlstr += " SELECT EVENT_CD	";
            sqlstr += "          ,  B.CD_NM AS EVENT_NM";
            sqlstr += "          ,  ACT_LOC_CD";
            sqlstr += "          ,  ACT_YMD";
            sqlstr += "          ,  ACT_HM";
            sqlstr += "          ,  EST_YMD";
            sqlstr += "          ,  EST_HM";
            sqlstr += "          ,  ACT_LOC_NM";
            sqlstr += "          , CASE WHEN (SELECT NOW_EVENT_CD FROM FMS_TRK_MST WHERE CNTR_NO = A.CNTR_NO AND HBL_NO = A.HBL_NO)  = A.EVENT_CD THEN 'Y' ";
            sqlstr += "                 WHEN ACT_YMD IS NOT NULL OR ACT_HM IS NOT NULL THEN 'E'";
            sqlstr += "                 ELSE 'N' END AS EVENT_STATUS";
            sqlstr += "         , (SELECT CASE WHEN CTRY_CD LIKE 'KR' THEN 'E' ELSE 'I' END FROM MDM_PORT_MST WHERE LOC_CD = C.POL_CD) AS EX_IM_TYPE";
            sqlstr += "         , C.REQ_SVC";
            sqlstr += "     FROM FMS_TRK_DTL A";
            sqlstr += "               LEFT OUTER JOIN FMS_HBL_MST C ON C.HBL_NO = A.HBL_NO";
            sqlstr += "               INNER JOIN MDM_COM_CODE B ON A.EVENT_CD = B.COMN_CD ";
            sqlstr += "               AND B.GRP_CD  = (CASE WHEN C.REQ_SVC = 'SEA' THEN 'B30' WHEN C.REQ_SVC = 'AIR' THEN 'A30' END ) ";
            sqlstr += "  WHERE ";
            sqlstr += "  A.CNTR_NO = '" + dr["CNTR_NO"] + "'";
            sqlstr += "    AND A.HBL_NO = '" + dr["HBL_NO"] + "'";

            if (dr["REQ_SVC"].ToString() == "SEA")
            {
                if (dr["EX_IM_TYPE"].ToString() == "E")
                {
                    sqlstr += " AND A.EVENT_CD in ('EMT','CIG','LOD','POL','ARR')";
                }
                else if (dr["EX_IM_TYPE"].ToString() == "I")
                {
                    sqlstr += " AND A.EVENT_CD in ('POL','ARR','ICC','CYA','CYD','EMR')";
                }
                else
                {
                    sqlstr += " AND A.EVENT_CD in ('EMT','CNI','CIG','LOD','POL','ARR','ICC','CYA','CYD','EMR')";
                }
            }
            else if (dr["REQ_SVC"].ToString() == "AIR")
            {
                sqlstr += " AND A.EVENT_CD in ('POL','ARR')";
            }

            sqlstr += "  ORDER BY SEQ";
            sqlstr += "  )";

            return sqlstr;
        }

        /// <summary>
        /// Tracking B/L 데이터 가져오기
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string Query_GetSeaTrackingBLData(DataRow dr)
        {
            sqlstr = "";
                        
            sqlstr += "   SELECT TRK.HBL_NO, ";
            sqlstr += "        MAX (TRK.CNTR_NO) AS CNTR_NO, ";
            sqlstr += "        POL_CD, ";
            sqlstr += "        MAX(MST.REQ_SVC) AS REQ_SVC, ";
            sqlstr += "        MAX (AUTH.EX_IM_TYPE) AS EX_IM_TYPE ";
            sqlstr += "   FROM FMS_HBL_MST MST ";
            sqlstr += "        INNER JOIN FMS_HBL_OTH OTH ";
            sqlstr += "           ON MST.HBL_NO = OTH.HBL_NO ";
            sqlstr += "        INNER JOIN FMS_TRK_DTL TRK ";
            sqlstr += "           ON MST.HBL_NO = TRK.HBL_NO ";
            sqlstr += "        INNER JOIN FMS_HBL_AUTH AUTH ";
            sqlstr += "           ON MST.HBL_NO = AUTH.HBL_NO AND AUTH.OFFICE_CD = '" + dr["OFFICE_CD"].ToString() + "' ";
            sqlstr += "            WHERE 1 = 1 ";
            sqlstr += "                  AND (TRK.HBL_NO = '" + dr["HBL_NO"].ToString() + "' ";
            if (dr["CNTR_NO"].ToString() != "")
            {
                sqlstr += "                       AND TRK.CNTR_NO = '" + dr["CNTR_NO"].ToString() + "' ";
            }
            else
            {
                sqlstr += "                       OR TRK.CNTR_NO = '" + dr["HBL_NO"].ToString() + "' ";
            }

            sqlstr += "                       ) ";
            sqlstr += "         GROUP BY TRK.HBL_NO, POL_CD ";

            return sqlstr;
        }

        /// <summary>
        /// Tracking B/L 제출 체크 로직 이후 리스트 데이터
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string Query_GetSeaTrackingData(DataRow dr)
        {
            sqlstr = "";

            sqlstr += "  SELECT A.HBL_NO, ";
            sqlstr += "          A.CNTR_NO, ";
            sqlstr += "          MST.MBL_NO, ";
            sqlstr += "          EVENT_CD, ";
            sqlstr += "          B.CD_NM AS EVENT_NM, ";
            sqlstr += "          EST_LOC_CD, ";
            sqlstr += "          EST_LOC_NM, ";
            sqlstr += "          ACT_LOC_CD, ";
            sqlstr += "          ACT_LOC_NM, ";
            sqlstr += "          RPAD (EST_YMD || EST_HM, 14, 0) AS EST, ";
            sqlstr += "          RPAD (ACT_YMD || ACT_HM, 14, 0) AS ACT, ";
            sqlstr += "          ACT_YMD, ";
            sqlstr += "          ACT_HM, ";
            sqlstr += "          EST_YMD, ";
            sqlstr += "          EST_HM, ";
            sqlstr += "          CASE ";
            sqlstr += "             WHEN (SELECT COUNT (*) ";
            sqlstr += "                     FROM FMS_TRK_DTL DTL ";
            sqlstr += "                    WHERE     DTL.SEQ >= A.SEQ ";
            sqlstr += "                          AND A.HBL_NO = DTL.HBL_NO ";
            sqlstr += "                          AND A.CNTR_NO = DTL.CNTR_NO ";
            sqlstr += "                          AND NOT DTL.ACT_YMD IS NULL ";
            sqlstr += "                          AND DTL.ACT_YMD <= TO_CHAR (SYSDATE, 'yyyyMMdd')) > 0 ";
            sqlstr += "             THEN ";
            sqlstr += "                'E' ";
            sqlstr += "             WHEN NOT A.ACT_YMD IS NULL ";
            sqlstr += "                  AND (SELECT MAX (SEQ) ";
            sqlstr += "                         FROM FMS_TRK_DTL DTL ";
            sqlstr += "                        WHERE A.HBL_NO = DTL.HBL_NO AND A.CNTR_NO = DTL.CNTR_NO) = ";
            sqlstr += "                         A.SEQ ";
            sqlstr += "                  AND A.ACT_YMD <= TO_CHAR (SYSDATE, 'yyyyMMdd') ";
            sqlstr += "             THEN ";
            sqlstr += "                'E' ";
            sqlstr += "             WHEN     NOT A.ACT_YMD IS NULL ";
            sqlstr += "                  AND A.ACT_YMD <= TO_CHAR (SYSDATE, 'yyyyMMdd') ";
            sqlstr += "                  AND (SELECT COUNT (*) ";
            sqlstr += "                         FROM FMS_TRK_DTL DTL ";
            sqlstr += "                        WHERE     DTL.SEQ > A.SEQ ";
            sqlstr += "                              AND A.HBL_NO = DTL.HBL_NO ";
            sqlstr += "                              AND A.CNTR_NO = DTL.CNTR_NO ";
            sqlstr += "                              AND (DTL.ACT_YMD IS NULL ";
            sqlstr += "                                   OR DTL.ACT_YMD <= ";
            sqlstr += "                                         TO_CHAR (SYSDATE, 'yyyyMMdd'))) = 0 ";
            sqlstr += "             THEN ";
            sqlstr += "                'Y' ";
            sqlstr += "             WHEN (A.ACT_YMD IS NULL ";
            sqlstr += "                   OR A.ACT_YMD > TO_CHAR (SYSDATE, 'yyyyMMdd')) ";
            sqlstr += "             THEN ";
            sqlstr += "                'N' ";
            sqlstr += "             ELSE ";
            sqlstr += "                'N' ";
            sqlstr += "          END ";
            sqlstr += "             AS ACT_EVT_CD, ";
            sqlstr += "          MST.COLD_TYPE, ";
            sqlstr += "          (SELECT MAX (LINE_BKG_NO) ";
            sqlstr += "             FROM FMS_MBL_OTH ";
            sqlstr += "            WHERE SR_NO = MST.SR_NO) ";
            sqlstr += "             AS LINE_BKG_NO ";
            sqlstr += "     FROM FMS_HBL_MST MST ";
            sqlstr += "          INNER JOIN FMS_TRK_DTL A ";
            sqlstr += "             ON MST.HBL_NO = A.HBL_NO ";
            sqlstr += "          INNER JOIN MDM_COM_CODE B ";
            sqlstr += "             ON A.EVENT_CD = B.COMN_CD AND B.GRP_CD = (CASE WHEN MST.REQ_SVC = 'SEA' THEN 'B30' WHEN MST.REQ_SVC = 'AIR' THEN 'A30' END ) ";
            sqlstr += "             WHERE 1=1 ";
            sqlstr += "                 AND MST.HBL_NO = '" + dr["HBL_NO"].ToString() + "'             ";
            sqlstr += "                 AND A.CNTR_NO = '" + dr["CNTR_NO"].ToString() + "'             ";

            if (dr["REQ_SVC"].ToString() == "AIR")
            {
                sqlstr += "             AND A.EVENT_CD in ('POL','ARR')";
            }
            else if (dr["REQ_SVC"].ToString() == "SEA")
            {
                if (dr["EX_IM_TYPE"].ToString() == "E")
                {
                    sqlstr += "             AND A.EVENT_CD in ('EMT','CIG','LOD','POL','ARR')";
                }
                else if (dr["EX_IM_TYPE"].ToString() == "I")
                {
                    sqlstr += "             AND A.EVENT_CD in ('POL','ARR','ICC','CYA','CYD','EMR')";
                }
                else
                {
                    sqlstr += "             AND A.EVENT_CD in ('EMT','CIG','LOD','POL','ARR','ICC','CYA','CYD','EMR')";
                }
            }

            sqlstr += " ORDER BY A.HBL_NO, A.SEQ ";

            return sqlstr;
        }

        /// <summary>
        /// Tracking B/L 데이터 가져오기
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string Query_GetAirTrackingBLData(DataRow dr)
        {
            sqlstr = "";

            sqlstr += "	SELECT A.HBL_NO		";
            sqlstr += "          , A.CNTR_NO		";
            sqlstr += "          , A.MBL_NO		";
            sqlstr += "          , (SELECT CD_NM FROM MDM_COM_CODE WHERE COMN_CD = A.NOW_EVENT_CD AND GRP_CD = CASE WHEN A.REQ_SVC = 'SEA' THEN 'B30' WHEN A.REQ_SVC = 'AIR' THEN 'A30' END) AS NOW_EVENT_NM		";
            sqlstr += "          , ACT_LOC_NM	";
            sqlstr += "          , A.EX_IM_TYPE 		";
            sqlstr += "          , A.REQ_SVC 		";
            sqlstr += "    FROM(  		";
            sqlstr += "    SELECT A.HBL_NO		";
            sqlstr += "             , A.CNTR_NO		";
            sqlstr += "             , MAX(C.MBL_NO) AS MBL_NO		";
            sqlstr += "             , MAX(NOW_EVENT_CD) AS NOW_EVENT_CD		";
            sqlstr += "             , MAX(LAST_EVENT_CD) AS LAST_EVENT_CD   ";
            sqlstr += "             , MAX(B.ACT_LOC_CD) AS ACT_LOC_CD		";
            sqlstr += "             , MAX(B.ACT_LOC_NM) AS ACT_LOC_NM		";
            sqlstr += "             , MAX(C.REQ_SVC) AS REQ_SVC		";
            sqlstr += "             , MAX(C.POL_CD) AS POL_CD		";
            sqlstr += "             , MAX(E.EX_IM_TYPE) AS EX_IM_TYPE		";
            sqlstr += "       FROM FMS_TRK_MST A		";
            sqlstr += "                LEFT OUTER JOIN FMS_TRK_DTL B ON A.HBL_NO = B.HBL_NO		";
            sqlstr += "                LEFT OUTER JOIN FMS_HBL_MST C ON A.HBL_NO = C.HBL_NO		";
            sqlstr += "                LEFT OUTER JOIN FMS_HBL_OTH D ON A.HBL_NO = D.HBL_NO		";
            sqlstr += "                LEFT OUTER JOIN FMS_HBL_AUTH E ON A.HBL_NO = E.HBL_NO AND E.OFFICE_CD = '" + dr["OFFICE_CD"] + "'	";
            sqlstr += "                WHERE 1 = 1		";
            sqlstr += "                AND D.CHKBL_YN = 'Y'		";

            if (dr["HBL_NO"].ToString() != "" && dr["CNTR_NO"].ToString() != "")
            {
                sqlstr += "     AND ((A.HBL_NO = '" + dr["HBL_NO"] + "' OR C.MBL_NO = '" + dr["HBL_NO"] + "') AND A.CNTR_NO = '" + dr["CNTR_NO"] + "')		";
            }
            else if (dr["HBL_NO"].ToString() != "")
            {
                sqlstr += "     AND (A.HBL_NO = '" + dr["HBL_NO"] + "' OR C.MBL_NO = '" + dr["HBL_NO"] + "')		";
            }
            else if (dr["CNTR_NO"].ToString() != "")
            {
                sqlstr += "      AND (A.CNTR_NO = '" + dr["CNTR_NO"] + "')		";
            }
            else
            {
                sqlstr += "     AND (A.HBL_NO = '" + dr["HBL_NO"] + "' OR A.CNTR_NO = '" + dr["HBL_NO"] + "' OR C.MBL_NO = '" + dr["HBL_NO"] + "')		";
            }

            sqlstr += "                GROUP BY A.HBL_NO,A.CNTR_NO		";
            sqlstr += "	 )A		";

            return sqlstr;
        }

        /// <summary>
        /// Tracking B/L 제출 체크 로직 이후 리스트 데이터
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string Query_GetAirTrackingData(DataRow dr)
        {
            sqlstr = "";

            sqlstr += " SELECT EVENT_CD	";
            sqlstr += "          ,  EVENT_NM";
            sqlstr += "          ,  ACT_LOC_CD";
            sqlstr += "          ,  TO_CHAR(TO_DATE(ACT_YMD),'YYYY.MM.DD') AS ACT_YMD ";
            sqlstr += "          ,  TO_CHAR(TO_DATE(ACT_HM,'HH24MISS'),'HH24:MI') AS ACT_HM";
            sqlstr += "          ,  TO_CHAR(TO_DATE(EST_YMD),'YYYY.MM.DD') AS EST_YMD ";
            sqlstr += "          ,  TO_CHAR(TO_DATE(EST_HM,'HH24MISS'),'HH24:MI') AS EST_HM";
            sqlstr += "          ,  ACT_LOC_NM";
            sqlstr += "          , EVENT_STATUS";
            sqlstr += "          , EX_IM_TYPE";
            sqlstr += "          , REQ_SVC";
            sqlstr += "          FROM (";
            sqlstr += " SELECT EVENT_CD	";
            sqlstr += "          ,  B.CD_NM AS EVENT_NM";
            sqlstr += "          ,  ACT_LOC_CD";
            sqlstr += "          ,  ACT_YMD";
            sqlstr += "          ,  ACT_HM";
            sqlstr += "          ,  EST_YMD";
            sqlstr += "          ,  EST_HM";
            sqlstr += "          ,  ACT_LOC_NM";
            sqlstr += "          , CASE WHEN (SELECT NOW_EVENT_CD FROM FMS_TRK_MST WHERE CNTR_NO = A.CNTR_NO AND HBL_NO = A.HBL_NO)  = A.EVENT_CD THEN 'Y' ";
            sqlstr += "                 WHEN ACT_YMD IS NOT NULL OR ACT_HM IS NOT NULL THEN 'E'";
            sqlstr += "                 ELSE 'N' END AS EVENT_STATUS";
            sqlstr += "         , '" + dr["EX_IM_TYPE"] + "' AS EX_IM_TYPE";
            sqlstr += "         , C.REQ_SVC";
            sqlstr += "     FROM FMS_TRK_DTL A";
            sqlstr += "               LEFT OUTER JOIN FMS_HBL_MST C ON C.HBL_NO = A.HBL_NO";
            sqlstr += "               INNER JOIN MDM_COM_CODE B ON A.EVENT_CD = B.COMN_CD ";
            sqlstr += "               AND B.GRP_CD  = (CASE WHEN C.REQ_SVC = 'SEA' THEN 'B30' WHEN C.REQ_SVC = 'AIR' THEN 'A30' END ) ";
            sqlstr += "  WHERE ";
            sqlstr += "  A.CNTR_NO = '" + dr["CNTR_NO"] + "'";
            sqlstr += "    AND A.HBL_NO = '" + dr["HBL_NO"] + "'";
            sqlstr += "  ORDER BY SEQ";
            sqlstr += "  )";

            return sqlstr;
        }

    }
}
