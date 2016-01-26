# SG16

For a couple years, I've kicked around the idea of building a CPU from scratch out of discrete logic gates. I never got far because without an FPGA it just isn't a practical goal.

Instead, I'm working on building an emulator instead, while designing the CPU architecture from the Assembly language level down and then up.

- [x] Define Assembly language specification
- [x] Define processor architecture
- [x] Assembler
- [x] Disassembler
- [ ] Emulator core
- [ ] Emulator GUI
- [ ] Peripheral emulators
- [ ] Software tools (C, BASIC compilers)


#Implemented Instructions

**Dual Argument**
- **R** Register
- **L** Literal
- **A** Absolute RAM addressing
- **I** Indirect RAM addressing

|      | R R | L R | A R | I R | R A | L A | A A | I A | R I | L I | A I | I I |
|-----:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|
| MOVE |  ✔  |  ✔  |  ✔  |    |  ✔  |    |     |     |     |     |     |     |       
| SWAP |  ✔  | N/A |     |     |     | N/A |     |     |     | N/A |     |     |       
|   OR |  ✔  |     |     |     |     |     |     |     |     |     |     |     |       
|  NOR |  ✔  |     |     |     |     |     |     |     |     |     |     |     |       
|  XOR |  ✔  |     |     |     |     |     |     |     |     |     |     |     |       
| XNOR |  ✔  |     |     |     |     |     |     |     |     |     |     |     |       
|  AND |  ✔  |     |     |     |     |     |     |     |     |     |     |     |       
| NAND |  ✔  |     |     |     |     |     |     |     |     |     |     |     |       
|  ADD |  ✔  |     |     |     |     |     |     |     |     |     |     |     |       
| SUBT |  ✔  |     |     |     |     |     |     |     |     |     |     |     |       
| MULT |  ✔  |     |     |     |     |     |     |     |     |     |     |     |       
| DIVI |  ✔  |     |     |     |     |     |     |     |     |     |     |     |       
| EXPO |  ✔  |     |     |     |     |     |     |     |     |     |     |     |       
| COMP |     |     |     |     |     |     |     |     |     |     |     |     |       

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
|  NOT | ✔ |N/A|
| INCR | ✔ |N/A|
| DECR | ✔ |N/A|
| GOTO |   | ✔ |
| EVAL |
| JMPZ |
| JMGZ |
| JMLZ |
| GSUB |   | ✔ |
| JMPE |
| JMPG |
| JMPL |
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
Defining the Assembly language first was a convenient way to define how the SG16 processor functions, and what features it offers. I started by defining basic memory operations like MOVE and SWAP, and then boolean logic operations like AND and OR. From there I kept expanding until I was confident the instruction set was complete enough to write useful applications, and then some.

One unique function I included was the hardware queue. compared to some common ALU operations, a FIFO queue is relatively simple to implement in hardware. As such, the SG16 architecture incorporates a hardware queue and two instructions to add and remove data.

Addressing modes were certainly a challenge. I was inspired by the indirect addressing mode used by the MSP-430, but when it came to planning the machine code I couldn't think of a simple way to represent it. I could have looked up how the MSP-430 does it, but I wanted the architecture to follow my style of doing things instead of copying someone else's implementation. As such, I solved it by adding a register for the reference address and indirect addressing simply specifies the offset.

**Assembler**
This is simpler than I had expected. The Assembler program is little more than an interface, with the actual assembly being performed by the ASM class of the SG16 library. Simply put, the assembler parses Assembly language code and converts it to machine code, outputing the result as a binary ROM file which will later be opened and executed by the emulator. The exercise brought me to the stunningly obvious realization than an emulator is an interpriter for machine code.

**Emulator**
The emulator is certainly the most challenging part of the project, and I am approaching it with strong OOP design principles. The imlementation is fairly abstract, contained within a DLL for easy adaptation into future frontends and UIs.

**Emulator UI**
The bulk of the emulator's facilities, such as registers and RAM, are public to allow a simple GUI application to read and display the contents, as well as providing some interface for pausing and stepping execution and connecting emulated peripherals. The first UI will use WinForms and be inspired heavily by the classic Easy68k emulator, with a traditional WYSIWYG interface and low-level display of registers, RAM, and other mechanisms.

**Peripherals**
There's no shortage of input and output peripherals I'd like to emulate, but the most important to start will certainly be a text terminal, followed by a simple graphics adapter. These will be emulated as simple memory-mapped I/O, making Assembly interaction as simple as possible. Other peripherals may eventually include serial ports, nonvolatile memory, direct interaction with the host mouse and keyboard, and sound output.

**Software**
Assembly is the first step, but writing useful or interesting software in Assembly is difficult and tedious. I am looking into a partial port of gcc, and definitely want to write a QBASIC compiler.
