using System.Data;

namespace SSGM_ELVISPRIME_COMMON.Query.LogisticsTools
{
    public class Surcharge_Query
    {
        string sqlstr;

        /// <summary>
        /// 부대비용 - PORT 가져오기 (선사 , 훼리)
        /// </summary>
        /// <returns></returns>
        public string SetSurchargePort_Query(DataRow dr)
        {
            sqlstr = "";

            sqlstr += "   SELECT PORT ";
            sqlstr += "     FROM PRM_CHARGE_MST ";
            sqlstr += "    WHERE SHIPPING = '" + dr["PORT_TYPE"].ToString() + "' ";
            sqlstr += " GROUP BY PORT ";
            sqlstr += " ORDER BY PORT ASC ";

            return sqlstr;
        }

        /// <summary>
        /// 수입 / 수출 국가옵션 가져오기
        /// </summary>
        /// <returns></returns>
        public string SetSurchargeCountry_Query(DataRow dr)
        {
            sqlstr = "";

            sqlstr += "   SELECT COUNTRY_OPTION ";
            sqlstr += "     FROM PRM_CHARGE_MST ";
            sqlstr += "    WHERE BOUND = '" + dr["BOUND"].ToString() + "' ";
            sqlstr += " GROUP BY COUNTRY_OPTION ";
            sqlstr += " ORDER BY COUNTRY_OPTION ASC ";

            return sqlstr;
        }

        /// <summary>
        /// 부대비용 - 컨테이너 타입 데이터 가져오기 (Select박스)
        /// </summary>
        /// <returns></returns>
        public string SetCntrType_Query(DataRow dr)
        {
            sqlstr = "";

            sqlstr += "   SELECT CASE ";
            sqlstr += "             WHEN CNTR_TYPE = 'DRY' THEN 4 ";
            sqlstr += "             WHEN CNTR_TYPE = 'REEFER' THEN 3 ";
            sqlstr += "             WHEN CNTR_TYPE = 'OPEN TOP' THEN 2 ";
            sqlstr += "             WHEN CNTR_TYPE = 'FLAT RACK' THEN 1 ";
            sqlstr += "             ELSE 0 ";
            sqlstr += "          END ";
            sqlstr += "             AS SEQ, ";
            sqlstr += "          CNTR_TYPE ";
            sqlstr += "     FROM PRM_CHARGE_MST ";
            sqlstr += "    WHERE    ";
            sqlstr += "    1=1 ";
            sqlstr += "    AND SHIPPING = '"+dr["SHIPPING"].ToString() + "' ";
            sqlstr += "    AND BOUND = '" + dr["BOUND"].ToString() + "' ";
            sqlstr += "    AND PORT = '" + dr["PORT"].ToString() + "' ";
            sqlstr += " GROUP BY CNTR_TYPE ";
            sqlstr += " ORDER BY SEQ DESC, CNTR_TYPE ASC ";

            return sqlstr;
        }

        /// <summary>
        /// 부대비용 - 컨테이너 사이즈 데이터 가져오기 (Select박스)
        /// </summary>
        /// <returns></returns>
        public string SetCntrSize_Query(DataRow dr)
        {
            sqlstr = "";

            sqlstr += " SELECT CNTR_SIZE ";
            sqlstr += "     FROM PRM_CHARGE_MST ";
            sqlstr += "    WHERE ";
            sqlstr += " 	1=1  ";
            sqlstr += " 	AND SHIPPING = '" + dr["SHIPPING"].ToString() + "' ";
            sqlstr += "     AND BOUND = '" + dr["BOUND"].ToString() + "' ";
            sqlstr += "     AND PORT = '" + dr["PORT"].ToString() + "' ";
            sqlstr += "     AND CNTR_TYPE = '" + dr["CNTR_TYPE"].ToString() + "' ";
            sqlstr += " GROUP BY CNTR_SIZE ";

            return sqlstr;
        }

        /// <summary>
        /// 부대비용 CntrList 검색
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string CntrListSurcharge_Query(DataRow dr)
        {
            sqlstr = "";

            sqlstr += "   SELECT CNTR_SIZE ";
            sqlstr += "     FROM PRM_CHARGE_MST ";
            sqlstr += "  WHERE     1 = 1 ";

            if (dr["PORT_TYPE"].ToString() != "")
            {
                sqlstr += "        AND SHIPPING = '" + dr["PORT_TYPE"].ToString() + "' ";
            }

            if (dr["BOUND"].ToString() != "")
            {
                sqlstr += "        AND BOUND = '" + dr["BOUND"].ToString() + "' ";
            }

            if (dr["PORT"].ToString() != "")
            {
                sqlstr += "        AND PORT = '" + dr["PORT"].ToString() + "' ";
            }

            if (dr["CNTR_TYPE"].ToString() != "")
            {
                sqlstr += "        AND CNTR_TYPE = '" + dr["CNTR_TYPE"].ToString() + "' ";
            }

            if (dr["PORT_TYPE"].ToString() == "S")
            {
                if (dr["COUNTRY_OPTION"].ToString() == "")
                {
                    sqlstr += "        AND COUNTRY_OPTION = '기본' ";
                }
                else
                {
                    sqlstr += "        AND COUNTRY_OPTION = '" + dr["COUNTRY_OPTION"].ToString() + "' ";
                }
            }

            sqlstr += " GROUP BY CNTR_SIZE ";
            sqlstr += " ORDER BY CNTR_SIZE ASC ";

            return sqlstr;
        }

        /// <summary>
        /// 부대비용 검색
        /// </summary>
        /// <returns></returns>
        public string SearchSurcharge_Query(DataRow dr)
        {
            sqlstr = "";

            sqlstr += " SELECT CONTENTS, ";
            sqlstr += "        CODE, ";
            sqlstr += "        UNIT, ";
            sqlstr += "        CNTR_SIZE, ";
            sqlstr += "        CURRENCY, ";
            sqlstr += "        PRICE ";
            sqlstr += "   FROM PRM_CHARGE_MST ";
            sqlstr += "  WHERE     1 = 1 ";

            if (dr["PORT_TYPE"].ToString() != "")
            {
                sqlstr += "        AND SHIPPING = '"+ dr["PORT_TYPE"].ToString() + "' ";
            }

            if (dr["BOUND"].ToString() != "")
            {
                sqlstr += "        AND BOUND = '"+ dr["BOUND"].ToString() + "' ";
            }

            if (dr["PORT"].ToString() != "")
            {
                sqlstr += "        AND PORT = '"+ dr["PORT"].ToString() + "' ";
            }

            if (dr["CNTR_TYPE"].ToString() != "")
            {
                sqlstr += "        AND CNTR_TYPE = '"+ dr["CNTR_TYPE"].ToString() + "' ";
            }            

            if (dr["PORT_TYPE"].ToString() == "S")
            {
                if (dr["COUNTRY_OPTION"].ToString() == "")
                {
                    sqlstr += "        AND COUNTRY_OPTION = '기본' ";
                }
                else
                {
                    sqlstr += "        AND COUNTRY_OPTION = '" + dr["COUNTRY_OPTION"].ToString() + "' ";
                }
            }

            return sqlstr;
        }


    }
}
