﻿////////////////////전역 변수//////////////////////////
var _vREQ_SVC = "SEA";
var _vPage = 1; //페이징
var _vHblNo = "";
var _vInvNo = "";
var _OrderBy = "";
var _Sort = "";
var _vCfsNo = "";
var _isSearch = false;
var _DocType_MBL = "'MANI','MBL'";
var _DocType_HBL = "'CIPL', 'CHBL' ,'HBL', 'CO', 'CC', 'IP','HDC','AN','DO','ETCH'"; //INV는 따로
var _DocType_Part = "'CIPL', 'CHBL' ,'HBL', 'CO','PDC'";  //파트너
////////////////////jquery event///////////////////////
$(function () {
    //로그인 세션 확인
    if (_fnToNull($("#Session_USR_ID").val()) == "") {
        window.location = window.location.origin;
    }
    else {
        $('.delete-btn').on('click', function () {
            $(this).siblings().val('');
        });

        $("#input_ETD").val(_fnPlusDate(-91));
        $("#input_ETA").val(_fnPlusDate(92));

        //fnSetServiceType("#select_CntrType", "SEA", "");

        //이메일에서 들어왔을 경우
        if (_fnToNull($("#view_HBL_NO").val())) {
            fnMyBoardSingleSearch($("#view_HBL_NO").val());
        }
    }
});

//출발지 도착지 삭제 버튼 이벤트
$(document).on("click", ".delete-btn", function () {
    if ($(this).attr("name") == "del_POL") {
        $("#input_POL").text("출발");
        $("#input_POLCD").val("");
    }
    else if ($(this).attr("name") == "del_POD") {
        $("#input_POD").text("도착");
        $("#input_PODCD").val("");
    }

    $(this).hide();
});


//수출 - POL 클릭 (AutoComplete X)
$(document).on("click", ".pop__export a", function () {
    if ($("#Select_Bound").val() == "E") {
        $("#select_SEA_pop01").show();
        $("#select_SEA_pop02").hide();
        $("#select_SEA_pop03").hide();
        $("#select_SEA_pop04").hide();
    } else {
        $("#select_SEA_pop01").hide();
        $("#select_SEA_pop02").hide();
        $("#select_SEA_pop03").show();
        $("#select_SEA_pop04").hide();
    }
});
$(document).on("click", ".pop__import a", function () {
    if ($("#Select_Bound").val() == "E") {
        $("#select_SEA_pop02").show();
        $("#select_SEA_pop01").hide();
        $("#select_SEA_pop03").hide();
        $("#select_SEA_pop04").hide();
    } else {
        $("#select_SEA_pop02").hide();
        $("#select_SEA_pop01").hide();
        $("#select_SEA_pop03").hide();
        $("#select_SEA_pop04").show();
    }
});

//퀵 Code 데이터 - POL
$(document).on("click", "#select_SEA_pop01 button", function () {

    var vValue = $(this).val();
    var vSplit = vValue.split(";");

    $("#input_POL").text(vSplit[0]);
    $("#input_POLCD").val(vSplit[1]);
    $("#select_SEA_pop01").hide();

    //X박스 만들기
    $(".pop__export").find(".delete-btn").show();

    if ($("#input_PODCD").val() == "") {
        $("#select_SEA_pop01").hide();
        $("#select_SEA_pop02").show();
    }

    if ($("#input_POLCD").val() == $("#input_PODCD").val()) {
        _fnAlertMsg("출발지와 도착지의 Port가 동일 합니다.");
        $("#input_POL").text("출발");
        $(".pop__export").find(".delete-btn").hide();
        $("#input_POLCD").val("");
    }
});

//퀵 Code 데이터 - POL
$(document).on("click", "#select_SEA_pop02 button", function () {

    var vValue = $(this).val();
    var vSplit = vValue.split(";");

    $("#input_POD").text(vSplit[0]);
    $("#input_PODCD").val(vSplit[1]);
    $("#select_SEA_pop02").hide();

    //X박스 만들기
    $(".pop__import").find(".delete-btn").show();

    if ($("#input_POLCD").val() == "") {
        $("#select_SEA_pop01").show();
        $("#select_SEA_pop02").hide();
    }

    if ($("#input_POLCD").val() == $("#input_PODCD").val()) {
        _fnAlertMsg("출발지와 도착지의 Port가 동일 합니다.");
        $("#input_POD").text("도착");
        $(".pop__import").find(".delete-btn").hide();
        $("#input_PODCD").val("");
    }
});

//퀵 Code 데이터 - POL
$(document).on("click", "#select_SEA_pop03 button", function () {

    var vValue = $(this).val();
    var vSplit = vValue.split(";");

    $("#input_POL").text(vSplit[0]);
    $("#input_POLCD").val(vSplit[1]);
    $("#select_SEA_pop03").hide();

    //X박스 만들기
    $(".pop__export").find(".delete-btn").show();

    if ($("#input_PODCD").val() == "") {
        $("#select_SEA_pop03").hide();
        $("#select_SEA_pop04").show();
    }

    if ($("#input_POLCD").val() == $("#input_PODCD").val()) {
        _fnAlertMsg("출발지와 도착지의 Port가 동일 합니다.");
        $("#input_POL").text("출발");
        $(".pop__export").find(".delete-btn").hide();
        $("#input_POLCD").val("");
    }
});

//퀵 Code 데이터 - POL
$(document).on("click", "#select_SEA_pop04 button", function () {

    var vValue = $(this).val();
    var vSplit = vValue.split(";");

    $("#input_POD").text(vSplit[0]);
    $("#input_PODCD").val(vSplit[1]);
    $("#select_SEA_pop04").hide();

    //X박스 만들기
    $(".pop__import").find(".delete-btn").show();

    if ($("#input_POLCD").val() == "") {
        $("#select_SEA_pop03").show();
        $("#select_SEA_pop04").hide();
    }

    if ($("#input_POLCD").val() == $("#input_PODCD").val()) {
        _fnAlertMsg("출발지와 도착지의 Port가 동일 합니다.");
        $("#input_POD").text("도착");
        $(".pop__import").find(".delete-btn").hide();
        $("#input_PODCD").val("");
    }
});

//ETD 날짜 yyyymmdd 로 입력 시 yyyy-mm-dd 로 변경
$(document).on("focusout", "#input_ETD", function () {
    var vValue = $("#input_ETD").val();
    var vValue_Num = vValue.replace(/[^0-9]/g, "");
    if (vValue != "") {
        vValue = vValue_Num.substring("0", "4") + "-" + vValue_Num.substring("4", "6") + "-" + vValue_Num.substring("6", "8");
        $(this).val(vValue);
    }

    //값 벨리데이션 체크
    if (!_fnisDate($(this).val())) {
        $(this).val(_fnPlusDate(-91));
    }

    //날짜 벨리데이션 체크
    var vETD = $("#input_ETD").val().replace(/[^0-9]/g, "");
    var vETA = $("#input_ETA").val().replace(/[^0-9]/g, "");

    if (vETA < vETD) {
        _fnAlertMsg("ETD가 ETA 보다 빠를 수 없습니다. ");
        $("#input_ETD").val(vETA.substring("0", "4") + "-" + vETA.substring("4", "6") + "-" + vETA.substring("6", "8"));
    }
});

//ETD 날짜 yyyymmdd 로 입력 시 yyyy-mm-dd 로 변경
$(document).on("focusout", "#input_ETA", function () {
    var vValue = $("#input_ETA").val();
    var vValue_Num = vValue.replace(/[^0-9]/g, "");
    if (vValue != "") {
        vValue = vValue_Num.substring("0", "4") + "-" + vValue_Num.substring("4", "6") + "-" + vValue_Num.substring("6", "8");
        $(this).val(vValue);
    }

    //값 벨리데이션 체크
    if (!_fnisDate($(this).val())) {
        $(this).val(_fnPlusDate(92));
    }

    //날짜 벨리데이션 체크
    var vETD = $("#input_ETD").val().replace(/[^0-9]/g, "");
    var vETA = $("#input_ETA").val().replace(/[^0-9]/g, "");

    if (vETA < vETD) {
        _fnAlertMsg("ETA가 ETD 보다 빠를 수 없습니다. ");
        $("#input_ETA").val(vETD.substring("0", "4") + "-" + vETD.substring("4", "6") + "-" + vETD.substring("6", "8"));
    }
});


//MyBoard 검색
$(document).on("click", "#btn_MyBoard_Search", function () {

    _OrderBy = "";
    _Sort = "";
    _vPage = 1;

    fnMyBoardSearch();
});

//정렬
$(document).on("click", ".title-ctrl", function () {

    if ($(this).children("button").val().length > 0) {
        if (_isSearch) {
            var vValue = "";

            if ($(this).children("button").hasClass("on")) {
                vValue = "desc"
            }
            else {
                vValue = "asc"
            }

            $("#MyBoard_orderby th button").removeClass("on");
            if (vValue == "asc") {
                $(this).children("button").addClass('on');
            } else if (vValue == "asc") {
                $(this).children("button").removeClass('on');
            }

            _vPage = 1;
            _OrderBy = $(this).children("button").val();
            _Sort = vValue.toUpperCase();
            fnMyBoardSearch();
        }
    }
});

//파일 문서 레이어 팝업
$(document).on("click", "a[name='btn_FileList']", function () {
    try {
    var objJsonData = new Object();

    objJsonData.MNGT_NO = $(this).find("input[name='File_HBL_NO']").val(); //HBL_NO
    objJsonData.BKG_NO =  $(this).find("input[name='File_BKG_NO']").val(); //HBL_NO
    objJsonData.INV_NO =  $(this).find("input[name='File_INV_NO']").val(); //HBL_NO
    objJsonData.MBL_DOC_TYPE = _DocType_MBL;
    objJsonData.HBL_DOC_TYPE = _DocType_HBL;
    objJsonData.PARTNER_DOC_TYPE = _DocType_Part;
    objJsonData.USR_TYPE = _fnToNull($("#Session_USR_TYPE").val());

    $.ajax({
        type: "POST",
        url: "/Popup/fnGetMyBoardFileList",
        async: false,
        dataType: "json",
        data: { "vJsonData": _fnMakeJson(objJsonData) },
        success: function (result) {
            fnMakeMyBoardFileList(result);
        }, error: function (xhr, status, error) {
            alert("담당자에게 문의 하세요.");
            console.log(error);
        }
    });
}
    catch (err) {
    console.log("[Error - fnGetMyBaordFileList]" + err.message);
}

});

/////////////////function MakeList/////////////////////
//MyBoard 문서 데이터 그려주기
function fnMakeMyBoardFileList(vJsonData) {
    try {

        var vHTML = "";
        layerPopup($("#File_detail"));
        if (JSON.parse(vJsonData).Result[0]["trxCode"] == "Y") {
            var vResult = JSON.parse(vJsonData).FileList;

            $.each(vResult, function (i) {
                vHTML += "   <li> ";
                vHTML += "   	<div class=\"file_sort\"> ";
                vHTML += "   		<p>" + _fnToNull(vResult[i]["DOC_NM"]) + "</p> ";
                vHTML += "   	</div> ";
                vHTML += "   	<div class=\"file_nm\"> ";
                vHTML += "   		<a href=\"javascript:void(0)\" class=\"file_name\" name=\"MyBoard_FileDown\"> ";
                vHTML += _fnToNull(vResult[i]["MNGT_NM"]);
                vHTML += "              <input type=\"hidden\" name=\"MyBoard_File_MNGT_NO\" value=\"" + _fnToNull(vResult[i]["MNGT_NO"]) + "\" /> ";
                vHTML += "              <input type=\"hidden\" name=\"MyBoard_File_SEQ\" value=\"" + _fnToNull(vResult[i]["SEQ"]) + "\" /> ";
                vHTML += "              <input type=\"hidden\" name=\"MyBoard_File_FILE_PATH\" value=\"" + _fnToNull(vResult[i]["FILE_PATH"]) + "\" /> ";
                vHTML += "              <input type=\"hidden\" name=\"MyBoard_File_REPLACE_FILE_NM\" value=\"" + _fnToNull(vResult[i]["REPLACE_FILE_NM"]) + "\" /> ";
                vHTML += "   		</a> ";
                vHTML += "   	</div> ";
                vHTML += "   </li> ";
            });

            $("#MyBoard_FileList_AREA")[0].innerHTML = vHTML;
        }
        else if (JSON.parse(vJsonData).Result[0]["trxCode"] == "N") {
            $(".mfp-close").click();
            alert("문서 파일이 없습니다.");
        }
        else if (JSON.parse(vJsonData).Result[0]["trxCode"] == "E") {
            $(".mfp-close").click();
            alert("담당자에게 문의 하세요.");
        }
    }
    catch (err) {
        console.log("[Error - fnMakeMyBoardFileList]" + err.message);
    }
}
//파일 다운로드 로직
$(document).on("click", "a[name='MyBoard_FileDown']", function () {
    var vMNGT_NO = $(this).find("input[name='MyBoard_File_MNGT_NO']").val();
    var vSEQ = $(this).find("input[name='MyBoard_File_SEQ']").val();

    fnMyBaord_FileDown(vMNGT_NO, vSEQ);
});

