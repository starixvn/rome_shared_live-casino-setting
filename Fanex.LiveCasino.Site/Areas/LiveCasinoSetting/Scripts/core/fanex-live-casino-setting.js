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


fanex.roundNumber = function (number, decimals) {
    'use strict';
    var newnumber = Number(number + '').toFixed(parseInt(decimals));
    return parseFloat(newnumber); // Output the result to the form field (change for your purposes)
};

fanex.createOptionsFromRange = function (from, to, gap, decimals) {
    var options = [];
    decimals = decimals || 2;
    to += gap;
    for (var i = from; i < to; i += gap) {
        var value = fanex.roundNumber(i, decimals);
        options.push(
        {
            id: value,
            value: value
        });

    }

    return options.reverse();
}



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

