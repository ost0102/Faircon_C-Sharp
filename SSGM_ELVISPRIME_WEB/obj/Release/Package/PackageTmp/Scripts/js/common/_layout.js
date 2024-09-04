////////////////////전역 변수//////////////////////////
var _isConfirm;
var _initPage = "";
var Login_Count = 0;
var _objTrkJsonData = new Object();
////////////////////jquery event///////////////////////
$(function () {

    //로그인 아이디 저장 체크가 되어있을 시 데이터 넣는 로직
    var userInputId = _fnGetCookie("Prime_CK_USR_ID_REMEMBER_SSGM");
    var vAutoLogin = _fnGetCookie("Prime_CK_PASSWORD_REMEMBER_SSGM");

    //TWKIM - 자동 로그인 , 아이디 기억하기 버튼 이벤트 체크
    if (_fnToNull($("#Session_USR_ID").val()) == "") {

        if (_getParameter("LoginPopup") == "Y") {
            fnShowLoginLayer("init");
        }
        else {
            //자동 로그인 체크 시
            if (_fnToNull(vAutoLogin) != "") {
                $("#Login_ID").val(userInputId);
                $("#Login_Password").val(vAutoLogin);
                $("#login_keep").attr("checked", true);
                $("#auto_login_keep").attr("checked", true);
                $("#Login_btn").click();
            } else if (_fnToNull(userInputId) != "") {
                $("#Login_ID").val(userInputId);
                $("#login_keep").attr("checked", true);
            }
        }
    }


    //파라미터가 LoginPopup Y 가 있을 경우 로그인 레이어 팝업이 뜨게 끔 하는 로직
    if (_getParameter("LoginPopup") == "Y" && _fnToNull($("#Session_USR_ID").val()) == "") {
        fnShowLoginLayer("init");
    }

    //만약 파트너로 로그인이 되어있을 경우 
    if (_fnToNull($("#Session_USR_TYPE").val()) != "") {
        if (_fnToNull($("#Session_USR_TYPE").val()) == "P") {
            $(".login_info").addClass("partner");
        }
    }

});


$(document).on("click", "#login_popdown", function () {
    $("#Login_Password").val("");
    layerClose2('#login_pop');
})

//Confrim 이벤트
$(document).on("click", "#alert02_confirm", function () {
    _isConfirm = true;
});

//Confrim 이벤트
$(document).on("click", "#alert02_cencel", function () {
    _isConfirm = false;
});

//PC - 화물추적 버튼 이벤트
$(document).on("click", "#Pc_btn_Tracking", function () {
    if (_fnToNull($("#Mngt_no").val()) == "") {
        _fnAlertMsg("검색할 번호를 입력해주세요.");
    } else {
        postPopUp($("#Mngt_no").val().toUpperCase().trim());
    }

});
//레이어 팝업 내에서 검색
$(document).on("click", "#L_btnAirTrkSearch", function () {
    if (_fnToNull($("#Layer_L_Tracking").val()) == "") {
        _fnLayerAlertMsg("검색할 번호를 입력해주세요.");
    } else {
        postPopUp($("#Layer_L_Tracking").val().toUpperCase().trim());
    }
});

//모바일 - 화물추적 버튼 엔터 이벤트
$(document).on("keyup", "#Layer_L_Tracking", function (e) {
    if (e.keyCode == 13) {
        if (_fnToNull($("#Layer_L_Tracking").val()) == "") {
            _fnLayerAlertMsg("검색할 번호를 입력해주세요.");
        } else {
            postPopUp($("#Layer_L_Tracking").val().toUpperCase().trim());
        }
    }
});

//해운 - 화물추적 레이어 안에 검색 버튼 이벤트
$(document).on("click", "#btnSeaTrkSearch", function (){
    fngetLayerTracking($("#SVT_P_HBL_NO").val().toUpperCase().trim(), $("#SVT_P_CNTR_NO").val().toUpperCase().trim());
});

//항공 - 화물추적 레이어 안에 검색 버튼 이벤트
$(document).on("click", "#btnAirTrkSearch", function () {
    fngetLayerTracking($("#LF_P_HBL_NO").val().toUpperCase().trim(), $("#LF_P_CNTR_NO").val().toUpperCase().trim());
});

//로그인 버튼 클릭 이벤트
$(document).on("click", "#Login_btn", function () {

        _fnLogin();
    
});

//로그인 엔터 이벤트
$(document).on("keyup", "#Login_ID", function (e) {
    if (e.keyCode == 13) {
        if (Login_Count == -3) {
            _fnLayerAlertMsg("보안문자를 그려주세요");
        } else {
            if (_fnToNull($("#Login_Password").val()) != "") {
                _fnLogin();
            } else {
                $("#Login_Password").focus();
            }
        }
    }
});

$(document).on("focus", "#Login_ID", function (e) {
    $("#Password_Warning").hide();
});

$(document).on("focus", "#Login_Password", function (e) {
    $("#Email_Warning").hide();
});

