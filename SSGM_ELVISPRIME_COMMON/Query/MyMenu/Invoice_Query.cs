using System.Data;

namespace SSGM_ELVISPRIME_COMMON.Query.MyMenu
{
    public class Invoice_Query
    {
        string sqlstr;

        /// <summary>
        /// 인보이스 조회 쿼리 
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string GetInvData_Query(DataRow dr)
        {
            sqlstr += "SELECT *";
            sqlstr += "  FROM (SELECT ROWNUM AS RNUM,";
            sqlstr += "               FLOOR ( (ROWNUM - 1) / 10 + 1) AS PAGE,";
            sqlstr += "               COUNT (*) OVER () AS TOTCNT,";
            sqlstr += "               REQ_SVC,";
            sqlstr += "               CASE WHEN EX_IM_TYPE = 'E' THEN '수출' ELSE '수입' END AS EX_IM_TYPE,";
            sqlstr += "               INV_NO,";
            sqlstr += "               INV_YMD,";
            sqlstr += "               MBL_HBL_NO,";
            sqlstr += "               HBL_NO,";
            sqlstr += "               MBL_NO,";
            sqlstr += "               POL_CD,";
            sqlstr += "               POD_CD,";
            sqlstr += "               POL_NM,";
            sqlstr += "               POD_NM,";
            sqlstr += "               TRIM (TO_CHAR (NVL(UNIT_PRC,0), '999,999,999,990'))  AS UNIT_PRC,";
            sqlstr += "               FARE_LOC_AMT,";
            sqlstr += "               TRIM (TO_CHAR (NVL(FARE_VAT_AMT,0), '999,999,999,990'))  AS FARE_VAT_AMT ,";
            sqlstr += "               FARE_AMT,";
            sqlstr += "               ETD,";
            sqlstr += "               ETA,";
            sqlstr += "               ONBD_YMD,";
            sqlstr += "               TO_CHAR(FARE_AMT)  AS FARE_TOT_AMT";
            sqlstr += "          FROM (  SELECT MAX(A.REQ_SVC) AS REQ_SVC,";
            sqlstr += "                         MAX(A.MBL_HBL_NO) AS MBL_HBL_NO ,";
            sqlstr += "                         MAX(C.HBL_NO) AS HBL_NO ,";
            sqlstr += "                         MAX(C.MBL_NO) AS MBL_NO ,";
            sqlstr += "                         MAX(C.POL_CD) AS POL_CD ,";
            sqlstr += "                         MAX(C.POD_CD) AS POD_CD ,";
            sqlstr += "                         MAX(C.POL_NM) AS POL_NM ,";
            sqlstr += "                         MAX(C.POD_NM) AS POD_NM ,";
            sqlstr += "                         (SELECT UNIT_PRC FROM ACT_INV_DTL WHERE INV_NO = A.INV_NO AND FARE_CD = 'AAF') AS UNIT_PRC ,";
            sqlstr += "                         MAX(A.EX_IM_TYPE) AS EX_IM_TYPE,";
            sqlstr += "                          TO_CHAR (TO_DATE (MAX (C.ONBD_YMD)), 'YYYY.MM.DD') AS ONBD_YMD,";
            sqlstr += "                         A.INV_NO,";
            sqlstr += "                         TO_CHAR (TO_DATE (MAX (B.INV_YMD)), 'YYYY.MM.DD') AS INV_YMD,";
            sqlstr += "                         MAX(A.CURR_CD) AS CURR_CD,";
            sqlstr += "                         TRIM(TO_CHAR(SUM (A.FARE_LOC_AMT), '999,999,999,990')) AS FARE_LOC_AMT,";
            sqlstr += "                         SUM(A.FARE_VAT_AMT) AS FARE_VAT_AMT ,";
            sqlstr += "                         TO_CHAR (TO_DATE (MAX(A.ETD)), 'YYYY.MM.DD') AS ETD,";
            sqlstr += "                         TO_CHAR (TO_DATE (MAX(A.ETA)), 'YYYY.MM.DD') AS ETA,";
            sqlstr += "                         TRIM(TO_CHAR(SUM (A.FARE_LOC_AMT) + SUM (A.FARE_VAT_AMT), '999,999,999,990')) AS FARE_AMT,";
            sqlstr += "                         SUM (A.FARE_LOC_AMT) + SUM (A.FARE_VAT_AMT) AS TOT_AMT";
            sqlstr += "                    FROM    ACT_INV_DTL A";
            sqlstr += "                         LEFT OUTER JOIN";
            sqlstr += "                            ACT_INV_MST B";
            sqlstr += "                         ON A.INV_NO = B.INV_NO AND A.OFFICE_CD = B.OFFICE_CD";
            sqlstr += "                         LEFT OUTER JOIN";
            sqlstr += "                            FMS_HBL_MST C";
            sqlstr += "                         ON A.MNGT_NO = C.HBL_NO";
            sqlstr += "                         LEFT OUTER JOIN FMS_HBL_AUTH D ";
            sqlstr += "                             ON C.HBL_NO = D.HBL_NO ";
            sqlstr += "  WHERE 1 = 1";
            sqlstr += "  AND A.OFFICE_CD = '" + dr["OFFICE_CD"].ToString() + "'";            
            sqlstr += "  AND A.MBL_HBL_TYPE = 'H'";
            sqlstr += "  AND B.INV_TYPE = 'S'";
            sqlstr += "  AND B.INV_YN = 'Y'";

            if (dr["AUTH_KEY"].ToString() == "")
            {
                if (dr["USER_TYPE"].ToString() != "M")
                {
                    //화주면 CUST_CD , 파트너면 PTN_CD로 조회
                    if (dr["USER_TYPE"].ToString() == "S")
                    {
                        sqlstr += "     AND D.CUST_CD = '" + dr["CUST_CD"].ToString() + "'";
                    }
                    else if (dr["USER_TYPE"].ToString() == "P")
                    {
                        sqlstr += "     AND D.PTN_CD = '" + dr["CUST_CD"].ToString() + "'";
                    }
                }
            }

            if (dr["MNGT_NO"].ToString() != "")
            {
                sqlstr += "     AND A.INV_NO = '" + dr["MNGT_NO"].ToString() + "'";
            }
            else
            {
                sqlstr += "    AND A.EX_IM_TYPE = '" + dr["EX_IM_TYPE"].ToString() + "'";
                sqlstr += "    AND ( ('" + dr["REQ_SVC"] + "' IS NULL and 1 = 1 ) or ('" + dr["REQ_SVC"] + "' IS NOT NULL and A.REQ_SVC = '" + dr["REQ_SVC"] + "' ) )";

                if (dr["ETD_ETA"].ToString() == "ETD_ETA")
                {
                    sqlstr += " AND ((A.EX_IM_TYPE = 'E' AND B.INV_YMD >= '" + dr["STRT_YMD"] + "' AND B.INV_YMD <= '" + dr["END_YMD"] + "')  OR (A.EX_IM_TYPE = 'I' AND B.INV_YMD >= '" + dr["STRT_YMD"] + "'AND B.INV_YMD <= '" + dr["END_YMD"] + "'))";
                }
                else
                {
                    if (dr["STRT_YMD"].ToString() != "")
                    {
                        sqlstr += "    AND A." + dr["ETD_ETA"] + " >= '" + dr["STRT_YMD"] + "'";
                    }
                    if (dr["END_YMD"].ToString() != "")
                    {
                        sqlstr += "    AND A." + dr["ETD_ETA"] + " <= '" + dr["END_YMD"] + "'";
                    }
                }

                if (dr["HBL_NO"].ToString() != "")
                {
                    sqlstr += "    AND ( ('" + dr["HBL_NO"] + "' IS NULL and 1 = 1 )or ('" + dr["HBL_NO"] + "' IS NOT NULL and C.HBL_NO LIKE '%" + dr["HBL_NO"] + "%' ) )";
                }

                if (dr["POL_CD"].ToString() == "")
                {
                    sqlstr += "    AND ( (UPPER('" + dr["POL"] + "') IS NULL and 1 = 1 ) or (UPPER('" + dr["POL"] + "') IS NOT NULL and C.POL_CD LIKE UPPER('%" + dr["POL"] + "%')) or (UPPER('" + dr["POL"] + "') IS NOT NULL and (SELECT LOC_NM FROM MDM_PORT_MST WHERE LOC_CD = C.POL_CD) LIKE UPPER('%" + dr["POL"] + "%') ) )";
                }
                else
                {
                    sqlstr += "    AND ( (UPPER('" + dr["POL"] + "') IS NULL and 1 = 1 ) or (UPPER('" + dr["POL"] + "') IS NOT NULL and C.POL_CD LIKE UPPER('%" + dr["POL_CD"] + "%') ) or (UPPER('" + dr["POL"] + "') IS NOT NULL and (SELECT LOC_NM FROM MDM_PORT_MST WHERE LOC_CD = C.POL_CD) LIKE UPPER('%" + dr["POL"] + "%') ) )";
                }

                if (dr["POD_CD"].ToString() == "")
                {
                    sqlstr += "    AND ( (UPPER('" + dr["POD"] + "') IS NULL and 1 = 1 ) or (UPPER('" + dr["POD"] + "') IS NOT NULL and C.POD_CD LIKE UPPER('%" + dr["POD"] + "%') ) or (UPPER('" + dr["POD"] + "') IS NOT NULL and (SELECT LOC_NM FROM MDM_PORT_MST WHERE LOC_CD = C.POD_CD) LIKE UPPER('%" + dr["POD"] + "%') ) )";
                }
                else
                {
                    sqlstr += "    AND ( (UPPER('" + dr["POD"] + "') IS NULL and 1 = 1 ) or (UPPER('" + dr["POD"] + "') IS NOT NULL and C.POD_CD LIKE UPPER('%" + dr["POD_CD"] + "%') ) or (UPPER('" + dr["POD"] + "') IS NOT NULL and (SELECT LOC_NM FROM MDM_PORT_MST WHERE LOC_CD = C.POD_CD) LIKE UPPER('%" + dr["POD"] + "%') ) )";
                }
            }

            sqlstr += " GROUP BY A.INV_NO ";
            if (dr["ID"].ToString() != "")
            {
                sqlstr += "ORDER BY " + dr["ID"] + " " + dr["ORDER"] + "";
            }
            else
                sqlstr += "ORDER BY ETD";
            sqlstr += " ))";
            sqlstr += "WHERE PAGE = " + dr["PAGE"];
            return sqlstr;
        }

