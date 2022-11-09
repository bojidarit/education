# Simple demonstration of 0.96/0.91 OLED in MicroPython
# Author: George Bantique, TechToTinker
# Date:   September 30, 2020
import ssd1306
import machine
import time

lcd_width = 128 # pixels
lcd_height = 32 # smal (0.91) is 32 and the bigger (0.96) is 64 pixels
lcd_address = 0x3c

scl = machine.Pin(22, machine.Pin.OUT, machine.Pin.PULL_UP)
sda = machine.Pin(21, machine.Pin.OUT, machine.Pin.PULL_UP)

led = machine.Pin(2, machine.Pin.OUT)

i2c = machine.SoftI2C(scl=scl, sda=sda, freq=400000)

oled = ssd1306.SSD1306_I2C(lcd_width, lcd_height, i2c, addr=lcd_address)

def print_text(msg, x, y, clr):
    if clr:
        oled.fill(0)
    oled.text(msg, x, y)
    oled.show()

def clear_display():
    oled.fill(0)
    oled.show()

oled.invert(True)

print_text('Hello', 10, 2, 0)
led.value(1)
time.sleep(1)

print_text('and Welcome', 20, 12, 0)
led.value(0)
time.sleep(1)

print_text('Earthlings ;-)', 10, 24, 0)
led.value(1)
time.sleep(1)
led.value(0)

prefix = 'Now sleeping'
counter = 0

while counter < 3:
    for i in range(4):
        print_text(prefix + (i * '.'), 0, 12, 1)
        led.value(1)
        time.sleep(0.3)
        led.value(0)

    counter = counter + 1
    
print_text('Bye Bye.', 0, 12, 1)
time.sleep(1)

oled.invert(False)
clear_display()
