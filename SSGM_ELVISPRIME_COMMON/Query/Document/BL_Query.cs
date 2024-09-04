using System.Data;

namespace SSGM_ELVISPRIME_COMMON.Query.Document
{
    public class BL_Query
    {
        string sqlstr;

        /// <summary>
        /// B/L 조회 쿼리 
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string GetBLData_Query(DataRow dr)
        {
            sqlstr = " SELECT * ";
            sqlstr += "   FROM (SELECT ROWNUM AS RNUM,";
            sqlstr += "                FLOOR ( (ROWNUM - 1) / 10 + 1) AS PAGE,";
            sqlstr += "                COUNT (*) OVER () AS TOTCNT,";
            sqlstr += "                  HBL_NO ";
            sqlstr += "                , MBL_NO";
            sqlstr += "                , BL_TYPE";
            sqlstr += "                , UFN_GET_NAME_ADDR(SHP_ADDR,'NAME') AS SHP_ADDR";
            sqlstr += "                , UFN_GET_NAME_ADDR(CNE_ADDR,'NAME') AS CNE_ADDR";
            sqlstr += "                , POL_CD";
            sqlstr += "                , POL_NM";
            sqlstr += "                , POD_CD";
            sqlstr += "                , POD_NM";            
            sqlstr += "                , CHKBL_YN";
            sqlstr += "                , LINE_NM";
            sqlstr += "                , VSL";
            sqlstr += "                , TO_CHAR(TO_DATE(ETD),'YYYY.MM.DD') AS ETD";
            sqlstr += "                , TO_CHAR(TO_DATE(ETA),'YYYY.MM.DD') AS ETA";
            sqlstr += "                , TO_CHAR(TO_DATE(ONBD_YMD),'YYYY.MM.DD') AS ONBD_YMD";
            sqlstr += "                , REQ_SVC";
            sqlstr += "                , CASE WHEN EX_IM_TYPE = 'E' THEN '수출' ELSE '수입' END AS EX_IM_TYPE";
            sqlstr += "                , EVENT_NM";
            sqlstr += "                , BKG_NO";
            sqlstr += "                , (SELECT COUNT(FILE_NM) FROM COM_DOC_MST WHERE MNGT_NO = A.BKG_NO) AS FILE_CNT";
            sqlstr += "                , HBL_SEQ";
            sqlstr += "                , DO_SEQ";
            sqlstr += "                , ACT_LOC_NM";
            sqlstr += "                , INV_NO";
            sqlstr += "           FROM (  SELECT A.HBL_NO ";
            sqlstr += "                        , MAX(A.MBL_NO) AS MBL_NO";
            sqlstr += "                        , MAX(A.SHP_ADDR) AS SHP_ADDR";
            sqlstr += "                        , MAX(A.CNE_ADDR) AS CNE_ADDR ";
            sqlstr += "                        , MAX(A.POL_CD) AS POL_CD";            
            sqlstr += "                        , MAX(A.POL_NM) AS POL_NM";
            sqlstr += "                        , MAX(A.POD_CD) AS POD_CD";
            sqlstr += "                        , MAX(A.POD_NM) AS POD_NM";
            sqlstr += "                        , MAX(A.ETD) AS ETD";
            sqlstr += "                        , MAX(A.VSL) AS VSL";
            sqlstr += "                        , MAX(A.ETA) AS ETA ";
            sqlstr += "                        , MAX(A.LINE_NM) AS LINE_NM ";
            sqlstr += "                        , MAX(A.BL_TYPE) AS BL_TYPE ";
            sqlstr += "                        , MAX(B.CHKBL_YN) AS CHKBL_YN";
            sqlstr += "                        , MAX(A.ONBD_YMD) AS ONBD_YMD";
            sqlstr += "                        , (SELECT MAX(ACT_LOC_NM) FROM FMS_TRK_DTL WHERE HBL_NO = A.HBL_NO AND EVENT_CD = (SELECT MAX(NOW_EVENT_CD) FROM FMS_TRK_MST WHERE HBL_NO = A.HBL_NO)) AS EVENT_NM";
            sqlstr += "                        , (SELECT MAX(SEQ) FROM FMS_TRK_DTL WHERE HBL_NO = A.HBL_NO AND ACT_YMD IS NOT NULL AND ACT_HM IS NOT NULL)  AS TRACK_SEQ";
            sqlstr += "                        , MAX(A.REQ_SVC) AS REQ_SVC";
            sqlstr += "                        , MAX(C.EX_IM_TYPE) AS EX_IM_TYPE";
            sqlstr += "                        , (SELECT MAX(BKG_NO) FROM PRM_BKG_MST WHERE HBL_NO = A.HBL_NO) AS BKG_NO";
            sqlstr += "                        , (SELECT MAX(SEQ) FROM COM_DOC_MST WHERE MNGT_NO = A.HBL_NO AND DOC_TYPE = 'HBL') AS HBL_SEQ";      //문서 관리에서 HBL건이 있는 것을 찾음
            sqlstr += "                        , (SELECT MAX(SEQ) FROM COM_DOC_MST WHERE MNGT_NO = A.HBL_NO AND DOC_TYPE = 'DO') AS DO_SEQ";
            sqlstr += "                        , (SELECT MAX(ACT_LOC_NM) FROM FMS_TRK_DTL WHERE HBL_NO = A.HBL_NO)   AS ACT_LOC_NM";
            sqlstr += "                        , (SELECT MAX(MST.INV_NO) FROM ACT_INV_MST MST INNER JOIN ACT_INV_DTL DTL ON MST.INV_NO = DTL.INV_NO WHERE DTL.MBL_HBL_NO = A.HBL_NO) AS INV_NO ";
            sqlstr += "  FROM FMS_HBL_MST A";
            sqlstr += "       LEFT OUTER JOIN FMS_HBL_OTH B ";
            sqlstr += "                       ON A.HBL_NO = B.HBL_NO";
            sqlstr += "       LEFT OUTER JOIN FMS_HBL_AUTH C ";
            sqlstr += "                       ON A.HBL_NO = C.HBL_NO AND OFFICE_CD = '" + dr["OFFICE_CD"].ToString() + "'";
            sqlstr += "  WHERE 1 = 1";
            sqlstr += "  AND C.OFFICE_CD = '" + dr["OFFICE_CD"].ToString() + "'";
            sqlstr += "     AND B.CHKBL_YN = 'Y'";

            if (dr["AUTH_KEY"].ToString() == "")
            {
                if (dr["USER_TYPE"].ToString() != "M")
                {
                    //화주면 CUST_CD , 파트너면 PTN_CD로 조회
                    if (dr["USER_TYPE"].ToString() == "S")
                    {
                        sqlstr += "     AND C.CUST_CD = '" + dr["CUST_CD"].ToString() + "'";
                    }
                    else if (dr["USER_TYPE"].ToString() == "P")
                    {
                        sqlstr += "     AND C.PTN_CD = '" + dr["CUST_CD"].ToString() + "'";
                    }
                }
            }

            if (dr["MNGT_NO"].ToString() != "")
            {
                sqlstr += "     AND A.HBL_NO = '" + dr["MNGT_NO"].ToString() + "'";
            }
            else
            {
                if (dr["ETD_ETA"].ToString() == "ETD_ETA")
                {
                    sqlstr += " AND ((C.EX_IM_TYPE = 'E' AND A.ETD >= '" + dr["STRT_YMD"].ToString() + "' AND A.ETD <= '" + dr["END_YMD"].ToString() + "')  OR (C.EX_IM_TYPE = 'I' AND A.ETA >= '" + dr["STRT_YMD"].ToString() + "'AND A.ETA <= '" + dr["END_YMD"].ToString() + "'))";
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

                sqlstr += "    AND C.EX_IM_TYPE = '" + dr["EX_IM_TYPE"].ToString() + "'";                

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
            sqlstr += "  GROUP BY A.HBL_NO";
            if (dr["ID"].ToString() != "")
            {
                sqlstr += " ORDER BY " + dr["ID"].ToString() + " " + dr["ORDER"].ToString() + "";
            }
            else
                sqlstr += " ORDER BY ETD";
            sqlstr += " ) A";
            sqlstr += ")WHERE PAGE = " + dr["PAGE"].ToString();

            return sqlstr;
        }
        
        /// <summary>
        /// 레이어 팝업 pdf 데이터 가져오는 쿼리
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string GetBLPrint_Query(DataRow dr)
        {

            sqlstr += "SELECT * FROM ";
            sqlstr += " COM_DOC_MST  ";
            sqlstr += " WHERE  MNGT_NO = '" + dr["HBL_NO"].ToString() + "'";
            sqlstr += "   AND  DOC_TYPE = '" + dr["DOC_TYPE"].ToString() + "'";
            sqlstr += "   AND SEQ = (SELECT MAX(SEQ) FROM COM_DOC_MST WHERE MNGT_NO = '" + dr["HBL_NO"].ToString() + "' AND DOC_TYPE = '" + dr["DOC_TYPE"].ToString() + "')";
            return sqlstr;
        }

        /// <summary>
        /// Check B/L 수정요청사항 저장
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string InsertBLRequest_Query(DataRow dr)
        {
            string sqlstr = "";
            sqlstr += "INSERT INTO PRM_CHKBL_MST (HBL_NO , SEQ,RMK,STATUS,REQ_YMD,CUST_CD,CUST_NM,CUST_PIC_NM,CUST_PIC_MAIL,REQ_SVC,INS_USR,INS_YMD,INS_HM,UPD_USR,UPD_YMD,UPD_HM)";
            sqlstr += "   VALUES(";
            sqlstr += "    '" + dr["HBL_NO"].ToString() + "'";
            sqlstr += " , (SELECT NVL(MAX(SEQ),0)+1 AS SEQ	   ";
            sqlstr += "  FROM PRM_CHKBL_MST	";
            sqlstr += "  WHERE HBL_NO = '" + dr["HBL_NO"].ToString() + "') ";
            sqlstr += "  ,REPLACE('" + dr["RMK"] + "',CHR(10),CHR(13)|| CHR(10))";
            sqlstr += "    ,'Q'";
            sqlstr += "   , UFN_DATE_FORMAT('DATE') ";
            sqlstr += "    ,'" + dr["CUST_CD"].ToString() + "'";
            sqlstr += "    ,(SELECT CUST_NM FROM MDM_CUST_MST WHERE CUST_CD = '" + dr["CUST_CD"].ToString() + "')";
            sqlstr += "   ,'" + dr["LOC_NM"].ToString() + "'";
            sqlstr += "   ,'" + dr["EMAIL"].ToString() + "'";
            sqlstr += "   ,'" + dr["REQ_SVC"].ToString() + "'";
            sqlstr += "   ,'" + dr["LOC_NM"].ToString() + "'";
            sqlstr += "   , UFN_DATE_FORMAT('DATE') ";
            sqlstr += "   , UFN_DATE_FORMAT('TIME') ";
            sqlstr += "   ,'" + dr["LOC_NM"] + "'";
            sqlstr += "   , UFN_DATE_FORMAT('DATE') ";
            sqlstr += "   , UFN_DATE_FORMAT('TIME')) ";

            return sqlstr;
        }

        /// <summary>
        /// Check B/L 수정요청사항 수정
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string UpdateBLRequest_Query(DataRow dr)
        {
            string sqlstr = "";
            sqlstr += " UPDATE PRM_CHKBL_MST";
            sqlstr += "      SET SEQ = (SELECT NVL(MAX(SEQ),0)+1 AS SEQ	   ";
            sqlstr += "  FROM PRM_CHKBL_MST	";
            sqlstr += "  WHERE HBL_NO = '" + dr["HBL_NO"].ToString() + "') ";
            sqlstr += "     , RMK =  REPLACE('" + dr["RMK"].ToString() + "',CHR(10),CHR(13)||CHR(10)) ";
            sqlstr += "     , STATUS =  'Q' ";
            sqlstr += "     , REQ_YMD =  UFN_DATE_FORMAT('DATE') ";
            sqlstr += "     , CUST_CD =  '" + dr["CUST_CD"].ToString() + "' ";
            sqlstr += "     , CUST_NM =  (SELECT CUST_NM FROM MDM_CUST_MST WHERE CUST_CD = '" + dr["CUST_CD"].ToString() + "')";
            sqlstr += "     , CUST_PIC_NM =  '" + dr["LOC_NM"].ToString() + "' ";
            sqlstr += "     , CUST_PIC_MAIL =  '" + dr["EMAIL"].ToString() + "' ";
            sqlstr += "     , REQ_SVC =  '" + dr["REQ_SVC"].ToString() + "' ";
            sqlstr += "     , UPD_USR =  '" + dr["LOC_NM"].ToString() + "' ";
            sqlstr += "     , UPD_YMD =  UFN_DATE_FORMAT('DATE') ";
            sqlstr += "     , UPD_HM =  UFN_DATE_FORMAT('TIME')";
            sqlstr += "      WHERE HBL_NO =  '" + dr["HBL_NO"].ToString() + "'";
            return sqlstr;            
        }

        /// <summary>
        /// Check B/L 수정요청사항 데이터 가져오기
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string SearchBLRequest_Query(DataRow dr)
        {
            string sqlstr = "";
            sqlstr += " SELECT * FROM PRM_CHKBL_MST WHERE HBL_NO =  '" + dr["HBL_NO"].ToString() + "'";
            return sqlstr;
        }

        /// <summary>
        /// Pre - Alert 데이터 가져오기
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string fnGetPreAlert_Query(DataRow dr)
        {
            string sqlstr = "";

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
            sqlstr += "    WHERE MNGT_NO = '" + dr["HBL_NO"].ToString() + "' ";
            sqlstr += "          AND DOC_TYPE IN (" + dr["DOC_TYPE"].ToString() + ") ";
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
            sqlstr += "                    WHERE HBL_NO = '" + dr["HBL_NO"].ToString() + "') ";
            sqlstr += "        AND DOC_TYPE IN ('DC', 'MBL') ";
            sqlstr += " GROUP BY DOC_TYPE ";

            return sqlstr;
        }


        /// <summary>
        /// 업로드 파일 정보 가져오기
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string SelectUploadFile(DataRow dr)
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
            sqlstr += "          AND DOC_TYPE IN (" + dr["HBL_DOC_TYPE"].ToString() + ") ";
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
            sqlstr += " UNION  ";
            sqlstr += "   SELECT MAX (A.MNGT_NO) AS MNGT_NO, ";
            sqlstr += "          MAX (MST.INV_NO) AS MNGT_NM, ";
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
            sqlstr += "     FROM ACT_INV_MST MST ";
            sqlstr += "          INNER JOIN ACT_INV_DTL DTL ";
            sqlstr += "             ON MST.INV_NO = DTL.INV_NO ";
            sqlstr += "          INNER JOIN COM_DOC_MST A ";
            sqlstr += "             ON DTL.MBL_HBL_NO = A.MNGT_NO ";
            sqlstr += "    WHERE     MST.INV_NO = '" + dr["INV_NO"].ToString() + "' ";
            sqlstr += "          AND MST.INV_YN = 'Y' ";
            sqlstr += "          AND A.DOC_TYPE = 'INV' ";
            sqlstr += " GROUP BY A.DOC_TYPE ";
            sqlstr += " UNION  ";
            sqlstr += "   SELECT A.MNGT_NO AS MNGT_NO, ";
            sqlstr += "          A.MNGT_NO AS MNGT_NM, ";
            sqlstr += "          A.SEQ AS SEQ, ";
            sqlstr += "          A.FILE_NM AS FILE_NM, ";
            sqlstr += "          DOC_TYPE, ";
            sqlstr += "          A.DOC_NO AS DOC_NO, ";
            sqlstr += "          A.FILE_PATH AS FILE_PATH, ";
            sqlstr += "          (SELECT CD_NM ";
            sqlstr += "             FROM MDM_COM_CODE ";
            sqlstr += "            WHERE GRP_CD = 'M33' AND COMN_CD = A.DOC_TYPE) ";
            sqlstr += "             AS DOC_NM, ";
            sqlstr += "          A.OFFICE_CD AS OFFICE_CD, ";
            sqlstr += "          A.SYS_ID AS SYS_ID, ";
            sqlstr += "          A.FORM_ID AS FORM_ID, ";
            sqlstr += "          A.FILE_SIZE AS FILE_SIZE, ";
            sqlstr += "          A.INS_YMD AS INS_YMD, ";
            sqlstr += "          A.INS_HM AS INS_HM, ";
            sqlstr += "          A.INS_YMD || A.INS_HM AS INS_TOTAL ";
            sqlstr += "     FROM COM_DOC_MST A ";
            sqlstr += "    WHERE MNGT_NO = '" + dr["BKG_NO"] + "' ";

            return sqlstr;
        }

        /// <summary>
        /// Pre - Alert 데이터 가져오기
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string fnGetDocFile_Query(DataRow dr)
        {
            sqlstr = "";

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
            sqlstr += "    WHERE MNGT_NO = '" + dr["HBL_NO"].ToString() + "' ";
            sqlstr += "          AND DOC_TYPE IN (" + dr["HBL_DOC_TYPE"].ToString() + ") ";
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
            sqlstr += "                    WHERE HBL_NO = '" + dr["HBL_NO"].ToString() + "') ";
            sqlstr += "        AND DOC_TYPE IN (" + dr["MBL_DOC_TYPE"].ToString() + ") ";
            sqlstr += " GROUP BY DOC_TYPE ";
            sqlstr += " UNION  ";
            sqlstr += "   SELECT MAX (A.MNGT_NO) AS MNGT_NO, ";
            sqlstr += "          MAX (MST.INV_NO) AS MNGT_NM, ";
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
            sqlstr += "     FROM ACT_INV_MST MST ";
            sqlstr += "          INNER JOIN ACT_INV_DTL DTL ";
            sqlstr += "             ON MST.INV_NO = DTL.INV_NO ";
            sqlstr += "          INNER JOIN COM_DOC_MST A ";
            sqlstr += "             ON DTL.MBL_HBL_NO = A.MNGT_NO ";
            sqlstr += "    WHERE     MST.INV_NO = '" + dr["INV_NO"].ToString() + "' ";
            sqlstr += "          AND MST.INV_YN = 'Y' ";
            sqlstr += "          AND A.DOC_TYPE = 'INV' ";
            sqlstr += " GROUP BY DOC_TYPE ";

            return sqlstr;
        }
    }
}
