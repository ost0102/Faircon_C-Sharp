using SSGM_ELVISPRIME_COMMON.Controllers;
using SSGM_ELVISPRIME_COMMON.YJIT_Utils;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Ionic.Zip;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace SSGM_ELVISPRIME_WEB.Controllers.API.Common
{
    public class FileController : Controller
    {
		FileInfo fi;
		private DataTable dt;

		Encryption ec = new Encryption(); //DB_Data - Encryption
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

		/// <summary>
		/// 파일 다운로드 로직
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
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
                    //return File(fi.FullName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", strREPLACE_FILE_NM);
                    return File(fi.FullName, "application/octet-stream", strREPLACE_FILE_NM);
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
		/// 부킹 - 파일 업로드 로직
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		public ActionResult Upload_Files()
		{			
            UTF8Encoding UTF8_Encodeing = new UTF8Encoding();
            DataTable dt = new DataTable();
            DataRow dr;
            FileInfo fi;
            Boolean isSuccess = true; 

            string strJson = "";
            string strResult = "";
            string SavePath = "/EDMS/WEB/" + ConfigurationManager.AppSettings["Domain"].ToString() + "/" + ConfigurationManager.AppSettings["OFFICE_CD"].ToString() + "/" + DateTime.Now.ToString("yyyyMMdd") + "/";
            string strFilePath = "";
            string strMNGT_NO = Request.Form["MNGT_NO"];
            string strDOC_NO = Request.Form["DOC_NO"];
            string strOFFICE_CD = Request.Form["OFFICE_CD"];
            string strDOC_TYPE = Request.Form["DOC_TYPE"];
            string strSEQ = Con_File.Con_fnGetSEQ(strMNGT_NO); //해당 MNGT_NO SEQ를 가져와서 쓴다.
            //string strSEQ = Request.Form["SEQ"];

            HttpFileCollectionBase files = Request.Files;            

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
                string sUploadHandler = System.Configuration.ConfigurationManager.AppSettings["Url"]  + "wcf/UploadHandler.aspx";
                System.Net.WebClient wc = new System.Net.WebClient();
                byte[] responseArray;

                NameValueCollection myQueryStringCollection = new NameValueCollection();                
                myQueryStringCollection.Add("SavePath", SavePath);

                string ChkFileNM = Path.GetFileName(files[0].FileName);

                if (ChkFileNM != "")
                {                    
                    for (int i = 0; i < files.Count; i++)
                    {
                        string InputFileName = "";
                        string strFileSize = "";
                        string strNowTime = "_" + DateTime.Now.ToString("yyyyMMddHHmmssffffff");
                        string strRealFileNM = "";

                        HttpPostedFileBase file = files[i];

                        if (file.FileName != "" && file.ContentLength != 0)
                        {
                            if (file != null)
                            {
                                string strFileName = "";
                                InputFileName = Path.GetFileName(file.FileName);								                                
                                InputFileName = Regex.Replace(InputFileName, @"[/\+:*?<>|""#]", "");
                                InputFileName = InputFileName.Replace(" ", "");
                                strRealFileNM = InputFileName;

                                string[] fileinfo = InputFileName.Split('.');
                                for (int j = 0; j < fileinfo.Length - 1; j++)
                                {                                    		
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
                                InputFileName = strMNGT_NO + "_" + (Int32.Parse(strSEQ) + 1) + "_" + InputFileName;
                                strFileSize = file.ContentLength.ToString();
                                strFilePath = Path.Combine(Server.MapPath("~/Files/TEMP/") + InputFileName);
								                                
                                file.SaveAs(strFilePath);
                            }

                            if (file != null && file.ContentLength > 0)
                            {
                                wc.QueryString = myQueryStringCollection;
                                responseArray = wc.UploadFile(sUploadHandler, "POST", strFilePath);
                                strResult = UTF8_Encodeing.GetString(responseArray);
                            }
                            else
                            {
                                responseArray = System.Text.Encoding.ASCII.GetBytes("N\n Upload failed!");
                                strResult = UTF8_Encodeing.GetString(responseArray);
                            }

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

                                System.IO.FileInfo existFile;
                                existFile = new System.IO.FileInfo(Path.Combine(Server.MapPath("~/Files/TEMP/") + InputFileName));
                                if (existFile.Exists)
                                {
                                    existFile.Delete();
                                }

                            }
                            else
                            {
                                isSuccess = false;
                            } 
                        }

                        //fi = new System.IO.FileInfo(strFilePath);
						//
                        //if (fi.Exists)
                        //{
                        //    fi.Delete();
                        //}
                    }
                }

                if (isSuccess)
                {					
					strResult = Con_File.Con_fnSetBKFileUpload(dt);
					strJson = ec.decryptAES256(strResult);
				}
                else
                {
                    strJson = MakeJson("N", "파일 디비 저장 실패",dt);
                }

                return Json(strJson);
            }
            catch (Exception e)
            {
                return Json("[Error]" + e.Message);
            }
		}

		/// <summary>
		/// 엘비스 파일 다운로드 로직
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		[HttpPost]
		public string DownloadElvis(JsonData value)
		{
			string rtnJson = "";
			DataTable dt = new DataTable();
			DataSet ds = new DataSet();
			try
			{
				string JsonVal = "";
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

				WebClient wc = new WebClient();
				wc.DownloadFile(ds.Tables["PATH"].Rows[0]["UrlLink"].ToString(), mapPath);				
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
			catch(Exception e)
			{
               return "E";
			}
		}

		/// <summary>
		/// 파일 삭제 로직		
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		[HttpPost]		
		public ActionResult fnDocDeleteFile(JsonData value)
		{
			string strJson = "";
			string strResult = "";

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

		/// <summary>
		/// 부킹 - 파일 업데이트 로직
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult fnDocUpdateFile(JsonData value)
		{
			string strJson = "";
			string strResult = "";

			try
			{
				string vJsonData = value.vJsonData.ToString();
				string vEncodeData = "";

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

        /// <summary>
        /// Json 형식으로 만드는 로직
        /// </summary>
        /// <param name="status"></param>
        /// <param name="Msg"></param>
        /// <param name="args"></param>
        /// <returns></returns>
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
		/// 부킹 - 파일 업로드 로직
		/// </summary>
		/// <returns></returns>
		[HttpPost]
        public ActionResult ConSole_Upload_Files()
        {
            UTF8Encoding UTF8_Encodeing = new UTF8Encoding();
            DataTable dt = new DataTable();
            DataRow dr;
            FileInfo fi;
            Boolean isSuccess = true;

            string strJson = "";
            string strResult = "";
            string SavePath = "/EDMS/DOC/" + ConfigurationManager.AppSettings["Domain"].ToString() + "/" + ConfigurationManager.AppSettings["OFFICE_CD"].ToString() + "/" + DateTime.Now.ToString("yyyyMMdd") + "/";
            string strFilePath = "";
            string strMNGT_NO = Request.Form["MNGT_NO"];
            string strDOC_NO = Request.Form["DOC_NO"];
            string strOFFICE_CD = Request.Form["OFFICE_CD"];
            string strDOC_TYPE = Request.Form["DOC_TYPE"];
            string strSEQ  = Con_File.Con_fnGetSEQ(strMNGT_NO); //해당 MNGT_NO SEQ를 가져와서 쓴다.
            //string strSEQ = Request.Form["SEQ"];
            HttpFileCollectionBase files = Request.Files;

            dt = new DataTable("FILE_INFO");
            dt.Columns.Add("MNGT_NO");
            dt.Columns.Add("FILE_NM");
            dt.Columns.Add("REPLACE_FILE_NM");            
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
                myQueryStringCollection.Add("SavePath", SavePath);

                string ChkFileNM = Path.GetFileName(files[0].FileName);

                if (ChkFileNM != "")
                {
                    for (int i = 0; i < files.Count; i++)
                    {
                        string strElvisFile = "";
                        string strFileNM = "";
                        string strRealFileNM = "";
                        string strFileSize = "";
                        string strNowTime = "_" + DateTime.Now.ToString("yyyyMMddHHmmssffffff");
                        //string strFileNM = ""; //파일명 고정은 아이폰에서 고정되기 때문에 세팅                       
                        //string strRealFileNM = "";

                        HttpPostedFileBase file = files[i];

                        if (file.FileName != "" && file.ContentLength != 0)
                        {
                            if (file != null)
                            {
                                strRealFileNM = (Int32.Parse(strSEQ) + 1) +"_"+file.FileName; //실제 파일명 넣어 둠      
                                strFileNM = file.FileName;
                                //strFileNM = (Int32.Parse(strSEQ) + 1) + "_IMAGE" + file.FileName.Substring(file.FileName.LastIndexOf("."), (file.FileName.Length - file.FileName.LastIndexOf("."))); //파일 확장자만 뒤에 붙히기
                                //엘비스 파일 => 관리번호_SEQ_파일네임.확장자
                                strElvisFile = strMNGT_NO + "_" + (Int32.Parse(strSEQ) + 1) + "_" + strFileNM;
                                strFileSize = file.ContentLength.ToString();
                                strFilePath = Path.Combine(Server.MapPath("~/Files/TEMP/") + strElvisFile);
                                file.SaveAs(strFilePath);
                            }

                            if (file != null && file.ContentLength > 0)
                            {
                                wc.QueryString = myQueryStringCollection;
                                responseArray = wc.UploadFile(sUploadHandler, "POST", strFilePath);
                                strResult = UTF8_Encodeing.GetString(responseArray);
                            }
                            else
                            {
                                responseArray = System.Text.Encoding.ASCII.GetBytes("N\n Upload failed!");
                                strResult = UTF8_Encodeing.GetString(responseArray);
                            }
                            
                            if (strResult.StartsWith("Y"))
                            {
                                dr = dt.NewRow();
                            
                                dr["MNGT_NO"] = strMNGT_NO;
                                dr["FILE_NM"] = strFileNM;
                                //dr["FILE_NM"] = InputFileName;
                                dr["REPLACE_FILE_NM"] = strRealFileNM;
                                dr["FILE_PATH"] = SavePath;
                                dr["FILE_SIZE"] = strFileSize;
                                dr["DOC_TYPE"] = strDOC_TYPE;
                                dr["DOC_NO"] = strDOC_NO;
                                dr["OFFICE_CD"] = strOFFICE_CD;
                                dr["SYS_ID"] = "WEB";
                                dr["FORM_ID"] = "ConsoleImage";
                                dr["INS_USR"] = "WEB";
                                dr["INS_YMD"] = DateTime.Now.ToString("yyyyMMdd");
                                dr["INS_HM"] = DateTime.Now.ToString("HHmmss");
                            
                                dt.Rows.Add(dr);

                                System.IO.FileInfo existFile;
                                existFile = new System.IO.FileInfo(Path.Combine(Server.MapPath("~/Files/TEMP/") + strElvisFile));
                                if (existFile.Exists)
                                {
                                    existFile.Delete();
                                }
                            }
                            else
                            {
                                isSuccess = false;
                            }
                        }

                        //fi = new System.IO.FileInfo(strFilePath);
                        //
                        //if (fi.Exists)
                        //{
                        //    fi.Delete();
                        //}
                    }
                }

                if (isSuccess)
                {
                    strResult = Con_File.Con_fnSetConsoleFileUpload(dt);
                    strJson = ec.decryptAES256(strResult);
                }
                else
                {
                    strJson = MakeJson("N", "파일 디비 저장 실패", dt);
                }

                return Json(strJson);
            }
            catch (Exception e)
            {
                return Json("[Error]" + e.Message);
            }
        }

        /// <summary>
		/// 콘솔 업로드 파일 삭제 로직		
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		[HttpPost]
        public ActionResult ConSole_DeleteFile(JsonData value)
        {
            string strJson = "";
            string strResult = "";

            try
            {
                string vJsonData = value.vJsonData.ToString();
                string vEncodeData = "";

                //암호화 걸기
                vEncodeData = ec.encryptAES256(vJsonData);

                strResult = Con_File.Con_ConSole_DeleteFile(vEncodeData);

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
		/// 콘솔 업로드 파일 다운로드 로직	
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		[HttpPost]
        public string ConSole_Download(JsonData value)
        {
            string rtnJson = "";
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                string JsonVal = "";
                JsonVal = Con_File.Con_ConSole_Download(value.vJsonData.ToString());
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

                WebClient wc = new WebClient();
                wc.DownloadFile(ds.Tables["PATH"].Rows[0]["UrlLink"].ToString(), mapPath);
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

        /// <summary>
		/// 콘솔 이미지 다운로드 로직
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		[HttpGet]
        public ActionResult Console_DownloadFile(RtnFilesInfo value)
        {
            string strFILE_NM = value.FILE_NM;
            string strFILE_PATH = value.FILE_PATH;
            string strREPLACE_FILE_NM = value.REPLACE_FILE_NM;

            try
            {
                System.IO.FileInfo fi = new System.IO.FileInfo(Server.MapPath("~/Content/TempFiles/") + strFILE_NM);

                if (fi.Exists)
                {                    
                    return File(fi.FullName, "application/octet-stream", strREPLACE_FILE_NM);                    
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
        string _NoticeFilePath = "/data/notice/";

        //
        // GET: /File/

        public ActionResult Download(string filename, string rFilename)
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

        public ActionResult Console_TempDelete(JsonData value)
        {
            string strJson = "";
            string strResult = "";
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                string vJsonData = value.vJsonData.ToString();
                dt = JsonConvert.DeserializeObject<DataTable>(vJsonData);
                for (int i = 0; i < dt.Rows.Count; i++) {
                    string file_nm = dt.Rows[i]["FILE_NM"].ToString();

                    System.IO.FileInfo existFile;
                    existFile = new System.IO.FileInfo(Server.MapPath("~/Content/TempFiles/") + file_nm);
                    if (existFile.Exists)
                    {
                        existFile.Delete();
                    }
                }
                //ds = JsonConvert.DeserializeObject<DataSet>(vJsonData);


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
