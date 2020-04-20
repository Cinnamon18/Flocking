using UnityEngine;

public static class RNG {
	public static float NextGaussianDouble(float mean, float stdev) {
		double u, v, S;

		do {
			u = 2.0 * Random.value - 1.0;
			v = 2.0 * Random.value - 1.0;
			S = u * u + v * v;
		}
		while (S >= 1.0);

		double fac = System.Math.Sqrt(-2.0 * System.Math.Log(S) / S);
		float idk = (float)(u * fac);
		idk *= stdev;
		idk += mean;
		if(idk < 0) {
			idk = 0;
		}
		return idk;
	}
}