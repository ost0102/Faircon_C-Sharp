using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Newtonsoft.Json;
using System.Text;

using System.Configuration;
using YJIT.Data;

namespace SSGM_ELVISPRIME_WEB.Models
{
    public class ADO_Conn
    {
        public string ConnectionStr = ConfigurationManager.ConnectionStrings["SSGM"].ConnectionString;
        private string memberKey = ConfigurationManager.AppSettings["memberKey"].ToString();

        public void ThrowMsg(bool ErrorOccur, string Msg)
        {
            ErrorOccur = true;
            throw new Exception(Msg);
        }

        public string Search_Notice(string Opt , string Type, string SearchText, int pageIndex)
        {
            string sSql = "";
            #region //이전 소스                        
            //sSql += "  SELECT * FROM ";
            //sSql += "  ( ";
            //sSql += "      SELECT ROW_NUMBER() OVER(ORDER BY A.REGDT) NUM ";
            //sSql += "              , A.* ";
            //sSql += "      FROM NOTICE A ";
            //sSql += "      WHERE A.USE_YN = 'y' ";
            //sSql += "         AND A.NOTICE_YN = 'y' ";
            //sSql += "      AND A.TITLE LIKE '%" + SearchText + "%' ";
            //sSql += "      ORDER BY A.REGDT DESC "; 
            //sSql += "  ) ";
            //sSql += "  UNION ALL ";
            //sSql += "  SELECT * FROM ";
            //sSql += "  ( ";
            //sSql += "      SELECT ROW_NUMBER() OVER(ORDER BY A.REGDT) NUM ";
            //sSql += "              , A.* ";
            //sSql += "      FROM NOTICE A ";
            //sSql += "      WHERE A.USE_YN = 'y' ";
            //sSql += "         AND A.NOTICE_YN = 'n' ";
            //sSql += "      AND A.TITLE LIKE '%" + SearchText + "%'";
            //sSql += "      ORDER BY A.REGDT DESC ";
            //sSql += " ) ";
            #endregion

            if (pageIndex == 0) pageIndex = 1;
            
            sSql += " SELECT * ";
            sSql += "  FROM ( ";
            sSql += "              SELECT ROWNUM AS RNUM ";
            sSql += "                     , FLOOR((ROWNUM-1) /10 + 1) AS PAGE ";
            sSql += "                     , COUNT(*) OVER () AS TOTCNT ";
            sSql += "                     , X.* ";
            sSql += "             FROM ( ";
            sSql += "                         SELECT * FROM             ";
            sSql += "                         ( SELECT ROW_NUMBER() OVER(ORDER BY A.REGDT) NUM, A.*  ";
            sSql += "                             FROM NOTICE A  ";
            sSql += "                            WHERE A.USE_YN = 'y'  ";
            sSql += "                               AND A.NOTICE_YN = 'y' ";
            if (!string.IsNullOrEmpty(SearchText)) sSql += "                               AND A." + Opt + " LIKE '%" + SearchText + "%' ";
            if (!string.IsNullOrEmpty(Type)) sSql += "                               AND A.TYPE = '" + Type + "' ";
            sSql += "                           ORDER BY A.NOTICE_ID DESC )  ";
            sSql += "                           UNION ALL ";
            sSql += "                         SELECT * FROM     ";
            sSql += "                           ( SELECT ROW_NUMBER() OVER(ORDER BY A.REGDT) NUM, A.*  ";
            sSql += "                              FROM NOTICE A  ";
            sSql += "                             WHERE A.USE_YN = 'y'  ";
            sSql += "                                AND A.NOTICE_YN = 'n'  ";
            if (!string.IsNullOrEmpty(SearchText)) sSql += "                               AND A." + Opt + " LIKE '%" + SearchText + "%' ";
            if (!string.IsNullOrEmpty(Type)) sSql += "                               AND A.TYPE = '" + Type + "' ";
            sSql += "                             ORDER BY A.NOTICE_ID DESC )   ";                   
            sSql += "                      ) X  ";
            sSql += "           ) XX ";
            sSql += "  WHERE XX.PAGE = "+ pageIndex +" ";
            
            return sSql;
        }

        public string Search_NoticeView(string noticeID)
        {
            string sSql = "";
            sSql += " SELECT 'VIEW' AS FLAG, V.* FROM NOTICE V ";
            sSql += " WHERE V.NOTICE_ID = " + noticeID + " ";
            sSql += " UNION ALL ";
            sSql += " SELECT 'PREV' AS FLAG, P.* FROM NOTICE P  ";
            sSql += " WHERE P.NOTICE_ID  = (SELECT MAX(NOTICE_ID) FROM NOTICE WHERE NOTICE_ID < " + noticeID + ") ";
            sSql += " UNION ALL ";
            sSql += " SELECT 'NEXT' AS FLAG, N.* FROM NOTICE N  ";
            sSql += " WHERE N.NOTICE_ID  = (SELECT MIN(NOTICE_ID) FROM NOTICE WHERE NOTICE_ID > " + noticeID + ") ";
            return sSql;
        }

