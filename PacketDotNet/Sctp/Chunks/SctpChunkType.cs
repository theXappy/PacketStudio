// Decompiled with JetBrains decompiler
// Type: PacketDotNet.Sctp.Chunks.SctpChunkType
// Assembly: PacketDotNet, Version=0.14.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F90CC42A-15BE-4D4B-8C38-4E8DECCB77A2
// Assembly location: C:\Desktop\PacketDotNet.dll

namespace PacketDotNet.Sctp.Chunks
{
  public enum SctpChunkType
  {
    Data,
    Init,
    InitAck,
    Sack,
    Heartbeat,
    HeartbeatAck,
    Abort,
    Shutdown,
    ShutdownAck,
    Error,
    CookieEcho,
    CookieAck,
    Ecne,
    Cwr,
    ShutdownComplete,
  }
}
