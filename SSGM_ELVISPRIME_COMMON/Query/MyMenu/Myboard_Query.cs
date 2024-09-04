using System.Data;

namespace SSGM_ELVISPRIME_COMMON.Query.MyMenu
{
    public class Myboard_Query
    {
        string sqlstr;

        /// <summary>
        /// 마이보드 데이터 가져오기
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string GetBoardList_Query(DataRow dr)
        {
            sqlstr += " SELECT * ";
            sqlstr += "   FROM (SELECT ROWNUM AS RNUM, ";
            sqlstr += "                FLOOR ( (ROWNUM - 1) / 10 + 1) AS PAGE, ";
            sqlstr += "                COUNT (*) OVER () AS TOTCNT, ";
            sqlstr += "                HBL_NO, ";
            sqlstr += "                REQ_SVC, ";
            sqlstr += "                EX_IM_TYPE, ";
            sqlstr += "                TO_CHAR (TO_DATE (ETD), 'YYYY.MM.DD') AS ETD, ";
            sqlstr += "                TO_CHAR (TO_DATE (ETA), 'YYYY.MM.DD') AS ETA, ";
            sqlstr += "                CASE ";
            sqlstr += "                   WHEN IS_CHKBL_YN IS NOT NULL ";
            sqlstr += "                   THEN ";
            sqlstr += "                      (SELECT POL_NM ";
            sqlstr += "                         FROM FMS_HBL_MST ";
            sqlstr += "                        WHERE HBL_NO = A.HBL_NO) ";
            sqlstr += "                   ELSE ";
            sqlstr += "                      (SELECT LOC_NM ";
            sqlstr += "                         FROM MDM_PORT_MST ";
            sqlstr += "                        WHERE LOC_CD = POL_CD) ";
            sqlstr += "                END ";
            sqlstr += "                   AS POL_NM, ";
            sqlstr += "                CASE ";
            sqlstr += "                   WHEN IS_CHKBL_YN IS NOT NULL ";
            sqlstr += "                   THEN ";
            sqlstr += "                      (SELECT POD_NM ";
            sqlstr += "                         FROM FMS_HBL_MST ";
            sqlstr += "                        WHERE HBL_NO = A.HBL_NO) ";
            sqlstr += "                   ELSE ";
            sqlstr += "                      (SELECT LOC_NM ";
            sqlstr += "                         FROM MDM_PORT_MST ";
            sqlstr += "                        WHERE LOC_CD = POD_CD) ";
            sqlstr += "                END ";
            sqlstr += "                   AS POD_NM, ";
            sqlstr += "                FILE_CNT, ";
            sqlstr += "                INV_NO, ";
            sqlstr += "                BKG_NO,	";
            sqlstr += "                PRM_BKG_NO,	";
            sqlstr += "	                     CFS_BKG_NO, ";
            sqlstr += "	                    (SELECT CFS_BKG_CHK FROM PRM_BKG_MST WHERE BKG_NO = PRM_BKG_NO) AS CFS_BKG_CHK, ";
            sqlstr += "	                    (SELECT STATUS FROM PRM_BKG_MST WHERE BKG_NO = PRM_BKG_NO) AS STATUS, ";
            sqlstr += "                (SELECT INV_YN ";
            sqlstr += "                   FROM ACT_INV_MST ";
            sqlstr += "                  WHERE INV_NO = A.INV_NO) ";
            sqlstr += "                   AS INV_YN, ";
            sqlstr += "                   CASE WHEN IS_CHKBL_YN IS NOT NULL THEN 'Y' ELSE 'N' END AS IS_CHKBL_YN ";
            sqlstr += "           FROM ( ";
            sqlstr += "      SELECT C.HBL_NO,  ";
            sqlstr += "	      			            UNISTR(A.CFS_BKG_NO) AS BKG_NO, ";
            sqlstr += "	                            UNISTR(A.CFS_BKG_NO) AS CFS_BKG_NO,     ";
            sqlstr += "	                            A.BKG_NO AS PRM_BKG_NO ";
            sqlstr += "     , B.EX_IM_TYPE ";
            sqlstr += "     , (SELECT LOC_CD FROM MDM_PORT_MST WHERE LOC_CD = B.POL_CD) AS POL_CD ";
            sqlstr += "     , (SELECT LOC_CD FROM MDM_PORT_MST WHERE LOC_CD = B.POD_CD) AS POD_CD ";
            sqlstr += "     , B.REQ_SVC ";
            sqlstr += "     ,  CASE ";
            sqlstr += "          WHEN C.HBL_NO IS NULL THEN B.ETD ";
            sqlstr += "          ELSE (SELECT ETD ";
            sqlstr += "                  FROM FMS_HBL_MST ";
            sqlstr += "                 WHERE HBL_NO = C.HBL_NO) ";
            sqlstr += "       END ";
            sqlstr += "          AS ETD, ";
            sqlstr += "       CASE ";
            sqlstr += "          WHEN C.HBL_NO IS NULL THEN B.ETA ";
            sqlstr += "          ELSE (SELECT ETA ";
            sqlstr += "                  FROM FMS_HBL_MST ";
            sqlstr += "                 WHERE HBL_NO = C.HBL_NO) ";
            sqlstr += "       END ";
            sqlstr += "          AS ETA, ";
            sqlstr += "      CASE WHEN C.HBL_NO IS NOT NULL THEN  ";
            sqlstr += "       (  SELECT COUNT (SEQ) ";
            sqlstr += "            FROM COM_DOC_MST ";
            sqlstr += "           WHERE MNGT_NO = C.HBL_NO ";
            sqlstr += "                 AND DOC_TYPE IN (";
            if (dr["USR_TYPE"].ToString() == "S" || dr["USR_TYPE"].ToString() == "M")
            {
                sqlstr += dr["HBL_DOC_TYPE"].ToString();
            }
            else if (dr["USR_TYPE"].ToString() == "P")
            {
                sqlstr += dr["PARTNER_DOC_TYPE"].ToString();
            }
                sqlstr += "       ) ) ";
            sqlstr += "       + (  SELECT COUNT (MAX(SEQ)) ";
            sqlstr += "            FROM COM_DOC_MST ";
            sqlstr += "           WHERE MNGT_NO = C.HBL_NO ";
            sqlstr += "                 AND DOC_TYPE = 'CHBL' ";
            sqlstr += "                     GROUP BY DOC_TYPE ) ";
            if (dr["USR_TYPE"].ToString() != "P")
            {
                sqlstr += "         +  ";
                sqlstr += "         (  SELECT COUNT (MAX (SEQ)) ";
                sqlstr += "             FROM COM_DOC_MST ";
                sqlstr += "            WHERE MNGT_NO = C.HBL_NO ";
                sqlstr += "                  AND DOC_TYPE = 'AN' ";
                sqlstr += "         GROUP BY DOC_TYPE) ";
            }
            sqlstr += "       + (  SELECT COUNT (MAX (SEQ)) ";
            sqlstr += "              FROM COM_DOC_MST ";
            sqlstr += "             WHERE MNGT_NO = (SELECT MBL_NO ";
            sqlstr += "                                FROM FMS_HBL_MST ";
            sqlstr += "                               WHERE HBL_NO = C.HBL_NO) ";
            sqlstr += "                   AND DOC_TYPE IN ('MANI', 'MBL') ";
            sqlstr += "          GROUP BY DOC_TYPE) ";
            if (dr["USR_TYPE"].ToString() != "P")
            {
                sqlstr += "       + (  SELECT COUNT (MAX (SEQ)) ";
                sqlstr += "              FROM ACT_INV_MST MST ";
                sqlstr += "                   INNER JOIN ACT_INV_DTL DTL ";
                sqlstr += "                      ON MST.INV_NO = DTL.INV_NO ";
                sqlstr += "                   INNER JOIN COM_DOC_MST DOC ";
                sqlstr += "                      ON DTL.MBL_HBL_NO = DOC.MNGT_NO ";
                sqlstr += "             WHERE MST.INV_NO = (SELECT MAX (INV_NO) ";
                sqlstr += "                                   FROM ACT_INV_DTL ";
                sqlstr += "                                  WHERE MBL_HBL_NO = C.HBL_NO) ";
                sqlstr += "                   AND DOC.DOC_TYPE = 'INV' ";
                sqlstr += "                   AND MST.INV_YN = 'Y' ";
                sqlstr += "          GROUP BY DOC_TYPE) ";
            }
            sqlstr += "          ELSE  ";
            sqlstr += "          (SELECT COUNT (FILE_NM) ";
            sqlstr += "            FROM COM_DOC_MST ";
            sqlstr += "           WHERE MNGT_NO = A.BKG_NO) ";
            sqlstr += "           END ";
            sqlstr += "          AS FILE_CNT ";
            sqlstr += "     , (SELECT MAX (MNGT_NO) ";
            sqlstr += "                    FROM COM_DOC_MST ";
            sqlstr += "                   WHERE MNGT_NO = C.HBL_NO ";
            sqlstr += "                         AND (DOC_TYPE = 'CHBL' OR DOC_TYPE = 'AN')) ";
            sqlstr += "                    AS IS_CHKBL_YN ";
            sqlstr += "     , (SELECT MAX (INV_NO) ";
            sqlstr += "                    FROM ACT_INV_DTL ";
            sqlstr += "                   WHERE MBL_HBL_NO = C.HBL_NO) ";
            sqlstr += "                    AS INV_NO ";
            sqlstr += "	FROM PRM_BKG_MST A ";
            sqlstr += "	INNER JOIN PRM_SCH_MST B ON A.SCH_NO = B.SCH_NO ";
            sqlstr += "	LEFT JOIN CFS_BK_MST C ON A.CFS_BKG_NO = C.BK_NO ";
            sqlstr += "	WHERE 1=1";
            sqlstr += "	AND (A.STATUS <> 'C' AND A.STATUS <> 'D' AND A.STATUS <> 'O')";
            if (dr["USR_TYPE"].ToString() != "M")
            {
                sqlstr += "   AND A.CUST_CD = '" + dr["CUST_CD"].ToString() + "'";
            }

            if (dr["MNGT_NO"].ToString() != "")
            {
                sqlstr += "     AND A.HBL_NO = '" + dr["MNGT_NO"].ToString() + "'";
            }
            else
            {
                if (dr["ETD_ETA"].ToString() == "ETD_ETA")
                {
                    sqlstr += " AND ((B.EX_IM_TYPE = 'E' AND B.ETD >= '" + dr["STRT_YMD"].ToString() + "' AND B.ETD <= '" + dr["END_YMD"].ToString() + "')  OR (B.EX_IM_TYPE = 'I' AND B.ETA >= '" + dr["STRT_YMD"].ToString() + "'AND B.ETA <= '" + dr["END_YMD"].ToString() + "'))";
                }
                else
                {
                    if (dr["STRT_YMD"].ToString() != "")
                    {
                        sqlstr += "    AND B." + dr["ETD_ETA"].ToString() + " >= '" + dr["STRT_YMD"].ToString() + "'";
                    }
                    if (dr["END_YMD"].ToString() != "")
                    {
                        sqlstr += "    AND B." + dr["ETD_ETA"].ToString() + " <= '" + dr["END_YMD"].ToString() + "'";
                    }
                }

                if (dr["CNTR_TYPE"].ToString() == "F")
                {
                    sqlstr += "    AND B.CNTR_TYPE = '" + dr["CNTR_TYPE"] + "'";
                }
                else if (dr["CNTR_TYPE"].ToString() == "L")
                {
                    sqlstr += "    AND (B.CNTR_TYPE = 'L' OR B.CNTR_TYPE = 'C')";
                }                

                sqlstr += "    AND B.EX_IM_TYPE = '" + dr["EX_IM_TYPE"].ToString() + "'";

                sqlstr += "    AND ( ('" + dr["REQ_SVC"].ToString() + "' IS NULL and 1 = 1 ) or ('" + dr["REQ_SVC"].ToString() + "' IS NOT NULL and B.REQ_SVC = '" + dr["REQ_SVC"].ToString() + "' ) )";
                sqlstr += "    AND ( ('" + dr["HBL_NO"].ToString() + "' IS NULL and 1 = 1 )or ('" + dr["HBL_NO"].ToString() + "' IS NOT NULL and C.HBL_NO LIKE '%" + dr["HBL_NO"].ToString() + "%' ) )";

                if (dr["POL_CD"].ToString() == "")
                {
                    sqlstr += "    AND ( (UPPER('" + dr["POL"] + "') IS NULL and 1 = 1 ) or (UPPER('" + dr["POL"] + "') IS NOT NULL and B.POL_CD LIKE UPPER('%" + dr["POL"] + "%')) or (UPPER('" + dr["POL"] + "') IS NOT NULL and (SELECT LOC_NM FROM MDM_PORT_MST WHERE LOC_CD = B.POL_CD) LIKE UPPER('%" + dr["POL"] + "%') ) )";
                }
                else
                {
                    sqlstr += "    AND ( (UPPER('" + dr["POL"] + "') IS NULL and 1 = 1 ) or (UPPER('" + dr["POL"] + "') IS NOT NULL and B.POL_CD LIKE UPPER('%" + dr["POL_CD"] + "%') ) or (UPPER('" + dr["POL"] + "') IS NOT NULL and (SELECT LOC_NM FROM MDM_PORT_MST WHERE LOC_CD = B.POL_CD) LIKE UPPER('%" + dr["POL"] + "%') ) )";
                }

                if (dr["POD_CD"].ToString() == "")
                {
                    sqlstr += "    AND ( (UPPER('" + dr["POD"] + "') IS NULL and 1 = 1 ) or (UPPER('" + dr["POD"] + "') IS NOT NULL and B.POD_CD LIKE UPPER('%" + dr["POD"] + "%') ) or (UPPER('" + dr["POD"] + "') IS NOT NULL and (SELECT LOC_NM FROM MDM_PORT_MST WHERE LOC_CD = B.POD_CD) LIKE UPPER('%" + dr["POD"] + "%') ) )";
                }
                else
                {
                    sqlstr += "    AND ( (UPPER('" + dr["POD"] + "') IS NULL and 1 = 1 ) or (UPPER('" + dr["POD"] + "') IS NOT NULL and B.POD_CD LIKE UPPER('%" + dr["POD_CD"] + "%') ) or (UPPER('" + dr["POD"] + "') IS NOT NULL and (SELECT LOC_NM FROM MDM_PORT_MST WHERE LOC_CD = B.POD_CD) LIKE UPPER('%" + dr["POD"] + "%') ) )";
                }
            }

            sqlstr += " 	UNION ";
            sqlstr += " 	SELECT ";
            sqlstr += "            A.HBL_NO, ";
            sqlstr += "	      						                      B.BK_NO AS BKG_NO,";
            sqlstr += "	                           B.BK_NO AS CFS_BKG_NO,";
            sqlstr += "	                           B.PRM_BKG_NO";
            sqlstr += "          , D.EX_IM_TYPE ";
            sqlstr += "          , A.POL_CD ";
            sqlstr += "          , A.POD_CD ";
            sqlstr += "          , A.REQ_SVC ";
            sqlstr += "          , A.ETD ";
            sqlstr += "          , A.ETA ";
            sqlstr += "	         , CASE WHEN A.HBL_NO IS NOT NULL THEN  ";
            sqlstr += "	   (  SELECT COUNT (SEQ) ";
            sqlstr += "	        FROM COM_DOC_MST ";
            sqlstr += "	       WHERE MNGT_NO = A.HBL_NO ";
            sqlstr += "                 AND DOC_TYPE IN (";
            if (dr["USR_TYPE"].ToString() == "S" || dr["USR_TYPE"].ToString() == "M")
            {
                sqlstr += dr["HBL_DOC_TYPE"].ToString();
            }
            else if (dr["USR_TYPE"].ToString() == "P")
            {
                sqlstr += dr["PARTNER_DOC_TYPE"].ToString();
            }
            sqlstr += "       ) ) ";

            sqlstr += "       + (  SELECT COUNT (MAX(SEQ)) ";
            sqlstr += "            FROM COM_DOC_MST ";
            sqlstr += "           WHERE MNGT_NO = A.HBL_NO ";
            sqlstr += "                 AND DOC_TYPE = 'CHBL' ";
            sqlstr += "                     GROUP BY DOC_TYPE ) ";
            if (dr["USR_TYPE"].ToString() != "P")
            {
                sqlstr += "         +  ";
                sqlstr += "         (  SELECT COUNT (MAX (SEQ)) ";
                sqlstr += "             FROM COM_DOC_MST ";
                sqlstr += "            WHERE MNGT_NO = A.HBL_NO ";
                sqlstr += "                  AND DOC_TYPE = 'AN' ";
                sqlstr += "         GROUP BY DOC_TYPE) ";
            }
            sqlstr += "	   + (  SELECT COUNT (MAX (SEQ)) ";
            sqlstr += "	          FROM COM_DOC_MST ";
            sqlstr += "	         WHERE MNGT_NO = (SELECT MBL_NO ";
            sqlstr += "	                            FROM FMS_HBL_MST ";
            sqlstr += "	                           WHERE HBL_NO = A.HBL_NO) ";
            sqlstr += "	               AND DOC_TYPE IN ('MANI', 'MBL') ";
            sqlstr += "	      GROUP BY DOC_TYPE) ";
            if (dr["USR_TYPE"].ToString() != "P")
            {
                sqlstr += "	   + (  SELECT COUNT (MAX (SEQ)) ";
                sqlstr += "	          FROM ACT_INV_MST MST ";
                sqlstr += "	               INNER JOIN ACT_INV_DTL DTL ";
                sqlstr += "	                  ON MST.INV_NO = DTL.INV_NO ";
                sqlstr += "	               INNER JOIN COM_DOC_MST DOC ";
                sqlstr += "	                  ON DTL.MBL_HBL_NO = DOC.MNGT_NO ";
                sqlstr += "	         WHERE MST.INV_NO = (SELECT MAX (INV_NO) ";
                sqlstr += "	                               FROM ACT_INV_DTL ";
                sqlstr += "	                              WHERE MBL_HBL_NO = A.HBL_NO) ";
                sqlstr += "	               AND DOC.DOC_TYPE = 'INV' ";
                sqlstr += "	               AND MST.INV_YN = 'Y' ";
                sqlstr += "	      GROUP BY DOC_TYPE) ";
            }
            sqlstr += "	       END  ";
            sqlstr += "	      AS FILE_CNT";
            sqlstr += "          ,  (SELECT MAX (MNGT_NO) ";
            sqlstr += "                         FROM COM_DOC_MST ";
            sqlstr += "                        WHERE MNGT_NO = A.HBL_NO ";
            sqlstr += "                              AND (DOC_TYPE = 'CHBL' OR DOC_TYPE = 'AN')) ";
            sqlstr += "                         AS IS_CHKBL_YN ";
            sqlstr += "          ,  (SELECT MAX (INV_NO) ";
            sqlstr += "                         FROM ACT_INV_DTL ";
            sqlstr += "                        WHERE MBL_HBL_NO = A.HBL_NO) ";
            sqlstr += "                         AS INV_NO ";
            sqlstr += "    FROM FMS_HBL_MST A  ";
            sqlstr += "      LEFT JOIN  CFS_BK_MST B  ";
            sqlstr += "                       ON A.HBL_NO = B.HBL_NO  ";
            sqlstr += "      INNER JOIN FMS_HBL_OTH C ";
            sqlstr += "                         ON A.HBL_NO = C.HBL_NO ";
            sqlstr += "                      LEFT JOIN FMS_HBL_AUTH D ";
            sqlstr += "                         ON A.HBL_NO = D.HBL_NO AND D.OFFICE_CD = 'SSGM' ";
            sqlstr += "    WHERE C.CHKBL_YN = 'Y'";
            if (dr["USR_TYPE"].ToString() != "M")
            {
                sqlstr += "   AND D.CUST_CD = '" + dr["CUST_CD"].ToString() + "'";
            }

            if (dr["MNGT_NO"].ToString() != "")
            {
                sqlstr += "     AND A.HBL_NO = '" + dr["MNGT_NO"].ToString() + "'";
            }
            else
            {
                if (dr["ETD_ETA"].ToString() == "ETD_ETA")
                {
                    sqlstr += " AND ((D.EX_IM_TYPE = 'E' AND A.ETD >= '" + dr["STRT_YMD"].ToString() + "' AND A.ETD <= '" + dr["END_YMD"].ToString() + "')  OR (D.EX_IM_TYPE = 'I' AND A.ETA >= '" + dr["STRT_YMD"].ToString() + "'AND A.ETA <= '" + dr["END_YMD"].ToString() + "'))";
                }
                else
                {
                    if (dr["STRT_YMD"].ToString() != "")
                    {
                        sqlstr += "    AND A." + dr["ETD_ETA"].ToString() + " >= '" + dr["STRT_YMD"].ToString() + "'";
                    }
                    if (dr["END_YMD"].ToString() != "")
                    {
                        sqlstr += "    AND A." + dr["ETD_ETA"].ToString() + " <= '" + dr["END_YMD"].ToString() + "'";
                    }
                }

                if (dr["CNTR_TYPE"].ToString() == "F")
                {
                    sqlstr += "    AND A.CNTR_TYPE = '" + dr["CNTR_TYPE"] + "'";
                }
                else if (dr["CNTR_TYPE"].ToString() == "L")
                {
                    sqlstr += "    AND (A.CNTR_TYPE = 'L' OR A.CNTR_TYPE = 'C')";
                }

                sqlstr += "    AND D.EX_IM_TYPE = '" + dr["EX_IM_TYPE"].ToString() + "'";

                sqlstr += "    AND ( ('" + dr["REQ_SVC"].ToString() + "' IS NULL and 1 = 1 ) or ('" + dr["REQ_SVC"].ToString() + "' IS NOT NULL and A.REQ_SVC = '" + dr["REQ_SVC"].ToString() + "' ) )";
                sqlstr += "    AND ( ('" + dr["HBL_NO"].ToString() + "' IS NULL and 1 = 1 )or ('" + dr["HBL_NO"].ToString() + "' IS NOT NULL and A.HBL_NO LIKE '%" + dr["HBL_NO"].ToString() + "%' ) )";

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
            }

            if (dr["ID"].ToString() != "")
            {
                sqlstr += " ORDER BY " + dr["ID"] + " " + dr["ORDER"] + "";
            }
            else
                sqlstr += " ORDER BY ETA DESC";
            sqlstr += " )A)	";
            sqlstr += " WHERE PAGE = " + dr["PAGE"];

            return sqlstr;
        }
        
