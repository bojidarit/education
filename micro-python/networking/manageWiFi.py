import network;
import time

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

def connect(ssid, key, number_of_attempts = 15):
    counter = 0;

    # STA_IF - Station Mode
    wifi = network.WLAN(network.STA_IF)

    if wifi.active():
        if wifi.isconnected():
            print("Already connected to Access-Point [" + wifi.config('essid') + "].")
            return True
    else:
        print("Activating Wi-Fi...")
        wifi.active(True)

    wifi.connect(ssid, key)

    if not wifi.isconnected():
        print('Connetcting to Access-Point [' + ssid + ']...')
        while (not wifi.isconnected() and counter < number_of_attempts):
            print(str(number_of_attempts - counter - 1), decode_status(wifi.status()))
            counter = counter + 1
            time.sleep(1)

    print("-" * 36)

    if wifi.isconnected():
        print("Connected to Access-Point [" + ssid + "].")
        print("('IP', 'subnet mask', 'gateway', 'DNS'):", wifi.ifconfig())
        print("RSSI:", wifi.status("rssi"), " with max transmit power ", wifi.config('txpower'), "dBm")
        return True
    else:
        print("Still no connection after " + str(number_of_attempts) + " attempt(s)")
        return False

def disconnect(decativate = True):
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
