// Decompiled with JetBrains decompiler
// Type: PacketDotNet.Sctp.Chunks.SctpDataChunkFields
// Assembly: PacketDotNet, Version=0.14.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F90CC42A-15BE-4D4B-8C38-4E8DECCB77A2
// Assembly location: C:\Desktop\PacketDotNet.dll

using System.Runtime.InteropServices;

namespace PacketDotNet.Sctp.Chunks
{
  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct SctpDataChunkFields
  {
    public static readonly int TypePosition = 0;
    public static readonly int FlagsPosition = 1;
    public static readonly int LengthPosition = 2;
    public static readonly int TsnPosition = 4;
    public static readonly int StreamIdentifierPosition = 8;
    public static readonly int StreamSequenceNumberPosition = 10;
    public static readonly int PayloadProtocolPosition = 12;
    public static readonly int HeaderLength = 16;
  }
}
