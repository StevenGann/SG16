# SG16 v1.1

**v1.1:**

- Transitioning from a memory-mapped IO model to a data-bus model
- Eliminating internal RAM (a la microcontroller) in favor of an external RAM interface
- Heavier focus on future hardware implementation
- Undoing a LOT of the emulator core

# Hardware Description

**CPU:**

- 16-bit word size
- External RAM with 8-bit parallel interface and paging space, and 16-bit address space
- External EEPROM with 16-bit parallel interface and address space for bootloader
- 16-bit parallel data bus for peripherals, with 8-bit address space
- Two independent UARTs with seperate buffers

**RAM:**

- 16 address lines
- 8 data I/O lines
- CAS
- RAS
- Write-Enable
- Parity CAS
- Parity Out
- Parity In
	
	The interface is designed with 30-pin or 72-pin SIMM RAM in mind, but could probably be adapted to any parallel RAM or EEPROM.
	
**Peripheral Bus:**

- 8 address lines
- 8 configuration lines (bidirectional)
- 16 data I/O lines (bidirectional)
	
	The peripheral bus is designed with a backplane/card system in mind. The 8 address lines are set by the CPU to select a card on the bus. The 8 configuration lines are arbitrary and device-specific, providing an additional byte of data bandwidth or space for handshaking, parity, etc.

**Experimental:**

**Instruction Cache**
SG16, as it currently stands, has no cache model at all. This is mostly because RAM latency isn't an issue for the intended application. This doesn't mean it couldn't benefit from a simple instruction cache, especially if it could allow more efficient ALU operation. For use with look-ahead and for speeding up tight instruction loops, a simple queue of recent and upcoming instructions would be sufficient. As instructions are read into the ALU, the would be copied into the queue simultaneously. If a GOTO/GOSUB/flow control statement redirect the Program Counter, the cache manager would continue loading instructions into the queue so the ALU wouldn't have to wait when it resumed moving forward. The biggest concern is with flow control statements that move the Program Counter outside of the existing cache, which would mean cache fragmentation, which would mean some form of garbage collection or defragmentation and a way of indexing the cache beyond a simple offset.
	
The simplest solution so far is to just purge the cache anytime execution flow strays beyond the scope of the existing cache. This means the ALU would be waiting on RAM access until the cache rebuilds, but cache misses are inevitable anyway and a simple cache is better than none.

**Multiple Instruction, Multiple Data**
I have been sketching out a system for executing multiple instructions simultaneously with a simple cache look-ahead system. So far, I have two solutions that I am considering:
	
- **Double-wide instruction combinations in the ALU**. The fully-integrated approach is to implement not just individual instructions in the ALU, but also whole sequences of instructions which can be executed in parallel. In addition to MOVE could be MOVE/MOVE, MOVE/ADD, etc. This significantly increases the size and complexity of the ALU, making for more complex implementation but simpler architecture. The greatest benefit is that merging of instructions into combos could be performed by the assembler, combining instructions into hybrid opcodes.
	
- **Multiple ALUs**. The tightly-integrated approach is to supplement the ALU with a second (or third or forth...) ALU that reads and executes the next instruction independently *IF* it can confirm that it is not dependent on the result of the previous instruction. The greatest benefit of this approach is that it is effectively transparent outside of the ALU, cache, and RAM.
	
Either way, my biggest hurdle for either solution is devising a cache that can be accessed by multiple ALUs simultaneously, reading and writing. Furthermore, I'll need to design the RAM, ROM, and register interfaces for multiple access also. Until I solve this problem, I will not go any further with multiple execution.

**Multiprocessing**
In a similar vein as multiple execution, I'm trying to picture how multiple CPUs (or a single multi-core CPU) could operate. A master/slave interface for a secondary coprocessor would be simple enough, but it's hard to imagine how it would be beneficial.

For a couple years I've kicked around the idea of building a CPU from scratch out of discrete logic gates, but mostly ruled it out as impractical. Instead, I'm working on building an emulator while designing the CPU architecture from the Assembly language level down and then up. As the project has progressed, I've discovered flaws in my designs and learned from experience why classic architectures (486, 68k, Z80) made certain choices.

- [ ] Define Assembly language specification
- [x] Define processor architecture (in flux)
- [ ] Assembler
- [ ] Disassembler
- [ ] Emulator core
- [x] Emulator GUI
- [ ] Peripheral emulators
- [ ] Software tools (C, BASIC compilers)
- [ ] Embedded hardware emulation (Possibly on an ARM MCU)
- [ ] Physical prototype

**Emulator GUI Features**
- [ ] Core emulation
- [x] Live view of registers
- [ ] Peripheral bus emulation
- [ ] Disassembled view of program code
- [ ] Loading multiple ROMs into arbitrary locations
- [ ] Live register editing
- [ ] Live Assembly editing

**Peripherals**
- [ ] Serial terminal
- [ ] Hardware COM ports
- [ ] HDD/Floppy controller
- [ ] Display adapter
- [ ] Graphics accellerator (Probably its own project!)
- [ ] Network adapter
- [ ] Audio controller

As the pure software emulation grows, I am planning out approaches to hardware implementation. A low-end ARM microprocessor should be sufficient to implement an embedded emulator. I have been experimenting with a cheap Altera CPLD, and I hope to work my way up to a full implementation of SG16 on a proper FPGA. A full ASIC is the end goal, but that's stretching into fantasy.

# Implemented Instructions

**The instruction set is currently in flux as I plan out the transition to external RAM.**

**Dual Argument**
- **R** Register
- **L** Literal
- **A** Absolute RAM addressing
- **I** Indirect RAM addressing

