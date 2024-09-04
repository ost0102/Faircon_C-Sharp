using SSGM_ELVISPRIME_COMMON.Query.Common;
using SSGM_ELVISPRIME_COMMON.Query.Admin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using SSGM_ELVISPRIME_COMMON.YJIT_Utils;
using SSGM_ELVISPRIME_DATA;
using Newtonsoft.Json.Linq;
using System.Collections;

namespace SSGM_ELVISPRIME_COMMON.Controllers
{
    public class Con_Admin
    {
        Encryption String_Encrypt = new Encryption(); //암호화
        Common comm = new Common(); //일반 함수 
        Admin_Query AQ = new Admin_Query();             //어드민 객체
        Notice_Query NQ = new Notice_Query();           //공지사항 객체
        Member_Query MQ = new Member_Query();           //관리자 관리 객체
        Port_Query PQ = new Port_Query();           //포트 객체

        //전역변수 선언
        string rtnJson = "";
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        DataTable Resultdt = new DataTable();

        /*************************관리자 페이지*******************************/
        public string Con_adminLogin(string id, string pwd, string memberkey)
        {
            DataHelper.ConnectionString_Select = "SSGM";

            try
            {
                Resultdt = DataHelper.ExecuteDataTable(AQ.fnAdminLogin_Query(id, pwd, memberkey), CommandType.Text);

                if (Resultdt.Rows.Count == 0)
                {
                    rtnJson = comm.MakeJson("N", "");
                }
                else
                {
                    rtnJson = comm.MakeJson("Y", "", Resultdt);
                }

                return rtnJson;

            }
            catch (Exception e)
            {
                //만약 오류가 발생 하였을 경우
                rtnJson = comm.MakeJson("E", e.Message);
                return rtnJson;
            }
        }

        /// <summary>
        /// 파일 로그 (공통)
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public string Con_InsertFileLog(DataTable dt)
        {
            int nResult = 0;

            DataHelper.ConnectionString_Select = "SSGM";

            try
            {
                nResult = DataHelper.ExecuteNonQuery(AQ.InsertFileLog_Query(dt.Rows[0]), CommandType.Text);

                if (nResult == 0)
                {
                    rtnJson = comm.MakeJson("N", "Fail");
                }
                else
                {
                    rtnJson = comm.MakeJson("Y", "Success");
                }

                return rtnJson;
            }
            catch (Exception e)
            {
                //만약 오류가 발생 하였을 경우
                rtnJson = comm.MakeJson("E", e.Message);
                rtnJson = String_Encrypt.decryptAES256(rtnJson);
                return rtnJson;
            }
        }

        //공지사항
        #region

        /// <summary>
        /// 공지사항 리스트 조회
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public DataTable Con_NoticeList(object value)
        {
            Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonConvert.SerializeObject(value).ToString());

            DataHelper.ConnectionString_Select = "SSGM";

            try
            {
                Resultdt = DataHelper.ExecuteDataTable(NQ.fnAdminNotice_Query(dictionary), CommandType.Text);

                if (Resultdt.Rows.Count == 0)
                {
                    //rtnJson = comm.MakeJson("N", "");
                    Resultdt = null;
                    return Resultdt;
                }
                else
                {
                    //rtnJson = comm.MakeJson("Y", "", Resultdt);
                    return Resultdt;
                }
            }
            catch (Exception e)
            {
                //만약 오류가 발생 하였을 경우
                //rtnJson = comm.MakeJson("E", e.Message);
                Resultdt = null;
                return Resultdt;
            }
        }

        /// <summary>
        /// 공지사항 데이터 가져오기
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable Con_NoticeView(string id)
        {
            DataHelper.ConnectionString_Select = "SSGM";

            try
            {
                Resultdt = DataHelper.ExecuteDataTable(NQ.AdminNoticeView_Query(id), CommandType.Text);

                if (Resultdt.Rows.Count == 0)
                {
                    //rtnJson = comm.MakeJson("N", "");
                    Resultdt = null;
                    return Resultdt;
                }
                else
                {
                    //rtnJson = comm.MakeJson("Y", "", Resultdt);
                    return Resultdt;
                }
            }
            catch (Exception e)
            {
                //만약 오류가 발생 하였을 경우
                //rtnJson = comm.MakeJson("E", e.Message);
                Resultdt = null;
                return Resultdt;
            }
        }

