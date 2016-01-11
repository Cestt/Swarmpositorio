using UnityEngine;
using System.Collections;

[System.Serializable]
public class EVector2 {

	//Fields
	public float x,y;

	//Constructors
	public EVector2(){
		x = 0;
		y = 0;
	}
	public EVector2(float _x, float _y){
		x = _x;
		y = _y;
	}
	public EVector2(Vector2 vector){
		x = vector.x;
		y = vector.y;
	}
	public EVector2(Vector3 vector){
		x = vector.x;
		y = vector.y;
	}


	//Static properties
	public static EVector2 down
	{
		get
		{
			return new EVector2 (0f, -1f);
		}
	}
	
	public static EVector2 left
	{
		get
		{
			return new EVector2 (-1f, 0f);
		}
	}
	
	public static EVector2 one
	{
		get
		{
			return new EVector2 (1f, 1f);
		}
	}
	
	public static EVector2 right
	{
		get
		{
			return new EVector2 (1f, 0f);
		}
	}
	
	public static EVector2 up
	{
		get
		{
			return new EVector2 (0f, 1f);
		}
	}
	
	public static EVector2 zero
	{
		get
		{
			return new EVector2 (0f, 0f);
		}
	}

	public static EVector2 Normalized(EVector2 a)
	{

			float magn = Mathf.Sqrt (a.x * a.x + a.y * a.y);

			if (magn > 1E-05f)
			{
				return a / magn;
			}
			else
			{
				return  EVector2.zero;
			}


	}

	//
	// Operators
	//
	public static EVector2 operator + (EVector2 a, EVector2 b)
	{
		return new EVector2 (a.x + b.x, a.y + b.y);
	}
	
	public static EVector2 operator / (EVector2 a, float d)
	{
		return new EVector2 (a.x / d, a.y / d);
	}
	
	public static bool operator == (EVector2 lhs, EVector2 rhs)
	{
		return EVector2.SqrMagnitude (lhs - rhs) < 9.99999944E-11f;
	}
	
	public static implicit operator EVector2 (EVector3 v)
	{
		return new EVector2 (v.x, v.y);
	}
	
	public static implicit operator EVector3 (EVector2 v)
	{
		return new EVector3 (v.x, v.y, 0f);
	}
	
	public static bool operator != (EVector2 lhs, EVector2 rhs)
	{
		return EVector2.SqrMagnitude (lhs - rhs) >= 9.99999944E-11f;
	}
	
	public static EVector2 operator * (float d, EVector2 a)
	{
		return new EVector2 (a.x * d, a.y * d);
	}
	
	public static EVector2 operator * (EVector2 a, float d)
	{
		return new EVector2 (a.x * d, a.y * d);
	}
	
	public static EVector2 operator - (EVector2 a, EVector2 b)
	{
		return new EVector2 (a.x - b.x, a.y - b.y);
	}
	
	public static EVector2 operator - (EVector2 a)
	{
		return new EVector2 (-a.x, -a.y);
	}


	public static float SqrMagnitude (EVector2 a)
	{
		return a.x * a.x + a.y * a.y;
	}



}
