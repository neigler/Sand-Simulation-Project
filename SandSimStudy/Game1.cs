using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;


namespace SandSimStudy
{

    public static class Storage
    {
        public static GraphicsDeviceManager GDM;
        public static ContentManager CM;
        public static SpriteBatch SB;
        public static Game1 game;
        public static Texture2D texture;
        public static Rectangle drawRec = new Rectangle(0, 0, 1, 1);
        public static Particle[,] particles;
    }

    public class Particle
    {
        public int x_velocity, y_velocity;
        public bool wood, water, sand, gas;
    }

    public class Game1 : Game
    {
        public const int size_x = 64;
        public const int size_y = size_x;
        public int timer = 0;

        public bool wood;
        public bool water;
        public bool sand;
        public bool gas;

        public Game1()
        {
            Storage.GDM = new GraphicsDeviceManager(this);
            Storage.GDM.GraphicsProfile = GraphicsProfile.HiDef;
            Storage.CM = Content;
            Storage.game = this;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromMilliseconds(16.667); // 60 frames/sec = 16.667 per frame
        }

        protected override void Initialize()
        {
            Storage.GDM.PreferredBackBufferWidth = 512;
            Storage.GDM.PreferredBackBufferHeight = 512;
            Storage.GDM.ApplyChanges();
            Storage.particles = new Particle[size_x, size_y];
            for (int x = 0; x < size_x; x++)
            {
                // loop through row
                for (int y = 0; y < size_y; y++)
                {
                    // loop through columns
                    Storage.particles[x, y] = null; // clear the grid
                }
            }

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Storage.SB = new SpriteBatch(GraphicsDevice); // set sprite batch
            if (Storage.texture == null)
            {
                // texture used for drawing, will be scaled later.
                Storage.texture = new Texture2D(Storage.GDM.GraphicsDevice, 1, 1);
                Storage.texture.SetData<Color>(new Color[] { Color.White });
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit(); // Destroy the window when pressing the escape button

            if (Keyboard.GetState().IsKeyDown(Keys.D1))
            {
                wood = true;
                sand = false;
                gas = false;
                water = false;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D2))
            {
                wood = false;
                gas = false;
                sand = true;
                water = false;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D3))
            {
                wood = false;
                sand = false;
                gas = false;
                water = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D4))
            {
                wood = false;
                sand = false;
                water = false;
                gas = true;
            }

