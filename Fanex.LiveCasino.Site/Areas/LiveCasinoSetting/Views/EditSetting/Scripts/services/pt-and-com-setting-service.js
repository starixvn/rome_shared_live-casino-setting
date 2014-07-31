if (fanex.liveCasinoSetting) {
    fanex.liveCasinoSetting.service('PtAndComSettingService', ['$http', '$timeout', function ($http, $timeout) {
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
            success(function (result) {
                custSettingScope.commandResults = result.CommandResults;
                if (result.IsSuccessful) {
                    $timeout(function () {
                        custSettingScope.commandResults = [];
                    }, fanex.liveCasinoSetting.TIME_OUT_SHOW_MESSAGE);
                }
            }).
            error(function (data) {
                //// loging error
            });
        };

    } ]);
}
else {
    
}