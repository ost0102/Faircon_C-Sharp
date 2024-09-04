using SSGM_ELVISPRIME_COMMON.Query.Booking;
using SSGM_ELVISPRIME_COMMON.YJIT_Utils;
using SSGM_ELVISPRIME_DATA;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Collections;

namespace SSGM_ELVISPRIME_COMMON.Controllers
{
    public class Con_Booking
    {
        Encryption String_Encrypt = new Encryption(); //암호화
        Common comm = new Common(); //일반 함수 
        Regist_Query RQ = new Regist_Query();
        Inquiry_Query IQ = new Inquiry_Query();

        //전역변수 선언
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        DataTable Resultdt = new DataTable();

        /// <summary>
        /// 부킹 스케줄 리스트 데이터 가져오기
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_fnGetSEASchedule(string strValue)
        {
            string rtnJson = "";
            string strResult = String_Encrypt.decryptAES256(strValue);

            DataHelper.ConnectionString_Select = "ELVIS";

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);

            try
            {
                Resultdt = DataHelper.ExecuteDataTable(RQ.fnGetSEASchedule_Query(dt.Rows[0]), CommandType.Text);
                Resultdt.TableName = "Schedule";

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
        /// 부킹 스케줄 리스트 데이터 가져오기
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_fnGetAIRSchedule(string strValue)
        {
            string rtnJson = "";
            string strResult = String_Encrypt.decryptAES256(strValue);

            DataHelper.ConnectionString_Select = "ELVIS";

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);

            try
            {
                Resultdt = DataHelper.ExecuteDataTable(RQ.fnGetAIRSchedule_Query(dt.Rows[0]), CommandType.Text);
                Resultdt.TableName = "Schedule";

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
        /// 해운 - 타리프 데이터 가져오기
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_fnGetSeaTariff(string strValue)
        {
            string rtnJson = "";
            string strResult = String_Encrypt.decryptAES256(strValue);

            DataHelper.ConnectionString_Select = "ELVIS";

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);

            try
            {
                Resultdt = DataHelper.ExecuteDataTable(RQ.fnGetSeaTariff_Query(dt.Rows[0]), CommandType.Text);
                Resultdt.TableName = "Tariff";

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
        /// 항공 - 타리프 데이터 가져오기
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_fnGetAirTariff(string strValue)
        {
            string rtnJson = "";
            string strResult = String_Encrypt.decryptAES256(strValue);

            DataHelper.ConnectionString_Select = "ELVIS";

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);

            try
            {
                Resultdt = DataHelper.ExecuteDataTable(RQ.fnGetAirTariff_Query(dt.Rows[0]), CommandType.Text);
                Resultdt.TableName = "Tariff";

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
        /// 부킹 스케줄 리스트 데이터 가져오기
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_SaveBooking(string strValue)
        {            
            string rtnJson = "";
            string strResult = String_Encrypt.decryptAES256(strValue);
            int nResult = 0;

            DataHelper.ConnectionString_Select = "ELVIS";

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);

            try
            {
                if (dt.Rows.Count > 0)
                {
                    string yyyy = System.DateTime.Now.ToString("yyyy");
                    string MM = System.DateTime.Now.ToString("MM");
                    string BKG_NO = "";
                    string SCH_NO = dt.Rows[0]["SCH_NO"].ToString();

                    if (dt.Rows.Count == 1)
                    {
                        if (dt.Rows[0]["BKG_NO"].ToString() == "")
                        {
                            Dictionary<string, object> ValidateParams = new Dictionary<string, object>();
                            ValidateParams.Add("P_JOB_TYPE", "BKG");
                            ValidateParams.Add("P_PRX1", "WBKG");
                            ValidateParams.Add("P_PRX2", yyyy);
                            ValidateParams.Add("P_PRX3", MM);
                            ValidateParams.Add("R_RTNCD", "");
                            ValidateParams.Add("R_RTNMSG", "");
                            Hashtable ht = new Hashtable();
                            ht = DataHelper.CallPRoc("USP_CREATE_AUTO_KEY", ValidateParams);
                            if ("" + ht["R_RTNCD"].ToString() == "E")
                            {
                                rtnJson = comm.MakeJson("E", "Error while Key No Create Processing!" + " " + " Error Msg: " + ht["R_RTNMSG"].ToString());
                            }
                            BKG_NO = ht["R_RTNCD"].ToString();

                            nResult = DataHelper.ExecuteNonQuery(RQ.fnGetBKInsertData_Query(dt.Rows[0], BKG_NO), CommandType.Text);      
                            if (nResult != 1)
                            {
                                rtnJson = comm.MakeJson("N", "[Insert - fnGetBKInsertData_Query]Data Table missing");
                            }
                            nResult = DataHelper.ExecuteNonQuery(RQ.fnGetBKInserPKG_Query(dt.Rows[0], BKG_NO), CommandType.Text);                       
                            if (nResult != 1)
                            {
                                rtnJson = comm.MakeJson("N", "[Insert - fnGetBKInserPKG_Query]Data Table missing");
                            }

                            nResult = DataHelper.ExecuteNonQuery(RQ.fnPRM_BKG_DIM_Query(dt.Rows[0], BKG_NO), CommandType.Text);   
                            nResult = DataHelper.ExecuteNonQuery(RQ.fnPRM_QUOT_MST_Query(dt.Rows[0], BKG_NO), CommandType.Text);

                            //별도 문의일 경우 여기는 패스
                            if(dt.Rows[0]["TARRIF_FLAG"].ToString() == "Y")
                            {
                                nResult = DataHelper.ExecuteNonQuery(RQ.fnPRM_QUOT_DTL_Query(dt.Rows[0], BKG_NO), CommandType.Text);   //부킹 저장
                            }
                            nResult = DataHelper.ExecuteNonQuery(RQ.fnPRM_QUOT_PKG_Query(dt.Rows[0], BKG_NO), CommandType.Text);  
                        }
                        else
                        {
                            //Update
                            BKG_NO = dt.Rows[0]["BKG_NO"].ToString();
                            nResult = DataHelper.ExecuteNonQuery(RQ.fnGetBKUpdateData_Query(dt.Rows[0], BKG_NO), CommandType.Text);
                            if (nResult != 1)
                            {
                                rtnJson = comm.MakeJson("N", "[Update - fnGetBKUpdateData_Query]Data Table missing");
                            }
                            nResult = DataHelper.ExecuteNonQuery(RQ.fnGetBKUpdatePKG_Query(dt.Rows[0], BKG_NO), CommandType.Text);
                            if (nResult != 1)
                            {
                                rtnJson = comm.MakeJson("N", "[Update - fnGetBKUpdatePKG_Query]Data Table missing");
                            }
                        }

                        dt = DataHelper.ExecuteDataTable(RQ.fnGetBKSearchMain_Query(BKG_NO), CommandType.Text);

                        rtnJson = comm.MakeJson("Y", "Success", dt);

                    }
                    else
                    {
                        rtnJson = comm.MakeJson("N", "Data Table missing");
                    }
                }
                else
                {
                    rtnJson = comm.MakeJson("N", "Data Table missing");
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
        /// 부킹 스케줄 리스트 데이터 가져오기
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_SaveBooking2(string strValue)
        {
            string rtnJson = "";
            string strResult = String_Encrypt.decryptAES256(strValue);
            int nResult = 0;

            DataHelper.ConnectionString_Select = "ELVIS";

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);

            try
            {
                if (dt.Rows.Count > 0)
                {
                    string yyyy = System.DateTime.Now.ToString("yyyy");
                    string MM = System.DateTime.Now.ToString("MM");
                    string BKG_NO = "";
                    string SCH_NO = dt.Rows[0]["SCH_NO"].ToString();

                    if(dt.Rows[0]["BKG_STATUS"].ToString() == "INSERT")
                    {
                        nResult = DataHelper.ExecuteNonQuery(RQ.fnGetBKInsertData_Query(dt.Rows[0], dt.Rows[0]["BKG_NO"].ToString()), CommandType.Text);
                        if (nResult != 1)
                        {
                            rtnJson = comm.MakeJson("N", "[Insert - fnGetBKInsertData_Query]Data Table missing");
                        }
                        nResult = DataHelper.ExecuteNonQuery(RQ.fnGetBKInserPKG_Query(dt.Rows[0], dt.Rows[0]["BKG_NO"].ToString()), CommandType.Text);
                        if (nResult != 1)
                        {
                            rtnJson = comm.MakeJson("N", "[Insert - fnGetBKInserPKG_Query]Data Table missing");
                        }

                        nResult = DataHelper.ExecuteNonQuery(RQ.fnPRM_BKG_DIM_Query(dt.Rows[0], dt.Rows[0]["BKG_NO"].ToString()), CommandType.Text);
                        nResult = DataHelper.ExecuteNonQuery(RQ.fnPRM_QUOT_MST_Query(dt.Rows[0], dt.Rows[0]["BKG_NO"].ToString()), CommandType.Text);

                        if (dt.Rows[0]["TARRIF_FLAG"].ToString() == "Y")
                        {
                            nResult = DataHelper.ExecuteNonQuery(RQ.fnPRM_QUOT_DTL_Query(dt.Rows[0], dt.Rows[0]["BKG_NO"].ToString()), CommandType.Text);   //부킹 저장
                        }
                        
                        nResult = DataHelper.ExecuteNonQuery(RQ.fnPRM_QUOT_PKG_Query(dt.Rows[0], dt.Rows[0]["BKG_NO"].ToString()), CommandType.Text);
                    }
                    else
                    {
                        //Update                        
                        nResult = DataHelper.ExecuteNonQuery(RQ.fnGetBKUpdateData_Query(dt.Rows[0], dt.Rows[0]["BKG_NO"].ToString()), CommandType.Text);
                        if (nResult != 1)
                        {
                            rtnJson = comm.MakeJson("N", "[Update - fnGetBKUpdateData_Query]Data Table missing");
                        }
                        nResult = DataHelper.ExecuteNonQuery(RQ.fnGetBKUpdatePKG_Query(dt.Rows[0], dt.Rows[0]["BKG_NO"].ToString()), CommandType.Text);
                        if (nResult != 1)
                        {
                            rtnJson = comm.MakeJson("N", "[Update - fnGetBKUpdatePKG_Query]Data Table missing");
                        }
                    }

                    dt = DataHelper.ExecuteDataTable(RQ.fnGetBKSearchMain_Query(dt.Rows[0]["BKG_NO"].ToString()), CommandType.Text);

                    if(dt.Rows.Count > 0)
                    {
                        rtnJson = comm.MakeJson("Y", "Success", dt);
                    }
                    else
                    {
                        rtnJson = comm.MakeJson("N", "Fail Data Save");
                    }
                }
                else
                {
                    rtnJson = comm.MakeJson("N", "Data Table missing");
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
        /// 부킹 스케줄 + 부킹 번호 데이터 가져오기
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_fnGetModifyBooking(string strValue)
        {
            string rtnJson = "";
            string strResult = String_Encrypt.decryptAES256(strValue);
            
            DataHelper.ConnectionString_Select = "ELVIS";

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);

            try
            {
                Resultdt = DataHelper.ExecuteDataTable(RQ.fnGetBkgSchedule_Query(dt.Rows[0]), CommandType.Text);
                Resultdt.TableName = "Schedule";
                
                if (Resultdt.Rows.Count == 0)
                {
                    
                    rtnJson = comm.MakeJson("N", "");
                    return rtnJson;
                }
                else
                {
                    ds.Tables.Add(Resultdt);
                }

                //부킹 데이터 가져오기
                Resultdt = DataHelper.ExecuteDataTable(RQ.fnGetBookingData_Query(dt.Rows[0]), CommandType.Text);
                Resultdt.TableName = "BK_MST";

                if (Resultdt.Rows.Count == 0)
                {

                    rtnJson = comm.MakeJson("N", "");
                    return rtnJson;
                }
                else
                {
                    ds.Tables.Add(Resultdt);
                }

                Resultdt = DataHelper.ExecuteDataTable(RQ.fnGetDocData_Query(dt.Rows[0]), CommandType.Text);
                Resultdt.TableName = "DOC_MST";

                if (Resultdt.Rows.Count > 0)
                {
                    ds.Tables.Add(Resultdt);
                }

                rtnJson = comm.DS_MakeJson("Y", "Success",ds);
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
        /// 파일 타입 리스트 가져오기
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_fnGetFIleType(string strValue)
        {
            string rtnJson = "";
            string strResult = String_Encrypt.decryptAES256(strValue);

            DataHelper.ConnectionString_Select = "ELVIS";

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);

            try
            {
                Resultdt = DataHelper.ExecuteDataTable(RQ.fnGetFIleType_Query(dt.Rows[0]), CommandType.Text);
                Resultdt.TableName = "FileType";

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
        /// 부킹 조회 데이터 가져오기
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_fnGetBKNO()
        {
            string rtnJson = "";            

            DataHelper.ConnectionString_Select = "ELVIS";            

            try
            {
                string yyyy = System.DateTime.Now.ToString("yyyy");
                string MM = System.DateTime.Now.ToString("MM");

                Dictionary<string, object> ValidateParams = new Dictionary<string, object>();
                ValidateParams.Add("P_JOB_TYPE", "BKG");
                ValidateParams.Add("P_PRX1", "WBKG");
                ValidateParams.Add("P_PRX2", yyyy);
                ValidateParams.Add("P_PRX3", MM);
                ValidateParams.Add("R_RTNCD", "");
                ValidateParams.Add("R_RTNMSG", "");
                Hashtable ht = new Hashtable();                
                ht = DataHelper.CallPRoc("USP_CREATE_AUTO_KEY", ValidateParams);
                if ("" + ht["R_RTNCD"].ToString() == "E")
                {
                    rtnJson = comm.MakeJson("E", "Error while Key No Create Processing!" + " " + " Error Msg: " + ht["R_RTNMSG"].ToString());
                }

                //데이터 테이블 생성
                Resultdt.Columns.Add("BKG_NO");
                Resultdt.Columns.Add("BKG_MSG");

                DataRow dr = Resultdt.NewRow();
                dr["BKG_NO"] = ht["R_RTNCD"].ToString();
                dr["BKG_MSG"] = ht["R_RTNMSG"].ToString();
                Resultdt.Rows.Add(dr);
                Resultdt.TableName = "BKG";

                //예외처리는 나중에 생각
                rtnJson = comm.MakeJson("Y", "Success", Resultdt);

                //if (Resultdt.Rows.Count == 0)
                //{
                //    rtnJson = comm.MakeJson("N", "부킹 채번을 실패하였습니다.");
                //}
                //else
                //{
                //    rtnJson = comm.MakeJson("Y", "Success", Resultdt);
                //}

                return rtnJson;

            }
            catch (Exception e)
            {
                //만약 오류가 발생 하였을 경우
                rtnJson = comm.MakeJson("E", e.Message);
                return rtnJson;
            }
        }

        //////////////////////////////////////부킹 조회////////////////////////////////////////
        /// <summary>
        /// 부킹 상태 플래그 데이터 가져오기
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_fnGetBookingStatus()
        {
            string rtnJson = "";            

            DataHelper.ConnectionString_Select = "ELVIS";

            try
            {
                Resultdt = DataHelper.ExecuteDataTable(IQ.fnGetBookingStatus_Query(), CommandType.Text);
                Resultdt.TableName = "STATUS";

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
        /// 부킹 조회 데이터 가져오기
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_fnGetBkgData(string strValue)
        {
            string rtnJson = "";
            string strResult = String_Encrypt.decryptAES256(strValue);

            DataHelper.ConnectionString_Select = "ELVIS";

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);

            try
            {
                Resultdt = DataHelper.ExecuteDataTable(IQ.fnGetBkgData_Query(dt.Rows[0]), CommandType.Text);
                Resultdt.TableName = "BKG";

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
        /// 부킹 상태 취소로 변경
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_fnSetCancelStatus(string strValue)
        {
            string rtnJson = "";
            string strResult = String_Encrypt.decryptAES256(strValue);
            int nResult = 0;

            DataHelper.ConnectionString_Select = "ELVIS";

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);

            try
            {
                nResult = DataHelper.ExecuteNonQuery(IQ.fnSetCancelStatus_Query(dt.Rows[0]), CommandType.Text);

                if (nResult == 1)
                {
                    rtnJson = comm.MakeJson("Y", "Success");
                }
                else if (nResult == 0){
                    rtnJson = comm.MakeJson("N", "[Update - Con_fnSetCancelStatus]");
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
}
