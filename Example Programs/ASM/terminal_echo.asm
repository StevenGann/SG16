#terminal_echo.asm
#Simple demonstration of input and output using the terminal.
#Stores bytes from the terminal into an array. When it recieves a '.',
#it echos the array back to the terminal. 

START 							#0000	#Start of the program

MOVE xFF00 RREF  				#0008	#Set RREF to the start address of the array
MOVE x0000 $0000 				#0010	#Clear start address of array
MOVE sR @FF00 					#0018 
MOVE se @FF00 					#0020 
MOVE sa @FF00 					#0028 
MOVE sd @FF00 					#0030 
MOVE sy @FF00 					#0038 
MOVE s: @FF00 					#0040 
MOVE x000A @FF00 # <new line> 	#0048 

#Read and store loop
EVAL @FF02  					#0050
JMPZ x0050  					#0058	#Wait for data in the terminal output
INCR $0000  					#0060	#Increment the value stored at the location RREF points to
MOVE @FF02 $RREF  				#0068	#Copy the contents of FF02 into the location RREF points to, offset by the value stored at RREF
COMP @FF02 s. 					#0070	#Check if the input is '.'
MOVE x0000 @FF02 				#0078	#Reset the input
JMPG x0050 						#0080	#Restart loop
JMPG x0050 						#0088	#Restart loop


#Print array to terminal loop
MOVE x0000 USR0 				#0090	#Using USR0 as counter variable
INCR USR0 						#0098	
MOVE $USR0 @FF00 				#00A0	#Copy array element to output
COMP USR0 $0000 				#00A8	
JMPL x0098 						#00B0	#Restart loop

MOVE x000A @FF00 # <new line>	#00B8

END								#00C0