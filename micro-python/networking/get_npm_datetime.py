import manageWiFi as net

net.disconnect(True)
net.connect_in_loop("mapex", "zdraveimapex", led_pin_no=15)

import ntptime
import time
import localtimeUtils

print("-" * 36)
try:
    # this queries the time from an NTP server
    print("Querying NTP Server...")
    ntptime.settime()
except:
    print("NTP's settime() rised an error, querying again...")
    time.sleep(1)
    ntptime.settime()

print("Time from NTP server", end=": ")
ntp_time = ntptime.time()
# print(ntp_time)
localtimeUtils.print_datetime(ntp_time, 2)
