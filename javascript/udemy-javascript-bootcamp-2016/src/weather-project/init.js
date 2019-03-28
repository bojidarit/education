"use strict";

var searchDiv = document.querySelector('#search');
var searchButton = searchDiv.querySelector('BUTTON');
var cityText = document.querySelector('#city');
//console.log(cityText);
cityText.placeholder = "Enter city like 'Sofia,bg'";

var loadingDiv = document.querySelector('#load');
var weatherDiv = document.querySelector('#weather');
var weatherCity = document.querySelector('#weatherCity');
var weatherDescription = document.querySelector('#weatherDescription');
var weatherTemperature = document.querySelector('#weatherTemperature');



