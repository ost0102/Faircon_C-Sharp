using SSGM_ELVISPRIME_COMMON.Query.File;
using SSGM_ELVISPRIME_COMMON.YJIT_Utils;
using SSGM_ELVISPRIME_DATA;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Text;
using System.Net;
using System.IO;

namespace SSGM_ELVISPRIME_COMMON.Controllers
{
    public class Con_File
    {
        public string ConnectionUrl = System.Configuration.ConfigurationManager.AppSettings["Url"];
        public string ConnectionPath = System.Configuration.ConfigurationManager.AppSettings["Path"];

        Encryption String_Encrypt = new Encryption(); //암호화
        Common comm = new Common(); //일반 함수 
        File_Query FQ = new File_Query();

        //전역 변수
        DataTable dt = new DataTable();
        DataTable Resultdt = new DataTable();
        DataSet ds = new DataSet();


        /// <summary>
        /// 파일 다운로드 데이터 로직 가져오기
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_DownFile(string strValue)
        {
            string rtnJson = "";
            string json = "";

            DataHelper.ConnectionString_Select = "ELVIS";

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strValue);

            try
            {
                if (dt == null)
                {
                    dt = new DataTable();
                    rtnJson = comm.MakeJson("N", "Data Missing", dt);
                }
                else
                {
                    if (dt.Rows.Count > 0)
                    {
                        dt = DataHelper.ExecuteDataTable(FQ.SelectHblFile_Query(dt.Rows[0], ConnectionUrl), CommandType.Text);
                        dt.TableName = "FILE";
                        string file_name = dt.Rows[0]["FILE_NAME"].ToString();
                        string real_name = dt.Rows[0]["FILE_NM"].ToString();

                        string targetUrl = ConnectionUrl + ConnectionPath;
                        HttpWebRequest gomRequest = (HttpWebRequest)WebRequest.Create(targetUrl);
                        gomRequest.ProtocolVersion = HttpVersion.Version10;
                        gomRequest.Method = "POST";
                        json = JsonConvert.SerializeObject(dt).ToString();
                        byte[] postByte = UTF8Encoding.UTF8.GetBytes(json);
                        gomRequest.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                        gomRequest.Accept = "application/json";
                        gomRequest.ContentLength = postByte.Length;
                        Stream requestStream = gomRequest.GetRequestStream();

                        requestStream.Write(postByte, 0, postByte.Length);
                        requestStream.Close();

                        HttpWebResponse response = (HttpWebResponse)gomRequest.GetResponse();

                        string result = "";
                        StreamReader rdr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                        result = rdr.ReadToEnd();

                        dt = new DataTable();
                        dt.Columns.Add("UrlLink");
                        DataRow row1 = dt.NewRow();
                        row1["UrlLink"] = result;
                        dt.Columns.Add("FILE_NAME");
                        row1["FILE_NAME"] = file_name;
                        dt.Columns.Add("UrlPath");
                        row1["UrlPath"] = ConnectionUrl;
                        dt.Columns.Add("FILE_REAL_NAME");
                        row1["FILE_REAL_NAME"] = real_name;
                        dt.Rows.Add(row1);
                        dt.TableName = "Path";

                        rtnJson = comm.MakeNonJson("Y", "Success", dt);
                    }
                    else
                    {
                        dt.Reset();
                        rtnJson = comm.MakeNonJson("N", "데이터가 없습니다.", dt);
                    }
                }
                return rtnJson;
            }
            catch (Exception e)
            {
                //만약 오류가 발생 하였을 경우
                rtnJson = comm.MakeJson("E", e.Message);
                rtnJson = String_Encrypt.decryptAES256(rtnJson);
                return rtnJson;
            }
        }

        /// <summary>
        /// 파일 업로드
        /// </summary>
        /// <param name="Filedt"></param>
        /// <returns></returns>
        public string Con_fnSetBKFileUpload(DataTable Filedt)
        {
            string rtnJson = "";
            DataTable dt = new DataTable();
            int nResult = 0;

            dt = Filedt;

            try
            {
                if (dt == null)
                {
                    rtnJson = comm.MakeJson("N", "저장 파일이 없습니다.", dt);
                }
                else
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        //파일이 있는 경우 파일 데이터 저장
                        nResult = DataHelper.ExecuteNonQuery(FQ.fnSetBKFileUpload_Query(dt.Rows[0]), CommandType.Text);
                        if (nResult != 1)
                        {
                            rtnJson = comm.MakeJson("N", "[부킹 File Upload]" + i + "번째 저장 실패 하였습니다.");
                        }
                    }
                }

                if (rtnJson == "")
                {
                    rtnJson = comm.MakeJson("Y", "Success");
                }

