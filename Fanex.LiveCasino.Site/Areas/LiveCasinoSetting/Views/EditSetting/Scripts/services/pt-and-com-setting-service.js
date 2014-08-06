/*global custSetting*/
fanex.liveCasinoSetting.service('PtAndComSettingService', ['$http', function ($http) {
    'use strict';
    var me = this;

    me.updatePtAndComSetting = function (updatePtAndComUrl, ptAndComSetting, custSettingScope) {
        var ptAndComUpdateModel = {
            settingContext: custSetting.SettingContext,
            ptAndComSetting: ptAndComSetting
        };

        $http({
            method: 'POST',
            url: updatePtAndComUrl,
            headers: { 'Content-Type': 'application/json' },
            data: ptAndComUpdateModel
        }).
            success(function (mainResult) {
                custSettingScope.mainResult = mainResult;
            });
    };

} ]);