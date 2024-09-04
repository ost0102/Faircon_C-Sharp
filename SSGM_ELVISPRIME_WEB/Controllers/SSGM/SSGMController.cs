using SSGM_ELVISPRIME_COMMON.Controllers;
using SSGM_ELVISPRIME_COMMON.YJIT_Utils;
using System;
using System.Data;
using System.Web.Mvc;

namespace SSGM_ELVISPRIME_WEB.Controllers.SSGM
{
    public class SSGMController : Controller
    {
        //
        // GET: /Default1/
        Encryption ec = new Encryption(); //DB_Data - Encryption      
        Con_SSGM Con_SSGM = new Con_SSGM();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        string strJson = "";
        string strResult = "";

        public class JsonData
        {
            public string vJsonData { get; set; }
        }

        public ActionResult Regist()
        {
            if (TempData.ContainsKey("REF1"))
            {
                string BKG_NO = TempData["REF1"].ToString();
                if (BKG_NO != "")
                {
                    ViewBag.BKG_NO = BKG_NO;
                }
            }

            if (TempData.ContainsKey("SCH_NO"))
            {
                string SCH_NO = TempData["SCH_NO"].ToString();
                if (SCH_NO != "")
                {
                    ViewBag.SCH_NO = SCH_NO;
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

            if (TempData.ContainsKey("POD_CD"))
            {
                string POD_CD = TempData["POD_CD"].ToString();
                if (POD_CD != "")
                {
                    ViewBag.POD_CD = POD_CD;
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
            if (TempData.ContainsKey("POD_NM"))
            {
                string POD_NM = TempData["POD_NM"].ToString();
                if (POD_NM != "")
                {
                    ViewBag.POD_NM = POD_NM;
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
            if (TempData.ContainsKey("ETD"))
            {
                string ETD = TempData["ETD"].ToString();
                if (ETD != "")
                {
                    ViewBag.ETD = ETD;
                }
            }
            if (TempData.ContainsKey("CNTR_TYPE"))
            {
                string CNTR_TYPE = TempData["CNTR_TYPE"].ToString();
                if (CNTR_TYPE != "")
                {
                    ViewBag.CNTR_TYPE = CNTR_TYPE;
                }
            }

            return View();
        }
        public ActionResult Sea()
        {
            return View();
        }
        public ActionResult Lcl()
        {
            return View();
        }
        //Schedule 
        #region
        [HttpPost]
        public ActionResult fnGetSchData(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = ec.encryptAES256(vJsonData);

                strResult = Con_SSGM.Con_fnGetSchData(vEncodeData);

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

        //Booking
        #region
        [HttpPost]
        public ActionResult fnGetFIleType(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = ec.encryptAES256(vJsonData);

                strResult = Con_SSGM.Con_fnGetFIleType(vEncodeData);

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
        /// 부킹 화면 - 스케줄 데이터 가져오기
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult fnGetBkgSchData(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = ec.encryptAES256(vJsonData);

                strResult = Con_SSGM.Con_fnGetBkgSchData(vEncodeData);

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
        /// 파일 타입 데이터 가져오기
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult fnGetBKNO()
        {
            try
            {
                strResult = Con_SSGM.Con_fnGetBKNO();

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
        /// 부킹 저장
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult fnSaveBooking(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = ec.encryptAES256(vJsonData);
                
                strResult = Con_SSGM.Con_SaveBooking(vEncodeData);

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
        /// 부킹 수정
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult fnGetModifyBooking(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = ec.encryptAES256(vJsonData);

                strResult = Con_SSGM.Con_fnGetModifyBooking(vEncodeData);

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

                strResult = Con_SSGM.Con_fnSetCancelStatus(vEncodeData);

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


        //Schedule 
        
        [HttpPost]
        public ActionResult fnArrivePort(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = ec.encryptAES256(vJsonData);

                strResult = Con_SSGM.Con_fnArrivePort(vEncodeData);

                strJson = ec.decryptAES256(strResult);

                return Json(strJson);
            }
            catch (Exception e)
            {
                strJson = e.Message;
                return Json(strJson);
            }
        }



        
        [HttpPost]
        public ActionResult fnWareArrivePort(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = ec.encryptAES256(vJsonData);

                strResult = Con_SSGM.Con_fnWareArrivePort(vEncodeData);

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
