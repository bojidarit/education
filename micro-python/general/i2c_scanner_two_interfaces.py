import uos
import usys
import machine
import time
import ssd1306


def print_line():
    print('-' * 63)
    

print_line()
print(usys.implementation[0], uos.uname()[3], "\nruns on", uos.uname()[4])
print_line()


scl_pin_0 = 22
sda_pin_0 = 21

scl_pin_1 = 18
sda_pin_1 = 19

lcd_width = 128 # pixels
lcd_height = 64 # 0.91 inch with 32 and 0.96 inch with 64 pixels

# i2c_0 = machine.SoftI2C(scl=machine.Pin(scl_pin_0), sda=machine.Pin(sda_pin_0), freq=400000)
# i2c_1 = machine.SoftI2C(scl=machine.Pin(scl_pin_1), sda=machine.Pin(sda_pin_1), freq=400000)

# Hardware I2C, using default pins
i2c_0 = machine.I2C(0) # scl=18, sda=19
i2c_1 = machine.I2C(1) # scl=25, sda=26


def scan_for_devices(i2c, title):
    print(title + " I2C:", i2c)
    print('Scan I2C bus...')
    devices = i2c.scan()
    if len(devices) == 0:
      print("No I2C devices!")
    else:
      print('I2C devices found:', len(devices))
      for device in devices:  
        print("Decimal address: ", device, " | Hexa address: ", hex(device))


scan_for_devices(i2c_0, "Default")

print_line()

scan_for_devices(i2c_1, "Secondary")

print_line()

