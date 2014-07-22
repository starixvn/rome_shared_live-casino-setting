/* File Created: July 21, 2014 */

var fanex = fanex || {};

//// Core function
fanex.cloneObject = function (obj) {
    'use strict';
    var newObj = (obj instanceof Array) ? [] : {};
    for (var i in obj) {
        if (i === 'clone') {
            continue;
        }
        if (obj[i] && typeof obj[i] === "object") {
            newObj[i] = fanex.cloneObject(obj[i]);
        } else {
            newObj[i] = obj[i];
        }
    }
    return newObj;
};

//// live casino setting app

fanex.liveCasinoSetting = angular.module('fanex.liveCasinoSetting', []);

fanex.liveCasinoSetting.formType = {
    Add: 0,
    Edit: 1
};

fanex.liveCasinoSetting.custRole = {
    Super: 4,
    Master: 3,
    Agent: 2,
    Member: 1
};

