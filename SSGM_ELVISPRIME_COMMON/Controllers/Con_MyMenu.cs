using SSGM_ELVISPRIME_COMMON.YJIT_Utils;
using SSGM_ELVISPRIME_DATA;
using SSGM_ELVISPRIME_COMMON.Query.MyMenu;
using System;
using System.Data;
using Newtonsoft.Json;

namespace SSGM_ELVISPRIME_COMMON.Controllers
{
    public class Con_MyMenu
    {
        Encryption String_Encrypt = new Encryption(); //암호화
        Common comm = new Common(); //일반 함수 
        BookingList_Query BQ = new BookingList_Query();
        Myboard_Query MQ = new Myboard_Query();
        Console_Query CQ = new Console_Query();
        BL_Query BLQ = new BL_Query();
        Invoice_Query IQ = new Invoice_Query();

        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        DataTable Resultdt = new DataTable();

        //Booking List
        #region
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
                Resultdt = DataHelper.ExecuteDataTable(BQ.fnGetBookingStatus_Query(), CommandType.Text);
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
                Resultdt = DataHelper.ExecuteDataTable(BQ.fnGetBkgData_Query(dt.Rows[0]), CommandType.Text);
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
                nResult = DataHelper.ExecuteNonQuery(BQ.fnSetCancelStatus_Query(dt.Rows[0]), CommandType.Text);

                if (nResult == 1)
                {
                    rtnJson = comm.MakeJson("Y", "Success");
                }
                else if (nResult == 0)
                {
                    rtnJson = comm.MakeJson("N", "[Fail - Con_fnSetCancelStatus]");
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

        //Myboard
        #region
        /// <summary>
        /// MyBoard 조회
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_fnGetBoardData(string strValue)
        {
            string rtnJson = "";
            string strResult = String_Encrypt.decryptAES256(strValue);

            DataHelper.ConnectionString_Select = "ELVIS";

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);

            try
            {
                
                Resultdt = DataHelper.ExecuteDataTable(MQ.GetBoardList_Query(dt.Rows[0]), CommandType.Text);
                
                Resultdt.TableName = "BOARD";

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
                rtnJson = String_Encrypt.decryptAES256(rtnJson);
                return rtnJson;
            }
        }

        /// <summary>
        /// 부킹 헤더 조회
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_fnGetCfsData(string strValue)
        {
            string rtnJson = "";
            string strResult = String_Encrypt.decryptAES256(strValue);

            DataHelper.ConnectionString_Select = "ELVIS";

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);
            try
            {
                Resultdt = DataHelper.ExecuteDataTable(MQ.fnGetCfsData_Query(dt.Rows[0]), CommandType.Text);
                Resultdt.TableName = "CFS_MST";
                
                if (Resultdt.Rows.Count == 0)
                {
                    rtnJson = comm.MakeJson("N", "");
                }else
                {
                    rtnJson = comm.MakeJson("Y", "", Resultdt);
                }
                return rtnJson;
            }
            catch(Exception e)
            {
                //만약 오류가 발생 하였을 경우
                rtnJson = comm.MakeJson("E", e.Message);
                rtnJson = String_Encrypt.decryptAES256(rtnJson);
                return rtnJson;
            }
        }
        /// <summary>
        /// Tracking 유무 체크
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_isTrackingAvailable(string strValue)
        {
            string rtnJson = "";
            string strResult = String_Encrypt.decryptAES256(strValue);

            DataHelper.ConnectionString_Select = "ELVIS";

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);

