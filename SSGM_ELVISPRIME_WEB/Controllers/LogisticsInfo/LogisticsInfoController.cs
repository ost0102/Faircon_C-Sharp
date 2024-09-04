using SSGM_ELVISPRIME_COMMON.YJIT_Utils;
using SSGM_ELVISPRIME_COMMON.Controllers;
using System;
using System.Web.Mvc;
using SSGM_ELVISPRIME_WEB.Models;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using YJIT.Data;
using Newtonsoft.Json;


namespace SSGM_ELVISPRIME_WEB.Controllers.LogisticsInfo
{
    public class LogisticsInfoController : Controller
    {
        //
        // GET: /Default1/
        public ActionResult Notice()
        {
            return View();
        }
        public ActionResult CBM()
        {
            return View();
        }
        public ActionResult Surcharge()
        {
            return View();
        }
        public ActionResult Incoterms()
        {
            return View();
        }
        public ActionResult PortLocation()
        {
            return View();
        }
        public ActionResult Tariff()
        {
            return View();
        }
        public ActionResult AEOInfo()
        {
            return View();
        }
        public ActionResult Vehicle()
        {
            return View();
        }

        Encryption ec = new Encryption(); //DB_Data - Encryption      
        Con_LogisticsTools CL = new Con_LogisticsTools();
        String strResult = "";
        String strJson = "";

        public class JsonData
        {
            public string vJsonData { get; set; }
        }




