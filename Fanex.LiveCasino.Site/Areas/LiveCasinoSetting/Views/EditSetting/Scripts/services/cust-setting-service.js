/* File Created: July 21, 2014 */

if (fanex.liveCasinoSetting) {
    fanex.liveCasinoSetting.service('CustomerSettingService', ['$http', function ($http) {
        'use strict';
        var me = this;

        me.custSetting = custSetting;
        me.updateSetting = function () {
            var custInfoScope = angular.element('[data-ng-controller="CustomerInfoController"]').scope();
            var custSettingForUpdate = {
                settingContext: custSetting.SettingContext,
                custInfo: custInfoScope.getCustInfo()
            };

            $http({
                method: 'POST',
                url: $('#tblSetting').data('update-href'),
                headers: { 'Content-Type': 'application/json' },
                data: custSettingForUpdate
            }).
            success(function (data) {
            }).
            error(function (data) {
                //// loging error
            });
        };

        me.updatePtAndComSetting = function (ptAndComSetting) {
            var demo = ptAndComSetting;
        };
    } ]);
}