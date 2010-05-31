/// <reference path="jquery.js" />
/// <reference path="jquery.linq.js" />

function markup(obj, idx) {
    return "<tr class='" + (idx % 2 == 0 ? "even" : "odd") + "'>" +
        $.Enumerable.From(obj)
            .ToString("", function(c) {
                return "<td>" + c + "</td>";
            });
}

function request(requestString) {
    $.ajax({
        url: requestString,
        dataType: 'json',
        cache: false,
        beforeSend: function(req) {
            $("#resultHeader").text("Waiting for response.");
            return true;
        },
        success: function(ret) {
            var seq = $.Enumerable.From(ret);
            $("#result").html(
                "<table><thead>" +
                markup(seq.First()) +
                "</thead><tbody>" +
                seq.Skip(1).Select(function(r, i) {
                    return markup(r, i);
                })
                .ToString()
            );
            $("#resultHeader").text("Result");
        },
        error: function(req, textStatus, errorThrown) {
            $("#result").html("<pre>" + req.responseText + "</pre>");
            $("#resultHeader").text("Error");
        }
    });
}

function requestString_keyPress(event) {
    if ((event ? event : window.event).keyCode == 13) {
        $("#requestButton").click();
        return false;
    } else {
        return true;
    }
}