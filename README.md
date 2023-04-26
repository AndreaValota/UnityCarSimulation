# Overview

The repository is intended to hold the progress of the project related to the AI for Videogames course at Università degli Studi di Milano.

# Info

The goal of this project is to create an application that simulate cars moving in a
track. The circuit is modeled based on the National Motor Racetrack of Monza
and cars move using two variations of path following algorithms: the Chase the
Rabbit approach and Predictive Path Following approach. The application is
realized using Unity version 2020.3.20f1 and the C# language.

# Implementation details 

The project presents 2 versions of the track:
- an accurate version
- an approximated version created using 3D meshes

Two path following algorithms are implemented:
- Chase the Rabbit
- Predictive Path Following 

The seek behaviour is implemented as the base movement and the destination point is set by the previous path following algorithms. The car movement is simulated by using Dynamic Movement based on physics (using accelerations, drag and fine tuned parameters) to obtain a behaviour similar to the one of a car while maintaining the possibility to generalize it for similar kind of veichles.

Bezier curves are implemented in order to create the path required by the path following algorithms and can be managed directly in the Unity editor.   

A more in depth description of the project and documentation about his use can be found in the PDF provided in this repository.

![Approximated track presentation](Examples/Approximated_track_demo.gif)




