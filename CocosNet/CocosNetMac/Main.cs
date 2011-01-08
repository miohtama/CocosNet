using System;
using System.Drawing;
using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.ObjCRuntime;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Audio;
//using OpenTK.Math;
using OpenTK.Input;
using OpenTK.Platform;

namespace CocosNetMac
{
  class Game : GameWindow
    {
        // Creates a new TextPrinter to draw text on the screen
        Font sans_serif = new Font(FontFamily.GenericSansSerif, 18.0f);
        int texture = 0;
 
        /// <summary>Creates a 800x600 window with the specified title.</summary>
        public Game()
            : base(800, 600, GraphicsMode.Default, "OpenTK Quick Start Sample")
        {
            VSync = VSyncMode.On;
        }
 
 
        [STAThread]
        static void Main()
        {
            // The 'using' idiom guarantees proper resource cleanup.
            // We request 30 UpdateFrame events per second, and unlimited
            // RenderFrame events (as fast as the computer can handle).
            using (Game game = new Game())
            {
                game.Run(30.0, 0.0);
            }
        }
    }
}

