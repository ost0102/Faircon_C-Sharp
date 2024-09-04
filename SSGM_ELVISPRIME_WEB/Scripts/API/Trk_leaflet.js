////////////////////전역 변수//////////////////////////
var _layerMap;
var _Map;
var _LFurl = "http://api2.elvisprime.com/api/Trk/GetTrackingAIS";
////////////////////jquery event///////////////////////

////////////////////////function///////////////////////
//레이어 - Leaflet 세팅
function fnLayerSetLF(vHBL, vCntrNo,vBool) {
    try {
        var objRPAJsonData = new Object();

        objRPAJsonData.reqVal1 = _fnToNull(vHBL);
        objRPAJsonData.reqVal2 = _fnToNull(vCntrNo);

        var AppKey = _RPA_MngtNo + "^" + _RPA_Key;
        
        $.ajax({
            url: _LFurl,
            beforeSend: function (xhr) {
                xhr.setRequestHeader('Authorization-Token', AppKey);
            },
            type: "POST",
            async: false,
            dataType: "json",
            data: { "": _fnMakeJson(objRPAJsonData) },
            success: function (result) {
                if (_fnToNull(result) != "") {
                    var rtnData = JSON.parse(result);
                    if (_fnToNull(rtnData.Result) != "") {
                        if (rtnData.Result[0].trxCode == "N" || rtnData.Result[0].trxCode == "E") {
                            if (vBool) {
                                //빈값
                                drawingLayerNodata("LF_Layer_map", _layerMap);
                            } else {
                                layerClose('#tracking_layer');
                                _fnAlertMsg("추적이 되지않는 데이터입니다.");
                            }
                            return false;
                        }
                    } else {
                        drawingLayer("LF_Layer_map", rtnData, _layerMap);
                    }
                } else {
                    if (vBool) {
                        drawingLayerNodata("LF_Layer_map", _layerMap);
                    } else {
                        layerClose("#tracking_layer");
                        _fnAlertMsg("추적이 되지않는 데이터입니다.");
                    }
                }
                //createMap()
            }, error: function (xhr) {
                layerClose("#tracking_layer");
                _fnAlertMsg("시스템 사정으로 요청하신 작업을 처리할 수 없습니다.");
                console.log(xhr);
                return;
            }
        });
    }
    catch (err) {
        console.log("[Error - fnLayerSetLF]"+err.message);
    }
}

//레이어 - Leaflet 세팅
function fnSetLF(vHBL, vCntrNo, vBool) {
    try {
        var objRPAJsonData = new Object();

        objRPAJsonData.reqVal1 = _fnToNull(vHBL);
        objRPAJsonData.reqVal2 = _fnToNull(vCntrNo);
                
        var AppKey = _RPA_MngtNo + "^" + _RPA_Key;

        $.ajax({
            url: _LFurl,
            beforeSend: function (xhr) {
                xhr.setRequestHeader('Authorization-Token', AppKey);
            },
            type: "POST",
            async: false,
            dataType: "json",
            data: { "": _fnMakeJson(objRPAJsonData) },
            success: function (result) {
                if (_fnToNull(result) != "") {
                    var rtnData = JSON.parse(result);
                    if (_fnToNull(rtnData.Result) != "") {
                        if (rtnData.Result[0].trxCode == "N" || rtnData.Result[0].trxCode == "E") {
                            if (vBool) {
                                //빈값
                                drawingLayerNodata("LF_map", _Map);
                            } else {
                                layerClose('#tracking_layer');
                                _fnAlertMsg("추적이 되지않는 데이터입니다.");
                            }
                            return false;
                        }
                    } else {
                        drawingLayer("LF_map", rtnData, _Map);
                    }
                } else {
                    if (vBool) {
                        drawingLayerNodata("LF_map", _Map);
                    } else {
                        layerClose("#tracking_layer");
                        _fnAlertMsg("추적이 되지않는 데이터입니다.");
                    }
                }
                //createMap()
            }, error: function (xhr) {
                layerClose("#tracking_layer");
                _fnAlertMsg("시스템 사정으로 요청하신 작업을 처리할 수 없습니다.");
                console.log(xhr);
                return;
            }
        });
    }
    catch (err) {
        console.log("[Error - fnLayerSetLF]" + err.message);
    }
}

/////////////////function MakeList/////////////////////
////////////////////////API////////////////////////////
function connectTheDots(data) {
    var c = [];
    for (i in data._layers) {
        var x = data._layers[i]._latlng.lat;
        var y = data._layers[i]._latlng.lng;
        c.push([x, y]);
    }
    return c;
}

//화물 추적 AIS - 데이터가 없을 경우.
function drawingLayerNodata(vMap,vMymap) {

    if (_fnToNull(vMymap) != "") {
        vMymap.remove();
    }
    vMymap = L.map(vMap, {
        //center: [lat, lng],
        center: [32.896531, 124.402956],
        zoom: 5,
        zoomControl: false
    });

    L.tileLayer('http://mt0.google.com/vt/lyrs=y&hl=kr&x={x}&y={y}&z={z}', {
        attribution: 'Map data &copy; Copyright Google Maps<a target="_blank" href="https://maps.google.com/maps?ll=24.53279,56.62833&amp;z=13&amp;t=m&amp;hl=ko-KR&amp;gl=US&amp;mapclient=apiv3"></a>' //화면 오른쪽 하단 attributors
    }).addTo(vMymap);
}

