/* File Created: July 21, 2014 */

if (fanex.liveCasinoSetting) {
    fanex.liveCasinoSetting.service('CustomerGameSettingService', ['$http', function ($http) {
        'use strict';
        var $this = this;

        $this.updateGameSetting = function (gameSetting) {
            var gameSettingModel = {
                settingContext: custSetting.SettingContext,
                gameSetting: gameSetting
            };

            return $http({
                method: 'POST',
                url: $('#tblGameSetting').data('update-href'),
                headers: { 'Content-Type': 'application/json' },
                data: gameSettingModel
            });
        };
    } ]);
}