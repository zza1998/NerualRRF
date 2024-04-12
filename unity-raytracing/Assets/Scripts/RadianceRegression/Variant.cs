using System;
using System.Runtime.CompilerServices;
using System.Globalization;
using System.Collections.Generic;

namespace PBRT {
	public class Variant : IConvertible {
        /*
        private List<object> values = new List<object>();
        private List<object> defaultValues = new List<object>();
        private bool outputDefault = false;

        public Variant()
			: this(null) {
		}

        public Variant(object defaultValue, bool outputDefault = false)
        {
            this.values.Add(defaultValue);
            this.defaultValues.Add(defaultValue);
            this.outputDefault = outputDefault;
        }

        public Variant(object value, object defaultValue, bool outputDefault = false)
        {
            this.values.Add(value);
            this.defaultValues.Add(defaultValue);
            this.outputDefault = outputDefault;
        }

        public Variant(List<object> defaultValues, bool outputDefault = false)
        {
            this.values = defaultValues.GetRange(0, defaultValues.Count);
            this.defaultValues = defaultValues.GetRange(0, defaultValues.Count);
            this.outputDefault = outputDefault;
        }

        public Variant(List<object> values, List<object> defaultValues, bool outputDefault = false)
        {
            this.values = values.GetRange(0, values.Count);
            this.defaultValues = defaultValues.GetRange(0, defaultValues.Count);
            this.outputDefault = outputDefault;
        }
		*/

        /*
        public Variant(object value) {
			this.value = value;
		}
		*/

        private object values;

        public Variant()
            : this(null)
        {
        }
        public Variant(object values)
        {
            this.values = values;
        }

#if NET_45_OR_GREATER
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        protected static string forceInteger(string v){
			string[] n = v.ToString().Split(new[]{
				CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator, 
				CultureInfo.CurrentCulture.NumberFormat.CurrencyGroupSeparator
			}, StringSplitOptions.None);
			return n[0];
		}
		#region Conversions
		public static implicit operator Variant(string v) {
			return new Variant(v);
		}
		public static implicit operator string(Variant v) {
			return v == null ? null : v.ToString();
		}

		public static implicit operator Variant(DateTime? v) {
			return new Variant(v);
		}
		public static implicit operator DateTime(Variant v) {
			DateTime o;
			DateTime.TryParse(v, out o);
			return o;
		}
		public static implicit operator DateTime?(Variant v) {
			DateTime o;
			if(DateTime.TryParse(v, out o))
				return o;
			return null;
		}

		public static implicit operator Variant(sbyte? v) {
			return new Variant(v);
		}
		public static implicit operator sbyte(Variant v) {
			sbyte o;
			sbyte.TryParse(forceInteger(v), out o);
			return o;
		}
		public static implicit operator sbyte?(Variant v) {
			sbyte o;
			if(sbyte.TryParse(forceInteger(v), out o))
				return o;
			return null;
		}

		public static explicit operator Variant(byte? v) {
			return new Variant(v);
		}
		public static implicit operator byte(Variant v) {
			byte o;
			byte.TryParse(forceInteger(v), out o);
			return o;
		}
		public static implicit operator byte?(Variant v) {
			byte o;
			if(byte.TryParse(forceInteger(v), out o))
				return o;
			return null;
		}

		public static implicit operator Variant(short? v) {
			return new Variant(v);
		}
		public static implicit operator short(Variant v) {
			short o;
			short.TryParse(forceInteger(v), out o);
			return o;
		}
		public static implicit operator short?(Variant v) {
			short o;
			if(short.TryParse(forceInteger(v), out o))
				return o;
			return null;
		}

		public static implicit operator Variant(int? v) {
			return new Variant(v);
		}
		public static implicit operator int(Variant v) {
			int o;
			//int.TryParse(forceInteger(v), out o);
			int.TryParse(v, out o);
			return o;
		}
		public static implicit operator int?(Variant v) {
			int o;
			//if(int.TryParse(forceInteger(v), out o))
			if (int.TryParse(v, out o))
				return o;
			return null;
		}

		public static implicit operator Variant(long? v) {
			return new Variant(v);
		}
		public static implicit operator long(Variant v) {
			long o;
			long.TryParse(forceInteger(v), out o);
			return o;
		}
		public static implicit operator long?(Variant v) {
			long o;
			if(long.TryParse(forceInteger(v), out o))
				return o;
			return null;
		}

		public static explicit operator Variant(ushort? v) {
			return new Variant(v);
		}
		public static implicit operator ushort(Variant v) {
			ushort o;
			ushort.TryParse(forceInteger(v), out o);
			return o;
		}
		public static implicit operator ushort?(Variant v) {
			ushort o;
			if(ushort.TryParse(forceInteger(v), out o))
				return o;
			return null;
		}

