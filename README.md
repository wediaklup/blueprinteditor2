# BlueprintEditor2 for Linux (Wine)
This repository contains a modification of the BlueprintEditor2 for DTG Train Simulator that, unlike the original, works under Wine/Proton. Prebuilt binaries can be found in the Releases. 

# Disclaimer
While AI was used in the making of this, it was done with care. I did all modifications to the code myself and did not copy and paste code before at least briefly understanding what the code is doing. Using this should be fine; however, we can't provide any warranty. We are not part of DTG, just regular TS users.

# How to get
If you wish to build from source, you can find some instructions under [Building](#Building) (however, this might be painful). Otherwise, you can download already built binaries in the Releases.  
Assuming you have Linux on your computer (since this is the whole point of this) and you have Steam+TS installed, download the latest binary from the releases and put it in your RailWorks directory. To prevent confusion with the already existing `BlueprintEditor2.exe`, the downloaded executable should be named `BlueprintEditor2.5.#.exe` (or similarly discriminating).

## `openvrpaths.vrpath` thing
For whatever reason, the tool `ConvertToTg.exe` requires a config file under `C:\users\steamuser\AppData\Local\openvr\openvrpaths.vrpath`. This is presumably due to some library loading in the background which looks for this config but has no purpose in this tool.
To fix this error, you can just put the following default config placeholder file under the required path inside the prefix:
```
{
    "version": 1,
    "jsonid": "vrpathreg",
     "runtime": [],
    "config": [],
    "log": [],
    "external_drivers": []
}
```
(Please note that I personally have no idea what all this openvr thing really is and that this default file is AI output. If you know more about that, we ask you kindly to report this information to an issue or something.)

# How to use
To start the executable we recommend using *Protontricks*.
- Open the Protontricks-GUI
- Select *Train Simulator* (This may take a moment)
- `Select default wine prefix`
- `start explorer` or `start wine cmd`
- Navigate to your RailWorks directory under `Z:\`. If you're using the explorer folders starting with `.` like `.local` are not shown; you'll need to append that folders name to the address bar manually to enter such a folder. 
- Start the downloaded executable by double clicking in explorer or entering its name in cmd.

## Notice
This modification of BlueprintEditor2 was only altered to make it work under Linux. But two additions in behaviour have been made:
1. A `PLogger` class has been added which writes debug messages to a `PLogger.log`-File.
2. Because `ConvertToTg.exe` somehow fails when it's called from BlueprintEditor2\* but works when called manually, we have added a `PDDSPrecompile`-class, which, when exporting a single file, compiles all `.dds`\*\* textures in the directory of the blueprint or subdirectories of it.

\* As far as I can tell what does not work are absolute paths (on `Z:\`?), while relative paths seem to work fine.

\*\* I was assuming that nobody would stil be using `.ace` and everyone would just use `.dds`, but I was proven wrong **very fast**.

# How it's made
I have to admit that none of us has ever before touched C#. So everything specific to C# was done with consultation of AI.
When we started to try this, we first decompiled the original BlueprintEditor2-Exe with dnSpy and patched one problem, which at least allowed the window to open. However, this was all very buggy.
Because you cannot compile this kind of C#-Application under Linux, I spent some time to create a super cursed build toolchain (See [Building](#Building)).
Sadly, the decompiled source was not compilable *at all*, and I spent a lot of time with AI fixing all the syntax problems in the code until eventually getting everything to compile.
Because compiling is not enough, I spend even more time finding the decompiler syntax issues that prevented everything from working but somehow compiled fine.
After the **`openvrpaths.vrpath` thing**, I noticed that `ConvertToTg.exe` somehow failed. Calling it manually, however, fixed the problem, so I actually added code with own functionality that calls `ConvertToTg.exe` to compile the textures on single file export, so when the internal export starts all textures are already up-to-date.
This is the current state of the development, and it is enough for my personal usage. See [potential future stuff](#Future).

# Building
As described in the above paragraph about *how it's made*, we are not familiar with the C#/.NET-Ecosystem and my build toolchain might be a total nightmare for someone who knows better. My setup is described in the old `ReadMeDE.md`. If you **are** someone who knows better, you are kindly asked to tell us if you managed to compile this from source with a more sane toolchain.

# Thank you and please help!
Thank you for reading.

## Contributing
If you have anything to say about this, please open an issue or something; we appreciate any helpful information or feedback. Also see paragraph below.

## Future
Following things might come in the future or are just things I would like to know. If you know please open an issue.
- This **`openvrpaths.vrpath` thing**. If you know what this is for, why `ConvertToTg.exe` needs it or even just if the default config file is legitimate, please contact us.
- The texture precompiler could be extended to also use `.ace` files or also work with mass-exports.
- The asset preview might work; however, I encountered the *Please login to steam* message inside the prefix and thought that preview is not necessary.
- `PLogger` will probably not be active in future releases.
