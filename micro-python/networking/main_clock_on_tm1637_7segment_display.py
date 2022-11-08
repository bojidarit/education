import tm1637
import machine
import time

dio = machine.Pin(21)
clk = machine.Pin(22)

tm = tm1637.TM1637(clk = clk, dio = dio)
tm.brightness(0)


def tm_show(text):
    tm.show(text)
    time.sleep(1)


tm_show("init")

import manageWiFi as net
wifi_result = net.connect('mapex', 'zdraveimapex')
print("Connected: ", wifi_result)
if wifi_result:
    tm_show("ch01")
else:
    tm_show("er01")


import ntptime
print("Setting time from NTP server...")
ntptime.settime() # this queries the time from an NTP server
tm_show("ch02")

UTC_OFFSET = 2 * 60 * 60   # East Europe Time Zone

print("Display initialized.")
print("Showing time loop...")

colon = True
while True:
    local_time = time.localtime(time.time() + UTC_OFFSET);
    tm.numbers(local_time[3], local_time[4], colon)
    time.sleep_ms(500)
    colon = not colon
