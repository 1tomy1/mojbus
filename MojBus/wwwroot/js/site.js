﻿$().ready(function () {
    $("#accordion").accordion({
        collapsible: true,
        active: true,
        heightStyle: "content",
        autoHeight: false,
        clearStyle: true,
    });

    $("#datepicker").datepicker({
        dateFormat: 'dd. mm. yy',
        minDate: 0,
        scrollDefault: 'now',
        altField: '#dateISOFormat',
        altFormat: 'yy-mm-dd'
    });

    $('#datepicker').datepicker($.datepicker.regional["si"]);

    $('#timepicker').timepicker({
        timeFormat: "H:i",
        step: 30,
        scrollDefault: 'now'
    });

    if ($("#datepicker").data("date") !== undefined) {
        $('#datepicker').datepicker("setDate", new Date($("#datepicker").data("date")));
    } else {
        $('#datepicker').datepicker("setDate", new Date());
    }

    if ($("#timepicker").data("date") !== undefined) {
        $('#timepicker').timepicker("setTime", new Date($("#timepicker").data("date")));
    } else {
        $('#timepicker').timepicker('setTime', new Date());
    }

    $('#accordion').removeClass('hidden');
});

function filterMenu(inputElement, filterListId) {
    var filter, ul, li;
    filter = inputElement.value.toUpperCase();
    ul = document.getElementById(filterListId);
    li = ul.getElementsByTagName("li");
    for (i = 0; i < li.length; i++) {
        stopName = li[i].getAttribute("name");
        if (stopName.toUpperCase().indexOf(filter) > -1) {
            li[i].style.display = "";
        } else {
            li[i].style.display = "none";

        }
    }
}

function ChangeFavourite(clickedItem, stopName, routeShortName, directionId) {
    $.post(
        "/Stops/AddToFavourite",
        {
            stopName,
            routeShortName,
            directionId
        },
        function (data) {
            if (data)
                $(clickedItem).html('<img class="favouritesStarImage" src="images/starRed.svg" />');
            else
                $(clickedItem).html('<img class="favouritesStarImage" src="images/starGray.svg" />');
        }
    );
}

function RemoveFavourite(clickedItem, stopName, routeShortName, directionId) {
    $.post(
        "/Stops/AddToFavourite",
        {
            stopName,
            routeShortName,
            directionId
        },
        function (data) {
            if (!data)
                $(clickedItem).parent().remove();
        }
    );
}

function navigateWithDate(requestedUri) {
    window.location.href = `${requestedUri}&date=${getDateTimeString()}`;
}

function getDateTimeString() {
    let time = "";
    if ($("#timepicker").val() !== undefined) {
        time = `T${$("#timepicker").val()}`;
    }
    let date = "";
    if ($("#dateISOFormat").val() !== undefined) {
        date = `${$("#dateISOFormat").val()}`;
    }

    return `${date}${time}`;
}

var lat;
var lng;
var endLat;
var endLng;
var stopsArray;

function initMap() {
    var map = new google.maps.Map(document.getElementById('map'));
    var markerSize = new google.maps.Size(50, 50);
    var bounds = new google.maps.LatLngBounds();

    var startMarker = new google.maps.Marker({
        position: { lat: lat, lng: lng },
        map: map,
        icon: {
            url: "/images/startStop.svg",
            scaledSize: markerSize
        }
    });
    bounds.extend(startMarker.getPosition());

    if (endLat !== undefined && endLng !== undefined) {
        var endMarker = new google.maps.Marker({
            position: { lat: endLat, lng: endLng },
            map: map,
            icon: {
                url: "/images/endStop.svg",
                scaledSize: markerSize
            }
        });
        bounds.extend(endMarker.getPosition());
    }    

    if (stopsArray !== undefined) {        
        stopsArray.forEach(function (item) {
            if (lat !== item.stopLat && lng !== item.stopLon) {
                var marker = new google.maps.Marker({
                    position: { lat: item.stopLat, lng: item.stopLon },
                    map: map,
                    icon: {
                        url: "/images/intermediateStop.svg",
                        scaledSize: new google.maps.Size(30, 30)
                    }
                });
                bounds.extend(marker.getPosition());
            }
        });        
    }
    map.fitBounds(bounds);
}

function mergeDateTime() {
    $('#dateISOFormat').val(getDateTimeString());
}

function uriWithDateTimeToHref(clickedItem, uri) {
    $(clickedItem).attr('href', `${uri}&date=${getDateTimeString()}`);
}

function chosenItemToInput(value, inputId) {
    document.getElementById(inputId).value = value;
}

function resetDateAndTimeToCurrent(clickedItem, navigationPath) {
    $('#datepicker').datepicker("setDate", new Date());
    $('#timepicker').timepicker('setTime', new Date());
    $(clickedItem).attr('href', `${navigationPath}&date=${getDateTimeString()}`);
}

function resetDateAndTimeToCurrentWithoutNavigation() {
    $('#datepicker').datepicker("setDate", new Date());
    $('#timepicker').timepicker('setTime', new Date());
}