        private List<NoticeModel> NoticeList = new List<NoticeModel>();
        public List<DataRow> dtList { get; set; }
        public class pageParam
        {
            public string Option { get; set; }
            public string Type { get; set; }
            public string SearchText { get; set; }
            public int Page { get; set; }
            public string Area { get; set; }
        }
        public string CallAjax(pageParam rtnVal)
        {
            string rtnJson = "";
            try
            {
                if (rtnVal != null)
                {
                    string strOpt = rtnVal.Option;
                    string strType = rtnVal.Type;
                    string strText = rtnVal.SearchText;
                    int pageIndex = rtnVal.Page;

                    ADO_Conn con = new ADO_Conn();
                    DBA dbConn = new DBA(con.ConnectionStr, DbConfiguration.Current.DatabaseType);
                    DataTable dt = dbConn.SqlGet(con.Search_Notice(strOpt, strType, strText, pageIndex));
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0) rtnJson = JsonConvert.SerializeObject(dt);
                    }

                }
                return rtnJson;
            }
            catch (Exception e) 
            {
                return e.Message;
            }
        }

        public ActionResult notice_view(string id)   //공지사항 상세보기
        {
            if (!string.IsNullOrEmpty(id))
            {
                ADO_Conn con = new ADO_Conn();
                DBA dbConn = new DBA(con.ConnectionStr, DbConfiguration.Current.DatabaseType);
                dbConn.SqlSet(con.Update_ViewCnt(id));
                dbConn.Commit();
                DataTable dt = dbConn.SqlGet(con.Search_NoticeView(id));


                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        NoticeList.Add(new NoticeModel
                        {
                            FLAG = dr["FLAG"].ToString(),
                            NOTICE_ID = int.Parse(dr["NOTICE_ID"].ToString()),
                            TITLE = dr["TITLE"].ToString(),
                            CNT = int.Parse(dr["CNT"].ToString()),
                            WRITER = dr["WRITER"].ToString(),
                            USE_YN = dr["USE_YN"].ToString(),
                            NOTICE_YN = dr["NOTICE_YN"].ToString(),
                            REGDT = dr["REGDT"].ToString(),
                            EDITDT = dr["EDITDT"].ToString(),
                            FILE = dr["FILE"].ToString(),
                            FILE_NAME = dr["FILE_NAME"].ToString(),
                            FILE1 = dr["FILE1"].ToString(),
                            FILE_NAME1 = dr["FILE1_NAME"].ToString(),
                            FILE2 = dr["FILE2"].ToString(),
                            FILE_NAME2 = dr["FILE2_NAME"].ToString(),
                            CONTENT = dr["CONTENT"].ToString(),
                            TYPE = dr["TYPE"].ToString()
                        });
                    }
                }
            }
            return View(NoticeList);
        }

        /// <summary>
        /// 분기 / 년 데이터 가져오기
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult fnSetYearQuarter(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = ec.encryptAES256(vJsonData);

                strResult = CL.Con_SetYearQuarter(vEncodeData);

                strJson = ec.decryptAES256(strResult);

                return Json(strJson);
            }
            catch (Exception e)
            {
                strJson = e.Message;
                return Json(strJson);
            }
        }

        /// <summary>
        /// 전체(시.도) 데이터 가져오기
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult fnSetAddrState(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = ec.encryptAES256(vJsonData);

                strResult = CL.Con_SetAddrState(vEncodeData);

                strJson = ec.decryptAES256(strResult);

                return Json(strJson);
            }
            catch (Exception e)
            {
                strJson = e.Message;
                return Json(strJson);
            }
        }

        /// <summary>
        /// 전체(시.군.구) 데이터 가져오기
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult fnSetAddrCity(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = ec.encryptAES256(vJsonData);

                strResult = CL.Con_SetAddrCity(vEncodeData);

                strJson = ec.decryptAES256(strResult);

                return Json(strJson);
            }
            catch (Exception e)
            {
                strJson = e.Message;
                return Json(strJson);
            }
        }

        /// <summary>
        /// 전체(읍,면,동 - 행정동)데이터 가져오기
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult fnSetAddrTownship(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = ec.encryptAES256(vJsonData);

                strResult = CL.Con_SetAddrTownship(vEncodeData);

                strJson = ec.decryptAES256(strResult);

                return Json(strJson);
            }
            catch (Exception e)
            {
                strJson = e.Message;
                return Json(strJson);
            }
        }

        /// <summary>
        /// 전체(읍,면,동 - 법정동) 데이터 가져오기
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult fnSetAddrTownship2(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = ec.encryptAES256(vJsonData);

                strResult = CL.Con_SetAddrTownship2(vEncodeData);

                strJson = ec.decryptAES256(strResult);

                return Json(strJson);
            }
            catch (Exception e)
            {
                strJson = e.Message;
                return Json(strJson);
            }
        }

        /// <summary>
        /// 항구 기점 데이터 가져오기
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult fnSetSection(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = ec.encryptAES256(vJsonData);

                strResult = CL.Con_fnSetSection(vEncodeData);

                strJson = ec.decryptAES256(strResult);

                return Json(strJson);
            }
            catch (Exception e)
            {
                strJson = e.Message;
                return Json(strJson);
            }
        }


        /// <summary>
        /// 할증 데이터 가져오기
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult fnSetPremiumRate(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = ec.encryptAES256(vJsonData);

                strResult = CL.Con_SetPremiumRate(vEncodeData);

                strJson = ec.decryptAES256(strResult);

                return Json(strJson);
            }
            catch (Exception e)
            {
                strJson = e.Message;
                return Json(strJson);
            }
        }

        /// <summary>
        /// 안전운임 데이터 검색
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult fnTariffSerach(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = ec.encryptAES256(vJsonData);

                strResult = CL.Con_TariffSerach(vEncodeData);

                strJson = ec.decryptAES256(strResult);

                return Json(strJson);
            }
            catch (Exception e)
            {
                strJson = e.Message;
                return Json(strJson);
            }
        }

        /// <summary>
        /// 안전운임 데이터 검색
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult fnSetSurchargePort(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = ec.encryptAES256(vJsonData);

                strResult = CL.Con_SetSurchargePort(vEncodeData);

                strJson = ec.decryptAES256(strResult);

                return Json(strJson);
            }
            catch (Exception e)
            {
                strJson = e.Message;
                return Json(strJson);
            }
        }

        /// <summary>
        /// 수입 / 수출 국가옵션 가져오기
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult fnSetSurchargeCountry(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = ec.encryptAES256(vJsonData);

                strResult = CL.Con_SetSurchargeCountry(vEncodeData);

                strJson = ec.decryptAES256(strResult);

                return Json(strJson);
            }
            catch (Exception e)
            {
                strJson = e.Message;
                return Json(strJson);
            }
        }

        /// <summary>
        /// 부대비용 검색 리스트 데이터 가져오기
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult fnSearchSurcharge(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = ec.encryptAES256(vJsonData);

                strResult = CL.Con_SearchSurcharge(vEncodeData);

                strJson = ec.decryptAES256(strResult);

                return Json(strJson);
            }
            catch (Exception e)
            {
                strJson = e.Message;
                return Json(strJson);
            }
        }

        ///////////////////////////////////////차량 제원 컨트롤러/////////////////////////////////////////////////
        ////// <summary>
        /// 차량 제원 구분 Select 박스 데이터 가져오기
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult fnSetVehicleDiv()
        {
            try
            {
                strResult = CL.Con_SetVehicleDiv();

                strJson = ec.decryptAES256(strResult);

                return Json(strJson);
            }
            catch (Exception e)
            {
                strJson = e.Message;
                return Json(strJson);
            }
        }

        ////// <summary>
        /// 차량 제원 구분 Select 박스 데이터 가져오기
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult fnSearchVehicle(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = ec.encryptAES256(vJsonData);

                strResult = CL.Con_SearchVehicle(vEncodeData);

                strJson = ec.decryptAES256(strResult);

                return Json(strJson);
            }
            catch (Exception e)
            {
                strJson = e.Message;
                return Json(strJson);
            }
        }

    }
}
