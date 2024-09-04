using System;
using System.Data;
using System.Text;
using Newtonsoft.Json;

namespace SSGM_ELVISPRIME_COMMON.YJIT_Utils
{
    public class Common
    {
        Encryption String_Encrypt = new Encryption();        

        /// <summary>
        /// Json 형식으로 데이터 만들기
        /// </summary>
        /// <param name="status"></param>
        /// <param name="Msg"></param>
        /// <returns></returns>
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
                json = String_Encrypt.encryptAES256(JsonConvert.SerializeObject(ds, Formatting.Indented));
                return json;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        /// <summary>
        /// Json 형식으로 데이터 만들기
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
                if (status != "E" && args.Rows.Count > 0)
                {
                    ds.Tables.Add(args);
                }
                string strValue = JsonConvert.SerializeObject(ds);

                //암호화 로직 추가 
                json = String_Encrypt.encryptAES256(strValue);
                return json;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        /// <summary>
        /// Json 형식으로 데이터 만들기
        /// </summary>
        /// <param name="Result"></param>
        /// <param name="DT1"></param>
        /// <returns></returns>
        public string MakeJson(DataTable Result, DataTable DT1)
        {

            string strJson = "";

            try
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(Result);
                ds.Tables.Add(DT1);

                strJson = JsonConvert.SerializeObject(ds, Formatting.Indented);

                strJson = String_Encrypt.encryptAES256(strJson);
            }
            catch (Exception e)
            {
                return e.Message;
            }

            return strJson;
        }

        /// <summary>
        /// Json 형식으로 데이터 만들기
        /// </summary>
        /// <param name="status"></param>
        /// <param name="Msg"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public string MakeNonJson(string status, string Msg, DataTable args)
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
                if (status != "E" && args.Rows.Count > 0)
                {
                    ds.Tables.Add(args);
                }
                string strValue = JsonConvert.SerializeObject(ds);
                json = strValue;
                return json;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        
        /// <summary>
        /// 결과값 DT로 만들어 주는 함수
        /// </summary>
        /// <param name="status"></param>
        /// <param name="Msg"></param>
        /// <returns></returns>
        public DataTable MakeResultDT(string status, string Msg)
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("trxCode");
            dt.Columns.Add("trxMsg");
            DataRow row1 = dt.NewRow();
            row1["trxCode"] = status;
            row1["trxMsg"] = Msg;
            dt.Rows.Add(row1);
            dt.TableName = "Result";

            return dt;
        }

        

        /// <summary>
        /// DataSet Json 형식으로 만들기
        /// </summary>
        /// <param name="status"></param>
        /// <param name="Msg"></param>
        /// <param name="ds"></param>
        /// <returns></returns>
        public string DS_MakeJson(string status, string Msg, DataSet ds)
        {

            string strJson = "";

            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("trxCode");
                dt.Columns.Add("trxMsg");
                DataRow row1 = dt.NewRow();
                row1["trxCode"] = status;
                row1["trxMsg"] = Msg;
                dt.Rows.Add(row1);
                dt.TableName = "Result";

                ds.Tables.Add(dt);               

                strJson = JsonConvert.SerializeObject(ds, Formatting.Indented);

                strJson = String_Encrypt.encryptAES256(strJson);
            }
            catch (Exception e)
            {
                return e.Message;
            }

            return strJson;
        }

        /// <summary>
        /// 비밀번호 찾기 인증키 만들기
        /// </summary>
        /// <param name="numLength">텍스트 길이</param>
        /// <returns></returns>
        public string fnGetConfirmKey()
        {

            string strResult = "";
            Random rand = new Random();
            string strRandomChar = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM";
            string strRandomNum = "1234567890";
            string strRandomSC = "!@$%^()";

            StringBuilder rs = new StringBuilder();

            //특수문자 1 + 문자 3 + 숫자 1 + 문자 3 + 특수문자 1
            rs.Append(strRandomSC[(int)(rand.NextDouble() * strRandomSC.Length)]);
            rs.Append(strRandomChar[(int)(rand.NextDouble() * strRandomChar.Length)]);
            rs.Append(strRandomChar[(int)(rand.NextDouble() * strRandomChar.Length)]);
            rs.Append(strRandomChar[(int)(rand.NextDouble() * strRandomChar.Length)]);
            rs.Append(strRandomNum[(int)(rand.NextDouble() * strRandomNum.Length)]);
            rs.Append(strRandomChar[(int)(rand.NextDouble() * strRandomChar.Length)]);
            rs.Append(strRandomChar[(int)(rand.NextDouble() * strRandomChar.Length)]);
            rs.Append(strRandomChar[(int)(rand.NextDouble() * strRandomChar.Length)]);
            rs.Append(strRandomSC[(int)(rand.NextDouble() * strRandomSC.Length)]);

            strResult = rs.ToString();

            return strResult;
        }

        /// <summary>
        /// 랜덤 텍스트 가지고 오기 + 숫자
        /// </summary>
        /// <param name="numLength">텍스트 길이</param>
        /// <returns></returns>
        public string fnGetRandomString(int numLength)
        {

            string strResult = "";
            Random rand = new Random();
            string strRandomChar = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM0123456789";

            StringBuilder rs = new StringBuilder();

            for (int i = 0; i < numLength; i++)
            {
                rs.Append(strRandomChar[(int)(rand.NextDouble() * strRandomChar.Length)]);
            }
            strResult = rs.ToString();

            return strResult;
        }
    }
}
