using SSGM_ELVISPRIME_COMMON.Controllers;
using SSGM_ELVISPRIME_COMMON.YJIT_Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;

namespace SSGM_ELVISPRIME_WEB.Controllers.MyData
{
    public class PopupController : Controller
    {
        Encryption ec = new Encryption(); //DB_Data - Encryption  
        Con_Popup Con_Popup = new Con_Popup();
        string strJson = "";
        string strResult = "";

        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult Console_Detail()
        {
            return View();
        }
        public ActionResult MyBoard_FileDownload()
        {
            return View();
        }
        public ActionResult Tracking_FileDownload()
        {
            return View();
        }

        public ActionResult SchArrExport()
        {
            return View();
        }
        public ActionResult SchArrImport()
        {
            return View();
        }
        public ActionResult SchDate()
        {
            return View();
        }
        public ActionResult SchDptExport()
        {
            return View();
        }
        public ActionResult SchDptImport()
        {
            return View();
        }
        public ActionResult Tracking()
        {
            return View();
        }
        public ActionResult Upload()
        {
            return View();
        }

        public class JsonData
        {
            public string vJsonData { get; set; }
        }

        //MyBoard - FileList
        #region
        /// <summary>
        /// 문서 파일 리스트 데이터 가져오기
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult fnGetMyBoardFileList(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = ec.encryptAES256(vJsonData);

                strResult = Con_Popup.Con_fnGetMyBoardFileList(vEncodeData);

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

        //Tracking
        #region
        /// <summary>
        /// Tracking 데이터 가져오기
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult fnGetTracking(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = ec.encryptAES256(vJsonData);

                strResult = Con_Popup.Con_fnGetTracking(vEncodeData);

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
        /// Tracking 마일스톤만 가져오기
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult fnGetTrackingData(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = ec.encryptAES256(vJsonData);

                strResult = Con_Popup.Con_fnGetTrackingData(vEncodeData);

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
        /// Tracking 마일스톤만 가져오기
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult fnGetLayerFileList(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = ec.encryptAES256(vJsonData);

                strResult = Con_Popup.Con_fnGetLayerFileList(vEncodeData);

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

        //Console_Detail
        #region
        /// <summary>
        /// 입고 상세내역 조회
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult fnGetLayerConsoleDetail_MYDATA(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = ec.encryptAES256(vJsonData);

                strResult = Con_Popup.Con_fnGetLayerConsoleDetail_MYDATA(vEncodeData);

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

        //Console_Detail
        #region
        /// <summary>
        /// 입고 상세내역 조회
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult fnGetLayerConsoleDetail(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = ec.encryptAES256(vJsonData);

                strResult = Con_Popup.Con_fnGetLayerConsoleDetail(vEncodeData);

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

        //Console_Detail
        #region
        /// <summary>
        /// 입고 상세내역 조회
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult fnGetLayerConsoleDetail_NEW(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = ec.encryptAES256(vJsonData);

                strResult = Con_Popup.Con_fnConsoleDetail(vEncodeData);

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