        /// <summary>
        /// Tracking 유무 체크
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string isTrackingAvailable_Query(DataRow dr)
        {
            sqlstr = "";

            sqlstr += "   SELECT MST.HBL_NO, ";
            sqlstr += "          (SELECT B.CHKBL_YN ";
            sqlstr += "             FROM FMS_HBL_MST A INNER JOIN FMS_HBL_OTH B ON A.HBL_NO = B.HBL_NO ";
            sqlstr += "            WHERE A.HBL_NO = MST.HBL_NO) ";
            sqlstr += "             AS CHKBL_YN ";
            sqlstr += "     FROM FMS_TRK_MST MST INNER JOIN FMS_TRK_DTL DTL ON MST.HBL_NO = DTL.HBL_NO ";
            sqlstr += "     WHERE MST.HBL_NO = '" + dr["HBL_NO"] + "' ";
            sqlstr += " GROUP BY MST.HBL_NO  ";

            return sqlstr;
        }

        /// <summary>
        /// 레이어 팝업 pdf 데이터 가져오는 쿼리
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string fnGetPrintData_Query(DataRow dr)
        {

            sqlstr += "SELECT * FROM ";
            sqlstr += " COM_DOC_MST  ";
            sqlstr += " WHERE  MNGT_NO = '" + dr["HBL_NO"].ToString() + "'";
            sqlstr += "   AND  DOC_TYPE = '" + dr["DOC_TYPE"].ToString() + "'";
            sqlstr += "   AND SEQ = (SELECT MAX(SEQ) FROM COM_DOC_MST WHERE MNGT_NO = '" + dr["HBL_NO"].ToString() + "' AND DOC_TYPE = '" + dr["DOC_TYPE"].ToString() + "')";
            return sqlstr;
        }