//로그인 엔터 이벤트
$(document).on("keyup", "#Login_Password", function (e) {
    if (e.keyCode == 13) {
        if (Login_Count == -3) {
            _fnLayerAlertMsg("보안문자를 그려주세요");
        } else {
            _fnLogin();
        }
    }
});



//첫번째 리스트 중 Tracking Data를 보여주기 위한 함수
$(document).on("click", ".Layer_clickevent", function () {

    var vThis = $(this).closest(".layer_wanna_open");
    var vObject = new Object();
    vObject.This = vThis;
    vObject.HBL_NO = vThis.find("input[type='hidden']").eq(0).val();
    vObject.CNTR_NO = vThis.find("input[type='hidden']").eq(1).val();
    vObject.REQ_SVC = vThis.find("input[type='hidden']").eq(2).val();
    vObject.EX_IM_TYPE = vThis.find("input[type='hidden']").eq(3).val();

    fnGetLayerTrackingDetail(vObject);

    var $par = $(this).closest('.layer_wanna_open');
    var inx = $par.index();
    if ($par.hasClass('open') == true) {
        $('.layer_wanna_open:eq(' + inx + ')').find('.Layer_tracking_box').stop().slideUp();
        $par.removeClass('open');
    }
    else {
        if ($('.layer_wanna_open').hasClass('open')) {
            $('.layer_wanna_open').removeClass('open');
            $('.layer_wanna_open').find('.Layer_tracking_box').slideUp();
        }
        $('.layer_wanna_open:eq(' + inx + ')').addClass('open');
        $par.find('.Layer_tracking_box').slideDown();
    }
});

//화물추적 디테일 검색 버튼 클릭 이벤트
$(document).on("click", "button[name='btn_LayerTrkDTL']", function () {

    var vThis = $(this).closest(".layer_wanna_open");
    var vObject = new Object();
    vObject.This = vThis;
    vObject.HBL_NO = vThis.find("input[type='hidden']").eq(0).val();
    vObject.CNTR_NO = vThis.find("input[type='hidden']").eq(1).val();
    vObject.REQ_SVC = vThis.find("input[type='hidden']").eq(2).val();
    vObject.EX_IM_TYPE = vThis.find("input[type='hidden']").eq(3).val();

    fnGetLayerTrackingDetail(vObject);

    var $par = $(this).closest('.layer_wanna_open');
    var inx = $par.index();
    if ($par.hasClass('open') == true) {
        $('.layer_wanna_open:eq(' + inx + ')').find('.Layer_tracking_box').stop().slideUp();
        $par.removeClass('open');
    }
    else {
        if ($('.layer_wanna_open').hasClass('open')) {
            $('.layer_wanna_open').removeClass('open');
            $('.layer_wanna_open').find('.Layer_tracking_box').slideUp();
        }
        $('.layer_wanna_open:eq(' + inx + ')').addClass('open');
        $par.find('.Layer_tracking_box').slideDown();
    }
});

////////////////////////function///////////////////////
//엘비스 공통 코드 - Service 타입 가져오기
function fnSetServiceType(vSelectID, vREQ_SVC, vPAGE) {
    try {       
        var objJsonData = new Object();

        objJsonData.REQ_SVC = vREQ_SVC;

        $.ajax({
            type: "POST",
            url: "/Common/fnGetServiceType",
            async: false,
            dataType: "json",
            data: { "vJsonData": _fnMakeJson(objJsonData) },
            success: function (result) {
                fnMakeServiceType(vSelectID, vPAGE , result);
            }, error: function (xhr, status, error) {                
                _fnAlertMsg("담당자에게 문의 하세요.");
                console.log(error);
            }
        });
    }
    catch (err) {
        console.log("[Error - fnSetServiceType]" + err.message);
    }
}

//로그인 레이어 팝업 보여주기
function fnShowLoginLayer(vValue) {

    if (vValue == "init") {
        _initPage = "";
    } else if (vValue.split(';')[0] == "goSEABooking" || vValue.split(';')[0] == "goAIRBooking") {
        _initPage = vValue;
    } else {
        _initPage = vValue;
    }

    layerPopup3("#login_pop", false);
    $("#Login_Password").val("");
    $("#Login_ID").focus();
}


