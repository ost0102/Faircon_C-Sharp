using System;
using System.Configuration;
using System.Data;

namespace SSGM_ELVISPRIME_COMMON.Query.SSGM
{
    class Booking_Query
    {
        string sqlstr;

        //string _SeaTariffCode = ConfigurationManager.AppSettings["SeaTariff"].ToString();
        string _SeaTariffCode = "";

        /// <summary>
        /// 부킹 타입 데이터 가져오기
        /// </summary>
        /// <param name="bkg_no"></param>
        /// <returns></returns>
        public string fnGetFIleType_Query(DataRow dr)
        {
            string sqlstr = "";

            sqlstr += "   SELECT COMN_CD, CD_NM ";
            sqlstr += "     FROM MDM_COM_CODE ";
            sqlstr += "    WHERE GRP_CD = 'M33' ";

            if (dr["FILE_TYPE"].ToString() != "")
            {
                sqlstr += "    AND COMN_CD IN (" + dr["FILE_TYPE"] + ") ";
            }
            else
            {
                sqlstr += "    AND COMN_CD IN ('') ";
                //sqlstr += "    WHERE GRP_CD = 'M33' AND COMN_CD IN ('CIPL','CO','CC','IP') ";
            }

            sqlstr += " ORDER BY SORT ";

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
            sqlstr += " , EX_IM_TYPE	";
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
            sqlstr += " , RMK	";
            sqlstr += " , TRANSIT_TIME	";
            sqlstr += " , TRANSIT_TIME_NM	";
            sqlstr += " , TS_DTL	";
            sqlstr += " , TS_COUNT	";
            sqlstr += " , CARGO_CLOSE_YMD ";
            sqlstr += " , CARGO_CLOSE_HM ";
            sqlstr += " , CNTR_TYPE ";
            sqlstr += " , PREV_CLOSE ";
            sqlstr += " , DOC_CLOSE ";
            sqlstr += " , BKG_YN ";
            sqlstr += " , USE_YN ";
            sqlstr += "         FROM(";
            sqlstr += "  SELECT SCH_NO";
            sqlstr += "           , OFFICE_CD";
            sqlstr += "           , REQ_SVC";
            sqlstr += "           , CASE WHEN POL_CD LIKE 'KR%' THEN 'E' ELSE 'I' END AS EX_IM_TYPE";
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
            sqlstr += "            , CASE WHEN CNTR_TYPE = 'F' THEN 'FCL' WHEN CNTR_TYPE = 'L' THEN 'LCL' WHEN A.CNTR_TYPE = 'C' THEN 'CONSOL' END AS CNTR_TYPE  ";
            sqlstr += "            , TO_CHAR (TO_DATE (DOC_CLOSE_YMD), 'YYYYMMDD') || TO_CHAR (TO_DATE (LPAD(DOC_CLOSE_HM,'4','0'), 'hh24miss'), 'hh24mi') AS DOC_CLOSE  ";
            sqlstr += "                    FROM PRM_SCH_MST A ";
            sqlstr += "                         INNER JOIN MDM_CARR_MST CARR ";
            sqlstr += "                            ON A.LINE_CD = CARR.CARR_CD ";
            sqlstr += "                         LEFT OUTER JOIN MDM_LINE_IMG B ";
            sqlstr += "                            ON CARR.SCAC_CD = B.LINE_CD ";
            sqlstr += "                               OR CARR.EDI_CD = B.LINE_CD ";
            sqlstr += "   WHERE 1 = 1 ";

            if (dr["SCH_NO"].ToString() != "")
            {
                sqlstr += "    AND A.SCH_NO = '" + dr["SCH_NO"] + "'";
            }
            else
            {
                sqlstr += "    AND A.WEB_FLAG = 'Y' ";
                sqlstr += "    AND A.ETD >=  '" + dr["ETD_START"] + "'";
                sqlstr += "    AND ( ('" + dr["REQ_SVC"] + "' IS NULL and 1 = 1 ) or ('" + dr["REQ_SVC"] + "'  = 'ALL' and 1 = 1 ) or ('" + dr["REQ_SVC"] + "' IS NOT NULL and A.REQ_SVC = '" + dr["REQ_SVC"] + "' ) ) ";
                if (dr["CNTR_TYPE"].ToString() != "")
                {
                    if (dr["CNTR_TYPE"].ToString() == "F")
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
            }

            if (dr["ID"].ToString() != "")
            {
                sqlstr += "ORDER BY " + dr["ID"] + " " + dr["ORDER"] + "";
            }
            else
                sqlstr += "ORDER BY ETA";
            sqlstr += " )A ";
            sqlstr += " WHERE DOC_CLOSE > TO_CHAR(SYSDATE,'YYYYMMDDhh24mi') ";
            sqlstr += ")WHERE PAGE = " + dr["PAGE"];

            return sqlstr;
        }

        public string GetBkgSchData_Query(DataRow dr)
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
            sqlstr += " , EX_IM_TYPE	";
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
            sqlstr += " , RMK	";
            sqlstr += " , TRANSIT_TIME	";
            sqlstr += " , TRANSIT_TIME_NM	";
            sqlstr += " , TS_DTL	";
            sqlstr += " , TS_COUNT	";
            sqlstr += " , CARGO_CLOSE_YMD ";
            sqlstr += " , CARGO_CLOSE_HM ";
            sqlstr += " , CNTR_TYPE ";
            sqlstr += " , PREV_CLOSE ";
            sqlstr += " , DOC_CLOSE ";
            sqlstr += "         FROM(";
            sqlstr += "  SELECT A.SCH_NO";
            sqlstr += "           , OFFICE_CD";
            sqlstr += "           , A.REQ_SVC";
            sqlstr += "           , CASE WHEN POL_CD LIKE 'KR%' THEN 'E' ELSE 'I' END AS EX_IM_TYPE";
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
            sqlstr += "            , A.RMK";
            sqlstr += "            , A.LINE_TYPE";
            sqlstr += "            , ( SELECT CNTN_GRP_CD FROM MDM_PORT_MST  WHERE LOC_CD= A.POD_CD) AS CNTN_GRP_CD ";
            sqlstr += "            , CASE WHEN TS_CNT =  0 THEN (SELECT MAX(SEQ) - 1 FROM PRM_SCH_TS WHERE SCH_NO = A.SCH_NO) ELSE 0 END AS TS_COUNT ";
            sqlstr += "            , CASE WHEN TS_CNT = '0'THEN(SELECT LISTAGG(POL_CD, '-') WITHIN GROUP (ORDER BY SEQ) AS POL_CD FROM PRM_SCH_TS WHERE SCH_NO = A.SCH_NO)|| '-' || A.POD_CD END AS TS_DTL ";
            sqlstr += "            , CASE WHEN CNTR_TYPE = 'F' THEN 'FCL' WHEN CNTR_TYPE = 'L' THEN 'LCL' WHEN A.CNTR_TYPE = 'C' THEN 'CONSOL' END AS CNTR_TYPE  ";
            sqlstr += "            , TO_CHAR (TO_DATE (DOC_CLOSE_YMD), 'YYYYMMDD') || TO_CHAR (TO_DATE (LPAD(DOC_CLOSE_HM,'4','0'), 'hh24miss'), 'hh24mi') AS DOC_CLOSE  ";
            sqlstr += "                    FROM PRM_SCH_MST A ";
            sqlstr += "                         INNER JOIN MDM_CARR_MST CARR ";
            sqlstr += "                            ON A.LINE_CD = CARR.CARR_CD ";
            sqlstr += "                         LEFT OUTER JOIN MDM_LINE_IMG B ";
            sqlstr += "                            ON CARR.SCAC_CD = B.LINE_CD ";
            sqlstr += "                               OR CARR.EDI_CD = B.LINE_CD ";
            sqlstr += "                             INNER JOIN PRM_BKG_MST BKG ";
            sqlstr += "                         ON A.SCH_NO = BKG.SCH_NO ";
            sqlstr += "   WHERE 1 = 1 ";
            sqlstr += " AND BKG.BKG_NO = '" + dr["BKG_NO"] + "' ";            

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

        /// <summary>
        /// 부킹 데이터 Insert - 수출
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="bkg_no"></param>
        /// <returns></returns>
        public string fnGetBKInsertData_E_Query(DataRow dr, string bkg_no)
        {
            string sqlstr = "";
            sqlstr = "INSERT INTO PRM_BKG_MST";

            sqlstr += " (BKG_NO ";
            sqlstr += " , SCH_NO  ";
            sqlstr += " , QUOT_NO ";
            sqlstr += " , REQ_SVC  ";
            sqlstr += " , CUST_PIC_NM  ";
            sqlstr += " , CUST_TEL_NO  ";
            sqlstr += " , CUST_EMAIL  ";
            sqlstr += " , CUST_CD ";
            sqlstr += " , CUST_NM  ";
            sqlstr += " , SHP_CD ";
            sqlstr += " , SHP_ADDR ";
            sqlstr += " , STATUS  ";
            sqlstr += " , LOAD_TYPE  ";            
            sqlstr += " , MAIN_ITEM_NM  ";
            sqlstr += " , PRC  ";
            sqlstr += " , CURR_CD  ";
            if(dr["COL1"].ToString() == "ETC")
            {
                sqlstr += " , CUSTOM_COL1 ";
                sqlstr += " , CUSTOM_COL1_ETC ";
            }
            else
            {
                sqlstr += " , CUSTOM_COL1 ";
            }
            sqlstr += " , CUSTOM_COL2 ";
            sqlstr += " , CUSTOM_COL3 ";
            sqlstr += " , CUSTOM_COL4 ";
            sqlstr += " , CUSTOM_COL5 ";
            sqlstr += " , CUSTOM_COL6 ";
            sqlstr += " , CUSTOM_COL7 ";
            sqlstr += " , CUSTOM_COL8 ";
            sqlstr += " , CUSTOM_COL18 ";
            sqlstr += " , INS_USR ";
            sqlstr += " , INS_YMD ";
            sqlstr += " , INS_HM ";
            sqlstr += " , UPD_USR ";
            sqlstr += " , UPD_YMD ";
            sqlstr += " , UPD_HM) ";
            sqlstr += " Values";
            sqlstr += "   ('" + bkg_no + "'";
            sqlstr += "  ,'" + dr["SCH_NO"] + "'";
            sqlstr += "  ,'" + bkg_no + "'";
            sqlstr += "   ,'" + dr["REQ_SVC"] + "' ";
            sqlstr += "  ,'" + dr["LOC_NM"] + "'";
            sqlstr += "  ,'" + dr["HP_NO"] + "'";
            sqlstr += "   ,'" + dr["EMAIL"] + "'";
            sqlstr += "   ,'" + dr["CUST_CD"] + "'";
            sqlstr += "   ,'" + dr["CUST_NM"] + "'";
            sqlstr += "   ,'" + dr["CUST_CD"] + "'";            
            sqlstr += "   , (SELECT CUST_NM || CHR (13) || CHR(10) || NVL(BL_ADDR,'') FROM MDM_CUST_MST WHERE CUST_CD = '" + dr["CUST_CD"] + "')";
            sqlstr += "   ,'" + dr["STATUS"] + "'";
            sqlstr += "   ,'" + dr["LOAD_TYPE"] + "'";            
            sqlstr += "   ,'" + dr["ITEM"] + "'";
            sqlstr += "   ,'" + dr["PRC"] + "'";
            sqlstr += "   ,'" + dr["CURR_CD"] + "'";
            if (dr["COL1"].ToString() == "ETC")
            {
                sqlstr += "   ,'" + dr["COL1"] + "'";
                sqlstr += "   ,'" + dr["COL1_ETC"] + "'";
            }
            else
            {
                sqlstr += "   ,'" + dr["COL1"] + "'";
            }
            sqlstr += "   ,'" + dr["COL2"] + "'";
            sqlstr += "   ,'" + dr["COL3"] + "'";
            sqlstr += "   ,'" + dr["COL4"] + "'";
            sqlstr += "   ,'" + dr["COL5"] + "'";
            sqlstr += "   ,'" + dr["COL6"] + "'";
            sqlstr += "   ,'" + dr["COL7"] + "'";            
            sqlstr += "    ,REPLACE('" + dr["COL8"] + "',CHR(10),CHR(13) || CHR(10))";
            sqlstr += "   ,'" + dr["COL18"] + "'";
            sqlstr += "  ,'" + dr["LOC_NM"] + "'";
            sqlstr += "   , UFN_DATE_FORMAT('DATE') ";
            sqlstr += "   , UFN_DATE_FORMAT('TIME') ";
            sqlstr += "   ,'" + dr["LOC_NM"] + "'";
            sqlstr += "   , UFN_DATE_FORMAT('DATE') ";
            sqlstr += "   , UFN_DATE_FORMAT('TIME') ";
            sqlstr += ")";

            return sqlstr;
        }

        /// <summary>
        /// 부킹 데이터 Insert - 수입
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="bkg_no"></param>
        /// <returns></returns>
        public string fnGetBKInsertData_I_Query(DataRow dr, string bkg_no)
        {
            string sqlstr = "";
            sqlstr = "INSERT INTO PRM_BKG_MST";
            sqlstr += " (BKG_NO ";
            sqlstr += " , SCH_NO  ";
            sqlstr += " , QUOT_NO ";
            sqlstr += " , REQ_SVC  ";
            sqlstr += " , CUST_PIC_NM  ";
            sqlstr += " , CUST_TEL_NO  ";
            sqlstr += " , CUST_EMAIL  ";
            sqlstr += " , CUST_CD ";
            sqlstr += " , CUST_NM  ";
            sqlstr += " , SHP_CD ";
            sqlstr += " , SHP_ADDR ";
            sqlstr += " , STATUS  ";
            sqlstr += " , LOAD_TYPE  ";
            sqlstr += " , MAIN_ITEM_NM  ";
            sqlstr += " , PRC  ";
            sqlstr += " , CURR_CD  ";
            sqlstr += " , CUSTOM_COL9 ";
            sqlstr += " , CUSTOM_COL10 ";
            sqlstr += " , CUSTOM_COL11 ";
            sqlstr += " , CUSTOM_COL12 ";
            sqlstr += " , CUSTOM_COL13 ";
            sqlstr += " , CUSTOM_COL14 ";
            sqlstr += " , CUSTOM_COL15 ";
            sqlstr += " , CUSTOM_COL16 ";
            sqlstr += " , CUSTOM_COL17 ";
            sqlstr += " , INS_USR ";
            sqlstr += " , INS_YMD ";
            sqlstr += " , INS_HM ";
            sqlstr += " , UPD_USR ";
            sqlstr += " , UPD_YMD ";
            sqlstr += " , UPD_HM) ";
            sqlstr += " Values";
            sqlstr += "   ('" + bkg_no + "'";
            sqlstr += "  ,'" + dr["SCH_NO"] + "'";
            sqlstr += "  ,'" + bkg_no + "'";
            sqlstr += "   ,'" + dr["REQ_SVC"] + "' ";
            sqlstr += "  ,'" + dr["LOC_NM"] + "'";
            sqlstr += "  ,'" + dr["HP_NO"] + "'";
            sqlstr += "   ,'" + dr["EMAIL"] + "'";
            sqlstr += "   ,'" + dr["CUST_CD"] + "'";
            sqlstr += "   ,'" + dr["CUST_NM"] + "'";
            sqlstr += "   ,'" + dr["CUST_CD"] + "'";
            sqlstr += "   , (SELECT CUST_NM || CHR (13) || CHR(10) || NVL(BL_ADDR,'') FROM MDM_CUST_MST WHERE CUST_CD = '" + dr["CUST_CD"] + "')";
            sqlstr += "   ,'" + dr["STATUS"] + "'";
            sqlstr += "   ,'" + dr["LOAD_TYPE"] + "'";
            sqlstr += "   ,'" + dr["ITEM"] + "'";
            sqlstr += "   ,'" + dr["PRC"] + "'";
            sqlstr += "   ,'" + dr["CURR_CD"] + "'";
            sqlstr += "   ,'" + dr["COL9"] + "'";
            sqlstr += "   ,'" + dr["COL10"] + "'";
            sqlstr += "   ,'" + dr["COL11"] + "'";
            sqlstr += "   ,'" + dr["COL12"] + "'";
            sqlstr += "   ,'" + dr["COL13"] + "'";
            sqlstr += "   ,'" + dr["COL14"] + "'";
            sqlstr += "   ,'" + dr["COL15"] + "'";
            sqlstr += "   ,'" + dr["COL16"] + "'";
            sqlstr += "   ,REPLACE('" + dr["COL17"] + "',CHR(10),CHR(13) || CHR(10))";
            sqlstr += "  ,'" + dr["LOC_NM"] + "'";
            sqlstr += "   , UFN_DATE_FORMAT('DATE') ";
            sqlstr += "   , UFN_DATE_FORMAT('TIME') ";
            sqlstr += "   ,'" + dr["LOC_NM"] + "'";
            sqlstr += "   , UFN_DATE_FORMAT('DATE') ";
            sqlstr += "   , UFN_DATE_FORMAT('TIME') ";
            sqlstr += ")";
            return sqlstr;
        }

        /// <summary>
        /// Booking 패키지 저장
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="bkg_no"></param>
        /// <returns></returns>
        public string fnGetBKInserPKG_Query(DataRow dr, string bkg_no)
        {
            string sqlstr = "";
            sqlstr = "INSERT INTO PRM_BKG_PKG";
            sqlstr += "   (BKG_NO, SEQ , LOAD_TYPE , CNTR_TYPE  , PKG , GRS_WGT , MSRMT , INS_USR , INS_YMD , INS_HM , UPD_USR , UPD_YMD , UPD_HM) ";
            sqlstr += " Values";
            sqlstr += "   ('" + bkg_no + "'";
            sqlstr += " , (SELECT NVL(MAX(SEQ),0)+1 AS SEQ	   ";
            sqlstr += "  FROM PRM_BKG_DIM	";
            sqlstr += "  WHERE BKG_NO = '" + bkg_no + "') ";
            sqlstr += "  ,'" + dr["LOAD_TYPE"] + "'";
            sqlstr += "  ,'" + dr["CNTR_TYPE"] + "'";
            sqlstr += "  ,'" + dr["PKG"] + "'";
            sqlstr += "  ,'" + dr["GRS_WGT"] + "'";
            sqlstr += "  ,'" + dr["VOL_WGT"] + "'";
            sqlstr += "  ,'" + dr["LOC_NM"] + "'";
            sqlstr += "   , UFN_DATE_FORMAT('DATE') ";
            sqlstr += "   , UFN_DATE_FORMAT('TIME') ";
            sqlstr += "   ,'" + dr["LOC_NM"] + "'";
            sqlstr += "   , UFN_DATE_FORMAT('DATE') ";
            sqlstr += "   , UFN_DATE_FORMAT('TIME'))";
            return sqlstr;
        }

        /// <summary>
        /// Booking 운임 데이터 저장
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="bkg_no"></param>
        /// <returns></returns>
        public string fnPRM_BKG_DIM_Query(DataRow dr, string bkg_no)
        {
            sqlstr = "";

            sqlstr += " INSERT INTO PRM_BKG_DIM (BKG_NO, ";
            sqlstr += "                          SEQ, ";
            sqlstr += "                          INS_USR, ";
            sqlstr += "                          INS_YMD, ";
            sqlstr += "                          INS_HM, ";
            sqlstr += "                          UPD_USR, ";
            sqlstr += "                          UPD_YMD, ";
            sqlstr += "                          UPD_HM)  ";
            sqlstr += "      VALUES ('" + bkg_no + "', ";
            sqlstr += "              (SELECT NVL (MAX (SEQ), 0) + 1 AS SEQ ";
            sqlstr += "                 FROM PRM_BKG_DIM ";
            sqlstr += "                WHERE BKG_NO = '" + bkg_no + "'), ";
            sqlstr += "              '" + dr["LOC_NM"] + "', ";
            sqlstr += "              UFN_DATE_FORMAT ('DATE'), ";
            sqlstr += "              UFN_DATE_FORMAT ('TIME'), ";
            sqlstr += "              '" + dr["LOC_NM"] + "', ";
            sqlstr += "              UFN_DATE_FORMAT ('DATE'), ";
            sqlstr += "              UFN_DATE_FORMAT ('TIME'))  ";

            return sqlstr;
        }

        /// <summary>
        /// Booking 견적 데이터 저장
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="bkg_no"></param>
        /// <returns></returns>
        public string fnPRM_QUOT_MST_Query(DataRow dr, string bkg_no)
        {
            sqlstr = "";

            sqlstr = " INSERT INTO PRM_QUOT_MST (QUOT_NO , STATUS, LOAD_TYPE , CUST_CD , CUST_NM , POL_CD , POD_CD , ETD , ETA , VSL , TRANSIT_TIME , MAIN_ITEM";
            sqlstr += "                                                ,RMK , REQ_SVC , SCH_NO , PRC , INS_USR , INS_YMD , INS_HM , UPD_USR , UPD_YMD , UPD_HM)         ";
            sqlstr += "     SELECT BKG_NO";
            sqlstr += "              ,  STATUS";
            sqlstr += "              ,  LOAD_TYPE";
            sqlstr += "              ,  CUST_CD";
            sqlstr += "              ,  CUST_NM";
            sqlstr += "              ,  POL_CD";
            sqlstr += "              ,  POD_CD";
            sqlstr += "              ,  ETD";
            sqlstr += "              ,  ETA";
            sqlstr += "              ,  VSL";
            sqlstr += "              ,  TRANSIT_TIME";
            sqlstr += "              ,  MAIN_ITEM_NM";
            sqlstr += "              ,  B.RMK";
            sqlstr += "              ,  B.REQ_SVC";
            sqlstr += "              ,  B.SCH_NO";
            sqlstr += "              ,  PRC";
            sqlstr += "	             , A.INS_USR	";
            sqlstr += "	             , UFN_DATE_FORMAT('DATE') 	";
            sqlstr += "	             , UFN_DATE_FORMAT('TIME') 	";
            sqlstr += "	             , A.INS_USR	";
            sqlstr += "	             , UFN_DATE_FORMAT('DATE') 	";
            sqlstr += "	             , UFN_DATE_FORMAT('TIME') 	";
            sqlstr += "       FROM PRM_BKG_MST A";
            sqlstr += "                INNER JOIN PRM_SCH_MST B ON A.SCH_NO = B.SCH_NO";
            sqlstr += "                WHERE A.BKG_NO = '" + bkg_no + "'";

            return sqlstr;
        }

        //PRM_QUOT_DTL QUOT_NO SEQ
        public string fnPRM_QUOT_DTL_Query(DataRow dr, string bkg_no)
        {
            sqlstr = "";

            sqlstr += " INSERT INTO PRM_QUOT_DTL (          ";
            sqlstr += " QUOT_NO ,";
            sqlstr += " SEQ ,";
            sqlstr += " CUST_CD ,";
            sqlstr += " LOAD_TYPE ,";
            sqlstr += " FARE_CD ,";
            sqlstr += " FARE_NM ,";
            sqlstr += " PKG_UNIT  ,";
            sqlstr += " CURR_CD  ,";
            sqlstr += " PKG  ,";
            sqlstr += " UNIT_PRC ,";
            sqlstr += " FARE_AMT ,";
            sqlstr += " FARE_LOC_AMT ,";
            sqlstr += " FARE_VAT_AMT ,";
            sqlstr += " LOC_FRGN_TYPE ,";
            sqlstr += " TOT_LOC_AMT ,";
            sqlstr += " EXRT ,";
            sqlstr += " INS_USR ,";
            sqlstr += " INS_YMD ,";
            sqlstr += " INS_HM ,";
            sqlstr += " UPD_USR ,";
            sqlstr += " UPD_YMD ,";
            sqlstr += " UPD_HM         ";
            sqlstr += " )         ";

            sqlstr += "     SELECT A.BKG_NO";                               //QUOT_NO
            sqlstr += " , (SELECT NVL(MAX(SEQ),0)+1 AS SEQ	   ";           //SEQ
            sqlstr += "  FROM PRM_QUOT_DTL	";
            sqlstr += "  WHERE QUOT_NO = '" + bkg_no + "') ";
            sqlstr += "              ,  A.CUST_CD ";                        //CUST_CD
            sqlstr += "              ,  A.LOAD_TYPE";                       //LOAD_TYPE
            sqlstr += "              ,  '" + _SeaTariffCode + "'";      //FARE_CD
            sqlstr += "              , (SELECT FARE_NM FROM MDM_FRT_MST WHERE FARE_CD =  '" + _SeaTariffCode + "')";    //FARE_NM     
            sqlstr += "              ,  '" + dr["PKG_UNIT"].ToString() + "'";
            sqlstr += "              ,  A.CURR_CD";                         //CURR_CD
            sqlstr += "              ,  B.PKG";                             //PKG

            if (dr["CURR_CD"].ToString() == "KRW")
            {
                sqlstr += "              ,  '" + dr["UNIT_PRC"].ToString() + "'"; //단가
                sqlstr += "              ,  '0'";   //외화금액
                sqlstr += "              ,  A.PRC";   //원화금액
                sqlstr += "              ,  '0'";   //세액
                sqlstr += "              ,  'L'";   //L
                sqlstr += "              ,  A.PRC";   //총금액
                sqlstr += "              ,  '0'";   //환율
            }
            else
            {
                sqlstr += "              ,  '" + dr["UNIT_PRC"].ToString() + "'"; //단가                
                sqlstr += "              ,  A.PRC";   //외화금액

                //단가 * 외화금액 * 환율
                sqlstr += " , (SELECT CASE ";
                sqlstr += "           WHEN NVL (MAX (WIRE_SEND_EXRT), 0) <> 0 THEN NVL (MAX (WIRE_SEND_EXRT), 0) * '" + dr["UNIT_PRC"].ToString() + "' * B.PKG ";
                sqlstr += "           WHEN NVL (MAX (WIRE_SEND_EXRT), 0) = 0 THEN 0 ";
                sqlstr += "        END ";
                sqlstr += "           AS WIRE_SEND_EXRT ";
                sqlstr += "   FROM MDM_EX_RATE ";
                sqlstr += "  WHERE EXRT_YMD = UFN_DATE_FORMAT('DATE') AND CURR_CD = '" + dr["CURR_CD"].ToString() + "') ";  //원화금액 

                sqlstr += "              ,  '0'";   //세액
                sqlstr += "              ,  'F'";   //L

                //단가 * 외화금액 * 환율 = 총금액
                sqlstr += " , (SELECT CASE ";
                sqlstr += "           WHEN NVL (MAX (WIRE_SEND_EXRT), 0) <> 0 THEN NVL (MAX (WIRE_SEND_EXRT), 0) * '" + dr["UNIT_PRC"].ToString() + "' * B.PKG ";
                sqlstr += "           WHEN NVL (MAX (WIRE_SEND_EXRT), 0) = 0 THEN 0 ";
                sqlstr += "        END ";
                sqlstr += "           AS WIRE_SEND_EXRT ";
                sqlstr += "   FROM MDM_EX_RATE ";
                sqlstr += "  WHERE EXRT_YMD = UFN_DATE_FORMAT('DATE') AND CURR_CD = '" + dr["CURR_CD"].ToString() + "') ";  //총금액 
                sqlstr += "              ,  (SELECT NVL(MAX(WIRE_SEND_EXRT),0) AS WIRE_SEND_EXRT FROM MDM_EX_RATE WHERE EXRT_YMD = UFN_DATE_FORMAT('DATE') AND CURR_CD = '" + dr["CURR_CD"].ToString() + "')";   //환율
            }

            sqlstr += "	             , A.INS_USR	";
            sqlstr += "	             , UFN_DATE_FORMAT('DATE') 	";
            sqlstr += "	             , UFN_DATE_FORMAT('TIME') 	";
            sqlstr += "	             , A.INS_USR	";
            sqlstr += "	             , UFN_DATE_FORMAT('DATE') 	";
            sqlstr += "	             , UFN_DATE_FORMAT('TIME') 	";
            sqlstr += "       FROM PRM_BKG_MST A";
            sqlstr += "                INNER JOIN PRM_BKG_PKG B ON A.BKG_NO = B.BKG_NO";
            sqlstr += "                WHERE A.BKG_NO = '" + bkg_no + "'";

            return sqlstr;
        }

        /// <summary>
        /// Booking 견적 패키지 데이터 저장
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="bkg_no"></param>
        /// <returns></returns>
        public string fnPRM_QUOT_PKG_Query(DataRow dr, string bkg_no)
        {
            sqlstr = "";

            sqlstr += " INSERT INTO PRM_QUOT_PKG (QUOT_NO, ";
            sqlstr += "                          SEQ, ";
            sqlstr += "                          CNTR_TYPE, ";
            sqlstr += "                          PKG, ";
            sqlstr += "                          GRS_WGT, ";
            sqlstr += "                          MSRMT, ";
            sqlstr += "                          INS_USR, ";
            sqlstr += "                          INS_YMD, ";
            sqlstr += "                          INS_HM, ";
            sqlstr += "                          UPD_USR, ";
            sqlstr += "                          UPD_YMD, ";
            sqlstr += "                          UPD_HM)  ";
            sqlstr += "      VALUES ('" + bkg_no + "', ";
            sqlstr += "              (SELECT NVL (MAX (SEQ), 0) + 1 AS SEQ ";
            sqlstr += "                 FROM PRM_QUOT_PKG ";
            sqlstr += "                WHERE QUOT_NO = '" + bkg_no + "'), ";
            sqlstr += "              '" + dr["CNTR_TYPE"] + "', ";
            sqlstr += "              '" + dr["PKG"] + "', ";
            sqlstr += "              '" + dr["GRS_WGT"] + "', ";
            sqlstr += "              '" + dr["VOL_WGT"] + "', ";
            sqlstr += "              '" + dr["LOC_NM"] + "', ";
            sqlstr += "              UFN_DATE_FORMAT ('DATE'), ";
            sqlstr += "              UFN_DATE_FORMAT ('TIME'), ";
            sqlstr += "              '" + dr["LOC_NM"] + "', ";
            sqlstr += "              UFN_DATE_FORMAT ('DATE'), ";
            sqlstr += "              UFN_DATE_FORMAT ('TIME'))  ";

            return sqlstr;
        }

        /// <summary>
        /// Update 부킹 데이터
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="bkg_no"></param>
        /// <returns></returns>
        public string fnGetBKUpdateData_E_Query(DataRow dr, string bkg_no)
        {
            string sqlstr = "";
            sqlstr = "UPDATE PRM_BKG_MST";
            sqlstr += "   SET CUST_PIC_NM = '" + dr["LOC_NM"] + "'";
            sqlstr += "     , CUST_EMAIL = '" + dr["EMAIL"] + "'";
            sqlstr += "     , MAIN_ITEM_NM = '" + dr["ITEM"] + "'";
            if(dr["COL1"].ToString() == "ETC")
            {
                sqlstr += "     , CUSTOM_COL1 = '" + dr["COL1"] + "'";
                sqlstr += "     , CUSTOM_COL1_ETC = '" + dr["COL1_ETC"] + "'";
            }
            else
            {
                sqlstr += "     , CUSTOM_COL1 = '" + dr["COL1"] + "'";
            }
            sqlstr += "     , CUSTOM_COL2 = '" + dr["COL2"] + "'";
            sqlstr += "     , CUSTOM_COL3 = '" + dr["COL3"] + "'";
            sqlstr += "     , CUSTOM_COL4 = '" + dr["COL4"] + "'";
            sqlstr += "     , CUSTOM_COL5 = '" + dr["COL5"] + "'";
            sqlstr += "     , CUSTOM_COL6 = '" + dr["COL6"] + "'";
            sqlstr += "     , CUSTOM_COL7 = '" + dr["COL7"] + "'";
            sqlstr += "     , CUSTOM_COL8 = REPLACE('" + dr["COL8"] + "',CHR(10),CHR(13) || CHR(10))";
            sqlstr += "     , CUSTOM_COL18 = '" + dr["COL18"] + "'";
            sqlstr += "     , UPD_USR = '" + dr["LOC_NM"] + "'";
            sqlstr += "     , UPD_YMD = UFN_DATE_FORMAT('DATE') ";
            sqlstr += "     , UPD_HM = UFN_DATE_FORMAT('TIME') ";
            sqlstr += " WHERE BKG_NO = '" + bkg_no + "'";

            return sqlstr;
        }

        /// <summary>
        /// Update 부킹 데이터
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="bkg_no"></param>
        /// <returns></returns>
        public string fnGetBKUpdateData_I_Query(DataRow dr, string bkg_no)
        {
            string sqlstr = "";
            sqlstr = "UPDATE PRM_BKG_MST";
            sqlstr += "   SET CUST_PIC_NM = '" + dr["LOC_NM"] + "'";            
            sqlstr += "     , CUST_EMAIL = '" + dr["EMAIL"] + "'";            
            sqlstr += "     , MAIN_ITEM_NM = '" + dr["ITEM"] + "'";
            sqlstr += "     , CUSTOM_COL9 = '" + dr["COL9"] + "'";
            sqlstr += "     , CUSTOM_COL10 = '" + dr["COL10"] + "'";
            sqlstr += "     , CUSTOM_COL11 = '" + dr["COL11"] + "'";
            sqlstr += "     , CUSTOM_COL12 = '" + dr["COL12"] + "'";
            sqlstr += "     , CUSTOM_COL13 = '" + dr["COL13"] + "'";
            sqlstr += "     , CUSTOM_COL14 = '" + dr["COL14"] + "'";
            sqlstr += "     , CUSTOM_COL15 = '" + dr["COL15"] + "'";
            sqlstr += "     , CUSTOM_COL16 = '" + dr["COL16"] + "'";
            sqlstr += "     , CUSTOM_COL17 = REPLACE('" + dr["COL17"] + "',CHR(10),CHR(13) || CHR(10))";
            sqlstr += "     , UPD_USR = '" + dr["LOC_NM"] + "'";
            sqlstr += "     , UPD_YMD = UFN_DATE_FORMAT('DATE') ";
            sqlstr += "     , UPD_HM = UFN_DATE_FORMAT('TIME') ";
            sqlstr += " WHERE BKG_NO = '" + bkg_no + "'";

            return sqlstr;
        }

        /// <summary>
        /// Booking 패키지 데이터 수정
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="bkg_no"></param>
        /// <returns></returns>
        public string fnGetBKUpdatePKG_Query(DataRow dr, string bkg_no)
        {
            string sqlstr = "";

            sqlstr = "UPDATE PRM_BKG_PKG";
            sqlstr += "   SET PKG = '" + dr["PKG"] + "'";
            sqlstr += "     , GRS_WGT = '" + dr["GRS_WGT"] + "'";
            sqlstr += "     , MSRMT = '" + dr["VOL_WGT"] + "'";
            sqlstr += "     , UPD_USR = '" + dr["LOC_NM"] + "'";
            sqlstr += "     , UPD_YMD = UFN_DATE_FORMAT('DATE') ";
            sqlstr += "     , UPD_HM = UFN_DATE_FORMAT('TIME') ";
            sqlstr += " WHERE BKG_NO = '" + bkg_no + "'";

            return sqlstr;
        }

        /// <summary>
        /// Booking 견적 패키지 데이터 저장
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="bkg_no"></param>
        /// <returns></returns>
        public string fnGetBKUpdateQuotPKG_Query(DataRow dr, string bkg_no)
        {
            sqlstr = "";

            sqlstr += " UPDATE PRM_QUOT_PKG SET ";            
            sqlstr += "      PKG = '" + dr["PKG"] + "' ";
            sqlstr += "     ,GRS_WGT = '" + dr["GRS_WGT"] + "' ";
            sqlstr += "     ,MSRMT = '" + dr["VOL_WGT"] + "' ";
            sqlstr += "     ,UPD_USR = '" + dr["LOC_NM"] + "' ";
            sqlstr += "     ,UPD_YMD = UFN_DATE_FORMAT ('DATE')";
            sqlstr += "     ,UPD_HM = UFN_DATE_FORMAT ('TIME')  ";
            sqlstr += " WHERE QUOT_NO = '" + bkg_no + "'";

            return sqlstr;
        }

        /// <summary>
        /// BK 데이터 가져오기
        /// </summary>
        /// <param name="bkg_no"></param>
        /// <returns></returns>
        public string fnGetBKSearchMain_Query(string bkg_no)
        {
            string sqlstr = "";
            sqlstr += " SELECT  ";
            sqlstr += "        A.SCH_NO,  ";
            sqlstr += "        A.BKG_NO, ";
            sqlstr += "        A.BKG_NO AS REG_BKG_NO, ";
            sqlstr += "        A.REQ_SVC, ";
            sqlstr += "        B.ETD, ";
            sqlstr += "        B.ETA, ";
            sqlstr += "        B.POL_CD, ";
            sqlstr += "        (SELECT LOC_NM ";
            sqlstr += "           FROM MDM_PORT_MST ";
            sqlstr += "          WHERE LOC_CD = B.POL_CD) ";
            sqlstr += "           AS POL_NM, ";
            sqlstr += "        B.POD_CD, ";
            sqlstr += "        (SELECT LOC_NM ";
            sqlstr += "           FROM MDM_PORT_MST ";
            sqlstr += "          WHERE LOC_CD = B.POD_CD) ";
            sqlstr += "           AS POD_NM ";
            sqlstr += "   FROM PRM_BKG_MST A LEFT OUTER JOIN PRM_SCH_MST B ON A.SCH_NO = B.SCH_NO ";
            sqlstr += "  WHERE BKG_NO = '" + bkg_no + "' ";
            return sqlstr;
        }

        /// <summary>
        /// Booking 데이터 가져오기
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string fnGetBookingData_Query(DataRow dr)
        {
            string sqlstr = "";
            sqlstr += " SELECT  ";
            sqlstr += "        A.BKG_NO, ";
            sqlstr += "        A.STATUS, ";
            sqlstr += "        CASE WHEN A.STATUS = 'F' THEN '확정' WHEN A.STATUS = 'C' THEN '취소' WHEN A.STATUS = 'D' THEN '삭제' WHEN A.STATUS = 'O' THEN '거절' ELSE ' 요청' END AS STATUS_NM, ";
            sqlstr += "        CASE WHEN A.STATUS = 'F' THEN 'Confirm' WHEN A.STATUS = 'C' THEN 'Cancel' WHEN A.STATUS = 'D' THEN 'Delete' WHEN A.STATUS = 'O' THEN 'Rejection' ELSE ' Request' END AS STATUS_NM_EN, ";
            sqlstr += "        A.RMK, ";
            sqlstr += "        A.MAIN_ITEM_NM, ";
            sqlstr += "        A.CUST_PIC_NM, ";
            sqlstr += "        A.CUST_TEL_NO, ";
            sqlstr += "        A.INS_USR, ";
            sqlstr += "        A.REQ_SVC, ";
            sqlstr += "        A.CUST_CD, ";
            sqlstr += "        A.CUST_EMAIL, ";
            sqlstr += "        A.LOAD_TYPE, ";
            sqlstr += "        B.PKG, ";
            sqlstr += "        B.GRS_WGT, ";
            sqlstr += "        B.MSRMT, ";
            sqlstr += "        A.HBL_NO, ";
            sqlstr += "        A.CUSTOM_COL1, ";
            sqlstr += "        A.CUSTOM_COL1_ETC, ";
            sqlstr += "        A.CUSTOM_COL2, ";
            sqlstr += "        A.CUSTOM_COL3, ";
            sqlstr += "        A.CUSTOM_COL4, ";
            sqlstr += "        A.CUSTOM_COL5, ";
            sqlstr += "        A.CUSTOM_COL6, ";
            sqlstr += "        A.CUSTOM_COL7, ";
            sqlstr += "        A.CUSTOM_COL8, ";
            sqlstr += "        A.CUSTOM_COL9, ";
            sqlstr += "        A.CUSTOM_COL10, ";
            sqlstr += "        A.CUSTOM_COL11, ";
            sqlstr += "        A.CUSTOM_COL12, ";
            sqlstr += "        A.CUSTOM_COL13, ";
            sqlstr += "        A.CUSTOM_COL14, ";
            sqlstr += "        A.CUSTOM_COL15, ";
            sqlstr += "        A.CUSTOM_COL16, ";
            sqlstr += "        A.CUSTOM_COL17, ";
            sqlstr += "        A.CUSTOM_COL18, ";
            sqlstr += "        A.CUSTOM_COL19, ";
            sqlstr += "        A.CUSTOM_COL20, ";
            sqlstr += "        A.CFS_BKG_NO,";
            sqlstr += "        A.CFS_BKG_CHK";
            //sqlstr += "        CASE";
            //sqlstr += "        WHEN A.CFS_BKG_NO IS NULL THEN '요청'";
            //sqlstr += "        ELSE '승인'";
            //sqlstr += "        END ";
            //sqlstr += "        AS CFS_BKG_STATUS";
            sqlstr += "   FROM PRM_BKG_MST A LEFT OUTER JOIN PRM_BKG_PKG B ON A.BKG_NO = B.BKG_NO ";
            sqlstr += "  WHERE A.BKG_NO = '" + dr["BKG_NO"] + "' ";
            return sqlstr;             
        }

        /// <summary>
        /// 부킹 문서 데이터 가지고 오기
        /// </summary>
        /// <param name="bkg_no"></param>
        /// <returns></returns>
        public string fnGetDocData_Query(DataRow dr)
        {
            string sqlstr = "";
            sqlstr += "  SELECT * FROM COM_DOC_MST  ";
            sqlstr += "        WHERE MNGT_NO = '" + dr["BKG_NO"] + "' ";
            return sqlstr;
        }

        /// <summary>
        /// 부킹 상태 취소로 변경
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string fnSetCancelStatus_Query(DataRow dr)
        {
            sqlstr = "";

            sqlstr += " UPDATE PRM_BKG_MST SET STATUS = 'C' ";
            sqlstr += " WHERE BKG_NO = '" + dr["BKG_NO"].ToString() + "' ";

            return sqlstr;
        }
    }
}