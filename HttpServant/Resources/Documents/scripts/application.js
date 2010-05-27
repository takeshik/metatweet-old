/// <reference path="jquery.js" />
/// <reference path="jquery.linq.js" />

function markup(obj, idx) {
    return "<tr class='" + (idx % 2 == 0 ? "even" : "odd") + "'>" +
        $.Enumerable.From(obj)
            .ToString("", function(c) {
                return "<td>" + c + "</td>";
            });
}

function toTable(obj, selector) {
    var seq = $.Enumerable.From(obj);
    $(selector, $(document)).html(
        "<table><thead>" +
        markup(seq.First()) +
        "</thead><tbody>" +
        seq.Skip(1).Select(function(r, i) {
            return markup(r, i);
        })
        .ToString()
    );
}