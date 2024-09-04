using SSGM_ELVISPRIME_COMMON.Query.LogisticsTools;
using SSGM_ELVISPRIME_COMMON.YJIT_Utils;
using SSGM_ELVISPRIME_DATA;
using Newtonsoft.Json;
using System;
using System.Data;

namespace SSGM_ELVISPRIME_COMMON.Controllers
{
    public class Con_LogisticsTools
    {
        Encryption String_Encrypt = new Encryption(); //암호화
        Common comm = new Common(); //일반 함수 
        Tariff_Query TQ = new Tariff_Query(); //안전운임 Query 
        Surcharge_Query SQ = new Surcharge_Query(); //부대비용 Query 
        Vehicle_Query VQ = new Vehicle_Query();

        //전역변수 선언
        string rtnJson = "";
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        DataTable Resultdt = new DataTable();

        /////////////////////////////화물 운임/////////////////////////////////

        /// <summary>
        /// 분기 / 년 데이터 가져오기
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_SetYearQuarter(string strValue)
        {
            DataHelper.ConnectionString_Select = "SSGM";

            try
            {
                Resultdt = DataHelper.ExecuteDataTable(TQ.SetYearQuarter_Query(), CommandType.Text);

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
        public string Con_SetAddrState(string strValue)
        {
            DataHelper.ConnectionString_Select = "SSGM";

            try
            {
                Resultdt = DataHelper.ExecuteDataTable(TQ.SetAddrState_Query(), CommandType.Text);

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
        /// 전체(시.군.구) 데이터 가져오기
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_SetAddrCity(string strValue)
        {
            string rtnJson = "";
            string strResult = String_Encrypt.decryptAES256(strValue);

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);

            DataHelper.ConnectionString_Select = "SSGM";

            try
            {
                Resultdt = DataHelper.ExecuteDataTable(TQ.SetAddrCity_Query(dt.Rows[0]), CommandType.Text);

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
        /// 전체(읍,면,동 - 행정동) 데이터 가져오기
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_SetAddrTownship(string strValue)
        {
            string rtnJson = "";
            string strResult = String_Encrypt.decryptAES256(strValue);

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);

            DataHelper.ConnectionString_Select = "SSGM";

            try
            {
                Resultdt = DataHelper.ExecuteDataTable(TQ.SetAddrTownship_Query(dt.Rows[0]), CommandType.Text);

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
        /// 전체(읍,면,동 - 법정동) 데이터 가져오기
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_SetAddrTownship2(string strValue)
        {
            string rtnJson = "";
            string strResult = String_Encrypt.decryptAES256(strValue);

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);

            DataHelper.ConnectionString_Select = "SSGM";

            try
            {
                Resultdt = DataHelper.ExecuteDataTable(TQ.SetAddrTownship2_Query(dt.Rows[0]), CommandType.Text);

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
        /// 항구 기점 데이터 가져오기
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_fnSetSection(string strValue)
        {
            string rtnJson = "";
            string strResult = String_Encrypt.decryptAES256(strValue);

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);

            DataHelper.ConnectionString_Select = "SSGM";

            try
            {
                Resultdt = DataHelper.ExecuteDataTable(TQ.fnSetSection_Query(dt.Rows[0]), CommandType.Text);

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

        public string Con_SetPremiumRate(string strValue)
        {
            string rtnJson = "";
            string strResult = String_Encrypt.decryptAES256(strValue);

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);

            DataHelper.ConnectionString_Select = "SSGM";

            try
            {
                Resultdt = DataHelper.ExecuteDataTable(TQ.SetPremiumRate_Query(dt.Rows[0]), CommandType.Text);

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
        /// 안전운임 데이터 검색
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_TariffSerach(string strValue)
        {
            string rtnJson = "";
            string strResult = String_Encrypt.decryptAES256(strValue);

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);

            DataHelper.ConnectionString_Select = "SSGM";

            try
            {
                Resultdt = DataHelper.ExecuteDataTable(TQ.TariffSerach_Query(dt.Rows[0]), CommandType.Text);

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

        /////////////////////////////부대 비용/////////////////////////////////

        /// <summary>
        /// 부대비용 포트 검색
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_SetSurchargePort(string strValue)
        {
            string rtnJson = "";
            string strResult = String_Encrypt.decryptAES256(strValue);

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);

            DataHelper.ConnectionString_Select = "SSGM";

            try
            {
                Resultdt = DataHelper.ExecuteDataTable(SQ.SetSurchargePort_Query(dt.Rows[0]), CommandType.Text);

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
        /// 수입 / 수출 국가옵션 가져오기
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_SetSurchargeCountry(string strValue)
        {
            string rtnJson = "";
            string strResult = String_Encrypt.decryptAES256(strValue);

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);

            DataHelper.ConnectionString_Select = "SSGM";

            try
            {
                Resultdt = DataHelper.ExecuteDataTable(SQ.SetSurchargeCountry_Query(dt.Rows[0]), CommandType.Text);

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
        /// 수입 / 수출 국가옵션 가져오기
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_SearchSurcharge(string strValue)
        {
            string rtnJson = "";
            string strResult = String_Encrypt.decryptAES256(strValue);

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);

            DataHelper.ConnectionString_Select = "SSGM";

            try
            {
                DataTable CntrListDt = new DataTable();

                CntrListDt = DataHelper.ExecuteDataTable(SQ.CntrListSurcharge_Query(dt.Rows[0]), CommandType.Text);
                CntrListDt.TableName = "CntrList";
                ds.Tables.Add(CntrListDt);

                Resultdt = DataHelper.ExecuteDataTable(SQ.SearchSurcharge_Query(dt.Rows[0]), CommandType.Text);
                Resultdt.TableName = "SurchargeData";
                ds.Tables.Add(Resultdt);

                //테스트               

                if (Resultdt.Rows.Count == 0)
                {
                    rtnJson = comm.DS_MakeJson("N", "", ds);
                }
                else
                {
                    rtnJson = comm.DS_MakeJson("Y", "", ds);
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

        /////////////////////////////차량 제원/////////////////////////////////
        
        /// <summary>
        /// 차량 제원 구분 Select 박스 데이터 가져오기
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_SetVehicleDiv()
        {
            string rtnJson = "";                        

            DataHelper.ConnectionString_Select = "SSGM";

            try
            {
                Resultdt = DataHelper.ExecuteDataTable(VQ.SetVehicleDiv_Query(), CommandType.Text);

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
        /// 
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_SearchVehicle(string strValue)
        {
            string rtnJson = "";
            string strResult = String_Encrypt.decryptAES256(strValue);

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);

            DataHelper.ConnectionString_Select = "SSGM";

            try
            {
                Resultdt = DataHelper.ExecuteDataTable(VQ.SearchVehicle_Query(dt.Rows[0]), CommandType.Text);

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
    }
}
