# MicroPython i2c scanner
# Return decimal and hexa adress of each i2c device
# https://projetsdiy.fr - https://diyprojects.io (dec. 2017)
# https://gist.github.com/projetsdiy/f4330be62589ab9b3da1a4eacc6b6b1c

SCL_PIN = 9 # S2-Mini: 35, 9; Regular ESP32: 22
SDA_PIN = 8 # S2-Mini: 34, 8; Regular ESP32: 21

import machine
i2c = machine.SoftI2C(scl=machine.Pin(SCL_PIN), sda=machine.Pin(SDA_PIN))

print("S-Clock Pin =", SCL_PIN)
print("S-Data  Pin =", SDA_PIN)
print('Scan I2C bus...')
devices = i2c.scan()

if len(devices) == 0:
  print("No I2C device !")
else:
  print('I2C devices found:',len(devices))

  for device in devices:  
    print("Decimal address: ",device," | Hexa address: ",hex(device))