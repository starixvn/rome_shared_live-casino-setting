if (fanex.liveCasinoSetting) {
    fanex.liveCasinoSetting.service('PtAndComSettingService', ['$http', function ($http) {
        'use strict';
        var me = this;

        me.ptAndComSetting = custSetting.PtAndComSetting;
        me.updatePtAndComSetting = function (ptAndComSetting, custSettingScope) {
            var ptAndComUpdateModel = {
                settingContext: custSetting.SettingContext,
                ptAndComSetting: ptAndComSetting
            };

            $http({
                method: 'POST',
                url: $('#tblPtComSetting').data('update-href'),
                headers: { 'Content-Type': 'application/json' },
                data: ptAndComUpdateModel
            }).
            success(function (commandResults) {
                custSettingScope.commandResults = commandResults;
            }).
            error(function (data) {
                //// loging error
            });
        };




    } ]);
}
else {
    
}