        public string Update_ViewCnt(string noticeID)
        {
            string sSql = "";
            sSql += " UPDATE NOTICE SET CNT = CNT + 1 WHERE NOTICE_ID = '" + noticeID + "'";
            return sSql;
        }

        //public string Update_RecruitmentViewCnt(string noticeID)
        //{
        //    string sSql = "";
        //    sSql += " UPDATE RECRUITMENT SET CNT = CNT + 1 WHERE RECRUITMENT_ID = '" + noticeID + "'";
        //    return sSql;
        //}

        public string Admin_Notice(string Opt, string Type, string SearchText, int pageIndex)
        {
            string sSql = "";
            if (pageIndex == 0) pageIndex = 1;

            sSql += " SELECT * ";
            sSql += "  FROM ( ";
            sSql += "              SELECT ROWNUM AS RNUM ";
            sSql += "                     , FLOOR((ROWNUM-1) /10 + 1) AS PAGE ";
            sSql += "                     , COUNT(*) OVER () AS TOTCNT ";
            sSql += "                     , X.* ";
            sSql += "             FROM ( ";
            sSql += "                         SELECT * FROM             ";
            sSql += "                         ( SELECT ROW_NUMBER() OVER(ORDER BY A.REGDT) NUM, A.*  ";
            sSql += "                             FROM NOTICE A  ";
            sSql += "                           WHERE A.NOTICE_YN = 'y' ";
            if (Opt == "ALL")
            {
                sSql += "                               AND ( A.TITLE LIKE '%" + SearchText + "%' OR A.CONTENT LIKE '%" + SearchText + "%') ";
            }
            else
            {
                if (!string.IsNullOrEmpty(SearchText)) sSql += "                               AND A." + Opt + " LIKE '%" + SearchText + "%' ";
            }
            if (!string.IsNullOrEmpty(Type)) sSql += "                               AND A.TYPE = '" + Type + "' ";
            sSql += "                           ORDER BY A.REGDT DESC )  ";
            sSql += "                           UNION ALL ";
            sSql += "                         SELECT * FROM     ";
            sSql += "                           ( SELECT ROW_NUMBER() OVER(ORDER BY A.REGDT) NUM, A.*  ";
            sSql += "                              FROM NOTICE A  ";
            sSql += "                                WHERE A.NOTICE_YN = 'n'  ";
            if (Opt == "ALL")
            {
                sSql += "                               AND ( A.TITLE LIKE '%" + SearchText + "%' OR A.CONTENT LIKE '%" + SearchText + "%') ";
            }
            else
            {
                if (!string.IsNullOrEmpty(SearchText)) sSql += "                               AND A." + Opt + " LIKE '%" + SearchText + "%' ";
            }
            if (!string.IsNullOrEmpty(Type)) sSql += "                               AND A.TYPE = '" + Type + "' ";
            sSql += "                             ORDER BY A.REGDT DESC )   ";
            sSql += "                      ) X  ";
            sSql += "           ) XX ";
            sSql += "  WHERE XX.PAGE = " + pageIndex + " ";

            return sSql;
        }

        public string Admin_NoticeView(string noticeID)
        {
            string sSql = "";
            sSql += " SELECT 'VIEW' AS FLAG, V.* FROM NOTICE V ";
            sSql += " WHERE V.NOTICE_ID = " + noticeID + " ";            
            return sSql;
        }

        public string Admin_NoticeDel(string noticeID)
        {
            string sSql = "";
            sSql += " DELETE FROM NOTICE ";
            sSql += " WHERE NOTICE_ID = " + noticeID + " ";
            return sSql;
        }

        public string Admin_NoticeAdd(Hashtable ht)
        {
            string sSql = "";
            sSql += " INSERT INTO NOTICE VALUES ( ";
            sSql += "  (SELECT NVL(MAX(NOTICE_ID), 0) + 1 FROM NOTICE) ";   // NOTICE_ID
            sSql += " , '"+ ht["TITLE"].ToString() +"'";                            //TITLE
            sSql += " , 0";                                                             //CNT
            sSql += " , '관리자'";                                                     //WRITER
            sSql += " , '" + ht["USE_YN"].ToString() + "'";                       //USE_YN
            sSql += " , '" + ht["NOTICE_YN"].ToString() + "'";                  //NOTICE_YN
            sSql += " , TO_CHAR(SYSDATE, 'YYYY-MM-DD hh24:mi:ss')";   //REGDT
            sSql += " , '' ";                                                           //EDITDT
            sSql += " , '" + ht["FILE"].ToString() + "'";                          //FILE
            sSql += " , '" + ht["FILE_NAME"].ToString() + "'";                 //FILE_NAME
            sSql += " , '" + ht["FILE1"].ToString() + "'";                          //FILE1
            sSql += " , '" + ht["FILE1_NAME"].ToString() + "'";                 //FILE1_NAME
            sSql += " , '" + ht["FILE2"].ToString() + "'";                          //FILE2
            sSql += " , '" + ht["FILE2_NAME"].ToString() + "'";                 //FILE2_NAME
            sSql += " , '" + ht["CONTENT"].ToString() + "'";                 //CONTENT
            sSql += " , '" + ht["S_TYPE"].ToString() + "'";                 //TYPE
            sSql += " ) ";
            return sSql;
        }

