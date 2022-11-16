import os

def df():
    s = os.statvfs('//')
    # print(s)
    mbs = (s[0]*s[3])/1048576
    return ('Free space\n{0} MB\n{1} KB'.format(mbs, mbs * 1024))

print(df())