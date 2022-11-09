import machine
import ssd1306
import font
import time
import manageWiFi as net


lcd_width = 128 # pixels
lcd_height = 64 # 0.91 inch with 32 and 0.96 inch with 64 pixels
lcd_address = 0x3c


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


scl = machine.Pin(22, machine.Pin.OUT, machine.Pin.PULL_UP)
sda = machine.Pin(21, machine.Pin.OUT, machine.Pin.PULL_UP)

i2c = machine.SoftI2C(scl=scl, sda=sda)
oled = ssd1306.SSD1306_I2C(lcd_width, lcd_height, i2c, addr=lcd_address)
f_oled = font.Font(oled)

oled.invert(True)

#-------------------------------------------------------------------------------------

wifi_result = net.connect('mapex', 'zdraveimapex')
text = "Wi-Fi Connected: " + str(wifi_result);
print(text)
font_show_text("Wi-Fi Connected", 3, 3, 8, True)
font_show_text(str(wifi_result), 3, 12, 16, False)
# time.sleep(1)

#-------------------------------------------------------------------------------------

import ntptime
ntptime.settime() # this queries the time from an NTP server
text = "Time set <- NTP"
print(text)
font_show_text(text, 3, 30, 16, False)
# time.sleep(1)

#-------------------------------------------------------------------------------------

oled.invert(False)
print("Now showing time in a infinite loop...")


def show_time(local_time, colon):
    colon_txt = ":" if colon else " "
    time_str = leading_zeroes(local_time[3]) + colon_txt + leading_zeroes(local_time[4])
    font_show_text(time_str, 48, 36, 32, False)

def show_date(local_time, delimitter = "."):
    days = {0:"Monday", 1:"Tuesday", 2:"Wednesday", 3:"Thursday", 4:"Friday", 5:"Saturday", 6:"Sunday"}
    # months = {1:"Jan", 2:"Feb", 3:"Mar", 4:"Apr", 5:"May", 6:"Jun", 7:"Jul", 8:"Aug", 9:"Sepr", 10:"Oct", 11:"Nov", 12:"Dec"}
    months = {1:"January", 2:"February", 3:"March", 4:"April", 5:"May", 6:"June", 7:"July", 8:"August", 9:"September", 10:"October", 11:"November", 12:"December"}

    date_str = str(local_time[0]) + delimitter + months[local_time[1]] + delimitter + leading_zeroes(local_time[2])
    font_show_text(days[local_time[6]], 1, 1, 16, False)
    font_show_text(date_str, 1, 18, 16, False)

colon = True
while True:
    oled.fill(0)
    utc_offset = 2 * 60 * 60   # East Europe Time Zone
    local_time = time.localtime(time.time() + utc_offset);
    show_date(local_time)
    show_time(local_time, colon)
    time.sleep_ms(500)
    colon = not colon


