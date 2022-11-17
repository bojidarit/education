import manageWiFi as net

# ESP32
led_pin = 2
# net.disconnect(led_pin_no=led_pin, led_flag=False)
net.connect_in_loop("mapex", "zdraveimapex", led_pin_no=led_pin, led_flag=True, led_stay_on=False)

# ESP8266
# led_pin = 16
# net.disconnect(led_pin_no=led_pin, led_flag=True)
# net.connect_in_loop("mapex", "zdraveimapex", led_pin_no=led_pin, led_flag=False, led_stay_on=False)

