using SSGM_ELVISPRIME_COMMON.Controllers;
using SSGM_ELVISPRIME_COMMON.YJIT_Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSGM_ELVISPRIME_WEB.Controllers.Document
{
    public class DocumentController : Controller
    {        
        // GET: /Document/
        Encryption ec = new Encryption(); //DB_Data - Encryption  
        Con_Document Con_Document = new Con_Document();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        string strJson = "";
        string strResult = "";

        public ActionResult BL()
        {
            if (TempData.ContainsKey("REF1"))
            {
                string HBL_NO = TempData["REF1"].ToString();
                if (HBL_NO != "")
                {
                    ViewBag.HBL_NO = HBL_NO;
                }
            }

            if (TempData.ContainsKey("HBL_NO"))
            {
                string HBL_NO = TempData["HBL_NO"].ToString();
                if (HBL_NO != "")
                {
                    ViewBag.HBL_NO = HBL_NO;
                }
            }

            return View();
        }

        public ActionResult Invoice()
        {
            if (TempData.ContainsKey("REF1"))
            {
                string INV_NO = TempData["REF1"].ToString();
                if (INV_NO != "")
                {
                    ViewBag.INV_NO = INV_NO;
                }
            }

            if (TempData.ContainsKey("INV_NO"))
            {
                string INV_NO = TempData["INV_NO"].ToString();
                if (INV_NO != "")
                {
                    ViewBag.INV_NO = INV_NO;
                }
            }

            return View();
        }

        public class JsonData
        {
            public string vJsonData { get; set; }
        }

        /// <summary>
        /// invoice 데이터 가져오기
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult fnGetInvData(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";
                
                //암호화 걸기
                vEncodeData = ec.encryptAES256(vJsonData);
                
                strResult = Con_Document.Con_fnGetInvData(vEncodeData);
                
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

                strResult = Con_Document.Con_fnGetInvPrint(vEncodeData);

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

                strResult = Con_Document.Con_fnSearchInvRequest(vEncodeData);

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

                strResult = Con_Document.Con_fnSaveInvRequest(vEncodeData);

                strJson = ec.decryptAES256(strResult);

                return Json(strJson);
            }
            catch (Exception e)
            {
                strJson = e.Message;
                return Json(strJson);
            }
        }

        ////////////////////////BL///////////////////////////
        /// <summary>
        /// BL 데이터 가져오기
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult fnGetBLData(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = ec.encryptAES256(vJsonData);

                strResult = Con_Document.Con_fnGetBLData(vEncodeData);

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
        /// BL 가져오기
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult fnGetBLPrint(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = ec.encryptAES256(vJsonData);

                strResult = Con_Document.Con_fnGetBLPrint(vEncodeData);

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

                strResult = Con_Document.Con_fnSearchBLRequest(vEncodeData);

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

                strResult = Con_Document.Con_fnSaveBLRequest(vEncodeData);

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
        /// BL - 파트너 문서타입 데이터 가져오기
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult fnGetPreAlert(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = ec.encryptAES256(vJsonData);

                strResult = Con_Document.Con_fnGetPreAlert(vEncodeData);

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
        /// MyBoard - 첨부파일 문서 가져오기
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SelectFile(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = ec.encryptAES256(vJsonData);

                strResult = Con_Document.SelectFile(vEncodeData);

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
        /// BL - 화주 문서타입 데이터 가져오기
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult fnGetDocFile(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = ec.encryptAES256(vJsonData);

                strResult = Con_Document.Con_fnGetDocFile(vEncodeData);

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
