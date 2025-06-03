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
            level1.mg = level1.LoadMap("C:/Users/hamno/OneDrive/Documents/GitHub/MonoGame_S1/TileMapsFiles/CSV_files/level1_mg.csv");
            level1.fg = level1.LoadMap("C:/Users/hamno/OneDrive/Documents/GitHub/MonoGame_S1/TileMapsFiles/CSV_files/level1_fg.csv");
            level1.dead = level1.LoadMap("C:/Users/hamno/OneDrive/Documents/GitHub/MonoGame_S1/TileMapsFiles/CSV_files/level1_dead.csv");
            level1.win = level1.LoadMap("C:/Users/hamno/OneDrive/Documents/GitHub/MonoGame_S1/TileMapsFiles/CSV_files/level1_win.csv");
            level1.sign = level1.LoadMap("C:/Users/hamno/OneDrive/Documents/GitHub/MonoGame_S1/TileMapsFiles/CSV_files/level1_Sign.csv");
            Levels.Add(level1);

            var level2 = new TileMaps();
            level2.mg = level2.LoadMap("C:/Users/hamno/OneDrive/Documents/GitHub/MonoGame_S1/TileMapsFiles/CSV_files/level2_mg.csv");
            level2.fg = level2.LoadMap("C:/Users/hamno/OneDrive/Documents/GitHub/MonoGame_S1/TileMapsFiles/CSV_files/level2_fg.csv");
            level2.dead = level2.LoadMap("C:/Users/hamno/OneDrive/Documents/GitHub/MonoGame_S1/TileMapsFiles/CSV_files/level2_dead.csv");
            level2.win = level2.LoadMap("C:/Users/hamno/OneDrive/Documents/GitHub/MonoGame_S1/TileMapsFiles/CSV_files/level2_win.csv");
            level2.sign = level2.LoadMap("C:/Users/hamno/OneDrive/Documents/GitHub/MonoGame_S1/TileMapsFiles/CSV_files/level2_Sign.csv");
            Levels.Add(level2);
        }

        public void NextLevel()
        {
            if (CurrentLevelIndex + 1 < Levels.Count)
            {
                CurrentLevelIndex++;
            }
            else
            {
                CurrentLevelIndex = 0;
            }
        }

    }



}