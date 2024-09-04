$(document).on('click', '#hamberger', function () {
    $('.total_menu.on .lnb > li:nth-child(3) .sub_depth').addClass('on');
});
////////////////////전역 변수//////////////////////////
var _objConsoleFile = new Object(); //파일 object - 수출
var _vPage = 1; //페이징
var _ImageMngtNo = "";
var _DetailMngtNo = "";
var _DetailSeq = "";

var _OrderBy = "";
var _Sort = "";
var _isSearch = false;

////////////////////jquery event///////////////////////
$('input[name="environment"]').change(function () {
    setImageFromFile(this, '#preview');
});

function setImageFromFile(input, expression) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        $("#preview").css({ "display": "block" });
        reader.onload = function (e) {
            $(expression).attr('src', e.target.result);
        }
        reader.readAsDataURL(input.files[0]);
    }
}


function setImageFromDownFile(fileName) {
    if (fileName != "") {
        var vMngtNo = $(this).parents(".file-box").find(".input_FileList_MNGT_NO").val();
        var vSeq = $(this).parents(".file-box").find(".input_FileList_SEQ").val();

        fnImageDown(vMngtNo, vSeq);
    }
}
//출발지 도착지 삭제 버튼 이벤트
$(document).on("click", ".delete-btn", function () {
    if ($(this).attr("name") == "del_BKG") {
        $("#input_BkNo").val("");
    }
    else if ($(this).attr("name") == "del_POD") {
        $("#input_POD").val("");
        $("#input_POD").attr("placeholder", "도착");
        $("#input_PODCD").val("");
    }

    $(this).hide();
});


$(function () {

    //로그인 세션 확인
    if (_fnToNull($("#Session_USR_ID").val()) == "") {
        window.location = window.location.origin;
    }
    else {
        $("#input_ETD").val(_fnMinusDate(365));
        $("#input_ETA").val(_fnPlusDate(0));
    }
});


//달력 클릭 이벤트
$(document).on("click", "#input_ETD_Icon", function () {
    $("#input_ETD").focus();
});

//달력 클릭 이벤트
$(document).on("click", "#input_ETA_Icon", function () {
    $("#input_ETA").focus();
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
        $(this).val(_fnPlusDate(0));
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
        $(this).val(_fnPlusDate(7));
    }

    //날짜 벨리데이션 체크
    var vETD = $("#input_ETD").val().replace(/[^0-9]/g, "");
    var vETA = $("#input_ETA").val().replace(/[^0-9]/g, "");

    if (vETA < vETD) {
        _fnAlertMsg("ETA가 ETD 보다 빠를 수 없습니다. ");
        $("#input_ETA").val(vETD.substring("0", "4") + "-" + vETD.substring("4", "6") + "-" + vETD.substring("6", "8"));
    }
});



//창고 조회 버튼 이벤트
$(document).on("click", "#btn_Console_Search", function () {

    _OrderBy = "";
    _Sort = "";
    _vPage = 1;
    fnConsoleSearch();


});
//파일 다운로드 로직
$(document).on("click", "#btn_Preview_Download", function () {

    if (_fnToNull($("#hdn_Preview_MngtNo").val()) == "") {
        alert("파일을 먼저 선택 해주시기 바랍니다.");
    } else {
        fnImageDown($("#hdn_Preview_MngtNo").val(), $("#hdn_Preview_Seq").val());
    }
});
//TEST


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

            $("#Console_orderby th button").removeClass("on");
            if (vValue == "asc") {
                $(this).children("button").addClass('on');
            } else if (vValue == "asc") {
                $(this).children("button").removeClass('on');
            }

            _vPage = 1;
            _OrderBy = $(this).children("button").val();
            _Sort = vValue.toUpperCase();
            fnConsoleSearch();
        }
    }

});

////파일 업로드 버튼 이벤트
$(document).on("click", "a[name='layer_ImageUpload']", function () {
    $("#Upload_pop").show();
    //초기화
    _objConsoleFile = new Object();
    _objConsoleFile.FILE_INFO = new Array();
    $("#layer_Upload_FileList").empty();
    $("#preview").attr("src", "");
    $("#now_file").text("");
    $("#now_seq").text("");
    ////파일 데이터 가져오기 
    _ImageMngtNo = $(this).siblings("input[type='hidden']").val(); //관리번호    
    fnSetFileList(_ImageMngtNo);
});

//파일 리스트 - 엑스  박스 버튼 이벤트 
$(document).on("click", "a[name='Layer_FileList_Delete']", function () {

    var vValue = $(this).parents(".file-box").find(".input_FileList_SetTime").val();
    var vList = $(this).parents(".file-box__text").val();
    var vThis;
    //처음 파일 저장시 SETTIME을 주어서 취소 눌렀을 때 그걸 비교하여 li에 있는 값들을 삭제 합니다.
    for (var i = 0; i < _objConsoleFile.FILE_INFO.length; i++) {
        if (vValue == _fnToNull(_objConsoleFile.FILE_INFO[i]["SETTIME"])) {

            if (_objConsoleFile.FILE_INFO[i].constructor.name == "File") {
                _objConsoleFile.FILE_INFO[i]["FILE_YN"] = "N";
                $(this).parents(".file-box").remove();
            } else {
                vThis = this;
                $("#ConsoleCancel_content").html("파일을 삭제 하시겠습니까?");
                layerPopup3('#ConsoleCancel', "");
                $("#ConsoleCancel_confirm").click(function (e) {
                    fnLayerFileDelete(vThis);
                    layerClose2('#ConsoleCancel');
                });

            }
        }
    }
});

//카메라 이벤트
$(document).on("change", "#camera", function (e) {
    fnLayerFileSet(this);
    $(this).val("");
});

//수출 POD AutoComplete
$(document).on("keyup", "#input_POD", function () {
    //input_POL 초기화
    if (_fnToNull($(this).val()) == "") {
        $("#input_PODCD").val("");
    }

    //출발 도시 바로 선택 화면 가리기
    if ($(this).val().length > 0) {
        $("button[name='input_POD']").show();
        $("#select_SEA_pop02").hide();
    }

    //autocomplete
    $(this).autocomplete({
        minLength: 3,
        open: function (event, ui) {
            $(this).autocomplete("widget").css({
                "width": $(".pop__import").width()
            });
        },
        source: function (request, response) {
            var result = _fnGetWebWarePortData($("#input_POD").val().toUpperCase());
            if (result != undefined) {
                result = JSON.parse(result).Table
                response(
                    $.map(result, function (item) {
                        return {
                            label: item.LOC_NM,
                            value: item.LOC_NM,
                            code: item.LOC_CD
                        }
                    })
                );
            }
        },
        select: function (event, ui) {
            if (ui.item.value.indexOf('데이터') == -1) {
                $("#input_POD").val(ui.item.value);
                $("#input_PODCD").val(ui.item.code);
                vPort = ui.item.code;
            } else {
                ui.item.value = "";
            }
        }
    }).autocomplete("instance")._renderItem = function (ul, item) {
        return $("<li>")
            .append("<div>" + item.value + "<br>" + item.code + "</div>")
            .appendTo(ul);
    };
});

