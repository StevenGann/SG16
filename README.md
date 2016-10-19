# SG16

For a couple years, I've kicked around the idea of building a CPU from scratch out of discrete logic gates. I never got far because without an FPGA it just isn't a practical goal.

Instead, I'm working on building an emulator while designing the CPU architecture from the Assembly language level down and then up.

- [x] Define Assembly language specification
- [x] Define processor architecture
- [x] Assembler
- [x] Disassembler
- [ ] Emulator core
- [x] Emulator GUI
- [ ] Peripheral emulators
- [ ] Software tools (C, BASIC compilers)

**Emulator GUI Features**
- [x] Core emulation
- [x] Live view of registers and RAM
- [ ] Disassembled view of program code
- [ ] Loading multiple ROMs into arbitrary locations
- [ ] Live register and RAM editing
- [ ] Live Assembly editing

**Peripherals**
- [x] Serial terminal
- [ ] Hardware COM ports
- [ ] HDD/Floppy controller
- [ ] Display adapter
- [ ] Graphics accellerator (Probably its own project!)
- [ ] Network adapter
- [ ] Audio controller

#Implemented Instructions

**Dual Argument**
- **R** Register
- **L** Literal
- **A** Absolute RAM addressing
- **I** Indirect RAM addressing

|      | R R | L R | A R | I R | R A | L A | A A | I A | R I | L I | A I | I I |
|-----:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|
| MOVE |  ✔  |  ✔  |  ✔  |    |  ✔  |  ✔  |  ✔  |     |     |     |     |     |       
| SWAP |  ✔  | N/A |     |     |     | N/A |     |     |     | N/A |     |     |       
|   OR |  ✔  |     |     |     |     |     |     |     |     |     |     |     |       
|  NOR |  ✔  |     |     |     |     |     |     |     |     |     |     |     |       
|  XOR |  ✔  |     |     |     |     |     |     |     |     |     |     |     |       
| XNOR |  ✔  |     |     |     |     |     |     |     |     |     |     |     |       
|  AND |  ✔  |     |     |     |     |     |     |     |     |     |     |     |       
| NAND |  ✔  |     |     |     |     |     |     |     |     |     |     |     |       
|  ADD |  ✔  |  ✔  |  ✔  |     |  ✔  |  ✔  |  ✔  |     |     |     |     |     |       
| SUBT |  ✔  |  ✔  |  ✔  |     |  ✔  |  ✔  |  ✔  |     |     |     |     |     |        
| MULT |  ✔  |  ✔  |  ✔  |     |  ✔  |  ✔  |  ✔  |     |     |     |     |     |        
| DIVI |  ✔  |  ✔  |  ✔  |     |  ✔  |  ✔  |  ✔  |     |     |     |     |     |         
| EXPO |  ✔  |     |     |     |     |     |     |     |     |     |     |     |       
| COMP |  ✔  |  ✔  |  ✔  |     |  ✔  |  ✔  |  ✔  |     |     |     |     |     |       

**Single Argument**
- **R** Register
- **L** Literal
- **A** Absolute RAM addressing
- **I** Indirect RAM addressing

|      | R | L | A | I |
|-----:|:-:|:-:|:-:|:-:|
|  REF |   | ✔ |
| ROTL | ✔ |N/A|
| ROTR | ✔ |N/A|
|  NOT | ✔ |N/A| ✔ |
| INCR | ✔ |N/A| ✔ |
| DECR | ✔ |N/A| ✔ |
| GOTO | ✔ | ✔ | ✔ |
| EVAL |
| JMPZ | ✔ | ✔ | ✔ |
| JMGZ | ✔ | ✔ | ✔ |
| JMLZ | ✔ | ✔ | ✔ |
| GSUB |   | ✔ |
| JMPE | ✔ | ✔ | ✔ |
| JMPG | ✔ | ✔ | ✔ |
| JMPL | ✔ | ✔ | ✔ |
| ENQU |
| DEQU |

**No Arguments**

|      | ✔ | Notes |
|-----:|:-:|:------|
| NULL | ✔ |
| START | ✔ |
|  END | ✔ |
| RTRN | ✔ |

**Background**
I've worked at the lowest levels with PIC, 68000, MSP-430, Atmega, and ARM processors, working with all of those in Assembly and sometimes C, as well as Assembly for x86. PIC, 68000, and MSP-430 were my first introductions to Assembly and processor architectures, and the SG16 reflects that in many ways.

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