        /// <summary>
        /// 부킹정보 가져오기
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string fnGetCfsData_Query(DataRow dr)
        {
            sqlstr += "  SELECT BKM.BK_NO AS BK_NO";
            sqlstr += "     , BKD.GR_NO AS GR_NO";
            sqlstr += "	 , BKD.GR_YMD	AS GR_YMD";
            sqlstr += "	 , BKD.ACT_CUST_NM	AS ACT_CUST_NM";
            sqlstr += "     , BKD.BK_SEQ AS BK_SEQ";
            sqlstr += "     , BKD.RMK2 AS BK_RMK2";
            sqlstr += "     , BKD.ACT_GR_CUST AS ACT_GR_CUST";
            sqlstr += "  FROM CFS_BK_MST BKM";
            sqlstr += "  LEFT OUTER ";
            sqlstr += "  JOIN CFS_BK_DTL BKD";
            sqlstr += "    ON BKM.WH_CD = BKD.WH_CD";
            sqlstr += "   AND BKM.BK_NO = BKD.BK_NO";
            sqlstr += " WHERE 1=1";
            sqlstr += " AND BKM.BK_NO LIKE '%" + dr["CFS_BKG_NO"].ToString()+"%'";
            sqlstr += "ORDER BY BKD.BK_SEQ ASC ";

            return sqlstr;

        }
    }
}
