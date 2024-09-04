using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Ionic.Zip;
using Newtonsoft.Json;
using SSGM_ELVISPRIME_COMMON.Controllers;
using SSGM_ELVISPRIME_COMMON.YJIT_Utils;
using System.Text.RegularExpressions;

namespace SSGM_ELVISPRIME_WEB.Controllers.API.Common
{
    public class HP_FileController : Controller
    {
        FileInfo fi;
        private DataTable dt;
        string strEmailJson;

        Encryption ec = new Encryption(); //DB_Data - Encryption
        Con_Common Con_Common = new Con_Common();
        Con_File Con_File = new Con_File();

        public class JsonGetData
        {
            public string FILE_NM { get; set; }
            public string FILE_PATH { get; set; }
            public string FILE_MNGTNO { get; set; }
            public string FILE_SEQ { get; set; }
            public string FILE_FORMID { get; set; }
            public string REAL_FILE_NM { get; set; }
        }

        public class FormData
        {
            public string strFILENM { get; set; }
            public string strPATH { get; set; }
        }

        public class JsonData
        {
            public string vJsonData { get; set; }
        }

        public class JsonData_File
        {
            public string FILE_INFO { get; set; }
        }

        public class RtnFilesInfo
        {
            public string FILE_NAME { get; set; }
            public string FILE_NM { get; set; }
            public string FILE_PATH { get; set; }
            public string REPLACE_FILE_NM { get; set; }
            public string MNGT_NO { get; set; }
            public string INS_USR { get; set; }
            public string SEQ { get; set; }
        }