//파일 업로드 버튼
$(document).on("click", "#layer_FileUpload", function () {

    var vBoolean = false;

    for (var i = 0; i < _objConsoleFile.FILE_INFO.length; i++) {
        if (_objConsoleFile.FILE_INFO[i]["IS"] == "FILE" && _objConsoleFile.FILE_INFO[i]["FILE_YN"] == "Y") {
            vBoolean = true;
        }
    }

    if (vBoolean) {
        fnLayerFileUpload(_ImageMngtNo, _objConsoleFile);
    } else {
        _fnLayerAlertMsg("파일을 먼저 등록 해 주세요.");
    }

});

//파일 다운로드
$(document).on("click", "p[name='Layer_File_Download']", function () {
    $("p[name='Layer_File_Download']").removeClass("font-bold");
    $(this).addClass("font-bold");
    var vMngtNo = $(this).parents(".file-box").find(".input_FileList_MNGT_NO").val();
    var vSeq = $(this).parents(".file-box").find(".input_FileList_SEQ").val();
    if (_fnToNull(vMngtNo) != "") {
        fnImageView(vMngtNo, vSeq);
    } else {
        $(".no_img_data").show();
    }

});

////창고 디테일 조회
$(document).on("click", "a[name='layer_ConsoleData']", function () {
    _DetailMngtNo = $(this).siblings("input[name='layer_DetailMngtNo']").val(); //관리번호

    fnLayer_Console_Detail(_DetailMngtNo, _DetailSeq)

});
////////////////////////function///////////////////////
function fnLayer_Console_Detail(mngt_no, seq) {
    try {
        var objJsonData = new Object();

        objJsonData.BK_NO = _fnToNull(mngt_no);
        objJsonData.BK_SEQ = _fnToNull(seq);
        objJsonData.USR_TYPE = _fnToNull($("#Session_USR_TYPE").val());
        objJsonData.CUST_CD = _fnToNull($("#Session_CUST_CD").val());

        $.ajax({
            type: "POST",
            url: "/Popup/fnGetLayerConsoleDetail",
            async: false,
            dataType: "json",
            data: { "vJsonData": _fnMakeJson(objJsonData) },
            success: function (result) {
                layerPopup2("#Ware_detail");
                fnMakeLayerConsole(result);

                var isMobile = /Android|webOS|iPhone|iPad|iPod|BlackBerry/i.test(navigator.userAgent) ? true : false;
                if (!isMobile) {
                    //PC
                    $("#PC_Layer_Console_Header tr").eq(0).click();
                } else {
                    //MOBILE
                    $(".MO_Layer_Console_Header_List div").eq(0).click();
                }
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
/////////////////function MakeList/////////////////////
//콘솔 상세 조회
function fnMakeLayerConsole(vJsonData) {
    try {
        var vHTML = "";

        if (JSON.parse(vJsonData).Result[0]["trxCode"] == "Y") {
            var vResult = JSON.parse(vJsonData).Console;
            $("#img_bk_no").val(_fnToNull(vResult[0]["BK_NO"]));

            
            if (_fnToNull(vResult[0]["FILE_NM"]) != "") {
                $("#img_yn").attr("src", "/Images/Masstige/e-service/upload-img.png")
            } else {
                $("#img_yn").attr("src", "/Images/Masstige/e-service/upload_no_img.png")
            };
            
            

            $.each(vResult, function (i) {
                vHTML += "   <tr> ";
                vHTML += "   	<td class=\"txt\">" + String(_fnToNull(vResult[i]["GR_YMD"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3') + "</td> ";
                vHTML += "   	<td class=\"txt\">" + _fnToNull(vResult[i]["ACT_GR_CUST"]) + "</td> ";
                vHTML += "   	<td class=\"txt\">" + _fnToNull(vResult[i]["SKU_NM"]) + "</td> ";
                vHTML += "   	<td class=\"txt\">" + fnSetComma(_fnToNull(vResult[i]["ACT_QTY"])) + "</td> ";
                vHTML += "   	<td class=\"txt\">" + _fnToNull(vResult[i]["BK_QTY_UNIT"]) + "</td> ";
                vHTML += "   	<td class=\"txt\">" + fnSetComma(_fnToNull(vResult[i]["BK_GRS_WGT"])) + "</td> ";
                vHTML += "   	<td class=\"txt\">" + fnSetComma(_fnToNull(vResult[i]["ACT_MSRMT"])) + "</td> ";
                vHTML += "          <input type='hidden' name='GR_NO' value='" + _fnToNull(vResult[i]["GR_NO"]) + "'> ";
                vHTML += "          <input type='hidden' name='BK_NO' value='" + _fnToNull(vResult[i]["BK_NO"]) + "'> ";
                vHTML += "   </tr> ";
            });

            $("#PC_Layer_Console_Header")[0].innerHTML = vHTML;

            vHTML = "";

            $.each(vResult, function (i) {
            vHTML += "                                       <div class='MO_Layer_Console_Header_List'>";
            vHTML += "                                            <div class='info-txt info-txt--full'> ";
            vHTML += "                                                <p class='title font-main'>입고일자</p> ";
            vHTML += "                                                <p class='des'>" + String(_fnToNull(vResult[i]["GR_YMD"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3') + "</p> ";
            vHTML += "                                            </div> ";
            vHTML += "                                            <div class='info-txt info-txt--full'> ";
            vHTML += "                                                <p class='title font-main'>실입고처</p> ";
            vHTML += "                                                <p class='des'>" + _fnToNull(vResult[i]["ACT_GR_CUST"]) + "</p> ";
            vHTML += "                                            </div> ";
            vHTML += "                                            <div class='info-txt info-txt--full'> ";
            vHTML += "                                                <p class='title font-main'>품목</p> ";
            vHTML += "                                                <p class='des'>" + _fnToNull(vResult[i]["SKU_NM"]) + "</p> ";
            vHTML += "                                            </div> ";
            vHTML += "                                            <div class='mo-flex'> ";
            vHTML += "                                                <div class='info-txt info-txt--full'> ";
            vHTML += "                                                    <p class='title font-main'>수량</p> ";
            vHTML += "                                                    <p class='des'>" + fnSetComma(_fnToNull(vResult[i]["ACT_QTY"])) + "</p> ";
            vHTML += "                                                </div> ";
            vHTML += "                                                <div class='info-txt info-txt--full'>";
            vHTML += "                                                    <p class='title font-main'>단위</p> ";
            vHTML += "                                                    <p class='des'>" + _fnToNull(vResult[i]["BK_QTY_UNIT"]) + "</p> ";
            vHTML += "                                                </div> ";
            vHTML += "                                            </div> ";
            vHTML += "                                            <div class='mo-flex'> ";
            vHTML += "                                                <div class='info-txt info-txt--full'> ";
            vHTML += "                                                    <p class='title font-main'>중량</p> ";
            vHTML += "                                                    <p class='des'>" + fnSetComma(_fnToNull(vResult[i]["BK_GRS_WGT"])) + "</p> ";
            vHTML += "                                                </div> ";
            vHTML += "                                                <div class='info-txt info-txt--full border_bottom'> ";
            vHTML += "                                                    <p class='title font-main'>CBM</p> ";
            vHTML += "                                                    <p class='des'>" + fnSetComma(_fnToNull(vResult[i]["ACT_MSRMT"])) + "</p> ";
            vHTML += "                                                </div> ";
             vHTML += "                                            </div> ";
                vHTML += "          <input type='hidden' name='GR_NO' value='" + _fnToNull(vResult[i]["GR_NO"]) + "'> ";
                vHTML += "          <input type='hidden' name='BK_NO' value='" + _fnToNull(vResult[i]["BK_NO"]) + "'> ";
            vHTML += "                                            </div> ";
            });


            $("#MO_Layer_Console_Header_List_MO")[0].innerHTML = vHTML;

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

$(document).on("click", "#PC_Layer_Console_Header tr", function () {
    var vGrNo = $(this).find("input[name='GR_NO']").val();
    var vBkNo = $(this).find("input[name='BK_NO']").val();

    fnLayer_Console_Detail_NEW(vGrNo, vBkNo)
});

$(document).on("click", ".MO_Layer_Console_Header_List", function () {
    var vGrNo = $(this).children("input[name='GR_NO']").val();
    var vBkNo = $(this).children("input[name='BK_NO']").val();

    fnLayer_Console_Detail_NEW(vGrNo, vBkNo)
});


//분할 입고 눌렀을때 화물 상세 나오는 함수
function fnLayer_Console_Detail_NEW(GR_NO,BK_NO) {
    try {
        var objJsonData = new Object();
        objJsonData.USR_TYPE = _fnToNull($("#Session_USR_TYPE").val());
        objJsonData.CUST_CD = _fnToNull($("#Session_CUST_CD").val());
        objJsonData.GR_NO = _fnToNull(GR_NO);
        objJsonData.BK_NO = _fnToNull(BK_NO);
        objJsonData.BK_SEQ = "";
        $.ajax({
            type: "POST",
            url: "/Popup/fnGetLayerConsoleDetail_NEW",
            async: false,
            dataType: "json",
            data: { "vJsonData": _fnMakeJson(objJsonData) },
            success: function (result) {
                fnMakeLayerConsoleDetail_NEW(result);
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



//콘솔 상세 조회 디테일
function fnMakeLayerConsoleDetail_NEW(vJsonData) {
    try {

        var vHTML = "";
        var vResult = JSON.parse(vJsonData).DeTail;

        if (JSON.parse(vJsonData).DeTail != undefined) {

            vHTML = "";

            $.each(vResult, function (i) {
                vHTML += "   <tr> ";
                vHTML += "   	<td class=\"txt\">" + _fnToNull(vResult[i]["GR_NO"]) + "</td> ";
                vHTML += "   	<td class=\"txt\">" + fnSetComma(_fnToNull(vResult[i]["SIZE_W"])) + "</td> ";
                vHTML += "   	<td class=\"txt\">" + fnSetComma(_fnToNull(vResult[i]["SIZE_D"])) + "</td> ";
                vHTML += "   	<td class=\"txt\">" + fnSetComma(_fnToNull(vResult[i]["SIZE_H"])) + "</td> ";
                vHTML += "   	<td class=\"txt\">" + fnSetComma(_fnToNull(vResult[i]["QTY"])) + "</td> ";
                vHTML += "   	<td class=\"txt\">" + fnSetComma(_fnToNull(vResult[i]["MSRMT"])) + "</td> ";
                vHTML += "   	<td class=\"txt\">" + _fnToNull(vResult[i]["RMK2"]) + "</td> ";
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
                vHTML += "   <li class=\"info-txt info-txt--full\"> ";
                vHTML += "   	<p class=\"title font-main\">용적</p> ";
                vHTML += "   	<p class=\"des\">" + fnSetComma(_fnToNull(vResult[i]["MSRMT"])) + "</p> ";
                vHTML += "   </li> ";
                vHTML += "   <li class=\"info-txt info-txt--full border_bottom\"> ";
                vHTML += "   	<p class=\"title font-main\">Mark</p> ";
                vHTML += "   	<p class=\"des\">" + _fnToNull(vResult[i]["RMK2"]) + "</p> ";
                vHTML += "   </li> ";
            });

            $("#MO_Layer_Console_Detail")[0].innerHTML = vHTML;
        } else {
            vHTML = "";

            vHTML += "   <tr> ";
            vHTML += "   	<td colspan=\"6\" class=\"no-data\">데이터가 없습니다.</td> ";
            vHTML += "   </tr> ";

            $("#PC_Layer_Console_Detail")[0].innerHTML = vHTML;

            vHTML = "";

            vHTML += "   <li class=\"no_data col-12 py-6\"><span class=\"font-weight-medium\">데이터가 없습니다.</span></li> ";

            $("#MO_Layer_Console_Detail")[0].innerHTML = vHTML;
        }
    }
    catch (err) {
        console.log("[Error - fnMakeLayerConsoleDetail]" + err.message);
    }
}


//콘솔 Serach
function fnConsoleSearch() {
    try {
        var objJsonData = new Object();

        objJsonData.ETD = _fnToNull($("#input_ETD").val().replace(/-/gi, ""));
        objJsonData.ETA = _fnToNull($("#input_ETA").val().replace(/-/gi, ""));
        objJsonData.BK_NO = _fnToNull($("#input_BkNo").val().toUpperCase().trim());

        objJsonData.POD = _fnToNull($("#input_POD").val());
        objJsonData.POD_CD = _fnToNull($("#input_PODCD").val());

        objJsonData.USR_TYPE = _fnToNull($("#Session_USR_TYPE").val());
        objJsonData.CUST_CD = $("#Session_CUST_CD").val();
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
            url: "/MyData/fnGetConsoleData",
            async: true,
            dataType: "json",
            data: { "vJsonData": _fnMakeJson(objJsonData) },
            success: function (result) {
                fnMakeConsoleList(result);
                if (JSON.parse(result).Result[0]["trxCode"] == "Y") {
                    fnConsolePaging(JSON.parse(result).Console[0]["TOTCNT"], 10, 10, objJsonData.PAGE);
                }
                //if (JSON.parse(result).Result[0]["trxCode"] == "Y") {
                //    fnMakeSetFileList(result);
                //}
            }, error: function (xhr, status, error) {
                alert("담당자에게 문의 하세요.");
                console.log(error);
                vReturn = false;
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
        console.log("[Error - fnConsoleSearch]" + err.message);
    }
}

//totalData = 총 데이터 count
//dataPerPage = 한페이지에 나타낼 데이터 수
//pageCount = 한화면에 나타낼 페이지 수
//currentPage = 선택한 페이지 
//공지사항 페이징
function fnConsolePaging(totalData, dataPerPage, pageCount, currentPage) {
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
    vHTML += "      	<a href=\"javascript:;\" class=\"prev-first\" onclick=\"fnConsoleGoPage(1)\" ><span>맨앞으로</span></a> ";
    vHTML += "      </li> ";
    vHTML += "      <li> ";
    vHTML += "      	<a href=\"javascript:;\" class=\"prev\" onclick=\"fnConsoleGoPage(" + prevPage + ")\"><span>이전으로</span></a> ";
    vHTML += "      </li> ";

    for (var i = first; i <= last; i++) {
        if (i == currentPage) {
            vHTML += "   <li> ";
            vHTML += "   	<a href=\"javascript:;\" class=\"active\">" + i + "<span></span></a> ";
            vHTML += "   </li> ";
        } else {
            vHTML += "   <li> ";
            vHTML += "   	<a href=\"javascript:;\" onclick=\"fnConsoleGoPage(" + i + ")\">" + i + "<span></span></a> ";
            vHTML += "   </li> ";
        }
    }

    vHTML += "      <li> ";
    vHTML += "      	<a href=\"javascript:;\" onclick=\"fnConsoleGoPage(" + nextPage + ")\" class=\"next\"><span>다음으로</span></a> ";
    vHTML += "      </li> ";
    vHTML += "      <li> ";
    vHTML += "      	<a href=\"javascript:;\" onclick=\"fnConsoleGoPage(" + totalPage + ")\" class=\"next-last\"><span>맨뒤로</span></a> ";
    vHTML += "      </li> ";
    vHTML += "   </ul> ";

    $("#Paging_Area").append(vHTML);    // 페이지 목록 생성
}

function fnConsoleGoPage(vPage) {
    _vPage = vPage;
    fnConsoleSearch();
}

//이미 올라간 파일 리스트 세팅
function fnSetFileList(vMngtNo) {
    try {
        var objJsonData = new Object();
        objJsonData.MNGT_NO = vMngtNo;

        $.ajax({
            type: "POST",
            url: "/MyData/fnLayerSetFileList",
            async: false,
            dataType: "json",
            data: { "vJsonData": _fnMakeJson(objJsonData) },
            success: function (result) {
                if (JSON.parse(result).Result[0]["trxCode"] == "Y") {
                    fnMakeSetFileList(result);
                }
                else {
                    $(".no_img_data").show();
                }

                //현재 로그인한 사람이 마스터가 아닐경우 업로드 , 삭제 불가
                if ($("#Session_USR_TYPE").val() == "M") {
                    $("#FileUploadBtn_Area").show();
                    $("a[name='Layer_FileList_Delete']").show();
                } else {
                    $("#FileUploadBtn_Area").css('visibility', 'hidden');
                    $("a[name='Layer_FileList_Delete']").hide();
                }

                layerPopup2('#Upload_pop');
            }, error: function (xhr, status, error) {
                alert("담당자에게 문의 하세요.");
                console.log(error);
                vReturn = false;
            }
        });
    }
    catch (err) {
        console.log("[Error - fnSetFileList]" + err.message);
    }
}

//실제 파일 업로드 세팅
function fnLayerFileSet(vThis) {
    try {
        var _arrFileValue = new Array(); //파일 정보 저장

        for (var i = 0; i < $(vThis).get(0).files.length; i++) {
            var vFileExtension = $(vThis).get(0).files[i].name.substring($(vThis).get(0).files[i].name.lastIndexOf(".") + 1, $(vThis).get(0).files[i].name.length).toUpperCase();
            var vFileNM = $(vThis).get(0).files[i].name;

            var vFileRegExp = /[\/\\+:*&?<>|\"#%^]/g;
            if (vFileRegExp.test(vFileNM)) {
                _fnAlertMsg('파일명에 특수문자를 제거 해주시기 바랍니다. (/, \, +, :, *, &, ?, <, >, |, ", #, % , ^) 금지');
                return false;
            }

            //파일 사이즈 10MB 이상일 경우 Exception
            if (10485759 < $(vThis).get(0).files[i].size) {
                _fnAlertMsg("10MB 이상되는 파일은 업로드 할 수 없습니다.");
                return false;
            }

            //이미지 파일이 아닐 경우
            if (vFileExtension == "BMP" || vFileExtension == "JPG" || vFileExtension == "JPEG" || vFileExtension == "JPE" || vFileExtension == "JFIF" || vFileExtension == "TIF" || vFileExtension == "TIFF" || vFileExtension == "PNG") {
                console.log("[파일 확장자] " + vFileExtension);
            } else {
                _fnAlertMsg("bmp , jpg , jpeg , jpe , jfif , tif , tiff , png 확장자만 파일 업로드를 할 수 있습니다.");
                return false;
            }

            //PC / MO 공용으로 사용하기 위한 이름 변경
            $(vThis).get(0).files[i].NAME = "IMAGE";
            $(vThis).get(0).files[i].SETTIME = _fnGetNowTime();
            $(vThis).get(0).files[i].FILE_YN = "Y";
            $(vThis).get(0).files[i].FILE_CRUD = "INSERT";
            $(vThis).get(0).files[i].IS = "FILE";
            $(vThis).get(0).files[i].DOC_NO = "";

            _arrFileValue.push($(vThis).get(0).files[i]);

            //SETTIME을 설정하기 위한 sleep 함수
            _fnsleep(50);
        }

        for (var i = 0; i < _arrFileValue.length; i++) {
            _objConsoleFile.FILE_INFO.push(_arrFileValue[i]);
        }

        var vHTML = "";

        $.each($(vThis)[0].files, function (i) {
            vHTML += "   <div class=\"file-box row align-items-center px-2\"> ";
            vHTML += "   	<a href=\"javascript:void(0)\" class=\"col-auto\" name=\"Layer_FileList_Delete\"><i class=\"xi-close-thin\"></i></a> ";
            vHTML += "   	<p class=\"file-box__text col not_uploaded\">" + $(vThis)[0].files[i].name + "</p> ";
            //vHTML += "   	<p class=\"file-box__text col not_uploaded\">IMAGE</p> ";
            vHTML += "   	<input type=\"hidden\" class=\"input_FileList_SEQ\"> ";
            vHTML += "   	<input type=\"hidden\" class=\"input_FileList_SetTime\" name=\"input_FileList_SetTime\" value=\"" + $(vThis)[0].files[i].SETTIME + "\"> ";
            vHTML += "   </div> ";
        });

        $("#layer_Upload_FileList").append(vHTML);
    }
    catch (err) {
        console.log("[Error - fnLayerFileSet]" + err.message);
    }
}

//파일 업로드 함수
function fnLayerFileUpload(vMNGT_NO, vFileObj) {
    try {

        /* 파일 업로드 & 삭제 구문 */
        var vfileData;
        var vBoolean = false;

        $("#ProgressBar_Loading_img").show();
        setTimeout(function () {
            /* 파일 Upload & 파일 삭제 로직*/
            for (var i = 0; i < vFileObj.FILE_INFO.length; i++) {
                if (vFileObj.FILE_INFO[i]["IS"] == "FILE" && vFileObj.FILE_INFO[i]["FILE_YN"] == "Y") { //File 형식이면 Insert 로직 태움

                    vfileData = new FormData(); //Form 초기화
                    vfileData.append("fileInput", vFileObj.FILE_INFO[i]);

                    //추후 세션 값으로 변경
                    vfileData.append("DOMAIN", $("#Session_DOMAIN").val());   //로그인한 User의 도메인
                    vfileData.append("MNGT_NO", vMNGT_NO); //화주 ID (사업자번호)  
                    vfileData.append("INS_USR", $("#Session_USR_ID").val());    //User
                    vfileData.append("OFFICE_CD", $("#Session_OFFICE_CD").val()); //회사 코드 
                    vfileData.append("DOC_TYPE", 'ETCH');
                    vfileData.append("DOC_NO", vMNGT_NO);



                    $.ajax({
                        type: "POST",
                        url: "/File/ConSole_Upload_Files",
                        dataType: "json",
                        async: false,
                        contentType: false, // Not to set any content header
                        processData: false, // Not to process data
                        data: vfileData,
                        success: function (result, status, xhr) {
                            if (result != null) {
                                if (JSON.parse(result).Result[0]["trxCode"] == "Y") {
                                    vBoolean = true;
                                    console.log("fnFileUpload[trxCode] " + JSON.parse(result).Result[0]["trxCode"]);
                                    console.log("fnFileUpload[trxMsg] " + JSON.parse(result).Result[0]["trxMsg"]);
                                } else {
                                    vBoolean = false;
                                    console.log("fnFileUpload[trxCode] " + JSON.parse(result).Result[0]["trxCode"]);
                                    console.log("fnFileUpload[trxMsg] " + JSON.parse(result).Result[0]["trxMsg"]);
                                }
                            } else {
                                _fnAlertMsg("파일 저장에 실패하였습니다.\n관리자에게 문의 해 주세요.");
                            }
                        },
                        error: function (xhr, status, error) {
                            alert("[Error]관리자에게 문의 해 주세요. " + status);
                        }
                        , complete: function () {
                            if (i == vFileObj.FILE_INFO.length - 1) {
                                if (vBoolean) {
                                    _fnLayerAlertMsg("파일 업로드가 완료 되었습니다.");

                                    //초기화
                                    _objConsoleFile = new Object();
                                    _objConsoleFile.FILE_INFO = new Array();

                                    fnSetFileList(_ImageMngtNo);
                                }
                                else {
                                    alert("파일 업로드가 실패 하였습니다. \n 관리자에게 문의 해 주세요.");
                                }

                                $("#ProgressBar_Loading_img").hide();
                            }
                        }
                    });
                }
            }
        }, 0);


    }
    catch (err) {
        console.log("[Error - fnLayerFileUpload]" + err.message);
    }
}
 
//이미지 Delete 파일
function fnLayerFileDelete(vThis) {
    try {
        
        for (var i = 0; i < _objConsoleFile.FILE_INFO.length; i++) {
            var test1 = $(vThis).parents(".file-box").find(".input_FileList_SetTime").val();
            var test2 = _objConsoleFile.FILE_INFO[i]["SETTIME"];
            console.log(_objConsoleFile.FILE_INFO);
            if ($(vThis).parents(".file-box").find(".input_FileList_SetTime").val() == _fnToNull(_objConsoleFile.FILE_INFO[i]["SETTIME"])) {
                ////삭제로직                    
                var objJsonData = new Object();
                objJsonData.FILE_INFO = new Array();
                objJsonData.FILE_INFO.push(_objConsoleFile.FILE_INFO[i]);

                $.ajax({
                    type: "POST",
                    url: "/File/ConSole_DeleteFile",
                    async: false,
                    dataType: "json",
                    data: { "vJsonData": _fnMakeJson(objJsonData) },
                    success: function (result) {
                        if (result != null) {
                            if (JSON.parse(result).Result[0]["trxCode"] == "Y") {

                                $('.preview-area > img').removeAttr('src');
                                $("#preview").css({ "display": "none" });
                                _fnLayerAlertMsg("삭제가 완료 되었습니다.");
                                _objConsoleFile.FILE_INFO.splice(i,1);
                                $(vThis).parents(".file-box").remove();
                                if ($("p[name=Layer_File_Download]").length > 0) {
                                    $("p[name=Layer_File_Download]").eq(0).click();
                                } else {
                                    $(".no_img_data").show();
                                }

                            } else {
                                _fnAlertMsg("삭제가 되지 않았습니다.\n담당자에게 문의하세요.");
                            }
                        } else {
                            _fnAlertMsg("담당자에게 문의하세요.");
                        }
                    }, error: function (xhr, status, error) {
                        _fnAlertMsg("담당자에게 문의 하세요.");
                        console.log(error);
                        vReturn = false;
                    }
                });
            }
        }
    }
    catch (err) {
        console.log("[Error - fnLayerFileDelete]" + err.message);
    }
}

function fnImageView(vMngtNo, vSEQ) {
    try {
        var objJsonData = new Object();

        objJsonData.MNGT_NO = vMngtNo; //관리번호
        objJsonData.DOMAIN = $("#Session_DOMAIN").val(); //접속 User
        objJsonData.SEQ = vSEQ;

        $.ajax({
            type: "POST",
            url: "/File/ConSole_Download",
            async: true,
            data: { "vJsonData": _fnMakeJson(objJsonData) },
            success: function (result, status, xhr) {
                if (result != "E") {
                    var rtnTbl = JSON.parse(result);
                    rtnTbl = rtnTbl.Path;
                    var file_nm = rtnTbl[0].FILE_NAME;
                    if (_fnToNull(rtnTbl) != "") {
                        $("#preview").attr('src', "../Content/TempFiles/" + file_nm);
                        $("#preview").show();
                        $("#now_file").text(rtnTbl[0].FILE_NAME);
                        $("#now_seq").text(rtnTbl[0].FILE_REAL_NAME);
                    }
                } else {
                    _fnAlertMsg("다운 받을 수 없습니다.");
                    console.log(result);
                }
            },
            error: function (xhr, status, error) {
                $("#ProgressBar_Loading").hide();
                alert("[Error]관리자에게 문의 해 주세요. " + status);
                return;
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
        console.log("[Error - fnPreAlertDown]" + err.message);
    }
}

//콘솔 이미지 다운로드
function fnImageDown(vMngtNo, vSEQ) {
    try {
        var objJsonData = new Object();

        objJsonData.MNGT_NO = vMngtNo; //관리번호
        objJsonData.DOMAIN = $("#Session_DOMAIN").val(); //접속 User
        objJsonData.SEQ = vSEQ;

        $.ajax({
            type: "POST",
            url: "/File/ConSole_Download",
            async: false,
            data: { "vJsonData": _fnMakeJson(objJsonData) },
            success: function (result, status, xhr) {
                if (result != "E") {
                    var rtnTbl = JSON.parse(result);
                    rtnTbl = rtnTbl.Path;
                    var file_nm = rtnTbl[0].FILE_NAME;
                    if (_fnToNull(rtnTbl) != "") {
                        window.location = "/File/Console_DownloadFile?FILE_NM=" + file_nm + "&REPLACE_FILE_NM=" + rtnTbl[0].FILE_REAL_NAME;
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
        console.log("[Error - fnPreAlertDown]" + err.message);
    }
}
/////////////////function MakeList/////////////////////
function fnMakeSetFileList(vJsonData) {
    try {

        var vHTML = "";
        var vResult = "";


        if (JSON.parse(vJsonData).Result[0]["trxCode"] == "Y") {
            vResult = JSON.parse(vJsonData).DOC_MST;

            $(".no_img_data").hide();
            $("#layer_Upload_FileList").empty();

            //DOC_TYPE - PC
            if (vResult != undefined) {
                if (vResult.length > 0) {
                    $.each(vResult, function (i) {

                        //문서 파일 수정
                        vResult[i]["SETTIME"] = _fnGetNowTime(); //파일 구분을 위한 값 "년월일시분초밀리초"
                        vResult[i]["FILE_YN"] = "Y";
                        vResult[i]["FILE_CRUD"] = "SELECT";
                        vResult[i]["IS"] = "OBJECT";             //파일 업로드 됐을 경우는 OBJECT / 파일 저장이 되지 않았을 때는 FILE

                        _objConsoleFile.FILE_INFO.push(vResult[i]);

                        vHTML += "   <div class=\"file-box row align-items-center px-2\"> ";
                        //vHTML += "   	<p class=\"file-box__text col\">" + $(vThis)[0].files[i].name + "</p> ";
                        vHTML += "   	<a href=\"javascript:void(0)\" class=\"col-auto\" name=\"Layer_FileList_Delete\"><i class=\"xi-close-thin\"></i></a> ";
                        vHTML += "   	<p class=\"file-box__text col\" name=\"Layer_File_Download\" style='cursor:pointer;'>" + vResult[i].FILE_NM + "</p> ";
                        vHTML += "   	<input type=\"hidden\" class=\"input_FileList_MNGT_NO\" value=\"" + vResult[i].MNGT_NO + "\"> ";
                        vHTML += "   	<input type=\"hidden\" class=\"input_FileList_SEQ\" value=\"" + vResult[i].SEQ + "\"> ";
                        vHTML += "   	<input type=\"hidden\" class=\"input_FileList_SetTime\" name=\"input_FileList_SetTime\" value=\"" + vResult[i].SETTIME + "\"> ";
                        vHTML += "   	<input type=\"hidden\" class=\"input_FileList_RealName\" name=\"input_FileList_RealName\" value=\"" + vResult[i].MNGT_NO + "_" + vResult[i].SEQ + "_" + vResult[i].FILE_NM + "\"> ";
                        vHTML += "   </div> ";

                        _fnsleep(50);
                    });


                    $("#layer_Upload_FileList").append(vHTML);
                    if ($("p[name=Layer_File_Download]").length > 0) {
                        $("p[name=Layer_File_Download]").eq(0).click();
                    } else {
                        $(".no_img_data").show();
                    }

                }
            }
        }
        else if (JSON.parse(vJsonData).Result[0]["trxCode"] == "N") {
            //test
            $(".no_img_data").show();
            console.log("[Fail - fnMakeSetFileList]" + JSON.parse(vJsonData).Result[0]["trxMsg"]);
        }
        else if (JSON.parse(vJsonData).Result[0]["trxCode"] == "E") {
            //test
            $(".no_img_data").show();
            console.log("[Error - fnMakeSetFileList]" + JSON.parse(vJsonData).Result[0]["trxMsg"]);
        }

        //UserType에 따라서 버튼 Area 보여줄지 말지 체크
        //if (_fnToNull($("#Session_USR_TYPE").val()) == "M") {
        //    $("#FileUploadBtn_Area").show();
        //}
        //else {
        //    $("#FileUploadBtn_Area").hide();
        //    $("a[name='Layer_FileList_Delete']").hide();
        //}

    }
    catch (err) {
        console.log("[Error - fnMakeSetFileList]" + err.message);
    }
}

function fnMakeConsoleList(vJsonData) {
    try {

        var vHTML = "";

        if (JSON.parse(vJsonData).Result[0]["trxCode"] == "Y") {
            _isSearch = true;
            var vResult = JSON.parse(vJsonData).Console;

            //데이터 반복문 - //PC
            $.each(vResult, function (i) {
                vHTML += "   <tr> ";
                vHTML += "   	<td class=\"txt txt--main font-weight-medium\">" + _fnToNull(vResult[i]["ACT_CUST_NM"]);
                if ((parseInt(vResult[i]["BK_SEQ"]) - 1) >= 1) {
                    vHTML += " 외 " + (parseInt(vResult[i]["BK_SEQ"]) - 1) + "건 " + "</td>";
                } else {
                     "</td>";
                }
                vHTML += "   	<td class=\"txt font-sub font-weight-medium\">" + _fnToNull(vResult[i]["BK_NO"]) + "</td>  ";
                vHTML += "   	<td class=\"txt font-sub font-weight-medium\">" + _fnToNull(vResult[i]["GR_NO"]);
                if ((parseInt(vResult[i]["BK_SEQ"]) - 1) >= 1) {
                    vHTML += " 외 " + (parseInt(vResult[i]["BK_SEQ"]) - 1) + "건 " + "</td>";
                } else {
                     "</td>";
                }
                vHTML += "   	<td class=\"txt font-sub font-weight-medium\">" + _fnToNull(vResult[i]["POD_CD"]) + " <br/> " + _fnToNull(vResult[i]["POD_NM"]) + "</td>  ";
                vHTML += "   	<td class=\"txt font-sub font-weight-medium\">" + String(_fnToNull(vResult[i]["GR_YMD"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3') + "</td>  ";
                vHTML += "   	<td class=\"txt font-sub font-weight-medium\">" + fnSetComma(_fnToNull(vResult[i]["ACT_QTY"])) + "</td> ";
                vHTML += "   	<td class=\"txt font-sub font-weight-medium\">" + fnSetComma(_fnToNull(vResult[i]["ACT_GRS_WGT"])) + "</td> ";
                vHTML += "   	<td class=\"txt font-sub font-weight-medium\">" + fnSetComma(_fnToNull(vResult[i]["ACT_MSRMT"])) + "</td> ";
                vHTML += "   	<td class=\"txt\"> ";
                vHTML += "   		<a href=\"javascript:void(0)\" name=\"layer_ConsoleData\" class=\"link\">상세</a> ";
                vHTML += "   		<input type=\"hidden\" name=\"layer_DetailMngtNo\" value=\"" + _fnToNull(vResult[i]["BK_NO"]) + "\" /> ";
                vHTML += "   		<input type=\"hidden\" name=\"layer_DetailSeq\" value=\"" + _fnToNull(vResult[i]["BK_SEQ"]) + "\" /> ";
                vHTML += "   	</td> ";
                vHTML += "   	<td></td> ";
                vHTML += "   </tr> ";
            });

            $("#Console_Result_AREA_PC")[0].innerHTML = vHTML;

            vHTML = "";

            //데이터 반복문 - MO
            $.each(vResult, function (i) {
                vHTML += " <ul class=\"info-box py-2 px-1\"> ";
                vHTML += "   <li class=\"info-txt info-txt--full\"> ";
                vHTML += "   	<p class=\"title font-main\">실화주명</p> ";
                vHTML += "   	<p class=\"txt des\">" + _fnToNull(vResult[i]["ACT_CUST_NM"]);
                if ((parseInt(vResult[i]["BK_SEQ"]) - 1) >= 1) {
                    vHTML += " 외 " + (parseInt(vResult[i]["BK_SEQ"]) - 1) + "건 " + "</p> ";
                } else {
                    "</p> ";
                }
                vHTML += "   </li> ";
                vHTML += "   <li class=\"info-txt info-txt--full\"> ";
                vHTML += "   	<p class=\"title font-main\">Booking</p> ";
                vHTML += "   	<p class=\"txt des\">" + _fnToNull(vResult[i]["BK_NO"]) + "</p> ";
                vHTML += "   </li> ";
                vHTML += "   <li class=\"info-txt info-txt--full\"> ";
                vHTML += "   	<p class=\"title font-main\">입고번호</p> ";
                vHTML += "   	<p class=\"des\">" + _fnToNull(vResult[i]["GR_NO"])
                if ((parseInt(vResult[i]["BK_SEQ"]) - 1) >= 1) {
                    vHTML += " 외 " + (parseInt(vResult[i]["BK_SEQ"]) - 1) + "건 " + "</p> ";
                } else {
                    "</p> ";
                }
                vHTML += "   </li> ";
                vHTML += "   <li class=\"info-txt info-txt--full\"> ";
                vHTML += "   	<p class=\"title\">POD</p> ";
                vHTML += "   	<p class=\"text-gray-6 common-text--14 des\">" + _fnToNull(vResult[i]["POD_CD"]) + " <br/> " + _fnToNull(vResult[i]["POD_NM"]) + "</p> ";
                vHTML += "   </li> ";
                vHTML += "   <li class=\"info-txt info-txt--full\"> ";
                vHTML += "   	<p class=\"title font-main\">입고일자</p> ";
                vHTML += "   	<p class=\"text-gray-6 common-text--14 des\">" + String(_fnToNull(vResult[i]["GR_YMD"])).replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3') + "</p> ";
                vHTML += "   </li> ";
                vHTML += "   <li class=\"info-txt info-txt--full\"> ";
                vHTML += "   	<p class=\"title font-main\">수량</p> ";
                vHTML += "   	<p class=\"text-gray-6 common-text--14 des\">" + fnSetComma(_fnToNull(vResult[i]["ACT_QTY"])) + "</p> ";
                vHTML += "   </li> ";
                vHTML += "   <li class=\"info-txt info-txt--full\"> ";
                vHTML += "   	<p class=\"title font-main\">중량</p> ";
                vHTML += "   	<p class=\"text-gray-6 common-text--14 des\">" + fnSetComma(_fnToNull(vResult[i]["ACT_GRS_WGT"])) + "</p> ";
                vHTML += "   </li> ";
                vHTML += "   <li class=\"info-txt info-txt--full\"> ";
                vHTML += "   	<p class=\"title font-main\">용적</p> ";
                vHTML += "   	<p class=\"text-gray-6 common-text--14 des\">" + fnSetComma(_fnToNull(vResult[i]["ACT_MSRMT"])) + "</p> ";
                vHTML += "   </li> ";
                vHTML += "   <li class=\"info-txt info-txt--full\"> ";
                vHTML += "   	<p class=\"title font-main\">상세보기</p> ";
                vHTML += "   	<p class=\"des\"> ";
                vHTML += "   		<a href=\"javascript:void(0)\" name=\"layer_ConsoleData\" class=\"link\"><i class=\"xi-plus\"></i></a> ";
                vHTML += "   		<input type=\"hidden\" name=\"layer_DetailMngtNo\" value=\"" + _fnToNull(vResult[i]["BK_NO"]) + "\" /> ";
                vHTML += "   		<input type=\"hidden\" name=\"layer_DetailSeq\" value=\"" + _fnToNull(vResult[i]["BK_SEQ"]) + "\" /> ";
                vHTML += "   	</p> ";
                vHTML += "   </li> ";
                vHTML += " </ul> ";
            });

            $("#Console_Result_AREA_MO")[0].innerHTML = vHTML;

            $("#Paging_Area").show();
        }
        else if (JSON.parse(vJsonData).Result[0]["trxCode"] == "N") {
            _isSearch = false;

            //PC
            vHTML += "   <tr> ";
            vHTML += "   	<td colspan=\"11\" class=\"no_data\"><span class=\"font-weight-medium\">데이터가 없습니다.</span></td> ";
            vHTML += "   </tr> ";

            $("#Console_Result_AREA_PC")[0].innerHTML = vHTML;

            vHTML = "";

            //MO
            vHTML += "   <ul class=\"info-box py-2 px-1\"> ";
            vHTML += "   	<li class=\"no_data col-12 py-6\"><span class=\"font-weight-medium\">데이터가 없습니다.</span></li> ";
            vHTML += "   </ul> ";

            $("#Console_Result_AREA_MO")[0].innerHTML = vHTML;

            $("#Paging_Area").hide();
        }
        else if (JSON.parse(vJsonData).Result[0]["trxCode"] == "E") {
            _isSearch = false;

            //PC
            vHTML += "   <tr> ";
            vHTML += "   	<td colspan=\"11\" class=\"no_data\"><span class=\"font-weight-medium\">관리자에게 문의하세요.</span></td> ";
            vHTML += "   </tr> ";

            $("#Console_Result_AREA_PC")[0].innerHTML = vHTML;

            vHTML = "";

            //MO
            vHTML += "   <ul class=\"info-box py-2 px-1\"> ";
            vHTML += "   	<li class=\"no_data col-12 py-6\"><span class=\"font-weight-medium\">관리자에게 문의하세요.</span></li> ";
            vHTML += "   </ul> ";

            $("#Console_Result_AREA_MO")[0].innerHTML = vHTML;
            $("#Paging_Area").hide();
        }
    }
    catch (err) {
        console.log("[Error - fnMakeConsoleList()]" + err.message);
    }
}

$(document).on("click", ".down_btn", function () {
    //$(".down_btn").click(function (e) {
    if ($("#now_file").text() != "" || $("#now_seq").text() != "") {
        //fnImageDown($("#now_file").text(), $("#now_seq").text());

        window.location = "/File/Console_DownloadFile?FILE_NM=" + $("#now_file").text() + "&REPLACE_FILE_NM=" + $("#now_seq").text();
    } else {
        _fnLayerAlertMsg("다운받을 파일을 선택해주세요");
    }
});

$(document).on("click", "#mfp_delete", function () {
    var FILE_NM = new Array();
    var objJsonData = new Object();
    $.each($("input[name=input_FileList_RealName]"), function (i) {
        objJsonData = new Object();
        objJsonData["FILE_NM"] = $("input[name=input_FileList_RealName]").eq(i).val();
        FILE_NM.push(objJsonData);
    });
    objJsonData = FILE_NM;
    $.ajax({
        type: "POST",
        url: "/File/Console_TempDelete",
        async: false,
        dataType: "json",
        data: { "vJsonData": _fnMakeJson(objJsonData) },
        success: function (result) {



        },
        error: {

        }

    })
    layerClose4('#Upload_pop');
});

$(document).on("click", ".mfp-close", function () {
    $(this).parents('.layer_zone').hide();
});


$('#camera').click(function (e) {
    $('.no_img_data').css("display", "none");
})
///*document.querySelector("")*/
//수출 - POD 클릭 (AutoComplete O)
$(document).on("click", "#input_POD", function () {

        $("#select_SEA_pop02").show();

    fnWareArrivePort();



});
//퀵 Code 데이터 - POL
$(document).on("click", "#select_SEA_pop02 button", function () {

    var vValue = $(this).val();
    var vSplit = $(this).text();

    $("#input_POD").val(vSplit);
    $("#input_PODCD").val(vValue);
    $("#select_SEA_pop02").hide();

    //X박스 만들기
    $(".pop__export").find(".delete-btn").show();


});

$(document).on("click", "#mfp_delete2", function () {
    var FILE_NM = new Array();
    var objJsonData = new Object();
    $.each($("input[name=input_FileList_RealName]"), function (i) {
        objJsonData = new Object();
        objJsonData["FILE_NM"] = $("input[name=input_FileList_RealName]").eq(i).val();
        FILE_NM.push(objJsonData);
    });
    objJsonData = FILE_NM;
    $.ajax({
        type: "POST",
        url: "/File/Console_TempDelete",
        async: false,
        dataType: "json",
        data: { "vJsonData": _fnMakeJson(objJsonData) },
        success: function (result) {



        },
        error: {

        }

    })
    layerClose('#Upload_pop');
});
function fnWareArrivePort() {
    var objJsonData = new Object();

    $.ajax({
        type: "POST",
        url: "/SSGM/fnWareArrivePort",
        async: false,
        dataType: "json",
        data: { "vJsonData": _fnMakeJson(objJsonData) },
        success: function (result) {
            fnMakeStartPort(result);


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



};

function fnMakeStartPort(vJsonData) {
    var vHTML = "";

    //2개 (PC하고 모바일하고 따로 만들어야 됨..)
    if (JSON.parse(vJsonData).Result[0]["trxCode"] == "Y") {

        var vResult = JSON.parse(vJsonData).StartPort;

        $.each(vResult, function (i) {

            // 첫 번째 항목이거나 BOUND_PORT 값이 변경된 경우를 확인합니다.
            if (i === 0 || _fnToNull(vResult[i]["BOUND_PORT"].trim()) != _fnToNull(vResult[i - 1]["BOUND_PORT"].trim())) {
                vHTML += "    <div class='location-list_all'> ";
                vHTML += "        <div class='continent'>" + _fnToNull(vResult[i]["BOUND_PORT"].trim()) + "</div> ";
                vHTML += "        <div class='location-list_detail'> ";
            }
            vHTML += "            <div class='location_nm'><button type='button' value='" + _fnToNull(vResult[i]["LOC_CD"]) + "'><span>" + _fnToNull(vResult[i]["LOC_NM"]) + "</span></button></div> ";
            // 마지막 항목이거나 다음 반복에서 BOUND_PORT 값이 변경될 경우 상위 div를 닫습니다.
            if (i === vResult.length - 1 || _fnToNull(vResult[i]["BOUND_PORT"].trim()) !== _fnToNull(vResult[i + 1]["BOUND_PORT"].trim())) {
                vHTML += "        </div> ";
                vHTML += "    </div> ";
            }
        });

        $("#ArrivePort")[0].innerHTML = vHTML;

        //모바일 화면 작업
        vHTML = "";
        var cnt;
        $.each(vResult, function (i) {
            // 첫 번째 항목이거나 BOUND_PORT 값이 변경된 경우를 확인합니다.
            if (i === 0 || vResult[i]["BOUND_PORT"].trim() !== vResult[i - 1]["BOUND_PORT"].trim()) {
                cnt = 0;
                vHTML += "<tr>";
                vHTML += "<th colspan='3'>" + _fnToNull(vResult[i]["BOUND_PORT"].trim()) + "</th>";
                vHTML += "</tr>";
                vHTML += "<tr>";
            }

            // 빈 값을 채워서 3칸을 만듭니다.
            var emptyData = { "LOC_CD": "", "LOC_NM": "" };

            // 데이터가 없는 경우 빈 값을 사용합니다.
            var currentData = vResult[i] || emptyData;
            var nextData = vResult[i + 1] || emptyData;
            cnt += 1;
            vHTML += "<td><button type='button' value='" + _fnToNull(currentData["LOC_CD"].trim()) + "'><span>" + _fnToNull(currentData["LOC_NM"].trim()) + "</span></button></td>";



            // 현재 항목이 섹션 내에서 마지막 항목이거나, 3의 배수일 때 행을 닫습니다.
            var isLastInSection = i === vResult.length - 1 || _fnToNull(vResult[i]["BOUND_PORT"].trim()) !== _fnToNull(vResult[i + 1]["BOUND_PORT"].trim());
            if ((cnt) % 3 === 0 || isLastInSection) {
                // 빈 값으로 나머지 칸을 채웁니다.

                for (var j = 0; j < 3 - (cnt) % 3; j++) {
                    if (cnt % 3 !== 0) {
                        vHTML += "<td></td>";
                    }
                }
                vHTML += "</tr>";
            }
        });


        $("#ArrivePort_Mo")[0].innerHTML = vHTML;


    }


}

$(document).on("click", ".MO_Layer_Console_Header_List", function () {
    /*$("#PC_Layer_Console_Detail tr").css("background-color", "#fff");*/
    $(".MO_Layer_Console_Header_List").css("background-color", "#fff");
    $(this).css("background-color", "#f5f5f5");
});
$(document).on("click", "#PC_Layer_Console_Header tr", function () {
    /*$("#PC_Layer_Console_Detail tr").css("background-color", "#fff");*/
    $("#PC_Layer_Console_Header tr").css("background-color", "#fff");
    $(this).css("background-color", "#f5f5f5");
});

