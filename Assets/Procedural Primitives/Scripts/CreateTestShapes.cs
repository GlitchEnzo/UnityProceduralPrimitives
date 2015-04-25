using UnityEngine;
using System.Collections;

public class CreateTestShapes : MonoBehaviour 
{
	/// <summary>
	/// An example of procedurally creating primitive shapes that will exist in the scene.
	/// </summary>
	void Start() 
	{
		/*
        GameObject sphere = Primitive.CreateSphereGameObject(0.5f, 100, 100);
        sphere.transform.position = new Vector3(0, 1, 0);

        GameObject cylinder = Primitive.CreateCylinderGameObject(0.5f, 0.5f, 1, 20, 20);
        cylinder.transform.position = new Vector3(2.5f, 1, 0);

        GameObject cone = Primitive.CreateCylinderGameObject(0.5f, 0.0f, 1, 20, 20);
        cone.transform.position = new Vector3(0, 3.0f, 0);

        GameObject box = Primitive.CreateBoxGameObject(1.0f, 2.0f, 1.0f);
        box.transform.position = new Vector3(-2.5f, 1.5f, 0);

        Primitive.CreatePlaneGameObject(10, 10, 2, 2);
        */

		GameObject circle = new GameObject("Circle");
		circle.CreateCircle(2.5f, 20, 0, 2 * Mathf.PI); // full circle
		//circle.CreateCircle(10, 20, 0, Mathf.PI); // half circle
		//circle.CreateCircle(10, 20, Mathf.PI / 2.0f, Mathf.PI); // half circle starting at PI/2
		//circle.CreateCircle(10, 20, Mathf.PI / 2.0f, Mathf.PI / 2.0f); // quarter circle starting at PI/2
		//circle.transform.position = new Vector3(-2.5f, 0.0f, 0);

		GameObject plane = new GameObject("Plane");
		plane.CreatePlane(5, 5, 5, 5);
		plane.transform.position = new Vector3(5f, 0.0f, 0);

		GameObject box = new GameObject("Box");
		box.CreateBox(5, 5, 5, 5, 5, 5);
		box.transform.position = new Vector3(-5f, 0.0f, 0);
	}  
}