        [HttpGet]
        public ActionResult DownloadFile(RtnFilesInfo value)
        {
            string strFILE_NM = value.FILE_NM;
            string strFILE_PATH = value.FILE_PATH;
            string strREPLACE_FILE_NM = value.REPLACE_FILE_NM;

            try
            {
                System.IO.FileInfo fi = new System.IO.FileInfo(Server.MapPath("~/Content/TempFiles/") + strFILE_NM);

                if (fi.Exists)
                {
                    return File(fi.FullName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", strREPLACE_FILE_NM);
                }
                else
                {
                    return Content("<script>alert('파일이 존재하지 않습니다.'); window.history.back();</script>");
                }
            }
            catch (Exception ex)
            {
                return Content("<script>alert('" + ex.Message + "')</script>");
            }
        }

        ///// <summary>
        ///// 관리자 페이지 - 차량제원 관리 파일 다운로드
        ///// </summary>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //[HttpGet]
        //public ActionResult Vehicle_DownloadFile(RtnFilesInfo value)
        //{
        //	string strFILE_NM = value.FILE_NM;
        //	string strFILE_PATH = value.FILE_PATH;
        //	string strREPLACE_FILE_NM = value.REPLACE_FILE_NM;

        //	try
        //	{
        //		//System.IO.FileInfo fi = new System.IO.FileInfo(Server.MapPath("~/Files/Vehicle/") + REPLACE_FILE_NM);
        //		//System.IO.FileInfo fi = new System.IO.FileInfo(strFILE_PATH+"/" + strREPLACE_FILE_NM);
        //		System.IO.FileInfo fi = new System.IO.FileInfo(Server.MapPath("~"+ strFILE_PATH) + "/" + strREPLACE_FILE_NM);

        //		if (fi.Exists)
        //		{
        //			//return File(fi.FullName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", strREPLACE_FILE_NM);
        //			return File(fi.FullName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", strFILE_NM);
        //		}
        //		else
        //		{
        //			return Content("<script>alert('파일이 존재하지 않습니다.'); window.history.back();</script>");
        //		}
        //	}
        //	catch (Exception ex)
        //	{
        //		return Content("<script>alert('" + ex.Message + "')</script>");
        //	}
        //}

        /// <summary>
        /// 이메일 전송에 필요한 파일 업로드
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Upload_EmailFiles()
        {
            string strJson = "";

            try
            {
                HttpFileCollectionBase files = Request.Files;

                //파일이 없으면 그냥 if 돌리기
                if (files[0].FileName != "")
                {
                    ///////////////////////////////////////////////////폴더 생성 및 현재 날 기준 하루 전 폴더 전부 삭제////////////////////////////////////////////////////////////////				
                    string path = Server.MapPath("~/Files/Online") + "\\" + DateTime.Now.ToString("yyyyMMdd");
                    string zipPath = path;

                    deleteFolder(Server.MapPath("~/Files/Online")); //파일 삭제 (Default 세팅 하루 이전 폴더 완전 삭제)

                    //현재 날짜 파일 생성
                    DirectoryInfo di = new DirectoryInfo(path); //폴더 관련 객체
                    if (di.Exists != true)
                    {
                        di.Create();
                    }

                    //압축 파일 넣을 경로 생성
                    string strDateTimeDi = DateTime.Now.ToString("yyyyMMddHHmmssFFF");
                    di.Refresh();

                    path += "/" + strDateTimeDi;

                    di = new DirectoryInfo(path);
                    if (di.Exists != true)
                    {
                        di.Create();
                    }
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


                    //////////////////////////////////////////////////////////////////////////첨부파일 저장/////////////////////////////////////////////////////////////////////////////
                    DataTable dt = new DataTable("ZIP_LIST");
                    dt.Columns.Add("ZIP_PATH");

                    System.IO.FileInfo fi;
                    string strDateTime = ""; //년월일시분초밀리초 시간은 계속 변해서 변수 사용.

                    for (int i = 0; i < files.Count; i++)
                    {
                        try
                        {
                            HttpPostedFileBase file = files[i];
                            strDateTime = DateTime.Now.ToString("yyyyMMddHHmmssFFF") + "." + file.FileName.Substring(file.FileName.LastIndexOf(".") + 1, file.FileName.Length - (file.FileName.LastIndexOf(".") + 1));

                            fi = new System.IO.FileInfo(path + "/" + strDateTime);

                            //파일 이름이 벌써 있는 경우 채번을 다시 시도. (무한루프)
                            while (true)
                            {
                                if (fi.Exists)
                                {
                                    strDateTime = DateTime.Now.ToString("yyyyMMddHHmmssFFF") + "." + file.FileName.Substring(file.FileName.LastIndexOf(".") + 1, file.FileName.Length - (file.FileName.LastIndexOf(".") + 1));  //파일이 있을 경우 다시 채번.
                                    fi = new System.IO.FileInfo(path + "/" + strDateTime);
                                }
                                else
                                {
                                    break;
                                }
                            }

                            if (fi.Exists != true)
                            {
                                //파일만 저장
                                file.SaveAs(path + "/" + file.FileName);
                            }
                        }
                        catch (Exception ex)
                        {
                            for (int j = 0; j < dt.Rows.Count; j++)
                            {
                                fi = new FileInfo(Server.MapPath("~/Files/Online/") + dt.Rows[j][1]);

                                if (fi.Exists == true)
                                {
                                    fi.Delete();
                                }
                            }

                            strJson = MakeJson("E", ex.Message, dt);

                            return Json(strJson);
                        }
                    }
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


                    //////////////////////////////////////////////////////////////////////////추가한 파일 압축//////////////////////////////////////////////////////////////////////
                    using (ZipFile zip = new ZipFile())
                    {
                        DirectoryInfo dir = new DirectoryInfo(Server.MapPath("~/Files/Online") + "/" + DateTime.Now.ToString("yyyyMMdd") + "/" + strDateTimeDi);
                        FileInfo[] infos = dir.GetFiles();
                        string[] files1 = new string[infos.Length];

                        for (int i = 0; i < infos.Length; i++)
                        {
                            files1[i] = infos[i].FullName;
                        }

                        byte[] b = null;
                        string d = null;

                        foreach (string file in files1)
                        {
                            // 시스템의 기본 인코딩 타입으로 읽어서
                            b = System.Text.Encoding.Default.GetBytes(file);
                            // IBM437로 변환해 준다.
                            d = System.Text.Encoding.GetEncoding("IBM437").GetString(b);

                            zip.AddEntry(d, "", System.IO.File.ReadAllBytes(file));
                        }

                        zip.Save(zipPath + "/" + strDateTimeDi + ".zip");
                    }

                    //파일 압축하기					
                    //using (ZipFile zip = new ZipFile())
                    //{				    
                    //    zip.AddDirectory(Server.MapPath("~/Files/Online") +"/"+DateTime.Now.ToString("yyyyMMdd")+"/"+strDateTimeDi);
                    //    zip.Save(zipPath+"/"+strDateTimeDi+".zip");

                    //    DataRow dr = dt.NewRow();

                    //    dr["ZIP_PATH"] = Server.MapPath("~/Files/Online") +"/"+DateTime.Now.ToString("yyyyMMdd")+"/"+strDateTimeDi+".zip";

                    //    dt.Rows.Add(dr);
                    //}
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                    //데이터 정리
                    DataRow dr = dt.NewRow();

                    //dr["ZIP_PATH"] = Server.MapPath("~/Files/Online") +"/"+DateTime.Now.ToString("yyyyMMdd")+"/"+strDateTimeDi+".zip";
                    dr["ZIP_PATH"] = ConfigurationManager.AppSettings["HomeUrl"] + "/Files/Online/" + DateTime.Now.ToString("yyyyMMdd") + "/" + strDateTimeDi + ".zip"; //zip 파일 정보 데이터 로우 저장

                    dt.Rows.Add(dr);

                    //압축된 파일 정보 담기
                    strJson = MakeJson("Y", "Success", dt);
                }
                else
                {
                    strJson = MakeJson("Y", "Success");
                }
            }
            catch (Exception ex)
            {
                strJson = MakeJson("E", ex.Message, dt);
            }

            return Json(strJson);
        }

        private static void deleteFolder(string folderDir)
        {
            try
            {
                int deleteDay = 1;
                DirectoryInfo di = new DirectoryInfo(folderDir);
                if (di.Exists)
                {
                    DirectoryInfo[] dirInfo = di.GetDirectories();
                    string lDate = DateTime.Today.AddDays(-deleteDay).ToString("yyyyMMdd");
                    foreach (DirectoryInfo dir in dirInfo)
                    {
                        if (lDate.CompareTo(dir.Name.ToString()) > 0)
                        {
                            dir.Attributes = FileAttributes.Normal;
                            dir.Delete(true);
                        }
                    }
                }
            }
            catch (Exception) { }
        }

        /// <summary>
        /// 파일 업로드
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Upload_Files()
        {
            UTF8Encoding UTF8_Encodeing = new UTF8Encoding();
            DataTable dt = new DataTable();
            DataRow dr;
            FileInfo fi;
            Boolean isSuccess = true; //파일 업로드 성공 or 실패

            string strJson = "";
            string strResult = "";
            string SavePath = "/EDMS/WEB/" + ConfigurationManager.AppSettings["Domain"].ToString() + "/" + ConfigurationManager.AppSettings["OFFICE_CD"].ToString() + "/" + DateTime.Now.ToString("yyyyMMdd") + "/";
            string strFilePath = "";
            string strMNGT_NO = Request.Form["MNGT_NO"];
            //string strDOC_NO = Request.Form["DOC_NO"]; 부킹번호로 DOC_NO 보내기
            string strDOC_NO = Request.Form["MNGT_NO"];
            string strOFFICE_CD = Request.Form["OFFICE_CD"];
            string strDOC_TYPE = Request.Form["DOC_TYPE"];
            string strSEQ = Request.Form["SEQ"];

            HttpFileCollectionBase files = Request.Files;

            //데이터 split(여러개 있을 경우)

            //dt 컬럼 만들기
            dt = new DataTable("FILE_INFO");
            dt.Columns.Add("MNGT_NO");
            dt.Columns.Add("FILE_NM");
            dt.Columns.Add("DOC_TYPE");
            dt.Columns.Add("DOC_NO");
            dt.Columns.Add("FILE_PATH");
            dt.Columns.Add("INS_USR");
            dt.Columns.Add("INS_YMD");
            dt.Columns.Add("INS_HM");
            dt.Columns.Add("OFFICE_CD");
            dt.Columns.Add("FILE_SIZE");
            dt.Columns.Add("ORD_NO");
            dt.Columns.Add("SYS_ID");
            dt.Columns.Add("FORM_ID");

            try
            {
                string sUploadHandler = System.Configuration.ConfigurationManager.AppSettings["Url"] + "wcf/UploadHandler.aspx";
                System.Net.WebClient wc = new System.Net.WebClient();
                byte[] responseArray;

                NameValueCollection myQueryStringCollection = new NameValueCollection();
                // 상대경로
                myQueryStringCollection.Add("SavePath", SavePath); // Ex)  SavePath  :  "/EMAIL/SEND/" 

                string ChkFileNM = Path.GetFileName(files[0].FileName);

                if (ChkFileNM != "")
                {
                    //foreach는 사용할 수 없습니다.
                    for (int i = 0; i < files.Count; i++)
                    {
                        string InputFileName = "";
                        string strFileSize = "";
                        string strNowTime = "_" + DateTime.Now.ToString("yyyyMMddHHmmssffffff");
                        string strRealFileNM = "";

                        HttpPostedFileBase file = files[i]; //스플릿 for문 마지막에 있는것만 확장자로 ㄱㄱ

                        if (file.FileName != "" && file.ContentLength != 0)
                        {
                            if (file != null)
                            {
                                string strFileName = "";

                                InputFileName = Path.GetFileName(file.FileName); //파일 이름

                                //특수문자 및 공백 없애기
                                InputFileName = Regex.Replace(InputFileName, @"[/\+:*?<>|""#]", "");
                                InputFileName = InputFileName.Replace(" ", "");

                                strRealFileNM = InputFileName;//파일 이름

                                string[] fileinfo = InputFileName.Split('.');
                                for (int j = 0; j < fileinfo.Length - 1; j++)
                                {
                                    //.이 파일명 사이에 있을 경우						
                                    if (j == fileinfo.Length - 2)
                                    {
                                        strFileName += fileinfo[j];
                                    }
                                    else
                                    {
                                        strFileName += fileinfo[j] + ".";
                                    }
                                }

                                //엘비스 파일 => 관리번호_SEQ_파일네임.확장자
                                //InputFileName = strFileName + strNowTime + "." + fileinfo[fileinfo.Length - 1]; 
                                InputFileName = strMNGT_NO + "_" + (Int32.Parse(strSEQ) + 1) + "_" + InputFileName;

                                strFileSize = file.ContentLength.ToString();
                                //파일 사이즈						
                                strFilePath = Path.Combine(Server.MapPath("~/Files/TEMP/") + InputFileName); //날짜 붙혀서 보내기

                                //Save file to server folder  
                                file.SaveAs(strFilePath);
                            }

                            if (file != null && file.ContentLength > 0)
                            {
                                wc.QueryString = myQueryStringCollection;
                                //responseArray = wc.UploadFile(sUploadHandler, "POST", strFilePath);
                                responseArray = wc.UploadFile(sUploadHandler, "POST", strFilePath);
                                strResult = UTF8_Encodeing.GetString(responseArray);
                            }
                            else
                            {
                                responseArray = System.Text.Encoding.ASCII.GetBytes("N\n Upload failed!");
                                strResult = UTF8_Encodeing.GetString(responseArray);
                            }

                            //foreach
                            if (strResult.StartsWith("Y"))
                            {
                                dr = dt.NewRow();

                                dr["MNGT_NO"] = strMNGT_NO;
                                dr["FILE_NM"] = strRealFileNM;
                                dr["FILE_PATH"] = SavePath;
                                dr["FILE_SIZE"] = strFileSize;
                                dr["DOC_TYPE"] = strDOC_TYPE;
                                dr["DOC_NO"] = strDOC_NO;
                                dr["OFFICE_CD"] = strOFFICE_CD;
                                dr["SYS_ID"] = "WEB";
                                dr["FORM_ID"] = "SimpleBooking";
                                dr["INS_USR"] = "WEB";
                                dr["INS_YMD"] = DateTime.Now.ToString("yyyyMMdd");
                                dr["INS_HM"] = DateTime.Now.ToString("HHmmss");

                                dt.Rows.Add(dr);
                            }
                            else
                            {
                                isSuccess = false;
                            }
                        }

                        //파일 TEMP 삭제 로직
                        fi = new System.IO.FileInfo(strFilePath);

                        if (fi.Exists)
                        {
                            fi.Delete();
                        }
                    }//end for
                }

                //파일 업로드 전부 성공 했을 경우
                if (isSuccess)
                {
                    //파일 DB 정보 insert 
                    strResult = Con_File.Con_fnSetBKFileUpload(dt);
                    strJson = ec.decryptAES256(strResult);
                    //strJson = MakeJson("Y", "파일 디비 저장 성공");
                }
                else
                {
                    strJson = MakeJson("N", "파일 디비 저장 실패", dt);

                    //만약 파일 업로드 실패 시 데이터 삭제
                    //if (strREQ_SVC_Split[0] == "SEA")
                    //{
                    //    strJson = apiInquiry.fnSimpleBk_Delete_SEA(strMNGT_NO_Split[0]); //파일 업로드 실패로 ORDER 지우기
                    //} 
                    //else if (strREQ_SVC_Split[0] == "AIR")
                    //{
                    //    strJson = apiInquiry.fnSimpleBk_Upload_AIR(strMNGT_NO_Split[0]); //파일 업로드 실패로 ORDER 지우기
                    //}
                }

                return Json(strJson);
            }
            catch (Exception e)
            {
                return Json("[Error]" + e.Message);
            }
        }

        /// <summary>
        /// 메일 전송 기능
        /// </summary>
        /// <returns></returns>
        //public ActionResult fnSendEmail(DataTable dt)
        //{
        //	string strResult;
        //
        //	try
        //	{
        //		#region
        //		System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient("mail.yjit.co.kr", 587)				
        //		{
        //			UseDefaultCredentials = false, // 시스템에 설정된 인증 정보를 사용하지 않는다. 
        //			EnableSsl = true,  // SSL을 사용한다. 
        //			DeliveryMethod = SmtpDeliveryMethod.Network, // 이걸 하지 않으면 Gmail에 인증을 받지 못한다. 
        //			Credentials = new System.Net.NetworkCredential("mailmaster@yjit.co.kr", "Yjit0921)#$%"),					
        //			Timeout = 100000
        //		};
        //
        //		MailAddress from = new MailAddress(dt.Rows[0]["Email"].ToString());				
        //		MailAddress to = new MailAddress("twkim@yjit.co.kr");
        //
        //		MailMessage message = new MailMessage(from, to);
        //		message.Body = "";
        //		//message.Body = "";
        //		message.IsBodyHtml = true;
        //		message.BodyEncoding = System.Text.Encoding.UTF8;
        //		message.Subject = "[은산해운항공] 견적 문의 입니다. (" + dt.Rows[0]["Title"].ToString() + ")";
        //		message.SubjectEncoding = System.Text.Encoding.UTF8;
        //
        //		//첨부파일 로직                
        //		//message.Attachments.Add(new Attachment(new FileStream(@"" + dt.Rows[0]["ZIP_PATH"].ToString() + "", FileMode.Open, FileAccess.Read), "첨부파일.zip"));
        //
        //		//서버 인증서의 유효성 검사하는 부분을 무조건 true 
        //		System.Net.ServicePointManager.ServerCertificateValidationCallback += (s, cert, chain, sslPolicyErrors) => true;
        //		client.Send(message);
        //		message.Dispose();
        //		#endregion
        //
        //		//이메일 성공시 Update
        //		//SetEmailLog("Y", "성공");
        //
        //		strResult = MakeJson("Y", "이메일 전송 성공");				
        //
        //		return Json(strEmailJson);
        //	}
        //	catch (Exception e)
        //	{
        //		//이메일 실패시 Update
        //		//SetEmailLog("N", e.Message);
        //
        //		strResult = MakeJson("N", e.Message);				
        //		return Json(strEmailJson);
        //	}
        //}


        public string MakeJson(string status, string Msg, DataTable args)
        {
            try
            {
                string json = "";

                if (status == "Y")
                {
                    DataSet ds = new DataSet();
                    DataTable dt = new DataTable();
                    dt.TableName = "Result";
                    dt.Columns.Add("trxCode");
                    dt.Columns.Add("trxMsg");
                    DataRow row1 = dt.NewRow();
                    row1["trxCode"] = status;
                    row1["trxMsg"] = Msg;
                    dt.Rows.Add(row1);
                    ds.Tables.Add(dt);
                    if (args.Rows.Count > 0)
                    {
                        ds.Tables.Add(args);
                    }
                    json = JsonConvert.SerializeObject(ds, Formatting.Indented);
                }
                else if (status == "E")
                {
                    DataSet ds = new DataSet();
                    DataTable dt = new DataTable();
                    dt.TableName = "Result";
                    dt.Columns.Add("trxCode");
                    dt.Columns.Add("trxMsg");
                    DataRow row1 = dt.NewRow();
                    row1["trxCode"] = status;
                    row1["trxMsg"] = Msg;
                    dt.Rows.Add(row1);
                    ds.Tables.Add(dt);
                    json = JsonConvert.SerializeObject(ds, Formatting.Indented);
                }

                return json;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        /// <summary>
        /// 양식 다운로드 로직
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Down_FormFile(FormData value)
        {
            string strFile_NM = value.strFILENM;
            string strFile_Path = value.strPATH;

            //파일이 있는지 확인 후 다운로드
            try
            {
                System.IO.FileInfo fi;

                fi = new System.IO.FileInfo(Server.MapPath("~/" + strFile_Path + "/") + strFile_NM + ".xlsx");

                if (fi.Exists)
                {
                    return File(fi.FullName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", strFile_NM + ".xlsx");
                }
                else
                {
                    return Content("<script>alert('파일이 존재하지 않습니다.'); window.history.back();</script>");
                }
            }
            catch (Exception ex)
            {
                return Content("<script>alert('" + ex.Message + "')</script>");
            }
        }


        /// <summary>
        /// 파일 DOWNLOAD
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DownLoad_Files(JsonGetData value)
        {
            string strServer_Path = "http://110.45.209.36:9632";
            string strFile_NM = value.FILE_NM;
            string strFile_MngtNo = value.FILE_MNGTNO;
            string strFile_SEQ = value.FILE_SEQ;
            string strFile_Path = value.FILE_PATH;
            string strFile_FormID = value.FILE_FORMID; //현재 OnlineHelp가 있을 경우는 FileNM이 다름
            string strRealFileNM = value.REAL_FILE_NM;
            string strLocalFilePath = Server.MapPath("~/Files/CRM") + "\\" + DateTime.Now.ToString("yyyyMMdd");

            //파일이 있는지 확인 후 다운로드
            try
            {
                deleteFolder(Server.MapPath("~/Files/CRM")); //파일 삭제 (Default 세팅 하루 이전 폴더 완전 삭제)

                //현재 날짜 파일 생성
                DirectoryInfo di = new DirectoryInfo(strLocalFilePath); //폴더 관련 객체
                if (di.Exists != true)
                {
                    di.Create();
                }

                di.Refresh();
                di = new DirectoryInfo(strLocalFilePath + "\\" + strFile_MngtNo);

                if (di.Exists != true)
                {
                    di.Create();
                }

                //똑같은 파일 있는지 확인.
                fi = new FileInfo(strLocalFilePath + "\\" + strFile_MngtNo + "\\" + strFile_NM);

                if (fi.Exists != true)
                {
                    //URL로 가져온 파일 내컴퓨터에 다운로드
                    WebClient wc = new WebClient();
                    wc.DownloadFile(strServer_Path + strFile_Path + strFile_NM, strLocalFilePath + "\\" + strFile_MngtNo + "\\" + strFile_NM);
                    fi.Refresh();
                    fi = new FileInfo(strLocalFilePath + "\\" + strFile_MngtNo + "\\" + strFile_NM);
                }

                //return File(fi.FullName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", strFile_NM);
                return File(fi.FullName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", strRealFileNM);
            }
            catch (Exception ex)
            {
                return Content("<script>alert('" + ex.Message + "'); window.history.back();</script>");
            }
        }

        string _NoticeFilePath = "/data/notice/";

        public ActionResult NoticeDownload(string filename, string rFilename)
        {
            try
            {
                string FullFilePath = Server.MapPath(_NoticeFilePath) + rFilename;
                if (System.IO.File.Exists(FullFilePath))    //파일이 존재한다면
                {
                    byte[] fileBytes = System.IO.File.ReadAllBytes(FullFilePath);
                    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
                }
                else
                {
                    return Content("<script>alert('파일이 존재하지 않습니다'); history.back(-1);</script>");
                    //return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound);
                }
            }
            catch
            {
                return Content("<script>alert('파일이 존재하지 않습니다');  history.back(-1);</script>");
                //return new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);
            }
        }

        [HttpPost]
        public string DownloadElvis(JsonData value)
        {
            string rtnJson = "";
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                string JsonVal = "";
                Con_File Con_File = new Con_File();
                //서버에 파일 복사

                JsonVal = Con_File.Con_DownFile(value.vJsonData.ToString());
                JsonVal = JsonVal.ToString().Replace("\\\\", "/");
                ds = JsonConvert.DeserializeObject<DataSet>(JsonVal);

                string mapPath = Server.MapPath("~/Content/TempFiles/" + ds.Tables["PATH"].Rows[0]["FILE_NAME"].ToString());

                string PrintPath = ds.Tables["PATH"].Rows[0]["FILE_NAME"].ToString();

                System.IO.FileInfo existFile;
                existFile = new System.IO.FileInfo(Server.MapPath("~/Content/TempFiles/") + ds.Tables["PATH"].Rows[0]["FILE_NAME"].ToString());

                if (existFile.Exists)
                {
                    existFile.Delete();
                }

                //복사받은 파일 내컴퓨터에 다운로드
                WebClient wc = new WebClient();
                wc.DownloadFile(ds.Tables["PATH"].Rows[0]["UrlLink"].ToString(), mapPath);

                //서버에 복사한 파일 다시 삭제
                string targetUrl = ds.Tables["PATH"].Rows[0]["UrlPath"].ToString() + "/WCF/delete.aspx?office_cd=" + Session["DOMAIN"] + "&file_nm=" + ds.Tables["PATH"].Rows[0]["FILE_NAME"].ToString();

                HttpWebRequest gomRequest = (HttpWebRequest)WebRequest.Create(new Uri(targetUrl));
                gomRequest.Method = "POST";
                byte[] postByte2 = UTF8Encoding.UTF8.GetBytes(ds.Tables["PATH"].Rows[0]["FILE_NAME"].ToString());
                Stream requestStream2 = gomRequest.GetRequestStream();

                requestStream2.Write(postByte2, 0, postByte2.Length);
                requestStream2.Close();

                var Callresponse = gomRequest.GetResponse();

                rtnJson = JsonConvert.SerializeObject(ds, Formatting.Indented);
                return rtnJson;
            }
            catch (Exception e)
            {
                return "E";
            }
        }

        [HttpPost]
        //파일 DB 삭제
        public ActionResult fnDocDeleteFile(JsonData value)
        {
            string strJson = "";
            string strResult = "";

            Con_File Con_File = new Con_File();

            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = ec.encryptAES256(vJsonData);

                strResult = Con_File.Con_fnDocDeleteFile(vEncodeData);

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
        //파일 DB 삭제
        public ActionResult fnDocUpdateFile(JsonData value)
        {
            string strJson = "";
            string strResult = "";

            Con_File Con_File = new Con_File();

            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = ec.encryptAES256(vJsonData);

                strResult = Con_File.Con_fnDocUpdateFile(vEncodeData);

                strJson = ec.decryptAES256(strResult);

                return Json(strJson);
            }
            catch (Exception e)
            {
                strJson = e.Message;
                return Json(strJson);
            }
        }

        public string MakeJson(string status, string Msg)
        {
            try
            {
                string json = "";
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                dt.Columns.Add("trxCode");
                dt.Columns.Add("trxMsg");
                DataRow row1 = dt.NewRow();
                row1["trxCode"] = status;
                row1["trxMsg"] = Msg;
                dt.Rows.Add(row1);
                dt.TableName = "Result";
                ds.Tables.Add(dt);
                json = JsonConvert.SerializeObject(ds, Formatting.Indented);
                return json;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}
