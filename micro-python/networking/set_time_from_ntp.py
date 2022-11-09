import manageWiFi as net
print("Connected: ", net.connect())
# net.disconnect(True)

#-----------------------------------------------------------------------------
print('-' * 77)

import ntptime
import time
import localtimeUtils

utc_offset_hours = 2

ntp_time = ntptime.time()
print("Time from NTP server", end=": ")
localtimeUtils.print_datetime(ntp_time, utc_offset_hours)

print("Seting time from NTP server...")
ntptime.settime() # this queries the time from an NTP server

local_time = time.time();

#-----------------------------------------------------------------------------
print('-' * 77)
print("Microcontroller local time", end=": ")
localtimeUtils.print_datetime(local_time, utc_offset_hours)
