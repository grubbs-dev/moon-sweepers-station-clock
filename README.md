# Station Clock 0.1.1

Displays local solar time and the real-time countdown to the next local sunrise or sunset. Local time is a 24-hour dial representing one lunar day, not Earth's civil time. The countdown uses the game's lunar-day duration and current time speed, updating twice per second at the player's position.

The HUD sits at the top left and reads `Local time: 06:14 | Sunset in 00:29:25`, switching to Sunrise at night. No DAY/NIGHT badge is shown.

## Requirements and installation

- [BepInEx 6.0.0-be.791 Unity IL2CPP win-x64](https://builds.bepinex.dev/projects/bepinex_be/791)
- [MoonSweepers BepInEx Patcher by boverson777](https://www.nexusmods.com/moonsweepers/mods/9)

Extract the mod ZIP into the directory containing MoonSweepers.exe. Remove older duplicate StationClock.dll files before installing. Keep only one copy of the plugin. Sleep Until Dawn is optional and is not bundled.

Requires Moon Sweepers 1.0.5 Windows x64, BepInEx 6.0.0-be.791 Unity IL2CPP and the MoonSweepers BepInEx Patcher. Install BepInEx/plugins/StationClock/StationClock.dll into your game folder. Close the game before installing or removing. Remove that plugin folder to uninstall.

Sleep Until Dawn is optional. Both plugins were loaded together during a live test; the HUD was visually observed at daytime around 06:14 after Sleep and nighttime around 18:15 after Nap. The project owner also operated the game during this session, so those key presses are not attributed solely to automated control. The logs record both solar-clock jumps without Station Clock errors. Multiplayer and station travel have not been tested for this HUD.

No game files or third-party dependencies are bundled.

## Source and support

https://github.com/grubbs-dev/moon-sweepers-station-clock

Report issues with the game version, mod version, host/client role, station and relevant BepInEx/LogOutput.log entries. Review logs for private information before posting. Version 0.1.1's layout was approved by the project owner. Multiplayer and station travel remain unverified for this HUD.

## Build

Use a .NET SDK supporting net6.0 and a legally installed game that has generated its BepInEx interop assemblies:

`dotnet build StationClock.csproj -c Release -p:GameDir="D:\SteamLibrary\steamapps\common\Moon Sweepers"`

Source is published for inspection; no redistribution license is granted in this initial release. Moon Sweepers, BepInEx, Il2CppInterop and the community patcher belong to their respective creators. This is an unofficial mod by grubbs-dev.
