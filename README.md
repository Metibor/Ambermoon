# Ambermoon

This repository should serve as a place where resources and information about Ambermoon can be stored.

If you have some valuable input, feel free to contact me: trobt@web.de

You can use the issue tracker or pull requests to contribute as well.

I will provide some helpful tools too.

## Tool releases

[v1.0.17 for Windows](https://github.com/Pyrdacor/Ambermoon/releases/download/v1.0.17/AmbermoonTools-Windows.zip): Current release with a command line packer.

[![Build status](https://ci.appveyor.com/api/projects/status/dn5n21r8m11an48i/branch/master?svg=true)](https://ci.appveyor.com/project/Pyrdacor/ambermoon/branch/master)



## Content

Section | Description
--- | ---
[Disks](Disks) | Game versions and patches
[FileSpecs](FileSpecs) | File format specifications
[Files](Files) | General file encoding information and file list
[Graphics](Graphics) | Exported game graphics

### File format specifications

- [Characters](FileSpecs/Characters.md) (data of party members, monsters and NPCs)
- [Graphics](FileSpecs/Graphics.md) (information about graphic formats)
- [Items](FileSpecs/Items.md) (data of items and where to find it)
- [Maps](FileSpecs/Maps.md) (data of maps)
  - [Maps2D](FileSpecs/Maps2D.md) (data specific for 2D maps)
  - [Maps3D](FileSpecs/Maps3D.md) (data specific for 3D maps)
  - [Labdata](FileSpecs/Labdata.md) (labyrinth data for 3D maps)
  - [MapEventData](FileSpecs/MapEventData.md) (data structure of specific map events)
- Enumerations (values of game enumerations)
  - [Abilities](FileSpecs/Enumerations/Abilities.md)
  - [Ailments](FileSpecs/Enumerations/Ailments.md)
  - [Attributes](FileSpecs/Enumerations/Attributes.md)
  - [CharacterTypes](FileSpecs/Enumerations/CharacterTypes.md)
  - [Classes](FileSpecs/Enumerations/Classes.md)
  - [Directions](FileSpecs/Enumerations/Directions.md)
  - [Gender](FileSpecs/Enumerations/Gender.md)
  - [ItemTypes](FileSpecs/Enumerations/ItemTypes.md)
  - [Languages](FileSpecs/Enumerations/Languages.md)
  - [MonsterElements](FileSpecs/Enumerations/MonsterElements.md)
  - [MonsterFlags](FileSpecs/Enumerations/MonsterFlags.md)
  - [Races](FileSpecs/Enumerations/Races.md)
  - [Spells](FileSpecs/Enumerations/Spells.md)
  - [SpellTypes](FileSpecs/Enumerations/SpellTypes.md)
- [Executables](Files/Executables.md)
  - [Hunk format](Files/Hunks.md)
  - [Imploder](Files/Imploding.md)

### Graphics

- [Items](Graphics/Items)
- [Portraits](Graphics/Portraits)
- [Maps](Graphics/Maps)
  - [Lyramion world map](Graphics/Maps/001.png)
  - [Forest moon map](Graphics/Maps/300.png)
  - [Morag map](Graphics/Maps/513.png)
- [80x80](Graphics/80x80)
- [Objects3D](Graphics/Objects3D)
- [Walls3D](Graphics/Walls3D)
- [Overlays3D](Graphics/Overlays3D)
- [Floors](Graphics/Floors)
- [World backgrounds](Graphics/WorldBackgrounds)
- [Palettes](Graphics/Palettes.jpg)


## Ambermoon.net

I plan to develop Ambermoon from scratch with C#. Have a look [here](https://github.com/Pyrdacor/Ambermoon.net).

## Special thanks

Most of the work was done by a bunch of good fellas.
- Oliver Gantert and others of project [Amberworld](http://amberworld.sourceforge.net/)
- Daniel Schulz from [Slothsoft](http://slothsoft.net/Ambermoon/) with his savegame editor
- Nico Bendlin and his [ambermoon research project](https://gitlab.com/ambermoon/research)

 And others provide information about Thalion and Ambermoon for ages now.
- Alex Holland from the [Thalion Webshrine](http://thalion.exotica.org.uk/)
- Gerald Müller-Bruhnke from the [Thalion Source](http://home.wtal.de/gmb/index.htm)
- and many others (if you know anyone I forgot, please let me know)

Moreover there are some Albion reworks and reverse engineering projects. The data formats are very similar so there is some valuable information as well:
- [freealbion](https://github.com/freealbion/freealbion) by Florian Ziesche
- [ualbion](https://github.com/csinkers/ualbion) by Cam Sinclair

I just want to gather the whole findings at a central place and provide it on a save storage for the future.

At least some contributions will come from me as well like some file specs and the formatting of all the data. :)

My latest contributions are decoding of map events, 3D map and lab formats and packed texture formats. Moreover I reverse engineered the imploder algorithm so the file AM2_CPU can be deploded and read on PCs.

I marked unknown values in the specs as **Unknown**. Would be nice if some of those values' meaning could be revealed by someone.

## Language

I will document everything in english so most people will understand. But I only played the german version and translated some things to my best knowledge. So there might be differences between the names and values given here and the names used in the original english game.

If you know the english version or have some other translation improvements, just let me know. You can use the issue tracker for that for example.

