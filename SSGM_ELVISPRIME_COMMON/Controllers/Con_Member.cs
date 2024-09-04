//using SSGM_ELVISPRIME_COMMON.Query.Member;
//using SSGM_ELVISPRIME_COMMON.YJIT_Utils;
//using SSGM_ELVISPRIME_DATA;
//using Newtonsoft.Json;
//using System;
//using System.Data;


//namespace SSGM_ELVISPRIME_COMMON.Controllers
//{
//    public class Con_Member
//    {
//        Encryption String_Encrypt = new Encryption(); //암호화
//        Common comm = new Common(); //일반 함수 
//        Member_Query MQ = new Member_Query();
//        Find_Query FQ = new Find_Query();
//        Modify_Query MDQ = new Modify_Query();

//        //전역 변수
//        DataTable dt = new DataTable();
//        DataTable Resultdt = new DataTable();
//        DataSet ds = new DataSet();

//        //회원가입
//        #region
//        /// <summary>
//        /// 아이디 중복 체크
//        /// </summary>
//        /// <param name="strValue"></param>
//        /// <returns></returns>
//        public string Con_isCheckID(string strValue)
//        {
//            string rtnJson = "";
//            string strResult = String_Encrypt.decryptAES256(strValue);

//            DataHelper.ConnectionString_Select = "ELVIS";

//            //데이터
//            dt = JsonConvert.DeserializeObject<DataTable>(strResult);

//            try
//            {
//                Resultdt = DataHelper.ExecuteDataTable(MQ.isCheckID_Query(dt.Rows[0]), CommandType.Text);
//                Resultdt.TableName = "ID";

//                if (Resultdt.Rows.Count == 0)
//                {
//                    rtnJson = comm.MakeJson("Y", "Success");
//                }
//                else
//                {
//                    rtnJson = comm.MakeJson("N", "Fail", Resultdt);
//                }

//                return rtnJson;
//            }
//            catch (Exception e)
//            {
//                //만약 오류가 발생 하였을 경우
//                rtnJson = comm.MakeJson("E", e.Message);
//                return rtnJson;
//            }
//        }

//        /// <summary>
//        /// 이메일 중복 체크
//        /// </summary>
//        /// <param name="strValue"></param>
//        /// <returns></returns>
//        public string Con_isCheckEmail(string strValue)
//        {
//            string rtnJson = "";
//            string strResult = String_Encrypt.decryptAES256(strValue);

//            DataHelper.ConnectionString_Select = "ELVIS";

//            //데이터
//            dt = JsonConvert.DeserializeObject<DataTable>(strResult);

//            try
//            {
//                Resultdt = DataHelper.ExecuteDataTable(MQ.isCheckEmail_Query(dt.Rows[0]), CommandType.Text);
//                Resultdt.TableName = "Email";

//                if (Resultdt.Rows.Count == 0)
//                {
//                    rtnJson = comm.MakeJson("Y", "Success");
//                }
//                else
//                {
//                    rtnJson = comm.MakeJson("N", "Fail", Resultdt);
//                }

//                return rtnJson;
//            }
//            catch (Exception e)
//            {
//                //만약 오류가 발생 하였을 경우
//                rtnJson = comm.MakeJson("E", e.Message);
//                return rtnJson;
//            }
//        }

//        /// <summary>
//        /// 사업자번호 유무 체크
//        /// </summary>
//        /// <param name="strValue"></param>
//        /// <returns></returns>
//        public string Con_isCheckCRN(string strValue)
//        {
//            string rtnJson = "";
//            string strResult = String_Encrypt.decryptAES256(strValue);

//            DataHelper.ConnectionString_Select = "ELVIS";

//            //데이터
//            dt = JsonConvert.DeserializeObject<DataTable>(strResult);

//            try
//            {
//                Resultdt = DataHelper.ExecuteDataTable(MQ.isCheckCRN_Query(dt.Rows[0]), CommandType.Text);
//                Resultdt.TableName = "CRN";

//                if (Resultdt.Rows.Count == 0)
//                {
//                    rtnJson = comm.MakeJson("N", "Fail");
//                }
//                else
//                {
//                    rtnJson = comm.MakeJson("Y", "Success", Resultdt);
//                }

