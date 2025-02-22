# DOOMer
A (mostly) functional DOOM WAD manager based on (q)ZDL, using WinForms and NET 8.0.

# Requirements
Running DOOMer [requires the NET 8.0 Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) (https://dotnet.microsoft.com/en-us/download/dotnet/8.0) on Windows 7 or above.

# WADs
Supports `WAD`, `PWAD`, `IWAD`, `PK3`, `IPK3`, `PK7`, `IPK7`, `7z`... okay you get the idea.

# Importing from ZDL
You can import your own `zdl.ini` (some options may not be transfered over, your mileage may vary).

# Usage
I've tried to maintain familiartiy to ZDL (minus some functionality that may come later), but with quality of life and ease of access improvements. If you've used ZDL, this should come as second nature.

Oh, you can Drag + Drop PWAD/IWAD files onto the "Play" button to launch them directly. Holding `CONTROL` while dropping files will mark the first WAD file as the `IWAD`. Dropping any `EXE` files will launch the first `EXE` file as the source port.
