namespace ProceduralPrimitives
{
    using UnityEngine;

    /// <summary>
    /// Holds extension methods for a Unity <see cref="GameObject"/>.
    /// </summary>
    public static class GameObjectExtensions
    {
        /// <summary>
        /// Adds a <see cref="MeshFilter"/> to the <see cref="GameObject"/> and assigns it the given <see cref="Mesh"/>.
        /// </summary>
        /// <param name="gameObject">The <see cref="GameObject"/> to add the <see cref="MeshFilter"/> to.</param>
        /// <param name="mesh">The <see cref="Mesh"/> to assign.</param>
        /// <returns>The newly created and added <see cref="MeshFilter"/>.</returns>
        public static MeshFilter AddMeshFilter(this GameObject gameObject, Mesh mesh)
        {
            MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
            meshFilter.sharedMesh = mesh;

            return meshFilter;
        }

        /// <summary>
        /// Adds a <see cref="MeshRenderer"/> to the <see cref="GameObject"/> and disables shadows on it.
        /// </summary>
        /// <param name="gameObject">The <see cref="GameObject"/> to add the <see cref="MeshRenderer"/> to.</param>
        /// <returns>The newly created and added <see cref="MeshRenderer"/>.</returns>
        public static MeshRenderer AddMeshRenderer(this GameObject gameObject)
        {
            MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
            meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            meshRenderer.receiveShadows = false;

            return meshRenderer;
        }

        /// <summary>
        /// Adds a <see cref="MeshRenderer"/> to the <see cref="GameObject"/>, disables shadows on it, and assigns it the given <see cref="Material"/>.
        /// </summary>
        /// <param name="gameObject">The <see cref="GameObject"/> to add the <see cref="MeshRenderer"/> to.</param>
        /// <param name="material">The <see cref="Material"/> to assign.</param>
        /// <returns>The newly created and added <see cref="MeshRenderer"/>.</returns>
        public static MeshRenderer AddMeshRenderer(this GameObject gameObject, Material material)
        {
            MeshRenderer meshRenderer = gameObject.AddMeshRenderer();
            meshRenderer.sharedMaterial = material;

            return meshRenderer;
        }

        /// <summary>
        /// Adds a <see cref="MeshRenderer"/> to the <see cref="GameObject"/>, disables shadows on it, assigns it the given <see cref="Material"/>, adds 
        /// a <see cref="MeshFilter"/> (if there isn't one), and assigns it the given <see cref="Mesh"/>.
        /// </summary>
        /// <param name="gameObject">The <see cref="GameObject"/> to add the <see cref="MeshRenderer"/> to.</param>
        /// <param name="material">The <see cref="Material"/> to assign.</param>
        /// <param name="mesh">The <see cref="Mesh"/> to assign.</param>
        /// <returns>The newly created and added <see cref="MeshRenderer"/>.</returns>
        public static MeshRenderer AddMeshRenderer(this GameObject gameObject, Material material, Mesh mesh)
        {
            // add a MeshFilter automatically, if there isn't already one
            MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
            if (meshFilter == null)
            {
                gameObject.AddMeshFilter(mesh);
            }
            else
            {
                meshFilter.sharedMesh = mesh;
            }

            return gameObject.AddMeshRenderer(material);
        }

        /// <summary>
        /// Creates a <see cref="GameObject"/> that has a box <see cref="Mesh"/>, a <see cref="MeshRenderer"/>.
        /// </summary>
        /// <param name="gameObject">The <see cref="GameObject"/> to add the <see cref="MeshRenderer"/> to.</param>
        /// <param name="radius">Radius of the circle. Value should be greater than or equal to 0.0f.</param>
        /// <param name="segments">The number of segments making up the circle. Value should be greater than or equal to 3.</param>
        /// <param name="startAngle">The starting angle of the circle.  Usually 0.</param>
        /// <param name="angularSize">The angular size of the circle.  2 pi is a full circle. Pi is a half circle.</param>
        public static void CreateCircle(this GameObject gameObject, float radius, int segments, float startAngle,
            float angularSize)
        {
            Mesh mesh = new Mesh();
            mesh.CreateCircle(radius, segments, startAngle, angularSize);

            //gameObject.name = "Circle";

            Shader shader = Shader.Find("Diffuse");
            gameObject.AddMeshRenderer(new Material(shader), mesh);
        }

        public static void CreatePlane(this GameObject gameObject, float width, float height, int widthSegments,
            int heightSegments)
        {
            Mesh mesh = new Mesh();
            mesh.CreatePlane(width, height, widthSegments, heightSegments);

            //gameObject.name = "Plane";

            Shader shader = Shader.Find("Diffuse");
            gameObject.AddMeshRenderer(new Material(shader), mesh);
        }

        public static void CreateBox(this GameObject gameObject, float width, float height, float depth,
            int widthSegments, int heightSegments, int depthSegments)
        {
            Mesh mesh = new Mesh();
            mesh.CreateBox(width, height, depth, widthSegments, heightSegments, depthSegments);

            //gameObject.name = "Box";

            Shader shader = Shader.Find("Diffuse");
            gameObject.AddMeshRenderer(new Material(shader), mesh);
        }
    }
}