        public string Admin_NoticeModify(Hashtable ht)
        {
            string sSql = "";
            sSql += " UPDATE NOTICE SET " + "\r\n";
            sSql += "     TITLE = '" + ht["TITLE"].ToString() + "' " + "\r\n";
            sSql += "    , USE_YN = '" + ht["USE_YN"].ToString() + "' " + "\r\n";
            sSql += "    , NOTICE_YN = '" + ht["NOTICE_YN"].ToString() + "' " + "\r\n";
            sSql += "    , EDITDT = TO_CHAR(SYSDATE, 'YYYY-MM-DD hh24:mi:ss') " + "\r\n";
            if (ht.ContainsKey("FILE"))
            {
                sSql += "    , \"FILE\" = '" + ht["FILE"].ToString() + "'" + "\r\n";                          //FILE
                sSql += "    , FILE_NAME = '" + ht["FILE_NAME"].ToString() + "'" + "\r\n";                          //FILE
            }
            if (ht.ContainsKey("FILE1"))
            {
                sSql += "    , FILE1 = '" + ht["FILE1"].ToString() + "'";                          //FILE
                sSql += "    , FILE1_NAME = '" + ht["FILE1_NAME"].ToString() + "'" + "\r\n";                          //FILE
            }
            if (ht.ContainsKey("FILE2"))
            {
                sSql += "    , FILE2 = '" + ht["FILE2"].ToString() + "'" + "\r\n";                          //FILE
                sSql += "    , FILE2_NAME = '" + ht["FILE2_NAME"].ToString() + "'" + "\r\n";                          //FILE
            }
            sSql += "    , CONTENT = '" + ht["CONTENT"].ToString() + "'" + "\r\n";                 //FILE2_NAME
            sSql += "    , TYPE = '" + ht["S_TYPE"].ToString() + "'" + "\r\n";                 //TYPE
            sSql += " WHERE NOTICE_ID = " + ht["NOTICE_ID"].ToString() + " " + "\r\n";
            return sSql;
        }

        public string Admin_MemberSearch()
        {
            string sSql = "";
            sSql += " SELECT * FROM ( ";
            sSql += " SELECT ROWNUM AS RNUM ";
            sSql += "         , MEMB_NO ";
            sSql += "         , M_ID ";
            sSql += "         , \"LEVEL\" AS LVL ";
            sSql += "         , AUTH_LEVEL ";
            sSql += "         , STATUS ";
            sSql += "         ,  CryptString.decrypt(PASSWORD, '" + memberKey + "') AS PWD ";
            sSql += "         , \"NAME\" AS M_NAME ";
            sSql += "         , MOBILE ";
            sSql += "         , SUBSTR(NVL(REGDT, ''), 0, 10) AS REGDT ";
            sSql += "         , LAST_LOGIN ";
            sSql += "  FROM E_MEMBER ";
            sSql += " WHERE NVL(DEL_FLAG, 'n') = 'n' ";
            sSql += "  )  ORDER BY RNUM DESC ";
            return sSql;
        }

        public string Admin_MemberSelect(string id)
        {
            string sSql = "";            
            sSql += " SELECT MEMB_NO ";
            sSql += "         , M_ID ";
            sSql += "         , \"LEVEL\" AS LVL ";
            sSql += "         , AUTH_LEVEL ";
            sSql += "         , STATUS ";
            sSql += "         ,  CryptString.decrypt(PASSWORD, '" + memberKey + "') AS PWD ";
            sSql += "         , \"NAME\" AS M_NAME ";
            sSql += "         , MOBILE ";
            sSql += "         , SUBSTR(NVL(REGDT, ''), 0, 10) AS REGDT ";
            sSql += "         , LAST_LOGIN ";
            sSql += "  FROM E_MEMBER ";
            sSql += " WHERE MEMB_NO = "+ id +" ";            
            return sSql;
        }

