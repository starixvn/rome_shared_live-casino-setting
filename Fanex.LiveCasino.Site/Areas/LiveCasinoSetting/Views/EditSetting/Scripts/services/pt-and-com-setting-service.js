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
            }).
            error(function (data, status) {
                if (status === fanex.liveCasinoSetting.UN_AUTHORIZED_CODE) {
                    fanex.liveCasinoSetting.errorResult.CommandResultsList[0].Results[0].Message = data;
                    custSettingScope.mainResult = fanex.liveCasinoSetting.errorResult;
                }
            });
    };

} ]);