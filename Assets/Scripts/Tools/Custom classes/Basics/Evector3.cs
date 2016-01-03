using UnityEngine;
using System.Collections;

public class EVector3 {

	//Fields
	public float x,y,z;


	//Constructors
	public EVector3(){
		x = 0;
		y = 0;
		z = 0;
	}
	public EVector3(float _x, float _y, float _z){
		x = _x;
		y = _y;
		z = _z;
	}
	public EVector3(Vector2 vector){
		x = vector.x;
		y = vector.y;
		z = 0;
	}
	public EVector3(Vector3 vector){
		x = vector.x;
		y = vector.y;
		z = vector.z;
	}

	//Static Properties

	public static EVector3 back
	{
		get
		{
			return new EVector3 (0f, 0f, -1f);
		}
	}
	
	public static EVector3 down
	{
		get
		{
			return new EVector3 (0f, -1f, 0f);
		}
	}
	
	public static EVector3 forward
	{
		get
		{
			return new EVector3 (0f, 0f, 1f);
		}
	}
	
	public static EVector3 left
	{
		get
		{
			return new EVector3 (-1f, 0f, 0f);
		}
	}
	
	public static EVector3 one
	{
		get
		{
			return new EVector3 (1f, 1f, 1f);
		}
	}
	
	public static EVector3 right
	{
		get
		{
			return new EVector3 (1f, 0f, 0f);
		}
	}
	
	public static EVector3 up
	{
		get
		{
			return new EVector3 (0f, 1f, 0f);
		}
	}
	
	public static EVector3 zero
	{
		get
		{
			return new EVector3 (0f, 0f, 0f);
		}
	}

	//Operators

	public static EVector3 operator + (EVector3 a, EVector3 b)
	{
		return new EVector3 (a.x + b.x, a.y + b.y, a.z + b.z);
	}
	
	public static EVector3 operator / (EVector3 a, float d)
	{
		return new EVector3 (a.x / d, a.y / d, a.z / d);
	}
	
	public static bool operator == (EVector3 lhs, EVector3 rhs)
	{
		return EVector3.SqrMagnitude (lhs - rhs) < 9.99999944E-11f;
	}
	
	public static bool operator != (EVector3 lhs, EVector3 rhs)
	{
		return EVector3.SqrMagnitude (lhs - rhs) >= 9.99999944E-11f;
	}
	
	public static EVector3 operator * (EVector3 a, float d)
	{
		return new EVector3 (a.x * d, a.y * d, a.z * d);
	}
	
	public static EVector3 operator * (float d, EVector3 a)
	{
		return new EVector3 (a.x * d, a.y * d, a.z * d);
	}
	
	public static EVector3 operator - (EVector3 a, EVector3 b)
	{
		return new EVector3 (a.x - b.x, a.y - b.y, a.z - b.z);
	}
	
	public static EVector3 operator - (EVector3 a)
	{
		return new EVector3 (-a.x, -a.y, -a.z);
	}


	public static float SqrMagnitude (EVector3 a)
	{
		return a.x * a.x + a.y * a.y + a.z * a.z;
	}
	
}