        public string Admin_MemberUpdate(Hashtable ht)
        {
            string sSql = "";
            sSql += " UPDATE E_MEMBER ";
            sSql += " SET ";
            if (ht.ContainsKey("DEL_FLAG"))
            {
                if (ht["DEL_FLAG"].ToString() == "y")
                {
                    sSql += "       DEL_FLAG = 'y' ";
                    sSql += "     , DELDATE = TO_CHAR(SYSDATE, 'YYYY-MM-DD hh24:mi:ss') ";
                }
                else
                {
                    
                     sSql += "         \"NAME\" = '" + ht["NAME"].ToString() + "' ";
                     if (!string.IsNullOrEmpty(ht["PASSWORD"].ToString())) sSql += "         , PASSWORD = CryptString.encrypt('" + ht["PASSWORD"].ToString() + "', '" + memberKey + "') ";                    
                    sSql += "         , MOBILE = '" + ht["MOBILE1"].ToString() + "-" + ht["MOBILE2"].ToString() + "-" + ht["MOBILE3"].ToString() + "' ";
                    sSql += "         , REGDT = TO_CHAR(SYSDATE, 'YYYY-MM-DD hh24:mi:ss') ";                
                }
            }
            else
            {                
                sSql += "         \"NAME\" = '" + ht["NAME"].ToString() + "' ";
                if (!string.IsNullOrEmpty(ht["PASSWORD"].ToString())) sSql += "         , PASSWORD = CryptString.encrypt('" + ht["PASSWORD"].ToString() + "', '" + memberKey + "') ";
                sSql += "         , MOBILE = '" + ht["MOBILE1"].ToString() + "-" + ht["MOBILE2"].ToString() + "-" + ht["MOBILE3"].ToString() + "' ";
                sSql += "         , REGDT = TO_CHAR(SYSDATE, 'YYYY-MM-DD hh24:mi:ss') ";                
            }
                        
            sSql += " WHERE MEMB_NO = '"+ ht["MEMB_NO"].ToString() +"' ";
            return sSql;
        }

        //public string Admin_MemberAdd(Hashtable ht)
        //{
        //    string sSql = "";
        //    sSql += " INSERT INTO E_MEMBER VALUES ( ";
        //    sSql += " (SELECT NVL(MAX(MEMB_NO), 0) + 1 FROM MEMBER) ";
        //    sSql += " , '" + ht["M_ID"].ToString() + "' ";
        //    sSql += " , 50 ";   //level
        //    sSql += " , '' ";   // auth_level
        //    sSql += " , '' ";   // status
        //    sSql += " , CryptString.encrypt('" + ht["PASSWORD"].ToString() + "', '" + memberKey + "') ";
        //    sSql += " , '" + ht["NAME"].ToString() + "' ";
        //    sSql += " , '" + ht["MOBILE1"].ToString() + "-" + ht["MOBILE2"].ToString() + "-" + ht["MOBILE3"].ToString() + "' ";
        //    sSql += " , TO_CHAR(SYSDATE, 'YYYY-MM-DD hh24:mi:ss') ";
        //    sSql += " , '' ";   // last_login
        //    sSql += " , '' ";   // last_loginIP
        //    sSql += " , 0 ";   // cnt_login
        //    sSql += " , '' ";   // deldate
        //    sSql += " , '' ";   // drop_comment
        //    sSql += " , '' ";   // del_flag
        //    sSql += " ) ";
        //    return sSql;
        //}

        //public string Admin_Schedule(string SearchText, int pageIndex)
        //{
        //    string sSql = "";
        //    if (pageIndex == 0) pageIndex = 1;


        //    sSql += " SELECT *		";
        //    sSql += "   FROM (SELECT FLOOR ( (ROWNUM - 1) / 20 + 1) AS PAGE,	";
        //    sSql += "                COUNT (*) OVER () AS TOTCNT,	";
        //    sSql += "                X.*	";
        //    sSql += "           FROM (SELECT *	";
        //    sSql += "                   FROM (  SELECT ROW_NUMBER () OVER (ORDER BY  A.USE_YN , A.REGDT) NUM, A.*	";
        //    sSql += "                             FROM SCHEDULE A	";
        //    if (SearchText != "")
        //    {
        //        sSql += "                               WHERE ( A.TITLE LIKE '%" + SearchText + "%' OR A.AREA LIKE '%" + SearchText + "%') ";
        //    }
        //    sSql += "                         ORDER BY NUM DESC)) X) XX	";
        //    sSql += "  WHERE XX.PAGE = " + pageIndex + " ";

        //    return sSql;
        //}
        public string Admin_Login(string id, string pwd)
        {
            string sSql = "";
            sSql += " SELECT MEMB_NO FROM E_MEMBER ";
            sSql += " WHERE M_ID = '" + id + "' ";
            sSql += " AND CryptString.decrypt(PASSWORD, '" + memberKey + "') = '" + pwd + "' ";
            return sSql;
        }

        //public string Admin_ScheduleView()
        //{
        //    string sSql = "";
        //    sSql += " SELECT * FROM SCHEDULE_FILE  WHERE ROWNUM = 1 ";
        //    return sSql;
        //}

