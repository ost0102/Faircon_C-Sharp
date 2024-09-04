using SSGM_ELVISPRIME_COMMON.Controllers;
using SSGM_ELVISPRIME_COMMON.YJIT_Utils;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace SSGM_ELVISPRIME_WEB.Controllers.Admin
{
    public class AdminController : Controller
    {
        //
        // GET: /Default1/
        Con_Admin CA = new Con_Admin();
        string _EditorFilePath = "/data/editor/";
        const string scriptTag = "<script type='text/javascript'>window.parent.CKEDITOR.tools.callFunction({0}, '{1}', '{2}')</script>";
        DataTable dt = new DataTable();
        Encryption String_Encrypt = new Encryption();
        SSGM_ELVISPRIME_COMMON.YJIT_Utils.Common Comm = new SSGM_ELVISPRIME_COMMON.YJIT_Utils.Common();
        string strResult = "";
        string _NoticeFilePath = "/data/notice/";
        string strJson = "";
        string memberKey = ConfigurationManager.AppSettings["memberKey"].ToString();


        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Notice()
        {
            return View();
        }

        public ActionResult Member()
        {
            return View();
        }
        public ActionResult Port()
        {
            return View();
        }

        public ActionResult MemberWrite(string UserID)
        {
            //신규, 수정
            if (UserID == null)
            {
                return View();
            }
            else
            {
                //Search 한번 하고 고고
                strResult = CA.Con_SearchMemberModify(UserID);
                strResult = String_Encrypt.decryptAES256(strResult);
                DataSet ds = JsonConvert.DeserializeObject<DataSet>(strResult);

                DataTable dt_result = ds.Tables["Result"];

                if (dt_result.Rows[0]["trxCode"].ToString() == "Y")
                {
                    dt = ds.Tables["Member"];

                    if (dt.Rows[0]["MEMB_NO"] != null)
                    {
                        ViewBag.MEMB_NO = dt.Rows[0]["MEMB_NO"];
                    }

                    if (dt.Rows[0]["M_ID"] != null)
                    {
                        ViewBag.M_ID = dt.Rows[0]["M_ID"];
                    }

                    if (dt.Rows[0]["LVL"] != null)
                    {
                        ViewBag.LVL = dt.Rows[0]["LVL"];
                    }

                    if (dt.Rows[0]["AUTH_LEVEL"] != null)
                    {
                        ViewBag.AUTH_LEVEL = dt.Rows[0]["AUTH_LEVEL"];
                    }

                    if (dt.Rows[0]["STATUS"] != null)
                    {
                        ViewBag.STATUS = dt.Rows[0]["STATUS"];
                    }

                    if (dt.Rows[0]["M_NAME"] != null)
                    {
                        ViewBag.M_NAME = dt.Rows[0]["M_NAME"];
                    }

                    string[] mList = dt.Rows[0]["MOBILE"].ToString().Split(new char[] { '-' });

                    if (mList.Length > 0)
                    {
                        switch (mList.Length)
                        {
                            case 1:
                                ViewBag.MOBILE1 = mList[0];
                                break;
                            case 2:
                                ViewBag.MOBILE1 = mList[0];
                                ViewBag.MOBILE2 = mList[1];
                                break;
                            case 3:
                                ViewBag.MOBILE1 = mList[0];
                                ViewBag.MOBILE2 = mList[1];
                                ViewBag.MOBILE3 = mList[2];
                                break;
                        }
                    }

                    if (dt.Rows[0]["MOBILE"] != null)
                    {
                        ViewBag.MOBILE = dt.Rows[0]["MOBILE"];
                    }

                    if (dt.Rows[0]["REGDT"] != null)
                    {
                        ViewBag.REGDT = dt.Rows[0]["REGDT"];
                    }

                    if (dt.Rows[0]["LAST_LOGIN"] != null)
                    {
                        ViewBag.LAST_LOGIN = dt.Rows[0]["LAST_LOGIN"];
                    }
                }
                return View();
            }
        }
        public class JsonData
        {
            public string vJsonData { get; set; }
        }


        /************************************************관리자 페이지****************************************************/


        public class LoginCls
        {
            public string m_id { get; set; }
            public string pwd { get; set; }
        }

        public ActionResult NoticeWrite(string id)
        {

            if (id == null) return View();

            dt = CA.Con_NoticeView(id);

            if (dt.Rows.Count > 0)
            {

                if (dt.Columns.Contains("FLAG"))
                {
                    ViewBag.FLAG = dt.Rows[0]["FLAG"].ToString();
                }

                if (dt.Columns.Contains("NOTICE_ID"))
                {
                    ViewBag.NOTICE_ID = dt.Rows[0]["NOTICE_ID"].ToString();
                }

                if (dt.Columns.Contains("TITLE"))
                {
                    ViewBag.TITLE2 = dt.Rows[0]["TITLE"].ToString();
                }

                if (dt.Columns.Contains("TYPE"))
                {
                    ViewBag.TYPE = dt.Rows[0]["TYPE"].ToString();
                }

                if (dt.Columns.Contains("CNT"))
                {
                    ViewBag.CNT = dt.Rows[0]["CNT"].ToString();
                }

                if (dt.Columns.Contains("WRITER"))
                {
                    ViewBag.WRITER = dt.Rows[0]["WRITER"].ToString();
                }

                if (dt.Columns.Contains("USE_YN"))
                {
                    ViewBag.USE_YN = dt.Rows[0]["USE_YN"].ToString();
                }

                if (dt.Columns.Contains("NOTICE_YN"))
                {
                    ViewBag.NOTICE_YN = dt.Rows[0]["NOTICE_YN"].ToString();
                }

                if (dt.Columns.Contains("REGDT"))
                {
                    ViewBag.REGDT = dt.Rows[0]["REGDT"].ToString();
                }

                if (dt.Columns.Contains("EDITDT"))
                {
                    ViewBag.EDITDT = dt.Rows[0]["EDITDT"].ToString();
                }

                if (dt.Columns.Contains("FILE"))
                {
                    ViewBag.FILE = dt.Rows[0]["FILE"].ToString();
                }

                if (dt.Columns.Contains("FILE_NAME"))
                {
                    ViewBag.FILE_NAME = dt.Rows[0]["FILE_NAME"].ToString();
                }

                if (dt.Columns.Contains("FILE1"))
                {
                    ViewBag.FILE1 = dt.Rows[0]["FILE1"].ToString();
                }

                if (dt.Columns.Contains("FILE1_NAME"))
                {
                    ViewBag.FILE1_NAME = dt.Rows[0]["FILE1_NAME"].ToString();
                }

                if (dt.Columns.Contains("FILE2"))
                {
                    ViewBag.FILE2 = dt.Rows[0]["FILE2"].ToString();
                }

                if (dt.Columns.Contains("FILE2_NAME"))
                {
                    ViewBag.FILE2_NAME = dt.Rows[0]["FILE2_NAME"].ToString();
                }

                if (dt.Columns.Contains("CONTENT"))
                {
                    ViewBag.CONTENT = dt.Rows[0]["CONTENT"].ToString();
                }
            }

            return View();
        }


        public ActionResult adminLogin(LoginCls loginObj)
        {
            bool loginCheck = false;
            string strMessage = "";

            try
            {
                if (loginObj != null)
                {

                    strJson = CA.Con_adminLogin(loginObj.m_id, loginObj.pwd, memberKey);

                    strJson = String_Encrypt.decryptAES256(strJson);

                    DataSet resultDs = JsonConvert.DeserializeObject<DataSet>(strJson);

                    if (resultDs.Tables["Result"].Rows[0]["trxCode"].ToString() == "Y")
                    {
                        loginCheck = true;
                        strMessage = "로그인 성공";

                        #region // 로그인 성공시 정보를 Session에 저장하자
                        Session["admin_idx"] = loginObj.m_id;
                        #endregion
                    }
                    else if (resultDs.Tables["Result"].Rows[0]["trxCode"].ToString() == "N")
                    {
                        loginCheck = false;
                        strMessage = "로그인 정보가 없습니다.";
                        return Json(new { Success = loginCheck, Message = strMessage });
                    }
                    else if (resultDs.Tables["Result"].Rows[0]["trxCode"].ToString() == "E")
                    {
                        loginCheck = false;
                        strMessage = "에러가 발생하였습니다. 담당자에게 문의 하세요";
                        return Json(new { Success = loginCheck, Message = strMessage });
                    }
                    else
                    {
                        loginCheck = false;
                        strMessage = "없는 아이디거나 패스워드가 틀렸습니다. 다시 시도해 주세요";
                    }
                }
                else
                {
                    loginCheck = false;
                    strMessage = "로그인 정보가 없습니다.";
                }

                return Json(new { Success = loginCheck, Message = strMessage });

            }
            catch
            {
                loginCheck = false;
                strMessage = "로그인 실패했습니다. 관리자에게 문의 부탁드립니다.";
                return Json(new { Success = loginCheck, Message = strMessage });

            }
        }

        /// <summary>
        /// 로그아웃 버튼
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }

        public class noticeParam
        {
            public string Option { get; set; }
            public string Type { get; set; }
            public string SearchText { get; set; }
            public int Page { get; set; }
        }

        public string Notice_CallAjax(noticeParam rtnVal)
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

                    dt = CA.Con_NoticeList(rtnVal);

                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0) rtnJson = JsonConvert.SerializeObject(dt);
                    }
                }
                return rtnJson;
            }
            catch (Exception e)
            {
                return rtnJson;
            }
        }

        //입력-수정 모두 처리        
        //[AcceptVerbs(HttpVerbs.Post), ValidateInput(false)]
        [HttpPost, ValidateInput(false)]
        public ActionResult NoticeModify()
        {
            try
            {
                Hashtable htParam = new Hashtable();
                if (Request.Form.Count > 0)
                {
                    if (Request.Form.AllKeys.Contains("notice_id")) htParam.Add("NOTICE_ID", Request.Form["notice_id"]);
                    if (Request.Form.AllKeys.Contains("user_id")) htParam.Add("USR_ID", Request.Form["user_id"]);
                    if (Request.Form.AllKeys.Contains("title")) htParam.Add("TITLE", Request.Form["title"]);
                    if (Request.Form.AllKeys.Contains("s_type")) htParam.Add("S_TYPE", Request.Form["s_type"]);
                    if (Request.Form.AllKeys.Contains("notice_yn")) htParam.Add("NOTICE_YN", Request.Form["notice_yn"]);
                    if (Request.Form.AllKeys.Contains("content")) htParam.Add("CONTENT", Request.Form["content"]);
                    if (Request.Form.AllKeys.Contains("use_yn")) htParam.Add("USE_YN", Request.Form["use_yn"]);
                    if (Request.Form.AllKeys.Contains("file_del")) htParam.Add("FILE_DEL", Request.Form["file_del"]);
                    if (Request.Form.AllKeys.Contains("file1_del")) htParam.Add("FILE1_DEL", Request.Form["file1_del"]);
                    if (Request.Form.AllKeys.Contains("file2_del")) htParam.Add("FILE2_DEL", Request.Form["file2_del"]);

                    htParam.Add("FILE", "");
                    htParam.Add("FILE_NAME", "");
                    htParam.Add("FILE1", "");
                    htParam.Add("FILE1_NAME", "");
                    htParam.Add("FILE2", "");
                    htParam.Add("FILE2_NAME", "");

                    if (htParam.ContainsKey("NOTICE_ID"))
                    {
                        if (!string.IsNullOrEmpty(htParam["NOTICE_ID"].ToString())) //notice id가 있다! => update
                        {
                            #region //파일 삭제 로직
                            if (htParam.ContainsKey("FILE_DEL")) //파일삭제가 체크 되어있고
                            {
                                if (htParam["FILE_DEL"].ToString() == "y")  //삭제값이 y 이면
                                {
                                    NoticeFileDel(htParam["NOTICE_ID"].ToString(), 1);
                                    htParam["FILE"] = "";
                                    htParam["FILE_NAME"] = "";
                                }
                            }
                            else
                            {
                                htParam.Remove("FILE");
                            }

                            if (htParam.ContainsKey("FILE1_DEL")) //파일삭제가 체크 되어있고
                            {
                                if (htParam["FILE1_DEL"].ToString() == "y")  //삭제값이 y 이면
                                {
                                    NoticeFileDel(htParam["NOTICE_ID"].ToString(), 2);
                                    htParam["FILE1"] = "";
                                    htParam["FILE1_NAME"] = "";
                                }
                            }
                            else
                            {
                                htParam.Remove("FILE1");
                            }

                            if (htParam.ContainsKey("FILE2_DEL")) //파일삭제가 체크 되어있고
                            {
                                if (htParam["FILE2_DEL"].ToString() == "y")  //삭제값이 y 이면
                                {
                                    NoticeFileDel(htParam["NOTICE_ID"].ToString(), 3);
                                    htParam["FILE2"] = "";
                                    htParam["FILE2_NAME"] = "";
                                }
                            }
                            else
                            {
                                htParam.Remove("FILE2");
                            }
                            #endregion
                        }
                    }

                    //파일객체가 있다면
                    if (Request.Files.Count > 0)
                    {
                        var file = Request.Files[0];
                        var filename = "";
                        var file1 = Request.Files[1];
                        var file1name = "";
                        var file2 = Request.Files[2];
                        var file2name = "";

                        if (file != null && file.ContentLength > 0)
                        {
                            filename = Path.GetFileName(file.FileName);
                            string file_name = DateTime.Now.ToString("yyyyMMddhhmmssfff") + "_" + GetRandomChar(20) + Path.GetExtension(file.FileName);

                            NoticeFileDel(htParam["NOTICE_ID"].ToString(), 1);

                            var path = Path.Combine(Server.MapPath(_NoticeFilePath), file_name);
                            file.SaveAs(path);

                            htParam["FILE"] = file_name;
                            htParam["FILE_NAME"] = filename;
                        }

                        if (file1 != null && file1.ContentLength > 0)
                        {
                            file1name = Path.GetFileName(file1.FileName);
                            string file1_name = DateTime.Now.ToString("yyyyMMddhhmmssfff") + "_" + GetRandomChar(20) + Path.GetExtension(file1.FileName);

                            NoticeFileDel(htParam["NOTICE_ID"].ToString(), 2);

                            var path = Path.Combine(Server.MapPath(_NoticeFilePath), file1_name);
                            file1.SaveAs(path);

                            htParam["FILE1"] = file1_name;
                            htParam["FILE1_NAME"] = file1name;
                        }

                        if (file2 != null && file2.ContentLength > 0)
                        {
                            file2name = Path.GetFileName(file2.FileName);
                            string file2_name = DateTime.Now.ToString("yyyyMMddhhmmssfff") + "_" + GetRandomChar(20) + Path.GetExtension(file2.FileName);

                            NoticeFileDel(htParam["NOTICE_ID"].ToString(), 3);

                            var path = Path.Combine(Server.MapPath(_NoticeFilePath), file2_name);
                            file2.SaveAs(path);

                            htParam["FILE2"] = file2_name;
                            htParam["FILE2_NAME"] = file2name;
                        }
                    }

                    string strResult = "";

                    if (htParam.ContainsKey("NOTICE_ID"))
                    {
                        if (!string.IsNullOrEmpty(htParam["NOTICE_ID"].ToString())) //notice id가 있다! => update
                        {
                            strResult = CA.Con_NoticeUpdate(htParam);
                        }
                        else
                        {
                            //Insert
                            strResult = CA.Con_NoticeInsert(htParam);
                        }
                    }
                    else
                    {
                        //Insert
                        strResult = CA.Con_NoticeInsert(htParam);
                    }

                    strResult = String_Encrypt.decryptAES256(strResult);

                    //return Json(strResult);                    
                    return RedirectToAction("Notice");
                }
                //return Content("<script>저장 할 수 없습니다.</script>");
                return RedirectToAction("Notice");
            }
            catch (Exception e)
            {
                strJson = e.Message;
                return RedirectToAction("Notice");
                //return Json(strJson);
            }
        }

        //데이터 삭제
        public void NoticeFileDel(string Notice_ID, int FileIndex)
        {
            try
            {
                if (!string.IsNullOrEmpty(Notice_ID))
                {
                    dt = CA.Con_NoticeView(Notice_ID);

                    string strFile1 = "";
                    string strFile2 = "";
                    string strFile3 = "";
                    if (dt.Rows.Count > 0)
                    {
                        #region //File Delete

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            strFile1 = dt.Rows[i]["FILE"].ToString();
                            strFile2 = dt.Rows[i]["FILE1"].ToString();
                            strFile3 = dt.Rows[i]["FILE2"].ToString();

                            string FullFilePath1 = Server.MapPath(_NoticeFilePath) + strFile1;
                            string FullFilePath2 = Server.MapPath(_NoticeFilePath) + strFile2;
                            string FullFilePath3 = Server.MapPath(_NoticeFilePath) + strFile3;

                            switch (FileIndex)
                            {
                                case 0:
                                    if (System.IO.File.Exists(FullFilePath1))    //파일이 존재한다면
                                    {
                                        System.IO.FileInfo file = new System.IO.FileInfo(FullFilePath1);
                                        file.Delete();
                                    }
                                    if (System.IO.File.Exists(FullFilePath2))    //파일이 존재한다면
                                    {
                                        System.IO.FileInfo file = new System.IO.FileInfo(FullFilePath1);
                                        file.Delete();
                                    }
                                    if (System.IO.File.Exists(FullFilePath3))    //파일이 존재한다면
                                    {
                                        System.IO.FileInfo file = new System.IO.FileInfo(FullFilePath1);
                                        file.Delete();
                                    }
                                    break;
                                case 1:
                                    if (System.IO.File.Exists(FullFilePath1))    //파일이 존재한다면
                                    {
                                        System.IO.FileInfo file = new System.IO.FileInfo(FullFilePath1);
                                        file.Delete();
                                    }
                                    break;
                                case 2:
                                    if (System.IO.File.Exists(FullFilePath2))    //파일이 존재한다면
                                    {
                                        System.IO.FileInfo file = new System.IO.FileInfo(FullFilePath1);
                                        file.Delete();
                                    }
                                    break;
                                case 3:
                                    if (System.IO.File.Exists(FullFilePath3))    //파일이 존재한다면
                                    {
                                        System.IO.FileInfo file = new System.IO.FileInfo(FullFilePath1);
                                        file.Delete();
                                    }
                                    break;
                            }
                        }
                        #endregion

                        //string strJson = "";
                        //strJson = CA.Con_AdminNoticeDel(Notice_ID);
                    }
                }
            }
            catch (Exception e)
            {

            }
        }


        /// <summary>
        /// 데이터 삭제
        /// </summary>
        /// <param name="Notice_ID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Notice_DelAjax(string Notice_ID)
        {
            string strJson = "";

            try
            {
                if (Notice_ID != null)
                {
                    strJson = CA.Con_AdminNoticeDel(Notice_ID);
                }
                var result = new { Success = "True", Message = "Complete" };
                return Json(result, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                var result = new { Success = "False", Message = e.ToString() };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult NoticeEditor()
        {
            //_EditorFilePath
            string ckEditor = System.Web.HttpContext.Current.Request["CKEditor"];
            string funcNum = System.Web.HttpContext.Current.Request["CKEditorFuncNum"];
            string langCode = System.Web.HttpContext.Current.Request["langCode"];

            try
            {
                int total = System.Web.HttpContext.Current.Request.Files.Count;
                if (total == 0) return Content(string.Format(scriptTag, funcNum, "", "no File"), "text/html");

                HttpPostedFile theFile = System.Web.HttpContext.Current.Request.Files[0];
                string strFilename = theFile.FileName;
                string sFileName = DateTime.Now.ToString("yyyyMMddhhmmssfff") + GetRandomChar(20) + Path.GetExtension(theFile.FileName);//Path.GetFileName(strFilename);
                string name = Path.Combine(Server.MapPath(_EditorFilePath), sFileName);
                theFile.SaveAs(name);
                string url = _EditorFilePath + sFileName.Replace("'", "\'");

                return Content(
                    string.Format(scriptTag, funcNum, HttpUtility.JavaScriptStringEncode(url ?? ""), ""),
                    "text/html"
                    );
            }
            catch (Exception ex)
            {
                return Content(
                    string.Format(scriptTag, funcNum, "", ex.ToString()),
                    "text/html"
                    );
            }

        }

        public static string GetRandomChar(int _totLen)
        {
            Random rand = new Random();
            string input = "abcdefghijklmnopqrstuvwxyz0123456789";
            var chars = Enumerable.Range(0, _totLen).Select(x => input[rand.Next(0, input.Length)]);
            return new string(chars.ToArray());
        }

        /// <summary>
        /// 공지사항 삭제
        /// </summary>
        public class noticeDel
        {
            public string Notice_ID { get; set; }
        }

        /*****************************************************************************************************************/


        /// <summary>
		/// 파일 업로드
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		[HttpPost]
        public ActionResult fnInsertVehicle()
        {
            DataRow dr;

            try
            {
                string strMNGT_NO = Request.Form["MNGT_NO"];
                string strSEQ = Request.Form["SEQ"];
                string strUSR_ID = Request.Form["USR_ID"];
                string strCAR_DIV = Request.Form["CAR_DIV"];
                string strCAR_DIV_CODE = Request.Form["CAR_DIV_CODE"];
                string strCAR_NAME = Request.Form["CAR_NAME"];
                string strSHORTHAND = Request.Form["SHORTHAND"];
                string strCAR_WIDTH = Request.Form["CAR_WIDTH"];
                string strTOP_HEIGHT = Request.Form["TOP_HEIGHT"];
                string strBOTTOM_HEIGHT = Request.Form["BOTTOM_HEIGHT"];
                string strCAR_AREA = Request.Form["CAR_AREA"];
                string strCAR_WEIGHT = Request.Form["CAR_WEIGHT"];
                string strCAR_CBM = Request.Form["CAR_CBM"];
                string strTOTAL_HEIGHT = Request.Form["TOTAL_HEIGHT"];
                string strRMK = Request.Form["RMK"];
                string strCRUD = Request.Form["DB_CRUD"];
                string strIS_FILE = Request.Form["IS_FILE"];

                HttpFileCollectionBase files = Request.Files;

                string strIMG_NAME = "";
                string strIMG_PATH = "";
                string strDBIMG_PATH = "";
                string strREPLACE_IMG_NAME = "";

                //파일 저장
                if (files.Count > 0)
                {
                    /////////////////////////////////////////////////////////////////////////////폴더 생성/////////////////////////////////////////////////////////////////////////////				
                    //strIMG_PATH = Server.MapPath("~/Files/Vehicle") + "\\" + DateTime.Now.ToString("yyyyMMdd");
                    strIMG_PATH = Server.MapPath("~/Files/Vehicle") + "\\" + DateTime.Now.ToString("yyyyMMdd");
                    strDBIMG_PATH = "/Files/Vehicle" + "\\" + DateTime.Now.ToString("yyyyMMdd");

                    //현재 날짜 파일 생성 - 날짜로 계산
                    DirectoryInfo di = new DirectoryInfo(strIMG_PATH); //폴더 관련 객체
                    if (di.Exists != true)
                    {
                        di.Create();
                    }

                    string strDateTimeDi = DateTime.Now.ToString("yyyyMMddHHmmssFFF");

                    if (strMNGT_NO == "")
                    {
                        strMNGT_NO = DateTime.Now.ToString("yyyyMMddHHmmssFFF");
                    }
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                    //////////////////////////////////////////////////////////////////////////첨부파일 저장/////////////////////////////////////////////////////////////////////////////
                    System.IO.FileInfo fi;

                    for (int i = 0; i < files.Count; i++)
                    {
                        try
                        {
                            HttpPostedFileBase file = files[i];
                            //파일 채번 룰 , 관리번호_업로드아이디_파일명
                            strREPLACE_IMG_NAME = strMNGT_NO + "_" + strDateTimeDi + "_" + file.FileName;
                            strIMG_NAME = file.FileName;

                            fi = new System.IO.FileInfo(strIMG_PATH + "/" + strREPLACE_IMG_NAME);

                            //파일 이름이 벌써 있는 경우 채번을 다시 시도. (무한루프)
                            while (true)
                            {
                                if (fi.Exists)
                                {
                                    strMNGT_NO = DateTime.Now.ToString("yyyyMMddHHmmssFFF");
                                    strREPLACE_IMG_NAME = strMNGT_NO + "_" + strUSR_ID + "_" + file.FileName;
                                    fi = new System.IO.FileInfo(strIMG_PATH + "/" + strREPLACE_IMG_NAME);
                                }
                                else
                                {
                                    break;
                                }
                            }

                            if (fi.Exists != true)
                            {
                                //파일만 저장
                                file.SaveAs(strIMG_PATH + "/" + strREPLACE_IMG_NAME);
                            }
                        }
                        catch (Exception ex)
                        {
                            fi = new FileInfo(strIMG_PATH + "/" + strREPLACE_IMG_NAME);

                            if (fi.Exists == true)
                            {
                                fi.Delete();
                            }

                            strJson = Comm.MakeJson("E", ex.Message);

                            return Json(strJson);
                        }
                    }
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                }

                //파일 데이터 까지 해서 데이터 저장
                //데이터 테이블 만들기
                dt = new DataTable("VEHICLE");
                dt.Columns.Add("MNGT_NO");
                dt.Columns.Add("SEQ");
                dt.Columns.Add("CAR_DIV");
                dt.Columns.Add("CAR_DIV_CODE");
                dt.Columns.Add("CAR_NAME");
                dt.Columns.Add("SHORTHAND");
                dt.Columns.Add("CAR_WIDTH");
                dt.Columns.Add("TOP_HEIGHT");
                dt.Columns.Add("BOTTOM_HEIGHT");
                dt.Columns.Add("CAR_AREA");
                dt.Columns.Add("CAR_WEIGHT");
                dt.Columns.Add("CAR_CBM");
                dt.Columns.Add("TOTAL_HEIGHT");
                dt.Columns.Add("RMK");
                dt.Columns.Add("USR_ID");
                dt.Columns.Add("IMG_NAME");
                dt.Columns.Add("IMG_PATH");
                dt.Columns.Add("REPLACE_IMG_NAME");
                dt.Columns.Add("IS_FILE");

                dr = dt.NewRow();

                dr["MNGT_NO"] = strMNGT_NO;
                dr["SEQ"] = strSEQ;
                dr["CAR_DIV"] = strCAR_DIV;
                dr["CAR_DIV_CODE"] = strCAR_DIV_CODE;
                dr["CAR_NAME"] = strCAR_NAME;
                dr["SHORTHAND"] = strSHORTHAND;
                dr["CAR_WIDTH"] = strCAR_WIDTH;
                dr["TOP_HEIGHT"] = strTOP_HEIGHT;
                dr["BOTTOM_HEIGHT"] = strBOTTOM_HEIGHT;
                dr["CAR_AREA"] = strCAR_AREA;
                dr["CAR_WEIGHT"] = strCAR_WEIGHT;
                dr["CAR_CBM"] = strCAR_CBM;
                dr["TOTAL_HEIGHT"] = strTOTAL_HEIGHT;
                dr["RMK"] = strRMK;
                dr["USR_ID"] = strUSR_ID;

                if (files.Count > 0)
                {
                    dr["IMG_NAME"] = strIMG_NAME;
                    dr["IMG_PATH"] = strDBIMG_PATH;
                    dr["REPLACE_IMG_NAME"] = strREPLACE_IMG_NAME;
                }

                dr["IS_FILE"] = strIS_FILE;

                dt.Rows.Add(dr);


                ////파일 데이터 넘기기~
                //if (strCRUD == "INSERT")
                //{
                //    strResult = CA.Con_InsertVehicle(dt);
                //}
                //else if (strCRUD == "UPDATE")
                //{
                //    //파일을 어떻게 할까..

                //    //실제 파일이 변경 된건지도 확인이 필요
                //    strResult = CA.Con_UpdateVehicle(dt);
                //}

                strJson = String_Encrypt.decryptAES256(strResult);

                return Json(strJson);
            }
            catch (Exception e)
            {
                strJson = Comm.MakeJson("E", e.Message);

                return Json(strJson);
            }
        }

        //// <summary>
        /// 관리자 관리 페이지 데이터 검색
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult fnSearchPort(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = String_Encrypt.encryptAES256(vJsonData);

                strResult = CA.Con_SearchPort(vEncodeData);

                strJson = String_Encrypt.decryptAES256(strResult);

                return Json(strJson);
            }
            catch (Exception e)
            {
                strJson = e.Message;
                return Json(strJson);
            }
        }



        //관리자 관리 페이지 컨트롤러 숨김
        #region
        //// <summary>
        /// 관리자 관리 페이지 데이터 검색
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult fnSearchMember(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = String_Encrypt.encryptAES256(vJsonData);

                strResult = CA.Con_SearchMember(vEncodeData);

                strJson = String_Encrypt.decryptAES256(strResult);

                return Json(strJson);
            }
            catch (Exception e)
            {
                strJson = e.Message;
                return Json(strJson);
            }
        }

        //// <summary>
        /// 아이디 중복 체크
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult fnCheckIDMember(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = String_Encrypt.encryptAES256(vJsonData);

                strResult = CA.Con_CheckIDMember(vEncodeData);

                strJson = String_Encrypt.decryptAES256(strResult);

                return Json(strJson);
            }
            catch (Exception e)
            {
                strJson = e.Message;
                return Json(strJson);
            }
        }

        //// <summary>
        /// 관리자 관리 등록
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult fnInsertMember(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = String_Encrypt.encryptAES256(vJsonData);

                strResult = CA.Con_InsertMember(vEncodeData);

                strJson = String_Encrypt.decryptAES256(strResult);

                return Json(strJson);
            }
            catch (Exception e)
            {
                strJson = e.Message;
                return Json(strJson);
            }
        }

        //// <summary>
        /// 관리자 관리 삭제
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult fnDeleteMember(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = String_Encrypt.encryptAES256(vJsonData);

                strResult = CA.Con_DeleteMember(vEncodeData);

                strJson = String_Encrypt.decryptAES256(strResult);

                return Json(strJson);
            }
            catch (Exception e)
            {
                strJson = e.Message;
                return Json(strJson);
            }
        }

        //// <summary>
        /// 관리자 관리 삭제
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult fnModifyMember(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = String_Encrypt.encryptAES256(vJsonData);

                strResult = CA.Con_ModifyMember(vEncodeData);

                strJson = String_Encrypt.decryptAES256(strResult);

                return Json(strJson);
            }
            catch (Exception e)
            {
                strJson = e.Message;
                return Json(strJson);
            }
        }

        //// <summary>
        /// 관리자 관리 삭제
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult fnModifyPort(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = String_Encrypt.encryptAES256(vJsonData);

                strResult = CA.Con_ModifyPort(vEncodeData);

                strJson = String_Encrypt.decryptAES256(strResult);

                return Json(strJson);
            }
            catch (Exception e)
            {
                strJson = e.Message;
                return Json(strJson);
            }
        }
        #endregion

        //// <summary>
        /// 포트 등록
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult fnInsertPort(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = String_Encrypt.encryptAES256(vJsonData);

                strResult = CA.Con_InsertPort(vEncodeData);

                strJson = String_Encrypt.decryptAES256(strResult);

                return Json(strJson);
            }
            catch (Exception e)
            {
                strJson = e.Message;
                return Json(strJson);
            }
        }

        //// <summary>
        /// 아이디 중복 체크
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult fnCheckPort(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = String_Encrypt.encryptAES256(vJsonData);

                strResult = CA.Con_CheckPort(vEncodeData);

                strJson = String_Encrypt.decryptAES256(strResult);

                return Json(strJson);
            }
            catch (Exception e)
            {
                strJson = e.Message;
                return Json(strJson);
            }
        }


        //// <summary>
        /// 관리자 관리 삭제
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult fnDeletePort(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = String_Encrypt.encryptAES256(vJsonData);

                strResult = CA.Con_DeletePort(vEncodeData);

                strJson = String_Encrypt.decryptAES256(strResult);

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
        public ActionResult fnSetStartPort(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = String_Encrypt.encryptAES256(vJsonData);

                strResult = CA.Con_SetStartPort(vEncodeData);

                strJson = String_Encrypt.decryptAES256(strResult);

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
        public ActionResult fnSetUsePort(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = String_Encrypt.encryptAES256(vJsonData);

                strResult = CA.Con_SetUsePort(vEncodeData);

                strJson = String_Encrypt.decryptAES256(strResult);

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
        public ActionResult fnSetBoundPort(JsonData value)
        {
            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = String_Encrypt.encryptAES256(vJsonData);

                strResult = CA.Con_SetBoundPort(vEncodeData);

                strJson = String_Encrypt.decryptAES256(strResult);

                return Json(strJson);
            }
            catch (Exception e)
            {
                strJson = e.Message;
                return Json(strJson);
            }
        }

        public ActionResult PortWrite(string Port_no)
        {
            //신규, 수정
            if (Port_no == null)
            {
                return View();
            }
            else
            {
                //Search 한번 하고 고고
                strResult = CA.Con_SearchPortModify(Port_no);
                strResult = String_Encrypt.decryptAES256(strResult);
                DataSet ds = JsonConvert.DeserializeObject<DataSet>(strResult);

                DataTable dt_result = ds.Tables["Result"];

                if (dt_result.Rows[0]["trxCode"].ToString() == "Y")
                {
                    dt = ds.Tables["PORT"];

                    if (dt.Rows[0]["PORT_NO"] != null)
                    {
                        ViewBag.PORT_NO = dt.Rows[0]["PORT_NO"];
                    }

                    if (dt.Rows[0]["START_PORT"] != null)
                    {
                        ViewBag.START_PORT = dt.Rows[0]["START_PORT"];
                    }

                    if (dt.Rows[0]["BOUND_PORT"] != null)
                    {
                        ViewBag.BOUND_PORT = dt.Rows[0]["BOUND_PORT"];
                    }

                    if (dt.Rows[0]["LOC_CD"] != null)
                    {
                        ViewBag.LOC_CD = dt.Rows[0]["LOC_CD"];
                    }

                    if (dt.Rows[0]["LOC_NM"] != null)
                    {
                        ViewBag.LOC_NM = dt.Rows[0]["LOC_NM"];
                    }

                    if (dt.Rows[0]["USE_YN"] != null)
                    {
                        ViewBag.USE_YN = dt.Rows[0]["USE_YN"];
                    }

                    if (dt.Rows[0]["INS_USR"] != null)
                    {
                        ViewBag.INS_USR = dt.Rows[0]["INS_USR"];
                    }

                    if (dt.Rows[0]["INS_YMD"] != null)
                    {
                        ViewBag.INS_YMD = dt.Rows[0]["INS_YMD"];
                    }

                }
                return View();
            }
        }

    }
}
