// Decompiled with JetBrains decompiler
// Type: PacketDotNet.Sctp.Chunks.Sack.SctpSackChunk
// Assembly: PacketDotNet, Version=0.14.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F90CC42A-15BE-4D4B-8C38-4E8DECCB77A2
// Assembly location: C:\Desktop\PacketDotNet.dll

using PacketDotNet.Utils;
using System;
using System.Text;
using PacketDotNet.Utils.Converters;

namespace PacketDotNet.Sctp.Chunks.Sack
{
  public class SctpSackChunk : SctpChunk
  {
    public uint AdvertisedReceiverWindowCreadit
    {
      get
      {
        return EndianBitConverter.Big.ToUInt32(this.Header.Bytes, this.Header.Offset + SctpSackChunkFields.AdvertisedReceiverWindowCredit);
      }
      set
      {
        EndianBitConverter.Big.CopyBytes(value, this.Header.Bytes, this.Header.Offset + SctpSackChunkFields.AdvertisedReceiverWindowCredit);
      }
    }

    public ushort GapAckBlocks
    {
      get
      {
        return EndianBitConverter.Big.ToUInt16(this.Header.Bytes, this.Header.Offset + SctpSackChunkFields.GapAckBlocks);
      }
      set
      {
        EndianBitConverter.Big.CopyBytes(value, this.Header.Bytes, this.Header.Offset + SctpSackChunkFields.GapAckBlocks);
      }
    }

    public ushort DuplicatedTSNs
    {
      get
      {
        return EndianBitConverter.Big.ToUInt16(this.Header.Bytes, this.Header.Offset + SctpSackChunkFields.DuplicatedTSNs);
      }
      set
      {
        EndianBitConverter.Big.CopyBytes(value, this.Header.Bytes, this.Header.Offset + SctpSackChunkFields.DuplicatedTSNs);
      }
    }

    public SctpSackChunk(ByteArraySegment bas)
    {
      this.Header = bas;
      byte[] Bytes = new byte[(int) this.Length];
      Array.Copy((Array) bas.Bytes, bas.Offset, (Array) Bytes, 0, (int) this.Length);
      this.Header = new ByteArraySegment(Bytes);
      this.Header.Length = SctpSackChunkFields.HeaderLength;
      this.PayloadPacketOrData = new LazySlim<PacketOrByteArraySegment>(()=> new PacketOrByteArraySegment());
    }

    public SctpSackChunk(ByteArraySegment bas, Packet parent)
      : this(bas)
    {
      this.ParentPacket = parent;
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
        stringBuilder.AppendFormat("{0}[SctpSackChunk: Type={2}, Flags={3}, Length={4}, TSN={5}, Advertised Receiver Window Creadit={6}, Gap Ack Blocks={7}, Duplicated TSNs={8}]{1}", (object) str1, (object) str2, (object) this.Type, (object) this.Flags.ToString("X2"), (object) this.Length, (object) this.TransmissionSequenceNumber, (object) this.AdvertisedReceiverWindowCreadit, (object) this.GapAckBlocks, (object) this.DuplicatedTSNs);
      return stringBuilder.ToString();
    }
  }
}