            try
            {
                Resultdt = DataHelper.ExecuteDataTable(MQ.isTrackingAvailable_Query(dt.Rows[0]), CommandType.Text);
                Resultdt.TableName = "Available";

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
                rtnJson = String_Encrypt.decryptAES256(rtnJson);
                return rtnJson;
            }
        }

        /// <summary>
        /// Web Print 데이터 가져오기
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_fnGetPrintData(string strValue)
        {
            string rtnJson = "";
            string strResult = String_Encrypt.decryptAES256(strValue);

            DataHelper.ConnectionString_Select = "ELVIS";

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);

            try
            {
                Resultdt = DataHelper.ExecuteDataTable(MQ.fnGetPrintData_Query(dt.Rows[0]), CommandType.Text);
                Resultdt.TableName = "Print";

                if (Resultdt.Rows.Count == 0)
                {
                    rtnJson = comm.MakeJson("N", "", Resultdt);
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
                rtnJson = String_Encrypt.decryptAES256(rtnJson);
                return rtnJson;
            }
        }
        #endregion

        //Console
        #region
        /// <summary>
        /// Console - 창고 업무 조회
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_fnGetConsoleData(string strValue)
        {
            string rtnJson = "";
            string strResult = String_Encrypt.decryptAES256(strValue);

            DataHelper.ConnectionString_Select = "ELVIS";

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);

            try
            {
                Resultdt = DataHelper.ExecuteDataTable(CQ.fnGetConsoleData_Query(dt.Rows[0]), CommandType.Text);
                Resultdt.TableName = "Console";

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
                rtnJson = String_Encrypt.decryptAES256(rtnJson);
                return rtnJson;
            }
        }

        /// <summary>
        /// B/L - 수정요청사항 저장
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_fnSaveBLRequest(string strValue)
        {
            string rtnJson = "";
            string strResult = String_Encrypt.decryptAES256(strValue);
            int nResult = 0;

            DataHelper.ConnectionString_Select = "ELVIS";

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);

            try
            {
                //TWKIM - 무조건 INSERT만 하게 하는 로직인데 백업 해둠
                //nResult = DataHelper.ExecuteNonQuery(BQ.InsertBLRequest_Query(dt.Rows[0]), CommandType.Text);

                //insert 혹은 update를 해야된다. (주석 걸어야됨)
                Resultdt = DataHelper.ExecuteDataTable(BLQ.SearchBLRequest_Query(dt.Rows[0]), CommandType.Text);

                if (Resultdt.Rows.Count == 0)
                {
                    nResult = DataHelper.ExecuteNonQuery(BLQ.InsertBLRequest_Query(dt.Rows[0]), CommandType.Text);
                }
                else
                {
                    nResult = DataHelper.ExecuteNonQuery(BLQ.UpdateBLRequest_Query(dt.Rows[0]), CommandType.Text);
                }

                if (nResult == 1)
                {
                    Resultdt = DataHelper.ExecuteDataTable(BLQ.SearchBLRequest_Query(dt.Rows[0]), CommandType.Text);
                    Resultdt.TableName = "Request";
                    rtnJson = comm.MakeJson("Y", "", Resultdt);
                }
                else
                {
                    rtnJson = comm.MakeJson("N", "저장에 실패 하였습니다.");
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

        /// <summary>
        /// invoice 데이터 가져오기
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_fnSearchInvRequest(string strValue)
        {
            string rtnJson = "";
            string strResult = String_Encrypt.decryptAES256(strValue);

            DataHelper.ConnectionString_Select = "ELVIS";

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);

            try
            {
                Resultdt = DataHelper.ExecuteDataTable(IQ.SearchInvRequest_Query(dt.Rows[0]), CommandType.Text);
                Resultdt.TableName = "Request";

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
                rtnJson = String_Encrypt.decryptAES256(rtnJson);
                return rtnJson;
            }
        }

        /// <summary>
        /// invoice 데이터 저장하기
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_fnSaveInvRequest(string strValue)
        {
            string rtnJson = "";
            string strResult = String_Encrypt.decryptAES256(strValue);
            int nResult = 0;

            DataHelper.ConnectionString_Select = "ELVIS";

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);

            try
            {
                //nResult = DataHelper.ExecuteNonQuery(IQ.InsertInvRequest_Query(dt.Rows[0]), CommandType.Text);

                //insert 혹은 update를 해야된다.
                Resultdt = DataHelper.ExecuteDataTable(IQ.SearchInvRequest_Query(dt.Rows[0]), CommandType.Text);
                if (Resultdt.Rows.Count == 0)
                {
                    nResult = DataHelper.ExecuteNonQuery(IQ.InsertInvRequest_Query(dt.Rows[0]), CommandType.Text);
                }
                else
                {
                    nResult = DataHelper.ExecuteNonQuery(IQ.UpdateInvRequest_Query(dt.Rows[0]), CommandType.Text);
                }

                if (nResult == 1)
                {
                    Resultdt = DataHelper.ExecuteDataTable(IQ.SearchInvRequest_Query(dt.Rows[0]), CommandType.Text);
                    Resultdt.TableName = "Request";
                    rtnJson = comm.MakeJson("Y", "", Resultdt);
                }
                else
                {
                    rtnJson = comm.MakeJson("N", "저장에 실패 하였습니다.");
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

        /// <summary>
        /// invoice 데이터 가져오기
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_fnGetInvPrint(string strValue)
        {
            string rtnJson = "";
            string strResult = String_Encrypt.decryptAES256(strValue);

            DataHelper.ConnectionString_Select = "ELVIS";

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);

            try
            {
                Resultdt = DataHelper.ExecuteDataTable(IQ.GetInvPrint_Query(dt.Rows[0]), CommandType.Text);
                Resultdt.TableName = "Invoice";

                if (Resultdt.Rows.Count == 0)
                {
                    rtnJson = comm.MakeJson("N", "", Resultdt);
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
                rtnJson = String_Encrypt.decryptAES256(rtnJson);
                return rtnJson;
            }
        }
        /// <summary>
        /// HBL 데이터 가져오기
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_fnSearchBLRequest(string strValue)
        {
            string rtnJson = "";
            string strResult = String_Encrypt.decryptAES256(strValue);

            DataHelper.ConnectionString_Select = "ELVIS";

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);

            try
            {
                Resultdt = DataHelper.ExecuteDataTable(BLQ.SearchBLRequest_Query(dt.Rows[0]), CommandType.Text);
                Resultdt.TableName = "Request";

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
                rtnJson = String_Encrypt.decryptAES256(rtnJson);
                return rtnJson;
            }
        }
        /// <summary>
        /// Console - 파일 리스트 가져오기
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_fnLayerSetFileList(string strValue)
        {
            string rtnJson = "";
            string strResult = String_Encrypt.decryptAES256(strValue);

            DataHelper.ConnectionString_Select = "ELVIS";

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);

            try
            {
                Resultdt = DataHelper.ExecuteDataTable(CQ.fnLayerSetFileList_Query(dt.Rows[0]), CommandType.Text);
                Resultdt.TableName = "DOC_MST";

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
                rtnJson = String_Encrypt.decryptAES256(rtnJson);
                return rtnJson;
            }
        }
        #endregion

    }
}
