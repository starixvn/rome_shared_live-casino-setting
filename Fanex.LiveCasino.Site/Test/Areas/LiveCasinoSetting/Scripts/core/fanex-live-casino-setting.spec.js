/* File Created: July 21, 2014 */
/// <reference path="../../../../../Areas/LiveCasinoSetting/Scripts/libs/angular.js" />
/// <reference path="../../../../../Areas/LiveCasinoSetting/Scripts/libs/angular-sanitize.js" />
/// <reference path="../../../../../Areas/LiveCasinoSetting/Scripts/core/fanex-live-casino-setting.js" />
/*global describe, it, expect*/
describe('Fanex.Core', function () {
    'use strict';
    it('should be clone object.', function () {
        //// Given
        var needBeCloneObject = {
            propertyA: 1,
            propertyB: '1',
            subObject: {
                propertyC: 1
            }
        };
        //// When
        var clonedObject = fanex.cloneObject(needBeCloneObject);

        //// Then
        expect(clonedObject).toEqual(needBeCloneObject);
    });

    it('should not contains clone function', function () {
        //// Given
        var needBeCloneObject = {
            clone: null
        };

        //// When
        var clonedObject = fanex.cloneObject(needBeCloneObject);

        //// Then
        expect(clonedObject.clone).toBeUndefined();
    });

    it('should round number with 2 decimals', function () {
        //// Given
        var notRoundedNumber = 1.22222;

        //// When
        var roundedNumber = fanex.roundNumber(notRoundedNumber, 2);

        //// Then
        expect(roundedNumber).toEqual(1.22);
    });

    it('should create range from 0.1 to 0.2 with 2 decimals', function () {
        //// Given
        var from = 0.1,
            to = 0.2,
            decimals = 2,
            current = 0.1,
            gap = 0.01;

        //// When
        var options = fanex.createOptionsFromRange(current, from, to, gap, decimals);

        //// Then
        expect(options.length).toEqual(11);
        expect(options[options.length - 1].value).toEqual(current);
        expect(options[0].value).toEqual(to);
    });

    it('should create range from 0.1 to 0.2 and current must lower than range', function () {
        //// Given
        var from = 0.1,
            to = 0.2,
            decimals = 2,
            current = 0.01,
            gap = 0.01;

        //// When
        var options = fanex.createOptionsFromRange(current, from, to, gap, decimals);

        //// Then
        expect(options.length).toEqual(12);
        expect(options[options.length - 1].value).toEqual(current);
        expect(options[0].value).toEqual(to);
    });

    it('should create range from 0.1 to 0.2 and current must lower than range', function () {
        //// Given
        var from = 0.1,
            to = 0.2,
            decimals = 2,
            current = 0.21,
            gap = 0.01;

        //// When
        var options = fanex.createOptionsFromRange(current, from, to, gap, decimals);

        //// Then
        expect(options.length).toEqual(12);
        expect(options[0].value).toEqual(current);
    });
});