        /// <summary>
        /// Invoice pdf 데이터 가져오기
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string GetInvPrint_Query(DataRow dr)
        {

            sqlstr += "SELECT * FROM ";
            sqlstr += " COM_DOC_MST  ";
            sqlstr += " WHERE  MNGT_NO = '" + dr["HBL_NO"].ToString() + "'";
            sqlstr += "   AND  DOC_TYPE = '" + dr["DOC_TYPE"].ToString() + "'";
            sqlstr += "   AND SEQ = (SELECT MAX(SEQ) FROM COM_DOC_MST WHERE MNGT_NO = '" + dr["HBL_NO"].ToString() + "' AND DOC_TYPE = '" + dr["DOC_TYPE"].ToString() + "')";
            return sqlstr;
        }

        /// <summary>
        /// Invoice 수정요청사항 데이터 저장
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string InsertInvRequest_Query(DataRow dr)
        {
            string sqlstr = "";
            sqlstr += "INSERT INTO PRM_INV_MST (INV_NO,HBL_NO , SEQ,RMK,STATUS,REQ_YMD,CUST_CD,CUST_NM,CUST_PIC_NM,CUST_PIC_MAIL,REQ_SVC,INS_USR,INS_YMD,INS_HM,UPD_USR,UPD_YMD,UPD_HM)";
            sqlstr += "   VALUES(";
            sqlstr += "    '" + dr["INV_NO"].ToString() + "'";
            sqlstr += "   , '" + dr["HBL_NO"].ToString() + "'";
            sqlstr += " , (SELECT NVL(MAX(SEQ),0)+1 AS SEQ	   ";
            sqlstr += "  FROM PRM_INV_MST	";
            sqlstr += "  WHERE INV_NO = '" + dr["INV_NO"].ToString() + "') ";
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
        /// Invoice 수정요성사항 업데이트
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string UpdateInvRequest_Query(DataRow dr)
        {
            string sqlstr = "";
            sqlstr += " UPDATE PRM_INV_MST";
            sqlstr += "      SET SEQ = (SELECT NVL(MAX(SEQ),0)+1 AS SEQ	   ";
            sqlstr += "  FROM PRM_INV_MST	";
            sqlstr += "  WHERE INV_NO = '" + dr["INV_NO"].ToString() + "') ";
            sqlstr += "     , RMK =  REPLACE('" + dr["RMK"].ToString() + "',CHR(10),CHR(13)||CHR(10)) ";
            sqlstr += "     , STATUS =  'Q' ";
            sqlstr += "     , REQ_YMD =  UFN_DATE_FORMAT('DATE') ";
            sqlstr += "     , CUST_CD =  '" + dr["CUST_CD"].ToString() + "' ";
            sqlstr += "     , CUST_NM =  (SELECT CUST_NM FROM MDM_CUST_MST WHERE CUST_CD = '" + dr["CUST_CD"].ToString() + "')";
            sqlstr += "     , CUST_PIC_NM =  '" + dr["LOC_NM"].ToString() + "' ";
            sqlstr += "     , CUST_PIC_MAIL =  '" + dr["EMAIL"].ToString() + "' ";
            sqlstr += "     , HBL_NO =  '" + dr["HBL_NO"].ToString() + "' ";
            sqlstr += "     , REQ_SVC =  '" + dr["REQ_SVC"].ToString() + "' ";
            sqlstr += "     , UPD_USR =  '" + dr["LOC_NM"].ToString() + "' ";
            sqlstr += "     , UPD_YMD =  UFN_DATE_FORMAT('DATE') ";
            sqlstr += "     , UPD_HM =  UFN_DATE_FORMAT('TIME')";
            sqlstr += "      WHERE INV_NO =  '" + dr["INV_NO"].ToString() + "'";
            return sqlstr;
        }

        /// <summary>
        /// Invoice 수정요성사항 데이터 가져오기
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string SearchInvRequest_Query(DataRow dr)
        {
            string sqlstr = "";
            sqlstr += " SELECT A.* , B.POD_CD FROM PRM_INV_MST A   LEFT OUTER JOIN FMS_HBL_MST B ON A.HBL_NO = B.HBL_NO WHERE A.INV_NO =  '" + dr["INV_NO"].ToString() + "'";
            return sqlstr;
        }

    }
}
