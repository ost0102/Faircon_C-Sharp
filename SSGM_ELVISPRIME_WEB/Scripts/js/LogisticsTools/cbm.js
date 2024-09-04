$(document).on('click', '#hamberger', function () {
    $('.total_menu.on .lnb > li:nth-child(4) .sub_depth').addClass('on');
});

var cntCal = 0;
$("#btn_calc_cbm").click(function () {
    fnSetCBM();
});

////////////////////////function///////////////////////
//숫자를 제외한 나머지 replace
function fnSetOnlyNum(vValue) {
    var vValue_Num = vValue.replace(/[^0-9]/g, "");
    return vValue_Num;
}
function fnSetCBM() {


    var cbm_vwgt = 0;
    var cbm_cwgt = 0;
    var cbm_calcul = 0;
    //단위 체크
    vCheckUnit = $("#CBM_UNIT option:selected").val();


    var vCBM_length = $("#cbm_length").val().replace(/,/gi, '');
    var vCBM_width = $("#cbm_width").val().replace(/,/gi, '');
    var vCBM_height = $("#cbm_height").val().replace(/,/gi, '');
    var vCBM_grs_wgt = $("#cbm_grs_wgt").val().replace(/,/gi, '');
    if (_fnToNull(vCBM_length) == "") {
        _fnAlertMsg("가로를 입력해주세요");
        return false;
    }
    if (_fnToNull(vCBM_width) == "") {
        _fnAlertMsg("세로를 입력해주세요");
        return false;
    }
    if (_fnToNull(vCBM_height) == "") {
        _fnAlertMsg("높이를 입력해주세요");
        return false;
    }
    if (_fnToNull(vCheckUnit) == "") {
        _fnAlertMsg("단위를 선택해주세요");
        return false;
    }
    if (_fnToNull(vCBM_grs_wgt) == "") {
        _fnAlertMsg("무게를 입력해주세요");
        return false;
    }

    if (vCheckUnit == "MM") {
        cbm_vwgt = (vCBM_length * vCBM_width * vCBM_height) / 6000000;
        cbm_calcul = (vCBM_length * vCBM_width * vCBM_height) / 1000000000;
    }
    else if (vCheckUnit == "CM") {
        cbm_vwgt = (vCBM_length * vCBM_width * vCBM_height) / 6000;
        cbm_calcul = (vCBM_length * vCBM_width * vCBM_height) / 1000000;
    }
    else if (vCheckUnit == "M") {
        cbm_vwgt = (vCBM_length * vCBM_width * vCBM_height) / 6 * 1000;
        cbm_calcul = (vCBM_length * vCBM_width * vCBM_height);

    } else if (vCheckUnit == "INCH") {
        cbm_vwgt = (vCBM_length / 39.37) * (vCBM_width / 39.37) * (vCBM_height / 39.37) / 6000 * 1000000;
        cbm_calcul = (vCBM_length / 39.37) * (vCBM_width / 39.37) * (vCBM_height / 39.37);
    }

    if (vCBM_grs_wgt >= cbm_vwgt) {
        cbm_cwgt = vCBM_grs_wgt;
    } else {
        cbm_cwgt = cbm_vwgt;
    }

    cbm_vwgt = cbm_vwgt * 100
    var vcbm_vwgt = Math.ceil(cbm_vwgt);
    var result1 = vcbm_vwgt / 100;
    result1 = result1.toFixed(3);


    cbm_cwgt = cbm_cwgt * 100
    var vcbm_cwgt = Math.floor(cbm_cwgt);
    var result2 = vcbm_cwgt / 100;
    result2 = result2.toFixed(3);


    cbm_calcul = cbm_calcul * 1000
    var vcbm_cacul = Math.floor(cbm_calcul);
    var result3 = vcbm_cacul / 1000;
    result3 = result3.toFixed(3);

    cntCal += 1;

    var apdVal = "";

    apdVal += " <div class='cbm_list'>	 ";
    apdVal += "               <div class='cbm_desc'> ";
    apdVal += "                 <div class='cbm_info_box'>";
    apdVal += "                   <div class='cbm_info'> ";
    apdVal += "                       <div class='cbm_unit'>가로 : <span>" + $("#cbm_length").val() + " </span></div> ";
    apdVal += "                       <div class='cbm_unit'>세로 : <span>" + $("#cbm_width").val() + " </span></div> ";
    apdVal += "                       <div class='cbm_unit'>높이 : <span>" + $("#cbm_height").val() + " </span></div> ";
    apdVal += "                       <div class='cbm_unit'>단위 : <span>" + $("#CBM_UNIT option:selected").text() + " </span></div> ";
    apdVal += "                       <div class='cbm_unit'>무게 : <span>" + $("#cbm_grs_wgt").val() + " </span></div> ";
    apdVal += "                   </div> ";
    apdVal += "                   <button type='button' class='btn_delete'>삭제<img src='/Images/icn_sm_delete.png'/></button> ";
    apdVal += "                 </div> ";
    apdVal += "                 <div class='cbm_detail'> ";
    apdVal += "                   <div class='cbm_cal'>";
    apdVal += "                       <span class='cbm_h'>V/WT(kg)</span>";
    apdVal += "                       <span class='rst_vol'>" + result1 + " <span class='cbm-unit'>kg</span></span>";
    apdVal += "                   </div>";
    apdVal += "                   <div class='cbm_cal'>";
    apdVal += "                       <span class='cbm_h'>C/WT(kg)</span >";
    apdVal += "                       <span class='rst_chr'>" + result2 + " <span class='cbm-unit'>kg</span></span>";
    apdVal += "                   </div>";
    apdVal += "                   <div class='cbm_cal'>";
    apdVal += "                       <span class='cbm_h'>CBM</span>";
    apdVal += "                       <span class='rst_cbm'>" + result3 + " <span class='cbm-unit'>CBM</span></span>";
    apdVal += "                   </div>";
    apdVal += "                   <div class='cbm_cal'>";
    apdVal += "                       <span class='cbm_h'>R/T(운임톤)</span>";
    apdVal += "                       <span class='rst_rton'>" + result3 + " <span class='cbm-unit'>CBM</span></span>";
    apdVal += "                   </div>";
    apdVal += "                 </div> ";
    apdVal += "               </div> ";
    apdVal += "           </div>";

    $(".scrollbar_cbm").append(apdVal);

    $(".one > input").val("");
    $(".one > .delete").hide();
    $('#CBM_UNIT').val('').prop("selected", true);

    var tot_vWgt = 0;
    var tot_cWgt = 0;
    var tot_cbm = 0;
    var tot_rton = 0;
    $(".rst_vol").each(function (i) {
        tot_vWgt += parseFloat($(this).text());

    });

    $("#tot_vWgt").text(tot_vWgt.toFixed(3));

    $(".rst_chr").each(function (i) {
        tot_cWgt += parseFloat($(this).text());

    });

    $(".rst_cbm").each(function (i) {
        tot_cbm += parseFloat($(this).text());

    });

    $(".rst_rton").each(function (i) {
        tot_rton += parseFloat($(this).text());

    });
    $("#tot_cWgt").text(tot_cWgt.toFixed(3));
    $("#tot_cbm").text(tot_cbm.toFixed(3));
    $("#tot_rton").text(tot_rton.toFixed(3));

    $("#result_cbm").show();
}

