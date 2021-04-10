// Decompiled with JetBrains decompiler
// Type: PacketDotNet.Sctp.Chunks.SctpUnsupportedChunk
// Assembly: PacketDotNet, Version=0.14.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F90CC42A-15BE-4D4B-8C38-4E8DECCB77A2
// Assembly location: C:\Desktop\PacketDotNet.dll

using PacketDotNet.Utils;
using System;
using System.Text;

namespace PacketDotNet.Sctp.Chunks
{
  public class SctpUnsupportedChunk : SctpChunk
  {
    public SctpUnsupportedChunk(ByteArraySegment bas)
    {
      this.Header = bas;
      byte[] Bytes = new byte[(int) this.Length];
      Array.Copy((Array) bas.Bytes, bas.Offset, (Array) Bytes, 0, (int) this.Length);
      this.Header = new ByteArraySegment(Bytes);
      this.Header.Length = SctpChunkFields.HeaderLength;
      ByteArraySegment byteArraySegment = this.Header.NextSegment((int) this.Length - SctpChunkFields.HeaderLength);
      this.PayloadPacketOrData = new LazySlim<PacketOrByteArraySegment>(()=> new PacketOrByteArraySegment()
      {
        ByteArraySegment = byteArraySegment
      });
    }

    public SctpUnsupportedChunk(ByteArraySegment bas, Packet parent)
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
        stringBuilder.AppendFormat("{0}[SctpUnsupportedChunk: Type={2}, Flags={3}, Length={4}, TSN={5}]{1}", (object) str1, (object) str2, (object) this.Type, (object) this.Flags.ToString("X2"), (object) this.Length, (object) this.TransmissionSequenceNumber);
      return stringBuilder.ToString();
    }
  }
}
