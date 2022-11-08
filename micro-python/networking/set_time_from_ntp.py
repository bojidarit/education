import manageWiFi as net
print("Connected: ", net.connect('mapex', 'zdraveimapex'))
# net.disconnect(True)

import ntptime
print("Setting time from NTP server...")
ntptime.settime() # this queries the time from an NTP server

import time
days = {0:"Monday", 1:"Tuesday", 2:"Wednesday", 3:"Thursday", 4:"Friday", 5:"Saturday", 6:"Sunday"}
months = {1:"January", 2:"February", 3:"March", 4:"April", 5:"May", 6:"June", 7:"July", 8:"August", 9:"September", 10:"October", 11:"November", 12:"December"}
UTC_OFFSET = 2 * 60 * 60   # East Europe Time Zone
local_time = time.localtime(time.time() + UTC_OFFSET);
(year, month, mday, hour, minute, second, weekday, yearday) = local_time
print('-' * 77)
# print("Local time in format (year, month, month-day, hour, min, second, weekday [Monday=0], year-day): ", local_time)
print(str(mday) + "." + months[month] + "." + str(year) + " " + str(hour) + ":" + str(minute) + ":" + str(second), end=", ")
print(days[weekday] + ", " + str(yearday) + " day of the year.")


def leadingZeroes(num):
    return '{:02d}'.format(num)

print(leadingZeroes(local_time[3]) + ":" + leadingZeroes(local_time[4]))