$(document).on("click", ".btn_delete", function () {
    $(this).closest(".cbm_list").remove();
    var tot_vWgt = 0;
    var tot_cWgt = 0;
    var tot_cbm = 0;
    var tot_rton = 0;
    $(".rst_vol").each(function (i) {
        tot_vWgt += parseFloat($(this).text());

    });

    $("#tot_vWgt").text(tot_vWgt.toFixed(3));

    $(".rst_chr").each(function (i) {
        tot_cWgt += parseFloat($(this).text());

    });

    $(".rst_cbm").each(function (i) {
        tot_cbm += parseFloat($(this).text());

    });

    $(".rst_rton").each(function (i) {
        tot_rton += parseFloat($(this).text());

    });
    $("#tot_cWgt").text(tot_cWgt.toFixed(3));
    $("#tot_cbm").text(tot_cbm.toFixed(3));
    $("#tot_rton").text(tot_rton.toFixed(3));

    $("#result_cbm").show();
});
$(document).on("focusin", ".wgt", function () {
    if ($(this).val() != "") {
        _fnUncomma(this, "");
    }
});
//콤마 풀기
function _fnUncomma(str, val) {
    if (val == "val") {
        var num = str.val();
    } else {
        var num = str.value;
    }
    num = num.replace(/,/g, '');
    str.value = num;
}

$(document).on("keyup", ".wgt", function (key) {

    if (key.keyCode != 8 && key.keyCode != 46) {
        //
        var _pattern2 = /^\d*[.]\d{4}$/; // 현재 value값이 소수점 셋째짜리 숫자이면 더이상 입력 불가
        if (_pattern2.test($(this).val().replace(/,/gi, ''))) {
            $(this).val($(this).val().substr(0, $(this).val().length - 1));
        }
        _fnGetNumber(this, "");
    }
});
function isNumberKey(evt) {

    var charCode = (evt.which) ? evt.which : event.keyCode;

    if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57))
        return false;

    var _value = event.srcElement.value;
    // 소수점(.)이 두번 이상 나오지 못하게

    var _pattern0 = /^\d*[.]\d*$/; // 현재 value값에 소수점(.) 이 있으면 . 입력불가

    if (_pattern0.test(_value)) {
        if (charCode == 46) {
            return false;
        }
    }

    return true;
}

