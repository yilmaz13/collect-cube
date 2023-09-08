using FullSerializer;
using GameCore;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using PrefabType = GameCore.PrefabType;

namespace EditorCode
{
    public class LevelEditorTab : EditorWindow
    {
        [MenuItem("Tools/Level Editor", false, 0)]
        private static void Init()
        {
            var window = GetWindow(typeof(LevelEditorTab));
            window.titleContent = new GUIContent("Level Editor");
        }
        private void OnEnable()
        {
            LoadResourcesData();
        }

        private void OnGUI()
        {
            Draw();
        }
        private int prevWidth = -1;
        private int prevHeight = -1;
        private float time = 30;

        private PrefabType currentColor;

        private LevelType levelType;

        private enum BrushMode
        {
            Tile,
            Row,
            Column,
            Fill
        }

        private BrushMode currentBrushMode = BrushMode.Tile;

        private readonly Dictionary<string, Texture> tileTextures = new Dictionary<string, Texture>();

        private Level currentLevel;

        private Vector2 scrollPos;

        public void LoadResourcesData()
        {
            var editorImagesPath = new DirectoryInfo(Application.dataPath + "/LevelEditor/Editor/Resources");
            var fileInfo = editorImagesPath.GetFiles("*.png", SearchOption.TopDirectoryOnly);
            foreach (var file in fileInfo)
            {
                var filename = Path.GetFileNameWithoutExtension(file.Name);
                tileTextures[filename] = Resources.Load(filename) as Texture;
            }
        }

        public void Draw()
        {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            var oldLabelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 90;

            GUILayout.Space(15);

            DrawMenu();

            if (currentLevel != null)
            {
                var level = currentLevel;
                prevWidth = level.width;

                GUILayout.Space(15);

                DrawLevelEditor();

                GUILayout.Space(100);
            }

            EditorGUIUtility.labelWidth = oldLabelWidth;
            EditorGUILayout.EndScrollView();
        }

        private void DrawMenu()
        {
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("New", GUILayout.Width(100), GUILayout.Height(50)))
            {
                currentLevel = new Level();
            }

            if (GUILayout.Button("Open", GUILayout.Width(100), GUILayout.Height(50)))
            {
                var path = EditorUtility.OpenFilePanel("Open level", Application.dataPath + "/LevelEditor/Resources/Levels", "json");
                if (!string.IsNullOrEmpty(path))
                {
                    currentLevel = LoadJsonFile<Level>(path);
                }
            }

            if (GUILayout.Button("Save", GUILayout.Width(100), GUILayout.Height(50)))
            {
                SaveLevel(Application.dataPath + "/LevelEditor/Resources");
            }

            GUILayout.EndHorizontal();
        }

        private void DrawLevelEditor()
        {
            EditorGUILayout.LabelField("Level", EditorStyles.boldLabel);
            GUILayout.BeginHorizontal(GUILayout.Width(300));
            EditorGUILayout.HelpBox("The layout settings of this level.", MessageType.Info);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("Prefab Color"), GUILayout.Width(EditorGUIUtility.labelWidth));

            currentLevel.levelType = (LevelType)EditorGUILayout.EnumPopup(currentLevel.levelType, GUILayout.Width(100));

            GUILayout.EndHorizontal();

            if (currentLevel.levelType == LevelType.Time)
            {
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(new GUIContent("Time"), GUILayout.Width(EditorGUIUtility.labelWidth));
                currentLevel.time = EditorGUILayout.FloatField(currentLevel.time, GUILayout.Width(30));
                GUILayout.EndHorizontal();
            }

            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical(GUILayout.Width(50));
            EditorGUILayout.LabelField(new GUIContent("Width"), GUILayout.Width(EditorGUIUtility.labelWidth));
            currentLevel.width = EditorGUILayout.IntField(currentLevel.width, GUILayout.Width(30));
            GUILayout.EndVertical();

            prevHeight = currentLevel.height;

            GUILayout.BeginVertical(GUILayout.Width(50));
            EditorGUILayout.LabelField(new GUIContent("Height"), GUILayout.Width(EditorGUIUtility.labelWidth));
            currentLevel.height = EditorGUILayout.IntField(currentLevel.height, GUILayout.Width(30));
            GUILayout.EndVertical();

            GUILayout.BeginVertical(GUILayout.Width(50));
            EditorGUILayout.LabelField(new GUIContent("Level ID"), GUILayout.Width(EditorGUIUtility.labelWidth));
            currentLevel.id = EditorGUILayout.IntField(currentLevel.id, GUILayout.Width(30));
            GUILayout.EndVertical();

            GUILayout.BeginVertical(GUILayout.Width(50));
            EditorGUILayout.LabelField(new GUIContent("Index"), GUILayout.Width(EditorGUIUtility.labelWidth));
            currentLevel.index = EditorGUILayout.IntField(currentLevel.index, GUILayout.Width(30));
            GUILayout.EndVertical();

            GUILayout.BeginVertical(GUILayout.Width(50));
            EditorGUILayout.LabelField(new GUIContent("One Cube Score"), GUILayout.Width(EditorGUIUtility.labelWidth));
            currentLevel.oneCubeScore = EditorGUILayout.IntField(currentLevel.oneCubeScore, GUILayout.Width(30));
            GUILayout.EndVertical();

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(GUILayout.Width(50));

            if (GUILayout.Button("Last", GUILayout.Width(50), GUILayout.Height(20)))
            {
                currentLevel.currentIndex--;
                if (currentLevel.currentIndex < 0)
                {
                    currentLevel.currentIndex = currentLevel.index - 1;
                }
            }

