import manageWiFi as net
print("Connected: ", net.connect('mapex', 'zdraveimapex'))

import ntptime
print("Setting time from NTP server...")
ntptime.settime() # this queries the time from an NTP server

import tm1637
import machine
import time

dio = machine.Pin(21)
clk = machine.Pin(22)

# rtc = machine.RTC()
# rtc.datetime((2022, 11, 5, 5, 23, 33, 0, 0))
# rtc.datetime((YYYY, MM, DD, WD, HH, MM, SS, MS))
# WD 1 = Monday
# WD 7 = Sunday

UTC_OFFSET = 2 * 60 * 60   # East Europe Time Zone

tm = tm1637.TM1637(clk = clk, dio = dio)
tm.brightness(0)

print("Display initialized.")

colon = True
while True:
    local_time = time.localtime(time.time() + UTC_OFFSET);
    tm.numbers(local_time[3], local_time[4], colon)
    time.sleep_ms(500)
    colon = not colon
