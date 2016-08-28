using UnityEngine;

public class Axis3{
	public float x;
	public float y;
	public float z;

	public static Axis3 Convert(Vector3 vec){
		Axis3 axis = new Axis3();
		axis.x = vec.x;
		axis.y = vec.y;
		axis.z = vec.z;
		return axis;
	}
}