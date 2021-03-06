[![GitHub Downloads](https://badgen.net/github/assets-dl/ivydrinkscoffee/ArmConverter.Net)](https://github.com/ivydrinkscoffee/ArmConverter.Net/releases/tag/v1.0.0) [![GitHub License](https://img.shields.io/github/license/ivydrinkscoffee/ArmConverter.Net)](https://github.com/ivydrinkscoffee/ArmConverter.Net/blob/main/LICENSE) [![GitHub Top Language](https://img.shields.io/github/languages/top/ivydrinkscoffee/ArmConverter.Net)](https://github.com/ivydrinkscoffee/ArmConverter.Net/search?l=c%23) [![ArmConverter on fuget.org](https://www.fuget.org/packages/ArmConverter/badge.svg)](https://www.fuget.org/packages/ArmConverter)
# ArmConverter.Net
A relatively small C# .NET class library to communicate with **https://armconverter.com**
## Instructions
Since the package has now been published on **NuGet**, you can now add the package to your `*.csproj` file like so
```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net5.0</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="ArmConverter" Version="1.0.0" />
  </ItemGroup>

</Project>
```
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
            Console.WriteLine (string.Join ('\n', hex));
        }
    }
}
```
### Single element of hex code
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
### Multiple elements of hex code
```cs
using System;

// Include library namespace
using ArmConverter;

namespace Example {
    class Program {
        static void Main (string[] args) {
            // Putting elements of hex code into the array (thanks to 3096 for this part of their code patch used)
            string[] hex = { "00008052", "C0035FD6", "230040B9", "631C0012", "7F840171", "E1000054", "21010010", "FF8301D1", "FD2FFF17" };
            
            // By default the 'archSelection' variable is set to AArch64 and the 'offset' variable is 0 when null so we only need to satisfy the first argument
            string[] assembly = Disassembler.MultiDisassemble (hex);
            
            // Printing the results to the console seperated by newlines
            Console.WriteLine (string.Join ('\n', assembly));
        }
    }
}
```
#### Notes
If you need **multiple lines** of hex code at one address, using the `System.Linq` namespace, it can be printed to the console as follows
```cs
Console.WriteLine (string.Concat (result.AsEnumerable ()));
```
The variable *result* being the output of `Assembler.MultiAssemble ()`
## Exceptions
### `System.FormatException`
Only thrown when an error occurs while attempting to **assemble**, the exception message being the one that the **API** returns, which depends on the error with the assembly code
### `System.Net.WebException`
Only thrown when an error occurs while attempting to **disassemble**, the exception message will most likely be *The remote server returned an error: (400) Bad Request*, due to the invalid hex code
## TODO
- [x] Different architectures
- [x] Offset selection
- [x] Assembling/disassembling of **arrays**
- [x] Handling **all** *known* exceptions from the API
- [x] **Asynchronous** copies of the methods
- [x] XML **documentation** for all of the `public` methods
- [x] Add support for **big-endian** byte order
- [x] Release the package on **[NuGet](https://www.nuget.org)**
