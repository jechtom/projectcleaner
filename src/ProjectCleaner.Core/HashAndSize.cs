using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCleaner.Core
{
    public struct HashAndSize
    {
        public Hash Hash;
        public long Size;

        public HashAndSize(Hash hash, long size)
        {
            this.Hash = hash;
            this.Size = size;
        }

        public override int GetHashCode()
        {
            // hash is random and anything xor random is random
            return this.Size.GetHashCode() ^ this.Hash.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is HashAndSize))
                return false;

            var second = (HashAndSize)obj;

            var result = Size == second.Size && Hash.Equals(second.Hash);
            return result;
        }

        public override string ToString()
        {
            return string.Format("{0}B {1}", Size, Hash);
        }

        public static Hash Parse(string input)
        {
            byte[] bytes = ByteArrayHelper.StringToByteArray(input);
            return new Hash(bytes);
        }
    }
}