        //public string Admin_ScheduleFileDel()
        //{
        //    string sSql = "";
        //    sSql += " DELETE SCHEDULE_FILE  ";
        //    return sSql;
        //}
        //public string Admin_ScheduleFileInsert(Hashtable ht)
        //{
        //    string sSql = "";
        //    sSql += " INSERT INTO SCHEDULE_FILE VALUES(  ";
        //    sSql += " 1   ";
        //    sSql += ",'" + ht["FILE"] + "'";
        //    sSql += ",'" + ht["FILE_NAME"] + "'";
        //    sSql += ",TO_CHAR(SYSDATE, 'YYYY-MM-DD hh24:mi:ss')";
        //    sSql += " )";
        //    return sSql;
        //}

        //public string Admin_Login_Update(string id, string userIP)
        //{
        //    string sSql = "";
        //    sSql += " UPDATE E_MEMBER ";
        //    sSql += " SET LAST_LOGIN = TO_CHAR(SYSDATE, 'YYYY-MM-DD hh24:mi:ss') ";
        //    sSql += " AND LAST_LOGIN_IP = '"+ userIP +"' ";
        //    sSql += " WHERE MEMB_NO = '" + id + "' ";
        //    return sSql;
        //}

        //public string Admin_Member_Check(string id)
        //{
        //    string sSql = "";
        //    sSql += " SELECT M_ID FROM E_MEMBER ";
        //    sSql += " WHERE M_ID = '" + id + "' ";            
        //    return sSql;
        //}

        /* 채용공고 sql */
        //public string Search_Recruitment(string Opt, string Type, string SearchText, int pageIndex)
        //{
        //    string sSql = "";
        //    if (pageIndex == 0) pageIndex = 1;

        //    sSql += " SELECT * ";
        //    sSql += "  FROM ( ";
        //    sSql += "              SELECT ROWNUM AS RNUM ";
        //    sSql += "                     , FLOOR((ROWNUM-1) /10 + 1) AS PAGE ";
        //    sSql += "                     , COUNT(*) OVER () AS TOTCNT ";
        //    sSql += "                     , X.* ";
        //    sSql += "             FROM ( ";
        //    sSql += "                         SELECT * FROM             ";
        //    sSql += "                         ( SELECT ROW_NUMBER() OVER(ORDER BY A.REGDT) NUM, A.*  ";
        //    sSql += "                             FROM RECRUITMENT A  ";
        //    sSql += "                            WHERE A.USE_YN = 'y'  ";
        //    sSql += "                               AND A.RECRUITMENT_YN = 'y' ";
        //    if (!string.IsNullOrEmpty(SearchText)) sSql += "                               AND A." + Opt + " LIKE '%" + SearchText + "%' ";
        //    if (!string.IsNullOrEmpty(Type)) sSql += "                               AND A.TYPE = '" + Type + "' ";
        //    sSql += "                           ORDER BY A.REGDT DESC )  ";
        //    sSql += "                           UNION ALL ";
        //    sSql += "                         SELECT * FROM     ";
        //    sSql += "                           ( SELECT ROW_NUMBER() OVER(ORDER BY A.REGDT) NUM, A.*  ";
        //    sSql += "                              FROM RECRUITMENT A  ";
        //    sSql += "                             WHERE A.USE_YN = 'y'  ";
        //    sSql += "                                AND A.RECRUITMENT_YN = 'n'  ";
        //    if (!string.IsNullOrEmpty(SearchText)) sSql += "                               AND A." + Opt + " LIKE '%" + SearchText + "%' ";
        //    if (!string.IsNullOrEmpty(Type)) sSql += "                               AND A.TYPE = '" + Type + "' ";
        //    sSql += "                             ORDER BY A.REGDT DESC )   ";
        //    sSql += "                      ) X  ";
        //    sSql += "           ) XX ";
        //    sSql += "  WHERE XX.PAGE = " + pageIndex + " ";

        //    return sSql;
        //}

        //public string Search_RecruitmentView(string noticeID)
        //{
        //    string sSql = "";
        //    sSql += " SELECT 'VIEW' AS FLAG, V.* FROM RECRUITMENT V ";
        //    sSql += " WHERE V.RECRUITMENT_ID = " + noticeID + " ";
        //    sSql += " UNION ALL ";
        //    sSql += " SELECT 'PREV' AS FLAG, P.* FROM RECRUITMENT P  ";
        //    sSql += " WHERE P.RECRUITMENT_ID  = (SELECT MAX(RECRUITMENT_ID) FROM RECRUITMENT WHERE RECRUITMENT_ID < " + noticeID + ") ";
        //    sSql += " UNION ALL ";
        //    sSql += " SELECT 'NEXT' AS FLAG, N.* FROM RECRUITMENT N  ";
        //    sSql += " WHERE N.RECRUITMENT_ID  = (SELECT MIN(RECRUITMENT_ID) FROM RECRUITMENT WHERE RECRUITMENT_ID > " + noticeID + ") ";
        //    return sSql;
        //}

