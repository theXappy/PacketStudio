// Decompiled with JetBrains decompiler
// Type: PacketDotNet.SctpFields
// Assembly: PacketDotNet, Version=0.14.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F90CC42A-15BE-4D4B-8C38-4E8DECCB77A2
// Assembly location: C:\Desktop\PacketDotNet.dll

using System.Runtime.InteropServices;

namespace PacketDotNet
{
  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct SctpFields
  {
    public static readonly int SourcePortPosition = 0;
    public static readonly int DestinationPortPosition = 2;
    public static readonly int VerificationTagPosition = 4;
    public static readonly int ChecksumPosition = 8;
    public static readonly int HeaderLength = 12;
  }
}
