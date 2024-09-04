using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SSGM_ELVISPRIME_COMMON.Query.Admin
{
    public class Notice_Query
    {   
        string sqlstr;

        /*****************************관리자 페이지************************************/                       
        public string fnAdminNotice_Query(Dictionary<string, string> Di)
        {
            string sSql = "";

            string Opt = "";
            string Type = "";
            string SearchText = "";
            int pageIndex = 0;

            if (Di.ContainsKey("Option"))
            {
                Opt = Di["Option"] == null ? "" : Di["Option"].ToString();
            }

            if (Di.ContainsKey("Type"))
            {
                Type = Di["Type"] == null ? "" : Di["Type"].ToString();
            }

            if (Di.ContainsKey("SearchText"))
            {
                SearchText = Di["SearchText"] == null ? "" : Di["SearchText"].ToString();
            }

            if (Di.ContainsKey("Page"))
            {
                pageIndex = Di["Page"] == null ? 0 : Int32.Parse(Di["Page"].ToString());
            }

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
            sSql += "";

            return sSql;
        }

        public string fnAdminNoticeView_Query(string noticeID)
        {
            sqlstr = "";
            
            sqlstr += " SELECT 'VIEW' AS FLAG, V.* FROM NOTICE V ";
            sqlstr += " WHERE V.NOTICE_ID = " + noticeID + " ";
            
            return sqlstr;
        }
        
        public string fnAdminNoticeDel_Query(string noticeID)
        {
            sqlstr = "";

            sqlstr += " DELETE FROM NOTICE ";
            sqlstr += " WHERE NOTICE_ID = " + noticeID + " ";
            
            return sqlstr;
        }

        public string AdminNoticeView_Query(string noticeID)
        {
            sqlstr = "";
            sqlstr += " SELECT 'VIEW' AS FLAG, V.* FROM NOTICE V ";
            sqlstr += " WHERE V.NOTICE_ID = " + noticeID + " ";
            return sqlstr;
        }

        public string NoticeInsert_Query(Hashtable ht)
        {
            sqlstr = "";
            
            sqlstr += " INSERT INTO NOTICE VALUES ( ";
            sqlstr += "  (SELECT NVL(MAX(NOTICE_ID), 0) + 1 FROM NOTICE) ";     //NOTICE_ID
            sqlstr += " , '" + ht["TITLE"].ToString() + "'";                    //TITLE
            sqlstr += " , 0";                                                   //CNT
            sqlstr += " , '" + ht["USR_ID"].ToString() + "'";                   //WRITER
            sqlstr += " , '" + ht["USE_YN"].ToString() + "'";                   //USE_YN
            sqlstr += " , '" + ht["NOTICE_YN"].ToString() + "'";                //NOTICE_YN
            sqlstr += " , TO_CHAR(SYSDATE, 'YYYY-MM-DD')";                      //REGDT
            sqlstr += " , '' ";                                                 //EDITDT
            sqlstr += " , '" + ht["FILE"].ToString() + "'";                     //FILE
            sqlstr += " , '" + ht["FILE_NAME"].ToString() + "'";                //FILE_NAME
            sqlstr += " , '" + ht["FILE1"].ToString() + "'";                    //FILE1
            sqlstr += " , '" + ht["FILE1_NAME"].ToString() + "'";               //FILE1_NAME
            sqlstr += " , '" + ht["FILE2"].ToString() + "'";                    //FILE2
            sqlstr += " , '" + ht["FILE2_NAME"].ToString() + "'";               //FILE2_NAME
            //sqlstr += " , '" + ht["CONTENT"].ToString().Substring(1300,1300) + "'";               //FILE2_NAME


            //TWKIM 20211124 - CLOB 데이터 타입은 한번에 4000byte 이상 데이터가 들어가지 않아서 아래와 같이 쪼개서 넣으면 데이터가 삽입 가능하기 때문에 로직 추가 하였습니다.
            int nConentLength = ht["CONTENT"].ToString().Trim().Length;            
            
            if (nConentLength < 1000)
            {
                sqlstr += " , '" + ht["CONTENT"].ToString() + "'";                  //CONTENT
            }
            else
            {
                int nFor = (nConentLength / 1000) + 1;
                string strContent = "";
            
                for(int i = 1; i <= nFor; i++)
                {
                    if (i==1)
                    {
                        strContent += " TO_CLOB('"+ ht["CONTENT"].ToString().Substring(0,(1000 * i)) + "') ";
                    }
                    else
                    {
                        //마지막
                        if(i == nFor)
                        {
                            strContent += " || TO_CLOB('"+ ht["CONTENT"].ToString().Substring((1000 * (i-1)), (ht["CONTENT"].ToString().Length-1) - (1000 * (i - 1))) + "') ";
                        }
                        else
                        {
                            strContent += " || TO_CLOB('" + ht["CONTENT"].ToString().Substring((1000 * (i - 1)), 1000) + "') ";
                        }
                    }
                }
            
                sqlstr += " , "+ strContent + " ";                  //CONTENT
            }

            sqlstr += " , '" + ht["S_TYPE"].ToString() + "'";                 //TYPE
            sqlstr += " ) ";

            return sqlstr;
        }

        public string NoticeUpdate_Query(Hashtable ht)
        {
            sqlstr = "";

            sqlstr += " UPDATE NOTICE SET " + "\r\n";
            sqlstr += "     TITLE = '" + ht["TITLE"].ToString() + "' " + "\r\n";
            sqlstr += "    , USE_YN = '" + ht["USE_YN"].ToString() + "' " + "\r\n";
            sqlstr += "    , NOTICE_YN = '" + ht["NOTICE_YN"].ToString() + "' " + "\r\n";
            sqlstr += "    , EDITDT = TO_CHAR(SYSDATE, 'YYYY-MM-DD') " + "\r\n";
            if (ht.ContainsKey("FILE"))
            {
                sqlstr += "    , \"FILE\" = '" + ht["FILE"].ToString() + "'" + "\r\n";                          //FILE
                sqlstr += "    , FILE_NAME = '" + ht["FILE_NAME"].ToString() + "'" + "\r\n";                          //FILE
            }
            if (ht.ContainsKey("FILE1"))
            {
                sqlstr += "    , FILE1 = '" + ht["FILE1"].ToString() + "'";                          //FILE
                sqlstr += "    , FILE1_NAME = '" + ht["FILE1_NAME"].ToString() + "'" + "\r\n";                          //FILE
            }
            if (ht.ContainsKey("FILE2"))
            {
                sqlstr += "    , FILE2 = '" + ht["FILE2"].ToString() + "'" + "\r\n";                          //FILE
                sqlstr += "    , FILE2_NAME = '" + ht["FILE2_NAME"].ToString() + "'" + "\r\n";                          //FILE
            }

            //TWKIM 20211124 - CLOB 데이터 타입은 한번에 4000byte 이상 데이터가 들어가지 않아서 아래와 같이 쪼개서 넣으면 데이터가 삽입 가능하기 때문에 로직 추가 하였습니다.
            int nConentLength = ht["CONTENT"].ToString().Trim().Length;

            if (nConentLength < 1000)
            {
                sqlstr += "    , CONTENT = '" + ht["CONTENT"].ToString() + "'" + "\r\n";                 //FILE2_NAME
            }
            else
            {
                int nFor = (nConentLength / 1000) + 1;
                string strContent = "";

                for (int i = 1; i <= nFor; i++)
                {
                    if (i == 1)
                    {
                        strContent += " TO_CLOB('" + ht["CONTENT"].ToString().Substring(0, (1000 * i)) + "') ";
                    }
                    else
                    {
                        //마지막
                        if (i == nFor)
                        {
                            strContent += " || TO_CLOB('" + ht["CONTENT"].ToString().Substring((1000 * (i - 1)), (ht["CONTENT"].ToString().Length - 1) - (1000 * (i - 1))) + "') ";
                        }
                        else
                        {
                            strContent += " || TO_CLOB('" + ht["CONTENT"].ToString().Substring((1000 * (i - 1)), 1000) + "') ";
                        }
                    }
                }

                sqlstr += "    , CONTENT = " + strContent + "" + "\r\n";                 //FILE2_NAME                
            }
            sqlstr += "    , TYPE = '" + ht["S_TYPE"].ToString() + "'" + "\r\n";                 //TYPE
            sqlstr += " WHERE NOTICE_ID = " + ht["NOTICE_ID"].ToString() + " " + "\r\n";

            return sqlstr;
        }
    }
}