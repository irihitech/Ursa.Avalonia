using System;

namespace Gma.QrCodeNet.Encoding;

/// <summary>
/// Use this exception for null or empty input string or when input string is too large.
/// </summary>
internal class InputOutOfBoundaryException : Exception
{
	public InputOutOfBoundaryException() : base()
	{
	}

	public InputOutOfBoundaryException(string message) : base(message)
	{
	}
}
