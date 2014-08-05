 /* File Created: July 21, 2014 */
/*global custSetting*/
if (fanex.liveCasinoSetting) {
    fanex.liveCasinoSetting.service('CustomerGameSettingService', ['$http', function ($http) {
        'use strict';
        var $this = this;

        $this.updateGameSetting = function (gameSetting, custSettingScope) {
            var gameSettingModel = {
                settingContext: custSetting.SettingContext,
                gameSetting: gameSetting
            };

            $http({
                method: 'POST',
                url: $('#tblGameSetting').data('update-href'),
                headers: { 'Content-Type': 'application/json' },
                data: gameSettingModel
            }).success(function (result) {
                custSettingScope.commandResults = result.CommandResultsList;
            }).error(function (data) {
                window.console.log(data);
            });
        };
    } ]);
}