            for (int x = 0; x < size_x; x++)
            {
                // loop through row
                for (int y = 0; y < size_y; y++)
                {
                    // loop through column 
                    var particle = Storage.particles[x, y];
                    if (particle != null)
                    {
                        if (particle.sand == true) // Sand Logic
                        {
                            particle.y_velocity += 1;
                            if (MathF.Abs(particle.y_velocity) > 1)
                            {
                                particle.y_velocity = 1 * Math.Sign(particle.y_velocity);
                            }

                            particle.x_velocity += 0;
                            if (MathF.Abs(particle.x_velocity) > 1)
                            {
                                particle.x_velocity = 1 * Math.Sign(particle.x_velocity);
                            }

                            int next_x = x + particle.x_velocity;
                            int next_y = y + particle.y_velocity;
                            if (!find_collision(next_x, next_y))
                            {
                                // Go there.
                            }
                            else if (!find_collision(next_x - 1, next_y))
                            {
                                next_x -= 1;
                            }
                            else if (!find_collision(next_x + 1, next_y))
                            {
                                next_x += 1;
                            }
                            else
                            {
                                // cant move
                                next_x = x;
                                particle.y_velocity = 0;

                                next_y = y;
                                particle.y_velocity = 0;
                            }
                            Storage.particles[x, y] = null;
                            x = next_x;
                            y = next_y;
                            Storage.particles[x, y] = particle;
                        }
                        if (particle.water == true) // Water Logic
                        {
                            particle.y_velocity += 1;
                            if (MathF.Abs(particle.y_velocity) > 1)
                            {
                                particle.y_velocity = 1 * Math.Sign(particle.y_velocity);
                            }

                            particle.x_velocity += 0;
                            if (MathF.Abs(particle.x_velocity) > 1)
                            {
                                particle.x_velocity = 1 * Math.Sign(particle.x_velocity);
                            }

                            int next_x = x + particle.x_velocity;
                            int next_y = y + particle.y_velocity;
                            if (!find_collision(next_x, next_y))
                            {
                                // Go there.
                            }
                            else if (!find_collision(next_x - 1, next_y))
                            {
                                next_x -= 1;
                            }
                            else if (!find_collision(next_x + 1, next_y))
                            {
                                next_x += 1;
                            }
                            else if (!find_collision(next_x - 1, y))
                            {
                                next_x -= 1;
                                next_y = y;
                                particle.y_velocity = 0;
                            }
                            else if (!find_collision(next_x + 1, y))
                            {
                                next_x += 1;
                                next_y = y;
                                particle.y_velocity = 0;
                            }
                            else
                            {
                                // cant move
                                next_x = x;
                                particle.y_velocity = 0;

                                next_y = y;
                                particle.y_velocity = 0;
                            }
                            Storage.particles[x, y] = null;
                            x = next_x;
                            y = next_y;
                            Storage.particles[x, y] = particle;
                        }
                        if (particle.gas == true) // Water Logic
                        {
                            particle.y_velocity -= 1;
                            if (MathF.Abs(particle.y_velocity) > 1)
                            {
                                particle.y_velocity = 1 * Math.Sign(particle.y_velocity);
                            }

                            particle.x_velocity += 0;
                            if (MathF.Abs(particle.x_velocity) > 1)
                            {
                                particle.x_velocity = 1 * Math.Sign(particle.x_velocity);
                            }

                            int next_x = x + particle.x_velocity;
                            int next_y = y + particle.y_velocity;
                            if (!find_collision(next_x, next_y))
                            {
                                // Go there.
                            }
                            else if (!find_collision(next_x - 1, next_y))
                            {
                                next_x -= 1;
                            }
                            else if (!find_collision(next_x + 1, next_y))
                            {
                                next_x += 1;
                            }
                            else if (!find_collision(next_x - 1, y))
                            {
                                next_x -= 1;
                                next_y = y;
                                particle.y_velocity = 0;
                            }
                            else if (!find_collision(next_x + 1, y))
                            {
                                next_x += 1;
                                next_y = y;
                                particle.y_velocity = 0;
                            }
                            else
                            {
                                // cant move
                                next_x = x;
                                particle.y_velocity = 0;

                                next_y = y;
                                particle.y_velocity = 0;
                            }
                            Storage.particles[x, y] = null;
                            x = next_x;
                            y = next_y;
                            Storage.particles[x, y] = particle;


                        }
                        if (particle.wood == true) // Wood Logic
                        {
                            // Do nothing
                        }
                    }
                }
            }

            // delay how fast you can create particles.
            timer += gameTime.ElapsedGameTime.Milliseconds;
            var mouse_state = Mouse.GetState();
            if (mouse_state.LeftButton == ButtonState.Pressed && timer > 10 && wood)
            {
                timer = 0;
                var x = mouse_state.Position.X * size_x / Storage.GDM.PreferredBackBufferWidth;
                var y = mouse_state.Position.Y * size_y / Storage.GDM.PreferredBackBufferHeight;

                // only create a particle in this grid tile if nothing is currently there.
                if (x >= 0 && x < size_x && y >= 0 && y < size_y && Storage.particles[x, y] == null)
                {
                    Particle particle = new Particle();
                    particle.wood = true;
                    Storage.particles[x, y] = particle;
                }
            }
            if (mouse_state.LeftButton == ButtonState.Pressed && timer > 10 && water)
            {
                timer = 0;
                var x = mouse_state.Position.X * size_x / Storage.GDM.PreferredBackBufferWidth;
                var y = mouse_state.Position.Y * size_y / Storage.GDM.PreferredBackBufferHeight;

                // only create a particle in this grid tile if nothing is currently there.
                if (x >= 0 && x < size_x && y >= 0 && y < size_y && Storage.particles[x, y] == null)
                {
                    Particle particle = new Particle();
                    particle.water = true;
                    Storage.particles[x, y] = particle;
                }
            }
            if (mouse_state.LeftButton == ButtonState.Pressed && timer > 10 && sand)
            {
                timer = 0;
                var x = mouse_state.Position.X * size_x / Storage.GDM.PreferredBackBufferWidth;
                var y = mouse_state.Position.Y * size_y / Storage.GDM.PreferredBackBufferHeight;

                // only create a particle in this grid tile if nothing is currently there.
                if (x >= 0 && x < size_x && y >= 0 && y < size_y && Storage.particles[x, y] == null)
                {
                    Particle particle = new Particle();
                    particle.sand = true;
                    Storage.particles[x, y] = particle;
                }
            }
            if (mouse_state.LeftButton == ButtonState.Pressed && timer > 10 && gas)
            {
                timer = 0;
                var x = mouse_state.Position.X * size_x / Storage.GDM.PreferredBackBufferWidth;
                var y = mouse_state.Position.Y * size_y / Storage.GDM.PreferredBackBufferHeight;

                // only create a particle in this grid tile if nothing is currently there.
                if (x >= 0 && x < size_x && y >= 0 && y < size_y && Storage.particles[x, y] == null)
                {
                    Particle particle = new Particle();
                    particle.gas = true;
                    Storage.particles[x, y] = particle;
                }
            }

