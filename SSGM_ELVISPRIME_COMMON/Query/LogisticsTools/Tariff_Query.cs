using System.Data;

namespace SSGM_ELVISPRIME_COMMON.Query.LogisticsTools
{
    public class Tariff_Query
    {
        string sqlstr;

        /// <summary>
        /// 분기 / 년 데이터 가져오기
        /// </summary>
        /// <returns></returns>
        public string SetYearQuarter_Query()
        {
            sqlstr = "";

            sqlstr += " SELECT A.PERIOD_YEAR, ";
            sqlstr += "        A.PERIOD_QUARTER, ";
            sqlstr += "        A.PERIOD, ";
            sqlstr += "        (SELECT OPT_ITEM2 ";
            sqlstr += "           FROM MDM_COM_CODE ";
            sqlstr += "          WHERE GRP_CD = 'P05' AND OPT_ITEM1 = A.PERIOD) ";
            sqlstr += "           AS PERIOD_NAME ";
            sqlstr += "   FROM (  SELECT PERIOD_YEAR, ";
            sqlstr += "                  PERIOD_QUARTER, ";
            sqlstr += "                  PERIOD_YEAR || PERIOD_QUARTER AS PERIOD ";
            sqlstr += "             FROM PRM_SAFE_FRE ";
            sqlstr += "         GROUP BY PERIOD_YEAR, PERIOD_QUARTER ";
            sqlstr += "         ORDER BY PERIOD_YEAR DESC, PERIOD_QUARTER DESC) A ";

            return sqlstr;
        }

        /// <summary>
        /// 주소 (시,도) 가져오기
        /// </summary>
        /// <returns></returns>
        public string SetAddrState_Query()
        {
            sqlstr = "";

            sqlstr += " SELECT DISTINCT OPT_ITEM1 ";
            sqlstr += "     FROM MDM_COM_CODE ";
            sqlstr += "    WHERE GRP_CD = 'P01' ";
            sqlstr += " ORDER BY OPT_ITEM1 ASC ";

            return sqlstr;
        }

        /// <summary>
        /// 주소 전체(시.군.구)
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string SetAddrCity_Query(DataRow dr)
        {
            sqlstr = "";

            sqlstr += " SELECT DISTINCT OPT_ITEM2 ";
            sqlstr += "     FROM MDM_COM_CODE ";
            sqlstr += "    WHERE GRP_CD = 'P01' ";
            sqlstr += "    AND OPT_ITEM1 = '"+dr["OPT_ITEM1"].ToString()+"' ";
            sqlstr += " ORDER BY OPT_ITEM2 ASC ";

            return sqlstr;
        }

        /// <summary>
        /// 전체(읍,면,동 - 행정동) 데이터 가져오기
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string SetAddrTownship_Query(DataRow dr)
        {
            sqlstr = "";

            sqlstr += " SELECT DISTINCT OPT_ITEM3 ";
            sqlstr += "     FROM MDM_COM_CODE ";
            sqlstr += "    WHERE GRP_CD = 'P01' ";
            sqlstr += "    AND OPT_ITEM1 = '" + dr["OPT_ITEM1"].ToString() + "' ";
            sqlstr += "    AND OPT_ITEM2 = '" + dr["OPT_ITEM2"].ToString() + "' ";
            sqlstr += " ORDER BY OPT_ITEM3 ASC ";

            return sqlstr;
        }

        /// <summary>
        /// 전체(읍,면,동 - 법정동) 데이터 가져오기
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string SetAddrTownship2_Query(DataRow dr)
        {
            sqlstr = "";

            sqlstr += " SELECT DISTINCT OPT_ITEM3 , OPT_ITEM4 ";
            sqlstr += "     FROM MDM_COM_CODE ";
            sqlstr += "    WHERE GRP_CD = 'P01' ";
            sqlstr += "    AND OPT_ITEM1 = '" + dr["OPT_ITEM1"].ToString() + "' ";
            sqlstr += "    AND OPT_ITEM2 = '" + dr["OPT_ITEM2"].ToString() + "' ";
            sqlstr += " ORDER BY OPT_ITEM3 ASC ";

            return sqlstr;
        }

        /// <summary>
        /// 항구 기점 데이터 가져오기
        /// </summary>
        /// <returns></returns>
        public string fnSetSection_Query(DataRow dr)
        {
            sqlstr = "";

            sqlstr += "   SELECT A.PORT, ";
            sqlstr += "          A.SECTION, ";
            sqlstr += "          TRIM (B.OPT_ITEM2) || '기점(' || A.SECTION || ')' AS PORT_NM ";
            sqlstr += "     FROM PRM_SAFE_FRE A INNER JOIN MDM_COM_CODE B ON A.PORT = B.OPT_ITEM1 ";
            sqlstr += " WHERE 1=1  ";
            sqlstr += " AND GRP_CD = 'P03' ";
            sqlstr += " AND A.PERIOD_YEAR = '" + dr["PERIOD_YEAR"].ToString() + "' ";
            sqlstr += " AND A.PERIOD_QUARTER = '" + dr["PERIOD_QUARTER"].ToString() + "' ";
            sqlstr += " GROUP BY PORT, SECTION, B.OPT_ITEM2 ";
            sqlstr += " ORDER BY SECTION ASC ";

            return sqlstr;
        }

