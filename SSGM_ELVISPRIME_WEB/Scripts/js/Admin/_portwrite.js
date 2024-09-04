////////////////////////전역 변수//////////////////////////
////var _vPage = 0;
var _objData = new Object(); //전역 변수

//////////////////////jquery event///////////////////////

$(function () {
	if ($("#select_Start").val() == "KRPUS") {
		$("#select_Bound_Busan").show();
		$("#select_Bound_Incheon").hide();
	} else {
		$("#select_Bound_Busan").hide();
		$("#select_Bound_Incheon").show();
	}
});


$(document).on("change", "#select_Start", function () {

	if ($("#select_Start").val() == "KRPUS") {
		$("#select_Bound_Busan").show();
		$("#select_Bound_Incheon").hide();
	} else {
		$("#select_Bound_Busan").hide();
		$("#select_Bound_Incheon").show();
	}
});


//관리자 관리 등록 클릭 이벤트
$(document).on("click", "#Insert_Port", function () {
	fnInsertPort();
});

//관리자 관리 저장 완료 후 페이지 이동
$(document).on("click", "#btn_save_complete_port", function () {
	location.href = "/Admin/Port";
});

//관리자 관리 저장 완료 후 페이지 이동
$(document).on("click", "#btn_save_complete_modify", function () {
	location.href = "/Admin/Port";
});

//관리자 관리 수정 버튼 이벤트
$(document).on("click", "#Modify_Port", function () {
	fnModifyPort();
});

//아이디 중복 체크
$(document).on("click", "#port_check", function () {
	fnCheckPort();
});


function fnSetStartPort() {

	var objJsonData = new Object();

	$.ajax({
		type: "POST",
		url: "/Admin/fnSetStartPort",
		async: false,
		cache: false,
		dataType: "Json",
		data: { "vJsonData": _fnMakeJson(objJsonData) },
		success: function (result) {
			//alert(result);
			fnMakeAddrState(result);
		}, error: function (xhr) {
			alert("시스템 사정으로 요청하신 작업을 처리할 수 없습니다.");
			console.log(xhr);
		}
	});

}

function fnSetBoundPort() {

	var objJsonData = new Object();

	$.ajax({
		type: "POST",
		url: "/Admin/fnSetBoundPort",
		async: false,
		cache: false,
		dataType: "Json",
		data: { "vJsonData": _fnMakeJson(objJsonData) },
		success: function (result) {
			//alert(result);
			fnMakeAddrState(result);
		}, error: function (xhr) {
			alert("시스템 사정으로 요청하신 작업을 처리할 수 없습니다.");
			console.log(xhr);
		}
	});

}

