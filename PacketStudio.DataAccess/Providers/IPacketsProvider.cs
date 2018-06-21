using System;
using System.Collections.Generic;

namespace PacketStudio.DataAccess.Providers
{
	public interface IPacketsProvider : IEnumerable<PacketSaveData>, IDisposable
	{
	}


}
