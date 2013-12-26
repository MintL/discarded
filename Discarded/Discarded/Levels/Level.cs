using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Discarded.Components;

namespace Discarded.Levels
{
    public class Level
    {
        protected Game game;
        protected SpriteBatch spriteBatch;
        protected Random random;

        private Player player;
        private Monster monster;
        private TimeSpan spawnTimer;

        // Static sprites
        protected GameObject ground;

        // Object lists
        public List<GameObject> ColliderObjects = new List<GameObject>();
        public List<GameObject> Objects = new List<GameObject>();
        public List<TerrainLine> Terrain = new List<TerrainLine>();


        public List<Layer> Layers = new List<Layer>();

        public Vector2 CameraPosition;
        protected int screenWidth;
        protected int screenHeight;

        public Level(Game game)
        {
            this.random = new Random();
            this.game = game;
            this.CameraPosition = Vector2.Zero;
            this.screenWidth = game.GraphicsDevice.Viewport.Width;
            this.screenHeight = game.GraphicsDevice.Viewport.Height;
        }

        public void LoadContent()
        {
            this.spriteBatch = (SpriteBatch)game.Services.GetService(typeof(SpriteBatch));

            player = new Player(game, this, new Vector2(200, 400));

            Vector2 tp = new Vector2(100, 250);
            Terrain.Add(new TerrainLine(tp + new Vector2(0, 432), tp + new Vector2(257, 347)));
            Terrain.Add(new TerrainLine(tp + new Vector2(257, 347), tp + new Vector2(362, 216)));
            Terrain.Add(new TerrainLine(tp + new Vector2(362, 216), tp + new Vector2(590, 188)));
            Terrain.Add(new TerrainLine(tp + new Vector2(590, 188), tp + new Vector2(754, 285)));
            Terrain.Add(new TerrainLine(tp + new Vector2(754, 285), tp + new Vector2(843, 387)));
            Terrain.Add(new TerrainLine(tp + new Vector2(843, 387), tp + new Vector2(1080, 450)));

            Terrain.Add(new TerrainLine(tp + new Vector2(1080, 450), new Vector2(1286, 710)));
            Terrain.Add(new TerrainLine(new Vector2(1286, 710), new Vector2(1454, 600)));
            Terrain.Add(new TerrainLine(new Vector2(1454, 600), new Vector2(1616, 515)));
            Terrain.Add(new TerrainLine(new Vector2(1616, 515), new Vector2(1760, 510)));

            //Texture2D groundTex = game.Content.Load<Texture2D>("terrain");
            //ground = new GameObject();
            //ground.AddComponent(new Transform(ground, tp));
            //ground.AddComponent(new Sprite(ground, spriteBatch, groundTex));
            //ground.AddComponent(new Collider(ground, new Rectangle(0, 0, groundTex.Width, groundTex.Height)));
            //Objects.Add(ground);

            // TODO: Add HUD sprites
            //AddStaticSprite(new Vector2(100, 100), "selector");

            Layer background = new Layer(1, 0.75f, spriteBatch, game.Content);
            background.AddStaticSprite(tp, "terrain");
            background.AddStaticSprite(new Vector2(1250, 475), "steps");
            background.AddStaticSprite(new Vector2(1665, 275), "tree1");
            background.AddStaticSprite(new Vector2(1401, 375), "tree1");
            background.AddStaticSprite(new Vector2(1489, 685), "flower1");
            background.AddStaticSprite(new Vector2(1683, 679), "stone1");
            background.AddStaticSprite(new Vector2(1475, 585), "fence");

            Layers.Add(background);
        }

        

        // Spawn monsters
        public void SpawnMonster(Vector2 pos)
        {
            monster = new Monster(game, pos);
            Objects.Add(monster);
            ColliderObjects.Add(monster);
        }

        public void Update(GameTime gameTime)
        {
            player.Update(gameTime);

            // Update camera position to always stay in the middle of screen, centered around the player
            CameraPosition = player.Transform.Position - Vector2.UnitX * screenWidth / 2;

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

            // Collision detection between collider objects
            // TODO: Quad tree
            for (int i = 0; i < ColliderObjects.Count; i++)
            {
                GameObject obj = ColliderObjects[i];
                if (!obj.Collider.Active) continue;

                for (int j = i + 1; j < ColliderObjects.Count; j++)
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

            // Remove all objects marked to be removed
            ColliderObjects.RemoveAll(o => o.Remove);
            Objects.RemoveAll(o => o.Remove);
        }

        public void Draw()
        {
            // Draw everything in relation to the position of the player in the middle of the screen
            Matrix cameraTransform = Matrix.CreateTranslation(-CameraPosition.X, 0.0f, 0.0f);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone, null, cameraTransform);

            foreach (Layer layer in Layers)
            {
                layer.Draw();
            }

            player.Draw();   

            // Draw every object
            foreach (var obj in Objects)
            {
                obj.Draw();
            }

            


            spriteBatch.End();
        }

    }
}