//화물추적 B/L 제출 되었는지 확인
function isLayerChkBL(vMngtNo,vCntrNo) {
    try {
        var isBoolean = true;
        var objJsonData = new Object();

        objJsonData.HBL_NO = vMngtNo;
        objJsonData.CNTR_NO = vCntrNo;
        objJsonData.OFFICE_CD = _Office_CD;

        $.ajax({
            type: "POST",
            url: "/Home/fnChkBL",
            async: false,
            dataType: "json",
            data: { "vJsonData": _fnMakeJson(objJsonData) },
            success: function (result) {

                //받은 데이터 Y / N 체크
                if (JSON.parse(result).Result[0]["trxCode"] == "Y") {
                    ////엘비스 - 제출 여부 확인 , 추후 엘비스 작업 후에 다시 열기 
                    if (JSON.parse(result).Check[0]["SEND_YN"] == "Y") {


                        if (JSON.parse(result).Check[0]["CHKBL_YN"] == "Y") {
                            _objTrkLayer.HBL_NO = JSON.parse(result).Check[0]["HBL_NO"];
                            _objTrkLayer.MBL_NO = JSON.parse(result).Check[0]["MBL_NO"];
                            _objTrkLayer.REQ_SVC = JSON.parse(result).Check[0]["REQ_SVC"];
                            _objTrkLayer.TOKEN = JSON.parse(result).Check[0]["TOKEN"];
                            _objTrkLayer.EXPIREDDT = JSON.parse(result).Check[0]["EXPIREDDT"] //  만료일 추가 , 쿼리로 수정 필요
                            /*_objTrkLayer.SCAC_CD = JSON.parse(result).Check[0]["SCAC_CD"];*/

                            isBoolean = true;
                        }
                        else if (JSON.parse(result).Check[0]["CHKBL_YN"] == "N") {
                        _fnAlertMsg("B/L 제출을 해주시기 바랍니다.");
                        isBoolean = false;
                        }

                        else {
                            _fnAlertMsg("담당자에게 문의 하세요.");
                            isBoolean = false;
                             }
                    }

                    else if (JSON.parse(result).Check[0]["SEND_YN"] == "D") {
                        _fnAlertMsg("Tracking 불가 자료입니다.");
                        isBoolean = false;
                             }
                        else {
                            _fnAlertMsg("화물 추적 준비중입니다.");
                            isBoolean = false;
                        }
                }
                else if (JSON.parse(result).Result[0]["trxCode"] == "N") {
                    _fnAlertMsg("Tracking 정보가 없습니다");
                    isBoolean = false;
                }
                else if (JSON.parse(result).Result[0]["trxCode"] == "E") {
                    _fnAlertMsg("B/L 정보가 없습니다");
                    isBoolean = false;
                }
            }, error: function (xhr, status, error) {
                _fnAlertMsg2("담당자에게 문의 하세요.");
                console.log(error);
            }
        });

        return isBoolean;
    }
    catch (err) {
        console.log("[Error - isLayerChkBL()]" + err.message);
    }

}




function fnLayerTrkList(vREQ_SVC) {
    try {

        if (vREQ_SVC == "SEA") {
             try {
                 //layerClose('#L_delivery_pop');
            
                $("#Tradlx_token").val("Bearer " + _objTrkLayer.TOKEN)
                frm = document.getElementById("viewerForm");
                frm.action = "https://txweb-vtrack-test.azurewebsites.net/custom/shipment-detail?blNo=" + _objTrkLayer.MBL_NO;
                frm.target = "viewer";
                frm.method = "post";
                frm.submit();
                layerPopup2("#SVT_tracking_layer");
            }
              catch (err) {
                console.log("[Error - API ERROR()]" + err.message);
            }

        } 
        else if (vREQ_SVC == "AIR") {
            //layerPopup2('#L_delivery_pop');
            $.ajax({
                type: "POST",
                url: "/Home/GetAirTrackingList",
                async: true,
                dataType: "json",
                data: { "vJsonData": _fnMakeJson(_objTrkLayer) },
                success: function (result) {                    
                    layerClose('#SVT_tracking_layer');

                    //받은 데이터 Y / N 체크
                    if (JSON.parse(result).Result[0]["trxCode"] == "Y") {
                        //데이터 그려주기
                        fnMakeLayerLAirTrkList(result);
                    }
                    else if (JSON.parse(result).Result[0]["trxCode"] == "N") {
                        _fnAlertMsg("화물추적 데이터가 없습니다.");
                        console.log("[Fail : fnLayerTrkList(" + vREQ_SVC+")]" + JSON.parse(result).Result[0]["trxMsg"]);                       
                    }
                    else if (JSON.parse(result).Result[0]["trxCode"] == "E") {                        
                        console.log("[Error : fnLayerTrkList(" + vREQ_SVC +")]" + JSON.parse(result).Result[0]["trxMsg"]);
                        _fnAlertMsg("[Error]관리자에게 문의 해 주세요.");
                    }
                }, error: function (xhr, status, error) {
                    alert("담당자에게 문의 하세요.");
                    console.log(error);
                },
                beforeSend: function () {
                    $("#ProgressBar_Loading").show(); //프로그래스 바
                },
                complete: function () {
                    $("#ProgressBar_Loading").hide(); //프로그래스 바
                }
            });
        }
    }
    catch (err) {
        console.log("[Error - fnLayerTrkList(" + vREQ_SVC +")]" + err.message);
    }
}

