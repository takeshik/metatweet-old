/// <reference path="jquery.js" />
/// <reference path="jquery.linq.js" />
/// <reference path="jquery-ui.js" />

function escapeRequestString(str) {
    return str.replace(/(\x07|\x1b|\f|\n|\r| |\t|\v|\\|!|$|#|;)/, function (m) {
        // HACK: Charm when m is empty string
        return m != ""
            ? "#" + m.charCodeAt(0) + ";"
            : "";
    });
}

(function(){
    /*
     * $Id$
     *
     * History:
     *   dankogai's original: character-based
     *   drry's fix: split string to array then join
     *   new version: regexp-based
     *   modified: remove useless codes
     */

    var b64chars 
        = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/';
    var b64tab = function(bin){
        var t = {};
        for (var i = 0, l = bin.length; i < l; i++) t[bin.charAt(i)] = i;
        return t;
    }(b64chars);

    var re_char_nonascii = /[^\x00-\xFF]/g;

    var sub_char_nonascii = function(m){
        var n = m.charCodeAt(0);
        return n < 0x800 ? String.fromCharCode(0xc0 | (n >>>  6))
                         + String.fromCharCode(0x80 | (n & 0x3f))
            :              String.fromCharCode(0xe0 | ((n >>> 12) & 0x0f))
                         + String.fromCharCode(0x80 | ((n >>>  6) & 0x3f))
                         + String.fromCharCode(0x80 |  (n         & 0x3f))
            ;
    };

    Base64 = {
        encodeURI:function(u){
            return window.btoa(u.replace(re_char_nonascii, sub_char_nonascii)).replace(/[+\/]/g, function(m0){
                return m0 == '+' ? '-' : '_';
            }).replace(/=+$/, '');
        },
    };
})();