function fnSetUsePort() {

	var objJsonData = new Object();

	$.ajax({
		type: "POST",
		url: "/Admin/fnSetUsePort",
		async: false,
		cache: false,
		dataType: "Json",
		data: { "vJsonData": _fnMakeJson(objJsonData) },
		success: function (result) {
			//alert(result);
			fnMakeAddrState(result);
		}, error: function (xhr) {
			alert("시스템 사정으로 요청하신 작업을 처리할 수 없습니다.");
			console.log(xhr);
		}
	});

}
////////////////////////////function///////////////////////
//////관리자 관리 - 포트 중복 체크 로직
function fnCheckPort() {
	try {
		var objJsonData = new Object();

		//아이디가 없으면
		if (_fnToNull($("#select_PortCd").val()) == "") {
			_fnAlertMsg("도착지 포트 코드를 입력 해주세요.", "select_PortCd");
			return false;
        }

		objJsonData.START_PORT = _fnToNull($("#select_Start").val().toUpperCase()); //검색 어떤걸로 하는지.
		objJsonData.LOC_CD = _fnToNull($("#select_PortCd").val().toUpperCase()); //검색 어떤걸로 하는지.

		$.ajax({
			type: "POST",
			url: "/Admin/fnCheckPort",
			async: true,
			cache: false,
			dataType: "Json",
			data: { "vJsonData": _fnMakeJson(objJsonData) },
			success: function (result) {
				if (JSON.parse(result).Result[0]["trxCode"] == "Y")
				{
					_fnAlertMsg("사용 가능한 포트 코드 입니다.");
					_objData.CheckID = true;
                }
				else if (JSON.parse(result).Result[0]["trxCode"] == "N") {
					_fnAlertMsg("이미 사용중인 포트 코드 입니다.");
				} else if (JSON.parse(result).Result[0]["trxCode"] == "E") {
					_fnAlertMsg("담당자에게 문의하세요");
					console.log("[Error - fnCheckIDMember]" + JSON.parse(result).Result[0]["trxMsg"]);
				}
			}, error: function (xhr) {
				$("#ProgressBar_Loading").hide(); //프로그래스 바
				alert("시스템 사정으로 요청하신 작업을 처리할 수 없습니다.");
				console.log(xhr);
			},
			beforeSend: function () {
				$("#ProgressBar_Loading").show(); //프로그래스 바
			},
			complete: function () {
				$("#ProgressBar_Loading").hide(); //프로그래스 바
			}
		});
	} catch (err) {
		console.log("[Error - fnCheckPort]" + err.message);
	}
}
//관리자 관리 - 포트 등록
function fnInsertPort() {
	try {
		if (fnInsertValidation()) {
			var objJsonData = new Object();
			objJsonData.START_PORT = _fnToNull($("#select_Start").find("option:selected").val());
			if (objJsonData.START_PORT == "KRPUS") {
				objJsonData.BOUND_PORT = _fnToNull($("#select_Bound_Busan").find("option:selected").val());
			} else {
				objJsonData.BOUND_PORT = _fnToNull($("#select_Bound_Incheon").find("option:selected").val());
            }
			objJsonData.LOC_CD = _fnToNull($("#select_PortCd").val().toUpperCase());
			objJsonData.LOC_NM = _fnToNull($("#select_PortNm").val().toUpperCase());
			objJsonData.USE_YN = _fnToNull($("#select_Use").find("option:selected").val());
			objJsonData.BKG_YN = _fnToNull($("#select_bkg").find("option:selected").val());
			objJsonData.INS_USR = _fnToNull($("#Session_USR_ID").val());
			$.ajax({
				type: "POST",
				url: "/Admin/fnInsertPort",
				async: true,
				cache: false,
				dataType: "Json",
				data: { "vJsonData": _fnMakeJson(objJsonData) },
				success: function (result) {
					if (JSON.parse(result).Result[0]["trxCode"] == "Y") {
						fnCompleteAlert_port("등록 되었습니다.");
					}
					else if (JSON.parse(result).Result[0]["trxCode"] == "N") {
						_fnAlertMsg("등록 되지 않았습니다.");
						console.log("[Fail - fnInsertMember]" + JSON.parse(result).Result[0]["trxMsg"]);
					}
					else if (JSON.parse(result).Result[0]["trxCode"] == "E") {
						_fnAlertMsg("담당자에게 문의하세요.");
						console.log("[Error - fnInsertMember]" + JSON.parse(result).Result[0]["trxMsg"]);
					}
					else if (JSON.parse(result).Result[0]["trxCode"] == "S") {
						_fnAlertMsg("중복된 포트입니다.");
						console.log("[Error - fnInsertMember]" + JSON.parse(result).Result[0]["trxMsg"]);
					}
				}, error: function (xhr) {
					$("#ProgressBar_Loading").hide(); //프로그래스 바
					_fnAlertMsg("시스템 사정으로 요청하신 작업을 처리할 수 없습니다.");
					console.log(xhr);
				},
				beforeSend: function () {
					$("#ProgressBar_Loading").show(); //프로그래스 바
				},
				complete: function () {
					$("#ProgressBar_Loading").hide(); //프로그래스 바
				}
			});
        }
	
	} catch (err) {
		console.log("[Error - fnCheckEmailMember]" + err.message);
	}
}

