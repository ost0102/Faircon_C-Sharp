using System.Data;

namespace SSGM_ELVISPRIME_COMMON.Query.LogisticsTools
{
    public class Vehicle_Query
    {
        string sqlstr;

        /// <summary>
        /// 차량 구분 select
        /// </summary>
        /// <returns></returns>
        public string SetVehicleDiv_Query()
        {
            sqlstr = "";

            sqlstr += " SELECT DISTINCT ";
            sqlstr += "          (CASE WHEN CAR_DIV_CODE = '50UP' THEN '6톤이상' ELSE CAR_DIV END) ";
            sqlstr += "             AS CAR_DIV, ";
            sqlstr += "          CAR_DIV_CODE ";
            sqlstr += "     FROM PRM_VEHICLE_MST ";
            sqlstr += " GROUP BY CAR_DIV, CAR_DIV_CODE ";
            sqlstr += " ORDER BY CAR_DIV_CODE ASC, CAR_DIV ASC ";

            return sqlstr;
        }

        /// <summary>
        /// 차량 제원 검색 SQL
        /// </summary>
        /// <returns></returns>
        public string SearchVehicle_Query(DataRow dr)
        {
            sqlstr = "";

            sqlstr += " SELECT CAR_DIV, ";
            sqlstr += "          CAR_NAME, ";
            sqlstr += "          SHORTHAND, ";
            sqlstr += "          CAR_WIDTH, ";
            sqlstr += "          TOP_HEIGHT, ";
            sqlstr += "          BOTTOM_HEIGHT, ";
            sqlstr += "          CAR_AREA, ";
            sqlstr += "          CAR_WEIGHT, ";
            sqlstr += "          CAR_CBM, ";
            sqlstr += "          TOTAL_HEIGHT, ";
            sqlstr += "          RMK, ";
            sqlstr += "          IMG_NAME, ";
            sqlstr += "          IMG_PATH, ";
            sqlstr += "          REPLACE_IMG_NAME ";
            sqlstr += "     FROM PRM_VEHICLE_MST ";
            sqlstr += "    WHERE 1=1 ";

            if (dr["CAR_DIV_CODE"].ToString() != "")
            {
                sqlstr += "    AND CAR_DIV_CODE = '" + dr["CAR_DIV_CODE"].ToString() + "' ";
            }
            
            sqlstr += " ORDER BY  CAR_DIV_CODE ASC, CAR_DIV ASC , SEQ ASC ";

            return sqlstr;
        }


        

    }
}
