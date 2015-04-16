﻿console.log("Javascript is working.");
$(document).ready(function () {
    console.log("JQuery loaded");
    $("#errorMess").dialog({
        modal: true,
        buttons: {
            Ok: function () {
                $(this).dialog("close");
            }
        },
        closeOnEscape: true,
        closeText: "X"
    }

        );

    $("#MainContent_eventdetails").dialog({
        modal: false,
        buttons: {
            Ok: function () {
                $(this).dialog("close");
            }
        },
        closeOnEscape: true,
        closeText: "X",
        dialogClass: "events",
        autoOpen: true
    }

       );
    $('#listStartTime').timePicker();
    $('#listEndTime').timePicker();

    $("#listPrice").spinner({
        min: 0,
        max: 999,
        step: 1,
        start: 0.00,
        numberFormat: "C"
    });

    $("#listTickets").spinner({
        min: 0,
        max: 999,
        step: 1,
        start: 0
    });




    /* last function because it will break on most pages */
    var inside = document.getElementById('MainContent_eventdetails');
    if ((inside.innerText.replace(" ", "") == "")) {
        $('#MainContent_eventdetails').dialog('close');
    }



});

function showDetails() {

    $('#MainContent_eventdetails').dialog('open');


}