//                return rtnJson;
//            }
//            catch (Exception e)
//            {
//                //만약 오류가 발생 하였을 경우
//                rtnJson = comm.MakeJson("E", e.Message);
//                return rtnJson;
//            }
//        }

//        /// <summary>
//        /// 사업자 코드 입력 하여 사업자 명 가져오기
//        /// </summary>
//        /// <returns></returns>
//        public string Con_GetOfficeCode()
//        {
//            string rtnJson = "";            
            
//            try
//            {
//                Resultdt = DataHelper.ExecuteDataTable(MQ.GetOfficeCode_Query(), CommandType.Text);
//                if (Resultdt.Rows.Count == 0)
//                {
//                    rtnJson = comm.MakeJson("N", "오피스 정보가 없습니다.");
//                }
//                else if (dt.Rows.Count == 1)
//                {
//                    rtnJson = comm.MakeJson("Y", "Success", Resultdt);
//                }
//                else
//                {
//                    rtnJson = comm.MakeJson("E", "Error");
//                }
//                return rtnJson;
//            }
//            catch (Exception e)
//            {
//                return "";
//            }
//        }

//        /// <summary>
//        /// 회원가입 
//        /// </summary>
//        /// <param name="strValue"></param>
//        /// <returns></returns>
//        public string Con_SetRegister(string strValue)
//        {
//            string rtnJson = "";
//            string strResult = String_Encrypt.decryptAES256(strValue);
//            int nResult = 0;

//            DataHelper.ConnectionString_Select = "ELVIS";

//            //데이터
//            dt = JsonConvert.DeserializeObject<DataTable>(strResult);

//            try
//            {
//                Resultdt = DataHelper.ExecuteDataTable(MQ.isCheckID_Query(dt.Rows[0]), CommandType.Text);
//                if(Resultdt.Rows.Count > 0)
//                {
//                    rtnJson = comm.MakeJson("N", "아이디가 존재합니다.");
//                }

//                Resultdt = DataHelper.ExecuteDataTable(MQ.isCheckEmail_Query(dt.Rows[0]), CommandType.Text);
//                if (Resultdt.Rows.Count > 0)
//                {
//                    rtnJson = comm.MakeJson("N", "이메일이 존재합니다.");
//                }

//                nResult = DataHelper.ExecuteNonQuery(MQ.InsertRegister_Query(dt.Rows[0]), CommandType.Text);
//                if(nResult == 1)
//                {
//                    //회원가입 성공 시 담당자 정보 가지고 오기
//                    DataTable DtRegister_Info = DataHelper.ExecuteDataTable(MQ.GetRegister_Query(dt.Rows[0]), CommandType.Text);
//                    DtRegister_Info.TableName = "Table1";
//                    ds.Tables.Add(DtRegister_Info);

//                    DataTable DtAdmin_Info = DataHelper.ExecuteDataTable(MQ.GetAdminList_Query(dt.Rows[0]), CommandType.Text);
//                    DtAdmin_Info.TableName = "Table";
//                    ds.Tables.Add(DtAdmin_Info);

//                    //데이터가 저장 됐을 경우
//                    if(DtRegister_Info.Rows.Count > 0)
//                    {
//                        rtnJson = comm.DS_MakeJson("Y", "Success", ds);
//                    }
//                    else
//                    {
//                        rtnJson = comm.DS_MakeJson("N", "Success", ds);
//                    }
//                }

//                return rtnJson;
//            }
//            catch (Exception e)
//            {
//                //만약 오류가 발생 하였을 경우
//                rtnJson = comm.MakeJson("E", e.Message);
//                return rtnJson;
//            }
//        }
//        #endregion

//        //아이디 찾기 / 비밀번호 찾기
//        #region
//        /// <summary>
//        /// 아이디 찾기
//        /// </summary>
//        /// <param name="strValue"></param>
//        /// <returns></returns>
//        public string Con_FindID(string strValue)
//        {
//            string rtnJson = "";
//            string strResult = String_Encrypt.decryptAES256(strValue);

//            DataHelper.ConnectionString_Select = "ELVIS";

//            //데이터
//            dt = JsonConvert.DeserializeObject<DataTable>(strResult);