        /// <summary>
        /// 할증 데이터 가져오기
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string SetPremiumRate_Query(DataRow dr)
        {
            sqlstr = "";

            sqlstr += " SELECT A.P_RATE_NAME, ";
            sqlstr += "        A.P_RATE_PRICE, ";
            sqlstr += "        A.P_RATE_WON, ";
            sqlstr += "        A.P_RATE_TYPE, ";
            sqlstr += "        B.P_RATE_TYPE_ROW, ";
            sqlstr += "        B.TYPE_TOTCNT, ";
            sqlstr += "        B.ROW_TOTCNT, ";
            sqlstr += "        B.TOTCNT, ";
            sqlstr += "        A.P_RATE_DEPTH, ";
            sqlstr += "        A.EXCEPTION, ";
            sqlstr += "        A.RMK ";
            sqlstr += "   FROM    PRM_SAFE_FRE_PR A ";
            sqlstr += "        LEFT JOIN ";
            sqlstr += "           (SELECT P_RATE_TYPE, ";
            sqlstr += "                   ROWNUM AS P_RATE_TYPE_ROW, ";
            sqlstr += "                   TYPE_TOTCNT, ";
            sqlstr += "                   COUNT (*) OVER () AS ROW_TOTCNT, ";
            sqlstr += "                   SUM (TYPE_TOTCNT) OVER () AS TOTCNT ";
            sqlstr += "              FROM (  SELECT P_RATE_TYPE,  ";
            sqlstr += "                           COUNT (P_RATE_TYPE) AS TYPE_TOTCNT ";
            sqlstr += "                        FROM PRM_SAFE_FRE_PR ";
            sqlstr += "                       WHERE P_RATE_TYPE IS NOT NULL ";
            sqlstr += "                    GROUP BY P_RATE_TYPE)) B ";
            sqlstr += "        ON A.P_RATE_TYPE = B.P_RATE_TYPE ";
            sqlstr += "  WHERE 1 = 1 ";
            sqlstr += "  AND PERIOD_YEAR = '" + dr["PERIOD_YEAR"].ToString() + "'  ";
            sqlstr += "  AND PERIOD_QUARTER = '" + dr["PERIOD_QUARTER"].ToString() + "' ";
            sqlstr += "  ORDER BY P_RATE_TYPE_ROW ASC";

            return sqlstr;
        }

        /// <summary>
        /// 안전 운임 데이터 검색
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string TariffSerach_Query(DataRow dr)
        {
            sqlstr = "";

            sqlstr += " SELECT (SELECT OPT_ITEM2 FROM MDM_COM_CODE  WHERE GRP_CD = 'P03' AND OPT_ITEM1 = PORT) AS PORT, ";
            sqlstr += "        ADDR_STATE, ";
            sqlstr += "        ADDR_CITY, ";
            sqlstr += "        ADDR_TOWNSHIP, ";

            if (dr["TYPE"].ToString() == "TF")
            {
                sqlstr += "        TF_20FT, ";
                sqlstr += "        TF_40FT, ";
            } 
            else if (dr["TYPE"].ToString() == "CF")
            {
                sqlstr += "        CF_20FT, ";
                sqlstr += "        CF_40FT, ";
            }
            else if (dr["TYPE"].ToString() == "IF")
            {
                sqlstr += "        IF_20FT, ";
                sqlstr += "        IF_40FT, ";
            }

            sqlstr += "        DISTANCE ";
            sqlstr += "   FROM PRM_SAFE_FRE ";

            sqlstr += " WHERE ";
            sqlstr += "   1=1 ";
            sqlstr += "   AND PERIOD_YEAR = '"+dr["YEAR"].ToString()+"' ";
            sqlstr += "   AND PERIOD_QUARTER = '" + dr["QUARTER"].ToString() + "' ";

            //if (dr["SECTION_TYPE"].ToString() == "왕복")
            //{
            //    sqlstr += "   AND SECTION_RETURN = '" + dr["SECTION"].ToString() + "' ";
            //}
            //else if (dr["SECTION_TYPE"].ToString() == "편도")
            //{
            //    sqlstr += "   AND SECTION_ONEWAY = '" + dr["SECTION"].ToString() + "' ";
            //}

            if (dr["PORT"].ToString() != "")
            {
                sqlstr += "   AND PORT = '" + dr["PORT"].ToString() + "' ";
            }

            if (dr["SECTION"].ToString() != "")
            {
                sqlstr += "   AND SECTION = '" + dr["SECTION"].ToString() + "' ";
            }

            if (dr["ADDRSTATE"].ToString() != "")
            {
                sqlstr += " AND ADDR_STATE = '" + dr["ADDRSTATE"].ToString() + "' ";
            }

            if (dr["ADDRCITY"].ToString() != "")
            {
                sqlstr += " AND ADDR_CITY = '" + dr["ADDRCITY"].ToString() + "' ";
            }

            if (dr["ADDRTOWNSHIP"].ToString() != "")
            {
                sqlstr += " AND ADDR_TOWNSHIP = '" + dr["ADDRTOWNSHIP"].ToString() + "' ";
            }

            //데이터가 없을 경우 기본
            //데이터가 있을 경우 SORT 다시

            if (dr["ID"].ToString() != "")
            {
                sqlstr += " ORDER BY "+ dr["ID"].ToString()+" "+ dr["ORDER"].ToString();
            }
            else
            {
                sqlstr += " ORDER BY ADDR_STATE ASC , ADDR_CITY ASC , ADDR_TOWNSHIP ASC , TO_NUMBER(DISTANCE) ";
            }            

            return sqlstr;
        }
    }
}
