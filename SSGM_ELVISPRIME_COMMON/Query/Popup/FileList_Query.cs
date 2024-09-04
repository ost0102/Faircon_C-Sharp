using System.Data;

namespace SSGM_ELVISPRIME_COMMON.Query.Popup
{
    public class FileList_Query
    {
        string sqlstr;

        /// <summary>
        /// MyBoard - 문서 파일 데이터 가져오기
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string GetMyBaordFileList_Query(DataRow dr)
        {
            sqlstr += "	SELECT A.MNGT_NO, ";
            sqlstr += "     A.MNGT_NO AS MNGT_NM, ";
            sqlstr += "     A.SEQ AS SEQ, ";
            sqlstr += "     A.FILE_NM AS FILE_NM, ";
            sqlstr += "     DOC_TYPE, ";
            sqlstr += "     A.DOC_NO AS DOC_NO, ";
            sqlstr += "     A.FILE_PATH AS FILE_PATH, ";
            sqlstr += "     (SELECT CD_NM ";
            sqlstr += "        FROM MDM_COM_CODE ";
            sqlstr += "       WHERE GRP_CD = 'M33' AND COMN_CD = A.DOC_TYPE) ";
            sqlstr += "        AS DOC_NM, ";
            sqlstr += "     A.OFFICE_CD AS OFFICE_CD, ";
            sqlstr += "     A.SYS_ID AS SYS_ID, ";
            sqlstr += "     A.FORM_ID AS FORM_ID, ";
            sqlstr += "     A.FILE_SIZE AS FILE_SIZE, ";
            sqlstr += "     A.INS_YMD AS INS_YMD, ";
            sqlstr += "     A.INS_HM AS INS_HM, ";
            sqlstr += "     A.INS_YMD || A.INS_HM AS INS_TOTAL";
            sqlstr += "     FROM COM_DOC_MST A ";
            sqlstr += "    WHERE MNGT_NO = '" + dr["MNGT_NO"].ToString() + "' ";
            if (dr["USR_TYPE"].ToString() == "S" || dr["USR_TYPE"].ToString() == "M")
            {
                sqlstr += "          AND DOC_TYPE IN (" + dr["HBL_DOC_TYPE"].ToString() + ") ";
            }
            else if (dr["USR_TYPE"].ToString() == "P")
            {
                sqlstr += "          AND DOC_TYPE IN (" + dr["PARTNER_DOC_TYPE"].ToString() + ") ";
            }
            sqlstr += "                UNION ";
            sqlstr += " SELECT A.MNGT_NO, ";
            sqlstr += "        A.MNGT_NO AS MNGT_NM, ";
            sqlstr += "        A.SEQ AS SEQ, ";
            sqlstr += "        A.FILE_NM AS FILE_NM, ";
            sqlstr += "        DOC_TYPE, ";
            sqlstr += "        A.DOC_NO AS DOC_NO, ";
            sqlstr += "        A.FILE_PATH AS FILE_PATH, ";
            sqlstr += "        (SELECT CD_NM ";
            sqlstr += "           FROM MDM_COM_CODE ";
            sqlstr += "          WHERE GRP_CD = 'M33' AND COMN_CD = A.DOC_TYPE) ";
            sqlstr += "           AS DOC_NM, ";
            sqlstr += "        A.OFFICE_CD AS OFFICE_CD, ";
            sqlstr += "        A.SYS_ID AS SYS_ID, ";
            sqlstr += "        A.FORM_ID AS FORM_ID, ";
            sqlstr += "        A.FILE_SIZE AS FILE_SIZE, ";
            sqlstr += "        A.INS_YMD AS INS_YMD, ";
            sqlstr += "        A.INS_HM AS INS_HM, ";
            sqlstr += "        A.INS_YMD || A.INS_HM AS INS_TOTAL ";
            sqlstr += "   FROM COM_DOC_MST A ";
            sqlstr += "  WHERE MNGT_NO = '" + dr["MNGT_NO"].ToString() + "'";
            sqlstr += "        AND DOC_TYPE = 'CHBL' ";
            sqlstr += "        AND SEQ = (SELECT MAX(SEQ) FROM COM_DOC_MST  WHERE MNGT_NO = '" + dr["MNGT_NO"].ToString() + "' AND DOC_TYPE = 'CHBL') ";
            sqlstr += "                UNION ";
            sqlstr += " SELECT A.MNGT_NO, ";
            sqlstr += "        A.MNGT_NO AS MNGT_NM, ";
            sqlstr += "        A.SEQ AS SEQ, ";
            sqlstr += "        A.FILE_NM AS FILE_NM, ";
            sqlstr += "        DOC_TYPE, ";
            sqlstr += "        A.DOC_NO AS DOC_NO, ";
            sqlstr += "        A.FILE_PATH AS FILE_PATH, ";
            sqlstr += "        (SELECT CD_NM ";
            sqlstr += "           FROM MDM_COM_CODE ";
            sqlstr += "          WHERE GRP_CD = 'M33' AND COMN_CD = A.DOC_TYPE) ";
            sqlstr += "           AS DOC_NM, ";
            sqlstr += "        A.OFFICE_CD AS OFFICE_CD, ";
            sqlstr += "        A.SYS_ID AS SYS_ID, ";
            sqlstr += "        A.FORM_ID AS FORM_ID, ";
            sqlstr += "        A.FILE_SIZE AS FILE_SIZE, ";
            sqlstr += "        A.INS_YMD AS INS_YMD, ";
            sqlstr += "        A.INS_HM AS INS_HM, ";
            sqlstr += "        A.INS_YMD || A.INS_HM AS INS_TOTAL ";
            sqlstr += "   FROM COM_DOC_MST A ";
            sqlstr += "  WHERE MNGT_NO = '" + dr["MNGT_NO"].ToString() + "'";
            sqlstr += "        AND DOC_TYPE = 'AN' ";
            sqlstr += "        AND SEQ = (SELECT MAX(SEQ) FROM COM_DOC_MST  WHERE MNGT_NO = '" + dr["MNGT_NO"].ToString() + "' AND DOC_TYPE = 'AN') ";
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
            if (dr["USR_TYPE"].ToString() != "P")
            {
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
            }
            if (dr["MNGT_NO"].ToString() == "")
            {
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
            }
            return sqlstr;
        }
    }
}