//화물추적 데이터 - Detail 부분만 데이터 가져오기
function fnGetLayerTrackingDetail(vObject) {
    try {

        var rtnJson;
        var objJsonData = new Object();

        objJsonData.HBL_NO = vObject.HBL_NO;
        objJsonData.CNTR_NO = vObject.CNTR_NO;
        objJsonData.REQ_SVC = vObject.REQ_SVC;
        objJsonData.EX_IM_TYPE = vObject.EX_IM_TYPE;

        $.ajax({
            type: "POST",
            url: "/Home/GetTrackingDetail",
            async: false,
            dataType: "json",
            data: { "vJsonData": _fnMakeJson(objJsonData) },
            success: function (result) {

                //받은 데이터 Y / N 체크
                if (JSON.parse(result).Result[0]["trxCode"] == "Y") {
                    //데이터 그려주기                    
                    fnMakeLayerAirTrackingData(vObject.This, result);
                }
                else if (JSON.parse(result).Result[0]["trxCode"] == "N") {
                    console.log("[Fail : getTracking()]" + JSON.parse(result).Result[0]["trxMsg"]);
                    _fnAlertMsg2("Tracking 정보가 없습니다");
                }
                else if (JSON.parse(result).Result[0]["trxCode"] == "E") {
                    $(".delivery_status").hide();
                    console.log("[Error : getTracking()]" + JSON.parse(result).Result[0]["trxMsg"]);
                    _fnAlertMsg2("[Error]관리자에게 문의 해 주세요.");
                }
            }, error: function (xhr, status, error) {
                _fnLayerAlertMsg("담당자에게 문의 하세요.");
                console.log(error);
            }
        });
    }
    catch (err) {
        console.log("[Error - fnGetTracking()]" + err.message);
    }
}

//화물추적 초기화
function fnTrkLayerInit() {
    try {                
        $("#layer_tracking_List").empty();

        //씨벤티지 누를때마다 초기화가 안되서 지웠다가 다시 그림
        $("#SVT_layer_Area").empty();
        var vHTML = "";
        vHTML += "<iframe src=\"\" id=\"SVT_layer_map\" class=\"map\" width=\"100%\" height=\"100%\"></iframe>";
        $("#SVT_layer_Area")[0].innerHTML = vHTML;
    }
    catch (err) {
        console.log("[Error - fnTrkInit]" + err.message);
    }
}


//로그인 함수
function _fnLogin() {
    try {
        //로그인 체크
        if ($("#Login_ID").val() == "") {
            $("#Password_Warning").hide();
            $("#Email_Warning").show();
            $("#Login_ID").focus();
            $("#Login_ID").click();
            return false;
        }
        else {
            $("#Email_Warning").hide();
        }
        if ($("#Login_Password").val() == "") {
            $("#Email_Warning").hide();
            $("#Password_Warning").show();
            $("#Login_Password").focus();
            $("#Login_Password").click();
            return false;
        }
        else {
            $("#Password_Warning").hide();
        }

        var objJsonData = new Object();
        objJsonData.USR_ID = $("#Login_ID").val();
        objJsonData.PSWD = $("#Login_Password").val();

        $.ajax({
            type: "POST",
            url: "/Home/fnLogin",
            async: true,
            dataType: "json",
            data: { "vJsonData": _fnMakeJson(objJsonData) },
            success: function (result) {

                if (JSON.parse(result).Result[0]["trxCode"] == "Y") {

                    if (JSON.parse(result).Table[0].APV_YN == "Y") {
                        //자동 로그인일 경우
                        if ($('input[name=login_auto]')[0].checked) {
                            _fnSetCookie("Prime_CK_USR_ID_REMEMBER_SSGM", JSON.parse(result).Table[0].USR_ID, "168");
                            _fnSetCookie("Prime_CK_PASSWORD_REMEMBER_SSGM", $("#Login_Password").val(), "168");
                        }
                        else {
                            _fnDelCookie("Prime_CK_USR_ID_REMEMBER_SSGM");
                            _fnDelCookie("Prime_CK_PASSWORD_REMEMBER_SSGM");

                            //아이디 저장 체크 일 경우 쿠키에 저장
                            if ($('input[name=login_keep]')[0].checked) {
                                _fnSetCookie("Prime_CK_USR_ID_REMEMBER_SSGM", JSON.parse(result).Table[0].USR_ID, "168");
                            } else {
                                _fnDelCookie("Prime_CK_USR_ID_REMEMBER_SSGM");
                            }
                        }


                        $.ajax({
                            type: "POST",
                            url: "/Home/SaveLogin",
                            async: true,
                            data: { "vJsonData": _fnMakeJson(JSON.parse(result)) },
                            success: function (result, status, xhr) {

                                if (_fnToNull(result) == "Y") {
                                    if (_initPage.split(';')[0] == "goBooking") {

                                        var objJsonData = new Object();
                                        objJsonData.SCH_NO = _initPage.split(';')[1];
                                        sessionStorage.setItem("BEFORE_VIEW_NAME", "SCHEDULE_SEA");
                                        sessionStorage.setItem("VIEW_NAME", "REGIST");
                                        sessionStorage.setItem("BOUND", $("#Select_Bound").find("option:selected").val());

                                        if ($("#Select_Bound").find("option:selected").val() == "E") {
                                            sessionStorage.setItem("POL_NM", $("#input_POL").text());
                                            sessionStorage.setItem("POL_CD", $("#input_POLCD").val());
                                            sessionStorage.setItem("POD_NM", $("#input_POD").val());
                                            sessionStorage.setItem("POD_CD", $("#input_PODCD").val());
                                        }

                                        sessionStorage.setItem("ETD", $("#input_ETD").val());
                                        sessionStorage.setItem("CNTR_TYPE", $("#select_CntrType").find("option:selected").val());

                                        controllerToLink("Regist", "Booking", objJsonData);
                                    } else {
                                        window.location = window.location.origin + _initPage;
                                    }
                                }
                                else if (_fnToNull(result) == "N") {
                                    console.log("[Fail : fnLogin()]");
                                    _fnAlertMsg("관리자에게 문의 하세요");
                                }
                                else {
                                    console.log("[Error : fnLogin()]" + _fnToNull(result));
                                    _fnAlertMsg("관리자에게 문의 하세요");
                                }
                            }
                        });
                    }
                    else if (JSON.parse(result).Table[0].APV_YN == "N") {
                        _fnAlertMsg("승인이 되지 않았습니다. 담당자에게 문의 하세요.");
                    }
                    else if (JSON.parse(result).Table[0].APV_YN == "D") {
                        _fnAlertMsg("가입 승인이 거절 되었습니다. 메일에서 거절 사유를 확인 해 주세요.");
                    }
                    else if (JSON.parse(result).Table[0].APV_YN == "S") {
                        _fnAlertMsg("아이디가 정지 되었습니다. 담당자에게 문의 하세요.");
                    }
                }
                else if (JSON.parse(result).Result[0]["trxCode"] == "N") {
                    _fnLayerAlertMsg("아이디 혹은 비밀번호가 틀렸습니다");
                }
            },  error: function (xhr) {
                console.log("시스템 사정으로 요청하신 작업을 처리할 수 없습니다.");
                console.log(xhr);
                return;
            }, beforeSend: function () {
                $("#ProgressBar_Loading").show(); //프로그래스 바 

            },
            complete: function () {
                $("#ProgressBar_Loading").hide(); //프로그래스 바 
            }
        });
    } catch (err) {
        console.log(err.message);
    }
}

