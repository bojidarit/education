"use strict";

searchButton.addEventListener('click', searchWeather);

function searchWeather() {
    loadingDiv.style.display = 'block';
    weatherDiv.style.display = 'none';
    var cityName = cityText.value;

    if (cityName.trim().length == 0) {
        return alert("Enter city name including country initials like 'Sofia,bg'");
    }

    var apiKey = 'd4a88a154659516d2f739fb59badb188';
    var url = `http://api.openweathermap.org/data/2.5/weather?q=${cityName}&APPID=${apiKey}&units=metric`;

    var http = new XMLHttpRequest();

    http.open('GET', url);

    http.onreadystatechange = function () {
        if (http.readyState == XMLHttpRequest.DONE) {
            if (http.status === 200) {
                var data = JSON.parse(http.responseText);
                var weather = new Weather(cityName, data.weather[0].description);
                weather.temperature = data.main.temp;
                showWeatherData(weather);
            } else {
                alert('Error. XMLHttpRequest done with status ' + rest.status);
            }
        }
    };

    http.send();
}

function showWeatherData(weather) {
    weatherCity.textContent = weather.cityName;
    weatherDescription.textContent = weather.description;
    weatherTemperature.textContent = weather.temperature;

    loadingDiv.style.display = 'none';
    weatherDiv.style.display = 'block';
}