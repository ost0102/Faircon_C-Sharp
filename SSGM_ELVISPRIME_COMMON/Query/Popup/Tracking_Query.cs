using System.Data;

namespace SSGM_ELVISPRIME_COMMON.Query.Popup
{
    public class Tracking_Query
    {
        string sqlstr;

        /// <summary>
        /// 화물추적 AIS (X) MST 쿼리
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string Query_GetTrackMstList(DataRow dr)
        {
            // 주석 -> 기존 쿼리 , 수정 테스트 -> 변경 쿼리
            sqlstr = "";

            sqlstr += "	SELECT A.HBL_NO		";
            sqlstr += "          , A.CNTR_NO		";
            sqlstr += "          , A.MBL_NO		";
            sqlstr += "          , (SELECT CD_NM FROM MDM_COM_CODE WHERE COMN_CD = A.NOW_EVENT_CD AND GRP_CD = CASE WHEN A.REQ_SVC = 'SEA' THEN 'B30' WHEN A.REQ_SVC = 'AIR' THEN 'A30' END) AS NOW_EVENT_NM		";
            sqlstr += "          , ACT_LOC_NM	";
            sqlstr += "          , A.EX_IM_TYPE ";
            sqlstr += "          , A.REQ_SVC 		";
            sqlstr += "          , A.FILE_CNT 		";
            sqlstr += "    FROM(  		";
            sqlstr += "    SELECT A.HBL_NO		";
            sqlstr += "             , A.CNTR_NO		";
            // 기존쿼리
            //sqlstr += "             , MAX(C.MBL_NO) AS MBL_NO		";
            //sqlstr += "             , MAX(NOW_EVENT_CD) AS NOW_EVENT_CD		";
            //sqlstr += "             , MAX(LAST_EVENT_CD) AS LAST_EVENT_CD   ";
            //sqlstr += "             , MAX(B.ACT_LOC_CD) AS ACT_LOC_CD		";
            //sqlstr += "             , MAX(B.ACT_LOC_NM) AS ACT_LOC_NM		";
            //sqlstr += "             , MAX(C.REQ_SVC) AS REQ_SVC		";
            //sqlstr += "             , MAX(C.POL_CD) AS POL_CD		";
            //sqlstr += "             , MAX(D.EX_IM_TYPE) AS EX_IM_TYPE		";
            // 기존쿼리

            //수정 테스트 
            sqlstr += "             , C.MBL_NO AS MBL_NO		";
            sqlstr += "             , NOW_EVENT_CD AS NOW_EVENT_CD		";
            sqlstr += "             , LAST_EVENT_CD AS LAST_EVENT_CD   ";
            sqlstr += "             , (SELECT ACT_LOC_CD FROM FMS_TRK_DTL WHERE SEQ = B.DTL_SEQ AND HBL_NO = A.HBL_NO AND CNTR_NO = A.CNTR_NO)AS ACT_LOC_CD";
            sqlstr += "             , (SELECT ACT_LOC_NM FROM FMS_TRK_DTL WHERE SEQ = B.DTL_SEQ AND HBL_NO = A.HBL_NO AND CNTR_NO = A.CNTR_NO)AS ACT_LOC_NM";
            sqlstr += "             , C.REQ_SVC AS REQ_SVC		";
            sqlstr += "             , C.POL_CD AS POL_CD		";
            sqlstr += "             , D.EX_IM_TYPE AS EX_IM_TYPE		";
            //수정 테스트

            sqlstr += "             ,             (SELECT COUNT (MAX (SEQ)) ";
            sqlstr += "                          		FROM COM_DOC_MST ";
            sqlstr += "                          	WHERE MNGT_NO = A.HBL_NO ";
            sqlstr += "                          			AND DOC_TYPE IN (" + dr["HBL_DOC_TYPE"].ToString() + ") ";
            sqlstr += "                          	GROUP BY DOC_TYPE) ";
            sqlstr += "                          	+ (  SELECT COUNT (MAX (SEQ)) ";
            sqlstr += "                          		FROM COM_DOC_MST ";
            sqlstr += "                          		WHERE MNGT_NO = (SELECT MBL_NO ";
            sqlstr += "                          							FROM FMS_HBL_MST ";
            sqlstr += "                          						WHERE HBL_NO = A.HBL_NO) ";
            sqlstr += "                          			AND DOC_TYPE IN (" + dr["MBL_DOC_TYPE"].ToString() + ") ";
            sqlstr += "                          	GROUP BY DOC_TYPE) + ";
            sqlstr += "                          (SELECT COUNT (MAX (SEQ)) ";
            sqlstr += "                          		FROM ACT_INV_MST MST ";
            sqlstr += "                          		 INNER JOIN ACT_INV_DTL DTL ";
            sqlstr += "                          		  ON MST.INV_NO = DTL.INV_NO ";
            sqlstr += "                          		  INNER JOIN COM_DOC_MST DOC ";
            sqlstr += "                          		  ON DTL.MBL_HBL_NO = DOC.MNGT_NO ";
            sqlstr += "                          		  WHERE MST.INV_NO = (SELECT MAX (INV_NO) FROM ACT_INV_DTL WHERE MBL_HBL_NO = A.HBL_NO) ";
            sqlstr += "                          		  AND DOC.DOC_TYPE = 'INV' AND MST.INV_YN = 'Y' ";
            sqlstr += "                          	GROUP BY DOC_TYPE) ";
            sqlstr += "                          	AS FILE_CNT ";
            sqlstr += "       FROM FMS_TRK_MST A		";
            //sqlstr += "                LEFT OUTER JOIN FMS_TRK_DTL B ON A.HBL_NO = B.HBL_NO		"; // 기존 쿼리
            sqlstr += "                LEFT OUTER JOIN (SELECT X.HBL_NO , X.CNTR_NO, MAX(X.SEQ)AS DTL_SEQ FROM(SELECT * FROM FMS_TRK_DTL WHERE ACT_YMD IS NOT NULL ORDER BY SEQ DESC)X GROUP BY X.HBL_NO, X.CNTR_NO) B ON A.HBL_NO = B.HBL_NO		";
            sqlstr += "                LEFT OUTER JOIN FMS_HBL_MST C ON A.HBL_NO = C.HBL_NO		";
            sqlstr += "                LEFT OUTER JOIN FMS_HBL_AUTH D ON C.HBL_NO = D.HBL_NO AND D.OFFICE_CD = '" + dr["OFFICE_CD"] + "'	";
            sqlstr += "                WHERE 1 = 1		";
            sqlstr += "                AND (A.HBL_NO = '" + dr["HBL_NO"] + "' OR A.CNTR_NO = '" + dr["HBL_NO"] + "' OR C.MBL_NO = '" + dr["HBL_NO"] + "')		";
            //sqlstr += "                GROUP BY A.HBL_NO,A.CNTR_NO		"; // 기존쿼리
            sqlstr += "	 )A		";

            return sqlstr;

        }