////로그아웃 (세션 , 쿠키 삭제)
function _fnLogout() {
    $.ajax({
        type: "POST",
        url: "/Home/LogOut",
        async: false,
        success: function (result, status, xhr) {

            $("#Session_USR_ID ").val("");
            $("#Session_LOC_NM ").val("");
            $("#Session_EMAIL").val("");
            $("#Session_CUST_CD").val("");
            $("#Session_HP_NO").val("");
            $("#Session_DOMAIN").val("");
            $("#Session_OFFICE_CD").val("");
            $("#Session_AUTH_KEY").val("");
            $("#Session_USR_TYPE").val("");

            _fnDelCookie("Prime_CK_PASSWORD_REMEMBER_SSGM"); //자동 로그인 떄문에 넣어둠

            location.href = window.location.origin;
        }
    });
}

//같은 Port를 선택 하였는지 체크 
//vPort = Port Code 명
//vREQ_SVC = SEA / AIR 
//vPOL_POD = POL / POD 
//vType = Q (즐겨찾기) , A (자동완성)
//vPopID = 즐겨찾기 ID 
function _fnCheckSamePort(vPort,vREQ_SVC,vPOL_POD,vType,vPopID) {
	try {

		var vPortPOL = "";
		var vPortPOD = "";

        //POL , POD 세팅
		if (vPOL_POD == "POL") {
            vPortPOL = vPort;
            if (vREQ_SVC != "") {
                vPortPOD = _fnToNull($("#input_" + vREQ_SVC + "_POD").val());
            } else {
                vPortPOD = _fnToNull($("#input_POD").val());
            }
		}
        else if (vPOL_POD == "POD") {
            if (vREQ_SVC != "") {
                vPortPOL = _fnToNull($("#input_" + vREQ_SVC + "_POL").val());
            } else {
                vPortPOL = _fnToNull($("#input_POL").val());
            }            
            vPortPOD = vPort;
        }

        //POL CD와 POD CD 비교
        if (vPortPOL == vPortPOD) {
            var vInit = "";     //TEXT 초기화 
            var vInitCD = "";   //Code 초기화

            //Focus On
            if (vPOL_POD == "POL") {
                if (vREQ_SVC != "") {                    
                    vInit = "input_" + vREQ_SVC + "_POL";
                    vInitCD = "input_" + vREQ_SVC + "_Departure";
                }
                else {                    
                    vInit = "input_POL";
                    vInitCD = "input_Departure";
                }
            } else if (vPOL_POD == "POD") {
                if (vREQ_SVC != "") {
                    vInit = "input_" + vREQ_SVC + "_POD";
                    vInitCD = "input_" + vREQ_SVC + "_Arrival";
                }
                else {
                    vInit = "input_POD";
                    vInitCD = "input_Arrival";
                }
            }

            if (vType == "Q") { //Quick 즐겨찾기 포트 
                _fnAlertMsg("출발지와 도착지의 Port가 동일 합니다.", vInitCD);
                _QuickMenu = vPopID;                
                $("#" + vInit).val("");
                $("#" + vInitCD).val("");
            }
            else if (vType == "A") { //Auto 자동완성 
                _fnAlertMsg("출발지와 도착지의 Port가 동일 합니다.", vInitCD);
            }

            return false;
        }
        return true; 
	}
	catch (err) {
		console.log("[Error - fnCheckSamePort]" + err.message);
    }
}

