Procedural Primitives
---------------------

Create spheres, boxes, planes, cylinders, and cones all in script at the size and quality you desire!  You don't need to 
learn to use a complicated 3D modelling application.  You're no longer confined by the sizes and mesh quality that Unity 
provides.  You no longer need messy model files cluttering your Assets folder.  

Do you want to create a primitive shape, but you want it to be a higher or lower resolution than the Unity mesh?  Do you 
want to be able to create cone primitives?  Do you want to change the quality of your meshes based upon what platform
you're running on? If so, then Procedural Primitives is what you're looking for!  You can call simple methods from script
that generate primitive shapes based upon various quality and sizing parameters.  Generate just the meshes, or generate 
an entire GameObject with all of the necessary components automatically.  Each mesh is automatially texture mapped and 
normal mapped for your convenience!

Now with a custom creation window to allow creation of shapes in the Editor!

Usage
-----

All of the methods are in the Primitive.cs file.   They are public static methods that can be called from anywhere.  
There are two types of methods: mesh creation methods and object creation methods.  The mesh creation methods just create
Mesh objects that can be used wherever a Mesh object is expected inside of Unity.  The object creation methods create
GameObject objects that contain a new Mesh of the shape as well as MeshFilter, MeshRenderer, and Collider components that 
correspond to the shape.

The parameters of the shapes are as follows:

Box:
width = The width of the box, along the X axis.
height = The height of the box, along the Y axis.
depth = The depth of the box, along the Z axis.

Plane:
width = The width of the plane, along the X axis.
depth = The depth of the plane, along the Z axis.
widthDivisions = The number of divisions along the X axis.
depthDivisions = The number of divisions along the Z axis.
    
Sphere:
radius = Radius of the sphere. This value should be greater than or equal to 0.0f.
slices = Number of slices around the Y axis.
stacks = Number of stacks along the Y axis. Should be 2 or greater. (stack of 1 is just a simple cylinder)
    
Cylinder:
bottomRadius = Radius at the negative Y end. Value should be greater than or equal to 0.0f.
topRadius = Radius at the positive Y end. Value should be greater than or equal to 0.0f.
length = Length of the cylinder along the Y-axis.
slices = Number of slices about the Y axis.
stacks = Number of stacks along the Y axis.
    
Note: In order to make a Cone, you use the Cylinder creation methods and set either the topRadius or bottomRadius to zero (0.0f).

Please refer to the comments in the code, as well as the example scene for more information.