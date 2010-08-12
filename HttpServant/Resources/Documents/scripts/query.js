/// <reference path="jquery.js" />
/// <reference path="jquery.linq.js" />
/// <reference path="jquery-ui.js" />
/// <reference path="jquery.dataTables.js" />
/// <reference path="application.js" />

var _storedReqs;
var _timer;

function request(requestString, interval) {
    clearInterval(_timer);
    interval = interval >= 0.5 ? interval : 0;
    $.ajax({
        url: requestString,
        dataType: 'json',
        cache: false,
        beforeSend: function (req) {
            $("#resultHeader").text("Waiting for response.");
            return true;
        },
        success: function (ret) {
            var seq = $.Enumerable.From(ret);
            $("#result").html("<table></table>");
            $("#result table").dataTable({
                "aaData": seq.Skip(1).ToArray(),
                "aoColumns": $.Enumerable.From(seq.First())
                    .Select(function (h) {
                        return {
                            "sTitle": h
                        };
                    })
                    .ToArray(),
                "sDom": "ift",
                "bPaginate": false,
                "bLengthChange": false,
                "bJQueryUI": true
            });
            $("#resultHeader").text("Result (table)" + (interval > 0 ? " - Interval: " + interval + " secs" : ""));
        },
        error: function (req, textStatus, errorThrown) {
            $("#result").html("<pre>" + req.responseText + "</pre>");
            $("#resultHeader").text("Result (scalar)" + (interval > 0 ? " - Interval: " + interval + " secs" : ""));
        }
    });
    if (interval > 0) {
        _timer = setInterval(function () {
            request(requestString, interval);
        }, interval * 1000);
    };
}

function loadStoredRequestList() {
    var $storedReqs = $("#storedReqs");
    $.ajax({
        url: "/!/storedmgr/stored-requests/!/.table.json?sign=true",
        dataType: 'json',
        cache: false,
        beforeSend: function (req) {
            $storedReqs.text("Loading...");
            return true;
        },
        success: function (ret) {
            _storedReqs = $.Enumerable.From(ret)
                .Skip(1)
                .Select(function (_) {
                    return [_[0], _[1], $.parseJSON(_[2])];
                });
            $storedReqs.html(
                "<dl>" + _storedReqs.Select(function (r) {
                    return "<dt class='storedReq'><a href='javascript:selectStoredRequest(\"" + r[0] + "\")'>" + r[0] + "</a></dt><dd>" + r[1] + "</dd>";
                }).ToString() + "</dl>"
            );
        },
        error: function (req, textStatus, errorThrown) {
            $storedReqs.text("Error");
        }
    });
}

function selectStoredRequest(name) {
    var storedReq = _storedReqs.Single(function (_) {
        return _[0] == name;
    });
    $(".ui-tabs-panel:visible ").html(
        "<table><caption>" + storedReq[0] + ": " + storedReq[1] + "</caption><tbody>"
            + $.Enumerable.From(storedReq[2]).Select(function (_) {
                  return "<tr><td>" + _.name + "</td><td>" + _.desc + "</td><td><input type='text' name='" + _.name + "' value='' /></td></tr>";
              }).ToString()
            + "</tbody></table><div id='queryPanel'>"
            + "<label for='interval'>Refresh Interval:</label><input type='text' name='interval' value='0' />"
            + "<button name='query'>Query</button><button name='stop'>Stop</button></div>"
    );
    $("button[name='query']:visible").button().bind("click", function () {
        var args = $.Enumerable.From($("table input:visible"))
            .Select(function (_) {
                return escapeRequestString(_.name) + "==" + Base64.encodeURI(escapeRequestString(_.value));
            })
            .ToString("&");
        request("/!/storedmgr/apply/"
            + storedReq[0]
            + (args != "" ? "?=" : "")
            + Base64.encodeURI(args)
            + "/!/.id",
            $("input[name='interval']:visible").val()
        );
    });
    $("button[name='stop']:visible").button().bind("click", function () {
        if (_timer) {
            clearInterval(_timer);
        }
    });
}

$(document).ready(function () {
    var $tab_name_input = $('#tab_name');
    var tab_counter = 0;
    var $tabs = $('#tabs').tabs({
        collapsible: true,
        tabTemplate: '<li><a href="#{href}">#{label}</a> <span class="ui-icon ui-icon-close">Remove Tab</span></li>',
        add: function (event, ui) {
            $(ui.panel).append('<p>Select stored request in the left pane to show the query form.</p>');
        }
    });

    var $dialog = $('#dialog').dialog({
        autoOpen: false,
        modal: true,
        buttons: {
            'Add': function () {
                addTab();
                $(this).dialog('close');
            },
            'Cancel': function () {
                $(this).dialog('close');
            }
        },
        open: function () {
            $tab_name_input.focus();
        },
        close: function () {
            $form[0].reset();
        }
    });

    var $form = $('form', $dialog).submit(function () {
        addTab();
        $dialog.dialog('close');
        return false;
    });

    function addTab(name) {
        $tabs.tabs('add', '#tabs-' + tab_counter, name || $tab_name_input.val() || 'Tab ' + tab_counter);
        $tabs.find(".ui-tabs-nav").sortable({ axis: 'x' });
        ++tab_counter;
    }

    $('#addTab').bind("click", function () {
        $dialog.dialog('open');
    });

    $('#tabs span.ui-icon-close').live('click', function () {
        var index = $('li', $tabs).index($(this).parent());
        if ($(".ui-tabs-panel").length > 1) {
            $tabs.tabs('remove', index);
        }
    });

    addTab();
    loadStoredRequestList();
});