        //public string Admin_Recruitment(string Opt, string SearchText, int pageIndex)
        //{
        //    string sSql = "";
        //    if (pageIndex == 0) pageIndex = 1;

        //    sSql += " SELECT * ";
        //    sSql += "  FROM ( ";
        //    sSql += "              SELECT ROWNUM AS RNUM ";
        //    sSql += "                     , FLOOR((ROWNUM-1) /10 + 1) AS PAGE ";
        //    sSql += "                     , COUNT(*) OVER () AS TOTCNT ";
        //    sSql += "                     , X.* ";
        //    sSql += "             FROM ( ";
        //    sSql += "                         SELECT * FROM             ";
        //    sSql += "                         ( SELECT ROW_NUMBER() OVER(ORDER BY A.REGDT) NUM, A.*  ";
        //    sSql += "                             FROM RECRUITMENT A  ";
        //    sSql += "                           WHERE A.RECRUITMENT_YN = 'y' ";
        //    if (Opt == "ALL")
        //    {
        //        sSql += "                               AND ( A.TITLE LIKE '%" + SearchText + "%' OR A.CONTENT LIKE '%" + SearchText + "%') ";
        //    }
        //    else
        //    {
        //        if (!string.IsNullOrEmpty(SearchText)) sSql += "                               AND A." + Opt + " LIKE '%" + SearchText + "%' ";
        //    }
        //    sSql += "                           ORDER BY A.REGDT DESC )  ";
        //    sSql += "                           UNION ALL ";
        //    sSql += "                         SELECT * FROM     ";
        //    sSql += "                           ( SELECT ROW_NUMBER() OVER(ORDER BY A.REGDT) NUM, A.*  ";
        //    sSql += "                              FROM RECRUITMENT A  ";
        //    sSql += "                                WHERE A.RECRUITMENT_YN = 'n'  ";
        //    if (Opt == "ALL")
        //    {
        //        sSql += "                               AND ( A.TITLE LIKE '%" + SearchText + "%' OR A.CONTENT LIKE '%" + SearchText + "%') ";
        //    }
        //    else
        //    {
        //        if (!string.IsNullOrEmpty(SearchText)) sSql += "                               AND A." + Opt + " LIKE '%" + SearchText + "%' ";
        //    }
        //    sSql += "                             ORDER BY A.REGDT DESC )   ";
        //    sSql += "                      ) X  ";
        //    sSql += "           ) XX ";
        //    sSql += "  WHERE XX.PAGE = " + pageIndex + " ";

        //    return sSql;
        //}

        //public string Admin_RecruitmentView(string RecruitmentID)
        //{
        //    string sSql = "";
        //    sSql += " SELECT 'VIEW' AS FLAG, V.* FROM RECRUITMENT V ";
        //    sSql += " WHERE V.RECRUITMENT_ID = " + RecruitmentID + " ";
        //    return sSql;
        //}

        //public string Admin_RecruitmentDel(string RecruitmentID)
        //{
        //    string sSql = "";
        //    sSql += " DELETE FROM RECRUITMENT ";
        //    sSql += " WHERE RECRUITMENT_ID = " + RecruitmentID + " ";
        //    return sSql;
        //}

        //public string Admin_RecruitmentAdd(Hashtable ht)
        //{
        //    string sSql = "";
        //    sSql += " INSERT INTO RECRUITMENT VALUES ( ";
        //    sSql += "  (SELECT NVL(MAX(RECRUITMENT_ID), 0) + 1 FROM RECRUITMENT) ";   // RECRUITMENT_ID
        //    sSql += " , '" + ht["TITLE"].ToString() + "'";                            //TITLE
        //    sSql += " , 0";                                                             //CNT
        //    sSql += " , '관리자'";                                                     //WRITER
        //    sSql += " , '" + ht["USE_YN"].ToString() + "'";                       //USE_YN
        //    sSql += " , '" + ht["RECRUITMENT_YN"].ToString() + "'";                  //RECRUITMENT_YN
        //    sSql += " , TO_CHAR(SYSDATE, 'YYYY-MM-DD hh24:mi:ss')";   //REGDT
        //    sSql += " , '' ";                                                           //EDITDT
        //    sSql += " , '" + ht["FILE"].ToString() + "'";                          //FILE
        //    sSql += " , '" + ht["FILE_NAME"].ToString() + "'";                 //FILE_NAME
        //    sSql += " , '" + ht["FILE1"].ToString() + "'";                          //FILE1
        //    sSql += " , '" + ht["FILE1_NAME"].ToString() + "'";                 //FILE1_NAME
        //    sSql += " , '" + ht["FILE2"].ToString() + "'";                          //FILE2
        //    sSql += " , '" + ht["FILE2_NAME"].ToString() + "'";                 //FILE2_NAME
        //    sSql += " , '" + ht["CONTENT"].ToString() + "'";                 //CONTENT
        //    sSql += " ) ";
        //    return sSql;
        //}

