using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Joust
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    /// 
    public class spriteClass
    {
        protected Texture2D texture;
        protected Viewport view;
        protected Rectangle currentRect;
        protected Rectangle spriteRectangle;
        protected int maxSize;
        protected int minSize;
        protected string facing;
        protected int currentFrame;
        public int ticks;
        protected int frameWidth;
        protected spriteClass(int passedInFrameWidth)
        {
            frameWidth = passedInFrameWidth;
            currentFrame = 0;
        }


       public void setTexture(Texture2D passedInTexture)
        {
            texture = passedInTexture;
        }

        public Texture2D getTexture()        {     return texture;
        }

        public Rectangle getRectangle()
          {
            return spriteRectangle;
        }

        public void setRectangle(Rectangle passedIn)
        {
            spriteRectangle = passedIn;
        }
       
        public Rectangle getCurrentFrame()
         {
            return currentRect;
         }
        public void setSourceRectangle(int x, int y,int width, int hieght)
        {
            currentRect = new Rectangle(x, y, width, hieght);
        }

    }

    public class bird : spriteClass
    {
        public float gravity;
        public int buffer;
        public bool standing;
        

        protected bird():base(20)
        {
            gravity = -3;
        }
        public void fall()
        {
            if (!standing)
            {
                spriteRectangle.Y -= (int)gravity;
            }
        }
        public void move(int change, int clock)
        {
            if (change == 10) { 
                currentRect.X += 10;
                nextFrame(350, 566,166);
            }
            else if (change == -10) {
                currentRect.X -= 10;
                nextFrame(260,86,215);
            }
        }
        private void nextFrame(int minSize,int maxSize, int row)
        {

            if (ticks >= 30)
            {
                if (currentFrame < maxSize) { currentFrame += frameWidth; }
                else if (currentFrame > maxSize) { currentFrame = minSize; }
                ticks = 0;
            }

            else
            {
                ticks++;
            }
        }

    }

    public class playerClass : bird
    {
        public bool canJump;
        public enum Actions
        {
            jump,
            left,
            right
        }

        public playerClass(Viewport currentView)
        {
            view = currentView;
            setRectangle(new Rectangle(0,300,50,50));
            currentRect = new Rectangle(0, 0, 20, 18);
            
        }


        
    }

    public class ground : spriteClass
    {
        Viewport currentView;

        public ground(Viewport view):base(0)
        {
            currentView = view;
        }


    }

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        playerClass player;
        ground[] floors = new ground[4];
        int clock = 0;

        public void setUpFloors()
        {
            
            floors[0].setRectangle(new Rectangle(600,100,100,10));
            floors[1].setRectangle(new Rectangle(100, 300, 100, 10));
            floors[2].setRectangle(new Rectangle(200,200,100,10));
            floors[3].setRectangle(new Rectangle(10,GraphicsDevice.Viewport.Height-3, 1500,6));

            floors[0].setSourceRectangle(74, 19, 56, 10);
            floors[1].setSourceRectangle(0,19,62,10);
            floors[2].setSourceRectangle(95, 0, 87, 10);
            floors[3].setSourceRectangle(4,31,384,10);


        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            player = new playerClass(GraphicsDevice.Viewport);
            for (int x = 0; x <4; x++)
            {
                floors[x] = new ground(GraphicsDevice.Viewport);
            }
            
            setUpFloors();
            base.Initialize();
            
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            player = new playerClass(GraphicsDevice.Viewport);
            player.setTexture(Content.Load<Texture2D>("brd"));
            
            for (int x = 0; x < 4; x++)
            {
                floors[x].setTexture(Content.Load<Texture2D>("jsprites"));
            }
            
            
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            clock++;
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            
            
            // TODO: Add your update logic here
             KeyboardState currentKeyboard = Keyboard.GetState();
            Keys[] keys;
             keys = currentKeyboard.GetPressedKeys();

            //section that determines gravity for the player and others
            

            
            for (int x = 0; x < 4; x++)
            {
                if (player.getRectangle().Intersects(floors[x].getRectangle()))
                {
                    player.standing = true;
                    
                    break;
                }
                if (x >= 3)
                {
                    player.standing = false;
                }
            }

           


            if (keys.Length != 0)
            {
                
                if (keys.Contains(Keys.Space) && player.canJump == true)
                {
                   
                }
                if (keys.Contains(Keys.D) )
                {
                    player.move(10, clock);
                  
                }
                if (keys.Contains(Keys.A) )
                {
                  player.move(-10,clock);

                }
            }
            
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            spriteBatch.Begin();

                     spriteBatch.Draw(player.getTexture(),player.getRectangle(), player.getCurrentFrame(), Color.White);

                     for (int x = 0; x < 4; x++)
                     {
                         spriteBatch.Draw(floors[x].getTexture(), floors[x].getRectangle(), floors[x].getCurrentFrame(), Color.White);

                     }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
