using SSGM_ELVISPRIME_COMMON.YJIT_Utils;
using SSGM_ELVISPRIME_COMMON.Controllers;
using System;
using System.Web.Mvc;

namespace SSGM_ELVISPRIME_WEB.Controllers.LogisticsTools
{
    public class MyDataController : Controller
    {
        //
        Encryption ec = new Encryption(); //DB_Data - Encryption  
        Con_MyMenu Con_MyMenu = new Con_MyMenu();
        string strJson = "";
        string strResult = "";
        // GET: /Default1/

        public ActionResult Index()
        {
            if (TempData.ContainsKey("REF2"))
            {
                string HBL_NO = TempData["REF2"].ToString();
                if (HBL_NO != "")
                {
                    ViewBag.HBL_NO = HBL_NO;
                }
            }

            return View();
        }

        public ActionResult BookingList()
        {
            if (TempData.ContainsKey("SCH_NO"))
            {
                string SCH_NO = TempData["SCH_NO"].ToString();
                if (SCH_NO != "")
                {
                    ViewBag.SCH_NO = SCH_NO;
                }
            }

            if (TempData.ContainsKey("REG_BKG_NO"))
            {
                string REG_BKG_NO = TempData["REG_BKG_NO"].ToString();
                if (REG_BKG_NO != "")
                {
                    ViewBag.REG_BKG_NO = REG_BKG_NO;
                }
            }

            if (TempData.ContainsKey("REF1"))
            {
                string REG_BKG_NO = TempData["REF1"].ToString();
                if (REG_BKG_NO != "")
                {
                    ViewBag.REG_BKG_NO = REG_BKG_NO;
                }
            }

            if (TempData.ContainsKey("BKG_NO"))
            {
                string BKG_NO = TempData["BKG_NO"].ToString();
                if (BKG_NO != "")
                {
                    ViewBag.BKG_NO = BKG_NO;
                }
            }
            if (TempData.ContainsKey("REQ_SVC"))
            {
                string REQ_SVC = TempData["REQ_SVC"].ToString();
                if (REQ_SVC != "")
                {
                    ViewBag.REQ_SVC = REQ_SVC;
                }
            }
            if (TempData.ContainsKey("ETD"))
            {
                string ETD = TempData["ETD"].ToString();
                if (ETD != "")
                {
                    ViewBag.ETD = ETD;
                }
            }
            if (TempData.ContainsKey("ETA"))
            {
                string ETA = TempData["ETA"].ToString();
                if (ETA != "")
                {
                    ViewBag.ETA = ETA;
                }
            }
            if (TempData.ContainsKey("POL_CD"))
            {
                string POL_CD = TempData["POL_CD"].ToString();
                if (POL_CD != "")
                {
                    ViewBag.POL_CD = POL_CD;
                }
            }
            if (TempData.ContainsKey("POL_NM"))
            {
                string POL_NM = TempData["POL_NM"].ToString();
                if (POL_NM != "")
                {
                    ViewBag.POL_NM = POL_NM;
                }
            }
            if (TempData.ContainsKey("POD_CD"))
            {
                string POD_CD = TempData["POD_CD"].ToString();
                if (POD_CD != "")
                {
                    ViewBag.POD_CD = POD_CD;
                }
            }
            if (TempData.ContainsKey("POD_NM"))
            {
                string POD_NM = TempData["POD_NM"].ToString();
                if (POD_NM != "")
                {
                    ViewBag.POD_NM = POD_NM;
                }
            }

            return View();
        }

        public ActionResult Console()
        {
            return View();
        }

        public class JsonData
        {
            public string vJsonData { get; set; }
        }

        //부킹 조회
        #region
        /// <summary>
        /// 부킹 상태 플래그 데이터 가져오기
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult fnGetBookingStatus()
        {
            try
            {
                strResult = Con_MyMenu.Con_fnGetBookingStatus();

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
        /// 부킹 조회
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult fnGetBkgData(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = ec.encryptAES256(vJsonData);

                strResult = Con_MyMenu.Con_fnGetBkgData(vEncodeData);

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
        /// 부킹 상태 취소로 변경
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult fnSetCancelStatus(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = ec.encryptAES256(vJsonData);

                strResult = Con_MyMenu.Con_fnSetCancelStatus(vEncodeData);

                strJson = ec.decryptAES256(strResult);

                return Json(strJson);
            }
            catch (Exception e)
            {
                strJson = e.Message;
                return Json(strJson);
            }
        }
        #endregion

        //MyBoard
        #region
        /// <summary>
        /// MyBoard - Myboard 검색
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult fnGetBoradData(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = ec.encryptAES256(vJsonData);

                strResult = Con_MyMenu.Con_fnGetBoardData(vEncodeData);

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
        /// MyBoard - cfs 부킹조회
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult fnGetCfsData(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = ec.encryptAES256(vJsonData);

                strResult = Con_MyMenu.Con_fnGetCfsData(vEncodeData);

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
        /// BL 수정요청사항 저장
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult fnSaveBLRequest(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = ec.encryptAES256(vJsonData);

                strResult = Con_MyMenu.Con_fnSaveBLRequest(vEncodeData);

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
        /// Invoice 수정요청사항 검색
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult fnSearchInvRequest(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = ec.encryptAES256(vJsonData);

                strResult = Con_MyMenu.Con_fnSearchInvRequest(vEncodeData);

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
        /// Invoice 출력 - 수정요청사항 저장
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult fnSaveInvRequest(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = ec.encryptAES256(vJsonData);

                strResult = Con_MyMenu.Con_fnSaveInvRequest(vEncodeData);

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
        /// Invoice 가져오기
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult fnGetInvPrint(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = ec.encryptAES256(vJsonData);

                strResult = Con_MyMenu.Con_fnGetInvPrint(vEncodeData);

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
        /// BL 수정요청사항 가져오기
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult fnSearchBLRequest(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = ec.encryptAES256(vJsonData);

                strResult = Con_MyMenu.Con_fnSearchBLRequest(vEncodeData);

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
        /// Tracking 유무 체크
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult isTrackingAvailable(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = ec.encryptAES256(vJsonData);

                strResult = Con_MyMenu.Con_isTrackingAvailable(vEncodeData);

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
        /// Web Print 데이터 가져오기
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult fnGetPrintData(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = ec.encryptAES256(vJsonData);

                strResult = Con_MyMenu.Con_fnGetPrintData(vEncodeData);

                strJson = ec.decryptAES256(strResult);

                return Json(strJson);
            }
            catch (Exception e)
            {
                strJson = e.Message;
                return Json(strJson);
            }
        }
        #endregion

        //Console
        #region
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult fnGetConsoleData(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = ec.encryptAES256(vJsonData);

                strResult = Con_MyMenu.Con_fnGetConsoleData(vEncodeData);

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
        /// Console - 파일 리스트 가져오기
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult fnLayerSetFileList(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = ec.encryptAES256(vJsonData);

                strResult = Con_MyMenu.Con_fnLayerSetFileList(vEncodeData);

                strJson = ec.decryptAES256(strResult);

                return Json(strJson);
            }
            catch (Exception e)
            {
                strJson = e.Message;
                return Json(strJson);
            }
        }


        #endregion
    }



}