        //public string Admin_RecruitmentModify(Hashtable ht)
        //{
        //    string sSql = "";
        //    sSql += " UPDATE RECRUITMENT SET " + "\r\n";
        //    sSql += "     TITLE = '" + ht["TITLE"].ToString() + "' " + "\r\n";
        //    sSql += "    , USE_YN = '" + ht["USE_YN"].ToString() + "' " + "\r\n";
        //    sSql += "    , RECRUITMENT_YN = '" + ht["RECRUITMENT_YN"].ToString() + "' " + "\r\n";
        //    sSql += "    , EDITDT = TO_CHAR(SYSDATE, 'YYYY-MM-DD hh24:mi:ss') " + "\r\n";
        //    if (ht.ContainsKey("FILE"))
        //    {
        //        sSql += "    , \"FILE\" = '" + ht["FILE"].ToString() + "'" + "\r\n";                          //FILE
        //        sSql += "    , FILE_NAME = '" + ht["FILE_NAME"].ToString() + "'" + "\r\n";                          //FILE
        //    }
        //    if (ht.ContainsKey("FILE1"))
        //    {
        //        sSql += "    , FILE1 = '" + ht["FILE1"].ToString() + "'";                          //FILE
        //        sSql += "    , FILE1_NAME = '" + ht["FILE1_NAME"].ToString() + "'" + "\r\n";                          //FILE
        //    }
        //    if (ht.ContainsKey("FILE2"))
        //    {
        //        sSql += "    , FILE2 = '" + ht["FILE2"].ToString() + "'" + "\r\n";                          //FILE
        //        sSql += "    , FILE2_NAME = '" + ht["FILE2_NAME"].ToString() + "'" + "\r\n";                          //FILE
        //    }
        //    sSql += "    , CONTENT = '" + ht["CONTENT"].ToString() + "'" + "\r\n";                 //FILE2_NAME
        //    sSql += " WHERE RECRUITMENT_ID = " + ht["RECRUITMENT_ID"].ToString() + " " + "\r\n";
        //    return sSql;
        //}

        //public string Admin_ScheduleSelect()
        //{
        //    string sSql = "";

        //    sSql += " SELECT NVL(MAX(SCHEDULE_ID), 0) + 1 AS SCH_ID FROM SCH_INFO ";   // SCH_INFO_ID

        //    return sSql;
        //}

     
        //public string Admin_ScheduleDetailSelect()
        //{
        //    string sSql = "";

        //    sSql += " SELECT NVL(MAX(SCH_INFO_ID), 0) AS SCH_INFO_ID FROM SCH_DETAIL ";   // SCH_INFO_ID

        //    return sSql;
        //}
        //public string Admin_ScheduleUpdateYn(Hashtable ht)
        //{
        //    string sSql = "";


        //    sSql += " UPDATE SCHEDULE SET USE_YN = 'n' WHERE AREA = '" + ht["AREA"] + "'";
        //    return sSql;
        //}

        //public string Admin_SchInsert(Hashtable ht)
        //{
        //    string sSql = "";

        //    sSql += " INSERT INTO SCHEDULE VALUES ( ";
        //    sSql += "  (SELECT NVL(MAX(SCHEDULE_ID), 0) + 1 FROM SCHEDULE) ";   // SCH_INFO_ID
        //    sSql += " ,'" + ht["TITLE"] + "'";   // SCH_INFO_ID
        //    sSql += " ,'" + ht["AREA"] + "'";   // SCH_INFO_ID
        //    sSql += " ,'" + ht["EXCEL_FILE"] + "'";   // SCH_INFO_ID
        //    sSql += " ,'" + ht["EXCEL_FILE_NAME"] + "'";   // SCH_INFO_ID
        //    sSql += " ,'y'";   // SCH_INFO_ID
        //    sSql += " ,''";   // SCH_INFO_ID
        //    sSql += " , TO_CHAR(SYSDATE, 'YYYY-MM-DD hh24:mi:ss')";   // SCH_INFO_ID
        //    sSql += " ) ";
        //    return sSql;
        //}

        //public string Admin_SchUpdate(Hashtable ht)
        //{
        //    string sSql = "";

        //    sSql += " UPDATE SCHEDULE SET ";
        //    sSql += " ,TITLE = '" + ht["TITLE"] + "'";   // SCH_INFO_ID
        //    sSql += " ,AREA = '" + ht["AREA"] + "'";   // SCH_INFO_ID
        //    sSql += " ,EXCEL_FILE = '" + ht["EXCEL_FILE"] + "'";   // SCH_INFO_ID
        //    sSql += " ,EXCEL_FILE_NAME = '" + ht["EXCEL_FILE_NAME"] + "'";   // SCH_INFO_ID
        //    sSql += " WHERE SCHEDULE_ID = '" + ht["SCHEDULE_ID"] + "'";
        //    return sSql;
        //}

