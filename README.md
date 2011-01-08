# CocosNet #
This readme last changed: 2/27/2010  
matt.e.greer@gmail.com  
http://github.com/city41/cocosnet  

## CocosNet is... #
A port of [Cocos2D for the iPhone](http://www.cocos2d-iphone.org) to the [MonoTouch](http://monotouch.net) platform. Cocos2D for the iPhone was in turn a port of [Cocos2D](http://www.cocos2d.org) which is written in Python.

CocosNet currently targets MonoTouch for the iPhone and iPad, but it has an ultimate goal of being used in just about any .NET environment (Windows, Windows Phone, Xbox, etc).

## License ##
Please see the LICENSE file for licensing info. CocosNet carries the same license as Cocos2D for the iPhone.

## Contributors ##
Please see the AUTHORS file for a list of contributors

## Prerequisitements ##

* [Apple XCode](http://developer.apple.com/)

* [Mono Framework](http://www.go-mono.com/mono-downloads/download.html)

* [MonoDevelop](http://monodevelop.com/download)

* [Git](http://www.macports.org)

* [OpenTK,Mac desktop](http://www.opentk.com/files/download-opentk.html)

## How to run ##
* Compile CocosNetLib and CocosNetTests
* You can just load CocosNET.sln into MonoDevelop and build it (Cmnd-B)
* Launch into the simulator or your device
* By default, cmnd-shift-enter in MonoDevelop should do this
* You will be launching the test app, which will load one of the tests and run it.
 To see which test is launched and/or change it, look at Main.cs in CocosNetTests and change the instantiate scene, the code looks like this:
  

          // To run a different test, instantiate a different class here  
          // SpriteTest -- SpriteManual  
          // ParallaxTest -- Parallax1  
          Scene scene = new Scene(new SpriteManual());

## How to use CocosNet in your MonoTouch app ##
CocosNet is not distributed as a binary, yet. So far it is all source code. The easiest way to add CocosNet into your application is to grab CocosNet from here, and then add the project to your MonoTouch solution, and build CocosNet along with your app.  
  
I recommend grabbing a tarball or zip file of CocosNet if you take this approach. I make certain CocosNet always builds and is usable.  
  
As CocosNet grows, we will move into official releases and you can then just reference an assembly in your app. We're not quite there yet.

## What has been ported so far ##
The following generally works well in CocosNet

The main infrastructure  
  
* Director
* CocosNode, TextureNode, AtlasNode
* Layer, Scene
* TextureMgr
* Label
* Camera
* Drawing primitives
* etc
  
Texture Related  

* Texture2D  
* PVRTexture  
  
Atlases  

* TextureAtlas and LayerAtlas  
  
Menus  

* Menu  
* MenuItemImage  
  
Actions  

* most sprite oriented actions implemented  
* see SpriteTest.cs and the Actions namespace  
  
Parallax  

* ParallaxNode  
* see ParallaxTest.cs  
  
Particle Systems  

* ParticleSystem base class, PointParticleSystem  
* most point examples  
* ParticleTest.cs  

## Using OpenTK ##

To use OpenTK, simply add "OpenTK.dll" as a reference to your project. You can find this file under "Binaries/OpenTK".

Additionally, you should add "OpenTK.dll.config" to your project and set it to be copied to your output directory. Without OpenTK.dll.config, your application will not function correctly on Linux or MacOS.

* Download and extract OpenTK

* Choose CocosNetMac project in MonoDeveloper

* Choose Project -> Edit References -> .NET assembly. Add OpenTK.dll

* Right click CocosNetMac

* Add.. File OpenTK.dll.config

* Right click OpenTK.dll.config

* Choose "Copy to output directory"

[More details](http://www.opentk.com/doc)

[Listing OpenTK as assemblies in OSX MonoDevelop](http://www.thebinaryidiot.com/archives/2010/09/26/monodevelop-mysql-connector-reference/)

## How to Contribute ##
CocosNet is my first open project like this, please bear with me if I'm not the best coordinator just yet :)

### Small Contribution: Send me a patch ###
If you would like to contribute something small (ideally all in one file), please send me a patch. This is very easy to do with Git, here is a nice [tutorial](http://ariejan.net/2009/10/26/how-to-create-and-apply-a-patch-with-git/) on how to do that.

### Large Contribution: fork on GitHub ###
If you would like to contribute a large bug fix, feature, etc, please fork my main branch on github, add your work, and let me know and I will pull it into the main branch.

Please see the ContributorsGuide.txt file (in the same directory as this file you are reading) for detailed information on contributing to CocosNet: what is needed, how I am approaching the port, reporting/dealing with bugs, etc.

## Further reading ##

OpenTK graphics library for Mono - http://www.opentk.com/project/opentk

## Troubleshooting ##

### Error: You should provide one root assembly only ###

This pops up if you try to compile CocosNet without MonoTouch installed

[Also check this thread](http://forums.monotouch.net/yaf_postst985.aspx)



