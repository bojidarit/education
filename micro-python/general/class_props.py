import machine

def props(cls):   
  return [i for i in cls.__dict__.keys() if i[:1] != '_']

properties = props(machine.Pin)
print(properties)