/////////////////function MakeList/////////////////////
//엘비스 공통 코드 - Service 타입 그려주기
function fnMakeServiceType(vSelectID, vPAGE, vJsonData) {
    try {

        var vHTML = "";
        var vResult = "";

        if (JSON.parse(vJsonData).Result[0]["trxCode"] == "Y") {
            vResult = JSON.parse(vJsonData).Service;

            if (vPAGE == "SCH") {
                vHTML += "<option value=\"\">Service</option>";
            }

            $.each(vResult, function (i) {
                vHTML += "<option value=\"" + vResult[i].CODE + "\">" + vResult[i].NAME + "</option>";
            });

            $(vSelectID)[0].innerHTML = vHTML;
        }
        else if (JSON.parse(vJsonData).Result[0]["trxCode"] == "N") {
            console.log("[Fail - fnMakeServiceType]" + JSON.parse(vJsonData).Result[0]["trxMsg"]);
        }
        else if (JSON.parse(vJsonData).Result[0]["trxCode"] == "E") {
            console.log("[Fail - fnMakeServiceType]" + JSON.parse(vJsonData).Result[0]["trxMsg"]);
        }
    }
    catch (err) {
        console.log("[Error - fnMakeServiceType]" + err.message);
    }
}


// 항공 트래킹 그리기
function fnMakeLayerLAirTrkList(vJsonData) {
    try {
        var vHTML = "";
        var vResult = "";

        if (JSON.parse(vJsonData).Result[0]["trxCode"] == "Y") {

            vResult = JSON.parse(vJsonData).Main;

            if (_fnToNull(vResult) != "") {
                layerPopup2('#L_delivery_pop');
                $("#layer_tracking_List").empty();
                $("#Layer_L_Tracking").val(_fnToNull(vResult[0].HBL_NO));

                $(vResult).each(function (i) {

                    vHTML += "   <div class=\"trk_box layer_wanna_open\"> ";
                    vHTML += "      <div class=\"trk_cover\"> ";

                    vHTML += " <input type=\"hidden\" value=\"" + _fnToNull(vResult[i].HBL_NO) + "\" /> ";
                    vHTML += " <input type=\"hidden\" value=\"" + _fnToNull(vResult[i].CNTR_NO) + "\" /> ";
                    vHTML += " <input type=\"hidden\" value=\"" + _fnToNull(vResult[i].REQ_SVC) + "\" /> ";
                    vHTML += " <input type=\"hidden\" value=\"" + _fnToNull(vResult[i].EX_IM_TYPE) + "\" /> ";

                    vHTML += "      	<div class=\"trk_info\"> ";
                    vHTML += "      		<p>HAWB No.</p> ";
                    vHTML += "      		<p class=\"trk_cont\">" + _fnToNull(vResult[i].HBL_NO) + "</p> ";
                    vHTML += "      	</div> ";
                    vHTML += "      	<div class=\"trk_info\" style=\"display:none;\"> ";
                    vHTML += "      		<p>Container No.</p> ";
                    vHTML += "      		<p class=\"trk_cont\">" + _fnToNull(vResult[i].CNTR_NO) + "</p> ";
                    vHTML += "      	</div> ";
                    vHTML += "      	<div class=\"trk_info\"> ";
                    vHTML += "      		<p>MAWB No.</p> ";
                    vHTML += "      		<p class=\"trk_cont\">" + _fnToNull(vResult[i].MBL_NO) + "</p> ";
                    vHTML += "      	</div> ";
                    vHTML += "      	<div class=\"trk_info\"> ";
                    vHTML += "      		<p>Status</p> ";
                    vHTML += "      		<p class=\"trk_cont\">" + _fnToNull(vResult[i].NOW_EVENT_NM) + "</p> ";
                    vHTML += "      	</div> ";
                    vHTML += "      	<div class=\"trk_info\"> ";
                    vHTML += "      		<p>Location</p> ";
                    vHTML += "      		<p class=\"trk_cont\">" + _fnToNull(vResult[i].ACT_LOC_NM) + "</p> ";
                    vHTML += "      	</div> ";
                    if (i == 0) {
                        vHTML += "      	<button class=\"btn_open Layer_clickevent\"></button> ";
                    } else {
                        vHTML += "      	<button class=\"btn_open\" name=\"btn_LayerTrkDTL\"></button> ";
                    }

                    vHTML += "      </div> ";
                    vHTML += "      <div class=\"Layer_tracking_box tracking_box\"></div> ";
                    vHTML += "   </div> ";
                });

                $("#layer_tracking_List").append(vHTML);
                $(".Layer_clickevent").click();
            } else {
                _fnLayerAlertMsg("화물추적 데이터가 없습니다.");
            }
        }
    }
    catch (err) {
        console.log("[Error - fnMakeLayerLAirTrkList]" + err.message);
    }
}

