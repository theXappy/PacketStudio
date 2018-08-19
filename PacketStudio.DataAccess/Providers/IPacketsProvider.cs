using System;
using System.Collections.Generic;
using PacketStudio.DataAccess.SaveData;

namespace PacketStudio.DataAccess.Providers
{
	public interface IPacketsProvider : IEnumerable<PacketSaveData>, IDisposable
	{
	}


}
