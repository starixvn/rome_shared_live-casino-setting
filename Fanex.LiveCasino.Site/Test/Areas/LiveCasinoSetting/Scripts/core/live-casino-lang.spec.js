/// <reference path="../../../../../Areas/LiveCasinoSetting/Scripts/core/live-casino-lang.js" />
/// <reference path="../../../../../Areas/LiveCasinoSetting/Scripts/core/language/LiveCasinoLang.js" />
/// <reference path="../../../../../Areas/LiveCasinoSetting/Scripts/core/language/LiveCasinoLang.en-US.js" />
/// <reference path="../../../../../Areas/LiveCasinoSetting/Scripts/core/language/LiveCasinoLang.zh-CN.js" />
/*global describe, it, language, expect, it*/
describe('Livecasino language', function () {
    'use strict';
    it('should get language value from key', function () {
        //// Given 

        //// When
        var resultString = language.getString("_101");

        //// Then
        expect(resultString).toEqual("Commission: updated successfully.");
    });

    it('should get default language when current language has no information', function () {
        //// Given
        language.setLanguageCode('zh-CN');

        //// When
        var resultString = language.getString("_101");

        //// Then
        expect(resultString).toEqual("佣金: 修改成功");
    });

    it('should get key if there is no langauge', function () {
        //// When
        var resultString = language.getString("_1011");

        //// Then
        expect(resultString).toEqual("_1011");
    });
});