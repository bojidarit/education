import network
import time
import machine

# Led Pin: '2' for general ESP 2, '15' for S2-Mini. '2' and '16' on the ESP8266 (Inverted!!!)

def decode_status(status_code):
    if status_code == network.STAT_IDLE:
        return 'No connection or activity'
    
    if status_code == network.STAT_CONNECTING:
        return 'Connection is progress'
    
    if status_code == network.STAT_WRONG_PASSWORD:
        return 'Incorrect network key'
    
    if status_code == network.STAT_NO_AP_FOUND:
        return 'No access point replied to the connection request'
    
    if status_code == network.STAT_GOT_IP:
        return 'Successfully connected and the IP is obtained'
        
    return 'Status Code: ' + str(status_code)


def connect(ssid, key, led_pin_no=2, led_flag=True, number_of_attempts=10):
    led = machine.Pin(led_pin_no, machine.Pin.OUT)
    
    counter = 0;

    # STA_IF - Station Mode
    wifi = network.WLAN(network.STA_IF)

    if wifi.active():
        if wifi.isconnected():
            led.value(1)
            print("Already connected to Access-Point [" + wifi.config('essid') + "].")
            print("('IP', 'subnet mask', 'gateway', 'DNS'):", wifi.ifconfig())
            return True
    else:
        print("Activating Wi-Fi...")
        wifi.active(True)

    wifi.connect(ssid, key)

    if not wifi.isconnected():
        print('Connetcting to Access-Point [' + ssid + ']...')
        flag = led_flag
        while (not wifi.isconnected() and counter < number_of_attempts):
            print(str(number_of_attempts - counter - 1), decode_status(wifi.status()))
            counter = counter + 1
            time.sleep(1)
            led.value(flag)
            flag = not flag

    print("-" * 36)

    if wifi.isconnected():
        print("Connected to Access-Point [" + ssid + "].")
        print("('IP', 'subnet mask', 'gateway', 'DNS'):", wifi.ifconfig())
        # print("RSSI:", wifi.status("rssi"), " with max transmit power ", wifi.config('txpower'), "dBm") # Do not wotk on S2-Mini
        led.value(led_flag)
        return True
    else:
        print("Still no connection after " + str(number_of_attempts) + " attempt(s)")
        led.value(not led_flag)
        return False


def disconnect(led_pin_no=2, led_flag=False, decativate=True):
    led = machine.Pin(led_pin_no, machine.Pin.OUT)
    led.value(led_flag)
    wifi = network.WLAN(network.STA_IF)
    
    if (not wifi.active()):
        print("Wi-Fi is not active.")
        return

    if not wifi.isconnected():
        print("Wi-Fi is Not connected.")
    else:
        print("Disconnecting Wi-Fi...")
        wifi.disconnect()

    if decativate:
        print("Deactivating Wi-Fi...")
        wifi.active(False)
    else:
        print("... Wi-Fi is active: " + str(wifi.active()))


def connect_in_loop(ssid, key, led_pin_no=2, interations_count=-1, led_stay_on=False, led_flag=True):
    result = False
    counter = 1
    while (not result) or (interations_count > 0 and counter <= interations_count):
        result = connect(ssid, key, led_pin_no)
        print("-" * 36)
        print("Connected:", result, "(Loop: " + str(counter) + ")")
        counter += 1

    if result:
        time.sleep(1)
        led = machine.Pin(led_pin_no, machine.Pin.OUT)
        led.value(not led_flag)
        flag = led_flag
        for i in range (0, 6):
            time.sleep_ms(300)
            led.value(flag)
            flag = not flag

    led.value(led_flag if led_stay_on else not led_flag)