//화물 추적 AIS 그림 그려주기
function drawingLayer(vMap, json_data, vMymap) {
    var spiral = {
        type: "FeatureCollection",
        features: []
    };
    var master = json_data.Master[0];//헤더 테이블 조회
    var result = [];
    for (var i in master)
        result.push([master[i]]);

    var POD = result[4].concat(result[5]);
    var POL = result[8].concat(result[9]);
    var lastRoute;
    var rotate;
    var detail = json_data.Detail;//디테일 테이블 조회

    //예외처리 - Port 정보가 마스터 데이터에 없을 경우 
    if (_fnToNull(POD[0]) == "" || _fnToNull(POD[1]) == "" || _fnToNull(POL[0]) == "" || _fnToNull(POL[1]) == "") {
        drawingLayerNodata(vMap);
        return false;
    }

    var result2 = [];

    for (var i = 0; i < detail.length; i++) {
        var arrayDt = [];
        arrayDt.push(detail[i]["MAP_LAT"]);
        arrayDt.push(detail[i]["MAP_LNG"]);
        lastRoute = arrayDt;    // 배 아이콘 위치
        rotate = detail[i]["MAP_COURSE"]; // 배 방향
        result2.push(arrayDt);
        var g = {
            "color": "red",
            "type": "Point",
            "coordinates": [detail[i]["MAP_LNG"], detail[i]["MAP_LAT"]]
        };
        var p = {
            "id": i,
            "speed": detail[i]["MAP_SPEED"],
            "course": detail[i]["MAP_COURSE"]
        };
        spiral.features.push({
            "geometry": g,
            "type": "Feature",
            "properties": p
        });
    }
    var zoom = 5; //줌 레벨

    if (_fnToNull(vMymap) != "") {
        vMymap.remove();
    }

    vMymap = L.map(vMap, {
        //center: [lat, lng],
        center: [32.896531, 124.402956],
        zoom: zoom,
        zoomControl: false
    });

    L.control.zoom({
        position: 'bottomright'
    }).addTo(vMymap);

    /*
    lyrs=m : Standard Road Map
    lyrs=p : Terrain
    lyrs=r : Somehow Altered Road Map
    lyrs=s : Satellite Only
    lyrs=t : Terrain Only
    lyrs=y : Hybrid
    lyrs=h : Roads Only
    */
    L.tileLayer('http://mt0.google.com/vt/lyrs=y&hl=kr&x={x}&y={y}&z={z}', {
        attribution: 'Map data &copy; Copyright Google Maps<a target="_blank" href="https://maps.google.com/maps?ll=24.53279,56.62833&amp;z=13&amp;t=m&amp;hl=ko-KR&amp;gl=US&amp;mapclient=apiv3"></a>' //화면 오른쪽 하단 attributors
    }).addTo(vMymap);
    // Creating a poly line


    //화물추적 dot(점) 색상 변경 (이미지로 색상 변경 해야됨)
    var circleIcon = L.icon({
        iconUrl: "../Images/circle_red.png",
        iconSize: [4, 4]  // size of the icon
    });

    var polyline = L.geoJson(spiral, {
        pointToLayer: function (feature, latlng) {
            return L.marker(latlng, {
                icon: circleIcon
            });
        },
        onEachFeature: function (feature, layer) {
            layer.bindPopup('<table><tbody><tr><td><div><b>speed:</b></div></td><td><div>' + feature.properties.speed + '</div></td></tr><tr><td><div><b>course:</b></div></td><td><div>' + feature.properties.course + '</div></td></tr></tbody></table>');
            layer.on('mouseover', function () { layer.openPopup(); });
            layer.on('mouseout', function () { layer.closePopup(); });
        }
    });

    //spiralBounds = polyline.getBounds();
    //vMymap.fitBounds(spiralBounds);
    polyline.addTo(vMymap);

    spiralCoords = connectTheDots(polyline);
    var spiralLine = L.polyline(spiralCoords, { color: '#ff2600' }).addTo(vMymap); //color 변경
    //var spiralLine = L.polyline(spiralCoords).addTo(vMymap)

    var shipIconBig = L.icon({
        iconUrl: "../Images/map_ship@73x87.png",
        iconSize: [24, 30]  // size of the icon
    });

    var shipIcon = L.icon({
        iconUrl: "../Images/map_ship@73x87.png",
        iconSize: [30, 42]
    });

    var makerIcon = L.icon({
        iconUrl: '../Images/map_marker@64x79.png',
        iconSize: [30, 42]
    });
    var portIcon = L.icon({
        iconUrl: '../Images/map_port@69x79.png',
        iconSize: [30, 42]
    });

    var makerIconBig = L.icon({
        iconUrl: '../Images/map_marker@64x79.png',
        iconSize: [24, 30]
    });
    var portIconBig = L.icon({
        iconUrl: '../Images/map_port@69x79.png',
        iconSize: [24, 30] // size of the icon
    });


    var maker_POD = L.marker(POD, { icon: makerIconBig }).addTo(vMymap);
    var maker_POL = L.marker(POL, { icon: portIconBig }).addTo(vMymap);

    if (lastRoute != undefined) {
        var LastMarker = L.marker(lastRoute, { icon: shipIcon, rotationOrigin: 'center center', rotationAngle: rotate }).addTo(vMymap);
    }

    vMymap.on('zoomend', function () {
        var currentZoom = vMymap.getZoom();
        if (currentZoom <= 5) {
            maker_POD.setIcon(makerIconBig);
            maker_POL.setIcon(portIconBig);
            LastMarker.setIcon(shipIconBig);
        }
        else {
            maker_POD.setIcon(makerIcon);
            maker_POL.setIcon(portIcon);
            LastMarker.setIcon(shipIcon);
        }
    });
}