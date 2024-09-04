////////////////////////전역 변수//////////////////////////
var _vPage = 0;
////////////////////////jquery event///////////////////////
$(function () {
	fnSearchPort();
});

$('input').on('keydown', function (e) {
	if (e.which === 13) { // 13은 엔터키를 의미합니다.
		if (this.id === 'input_Port') {
			$('#Port_Search').click();
		}
	}
});




//관리자 등록 버튼
$(document).on("click", "#btn_PortWrite", function () {
	location.href = "/Admin/PortWrite";
});

//검색 버튼
$(document).on("click", "#Port_Search", function () {
	_vPage = 0;
	fnSearchPort();
});

$(document).on("click", "a[name='Port_Modify']", function () {

	goView($(this).siblings("input[name='Port_Mngt']").val());

})

$(document).on("click", "a[name='Port_Delete']", function () {
	var Del_mngt = $(this).siblings("input[name='Port_Del_Mngt']").val();

	fnPort_Confirm("삭제 하시겠습니까?", Del_mngt);
})


$(document).on("click", "#Layer_Confirm_port", function () {

	try {
		var objJsonData = new Object();
		
		objJsonData.PORT_NO = $(this).siblings("#Del_Mngt_No").val();

		$.ajax({
			type: "POST",
			url: "/Admin/fnDeletePort",
			dataType: "json",
			data: { "vJsonData": _fnMakeJson(objJsonData) },
			success: function (result) {

				_vPage = 0;
				fnSearchPort();

			}, error: function (xhr, status, error) {
				alert("담당자에게 문의 하세요.");
				console.log(error);
				vReturn = false;
			}


		})
	}
	catch (err) {
		console.log("[Error - fnDeletePort]" + err.message);
	}

})
////////////////////////////function///////////////////////
//관리자 검색
function fnSearchPort() {

	try {
		var objJsonData = new Object();

		objJsonData.SEARCH_TYPE = $("#select_Port option:selected").val(); //검색 어떤걸로 하는지.
		objJsonData.SEARCH_DATA = $("#input_Port").val(); //검색 어떤걸로 하는지.

		if (_vPage == 0) {
			objJsonData.PAGE = 1;
		} else {
			objJsonData.PAGE = _vPage;
		}

		_vPage++;

		$.ajax({
			type: "POST",
			url: "/Admin/fnSearchPort",
			async: true,
			cache: false,
			dataType: "Json",
			data: { "vJsonData": _fnMakeJson(objJsonData) },
			success: function (result) {
				fnMakePortList(result);
				if (JSON.parse(result).Result[0]["trxCode"] == "Y") {
					fnPaging(JSON.parse(result).PORT[0]["TOTCNT"], 10, 10, objJsonData.PAGE);
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
		//SelectBox
		//검색

	}
	catch (err) {
		console.log("[Error - fnSearchMember]" + err.message);
	}

}

//페이징 검색
function goPage(vPage) {
	_vPage = vPage;
	fnSearchPort();
}

//totalData = 총 데이터 count
//dataPerPage = 한페이지에 나타낼 데이터 수
//pageCount = 한화면에 나타낼 페이지 수
//currentPage = 선택한 페이지 
//공지사항 페이징
function fnPaging(totalData, dataPerPage, pageCount, currentPage) {

	try {
		var totalPage = Math.ceil(totalData / dataPerPage);    // 총 페이지 수
		var pageGroup = Math.ceil(currentPage / pageCount);    // 페이지 그룹
		if (pageCount > totalPage) pageCount = totalPage;
		var last = pageGroup * pageCount;    // 화면에 보여질 마지막 페이지 번호
		if (last > totalPage) last = totalPage;
		var first = last - (pageCount - 1);    // 화면에 보여질 첫번째 페이지 번호
		var next = last + 1;
		var prev = first - 1;

		$("#paging_Area").empty();

		var prevPage;
		var nextPage;
		if (currentPage - 1 < 1) { prevPage = 1; } else { prevPage = currentPage - 1; }
		if (last < totalPage) { nextPage = currentPage + 1; } else { nextPage = last; }

		var vHTML = "";

		vHTML += " <li><a href=\"javascript:void(0);\" onclick=\"goPage(1)\"><i class=\"fa fa-angle-double-left\"></i><span class=\"sr-only\">처음페이지로 가기</span></a></li> ";
		vHTML += " <li><a href=\"javascript:void(0);\" onclick=\"goPage(" + prevPage + ")\"><i class=\"fa fa-angle-left\"></i><span class=\"sr-only\">이전페이지로 가기</span></a></li> ";

		for (var i = first; i <= last; i++) {
			if (i == currentPage) {
				vHTML += " <li class=\"active\"><a href=\"javascript:void(0);\"onclick=\"goPage(" + i + ")\" >" + i + "</a></li> ";
			} else {
				vHTML += " <li><a href=\"javascript:void(0);\"onclick=\"goPage(" + i + ")\" >" + i + "</a></li> ";
			}
		}

		vHTML += " <li><a href=\"javascript:void(0);\" onclick=\"goPage(" + nextPage + ")\"><i class=\"fa fa-angle-right\"></i><span class=\"sr-only\">다음페이지로 가기</span></a></li> ";
		vHTML += " <li><a href=\"javascript:void(0);\" onclick=\"goPage(" + totalPage + ")\"><i class=\"fa fa-angle-double-right\"></i><span class=\"sr-only\">마지막페이지로 가기</span></a></li> ";
		$("#paging_Area").append(vHTML);    // 페이지 목록 생성		
	} catch (err) {
		console.log("[Error - fnPaging]" + err.message);
	}
}

function goView(PortNo) {
	location.href = "/Admin/PortWrite?Port_no=" + PortNo;
}

/////////////////////function MakeList/////////////////////
//관리자 리스트 만들기
function fnMakePortList(vJsonData) {

	try {
		var vHTML = "";

		var vResult = JSON.parse(vJsonData).PORT;

		if (JSON.parse(vJsonData).Result[0]["trxCode"] == "Y") {

			$.each(vResult, function (i) {
				vHTML += "   <tr> "; 
				vHTML += "   	<td>" + _fnToNull(vResult[i]["RNUM"]) + "</td> ";
				vHTML += "   	<td>" + _fnToNull(vResult[i]["START_PORT"]) + "</td> ";
				vHTML += "   	<td>" + _fnToNull(vResult[i]["BOUND_PORT"]) + "</td> ";
				vHTML += "   	<td>" + _fnToNull(vResult[i]["LOC_NM"]) + "</td> ";
				vHTML += "   	<td>" + _fnToNull(vResult[i]["LOC_CD"]) + "</td> ";
				vHTML += "   	<td>" + _fnToNull(vResult[i]["USE_YN"]) + "</td> ";
				vHTML += "   	<td>" + _fnToNull(vResult[i]["BKG_YN"]) + "</td> ";
				vHTML += "   	<td>" + _fnFormatDate(_fnToNull(vResult[i]["INS_YMD"])) + "</td> ";
				vHTML += "   	<td> ";
				vHTML += "   		<div class=\"btn-group btn_padding\" role=\"group\" aria-label=\"버튼\"> ";
				vHTML += "   			<a href=\"javascript:void(0);\" type=\"button\" name=\"Port_Modify\" class=\"btn btn-primary pull-right _btn_modify\"><i class=\"fa fa-pencil-square-o\"></i>&nbsp;수정</a> ";
				vHTML += "   		<input type='hidden' name='Port_Mngt' value='" + _fnToNull(vResult[i]["PORT_NO"]) + "'> ";
				vHTML += "   		</div> ";
				vHTML += "   		<div class=\"btn-group\" role=\"group\" aria-label=\"버튼\"> ";
				vHTML += "   			<a href=\"javascript:void(0);\" type=\"button\" name=\"Port_Delete\" class=\"btn btn-primary pull-right _btn_delete\"><i class=\"fa fa-th-list\"></i>&nbsp;삭제</a> ";
				vHTML += "   		<input type='hidden' name='Port_Del_Mngt' value='" + _fnToNull(vResult[i]["PORT_NO"]) + "'> ";
				vHTML += "   		</div> ";
				vHTML += "   	</td> ";
				vHTML += "   </tr> ";
			});
		}
		else if (JSON.parse(vJsonData).Result[0]["trxCode"] == "N") {
			$("#paging_Area").empty();
			vHTML += "   <tr> ";
			vHTML += "   	<td colspan=\"8\">데이터가 없습니다.</td> ";
			vHTML += "   </tr> ";
			console.log("[Fail - fnMakeMemberList]" + JSON.parse(vJsonData).Result[0]["trxMsg"]);
		}
		else if (JSON.parse(vJsonData).Result[0]["trxCode"] == "E") {
			$("#paging_Area").empty();
			vHTML += "   <tr> ";
			vHTML += "   	<td colspan=\"\">관리자에게 문의하세요.</td> ";
			vHTML += "   </tr> ";
			console.log("[Error - fnMakeMemberList]" + JSON.parse(vJsonData).Result[0]["trxMsg"]);
		}

		$("#Port_Result")[0].innerHTML = vHTML;
	}
	catch (err) {
		console.log("[Error - fnMakeMemberList]" + err.message);
	}
}
//confirm 레이어 팝업 띄우기
function fnPort_Confirm(msg,mngt_no) {
	$("#Port_Confirm .inner").html(msg);
	layerPopup2('#Port_Confirm');
	$("#Del_Mngt_No").val(mngt_no);
	$("#Layer_Confirm_port").focus();
}

////////////////////////////API////////////////////////////