            if (GUILayout.Button("Next", GUILayout.Width(50), GUILayout.Height(20)))
            {
                currentLevel.currentIndex++;
                if (currentLevel.currentIndex > currentLevel.index - 1)
                {
                    currentLevel.currentIndex = 0;
                }
            }
            GUILayout.Button("Current Index: " + currentLevel.currentIndex, GUILayout.Width(100), GUILayout.Height(20));

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(new GUIContent("Prefab Color"), GUILayout.Width(EditorGUIUtility.labelWidth));
            currentColor = (PrefabType)EditorGUILayout.EnumPopup(currentColor, GUILayout.Width(100));
            GUILayout.EndHorizontal();


            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(new GUIContent("Brush mode", "The current brush mode."),
                GUILayout.Width(EditorGUIUtility.labelWidth));
            currentBrushMode = (BrushMode)EditorGUILayout.EnumPopup(currentBrushMode, GUILayout.Width(100));
            GUILayout.EndHorizontal();

            GUILayout.Space(10);


            if (prevWidth != currentLevel.width || prevHeight != currentLevel.height)
            {
                currentLevel.tiles = new List<Block>(currentLevel.width * currentLevel.height);
                // currentLevel.blockList = new List<BlockList>(currentLevel.index);
                //  currentLevel.blockList = new List<BlockList>(2);
                //  for (int index = 0; index < 2; index++)
                //  {
                // var currentblockList = currentLevel.blockList[index];
                for (var i = 0; i < currentLevel.width; i++)
                {
                    for (var j = 0; j < currentLevel.height; j++)
                    {
                        //currentLevel.tiles.Add(new Block() { color = PrefabType.Random });                       
                    }
                }
                //  }
            }

            if (prevWidth != currentLevel.width || prevHeight != currentLevel.height)
            {
                //   currentLevel.blockList = new List<BlockList>(currentLevel.index);
                currentLevel.blockList = new List<BlockList>();
                for (int index = 0; index < currentLevel.index; index++)
                {
                    currentLevel.blockList.Add(new BlockList());
                    var currentblockList = currentLevel.blockList[index];
                    for (var i = 0; i < currentLevel.width; i++)
                    {
                        for (var j = 0; j < currentLevel.height; j++)
                        {
                            currentblockList.blocks.Add(new Block() { color = PrefabType.Random });
                            //  Debug.Log("Test");
                        }
                    }
                }
            }


            for (var i = 0; i < currentLevel.height; i++)
            {
                GUILayout.BeginHorizontal();
                for (var j = 0; j < currentLevel.width; j++)
                {
                    var tileIndex = (currentLevel.width * i) + j;
                    CreateButton(tileIndex);
                }
                GUILayout.EndHorizontal();
            }
        }


        private void CreateButton(int tileIndex)
        {
            var tileTypeName = string.Empty;
            var currentblockList = currentLevel.blockList[currentLevel.currentIndex];
            if (currentblockList.blocks[tileIndex] != null)
            {
                if (currentblockList.blocks[tileIndex] is Block)
                {
                    var blockTile = (Block)currentblockList.blocks[tileIndex];
                    tileTypeName = blockTile.color.ToString();
                }

            }

            if (tileTextures.ContainsKey(tileTypeName))
            {
                if (GUILayout.Button(tileTextures[tileTypeName], GUILayout.Width(60), GUILayout.Height(60)))
                {
                    DrawTile(tileIndex);
                }
            }
            else
            {
                if (GUILayout.Button("", GUILayout.Width(60), GUILayout.Height(60)))
                {
                    DrawTile(tileIndex);
                }
            }
        }

        private void DrawTile(int tileIndex)
        {
            var x = tileIndex % currentLevel.width;
            var y = tileIndex / currentLevel.width;
            var currentblockList = currentLevel.blockList[currentLevel.currentIndex];
            switch (currentBrushMode)
            {
                case BrushMode.Tile:
                    currentblockList.blocks[tileIndex] = new Block { color = currentColor }; break;

                case BrushMode.Row:
                    for (var i = 0; i < currentLevel.width; i++)
                    {
                        var idx = i + (y * currentLevel.width);
                        currentblockList.blocks[idx] = new Block { color = currentColor };
                    }
                    break;

                case BrushMode.Column:
                    for (var j = 0; j < currentLevel.height; j++)
                    {
                        var idx = x + (j * currentLevel.width);
                        currentblockList.blocks[idx] = new Block { color = currentColor };
                    }
                    break;

                case BrushMode.Fill:
                    for (var j = 0; j < currentLevel.height; j++)
                    {
                        for (var i = 0; i < currentLevel.width; i++)
                        {
                            var idx = i + (j * currentLevel.width);
                            currentblockList.blocks[idx] = new Block { color = currentColor };
                        }
                    }
                    break;
            }
        }

        public void SaveLevel(string path)
        {
#if UNITY_EDITOR
            SaveJsonFile(path + "/Levels/" + currentLevel.id + ".json", currentLevel);
            AssetDatabase.Refresh();
#endif
        }
        protected T LoadJsonFile<T>(string path) where T : class
        {
            if (!File.Exists(path))
            {
                return null;
            }

            var file = new StreamReader(path);
            var fileContents = file.ReadToEnd();
            var data = fsJsonParser.Parse(fileContents);
            object deserialized = null;
            var serializer = new fsSerializer();
            serializer.TryDeserialize(data, typeof(T), ref deserialized).AssertSuccessWithoutWarnings();
            file.Close();
            return deserialized as T;
        }

        protected void SaveJsonFile<T>(string path, T data) where T : class
        {
            fsData serializedData;
            var serializer = new fsSerializer();
            serializer.TrySerialize(data, out serializedData).AssertSuccessWithoutWarnings();
            var file = new StreamWriter(path);
            var json = fsJsonPrinter.PrettyJson(serializedData);
            file.WriteLine(json);
            file.Close();
        }
    }
}
