# VR Racing Game Project
A class-based, combat-oriented VR racing game for the Oculus Rift (and later HTC Vive). 

Useful skills learned thus far:
- Object oriented programming principles:
  - Used polymorphism and inheritance to create a power-up system (soon to be replaced with Scriptable Objects)
- Machine learning principles:
  - Using Unity's ml-agents framework, I've successfully trained an enemy vehicle AI using the reinforcement-based learning method to avoid obstacles while racing around the track and picking up power-ups, all while prioritizing speed. 
  - After experimenting with visual observations based off a camera mounted to the front of the vehicle, I saw results but they weren't as clean as I'd like them to be, so I switched to a raycast-based observation strategy, which proved to be more optimal in my case. 
  - <a href="https://imgur.com/a/UQyxCEc">Here is a link</a> to a gif showing the training of agents in action, soon after being introduced to one another on the same track.
- Editor Scripting
  - Created a custom inspector for the power-up system that allows the designer to choose from a list of enumerators and specify values for the given power-up.
- Delegate pattern principles
  - Created a system to monitor controller events to look for changes in trigger and touchpad axes. 
- Unity Physics
  - Used wheel colliders and rigidbodies to control the movement of the kart.
- Unity Shader Graph
  - Created several simple shaders to use as power-ups 
  - 3 of the shaders so far: <a href="https://imgur.com/a/bhucOxS">Hologram, Ripple and Dissolve</a>


## Links
Trello: https://trello.com/b/GrUn485m/vr-racing-game
