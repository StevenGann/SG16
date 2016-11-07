# 2016-11-07: Updated for SG16 v1.1
#hello_world.asm
#The classic "Hello world!" implemented in SG16 Assembly
#Uses UART0

START				#Start of the program

MOVE x0000 UART0	# Set UART 0 to default configuration
TXD0 x4865 			# He
TXD0 x6C6C 			# ll
TXD0 x6F20 			# o<space>
TXD0 x776F 			# wo
TXD0 x726C 			# rl
TXD0 x6421 			# d!
TXD0 x0A 			# <new line>

END 				#End of the program