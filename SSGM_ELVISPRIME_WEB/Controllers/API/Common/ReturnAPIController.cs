using SSGM_ELVISPRIME_COMMON.Controllers;
using SSGM_ELVISPRIME_COMMON.YJIT_Utils;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SSGM_ELVISPRIME_WEB.Controllers.API.Common
{
    public class ReturnAPIController : Controller
    {        
        Encryption ec = new Encryption();

        public class paramData
        {
            public string LOCATION { get; set; }
            public string CONTROLLER { get; set; }
            public string SCH_NO { get; set; }
            public string BKG_NO { get; set; }
            public string REG_BKG_NO { get; set; }
            public string HBL_NO { get; set; }
            public string INV_NO { get; set; }
            public string REQ_SVC { get; set; }
            public string ETD { get; set; }
            public string ETA { get; set; }
            public string POL_CD { get; set; }
            public string POL_NM { get; set; }
            public string POD_CD { get; set; }
            public string POD_NM { get; set; }
            public string CNTR_TYPE { get; set; }
            public string LINE_TYPE { get; set; }
        }

        /// <summary>
        /// 컨트롤러 이동 시 매개변수 저장
        /// </summary>
        /// <param name="paramData"></param>
        /// <returns></returns>
        [HttpPost]
        public string CallPage(paramData paramData)
        {
            string rtnPage = "";
            string view = paramData.LOCATION;
            string controller = paramData.CONTROLLER;
            string SCH_NO = paramData.SCH_NO;
            string BKG_NO = paramData.BKG_NO;
            string REG_BKG_NO = paramData.REG_BKG_NO;
            string HBL_NO = paramData.HBL_NO;
            string INV_NO = paramData.INV_NO;
            string REQ_SVC = paramData.REQ_SVC;
            string ETD = paramData.ETD;
            string ETA = paramData.ETA;
            string POL_CD = paramData.POL_CD;
            string POL_NM = paramData.POL_NM;
            string POD_CD = paramData.POD_CD;
            string POD_NM = paramData.POD_NM;
            string CNTR_TYPE = paramData.CNTR_TYPE;
            string LINE_TYPE = paramData.LINE_TYPE;

            try
            {
                if (paramData != null)
                {
                    if (SCH_NO != null)
                    {
                        TempData["SCH_NO"] = SCH_NO;
                    }

                    if (BKG_NO != null)
                    {
                        TempData["BKG_NO"] = BKG_NO;
                    }

                    if (HBL_NO != null)
                    {
                        TempData["HBL_NO"] = HBL_NO;
                    }

                    if (REG_BKG_NO != null)
                    {
                        TempData["REG_BKG_NO"] = REG_BKG_NO;
                    }

                    if (INV_NO != null)
                    {
                        TempData["INV_NO"] = INV_NO;
                    }

                    if (REQ_SVC != null)
                    {
                        TempData["REQ_SVC"] = REQ_SVC;
                    }
                    if (ETD != null)
                    {
                        TempData["ETD"] = ETD;
                    }
                    if (ETA != null)
                    {
                        TempData["ETA"] = ETA;
                    }

                    if (POL_CD != null)
                    {
                        TempData["POL_CD"] = POL_CD;
                    }
                    if (POL_NM != null)
                    {
                        TempData["POL_NM"] = POL_NM;
                    }
                    if (POD_CD != null)
                    {
                        TempData["POD_CD"] = POD_CD;
                    }
                    if (POD_NM != null)
                    {
                        TempData["POD_NM"] = POD_NM;
                    }
                    if (CNTR_TYPE != null)
                    {
                        TempData["CNTR_TYPE"] = CNTR_TYPE;
                    }
                    if (LINE_TYPE != null)
                    {
                        TempData["LINE_TYPE"] = LINE_TYPE;
                    }
                    rtnPage = "/" + controller + "/" + view;
                }
                return rtnPage;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public class paramList
        {
            public string JOB_TYPE { get; set; }
            public string MSG { get; set; }
            public string REF1 { get; set; }
            public string REF2 { get; set; }
            public string REF3 { get; set; }
            public string REF4 { get; set; }
            public string REF5 { get; set; }
        }

        [HttpPost]
        public string CallPushPage(paramList paramList)
        {
            string strType = paramList.JOB_TYPE;
            string strRef1 = paramList.REF1;
            string strRef2 = paramList.REF2;
            string strRef3 = paramList.REF3;
            string strRef4 = paramList.REF4;
            string strRef5 = paramList.REF5;
            string rtnPage = "";
            try
            {
                string view = "";
                string controller = "";
                switch (strType)
                {
                    case "BKG": //부킹
                        view = "BookingInfo";
                        controller = "Booking";
                        break;
                    case "HBL": //BL
                        view = "BL";
                        controller = "Document";
                        break;
                    case "INV": //청구서
                        view = "Invoice";
                        controller = "Document";
                        break;
                }

                if (strRef1 != null)
                {
                    TempData["REF1"] = strRef1;
                }
                if (strRef2 != null)
                {
                    TempData["REF2"] = strRef2;
                }
                if (strRef3 != null)
                {
                    TempData["REF3"] = strRef3;
                }
                if (strRef4 != null)
                {
                    TempData["REF4"] = strRef4;
                }
                if (strRef5 != null)
                {
                    TempData["REF5"] = strRef5;
                }
                rtnPage = "/" + controller + "/" + view;
                return rtnPage;
            }
            catch
            {
                return "";
            }
        }

        public ActionResult CallMailPage()
        {
            #region // Param 정의 내용
            /*
             * Param 구조
             * domain : domain
             * key : UserID
             * no : Primary Key
             * type : 업무구분
             *
             * MSG01 USR 회원가입
                MSG01 BKG 부킹
                MSG01 QUO 견적
                MSG01 HBL 비엘
                MSG01 INV 청구서
                MSG01 TRC 화물추적
             */
            #endregion
            string jSonParam = "";
            string jSonParam2 = "";
            string strDomain = ""; //업체 도메인
            string strKey = ""; //사용자 아이디
            string strAuthKey = ""; //사용자 인증키
            string strType = ""; // 업무구분
            string strFlag = ""; // 해운/항공
            string strRef1 = "";
            string strRef2 = "";
            string strRef3 = "";
            string strRef4 = "";
            string strRef5 = "";
            DataTable LoginDt = new DataTable();

            try
            {                
                string param = Request["param"];                
                string param2 = Request["params"];
                jSonParam = "[" + ec.decryptAES256(param.Replace(" ", "+")) + "]";
                jSonParam2 = "[" + ec.decryptAES256(param2.Replace(" ", "+")) + "]";
                DataTable paramDt = JsonConvert.DeserializeObject<DataTable>(jSonParam);
                DataTable paramDt2 = JsonConvert.DeserializeObject<DataTable>(jSonParam2);
                paramDt.Columns.Add("DOMAIN");
                paramDt.Rows[0]["DOMAIN"] = paramDt2.Rows[0]["DOMAIN"].ToString();

                if (paramDt.Rows.Count > 0)
                {

                    //Proc1. 자동로그인
                    #region // 로그인 하기
                    Hashtable paramHt = new Hashtable();
                    paramHt.Add("DOMAIN", paramDt.Rows[0]["DOMAIN"]);
                    paramHt.Add("USR_ID", paramDt.Rows[0]["AUTH_KEY"]);
                    string rtnJson = JsonConvert.SerializeObject(paramHt);
                    if (rtnJson.ToString().Substring(0, 1) != "[") rtnJson = "[" + rtnJson + "]";
                    string returnVal = "";

                    Con_Main Con_Main = new Con_Main();

                    returnVal = Con_Main.GetMailLogin(rtnJson);

                    returnVal = ec.decryptAES256(returnVal);

                    if (returnVal != null)
                    {

                        DataSet ds = JsonConvert.DeserializeObject<DataSet>(returnVal);
                        DataTable rst = ds.Tables["Result"];
                        LoginDt = ds.Tables["Table1"];
                        if (rst.Rows[0]["trxCode"].ToString() == "Y")
                        {
                            Session["USR_ID"] = LoginDt.Rows[0]["USR_ID"].ToString();
                            Session["USER_NM"] = LoginDt.Rows[0]["USER_NM"].ToString();
                            Session["CUST_CD"] = LoginDt.Rows[0]["CUST_CD"].ToString();
                            Session["CUST_NM"] = LoginDt.Rows[0]["CUST_NM"].ToString();
                            Session["EMAIL"] = LoginDt.Rows[0]["EMAIL"].ToString();
                            Session["AUTH_KEY"] = LoginDt.Rows[0]["AUTH_KEY"].ToString();
                            Session["AUTH_TYPE"] = LoginDt.Rows[0]["AUTH_TYPE"].ToString();
                            Session["USR_TYPE"] = LoginDt.Rows[0]["USR_TYPE"].ToString();
                            Session["HP_NO"] = LoginDt.Rows[0]["HP_NO"].ToString();
                            Session["DOMAIN"] = System.Configuration.ConfigurationManager.AppSettings["Domain"];
                            Session["OFFICE_CD"] = LoginDt.Rows[0]["OFFICE_CD"].ToString();

                            strRef1 = LoginDt.Rows[0]["REF1"].ToString();
                            strRef2 = LoginDt.Rows[0]["REF2"].ToString();
                            strRef3 = LoginDt.Rows[0]["REF3"].ToString();
                            strRef4 = LoginDt.Rows[0]["REF4"].ToString();
                            strRef5 = LoginDt.Rows[0]["REF5"].ToString();
                            strType = LoginDt.Rows[0]["JOB_TYPE"].ToString();
                        }
                        else
                        {
                            //결과값이 없다!
                            return RedirectToAction("Index", "");
                        }
                    }
                    else
                    {
                        //결과값이 없다!
                        return RedirectToAction("Index", "");
                    }
                    #endregion
                    string view = "";
                    string controller = "";
                    if (LoginDt.Rows.Count > 0) //로그인이 성공했다면
                    {
                        switch (strType)
                        {
                            case "BKG": //부킹
                                view = "BookingInfo";
                                controller = "Booking";
                                break;
                            case "HBL": //BL
                                view = "Index";
                                controller = "MyData";
                                break;
                            case "INV": //청구서
                                view = "Index";
                                controller = "MyData";
                                break;
                        }

                        TempData["REF1"] = strRef1;
                        TempData["REF2"] = strRef2;
                        TempData["REF3"] = strRef3;
                        TempData["REF4"] = strRef4;
                        TempData["REF5"] = strRef5;

                        return RedirectToAction(view, controller);
                    }
                    else
                    {
                        return RedirectToAction("index", "");
                    }                   
                }
                else
                {
                    //예외상황은 무조건 로그인 화면으로 이동
                    return RedirectToAction("index", "");
                }
            }
            catch (Exception e)
            {
                //예외상황은 무조건 로그인 화면으로 이동
                return RedirectToAction("index", "");
            }
        }

        /// <summary>
        /// 홈페이지에서 로그인 / 비밀번호 연결
        /// </summary>
        /// <returns></returns>
        public ActionResult CallHomePage()
        {
            //테스트 자료
            //DataTable DataT;
            //DataRow dr;
            //
            //DataT = new DataTable("USR_INFO");
            //DataT.Columns.Add("USR_ID");
            //DataT.Columns.Add("PSWD");
            //
            //dr = DataT.NewRow();
            //dr["USR_ID"] = "twkim";
            //dr["PSWD"] = "yjit12#$";
            //DataT.Rows.Add(dr);
            //
            //string encrypt = "";
            //
            //encrypt = ec.encryptAES256(JsonConvert.SerializeObject(DataT, Formatting.Indented));
            
            string jSonParam = "";          
            string strKey = ""; //사용자 아이디            
            string strPWD = "";            
            DataTable LoginDt = new DataTable();
            
            try
            {
                string param = Request["params"];
                //jSonParam = "[" + ec.decryptAES256(param.Replace(" ", "+")) + "]";                
                jSonParam = ec.decryptAES256(param.Replace(" ", "+"));
                DataTable paramDt = JsonConvert.DeserializeObject<DataTable>(jSonParam);                
            
                if (paramDt.Rows.Count > 0)
                {
            
                    //Proc1. 자동로그인
                    string returnVal = "";
                    Con_Main Con_Main = new Con_Main();
            
                    returnVal = Con_Main.GetHomePageLogin(paramDt);
            
                    returnVal = ec.decryptAES256(returnVal);
            
                    if (returnVal != null)
                    {
            
                        DataSet ds = JsonConvert.DeserializeObject<DataSet>(returnVal);
                        DataTable rst = ds.Tables["Result"];
                        LoginDt = ds.Tables["Table1"];
                        if (rst.Rows[0]["trxCode"].ToString() == "Y")
                        {
                            Session["USR_ID"] = LoginDt.Rows[0]["USR_ID"].ToString();
                            Session["USER_NM"] = LoginDt.Rows[0]["USER_NM"].ToString();
                            Session["CUST_CD"] = LoginDt.Rows[0]["CUST_CD"].ToString();
                            Session["CUST_NM"] = LoginDt.Rows[0]["CUST_NM"].ToString();
                            Session["EMAIL"] = LoginDt.Rows[0]["EMAIL"].ToString();
                            Session["AUTH_KEY"] = LoginDt.Rows[0]["AUTH_KEY"].ToString();
                            Session["AUTH_TYPE"] = LoginDt.Rows[0]["AUTH_TYPE"].ToString();
                            Session["USR_TYPE"] = LoginDt.Rows[0]["USR_TYPE"].ToString();
                            Session["HP_NO"] = LoginDt.Rows[0]["HP_NO"].ToString();
                            Session["DOMAIN"] = System.Configuration.ConfigurationManager.AppSettings["Domain"];
                            Session["OFFICE_CD"] = LoginDt.Rows[0]["OFFICE_CD"].ToString();
            
                            return RedirectToAction("MyBoard", "MyMenu");
                        }
                        else
                        {
                            //결과값이 없다!
                            return RedirectToAction("Index", "", new { LoginFail = "Y" });
                        }
                    }
                    else
                    {
                        //결과값이 없다!
                        return RedirectToAction("Index", "", new { LoginFail = "Y" });
                    }                    
                }
                else
                {
                    //예외상황은 무조건 로그인 화면으로 이동
                    return RedirectToAction("Index", "", new { LoginFail = "Y" });
                }
            }
            catch (Exception e)
            {
                //예외상황은 무조건 로그인 화면으로 이동
                return RedirectToAction("Index", "", new { LoginFail = "Y" });
            }
        }

    }
}