//////포트 관리 - 포트 수정
function fnModifyPort() {
	try {
		if (fnModifyValidation()) {
			var objJsonData = new Object();
			objJsonData.PORT_NO = _fnToNull($("#View_PORT_NO").val());
			objJsonData.START_PORT = _fnToNull($("#select_Start").find("option:selected").val());
			if (objJsonData.START_PORT == "KRPUS") {
				objJsonData.BOUND_PORT = _fnToNull($("#select_Bound_Busan").find("option:selected").val());
			} else {
				objJsonData.BOUND_PORT = _fnToNull($("#select_Bound_Incheon").find("option:selected").val());
			}
			objJsonData.LOC_CD = _fnToNull($("#select_PortCd").val().toUpperCase());
			objJsonData.LOC_NM = _fnToNull($("#select_PortNm").val().toUpperCase());
			objJsonData.USE_YN = _fnToNull($("#select_Use").find("option:selected").val());
			objJsonData.BKG_YN = _fnToNull($("#select_bkg").find("option:selected").val());
			objJsonData.UPD_USR = _fnToNull($("#Session_USR_ID").val());

			$.ajax({
				type: "POST",
				url: "/Admin/fnModifyPort",
				async: true,
				cache: false,
				dataType: "Json",
				data: { "vJsonData": _fnMakeJson(objJsonData) },
				success: function (result) {
					if (JSON.parse(result).Result[0]["trxCode"] == "Y") {
						fnCompleteAlert_modify("수정 되었습니다.");
					}
					else if (JSON.parse(result).Result[0]["trxCode"] == "N") {
						_fnAlertMsg("수정 되지 않았습니다.");
						console.log("[Fail - fnCheckEmailMember]" + JSON.parse(result).Result[0]["trxMsg"]);
					}
					else if (JSON.parse(result).Result[0]["trxCode"] == "E") {
						_fnAlertMsg("담당자에게 문의하세요.");
						console.log("[Error - fnCheckEmailMember]" + JSON.parse(result).Result[0]["trxMsg"]);
					}
				}, error: function (xhr) {
					$("#ProgressBar_Loading").hide(); //프로그래스 바
					alert("시스템 사정으로 요청하신 작업을 처리할 수 없습니다.");
					console.log(xhr);
				},
				beforeSend: function () {
					$("#ProgressBar_Loading").show(); //프로그래스 바
				},
				complete: function () {
					$("#ProgressBar_Loading").hide(); //프로그래스 바
				}
			});
		}
	}catch (err) {
		console.log("[Error - fnCheckEmailMember]" + err.message);
		}

}

//관리자 삭제 함수
function fnDeleteMember() {
	try {

		var objJsonData = new Object();
		objJsonData.MEMB_NO = _fnToNull($("#memb_no").val());

		$.ajax({
			type: "POST",
			url: "/Admin/fnDeleteMember",
			async: true,
			cache: false,
			dataType: "Json",
			data: { "vJsonData": _fnMakeJson(objJsonData) },
			success: function (result) {
				if (JSON.parse(result).Result[0]["trxCode"] == "Y") {
					fnCompleteAlert("삭제 되었습니다.");
				}
				else if (JSON.parse(result).Result[0]["trxCode"] == "N") {
					_fnAlertMsg("삭제 되지 않았습니다.");
					console.log("[Fail - fnDeleteMember]" + JSON.parse(result).Result[0]["trxMsg"]);
				}
				else if (JSON.parse(result).Result[0]["trxCode"] == "E") {
					_fnAlertMsg("담당자에게 문의하세요.");
					console.log("[Error - fnDeleteMember]" + JSON.parse(result).Result[0]["trxMsg"]);
				}

			}, error: function (xhr) {
				_fnAlertMsg("시스템 사정으로 요청하신 작업을 처리할 수 없습니다.");
				console.log(xhr);
			}
		});

	}
	catch (err) {
		console.log("[Error - fnDeleteMember]" + err.message);
	}
}

//완료 후 alert창
function fnCompleteAlert(msg) {
	$("#layer_complete_alert .inner").html(msg);
	layerPopup2('#layer_complete_alert');
	$("#btn_save_complete").focus();
}