$(document).on("keyup", "input", function (e) {
    if (e.which == 13) {
        var tr = $(this).closest('div');
        if (tr.length > 0) {
            var $this = $(e.target);
            var td = tr[0].className;
            td = td.split(" ")[0];
            var index = parseFloat($this.attr('data-index'));
            $('.' + td + ' [data-index="' + (index + 1).toString() + '"]').focus();
        } else {
            var $this = $(e.target);
            var index = parseFloat($this.attr('data-index'));
            $('[data-index="' + (index + 1).toString() + '"]').focus();
        }
    }
});

function _fnGetNumber(obj, sum) {
    var num01;
    var num02;
    if (sum == "sum") {
        num02 = obj;
        num01 = fnSetComma(num02); //콤마 찍기
        return num01;
    }
    else {
        num01 = obj.value.slice(0, 13);
        num02 = num01.replace(/[^0-9.]/g, ""); //0으로 시작하거나 숫자가 아닌것을 제거,
        num01 = fnSetComma(num02); //콤마 찍기
        obj.value = num01;
    }

}

/////////////////function MakeList/////////////////////

////////////////////////API////////////////////////////


$(document).on("click", "#btn_clear_cbm", function () {

    $("#result_cbm").css("display", "none");
    $(".scrollbar_cbm").empty();
    $("#tot_vWgt").text("");
    $("#tot_cWgt").text("");
    $("#tot_cbm").text("");
    $("#tot_rton").text("");
    $("#cbm_length").val("");
    $("#cbm_width").val("");
    $("#cbm_height").val("");
    $("#cbm_grs_wgt").val("");
    $(".one > .delete").hide();
    $('#CBM_UNIT').val('').prop("selected", true);
    cntCal = 0;
});

$(document).on('click', '.cntr_status.air.off', function () {
    $('.cntr_ship').hide();
    $('.cntr_air').show();
    $('#show_air').hide();
    $('#hide_air').show();
    $('#show_air_mo').hide();
    $('#hide_air_mo').show();
    $('.cntr_status.sea').removeClass('on');
    $('.cntr_status.sea').addClass('off');
    $('.cntr_status.air').addClass('on');
    $('.cntr_status.air').removeClass('off');
    if ($('.cntr_ship').css("display") == 'none') {
        $('#hide_ship').hide();
        $('#show_ship').show();
        $('#hide_ship_mo').hide();
        $('#show_ship_mo').show();
    }
})
$(document).on('click', '.cntr_status.air.on', function () {
    $('.cntr_air').hide();
    $('#hide_air').hide();
    $('#show_air').show();
    $('#hide_air_mo').hide();
    $('#show_air_mo').show();
    if ($('.cntr_ship').css('display') == 'none') {
        $('.cntr_status.air').removeClass('on');
        $('.cntr_status.air').addClass('off');
    }
})

$(document).on('click', '.cntr_status.sea.off', function () {
    $('.cntr_air').hide();
    $('.cntr_ship').show();
    $('#show_ship').hide();
    $('#hide_ship').show();
    $('#show_ship_mo').hide();
    $('#hide_ship_mo').show();
    $('.cntr_status.air').removeClass('on');
    $('.cntr_status.air').addClass('off');
    $('.cntr_status.sea').addClass('on');
    $('.cntr_status.sea').removeClass('off');
    if ($('.cntr_air').css("display") == 'none') {
        $('#hide_air').hide();
        $('#show_air').show();
        $('#hide_air_mo').hide();
        $('#show_air_mo').show();
    }
})
$(document).on('click', '.cntr_status.sea.on', function () {
    $('.cntr_ship').hide();
    $('#hide_ship').hide();
    $('#show_ship').show();
    $('#hide_ship_mo').hide();
    $('#show_ship_mo').show();
    if ($('.cntr_air').css('display') == 'none') {
        $('.cntr_status.sea').removeClass('on');
        $('.cntr_status.sea').addClass('off');
    }
})

$('input').on('keydown', function (e) {
    if (e.which === 13) { // 13은 엔터키를 의미합니다.
        if (this.id === 'cbm_height') { // 만약 현재 input의 id가 'cbm_height'라면
            $('#cbm_grs_wgt').focus(); // id가 'cbm_grs_wgt'인 input으로 이동
        } else {
            var index = $('.int').index(this) + 1;
            $('.int').eq(index).focus(); // 아니라면, 다음 input으로 이동
        }
    }
});