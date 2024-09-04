

$(function () {
    goSearch();


    $("#btn_search").click(function () {
        goSearch();
    });

    $("#stx").keyup(function (e) {
        if (e.keyCode == 13) {
            goSearch();
        }

    });
});


//$(document).on('click', '.file_btn', function () {


//    var cnt = $(this).children().children().length;
//    const agent = navigator.userAgent.toLowerCase();

//    if ((navigator.appName === 'Netscape' && agent.search('trident') !== -1) || (agent.indexOf('mise') !== -1)) {
//        alert("iE!");

//    }
//    else {
//        for (var i = 0; i < cnt; i++) {
//            window.location.href = ($(this).children().children().eq(i).prop('href'));
//            _fnsleep(1000);
//        }
//    }


//});



function goView(pageID) {
    location.href = "/LogisticsInfo/notice_view?id=" + pageID;
}

function goPage(pageIndex) {
    _fnSearchData(pageIndex);
}

function goSearch() {
    _fnSearchData(1);
}

function _fnSearchData(pageIndex) {

    var opt1 = $("#type option:selected").val();
    var opt = $("#sf1 option:selected").val();
    var txtVal = $("#stx").val();

    var rtnJson;
    var rtnVal;
    var callObj = new Object();

    callObj.Option = opt;
    callObj.Type = opt1;
    callObj.SearchText = txtVal;
    callObj.Page = pageIndex;

    $.ajax({
        type: "POST",
        url: "/LogisticsInfo/CallAjax",
        async: false,
        dataType: "json",
        data: callObj,
        success: function (result) {
            rtnVal = result;    //JSON.stringify(result);
        }, error: function (xhr) {
            console.log("시스템 사정으로 요청하신 작업을 처리할 수 없습니다.");
            $(".loading-imge").hide();
            console.log(xhr);
            return;
        }
    });

    $(".board_list tr").remove();

    var innerText = "";
    var innerPage = "";
    var totPageCnt;
    var maxPageCnt = 0;
    var nPage = 1;

    if (rtnVal == undefined) {
        innerText += "<tr> ";
        innerText += "<td class=\"no_data\" colspan=\"6\">등록된 게시물이 없습니다.</td>";
        innerText += "</tr> ";

        $(".paging-area").hide();
        // 삭제 해야함
        // fnPaging(0, 10, 5, 1);
    } else {
        $(rtnVal).each(function (i) {
            totPageCnt = rtnVal[i].TOTCNT;
            maxPageCnt = maxPageCnt + 1;
            nPage = rtnVal[i].PAGE;

            if (rtnVal[i].NOTICE_YN == "n") {
                innerText += "<tr> ";
                if (rtnVal[i].TYPE == 0) {
                    innerText += "	<td>공지사항</td> ";
                } else {
                    innerText += "	<td>스케줄</td> ";
                }
                innerText += "	<td class=\"text_left\"><a href='#' onclick='goView(" + rtnVal[i].NOTICE_ID + ")'>" + rtnVal[i].TITLE + "</a></td> ";
                innerText += "	<td>";
                if (!(_fnToNull(rtnVal[i].FILE_NAME) == "" && _fnToNull(rtnVal[i].FILE1_NAME) == "" && _fnToNull(rtnVal[i].FILE2_NAME) == "")) {
                    innerText += "      <button type=\"button\" class=\"file_btn\" onclick='goView(" + rtnVal[i].NOTICE_ID + ")'>";
                    innerText += "	        <i class='xi-file-o down_file'>";
                    //if (_fnToNull(rtnVal[i].FILE_NAME) != "") {
                    //    innerText += "	    <a href=\"/File/Download?filename=" + _fnToNull(rtnVal[i].FILE_NAME) + "&rFilename=" + _fnToNull(rtnVal[i].FILE) + "\" class=''></a>";
                    //}
                    //if (_fnToNull(rtnVal[i].FILE1_NAME) != "") {
                    //    innerText += "	    <a href=\"/File/Download?filename=" + _fnToNull(rtnVal[i].FILE1_NAME) + "&rFilename=" + _fnToNull(rtnVal[i].FILE1) + "\" class=''></a>";
                    //}
                    //if (_fnToNull(rtnVal[i].FILE2_NAME) != "") {
                    //    innerText += "	    <a href=\"/File/Download?filename=" + _fnToNull(rtnVal[i].FILE2_NAME) + "&rFilename=" + _fnToNull(rtnVal[i].FILE2) + "\" class=''></a>";
                    //}

                    innerText += "</i>";
                    //if (_fnToNull(rtnVal[i].FILE_NAME) != "") {
                    // //   innerText += "          <input type=\"hidden\" value = \"/File/Download?filename="+_fnToNull(rtnVal[i].FILE_NAME)+"&rFilename="+_fnToNull(rtnVal[i].FILE)+"\"/>";
                    //}

                    innerText += "      </button>";
                }
                innerText += "	</td>";
                innerText += "	<td>" + rtnVal[i].WRITER + "</td> ";
                innerText += "	<td>" + rtnVal[i].REGDT.substring(0, 10) + "</td> ";
                innerText += "	<td>" + rtnVal[i].CNT + "회</td> ";
                innerText += "</tr> ";
            } else {
                innerText += "<tr> ";
                /*                innerText += "	<td>" + rtnVal[i].NUM + "</td> ";*/
                if (rtnVal[i].TYPE == 0) {
                    innerText += "	<td>공지사항</td> ";
                } else {
                    innerText += "	<td>스케줄</td> ";
                }
                //popup-link
                //a_left 가운데 정렬
                innerText += "	<td class='text_left'><a href='#' onclick='goView(" + rtnVal[i].NOTICE_ID + ")'>" + rtnVal[i].TITLE + "</a></td> ";
                innerText += "	<td>";
                if (!(_fnToNull(rtnVal[i].FILE_NAME) == "" && _fnToNull(rtnVal[i].FILE1_NAME) == "" && _fnToNull(rtnVal[i].FILE2_NAME) == "")) {
                    innerText += "      <button type=\"button\" class=\"file_btn\" onclick='goView(" + rtnVal[i].NOTICE_ID + ")'>";
                    innerText += "	        <i class='xi-file-o down_file'>";
                    //if (_fnToNull(rtnVal[i].FILE_NAME) != "") {
                    //    innerText += "	    <a href=\"/File/Download?filename=" + _fnToNull(rtnVal[i].FILE_NAME) + "&rFilename=" + _fnToNull(rtnVal[i].FILE) + "\" class=''></a>";
                    //}
                    //if (_fnToNull(rtnVal[i].FILE1_NAME) != "") {
                    //    innerText += "	    <a href=\"/File/Download?filename=" + _fnToNull(rtnVal[i].FILE1_NAME) + "&rFilename=" + _fnToNull(rtnVal[i].FILE1) + "\" class=''></a>";
                    //}
                    //if (_fnToNull(rtnVal[i].FILE2_NAME) != "") {
                    //    innerText += "	    <a href=\"/File/Download?filename=" + _fnToNull(rtnVal[i].FILE2_NAME) + "&rFilename=" + _fnToNull(rtnVal[i].FILE2) + "\" class=''></a>";
                    //}

                    innerText += "          </i>";
                    innerText += "      </button>";
                }
                innerText += "	    </a>";
                innerText += "	</td>";
                innerText += "	<td>" + rtnVal[i].WRITER + "</td> ";
                innerText += "	<td>" + rtnVal[i].REGDT.substring(0, 10) + "</td> ";
                innerText += "	<td>" + rtnVal[i].CNT + "회</td> ";
                innerText += "</tr> ";
            }
        });

        fnPaging(totPageCnt, 10, 5, pageIndex);
    }



    $(".board_list").append(innerText);

}

