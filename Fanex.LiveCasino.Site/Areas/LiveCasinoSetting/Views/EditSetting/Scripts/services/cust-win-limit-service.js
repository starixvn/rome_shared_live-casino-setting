/* File Created: July 21, 2014 */

if (fanex.liveCasinoSetting) {
    fanex.liveCasinoSetting.service('CustomerWinLimitService', ['$http', function ($http) {
        'use strict';
        var $this = this;

        $this.getCurrentMemberWinLimit = function (custId) {
            return $http({
                method: 'GET',
                url: $('#memberWinLimitLink').val() + '?custId=' + custId,
                headers: { 'Content-Type': 'application/json' }
            });
        };

        $this.updateWinLimitSetting = function (winLimitModel) {
            return $http({
                method: 'POST',
                url: $('#memberWinLimitLink').val() + '?custId=' + custId,
                headers: { 'Content-Type': 'application/json' }
            });
        };
    } ]);
}