//파일 다운로드 로직
function fnMyBaord_FileDown(vMNGT, vSEQ) {
    try {
        var objJsonData = new Object();

        objJsonData.MNGT_NO = vMNGT;  //부킹 번호        
        objJsonData.DOMAIN = $("#Session_DOMAIN").val(); //도메인
        objJsonData.SEQ = vSEQ;

        $.ajax({
            type: "POST",
            url: "/File/DownloadElvis",
            async: false,
            data: { "vJsonData": _fnMakeJson(objJsonData) },
            success: function (result, status, xhr) {

                if (result != "E") {
                    var rtnTbl = JSON.parse(result);
                    rtnTbl = rtnTbl.Path;
                    var file_nm = rtnTbl[0].FILE_NAME;
                    if (_fnToNull(rtnTbl) != "") {
                        window.location = "/File/DownloadFile?FILE_NM=" + file_nm + "&REPLACE_FILE_NM=" + rtnTbl[0].FILE_REAL_NAME;
                    }
                } else {
                    _fnAlertMsg("다운 받을 수 없습니다.");
                }
            },
            error: function (xhr, status, error) {
                alert("[Error]관리자에게 문의 해 주세요. " + status);
                return;
            }
        });
    }
    catch (err) {
        console.log("[Error - fnMyBaord_FileDown]" + err.message);
    }
}


//Tracking 레이어 팝업 
$(document).on("click", "a[name='btn_Tracking']", function () {
    if (_fnToNull($(this).find("input[name='Tracking_HBL_NO']").val()) == "") {
        _fnAlertMsg("B/L 제출을 해주시기 바랍니다.");
    } else {
        postPopUp($(this).find("input[name='Tracking_HBL_NO']").val().toUpperCase().trim());
    }
});

//BL 점 클릭 이벤트 - Web Viewer 보여주기
$(document).on("click", "span[name='layer_BLPrintData']", function () {

    vHBL_NO = $(this).siblings("input[name='HBL_NO']").val();
    var vDOC_TYPE = $(this).siblings("input[name='DOC_TYPE']").val();

    if (vDOC_TYPE == "CHBL") {
        $("#Layer_Iframe_title").text("Check B/L");
    } else if (vDOC_TYPE == "AN") {
        $("#Layer_Iframe_title").text("Arrival Notice");
    }

    fnPrint(vHBL_NO, vDOC_TYPE);
});

//Invoice 점 클릭 이벤트 - Web Viewer 보여주기
$(document).on("click", "span[name='layer_InvPrintData']", function () {

    _vHblNo = $(this).siblings("input[name='HBL_NO']").val();
    _vInvNo = $(this).siblings("input[name='INV_NO']").val();

    $("#Layer_Iframe_title").text("Invoice");
    fnInvPrint(_vHblNo);
});

//부킹 눌렀을때 데이터 채우기
$(document).on("click", "span[name='layer_CFSPrintData']", function () {
    _vCfsNo = $(this).siblings("input[name='CFS_BKG_NO']").val();

    fnCfsPrint(_vCfsNo);

    var isMobile = /Android|webOS|iPhone|iPad|iPod|BlackBerry/i.test(navigator.userAgent) ? true : false;
    if (!isMobile) {
        //PC
        $("#PC_Layer_Console_Header tr").eq(0).click();
    } else {
        //MOBILE
        $("#MO_Layer_Console_Header ul").eq(0).click();
    }

    layerPopup($("#ware_pop"));
});


function fnCfsPrint(vCFS) {
    var objJsonData = new Object();

    objJsonData.CFS_BKG_NO = vCFS;

    $.ajax({
        type: "POST",
        url: "/MyData/fnGetCfsData",
        async: false,
        dataType: "json",
        data: { "vJsonData": _fnMakeJson(objJsonData) },
        success: function (result) {
            fnLayer_Console_Header(result);
            //fnLayer_Console_Detail(result);
        }, error: function (xhr, status, error) {
            alert("담당자에게 문의 하세요.");
            console.log(error);
            vReturn = false;
        }
    });
}

//부킹 헤더 그리기
function fnLayer_Console_Header(vJsonData) {
    var vHTML = "";

    try {
        if (_fnToNull(JSON.parse(vJsonData).CFS_MST) != "") {
            var vResult = JSON.parse(vJsonData).CFS_MST;

            $.each(vResult, function (i) {
                vHTML += "   <tr> ";
                vHTML += "   	<td class=\"txt\">" + _fnToNull(vResult[i]["BK_NO"]) + "</td> ";
                vHTML += "   	<td class=\"txt\">" + _fnToNull(vResult[i]["GR_NO"]) + "</td> ";
                vHTML += "   	<td class=\"txt\">" + _fnFormatDate(_fnToNull(vResult[i]["GR_YMD"])) + "</td> ";
                vHTML += "   	<td class=\"txt\">" + _fnToNull(vResult[i]["ACT_CUST_NM"]) + "</td> ";
                vHTML += "          <input type='hidden' name='BK_NO' value='" + _fnToNull(vResult[i]["BK_NO"]) + "'> ";
                vHTML += "          <input type='hidden' name='BK_SEQ' value='" + _fnToNull(vResult[i]["BK_SEQ"]) + "'> ";
                vHTML += "   </tr> ";
            });

            $("#PC_Layer_Console_Header")[0].innerHTML = vHTML;
        }

        vHTML = "";

        $.each(vResult, function (i) {
            vHTML += "                                    <ul class='layer_console_mo'> ";
            vHTML += "                                        <li class='info-txt info-txt--full'>";
            vHTML += "                                            <p class='title font - main'>Booking No</p>";
            vHTML += "                                            <p class='des'> " + _fnToNull(vResult[i]["BK_NO"]) + " </p>";
            vHTML += "                                        </li>";
            vHTML += "                                        <li class='info-txt info-txt--full'>";
            vHTML += "                                            <p class='title font - main'>입고 번호</p>";
            vHTML += "                                            <p class='des'>" + _fnToNull(vResult[i]["GR_NO"]) + "</p>";
            vHTML += "                                        </li>";
            vHTML += "                                        <li class='info-txt info-txt--full'>";
            vHTML += "                                            <p class='title font - main'>입고 일자</p>";
            vHTML += "                                            <p class='des'>" + _fnFormatDate(_fnToNull(vResult[i]["GR_YMD"])) + "</p>";
            vHTML += "                                        </li>";
            vHTML += "                                        <li class='info-txt info-txt--full'>";
            vHTML += "                                            <p class='title font - main'>실화주</p>";
            vHTML += "                                            <p class='des'>" + _fnToNull(vResult[i]["ACT_CUST_NM"]) + "</p>";
            vHTML += "                                        </li>";
            vHTML += "          <input type='hidden' name='BK_NO' value='" + _fnToNull(vResult[i]["BK_NO"]) + "'> ";
            vHTML += "          <input type='hidden' name='BK_SEQ' value='" + _fnToNull(vResult[i]["BK_SEQ"]) + "'> ";
            vHTML += "                                    </ul>";
        });

        $("#MO_Layer_Console_Header")[0].innerHTML = vHTML;

    } catch (err) {
        console.log("[Error - fnLayer_Console_Header]" + err.message);
    }

}

$(document).on("click", "#PC_Layer_Console_Header tr", function () {
    var vBkNo = $(this).find("input[name='BK_NO']").val();
    var vBkSeq = $(this).find("input[name='BK_SEQ']").val();


    fnLayer_Console_Detail(vBkNo, vBkSeq)
});

$(document).on("click", "#MO_Layer_Console_Header ul", function () {
    var vBkNo = $(this).find("input[name='BK_NO']").val();
    var vBkSeq = $(this).find("input[name='BK_SEQ']").val();


    fnLayer_Console_Detail(vBkNo, vBkSeq)
});

function fnLayer_Console_Detail(vBkNo, vBkSeq) {
    try {
        var objJsonData = new Object();

        objJsonData.BK_NO = _fnToNull(vBkNo);
        objJsonData.BK_SEQ = _fnToNull(vBkSeq);
        objJsonData.USR_TYPE = _fnToNull($("#Session_USR_TYPE").val());
        objJsonData.CUST_CD = _fnToNull($("#Session_CUST_CD").val());

        $.ajax({
            type: "POST",
            url: "/Popup/fnGetLayerConsoleDetail",
            async: false,
            dataType: "json",
            data: { "vJsonData": _fnMakeJson(objJsonData) },
            success: function (result) {
                fnMakeLayerConsole(result);//입고정보 조회
                fnMakeLayerConsoleDetail(result);//입고상세 조회
            }, error: function (xhr, status, error) {
                alert("담당자에게 문의 하세요.");
                console.log(error);
                vReturn = false;
            }
        });
    }
    catch (err) {
        console.log("[Error - fnLayer_Console_Detail]" + err.message);
    }
}

