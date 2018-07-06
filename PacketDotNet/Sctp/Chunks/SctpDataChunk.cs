// Decompiled with JetBrains decompiler
// Type: PacketDotNet.Sctp.Chunks.SctpDataChunk
// Assembly: PacketDotNet, Version=0.14.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F90CC42A-15BE-4D4B-8C38-4E8DECCB77A2
// Assembly location: C:\Desktop\PacketDotNet.dll

using PacketDotNet.MiscUtil.Conversion;
using PacketDotNet.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PacketDotNet.Sctp.Chunks
{
  public class SctpDataChunk : SctpChunk
  {
    public ushort StreamIdentifier
    {
      get
      {
        return EndianBitConverter.Big.ToUInt16(this.Header.Bytes, this.Header.Offset + SctpDataChunkFields.StreamIdentifierPosition);
      }
      set
      {
        EndianBitConverter.Big.CopyBytes(value, this.Header.Bytes, this.Header.Offset + SctpDataChunkFields.StreamIdentifierPosition);
      }
    }

    public ushort StreamSequenceNumber
    {
      get
      {
        return EndianBitConverter.Big.ToUInt16(this.Header.Bytes, this.Header.Offset + SctpDataChunkFields.StreamSequenceNumberPosition);
      }
      set
      {
        EndianBitConverter.Big.CopyBytes(value, this.Header.Bytes, this.Header.Offset + SctpDataChunkFields.StreamSequenceNumberPosition);
      }
    }

    public SctpPayloadProtocol PayloadProtocol
    {
      get
      {
        return (SctpPayloadProtocol) EndianBitConverter.Big.ToUInt32(this.Header.Bytes, this.Header.Offset + SctpDataChunkFields.PayloadProtocolPosition);
      }
      set
      {
        EndianBitConverter.Big.CopyBytes((uint) value, this.Header.Bytes, this.Header.Offset + SctpDataChunkFields.PayloadProtocolPosition);
      }
    }

    public byte[] Padding { get; private set; }

    public new byte[] PayloadData
    {
      get => PayloadPacketOrData.Value.ByteArraySegment?.ActualBytes();
        set
      {
        if (value.Length > (int) ushort.MaxValue)
          throw new ArgumentException("Data inside an SCTP Data Chunk could be at most " + (object) ushort.MaxValue);
        byte[] Bytes = value;
        ushort length = (ushort) value.Length;
        if ((uint) length % 4U > 0U)
        {
          ushort num1 = (ushort) (4 - (int) length % 4);
          ushort num2 = (ushort) ((uint) length + (uint) num1);
          Bytes = new byte[(int) num2];
          Array.Copy((Array) value, 0, (Array) Bytes, 0, value.Length);
          this.Padding = new byte[(int) num1];
          this.Length = num2;
        }
        else
        {
          this.Padding = new byte[0];
          this.Length = length;
        }
          this.PayloadPacketOrData = new Lazy<PacketOrByteArraySegment>(() => new PacketOrByteArraySegment()
          {
              ByteArraySegment = new ByteArraySegment(Bytes, 0, Bytes.Length)
          });
      }
    }

    public SctpDataChunk(ByteArraySegment bas)
    {
      this.Header = bas;
      byte[] Bytes = new byte[(int) this.Length];
      Array.Copy((Array) bas.Bytes, bas.Offset, (Array) Bytes, 0, (int) this.Length);
      this.Header = new ByteArraySegment(Bytes);
      this.Header.Length = SctpDataChunkFields.HeaderLength;
      ByteArraySegment payload;
      if ((uint) this.Length % 4U > 0U)
      {
        int length = 4 - (int) this.Length % 4;
        int NewSegmentLength = (int) this.Length - SctpFields.HeaderLength;
        int num = this.Header.Bytes.Length - this.Header.Offset;
        if (num < NewSegmentLength + length)
          payload = new ByteArraySegment(((IEnumerable<byte>) this.Header.EncapsulatedBytes(NewSegmentLength).ActualBytes()).Concat<byte>((IEnumerable<byte>) new byte[length]).ToArray<byte>())
          {
            Length = num
          };
        else
          payload = this.Header.EncapsulatedBytes((int) this.Length);
      }
      else
        payload = this.Header.EncapsulatedBytes((int) this.Length);
        this.PayloadPacketOrData = new Lazy<PacketOrByteArraySegment>(() => SctpDataChunk.ParseEncapsulatedBytes(payload, this.PayloadProtocol));
    }

    public SctpDataChunk(ByteArraySegment bas, Packet parent)
      : this(bas)
    {
      this.ParentPacket = parent;
    }

    private static PacketOrByteArraySegment ParseEncapsulatedBytes(ByteArraySegment payload, SctpPayloadProtocol nextProtocol)
    {
      return new PacketOrByteArraySegment()
      {
        ByteArraySegment = payload
      };
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
        stringBuilder.AppendFormat("{0}[SctpDataChunk: Type={2}, Flags={3}, Length={4}, TSN={5}, Stream ID={6}, Stream SN={7}, Payload Protocol={8}]{1}", (object) str1, (object) str2, (object) this.Type, (object) this.Flags.ToString("X2"), (object) this.Length, (object) this.TransmissionSequenceNumber, (object) this.StreamIdentifier, (object) this.StreamSequenceNumber, (object) this.PayloadProtocol);
      return stringBuilder.ToString();
    }
  }
}
