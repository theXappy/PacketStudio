using Haukcode.PcapngUtils.PcapNG.BlockTypes;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace PacketStudio.DataAccess.SmartCapture
{
    public class PcapngWeakHandle
    {
        private List<long> _cachedOffsets = null;
        private List<InterfaceDescriptionBlock> _cachedIfacesBlock;
        public string Path { get; set; }


        public PcapngWeakHandle(string path)
        {
            Path = path;
        }

        public List<long> GetPacketsOffsets()
        {
            if (_cachedOffsets == null) {
                _cachedOffsets = new List<long>();

                int packetCounter = 0;
                using (FileStream fileStream = File.OpenRead(Path))
                using (BinaryReader binReader = new BinaryReader(fileStream)) {
                    while (fileStream.Position != fileStream.Length) {
                        // Read next 8 bytes of block:
                        BaseBlock.Types type = (BaseBlock.Types)binReader.ReadUInt32();
                        uint len = binReader.ReadUInt32();
                        if (type == BaseBlock.Types.EnhancedPacket) {
                            _cachedOffsets.Add(fileStream.Position - 8);
                            packetCounter++;
                        }

                        // Advance to next block start
                        fileStream.Seek(len - 8, SeekOrigin.Current);
                    }
                }
            }
            return _cachedOffsets;
        }

        public List<InterfaceDescriptionBlock> GetInterfaces()
        {
            if (_cachedIfacesBlock == null) {
                // TODO:
                bool reverseByteOrder = false;
                
                _cachedIfacesBlock = new List<InterfaceDescriptionBlock>();


                // Checking until first packet block or end of file
                int packetCounter = 0;
                using (FileStream fileStream = File.OpenRead(Path))
                using (BinaryReader binReader = new BinaryReader(fileStream)) {
                    while (fileStream.Position != fileStream.Length) {
                        // Read next 8 bytes of block:
                        BaseBlock.Types type = (BaseBlock.Types)binReader.ReadUInt32();
                        uint len = binReader.ReadUInt32();
                        if (type == BaseBlock.Types.EnhancedPacket) {
                            // Found a packet block, stopping search
                            break;
                        }

                        if (type == BaseBlock.Types.InterfaceDescription) {
                            fileStream.Seek(-8, SeekOrigin.Current);
                            var block = AbstractBlockFactory.ReadNextBlock(binReader, reverseByteOrder, (ex) => Debug.WriteLine("Kek!" + ex));
                            if (block == null || block.BlockType != BaseBlock.Types.InterfaceDescription) {
                                throw new Exception("Block at given position was not parsed to a INTERFACE DESCRIPTION BLOCK");
                            }
                            _cachedIfacesBlock.Add(block as InterfaceDescriptionBlock);
                        }
                        else {
                            // Advance to next block start
                            fileStream.Seek(len - 8, SeekOrigin.Current);
                        }
                    }
                }
            }
            return _cachedIfacesBlock;
        }

        public EnhancedPacketBlock GetPacketAt(long packetBlockOffset)
        {
            // TODO:
            bool reverseByteOrder = false;

            using (FileStream fileStream = File.Open(Path, FileMode.Open, FileAccess.ReadWrite, FileShare.Read))
            using (BinaryReader binReader = new BinaryReader(fileStream)) {
                // Navigate to the overridden block start
                long actualOffset = fileStream.Seek(packetBlockOffset, SeekOrigin.Begin);
                // Make sure seek succeeded
                if (actualOffset != packetBlockOffset) {
                    throw new Exception($"Couldn't seek to offset {packetBlockOffset}");
                }

                // Read next 8 bytes of the block we are overriding:
                BaseBlock.Types type = (BaseBlock.Types)binReader.ReadUInt32();
                if (type != BaseBlock.Types.EnhancedPacket) {
                    throw new Exception($"Expected an ENHANCED PACKET BLOCK (val:{BaseBlock.Types.EnhancedPacket}) in the given offset but got: {type}");
                }
                fileStream.Seek(-4, SeekOrigin.Current);

                var block = AbstractBlockFactory.ReadNextBlock(binReader, reverseByteOrder, (ex) => Debug.WriteLine("Kek!" + ex));
                if (block == null || block.BlockType != BaseBlock.Types.EnhancedPacket) {
                    throw new Exception("Block at given position was not parsed to a ENHANCED PACKET BLOCK");
                }
                return block as EnhancedPacketBlock;
            }
        }

        /// <summary>
        /// Replaces a packet with a new packet
        /// </summary>
        /// <param name="packetBlockOffset">Offset in the file where the block begins</param>
        /// <param name="newPacket">New packet block</param>
        public void ReplacePacket(long packetBlockOffset, EnhancedPacketBlock newPacket)
        {
            // TODO: Reverse Byte order not always false probably...
            byte[] data = newPacket.ConvertToByte(false, (ex) => Debug.WriteLine("LOL!" + ex));

            using (FileStream fileStream = File.Open(Path, FileMode.Open, FileAccess.ReadWrite, FileShare.Read))
            using (BinaryReader binReader = new BinaryReader(fileStream)) {
                // Navigate to the overridden block start
                long actualOffset = fileStream.Seek(packetBlockOffset, SeekOrigin.Begin);
                // Make sure seek succeeded
                if (actualOffset != packetBlockOffset) {
                    throw new Exception($"Couldn't seek to offset {packetBlockOffset}");
                }

                // Read next 8 bytes of the block we are overriding:
                BaseBlock.Types type = (BaseBlock.Types)binReader.ReadUInt32();
                uint len = binReader.ReadUInt32();
                if (type != BaseBlock.Types.EnhancedPacket) {
                    throw new Exception($"Expected an ENHANCED PACKET BLOCK (val:{BaseBlock.Types.EnhancedPacket}) in the given offset but got: {type}");
                }
                if (len < data.Length) {
                    ReplacePacketLonger(data, fileStream, len);
                }
                if (len > data.Length) {
                    ReplacePacketShorter(data, fileStream, len);
                }
                else {
                    // Lengths match exactly
                    fileStream.Seek(-4, SeekOrigin.Current);
                    fileStream.Write(data, 4, data.Length - 4);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ReplacePacketShorter(byte[] data, FileStream fileStream, uint len)
        {
            uint diff = len - (uint)data.Length;
            if (diff % 4 != 0) {
                throw new NotImplementedException($"Trying to override with smaller packet but length diff is not multiply of 4");
            }
            // The trick: Adding unknown 'Option' blocks at the end of the packet

            // Starting by just copying everything except:
            // * first 4 bytes of Block type (stays the same)
            // * 4 bytes of block length (stays the same)
            // * 4 last bytes of 'Options', which are set to 0x00_00_00_00 (end indicator)
            // * last 4 bytes - repeated block length (stays the same)
            fileStream.Write(data, 8, data.Length - 16);

            // Number of DWORDs to add: the difference calculated + 1 for the removed end indicator in Options
            uint numOfDwordsToPad = diff / 4 + 1;
            for (uint i = 0; i < numOfDwordsToPad; i++) {
                // Option Type:
                // * 0x00AA (unknown) if middle
                // * 0x0000 (end indicator) if last
                if (i == numOfDwordsToPad - 1) {
                    fileStream.WriteByte(0x00);
                }
                else {
                    fileStream.WriteByte(0xAA);
                }
                fileStream.WriteByte(0x00);
                // Option Length: 0
                fileStream.WriteByte(0x00);
                fileStream.WriteByte(0x00);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ReplacePacketLonger(byte[] data, FileStream fileStream, uint len)
        {
            // We're copying the file to new temporary file, update the temp file 
            // Actually I only need this (File.Copy) to copy everything until the change point.
            // Since I don't know id it's a lot a or a little of the file I'll risk it.
            // TODO: Heuristicly choose
            string tempFilePath = System.IO.Path.GetTempFileName();
            File.Copy(Path, tempFilePath);


            using (FileStream tempFileStream = File.Open(tempFilePath, FileMode.Open, FileAccess.ReadWrite)) {
                // Move to the change position in the temp file
                tempFileStream.Seek(fileStream.Position - 8, SeekOrigin.Begin);
                // Write the new packet isntead of the old packet
                tempFileStream.Write(data);

                // Move to start of NEXT block in original file
                fileStream.Seek(len - 8, SeekOrigin.Current);
                // Copy rest of file
                while (fileStream.Position < fileStream.Length) {
                    byte[] buff = new byte[Math.Min(1024, fileStream.Length - fileStream.Position)];
                    fileStream.Read(buff);
                    tempFileStream.Write(buff);
                }
            }
            // Close current file so we can OVERRIDE it with a 'Move' operation from the temp to our file
            fileStream.Close();
            File.Move(tempFilePath, Path);
            try { File.Delete(tempFilePath); } catch { /* I'm just being polite to the files system */ };
            _cachedOffsets = null;
        }
    }
}
