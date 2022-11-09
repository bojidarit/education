import manageWiFi as net
print("Connected: ", net.connect())
# net.disconnect(True)

import ntptime
import time
import localtimeUtils

ntp_time = ntptime.time()
# print(time.localtime(ntp_time))

print("Time from NTP server", end=": ")
localtimeUtils.print_datetime(ntp_time, 2)
