var language = language || {};

language.defaultCode = 'en-US';
language.code = 'en-US';

language.setLanguageCode = function (languageCode) {
    language.code = languageCode;
};

language.getString = function (key) {
    var value = language.liveCasino[language.code][key];
    if (!value) {
        value = language.liveCasino[language.defaultCode][key];
        if (!value) {
            value = key;
        }
    }

    return value;
}