//완료 후 alert창
function fnCompleteAlert_modify(msg) {
	$("#layer_complete_alert_modify .inner").html(msg);
	layerPopup2('#layer_complete_alert_modify');
	$("#btn_save_complete_modify").focus();
}

//완료 후 alert창
function fnCompleteAlert_port(msg) {
	$("#layer_complete_alert_port .inner").html(msg);
	layerPopup2('#layer_complete_alert_port');
	$("#btn_save_complete_port").focus();
}

//confirm 레이어 팝업 띄우기
function fnMember_Confirm(msg) {
	$("#Member_Confirm .inner").html(msg);
	layerPopup2('#Member_Confirm');
	$("#Layer_Confirm").focus();
}

//////관리자 관리 - 관리자 수정
//////function fnUpdateMember() {
//////	try {
//////		var objJsonData = new Object();
//////		objJsonData.EMAIL = $("#input_Email").val(); //검색 어떤걸로 하는지.		

//////		$.ajax({
//////			type: "POST",
//////			url: "/Admin/fnCheckEmailMember",
//////			async: true,
//////			cache: false,
//////			dataType: "Json",
//////			data: { "vJsonData": _fnMakeJson(objJsonData) },
//////			success: function (result) {
//////				fnMakeMember(result);
//////				if (JSON.parse(result).Result[0]["trxCode"] == "Y") {
//////					fnPaging(JSON.parse(result).Member[0]["TOTCNT"], 10, 10, objJsonData.PAGE);
//////				}
//////			}, error: function (xhr) {
//////				$("#ProgressBar_Loading").hide(); //프로그래스 바
//////				alert("시스템 사정으로 요청하신 작업을 처리할 수 없습니다.");
//////				console.log(xhr);
//////			},
//////			beforeSend: function () {
//////				$("#ProgressBar_Loading").show(); //프로그래스 바
//////			},
//////			complete: function () {
//////				$("#ProgressBar_Loading").hide(); //프로그래스 바
//////			}
//////		});
//////	} catch (err) {
//////		console.log("[Error - fnCheckEmailMember]" + err.message);
//////	}
//////}

//등록 밸리데이션
function fnInsertValidation() {

	//아이디 밸리데이션
	if (_fnToNull($("#select_PortCd").val()) == "") {
		_fnAlertMsg("도착지 포트 코드를 입력 해 주세요.", "select_PortCd");
		return false;
	}	

	//비밀번호 밸리데이션
	if (_fnToNull($("#select_PortNm").val()) == "") {
		_fnAlertMsg("도착지 포트 명을 입력 해 주세요.", "select_PortNm");
		return false;
	}


	//아이디 중복 확인 체크
	if (!_objData.CheckID) {
		_fnAlertMsg("도착지 포트 코드 중복 체크를 해 주세요.", "select_PortCd");
		return false;
	}

	return true;
}

//등록 밸리데이션
function fnModifyValidation() {

	//비밀번호 밸리데이션
	if (_fnToNull($("#select_PortNm").val()) == "") {
		_fnAlertMsg("도착지 포트 명을 입력 해 주세요.", "select_PortNm");
		return false;
	}


	return true;
}

////////검색
//////function fnMemberSearch() {
//////
//////	try {
//////
//////		var objJsonData = new Object();
//////		objJsonData.MEMBER_DIV = $("#select_Member_div").find('option:selected').val(); //검색 어떤걸로 하는지.
//////		objJsonData.INPUT_SEARCH = _fnToNull($("#input_Search").val());
//////
//////		if (_vPage == 0) {
//////			objJsonData.PAGE = 1;
//////		} else {
//////			objJsonData.PAGE = _vPage;
//////		}
//////
//////		_vPage++;
//////
//////		$.ajax({
//////			type: "POST",
//////			url: "/Admin/fnSearchMember",
//////			async: true,
//////			cache: false,
//////			dataType: "Json",
//////			data: { "vJsonData": _fnMakeJson(objJsonData) },
//////			success: function (result) {
//////				fnMakeMember(result);
//////				if (JSON.parse(result).Result[0]["trxCode"] == "Y") {
//////					fnPaging(JSON.parse(result).Member[0]["TOTCNT"], 10, 10, objJsonData.PAGE);
//////				}
//////			}, error: function (xhr) {
//////				$("#ProgressBar_Loading").hide(); //프로그래스 바
//////				alert("시스템 사정으로 요청하신 작업을 처리할 수 없습니다.");
//////				console.log(xhr);
//////			},
//////			beforeSend: function () {
//////				$("#ProgressBar_Loading").show(); //프로그래스 바
//////			},
//////			complete: function () {
//////				$("#ProgressBar_Loading").hide(); //프로그래스 바
//////			}
//////		});
//////	} catch (err) {
//////		console.log("[Error - fnMemberSearch]" + err.message);
//////    }
//////}



