using System.Data;

namespace SSGM_ELVISPRIME_COMMON.Query.File
{
    public class File_Query
    {
        string sqlstr;

        /// <summary>
        /// 파일 데이터 가져오는 쿼리
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="url_path"></param>
        /// <returns></returns>
        public string SelectHblFile_Query(DataRow dr, string url_path)
        {
            sqlstr = " SELECT MNGT_NO ";
            sqlstr += "         , SEQ ";
            sqlstr += "         , FILE_NM ";
            sqlstr += "         , FILE_PATH ";
            sqlstr += "         , '" + dr["DOMAIN"].ToString() + "' AS OFFICE_CD ";
            sqlstr += "         , OFFICE_CD ";
            sqlstr += "         , '" + url_path + "' AS URL_PATH ";
            sqlstr += "         , MNGT_NO || '_' || SEQ || '_' || FILE_NM AS FILE_NAME ";
            sqlstr += "   FROM  COM_DOC_MST ";
            sqlstr += "   WHERE MNGT_NO = '" + dr["MNGT_NO"].ToString() + "'";
            sqlstr += "       AND  SEQ = " + dr["SEQ"].ToString();
            return sqlstr;
        }

        /// <summary>
        /// 부킹 파일 업로드 로직
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string fnSetBKFileUpload_Query(DataRow dr)
        {
            sqlstr = "";

            sqlstr += " 	INSERT                                                                                                  ";
            sqlstr += " 	    INTO COM_DOC_MST                                                                                    ";
            sqlstr += " 	    (MNGT_NO                                                                                            ";
            sqlstr += " 	    , SEQ                                                                                               ";
            sqlstr += " 	    , FILE_NM                                                                                           ";
            sqlstr += " 	    , FILE_PATH                                                                                         ";
            sqlstr += " 	    , FILE_SIZE                                                                                         ";
            sqlstr += " 	    , DOC_TYPE                                                                                          ";
            sqlstr += " 	    , DOC_NO                                                                                            ";
            sqlstr += " 	    , OFFICE_CD                                                                                         ";
            sqlstr += " 	    , SYS_ID                                                                                            ";
            sqlstr += " 	    , FORM_ID                                                                                            ";
            sqlstr += " 	    , INS_USR                                                                                           ";
            sqlstr += " 	    , INS_YMD                                                                                           ";
            sqlstr += " 	    , INS_HM)                                                                                           ";
            sqlstr += " 	    VALUES                                                                                              ";
            sqlstr += " 	    ('" + dr["MNGT_NO"].ToString() + "'                                                                 ";
            sqlstr += " 		,(SELECT NVL (MAX(SEQ), 0) + 1 FROM COM_DOC_MST WHERE MNGT_NO = '" + dr["MNGT_NO"].ToString() + "') ";
            sqlstr += " 		,'" + dr["FILE_NM"].ToString() + "'	                                                                ";
            sqlstr += " 		,'" + dr["FILE_PATH"].ToString() + "'	                                                            ";
            sqlstr += " 		,'" + dr["FILE_SIZE"].ToString() + "'	                                                            ";
            sqlstr += " 		,'" + dr["DOC_TYPE"].ToString() + "'	                                                            ";
            sqlstr += " 		,'" + dr["DOC_NO"].ToString() + "'	                                                                ";
            sqlstr += " 		,'" + dr["OFFICE_CD"].ToString() + "'	                                                            ";
            sqlstr += " 		,'" + dr["SYS_ID"].ToString() + "'	                                                                ";
            sqlstr += " 		,'" + dr["FORM_ID"].ToString() + "'	                                                                ";
            sqlstr += " 		,'" + dr["INS_USR"].ToString() + "'	                                                                ";
            sqlstr += " 		,'" + dr["INS_YMD"].ToString() + "'	                                                                ";
            sqlstr += " 		,'" + dr["INS_HM"].ToString() + "')	                                                            ";

            return sqlstr;
        }

        /// <summary>
        /// 파일 리스트 삭제
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="url_path"></param>
        /// <returns></returns>
        public string fnDocDeleteFile_Query(DataRow dr)
        {
            sqlstr = " ";
            sqlstr += " DELETE FROM COM_DOC_MST ";
            sqlstr += " WHERE MNGT_NO = '" + dr["MNGT_NO"].ToString() + "' AND";
            sqlstr += " (FILE_NM = '" + dr["FILE_NM"].ToString() + "' AND SEQ = '" + dr["SEQ"].ToString() + "') ";

            return sqlstr;
        }

