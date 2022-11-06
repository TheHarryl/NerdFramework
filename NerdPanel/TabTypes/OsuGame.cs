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
        Easy        = 1 << 0,
        NoFail      = 1 << 1,
        HalfTime    = 1 << 2,

        HardRock    = 1 << 3,
        SuddenDeath = 1 << 4,
        Perfect     = 1 << 5,
        DoubleTime  = 1 << 6,
        Nightcore   = 1 << 7,
        Hidden      = 1 << 8,
        Flashlight  = 1 << 9,

        Relax       = 1 << 10,
        Autopilot   = 1 << 11,
        SpunOut     = 1 << 12,
        Auto        = 1 << 13,
        Cinema      = 1 << 14,
        ScoreV2     = 1 << 15,
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

        private Beatmap _map;
        private Skin _skin;

        public Beatmap map
        {
            get => _map;
            set
            {
                _map = value;
                UpdateGlobals();
            }
        }
        public Skin skin
        {
            get => _skin;
            set
            {
                _skin = value;
                UpdateGlobals();
            }
        }
        public OsuGameMods mods;

        public double elapsedTime = 0.0;

        public double cursorSize = 1.0;
        public double hitboxSize = 1.0;

        public double baseSpeed = 1.0;
        public double speed = 1.0;

        public double hitWindow300 = 0.0;
        public double hitWindow100 = 0.0;
        public double hitWindow50 = 0.0;
        public double approachDuration = 0.0;

        public void UpdateGlobals()
        {
            if (mods.HasFlag(OsuGameMods.DoubleTime) || mods.HasFlag(OsuGameMods.Nightcore))
                speed = baseSpeed * 1.5;
            else if (mods.HasFlag(OsuGameMods.HalfTime))
                speed = baseSpeed * 0.75;
            else
                speed = baseSpeed;

            hitWindow300 = -6  * (_map.OverallDifficulty - 13.25  );
            hitWindow100 = -8  * (_map.OverallDifficulty - 17.4375);
            hitWindow50  = -10 * (_map.OverallDifficulty - 19.95  );

            approachDuration = (_map.ApproachRate >= 5.0 ?
                1200.0 - 150.0 * (_map.ApproachRate - 5.0) :
                1800.0 - 120.0 * _map.ApproachRate
            );
            if (approachDuration < 300.0)
                approachDuration = 300.0;
        }

        public OsuGame(params string[] args) : base(args)
        {
            foreach (string argument in args)
            {
                string value = argument.Substring(argument.IndexOf('=') + 1);
                if      (argument.StartsWith("map="))
                    this.map = new Beatmap("%localappdata%\\osu!\\Songs\\" + value);

                else if (argument.StartsWith("skin="))
                    this.skin = new Skin("%localappdata%\\osu!\\Skins\\" + value);

                else if (argument.StartsWith("mods="))
                    this.mods = (OsuGameMods)int.Parse(value);

                else if (argument.StartsWith("speed="))
                    this.baseSpeed = double.Parse(value);

                else if (argument.StartsWith("cursor="))
                    this.cursorSize = double.Parse(value);

                else if (argument.StartsWith("hitbox="))
                    this.hitboxSize = double.Parse(value);

                else if (argument.StartsWith("time="))
                    this.elapsedTime = double.Parse(value);
                
                else if (argument.StartsWith("hp="))
                    this._map.HPDrainRate = double.Parse(value);
                else if (argument.StartsWith("cs="))
                    this._map.CircleSize = double.Parse(value);
                else if (argument.StartsWith("od="))
                    this.map.OverallDifficulty = double.Parse(value);
                else if (argument.StartsWith("ar="))
                    this.map.ApproachRate = double.Parse(value);

                else if (argument.StartsWith("time="))
                    this.elapsedTime = double.Parse(value);
            }
            UpdateGlobals();
        }

        public override void Update(InterfaceEngine engine, double delta)
        {
            elapsedTime += delta * speed;
        }

        public override void Draw(InterfaceEngine renderer)
        {
            List<HitObject> visibleObjects = map.HitObjects.Where(h => h.time >= (elapsedTime * 1000) - hitWindow300).ToList();
            {

            }
        }
    }
}
