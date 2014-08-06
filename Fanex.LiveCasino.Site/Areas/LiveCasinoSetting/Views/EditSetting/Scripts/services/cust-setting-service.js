/* File Created: July 21, 2014 */
/*global custSetting*/
if (fanex.liveCasinoSetting) {
    fanex.liveCasinoSetting.service('CustomerSettingService', ['$http', function ($http) {
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
            success(function (mainResult) {
                scope.mainResult = mainResult;
            });
        };
    } ]);
}