//            try
//            {
//                Resultdt = DataHelper.ExecuteDataTable(FQ.FindID_Query(dt.Rows[0]), CommandType.Text);
//                Resultdt.TableName = "FindID";

//                if (Resultdt.Rows.Count == 0)
//                {
//                    rtnJson = comm.MakeJson("N", "Fail");
//                }
//                else
//                {
//                    rtnJson = comm.MakeJson("Y", "Success", Resultdt);
//                }

//                return rtnJson;
//            }
//            catch (Exception e)
//            {
//                //만약 오류가 발생 하였을 경우
//                rtnJson = comm.MakeJson("E", e.Message);
//                return rtnJson;
//            }
//        }

//        /// <summary>
//        /// 비밀번호 찾기 - 인증번호 발급
//        /// </summary>
//        /// <param name="strValue"></param>
//        /// <returns></returns>
//        public string Con_GetAuthNum(string strValue)
//        {
//            string rtnJson = "";
//            string strResult = String_Encrypt.decryptAES256(strValue);

//            DataHelper.ConnectionString_Select = "ELVIS";

//            //데이터
//            dt = JsonConvert.DeserializeObject<DataTable>(strResult);

//            try
//            {
//                Resultdt = DataHelper.ExecuteDataTable(FQ.GetAuthNum_Query(dt.Rows[0]), CommandType.Text);
//                Resultdt.TableName = "AuthNum";

//                if (Resultdt.Rows.Count == 0)
//                {
//                    rtnJson = comm.MakeJson("N", "Fail");
//                }
//                else
//                {
//                    if (Resultdt.Rows[0]["APV_YN"].ToString() == "N")
//                    {
//                        rtnJson = comm.MakeJson("S", "미승인 계정");
//                    }
//                    else
//                    {
//                        //인증번호 업데이트
//                        string strConfirmKey = comm.fnGetConfirmKey();
//                        DataHelper.ExecuteNonQuery(FQ.SetAuthNum_Query(dt.Rows[0], strConfirmKey), CommandType.Text);

//                        Resultdt.Columns.Add("KEY");
//                        Resultdt.Rows[0]["KEY"] = strConfirmKey;

//                        rtnJson = comm.MakeJson("Y", "Success", Resultdt);
//                    }
//                }

//                return rtnJson;
//            }
//            catch (Exception e)
//            {
//                //만약 오류가 발생 하였을 경우
//                rtnJson = comm.MakeJson("E", e.Message);
//                return rtnJson;
//            }
//        }

//        /// <summary>
//        /// 임시 비밀번호 발급
//        /// </summary>
//        /// <param name="strValue"></param>
//        /// <returns></returns>
//        public string Con_GetNewPW(string strValue)
//        {
//            string rtnJson = "";
//            string strResult = String_Encrypt.decryptAES256(strValue);

//            DataHelper.ConnectionString_Select = "ELVIS";

//            //데이터
//            dt = JsonConvert.DeserializeObject<DataTable>(strResult);

//            try
//            {
//                Resultdt = DataHelper.ExecuteDataTable(FQ.FindPW_Query(dt.Rows[0]), CommandType.Text);
//                Resultdt.TableName = "NewPSWD";

//                if (Resultdt.Rows.Count == 0)
//                {
//                    rtnJson = comm.MakeJson("N", "임시 비밀번호가 틀립니다.");
//                }
//                else
//                {
//                    //성공
//                    string strRandom = comm.fnGetRandomString(8);

//                    string hiddenPwd = strRandom.Substring(0, strRandom.Length / 5);
//                    for (int i = 0; i < strRandom.Length - strRandom.Length / 5; i++)
//                    {
//                        hiddenPwd += "*";
//                    }

//                    DataHelper.ExecuteNonQuery(FQ.SetNewPW_Query(dt.Rows[0], strRandom, hiddenPwd), CommandType.Text);

//                    Resultdt.Columns.Add("PSWD");
//                    Resultdt.Rows[0]["PSWD"] = strRandom;

//                    rtnJson = comm.MakeJson("Y", "Success", Resultdt);
//                }

