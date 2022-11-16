# Source: https://randomnerdtutorials.com/esp32-esp8266-micropython-web-server/

# import correct socket libraty
try:
    import usocket as socket
except:
    import socket

# Enambles garbage collection
import gc
gc.collect()

import machine
import time
import manageWiFi as net

PORT_NUMBER = 80
LED_PIN = 15
MAX_QUEUED_CONNECTIONS_NUM = 5

net.disconnect()
net.connect_in_loop("mapex", "zdraveimapex", led_pin_no=LED_PIN)

# On-board led
led = machine.Pin(LED_PIN, machine.Pin.OUT)


def web_page():
    led_state = "ON" if led.value() == 1 else "OFF"
    html = """<html><head> <title>ESP Web Server</title> <meta name="viewport" content="width=device-width, initial-scale=1">
        <link rel="icon" href="data:,"> <style>html{font-family: Helvetica; display:inline-block; margin: 0px auto; text-align: center;}
        h1{color: #0F3376; padding: 2vh;}p{font-size: 1.5rem;}.button{display: inline-block; background-color: #e7bd3b; border: none;
        border-radius: 4px; color: white; padding: 16px 40px; text-decoration: none; font-size: 30px; margin: 2px; cursor: pointer;}
        .button2{background-color: #4286f4;}</style></head><body> <h1>ESP Web Server</h1>
        <p>GPIO state: <strong>""" + led_state + """</strong></p><p><a href="/?led=on"><button class="button">ON</button></a></p>
        <p><a href="/?led=off"><button class="button button2">OFF</button></a></p></body></html>"""
    return html      


def print_line(number=36):
    print('-' * number)


def manage_request(request):
    req_list = request.split('/')
    request_type = req_list[0]
    query_string = req_list[1]
    request_details = req_list[2]
    print("Request type:", request_type)
    print("Query string:", query_string)
    print("Request Details:", request_details)
    led_on = 'led=on' in query_string
    led_off = 'led=off' in query_string
    if led_on:
        print("Switching LED ON...")
        led.value(1)
    if led_off:
        print("Switching LED OFF...")
        led.value(0)


print_line()
print("Creating a socket...")
s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

# Bind accepts a tupple variable with the ip address, and port number
# In this case, the empty string refers to the localhost IP address (this means the ESP32 or ESP8266 IP address)
print("Binding to port number = " + str(PORT_NUMBER))
s.bind(('', PORT_NUMBER))

# Listening socket enables the server to accept connections.
# The argument specifies the maximum number of queued connections.
print("Listenning for requests with maximum number of queued connections = " + str(MAX_QUEUED_CONNECTIONS_NUM))
s.listen(MAX_QUEUED_CONNECTIONS_NUM)

ok = True
while ok:
    try:
        if gc.mem_free() < 102000:
          gc.collect()
        # Getting request
        print_line()
        conn, addr = s.accept()
        print('Got a connection from ' + str(addr))
        request = conn.recv(1024)
        request = str(request)
        # print('Content = ' + request)
        manage_request(request)
        # Returning the response page
        response = web_page()
        conn.send('HTTP/1.1 200 OK\n')
        conn.send('Content-Type: text/html\n')
        conn.send('Connection: close\n\n')
        conn.sendall(response)
        conn.close()
    except OSError as ex:
        print("OSError:", ex)
        print("Closing connection and terminating the Web server...")
        conn.close()
        ok = False
