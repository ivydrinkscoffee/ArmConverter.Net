# ArmConverter.Net
A C# .NET class library to communicate with **https://armconverter.com**
## Use
Since it is not, and will not be soon, published on **NuGet**, you will need to add the DLL **manually** to your `.csproj` file like so
```xml
<ItemGroup>
  <Reference Include="ArmConverter">
    <HintPath>ArmConverter.dll</HintPath>
  </Reference>
</ItemGroup>
```
The group `HintPath` being the path to where `ArmConverter.dll` is located
## Examples
### Single line of assembly code
```cs
using System;

// Include library namespace
using ArmConverter;

namespace Example {
    class Program {
        static void Main (string[] args) {
            // By default the 'archSelection' variable is set to AArch64 and the 'offset' variable is 0 when null so we only need to satisfy the first argument
            string hex = Assembler.Assemble ("mov w0, #0");
            
            // Printing the result to the console (with a newline obviously)
            Console.WriteLine (hex);
        }
    }
}
```
### Multiple lines of assembly code
```cs
using System;

// Include library namespace
using ArmConverter;

namespace Example {
    class Program {
        static void Main (string[] args) {
            // Putting lines of assembly code into the array (thanks to 3096 for this part of their code patch used)
            string[] assembly = { "mov w0, #0", "ret", "ldr w3, [x1]", "and w3, w3, #0xff", "cmp w3, #0x61", "b.ne #0x1c", "adr x1, #0x24", "sub sp, sp, #0x60", "b #0xfffffffffffcbff4" };
            
            // By default the 'archSelection' variable is set to AArch64 and the 'offset' variable is 0 when null so we only need to satisfy the first argument
            string[] hex = Assembler.MultiAssemble (assembly);
            
            // Printing the results to the console seperated by newlines
            Console.WriteLine (string.Join('\n', hex));
        }
    }
}
```
### Single line of hex code
```cs
using System;

// Include library namespace
using ArmConverter;

namespace Example {
    class Program {
        static void Main (string[] args) {
            // By default the 'archSelection' variable is set to AArch64 and the 'offset' variable is 0 when null so we only need to satisfy the first argument
            string assembly = Disassembler.Disassemble ("00008052");
            
            // Printing the result to the console (with a newline obviously)
            Console.WriteLine (assembly);
        }
    }
}
```
### Multiple lines of hex code
```cs
using System;

// Include library namespace
using ArmConverter;

namespace Example {
    class Program {
        static void Main (string[] args) {
            // Putting lines of assembly code into the array (thanks to 3096 for this part of their code patch used)
            string[] hex = { "00008052", "C0035FD6", "230040B9", "631C0012", "7F840171", "E1000054", "21010010", "FF8301D1", "FD2FFF17" };
            
            // By default the 'archSelection' variable is set to AArch64 and the 'offset' variable is 0 when null so we only need to satisfy the first argument
            string[] assembly = Disassembler.MultiDisassemble (hex);
            
            // Printing the results to the console seperated by newlines
            Console.WriteLine (string.Join('\n', assembly));
        }
    }
}
```
#### Notes
If you need **multiple lines** of hex code at one address, using the `System.Linq` namespace, it can be printed to the console as follows
```cs
Console.WriteLine (string.Concat(result.AsEnumerable()));
```
The variable 'result' being the output of `Assembler.MultiAssemble()`
## Exceptions
### `System.InvalidOperationException`
Only thrown when an error occurs while attempting to **assemble**, this is because when **disassembling** it will directly return an HTTP error code **400**, which is known as **"Bad Request"**
## TODO
- [x] Different architectures
- [x] Offset selection
- [x] Assembling/disassembling of **arrays**
- [ ] Handling **all** exceptions from the API
- [ ] XML documentation for the methods
- [ ] **Asynchronous** copies of the methods
