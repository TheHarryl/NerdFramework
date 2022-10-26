using NerdFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NerdPanel
{
    [Flags]
    public enum OsuGameMods
    {
        Easy = 1,
        NoFail = 2,
        HalfTime = 4,

        HardRock = 8,
        SuddenDeath = 16,
        Perfect = 32,
        DoubleTime = 64,
        Nightcore = 128,
        Hidden = 256,
        Flashlight = 512,

        Relax = 1024,
        Autopilot = 2048,
        SpunOut = 4096,
        Auto = 8192,
        Cinema = 16384,
        ScoreV2 = 32768,
    }

    public class OsuGame : BaseTab
    {
        public struct HitObject
        {
            public int x;
            public int y;
            public int time;
            public int type;
            public int hitSound;
            public string objectParams;
            public string hitSample;

            public HitObject(string data)
            {
                string[] args = data.Split(",");
                this.x = int.Parse(args[0]);
                this.y = int.Parse(args[1]);
                this.time = int.Parse(args[2]);
                this.type = int.Parse(args[3]);
                this.hitSound = int.Parse(args[4]);
                this.objectParams = args[5];
                this.hitSample = args[6];
            }
        }

        public class Beatmap
        {
            // [General]
            public string AudioFilename;
            public int AudioLeadIn;
            public int PreviewTime;
            public int Countdown;
            public string SampleSet;
            public double StackLeniency;
            public int Mode;
            public bool LetterboxInBreaks;
            public bool UseSkinSprites;
            public string OverlayPosition;
            public string SkinPreference;
            public bool EpilepsyWarning;
            public int CountdownOffset;
            public bool SpecialStyle;
            public bool WidescreenStoryboard;
            public bool SamplesMatchPlaybackRate;

            // [Metadata]
            public string Title;
            public string TitleUnicode;
            public string Artist;
            public string ArtistUnicode;
            public string Creator;
            public string Version;
            public string Source;
            public string[] Tags;
            public int BeatmapID;
            public int BeatmapSetID;

            // [Difficulty]
            public double HPDrainRate;
            public double CircleSize;
            public double OverallDifficulty;
            public double ApproachRate;
            public double SliderMultiplier;
            public double SliderTickRate;

            // [TimingPoints]

            // [Colours]

            // [HitObjects]
            public List<HitObject> HitObjects;

            public Beatmap(string fileLocation)
            {
                string[] lines = System.IO.File.ReadAllLines(@fileLocation);
                string section = "";

                foreach (string line in lines)
                {
                    if (line.StartsWith("["))
                    {
                        section = line;
                        continue;
                    }
                    string value = line.Substring(line.IndexOf(':') + 1);
                    while (value.StartsWith(" "))
                        value = value.Substring(1);
                    switch (section)
                    {
                        case "[General]":
                            if (line.StartsWith("AudioFilename"))
                                this.AudioFilename = value;
                            else if (line.StartsWith("AudioLeadIn"))
                                this.AudioLeadIn = int.Parse(value);
                            else if (line.StartsWith("PreviewTime"))
                                this.PreviewTime = int.Parse(value);
                            else if (line.StartsWith("Countdown"))
                                this.Countdown = int.Parse(value);
                            else if (line.StartsWith("SampleSet"))
                                this.SampleSet = value;
                            else if (line.StartsWith("StackLeniency"))
                                this.StackLeniency = double.Parse(value);
                            else if (line.StartsWith("Mode"))
                                this.Mode = int.Parse(value);
                            else if (line.StartsWith("LetterboxInBreaks"))
                                this.LetterboxInBreaks = int.Parse(value) == 1;
                            else if (line.StartsWith("UseSkinSprites"))
                                this.UseSkinSprites = int.Parse(value) == 1;
                            else if (line.StartsWith("OverlayPosition"))
                                this.OverlayPosition = value;
                            else if (line.StartsWith("SkinPreference"))
                                this.SkinPreference = value;
                            else if (line.StartsWith("EpilepsyWarning"))
                                this.EpilepsyWarning = int.Parse(value) == 1;
                            else if (line.StartsWith("CountdownOffset"))
                                this.CountdownOffset = int.Parse(value);
                            else if (line.StartsWith("SpecialStyle"))
                                this.SpecialStyle = int.Parse(value) == 1;
                            else if (line.StartsWith("WidescreenStoryboard"))
                                this.WidescreenStoryboard = int.Parse(value) == 1;
                            else if (line.StartsWith("SamplesMatchPlaybackRate"))
                                this.SamplesMatchPlaybackRate = int.Parse(value) == 1;
                            break;
                        case "[Metadata]":
                            if (line.StartsWith("Title"))
                                this.Title = value;
                            else if (line.StartsWith("TitleUnicode"))
                                this.TitleUnicode = value;
                            else if (line.StartsWith("Artist"))
                                this.Artist = value;
                            else if (line.StartsWith("ArtistUnicode"))
                                this.ArtistUnicode = value;
                            else if (line.StartsWith("Creator"))
                                this.Creator = value;
                            else if (line.StartsWith("Version"))
                                this.Version = value;
                            else if (line.StartsWith("Source"))
                                this.Source = value;
                            else if (line.StartsWith("Tags"))
                                this.Tags = value.Split(" ");
                            else if (line.StartsWith("BeatmapID"))
                                this.BeatmapID = int.Parse(value);
                            else if (line.StartsWith("BeatmapSetID"))
                                this.BeatmapSetID = int.Parse(value);
                            break;
                        case "[Difficulty]":
                            double numericalValue = double.Parse(value);
                            if (line.StartsWith("HPDrainRate"))
                                this.HPDrainRate = numericalValue;
                            else if (line.StartsWith("CircleSize"))
                                this.CircleSize = numericalValue;
                            else if (line.StartsWith("OverallDifficulty"))
                                this.OverallDifficulty = numericalValue;
                            else if (line.StartsWith("ApproachRate"))
                                this.ApproachRate = numericalValue;
                            else if (line.StartsWith("SliderMultiplier"))
                                this.SliderMultiplier = numericalValue;
                            else if (line.StartsWith("SliderTickRate"))
                                this.SliderTickRate = numericalValue;
                            break;
                        case "[Colours]":
                            break;
                        case "[HitObjects]":
                            HitObjects.Add(new HitObject(line));
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public class Skin
        {
            public Skin(string fileLocation)
            {
                string[] lines = System.IO.File.ReadAllLines(@fileLocation);

                foreach (string line in lines)
                {
                }
            }
        }

        public Beatmap map;
        public Skin skin;
        public OsuGameMods mods;

        public double elapsedTime = 0.0;

        public double cursorSize = 1.0;
        public double hitboxSize = 1.0;


        public OsuGame(string args) : base(args)
        {

        }

        public OsuGame(string beatmapFileLocation, string skinFileLocation, OsuGameMods mod) : base("")
        {
            this.map = new Beatmap(beatmapFileLocation);
            this.skin = new Skin(skinFileLocation);
        }

        public override void Update(double delta)
        {
            elapsedTime += delta;
        }

        public override void Draw(Color3[,] screen)
        {
            double speed = 1.0;
            if (mods.HasFlag(OsuGameMods.DoubleTime) || mods.HasFlag(OsuGameMods.Nightcore))
                speed = 1.5;
            else if (mods.HasFlag(OsuGameMods.HalfTime))
                speed = 0.75;

            List<HitObject> visibleObjects = map.HitObjects.Where(h => h.time >= (elapsedTime * 1000) - (10 * (map.OverallDifficulty - 19.95) / speed)).ToList();
            {

            }
        }
    }
}
