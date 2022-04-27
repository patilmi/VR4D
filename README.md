# VR4D

# Purpose
To expand mind to include spatial model/intuition/navigation of four dimensional space.

# Strategy
Rotates a rigid body (4D cube, 4D sphere, 4D plane etc) in four dimensional space while projecting onto a 3Dimensional screen (VR headset).

Rigid body shapes are created as point clouds and points are represented by tiny spheres.

# Brief Theory

Attempts to extend the algorithm our brain uses to construct our model/intuition of 3Dimensional space from 2D projections onto our retina (which is itself extends point light sensors on our retina into a 2D projection) by feeding it 3D projections of 4Dimensional rotations.


# Why it is unique

There are other attempts to explore 4D with VR, these mostly operate on the principle of rendering slices of the 4D world.

This project uses a cognitive approach that builds up our intuition organically through rotations, while slicing loses the continuity that our brain needs to
stitch 3D projections into a 4D whole. 

# Implications

Opens up the world of intuitive 4D problem solving.

Huge cognitive space implications: 

  What is happening in the brain when we start to grasp 4D space?
  
  Brain changes?
  
  Make us smarter?
  
  See patterns previously invisible to us?
  
  4D metaverse?
  
 # Direction
 
 A web application that uses brain training app like structure, which allows users to freely manipulate 4D shapes and rotations, and provides self testing
 functionality to test 4D navigational skills. 
 
 # How to Run (This needs testing)
Cleanest way to run this code currently is to start a new Unity VR/OpenXR project with your VR setup's recommended settings
add the [src](Assets/src) folder to your Assets folder, attach [4dim.cs](Assets/src) to an empty game object, and then run. Remove default backgrounds objects in the unity project.

Short sample of project rendering as a [3D video on youtube](https://www.youtube.com/watch?v=IGM0zfpOdsg&ab_channel=MihirPatil) 
The recording quality and effect is greatly diminished compared to original rendering. More videos and a website to come.

# Code Guide

All source code is in [Assets/src](Assets/src). Main file with Start() and Update() functions is [FourDim.cs](Assets/src). 

Start() mainly calls utility functions to build up 4D rigidbody structures and rotation objects and Update() projects onto 3D, renders onto 3D screen, and appplies rotations, fog, and scaling effects.

[FourDim.cs](Assets/src) also has ExponentialFog() function, and rotation matrix construction.


[Assets/src/utils](Assets/src/utils) contains BuildFourD.cs and FourDMath.cs

[BuildFourD.cs](Assets/src/utils) contains utilities for loading configuration objects and building 4D structures, like 4D planes, cubes, spheres, etc. It also includes build effects like clustering and chaining to generate a variety of effects for embedding 4D points(tiny spheres) in 4D space.

FourDMath contains utilities for math calculations.


[src/config](Assets/src/config)  Contains various objects and wrappers to build and read in 4D structures and 4D rotations from json.



### Rotation Math

R = A tiny rotation, I = Identity matrix, m = Infinitesmal matrix, v = any vector

R = I + m

((I + m)v)<sup>2</sup> = v<sup>2</sup>   (Because it's a rigid body transformation)

v<sup>T</sup>(I + m<sup>T</sup>)((I + m)v) = v<sup>T</sup>v  (Rewrite in matrix form)

v<sup>T</sup>(I + m<sup>T</sup> + m)v = v<sup>T</sup>v    (Drop m<sup>2</sup> terms)

v<sup>T</sup>(m<sup>T</sup> + m)v = 0

m<sup>T</sup> + m = 0

m must be antisymmetric

### Projection Math

Point represented by tiny sphere

p = 4D point vector, wHat = unit vector (0, 0, 0, 1), the 4D eye is also conveniently placed at wHat, Pi(p) = Projection of 4D point

Pi(p) = (p - (p dot wHat))/(1 - (p dot wHat))

s = ball radius scaling due to perspective projection

s<sup>'</sup> = elongation in Pi(p) direction 

s = 1/(1-(p dot w))

s<sup>'</sup> = sqrt{1 + pi<sup>2</sup>(p)}



