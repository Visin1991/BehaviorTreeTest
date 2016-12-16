using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AiUtility{

    public class FieldOfView
    {
        public Transform eye;
        float viewDistance;
        float viewAngle;

        public FieldOfView(Transform _eye,float _distance,float _angle) {
            eye = _eye;
            viewDistance = _distance;
            viewAngle = _angle;
        }

        public T GetNearestObjInsideFieldOfView<T>() where T : MonoBehaviour {
            List<T> allObjectInrange = new List<T>();
            List<T> allObjectInView = new List<T>();

            //Find all Object in the Radius range
            AiFind.Finds<T>(eye.position, viewDistance,ref allObjectInrange);

            //find all Object in View
            foreach (T t in allObjectInrange) {
                if (Vector3.Angle(eye.forward, t.transform.position - eye.position) < viewAngle / 2.0f)
                {
                    allObjectInView.Add(t);
                }
            }

            float miniDis = float.MaxValue;
            T nearestObj = null;

            foreach (T obj in allObjectInView)
            {
                float dist = (obj.transform.position - eye.position).magnitude;
                if (dist < miniDis) {
                    miniDis = dist;
                    nearestObj = obj;
                }
            }

            return nearestObj;

        }

        public List<Collider> GetAllColliderInsideFieldOfView()
        {
            List<Collider> allColliderInView = new List<Collider>();

            //get all Collider in a sphere
            Collider[] allColliders =  Physics.OverlapSphere(eye.position, viewDistance);

            //get all Collider in View
            foreach (Collider c in allColliders) {
                if (Vector3.Angle(eye.forward, c.transform.position - eye.position) < viewAngle / 2) {
                    allColliderInView.Add(c);
                }
            }
            return allColliderInView;
        }

        public List<Collider> GetAllColliderInsideFieldOfView(LayerMask layerMask)
        {
            List<Collider> allColliderInView = new List<Collider>();

            //get all Collider in a sphere
            Collider[] allColliders = Physics.OverlapSphere(eye.position, viewDistance,layerMask);

            //get all Collider in View
            foreach (Collider c in allColliders)
            {
                if (Vector3.Angle(eye.forward, c.transform.position - eye.position) < viewAngle / 2)
                {
                    allColliderInView.Add(c);
                }
            }
            return allColliderInView;
        }

        public Collider GetNearestColliderInsideFieldOfView() {
            return AiFind.FindNearestColliderOverlapSphere(eye.position, viewDistance);
        }

        public Collider GetNearestColliderInsideFieldOfView(LayerMask layerMask)
        {
            return AiFind.FindNearestColliderOverlapSphere(eye.position, viewDistance,layerMask);
        }

    }

    //Static Filed........
    public static class AiFind
    {
        //Find the nearest T; center is a given position;
        public static T FindNearestObj<T>(Vector3 position) where T : MonoBehaviour
        {

            float minDistance = float.MaxValue;

            T retObj = null;
            foreach (T obj in Object.FindObjectsOfType(typeof(T)))
            {
                //Debug.Log(obj);
                float dist = (obj.gameObject.transform.position - position).magnitude;
                if (dist < minDistance)
                {
                    minDistance = dist;
                    retObj = obj;
                }
            }
            return retObj;
        }

        //Find the nearest T which inside the radius of range; center is a given position;
        public static T FindNearestObj<T>(Vector3 position, float range) where T : MonoBehaviour
        {

            float minDistance = float.MaxValue;

            T retObj = null;
            foreach (T obj in Object.FindObjectsOfType(typeof(T)))
            {
                //Debug.Log(obj);
                float dist = (obj.gameObject.transform.position - position).magnitude;
                if (dist < minDistance)
                {
                    minDistance = dist;
                    retObj = obj;
                }
            }

            //Check the range
            if ((retObj.gameObject.transform.position - position).magnitude <= range)
            {
                return retObj;
            }
            else
            {
                return null;
            }
        }

        public static Object[] Finds<T>() where T : MonoBehaviour
        {
           return Object.FindObjectsOfType(typeof(T));
        }

        //Find all T Objects inside the raduis range; center is a given position. and out put to the out_objs. Return all object inside the Scene
        public static Object[] Finds<T>(Vector3 position, float range, ref List<T> out_objs) where T : MonoBehaviour
        {
            Object[] objs = Object.FindObjectsOfType(typeof(T));
            foreach (T obj in objs)
            {
                if ((obj.transform.position - position).magnitude <= range)
                {
                    out_objs.Add(obj);
                }
            }
            return objs;
        }

        //Find all Collider with LayerMask of targetMask inside the raduis range; center is a given position
        public static Collider[] FindCollidersOverlapSphere(Vector3 position, float range)
        {
            Collider[] targetsInRangeRadius = Physics.OverlapSphere(position, range); //gameobject with targetMask will be selected
            return targetsInRangeRadius;
        }

        //Find all Collider with LayerMask of targetMask inside the raduis range; center is a given position
        public static Collider[] FindCollidersOverlapSphere(Vector3 position, float range, LayerMask targetMask)
        {
            Collider[] targetsInRangeRadius = Physics.OverlapSphere(position, range, targetMask); //gameobject with targetMask will be selected
            return targetsInRangeRadius;
        }

        //Find the nearest Collider with LayerMask of targetMask inside the raduis range; center is a given position
        public static Collider FindNearestColliderOverlapSphere(Vector3 position, float range)
        {

            Collider[] targetsInRangeRadius = Physics.OverlapSphere(position, range); //gameobject with targetMask will be selected

            Collider nearestCollider = null;
            float nearestDis = float.MaxValue;

            foreach (Collider collider in targetsInRangeRadius)
            {
                float dist = (collider.transform.position - position).magnitude;
                if (dist < nearestDis)
                {
                    nearestDis = dist;
                    nearestCollider = collider;
                }
            }
            return nearestCollider;
        }

        //Find the nearest Collider with LayerMask of targetMask inside the raduis range; center is a given position
        public static Collider FindNearestColliderOverlapSphere(Vector3 position, float range, LayerMask targetMask)
        {

            Collider[] targetsInRangeRadius = Physics.OverlapSphere(position, range, targetMask); //gameobject with targetMask will be selected

            Collider nearestCollider = null;
            float nearestDis = float.MaxValue;

            foreach (Collider collider in targetsInRangeRadius)
            {
                float dist = (collider.transform.position - position).magnitude;
                if (dist < nearestDis)
                {
                    nearestDis = dist;
                    nearestCollider = collider;
                }
            }
            return nearestCollider;
        }

        //Find the nearest Collider with LayerMask of targetMask inside the raduis range; center is a given position
        public static Collider FindNearestColliderOverlapSphere(Vector2 position, float range, LayerMask targetMask)
        {
            Collider[] targetsInRangeRadius = Physics.OverlapSphere(position, range, targetMask); //gameobject with targetMask will be selected

            Collider nearestCollider = null;
            float nearestDis = float.MaxValue;

            foreach (Collider collider in targetsInRangeRadius)
            {
                float dist = (new Vector2(collider.transform.position.x, collider.transform.position.z) - position).magnitude;
                if (dist < nearestDis)
                {
                    nearestDis = dist;
                    nearestCollider = collider;
                }
            }
            return nearestCollider;
        }
    }

}
