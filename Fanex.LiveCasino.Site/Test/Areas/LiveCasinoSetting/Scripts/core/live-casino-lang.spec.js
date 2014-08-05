/// <reference path="../../../../../Areas/LiveCasinoSetting/Scripts/core/live-casino-lang.js" />
/// <reference path="../../../../../Areas/LiveCasinoSetting/Scripts/core/language/LiveCasinoLang.js" />
/// <reference path="../../../../../Areas/LiveCasinoSetting/Scripts/core/language/LiveCasinoLang.en-US.js" />
/// <reference path="../../../../../Areas/LiveCasinoSetting/Scripts/core/language/LiveCasinoLang.zh-CN.js" />
/*global describe, it, language, expect, it*/
describe('Livecasino language', function () {
    'use strict';
    it('should get language value from key', function () {
        //// Arrange 

        //// Acc
        var resultString = language.getString("_101");

        //// Assert
        expect(resultString).toEqual("Commission is updated successfully.");
    });

    it('should get default language when current language has no information', function () {
        //// Arrange
        language.setLanguageCode('zh-CN');

        //// Acc
        var resultString = language.getString("_101");

        //// Assert
        expect(resultString).toEqual("佣金已更新成功");
    });

    it('should get key if there is no langauge', function () {
        //// Acc
        var resultString = language.getString("_1011");

        //// Assert
        expect(resultString).toEqual("_1011");
    });
});