                return rtnJson;
            }
            catch (Exception e)
            {
                rtnJson = comm.MakeJson("E", "파일 저장에 실패 하였습니다.\n" + e.Message);
                return rtnJson;
            }
        }

        /// <summary>
        /// 파일 삭제 
        /// </summary>
        /// <param name="Filedt"></param>
        /// <returns></returns>
        public string Con_fnDocDeleteFile(string strValue)
        {
            string rtnJson = "";
            string json = "";
            int nResult = 0;

            string strResult = String_Encrypt.decryptAES256(strValue);
            DataHelper.ConnectionString_Select = "ELVIS";

            //데이터
            ds = JsonConvert.DeserializeObject<DataSet>(strResult);
            dt = ds.Tables["FILE_INFO"];
            try
            {
                if (dt == null)
                {
                    rtnJson = comm.MakeJson("N", "삭제 파일이 없습니다.", dt);
                }
                else
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        //파일이 있는 경우 파일 데이터 저장
                        nResult = DataHelper.ExecuteNonQuery(FQ.fnDocDeleteFile_Query(dt.Rows[0]), CommandType.Text);
                        if (nResult != 1)
                        {
                            rtnJson = comm.MakeJson("N", "[부킹 File Delete]" + i + "번째 저장 실패 하였습니다.");
                        }
                    }
                }

                if (rtnJson == "")
                {
                    rtnJson = comm.MakeJson("Y", "Success");
                }

                return rtnJson;
            }
            catch (Exception e)
            {
                rtnJson = comm.MakeJson("E", "파일 데이터 삭제에 실패 하였습니다.\n" + e.Message);
                return rtnJson;
            }
        }

        /// <summary>
        /// 파일 삭제 
        /// </summary>
        /// <param name="Filedt"></param>
        /// <returns></returns>
        public string Con_fnDocUpdateFile(string strValue)
        {
            string rtnJson = "";
            string json = "";
            int nResult = 0;

            string strResult = String_Encrypt.decryptAES256(strValue);
            DataHelper.ConnectionString_Select = "ELVIS";

            //데이터
            ds = JsonConvert.DeserializeObject<DataSet>(strResult);
            dt = ds.Tables["FILE_INFO"];
            try
            {
                if (dt == null)
                {
                    rtnJson = comm.MakeJson("N", "업데이트 파일이 없습니다.", dt);
                }
                else
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        //파일이 있는 경우 파일 데이터 저장
                        nResult = DataHelper.ExecuteNonQuery(FQ.fnDocUpdateFile_Query(dt.Rows[0]), CommandType.Text);
                        if (nResult != 1)
                        {
                            rtnJson = comm.MakeJson("N", "[부킹 File Update]" + i + "번째 저장 실패 하였습니다.");
                        }
                    }
                }

                if (rtnJson == "")
                {
                    rtnJson = comm.MakeJson("Y", "Success");
                }

                return rtnJson;
            }
            catch (Exception e)
            {
                rtnJson = comm.MakeJson("E", "파일 데이터 삭제에 실패 하였습니다.\n" + e.Message);
                return rtnJson;
            }
        }

        /// <summary>
        /// 현재 MNGT_NO MAX SEQ 가져오기
        /// </summary>
        /// <param name="Filedt"></param>
        /// <returns></returns>
        public string Con_fnGetSEQ(string strMngtNo)
        {
            string rtnJson = "";
            string json = "";            

            DataHelper.ConnectionString_Select = "ELVIS";
                        
            try
            {
                Resultdt = DataHelper.ExecuteDataTable(FQ.fnGetSEQ_Query(strMngtNo), CommandType.Text);

                if (Resultdt.Rows[0]["SEQ"].ToString() == "")
                {
                    rtnJson = "0";
                }
                else
                {
                    rtnJson = Resultdt.Rows[0]["SEQ"].ToString();
                }

                return rtnJson;
            }
            catch (Exception e)
            {
                rtnJson = comm.MakeJson("E", "파일 데이터 삭제에 실패 하였습니다.\n" + e.Message);
                return rtnJson;
            }
        }

        /// <summary>
        /// 파일 업로드
        /// </summary>
        /// <param name="Filedt"></param>
        /// <returns></returns>
        public string Con_fnSetConsoleFileUpload(DataTable Filedt)
        {
            string rtnJson = "";
            DataTable dt = new DataTable();
            int nResult = 0;

            dt = Filedt;

            try
            {
                if (dt == null)
                {
                    rtnJson = comm.MakeJson("N", "저장 파일이 없습니다.", dt);
                }
                else
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        //파일이 있는 경우 파일 데이터 저장
                        nResult = DataHelper.ExecuteNonQuery(FQ.fnSetConsoleFileUpload_Query(dt.Rows[0]), CommandType.Text);
                        if (nResult != 1)
                        {
                            rtnJson = comm.MakeJson("N", "[부킹 File Upload]" + i + "번째 저장 실패 하였습니다.");
                        }
                    }
                }

                if (rtnJson == "")
                {
                    rtnJson = comm.MakeJson("Y", "Success");
                }

                return rtnJson;
            }
            catch (Exception e)
            {
                rtnJson = comm.MakeJson("E", "파일 저장에 실패 하였습니다.\n" + e.Message);
                return rtnJson;
            }
        }

        /// <summary>
        /// 콘솔 - 파일 업로드 이미지 삭제
        /// </summary>
        /// <param name="Filedt"></param>
        /// <returns></returns>
        public string Con_ConSole_DeleteFile(string strValue)
        {
            string rtnJson = "";
            string json = "";
            int nResult = 0;

            string strResult = String_Encrypt.decryptAES256(strValue);
            DataHelper.ConnectionString_Select = "ELVIS";

            //데이터
            ds = JsonConvert.DeserializeObject<DataSet>(strResult);
            dt = ds.Tables["FILE_INFO"];
            try
            {
                //파일이 있는 경우 파일 데이터 저장
                nResult = DataHelper.ExecuteNonQuery(FQ.fnDocDeleteFile_Query(dt.Rows[0]), CommandType.Text);
                if (nResult == 1)
                {
                    rtnJson = comm.MakeJson("Y", "Success");
                }
                else
                {
                    rtnJson = comm.MakeJson("N", "Fail");
                }

                return rtnJson;
            }
            catch (Exception e)
            {
                rtnJson = comm.MakeJson("E", "파일 데이터 삭제에 실패 하였습니다.\n" + e.Message);
                return rtnJson;
            }
        }

        /// <summary>
        /// 콘솔 - 파일 업로드 이미지 다운로드
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_ConSole_Download(string strValue)
        {
            string rtnJson = "";
            string json = "";

            DataHelper.ConnectionString_Select = "ELVIS";

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strValue);

            try
            {
                if (dt == null)
                {
                    dt = new DataTable();
                    rtnJson = comm.MakeJson("N", "Data Missing", dt);
                }
                else
                {
                    if (dt.Rows.Count > 0)
                    {
                        dt = DataHelper.ExecuteDataTable(FQ.fnConsoleDownloadFile_Query(dt.Rows[0], ConnectionUrl), CommandType.Text);
                        dt.TableName = "FILE";
                        string file_name = dt.Rows[0]["FILE_NAME"].ToString();
                        string real_name = dt.Rows[0]["FILE_NM"].ToString();

                        string targetUrl = ConnectionUrl + ConnectionPath;
                        HttpWebRequest gomRequest = (HttpWebRequest)WebRequest.Create(targetUrl);
                        gomRequest.ProtocolVersion = HttpVersion.Version10;
                        gomRequest.Method = "POST";
                        json = JsonConvert.SerializeObject(dt).ToString();
                        byte[] postByte = UTF8Encoding.UTF8.GetBytes(json);
                        gomRequest.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                        gomRequest.Accept = "application/json";
                        gomRequest.ContentLength = postByte.Length;
                        Stream requestStream = gomRequest.GetRequestStream();

                        requestStream.Write(postByte, 0, postByte.Length);
                        requestStream.Close();

                        HttpWebResponse response = (HttpWebResponse)gomRequest.GetResponse();

                        string result = "";
                        StreamReader rdr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                        result = rdr.ReadToEnd();

                        dt = new DataTable();
                        dt.Columns.Add("UrlLink");
                        DataRow row1 = dt.NewRow();
                        row1["UrlLink"] = result;
                        dt.Columns.Add("FILE_NAME");
                        row1["FILE_NAME"] = file_name;
                        dt.Columns.Add("UrlPath");
                        row1["UrlPath"] = ConnectionUrl;
                        dt.Columns.Add("FILE_REAL_NAME");
                        row1["FILE_REAL_NAME"] = real_name;
                        dt.Rows.Add(row1);
                        dt.TableName = "Path";

                        rtnJson = comm.MakeNonJson("Y", "Success", dt);
                    }
                    else
                    {
                        dt.Reset();
                        rtnJson = comm.MakeNonJson("N", "데이터가 없습니다.", dt);
                    }
                }
                return rtnJson;
            }
            catch (Exception e)
            {
                //만약 오류가 발생 하였을 경우
                rtnJson = comm.MakeJson("E", e.Message);
                rtnJson = String_Encrypt.decryptAES256(rtnJson);
                return rtnJson;
            }
        }
    }
}
