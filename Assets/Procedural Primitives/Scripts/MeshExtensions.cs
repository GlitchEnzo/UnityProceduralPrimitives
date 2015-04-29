namespace ProceduralPrimitives
{
    using UnityEngine;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Holds extension methods for a Unity <see cref="Mesh"/>.
    /// </summary>
    public static class MeshExtensions
    {
        /// <summary>
        /// The formatted path to a <see cref="Mesh"/> asset.
        /// </summary>
        private const string MeshPath = "Assets/Meshes/{0}.asset";

        /// <summary>
        /// Serializes the <see cref="Mesh"/> out to the hard drive as an asset.  This only works in the Editor and will do nothing at runtime.
        /// </summary>
        /// <param name="mesh">The <see cref="Mesh"/> to create the asset from.</param>
        public static void CreateAsset(this Mesh mesh)
        {
#if UNITY_EDITOR
            // Detect if Meshes folder exists and create it if it does not
            if (!Directory.Exists(Application.dataPath + "/Meshes"))
            {
                UnityEditor.AssetDatabase.CreateFolder("Assets", "Meshes");
            }

            UnityEditor.AssetDatabase.CreateAsset(mesh, string.Format(MeshPath, mesh.name));
#endif
        }

        /// <summary>
        /// Fills this <see cref="Mesh"/> with vertices forming a 3D box.
        /// </summary>
        /// <param name="mesh">The <see cref="Mesh"/> to fill with vertices.</param>
        /// <param name="radius">Radius of the circle. Value should be greater than or equal to 0.0f.</param>
        /// <param name="segments">The number of segments making up the circle. Value should be greater than or equal to 3.</param>
        /// <param name="startAngle">The starting angle of the circle.  Usually 0.</param>
        /// <param name="angularSize">The angular size of the circle.  2 pi is a full circle. Pi is a half circle.</param>
        public static void CreateBox(this Mesh mesh, float width, float height, float depth, int widthSegments,
            int heightSegments, int depthSegments)
        {
            mesh.name = "Box";
            mesh.Clear();

            List<Vector2> uvs = new List<Vector2>();
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();

            var width_half = width/2.0f;
            var height_half = height/2.0f;
            var depth_half = depth/2.0f;

            BuildBoxSide(ref vertices, ref uvs, ref triangles, 2, 1, -1, -1, depth, height, width_half, widthSegments,
                heightSegments, depthSegments); // px
            BuildBoxSide(ref vertices, ref uvs, ref triangles, 2, 1, 1, -1, depth, height, -width_half, widthSegments,
                heightSegments, depthSegments); // nx
            BuildBoxSide(ref vertices, ref uvs, ref triangles, 0, 2, 1, 1, width, depth, height_half, widthSegments,
                heightSegments, depthSegments); // py
            BuildBoxSide(ref vertices, ref uvs, ref triangles, 0, 2, 1, -1, width, depth, -height_half, widthSegments,
                heightSegments, depthSegments); // ny
            BuildBoxSide(ref vertices, ref uvs, ref triangles, 0, 1, 1, -1, width, height, depth_half, widthSegments,
                heightSegments, depthSegments); // pz
            BuildBoxSide(ref vertices, ref uvs, ref triangles, 0, 1, -1, -1, width, height, -depth_half, widthSegments,
                heightSegments, depthSegments); // nz

            //this.computeCentroids();
            //this.mergeVertices();

            mesh.vertices = vertices.ToArray();
            //mesh.normals = normals;
            mesh.uv = uvs.ToArray();
            mesh.triangles = triangles.ToArray();

            mesh.RecalculateNormals();
            //mesh.RecalculateBounds();
        }

        /// <summary>
        /// Helper method used to build a side of a box.
        /// </summary>
        /// <param name="vertices">Vertices.</param>
        /// <param name="uvs">Uvs.</param>
        /// <param name="triangles">Triangles.</param>
        /// <param name="u">U.</param>
        /// <param name="v">V.</param>
        /// <param name="udir">Udir.</param>
        /// <param name="vdir">Vdir.</param>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        /// <param name="depth">Depth.</param>
        /// <param name="widthSegments">Width segments.</param>
        /// <param name="heightSegments">Height segments.</param>
        /// <param name="depthSegments">Depth segments.</param>
        private static void BuildBoxSide(ref List<Vector3> vertices, ref List<Vector2> uvs, ref List<int> triangles,
            int u, int v, int udir, int vdir, float width, float height, float depth, int widthSegments,
            int heightSegments, int depthSegments)
        {
            int w = 2;
            int ix;
            int iy;
            var gridX = widthSegments;
            var gridY = heightSegments;
            var width_half = width/2.0f;
            var height_half = height/2.0f;
            var offset = vertices.Count;

            if ((u == 0 && v == 1) || (u == 1 && v == 0))
            {
                w = 2;
            }
            else if ((u == 0 && v == 2) || (u == 2 && v == 0))
            {
                w = 1;
                gridY = depthSegments;
            }
            else if ((u == 2 && v == 1) || (u == 1 && v == 2))
            {
                w = 0;
                gridX = depthSegments;
            }

            var gridX1 = gridX + 1;
            var gridY1 = gridY + 1;
            var segment_width = width/gridX;
            var segment_height = height/gridY;
            var normal = new Vector3();

            normal[w] = depth > 0 ? 1 : - 1;

            for (iy = 0; iy < gridY1; iy ++)
            {
                for (ix = 0; ix < gridX1; ix ++)
                {
                    var vector = new Vector3();
                    vector[u] = (ix*segment_width - width_half)*udir;
                    vector[v] = (iy*segment_height - height_half)*vdir;
                    vector[w] = depth;

                    var uv = new Vector2(1.0f - (float) ix/gridX, 1.0f - (float) iy/gridY);

                    vertices.Add(vector);
                    uvs.Add(uv);
                }
            }

            for (iy = 0; iy < gridY; iy++)
            {
                for (ix = 0; ix < gridX; ix++)
                {
                    var a = ix + gridX1*iy;
                    var b = ix + gridX1*(iy + 1);
                    var c = (ix + 1) + gridX1*(iy + 1);
                    var d = (ix + 1) + gridX1*iy;

                    triangles.Add(a + offset);
                    triangles.Add(b + offset);
                    triangles.Add(d + offset);

                    triangles.Add(b + offset);
                    triangles.Add(c + offset);
                    triangles.Add(d + offset);
                }
            }
        }

        /// <summary>
        /// Fills this <see cref="Mesh"/> with vertices forming a 2D circle.
        /// </summary>
        /// <param name="mesh">The <see cref="Mesh"/> to fill with vertices.</param>
        /// <param name="topRadius">Top radius of the cylinder. Value should be greater than or equal to 0.0f.</param>
        /// <param name="segments">The number of segments making up the circle. Value should be greater than or equal to 3.</param>
        /// <param name="startAngle">The starting angle of the circle.  Usually 0.</param>
        /// <param name="angularSize">The angular size of the circle.  2 pi is a full circle. Pi is a half circle.</param>
        public static void CreateCylinder(this Mesh mesh, float topRadius, float bottomRadius, float height,
            int radialSegments, int heightSegments, bool openEnded)
        {
            mesh.name = "Cylinder";
            mesh.Clear();

            List<Vector2> uvs = new List<Vector2>();
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();

            var heightHalf = height/2;
            int y;

            List<List<int>> verticesLists = new List<List<int>>();
            List<List<Vector2>> uvsLists = new List<List<Vector2>>();

            for (y = 0; y <= heightSegments; y++)
            {
                List<int> verticesRow = new List<int>();
                List<Vector2> uvsRow = new List<Vector2>();

                var v = y/(float) heightSegments;
                var radius = v*(bottomRadius - topRadius) + topRadius;

                for (int x = 0; x <= radialSegments; x++)
                {
                    float u = (float) x/(float) radialSegments;

                    var vertex = new Vector3();
                    vertex.x = radius*Mathf.Sin(u*Mathf.PI*2.0f);
                    vertex.y = - v*height + heightHalf;
                    vertex.z = radius*Mathf.Cos(u*Mathf.PI*2.0f);

                    vertices.Add(vertex);
                    uvs.Add(new Vector2(1 - u, 1 - v));

                    verticesRow.Add(vertices.Count - 1);
                    uvsRow.Add(new Vector2(u, 1 - v));
                }

                verticesLists.Add(verticesRow);
                uvsLists.Add(uvsRow);
            }

            var tanTheta = (bottomRadius - topRadius)/height;
            Vector3 na;
            Vector3 nb;



            for (int x = 0; x < radialSegments; x++)
            {
                if (topRadius != 0)
                {
                    na = vertices[verticesLists[0][x]];
                    nb = vertices[verticesLists[0][x + 1]];
                }
                else
                {
                    na = vertices[verticesLists[1][x]];
                    nb = vertices[verticesLists[1][x + 1]];
                }

                // normalize?
                na.y = (Mathf.Sqrt(na.x*na.x + na.z*na.z)*tanTheta);
                nb.y = (Mathf.Sqrt(nb.x*nb.x + nb.z*nb.z)*tanTheta);

                for (y = 0; y < heightSegments; y++)
                {
                    var v1 = verticesLists[y][x];
                    var v2 = verticesLists[y + 1][x];
                    var v3 = verticesLists[y + 1][x + 1];
                    var v4 = verticesLists[y][x + 1];

                    //var n1 = na;
                    //var n2 = na;
                    //var n3 = nb;
                    //var n4 = nb;

                    triangles.Add(v1);
                    triangles.Add(v2);
                    triangles.Add(v4);

                    triangles.Add(v2);
                    triangles.Add(v3);
                    triangles.Add(v4);
                }

            }

            // top cap
            if (!openEnded && topRadius > 0)
            {
                vertices.Add(new Vector3(0, heightHalf, 0));
                //uvs.Add(new Vector2(uvsLists[0][0 + 1].x, 0));
                uvs.Add(new Vector2(0.5f, 0));

                for (int x = 0; x < radialSegments; x ++)
                {
                    var v1 = verticesLists[0][x];
                    var v2 = verticesLists[0][x + 1];
                    var v3 = vertices.Count - 1;

                    //var n1 = new Vector3( 0, 1, 0 );
                    //var n2 = new Vector3( 0, 1, 0 );
                    //var n3 = new Vector3( 0, 1, 0 );

                    //var uv1 = uvsLists[ 0 ][ x ];
                    //var uv2 = uvsLists[ 0 ][ x + 1 ];
                    //var uv3 = new Vector2( uv2.x, 0 );

                    triangles.Add(v1);
                    triangles.Add(v2);
                    triangles.Add(v3);
                }
            }

            // bottom cap
            if (!openEnded && bottomRadius > 0)
            {
                vertices.Add(new Vector3(0, - heightHalf, 0));
                uvs.Add(new Vector2(0.5f, 1));

                for (int x = 0; x < radialSegments; x++)
                {
                    var v1 = verticesLists[y][x + 1];
                    var v2 = verticesLists[y][x];
                    var v3 = vertices.Count - 1;

                    //var n1 = new Vector3( 0, - 1, 0 );
                    //var n2 = new Vector3( 0, - 1, 0 );
                    //var n3 = new Vector3( 0, - 1, 0 );

                    //var uv1 = uvsLists[ y ][ x + 1 ];
                    //var uv2 = uvsLists[ y ][ x ];
                    //var uv3 = new Vector2( uv2.x, 1 );

                    triangles.Add(v1);
                    triangles.Add(v2);
                    triangles.Add(v3);
                }
            }

            mesh.vertices = vertices.ToArray();
            //mesh.normals = normals;
            mesh.uv = uvs.ToArray();
            mesh.triangles = triangles.ToArray();

            mesh.RecalculateNormals();
            //mesh.RecalculateBounds();
        }

        /// <summary>
        /// Fills this <see cref="Mesh"/> with vertices forming a 2D circle.
        /// </summary>
        /// <param name="mesh">The <see cref="Mesh"/> to fill with vertices.</param>
        /// <param name="radius">Radius of the circle. Value should be greater than or equal to 0.0f.</param>
        /// <param name="segments">The number of segments making up the circle. Value should be greater than or equal to 3.</param>
        /// <param name="startAngle">The starting angle of the circle.  Usually 0.</param>
        /// <param name="angularSize">The angular size of the circle.  2 pi is a full circle. Pi is a half circle.</param>
        public static void CreateCircle(this Mesh mesh, float radius, int segments, float startAngle, float angularSize)
        {
            mesh.name = "Circle";
            mesh.Clear();

            List<Vector2> uvs = new List<Vector2>();
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();

            vertices.Add(Vector3.zero);
            uvs.Add(new Vector2(0.5f, 0.5f));

            float stepAngle = angularSize/segments;

            for (int i = 0; i <= segments; i++)
            {
                var vertex = new Vector3();
                float angle = startAngle + stepAngle*i;

                //Debug.Log(string.Format("{0}: {1}", i, angle));
                vertex.x = radius*Mathf.Cos(angle);
                vertex.y = radius*Mathf.Sin(angle);

                vertices.Add(vertex);
                uvs.Add(new Vector2((vertex.x/radius + 1)/2, (vertex.y/radius + 1)/2));
            }

            //var n = new Vector3(0, 0, 1);

            for (int i = 1; i <= segments; i++)
            {
                triangles.Add(i + 1);
                triangles.Add(i);
                triangles.Add(0);
            }

            mesh.vertices = vertices.ToArray();
            //mesh.normals = normals;
            mesh.uv = uvs.ToArray();
            mesh.triangles = triangles.ToArray();

            mesh.RecalculateNormals();
            //mesh.RecalculateBounds();
        }

        public static void CreateLathe(this Mesh mesh, List<Vector3> points, int segments, float phiStart,
            float phiLength)
        {
            mesh.name = "Lathe";
            mesh.Clear();

            List<Vector2> uvs = new List<Vector2>();
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();

            ////var inversePointLength = 1.0f / ( points.Count - 1 );
            var inverseSegments = 1.0f/segments;

            for (var i = 0; i <= segments; i++)
            {
                var phi = phiStart + i*inverseSegments*phiLength;

                var c = Mathf.Cos(phi);
                var s = Mathf.Sin(phi);

                for (var j = 0; j < points.Count; j++)
                {
                    var pt = points[j];

                    var vertex = new Vector3();

                    vertex.x = c*pt.x - s*pt.y;
                    vertex.y = s*pt.x + c*pt.y;
                    vertex.z = pt.z;

                    vertices.Add(vertex);
                    uvs.Add(new Vector2(i*inverseSegments, j*inverseSegments));
                }
            }

            var np = points.Count;

            for (var i = 0; i < segments; i++)
            {
                for (var j = 0; j < points.Count - 1; j++)
                {
                    var baseP = j + np*i;
                    var a = baseP;
                    var b = baseP + np;
                    var c = baseP + 1 + np;
                    var d = baseP + 1;

//				var u0 = i * inverseSegments;
//				var v0 = j * inversePointLength;
//				var u1 = u0 + inverseSegments;
//				var v1 = v0 + inversePointLength;

                    triangles.Add(a);
                    triangles.Add(b);
                    triangles.Add(d);

                    triangles.Add(b);
                    triangles.Add(c);
                    triangles.Add(d);

//				this.faces.push( new THREE.Face3( a, b, d ) );
//				
//				this.faceVertexUvs[ 0 ].push( [
//				                               
//				                               new THREE.Vector2( u0, v0 ),
//				                               new THREE.Vector2( u1, v0 ),
//				                               new THREE.Vector2( u0, v1 )
//				                               
//				                               ] );
//				
//				this.faces.push( new THREE.Face3( b, c, d ) );
//				
//				this.faceVertexUvs[ 0 ].push( [
//				                               
//				                               new THREE.Vector2( u1, v0 ),
//				                               new THREE.Vector2( u1, v1 ),
//				                               new THREE.Vector2( u0, v1 )
//				                               
//				                               ] );


                }

            }

            mesh.vertices = vertices.ToArray();
            //mesh.normals = normals;
            mesh.uv = uvs.ToArray();
            mesh.triangles = triangles.ToArray();

            mesh.RecalculateNormals();
            //mesh.RecalculateBounds();
        }

        /// <summary>
        /// Fills this <see cref="Mesh"/> with vertices forming a 2D circle.
        /// </summary>
        /// <param name="mesh">The <see cref="Mesh"/> to fill with vertices.</param>
        /// <param name="radius">Radius of the circle. Value should be greater than or equal to 0.0f.</param>
        /// <param name="segments">The number of segments making up the circle. Value should be greater than or equal to 3.</param>
        /// <param name="startAngle">The starting angle of the circle.  Usually 0.</param>
        /// <param name="angularSize">The angular size of the circle.  2 pi is a full circle. Pi is a half circle.</param>
        public static void CreateSphere(this Mesh mesh, float radius, int widthSegments, int heightSegments,
            float phiStart, float phiLength, float thetaStart, float thetaLength)
        {
            mesh.name = "Sphere";
            mesh.Clear();

            List<Vector2> uvs = new List<Vector2>();
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();

            int x, y;

            List<List<int>> verticesLists = new List<List<int>>();
            List<List<Vector2>> uvsLists = new List<List<Vector2>>();

            for (y = 0; y <= heightSegments; y ++)
            {
                List<int> verticesRow = new List<int>();
                List<Vector2> uvsRow = new List<Vector2>();

                for (x = 0; x <= widthSegments; x ++)
                {
                    var u = x/(float) widthSegments;
                    var v = y/(float) heightSegments;

                    var vertex = new Vector3();
                    vertex.x = - radius*Mathf.Cos(phiStart + u*phiLength)*Mathf.Sin(thetaStart + v*thetaLength);
                    vertex.y = radius*Mathf.Cos(thetaStart + v*thetaLength);
                    vertex.z = radius*Mathf.Sin(phiStart + u*phiLength)*Mathf.Sin(thetaStart + v*thetaLength);

                    vertices.Add(vertex);
                    uvs.Add(new Vector2(u, 1 - v));

                    verticesRow.Add(vertices.Count - 1);
                    uvsRow.Add(new Vector2(u, 1 - v));
                }

                verticesLists.Add(verticesRow);
                uvsLists.Add(uvsRow);
            }

            for (y = 0; y < heightSegments; y ++)
            {
                for (x = 0; x < widthSegments; x ++)
                {
                    var v1 = verticesLists[y][x + 1];
                    var v2 = verticesLists[y][x];
                    var v3 = verticesLists[y + 1][x];
                    var v4 = verticesLists[y + 1][x + 1];

                    // normalize
                    //var n1 = vertices[ v1 ];
                    //var n2 = vertices[ v2 ];
                    //var n3 = vertices[ v3 ];
                    //var n4 = vertices[ v4 ];

                    var uv1 = uvsLists[y][x + 1];
                    var uv2 = uvsLists[y][x];
                    var uv3 = uvsLists[y + 1][x];
                    var uv4 = uvsLists[y + 1][x + 1];

                    if (Mathf.Abs(vertices[v1].y) == radius)
                    {
                        uv1.x = (uv1.x + uv2.x)/2;

                        triangles.Add(v1);
                        triangles.Add(v3);
                        triangles.Add(v4);

                        //this.faces.push( new THREE.Face3( v1, v3, v4, [ n1, n3, n4 ] ) );
                        //this.faceVertexUvs[ 0 ].push( [ uv1, uv3, uv4 ] );
                    }
                    else if (Mathf.Abs(vertices[v3].y) == radius)
                    {
                        uv3.x = (uv3.x + uv4.x)/2;

                        triangles.Add(v1);
                        triangles.Add(v2);
                        triangles.Add(v3);

                        //this.faces.push( new THREE.Face3( v1, v2, v3, [ n1, n2, n3 ] ) );
                        //this.faceVertexUvs[ 0 ].push( [ uv1, uv2, uv3 ] );
                    }
                    else
                    {
                        triangles.Add(v1);
                        triangles.Add(v2);
                        triangles.Add(v4);

                        triangles.Add(v2);
                        triangles.Add(v3);
                        triangles.Add(v4);

                        //this.faces.push( new THREE.Face3( v1, v2, v4, [ n1, n2, n4 ] ) );
                        //this.faceVertexUvs[ 0 ].push( [ uv1, uv2, uv4 ] );

                        //this.faces.push( new THREE.Face3( v2, v3, v4, [ n2.clone(), n3, n4.clone() ] ) );
                        //this.faceVertexUvs[ 0 ].push( [ uv2.clone(), uv3, uv4.clone() ] );
                    }
                }
            }

            mesh.vertices = vertices.ToArray();
            //mesh.normals = normals;
            mesh.uv = uvs.ToArray();
            mesh.triangles = triangles.ToArray();

            mesh.RecalculateNormals();
            //mesh.RecalculateBounds();
        }

        /// <summary>
        /// Fills this <see cref="Mesh"/> with vertices forming a 2D circle.
        /// </summary>
        /// <param name="mesh">The <see cref="Mesh"/> to fill with vertices.</param>
        /// <param name="radius">Radius of the circle. Value should be greater than or equal to 0.0f.</param>
        /// <param name="segments">The number of segments making up the circle. Value should be greater than or equal to 3.</param>
        /// <param name="startAngle">The starting angle of the circle.  Usually 0.</param>
        /// <param name="angularSize">The angular size of the circle.  2 pi is a full circle. Pi is a half circle.</param>
        public static void CreateTorus(this Mesh mesh, float radius, float tube, int radialSegments, int tubularSegments,
            float arc)
        {
            mesh.name = "Torus";
            mesh.Clear();

            List<Vector2> uvs = new List<Vector2>();
            List<Vector3> vertices = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<int> triangles = new List<int>();

            var center = new Vector3();

            for (var j = 0; j <= radialSegments; j++)
            {
                for (var i = 0; i <= tubularSegments; i++)
                {
                    var u = i/(float) tubularSegments*arc;
                    var v = j/(float) radialSegments*Mathf.PI*2.0f;

                    center.x = radius*Mathf.Cos(u);
                    center.y = radius*Mathf.Sin(u);

                    var vertex = new Vector3();
                    vertex.x = (radius + tube*Mathf.Cos(v))*Mathf.Cos(u);
                    vertex.y = (radius + tube*Mathf.Cos(v))*Mathf.Sin(u);
                    vertex.z = tube*Mathf.Sin(v);

                    vertices.Add(vertex);

                    uvs.Add(new Vector2(i/(float) tubularSegments, j/(float) radialSegments));
                    Vector3 normal = vertex - center;
                    normal.Normalize();
                    normals.Add(normal);
                }
            }


            for (var j = 1; j <= radialSegments; j++)
            {
                for (var i = 1; i <= tubularSegments; i++)
                {
                    var a = (tubularSegments + 1)*j + i - 1;
                    var b = (tubularSegments + 1)*(j - 1) + i - 1;
                    var c = (tubularSegments + 1)*(j - 1) + i;
                    var d = (tubularSegments + 1)*j + i;

                    triangles.Add(a);
                    triangles.Add(b);
                    triangles.Add(d);

                    triangles.Add(b);
                    triangles.Add(c);
                    triangles.Add(d);
                }

            }

            mesh.vertices = vertices.ToArray();
            mesh.normals = normals.ToArray();
            mesh.uv = uvs.ToArray();
            mesh.triangles = triangles.ToArray();
        }

        public static void CreateTorusKnot(this Mesh mesh, float radius, float tube, int radialSegments,
            int tubularSegments)
        {
            mesh.CreateTorusKnot(radius, tube, radialSegments, tubularSegments, 2, 3, 1);
        }

        /// <summary>
        /// Fills this <see cref="Mesh"/> with vertices forming a 2D circle.
        /// </summary>
        /// <param name="mesh">The <see cref="Mesh"/> to fill with vertices.</param>
        /// <param name="radius">Radius of the circle. Value should be greater than or equal to 0.0f.</param>
        /// <param name="segments">The number of segments making up the circle. Value should be greater than or equal to 3.</param>
        /// <param name="startAngle">The starting angle of the circle.  Usually 0.</param>
        /// <param name="angularSize">The angular size of the circle.  2 pi is a full circle. Pi is a half circle.</param>
        public static void CreateTorusKnot(this Mesh mesh, float radius, float tube, int radialSegments,
            int tubularSegments, int p, int q, float heightScale)
        {
            mesh.name = "TorusKnot";
            mesh.Clear();

            List<Vector2> uvs = new List<Vector2>();
            List<Vector3> vertices = new List<Vector3>();
            ////List<Vector3> normals = new List<Vector3>();
            List<int> triangles = new List<int>();

            int[][] grid = new int[radialSegments][];

            var tang = new Vector3();
            var n = new Vector3();
            var bitan = new Vector3();

            for (var i = 0; i < radialSegments; ++i)
            {
                grid[i] = new int[tubularSegments];
                var u = i/(float) radialSegments*2.0f*p*Mathf.PI;
                var p1 = GetPos(u, q, p, radius, heightScale);
                var p2 = GetPos(u + 0.01f, q, p, radius, heightScale);
                //tang.subVectors( p2, p1 );
                tang = p2 - p1;
                //n.addVectors( p2, p1 );
                n = p2 + p1;

                //bitan.crossVectors( tang, n );
                bitan = Vector3.Cross(tang, n);
                //n.crossVectors( bitan, tang );
                n = Vector3.Cross(bitan, tang);
                bitan.Normalize();
                n.Normalize();

                for (var j = 0; j < tubularSegments; ++j)
                {
                    var v = j/(float) tubularSegments*2.0f*Mathf.PI;
                    var cx = -tube*Mathf.Cos(v); // TODO: Hack: Negating it so it faces outside.
                    var cy = tube*Mathf.Sin(v);

                    var pos = new Vector3();
                    pos.x = p1.x + cx*n.x + cy*bitan.x;
                    pos.y = p1.y + cx*n.y + cy*bitan.y;
                    pos.z = p1.z + cx*n.z + cy*bitan.z;

                    vertices.Add(pos);
                    uvs.Add(new Vector2(i/(float) radialSegments, j/(float) tubularSegments));

                    grid[i][j] = vertices.Count - 1;
                }

            }

            for (var i = 0; i < radialSegments; ++i)
            {
                for (var j = 0; j < tubularSegments; ++j)
                {
                    var ip = (i + 1)%radialSegments;
                    var jp = (j + 1)%tubularSegments;

                    var a = grid[i][j];
                    var b = grid[ip][j];
                    var c = grid[ip][jp];
                    var d = grid[i][jp];

                    ////var uva = new Vector2( i / (float)radialSegments, j / (float)tubularSegments );
                    ////var uvb = new Vector2( ( i + 1 ) / (float)radialSegments, j / (float)tubularSegments );
                    ////var uvc = new Vector2( ( i + 1 ) / (float)radialSegments, ( j + 1 ) / (float)tubularSegments );
                    ////var uvd = new Vector2( i / (float)radialSegments, ( j + 1 ) / (float)tubularSegments );

                    triangles.Add(a);
                    triangles.Add(b);
                    triangles.Add(d);

                    triangles.Add(b);
                    triangles.Add(c);
                    triangles.Add(d);

                    ////this.faces.push( new THREE.Face3( a, b, d ) );
                    ////this.faceVertexUvs[ 0 ].push( [ uva, uvb, uvd ] );

                    ////this.faces.push( new THREE.Face3( b, c, d ) );
                    ////this.faceVertexUvs[ 0 ].push( [ uvb.clone(), uvc, uvd.clone() ] );
                }
            }

            mesh.vertices = vertices.ToArray();
            //mesh.normals = normals.ToArray();
            mesh.uv = uvs.ToArray();
            mesh.triangles = triangles.ToArray();
        }

        private static Vector3 GetPos(float u, float in_q, float in_p, float radius, float heightScale)
        {
            var cu = Mathf.Cos(u);
            var su = Mathf.Sin(u);
            var quOverP = in_q/in_p*u;
            var cs = Mathf.Cos(quOverP);

            var tx = radius*(2.0f + cs)*0.5f*cu;
            var ty = radius*(2.0f + cs)*su*0.5f;
            var tz = heightScale*radius*Mathf.Sin(quOverP)*0.5f;

            return new Vector3(tx, ty, tz);
        }

        /// <summary>
        /// Fills this <see cref="Mesh"/> with vertices forming a 2D plane.
        /// </summary>
        /// <param name="mesh">The <see cref="Mesh"/> to fill with vertices.</param>
        /// <param name="width">Width of the plane. Value should be greater than or equal to 0.0f.</param>
        /// <param name="height">Height of the plane. Value should be greater than or equal to 0.0f.</param>
        /// <param name="widthSegments">The number of subdivisions along the width direction.</param>
        /// <param name="heightSegments">The number of subdivisions along the height direction.</param>
        public static void CreatePlane(this Mesh mesh, float width, float height, int widthSegments, int heightSegments)
        {
            mesh.name = "Plane";
            mesh.Clear();

            List<Vector2> uvs = new List<Vector2>();
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();

            var width_half = width/2.0f;
            var height_half = height/2.0f;

            var gridX = widthSegments;
            var gridZ = heightSegments;

            var gridX1 = gridX + 1;
            var gridZ1 = gridZ + 1;

            var segment_width = width/gridX;
            var segment_height = height/gridZ;

            //var normal = new Vector3(0, 0, 1);

            for (int iz = 0; iz < gridZ1; iz ++)
            {
                for (int ix = 0; ix < gridX1; ix ++)
                {
                    var x = ix*segment_width - width_half;
                    var y = iz*segment_height - height_half;

                    var uv = new Vector2((float) ix/gridX, 1.0f - (float) iz/gridZ);

                    vertices.Add(new Vector3(x, -y, 0));
                    uvs.Add(uv);
                }
            }

            for (int iz = 0; iz < gridZ; iz++)
            {
                for (int ix = 0; ix < gridX; ix++)
                {
                    var a = ix + gridX1*iz;
                    var b = ix + gridX1*(iz + 1);
                    var c = (ix + 1) + gridX1*(iz + 1);
                    var d = (ix + 1) + gridX1*iz;

                    //var uva = new Vector2( ix / gridX, 1 - iz / gridZ );
                    //var uvb = new Vector2( ix / gridX, 1 - ( iz + 1 ) / gridZ );
                    //var uvc = new Vector2( ( ix + 1 ) / gridX, 1 - ( iz + 1 ) / gridZ );
                    //var uvd = new Vector2( ( ix + 1 ) / gridX, 1 - iz / gridZ );

                    triangles.Add(a);
                    triangles.Add(d);
                    triangles.Add(b);

                    triangles.Add(b);
                    triangles.Add(d);
                    triangles.Add(c);
                }
            }

            mesh.vertices = vertices.ToArray();
            //mesh.normals = normals;
            mesh.uv = uvs.ToArray();
            mesh.triangles = triangles.ToArray();

            mesh.RecalculateNormals();
            //mesh.RecalculateBounds();
        }

        public static void CreateRing(this Mesh mesh, float innerRadius, float outerRadius, int thetaSegments,
            int phiSegments, float thetaStart, float thetaLength)
        {
            mesh.name = "Ring";
            mesh.Clear();

            List<Vector2> uvs = new List<Vector2>();
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();

            float radius = innerRadius;
            float radiusStep = ((outerRadius - innerRadius)/phiSegments);

            for (int i = 0; i <= phiSegments; i++) // concentric circles inside ring
            {
                for (int o = 0; o <= thetaSegments; o++) // number of segments per circle
                {
                    var vertex = new Vector3();
                    var segment = thetaStart + o/(float) thetaSegments*thetaLength;

                    vertex.x = radius*Mathf.Cos(segment);
                    vertex.y = radius*Mathf.Sin(segment);

                    vertices.Add(vertex);
                    uvs.Add(new Vector2((vertex.x/outerRadius + 1)/2.0f, (vertex.y/outerRadius + 1)/2.0f));
                }

                radius += radiusStep;
            }

            ////var n = new Vector3( 0, 0, 1 );

            for (int i = 0; i < phiSegments; i++) // concentric circles inside ring
            {
                var thetaSegment = i*thetaSegments;

                for (int o = 0; o <= thetaSegments; o++) // number of segments per circle
                {
                    var segment = o + thetaSegment;

                    var v1 = segment + i;
                    var v2 = segment + thetaSegments + i;
                    var v3 = segment + thetaSegments + 1 + i;

                    // prevent from connecting the start and end when not drawing a full ring
                    if (o > 0)
                    {
                        triangles.Add(v1);
                        triangles.Add(v3);
                        triangles.Add(v2);
                    }

                    // prevent from connecting the start and end when not drawing a full ring
                    if (o < thetaSegments)
                    {
                        v1 = segment + i;
                        v2 = segment + thetaSegments + 1 + i;
                        v3 = segment + 1 + i;

                        triangles.Add(v1);
                        triangles.Add(v3);
                        triangles.Add(v2);
                    }
                }
            }


            mesh.vertices = vertices.ToArray();
            //mesh.normals = normals;
            mesh.uv = uvs.ToArray();
            mesh.triangles = triangles.ToArray();

            mesh.RecalculateNormals();
            //mesh.RecalculateBounds();
        }




        /// <summary>
        /// Adds Barycentric coordinates to each vertex in the uv2 field of the <see cref="Mesh"/>.
        /// </summary>
        /// <param name="mesh">The <see cref="Mesh"/> to add the barycentric coordinates to.</param>
        public static void AddBarycentricCoordinates(this Mesh mesh)
        {
            int[] triangles = mesh.triangles;

            //Vector2[] bary = new Vector2[mesh.vertices.Length];
            Vector4[] bary = new Vector4[mesh.vertices.Length];

            // force them all to unused value
            for (int i = 0; i < bary.Length; i++)
            {
                bary[i] = Vector4.one;
            }

            Vector4 z = new Vector4(0, 0, 1, 0);
            Vector4 y = new Vector4(0, 1, 0, 0);
            Vector4 x = new Vector4(1, 0, 0, 0);

            bool zUsed = false;
            bool yUsed = false;
            bool xUsed = false;

            bool vert1Used = false;
            bool vert2Used = false;
            bool vert3Used = false;

            for (int i = 0; i < triangles.Length; i += 3)
            {
                Vector4 vert1 = bary[triangles[i]];
                Vector4 vert2 = bary[triangles[i + 1]];
                Vector4 vert3 = bary[triangles[i + 2]];

                zUsed = vert1 == z || vert2 == z || vert3 == z;
                yUsed = vert1 == y || vert2 == y || vert3 == y;
                xUsed = vert1 == x || vert2 == x || vert3 == x;

                vert1Used = vert1 != z && vert1 != y && vert1 != x;
                vert2Used = vert2 != z && vert2 != y && vert2 != x;
                vert3Used = vert3 != z && vert3 != y && vert3 != x;

                if (!zUsed)
                {
                    if (vert1Used)
                    {
                        vert1 = z;
                        vert1Used = false;
                    }
                    else if (vert2Used)
                    {
                        vert2 = z;
                        vert2Used = false;
                    }
                    else if (vert3Used)
                    {
                        vert3 = z;
                        vert3Used = false;
                    }
                }

                if (!yUsed)
                {
                    if (vert1Used)
                    {
                        vert1 = y;
                        vert1Used = false;
                    }
                    else if (vert2Used)
                    {
                        vert2 = y;
                        vert2Used = false;
                    }
                    else if (vert3Used)
                    {
                        vert3 = y;
                        vert3Used = false;
                    }
                }

                if (!xUsed)
                {
                    if (vert1Used)
                    {
                        vert1 = x;
                    }
                    else if (vert2Used)
                    {
                        vert2 = x;
                    }
                    else if (vert3Used)
                    {
                        vert3 = x;
                    }
                }

                if (vert1 == vert2 || vert1 == vert3 || vert2 == vert3)
                {
                    Debug.Log("Error!");
                }

                bary[triangles[i]] = vert1;
                bary[triangles[i + 1]] = vert2;
                bary[triangles[i + 2]] = vert3;

                zUsed = false;
                yUsed = false;
                xUsed = false;
            }

            //mesh.uv2 = bary;
            mesh.tangents = bary;
        }
    }
}