//콘솔 상세 조회
function fnMakeLayerConsole(vJsonData) {
    try {
        var vHTML = "";

        if (JSON.parse(vJsonData).Result[0]["trxCode"] == "Y") {
            var vResult = JSON.parse(vJsonData).Console;

            vHTML += "   <li class=\"item col-sm-6 col-12\"> ";
            vHTML += "   	<div class=\"text-box row\"> ";
            vHTML += "   		<span class=\"tit col-md-4 col-12\">입고일자</span> ";
            vHTML += "   		<span class=\"des col-md col-12\">" + String(_fnToNull(vResult[0]["GR_YMD"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3') + "</span> ";
            vHTML += "   	</div> ";
            vHTML += "   </li> ";
            vHTML += "   <li class=\"item col-sm-6 col-12\"> ";
            vHTML += "   	<div class=\"text-box row\"> ";
            vHTML += "   		<span class=\"tit col-md-4 col-12\">입고번호</span> ";
            vHTML += "   		<span class=\"des col-md col-12\">" + _fnToNull(vResult[0]["GR_NO"]) + "</span> ";
            vHTML += "   	</div> ";
            vHTML += "   </li> ";
            vHTML += "   <li class=\"item col-sm-6 col-12\"> ";
            vHTML += "   	<div class=\"text-box row\"> ";
            vHTML += "   		<span class=\"tit col-md-4 col-12\">실화주</span> ";
            vHTML += "   		<span class=\"des col-md col-12\">" + _fnToNull(vResult[0]["ACT_CUST_NM"]) + "</span> ";
            vHTML += "   	</div> ";
            vHTML += "   </li> ";
            vHTML += "   <li class=\"item col-sm-6 col-12\"> ";
            vHTML += "   	<div class=\"text-box row\"> ";
            vHTML += "   		<span class=\"tit col-md-4 col-12\">품명</span> ";
            vHTML += "   		<span class=\"des col-md col-12\">" + _fnToNull(vResult[0]["SKU_NM"]) + "</span> ";
            vHTML += "   	</div> ";
            vHTML += "   </li> ";
            vHTML += "   <li class=\"item col-sm-6 col-12\"> ";
            vHTML += "   	<div class=\"text-box row\"> ";
            vHTML += "   		<span class=\"tit col-md-4 col-12\">도착지</span> ";
            vHTML += "   		<span class=\"des col-md col-12\">" + _fnToNull(vResult[0]["POD_CD"]) + "<br/>" + _fnToNull(vResult[0]["POD_NM"]) + "</span> ";
            vHTML += "   	</div> ";
            vHTML += "   </li> ";
            vHTML += "   <li class=\"item col-sm-6 col-12\"> ";
            vHTML += "   	<div class=\"text-box row\"> ";
            vHTML += "   		<span class=\"tit col-md-4 col-12\">MARK</span> ";
            vHTML += "   		<span class=\"des col-md col-12\">" + _fnToNull(vResult[0]["MARK"]) + "</span> ";
            vHTML += "   	</div> ";
            vHTML += "   </li> ";
            vHTML += "   <li class=\"item col-sm-6 col-12\"> ";
            vHTML += "   	<div class=\"text-box row\"> ";
            vHTML += "   		<span class=\"tit col-md-4 col-12\">수량</span> ";
            vHTML += "   		<span class=\"des col-md col-12\">" + fnSetComma(_fnToNull(vResult[0]["ACT_QTY"])) + "</span> ";
            vHTML += "   	</div> ";
            vHTML += "   </li> ";
            vHTML += "   <li class=\"item col-sm-6 col-12\"> ";
            vHTML += "   	<div class=\"text-box row\"> ";
            vHTML += "   		<span class=\"tit col-md-4 col-12\">중량</span> ";
            vHTML += "   		<span class=\"des col-md col-12\">" + fnSetComma(_fnToNull(vResult[0]["ACT_GRS_WGT"])) + "</span> ";
            vHTML += "   	</div> ";
            vHTML += "   </li> ";
            vHTML += "   <li class=\"item col-sm-6 col-12\"> ";
            vHTML += "   	<div class=\"text-box row\"> ";
            vHTML += "   		<span class=\"tit col-md-4 col-12\">용적</span> ";
            vHTML += "   		<span class=\"des col-md col-12\">" + fnSetComma(_fnToNull(vResult[0]["ACT_MSRMT"])) + "</span> ";
            vHTML += "   	</div> ";
            vHTML += "   </li> ";

            $("#Layer_Console_Info")[0].innerHTML = vHTML;

            vHTML = "";


            vHTML += "                                            <li class='item col-sm-6 col-12'>";
            vHTML += "											   <div class='text-box row'>";
            vHTML += "                                                    <span class='tit col-md-4 col-12'>입고일자</span>";
            vHTML += "                                                    <span class='des col-md col-12'>" + String(_fnToNull(vResult[0]["GR_YMD"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3') + "</span>";
            vHTML += "                                                </div>";
            vHTML += "                                            </li>";
            vHTML += "                                            <li class='item col-sm-6 col-12'>";
            vHTML += "                                                <div class='text-box row'>";
            vHTML += "                                                    <span class='tit col-md-4 col-12'>입고번호</span>";
            vHTML += "                                                    <span class='des col-md col-12'>" + _fnToNull(vResult[0]["GR_NO"]) + "</span>";
            vHTML += "                                                </div>";
            vHTML += "                                            </li>";
            vHTML += "                                            <li class='item col-sm-6 col-12'>";
            vHTML += "                                                <div class='text-box row'>";
            vHTML += "                                                    <span class='tit col-md-4 col-12'>실화주</span>";
            vHTML += "                                                    <span class='des col-md col-12'>" + _fnToNull(vResult[0]["ACT_CUST_NM"]) + "</span>";
            vHTML += "                                                </div>";
            vHTML += "                                            </li>";
            vHTML += "                                            <li class='item col-sm-6 col-12'>";
            vHTML += "                                                <div class='text-box row'>";
            vHTML += "                                                    <span class='tit col-md-4 col-12'>품명</span>";
            vHTML += "                                                    <span class='des col-md col-12'>" + _fnToNull(vResult[0]["SKU_NM"]) + "</span>";
            vHTML += "                                                </div>";
            vHTML += "                                            </li>";
            vHTML += "                                            <li class='item col-sm-6 col-12'>";
            vHTML += "                                                <div class='text-box row'>";
            vHTML += "                                                    <span class='tit col-md-4 col-12'>도착지</span>";
            vHTML += "                                                    <span class='des col-md col-12'>" + _fnToNull(vResult[0]["POD_CD"]) + "<br/>" + _fnToNull(vResult[0]["POD_NM"]) + "</span>";
            vHTML += "                                                </div>";
            vHTML += "                                            </li>";
            vHTML += "                                            <li class='item col-sm-6 col-12'>";
            vHTML += "                                                <div class='text-box row'>";
            vHTML += "                                                    <span class='tit col-md-4 col-12'>MARK</span>";
            vHTML += "                                                    <span class='des col-md col-12'>" + _fnToNull(vResult[0]["MARK"]) + "</span>";
            vHTML += "                                                </div>";
            vHTML += "                                            </li>";
            vHTML += "                                            <li class='item col-sm-6 col-12'>";
            vHTML += "                                                <div class='text-box row'>";
            vHTML += "                                                    <span class='tit col-md-4 col-12'>수량</span>";
            vHTML += "                                                    <span class='des col-md col-12'>" + fnSetComma(_fnToNull(vResult[0]["ACT_QTY"])) + "</span>";
            vHTML += "                                                </div>";
            vHTML += "                                            </li>";
            vHTML += "                                            <li class='item col-sm-6 col-12'>";
            vHTML += "                                                <div class='text-box row'>";
            vHTML += "                                                    <span class='tit col-md-4 col-12'>중량</span>";
            vHTML += "                                                    <span class='des col-md col-12'>" + fnSetComma(_fnToNull(vResult[0]["ACT_GRS_WGT"])) + "</span>";
            vHTML += "                                                </div>";
            vHTML += "                                            </li>";
            vHTML += "                                            <li class='item col-sm-6 col-12'>";
            vHTML += "                                                <div class='text-box row'>";
            vHTML += "                                                    <span class='tit col-md-4 col-12'>용적</span>";
            vHTML += "                                                    <span class='des col-md col-12'>" + fnSetComma(_fnToNull(vResult[0]["ACT_MSRMT"])) + "</span>";
            vHTML += "                                                </div>";
            vHTML += "                                            </li>";

            $("#Mo_Layer_Console_Num")[0].innerHTML = vHTML;

        }
        else if (JSON.parse(vJsonData).Result[0]["trxCode"] == "N") {
            magnificPopup.close();
            alert("상세 데이터가 없습니다.");
        }
        else if (JSON.parse(vJsonData).Result[0]["trxCode"] == "N") {
            magnificPopup.close();
            alert("데이터를 가져 올 수 없습니다. \n관리자에게 문의하세요.");
        }
    }
    catch (err) {
        console.log("[Error - fnMakeLayerConsole]" + err.message);
    }
}

//콘솔 상세 조회 디테일
function fnMakeLayerConsoleDetail(vJsonData) {
    try {

        var vHTML = "";
        var vResult = JSON.parse(vJsonData).DeTail;

        if (_fnToNull(JSON.parse(vJsonData).DeTail) != "") {

            vHTML = "";

            $.each(vResult, function (i) {
                vHTML += "   <tr> ";
                vHTML += "   	<td class=\"txt\">" + _fnToNull(vResult[i]["GR_NO"]) + "</td> ";
                vHTML += "   	<td class=\"txt\">" + fnSetComma(_fnToNull(vResult[i]["SIZE_W"])) + "</td> ";
                vHTML += "   	<td class=\"txt\">" + fnSetComma(_fnToNull(vResult[i]["SIZE_D"])) + "</td> ";
                vHTML += "   	<td class=\"txt\">" + fnSetComma(_fnToNull(vResult[i]["SIZE_H"])) + "</td> ";
                vHTML += "   	<td class=\"txt\">" + fnSetComma(_fnToNull(vResult[i]["QTY"])) + "</td> ";
                vHTML += "   	<td class=\"txt\">" + fnSetComma(_fnToNull(vResult[i]["MSRMT"])) + "</td> ";
                vHTML += "   </tr> ";
            });

            $("#PC_Layer_Console_Detail")[0].innerHTML = vHTML;

            vHTML = "";

            $.each(vResult, function (i) {
                vHTML += "   <li class=\"info-txt info-txt--full\"> ";
                vHTML += "   	<p class=\"title font-main\">입고번호</p> ";
                vHTML += "   	<p class=\"des\">" + _fnToNull(vResult[i]["GR_NO"]) + "</p> ";
                vHTML += "   </li> ";
                vHTML += "   <li class=\"info-txt info-txt--full\"> ";
                vHTML += "   	<p class=\"title font-main\">가로</p> ";
                vHTML += "   	<p class=\"des\">" + fnSetComma(_fnToNull(vResult[i]["SIZE_W"])) + "</p> ";
                vHTML += "   </li> ";
                vHTML += "   <li class=\"info-txt info-txt--full\"> ";
                vHTML += "   	<p class=\"title font-main\">세로</p> ";
                vHTML += "   	<p class=\"des\">" + fnSetComma(_fnToNull(vResult[i]["SIZE_D"])) + "</p> ";
                vHTML += "   </li> ";
                vHTML += "   <li class=\"info-txt info-txt--full\"> ";
                vHTML += "   	<p class=\"title font-main\">높이</p> ";
                vHTML += "   	<p class=\"des\">" + fnSetComma(_fnToNull(vResult[i]["SIZE_H"])) + "</p> ";
                vHTML += "   </li> ";
                vHTML += "   <li class=\"info-txt info-txt--full\"> ";
                vHTML += "   	<p class=\"title font-main\">수량</p> ";
                vHTML += "   	<p class=\"des\">" + fnSetComma(_fnToNull(vResult[i]["QTY"])) + "</p> ";
                vHTML += "   </li> ";
                vHTML += "   <li class=\"info-txt info-txt--full border_bottom\"> ";
                vHTML += "   	<p class=\"title font-main\">용적</p> ";
                vHTML += "   	<p class=\"des\">" + fnSetComma(_fnToNull(vResult[i]["MSRMT"])) + "</p> ";
                vHTML += "   </li> ";
            });

            $("#Mo_Layer_Console_Detail")[0].innerHTML = vHTML;
        } else {
            vHTML = "";

            vHTML += "   <tr> ";
            vHTML += "   	<td colspan=\"6\" class=\"no-data\">데이터가 없습니다.</td> ";
            vHTML += "   </tr> ";

            $("#PC_Layer_Console_Detail")[0].innerHTML = vHTML;

            vHTML = "";

            vHTML += "   <li class=\"no_data col-12 py-6\"><span class=\"font-weight-medium\">데이터가 없습니다.</span></li> ";

            $("#Mo_Layer_Console_Detail")[0].innerHTML = vHTML;
        }
    }
    catch (err) {
        console.log("[Error - fnMakeLayerConsoleDetail]" + err.message);
    }
}
////invoice 출력 버튼 이벤트
//$(document).on("click", ".layer_invoice_btn", function () {

//    //인보이스로 데이터를 가져와서 iframe 해두기
//    _vInvNo = $(this).siblings("input[name='INV_NO']").val();
//    _vHblNo = $(this).siblings("input[name='HBL_NO']").val();
//    fnInvPrint($(this).siblings("input[name='HBL_NO']").val());

//});

//인보이스 확인 창 확인 시 수정요청사항 저장
$(document).on("click", "#Inv_List_Confirm_confirm", function () {
    fnSaveInvRequest();
    layerClose("#Inv_List_Confirm");
});

//확인 레이어 팝업 끄기
$(document).on("click", "#Inv_List_Confirm_cencel", function () {
    layerClose("#Inv_List_Confirm");
});


//Invoice 출력 함수
function fnInvPrint(vHBL) {
    try {

        var vResult;
        vResult = fnGetInvPrintData(vHBL); // 데이터 가져오기

        if (JSON.parse(vResult).Result[0]["trxCode"] == "Y") {

            vResult = JSON.parse(vResult).Invoice[0];
            var objJsonData = new Object();

            objJsonData.FILE_NM = vResult.FILE_NM;
            objJsonData.MNGT_NO = vResult.MNGT_NO;              //부킹 번호
            objJsonData.INS_USR = $("#Session_LOC_NM").val();   //부킹 번호
            objJsonData.DOMAIN = $("#Session_DOMAIN").val();    //접속 User
            objJsonData.SEQ = vResult.SEQ;

            $.ajax({
                type: "POST",
                url: "/File/DownloadElvis",
                async: true,
                data: { "vJsonData": _fnMakeJson(objJsonData) },
                success: function (result, status, xhr) {
                    //alert(result);
                    if (result != "E") {
                        var rtnTbl = JSON.parse(result);
                        rtnTbl = rtnTbl.Path;
                        var file_nm = rtnTbl[0].FILE_NAME;
                        if (_fnToNull(rtnTbl) != "") {
                            var agent = navigator.userAgent.toLowerCase();
                            if (file_nm.substring(file_nm.length, file_nm.length - 4) == ".pdf" || file_nm.substring(file_nm.length, file_nm.length - 4) == ".PDF") {
                                if (agent.indexOf('trident') > -1) {
                                    window.location = "/HP_File/DownloadFile?FILE_NM=" + file_nm + "&REPLACE_FILE_NM=" + rtnTbl[0].FILE_REAL_NAME;
                                } else {
                                    //체크 비엘 제외하고 다른쪽은 아이프레임 변경해서 하게
                                    $("#layer_Request_area_INV").val(""); //다시 수정요청사항을 켰을 때 Textarea 초기화
                                    $("#Only_Iframe_INV").attr("src", "/web/viewer.html?file=/Content/TempFiles/" + file_nm);
                                    layerPopup2('#Only_Iframe_pop_INV');
                                    fnGetInvRequest();
                                }
                            } else {
                                window.location = "/HP_File/DownloadFile?FILE_NM=" + file_nm + "&REPLACE_FILE_NM=" + rtnTbl[0].FILE_REAL_NAME;
                            }

                        }
                    } else {
                        _fnAlertMsg("다운받을 수 없는 출력물입니다");
                    }
                },
                error: function (xhr, status, error) {
                    alert("[Error]관리자에게 문의 해 주세요. " + status);
                    return;
                }
                , beforeSend: function () {
                    $("#ProgressBar_Loading").show(); //프로그래스 바
                },
                complete: function () {
                    $("#ProgressBar_Loading").hide(); //프로그래스 바
                }
            });
        }
        else if (JSON.parse(vResult).Result[0]["trxCode"] == "N") {
            _fnAlertMsg("다운받을 수 없는 출력물입니다");
            console.log("[Fail]fnPrint" + JSON.parse(vResult).Result[0]["trxMsg"]);
        }
        else if (JSON.parse(vResult).Result[0]["trxCode"] == "E") {
            _fnAlertMsg("다운받을 수 없는 출력물입니다");
            console.log("[Error]fnPrint" + JSON.parse(vResult).Result[0]["trxMsg"]);
        }

    } catch (err) {
        console.log("[Error]fnPrint" + err);
    }
}

//Invoice 출력 함수 데이터 가져오기
function fnGetInvPrintData(vHBL) {

    try {
        var vResult;

        var objJsonData = new Object();
        objJsonData.HBL_NO = vHBL;
        objJsonData.DOC_TYPE = "INV";

        $.ajax({
            type: "POST",
            dataType: "json",
            url: "/MyData/fnGetInvPrint",
            async: false,
            data: { "vJsonData": _fnMakeJson(objJsonData) },
            success: function (result, status, xhr) {
                vResult = result;
            },
            error: function (xhr, status, error) {
                _fnAlertMsg("[Error]관리자에게 문의 해 주세요. " + status);
                return;
            }
        });

        return vResult;
    }
    catch (err) {
        console.log("[Error]fnGetInvPrintData" + err);
    }
}

$(document).on("click", "#layer_Request_btn", function () {
    if ($("#layer_Request_area").val().length > 0) {
        fnBLRequestConfirm("수정요청 하시겠습니까?");
    }
    else {
        _fnLayerAlertMsg("수정요청 사항 내용을 입력 해 주세요.");
    }
});

$(document).on("click", "#layer_Request_btn_INV", function () {
    if ($("#layer_Request_area_INV").val().length > 0) {
        fnInvRequestConfirm("수정요청 하시겠습니까?");
    }
    else {
        _fnLayerAlertMsg("수정요청 사항 내용을 입력 해 주세요.");
    }
});
////////////////////////function///////////////////////
function fnInitData() {
    try {
        $(".delete-btn").hide();

        //스케줄 검색 화면 초기화
        $("#input_POL").text("출발");
        $("#input_POLCD").val("");
        $("#input_POL").val("");
        $("#input_POLCD").val("");
        $("#input_POD").text("도착");
        $("#input_PODCD").val("");
        $("#input_HouseBL").val("");
        $("#Paging_Area").hide();

        //스케줄 화면 초기화
        $("#MyBoard_orderby th button").removeClass("on"); //Order By 초기화

        var vHTML = "";
        //PC
        vHTML += "   <tr> ";
        vHTML += "   	<td class=\"no_data\" colspan=\"9\"></td> ";
        vHTML += "   </tr> ";

        $("#MyBoard_Result_AREA_PC")[0].innerHTML = vHTML;

        vHTML = "";

        //MO
        vHTML += "   <ul class=\"info-box py-2 px-1\"> ";
        vHTML += "   	<li class=\"no_data col-12 py-6\"></li> ";
        vHTML += "   </ul> ";

        $("#MyBoard_Result_AREA_MO")[0].innerHTML = vHTML;

        //부킹 전역 변수 초기화
        _isSearch = false;
    }
    catch (err) {
        console.log("[Error - fnInitData()]" + err.message);
    }
}

//즐겨찾기 메뉴 띄우기
function fnShowQuickMenu() {
    try {
        if ($("#Select_Bound").find("option:selected").val() == "E") {
            $(".sch-pop--pod--export").show();
        }
        else if ($("#Select_Bound").find("option:selected").val() == "I") {
            $(".sch-pop--pod--import").show();
        }
    }
    catch (err) {
        console.log("[Error - fnShowQuickMenu()]" + err.message);
    }
}

//하나만 검색
function fnMyBoardSingleSearch(vMngt_No) {
    try {
        var objJsonData = new Object();

        objJsonData.MNGT_NO = _fnToNull(vMngt_No);
        objJsonData.ID = "";
        objJsonData.ORDER = "";
        objJsonData.CUST_CD = $("#Session_CUST_CD").val();
        objJsonData.USR_TYPE = _fnToNull($("#Session_USR_TYPE").val());
        objJsonData.OFFICE_CD = $("#Session_OFFICE_CD").val();
        objJsonData.PAGE = 1;
        objJsonData.MBL_DOC_TYPE = _DocType_MBL;
        objJsonData.HBL_DOC_TYPE = _DocType_HBL;

        $.ajax({
            type: "POST",
            url: "/MyData/fnGetBoradData",
            async: true,
            dataType: "json",
            data: { "vJsonData": _fnMakeJson(objJsonData) },
            success: function (result) {
                fnMakeMyBoardList(result);
                fnSetSearchData(result);
                $("#Paging_Area").hide();
            }, error: function (xhr, status, error) {
                $("#ProgressBar_Loading").hide(); //프로그래스 바
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
    catch (err) {
        console.log("[Error - fnMyBoardSingleSearch]" + err.message);
    }
}

//MyBoard 검색
function fnMyBoardSearch() {
    try {

        var objJsonData = new Object();

        objJsonData.MNGT_NO = "";
        objJsonData.OFFICE_CD = $("#Session_OFFICE_CD").val();
        objJsonData.CUST_CD = $("#Session_CUST_CD").val();
        objJsonData.USR_TYPE = _fnToNull($("#Session_USR_TYPE").val());

        objJsonData.REQ_SVC = "SEA";
        objJsonData.CNTR_TYPE = $("#select_CntrType option:selected").val();
        objJsonData.EX_IM_TYPE = $("#Select_Bound option:selected").val();
        objJsonData.ETD_ETA = $("#Select_ETD_ETA option:selected").val();
        objJsonData.STRT_YMD = $("#input_ETD").val().substring(0, 10).replace(/-/gi, "");
        objJsonData.END_YMD = $("#input_ETA").val().substring(0, 10).replace(/-/gi, "");

        if ($("#Select_Bound").find("option:selected").val() == "E") {
            if ($("#input_POL").text() == "출발") {
                objJsonData.POL = "";
            } else {
                objJsonData.POL = $("#input_POL").text();
            }
            objJsonData.POL_CD = $("#input_POLCD").val();

            if ($("#input_POD").text() == "도착") {
                objJsonData.POD = "";
            } else {
                objJsonData.POD = $("#input_POD").text();
            }
            objJsonData.POD_CD = $("#input_PODCD").val();
        } else if ($("#Select_Bound").find("option:selected").val() == "I") {

            if ($("#input_POL").text() == "출발") {
                objJsonData.POL = "";
            } else {
                objJsonData.POL = $("#input_POL").text();
            }
            objJsonData.POL_CD = $("#input_POLCD").val();

            if ($("#input_POD").text() == "도착") {
                objJsonData.POD = "";
            } else {
                objJsonData.POD = $("#input_POD").text();
            }
            objJsonData.POD_CD = $("#input_PODCD").val();
        }
       

        objJsonData.HBL_NO = _fnToNull($("#input_HouseBL").val().toUpperCase().trim());

        objJsonData.MBL_DOC_TYPE = _DocType_MBL;
        objJsonData.HBL_DOC_TYPE = _DocType_HBL;
        objJsonData.PARTNER_DOC_TYPE = _DocType_Part;
        objJsonData.USR_TYPE = $("#Session_USR_TYPE").val();
        objJsonData.PAGE = _vPage;

        if (_fnToNull(_OrderBy) != "" || _fnToNull(_Sort) != "") {
            objJsonData.ID = _OrderBy;
            objJsonData.ORDER = _Sort;
        } else {
            objJsonData.ID = "";
            objJsonData.ORDER = "";
        }

        $.ajax({
            type: "POST",
            url: "/MyData/fnGetBoradData",
            async: true,
            dataType: "json",
            data: { "vJsonData": _fnMakeJson(objJsonData) },
            success: function (result) {
                fnMakeMyBoardList(result);
                if (result != null) {
                    if (JSON.parse(result).Result[0]["trxCode"] == "Y") {
                        fnMyBoardPaging(JSON.parse(result).BOARD[0]["TOTCNT"], 10, 10, objJsonData.PAGE);
                    }
                }
            }, error: function (xhr, status, error) {
                $("#ProgressBar_Loading").hide(); //프로그래스 바
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

    } catch (err) {
        console.log("[Error - fnMyBoardSearch]" + err.message);
    }
}

//MyBoard 밸리데이션
function fnMyBoardSearch_Validation() {
    try {

        if (_fnToNull($("#input_ETD").val().replace(/-/gi, "")) == "") {
            _fnAlertMsg("ETD를 입력 해 주세요.");
            return false;
        }

        if (_fnToNull($("#input_ETA").val().replace(/-/gi, "")) == "") {
            _fnAlertMsg("ETA를 입력 해 주세요.");
            return false;
        }

    }
    catch (err) {
        console.log("[Error - fnMyBoardSearch_Validation]" + err.message);
        return false;
    }
}
//이메일 검색 후 검색 조건 데이터 채우기
function fnSetSearchData(vJsonData) {
    try {
        if (_fnToNull(JSON.parse(vJsonData).Result[0]["trxCode"]) == "Y") {
            //Service 세팅
            if (_fnToNull(JSON.parse(vJsonData).BOARD[0]["CNTR_TYPE"]) == "FCL") {
                $("#select_CntrType").val("F").prop('checked', true);
            }
            else if (_fnToNull(JSON.parse(vJsonData).BOARD[0]["CNTR_TYPE"]) == "LCL" || _fnToNull(JSON.parse(vJsonData).BOARD[0]["CNTR_TYPE"]) == "CONSOL") {
                $("#select_CntrType").val("L").prop('checked', true);
            }

            if (_fnToNull(JSON.parse(vJsonData).BOARD[0]["EX_IM_TYPE"]) == "E") {
                $("#Select_Bound").val("E");
                $(".pop__export").show();
                $(".pop__import").hide();
                $('.city-text-dpt').text('출발');
                $('.city-text-arrive').text('도착');
                $("#input_POL").text(_fnToNull(JSON.parse(vJsonData).BOARD[0]["POL_NM"]));
                $("#input_POLCD").val(_fnToNull(JSON.parse(vJsonData).BOARD[0]["POL_CD"]));
                $("button[name='input_POD']").show();
                $("#input_POD").val(_fnToNull(JSON.parse(vJsonData).BOARD[0]["POD_NM"]));
                $("#input_PODCD").val(_fnToNull(JSON.parse(vJsonData).BOARD[0]["POL_CD"]));
            }
            else if (_fnToNull(JSON.parse(vJsonData).BOARD[0]["EX_IM_TYPE"]) == "I") {
                $("#Select_Bound").val("I");
                $(".pop__export").hide();
                $(".pop__import").show();
                $('.city-text-arrive').text('출발');
                $('.city-text-dpt').text('도착');
                $("#input_POL").val(_fnToNull(JSON.parse(vJsonData).BOARD[0]["POL_NM"]));
                $("#input_POLCD").val(_fnToNull(JSON.parse(vJsonData).BOARD[0]["POL_CD"]));
                $("button[name='input_POL']").show();
                $("#input_POD").text(_fnToNull(JSON.parse(vJsonData).BOARD[0]["POD_NM"]));
                $("#input_PODCD").val(_fnToNull(JSON.parse(vJsonData).BOARD[0]["POL_CD"]));
            }

            $("#input_ETD").val(_fnFormatDate(_fnToNull(JSON.parse(vJsonData).BOARD[0]["ETD"].replace(/\./gi, "")))); //ETD
            if (_fnToNull(JSON.parse(vJsonData).BOARD[0]["ETA"]) != "") {
                $("#input_ETA").val(_fnFormatDate(_fnToNull(JSON.parse(vJsonData).BOARD[0]["ETA"].replace(/\./gi, "")))); //ETA
            }

            $("#input_HouseBL").val(_fnToNull(JSON.parse(vJsonData).BOARD[0]["HBL_NO"]));
            $("#input_HouseBL").siblings(".delete-btn").show();
        }
    }
    catch (err) {
        console.log("[Error - fnSetSearchData]" + err.message);
    }
}

//totalData = 총 데이터 count
//dataPerPage = 한페이지에 나타낼 데이터 수
//pageCount = 한화면에 나타낼 페이지 수
//currentPage = 선택한 페이지 
//공지사항 페이징
function fnMyBoardPaging(totalData, dataPerPage, pageCount, currentPage) {
    var totalPage = Math.ceil(totalData / dataPerPage);    // 총 페이지 수
    var pageGroup = Math.ceil(currentPage / pageCount);    // 페이지 그룹
    if (pageCount > totalPage) pageCount = totalPage;
    var last = pageGroup * pageCount;    // 화면에 보여질 마지막 페이지 번호
    if (last > totalPage) last = totalPage;
    var first = last - (pageCount - 1);    // 화면에 보여질 첫번째 페이지 번호
    var next = last + 1;
    var prev = first - 1;

    $("#Paging_Area").empty();

    var prevPage;
    var nextPage;
    if (currentPage - 1 < 1) { prevPage = 1; } else { prevPage = currentPage - 1; }
    //if (last < totalPage) { nextPage = currentPage + 1; } else { nextPage = last; }
    if (currentPage == last) {
        if (last == totalPage) {
            nextPage = last
        } else {
            nextPage = currentPage + 1;
        }
    } else {
        nextPage = currentPage + 1;
    }

    var vHTML = "";

    vHTML += "   <ul> ";
    vHTML += "      <li> ";
    vHTML += "      	<a href=\"javascript:;\" class=\"prev-first\" onclick=\"fnMyBoardGoPage(1)\" ><span>맨앞으로</span></a> ";
    vHTML += "      </li> ";
    vHTML += "      <li> ";
    vHTML += "      	<a href=\"javascript:;\" class=\"prev\" onclick=\"fnMyBoardGoPage(" + prevPage + ")\"><span>이전으로</span></a> ";
    vHTML += "      </li> ";

    for (var i = first; i <= last; i++) {
        if (i == currentPage) {
            vHTML += "   <li> ";
            vHTML += "   	<a href=\"javascript:;\" class=\"active\">" + i + "<span></span></a> ";
            vHTML += "   </li> ";
        } else {
            vHTML += "   <li> ";
            vHTML += "   	<a href=\"javascript:;\" onclick=\"fnMyBoardGoPage(" + i + ")\">" + i + "<span></span></a> ";
            vHTML += "   </li> ";
        }
    }

    vHTML += "      <li> ";
    vHTML += "      	<a href=\"javascript:;\" onclick=\"fnMyBoardGoPage(" + nextPage + ")\" class=\"next\"><span>다음으로</span></a> ";
    vHTML += "      </li> ";
    vHTML += "      <li> ";
    vHTML += "      	<a href=\"javascript:;\" onclick=\"fnMyBoardGoPage(" + totalPage + ")\" class=\"next-last\"><span>맨뒤로</span></a> ";
    vHTML += "      </li> ";
    vHTML += "   </ul> ";

    $("#Paging_Area").append(vHTML);    // 페이지 목록 생성
}

function fnMyBoardGoPage(vPage) {
    _vPage = vPage;
    fnMyBoardSearch();
}

//Tracking 가능한지 체크 로직
function isTrackingAvailable(vHBL_NO) {
    try {

        var objJsonData = new Object();
        var vResult = false;

        objJsonData.HBL_NO = vHBL_NO;

        $.ajax({
            type: "POST",
            url: "/MyData/isTrackingAvailable",
            async: false,
            dataType: "json",
            data: { "vJsonData": _fnMakeJson(objJsonData) },
            success: function (result) {

                if (JSON.parse(result).Result[0]["trxCode"] == "Y") {

                    if (JSON.parse(result).Available[0]["CHKBL_YN"] == "Y") {
                        vResult = true;
                    } else {
                        vResult = false;
                        _fnAlertMsg("B/L 제출을 먼저 해주시기 바랍니다.");
                    }
                } else {
                    _fnAlertMsg2("Tracking 정보가 없습니다.");
                    vResult = false;
                }
            }, error: function (xhr, status, error) {
                alert("담당자에게 문의 하세요.");
                console.log(error);
            }
        });

        return vResult;

    } catch (err) {
        console.log("[Error - isTrackingAvailable]");
    }
}

//BL 출력 함수
function fnPrint(vHBL, vDocType) {

    try {

        var vResult;
        vResult = fnGetPrintData(vHBL, vDocType); //BL 데이터 가져오기

        if (JSON.parse(vResult).Result[0]["trxCode"] == "Y") {

            vResult = JSON.parse(vResult).Print[0];

            var objJsonData = new Object();

            objJsonData.FILE_NM = vResult.FILE_NM;
            objJsonData.MNGT_NO = vResult.MNGT_NO;              //부킹 번호
            objJsonData.INS_USR = $("#Session_LOC_NM").val();   //부킹 번호
            objJsonData.DOMAIN = $("#Session_DOMAIN").val();    //접속 User
            objJsonData.SEQ = vResult.SEQ;

            $.ajax({
                type: "POST",
                url: "/File/DownloadElvis",
                async: true,
                data: { "vJsonData": _fnMakeJson(objJsonData) },
                success: function (result, status, xhr) {
                    //alert(result);
                    if (result != "E") {
                        var rtnTbl = JSON.parse(result);
                        rtnTbl = rtnTbl.Path;
                        var file_nm = rtnTbl[0].FILE_NAME;
                        if (_fnToNull(rtnTbl) != "") {
                            var agent = navigator.userAgent.toLowerCase();
                            if (file_nm.substring(file_nm.length, file_nm.length - 4) == ".pdf" || file_nm.substring(file_nm.length, file_nm.length - 4) == ".PDF") {
                                if (agent.indexOf('trident') > -1) {
                                    window.location = "/HP_File/DownloadFile?FILE_NM=" + file_nm + "&REPLACE_FILE_NM=" + rtnTbl[0].FILE_REAL_NAME;
                                } else {
                                    //체크 비엘 제외하고 다른쪽은 아이프레임 변경해서 하게
                                    if (vDocType == "CHBL") {
                                        $("#layer_Request_area").val(""); //다시 수정요청사항을 켰을 때 Textarea 초기화
                                        $("#Only_Iframe").attr("src", "/web/viewer.html?file=/Content/TempFiles/" + file_nm);
                                        layerPopup2('#Only_Iframe_pop');
                                        fnGetBLRequest();
                                    }
                                    else {
                                        $("#Only_Iframe_AN").attr("src", "/web/viewer.html?file=/Content/TempFiles/" + file_nm);
                                        layerPopup2('#Only_Iframe_pop_AN');
                                    }
                                }
                            } else {
                                window.location = "/HP_File/DownloadFile?FILE_NM=" + file_nm + "&REPLACE_FILE_NM=" + rtnTbl[0].FILE_REAL_NAME;
                            }

                        }
                    } else {
                        _fnAlertMsg("다운받을 수 없는 출력물입니다");
                    }
                },
                error: function (xhr, status, error) {
                    alert("[Error]관리자에게 문의 해 주세요. " + status);
                    return;
                }
                , beforeSend: function () {
                    $("#ProgressBar_Loading").show(); //프로그래스 바
                },
                complete: function () {
                    $("#ProgressBar_Loading").hide(); //프로그래스 바
                }
            });
        }
        else if (JSON.parse(vResult).Result[0]["trxCode"] == "N") {
            _fnAlertMsg("다운받을 수 없는 출력물입니다");
            console.log("[Fail]fnPrint" + JSON.parse(vResult).Result[0]["trxMsg"]);
        }
        else if (JSON.parse(vResult).Result[0]["trxCode"] == "E") {
            _fnAlertMsg("다운받을 수 없는 출력물입니다");
            console.log("[Error]fnPrint" + JSON.parse(vResult).Result[0]["trxMsg"]);
        }

    } catch (err) {
        console.log("[Error]fnPrint" + err);
    }
}

//BL 출력 함수 데이터 가져오기
function fnGetPrintData(vHBL, vDocType) {

    try {
        var vResult;

        var objJsonData = new Object();
        objJsonData.HBL_NO = vHBL;
        objJsonData.DOC_TYPE = vDocType;

        $.ajax({
            type: "POST",
            dataType: "json",
            url: "/MyData/fnGetPrintData",
            async: false,
            data: { "vJsonData": _fnMakeJson(objJsonData) },
            success: function (result, status, xhr) {
                vResult = result;
            },
            error: function (xhr, status, error) {
                alert("[Error]관리자에게 문의 해 주세요. " + status);
                return;
            }
            , beforeSend: function () {
                $("#ProgressBar_Loading").show(); //프로그래스 바
            },
            complete: function () {
                $("#ProgressBar_Loading").hide(); //프로그래스 바
            }
        });

        return vResult;
    }
    catch (err) {
        console.log("[Error]fnGetBLPrintData" + err);
    }
}
//달력 클릭 이벤트
$(document).on("click", "#input_ETD_Icon", function () {
    $("#input_ETD").focus();
});

//달력 클릭 이벤트
$(document).on("click", "#input_ETA_Icon", function () {
    $("#input_ETA").focus();
});

//수정요청사항 레이어 팝업 켜기
function fnBLRequestConfirm(msg) {
    $("#BL_List_Confirm .inner").html(msg);
    layerPopup3('#BL_List_Confirm');
    $("#BL_List_Confirm_confirm").focus();
}

//인보이스수정요청사항 레이어 팝업 켜기
function fnInvRequestConfirm(msg) {
    $("#Inv_List_Confirm .inner").html(msg);
    layerPopup2('#Inv_List_Confirm');
    $("#Inv_List_Confirm_confirm").focus();
}
//수정요청 사항 데이터 가져오기
function fnGetBLRequest() {
    try {
        var vResult;

        var objJsonData = new Object();
        objJsonData.HBL_NO = vHBL_NO;

        $.ajax({
            type: "POST",
            dataType: "json",
            url: "/MyData/fnSearchBLRequest",
            async: false,
            data: { "vJsonData": _fnMakeJson(objJsonData) },
            success: function (result, status, xhr) {
                fnMakeBLRequest(result);
            },
            error: function (xhr, status, error) {
                _fnAlertMsg("[Error]관리자에게 문의 해 주세요. " + status);
                return;
            }
        });
    }
    catch (err) {
        console.log(err);
    }
}


//확인 창 확인 시 수정요청사항 저장
$(document).on("click", "#BL_List_Confirm_confirm", function () {
    fnSaveBLRequest();
    layerClose3("#BL_List_Confirm");
});

//확인 레이어 팝업 끄기
$(document).on("click", "#BL_List_Confirm_cencel", function () {
    layerClose3("#BL_List_Confirm");
});
//수정확인 레이어 팝업 끄기
$(document).on("click", "#Check_Modify", function () {
    layerClose("#all_alert");
});
/////////////////function MakeList/////////////////////
function fnMakeMyBoardList(vJsonData) {
    try {

        var vHTML = "";

        if (JSON.parse(vJsonData).Result[0]["trxCode"] == "Y") {
            _isSearch = true;
            var vResult = JSON.parse(vJsonData).BOARD;


            //데이터 반복문 - //PC
            $.each(vResult, function (i) {
                vHTML += "   <tr> ";
                vHTML += "   	<td> ";
                vHTML += "   		<div> ";
                if (_fnToNull(vResult[i]["EX_IM_TYPE"]) == "E") {
                    vHTML += "   			<img class=\"td-icon td-icon--1\" src=\"/Images/Masstige/e-service/export-icon.png\" alt=\"export\" /> ";
                }
                else if (_fnToNull(vResult[i]["EX_IM_TYPE"]) == "I") {
                    vHTML += "   			<img class=\"td-icon td-icon--2\" src=\"/Images/Masstige/e-service/import-icon.png\" alt=\"import\" /> ";
                }
                vHTML += "   		</div> ";
                vHTML += "   	</td> ";

                vHTML += "   	<td class=\"txt font-sub pr-4 font-weight-medium\">";
                vHTML += _fnToNull(vResult[i]["HBL_NO"]);
                vHTML += "   	</td> ";
                vHTML += "   	<td class=\"txt\"> ";
                vHTML += String(_fnToNull(vResult[i]["ETD"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3') + " (" + (_fnGetWhatDay(_fnToNull(vResult[i]["ETD"]).replace(/\./gi, ""))) + ")<br /> ";
                vHTML += _fnToNull(vResult[i]["POL_NM"]);
                vHTML += "   	</td> ";
                vHTML += "   	<td class=\"txt\"> ";
                vHTML += String(_fnToNull(vResult[i]["ETA"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3') + " (" + (_fnGetWhatDay(_fnToNull(vResult[i]["ETA"]).replace(/\./gi, ""))) + ")<br /> ";
                vHTML += _fnToNull(vResult[i]["POD_NM"]) + "";
                vHTML += "   	</td> ";

                vHTML += "   	<td class=\"txt doc\"> ";
                if (_fnToNull(vResult[i].FILE_CNT) == "0") {
                    vHTML += "   		<a href='javascript:void(0)'> ";
                    vHTML += "   			<span class='count'>0</span> ";
                    vHTML += "   		</a> ";
                } else {
                    vHTML += "   		<a href='javascript:void(0)' name='btn_FileList'> ";
                    vHTML += "              <input type='hidden' name='File_HBL_NO' value='" + _fnToNull(vResult[i]["HBL_NO"]) + "'> ";
                    vHTML += "              <input type='hidden' name='File_BKG_NO' value='" + _fnToNull(vResult[i]["BKG_NO"]) + "'> ";
                    vHTML += "              <input type='hidden' name='File_INV_NO' value='" + _fnToNull(vResult[i]["INV_NO"]) + "'> ";
                    vHTML += "              <input type='hidden' name='File_SEQ' value='" + i + "'> ";
                    vHTML += "   			<span class='count'>" + _fnToNull(vResult[i].FILE_CNT) + "</span> ";
                    vHTML += "   		</a> ";
                }
                vHTML += "   	</td> ";

                //부킹버튼 쪽
                var v = $("#Select_Bound").val();
                if (v == 'E') {
                    if (_fnToNull(vResult[i]["STATUS"]) == "Q") {
                        vHTML += "   	<td class='txt'> ";
                        vHTML += "   		<span class='success-btn btn1 icn_state1'></span> ";
                        vHTML += "   	</td> ";
                        vHTML += "   	<td class='txt'> ";
                    } else if (_fnToNull(vResult[i]["CFS_BKG_NO"]) == "" || _fnToNull(vResult[i]["STATUS"]) == "Y") {
                        if (_fnToNull(vResult[i]["CFS_BKG_CHK"]) == "Y") {
                            vHTML += "   	<td class='txt'> ";
                            vHTML += "   		<span class='success-btn btn1 icn_state2' name='layer_CFSPrintData'></span> ";
                            vHTML += "          <input type='hidden' name='CFS_BKG_NO' value='" + _fnToNull(vResult[i]["CFS_BKG_NO"]) + "'> ";
                            vHTML += "          <input type='hidden' name='HBL_NO' value='" + _fnToNull(vResult[i]["HBL_NO"]) + "'> ";
                            vHTML += "   	</td> ";
                            vHTML += "   	<td class='txt'> ";
                        } else if (_fnToNull(vResult[i]["CFS_BKG_NO"]) == "") {
                            vHTML += "   	<td class='txt'> ";
                            vHTML += "   		<span class='success-btn btn1 icn_state1'></span> ";
                            vHTML += "   	</td> ";
                            vHTML += "   	<td class='txt'> ";
                        }
                        else {
                            vHTML += "   	<td class='txt'> ";
                            vHTML += "   		<span class='success-btn btn1 icn_state3'></span> ";
                            vHTML += "   	</td> ";
                            vHTML += "   	<td class='txt'> ";
                        }
                    }
                    else {
                        vHTML += "   	<td class='txt'> ";
                        vHTML += "   		<span class='success-btn btn1 icn_state1'></span> ";
                        vHTML += "   	</td> ";
                        vHTML += "   	<td class='txt'> ";
                    }
                }
                else {
                    vHTML += "   	<td class='txt' style='display:none'> ";
                    vHTML += "   		<span class='success-btn btn1 icn_state2'></span> ";
                    vHTML += "   	</td> ";
                    vHTML += "   	<td class='txt'> ";
                }

                if (_fnToNull(vResult[i]["IS_CHKBL_YN"]) == "Y") {
                    vHTML += "   		<span class=\"success-btn btn2 icn_state2\" name=\"layer_BLPrintData\"></span> ";
                    vHTML += "          <input type=\"hidden\" name=\"HBL_NO\" value=\"" + _fnToNull(vResult[i]["HBL_NO"]) + "\"> ";

                    if (_fnToNull(vResult[i]["EX_IM_TYPE"]) == "E") {
                        vHTML += "          <input type=\"hidden\" name=\"DOC_TYPE\" value=\"CHBL\"> ";
                    }
                    else if (_fnToNull(vResult[i]["EX_IM_TYPE"]) == "I") {
                        vHTML += "          <input type=\"hidden\" name=\"DOC_TYPE\" value=\"AN\"> ";
                    }
                }
                else if (_fnToNull(vResult[i]["IS_CHKBL_YN"]) == "N") {
                    vHTML += "   		<span class=\"success-btn btn2 icn_state1\"></span> ";
                }

                vHTML += "   	</td> ";
                if (_fnToNull($("#Session_USR_TYPE").val()) != "P") {
                    vHTML += "   	<td class=\"txt\"> ";
                    if (_fnToNull(vResult[i].INV_NO) != "") {
                        if (_fnToNull(vResult[i].INV_YN) == "Y") {
                            vHTML += "   		<span class=\"success-btn btn3 icn_state2\" name=\"layer_InvPrintData\"></span> ";
                            vHTML += "          <input type=\"hidden\" name=\"HBL_NO\" value=\"" + _fnToNull(vResult[i]["HBL_NO"]) + "\"> ";
                            vHTML += "          <input type=\"hidden\" name=\"INV_NO\" value=\"" + _fnToNull(vResult[i]["INV_NO"]) + "\"> ";
                            vHTML += "          <input type=\"hidden\" name=\"DOC_TYPE\" value=\"INV\"> ";
                        } else {
                            vHTML += "   		<span class=\"success-btn btn3 icn_state3\"></span> ";
                        }
                    } else {
                        vHTML += "   		<span class=\"success-btn btn3 icn_state1\"></span> ";
                    }
                    vHTML += "   	</td> ";
                } else {
                    $("#th_iv").hide();
                }
                //화물추적
                vHTML += "   	<td class=\"txt\"> ";
                vHTML += "   		<a href=\"javascript:void(0)\" class=\"bk-td\" name=\"btn_Tracking\">";
                vHTML += "              Tracking";
                vHTML += "              <input type=\"hidden\" name=\"Tracking_HBL_NO\" value=\"" + _fnToNull(vResult[i]["HBL_NO"]) + "\"> ";
                vHTML += "          </a > ";
                vHTML += "   	</td> ";
                vHTML += "   </tr> ";
            });

            $("#MyBoard_Result_AREA_PC")[0].innerHTML = vHTML;

            vHTML = "";

            //데이터 반복문 - MO
            $.each(vResult, function (i) {

                vHTML += "   <ul class=\"info-box py-2 px-1\"> ";
                vHTML += "   	<li class=\"info-txt info-txt--full\"> ";
                if (_fnToNull(vResult[i]["EX_IM_TYPE"]) == "E") {
                    vHTML += "   		<p class=\"title tit-type--1\">Export</p> ";
                    vHTML += "   		<div class=\"logo-img\"> ";
                    vHTML += "   			<img class=\"td-icon td-icon--1\" src=\"/Images/Masstige/e-service/export-icon.png\" alt=\"export\" /> ";
                    vHTML += "   		</div> ";
                }
                else if (_fnToNull(vResult[i]["EX_IM_TYPE"]) == "I") {
                    vHTML += "   		<p class=\"title tit-type--2\">Import</p> ";
                    vHTML += "   		<div class=\"logo-img\"> ";
                    vHTML += "   			<img class=\"td-icon td-icon--2\" src=\"/Images/Masstige/e-service/import-icon.png\" alt=\"import\" /> ";
                    vHTML += "   		</div> ";
                }
                vHTML += "   	</li> ";
                vHTML += "   	<li class=\"info-txt info-txt--full\"> ";
                vHTML += "   		<p class=\"title\">House B/L</p> ";
                vHTML += "   		<p class=\"des des--bl des\"> ";
                vHTML += _fnToNull(vResult[i]["HBL_NO"]);
                vHTML += "   		</p> ";
                vHTML += "   	</li> ";
                vHTML += "   	<li class=\"info-txt info-txt--full\"> ";
                vHTML += "   		<p class=\"title\">Departure</p> ";
                vHTML += "   		<p class=\"text-gray-6 common-text--14 des\">		 ";
                vHTML += String(_fnToNull(vResult[i]["ETD"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3') + " (" + (_fnGetWhatDay(_fnToNull(vResult[i]["ETD"]).replace(/\./gi, ""))) + ")<br /> ";
                vHTML += _fnToNull(vResult[i]["POL_NM"]);
                vHTML += "   		</p> ";
                vHTML += "   	</li> ";
                vHTML += "   	<li class=\"info-txt info-txt--full\"> ";
                vHTML += "   		<p class=\"title\">Arrival</p> ";
                vHTML += "   		<p class=\"text-gray-6 common-text--14 des\"> ";
                vHTML += String(_fnToNull(vResult[i]["ETA"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3') + " (" + (_fnGetWhatDay(_fnToNull(vResult[i]["ETA"]).replace(/\./gi, ""))) + ")<br /> ";
                vHTML += _fnToNull(vResult[i]["POD_NM"]) + "";
                vHTML += "   		</p> ";
                vHTML += "   	</li> ";
                vHTML += "   	<li class=\"info-txt doc info-txt--full\"> ";
                vHTML += "   		<p class=\"title\">문서</p> ";
                vHTML += "   		<p class=\"des\"> ";
                if (_fnToNull(vResult[i].FILE_CNT) == "0") {
                    vHTML += "   	<a href=\"javascript:void(0)\"> ";
                    vHTML += "   		<span class=\"count\">0</span> ";
                    vHTML += "   	</a> ";
                } else {
                    vHTML += "   		<a href=\"javascript:void(0)\" name=\"btn_FileList\"> ";
                    vHTML += "              <input type=\"hidden\" name=\"File_HBL_NO\" value=\"" + _fnToNull(vResult[i]["HBL_NO"]) + "\"> ";
                    vHTML += "              <input type=\"hidden\" name=\"File_BKG_NO\" value=\"" + _fnToNull(vResult[i]["BKG_NO"]) + "\"> ";
                    vHTML += "              <input type=\"hidden\" name=\"File_INV_NO\" value=\"" + _fnToNull(vResult[i]["INV_NO"]) + "\"> ";
                    vHTML += "              <input type=\"hidden\" name=\"File_SEQ\" value=\"" + i + "\"> ";
                    vHTML += "   			<span class=\"count\">" + _fnToNull(vResult[i].FILE_CNT) + "</span> ";
                    vHTML += "   		</a> ";
                }
                vHTML += "   		</p> ";
                vHTML += "   	</li> ";

                var v = $("#Select_Bound").val();
                if (v == 'E') {
                    if (_fnToNull(vResult[i]["STATUS"]) == "Q") {
                        vHTML += "   	<li class=\"info-txt info-txt--full\"> ";
                        vHTML += "   		<p class=\"title\">부킹</p> ";
                        vHTML += "   		<p class=\"des\"> ";
                        vHTML += "   		<span class=\"success-btn btn1 icn_state1\"></span> ";
                        vHTML += "   		</p> ";
                        vHTML += "   	</li> ";
                    } else if (_fnToNull(vResult[i]["CFS_BKG_NO"]) == "" || _fnToNull(vResult[i]["STATUS"]) == "Y") {
                        if (_fnToNull(vResult[i]["CFS_BKG_CHK"]) == "Y") {
                            vHTML += "   	<li class=\"info-txt info-txt--full\"> ";
                            vHTML += "   		<p class=\"title\">부킹</p> ";
                            vHTML += "   		<p class=\"des\"> ";
                            vHTML += "   		<span class=\"success-btn btn1 icn_state3\"name='layer_CFSPrintData'></span> ";
                            vHTML += "          <input type='hidden' name='CFS_BKG_NO' value='" + _fnToNull(vResult[i]["CFS_BKG_NO"]) + "'> ";
                            vHTML += "          <input type='hidden' name='HBL_NO' value='" + _fnToNull(vResult[i]["HBL_NO"]) + "'> ";
                            vHTML += "   		</p> ";
                            vHTML += "   	</li> ";
                        }
                        else if (_fnToNull(vResult[i]["CFS_BKG_NO"]) == "") {
                            vHTML += "   	<li class=\"info-txt info-txt--full\"> ";
                            vHTML += "   		<p class=\"title\">부킹</p> ";
                            vHTML += "   		<p class=\"des\"> ";
                            vHTML += "   		<span class=\"success-btn btn1 icn_state1\"></span> ";
                            vHTML += "   		</p> ";
                            vHTML += "   	</li> ";
                        }
                        else {
                            vHTML += "   	<li class=\"info-txt info-txt--full\"> ";
                            vHTML += "   		<p class=\"title\">부킹</p> ";
                            vHTML += "   		<p class=\"des\"> ";
                            vHTML += "   		<span class=\"success-btn btn1 icn_state2\"></span> ";
                            vHTML += "   		</p> ";
                            vHTML += "   	</li> ";
                        }
                    }
                    else {
                        vHTML += "   	<li class=\"info-txt info-txt--full\"> ";
                        vHTML += "   		<p class=\"title\">부킹</p> ";
                        vHTML += "   		<p class=\"des\"> ";
                        vHTML += "   		<span class=\"success-btn btn1 icn_state3\"></span> ";
                        vHTML += "   		</p> ";
                        vHTML += "   	</li> ";
                    }
                } else {
                    vHTML += "   	<li class=\"info-txt info-txt--full\" style=\"display:none\"> ";
                    vHTML += "   		<p class=\"title\">부킹</p> ";
                    vHTML += "   		<p class=\"des\"> ";
                    vHTML += "   		<span class=\"success-btn btn1 icn_state3\"></span> ";
                    vHTML += "   		</p> ";
                    vHTML += "   	</li> ";
                }


                vHTML += "   	<li class=\"info-txt info-txt--full\"> ";
                if (_fnToNull(vResult[i]["EX_IM_TYPE"]) == "E") {
                    vHTML += "   		<p class=\"title\">B/L</p> ";
                } else {
                    vHTML += "   		<p class=\"title\">A/N</p> ";
                }
                vHTML += "   		<p class=\"des\"> ";

                if (_fnToNull(vResult[i]["IS_CHKBL_YN"]) == "Y") {
                    vHTML += "   		<span class=\"success-btn btn1 icn_state3\" name=\"layer_BLPrintData\"></span> ";
                    vHTML += "          <input type=\"hidden\" name=\"HBL_NO\" value=\"" + _fnToNull(vResult[i]["HBL_NO"]) + "\"> ";

                    if (_fnToNull(vResult[i]["EX_IM_TYPE"]) == "E") {
                        vHTML += "          <input type=\"hidden\" name=\"DOC_TYPE\" value=\"CHBL\"> ";
                    }
                    else if (_fnToNull(vResult[i]["EX_IM_TYPE"]) == "I") {
                        vHTML += "          <input type=\"hidden\" name=\"DOC_TYPE\" value=\"AN\"> ";
                    }
                }
                else if (_fnToNull(vResult[i]["IS_CHKBL_YN"]) == "N") {
                    vHTML += "   		<span class=\"success-btn btn1 icn_state1\" ></span> ";
                }

                vHTML += "   		</p> ";
                vHTML += "   	</li> ";
                if (_fnToNull($("#Session_USR_TYPE").val()) != "P") {
                    vHTML += "   	<li class=\"info-txt info-txt--full\"> ";
                    vHTML += "   		<p class=\"title\">Invoice</p> ";
                    vHTML += "   		<p class=\"des\"> ";
                    if (_fnToNull(vResult[i].INV_NO) != "") {
                        if (_fnToNull(vResult[i].INV_YN) == "Y") {
                            vHTML += "   		<span class=\"success-btn btn3 icn_state3\" name=\"layer_InvPrintData\"></span> ";
                            vHTML += "          <input type=\"hidden\" name=\"HBL_NO\" value=\"" + _fnToNull(vResult[i]["HBL_NO"]) + "\"> ";
                            vHTML += "          <input type=\"hidden\" name=\"INV_NO\" value=\"" + _fnToNull(vResult[i]["INV_NO"]) + "\"> ";
                            vHTML += "          <input type=\"hidden\" name=\"DOC_TYPE\" value=\"INV\"> ";
                        } else {
                            vHTML += "   		<span class=\"success-btn btn3 icn_state2\"></span> ";
                        }
                    } else {
                        vHTML += "   		<span class=\"success-btn btn3 icn_state1\"></span> ";
                    }
                    vHTML += "   		</p> ";
                    vHTML += "   	</li> ";
                }
                vHTML += "   	<li class=\"info-txt info-txt--full\">		 ";
                vHTML += "   		<a href=\"javascript:void(0)\" class=\"bk-td\" name=\"btn_Tracking\"> ";
                vHTML += "   			Tracking ";
                vHTML += "              <input type=\"hidden\" name=\"Tracking_HBL_NO\" value=\"" + _fnToNull(vResult[i]["HBL_NO"]) + "\"> ";
                vHTML += "   		</a> ";
                vHTML += "   	</li> ";
                vHTML += "   </ul> ";
            });

            $("#MyBoard_Result_AREA_MO")[0].innerHTML = vHTML;
            $("#Paging_Area").show();
        }
        else if (JSON.parse(vJsonData).Result[0]["trxCode"] == "N") {
            _isSearch = false;

            //PC
            vHTML += "   <tr> ";
            vHTML += "   	<td class=\"no_data\" colspan=\"9\"><span class=\"font-weight-medium\">데이터가 없습니다.</span></td> ";
            vHTML += "   </tr> ";

            $("#MyBoard_Result_AREA_PC")[0].innerHTML = vHTML;

            vHTML = "";

            //MO
            vHTML += "   <ul class=\"info-box py-2 px-1\"> ";
            vHTML += "   	<li class=\"no_data col-12 py-6\"><span class=\"font-weight-medium\">데이터가 없습니다.</span></li> ";
            vHTML += "   </ul> ";

            $("#MyBoard_Result_AREA_MO")[0].innerHTML = vHTML;

            $("#Paging_Area").hide();
        }
        else if (JSON.parse(vJsonData).Result[0]["trxCode"] == "E") {
            _isSearch = false;

            //PC
            vHTML += "   <tr> ";
            vHTML += "   	<td class=\"no_data\" colspan=\"9\"><span class=\"font-weight-medium\">관리자에게 문의하세요.</span></td> ";
            vHTML += "   </tr> ";

            $("#MyBoard_Result_AREA_PC")[0].innerHTML = vHTML;

            vHTML = "";

            //MO
            vHTML += "   <ul class=\"info-box py-2 px-1\"> ";
            vHTML += "   	<li class=\"no_data col-12 py-6\"><span class=\"font-weight-medium\">관리자에게 문의하세요.</span></li> ";
            vHTML += "   </ul> ";

            $("#MyBoard_Result_AREA_MO")[0].innerHTML = vHTML;
            $("#Paging_Area").hide();
        }
    }
    catch (err) {
        console.log("[Error - fnMakeMyBoardList()]" + err.message);
    }
}

$(document).on('click', 'span[name=layer_BLPrintData], span[name=layer_InvPrintData], span[name=layer_CFSPrintData]', function () {
    $('.main-body').css('overflow', 'hidden')
});
$(document).on('click', '.close.white', function () {
    $('.main-body').css('overflow', 'unset')
})

//BL 출력 - 수정요청사항 데이터  저장하기
function fnSaveBLRequest() {
    try {
        var vResult;

        var objJsonData = new Object();
        objJsonData.HBL_NO = vHBL_NO;
        objJsonData.RMK = $("#layer_Request_disabled").val() + $("#layer_Request_area").val();
        objJsonData.RMK = objJsonData.RMK.replace(/'/gi, "`").replace(/\[/gi, "{").replace(/\]/gi, "}"); //twkim 대괄호가 JSON 형식으로 보낼때 꼬여서 됨.
        //objJsonData.RMK = $("#layer_Request_area").val().replace(/'/gi, "`").replace(/\[/gi, "{").replace(/\]/gi, "}"); //twkim 대괄호가 JSON 형식으로 보낼때 꼬여서 됨.
        objJsonData.CUST_CD = $("#Session_CUST_CD").val();
        objJsonData.LOC_NM = $("#Session_LOC_NM").val();
        objJsonData.USR_ID = $("#Session_USR_ID").val();
        objJsonData.REQ_SVC = _vREQ_SVC;
        objJsonData.EMAIL = $("#Session_EMAIL").val();

        $.ajax({
            type: "POST",
            dataType: "json",
            url: "/MyData/fnSaveBLRequest",
            async: false,
            data: { "vJsonData": _fnMakeJson(objJsonData) },
            success: function (result, status, xhr) {
                $("#layer_Request_area").val("");
                $("#layer_Request_area").text("");
                fnMakeBLRequest(result);
                _fnLayerAlertMsg("수정 요청되었습니다.");

            },
            error: function (xhr, status, error) {
                _fnAlertMsg("[Error]관리자에게 문의 해 주세요. " + status);
                return;
            }
        });
    }
    catch (err) {
        console.log(err);
    }
}

//수정 요청사항 데이터 그려주기
function fnMakeBLRequest(vJsonData) {

    var vHTML = "";

    if (JSON.parse(vJsonData).Result[0]["trxCode"] == "Y") {
        var vResult = JSON.parse(vJsonData).Request;

        //$.each(vResult, function (i) {
        //    //vHTML += (i+1) + "차 요청 사항입니다.\n";
        //    vHTML += "[";
        //    vHTML += _fnFormatDate(_fnToNull(vResult[i]["INS_YMD"]));
        //    vHTML += " ";
        //    vHTML += _fnFormatHHMMSSTime(_fnToNull(vResult[i]["INS_HM"]));
        //    vHTML += "]\n";
        //    vHTML += _fnToNull(vResult[i]["RMK"]);
        //    if ((i+1) != vResult.length) {
        //        vHTML += "\n\n";
        //    }
        //});

        if (vResult.length > 0) {
            vHTML += _fnToNull(vResult[0]["RMK"]);
            vHTML += "\n===============\n" + (vResult[0]["SEQ"] + 1) + "차 요청사항입니다\n";
        } else {
            vHTML += "===============\n1차 요청사항입니다\n";
        }

        $("#layer_Request_disabled").text(vHTML);

    }
    else if (JSON.parse(vJsonData).Result[0]["trxCode"] == "N") {
        vHTML += "수정요청사항을 아래 적어주세요.\n";
        $("#layer_Request_disabled").text(vHTML);
    }
    else if (JSON.parse(vJsonData).Result[0]["trxCode"] == "E") {
        console.log("[Error]fnMakeBLRequest :" + JSON.parse(vJsonData).Result[0]["trxMsg"]);
    }

    //스크롤 맨 아래로 내림
    var vScrollDown = $("#layer_Request_disabled").prop('scrollHeight');
    $("#layer_Request_disabled").scrollTop(vScrollDown);

}

//function Chathub_Push_Message(obj) {
//    strDomain = $("#Session_DOMAIN").val();
//    if (strDomain == null) strDomain = _fnToNull(_Domain);
//    var flagLogin = false;
//    if (_fnToNull($("#Session_DOMAIN").val()) != "") flagLogin = true;

//    var userId = obj.USR_ID;
//    var jobType = obj.JOB_TYPE;
//    var Message = obj.MSG;
//    var ref1 = obj.REF1;
//    var ref2 = obj.REF2;
//    var ref3 = obj.REF3;
//    var ref4 = obj.REF4;
//    var ref5 = obj.REF5;

//    var conObj = new Object();

//    if (_fnToNull(strDomain) != "") {  //도메인 정보가 있을때만 가능하다

//        if (chatHub == null) {  //ChatHub가 값이 없으면 reConnect           

//            console.log('Could not connect');
//            hubConn = $.hubConnection(vChatHubUrl);
//            chatHub = hubConn.createHubProxy('chatHub');
//            registerClientMethods(chatHub);

//            hubConn.start({ jsonp: true })
//                .done(function () {
//                    console.log('Now connected, connection ID=' + hubConn.id);
//                    if (_fnToNull($("#Session_DOMAIN").val()) != "") {  //로그인 되어있다고 판단
//                        var conObj = new Object();
//                        conObj.NAME = userId;
//                        conObj.DOMAIN = strDomain;
//                        chatHub.invoke("Connect", conObj);
//                    }
//                })
//                .fail(function () {
//                    console.log('Could not connect');
//                });
//        } else {
//            conObj.NAME = userId;
//            conObj.DOMAIN = strDomain;
//            chatHub.invoke("Connect", conObj);
//        }

//        if (userId != "") {
//            //도메인|보내는사람|받는사람|요청서비스|구분|형태|메세지|key아이디|            
//            var FullMsg = strDomain + "|" + userId + "|" + "" + "|" + "WE" + "|" + "P" + "|" + jobType + "|" + Message + "|" + "" + "|" + ref1 + "|" + ref2 + "|" + ref3 + "|" + ref4 + "|" + ref5;
//            // alert(userId);
//            chatHub.invoke("prime_Message", strDomain, FullMsg);
//            //메세지를 보냈으면 커넥팅을 끊자
//            if (flagLogin == false) {   //로그인을 하지 않은상태면 연결을 끊는다
//                chatHub.invoke("DisConnect", conObj);
//            }

//        } else {
//            console.log('User ID is Empty');
//        }
//    }
//}
//Invoice 출력 - 수정요청사항 데이터 가져오기
function fnGetInvRequest() {
    try {
        var vResult;

        var objJsonData = new Object();
        objJsonData.INV_NO = _vInvNo;
        objJsonData.DOC_TYPE = "INV";

        $.ajax({
            type: "POST",
            dataType: "json",
            url: "/MyData/fnSearchInvRequest",
            async: false,
            data: { "vJsonData": _fnMakeJson(objJsonData) },
            success: function (result, status, xhr) {
                fnMakeInvRequest(result);
            },
            error: function (xhr, status, error) {
                _fnAlertMsg("[Error]관리자에게 문의 해 주세요. " + status);
                return;
            }
        });
    }
    catch (err) {
        console.log(err);
    }
}
//수정 요청사항 데이터 그려주기
function fnMakeInvRequest(vJsonData) {

    var vHTML = "";

    if (JSON.parse(vJsonData).Result[0]["trxCode"] == "Y") {
        var vResult = JSON.parse(vJsonData).Request;

        //$.each(vResult, function (i) {
        //    //vHTML += (i+1) + "차 요청 사항입니다.\n";
        //    vHTML += "[";
        //    vHTML += _fnFormatDate(_fnToNull(vResult[i]["INS_YMD"]));
        //    vHTML += " ";
        //    vHTML += _fnFormatHHMMSSTime(_fnToNull(vResult[i]["INS_HM"]));
        //    vHTML += "]\n";
        //    vHTML += _fnToNull(vResult[i]["RMK"]);
        //    if ((i + 1) != vResult.length) {
        //        vHTML += "\n\n";
        //    }
        //});

        if (vResult.length > 0) {
            vHTML += _fnToNull(vResult[0]["RMK"]);
            vHTML += "\n===============\n" + (vResult[0]["SEQ"] + 1) + "차 요청사항입니다\n";
        } else {
            vHTML += "===============\n1차 요청사항입니다\n";
        }

        $("#layer_Request_disabled_INV").text(vHTML);
    }
    else if (JSON.parse(vJsonData).Result[0]["trxCode"] == "N") {
        //vHTML += "===============\n1차 요청사항입니다\n";
        vHTML += "수정요청사항을 아래 적어주세요.\n";
        $("#layer_Request_disabled_INV").text(vHTML);
    }
    else if (JSON.parse(vJsonData).Result[0]["trxCode"] == "E") {
        console.log("[Error]fnMakeInvRequest :" + JSON.parse(vJsonData).Result[0]["trxMsg"]);
    }

    //스크롤 맨 아래로 내리기 - 맨 처음은 레이어 팝업이 열리지 않아서 이동하지 않는다.
    var vScrollDown = $("#layer_Request_disabled_INV").prop('scrollHeight');
    $("#layer_Request_disabled_INV").scrollTop(vScrollDown);

}

//Invoice 출력 - 수정요청사항 데이터  저장하기
function fnSaveInvRequest() {
    try {
        var vResult;

        var objJsonData = new Object();
        objJsonData.INV_NO = _vInvNo;
        objJsonData.HBL_NO = _vHblNo;
        objJsonData.RMK = $("#layer_Request_disabled_INV").val() + $("#layer_Request_area_INV").val();
        objJsonData.RMK = objJsonData.RMK.replace(/'/gi, "`").replace(/\[/gi, "{").replace(/\]/gi, "}"); //twkim 대괄호가 JSON 형식으로 보낼때 꼬여서 됨.
        objJsonData.CUST_CD = $("#Session_CUST_CD").val();
        objJsonData.LOC_NM = $("#Session_LOC_NM").val();
        objJsonData.USR_ID = $("#Session_USR_ID").val();
        objJsonData.REQ_SVC = _vREQ_SVC;
        objJsonData.EMAIL = $("#Session_EMAIL").val();

        $.ajax({
            type: "POST",
            dataType: "json",
            url: "/MyData/fnSaveInvRequest",
            async: false,
            data: { "vJsonData": _fnMakeJson(objJsonData) },
            success: function (result, status, xhr) {
                $("#layer_Request_area_INV").val("");
                $("#layer_Request_area_INV").text("");
                fnGetInvRequest();
                _fnLayerAlertMsg("수정 요청되었습니다.");

            },
            error: function (xhr, status, error) {
                _fnAlertMsg("[Error]관리자에게 문의 해 주세요. " + status);
                return;
            }
        });
    }
    catch (err) {
        console.log(err);
    }
}
//EXPORT 선택->ETD, IMPORT 선택->ETA
$(function () {
    $("#Select_Bound").change(function () {
        var v1 = $("#Select_Bound").val();
        if (v1 == "E") {
            $("#Select_ETD_ETA").val("ETD");
            $("#th_booking").show();
            $('#th_bl').text("B/L");
        } else {
            $("#Select_ETD_ETA").val("ETA");
            $("#th_booking").hide();
            $('#th_bl').text("A/N");
        }

    });

    $("#Select_ETD_ETA").change(function () {
        var v2 = $("#Select_ETD_ETA").val();
        if (v2 == "ETD") {
            $("#Select_Bound").val("E");
            $("#th_booking").show();
            $('#th_bl').text("B/L");
        } else {
            $("#Select_Bound").val("I");
            $("#th_booking").hide();
            $('#th_bl').text("A/N");
        }
        fnInitData();
    });
});
//수출 / 수입 변경 이벤트
$(document).on("change", "#Select_Bound", function () {

    if ($(this).find("option:selected").val() == "E") {
        $(".pop__export").show();
/*        $(".pop__import").hide();*/
        $('.city-text-dpt').text('출발');
        $('.city-text-arrive').text('도착');
        $('#th_bl').text("B/L");
    }
    else if ($(this).find("option:selected").val() == "I") {
/*        $(".pop__export").hide();*/
        $(".pop__import").show();
        $('.city-text-arrive').text('출발');
        $('.city-text-dpt').text('도착');
        $('#th_bl').text("A/N");
    }

    fnInitData();
});
//$(document).on("click", "span.btn1.icn_state3", function () {
//    $('#ware_pop').show();
//    $('.main-body').css('overflow', 'hidden');
//});
$(document).on("click", ".mfp-close", function () {

    layerClose("#ware_pop");

    //$('#ware_pop').hide();
    //$('.main-body').css('overflow', 'unset');
    //$('body').removeClass('hidden_scroll');
});

$(document).on("click", "#PC_Layer_Console_Header tr", function () {
    /*$("#PC_Layer_Console_Detail tr").css("background-color", "#fff");*/
    $("#PC_Layer_Console_Header tr").css("background-color", "#fff");
    $(this).css("background-color", "#f5f5f5");
});

$(document).on("click", "#MO_Layer_Console_Detail .layer_console_mo, #MO_Layer_Console_Header .layer_console_mo", function () {
    /*$("#MO_Layer_Console_Detail .layer_console_mo").css("background-color", "#fff");*/
    $("#MO_Layer_Console_Header .layer_console_mo").css("background-color", "#fff");
    $(this).css("background-color", "#f5f5f5");
});
