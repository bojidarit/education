import machine
import ssd1306
import font
import time

def font_text(msg, x, y, f, clr):
    if clr:
        oled.fill(0)
    f_oled.text(msg, x, y, f)
    f_oled.show()

def clear_display():
    oled.fill(0)
    oled.show()

def print_demo(invert):
    oled.invert(invert)
    font_text("12:34", 3, 3, 8, 1)
    font_text("12:34", 3, 12, 16, 0)
    font_text("12:34", 55, 0, 24, 0)
    font_text("12:34", 23, 30, 32, 0)


lcd_width = 128 # pixels
lcd_height = 64 # smal (0.91) is 32 and the bigger (0.96) is 64 pixels
lcd_address = 0x3c

scl = machine.Pin(22, machine.Pin.OUT, machine.Pin.PULL_UP)
sda = machine.Pin(21, machine.Pin.OUT, machine.Pin.PULL_UP)

i2c = machine.SoftI2C(scl=scl, sda=sda)
oled = ssd1306.SSD1306_I2C(lcd_width, lcd_height, i2c, addr=lcd_address)
f_oled = font.Font(oled)


print_demo(False)

time.sleep(5)

print_demo(True)

time.sleep(5)

oled.invert(False)
# clear_display()