        /// <summary>
        /// 화물추적 AIS (X) DTL 쿼리
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
            sqlstr += "          , FILE_PATH";
            sqlstr += "          , CASE WHEN EVENT_STATUS = 'N' THEN EST_FILE_NM ELSE ACT_FILE_NM END AS EVENT_FILE_NM";
            sqlstr += "          , EVENT_STATUS";
            sqlstr += "          FROM (";
            sqlstr += " SELECT EVENT_CD	";
            sqlstr += "          ,  B.CD_NM AS EVENT_NM";
            sqlstr += "          ,  ACT_LOC_CD";
            sqlstr += "          ,  ACT_YMD";
            sqlstr += "          ,  ACT_HM";
            sqlstr += "          ,  EST_YMD";
            sqlstr += "          ,  EST_HM";
            sqlstr += "          ,  ACT_LOC_NM";
            sqlstr += "          , FILE_PATH";
            sqlstr += "          , EST_FILE_NM";
            sqlstr += "          , ACT_FILE_NM";
            sqlstr += "          , CASE WHEN (SELECT NOW_EVENT_CD FROM FMS_TRK_MST WHERE CNTR_NO = A.CNTR_NO AND HBL_NO = A.HBL_NO)  = A.EVENT_CD THEN 'Y' ";
            sqlstr += "                 WHEN ACT_YMD IS NOT NULL OR ACT_HM IS NOT NULL THEN 'E'";
            sqlstr += "                 ELSE 'N' END AS EVENT_STATUS";
            sqlstr += "         , '" + dr["EX_IM_TYPE"] + "' AS EX_IM_TYPE";
            sqlstr += "         , C.REQ_SVC";
            sqlstr += "     FROM FMS_TRK_DTL A";
            sqlstr += "               LEFT OUTER JOIN FMS_HBL_MST C ON C.HBL_NO = A.HBL_NO";
            sqlstr += "               INNER JOIN MDM_COM_CODE B ON A.EVENT_CD = B.COMN_CD ";
            sqlstr += "               AND B.GRP_CD  = (CASE WHEN C.REQ_SVC = 'SEA' THEN 'B30' WHEN C.REQ_SVC = 'AIR' THEN 'A30' END ) ";
            sqlstr += "               INNER JOIN MDM_ROUTE_IMG@DL_SYS D";
            sqlstr += "               ON B.OPT_ITEM10 = D.MNGT_NO";
            sqlstr += "  WHERE ";
            sqlstr += "  A.CNTR_NO = '" + dr["CNTR_NO"] + "'";
            sqlstr += "    AND A.HBL_NO = '" + dr["HBL_NO"] + "'";

