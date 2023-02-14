using MonoGame.Extended;
using IniParser;
using IniParser.Model;
using static MyGame.Component.GamePhysicsManager;

namespace MyGame.Component
{
    public class GameConfig
    {
        private static GameConfig self;
        public bool DebugMode { get; set; }
        //地图瓦片
        public int TileWidth { get; set; }
        public int TileHeight { get; set; }
        public int TileColumns { get; set; }
        public int TileRows { get; set; }
        //玩家
        public int PlayerWidth { get; set; }
        public int PlayerHeight { get; set; }
        //活动区域大小
        public RectangleF PlayerActivityArea { get; set; }
        //视野区域大小
        public RectangleF PlayerViewArea { get; set; }
        //遮罩可视区域大小
        public bool ShaderEnabled { get; set; }
        public RectangleF ShaderViewArea { get; set; }
        //动画
        public float AnimationDuration { get; set; }
        //碰撞
        public CollisionAlgorithm CollisionAlgorithm { get;set;}
        public float PnetrationMultiplier { get; set; }
        //力度
        public int ForceUpwardMultiplier { get; set; }
        public int ForceRightwardMultiplier { get; set; }
        public int ForceDownwardMultiplier { get; set; }
        public int ForceLeftwardMultiplier { get; set; }
        //重力
        public float Gravity { get; set; }
        public static GameConfig Create()
        {
            if (self == null) 
            {
                FileIniDataParser parser = new();
                IniData setting = parser.ReadFile("Game1.ini");
                self = new GameConfig
                {
                    DebugMode = bool.Parse(setting["DEBUG"]["Enabled"]),
                    TileWidth = int.Parse(setting["TILE"]["Width"]),
                    TileHeight = int.Parse(setting["TILE"]["Height"]),
                    TileColumns = int.Parse(setting["TILE"]["Columns"]),
                    TileRows = int.Parse(setting["TILE"]["Rows"]),
                    PlayerWidth = int.Parse(setting["PLAYER"]["Width"]),
                    PlayerHeight = int.Parse(setting["PLAYER"]["Height"]),
                    PlayerActivityArea = new(0, 0, int.Parse(setting["PLAYER"]["ActivityWidth"]), int.Parse(setting["PLAYER"]["ActivityHeight"])),
                    PlayerViewArea = new(-(int.Parse(setting["PLAYER"]["ViewWidth"]) - int.Parse(setting["PLAYER"]["Width"])) / 2, -(int.Parse(setting["PLAYER"]["ViewHeight"]) - int.Parse(setting["PLAYER"]["Height"])) / 2, int.Parse(setting["PLAYER"]["ViewWidth"]), int.Parse(setting["PLAYER"]["ViewHeight"])),
                    ShaderEnabled = bool.Parse(setting["SHADER"]["Enabled"]),
                    ShaderViewArea = new(0, 0, int.Parse(setting["SHADER"]["ViewWidth"]), int.Parse(setting["SHADER"]["ViewHeight"])),
                    AnimationDuration = float.Parse(setting["ANIMATION"]["Duration"]),
                    PnetrationMultiplier = float.Parse(setting["PHYSICS"]["PnetrationMultiplier"]),
                    ForceUpwardMultiplier = int.Parse(setting["PHYSICS"]["UpwardMultiplier"]),
                    ForceRightwardMultiplier = int.Parse(setting["PHYSICS"]["RightwardMultiplier"]),
                    ForceDownwardMultiplier = int.Parse(setting["PHYSICS"]["DownwardMultiplier"]),
                    ForceLeftwardMultiplier = int.Parse(setting["PHYSICS"]["LeftwardMultiplier"]),
                    Gravity = float.Parse(setting["PHYSICS"]["Gravity"]),
                    CollisionAlgorithm = setting["PHYSICS"]["CollisionAlgorithm"] == "EdgeLeftRowScanMergeRectangle" ? CollisionAlgorithm.EdgeLeftRowScanMergeRectangle : CollisionAlgorithm.EdgePointRectangle
                };
            }
            return self;
        }
    }
}