        //public string Admin_ScheduleInsert(DataRow dr, string sch_id)
        //{
        //    string sSql = "";

        //    sSql += " INSERT INTO SCH_INFO VALUES ( ";
        //    sSql += "  (SELECT NVL(MAX(SCH_INFO_ID), 0) + 1 FROM SCH_INFO) ";   
        //    sSql += " ,'" + sch_id + "'";   
        //    sSql += " ,'" + dr[0] + "'";   
        //    sSql += " ,'" + dr[1] + "'";   
        //    sSql += " ,'" + dr[2] + "'";   
        //    sSql += " ,'" + dr[3] + "'";   
        //    sSql += " ,'" + dr[4] + "'";   
        //    sSql += " ,'" + dr[5] + "'";   
        //    sSql += " ) ";
        //    return sSql;
        //}
        //public string Admin_ScheduleDetailInsert(DataRow dr, string sch_id, int sch_detail_id)
        //{
        //    string sSql = "";

        //    sSql += " INSERT INTO SCH_DETAIL VALUES ( ";
        //    sSql += "  (SELECT NVL(MAX(SCH_DETAIL_ID), 0) + 1 FROM SCH_DETAIL) ";   // SCH_DETAIL_ID
        //    sSql += " ,'" + sch_detail_id + "'";   // SCH_INFO_ID
        //    sSql += " ,'" + sch_id + "'";   // SCHEDULE_ID
        //    sSql += " ,'" + dr[0] + "'";   // VSL
        //    sSql += " ,'" + dr[1] + "'";   // VOY
        //    sSql += " ,'" + dr[2] + "'";   // DOC_CLOSE_YMD
        //    sSql += " ,'" + dr[3] + "'";   // CARGO_CLOSE_YMD
        //    sSql += " ,'" + dr[4] + "'";   // ETD
        //    sSql += " ,'" + dr[5] + "'";   // TS_YMD
        //    sSql += " ,'" + dr[6] + "'";   // ETA
        //    sSql += " ,'" + dr[7] + "'";   // LINE_CD
        //    sSql += " ) ";
        //    return sSql;
        //}
        //public string ScheduleHeader(string sch_id)
        //{
        //    string sSql = "";
        //    sSql += " SELECT * FROM SCHEDULE ";
        //    sSql += " WHERE AREA = '" + sch_id + "'";
        //    sSql += " AND USE_YN = 'y'";

        //    return sSql;
        //}

        //public string Admin_ScheduleDelete(string sch_id)
        //{
        //    string sSql = "";
        //    sSql += " BEGIN ";
        //    sSql += " DELETE SCHEDULE WHERE SCHEDULE_ID = '" + sch_id + "'; ";
        //    sSql += " DELETE SCH_INFO WHERE SCHEDULE_ID = '" + sch_id + "'; ";
        //    sSql += " DELETE SCH_DETAIL WHERE SCHEDULE_ID = '" + sch_id + "'; ";
        //    sSql += " END ; ";

        //    return sSql;
        //}

        //public string Admin_ScheduleHeader(string sch_id)
        //{
        //    string sSql = "";
        //    sSql += " SELECT * FROM SCHEDULE ";
        //    sSql += " WHERE SCHEDULE_ID = '" + sch_id + "'";

        //    return sSql;
        //}
        //public string Admin_ScheduleMaster(string sch_id)
        //{
        //    string sSql = "";
        //    sSql += " SELECT SCH_INFO_ID";
        //    sSql += "      , SCHEDULE_ID ";
        //    sSql += "      , REPLACE(MANAGER , CHR(10) , '<br>') AS MANAGER ";
        //    sSql += "      , REPLACE(EMAIL , CHR(10) , '<br>') AS EMAIL ";
        //    sSql += "      , REPLACE(PORT , CHR(10) , '<br>') AS PORT ";
        //    sSql += "      , REPLACE(TRANSIT , CHR(10) , '<br>') AS TRANSIT ";
        //    sSql += "      , REPLACE(ETC , CHR(10) , '<br>') AS ETC ";
        //    sSql += "      , REPLACE(CARRY_IN , CHR(10) , '<br>') AS CARRY_IN";
        //    sSql += " FROM SCH_INFO ";
        //    sSql += " WHERE SCHEDULE_ID = '" + sch_id + "'";
        //    sSql += " ORDER BY SCH_INFO_ID ";
        //    return sSql;
        //}
        //public string Admin_ScheduleDetail(string sch_id)
        //{
        //    string sSql = "";
        //    sSql += " SELECT * FROM SCH_DETAIL ";
        //    sSql += " WHERE SCHEDULE_ID = '" + sch_id + "'";
        //    sSql += " ORDER BY SCH_INFO_ID , SCH_DETAIL_ID";
        //    return sSql;
        //}
    }
}