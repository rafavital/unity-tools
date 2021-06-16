# How To Use

Here is a break down of each tool and how to use it:

## Script Cleaner

This tool will clean any (one) selected object in the hierarchy window and it's children of all scripts associated with them.

### Step 1

Select the object you want to clean

Image of the inspector window before the cleaning:

![Image showing the project's hierachy and two inspector windows, one showing the Parent's components and the other showing the child's components.](./img/script_cleaner_inspector.png)

1. The parent in the hierarchy
2. The child in the hierarchy
3. The parent's inspector window
4. The child's inspector window

### Step 2

Right Click the game object in the hierarchy window and click in _"Automation" > "Remove Scripts"_

Then click _"yes"_ in the confirmation window:

![Image showing a confirmation script asking if the user wants to remove all associated scripts from the selected object and it's children](./img/script_cleaner_confirm.png)

After that the objects will have been stripped clean from their components (scripts)

![Image showing the project's hierachy and two inspector windows, one showing the Parent without any components and the other showing the child without any components.](./img/script_cleaner_inspector_after_clean.png)

1. The parent in the hierarchy
2. The child in the hierarchy
3. The parent's inspector window
4. The child's inspector window

## Animation Rec

This tool can be used for recording an object's transform changes inside the editor (outside of play mode) and saving it as an animation clip for later use.

### Step 1

Open the tool's window in the top menu of the editor _"Tools"_ > _"Animation Rec"_

### Step 2

Setup the tool:

![The image shows the window of the tool inside of Unity](img/animation_rec_window.png)

1. Select the object you want to record and assign it into the "Object to Animate" field.
2. Setup the keys to start, stop and delete the recording if you don't want to use the default ones.
3. Select your recording frequency (that is the speed with which each new frame will be set).
4. Change the clip's name.
5. Select the folder where you want to save the animation.

### Step 3

Start recording by either pressing the assigned "Start Recording Key" or pressing the "Start Recording" button in the window.

![](img/animation_rec_start_btn.png)

### Step 4

#### Saving the clip

Make the changes you want to the transform and when you're happy with the animation press the assigned "Stop Recording Key" or press the "Stop Recording" button in the tool window.

![](img/animation_rec_stop_btn.png)

#### Deleting the clip

If you don't want to save the currently recording clip you can press the assigned "Delete Recording Key" which will stop the recording and delete the clip.

### Step 5

In order to use the recently recorded clip add an Animator Controller to the object you selected for the animation (if it doesn't already have one) and add the clip to the AC, by dragging it or selecting it inside of the AC.

![](img/animation_rec_ac.png)

## Physical Simulator

This tool allow's the user to run Unity's physics simulation inside the editor without having to press play. Thus allowing the user to test the physics simulation or place objects organically using physics for example.

### Step 1

Open the tool's window in the top menu of the editor _"Automation"_ > _"Physical Simulator"_

### Step 2

Fill the "Objects to Simulate" list with the objects you want to simulate.

![](img/physical_simulator_window.png)

### Step 3

#### Start Simulation

Press the play button to start simulating

![Physical simulator play button](img/physical_simulator_play_btn.png)

#### Stop Simulation

Press the stop button to stop simulating

![Physical simulator stop button](img/physical_simulator_stop_btn.png)

### Step 4

If you're not satisfied with the end of the simulation or want to modify the initial setup you can press the "undo" button to reset the object's transforms to how they were prior to the simulation.

![Physical simulator undo button](img/physical_simulator_undo_btn.png)