/////////////////////function MakeList/////////////////////
////////관리자 검색 데이터 그리기
//////function fnMakeMember(vJsonData) {
//////	var vHTML = "";
//////	vResult = JSON.parse(vJsonData).Member;
//////
//////	if (JSON.parse(vJsonData).Result[0]["trxCode"] == "Y") {
//////
//////		$.each(vResult, function (i) {
//////
//////			vHTML += "   <tr> ";
//////			vHTML += "   	<td style=\"display:none\">" + _fnToNull(vResult[i]["USR_ID"]) + "</td> ";			
//////
//////			vHTML += "   	<td>" + _fnToNull(vResult[i]["USR_ID"]) + "</td> ";
//////			vHTML += "   	<td>" + _fnToNull(vResult[i]["LOC_NM"]) + "</td> ";
//////			vHTML += "   	<td>" + _fnToNull(vResult[i]["HP_NO"]) + "</td> ";
//////			vHTML += "   	<td>" + _fnToNull(vResult[i]["CUST_NM"]) + "</td> ";
//////			vHTML += "   	<td>" + _fnToNull(vResult[i]["USR_TYPE"]) + "</td> ";
//////			vHTML += "   	<td>" + _fnToNull(vResult[i]["AUTH_TYPE"]) + "</td> ";
//////			vHTML += "   	<td>" + _fnFormatDate(_fnToNull(vResult[i]["INS_YMD"])) + "</td> ";
//////			vHTML += "   	<td> ";
//////			vHTML += "   		<div class=\"btn-group btn_padding\" role=\"group\"> ";
//////			vHTML += "   			<a href=\"javascript:void(0);\" type=\"button\" name=\"Member_Modify\" class=\"btn btn-primary pull-right _btn_modify\"><i class=\"fa fa-pencil-square-o\"></i>&nbsp;수정</a>			 ";
//////			vHTML += "   		</div> ";
//////			vHTML += "   		<div class=\"btn-group\" role=\"group\" aria-label=\"버튼\"> ";
//////			vHTML += "   			<a href=\"javascript:void(0);\" type=\"button\" name=\"Member_Delete\" class=\"btn btn-primary pull-right _btn_delete\"><i class=\"fa fa-th-list\"></i>&nbsp;삭제</a> ";
//////			vHTML += "   		</div> ";
//////			vHTML += "   	</td> ";
//////			vHTML += "   </tr> ";
//////		});
//////	}
//////	else if (JSON.parse(vJsonData).Result[0]["trxCode"] == "N") {
//////		vHTML += "   <tr> ";
//////		vHTML += "   	<td colspan=\"8\">데이터가 없습니다.</td> ";
//////		vHTML += "   </tr> ";
//////		console.log("[Fail - fnMakeMember] : " + JSON.parse(vJsonData).Result[0]["trxMsg"]);
//////	}
//////	else if (JSON.parse(vJsonData).Result[0]["trxCode"] == "E") {
//////		console.log("[Error - fnMakeMember] : " + JSON.parse(vJsonData).Result[0]["trxMsg"]);
//////	}
//////
//////	$("#Member_Result")[0].innerHTML = vHTML;
//////} 


////////////////////////////API////////////////////////////

