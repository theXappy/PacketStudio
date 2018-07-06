// Decompiled with JetBrains decompiler
// Type: PacketDotNet.Sctp.Chunks.Heartbeat.SctpHeartbeatChunkFields
// Assembly: PacketDotNet, Version=0.14.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F90CC42A-15BE-4D4B-8C38-4E8DECCB77A2
// Assembly location: C:\Desktop\PacketDotNet.dll

using System.Runtime.InteropServices;

namespace PacketDotNet.Sctp.Chunks.Heartbeat
{
  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct SctpHeartbeatChunkFields
  {
    public static readonly int TypePosition = 0;
    public static readonly int FlagsPosition = 1;
    public static readonly int LengthPosition = 2;
    public static readonly int ParameterTypePosition = 4;
    public static readonly ushort ParameterTypeMask = 16383;
    public static readonly int SkipOrStopBitPosition = 4;
    public static readonly ushort SkipOrStopBitMask = 32768;
    public static readonly int SkipOrStopBitShift = 7;
    public static readonly int ReportBitPosition = 4;
    public static readonly ushort ReportBitMask = 16384;
    public static readonly int ReportBitShift = 6;
    public static readonly int ParameterLengthPosition = 6;
    public static readonly int HeaderLength = 8;
  }
}
