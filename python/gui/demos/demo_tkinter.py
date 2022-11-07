# Source: https://realpython.com/python-gui-tkinter/

from platform import python_version
import tkinter as tk
from tkinter.constants import *

blueColor = "#3A80A8"
yellowColor = "#FFD43C"

win = tk.Tk()
win.title("TKInter Demo")
win.geometry("300x136")

frm_main = tk.Frame(
    master=win,
    relief=GROOVE,
    borderwidth=2)
frm_main.pack(
    fill=BOTH,
    expand=1,
    padx=3,
    pady=3)

text = "Hello World ;-)\nPython ver.: " + \
    str(python_version()) + "\nWith TKInter GUI ver.: " + str(tk.TkVersion)
tk.Label(
    frm_main,
    text=text,
    foreground=yellowColor,
    background=blueColor).pack(fill=X, expand=1)

text = "Enter some text here..."
ent_text = tk.Entry(
    master=frm_main,
    bg=yellowColor,
    fg=blueColor)
ent_text.insert(0, text)
ent_text.pack(fill=X, expand=1)


def btnCommand():
    msgBox = tk.Tk()
    msgBox.title("Text Entry content")
    msgBox.geometry("250x60")
    lbl_message = tk.Label(msgBox, text=ent_text.get())
    lbl_message.pack()
    btn_ok = tk.Button(master=msgBox, width=10, text="OK", command=msgBox.destroy)
    btn_ok.pack(side=BOTTOM)


tk.Button(
    master=frm_main,
    fg=yellowColor,
    bg=blueColor,
    text="Close",
    command=win.destroy).pack(side=BOTTOM, fill=X, expand=1)

tk.Button(
    master=frm_main,
    fg=yellowColor,
    bg=blueColor,
    text="Show text",
    command=btnCommand).pack(side=BOTTOM, fill=X, expand=1)

win.mainloop()
