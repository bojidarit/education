"use strict";

function Weather(cityName, description) {
    this.cityName = cityName;
    this.description = description.toUpperCase();
    this._temperature = 0.0;
    this._temperatureF = 0.0;
}

Object.defineProperty(Weather.prototype, 'temperature', {
    enumerable: true,

    get: function() {
        return this._temperatureF;
    },
    set: function(value) {
        this._temperature = value;
        var calculated = value * 1.8 + 32.0;
        this._temperatureF = calculated.toFixed(2) + 'F. (' + value.toFixed(2) + '°)';
    }
});