import time

# Notes: Both LEDs operate in “inverted” mode, with regard to the pin levels.
# When the pin is HIGH, the LED is off; When the pin is LOW, the LED is on.

led2 = machine.Pin(2, machine.Pin.OUT) # GPIO2 - On the ESP-12 module’s PCB
led16 = machine.Pin(16, machine.Pin.OUT) # GPIO16 - On the NodeMCU PCB

flag = True
for i in range(0, 6):
    led2.value(flag)
    flag = not flag
    led16.value(flag)
    time.sleep_ms(500)

flag = True
for i in range(0, 6):
    led2.value(flag)
    led16.value(flag)
    flag = not flag
    time.sleep_ms(300)

led2.value(1)
led16.value(1)
