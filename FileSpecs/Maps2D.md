# 2D Map file format specs

2D Maps use 3 kinds of files:
- The actual map data (1Map_data.amb, 2Map_data.amb and 3Map_data.amb)
- Icon data used for tiles (Icon_data.amb)
- Icon graphic data (1Icon_gfx.amb, 2Icon_gfx.amb and 3Icon_gfx.amb)

Each of those contains multiple files which represent a specific map, icon data (tileset) or icon graphic.

## Icon graphics

Icon graphics are 5 bit images with a size of 16x16. They use a 32 color palette from Palettes.amb as follows:

Tilesets | Palette
---- | ----
1,2 | 1
3 | 3
4,5,6,7 | 7
8 | 10

## Icon data

Icon data files represent tilesets. There are 8 icon data files (tilesets).

Offsets are given in hex. Sizes/lengths in dec. 16 and 32 bit values are stored in big endian format. So the most significant bytes come first. Example: The value 0x1234 is stored as 0x12 0x34 and the value 0x12345678 is stored as 0x12 0x34 0x56 0x78.

Offset | Type | Description
----|----|----
0x0000 | uword | Number of data entries
0x0002 | IconData[NumEntries] | Data entries

A data entry (IconData) looks like this:

Offset | Type | Description
----|----|----
0x0000 | uword | Tile flags (see below)
0x0002 | uword | **Unknown**
0x0004 | uword | Icon graphic index
0x0006 | ubyte | Number of animation tiles
0x0007 | ubyte | **Unknown**

The icon graphic index refers to an icon graphic. The index starts with 1 (index 1 is the first icon graphic). The number of animation tiles specifies the number of icon graphics belonging to the tile's animation (e.g. water uses multiple tiles for animation). Non-animated tiles have a value of 1.

So if you have icon graphic index 1 and 3 animation tiles the icons 1, 2 and 3 are used for the animation.

### Tile flags

- Bit 0: Allow movement (0 means block movement)
- Bit 7-9: Sit/sleep value
  - 0 -> no sitting nor sleeping
  - 1 -> sit and look up
  - 2 -> sit and look right
  - 3 -> sit and look down
  - 4 -> sit and look left
  - 5 -> sleep (always face down)

Note: If there is an overlay tile, its flags are used and if not, the underlay flags are used.

## Map data

Offset | Type | Description
----|----|----
0x0000 | ubyte[12] | Header (see [Maps](Maps.md))
0x000C | ubyte[320] | Character references (see [Maps](Maps.md))
0x014C | TileData[Width*Height] | Map tile data
... | ? | Map events etc

A tile data entry (TileData) consists of 4 ubytes.

```
underlay_tile_index = ((tile_data[1] & 0xe0) << 3) | tile_data[0];
overlay_tile_index = ((tile_data[2] & 0x07) << 8) | tile_data[3];
map_event_index = tile_data[1] & 0x1f;
```

Underlay is the background tile graphic and overlay an optional graphic on top of the background.

The indices refer to an IconData from the given tileset. Index 0 means 'nothing' or 'empty'.