//Air 레이어 화물 추적 마일스톤 데이터 그리기
function fnMakeLayerAirTrackingData(vThis, vJsonData) {
    try {

        var vHTML = "";

        if (JSON.parse(vJsonData).Result[0]["trxCode"] == "Y") {
            var vResult = JSON.parse(vJsonData).DTL;

            $.each(vResult, function (i) {

                var vLast = "";

                if (vResult[i].EX_IM_TYPE == "E") {
                    if (i == 1) {
                        vLast = "last";
                    }

                    if (_fnToNull(vResult[i].EVENT_STATUS) == "Y") {
                        vHTML += "   <div class=\"track_stat air on " + vLast + " export\"> ";
                    }
                    else if (_fnToNull(vResult[i].EVENT_STATUS) == "N") {
                        vHTML += "   <div class=\"track_stat air yet " + vLast + " export\"> ";
                    }
                    else if (_fnToNull(vResult[i].EVENT_STATUS) == "E") {
                        vHTML += "   <div class=\"track_stat air " + vLast + " export\"> ";
                    }

                    vHTML += "   	<div class=\"track_proc\"> ";
                    vHTML += "   		<div class=\"track_cell\"> ";
                    vHTML += "   			<p>" + _fnToNull(vResult[i].EVENT_NM) + "</p> ";
                    vHTML += "   		</div> ";
                    vHTML += "   	</div> ";
                    vHTML += "   	<div class=\"track_process\"> ";
                    vHTML += "   		<div class=\"track_inner\"> ";
                    vHTML += "   			<div class=\"track img\"> ";
                    vHTML += "   				<div class=\"track_cell\"> ";
                    vHTML += "   				</div> ";
                    vHTML += "   			</div> ";
                    vHTML += "   			<div class=\"track loc\"> ";
                    vHTML += "   				<div class=\"track_cell\"> ";
                    vHTML += "   					<p class=\"title\">LOCATION</p> ";
                    vHTML += "   					<p style='color:#0085b4'>" + _fnToNull(vResult[i].ACT_LOC_NM) + "</p> ";
                    vHTML += "   				</div> ";
                    vHTML += "   			</div> ";
                    vHTML += "   			<div class=\"track dnt\"> ";
                    vHTML += "   				<div class=\"track_cell\"> ";
                    vHTML += "   					<p class=\"title\">DATE AND TIME</p> ";
                    //vHTML += "   					<p>" + _fnToNull(vResult[i].EST_YMD) + " " + _fnToNull(vResult[i].EST_HM) + "</p> ";
                    vHTML += "   					<p style='color:#0085b4'>" + _fnToNull(vResult[i].ACT_YMD) + " " + _fnToNull(vResult[i].ACT_HM) + "</p> ";
                    vHTML += "   				</div> ";
                    vHTML += "   			</div> ";
                    vHTML += "   		</div> ";
                    vHTML += "   	</div> ";
                    vHTML += "   </div> ";
                }
                else if (vResult[i].EX_IM_TYPE == "I") {
                    if (i == 1) {
                        vLast = "last";
                    }

                    if (_fnToNull(vResult[i].EVENT_STATUS) == "Y") {
                        vHTML += "   <div class=\"track_stat air on " + vLast + " import\"> ";
                    }
                    else if (_fnToNull(vResult[i].EVENT_STATUS) == "N") {
                        vHTML += "   <div class=\"track_stat air yet " + vLast + " import\"> ";
                    }
                    else if (_fnToNull(vResult[i].EVENT_STATUS) == "E") {
                        vHTML += "   <div class=\"track_stat air " + vLast + " import\"> ";
                    }

                    vHTML += "   	<div class=\"track_proc\"> ";
                    vHTML += "   		<div class=\"track_cell\"> ";
                    vHTML += "   			<p>" + _fnToNull(vResult[i].EVENT_NM) + "</p> ";
                    vHTML += "   		</div> ";
                    vHTML += "   	</div> ";
                    vHTML += "   	<div class=\"track_process\"> ";
                    vHTML += "   		<div class=\"track_inner\"> ";
                    vHTML += "   			<div class=\"track img\"> ";
                    vHTML += "   				<div class=\"track_cell\"> ";
                    vHTML += "   				</div> ";
                    vHTML += "   			</div> ";
                    vHTML += "   			<div class=\"track loc\"> ";
                    vHTML += "   				<div class=\"track_cell\"> ";
                    vHTML += "   					<p class=\"title\">LOCATION</p> ";
                    vHTML += "   					<p style='color:#0085b4'>" + _fnToNull(vResult[i].ACT_LOC_NM) + "</p> ";
                    vHTML += "   				</div> ";
                    vHTML += "   			</div> ";
                    vHTML += "   			<div class=\"track dnt\"> ";
                    vHTML += "   				<div class=\"track_cell\"> ";
                    vHTML += "   					<p class=\"title\">DATE AND TIME</p> ";
                    //vHTML += "   					<p>" + _fnDateFormat(_fnToNull(vResult[i].EST_YMD)) + " " + _fnDateFormat(_fnToNull(vResult[i].EST_HM)) + "</p> ";
                    vHTML += "   					<p style='color:#0085b4'>" + _fnDateFormat(_fnToNull(vResult[i].ACT_YMD)) + " " + _fnDateFormat(_fnToNull(vResult[i].ACT_HM)) + "</p> ";
                    vHTML += "   				</div> ";
                    vHTML += "   			</div> ";
                    vHTML += "   		</div> ";
                    vHTML += "   	</div> ";
                    vHTML += "   </div> ";
                }
            });

            $(vThis.find(".Layer_tracking_box"))[0].innerHTML = vHTML;
        }
        else if (JSON.parse(vJsonData).Result[0]["trxCode"] == "N") {
            console.log("[Fail - fnMakeLayerAirTrackingData]" + JSON.parse(vJsonData).Result[0]["trxMsg"]);
        }
        else if (JSON.parse(vJsonData).Result[0]["trxCode"] == "E") {
            _fnAlertMsg2("관리자에게 문의 하세요.");
            console.log("[Error - fnMakeLayerAirTrackingData]" + JSON.parse(vJsonData).Result[0]["trxMsg"]);
        }
    }
    catch (err) {
        console.log("[Error - fnMakeLayerAirTrackingData]" + err.message);
    }
}
var _objTrkLayer;
////////////////////////API////////////////////////////
function postPopUp(MngtNo) {
    try {
        var objJsonData = new Object();

        _objTrkLayer = new Object();

        _objTrkLayer.OFFICE_CD = _Office_CD;
        if (isLayerChkBL(MngtNo, "")) {

            /*            fnTrkLayerInit(); //초기화*/

            if (_objTrkLayer.REQ_SVC == "SEA") {

                //layerClose('#L_delivery_pop');
                //$("#SVT_P_HBL_NO").val(vMngtNo);
                //$("#SVT_P_CNTR_NO").val(vCntrNo);
                _objTrkLayer.HBL_NO = MngtNo;
                _objTrkLayer.CNTR_NO = "";
                _objTrkLayer.OFFICE_CD = _Office_CD;
                fnLayerTrkList("SEA");

            }
            else if (_objTrkLayer.REQ_SVC == "AIR") {

                //Delivery Pop을 띄어주는 로직으로 변경.
                //$("#LF_P_HBL_NO").val(vMngtNo);
                //$("#LF_P_CNTR_NO").val(vCntrNo);
                _objTrkLayer.HBL_NO = MngtNo;
                _objTrkLayer.CNTR_NO = "";
                _objTrkLayer.OFFICE_CD = _Office_CD;
                fnLayerTrkList("AIR");
            }
        }
        else {
            //layerClose('#L_delivery_pop');
            //layerClose('#SVT_tracking_layer');
        }
}
    catch (err) {
    console.log("[Error - fnfngetLayerTracking()]" + err.message);
}

}

