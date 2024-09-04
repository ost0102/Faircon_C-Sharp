using SSGM_ELVISPRIME_COMMON.Query.Tracking;
using SSGM_ELVISPRIME_COMMON.YJIT_Utils;
using SSGM_ELVISPRIME_DATA;
using Newtonsoft.Json;
using System;
using System.Data;

namespace SSGM_ELVISPRIME_COMMON.Controllers
{
    public class Con_Tracking
    {
        Encryption String_Encrypt = new Encryption(); //암호화
        Common comm = new Common(); //일반 함수 
        Tracking_Query TQ = new Tracking_Query();

        //전역 변수
        DataTable dt = new DataTable();
        DataTable Resultdt = new DataTable();
        DataSet ds = new DataSet();

        /// <summary>
        /// Tracking 리스트 데이터 가져오기
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_GetSeaTrackingList(string strValue)
        {
            string rtnJson = "";
            string strResult = String_Encrypt.decryptAES256(strValue);

            DataHelper.ConnectionString_Select = "ELVIS";

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);

            try
            {
                DataTable Checkdt = DataHelper.ExecuteDataTable(TQ.Query_GetSeaTrackingBLData(dt.Rows[0]), CommandType.Text);

                if (Checkdt != null)
                {
                    if (Checkdt.Rows.Count == 0)
                    {
                        rtnJson = comm.MakeJson("N", "데이터 없음");
                    }
                    else
                    {
                        Resultdt = DataHelper.ExecuteDataTable(TQ.Query_GetSeaTrackingData(Checkdt.Rows[0]), CommandType.Text);
                        Resultdt.TableName = "TrackingList";

                        if (Resultdt.Rows.Count == 0)
                        {
                            rtnJson = comm.MakeJson("N", "데이터 없음");
                        }
                        else
                        {
                            rtnJson = comm.MakeJson("Y", "Success", Resultdt);
                        }
                    }
                }
                else
                {
                    rtnJson = comm.MakeJson("N", "데이터가 없습니다.");
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
        /// Tracking 리스트 데이터 가져오기
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_GetAirTrackingList(string strValue)
        {
            string rtnJson = "";
            string strResult = String_Encrypt.decryptAES256(strValue);

            DataHelper.ConnectionString_Select = "ELVIS";

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);

            try
            {
                Resultdt = DataHelper.ExecuteDataTable(TQ.Query_GetAirTrackingBLData(dt.Rows[0]), CommandType.Text);
                Resultdt.TableName = "Main";
                ds.Tables.Add(Resultdt);

                if (Resultdt.Rows.Count == 0)
                {
                    rtnJson = comm.MakeJson("N", "No Data", Resultdt);
                }
                else
                {
                    Resultdt = DataHelper.ExecuteDataTable(TQ.Query_GetAirTrackingData(Resultdt.Rows[0]), CommandType.Text);
                    Resultdt.TableName = "DTL";
                    ds.Tables.Add(Resultdt);

                    if (Resultdt.Rows.Count == 0)
                    {
                        rtnJson = comm.DS_MakeJson("N", "No Tracking", ds);
                    }
                    else
                    {
                        rtnJson = comm.DS_MakeJson("Y", "Success", ds);
                    }
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
        /// Tracking 마일스톤 디테일 데이터 가져오기
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_fnIsCheckBL(string strValue)
        {
            string rtnJson = "";
            string strResult = String_Encrypt.decryptAES256(strValue);

            DataHelper.ConnectionString_Select = "ELVIS";

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);

            try
            {
                Resultdt = DataHelper.ExecuteDataTable(TQ.Query_fnIsCheckBL(dt.Rows[0]), CommandType.Text);
                Resultdt.TableName = "Check";

                if (Resultdt.Rows.Count == 0)
                {
                    rtnJson = comm.MakeJson("N", "Fail", Resultdt);
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
                rtnJson = String_Encrypt.decryptAES256(rtnJson);
                return rtnJson;
            }
        }

        /// <summary>
        /// Tracking 마일스톤 디테일 데이터 가져오기
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_GetTrackDetail(string strValue)
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

    }
}
