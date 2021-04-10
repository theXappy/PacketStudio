// Decompiled with JetBrains decompiler
// Type: PacketDotNet.SctpPacket
// Assembly: PacketDotNet, Version=0.14.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F90CC42A-15BE-4D4B-8C38-4E8DECCB77A2
// Assembly location: C:\Desktop\PacketDotNet.dll

using PacketDotNet.Sctp.Chunks;
using PacketDotNet.Sctp.Chunks.Sack;
using PacketDotNet.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PacketDotNet.Utils.Converters;

namespace PacketDotNet
{
  public class SctpPacket : Packet
  {
    public ushort SourcePort
    {
      get
      {
        return EndianBitConverter.Big.ToUInt16(this.Header.Bytes, this.Header.Offset + SctpFields.SourcePortPosition);
      }
      set
      {
        EndianBitConverter.Big.CopyBytes(value, this.Header.Bytes, this.Header.Offset + SctpFields.SourcePortPosition);
      }
    }

    public ushort DestinationPort
    {
      get
      {
        return EndianBitConverter.Big.ToUInt16(this.Header.Bytes, this.Header.Offset + SctpFields.DestinationPortPosition);
      }
      set
      {
        EndianBitConverter.Big.CopyBytes(value, this.Header.Bytes, this.Header.Offset + SctpFields.DestinationPortPosition);
      }
    }

    public uint VerificationTag
    {
      get
      {
        return EndianBitConverter.Big.ToUInt32(this.Header.Bytes, this.Header.Offset + SctpFields.VerificationTagPosition);
      }
      set
      {
        EndianBitConverter.Big.CopyBytes(value, this.Header.Bytes, this.Header.Offset + SctpFields.VerificationTagPosition);
      }
    }

    public uint Checksum
    {
      get
      {
        return EndianBitConverter.Big.ToUInt32(this.Header.Bytes, this.Header.Offset + SctpFields.ChecksumPosition);
      }
      set
      {
        EndianBitConverter.Big.CopyBytes(value, this.Header.Bytes, this.Header.Offset + SctpFields.ChecksumPosition);
      }
    }

    public List<SctpChunk> PayloadChunks { get; private set; }

    public override Packet PayloadPacket
    {
      get
      {
        return this.PayloadChunks.Count == 1 ? (Packet) this.PayloadChunks[0] : (Packet) null;
      }
      set
      {
        if (this.PayloadChunks.Count > 1)
          throw new InvalidOperationException("Cannot set single payload chunk to Sctp packet. Packet currently has multiple chunks.");
        SctpChunk sctpChunk = value as SctpChunk;
        if (sctpChunk == null)
          throw new ArgumentException("Can not set the PayloadPacket of a SctpPacket to an object which isn't a SctpChunk.");
        this.PayloadChunks = new List<SctpChunk>()
        {
          sctpChunk
        };
        this.PayloadPacketOrData.Value.Packet = (Packet) sctpChunk;
      }
    }

    public SctpPacket(ushort srcPort, ushort dstPort, uint verificationTag, uint checksum)
    {
      this.Header = new ByteArraySegment(new byte[SctpFields.HeaderLength]);
      this.SourcePort = srcPort;
      this.DestinationPort = dstPort;
      this.VerificationTag = verificationTag;
      this.Checksum = checksum;
      this.PayloadChunks = new List<SctpChunk>();
    }

    public SctpPacket(ByteArraySegment bas)
    {
      this.Header = new ByteArraySegment(bas);
      this.Header.Length = SctpFields.HeaderLength;
      List<SctpChunk> foundSctpChunks;
        PacketOrByteArraySegment parsed =
            SctpPacket.ParseEncapsulatedBytes(this.Header, (Packet) this, out foundSctpChunks);
        this.PayloadPacketOrData = new LazySlim<PacketOrByteArraySegment>(()=> parsed);
      this.PayloadChunks = foundSctpChunks;
    }

    public SctpPacket(ByteArraySegment bas, Packet parent)
      : this(bas)
    {
      this.ParentPacket = parent;
    }

    private static PacketOrByteArraySegment ParseEncapsulatedBytes(ByteArraySegment header, Packet packet, out List<SctpChunk> foundSctpChunks)
    {
      foundSctpChunks = new List<SctpChunk>();
      PacketOrByteArraySegment byteArraySegment = new PacketOrByteArraySegment();
      ByteArraySegment bas = header.NextSegment();
      if (bas == null || bas.Length == 0)
        return new PacketOrByteArraySegment()
        {
          ByteArraySegment = bas
        };
      for (; bas.Length > SctpChunkFields.HeaderLength; bas = bas.NextSegment())
      {
        SctpChunk sctpChunk;
        switch ((SctpChunkType) bas.Bytes[bas.Offset])
        {
          case SctpChunkType.Data:
            sctpChunk = (SctpChunk) new SctpDataChunk(bas, packet);
            break;
          case SctpChunkType.Sack:
            sctpChunk = (SctpChunk) new SctpSackChunk(bas, packet);
            break;
          default:
            sctpChunk = (SctpChunk) new SctpUnsupportedChunk(bas, packet);
            break;
        }
        foundSctpChunks.Insert(0, sctpChunk);
        bas.Length = (int) sctpChunk.Length;
      }
      if (foundSctpChunks.Count == 1)
      {
        byteArraySegment.Packet = (Packet) foundSctpChunks.Single<SctpChunk>();
        return byteArraySegment;
      }
      byteArraySegment.ByteArraySegment = header.NextSegment();
      return byteArraySegment;
    }

    public override string ToString(StringOutputType outputFormat)
    {
      StringBuilder stringBuilder = new StringBuilder();
      string str1 = "";
      string str2 = "";
      if (outputFormat == StringOutputType.Colored || outputFormat == StringOutputType.VerboseColored)
      {
        str1 = this.Color;
        str2 = AnsiEscapeSequences.Reset;
      }
      if (outputFormat == StringOutputType.Normal || outputFormat == StringOutputType.Colored)
        stringBuilder.AppendFormat("{0}[SctpPacket: SourcePort={2}, DestinationPort={3}, VerificationTag={4}, Checksum={5}, Chunks={6}]{1}", (object) str1, (object) str2, (object) this.SourcePort, (object) this.DestinationPort, (object) this.VerificationTag, (object) this.Checksum, (object) this.PayloadChunks.Count);
      return stringBuilder.ToString();
    }
  }
}
