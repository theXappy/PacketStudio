// Decompiled with JetBrains decompiler
// Type: PacketDotNet.Sctp.Chunks.SctpChunk
// Assembly: PacketDotNet, Version=0.14.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F90CC42A-15BE-4D4B-8C38-4E8DECCB77A2
// Assembly location: C:\Desktop\PacketDotNet.dll


using PacketDotNet.Utils.Converters;

namespace PacketDotNet.Sctp.Chunks
{
  public abstract class SctpChunk : Packet
  {
    public SctpChunkType Type
    {
      get
      {
        return (SctpChunkType) ((int) this.Header.Bytes[this.Header.Offset + SctpChunkFields.TypePosition] & (int) SctpChunkFields.TypeMask);
      }
      set
      {
        byte num = (byte) ((uint) this.Header.Bytes[this.Header.Offset + SctpChunkFields.TypePosition] & ((uint) SctpChunkFields.TypeMask ^ (uint) byte.MaxValue));
        this.Header.Bytes[this.Header.Offset + SctpChunkFields.TypePosition] = (byte) ((uint) num | (uint) (byte) value & (uint) SctpChunkFields.TypeMask);
      }
    }

    public ErrorActionType SkipOrStopBit
    {
      get
      {
        return (ErrorActionType) (byte) (((int) this.Header.Bytes[this.Header.Offset + SctpChunkFields.SkipOrStopBitPosition] & (int) SctpChunkFields.SkipOrStopBitMask) >> SctpChunkFields.SkipOrStopBitShift);
      }
      set
      {
        byte num1 = (byte) ((uint) this.Header.Bytes[this.Header.Offset + SctpChunkFields.SkipOrStopBitPosition] & ((uint) SctpChunkFields.SkipOrStopBitMask ^ (uint) byte.MaxValue));
        byte num2 = (byte) ((uint) (byte) value << SctpChunkFields.SkipOrStopBitShift & (uint) SctpChunkFields.SkipOrStopBitMask);
        this.Header.Bytes[this.Header.Offset + SctpChunkFields.SkipOrStopBitPosition] = (byte) ((uint) num1 | (uint) num2);
      }
    }

    public ErrorReportType ReportBit
    {
      get
      {
        return (ErrorReportType) (byte) (((int) this.Header.Bytes[this.Header.Offset + SctpChunkFields.ReportBitPosition] & (int) SctpChunkFields.ReportBitMask) >> SctpChunkFields.ReportBitShift);
      }
      set
      {
        byte num1 = (byte) ((uint) this.Header.Bytes[this.Header.Offset + SctpChunkFields.ReportBitPosition] & ((uint) SctpChunkFields.ReportBitMask ^ (uint) byte.MaxValue));
        byte num2 = (byte) ((uint) (byte) value << SctpChunkFields.ReportBitShift & (uint) SctpChunkFields.ReportBitMask);
        this.Header.Bytes[this.Header.Offset + SctpChunkFields.ReportBitPosition] = (byte) ((uint) num1 | (uint) num2);
      }
    }

    public byte Flags
    {
      get
      {
        return this.Header.Bytes[this.Header.Offset + SctpChunkFields.FlagsPosition];
      }
      set
      {
        this.Header.Bytes[this.Header.Offset + SctpChunkFields.FlagsPosition] = value;
      }
    }

    public ushort Length
    {
      get
      {
        return EndianBitConverter.Big.ToUInt16(this.Header.Bytes, this.Header.Offset + SctpChunkFields.LengthPosition);
      }
      set
      {
        EndianBitConverter.Big.CopyBytes(value, this.Header.Bytes, this.Header.Offset + SctpChunkFields.LengthPosition);
      }
    }

    public uint TransmissionSequenceNumber
    {
      get
      {
        return EndianBitConverter.Big.ToUInt32(this.Header.Bytes, this.Header.Offset + SctpChunkFields.TsnPosition);
      }
      set
      {
        EndianBitConverter.Big.CopyBytes(value, this.Header.Bytes, this.Header.Offset + SctpChunkFields.TsnPosition);
      }
    }
  }
}
