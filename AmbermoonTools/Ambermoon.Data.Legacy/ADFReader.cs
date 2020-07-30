﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ambermoon.Data.Legacy
{
    internal static class ADFReader
    {
        private enum SectorType
        {
            Unknown,
            File,
            Directory
        }

        private class Sector
        {
            public SectorType Type = SectorType.Unknown;
            public string Name = "";
            public UInt32 NextHashBlock = 0u;
            public UInt32 ParentBlock = 0u;
            public UInt32 FirstExtensionBlock = 0u;
            public UInt32 Offset = UInt32.MaxValue;
            public UInt32 Length = 0u;

            public byte[] GetData(BinaryReader reader, UInt32 fileSize = 0u)
            {
                if (Type != SectorType.File)
                    return null;

                if (fileSize == 0u)
                {
                    reader.BaseStream.Position = Offset + 512 - 188; // offset to file size

                    fileSize = reader.ReadUInt32BigEndian();
                }

                reader.BaseStream.Position = Offset + 24;

                byte[] fileData = new byte[fileSize];
                UInt32[] dataOffsets = new UInt32[72];

                for (int i = 0; i < 72; ++i)
                    dataOffsets[71 - i] = reader.ReadUInt32BigEndian();

                int offset = 0;

                for (int i = 0; i < 72; ++i)
                {
                    if (dataOffsets[i] == 0)
                        break;

                    AppendData(reader, fileData, ref offset, dataOffsets[i]);
                }

                if (FirstExtensionBlock != 0)
                {
                    var extensionSector = ReadSector(reader, FirstExtensionBlock, true);

                    if (extensionSector == null)
                        throw new IOException($"Invalid ADF file data for file \"{Name}\".");

                    var extensionData = extensionSector.GetData(reader, fileSize - (UInt32)offset);

                    if (offset + extensionData.Length != fileSize)
                        throw new IOException($"Invalid ADF file data for file \"{Name}\".");

                    System.Buffer.BlockCopy(extensionData, 0, fileData, offset, extensionData.Length);

                    offset = (int)fileSize;
                }

                if (offset != fileSize)
                    throw new IOException($"Invalid ADF file data for file \"{Name}\".");

                return fileData;
            }

            void AppendData(BinaryReader reader, byte[] buffer, ref int offset, UInt32 block)
            {
                reader.BaseStream.Position = block * 512;

                if (reader.ReadUInt32BigEndian() != 8)
                    throw new IOException("Invalid file data sector header.");

                reader.ReadBytes(8); // skip some bytes

                var size = reader.ReadUInt32BigEndian();

                if (size > 512 - 24)
                    throw new IOException("Invalid file data sector size.");

                reader.ReadBytes(8); // skip some bytes

                var data = reader.ReadBytes((int)size);

                System.Buffer.BlockCopy(data, 0, buffer, offset, data.Length);

                offset += data.Length;
            }
        }

        private static UInt32 GetHash(string name)
        {
            UInt32 hash, l;

            l = hash = (UInt32)name.Length;

            for (int i = 0; i < l; ++i)
            {
                hash *= 13;
                hash += (UInt32)char.ToUpper(name[i]);
                hash &= 0x7ff;
            }

            return hash % 72;
        }

        private static Sector GetSector(BinaryReader reader, UInt32[] hashTable, string name)
        {
            var hash = GetHash(name);

            if (hash == 0)
                return null;

            name = name.ToUpper();

            var sector = ReadSector(reader, hashTable[hash]);

            if (sector == null)
                return null;

            while (sector.Name.ToUpper() != name && sector.NextHashBlock != 0)
            {
                sector = ReadSector(reader, sector.NextHashBlock);
            }

            if (sector.Name.ToUpper() != name)
                return null;

            return sector;
        }

        private static Sector ReadSector(BinaryReader reader, UInt32 block, bool expectExtension = false)
        {
            if (block == 0)
                return null;

            reader.BaseStream.Position = block * 512;
            var type = reader.ReadUInt32BigEndian();

            if ((type != 2 && !expectExtension) || (type != 16 && expectExtension)) // primary type (T_HEADER or T_LIST)
                throw new IOException("Unexpected ADF sector type.");

            if (reader.ReadUInt32BigEndian() != block)
                throw new IOException("Invalid ADF sector.");

            // move pointer to file size
            reader.BaseStream.Position = block * 512 + 512 - 188;

            var sector = new Sector()
            {
                Offset = block * 512,
                Length = reader.ReadUInt32BigEndian()
            };

            // move pointer to name length
            reader.BaseStream.Position = block * 512 + 512 - 80;

            int nameLength = Math.Min(30, (int)reader.ReadByte());

            sector.Name = Encoding.GetEncoding("iso-8859-1").GetString(reader.ReadBytes(nameLength));

            // move pointer to next hash ptr
            reader.BaseStream.Position = block * 512 + 512 - 16;

            sector.NextHashBlock = reader.ReadUInt32BigEndian();
            sector.ParentBlock = reader.ReadUInt32BigEndian();
            sector.FirstExtensionBlock = reader.ReadUInt32BigEndian();

            var secondaryType = (int)reader.ReadUInt32BigEndian(); // secondary type

            if (secondaryType == -3)
                sector.Type = SectorType.File;
            else if (secondaryType == 2)
                sector.Type = SectorType.Directory;
            else
                sector.Type = SectorType.Unknown;

            return sector;
        }

        public static Dictionary<string, byte[]> ReadADF(Stream stream)
        {
            using (var reader = new BinaryReader(stream))
            {
                // Reading bootblock (sectors 1 and 2 -> byte 0 - 1023)
                byte[] header = reader.ReadBytes(4);

                if (header[0] != 'D' || header[1] != 'O' ||
                    header[2] != 'S')
                    throw new IOException("Invalid ADF file header.");

                byte flags = (byte)(header[3] & 0x07);

                if (flags != 0)
                    throw new IOException("Invalid ADF file format." + Environment.NewLine + "Supported is only AmigaDOS 1.2 format (OFS)");

                // Reading rootblock (sector 880 -> offset 0x6e000)
                reader.BaseStream.Position = 0x6e000;

                if (reader.ReadUInt32BigEndian() != 2 || // type = T_HEADER
                    reader.ReadUInt32BigEndian() != 0 || // header_key = unused
                    reader.ReadUInt32BigEndian() != 0 || // high_seq = unused
                    reader.ReadUInt32BigEndian() != 0x48 || // ht_size = 0x48
                    reader.ReadUInt32BigEndian() != 0) // first_data = unused
                    throw new IOException("Invalid ADF file format.");

                reader.ReadUInt32(); // skip checksum

                var hashTable = new UInt32[72];

                for (int i = 0; i < 72; ++i)
                    hashTable[i] = reader.ReadUInt32BigEndian();

                bool bmFlagsValid = reader.ReadUInt32() == 0xFFFFFFFF;

                var bitmapBlockPointers = new UInt32[25];

                for (int i = 0; i < 25; ++i)
                    bitmapBlockPointers[i] = reader.ReadUInt32BigEndian();

                reader.ReadUInt32(); // skip first bitmap extension block (only used for hard disks)
                reader.ReadBytes(12); // skip last root alteration date values
                reader.ReadBytes(32); // skip volume name
                reader.ReadBytes(8); // skip unused bytes
                reader.ReadBytes(12); // skip last disk alteration date values
                reader.ReadBytes(12); // skip filesystem creation date values
                reader.ReadUInt32(); // skip next hash
                reader.ReadUInt32(); // skip parent directory

                if (reader.ReadUInt32BigEndian() != 0 || // extension must be 0
                    reader.ReadUInt32BigEndian() != 1) // block secondary type = ST_ROOT (1)
                    throw new IOException("Invalid ADF file format.");

                var loadedFiles = new Dictionary<string, byte[]>();

                foreach (var file in Files.AmigaFiles.Keys)
                {
                    var fileSector = GetSector(reader, hashTable, file);

                    if (fileSector != null)
                        loadedFiles.Add(file, fileSector.GetData(reader));
                }

                return loadedFiles;
            }
        }
    }

    internal static class EndianExtensions
    {
        public static UInt16 ReadUInt16BigEndian(this BinaryReader reader)
        {
            return (UInt16)((reader.ReadByte() << 8) | reader.ReadByte());
        }

        public static UInt32 ReadUInt32BigEndian(this BinaryReader reader)
        {
            return (UInt32)((reader.ReadByte() << 24) | (reader.ReadByte() << 16) | (reader.ReadByte() << 8) | reader.ReadByte());
        }
    }
}