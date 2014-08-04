/* File Created: July 21, 2014 */
/*global custSetting*/
if (fanex.liveCasinoSetting) {
    fanex.liveCasinoSetting.service('CustomerSettingService', ['$http', '$timeout', function ($http, $timeout) {
        'use strict';
        var me = this;

        me.custSetting = custSetting;
        me.updateAllSetting = function (custSettingForUpdate, scope) {
            $http({
                method: 'POST',
                url: $('#tblSetting').data('update-href'),
                headers: { 'Content-Type': 'application/json' },
                data: custSettingForUpdate
            }).
            success(function (result) {
                scope.commandResults = result.CommandResultsList;
                if (result.IsSuccessful) {
                    $timeout(function () {
                        //$window.localtion.href = $window.location.href;
                    }, fanex.liveCasinoSetting.TIME_OUT_SHOW_MESSAGE);
                }
            });
        };
    } ]);
}