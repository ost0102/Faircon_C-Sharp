using SSGM_ELVISPRIME_COMMON.Controllers;
using SSGM_ELVISPRIME_COMMON.YJIT_Utils;
using System;
using System.Data;
using System.Web.Mvc;

namespace SSGM_ELVISPRIME_WEB.Controllers.Schedule
{
    public class ScheduleController : Controller
    {
        //
        // GET: /Default1/
        Encryption ec = new Encryption(); //DB_Data - Encryption      
        //Con_Schedule Con_Schedule = new Con_Schedule();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        string strJson = "";
        string strResult = "";

        public class JsonData
        {
            public string vJsonData { get; set; }
        }

        public ActionResult Index()
        {
            return View();
        }

        ///// <summary>
        ///// 선사 / 훼리 라이너 데이터 가져오기
        ///// </summary>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public ActionResult fnGetSEALiner(JsonData value)
        //{
        //    try
        //    {
        //        string vJsonData = value.vJsonData.ToString();
        //        string vEncodeData = "";

        //        //암호화 걸기
        //        vEncodeData = ec.encryptAES256(vJsonData);

        //        strResult = Con_Schedule.Con_GetSEALinerData(vEncodeData);

        //        strJson = ec.decryptAES256(strResult);

        //        return Json(strJson);
        //    }
        //    catch (Exception e)
        //    {
        //        strJson = e.Message;
        //        return Json(strJson);
        //    }
        //}

        ///// <summary>
        ///// 선사 / 훼리 스케줄 데이터 가져오기
        ///// </summary>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public ActionResult fnGetSEASchedule(JsonData value)
        //{
        //    try
        //    {
        //        string vJsonData = value.vJsonData.ToString();
        //        string vEncodeData = "";

        //        //암호화 걸기
        //        vEncodeData = ec.encryptAES256(vJsonData);

        //        strResult = Con_Schedule.Con_GetSEAScheduleData(vEncodeData);

        //        strJson = ec.decryptAES256(strResult);

        //        return Json(strJson);
        //    }
        //    catch (Exception e)
        //    {
        //        strJson = e.Message;
        //        return Json(strJson);
        //    }
        //}

        ///// <summary>
        ///// 선사 / 훼리 Carr 선택 된 선사만 데이터 가져오기
        ///// </summary>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public ActionResult fnGetSEAChkSchedule(JsonData value)
        //{
        //    try
        //    {
        //        string vJsonData = value.vJsonData.ToString();
        //        string vEncodeData = "";

        //        //암호화 걸기
        //        vEncodeData = ec.encryptAES256(vJsonData);

        //        strResult = Con_Schedule.Con_GetSEAChkScheduleData(vEncodeData);

        //        strJson = ec.decryptAES256(strResult);

        //        return Json(strJson);
        //    }
        //    catch (Exception e)
        //    {
        //        strJson = e.Message;
        //        return Json(strJson);
        //    }
        //}

        ///// <summary>
        ///// 항공 라이너 데이터 가져오기
        ///// </summary>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public ActionResult fnGetAIRLiner(JsonData value)
        //{
        //    try
        //    {
        //        string vJsonData = value.vJsonData.ToString();
        //        string vEncodeData = "";

        //        //암호화 걸기
        //        vEncodeData = ec.encryptAES256(vJsonData);

        //        strResult = Con_Schedule.Con_GetAIRLinerData(vEncodeData);

        //        strJson = ec.decryptAES256(strResult);

        //        return Json(strJson);
        //    }
        //    catch (Exception e)
        //    {
        //        strJson = e.Message;
        //        return Json(strJson);
        //    }
        //}

        ///// <summary>
        ///// 항공 스케줄 데이터 가져오기
        ///// </summary>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public ActionResult fnGetAIRSchedule(JsonData value)
        //{
        //    try
        //    {
        //        string vJsonData = value.vJsonData.ToString();
        //        string vEncodeData = "";

        //        //암호화 걸기
        //        vEncodeData = ec.encryptAES256(vJsonData);

        //        strResult = Con_Schedule.Con_GetAIRScheduleData(vEncodeData);

        //        strJson = ec.decryptAES256(strResult);

        //        return Json(strJson);
        //    }
        //    catch (Exception e)
        //    {
        //        strJson = e.Message;
        //        return Json(strJson);
        //    }
        //}

        ///// <summary>
        ///// 항공 Carr 선택 된 선사만 데이터 가져오기
        ///// </summary>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public ActionResult fnGetAIRChkSchedule(JsonData value)
        //{
        //    try
        //    {
        //        string vJsonData = value.vJsonData.ToString();
        //        string vEncodeData = "";

        //        //암호화 걸기
        //        vEncodeData = ec.encryptAES256(vJsonData);

        //        strResult = Con_Schedule.Con_GetAIRChkScheduleData(vEncodeData);

        //        strJson = ec.decryptAES256(strResult);

        //        return Json(strJson);
        //    }
        //    catch (Exception e)
        //    {
        //        strJson = e.Message;
        //        return Json(strJson);
        //    }
        //}


    }
}