//                return rtnJson;
//            }
//            catch (Exception e)
//            {
//                //만약 오류가 발생 하였을 경우
//                rtnJson = comm.MakeJson("E", e.Message);
//                return rtnJson;
//            }
//        }
//        #endregion

//        //회원정보수정
//        #region
//        /// <summary>
//        /// 회원정보수정 - 회원정보 가져오기
//        /// </summary>
//        /// <param name="strValue"></param>
//        /// <returns></returns>
//        public string Con_GetModifyInfo(string strValue)
//        {
//            string rtnJson = "";
//            string strResult = String_Encrypt.decryptAES256(strValue);

//            DataHelper.ConnectionString_Select = "ELVIS";

//            //데이터
//            dt = JsonConvert.DeserializeObject<DataTable>(strResult);

//            try
//            {
//                Resultdt = DataHelper.ExecuteDataTable(MDQ.GetModifyInfo_Query(dt.Rows[0]), CommandType.Text);
//                Resultdt.TableName = "UserInfo";

//                if (Resultdt.Rows.Count == 0)
//                {
//                    rtnJson = comm.MakeJson("N", "Fail");
//                }
//                else
//                {
//                    rtnJson = comm.MakeJson("Y", "Success", Resultdt);
//                }

//                return rtnJson;
//            }
//            catch (Exception e)
//            {
//                //만약 오류가 발생 하였을 경우
//                rtnJson = comm.MakeJson("E", e.Message);
//                return rtnJson;
//            }
//        }

//        public string Con_ChkNowPSWD(string strValue)
//        {
//            string rtnJson = "";
//            string strResult = String_Encrypt.decryptAES256(strValue);

//            DataHelper.ConnectionString_Select = "ELVIS";

//            //데이터
//            dt = JsonConvert.DeserializeObject<DataTable>(strResult);

//            try
//            {
//                Resultdt = DataHelper.ExecuteDataTable(MDQ.ChkNowPSWD_Query(dt.Rows[0]), CommandType.Text);
                
//                //비밀번호 체크~
//                if(Resultdt != null)
//                {
//                    if (Resultdt.Rows.Count == 0)
//                    {
//                        rtnJson = comm.MakeJson("E", "아이디와 비밀번호가 없습니다.");
//                    }
//                    else
//                    {
//                        if(Resultdt.Rows[0]["PSWD"].ToString() == YJIT.Utils.StringUtils.Md5Hash((string)dt.Rows[0]["NOW_PSWD"]))
//                        {
//                            rtnJson = comm.MakeJson("Y", "Success");
//                        }
//                        else
//                        {
//                            rtnJson = comm.MakeJson("N", "현재 비밀번호가 아닙니다.");
//                        }
//                    }
//                }
//                else
//                {
//                    rtnJson = comm.MakeJson("E", "아이디와 비밀번호가 없습니다.");
//                }

//                return rtnJson;
//            }
//            catch (Exception e)
//            {
//                //만약 오류가 발생 하였을 경우
//                rtnJson = comm.MakeJson("E", e.Message);
//                return rtnJson;
//            }
//        }

//        /// <summary>
//        /// 회원정보수정 - 회원정보 저장 하기 
//        /// </summary>
//        /// <param name="strValue"></param>
//        /// <returns></returns>
//        public string Con_ModifySave(string strValue)
//        {
//            string rtnJson = "";
//            string strResult = String_Encrypt.decryptAES256(strValue);
//            int nResult = 0;

//            DataHelper.ConnectionString_Select = "ELVIS";

//            //데이터
//            dt = JsonConvert.DeserializeObject<DataTable>(strResult);

//            try
//            {
//                nResult = DataHelper.ExecuteNonQuery(MDQ.ModifySave_Query(dt.Rows[0]), CommandType.Text);

//                if(nResult == 1)
//                {
//                    rtnJson = comm.MakeJson("Y", "Success");
//                }
//                else
//                {
//                    rtnJson = comm.MakeJson("N", "Fail");
//                }

//                return rtnJson;
//            }
//            catch (Exception e)
//            {
//                //만약 오류가 발생 하였을 경우
//                rtnJson = comm.MakeJson("E", e.Message);
//                return rtnJson;
//            }
//        }

//        #endregion

//    }
//}