        /// <summary>
        /// 공지사항 -  글 생성  (Insert)
        /// </summary>
        /// <param name="htParam"></param>
        /// <returns></returns>
        public string Con_NoticeInsert(Hashtable htParam)
        {
            string rtnJson = "";
            int nResult = 0;

            DataHelper.ConnectionString_Select = "SSGM";

            try
            {
                nResult = DataHelper.ExecuteNonQuery(NQ.NoticeInsert_Query(htParam), CommandType.Text);

                if (nResult == 0)
                {
                    rtnJson = comm.MakeJson("N", "insert가 실패 하였습니다.");
                    return rtnJson;
                }

                rtnJson = comm.MakeJson("Y", "Success");
                return rtnJson;
            }
            catch (Exception e)
            {
                //만약 오류가 발생 하였을 경우
                rtnJson = comm.MakeJson("E", e.Message);
                return rtnJson;
            }
        }

        /// <summary>
        /// 공지사항 -  글 수정  (Update)
        /// </summary>
        /// <param name="htParam"></param>
        /// <returns></returns>
        public string Con_NoticeUpdate(Hashtable htParam)
        {
            string rtnJson = "";
            int nResult = 0;

            DataHelper.ConnectionString_Select = "SSGM";

            try
            {
                nResult = DataHelper.ExecuteNonQuery(NQ.NoticeUpdate_Query(htParam), CommandType.Text);

                if (nResult == 0)
                {
                    rtnJson = comm.MakeJson("N", "Update가 실패 하였습니다.");
                    return rtnJson;
                }

                rtnJson = comm.MakeJson("Y", "Success");
                return rtnJson;
            }
            catch (Exception e)
            {
                //만약 오류가 발생 하였을 경우
                rtnJson = comm.MakeJson("E", e.Message);
                return rtnJson;
            }
        }

        /// <summary>
        /// NoticeID로 되어있는 데이터가 있는지 체크하는 로직
        /// </summary>
        /// <param name="strNoticeID"></param>
        /// <returns></returns>
        //public DataTable Con_NoticeFileDel(string strNoticeID)
        //{
        //    DataHelper.ConnectionString_Select = "SSGM";
        //
        //    try
        //    {
        //        Resultdt = DataHelper.ExecuteDataTable(AQ.fnAdminNoticeView_Query(strNoticeID), CommandType.Text);
        //
        //        if (Resultdt.Rows.Count == 0)
        //        {
        //            //rtnJson = comm.MakeJson("N", "");
        //            Resultdt = null;
        //            return Resultdt;
        //        }
        //        else
        //        {
        //            //rtnJson = comm.MakeJson("Y", "", Resultdt);
        //            return Resultdt;
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        //만약 오류가 발생 하였을 경우
        //        //rtnJson = comm.MakeJson("E", e.Message);
        //        Resultdt = null;
        //        return Resultdt;
        //    }
        //}

        /// <summary>
        /// NoticeID로 되어있는 데이터가 있으면 삭제
        /// </summary>
        /// <param name="strNoticeID"></param>
        /// <returns></returns>
        public string Con_AdminNoticeDel(string strNoticeID)
        {
            DataHelper.ConnectionString_Select = "SSGM";
            int nResult = 0;

            try
            {
                //DataHelper.ExecuteDataTable(AQ.fnAdminNoticeView_Query(strNoticeID), CommandType.Text);
                nResult = DataHelper.ExecuteNonQuery(NQ.fnAdminNoticeDel_Query(strNoticeID), CommandType.Text);

                if (nResult == 1)
                {
                    rtnJson = comm.MakeJson("N", "Fail");
                }
                else
                {
                    rtnJson = comm.MakeJson("Y", "Success");
                }

                return rtnJson;
            }
            catch (Exception e)
            {
                //만약 오류가 발생 하였을 경우
                rtnJson = comm.MakeJson("E", e.Message);
                return rtnJson;
            }
        }
        #endregion

        //관리자 관리
        #region
        /// <summary>
        /// 관리자 관리 - 검색
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_SearchMember(string strValue)
        {
            string rtnJson = "";
            string strResult = String_Encrypt.decryptAES256(strValue);

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);

