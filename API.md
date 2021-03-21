# ArmConverter
## Assembler
The main Assembler class. 

Contains methods for assembling ARM assembly code into hex code. 

* Assemble - Main assembling method
* MultiAssemble - Main multi-assembling method
#### Remarks
This class can assemble all types of ARM assembly code.

These methods are performed on strings and string arrays only.
### M:ArmConverter.Assemble(assembly, archSelection, offset)
Assembles the specified assembly code and returns the result.
#### Returns
The resulting hex code from the assembled assembly code.

*System.InvalidOperationException:*  Thrown when an error occurs after attempting to assemble the assembly code. 

| Name | Description |
| ---- | ----------- |
| assembly | *System.String*<br>The assembly code string to assemble. |
| archSelection | *ArmConverter.Utilities.ArchSelection*<br>The architecture that the assembly code corresponds to. |
| offset | *System.Nullable{System.Int32}*<br>The offset integer that the resulting hex code should be shifted by. |
### M:ArmConverter.MultiAssemble(assembly, archSelection, offset)
Assembles the specified assembly code array and returns the result.
#### Returns
The resulting hex code array from the assembled assembly code.

*System.InvalidOperationException:*  Thrown when an error occurs after attempting to assemble one of the lines of the assembly code. 

| Name | Description |
| ---- | ----------- |
| assembly | *System.String[]*<br>The assembly code string array to assemble. |
| archSelection | *ArmConverter.Utilities.ArchSelection*<br>The architecture that the assembly code corresponds to. |
| offset | *System.Nullable{System.Int32}*<br>The offset integer that the resulting hex code should be shifted by. |
## T:ArmConverter.Disassembler
The main Disassembler class. 

Contains methods for disassembling ARM hex code into assembly code. 

* Disassemble - Main disassembling method
* MultiDisassemble - Main multi-disassembling method
#### Remarks
This class can disassemble all types of ARM hex code.

These methods are performed on strings and string arrays only.
### M:ArmConverter.Disassembler.Disassemble(hex, archSelection, offset)
Disassembles the specified hex code and returns the result.
#### Returns
The resulting assembly code from the disassembled hex code.

*System.Net.WebException:*  Thrown when the hex code is invalid. 

| Name | Description |
| ---- | ----------- |
| hex | *System.String*<br>The hex code string to disassemble. |
| archSelection | *ArmConverter.Utilities.ArchSelection*<br>The architecture that the hex code corresponds to. |
| offset | *System.Nullable{System.Int32}*<br>The offset integer that the hex code is shifted by. |
### M:ArmConverter.Disassembler.MultiDisassemble(hex, archSelection, offset)

Disassembles the specified hex code array and returns the result.
#### Returns
The resulting assembly code array from the disassembled hex code.

*System.Net.WebException:*  Thrown when one of the elements of the hex code is invalid. 

| Name | Description |
| ---- | ----------- |
| hex | *System.String[]*<br>The hex code string array to disassemble. |
| archSelection | *ArmConverter.Utilities.ArchSelection*<br>The architecture that the hex code corresponds to. |
| offset | *System.Nullable{System.Int32}*<br>The offset integer that the hex code is shifted by. |
## T:ArmConverter.Utilities
The main Utilities class.

Contains an enum for aiding with architecture choices.

* ArchSelection - Main architecture enum
#### Remarks
This class can handle choices for every architecture that the API offers.
## T:ArmConverter.Utilities.ArchSelection
An enum for handling the architecture choice given by the main methods.
### F:ArmConverter.Utilities.ArchSelection.AArch32
Resembles the AArch32 or ARM architecture.
### F:ArmConverter.Utilities.ArchSelection.AArch64
Resembles the AArch64 or ARM64 architecture.
### F:ArmConverter.Utilities.ArchSelection.Thumb
Resembles the Thumb, Thumb-2 or T32 architecture extension.
