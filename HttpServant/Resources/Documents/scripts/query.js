/// <reference path="jquery.js" />
/// <reference path="jquery.linq.js" />
/// <reference path="jquery-ui.js" />

var _storedReqs;

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
            $("#resultHeader").text("Result (table)");
        },
        error: function(req, textStatus, errorThrown) {
            $("#result").html("<pre>" + req.responseText + "</pre>");
            $("#resultHeader").text("Result (scalar)");
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

function selectStoredReq(name) {
    var signatures = $.Enumerable.From(_storedReqs.Single(function (_) {
        return _[0] == name;
    })[2]);
    $(".ui-tabs-panel :visible").parent().html(
        "<table><tbody>"
            + signatures.Select(function (_) {
                return "<tr><td>" + _.name + "</td><td>" + _.desc + "</td><td><input type='text' name='" + _.name + "' value='' /></td></tr>";
            }).ToString()
            + "</tbody></table><button id='requestButton'>Request</button>"
    );
    $("#requestButton").bind("click", function () {
        var params = $.Enumerable.From($("table input")).Select(function (_) { return { "name": _.name, "value": _.value }; }).ToArray();
    });
}

$(function () {
    var $tab_name_input = $('#tab_name');
    var tab_counter = 0;
    var $tabs = $('#tabs').tabs({
        collapsible: true,
        tabTemplate: '<li><a href="#{href}">#{label}</a> <span class="ui-icon ui-icon-close">Remove Tab</span></li>',
        add: function (event, ui) {
            $(ui.panel).append('<p>Select stored request to show the request form.</p>');
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

    function addTab() {
        $tabs.tabs('add', '#tabs-' + tab_counter, $tab_name_input.val() || 'Tab ' + tab_counter);
        $tabs.find(".ui-tabs-nav").sortable({ axis: 'x' });
        ++tab_counter;
    }

    $('#add_tab')
        .button()
        .click(function () {
            $dialog.dialog('open');
        });

    $('#tabs span.ui-icon-close').live('click', function () {
        var index = $('li', $tabs).index($(this).parent());
        if (tab_counter > 1) {
            $tabs.tabs('remove', index);
        }
    });

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
                    return "<dt class='storedReq'><a href='javascript:selectStoredReq(\"" + r[0] + "\")'>" + r[0] + "</a></dt><dd>" + r[1] + "</dd>";
                }).ToString() + "</dl>"
            );
        },
        error: function (req, textStatus, errorThrown) {
            $storedReqs.text("Error");
        }
    });
});
