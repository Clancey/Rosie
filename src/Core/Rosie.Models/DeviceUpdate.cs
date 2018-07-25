using System;
using SQLite;
using Newtonsoft.Json;
using System.Dynamic;
using System.Collections.Generic;
using Newtonsoft.Json.Converters;

namespace Rosie
{
	public enum DataTypes
	{
		Bool = 1,
		Byte = 2,
		Decimal = 3,
		Int = 4,
		List = 5,
		Short = 6,
		Raw = 7,
		String = 8,
	}
	public class DeviceUpdate
	{
		[PrimaryKey]
		public virtual string Id { get; set; } = Guid.NewGuid ().ToString ();
		public virtual string DeviceId { get; set; }
		public virtual string PropertyKey { get; set; }

		object value;
		[Ignore]
		public object Value {
			get { return value; }
			set {
				this.value = value;
				SetCastedValue ();
			}
		}
		public string DataFormat { get; set; }
		DataTypes dataType;
		public DataTypes DataType { 
			get { return dataType; }
			set {
				if (dataType == value)
					return;
				dataType = value;
				SetCastedValue ();
			}
		}
		//[JsonConverter (typeof (IsoDateTimeConverter))]
		public DateTime DateTime { get; set; } = DateTime.Now;

		//These are the proper casted types

		bool? boolValue;
		[JsonIgnore]
		public bool? BoolValue {
			get { return boolValue;} 
			set {
				if (value == null)
					return;
				this.value = boolValue = value;
			}
		}

		byte? byteValue;
		[JsonIgnore]
		public byte? ByteValue {
			get { return byteValue; }
			set {
				if (value == null)
					return;
				this.value = byteValue = value;
			}
		}

		double? decimalValue;
		[JsonIgnore]
		public double? DecimalValue {
			get { return decimalValue; }
			set {
				if (value == null)
					return;
				this.value = decimalValue = value;
			}
		}

		int? intValue;
		[JsonIgnore]
		public int? IntValue {
			get { return intValue; }
			set {
				if (value == null)
					return;
				this.value = intValue = value;
			}
		}

		Int16? shortValue;
		[JsonIgnore]
		public Int16? ShortValue {
			get { return shortValue; }
			set {
				if (value == null)
					return;
				this.value = shortValue = value;
			}
		}

		string stringValue;
		[JsonIgnore]
		public string StringValue {
			get { return stringValue; }
			set {
				if (value == null)
					return;
				this.value = stringValue = value;
			}
		}
		//TODO: List
		string [] listValue;
		[JsonIgnore]
		[Ignore]
		public string [] ListValue {
			get { return listValue; }
			set {
				if (value == null)
					return;
				listValue = value;
				 this.value = this.StringValue = string.Join (",", value);
			}

		}
		byte [] rawValue;
		[JsonIgnore]
		public byte [] RawValue {
			get { return rawValue; }
			set {
				if (value == null)
					return;
				this.value = rawValue = value;
			}
		}

		void SetCastedValue ()
		{
			try {
				if (value == null || (int)DataType == 0)
					return;
				switch (DataType) {
				case DataTypes.Bool:
					boolValue = Convert.ToBoolean (Value);
					return;
				case DataTypes.Byte:
					byteValue = Convert.ToByte (Value);
					return;
				case DataTypes.Decimal:
					decimalValue = Convert.ToDouble (Value);
					return;
				case DataTypes.Int:
					intValue = Convert.ToInt32 (Value);
					return;
				case DataTypes.List:
					listValue = Value?.ToString ().Split (new char [] { ',' }, StringSplitOptions.RemoveEmptyEntries);
					return;
				case DataTypes.Raw:
					rawValue = (byte [])Value;
					return;
				case DataTypes.Short:
					shortValue = Convert.ToInt16 (Value);
					return;
				case DataTypes.String:
					stringValue = value?.ToString ();
					return;
				}
			} catch (Exception ex) {
				Console.WriteLine (value);
				Console.WriteLine (ex);
				return;
			}
			throw new NotImplementedException ();
		}

		public virtual object ToSimpleObject ()
		{
			return new Dictionary<string, object>{
				{PropertyKey,Value},
				{nameof(DateTime),DateTime},
				{nameof(DataFormat),DataFormat},
				{nameof(DataType),DataType.ToString()}
			};
		}

	}
}

