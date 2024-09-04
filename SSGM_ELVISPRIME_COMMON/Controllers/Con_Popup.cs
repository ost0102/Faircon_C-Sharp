using SSGM_ELVISPRIME_COMMON.YJIT_Utils;
using SSGM_ELVISPRIME_DATA;
using SSGM_ELVISPRIME_COMMON.Query.Popup;
using System;
using System.Data;
using Newtonsoft.Json;

namespace SSGM_ELVISPRIME_COMMON.Controllers
{
    public class Con_Popup
    {
        Encryption String_Encrypt = new Encryption(); //암호화
        Common comm = new Common(); //일반 함수 
        FileList_Query FQ = new FileList_Query();
        Tracking_Query TQ = new Tracking_Query();
        Console_Query CQ = new Console_Query();

        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        DataTable Resultdt = new DataTable();

        //FileList
        #region
        /// <summary>
        /// 문서 파일 리스트 데이터 가져오기
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_fnGetMyBoardFileList(string strValue)
        {
            string rtnJson = "";
            string strResult = String_Encrypt.decryptAES256(strValue);

            DataHelper.ConnectionString_Select = "ELVIS";

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);

            try
            {
                Resultdt = DataHelper.ExecuteDataTable(FQ.GetMyBaordFileList_Query(dt.Rows[0]), CommandType.Text);
                Resultdt.TableName = "FileList";

                rtnJson = comm.MakeJson("Y", "", Resultdt);

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

        //Tracking
        #region
        /// <summary>
        /// Tracking 데이터 가져오기
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_fnGetTracking(string strValue)
        {
            string rtnJson = "";
            string strResult = String_Encrypt.decryptAES256(strValue);

            DataHelper.ConnectionString_Select = "ELVIS";

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);

            try
            {
                Resultdt = DataHelper.ExecuteDataTable(TQ.Query_GetTrackMstList(dt.Rows[0]), CommandType.Text);
                Resultdt.TableName = "Main";
                ds.Tables.Add(Resultdt);

                if (Resultdt.Rows.Count == 0)
                {
                    rtnJson = comm.MakeJson("N", "No Data", Resultdt);
                }
                else
                {
                    Resultdt = DataHelper.ExecuteDataTable(TQ.Query_GetTrackDTL(Resultdt.Rows[0]), CommandType.Text);
                    Resultdt.TableName = "DTL";
                    ds.Tables.Add(Resultdt);

                    if (Resultdt.Rows.Count == 0)
                    {
                        rtnJson = comm.DS_MakeJson("N", "", ds);
                    }
                    else
                    {
                        rtnJson = comm.DS_MakeJson("Y", "", ds);
                    }
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
        /// Tracking 데이터 가져오기
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_fnGetTrackingData(string strValue)
        {
            string rtnJson = "";
            string strResult = String_Encrypt.decryptAES256(strValue);

            DataHelper.ConnectionString_Select = "ELVIS";

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);

            try
            {
                Resultdt = DataHelper.ExecuteDataTable(TQ.Query_GetTrackDTL(dt.Rows[0]), CommandType.Text);
                Resultdt.TableName = "DTL";

                if (Resultdt.Rows.Count == 0)
                {
                    rtnJson = comm.MakeJson("N", "Fail");
                }
                else
                {
                    rtnJson = comm.MakeJson("Y", "Success", Resultdt);
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

        public string Con_fnGetLayerFileList(string strValue)
        {
            string rtnJson = "";
            string strResult = String_Encrypt.decryptAES256(strValue);

            DataHelper.ConnectionString_Select = "ELVIS";

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);

            try
            {
                Resultdt = DataHelper.ExecuteDataTable(TQ.GetLayerFileList_Query(dt.Rows[0]), CommandType.Text);
                Resultdt.TableName = "FileList";

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

        //Console_Detail

        /// <summary>
        /// 분할 입고 조회
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        #region
        public string Con_fnGetLayerConsoleDetail(string strValue)
        {
            string rtnJson = "";
            string strResult = String_Encrypt.decryptAES256(strValue);

            DataHelper.ConnectionString_Select = "ELVIS";


            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);
            try
            {

                Resultdt = DataHelper.ExecuteDataTable(CQ.GetLayerConsole_Query(dt.Rows[0]), CommandType.Text);
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
                return rtnJson;
            }
        }


        //Console_Detail

        /// <summary>
        /// 입고 상세내역 조회
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        #region
        public string Con_fnGetLayerConsoleDetail_MYDATA(string strValue)
        {
            string rtnJson = "";
            string strResult = String_Encrypt.decryptAES256(strValue);

            DataHelper.ConnectionString_Select = "ELVIS";

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);

            try
            {
                Resultdt = DataHelper.ExecuteDataTable(CQ.GetLayerConsole_Query(dt.Rows[0]), CommandType.Text);
                Resultdt.TableName = "Console";

                if (Resultdt.Rows.Count == 0)
                {

                    rtnJson = comm.MakeJson("N", "");
                    return rtnJson;
                }
                else
                {
                    ds.Tables.Add(Resultdt);
                }

                Resultdt = DataHelper.ExecuteDataTable(CQ.GetLayerConsoleDetail_Query(dt.Rows[0]), CommandType.Text);
                Resultdt.TableName = "DeTail";

                if (Resultdt.Rows.Count > 0)
                {
                    ds.Tables.Add(Resultdt);
                }

                rtnJson = comm.DS_MakeJson("Y", "Success", ds);

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


        #endregion
        #region
        /// <summary>
        /// 화물상세 조회
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_fnConsoleDetail(string strValue)
        {
            string rtnJson = "";
            string strResult = String_Encrypt.decryptAES256(strValue);

            DataHelper.ConnectionString_Select = "ELVIS";

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);

            try
            {

                Resultdt = DataHelper.ExecuteDataTable(CQ.GetLayerConsoleDetail_Query(dt.Rows[0]), CommandType.Text);
                Resultdt.TableName = "DeTail";

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
