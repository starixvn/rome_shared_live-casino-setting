/* File Created: July 21, 2014 */
/*global custSetting*/
if (fanex.liveCasinoSetting) {
    fanex.liveCasinoSetting.service('CustomerWinLimitService', ['$http', '$timeout', function ($http, $timeout) {
        'use strict';
        var $this = this;

        $this.getCurrentMemberWinLimit = function (custId) {
            return $http({
                method: 'GET',
                url: $('#memberWinLimitLink').val() + '?custId=' + custId,
                headers: { 'Content-Type': 'application/json' }
            });
        };

        $this.updateWinLimitSetting = function (winLimitModel, custSettingScope) {
            var winLimitUpdate = {
                settingContext: custSetting.SettingContext,
                model: {
                    Max: winLimitModel.Max.value,
                    IsAutoReset: winLimitModel.IsAutoReset
                }
            };

            $http({
                method: 'POST',
                url: $('#tblMaxWinLimit').data('update-href'),
                headers: { 'Content-Type': 'application/json' },
                data: winLimitUpdate
            }).success(function (result) {
                custSettingScope.commandResults = result.CommandResultsList;
                if (result.IsSuccessful) {
                    $timeout(function () {
                        custSettingScope.commandResults = [];
                    }, fanex.liveCasinoSetting.TIME_OUT_SHOW_MESSAGE);
                }
            }).error(function (data) {
                window.console.log(data);
            });
        };
    } ]);
}