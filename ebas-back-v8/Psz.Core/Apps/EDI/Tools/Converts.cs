using System;

namespace Psz.Core.Apps.EDI.Tools
{
	public class Converts
	{

		public static bool nearlyEqual(float a, float b, float epsilon)
		{
			float absA = Math.Abs(a);
			float absB = Math.Abs(b);
			float diff = Math.Abs(a - b);

			return diff <= epsilon;

			//if(a == b)
			//{ // shortcut, handles infinities
			//	return true;
			//}
			//else if(a == 0 || b == 0 || absA + absB < float.MinValue)
			//{
			//	// a or b is zero or both are extremely close to it
			//	// relative error is less meaningful here
			//	return diff < (epsilon * float.MinValue);
			//}
			//else
			//{ // use relative error
			//	return diff / (absA + absB) < epsilon;
			//}
		}
	}
}
