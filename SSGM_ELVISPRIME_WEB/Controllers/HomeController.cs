using SSGM_ELVISPRIME_COMMON.Controllers;
using SSGM_ELVISPRIME_COMMON.YJIT_Utils;
using System;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Data;

namespace SSGM_ELVISPRIME_HOME.Controllers
{
	public class HomeController : Controller
	{

        Encryption ec = new Encryption(); //DB_Data - Encryption      
        Con_Main Con_Main = new Con_Main();

        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        string strJson = "";
        string strResult = "";

        public ActionResult Index()
		{
            return View();
		}

        public class JsonData
        {
            public string vJsonData { get; set; }
        }

        /// <summary>
        /// HBL_NO CheckBL 확인
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult fnChkBL(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = ec.encryptAES256(vJsonData);

                strResult = Con_Main.Con_ChkBl(vEncodeData);

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
        /// Sea - Tracking 리스트 데이터 가져오기
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetSeaTrackingList(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = ec.encryptAES256(vJsonData);

                strResult = Con_Main.Con_GetSeaTrackingList(vEncodeData);

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
        /// Air - Tracking 리스트 데이터 가져오기
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetAirTrackingList(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = ec.encryptAES256(vJsonData);

                strResult = Con_Main.Con_GetAirTrackingList(vEncodeData);

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
        /// 화물추적 데이터 가지고 오기
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetTrackingDetail(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = ec.encryptAES256(vJsonData);

                strResult = Con_Main.Con_GetTrackDetail(vEncodeData);

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
        /// 로그인 함수
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult fnLogin(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = ec.encryptAES256(vJsonData);

                strResult = Con_Main.Con_fnLogin(vEncodeData);

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
        /// 로그인 후 데이터 세션에 Save
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public ActionResult SaveLogin(JsonData value)
        {
            DataSet ds = JsonConvert.DeserializeObject<DataSet>(value.vJsonData);
            DataTable rst = ds.Tables["Result"];
            DataTable dt = ds.Tables["Table"];

            try
            {
                if (rst.Rows[0]["trxCode"].ToString() == "N") return Content("N");

                if (rst.Rows[0]["trxCode"].ToString() == "Y")
                {
                    Session["USR_ID"] = dt.Rows[0]["USR_ID"].ToString();
                    Session["USER_NM"] = dt.Rows[0]["USER_NM"].ToString();
                    Session["CUST_CD"] = dt.Rows[0]["CUST_CD"].ToString();
                    Session["EMAIL"] = dt.Rows[0]["EMAIL"].ToString();
                    Session["AUTH_KEY"] = dt.Rows[0]["AUTH_KEY"].ToString();
                    Session["AUTH_TYPE"] = dt.Rows[0]["AUTH_TYPE"].ToString();
                    Session["USR_TYPE"] = dt.Rows[0]["USR_TYPE"].ToString();
                    Session["HP_NO"] = dt.Rows[0]["HP_NO"].ToString();
                    Session["DOMAIN"] = System.Configuration.ConfigurationManager.AppSettings["Domain"];
                    Session["OFFICE_CD"] = dt.Rows[0]["OFFICE_CD"].ToString();
                    Session["CUST_NM"] = dt.Rows[0]["CUST_NM"].ToString();

                    return Content("Y");
                }

                return Content("N");
            }
            catch (Exception e)
            {
                return Content(e.Message);
            }
        }

        /// <summary>
        /// 로그아웃 페이지
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string LogOut()
        {
            Session.Clear();
            Session.RemoveAll();
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();

            return "Y";
        }
    }
}
