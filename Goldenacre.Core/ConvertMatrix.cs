namespace Goldenacre.Core
{
    /// <summary>
    /// </summary>
    /// <remarks>n/a.</remarks>
    internal sealed class ConvertMatrix
    {
        internal int IntBottomLeft;
        internal int IntBottomMid;
        internal int IntBottomRight;
        internal int IntFactor = 1;
        internal int IntMidLeft;
        internal int IntMidRight;
        internal int IntOffset;
        internal int IntPixel = 1;
        internal int IntTopLeft;
        internal int IntTopMid;
        internal int IntTopRight;

        internal void SetAll(int nVal)
        {
            IntTopLeft = nVal;
            IntTopMid = nVal;
            IntTopRight = nVal;
            IntMidLeft = nVal;
            IntPixel = nVal;
            IntMidRight = nVal;
            IntBottomLeft = nVal;
            IntBottomMid = nVal;
            IntBottomRight = nVal;
        }
    }
}