            DataHelper.ConnectionString_Select = "SSGM";

            try
            {
                Resultdt = DataHelper.ExecuteDataTable(MQ.SearchMember_Query(dt.Rows[0]), CommandType.Text);
                Resultdt.TableName = "Member";

                if (Resultdt.Rows.Count == 0)
                {
                    rtnJson = comm.MakeJson("N", "");
                }
                else
                {
                    rtnJson = comm.MakeJson("Y", "", Resultdt);
                }

                return rtnJson;

            }
            catch (Exception e)
            {
                //만약 오류가 발생 하였을 경우
                rtnJson = comm.MakeJson("E", e.Message);
                return rtnJson;
            }
        }

        /// <summary>
        /// 관리자 관리 - 관리자 관리 수정 검색
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_SearchMemberModify(string strUserID)
        {
            string rtnJson = "";

            DataHelper.ConnectionString_Select = "SSGM";

            try
            {
                Resultdt = DataHelper.ExecuteDataTable(MQ.SearchMemberModify_Query(strUserID), CommandType.Text);
                Resultdt.TableName = "Member";

                if (Resultdt.Rows.Count == 0)
                {
                    rtnJson = comm.MakeJson("N", "");
                }
                else
                {
                    rtnJson = comm.MakeJson("Y", "", Resultdt);
                }

                return rtnJson;

            }
            catch (Exception e)
            {
                //만약 오류가 발생 하였을 경우
                rtnJson = comm.MakeJson("E", e.Message);
                return rtnJson;
            }
        }

        /// <summary>
        /// 관리자 관리 - 아이디 중복 체크
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_CheckIDMember(string strValue)
        {
            string rtnJson = "";
            string strResult = String_Encrypt.decryptAES256(strValue);

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);

            DataHelper.ConnectionString_Select = "SSGM";

            try
            {
                Resultdt = DataHelper.ExecuteDataTable(MQ.CheckIDMember_Query(dt.Rows[0]), CommandType.Text);
                Resultdt.TableName = "Member";

                if (Resultdt.Rows.Count == 0)
                {
                    rtnJson = comm.MakeJson("Y", "Success");                    
                }
                else
                {
                    rtnJson = comm.MakeJson("N", "Fail");
                }

                return rtnJson;

            }
            catch (Exception e)
            {
                //만약 오류가 발생 하였을 경우
                rtnJson = comm.MakeJson("E", e.Message);
                return rtnJson;
            }
        }        

        /// <summary>
        /// 관리자 관리 - 아이디 등록
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_InsertMember(string strValue)
        {
            string rtnJson = "";
            string strResult = String_Encrypt.decryptAES256(strValue);
            int nResult = 0;

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);

            DataHelper.ConnectionString_Select = "SSGM";

            try
            {
                nResult = DataHelper.ExecuteNonQuery(MQ.InsertMember_Query(dt.Rows[0]), CommandType.Text);

                if (nResult == 1)
                {
                    rtnJson = comm.MakeJson("Y", "Success");
                }
                else
                {
                    rtnJson = comm.MakeJson("N", "Fail");
                }

                return rtnJson;

            }
            catch (Exception e)
            {
                //만약 오류가 발생 하였을 경우
                rtnJson = comm.MakeJson("E", e.Message);
                return rtnJson;
            }
        }

        /// <summary>
        /// 관리자 관리 - 아이디 삭제
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_DeleteMember(string strValue)
        {
            string rtnJson = "";
            string strResult = String_Encrypt.decryptAES256(strValue);
            int nResult = 0;

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);

            DataHelper.ConnectionString_Select = "SSGM";

            try
            {
                nResult = DataHelper.ExecuteNonQuery(MQ.DeleteMember_Query(dt.Rows[0]), CommandType.Text);

                if (nResult == 1)
                {
                    rtnJson = comm.MakeJson("Y", "Success");
                }
                else
                {
                    rtnJson = comm.MakeJson("N", "Fail");
                }

                return rtnJson;

            }
            catch (Exception e)
            {
                //만약 오류가 발생 하였을 경우
                rtnJson = comm.MakeJson("E", e.Message);
                return rtnJson;
            }
        }

        /// <summary>
        /// 관리자 관리 - 아이디 삭제
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_ModifyMember(string strValue)
        {
            string rtnJson = "";
            string strResult = String_Encrypt.decryptAES256(strValue);
            int nResult = 0;

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);

            DataHelper.ConnectionString_Select = "SSGM";

            try
            {
                nResult = DataHelper.ExecuteNonQuery(MQ.ModifyMember_Query(dt.Rows[0]), CommandType.Text);

                if (nResult == 1)
                {
                    rtnJson = comm.MakeJson("Y", "Success");
                }
                else
                {
                    rtnJson = comm.MakeJson("N", "Fail");
                }

                return rtnJson;

            }
            catch (Exception e)
            {
                //만약 오류가 발생 하였을 경우
                rtnJson = comm.MakeJson("E", e.Message);
                return rtnJson;
            }
        }

        /// <summary>
        /// 관리자 관리 - 아이디 삭제
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_ModifyPort(string strValue)
        {
            string rtnJson = "";
            string strResult = String_Encrypt.decryptAES256(strValue);
            int nResult = 0;

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);

            DataHelper.ConnectionString_Select = "SSGM";

            try
            {
                nResult = DataHelper.ExecuteNonQuery(PQ.ModifyPort_Query(dt.Rows[0]), CommandType.Text);

                if (nResult == 1)
                {
                    rtnJson = comm.MakeJson("Y", "Success");
                }
                else
                {
                    rtnJson = comm.MakeJson("N", "Fail");
                }

                return rtnJson;

            }
            catch (Exception e)
            {
                //만약 오류가 발생 하였을 경우
                rtnJson = comm.MakeJson("E", e.Message);
                return rtnJson;
            }
        }
        //관리자 관리
        #region
        /// <summary>
        /// 관리자 관리 - 검색
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_SearchPort(string strValue)
        {
            string rtnJson = "";
            string strResult = String_Encrypt.decryptAES256(strValue);

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);

            DataHelper.ConnectionString_Select = "SSGM";

            try
            {
                Resultdt = DataHelper.ExecuteDataTable(PQ.SearchPort_Query(dt.Rows[0]), CommandType.Text);
                Resultdt.TableName = "PORT";

                if (Resultdt.Rows.Count == 0)
                {
                    rtnJson = comm.MakeJson("N", "");
                }
                else
                {
                    rtnJson = comm.MakeJson("Y", "", Resultdt);
                }

                return rtnJson;

            }
            catch (Exception e)
            {
                //만약 오류가 발생 하였을 경우
                rtnJson = comm.MakeJson("E", e.Message);
                return rtnJson;
            }
        }
        #endregion



        /// <summary>
        /// 관리자 관리 - 아이디 등록
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_InsertPort(string strValue)
        {
            string rtnJson = "";
            string strResult = String_Encrypt.decryptAES256(strValue);
            int nResult = 0;


            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);

            DataHelper.ConnectionString_Select = "SSGM";

            try
            {
                Resultdt = DataHelper.ExecuteDataTable(PQ.CheckPort_Query(dt.Rows[0]), CommandType.Text);

                if(Resultdt.Rows.Count == 0){
                    nResult = DataHelper.ExecuteNonQuery(PQ.InsertPort_Query(dt.Rows[0]), CommandType.Text);

                    if (nResult == 1)
                    {
                        rtnJson = comm.MakeJson("Y", "Success");
                    }
                    else
                    {
                        rtnJson = comm.MakeJson("N", "Fail");
                    }

                }
                else
                {
                    rtnJson = comm.MakeJson("S", "Fail");
                }

                return rtnJson;

            }
            catch (Exception e)
            {
                //만약 오류가 발생 하였을 경우
                rtnJson = comm.MakeJson("E", e.Message);
                return rtnJson;
            }
        }


        /// <summary>
        /// 관리자 관리 - 아이디 삭제
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_DeletePort(string strValue)
        {
            string rtnJson = "";
            string strResult = String_Encrypt.decryptAES256(strValue);
            int nResult = 0;

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);

            DataHelper.ConnectionString_Select = "SSGM";

            try
            {
                nResult = DataHelper.ExecuteNonQuery(PQ.DeletePort_Query(dt.Rows[0]), CommandType.Text);

                if (nResult == 1)
                {
                    rtnJson = comm.MakeJson("Y", "Success");
                }
                else
                {
                    rtnJson = comm.MakeJson("N", "Fail");
                }

                return rtnJson;

            }
            catch (Exception e)
            {
                //만약 오류가 발생 하였을 경우
                rtnJson = comm.MakeJson("E", e.Message);
                return rtnJson;
            }
        }
        /// <summary>
        /// 관리자 관리 - 관리자 관리 수정 검색
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_SearchPortModify(string strUserID)
        {
            string rtnJson = "";

            DataHelper.ConnectionString_Select = "SSGM";

            try
            {
                Resultdt = DataHelper.ExecuteDataTable(PQ.SearchPortModify_Query(strUserID), CommandType.Text);
                Resultdt.TableName = "PORT";

                if (Resultdt.Rows.Count == 0)
                {
                    rtnJson = comm.MakeJson("N", "");
                }
                else
                {
                    rtnJson = comm.MakeJson("Y", "", Resultdt);
                }

                return rtnJson;

            }
            catch (Exception e)
            {
                //만약 오류가 발생 하였을 경우
                rtnJson = comm.MakeJson("E", e.Message);
                return rtnJson;
            }
        }

        /// <summary>
        /// 전체(시.도) 데이터 가져오기
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_SetStartPort(string strValue)
        {
            DataHelper.ConnectionString_Select = "PRIME";

            try
            {
                Resultdt = DataHelper.ExecuteDataTable(PQ.SetStartPort_Query(), CommandType.Text);

                if (Resultdt.Rows.Count == 0)
                {
                    rtnJson = comm.MakeJson("N", "");
                }
                else
                {
                    rtnJson = comm.MakeJson("Y", "", Resultdt);
                }

                return rtnJson;

            }
            catch (Exception e)
            {
                //만약 오류가 발생 하였을 경우
                rtnJson = comm.MakeJson("E", e.Message);
                return rtnJson;
            }
        }

        /// <summary>
        /// 전체(시.도) 데이터 가져오기
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_SetBoundPort(string strValue)
        {
            DataHelper.ConnectionString_Select = "PRIME";

            try
            {
                Resultdt = DataHelper.ExecuteDataTable(PQ.SetBoundPort_Query(), CommandType.Text);

                if (Resultdt.Rows.Count == 0)
                {
                    rtnJson = comm.MakeJson("N", "");
                }
                else
                {
                    rtnJson = comm.MakeJson("Y", "", Resultdt);
                }

                return rtnJson;

            }
            catch (Exception e)
            {
                //만약 오류가 발생 하였을 경우
                rtnJson = comm.MakeJson("E", e.Message);
                return rtnJson;
            }
        }

        /// <summary>
        /// 전체(시.도) 데이터 가져오기
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_SetUsePort(string strValue)
        {
            DataHelper.ConnectionString_Select = "PRIME";

            try
            {
                Resultdt = DataHelper.ExecuteDataTable(PQ.SetUsePort_Query(), CommandType.Text);

                if (Resultdt.Rows.Count == 0)
                {
                    rtnJson = comm.MakeJson("N", "");
                }
                else
                {
                    rtnJson = comm.MakeJson("Y", "", Resultdt);
                }

                return rtnJson;

            }
            catch (Exception e)
            {
                //만약 오류가 발생 하였을 경우
                rtnJson = comm.MakeJson("E", e.Message);
                return rtnJson;
            }
        }

        /// <summary>
        /// 관리자 관리 - 아이디 중복 체크
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_CheckPort(string strValue)
        {
            string rtnJson = "";
            string strResult = String_Encrypt.decryptAES256(strValue);

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);

            DataHelper.ConnectionString_Select = "SSGM";

            try
            {
                Resultdt = DataHelper.ExecuteDataTable(PQ.CheckPort_Query(dt.Rows[0]), CommandType.Text);
                Resultdt.TableName = "Port";

                if (Resultdt.Rows.Count == 0)
                {
                    rtnJson = comm.MakeJson("Y", "Success");
                }
                else
                {
                    rtnJson = comm.MakeJson("N", "Fail");
                }

                return rtnJson;

            }
            catch (Exception e)
            {
                //만약 오류가 발생 하였을 경우
                rtnJson = comm.MakeJson("E", e.Message);
                return rtnJson;
            }
        }
    }





    #endregion


}