        /// <summary>
        /// 파일 구분 업데이트
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="url_path"></param>
        /// <returns></returns>
        public string fnDocUpdateFile_Query(DataRow dr)
        {
            sqlstr = " ";
            sqlstr += " UPDATE COM_DOC_MST ";
            sqlstr += " SET DOC_TYPE = '" + dr["DOC_TYPE"].ToString() + "' ";
            sqlstr += " WHERE MNGT_NO = '" + dr["MNGT_NO"].ToString() + "' AND";
            sqlstr += " (FILE_NM = '" + dr["FILE_NM"].ToString() + "' AND SEQ = '" + dr["SEQ"].ToString() + "') ";

            return sqlstr;
        }

        /// <summary>
        /// 현재 MNGT_NO MAX SEQ 가져오기
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="url_path"></param>
        /// <returns></returns>
        public string fnGetSEQ_Query(string MngtNo)
        {
            sqlstr = " ";
            sqlstr += " SELECT MAX(SEQ) AS SEQ FROM COM_DOC_MST ";
            sqlstr += " WHERE MNGT_NO = '" + MngtNo + "' ";

            return sqlstr;
        }

        /// <summary>
        /// 콘솔 파일 업로드 로직
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string fnSetConsoleFileUpload_Query(DataRow dr)
        {
            sqlstr = "";

            sqlstr += " 	INSERT                                                                                                  ";
            sqlstr += " 	    INTO COM_DOC_MST                                                                                    ";
            sqlstr += " 	    (MNGT_NO                                                                                            ";
            sqlstr += " 	    , SEQ                                                                                               ";
            sqlstr += " 	    , FILE_NM                                                                                           ";
            sqlstr += " 	    , FILE_PATH                                                                                         ";
            sqlstr += " 	    , FILE_SIZE                                                                                         ";
            sqlstr += " 	    , DOC_TYPE                                                                                          ";
            sqlstr += " 	    , DOC_NO                                                                                            ";
            sqlstr += " 	    , OFFICE_CD                                                                                         ";
            sqlstr += " 	    , SYS_ID                                                                                            ";
            sqlstr += " 	    , FORM_ID                                                                                            ";
            sqlstr += " 	    , REPLACE_FILE_NM                                                                                            ";
            sqlstr += " 	    , INS_USR                                                                                           ";
            sqlstr += " 	    , INS_YMD                                                                                           ";
            sqlstr += " 	    , INS_HM)                                                                                           ";
            sqlstr += " 	    VALUES                                                                                              ";
            sqlstr += " 	    ('" + dr["MNGT_NO"].ToString() + "'                                                                 ";
            sqlstr += " 		,(SELECT NVL (MAX(SEQ), 0) + 1 FROM COM_DOC_MST WHERE MNGT_NO = '" + dr["MNGT_NO"].ToString() + "') ";
            sqlstr += " 		,'" + dr["FILE_NM"].ToString() + "'	                                                                ";
            sqlstr += " 		,'" + dr["FILE_PATH"].ToString() + "'	                                                            ";
            sqlstr += " 		,'" + dr["FILE_SIZE"].ToString() + "'	                                                            ";
            sqlstr += " 		,'" + dr["DOC_TYPE"].ToString() + "'	                                                            ";
            sqlstr += " 		,'" + dr["DOC_NO"].ToString() + "'	                                                                ";
            sqlstr += " 		,'" + dr["OFFICE_CD"].ToString() + "'	                                                            ";
            sqlstr += " 		,'" + dr["SYS_ID"].ToString() + "'	                                                                ";
            sqlstr += " 		,'" + dr["FORM_ID"].ToString() + "'	                                                                ";
            sqlstr += " 		,'" + dr["REPLACE_FILE_NM"].ToString() + "'	                                                                ";
            sqlstr += " 		,'" + dr["INS_USR"].ToString() + "'	                                                                ";
            sqlstr += " 		,'" + dr["INS_YMD"].ToString() + "'	                                                                ";
            sqlstr += " 		,'" + dr["INS_HM"].ToString() + "')	                                                            ";

            return sqlstr;
        }

        /// <summary>
        /// 콘솔 - 파일 데이터 가져오는 쿼리
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="url_path"></param>
        /// <returns></returns>
        public string fnConsoleDownloadFile_Query(DataRow dr, string url_path)
        {
            sqlstr = " SELECT MNGT_NO ";
            sqlstr += "         , SEQ ";
            sqlstr += "         , FILE_NM ";
            sqlstr += "         , FILE_PATH ";
            sqlstr += "         , '" + dr["DOMAIN"].ToString() + "' AS OFFICE_CD ";
            sqlstr += "         , OFFICE_CD ";
            sqlstr += "         , '" + url_path + "' AS URL_PATH ";
            sqlstr += "         , MNGT_NO || '_' || SEQ || '_' || FILE_NM AS FILE_NAME ";
            sqlstr += "   FROM  COM_DOC_MST ";
            sqlstr += "   WHERE MNGT_NO = '" + dr["MNGT_NO"].ToString() + "'";
            sqlstr += "       AND  SEQ = " + dr["SEQ"].ToString();
            return sqlstr;
        }
    }
}