//totalData = 총 데이터 count
//dataPerPage = 한페이지에 나타낼 데이터 수
// pageCount = 한화면에 나타낼 페이지 수
//currentPage = 선택한 페이지 
function fnPaging(totalData, dataPerPage, pageCount, currentPage) {

    var totalPage = Math.ceil(totalData / dataPerPage);    // 총 페이지 수
    var pageGroup = Math.ceil(currentPage / pageCount);    // 페이지 그룹
    if (pageCount > totalPage) pageCount = totalPage;
    var last = pageGroup * pageCount;    // 화면에 보여질 마지막 페이지 번호
    if (last > totalPage) last = totalPage;
    var first = last - (pageCount - 1);    // 화면에 보여질 첫번째 페이지 번호
    var next = last + 1;
    var prev = first - 1;

    $(".paging-area ul").remove();

    var prevPage;
    var nextPage;
    if (currentPage - 1 < 1) { prevPage = 1; } else { prevPage = currentPage - 1; }
    if (last < totalPage) { nextPage = currentPage + 1; } else { nextPage = last; }

    var html = "";

    html += "<ul> ";
    html += "	<li><a href='#' onclick='goPage(1)' class='prev-first'><span>맨앞으로</span></a></li>";
    html += "	<li><a href='#' onclick='goPage(" + prevPage + ")' class='prev'><span>이전으로</span></a></li>";
    //html += "	<span class='number'> ";

    for (var i = first; i <= last; i++) {

        if (i == currentPage) {
            //html += "		<span class='on'>" + i + "</span> ";
            html += "	<li><a href='javascript:void(0)' class='active'>" + i + "</a></li>";
        } else {
            //html += "		<a href='#' onclick='goPage(" + i + ")'>" + i + "</a> ";
            html += "	<li><a href='#' onclick='goPage(" + i + ")' class='prevt'>" + i + "</a></li>";
        }
    }

    //html += "	</span> ";
    html += "	<li><a href='#' onclick='goPage(" + nextPage + ")' class='next'><span>다음으로</span></a></li>";
    html += "	<li><a href='#' onclick='goPage(" + totalPage + ")' class='next-last'><span>맨뒤로</span></a></li>";
    //html += "	<a href='#' onclick='goPage(" + nextPage + ")' class='page next'><span class='blind'>다음페이지로 가기</span></a> ";
    //html += "	<a href='#' onclick='goPage(" + totalPage + ")' class='page last'><span class='blind'>마지막페이지로 가기</span></a> ";
    html += "</ul> ";

    $(".paging-area").append(html);    // 페이지 목록 생성
    $(".paging-area").show();
}