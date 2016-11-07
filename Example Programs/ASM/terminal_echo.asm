# 2016-11-07: Updated for SG16 v1.1
#terminal_echo.asm
#Simple demonstration of input and output using UART0.
#Stores bytes from the terminal into an array. When it recieves a '.',
#it echos the array back to the terminal. 

START 							# Start of the program

MOVE xFF00 RREF  				# Set RREF to the start address of the array
MOVE x00 $0000 					# Clear start address of array

MOVE x0000 UART0				# Set UART 0 to default configuration
TXD0 xRe
TXD0 xad
TXD0 xy:

#Read and store loop
READ_STORE_LOOP:
MOVE x0000 USR0					# Clear USR0
RXD0 USR0						# Copy UART0 input to USR0
EVAL USR0						# Evaluate the input
JMPZ READ_STORE_LOOP  			# If input was <null>, go to start of loop. If not...
INCR $0000  					# Increment the value stored at the location RREF points to
INCR $0000						# ...twice, because it's 2 bytes
MOVE USR0 $RREF  				# Copy the value into the end of the array, i.e. RREF + the value stored at $0000
COMP USR0.U s. 					# Check if the input's upper byte is "."
JMPG READ_STORE_LOOP			# If not, restart loop
COMP USR0.L s. 					# Check if the input's lower byte is "."
JMPG READ_STORE_LOOP			# If not, restart loop
MOVE x0000 $RREF				# Make sure the array is null-terminated


#Print array to terminal loop
MOVE x0000 USR0 				# Using USR0 as counter variable, so clear it
MOVE x0000 USR1					# Using USR1 as a buffer, so clear it too
PRINT_ARRAY_LOOP:
INCR USR0 						# Increment counter
MOVE $USR0.L USR1.L 			# Copy array element into USR1
INCR USR0 						# Increment counter
MOVE $USR0.L USR1.U 			# Copy array element into USR1
TXD0 USR1						# Transmit two bytes
NAND USR1.U USR1.L				# NAND upper and lower bytes of buffer and see if either is <null>
EVAL USR1.L						# Check if output was <null>
JMGZ PRINT_ARRAY_LOOP 			# If not, restart loop

TXD0 x0A 						# <new line>

END