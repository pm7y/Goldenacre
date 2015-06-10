namespace Goldenacre.Core
{
    /// <summary>
    /// </summary>
    /// <remarks>n/a.</remarks>
    internal sealed class ConvertMatrix
    {
        internal int intBottomLeft;
        internal int intBottomMid;
        internal int intBottomRight;
        internal int intFactor = 1;
        internal int intMidLeft;
        internal int intMidRight;
        internal int intOffset;
        internal int intPixel = 1;
        internal int intTopLeft;
        internal int intTopMid;
        internal int intTopRight;

        internal void SetAll(int nVal)
        {
            intTopLeft = nVal;
            intTopMid = nVal;
            intTopRight = nVal;
            intMidLeft = nVal;
            intPixel = nVal;
            intMidRight = nVal;
            intBottomLeft = nVal;
            intBottomMid = nVal;
            intBottomRight = nVal;
        }
    }
}