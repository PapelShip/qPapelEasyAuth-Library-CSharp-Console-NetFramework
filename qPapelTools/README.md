# qpapeltools

this directory contains tools used to protect and pack the target executable after build.

## files

- `qPapelEasyAuth.dll`: core security engine (do not distribute to end users). you need to put the `qPapelEasyAuth.dll` file here; you can download this file from https://papelship.com/dashboard/downloads. if it is missing, the build fails with `qPapelEasyAuth.dll is missing. download it and try again`.
- `qPapelPacker.exe`: standalone packer that encrypts and embeds the dll into the target executable.
- `pack_release.bat`: convenience script to pack the release build.

## usage

1. build the project in `Release` configuration (`dotnet build -c Release`).
2. pack the generated executable (post-build already does this when the files above exist):
```cmd
qPapelPacker.exe --dll qPapelEasyAuth.dll --input ..\bin\Release\EasyAuth-CS-Example.exe --output ..\bin\Release\EasyAuth-CS-Example_packed.exe
```
3. distribute only `EasyAuth-CS-Example_packed.exe`.

do not ship `qPapelEasyAuth.dll`; it is decrypted and mapped directly in memory at runtime.
the c# wrapper is already embedded by costura.fody, so you also do not ship `qPapelEasyAuth.NET.dll`.
