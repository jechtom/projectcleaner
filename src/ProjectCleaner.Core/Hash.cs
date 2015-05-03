using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCleaner.Core
{
    [TypeConverter(typeof(Hash.Converter))]
    public struct Hash
    {
        public byte[] Bytes;

        public Hash(byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException("bytes");

            this.Bytes = bytes;
        }

        public override int GetHashCode()
        {
            // hash should have random distribution, take first 4 bytes
            return BitConverter.ToInt32(Bytes, 0);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Hash))
                return false;

            var second = (Hash)obj;

            var result = Enumerable.SequenceEqual(Bytes, second.Bytes);
            return result;
        }

        public override string ToString()
        {
            return ByteArrayHelper.ByteArrayToString(Bytes);
        }

        public static Hash Parse(string input)
        {
            byte[] bytes = ByteArrayHelper.StringToByteArray(input);
            return new Hash(bytes);
        }

        public class Converter : System.ComponentModel.TypeConverter
        {
            public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext context, Type sourceType)
            {
                if (sourceType == typeof(string) || sourceType == typeof(Hash))
                    return true;

                return false;
            }

            public override bool CanConvertTo(System.ComponentModel.ITypeDescriptorContext context, Type destinationType)
            {
                if (destinationType == typeof(string) || destinationType == typeof(Hash))
                    return true;

                return false;
            }

            public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
            {
                if (value is string)
                {
                    var bytes = ByteArrayHelper.StringToByteArray((string)value);
                    return new Hash(bytes);
                }

                if (value is Hash)
                {
                    return value;
                }

                return base.ConvertFrom(context, culture, value);
            }

            public override object ConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
            {
                if (destinationType == typeof(string))
                {
                    return value.ToString();
                }

                if (destinationType == typeof(Hash))
                {
                    return value;
                }

                return base.ConvertTo(context, culture, value, destinationType);
            }
        }
    }
}
