﻿// Copyright (C) 2023 Jonah Stagner (StagPoint). All rights reserved.

using System;
using System.IO;
using System.Text;

namespace StagPoint.EDF.Net
{

	/// <summary>
	/// Stores a fixed-length ASCII string, which will be right-padded with spaces as necessary to maintain
	/// the fixed-length requirement. In the Header file, these string must only contain the ASCII characters
	/// 32..126 (inclusive).
	/// </summary>
	public class EdfAsciiString : EdfAsciiField
	{
		#region Public properties

		public virtual string Value
		{
			get { return _value; }
			set
			{
				if( value == null )
				{
					throw new ArgumentNullException( $"NULL values are not supported by the {nameof( Value )} property. Use String.Empty for empty strings.", nameof( Value ) );
				}

				if( value.Length < FieldLength )
				{
					_value = value;
				}
				else
				{
					_value = value.Substring( 0, FieldLength );
				}
			}
		}

		#endregion

		#region Constructors

		public EdfAsciiString( int fieldLength ) : base( fieldLength ) { }

		public EdfAsciiString( int fieldLength, string value )
			: base( fieldLength )
		{
			this.Value = value;
		}

		#endregion

		#region Private fields

		private string _value = string.Empty;

		#endregion

		#region EdfAsciiField overrides

		internal override void WriteTo( BinaryWriter buffer )
		{
			if( Value == null )
			{
				throw new NullReferenceException( $"The {nameof( Value )} property of the {nameof( EdfAsciiString )} object was set to NULL" );
			}

			BufferHelper.WriteToBuffer( buffer, Value, FieldLength );
		}

		internal override void ReadFrom( BinaryReader buffer )
		{
			_value = BufferHelper.ReadFromBuffer( buffer, this.FieldLength );
		}

		#endregion

		#region Base class overrides and implicit type conversion

		/// <inheritdoc />
		public override string ToString()
		{
			return _value;
		}

		/// <inheritdoc />
		public static implicit operator string( EdfAsciiString field )
		{
			// It may be tempting to change this function to return the _value field directly. Don't. Subclasses rely on calling ToString(). 
			return field.ToString();
		}

		#endregion
	}
}
