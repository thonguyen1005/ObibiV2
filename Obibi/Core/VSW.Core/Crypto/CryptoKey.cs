using System;
using System.Collections.Generic;
using System.Text;

namespace VSW.Core.Crypto
{
    public class CryptoKey
    {
        public string Key { get; private set; }

        public byte[] _rawData;

        public byte[] GetRawKey()
        {
            if (_rawData == null && Key.IsNotEmpty())
            {
                _rawData = EncryptionHelper.FromBase64(Key);
            }
            return _rawData;
        }

        public CryptoKey(string key)
        {
            Key = key;
            _rawData = EncryptionHelper.FromBase64(Key);
        }

        public CryptoKey(byte[] key)
        {
            _rawData = key;
            Key = EncryptionHelper.Base64(_rawData);
        }

        public static implicit operator CryptoKey(string value)
        {
            return new CryptoKey(value);
        }

        public static implicit operator CryptoKey(byte[] value)
        {
            return new CryptoKey(value);
        }

        public static implicit operator string(CryptoKey key)
        {
            return key.Key;
        }

        public static implicit operator byte[](CryptoKey key)
        {
            return key.GetRawKey();
        }

    }
}
