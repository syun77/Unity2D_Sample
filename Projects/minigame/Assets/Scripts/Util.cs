using UnityEngine;
using System.Collections;

public class Util {
	public static float CosEx(float Deg) {
		return Mathf.Cos(Mathf.Deg2Rad * Deg);
	}
	public static float SinEx(float Deg) {
		return Mathf.Sin(Mathf.Deg2Rad * Deg);
	}
}