            if (dr["EX_IM_TYPE"].ToString() == "E")
            {
                sqlstr += " AND A.EVENT_CD in ('EMT','CIG','LOD','POL','ARR')";
            }
            else if (dr["EX_IM_TYPE"].ToString() == "I")
            {
                //sqlstr += " AND A.EVENT_CD in ('CNI','POL','ARR','ICC','CYA','CYD')";
                sqlstr += " AND A.EVENT_CD in ('EMR','POL','ARR','ICC','CYA','CYD')";
            }
            else
            {
                sqlstr += " AND A.EVENT_CD in ('EMT','CNI','CIG','LOD','POL','ARR','ICC','CYA','CYD','EMR')";
            }

            sqlstr += "  ORDER BY SEQ";
            sqlstr += "  )";

            return sqlstr;
        }

        /// <summary>
        /// Tracking - 문서 파일 데이터 가져오기
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string GetLayerFileList_Query(DataRow dr)
        {
            sqlstr += "   SELECT MAX (A.MNGT_NO) AS MNGT_NO, ";
            sqlstr += "          MAX (A.MNGT_NO) AS MNGT_NM, ";
            sqlstr += "          MAX (A.SEQ) AS SEQ, ";
            sqlstr += "          MAX (A.FILE_NM) AS FILE_NM, ";
            sqlstr += "          DOC_TYPE, ";
            sqlstr += "          MAX (A.DOC_NO) AS DOC_NO, ";
            sqlstr += "          MAX (A.FILE_PATH) AS FILE_PATH, ";
            sqlstr += "          (SELECT CD_NM ";
            sqlstr += "             FROM MDM_COM_CODE ";
            sqlstr += "            WHERE GRP_CD = 'M33' AND COMN_CD = A.DOC_TYPE) ";
            sqlstr += "             AS DOC_NM, ";
            sqlstr += "          MAX (A.OFFICE_CD) AS OFFICE_CD, ";
            sqlstr += "          MAX (A.SYS_ID) AS SYS_ID, ";
            sqlstr += "          MAX (A.FORM_ID) AS FORM_ID, ";
            sqlstr += "          MAX (A.FILE_SIZE) AS FILE_SIZE, ";
            sqlstr += "          MAX (A.INS_YMD) AS INS_YMD, ";
            sqlstr += "          MAX (A.INS_HM) AS INS_HM, ";
            sqlstr += "          MAX (A.INS_YMD) || MAX (A.INS_HM) AS INS_TOTAL ";
            sqlstr += "     FROM COM_DOC_MST A ";
            sqlstr += "    WHERE MNGT_NO = '" + dr["MNGT_NO"].ToString() + "' ";
            sqlstr += "          AND DOC_TYPE IN (" + dr["HBL_DOC_TYPE"].ToString() + ",'INV') ";
            sqlstr += " GROUP BY DOC_TYPE ";
            sqlstr += " UNION  ";
            sqlstr += "   SELECT MAX (A.MNGT_NO) AS MNGT_NO, ";
            sqlstr += "          MAX (A.MNGT_NO) AS MNGT_NM, ";
            sqlstr += "          MAX (A.SEQ) AS SEQ, ";
            sqlstr += "          MAX (A.FILE_NM) AS FILE_NM, ";
            sqlstr += "          DOC_TYPE, ";
            sqlstr += "          MAX (A.DOC_NO) AS DOC_NO, ";
            sqlstr += "          MAX (A.FILE_PATH) AS FILE_PATH, ";
            sqlstr += "          (SELECT CD_NM ";
            sqlstr += "             FROM MDM_COM_CODE ";
            sqlstr += "            WHERE GRP_CD = 'M33' AND COMN_CD = A.DOC_TYPE) ";
            sqlstr += "             AS DOC_NM, ";
            sqlstr += "          MAX (A.OFFICE_CD) AS OFFICE_CD, ";
            sqlstr += "          MAX (A.SYS_ID) AS SYS_ID, ";
            sqlstr += "          MAX (A.FORM_ID) AS FORM_ID, ";
            sqlstr += "          MAX (A.FILE_SIZE) AS FILE_SIZE, ";
            sqlstr += "          MAX (A.INS_YMD) AS INS_YMD, ";
            sqlstr += "          MAX (A.INS_HM) AS INS_HM, ";
            sqlstr += "          MAX (A.INS_YMD) || MAX (A.INS_HM) AS INS_TOTAL ";
            sqlstr += "     FROM COM_DOC_MST A ";
            sqlstr += "    WHERE MNGT_NO = (SELECT MBL_NO ";
            sqlstr += "                     FROM FMS_HBL_MST ";
            sqlstr += "                    WHERE HBL_NO = '" + dr["MNGT_NO"].ToString() + "') ";
            sqlstr += "        AND DOC_TYPE IN (" + dr["MBL_DOC_TYPE"].ToString() + ") ";
            sqlstr += " GROUP BY DOC_TYPE ";

            return sqlstr;
        }
    }
}