$(document).on("keyup", "#mng_no", function (e) {
    if (e.keyCode == 13) {
        if (_fnToNull($("#mng_no").val()) == "") {
            _fnAlertMsg("검색할 번호를 입력해주세요.");
        } else {
            postPopUp($("#mng_no").val().toUpperCase().trim());
        }
    }
});

$(document).on("keyup", "#Mngt_no", function (e) {
    if (e.keyCode == 13) {
        if (_fnToNull($("#Mngt_no").val()) == "") {
            _fnAlertMsg("검색할 번호를 입력해주세요.");
        } else {
            postPopUp($("#Mngt_no").val().toUpperCase().trim());
        }
    }
});

$(document).on("keyup", "#Mngt_no_Mo", function (e) {
    if (e.keyCode == 13) {
        if (_fnToNull($("#Mngt_no_Mo").val()) == "") {
            _fnAlertMsg("검색할 번호를 입력해주세요.");
        } else {
            postPopUp($("#Mngt_no_Mo").val().toUpperCase().trim());
        }
    }
});
$(document).on("keyup", "#Mng_no_Mo", function (e) {
    if (e.keyCode == 13) {
        if (_fnToNull($("#Mng_no_Mo").val()) == "") {
            _fnAlertMsg("검색할 번호를 입력해주세요.");
        } else {
            postPopUp($("#Mng_no_Mo").val().toUpperCase().trim());
        }
    }
});
$(document).on("click", "#SearchTracking", function () {
    if (_fnToNull($("#mng_no").val()) == "") {
        _fnAlertMsg3("검색할 번호를 입력해주세요.");
    } else {
        postPopUp($("#mng_no").val().toUpperCase().trim());
    }
})
$(document).on("click", "#SearchTrackingMo", function () {
    if (_fnToNull($("#Mngt_no_Mo").val()) == "") {
        _fnAlertMsg("검색할 번호를 입력해주세요.");
    } else {
        postPopUp($("#Mngt_no_Mo").val().toUpperCase().trim());
    }
})

$(document).on("click", "#SearchTrackingMo_SUB", function () {
    if (_fnToNull($("#Mng_no_Mo").val()) == "") {
        _fnAlertMsg("검색할 번호를 입력해주세요.");
    } else {
        postPopUp($("#Mng_no_Mo").val().toUpperCase().trim());
    }
})




