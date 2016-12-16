using UnityEngine;
using System;
using System.Collections;

public static class WeiVector2 {

    //Check if from vector R to vector D is ClockWise in a anti-Clockwise space(which we usually used in trigonometric function).
	public static bool IsClockWise_AngleBetween_R_D_AntiClockwise(float angle){
		float absAngle = Mathf.Abs(angle);
		if(angle <= 0.0f){
			if(absAngle>180.0f){
				return false;
			}else
				return true;
		}else{
			if(absAngle>180.0f){
				return true;
			}else
				return false;
		}
	}

    //Get the angle form R to D. based on the anti-Clockwise space(which we usually used in trigonometric function).
    public static float AngleBetween_R_D_AntiClockwise(Vector2 refVector,Vector2 destinationVector){
		WeiVector2Poarl pr = new WeiVector2Poarl(refVector);
		WeiVector2Poarl pd = new WeiVector2Poarl(destinationVector);
		return (pd.a -pr.a);
	}

    //Check which Region (which we usually used in trigonometric function) of the vector belong to.
    public static int VectorRegion(Vector2 v){
		bool ref_V1_or_V4 = v.x > 0;
		bool ref_V1_or_v2 = v.y > 0;

		int vectorRigion;

		if(ref_V1_or_V4){
			if(ref_V1_or_v2){
				vectorRigion = 1;
			}else{
				vectorRigion = 4;
			}
		}else{ // V2_or_V3
			if(ref_V1_or_v2){
				vectorRigion = 2;
			}else{
				vectorRigion = 3;
			}
		}
		return vectorRigion;
	}
}

public class WeiVector2Poarl{
	public float r;
	public float a;
	public WeiVector2Poarl(float radius, float angle){
		r = radius;
		a = angle;
	}

	public WeiVector2Poarl(Vector2 v2){		
		r = Mathf.Sqrt(v2.x * v2.x + v2.y * v2.y);
		Vector2 newv2 = v2.normalized;
		if(newv2.y ==0){
			if(newv2.x ==1)
				a = 0.0f;
			if(newv2.x ==-1)
				a = 180f;
		}else if(newv2.x ==0){
			if(newv2.y ==1)
				a = 90.0f;
			if(newv2.y ==-1){
				a = 270.0f;
			}
		}else{
			int region = WeiVector2.VectorRegion(v2);

			if(region==1)
				a =  Mathf.Atan(Mathf.Abs(v2.y / v2.x)) * 180/Mathf.PI;// + (WeiVector2.VectorRegion(v2) - 1)*90.0f;
			else if(region==2){
				a = 180.0f - Mathf.Atan(Mathf.Abs(v2.y / v2.x)) * 180/Mathf.PI;
			}else if(region==3){
				a = 180.0f + Mathf.Atan(Mathf.Abs(v2.y / v2.x)) * 180/Mathf.PI;
			}else{
				a = 360 - Mathf.Atan(Mathf.Abs(v2.y / v2.x)) * 180/Mathf.PI;
			}
		}
	}
}

public static class WeiVector3{
	public static Vector3 MoveToDestinationBasedOnSpeed(Vector3 currentPos,Vector3 destPos,float mps){
		return Vector3.Lerp(currentPos,destPos,mps*Time.deltaTime);
	}
}

public static class WeiTransform{
	public static void LookAtRaycastHit_X_Z(RaycastHit hit,Transform transform){
		Vector3 hitPos = hit.point;
		Vector3 look = new Vector3(hitPos.x, transform.position.y, hitPos.z);
		transform.LookAt(look);
	}

	public static void LookAtRaycastHit(RaycastHit hit,Transform transform){
		Vector3 hitPos = hit.point;
		transform.LookAt(hitPos);
	}
}

public static class MouseAndCamera{
	static Plane groundPlane = new Plane(Vector3.up,Vector3.zero);
	// function 
	public static Vector3 GetMouseGroundIntersectionPoint() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		float cameraToIntersectpointDst;

		if (groundPlane.Raycast(ray, out cameraToIntersectpointDst))
		{
			return ray.GetPoint(cameraToIntersectpointDst);
		}
		else return new Vector3(0.0f, 0.0f, 0.0f); //only if the screen parralel or opsite with the ground
	}

	public static void TramsformLookAtMouseGroundIntersection(Transform tramsform) {
		Vector3 intersectpoint = GetMouseGroundIntersectionPoint();
		tramsform.LookAt(new Vector3(intersectpoint.x, tramsform.position.y, intersectpoint.z));
	}

    public static RaycastHit GetScreenPointToRayColliderInfo(out RaycastHit info) {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out info,float.MaxValue);
        return info;
    }
}

public static class GaussianDist{
	static System.Random rand = new System.Random();
	/// <summary>
	/// This is use The  Box-Muller transform to get a randNormal
	/// </summary>
	/// <param name="mean">the average value</param>
	/// <param name="stdDev">the standard Devition</param>
	/// <returns></returns>
	public static double GetRandomNumber(float mean,float stdDev) {
		//reuse this if you are generating many
		double u1 = rand.NextDouble(); //these are uniform(0,1) random doubles
		double u2 = rand.NextDouble();
		double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
		return (mean + stdDev * randStdNormal); //random normal(mean,stdDev^2)
	}

	/// <summary>
	/// the function will give the probability density of the normal distribution 
	/// reference : https://en.wikipedia.org/wiki/Normal_distribution
	/// </summary>
	/// <param name="x"> x means the normal value which and f(x) is the crresponding Z value???????</param>
	/// <param name="u"> u means the average of the distribution</param>
	/// <param name="std"> std means the Standard Divition of the distribution</param>
	/// <returns></returns>

	public static double GetPDF(double x, double u, double std)
	{
		double exponent = -1 * (0.5 * Math.Pow(std,-2) * Math.Pow(x-u, 2));
		double numerator = Math.Pow(Math.E, exponent);
		double denominator = Math.Sqrt(2 * Math.PI * Math.Pow(std, 2));
		return numerator / denominator;
	}

	public static double StandardNormalPdf(double x)
	{
		double exponent = -1 * (0.5 * Math.Pow(x, 2));
		double numerator = Math.Pow(Math.E, exponent);
		double denominator = Math.Sqrt(2 * Math.PI);
		return numerator / denominator;
	}

}

public static class PhysicsRaycast{

    //Check if there is any collider between A and B
	public static bool DetecteCollisionFromAtoB(Vector3 fromPos,Vector3 toPos){
		Vector3 dir = (toPos - fromPos);
		float length = dir.magnitude;
		if(Physics.Raycast(fromPos,dir,length)){
			return true;
		}else{
			return false;
		}
	}

    //Check if there is any collider between A and B, if there is then output the collider info to RaycastHit out_hit.
	public static bool DetecteCollisionFromAtoB(Vector3 fromPos,Vector3 toPos,ref RaycastHit out_hit){
		Vector3 dir = (toPos - fromPos);
		float length = dir.magnitude;
		if(Physics.Raycast(fromPos,dir,out out_hit,length)){
			return true;
		}else{
			return false;
		}
	}
}
