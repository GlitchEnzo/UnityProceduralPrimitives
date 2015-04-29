namespace ProceduralPrimitives
{
    using System;
    using UnityEngine;

    /// <summary>
    /// Defines static creation methods that procedurally create 3D primitive shapes like spheres, boxes, cones, planes, and cylinders.
    /// 
    /// DEPRECATED!
    /// 
    /// Please use the extension methods in MeshExtensions and GameObjectExtensions to programatically create primitives now.
    /// This is being left in as a reference for how things used to work, and to aid in backwards compatibility with old versions.
    /// </summary>
    public static class Primitive
    {
        #region Meshes

        /// <summary>
        /// Creates a <see cref="Mesh"/> filled with vertices forming a box.
        /// </summary>
        /// <param name="width">Width of the box, along the x-axis.</param>
        /// <param name="height">Height of the box, along the y-axis.</param>
        /// <param name="depth">Depth of the box, along the z-axis.</param>
        /// <returns>A <see cref="Mesh"/> filled with vertices forming a box.</returns>
        public static Mesh CreateBoxMesh(float width, float height, float depth)
        {
            Mesh mesh = new Mesh();
            mesh.name = "BoxMesh";

            // Because the box is centered at the origin, need to divide by two to find the + and - offsets
            width = width/2.0f;
            height = height/2.0f;
            depth = depth/2.0f;

            Vector3[] boxVertices = new Vector3[36];
            Vector3[] boxNormals = new Vector3[36];
            Vector2[] boxUVs = new Vector2[36];

            Vector3 topLeftFront = new Vector3(-width, height, depth);
            Vector3 bottomLeftFront = new Vector3(-width, -height, depth);
            Vector3 topRightFront = new Vector3(width, height, depth);
            Vector3 bottomRightFront = new Vector3(width, -height, depth);
            Vector3 topLeftBack = new Vector3(-width, height, -depth);
            Vector3 topRightBack = new Vector3(width, height, -depth);
            Vector3 bottomLeftBack = new Vector3(-width, -height, -depth);
            Vector3 bottomRightBack = new Vector3(width, -height, -depth);

            Vector2 textureTopLeft = new Vector2(0.0f, 0.0f);
            Vector2 textureTopRight = new Vector2(1.0f, 0.0f);
            Vector2 textureBottomLeft = new Vector2(0.0f, 1.0f);
            Vector2 textureBottomRight = new Vector2(1.0f, 1.0f);

            Vector3 frontNormal = new Vector3(0.0f, 0.0f, 1.0f);
            Vector3 backNormal = new Vector3(0.0f, 0.0f, -1.0f);
            Vector3 topNormal = new Vector3(0.0f, 1.0f, 0.0f);
            Vector3 bottomNormal = new Vector3(0.0f, -1.0f, 0.0f);
            Vector3 leftNormal = new Vector3(-1.0f, 0.0f, 0.0f);
            Vector3 rightNormal = new Vector3(1.0f, 0.0f, 0.0f);

            // Front face.
            boxVertices[0] = topLeftFront;
            boxNormals[0] = frontNormal;
            boxUVs[0] = textureTopLeft;
            boxVertices[1] = bottomLeftFront;
            boxNormals[1] = frontNormal;
            boxUVs[1] = textureBottomLeft;
            boxVertices[2] = topRightFront;
            boxNormals[2] = frontNormal;
            boxUVs[2] = textureTopRight;
            boxVertices[3] = bottomLeftFront;
            boxNormals[3] = frontNormal;
            boxUVs[3] = textureBottomLeft;
            boxVertices[4] = bottomRightFront;
            boxNormals[4] = frontNormal;
            boxUVs[4] = textureBottomRight;
            boxVertices[5] = topRightFront;
            boxNormals[5] = frontNormal;
            boxUVs[5] = textureTopRight;

            // Back face.
            boxVertices[6] = topLeftBack;
            boxNormals[6] = backNormal;
            boxUVs[6] = textureTopRight;
            boxVertices[7] = topRightBack;
            boxNormals[7] = backNormal;
            boxUVs[7] = textureTopLeft;
            boxVertices[8] = bottomLeftBack;
            boxNormals[8] = backNormal;
            boxUVs[8] = textureBottomRight;
            boxVertices[9] = bottomLeftBack;
            boxNormals[9] = backNormal;
            boxUVs[9] = textureBottomRight;
            boxVertices[10] = topRightBack;
            boxNormals[10] = backNormal;
            boxUVs[10] = textureTopLeft;
            boxVertices[11] = bottomRightBack;
            boxNormals[11] = backNormal;
            boxUVs[11] = textureBottomLeft;

            // Top face.
            boxVertices[12] = topLeftFront;
            boxNormals[12] = topNormal;
            boxUVs[12] = textureBottomLeft;
            boxVertices[13] = topRightBack;
            boxNormals[13] = topNormal;
            boxUVs[13] = textureTopRight;
            boxVertices[14] = topLeftBack;
            boxNormals[14] = topNormal;
            boxUVs[14] = textureTopLeft;
            boxVertices[15] = topLeftFront;
            boxNormals[15] = topNormal;
            boxUVs[15] = textureBottomLeft;
            boxVertices[16] = topRightFront;
            boxNormals[16] = topNormal;
            boxUVs[16] = textureBottomRight;
            boxVertices[17] = topRightBack;
            boxNormals[17] = topNormal;
            boxUVs[17] = textureTopRight;

            // Bottom face. 
            boxVertices[18] = bottomLeftFront;
            boxNormals[18] = bottomNormal;
            boxUVs[18] = textureTopLeft;
            boxVertices[19] = bottomLeftBack;
            boxNormals[19] = bottomNormal;
            boxUVs[19] = textureBottomLeft;
            boxVertices[20] = bottomRightBack;
            boxNormals[20] = bottomNormal;
            boxUVs[20] = textureBottomRight;
            boxVertices[21] = bottomLeftFront;
            boxNormals[21] = bottomNormal;
            boxUVs[21] = textureTopLeft;
            boxVertices[22] = bottomRightBack;
            boxNormals[22] = bottomNormal;
            boxUVs[22] = textureBottomRight;
            boxVertices[23] = bottomRightFront;
            boxNormals[23] = bottomNormal;
            boxUVs[23] = textureTopRight;

            // Left face.
            boxVertices[24] = topLeftFront;
            boxNormals[24] = leftNormal;
            boxUVs[24] = textureTopRight;
            boxVertices[25] = bottomLeftBack;
            boxNormals[25] = leftNormal;
            boxUVs[25] = textureBottomLeft;
            boxVertices[26] = bottomLeftFront;
            boxNormals[26] = leftNormal;
            boxUVs[26] = textureBottomRight;
            boxVertices[27] = topLeftBack;
            boxNormals[27] = leftNormal;
            boxUVs[27] = textureTopLeft;
            boxVertices[28] = bottomLeftBack;
            boxNormals[28] = leftNormal;
            boxUVs[28] = textureBottomLeft;
            boxVertices[29] = topLeftFront;
            boxNormals[29] = leftNormal;
            boxUVs[29] = textureTopRight;

            // Right face. 
            boxVertices[30] = topRightFront;
            boxNormals[30] = rightNormal;
            boxUVs[30] = textureTopLeft;
            boxVertices[31] = bottomRightFront;
            boxNormals[31] = rightNormal;
            boxUVs[31] = textureBottomLeft;
            boxVertices[32] = bottomRightBack;
            boxNormals[32] = rightNormal;
            boxUVs[32] = textureBottomRight;
            boxVertices[33] = topRightBack;
            boxNormals[33] = rightNormal;
            boxUVs[33] = textureTopRight;
            boxVertices[34] = topRightFront;
            boxNormals[34] = rightNormal;
            boxUVs[34] = textureTopLeft;
            boxVertices[35] = bottomRightBack;
            boxNormals[35] = rightNormal;
            boxUVs[35] = textureBottomRight;

            mesh.vertices = boxVertices;
            mesh.normals = boxNormals;
            mesh.uv = boxUVs;
            //mesh.triangles = CreateIndexBuffer(vertexCount, indexCount, slices);
            mesh.triangles = new int[]
            {
                0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28,
                29, 30, 31, 32, 33, 34, 35
            };

            return mesh;
        }

        /// <summary>
        /// Creates a <see cref="Mesh"/> filled with vertices forming a plane.
        /// </summary>
        /// <param name="width">The width of the plane, along the X axis.</param>
        /// <param name="depth">The depth of the plane, along the Z axis.</param>
        /// <param name="widthDivisions">The number of divisions along the X axis.</param>
        /// <param name="depthDivisions">The number of divisions along the Z axis.</param>
        /// <returns>A <see cref="Mesh"/> filled with vertices forming a plane.</returns>
        public static Mesh CreatePlaneMesh(float width, float depth, int widthDivisions, int depthDivisions)
        {
            if (widthDivisions < 1 || depthDivisions < 1)
                return null;

            Mesh mesh = new Mesh();
            mesh.name = "PlaneMesh";

            float widthStep = width/widthDivisions;
            float depthStep = depth/depthDivisions;
            float halfWidth = width/2.0f;
            float halfDepth = depth/2.0f;

            int vertexCount = (widthDivisions + 1)*(depthDivisions + 1);

            Vector3[] planeVertices = new Vector3[vertexCount];
            Vector3[] planeNormals = new Vector3[vertexCount];
            Vector2[] planeUVs = new Vector2[vertexCount];

            int currentVertex = 0;

            for (float currentX = -halfWidth; currentX <= halfWidth; currentX += widthStep)
            {
                for (float currentZ = -halfDepth; currentZ <= halfDepth; currentZ += depthStep)
                {
                    planeVertices[currentVertex] = new Vector3(currentX, 0, currentZ);
                    planeNormals[currentVertex] = Vector3.up;
                    planeUVs[currentVertex] = new Vector2(1 - (currentX + halfWidth)/width,
                        1 - (currentZ + halfDepth)/depth);

                    currentVertex++;
                }
            }

            int[] indices = new int[widthDivisions*depthDivisions*2*3];
            int currentIndex = 0;

            int index = 0;
            for (int row = 0; row < depthDivisions; row++)
            {
                for (int column = 0; column < widthDivisions; column++)
                {
                    //index, index + 1, index + widthDivisions
                    //index + 1, index + widthDivisions + 1, index + widthDivisions;                    

                    indices[currentIndex++] = index;
                    indices[currentIndex++] = index + 1;
                    indices[currentIndex++] = index + widthDivisions + 1;

                    indices[currentIndex++] = index + 1;
                    indices[currentIndex++] = index + widthDivisions + 2;
                    indices[currentIndex++] = index + widthDivisions + 1;

                    index++;
                }

                index++;
            }

            mesh.vertices = planeVertices;
            mesh.normals = planeNormals;
            mesh.uv = planeUVs;
            mesh.triangles = indices;

            return mesh;
        }

        /// <summary>
        /// Creates a <see cref="Mesh"/> filled with vertices forming a sphere.
        /// </summary>
        /// <remarks>
        /// The values are as follows:
        /// Vertex Count   = slices * (stacks - 1) + 2
        /// Triangle Count = slices * (stacks - 1) * 2
        /// 
        /// Default sphere mesh in Unity has a radius of 0.5 with 20 slices and 20 stacks.
        /// </remarks>
        /// <param name="radius">Radius of the sphere. This value should be greater than or equal to 0.0f.</param>
        /// <param name="slices">Number of slices around the Y axis.</param>
        /// <param name="stacks">Number of stacks along the Y axis. Should be 2 or greater. (stack of 1 is just a cylinder)</param>
        public static Mesh CreateSphereMesh(float radius, int slices, int stacks)
        {
            Mesh mesh = new Mesh();
            mesh.name = "SphereMesh";

            float sliceStep = (float) Math.PI*2.0f/slices;
            float stackStep = (float) Math.PI/stacks;
            int vertexCount = slices*(stacks - 1) + 2;
            int triangleCount = slices*(stacks - 1)*2;
            int indexCount = triangleCount*3;

            Vector3[] sphereVertices = new Vector3[vertexCount];
            Vector3[] sphereNormals = new Vector3[vertexCount];
            Vector2[] sphereUVs = new Vector2[vertexCount];

            int currentVertex = 0;
            sphereVertices[currentVertex] = new Vector3(0, -radius, 0);
            sphereNormals[currentVertex] = Vector3.down;
            currentVertex++;
            float stackAngle = (float) Math.PI - stackStep;
            for (int i = 0; i < stacks - 1; i++)
            {
                float sliceAngle = 0;
                for (int j = 0; j < slices; j++)
                {
                    //NOTE: y and z were switched from normal spherical coordinates because the sphere is "oriented" along the Y axis as opposed to the Z axis
                    float x = (float) (radius*Math.Sin(stackAngle)*Math.Cos(sliceAngle));
                    float y = (float) (radius*Math.Cos(stackAngle));
                    float z = (float) (radius*Math.Sin(stackAngle)*Math.Sin(sliceAngle));

                    Vector3 position = new Vector3(x, y, z);
                    sphereVertices[currentVertex] = position;
                    sphereNormals[currentVertex] = Vector3.Normalize(position);
                    sphereUVs[currentVertex] =
                        new Vector2((float) (Math.Sin(sphereNormals[currentVertex].x)/Math.PI + 0.5f),
                            (float) (Math.Sin(sphereNormals[currentVertex].y)/Math.PI + 0.5f));

                    currentVertex++;

                    sliceAngle += sliceStep;
                }
                stackAngle -= stackStep;
            }
            sphereVertices[currentVertex] = new Vector3(0, radius, 0);
            sphereNormals[currentVertex] = Vector3.up;

            mesh.vertices = sphereVertices;
            mesh.normals = sphereNormals;
            mesh.uv = sphereUVs;
            mesh.triangles = CreateIndexBuffer(vertexCount, indexCount, slices);

            return mesh;
        }

        /// <summary>
        /// Creates a <see cref="Mesh"/> filled with vertices forming a cylinder.
        /// </summary>
        /// <remarks>
        /// The values are as follows:
        /// Vertex Count    = slices * (stacks + 1) + 2
        /// Primitive Count = slices * (stacks + 1) * 2
        /// </remarks>
        /// <param name="bottomRadius">Radius at the negative Y end. Value should be greater than or equal to 0.0f.</param>
        /// <param name="topRadius">Radius at the positive Y end. Value should be greater than or equal to 0.0f.</param>
        /// <param name="length">Length of the cylinder along the Y-axis.</param>
        /// <param name="slices">Number of slices about the Y axis.</param>
        /// <param name="stacks">Number of stacks along the Y axis.</param>
        public static Mesh CreateCylinderMesh(float bottomRadius, float topRadius, float length, int slices, int stacks)
        {
            // if both the top and bottom have a radius of zero, just return null, because invalid
            if (bottomRadius <= 0 && topRadius <= 0)
            {
                return null;
            }

            Mesh mesh = new Mesh();
            mesh.name = "CylinderMesh";
            float sliceStep = (float) Math.PI*2.0f/slices;
            float heightStep = length/stacks;
            float radiusStep = (topRadius - bottomRadius)/stacks;
            float currentHeight = -length/2;
            int vertexCount = (stacks + 1)*slices + 2; //cone = stacks * slices + 1
            int triangleCount = (stacks + 1)*slices*2; //cone = stacks * slices * 2 + slices
            int indexCount = triangleCount*3;
            float currentRadius = bottomRadius;

            Vector3[] cylinderVertices = new Vector3[vertexCount];
            Vector3[] cylinderNormals = new Vector3[vertexCount];
            Vector2[] cylinderUVs = new Vector2[vertexCount];

            // Start at the bottom of the cylinder            
            int currentVertex = 0;
            cylinderVertices[currentVertex] = new Vector3(0, currentHeight, 0);
            cylinderNormals[currentVertex] = Vector3.down;
            currentVertex++;
            for (int i = 0; i <= stacks; i++)
            {
                float sliceAngle = 0;
                for (int j = 0; j < slices; j++)
                {
                    float x = currentRadius*(float) Math.Cos(sliceAngle);
                    float y = currentHeight;
                    float z = currentRadius*(float) Math.Sin(sliceAngle);

                    Vector3 position = new Vector3(x, y, z);
                    cylinderVertices[currentVertex] = position;
                    cylinderNormals[currentVertex] = Vector3.Normalize(position);
                    cylinderUVs[currentVertex] =
                        new Vector2((float) (Math.Sin(cylinderNormals[currentVertex].x)/Math.PI + 0.5f),
                            (float) (Math.Sin(cylinderNormals[currentVertex].y)/Math.PI + 0.5f));

                    currentVertex++;

                    sliceAngle += sliceStep;
                }
                currentHeight += heightStep;
                currentRadius += radiusStep;
            }
            cylinderVertices[currentVertex] = new Vector3(0, length/2, 0);
            cylinderNormals[currentVertex] = Vector3.up;
            currentVertex++;

            mesh.vertices = cylinderVertices;
            mesh.normals = cylinderNormals;
            mesh.uv = cylinderUVs;
            mesh.triangles = CreateIndexBuffer(vertexCount, indexCount, slices);

            return mesh;
        }

        /// <summary>
        /// Creates an index buffer for spherical shapes like Spheres, Cylinders, and Cones.
        /// </summary>
        /// <param name="vertexCount">The total number of vertices making up the shape.</param>
        /// <param name="indexCount">The total number of indices making up the shape.</param>
        /// <param name="slices">The number of slices about the Y axis.</param>
        /// <returns>The index buffer containing the index data for the shape.</returns>
        private static int[] CreateIndexBuffer(int vertexCount, int indexCount, int slices)
        {
            int[] indices = new int[indexCount];
            int currentIndex = 0;

            // Bottom circle/cone of shape
            for (int i = 1; i <= slices; i++)
            {
                indices[currentIndex++] = i;
                indices[currentIndex++] = 0;
                if (i - 1 == 0)
                    indices[currentIndex++] = i + slices - 1;
                else
                    indices[currentIndex++] = i - 1;
            }

            // Middle sides of shape
            for (int i = 1; i < vertexCount - slices - 1; i++)
            {
                indices[currentIndex++] = i + slices;
                indices[currentIndex++] = i;
                if ((i - 1)%slices == 0)
                    indices[currentIndex++] = i + slices + slices - 1;
                else
                    indices[currentIndex++] = i + slices - 1;

                indices[currentIndex++] = i;
                if ((i - 1)%slices == 0)
                    indices[currentIndex++] = i + slices - 1;
                else
                    indices[currentIndex++] = i - 1;
                if ((i - 1)%slices == 0)
                    indices[currentIndex++] = i + slices + slices - 1;
                else
                    indices[currentIndex++] = i + slices - 1;
            }

            // Top circle/cone of shape
            for (int i = vertexCount - slices - 1; i < vertexCount - 1; i++)
            {
                indices[currentIndex++] = i;
                if ((i - 1)%slices == 0)
                    indices[currentIndex++] = i + slices - 1;
                else
                    indices[currentIndex++] = i - 1;
                indices[currentIndex++] = vertexCount - 1;
            }

            return indices;
        }

        #endregion

        #region GameObjects

        /// <summary>
        /// Creates a <see cref="GameObject"/> that has a box <see cref="Mesh"/>, a <see cref="MeshRenderer"/>, and 
        /// a <see cref="BoxCollider"/>.
        /// </summary>
        /// <param name="width">Width of the box, along the x-axis.</param>
        /// <param name="height">Height of the box, along the y-axis.</param>
        /// <param name="depth">Depth of the box, along the z-axis.</param>
        /// <returns>A new <see cref="GameObject"/> containing a cylinder <see cref="Mesh"/>, <see cref="MeshRenderer"/>, and <see cref="SphereCollider"/></returns>
        public static GameObject CreateBoxGameObject(float width, float height, float depth)
        {
            Mesh mesh = CreateBoxMesh(width, height, depth);

            GameObject gameObject = new GameObject("Box");
            MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
            meshFilter.mesh = mesh;

            BoxCollider collider = gameObject.AddComponent<BoxCollider>();
            collider.size = new Vector3(width, height, depth);
            gameObject.AddComponent<MeshRenderer>();

            Shader shader = Shader.Find("Diffuse");
            gameObject.GetComponent<Renderer>().material = new Material(shader);

            return gameObject;
        }

        /// <summary>
        /// Creates a <see cref="GameObject"/> that has a sphere <see cref="Mesh"/>, a <see cref="MeshRenderer"/>, and 
        /// a <see cref="SphereCollider"/> just like the built-in sphere object.  The difference is you can change the 
        /// quality of the mesh based on the given parameters.
        /// </summary>
        /// <param name="radius">The radius of the sphere mesh.  (Built-in is 0.5)</param>
        /// <param name="slices">The number of slices (divisions around the Y axis) making up the sphere mesh.  (Built-in is 20)</param>
        /// <param name="stacks">The number of stacks (division along the Y axis) making up the sphere mesh.  (Built-in is 20)</param>
        /// <returns>A new <see cref="GameObject"/> containing a sphere <see cref="Mesh"/>, <see cref="MeshRenderer"/>, and <see cref="SphereCollider"/></returns>
        public static GameObject CreateSphereGameObject(float radius, int slices, int stacks)
        {
            Mesh mesh = CreateSphereMesh(radius, slices, stacks);

            GameObject gameObject = new GameObject("Sphere");
            MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
            meshFilter.mesh = mesh;

            SphereCollider collider = gameObject.AddComponent<SphereCollider>();
            collider.radius = radius;
            gameObject.AddComponent<MeshRenderer>();

            Shader shader = Shader.Find("Diffuse");
            gameObject.GetComponent<Renderer>().material = new Material(shader);

            return gameObject;
        }

        /// <summary>
        /// Creates a <see cref="GameObject"/> that has a cylinder <see cref="Mesh"/>, a <see cref="MeshRenderer"/>, and 
        /// a <see cref="SphereCollider"/>.
        /// </summary>
        /// <param name="bottomRadius">Radius at the negative Y end. Value should be greater than or equal to 0.0f.</param>
        /// <param name="topRadius">Radius at the positive Y end. Value should be greater than or equal to 0.0f.</param>
        /// <param name="length">Length of the cylinder along the Y-axis.</param>
        /// <param name="slices">Number of slices about the Y axis.</param>
        /// <param name="stacks">Number of stacks along the Y axis.</param>
        /// <returns>A new <see cref="GameObject"/> containing a cylinder <see cref="Mesh"/>, <see cref="MeshRenderer"/>, and <see cref="SphereCollider"/></returns>
        public static GameObject CreateCylinderGameObject(float bottomRadius, float topRadius, float length, int slices,
            int stacks)
        {
            Mesh mesh = CreateCylinderMesh(bottomRadius, topRadius, length, slices, stacks);

            GameObject gameObject = new GameObject("Cylinder");
            MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
            meshFilter.mesh = mesh;

            MeshCollider collider = gameObject.AddComponent<MeshCollider>();
            collider.sharedMesh = mesh;
            collider.convex = true;
            gameObject.AddComponent<MeshRenderer>();

            Shader shader = Shader.Find("Diffuse");
            gameObject.GetComponent<Renderer>().material = new Material(shader);

            return gameObject;
        }

        /// <summary>
        /// Creates a <see cref="GameObject"/> that has a plane <see cref="Mesh"/>, a <see cref="MeshRenderer"/>, and 
        /// a <see cref="MeshCollider"/>.
        /// </summary>
        /// <param name="width">The width of the plane, along the X axis.</param>
        /// <param name="depth">The depth of the plane, along the Z axis.</param>
        /// <param name="widthDivisions">The number of divisions along the X axis.</param>
        /// <param name="depthDivisions">The number of divisions along the Z axis.</param>
        /// <returns>A new <see cref="GameObject"/> containing a plane <see cref="Mesh"/>, <see cref="MeshRenderer"/>, and <see cref="MeshCollider"/></returns>
        public static GameObject CreatePlaneGameObject(float width, float depth, int widthDivisions, int depthDivisions)
        {
            Mesh mesh = CreatePlaneMesh(width, depth, widthDivisions, depthDivisions);

            GameObject gameObject = new GameObject("Plane");
            MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
            meshFilter.mesh = mesh;

            MeshCollider collider = gameObject.AddComponent<MeshCollider>();
            collider.sharedMesh = mesh;
            collider.convex = false;
            gameObject.AddComponent<MeshRenderer>();

            Shader shader = Shader.Find("Diffuse");
            gameObject.GetComponent<Renderer>().material = new Material(shader);

            return gameObject;
        }

        #endregion
    }
}