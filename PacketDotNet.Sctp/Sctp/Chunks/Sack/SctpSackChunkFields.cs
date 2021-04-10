// Decompiled with JetBrains decompiler
// Type: PacketDotNet.Sctp.Chunks.Sack.SctpSackChunkFields
// Assembly: PacketDotNet, Version=0.14.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F90CC42A-15BE-4D4B-8C38-4E8DECCB77A2
// Assembly location: C:\Desktop\PacketDotNet.dll

using System.Runtime.InteropServices;

namespace PacketDotNet.Sctp.Chunks.Sack
{
  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct SctpSackChunkFields
  {
    public static readonly int TypePosition = 0;
    public static readonly int FlagsPosition = 1;
    public static readonly int LengthPosition = 2;
    public static readonly int TsnPosition = 4;
    public static readonly int AdvertisedReceiverWindowCredit = 8;
    public static readonly int GapAckBlocks = 12;
    public static readonly int DuplicatedTSNs = 14;
    public static readonly int HeaderLength = 16;
  }
}
