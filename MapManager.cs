using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;



namespace MonoGame_S1
{
    public class MapManager
    {
        public List<TileMaps> Levels = new();
        public int CurrentLevelIndex = 0;

        public TileMaps CurrentMap => Levels[CurrentLevelIndex];

        public void LoadLevels()
        {
            var level1 = new TileMaps();
            level1.mg = level1.LoadMap("../../../TileMapsFiles/CSV_files/level1_mg.csv");
            level1.fg = level1.LoadMap("../../../TileMapsFiles/CSV_files/level1_fg.csv");
            level1.dead = level1.LoadMap("../../../TileMapsFiles/CSV_files/level1_dead.csv");
            level1.win = level1.LoadMap("../../../TileMapsFiles/CSV_files/level1_win.csv");
            level1.sign = level1.LoadMap("../../../TileMapsFiles/CSV_files/level1_sign.csv");
            Levels.Add(level1);

            // var level2 = new TileMaps();
            // level1.mg = level1.LoadMap("../../../TileMapsFiles/CSV_files/level1_mg.csv");
            // level1.fg = level1.LoadMap("../../../TileMapsFiles/CSV_files/level1_fg.csv");
            // level1.dead = level1.LoadMap("../../../TileMapsFiles/CSV_files/level1_bg.csv");
            // level1.win = level1.LoadMap("../../../TileMapsFiles/CSV_files/level1_bg.csv");
            // level1.sign = level1.LoadMap("../../../TileMapsFiles/CSV_files/level1_bg.csv");
            // Levels.Add(level1);
        }

        public void NextLevel()
        {
            if (CurrentLevelIndex + 1 < Levels.Count)
            {
                CurrentLevelIndex++;
            }
            else
            {
                // Reset to the first level if there are no more levels

            }
        }

    }


}