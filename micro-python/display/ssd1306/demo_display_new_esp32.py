import uos
import usys
import machine
import time
import ssd1306


def print_line():
    print('-' * 63)
    

LCD_WIDTH = 128
LCD_HEIGHT = 32

print_line()
print(usys.implementation[0], uos.uname()[3], "\nruns on", uos.uname()[4])
print_line()

oled_i2c = machine.I2C(0)
print("Default I2C_0:", oled_i2c)

oled_i2c_1 = machine.I2C(1)
print("Secondary I2C_1:", oled_i2c_1)

used_i2c = oled_i2c_1
used_i2c_id = str(oled_i2c_1)[4:5]

print_line()
try:
    oled_ssd1306 = ssd1306.SSD1306_I2C(LCD_WIDTH, LCD_HEIGHT, used_i2c)
    print("SSD1306 I2C_"+used_i2c_id+" address:", oled_ssd1306.addr, "/", hex(oled_ssd1306.addr))
    oled_ssd1306.text('Hello, World!', 0, 0, 1)
    oled_ssd1306.show()
except OSError as exc:
    print_line()
    print("OSError:", exc)
    if exc.errno == errno.ENODEV:
        print("No such device")
    print_line()

print('-= END =-')