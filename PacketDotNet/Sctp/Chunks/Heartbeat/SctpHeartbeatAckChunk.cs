// Decompiled with JetBrains decompiler
// Type: PacketDotNet.Sctp.Chunks.Heartbeat.SctpHeartbeatAckChunk
// Assembly: PacketDotNet, Version=0.14.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F90CC42A-15BE-4D4B-8C38-4E8DECCB77A2
// Assembly location: C:\Desktop\PacketDotNet.dll

using PacketDotNet.MiscUtil.Conversion;
using PacketDotNet.Utils;
using System;
using System.Text;

namespace PacketDotNet.Sctp.Chunks.Heartbeat
{
  public class SctpHeartbeatAckChunk : SctpChunk
  {
    public SctpChunkType ParameterType
    {
      get
      {
        return (SctpChunkType) ((int) this.Header.Bytes[this.Header.Offset + SctpHeartbeatChunkFields.ParameterTypePosition] & (int) SctpHeartbeatChunkFields.ParameterTypeMask);
      }
      set
      {
        byte num = (byte) ((uint) this.Header.Bytes[this.Header.Offset + SctpHeartbeatChunkFields.ParameterTypePosition] & ((uint) SctpHeartbeatChunkFields.ParameterTypeMask ^ (uint) byte.MaxValue));
        this.Header.Bytes[this.Header.Offset + SctpHeartbeatChunkFields.ParameterTypePosition] = (byte) ((uint) num | (uint) (byte) value & (uint) SctpHeartbeatChunkFields.ParameterTypeMask);
      }
    }

    public new ErrorActionType SkipOrStopBit
    {
      get
      {
        return (ErrorActionType) (byte) (((int) this.Header.Bytes[this.Header.Offset + SctpHeartbeatChunkFields.SkipOrStopBitPosition] & (int) SctpHeartbeatChunkFields.SkipOrStopBitMask) >> SctpHeartbeatChunkFields.SkipOrStopBitShift);
      }
      set
      {
        byte num1 = (byte) ((uint) this.Header.Bytes[this.Header.Offset + SctpHeartbeatChunkFields.SkipOrStopBitPosition] & ((uint) SctpHeartbeatChunkFields.SkipOrStopBitMask ^ (uint) byte.MaxValue));
        byte num2 = (byte) ((uint) (byte) value << SctpHeartbeatChunkFields.SkipOrStopBitShift & (uint) SctpHeartbeatChunkFields.SkipOrStopBitMask);
        this.Header.Bytes[this.Header.Offset + SctpHeartbeatChunkFields.SkipOrStopBitPosition] = (byte) ((uint) num1 | (uint) num2);
      }
    }

    public new ErrorReportType ReportBit
    {
      get
      {
        return (ErrorReportType) (byte) (((int) this.Header.Bytes[this.Header.Offset + SctpHeartbeatChunkFields.ReportBitPosition] & (int) SctpHeartbeatChunkFields.ReportBitMask) >> SctpHeartbeatChunkFields.ReportBitShift);
      }
      set
      {
        byte num1 = (byte) ((uint) this.Header.Bytes[this.Header.Offset + SctpHeartbeatChunkFields.ReportBitPosition] & ((uint) SctpHeartbeatChunkFields.ReportBitMask ^ (uint) byte.MaxValue));
        byte num2 = (byte) ((uint) (byte) value << SctpHeartbeatChunkFields.ReportBitShift & (uint) SctpHeartbeatChunkFields.ReportBitMask);
        this.Header.Bytes[this.Header.Offset + SctpHeartbeatChunkFields.ReportBitPosition] = (byte) ((uint) num1 | (uint) num2);
      }
    }

    public ushort ParameterLength
    {
      get
      {
        return EndianBitConverter.Big.ToUInt16(this.Header.Bytes, this.Header.Offset + SctpHeartbeatChunkFields.ParameterLengthPosition);
      }
      set
      {
        EndianBitConverter.Big.CopyBytes(value, this.Header.Bytes, this.Header.Offset + SctpHeartbeatChunkFields.ParameterLengthPosition);
      }
    }

    public byte[] HeartbeatInfo
    {
      get
      {
        return this.PayloadData;
      }
      set
      {
        this.PayloadData = value;
      }
    }

    public SctpHeartbeatAckChunk(ByteArraySegment bas)
    {
      this.Header = bas;
      byte[] Bytes = new byte[(int) this.Length];
      Array.Copy((Array) bas.Bytes, bas.Offset, (Array) Bytes, 0, (int) this.Length);
      this.Header = new ByteArraySegment(Bytes);
      this.Header.Length = SctpHeartbeatChunkFields.HeaderLength;
      this.PayloadPacketOrData = new Lazy<PacketOrByteArraySegment>(()=> new PacketOrByteArraySegment()
      {
        ByteArraySegment = this.Header.EncapsulatedBytes()
      });
    }

    public SctpHeartbeatAckChunk(ByteArraySegment bas, Packet parent)
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
        stringBuilder.AppendFormat("{0}[SctpSackChunk: ParameterType={2}, ParameterLength={3}]{1}", (object) str1, (object) str2, (object) this.ParameterType, (object) this.ParameterLength);
      return stringBuilder.ToString();
    }
  }
}
