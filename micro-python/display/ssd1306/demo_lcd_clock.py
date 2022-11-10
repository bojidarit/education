import machine
import ssd1306
import font
import time
import manageWiFi as net


lcd_width = 128 # pixels
lcd_height = 64 # 0.91 inch with 32 and 0.96 inch with 64 pixels
lcd_address = 0x3c

scl = machine.Pin(22, machine.Pin.OUT, machine.Pin.PULL_UP)
sda = machine.Pin(21, machine.Pin.OUT, machine.Pin.PULL_UP)

i2c = machine.SoftI2C(scl=scl, sda=sda)
oled = ssd1306.SSD1306_I2C(lcd_width, lcd_height, i2c, addr=lcd_address)
f_oled = font.Font(oled)

#-------------------------------------------------------------------------------------


def font_show_text(msg, x, y, f, clr):
    if clr:
        oled.fill(0)
    f_oled.text(msg, x, y, f)
    f_oled.show()

def clear_display():
    oled.fill(0)
    oled.show()

def leading_zeroes(num, zeroes=2):
    return ('{:0'+str(zeroes)+'d}').format(num)


oled.invert(False)
text = "Connecting Wi-Fi..."
print(text)
font_show_text(text, 3, 3, 16, True)
# time.sleep(1)

wifi_result = net.connect('mapex', 'zdraveimapex')
text = "Wi-Fi Connected: " + str(wifi_result);
print(text)
oled.invert(True)
font_show_text("Wi-Fi Connected", 3, 3, 8, True)
font_show_text(str(wifi_result), 3, 12, 16, False)
# time.sleep(1)

#-------------------------------------------------------------------------------------

import ntptime

text = "Querying NTP..."
print(text)
font_show_text(text, 3, 29, 16, False)
# time.sleep(1)

ntptime.settime() # this queries the time from an NTP server

text = "NTP: Time set"
print(text)
font_show_text(text, 3, 46, 16, False)
# time.sleep(1)

#-------------------------------------------------------------------------------------

days = {0:"Monday", 1:"Tuesday", 2:"Wednesday", 3:"Thursday", 4:"Friday", 5:"Saturday", 6:"Sunday"}
# months = {1:"Jan", 2:"Feb", 3:"Mar", 4:"Apr", 5:"May", 6:"Jun", 7:"Jul", 8:"Aug", 9:"Sepr", 10:"Oct", 11:"Nov", 12:"Dec"}
months = {1:"January", 2:"February", 3:"March", 4:"April", 5:"May", 6:"June", 7:"July", 8:"August", 9:"September", 10:"October", 11:"November", 12:"December"}

def show_time(local_time, colon):
    colon_txt = ":" if colon else " "
    time_str = leading_zeroes(local_time[3]) + colon_txt + leading_zeroes(local_time[4])
    font_show_text(time_str, 48, 36, 32, False)

def show_date(local_time, delimitter = "."):
    oled.fill(0)
    date_str = str(local_time[0]) + delimitter + months[local_time[1]] + delimitter + leading_zeroes(local_time[2])
    font_show_text(days[local_time[6]], 1, 1, 16, False)
    font_show_text(date_str, 1, 18, 16, False)

def date_changed(old_time, new_time):
    return old_time[0] != new_time[0] or old_time[1] != new_time[1] or old_time[2] != new_time[2]

def time_changed(old_time, new_time):
    return old_time[3] != new_time[3] or old_time[4] != new_time[4]


oled.invert(False)
oled.fill(0)
print("Now showing time in a infinite loop...")

utc_offset = 2 * 60 * 60   # East Europe Time Zone
local_time = time.localtime(1)
while True:
    old_time = local_time
    local_time = time.localtime(time.time() + utc_offset);
    if date_changed(old_time, local_time):
        show_date(local_time)
    if time_changed(old_time, local_time):
        show_time(local_time, True)
    time.sleep_ms(500)

