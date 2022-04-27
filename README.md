# VR4D

# Purpose
To expand mind to include spatial model/intuition/navigation of four dimensional space

# What/How it does
Rotates a rigid body (4D cube, 4D sphere, 4D plane etc) in four dimensional space while projecting onto a 3Dimensional screen (VR headset)

Objects are created as point clouds and points are represented by tiny spheres.

# Brief Theory

Attempts to extend the algorithm our brain uses to construct our model/intuition of 3Dimensional space from 2D projections onto our retina (which is itself extending point light sensors on our retina into a 2D projection) by feeding it 3D projections of 4Dimensional rotations.


# Why it is unique

There are other projects looking to explore 4D with VR, these mostly operate on the principle of rendering slices of the 4D world.
This project uses a cognitive approach that builds up our intuition organically (through rotation) while slicing loses the continuity that our brain needs to
stitch 3D projections into a 4D whole. 

# Implications

Opens up the world of intuitive 4D problem solving
Huge cognitive space implications: 
  What is happening in the brain when we start to grasp 4D space?
  Brain changes?
  Make us smarter?
  See patterns previously invisible to us?
  4D metaverse?
  
 # Direction
 
 A web application that uses brain training app like structure, which allows users to freely manipulate objects and rotations, and provides self testing
 functionality to test 4D navigational skills. 
 
 # How to Run (This needs testing)
Cleanest way to run this code currently is to start a new Unity VR/OpenXR project with your VR setups recommended settings
add the src folder to your Assets folder, attach '4dim.cs' to an empty game object, and then run. Remove default backgrounds objects in the unity project.

https://www.youtube.com/watch?v=IGM0zfpOdsg&ab_channel=MihirPatil
The recording quality and effect is greatly diminished compared to original rendering. 

# Code Guide

All source code is in VR4D/Assets/src https://github.com/patilmi/VR4D/tree/main/Assets/src. Main file with start() and update() functions is FourDim.cs. 

Rotation Math

R = Tiny rotation, I = Identity matrix, m = tiny matrix 
R = I + m



  
