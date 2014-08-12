/* File Created: July 21, 2014 */

var fanex = fanex || {}; // jshint ignore:line

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
    decimals = decimals || 2;
    var newnumber = Number(number + '').toFixed(parseInt(decimals));
    return parseFloat(newnumber); // Output the result to the form field (change for your purposes)
};

fanex.createOptionsFromRange = function (current, from, to, gap, decimals) {
    'use strict';
    var options = [];
    decimals = decimals || 2;
    to = fanex.roundNumber(to, decimals);

    if (current < from) {
        options.push({
            id: current,
            value: current
        });
    }

    for (var i = from; i <= to; i = fanex.roundNumber(i + gap, decimals)) {
        options.push({
            id: i,
            value: i
        });
    }

    if (current > to) {
        options.push({
            id: current,
            value: current
        });
    }

    return options.reverse();
};



//// live casino setting app

fanex.liveCasinoSetting = angular.module('fanex.liveCasinoSetting', ['ngSanitize']);

fanex.liveCasinoSetting.config(['$httpProvider', function ($httpProvider) {
    'use strict';
    $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
} ]);

fanex.liveCasinoSetting.formType = {
    Add: 0,
    Edit: 1,
    ReadOnly: 2
};

fanex.liveCasinoSetting.custRole = {
    Super: 4,
    Master: 3,
    Agent: 2,
    Member: 1
};
//// Constant
fanex.liveCasinoSetting.UN_AUTHORIZED_CODE = 403;
fanex.liveCasinoSetting.PT_INTERVAL = 0.01;
fanex.liveCasinoSetting.COM_INTERVAL = 0.0005;
fanex.liveCasinoSetting.COM_DECIMAL = 4;
fanex.liveCasinoSetting.PT_DECIMAL = 2;
fanex.liveCasinoSetting.TIME_OUT_SHOW_MESSAGE = 5000;
fanex.liveCasinoSetting.errorResult = {
    CommandResultsList: [
        {
            Results: [
                {
                    IsSuccessful: false,
                    Message: ''
                }
            ]
        }
    ],
    IsSuccessful: false,
    isUnAuthorized: true
};

