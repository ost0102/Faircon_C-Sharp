using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SSGM_ELVISPRIME_COMMON.Query.Admin
{
    public class Admin_Query
    {   
        string sqlstr;

        /*****************************관리자 페이지************************************/
        public string fnAdminLogin_Query(string id,string pwd,string memberKey)
        {

            sqlstr += " SELECT MEMB_NO FROM MEMBER ";
            sqlstr += " WHERE M_ID = '" + id + "' ";
            sqlstr += " AND CryptString.decrypt(PASSWORD, '" + memberKey + "') = '" + pwd + "' ";

            return sqlstr;
        }
        /******************************************************************************/

        /// <summary>
        /// 파일 로그 (공통)
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string InsertFileLog_Query(DataRow dr)
        {
            sqlstr = "";

            sqlstr += " INSERT INTO PRM_FILE_LOG  ";
            sqlstr += " ( ";
            sqlstr += " MNGT_NO  ";
            sqlstr += " ,DOC_TYPE  ";
            sqlstr += " ,FILE_NM  ";
            sqlstr += " ,REPLACE_FILE_NM  ";
            sqlstr += " ,FILE_PATH  ";
            sqlstr += " ,FILE_SIZE  ";
            sqlstr += " ,EXTENSION  ";
            sqlstr += " ,INS_USR  ";
            sqlstr += " ,INS_YMD  ";
            sqlstr += " ,INS_HM  ";
            sqlstr += " ) ";
            sqlstr += " VALUES ";
            sqlstr += " ( ";
            sqlstr += " '" + dr["MNGT_NO"].ToString() + "' ";            
            sqlstr += " ,'" + dr["DOC_TYPE"].ToString() + "' ";
            sqlstr += " ,'" + dr["FILE_NM"].ToString() + "' ";
            sqlstr += " ,'" + dr["REPLACE_FILE_NM"].ToString() + "' ";
            sqlstr += " ,'" + dr["FILE_PATH"].ToString() + "' ";
            sqlstr += " ,'" + dr["FILE_SIZE"].ToString() + "' ";
            sqlstr += " ,'" + dr["EXTENSION"].ToString() + "' ";            
            sqlstr += " ,'" + dr["INS_USR"].ToString() + "' ";
            sqlstr += " ,UFN_DATE_FORMAT('DATE') ";
            sqlstr += " ,UFN_DATE_FORMAT('TIME') ";
            sqlstr += " ) ";

            return sqlstr;
        }




    }
}