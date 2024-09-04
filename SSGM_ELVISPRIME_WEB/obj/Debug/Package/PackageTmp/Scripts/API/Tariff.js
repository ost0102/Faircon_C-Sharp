////////////////////전역 변수//////////////////////////
var _vPR = 0 // 현재 할증률

////////////////////jquery event///////////////////////

////////////////////////function///////////////////////
//할증률 검색
function fnSetPR() {

	//배열에 데이터를 넣을까?
	var arrData = new Array();
	var i = 0;

	var vLimitPR = 0; //할증 상한가
	var vMaxPR = 0; //제일 높은 할증률
	var vTotalPR = 0; //최대 할증률	
	
	var isboolean = true; //중복 할증률 체크	

	//1. 선택한 체크박스 부터 확인
	$.each($(".PR_Check"), function () {
		if ($(this).prop("checked") == true && !$(this).hasClass("input_checkbox_Y")) {
			arrData[i] = $(this).val();
			i++;
        }
	});	

	//데이터가 없으면 무한대가 나와버림
	if (arrData.length != 0) {
		vLimitPR = Math.max.apply(null, arrData) + 50;  //할증률 상한가 계산
		vMaxPR = Math.max.apply(null, arrData);			//최대 할증률 계산
	} else {
		vLimitPR = 0;
		vMaxPR = 0;
    }

	//for문 변수 초기화
	i = 0;

	//2. 할증률 계산 (최대 할증률을 제외한 나머지는 50% 할인)
	for (i=0;i<arrData.length;i++) {
		if (arrData[i] == vMaxPR) {
			if (isboolean) {
				vTotalPR += Number(vMaxPR);
				isboolean = false;
			} else {
				var vValue = Number(arrData[i]);
				vTotalPR += vValue / 2;
            }
		} else {
			var vValue = Number(arrData[i]);
			vTotalPR += vValue / 2;
		}
    }

	//3. 최대 할증률 
	if (vLimitPR < vTotalPR) {
		//최대할증률 보다 높다.
		_vPR = vLimitPR;
	} else {
		_vPR = vTotalPR;
	}
}

//할증률 계산 Calculation Primium Rate // 사사오입 (2번째 정수부터)
function fnCal_PR(vValue, vPR) {
	return Math.round((vValue + (vValue * (vPR / 100))) / 100) * 100;
}

//예외로 추가해야되는 금액 계산 함수
function fnCal_EP() {
	var i = 0;
	var arrData = new Array();
	var vResult = 0; 
		
	$.each($(".input_checkbox_Y"), function () {
		if ($(this).prop("checked") == true) {
			arrData[i] = $(this).val();
			i++;
		}
	});	

	for (i = 0; i < arrData.length; i++) {
		vResult += Number(arrData[i]);
	}

	return Number(_fnToZero(vResult));
}
/////////////////function MakeList/////////////////////
////////////////////////API////////////////////////////

