#hello_world.asm
#The classic "Hello world!" implemented in SG16 Assembly
#Assumes a terminal input is attached at @FF00

START #Start of the program

MOVE x0048 @FF00 # H
MOVE x0065 @FF00 # e
MOVE x006C @FF00 # l
MOVE x006C @FF00 # l
MOVE x006F @FF00 # o
MOVE x0020 @FF00 # <space>
MOVE x0077 @FF00 # w
MOVE x006F @FF00 # o
MOVE x0072 @FF00 # r
MOVE x006C @FF00 # l
MOVE x0064 @FF00 # d
MOVE x0021 @FF00 # !

END 