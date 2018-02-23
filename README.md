# Deflector
[Logo]

Deflector is a game made in the Unity engine. All of its code and art assets are open source and available for anyone to look at, modify and include in other games. If this project has helped you in any way, feel free to leave a donation at Deflector’s Itch.io page.

## Author
Deflector was made by Thomas Viktil. [Follow me on Twitter](https://twitter.com/mandarinx) for tweets about programming and game development.

[Screenshots 3x3 / 2x2]

Deflector started as a challenge to make a game in a week. I didn’t have a full week to spend on it and ended up spending about 10 hours making the first version. The reception was fairly good, so I kept working on it. The goal of the project was to release a game, no matter how small or terrible. I chose to release the source code so that other game developers could have a look behind the scene, and maybe even learn a thing or two.

I have written as much documentation as I think is necessary to understand the source code. Most of it is, in my opinion, fairly simple and straight forward, and shouldn’t be hard to understand. Some of the concepts are taken from elsewhere. I won’t explain the concepts, but rather link to the source and instead provide my reflections and experiences with the concepts.

## Downloads
You can download the latest builds from the release tab, here.

## Contents
- [Architecture](#architecture)
- Events
- Tilemap
- Custom assets
- UI
- Pixel art camera

## Contributions
I’d be happy to accept pull requests. You can also fork the project and do whatever you want with it. Please let me know if you make anything out of this. I’d love to see it!

## License
MIT License

Copyright (c) 2017 Thomas Viktil

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

# <a name=“architecture”></a>Architecture
I chose an event driven architecture, where most of the components communicates via events as ScriptableObjects and UnityEvents for event handlers. As much of the data as possible was separated from the MonoBehaviours and put in ScriptableObjects. 

The idea of using ScriptableObjects for events was taken from [Ryan Hipple’s talk at the Unite Austin 2017 conference](https://www.youtube.com/watch?v=raQ3iHhE_Kk).

A reason for separating data from code is to make it easier to change data without causing too many changes in the source files. When you change a field of a MonoBehaviour, the entire scene file is marked as changed. Committing scene files is an almost guaranteed way of causing merge conflicts. Even if you made only a single change, you can bet something else has changed, too.

Another reason for keeping data in ScriptableObjects is to be able to edit values in play mode, without loosing them when reentering edit mode. ScriptableObjects live outside the scene and are not affected by the switch between play and edit mode.

No matter what type architecture you use, it’s always a big challenge to create a clean and flexible API, and to stay in control of the code flow. This kind of architecture is no different. I think this project can work as an introduction to thinking of code as many smaller, isolated pieces. Components works best when they have few to no dependencies. Components that depend on many other components will more easily lead to spaghetti, and spaghetti is very visible in a setup like this. Although it’s hard to work with a setup like this, it can help you to always reflect on the choices you make and the code you write.


# Advantages to the architecture
**Composition over inheritance**
By using composition instead of inheritance, you end up creating a library of many smaller components that do one or a few things. It’s very easy to find bugs when the classes are less than a hundred lines. It can also speed up the development since you don’t have to compile the project for every change. You simply drag and drop all the components you need, and do the plumbing via Unity’s Inspector instead of in code.

**Reusable components**
Generic components are free of context. A component for tinting a sprite’s color can be used in many different contexts, but the act of changing the color is still the same. These components are easier to reuse, both within the project and across multiple projects. 

**Flexibility**
A library of generic components with a well designed API, makes for a very flexible system. This is very good for prototyping and the first phase of development. 

**Data in ScriptableObjects**
ScriptableObjects live outside the scene. Any changes you make to a ScriptableObject while in play mode, gets serialized to disk and not lost when you reenter edit mote. Due to living outside the scene, they make it a lot easier to work in a multi scene setup. You can rely on ScriptableObjects for inter-scene communication. Unity won’t allow you to make a reference between two GameObjects in two different scenes. So, you can use a ScriptableObject as a middle man. This goes for any other Unity asset that doesn’t live in the scene, like an Animator or a Timeline asset.

# Disadvantages to the architecture
**Hard to get an overview**
Your GameObjects will often end up with lots and lots of components, and they will all be referencing each other via UnityEvents. When they also communicate with other GameObjects via ScriptableObjects, it tends to be quite hard to really see what’s going on. Fixing bugs within a component is easier, but finding bugs in the plumbing is very hard. You need to good editor tools to help you visualize the code flow from component to component.

**Temporarily increasing complexity**
As you create more and more complex behaviours by composing lots of little components, you add complexity to the project. At some point it becomes hard to work with, and that’s when you should consider collapsing many generic components into a one or a few more game specific components. This is a disadvantage because you have to spend time making those game specific components. That work won’t take you any further towards your goal. In addition you are adding new code to the project, and therefore new bugs. There’s a technical debt building up as the complexity of your setup increases.

**Generic callbacks**
Due to the generic nature of the components, they will never output any of the custom data types you have created for your project. You’ll find that in some cases you would like a component to output a specific value. Instead you get something like a GameObject or a Transform, and therefore have to use GetComponent to get what you want. Sometimes you need to call GetComponent in multiple places within the same frame, which is a waste of CPU time. It might not be much, but it’s still unnecessary.

# Events
Ryan Hipple gave a very good explanation of the event system in his talk. Watch his talk to get all the details of how the system works.

I have used this system in two different projects, and have seen some of it’s strengths and weaknesses. As Ryan demonstrates in his talk, one of the strengths is how easy it is for a game designer to create behaviours simply via drag and drop. I experienced that the first phase of a project went really fast. I had things up and running in no time. As mentioned in the chapter on architecture, the complexity increases rapidly along with the technical debt.

In both projects where I used this technique, I went all in and used it everywhere. In hindsight, that wasn’t such a good idea. At least I got to really try the concept. For a future project I would create a more clear separation of system code and gameplay code, and expose certain ScriptableObject events to the game designer. By exposing all events, you create a lot of clutter. It makes it too easy for a game designer to hook up with the wrong events.

Although I consider Deflector to be a simple game, this kind of architecture resulted in a surprisingly complex setup. I have drawn a event map to show you how all things are connected.

[Event map]

# Tilemap
The brushes used were originally taken from the [RoboDash repo](https://github.com/Unity-Technologies/2d-gamedemo-robodash) and some code from the [2d-extras repo](https://github.com/Unity-Technologies/2d-extras). I found the RoboDash code quite messy, and cleaned it up and fixed a few bugs.

# Custom assets
Building on the thought of putting all data in ScriptableObjects, I put data like player health, multiplier and score value in each their own ScriptableObject. Using the observer concept, I made them so that one can subscribe to changes in the values. This made setting up UI a lot more easier. The component responsible for changing the value of a ScriptableObject won’t have to know about the UI at all. It simply sets the value. The asset provides the functionality to let any subscribers be notified of changes. If you have tried reactive programming, you’ll recognize this concept.

I haven’t found any disadvantages to putting data in ScriptableObjects. If I have to pick one, it’s that organizing all of the assets becomes a little challenge. It’s certainly not unsolvable if you use good naming conventions and folders. I think organization wise, it would be more clear if one could group these assets into a parent asset. So, health, score and multiplier are added as sub assets to a parent asset. That way, you could also instantiate a new copy of the parent asset for player number two. I haven’t thought out all the details, so I’m not completely sure if this is a good idea, or doable.

# UI
Over the years I have done quite a lot of UI, and they have always been quit the challenge to keep well organized and bug free. This time I tried using an Animator as a state machine for the UI state. The thought was to handle verification of which state one state can lead to, to the Animator. I wanted to reduced the amount to plumbing code to an absolute minimum. I settled on using triggers for switching state. Setting a trigger on the Animator is simple and done in one line.

Unity’s Animator component has a few caveats that one should be aware of. For instance, if you set two triggers at once, depending on the state the Animator’s in, it might transition to another state. The transition will reset the trigger that caused the transition, but the not the other. That one will still be triggered. If the Animator transitions to a state which can transition to another state using the other trigger you set, then the Animator will instantly transition to the next state.  This kind of behaviour can be a bit hard to figure out why happens. You still need to set the triggers at the right time.

I’m not completely sold on the idea of using an Animator for controlling UI state, but I think there’s something worthwhile investigating further.

# Pixel art camera
Pixel art cameras are a much debated topic among developers. There seem to be as many solutions as there are developers. I researched the subject to get familiar with the problem a pixel art camera tries to solve. I tried many different solutions, but only the free ones. I was of the opinion that a pixel art camera solves a rather simple problem, and I wasn’t willing to spend money on it.

Deflector uses a simple setup with a single screen per level. The camera never moves, and the size of the levels are always the same. It was important for this camera to maintain the correct aspect ratio of the scene, no matter what aspect ratio the monitor has. I opted for an old school solution of adding black borders around the scene when the monitor’s aspect ratio is different from the scene’s.

The math involved in figuring out what size to render the game in, is rather simple. The camera blits the scene to a RenderTexture, and passes it on to screen buffer via a specific material. The material uses a shader which positions the RenderTexture at the center of the screen and fills the borders with the camera’s background color.
