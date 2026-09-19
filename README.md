# easyauth c# sdk & example

c# client wrapper and example console application for the qpapel easyauth system (.net framework 4.8).

## requirements

- .net framework 4.8 developer pack
- windows x64
- `qPapelPacker.exe` and `qPapelEasyAuth.dll` in `qPapelTools\` (download from https://papelship.com/dashboard/downloads)
- `qPapelEasyAuth.NET.dll` in `wrapper\` (download from https://papelship.com/dashboard/downloads)

if `qPapelEasyAuth.dll` is missing, debug and release post-build fail with: `qPapelEasyAuth.dll is missing. download it and try again`

## structure

- `wrapper/`: c# wrapper dll. costura.fody embeds it into the example so it is not shipped as a loose file.
- `EasyAuth-CS-Example.csproj`: example application with automated post-build packer integration.
- `qPapelTools/`: native core dll and pe section packer tool.

## usage

```csharp
using qPapelEasyAuth;

qPapelEasyAuth.qPapelEasyAuth.SetConfig(new EasyAuthConfig
{
    Flags = ProtectionFlags.All,
    DefaultApiKey = "pk_your_api_key",
    ClientVersion = "2.0.0",
    WatchdogIntervalMs = 500
});

qPapelEasyAuth.qPapelEasyAuth.Initialize();

if (qPapelEasyAuth.qPapelEasyAuth.Connect())
{
    var session = qPapelEasyAuth.qPapelEasyAuth.InitSession();
    var auth = qPapelEasyAuth.qPapelEasyAuth.Authenticate("USER_LICENSE_KEY");
    if (auth.Success)
    {
        Console.WriteLine($"auth success: {auth.ExpireDate}");
        string val = qPapelEasyAuth.qPapelEasyAuth.GetVariable("var_id");
    }
}
```

`Initialize()` requires a packed binary. unpacked builds are rejected on purpose.

## build and distribution

build in release mode:

```cmd
dotnet build -c Release
```

the post-build step packs `EasyAuth-CS-Example.exe` into `EasyAuth-CS-Example_packed.exe`.
costura.fody already hid `qPapelEasyAuth.NET.dll` inside the exe. the packer then embeds `qPapelEasyAuth.dll` into the packed exe.

distribute only `EasyAuth-CS-Example_packed.exe` from `bin/Release/` (or `bin/x64/Release/`).

do not ship `qPapelEasyAuth.dll` or `qPapelEasyAuth.NET.dll`. the native engine is decrypted and mapped in memory at runtime.
