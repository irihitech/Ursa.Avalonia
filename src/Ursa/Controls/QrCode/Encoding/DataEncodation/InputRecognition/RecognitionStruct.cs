namespace Gma.QrCodeNet.Encoding.DataEncodation.InputRecognition;

internal struct RecognitionStruct
{
	public RecognitionStruct(string encodingName)
		: this()
	{
		EncodingName = encodingName;
	}

	public string EncodingName { get; private set; }
}
