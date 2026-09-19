# wrapper

put `qPapelEasyAuth.NET.dll` here (c# wrapper).

you can download this file from https://papelship.com/dashboard/downloads.

costura.fody embeds it into the example so it is not shipped as a loose file.
the packer then embeds `qPapelEasyAuth.dll` from `qPapelTools\` into the packed executable.
