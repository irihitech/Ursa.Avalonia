namespace Gma.QrCodeNet.Encoding.Positioning.Stencils;

internal class AlignmentPattern : PatternStencilBase
{
	public AlignmentPattern(int version)
		: base(version)
	{
	}

	private static bool[,] AlignmentPatternArray { get; } =
		new[,]
		{
				{ X, X, X, X, X },
				{ X, O, O, O, X },
				{ X, O, X, O, X },
				{ X, O, O, O, X },
				{ X, X, X, X, X }
		};

	public override bool[,] Stencil => AlignmentPatternArray;

	// Table E.1 — Row/column coordinates of center module of Alignment Patterns
	private static byte[][] AlignmentPatternCoordinatesByVersion { get; } =
    [
        [],
				[],
        [6, 18],
        [6, 22],
        [6, 26],
        [6, 30],
        [6, 34],
        [6, 22, 38],
        [6, 24, 42],
        [6, 26, 46],
        [6, 28, 50],
        [6, 30, 54],
        [6, 32, 58],
        [6, 34, 62],
        [6, 26, 46, 66],
        [6, 26, 48, 70],
        [6, 26, 50, 74],
        [6, 30, 54, 78],
        [6, 30, 56, 82],
        [6, 30, 58, 86],
        [6, 34, 62, 90],
        [6, 28, 50, 72, 94],
        [6, 26, 50, 74, 98],
        [6, 30, 54, 78, 102],
        [6, 28, 54, 80, 106],
        [6, 32, 58, 84, 110],
        [6, 30, 58, 86, 114],
        [6, 34, 62, 90, 118],
        [6, 26, 50, 74, 98, 122],
        [6, 30, 54, 78, 102, 126],
        [6, 26, 52, 78, 104, 130],
        [6, 30, 56, 82, 108, 134],
        [6, 34, 60, 86, 112, 138],
        [6, 30, 58, 86, 114, 142],
        [6, 34, 62, 90, 118, 146],
        [6, 30, 54, 78, 102, 126, 150],
        [6, 24, 50, 76, 102, 128, 154],
        [6, 28, 54, 80, 106, 132, 158],
        [6, 32, 58, 84, 110, 136, 162],
        [6, 26, 54, 82, 110, 138, 166],
        [6, 30, 58, 86, 114, 142, 170]
    ];

	public override void ApplyTo(TriStateMatrix matrix)
	{
		foreach (var coordinatePair in GetNonColidingCoordinatePairs(matrix))
		{
			CopyTo(matrix, coordinatePair, MatrixStatus.NoMask);
		}
	}

	public IEnumerable<MatrixPoint> GetNonColidingCoordinatePairs(TriStateMatrix matrix)
	{
		return
			GetAllCoordinatePairs()
				.Where(point => matrix.MStatus(point.Offset(2, 2)) == MatrixStatus.None);
	}

	private IEnumerable<MatrixPoint> GetAllCoordinatePairs()
	{
		var coordinates = GetPatternCoordinatesByVersion(Version).ToList();
		foreach (var centerX in coordinates)
		{
			foreach (var centerY in coordinates)
			{
				MatrixPoint location = new(centerX - 2, centerY - 2);
				yield return location;
			}
		}
	}

	private static IEnumerable<byte> GetPatternCoordinatesByVersion(int version)
	{
		return AlignmentPatternCoordinatesByVersion[version];
	}
}
