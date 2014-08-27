/*global currentLaguageKey*/
var language = language || {};

language.defaultCode = currentLaguageKey || 'en-US';
language.code = language.defaultCode;

language.setLanguageCode = function (languageCode) {
    'use strict';
    language.code = languageCode;
};

language.getString = function (key) {
    'use strict';
    var value = language.liveCasino[language.code][key];
    if (!value) {
        value = language.liveCasino[language.defaultCode][key];
        if (!value) {
            value = key;
        }
    }

    return value;
};