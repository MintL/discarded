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
using Discarded.Components;

namespace Discarded
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class DiscardedGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Random random;

        private Player player;
        private Monster monster;
        private TimeSpan spawnTimer;

        // Static sprites
        private GameObject ground;
        private GameObject selector;

        // Object lists
        public List<GameObject> ColliderObjects = new List<GameObject>();
        public List<GameObject> Objects = new List<GameObject>();
        public List<TerrainLine> Terrain = new List<TerrainLine>();

        public DiscardedGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 800;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            IsMouseVisible = true;
            random = new Random();

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

            Services.AddService(typeof(SpriteBatch), spriteBatch);

            player = new Player(this, new Vector2(200, 400));
            //Objects.Add(player);

            Vector2 tp = new Vector2(100, 250);
            Terrain.Add(new TerrainLine(tp + new Vector2(0, 432), tp + new Vector2(257, 347)));
            Terrain.Add(new TerrainLine(tp + new Vector2(257, 347), tp + new Vector2(362, 216)));
            Terrain.Add(new TerrainLine(tp + new Vector2(362, 216), tp + new Vector2(590, 188)));
            Terrain.Add(new TerrainLine(tp + new Vector2(590, 188), tp + new Vector2(754, 285)));
            Terrain.Add(new TerrainLine(tp + new Vector2(754, 285), tp + new Vector2(843, 387)));
            Terrain.Add(new TerrainLine(tp + new Vector2(843, 387), tp + new Vector2(1080, 450)));

            Texture2D groundTex = Content.Load<Texture2D>("terrain");
            ground = new GameObject();
            ground.AddComponent(new Transform(ground, tp));
            ground.AddComponent(new Sprite(ground, spriteBatch, groundTex));
            ground.AddComponent(new Collider(ground, new Rectangle(0, 0, groundTex.Width, groundTex.Height)));
            Objects.Add(ground);

            selector = new GameObject();
            selector.AddComponent(new Transform(selector, new Vector2(100, 100)));
            selector.AddComponent(new Sprite(selector, spriteBatch, Content.Load<Texture2D>("selector")));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        public void AddBullet(Bullet bullet)
        {
            Objects.Add(bullet);
            ColliderObjects.Add(bullet);
        }

        // Spawn monsters
        public void SpawnMonster(Vector2 pos)
        {
            monster = new Monster(this, pos);
            Objects.Add(monster);
            ColliderObjects.Add(monster);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }

            player.Update(gameTime);

            spawnTimer -= gameTime.ElapsedGameTime;
            if (spawnTimer < TimeSpan.FromSeconds(0))
            {
                SpawnMonster(new Vector2(1000, random.Next(200, 400)));
                spawnTimer = TimeSpan.FromSeconds(3);
            }

            // Player v Terrain
            player.Grounded = false;
            TerrainCollider tc = (TerrainCollider)player.GetComponent<TerrainCollider>();
            foreach (TerrainLine line in Terrain)
            {
                tc.CheckCollision(line);
            }


            foreach (var obj in Objects)
            {
                obj.Update(gameTime);
            }

            for (int i=0; i<ColliderObjects.Count; i++) {
                GameObject obj = ColliderObjects[i];
                if (!obj.Collider.Active) continue;

                for (int j=i+1; j<ColliderObjects.Count; j++)
                {
                    GameObject other = ColliderObjects[j];
                    if (!other.Collider.Active) continue;
                    
                    if (obj != other)
                    {
                        obj.Collider.CheckCollision(other);
                    }
                }

                TerrainCollider terrainCollider = (TerrainCollider)obj.GetComponent<TerrainCollider>();
                if (terrainCollider != null)
                {
                    foreach (TerrainLine line in Terrain)
                    {
                        terrainCollider.CheckCollision(line);
                    }
                }
            }
            ColliderObjects.RemoveAll(o => o.Remove);
            Objects.RemoveAll(o => o.Remove);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Snow);

            spriteBatch.Begin();

            selector.Draw();
            //ground.Draw();
            player.Draw();
            //monster.Draw();

            foreach (var obj in Objects)
            {
                obj.Draw();
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
