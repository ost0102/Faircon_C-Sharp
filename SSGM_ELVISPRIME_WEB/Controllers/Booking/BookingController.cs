using SSGM_ELVISPRIME_COMMON.Controllers;
using SSGM_ELVISPRIME_COMMON.YJIT_Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSGM_ELVISPRIME_WEB.Controllers.Booking
{
    public class BookingController : Controller
    {
        Encryption ec = new Encryption(); //DB_Data - Encryption  
        Con_Booking Con_Booking = new Con_Booking();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        string strJson = "";
        string strResult = "";
        //
        // GET: /Default1/
        public ActionResult BookingInfo()
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
            if (TempData.ContainsKey("LINE_TYPE"))
            {
                string LINE_TYPE = TempData["LINE_TYPE"].ToString();
                if (LINE_TYPE != "")
                {
                    ViewBag.LINE_TYPE = LINE_TYPE;
                }
            }

            return View();
        }

        public class JsonData
        {
            public string vJsonData { get; set; }
        }

        /// <summary>
        /// 해운 스케줄 데이터 가져오기
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult fnGetSEASchedule(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = ec.encryptAES256(vJsonData);

                strResult = Con_Booking.Con_fnGetSEASchedule(vEncodeData);

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

                //Con_SaveBooking - 부킹 저장 후 파일 저장 / Con_SaveBooking2 파일 저장 후 부킹 저장
                strResult = Con_Booking.Con_SaveBooking2(vEncodeData);

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

                strResult = Con_Booking.Con_fnGetModifyBooking(vEncodeData);

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

                strResult = Con_Booking.Con_fnGetBkgData(vEncodeData);

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

                strResult = Con_Booking.Con_fnSetCancelStatus(vEncodeData);

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
        /// 부킹 상태 플래그 데이터 가져오기
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult fnGetBookingStatus()
        {
            try
            {
                strResult = Con_Booking.Con_fnGetBookingStatus();

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
        public ActionResult fnGetFIleType(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = ec.encryptAES256(vJsonData);

                strResult = Con_Booking.Con_fnGetFIleType(vEncodeData);

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
                strResult = Con_Booking.Con_fnGetBKNO();

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
