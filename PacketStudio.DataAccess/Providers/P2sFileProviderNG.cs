using System;
using System.Collections;
using System.Collections.Generic;
using PacketStudio.DataAccess.SaveData;

namespace PacketStudio.DataAccess.Providers
{
    public class P2sFileProviderNG : IPacketsProviderNG
    {
        public P2sFileProviderNG(string path)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IEnumerator<PacketSaveDataNG> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}