# Scanner i2c en MicroPython | MicroPython i2c scanner
# Return decimal and hexa adress of each i2c device
# https://projetsdiy.fr - https://diyprojects.io (dec. 2017)
# https://gist.github.com/projetsdiy/f4330be62589ab9b3da1a4eacc6b6b1c


import machine
i2c = machine.SoftI2C(scl=machine.Pin(9), sda=machine.Pin(8))

print('Scan i2c bus...')
devices = i2c.scan()

if len(devices) == 0:
  print("No i2c device !")
else:
  print('i2c devices found:',len(devices))

  for device in devices:  
    print("Decimal address: ",device," | Hexa address: ",hex(device))