		public static explicit operator Variant(uint? v) {
			return new Variant(v);
		}
		public static implicit operator uint(Variant v) {
			uint o;
			uint.TryParse(forceInteger(v), out o);
			return o;
		}
		public static implicit operator uint?(Variant v) {
			uint o;
			if(uint.TryParse(forceInteger(v), out o))
				return o;
			return null;
		}

		public static explicit operator Variant(ulong? v) {
			return new Variant(v);
		}
		public static implicit operator ulong(Variant v) {
			ulong o;
			ulong.TryParse(forceInteger(v), out o);
			return o;
		}
		public static implicit operator ulong?(Variant v) {
			ulong o;
			if(ulong.TryParse(forceInteger(v), out o))
				return o;
			return null;
		}
        
		public static implicit operator Variant(float? v) {
			return new Variant(v);
		}
		public static implicit operator float(Variant v) {
			float o;
			float.TryParse(v, out o);
			return o;
		}
		public static implicit operator float?(Variant v) {
			float o;
			if(float.TryParse(v, out o))
				return o;
			return null;
		}

		public static implicit operator Variant(double? v) {
			return new Variant(v);
		}
		public static implicit operator double(Variant v) {
			double o;
			double.TryParse(v, out o);
			return o;
		}
		public static implicit operator double?(Variant v) {
			double o;
			if(double.TryParse(v, out o))
				return o;
			return null;
		}

		public static implicit operator Variant(bool? v) {
			return new Variant(v.GetValueOrDefault() ? 1 : 0);
		}
		public static implicit operator bool(Variant v) {
			try {
				return double.Parse(v, CultureInfo.InvariantCulture) != 0;
			}
			catch {
				return v.ToString().Length > 0;
			}
		}
		public static implicit operator bool?(Variant v) {
			if(v == null)
				return null;
			return (bool)v;
		}

		public static explicit operator Variant(char? v) {
			return new Variant(v);
		}
		public static implicit operator char(Variant v) {
			char o;
			char.TryParse(v, out o);
			return o;
		}
		public static implicit operator char?(Variant v) {
			char o;
			if(char.TryParse(v, out o))
				return o;
			return null;
		}

		public static implicit operator Variant(decimal? v) {
			return new Variant(v);
		}
		public static implicit operator decimal(Variant v) {
			decimal o;
			decimal.TryParse(v, out o);
			return o;
		}
		public static implicit operator decimal?(Variant v) {
			decimal o;
			if(decimal.TryParse(v, out o))
				return o;
			return null;
		}
		#endregion

		#region IConvertible

		public TypeCode GetTypeCode() {
			return TypeCode.Object;
		}

		bool IConvertible.ToBoolean(IFormatProvider provider) {
			return this;
		}

		byte IConvertible.ToByte(IFormatProvider provider) {
			return this;
		}

		char IConvertible.ToChar(IFormatProvider provider) {
			return this;
		}

		DateTime IConvertible.ToDateTime(IFormatProvider provider) {
			return this;
		}

		decimal IConvertible.ToDecimal(IFormatProvider provider) {
			return this;
		}

		short IConvertible.ToInt16(IFormatProvider provider) {
			return this;
		}

		int IConvertible.ToInt32(IFormatProvider provider) {
			return this;
		}

		long IConvertible.ToInt64(IFormatProvider provider) {
			return this;
		}

		sbyte IConvertible.ToSByte(IFormatProvider provider) {
			return this;
		}

		float IConvertible.ToSingle(IFormatProvider provider) {
			return this;
		}

		string IConvertible.ToString(IFormatProvider provider) {
			return this;
		}

		object IConvertible.ToType(Type conversionType, IFormatProvider provider) {
			return Convert.ChangeType(this, conversionType);
		}

		ushort IConvertible.ToUInt16(IFormatProvider provider) {
			return this;
		}

		uint IConvertible.ToUInt32(IFormatProvider provider) {
			return this;
		}

		ulong IConvertible.ToUInt64(IFormatProvider provider) {
			return this;
		}

		double IConvertible.ToDouble(IFormatProvider provider) {
			return this;
		}
		#endregion

		public static bool operator ==(Variant a, Variant b) {
			bool na = (object)a == null || a.values == null, nb = (object)b == null || b.values == null;
			return na && nb || (!na && !nb && a.values.ToString() == b.values.ToString());
		}
		public static bool operator !=(Variant a, Variant b) {
			return !(a == b);
		}
		public override bool Equals(object obj) {
			//return this == obj is Variant ? (Variant)obj : new Variant(obj);
			return this == (Variant)obj;
		}
		public override int GetHashCode() {
			return base.GetHashCode();
		}
		public override string ToString() {
			return values.ToString() + "";
		}
	}
}