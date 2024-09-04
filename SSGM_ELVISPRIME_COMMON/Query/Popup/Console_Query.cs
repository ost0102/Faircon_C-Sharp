using System.Data;

namespace SSGM_ELVISPRIME_COMMON.Query.Popup
{
    public class Console_Query
    {
        string sqlstr;

        /// <summary>
        /// 입고 상세 내역 조회 - 입고 정보
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>6
        public string GetLayerConsole_Query(DataRow dr)
        {
            sqlstr = "";

            sqlstr += " SELECT D.GR_YMD ";
            sqlstr += "      , D.GR_NO ";
            sqlstr += "      , D.ACT_CUST_NM ";
            sqlstr += "      , D.SKU_NM ";
            sqlstr += "      , M.POD_NM ";
            sqlstr += "      , M.POD_CD ";
            sqlstr += "      , D.BK_QTY ";
            sqlstr += "      , D.BK_GRS_WGT ";
            sqlstr += "      , D.BK_MSRMT ";
            sqlstr += "      , D.MARK ";
            sqlstr += "      , D.BK_QTY_UNIT ";
            sqlstr += "      , M.BK_NO ";
            sqlstr += "      , (SELECT MAX(FILE_NM) FROM COM_DOC_MST WHERE MNGT_NO = M.BK_NO  AND DOC_TYPE = 'ETCH') AS FILE_NM";
            sqlstr += "      , D.RMK2 ";
            sqlstr += "      , D.ACT_GR_CUST ";
            sqlstr += "      , D.ACT_MSRMT ";
            sqlstr += "      , D.ACT_MSRMT ";
            sqlstr += "      , D.ACT_QTY ";
            sqlstr += "   FROM CFS_BK_DTL D ";
            sqlstr += "   JOIN CFS_BK_MST M ";
            sqlstr += "     ON D.WH_CD  = M.WH_CD ";
            sqlstr += "    AND D.BK_NO  = M.BK_NO ";
            sqlstr += "  WHERE 1=1 ";
            //sqlstr += "    AND D.WH_CD  = 'YS1' ";
            sqlstr += "    AND D.BK_NO  = '" + dr["BK_NO"].ToString() + "' ";
            if (dr["BK_SEQ"].ToString() != "")
            {
                sqlstr += "    AND D.BK_SEQ  = '" + dr["BK_SEQ"].ToString() + "' ";
            }

            if (dr["USR_TYPE"].ToString() != "M")
            {
                sqlstr += "   AND M.SHP_CD = '" + dr["CUST_CD"].ToString() + "'";
            }

            return sqlstr;
        }

        /// <summary>
        /// 입고 상세 내역 조회 - 세부 정보
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string GetLayerConsoleDetail_Query(DataRow dr)
        {
            sqlstr = "";

            sqlstr += " SELECT ";
            sqlstr += "      D.GR_NO ";
            sqlstr += "      , DIM.DIM_SEQ ";
            sqlstr += "      , DIM.SIZE_W ";
            sqlstr += "      , DIM.SIZE_D ";
            sqlstr += "      , DIM.SIZE_H ";
            sqlstr += "      , DIM.QTY ";
            sqlstr += "      , DIM.MSRMT ";
            sqlstr += "      , D.RMK2 ";
            sqlstr += "   FROM ";
            sqlstr += "   CFS_BK_MST M ";
            sqlstr += "   JOIN CFS_BK_DTL D ";
            sqlstr += "   ON M.BK_NO = D.BK_NO ";
            sqlstr += "   JOIN CFS_BK_DIM DIM ";
            sqlstr += "     ON D.WH_CD  = DIM.WH_CD ";
            sqlstr += "    AND D.BK_NO  = DIM.BK_NO ";
            sqlstr += "    AND D.BK_SEQ = DIM.BK_SEQ ";
            sqlstr += "  WHERE 1=1 ";
            //sqlstr += "    AND D.WH_CD  = 'YS1' ";
            sqlstr += "    AND D.GR_NO  = '" + dr["GR_NO"].ToString() + "' ";
            sqlstr += "    AND M.BK_NO  = '" + dr["BK_NO"].ToString() + "' ";
            if (dr["BK_SEQ"].ToString() != "")
            {
                sqlstr += "    AND DIM.BK_SEQ = '" + dr["BK_SEQ"].ToString() + "' ";
            }
            if (dr["USR_TYPE"].ToString() != "M")
            {
                sqlstr += "   AND M.SHP_CD = '" + dr["CUST_CD"].ToString() + "'";
            }

            return sqlstr; 
        }
    }
}
