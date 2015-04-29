namespace ProceduralPrimitives
{
    using UnityEngine;
    using System.Collections.Generic;

    public class InGameMenu : MonoBehaviour
    {
        public Material material;

        private PrimitiveType primitiveType = PrimitiveType.Box;
        private PrimitiveType prevPrimitiveType = PrimitiveType.Box;

        // box, plane
        private float width = 5;
        private float height = 5;
        private float depth = 5;
        private int widthDivisions = 10;
        private int heightDivisions = 10;
        private int depthDivisions = 10;

        // sphere
        private float radius = 2.5f;
        private int stacks = 20;
        private int slices = 20;
        private float phiStart = 0;
        private float phiLength = 2*Mathf.PI;
        private float thetaStart = 0;
        private float thetaLength = Mathf.PI;

        // cylinder
        private float topRadius = 1;
        private float bottomRadius = 1;
        private float length = 5;
        private bool openEnded = false;

        // circle
        private int segments = 20;
        private float startAngle = 0;
        private float angularSize = 2*Mathf.PI;

        // ring
        private float innerRadius = 3;
        private float outerRadius = 5;
        private int phiSegments = 20;

        // torus
        private float tube = 1;
        private int radialSegments = 50;
        private int tubularSegments = 20;
        private float arc = 2*Mathf.PI;

        // torus knot
        private int p = 2;
        private int q = 3;
        private float heightScale = 1;

        private GUIStyle listStyle;
        private EnumComboBox comboBoxControl = new EnumComboBox();

        private GameObject shape;
        private MeshFilter filter;
        private MouseCamera mouseCamera;

        private bool wireframe;

        private List<Vector3> points = new List<Vector3>(); // {new };

        private void Start()
        {
            listStyle = new GUIStyle();
            listStyle.normal.textColor = Color.white;

            var texture = new Texture2D(2, 2);
            listStyle.hover.background = texture;
            listStyle.onHover.background = texture;

            listStyle.padding.left = listStyle.padding.right = listStyle.padding.top = listStyle.padding.bottom = 4;

            shape = new GameObject("Test Shape");
            shape.CreateBox(width, height, depth, widthDivisions, heightDivisions, depthDivisions);
            shape.GetComponent<Renderer>().sharedMaterial = material;

            filter = shape.GetComponent<MeshFilter>();
            GL.wireframe = true;

            mouseCamera = GetComponent<MouseCamera>();
            mouseCamera.target = shape.transform;

            for (var i = 0; i < 50; i ++)
            {
                //points.Add(new Vector3(Mathf.Sin( i * 0.2f ) * Mathf.Sin( i * 0.1f ) * 15 + 50, 0, ( i - 5.0f ) * 2.0f ) );
                points.Add(new Vector3(Mathf.Sin(i*0.2f)*Mathf.Sin(i*0.1f)*15, 0, (i - 5.0f)*2.0f));
                //points.Add(new Vector3(Mathf.Sin(i * 0.4f) * Mathf.Sin(i * 0.2f) * 1, 0, (i-2) * 2.0f));
            }
        }

        private void OnPreRender()
        {
            GL.wireframe = wireframe;
        }

        private void OnPostRender()
        {
            GL.wireframe = false;
        }

        private void OnGUI()
        {
            points.Clear();
            for (var i = 0; i < 50; i ++)
            {
                points.Add(new Vector3(Mathf.Sin(i*0.2f)*Mathf.Sin(i*0.1f)*15 + 50, 0, (i - 25.0f)*2.0f));
                //points.Add(new Vector3(Mathf.Sin(i * 0.2f) * Mathf.Sin(i * 0.1f) * 15 + 40, 0, i));
                //points.Add(new Vector3(Mathf.Sin(i * 0.4f) * Mathf.Sin(i * 0.2f) * 1, 0, (i-2) * 2.0f));
            }

            GUILayout.Label("Primitive Type: ", GUILayout.MinWidth(500));
            primitiveType =
                (PrimitiveType) comboBoxControl.List(new Rect(100, 2, 100, 20), prevPrimitiveType, listStyle);

            if (primitiveType != prevPrimitiveType)
            {
                // if it's a 2D shape, reset the camera position so it is visible
                if (primitiveType == PrimitiveType.Circle ||
                    primitiveType == PrimitiveType.Plane ||
                    primitiveType == PrimitiveType.Ring)
                {
                    Camera.main.transform.localPosition = new Vector3(0, 0, -10);
                    Camera.main.transform.localRotation = Quaternion.identity;

                    mouseCamera.UpdateAngles();
                }

                if (prevPrimitiveType == PrimitiveType.Lathe)
                {
                    shape.transform.localScale = Vector3.one;
                    //Camera.main.transform.localPosition = new Vector3(0, 0, -10);
                    //Camera.main.transform.localRotation = Quaternion.identity;

                    //mouseCamera.UpdateAngles();
                }

                if (primitiveType == PrimitiveType.Lathe)
                {
                    shape.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                    //Camera.main.transform.localPosition = new Vector3(0, 0, -10);
                    //Camera.main.transform.localRotation = Quaternion.identity;

                    //mouseCamera.UpdateAngles();
                }

                prevPrimitiveType = primitiveType;
            }

            wireframe = GUILayout.Toggle(wireframe, "  Wireframe");

            switch (primitiveType)
            {
                case PrimitiveType.Box:
                    width = GUILayoutHelper.Slider("Width: ", width, 0.01f, 10);
                    height = GUILayoutHelper.Slider("Height: ", height, 0.01f, 10);
                    depth = GUILayoutHelper.Slider("Depth: ", depth, 0.01f, 10);
                    widthDivisions = GUILayoutHelper.IntSlider("Width Divisions: ", widthDivisions, 1, 75);
                    heightDivisions = GUILayoutHelper.IntSlider("Height Divisions: ", heightDivisions, 1, 75);
                    depthDivisions = GUILayoutHelper.IntSlider("Depth Divisions: ", depthDivisions, 1, 75);
                    break;
                case PrimitiveType.Circle:
                    radius = GUILayoutHelper.Slider("Radius: ", radius, 0.01f, 5);
                    segments = GUILayoutHelper.IntSlider("Segments: ", segments, 3, 50);
                    startAngle = GUILayoutHelper.Slider("Start Angle: ", startAngle, 0.0f, Mathf.PI*2);
                    angularSize = GUILayoutHelper.Slider("Angular Size: ", angularSize, 0.0f, Mathf.PI*2);
                    break;
                case PrimitiveType.Cylinder:
                    bottomRadius = GUILayoutHelper.Slider("Bottom Radius: ", bottomRadius, 0.0f, 10);
                    topRadius = GUILayoutHelper.Slider("Top Radius: ", topRadius, 0.0f, 10);
                    length = GUILayoutHelper.Slider("Length: ", length, 0.01f, 10);
                    stacks = GUILayoutHelper.IntSlider("Stacks: ", stacks, 1, 100);
                    slices = GUILayoutHelper.IntSlider("Slices: ", slices, 2, 100);
                    openEnded = GUILayout.Toggle(openEnded, "  Open Ended");
                    break;
                case PrimitiveType.Lathe:
                    //radius = GUILayoutHelper.Slider("Radius: ", radius, 0.01f, 5);
                    segments = GUILayoutHelper.IntSlider("Segments: ", segments, 3, 50);
                    startAngle = GUILayoutHelper.Slider("Start Angle: ", startAngle, 0.0f, Mathf.PI*2);
                    angularSize = GUILayoutHelper.Slider("Angular Size: ", angularSize, 0.0f, Mathf.PI*2);
                    break;
                case PrimitiveType.Plane:
                    width = GUILayoutHelper.Slider("Width: ", width, 0.01f, 10);
                    height = GUILayoutHelper.Slider("Height: ", height, 0.01f, 10);
                    widthDivisions = GUILayoutHelper.IntSlider("Width Divisions: ", widthDivisions, 1, 100);
                    heightDivisions = GUILayoutHelper.IntSlider("Height Divisions: ", heightDivisions, 1, 100);
                    break;
                case PrimitiveType.Ring:
                    innerRadius = GUILayoutHelper.Slider("Inner Radius: ", innerRadius, 0.01f, 15);
                    outerRadius = GUILayoutHelper.Slider("Outer Radius: ", outerRadius, 0.01f, 15);
                    segments = GUILayoutHelper.IntSlider("Theta Segments: ", segments, 3, 50);
                    phiSegments = GUILayoutHelper.IntSlider("Phi Segments: ", phiSegments, 1, 50);
                    startAngle = GUILayoutHelper.Slider("Start Angle: ", startAngle, 0.0f, Mathf.PI*2);
                    angularSize = GUILayoutHelper.Slider("Angular Size: ", angularSize, 0.0f, Mathf.PI*2);
                    break;
                case PrimitiveType.Sphere:
                    radius = GUILayoutHelper.Slider("Radius: ", radius, 0.01f, 5);
                    stacks = GUILayoutHelper.IntSlider("Stacks: ", stacks, 1, 100);
                    slices = GUILayoutHelper.IntSlider("Slices: ", slices, 1, 100);
                    phiStart = GUILayoutHelper.Slider("Phi Start: ", phiStart, 0.0f, 2*Mathf.PI);
                    phiLength = GUILayoutHelper.Slider("Phi Length: ", phiLength, 0.0f, 2*Mathf.PI);
                    thetaStart = GUILayoutHelper.Slider("Theta Start: ", thetaStart, 0.0f, Mathf.PI);
                    thetaLength = GUILayoutHelper.Slider("Theta Length: ", thetaLength, 0.0f, Mathf.PI);
                    break;
                case PrimitiveType.Torus:
                    radius = GUILayoutHelper.Slider("Radius: ", radius, 0.01f, 5);
                    tube = GUILayoutHelper.Slider("Tube Radius: ", tube, 0.01f, 5);
                    radialSegments = GUILayoutHelper.IntSlider("Radial Segments: ", radialSegments, 8, 100);
                    tubularSegments = GUILayoutHelper.IntSlider("Tubular Segments: ", tubularSegments, 6, 100);
                    arc = GUILayoutHelper.Slider("Arc: ", arc, 0.0f, 2*Mathf.PI);
                    break;
                case PrimitiveType.TorusKnot:
                    radius = GUILayoutHelper.Slider("Radius: ", radius, 0.01f, 5);
                    tube = GUILayoutHelper.Slider("Tube Radius: ", tube, 0.01f, 5);
                    radialSegments = GUILayoutHelper.IntSlider("Radial Segments: ", radialSegments, 8, 100);
                    tubularSegments = GUILayoutHelper.IntSlider("Tubular Segments: ", tubularSegments, 6, 100);
                    p = GUILayoutHelper.IntSlider("P: ", p, 1, 10);
                    q = GUILayoutHelper.IntSlider("Q: ", q, 1, 10);
                    heightScale = GUILayoutHelper.Slider("Height Scale: ", heightScale, 0.0f, 2.0f);
                    break;
            }

            //primitiveType = (PrimitiveType)comboBoxControl.List(new Rect(100, 2, 100, 20), primitiveType, listStyle);

            //if (GUILayout.Button("Create"))
            {
                switch (primitiveType)
                {
                    case PrimitiveType.Box:
                        filter.sharedMesh.CreateBox(width, height, depth, widthDivisions, heightDivisions,
                            depthDivisions);
                        break;
                    case PrimitiveType.Circle:
                        filter.sharedMesh.CreateCircle(radius, segments, startAngle, angularSize);
                        break;
                    case PrimitiveType.Cylinder:
                        filter.sharedMesh.CreateCylinder(topRadius, bottomRadius, length, slices, stacks, openEnded);
                        break;
                    case PrimitiveType.Lathe:
                        filter.sharedMesh.CreateLathe(points, segments, startAngle, angularSize);
                        break;
                    case PrimitiveType.Plane:
                        filter.sharedMesh.CreatePlane(width, height, widthDivisions, heightDivisions);
                        break;
                    case PrimitiveType.Ring:
                        filter.sharedMesh.CreateRing(innerRadius, outerRadius, segments, phiSegments, startAngle,
                            angularSize);
                        break;
                    case PrimitiveType.Sphere:
                        filter.sharedMesh.CreateSphere(radius, slices, stacks, phiStart, phiLength, thetaStart,
                            thetaLength);
                        break;
                    case PrimitiveType.Torus:
                        filter.sharedMesh.CreateTorus(radius, tube, radialSegments, tubularSegments, arc);
                        break;
                    case PrimitiveType.TorusKnot:
                        filter.sharedMesh.CreateTorusKnot(radius, tube, radialSegments, tubularSegments, p, q,
                            heightScale);
                        break;
                }
            }

        }

    }
}