            base.Update(gameTime);
        }

        bool find_collision(int x, int y)
        {
            if (y >= size_x)
            {
                return true;
            }
            if (x <= -1 || x >= size_y)
            {
                return true;
            }
            return Storage.particles[x, y] != null;
        }

        protected override void Draw(GameTime gameTime)
        {
            SpriteBatch targetBatch = new SpriteBatch(GraphicsDevice);
            RenderTarget2D target = new RenderTarget2D(GraphicsDevice, size_x, size_y);
            GraphicsDevice.SetRenderTarget(target);

            GraphicsDevice.Clear(Color.Black);

            // Storage.SB.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
            Storage.SB.Begin();

            int depth = 0;
            for (int x = 0; x < size_x; x++)
            {
                for (int y = 0; y < size_y; y++)
                {
                    depth += 1;
                    Particle particle = Storage.particles[x, y];
                    if (particle != null)
                    {
                        if (particle.wood == true)
                        {
                            Vector2 pos = new Vector2(x, y);
                            // Draw each particle as a sprite.
                            Storage.SB.Draw(Storage.texture,
                                pos,
                                Storage.drawRec,
                                Color.SaddleBrown,
                                0,
                                Vector2.Zero,
                                1.0f, // Scale
                                SpriteEffects.None,
                                1);
                        }
                        if (particle.sand == true)
                        {
                            Vector2 pos = new Vector2(x, y);
                            // Draw each particle as a sprite.
                            Storage.SB.Draw(Storage.texture,
                                pos,
                                Storage.drawRec,
                                Color.SandyBrown,
                                0,
                                Vector2.Zero,
                                1.0f, // Scale
                                SpriteEffects.None,
                                depth * 0.00001f);
                        }
                        if (particle.water == true)
                        {
                            Vector2 pos = new Vector2(x, y);
                            // Draw each particle as a sprite.
                            Storage.SB.Draw(Storage.texture,
                                pos,
                                Storage.drawRec,
                                Color.SkyBlue,
                                0,
                                Vector2.Zero,
                                1.0f, // Scale
                                SpriteEffects.None,
                                depth * 0.00001f);
                        }
                        if (particle.gas == true)
                        {
                            Vector2 pos = new Vector2(x, y);
                            // Draw each particle as a sprite.
                            Storage.SB.Draw(Storage.texture,
                                pos,
                                Storage.drawRec,
                                Color.Gray,
                                0,
                                Vector2.Zero,
                                1.0f, // Scale
                                SpriteEffects.None,
                                depth * 0.00001f);
                        }
                    }
                }
            }

            Storage.SB.End();

            // Set rendering back to the back buffer.
            GraphicsDevice.SetRenderTarget(null);

            // Render target to back buffer.
            targetBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp);
            // Scale target from 64x64 to 512x512.
            targetBatch.Draw(target, new Rectangle(0, 0, Storage.GDM.PreferredBackBufferWidth, Storage.GDM.PreferredBackBufferHeight), Color.White);
            targetBatch.End();

            base.Draw(gameTime);
        }
    }
}
