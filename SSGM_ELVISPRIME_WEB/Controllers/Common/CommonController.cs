using SSGM_ELVISPRIME_COMMON.Controllers;
using SSGM_ELVISPRIME_COMMON.YJIT_Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSGM_ELVISPRIME_WEB.Controllers.Common
{
    public class CommonController : Controller
    {
        Encryption ec = new Encryption(); //DB_Data - Encryption         
        Con_Common Con_Com = new Con_Common();
        
        //
        // GET: /Common/
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        string strJson = "";
        string strResult = "";

        public class JsonData
        {
            public string vJsonData { get; set; }
        }

        /// <summary>
        /// Port 가져오기
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult fnGetPort(JsonData value)
        {   
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = ec.encryptAES256(vJsonData);

                strResult = Con_Com.Con_GetPortData(vEncodeData);

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
        /// Service 타입 가져오기
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult fnGetServiceType(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = ec.encryptAES256(vJsonData);

                strResult = Con_Com.Con_fnGetServiceType(vEncodeData);

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
        /// Port 가져오기
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult fnGetWebPort(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = ec.encryptAES256(vJsonData);

                strResult = Con_Com.Con_GetPortWebData(vEncodeData);

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
        /// Port 가져오기
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult fnGetWebWarePort(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = ec.encryptAES256(vJsonData);

                strResult = Con_Com.Con_GetPortWebWareData(vEncodeData);

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