|      | R R | L R | A R | I R | R A | L A | A A | I A | R I | L I | A I | I I |
|-----:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|
| MOVE |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |       
| SWAP |  ✔  | N/A |  ✔  |  ✔  |  ✔  | N/A |  ✔  |  ✔  |  ✔  | N/A |  ✔  |  ✔  |       
|   OR |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |      
|  NOR |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |    
|  XOR |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |    
| XNOR |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |       
|  AND |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |      
| NAND |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  | 
|  ADD |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |    
| SUBT |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |      
| MULT |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |       
| DIVI |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |       
| EXPO |     |     |     |     |     |     |     |     |     |     |     |     |       
| COMP |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |  ✔  |   
| ROMR |     |     |     |     |     |     |     |     |     |     |     |     |       
| ROMW |     |     |     |     |     |     |     |     |     |     |     |     |       

**Single Argument**
- **R** Register
- **L** Literal
- **A** Absolute RAM addressing
- **I** Indirect RAM addressing

|      | R | L | A | I |
|-----:|:-:|:-:|:-:|:-:|
|  REF | ✔ | ✔ | ✔ | ✔ |
| ROTL | ✔ |N/A| ✔ | ✔ |
| ROTR | ✔ |N/A| ✔ | ✔ |
|  NOT | ✔ |N/A| ✔ | ✔ |
| INCR | ✔ |N/A| ✔ | ✔ |
| DECR | ✔ |N/A| ✔ | ✔ |
| GOTO | ✔ | ✔ | ✔ | ✔ |
| EVAL | ✔ | ✔ | ✔ | ✔ |
| JMPZ | ✔ | ✔ | ✔ | ✔ |
| JMGZ | ✔ | ✔ | ✔ | ✔ |
| JMLZ | ✔ | ✔ | ✔ | ✔ |
| GSUB | ✔ | ✔ | ✔ | ✔ |
| JMPE | ✔ | ✔ | ✔ | ✔ |
| JMPG | ✔ | ✔ | ✔ | ✔ |
| JMPL | ✔ | ✔ | ✔ | ✔ |
| ENQU | ✔ | ✔ | ✔ | ✔ |
| DEQU |  |  |  |  |
| PUSH | ✔ | ✔ | ✔ | ✔ |
|  POP |  |  |  |  |
| TXD0 | ✔ | ✔ | ✔ | ✔ |
| RXD0 |  |  |  |  |
| TXD1 | ✔ | ✔ | ✔ | ✔ |
| RXD1 |  |  |  |  |

NOTE: Not all STAT flags are fully implemented yet.

**No Arguments**

|      | ✔ | Notes |
|-----:|:-:|:------|
| NULL | ✔ |
| START | ✔ |
|  END | ✔ |
| RTRN | ✔ |

**Background**
I've worked at the lowest levels with PIC, 68000, MSP-430, Atmega, x86, and ARM processors, working with all of those in Assembly and C. PIC, 68000, and MSP-430 were my first introductions to Assembly and processor architectures, and the SG16 reflects that in many ways.

**Assembly Language**
Defining the Assembly language first was a convenient way to define how the SG16 processor functions, and what features it offers. I started by defining basic memory operations like MOVE and SWAP, and then boolean logic operations like AND and OR. From there I kept expanding until I was confident the instruction set was complete enough to write useful applications. As I flesh out the assembler and emulator, plan out compilers and bootloaders, and research hardware implementations, I keep discovering shortcoming and devising improvements.

Addressing modes were a challenge. I was inspired by the indirect addressing mode used by the MSP-430, but when it came to planning the machine code I couldn't think of a simple way to represent it. I could have looked up how the MSP-430 does it, but I wanted the architecture to follow my style of doing things instead of copying someone else's implementation. As such, I solved it by adding a register for the reference address and indirect addressing simply specifies the offset.

**Assembler**
This is simpler than I had expected. The Assembler program is little more than an interface, with the actual assembly being performed by the ASM class of the SG16 library. Simply put, the assembler parses Assembly language code and converts it to machine code, outputing the result as a binary ROM file which will later be opened and executed by the emulator. The exercise brought me to the stunningly obvious realization than an emulator is just an interpriter for machine code.

**Emulator**
The core emulator is certainly the most challenging part of the project, and I am approaching it with strong OOP design principles. The imlementation is fairly abstract, contained within a DLL for easy adaptation into future frontends and UIs. Operations on registers are the simplest in terms of logic, though operations on RAM have already resulted in some issues regarding endianess, and a reminder of how important it is to keep that in mind. 

**Emulator UI**
The bulk of the core emulator's facilities, such as registers and RAM, are public to allow any application to easily read and display the contents, as well as providing some interface for pausing and stepping execution and connecting emulated peripherals. The first UI uses WinForms and is inspired heavily by the classic Easy68k emulator, with a traditional WYSIWYG interface and low-level display of registers and RAM. With a convenient UI for testing, debugging the core emulator has become much more efficient.

**Internal Peripherals**
The SG16 architecture includes two independent UARTs with their own TX and RX buffers, plus a flexible external memory bus with a 32-bit addressing space.

**Peripheral Bus**
In the interest of future hardware implementation, SG16 has moved from a memory-mapped IO model to a data bus model. The peripheral bus is the most generic interface, intended for a backplane or similar parallel system. The bus is 6 bytes wide, with the first byte serving as a device ID selection. Bytes 2 through 5 are for bidirectional data, while the 6th byte serves as dedicated feedback from the selected device.

**Software**
Assembly is the first step, but writing useful or interesting software in Assembly is difficult and tedious. BASIC is probably the simplest language I know, in terms of compiling, and from there more sophisticated tools can be built. A proper C compiler would be fantastic, but the complexity of C syntax makes it much more challenging than BASIC. A port of CP/M would also be a major accomplishment, though I am planning out a more humble bootloader.