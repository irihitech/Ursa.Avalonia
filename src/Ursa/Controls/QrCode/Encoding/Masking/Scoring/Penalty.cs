namespace Gma.QrCodeNet.Encoding.Masking.Scoring;

internal abstract class Penalty
{
	internal abstract int PenaltyCalculate(BitMatrix matrix);
}
