using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
 
[ExecuteInEditMode]
public class TileMaster : EditorWindow {
	static Texture[] cmTileSets;
	static Sprite[] cmSprites;
	static Sprite[] cmCurSprites;
	static int cmSelectedTileSet = 0;
	static List<int> cmSelectedTile = new List<int>();
	static GameObject cmquad;
	static Texture2D cmSelectedColor;
	static Vector2 tileScrollPosition = Vector2.zero;
	static List<Sprite> cmCurSprite = new List<Sprite>();
	static int curTool = 0;
	static int curMode = 0;
	static GameObject curLayer;
	static int selectedLayer = 0;
	static Vector3 cmCurPos;
	static List<Transform> layers = new List<Transform>();
	static bool highlightLayer = false;
    static Vector2 drawBox;
	static bool makeCollider = false;
	//static bool toggleAdvanced = false; //Added it for features later down the line possibly.
	static EditorWindow window;
	static int renameId = -1;
	static Texture2D texVisible;
	static Texture2D texHidden;
	static Color highlightColor = Color.red;
	static Event e;
	static int gridSizeX = 32;
	static int gridSizeY = 32;
	static int padSizeX = 1;
	static int padSizeY = 1;

	[MenuItem("Window/CM Tools/Tile Master")]
	public static void OnEnable() 
	{
		//Reset variables chunk. This is for new files being added, generated, etc.
		AssetDatabase.Refresh();
		cmTileSets = new Texture[0];
		cmSprites = new Sprite[0];
		layers.Clear();
		
		SceneView.onSceneGUIDelegate += OnSceneGUI; //Sets delegate for adding the OnSceneGUI event
		

		cmTileSets = Resources.LoadAll<Texture>("Tilesets"); //Load all tilesets as texture
		cmSprites = Resources.LoadAll<Sprite>("Tilesets"); //Load all tileset sub objects as tiles
		texVisible = Resources.Load("Editor/CM Tools/TileMaster/Visible") as Texture2D; //Load visible icon
		texHidden = Resources.Load("Editor/CM Tools/TileMaster/Hidden") as Texture2D; //Load hidden icon

		LoadTileset(0);//processes loaded tiles into proper tilesets
		 
		cmSelectedColor = new Texture2D(1,1); //makes highlight color for selecting tiles
		cmSelectedColor.alphaIsTransparency = true;
		cmSelectedColor.filterMode = FilterMode.Point;
		cmSelectedColor.SetPixel(0,0, new Color(.5f,.5f,1f,.5f));
		cmSelectedColor.Apply();

		window = EditorWindow.GetWindow(typeof(TileMaster));//Initialize window
		window.minSize = new Vector2(325,400);
	}

	static void AddLayer()
	{
		//Creates new layer, renames it, and sets it's proper layer settings
		layers.Add(new GameObject().transform);
		int index = layers.Count-1;
		layers[index].name = "New Layer";
		layers[index].transform.parent = cmquad.transform;
		SpriteRenderer tmpRenderer = layers[index].gameObject.AddComponent<SpriteRenderer>();
		tmpRenderer.sortingOrder = index;
	}
	
	static void ResetLayers()
	{
		layers.Clear(); //Rebuilds a list of all of the layers.
		
		foreach( Transform t in cmquad.GetComponentsInChildren(typeof(Transform),true))
		{
			//gets a list of all possible layers, and checks to see if the parent is the main game object for the tiles.
			if(t.parent == cmquad.transform)
			{
				layers.Add(t);
			}
		}
	}

	static void ResetManager()
	{
		//Intended to create objects required for the tileset editor to work
		if(cmquad == null)
		{
			cmquad = GameObject.Find("TileMasterField"); //Look for my tileset object. If it doesn't exist, create a very large quad.
			if(cmquad == null)
			{
				cmquad = GameObject.CreatePrimitive(PrimitiveType.Quad);
				cmquad.name = "TileMasterField";
				cmquad.transform.localScale = new Vector3 (1000000,1000000,1000000);
				AddLayer();
				ResetLayers();
				curLayer = layers[0].gameObject;
			}
			
			cmquad.GetComponent<Renderer>().enabled = false; //disable quad's renderer
			
			EditorUtility.SetSelectedWireframeHidden(cmquad.GetComponent<Renderer>(), true); //hide wireframe from wireframe view mode
		}
		if(window != null)
		{
			//force repaint to show proper layers if the window exists.
			window.Repaint();
		}
	}
	
	static void LoadTileset(int tileSetID)
	{
		//loads the tilesets into proper variables
		ResetManager();

		cmCurSprites = new Sprite[cmSprites.Length];
		
		int curCount = 0;
		int i = 0;

		//sets the displayed tileset based on the name of the tile. Also counts the number of tiles in the new tileset that's loaded.
		for(i =0; i < cmSprites.Length; i++)
		{
			if(cmSprites[i].texture.name == cmTileSets[tileSetID].name)
			{
				cmCurSprites[curCount] = cmSprites[i];
				curCount++;
			}
		}

		//resizes the displayed tileset's array size to match the new size
		Sprite[] tmpSprite = new Sprite[curCount];
		for(i = 0; i < curCount; i++)
		{
			tmpSprite[i] = cmCurSprites[i];
		}
		cmCurSprites = tmpSprite;
	}
	

	void OnGUI()
	{
		int i, j;
		ResetManager();//Remakes game objects require to operate.
		e = Event.current; //Gets current event (mouse move, repaint, keyboard press, etc)

		if(renameId != -1 && (e.keyCode == KeyCode.Return || e.keyCode == KeyCode.KeypadEnter))
		{
			renameId = -1;
		}

		if(cmTileSets == null) //Check to make certain there is actually a tileset in the resources/tileset folder.
		{
			EditorGUILayout.LabelField("No tilesets found. Retrying.");
			OnEnable();
		}else{
			string[] names = new string[cmTileSets.Length]; //Gets the name of the tilesets into a useable list
			for(i = 0; i < cmTileSets.Length; i++)
			{
				try
				{
					names[i] = cmTileSets[i].name;
				}
				catch(System.Exception ex)
				{
					Debug.Log ("There was an error getting the names of the files. We'll try to reload the tilesets. If this continues to show, please close the script and try remimporting and check your images.");
					Debug.Log ("Full system error: " + ex.Message);
					OnEnable();
				}
			} 

			//Mode variable to swith between major features.
			string[] mode = {"Tile Painter", "Help Video"};//, "Pad Tileset"};// Pad tileset not finished yet, removed to allow for earlier release. You can try it out if you want, but is has issues with larger images and places tiles in the wrong order.
			curMode = GUILayout.Toolbar(curMode, mode);

			if(curMode == 0)
			{
				//If in standard paint mode, display the proper gui elements for the user to use.
				EditorGUILayout.BeginHorizontal();
				int tmpInt = EditorGUILayout.Popup("Tileset", cmSelectedTileSet, names);
				if(GUILayout.Button("Reload"))
				{
					OnEnable();
				}
				EditorGUILayout.EndHorizontal();

				string[] tools = {"Paint", "Erase", "Box Paint"};
				//curTool = EditorGUILayout.Popup("Tool", curTool, tools);

				EditorGUILayout.BeginHorizontal(GUILayout.Width(position.width)); 
				//Causes an error on editor load if the window is visible.
				//This seems to be a problem with how the gui is drawn the first
				//loop of the script. It only happens the once, and I can't figure
				//out why. I've been trying for literally weeks and still can't
				//find an answer. This is the only known bug, but it doesn't
				//stop the functionality of the script in any way, and only serves
				//as an annoying message on unity load or this script being 
				//recompiled. Sorry for this bug. I am looking for a way to remove
				//this error, but I really am stummped as to why it's happening
				//and I just can not find an answer online.


				EditorGUILayout.LabelField("Tool",GUILayout.Width(50));
				GUILayout.FlexibleSpace();
				curTool = GUILayout.Toolbar(curTool, tools);
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Paint With Collider",GUILayout.Width(150));
				makeCollider = EditorGUILayout.Toggle(makeCollider);
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Highlight Current Layer",GUILayout.Width(150));
				highlightLayer = EditorGUILayout.Toggle(highlightLayer, GUILayout.Width(25));
				highlightColor = EditorGUILayout.ColorField(highlightColor);
				EditorGUILayout.EndHorizontal();

				if(tmpInt != cmSelectedTileSet) //Forces selection of first tileset if none are selected.
				{
					LoadTileset(tmpInt);
				}
				
				cmSelectedTileSet = tmpInt; //sets the selected tileset value

				i = 0;
			int columnCount = Mathf.RoundToInt((position.width)/38)-2; //figures out how many columns are required for the tileset
				j = 0;
				int current = 0;

			tileScrollPosition = EditorGUILayout.BeginScrollView(tileScrollPosition,false,true,GUILayout.Width(position.width));
				//creates scrollbox area for tiles inside of the current tileset.

				GUILayout.BeginHorizontal(); //Initializing first row

				for(int q = 0; q < cmSprites.Length; q++)
				{
					Sprite child = cmSprites[q];
					//for every tile inside the currently selected tileset, add a tile
					try
					{
					if(child.texture.name == names[cmSelectedTileSet] && child.name != names[cmSelectedTileSet])
					{
						//if it's the tiles inside the image, not the entire image

						Rect newRect = new Rect(
											child.rect.x/child.texture.width, 
											child.rect.y/child.texture.height,
											child.rect.width/child.texture.width,
											child.rect.height/child.texture.height
										   );//gets the x and y position in pixels of where the image is. Used later for display.

						if(GUILayout.Button("", GUILayout.Width(34), GUILayout.Height(34)))
						{
							//draw a clickable button
							if (cmSelectedTile != null && !e.control)
							{
								//empty the selected tile list if control isn't held. Allows multiselect of tiles.
								cmSelectedTile.Clear();
								cmCurSprite.Clear();
							}
							cmSelectedTile.Add(current); //Adds clicked on tile to list of selected tiles.
							cmCurSprite.Add(cmCurSprites[current]);
						}

						GUI.DrawTextureWithTexCoords(new Rect(5+(j*38), 4+(i*37), 32, 32), child.texture, newRect ,true); //draws tile base on pixels gotten at the beginning of the loop
						if(cmSelectedTile != null && cmSelectedTile.Contains(current))
						{
							//if the current tile is inside of the list of selected tiles, draw a highlight indicator over the button.
							if(cmSelectedColor == null)
							{
								cmSelectedColor = new Texture2D(1,1);
								cmSelectedColor.alphaIsTransparency = true;
								cmSelectedColor.filterMode = FilterMode.Point;
								cmSelectedColor.SetPixel(0,0, new Color(.5f,.5f,1f,.5f));
								cmSelectedColor.Apply();
							}
							GUI.DrawTexture(new Rect(5+(j*38), 4+(i*37), 32, 32), cmSelectedColor,ScaleMode.ScaleToFit,true);
						}

						if(j < columnCount)
						{
							j++;
						}else{
							// if we have enough columns to fill the scroll area, reset the column count and start a new line of buttons
							j = 0;
							i++;
							GUILayout.EndHorizontal();
							GUILayout.BeginHorizontal();
						}
						current++;
					}
					}catch(System.Exception ex){
						if(ex.Message.StartsWith("IndexOutOfRangeException"))
						{
							Debug.Log("Tileset index was out of bounds, reloading and trying again.");
							OnEnable();
							return;
						}
					}
				}
				GUILayout.EndHorizontal(); //finish the drawing of tiles
				EditorGUILayout.EndScrollView();
				//Display all of the layers. May be put into a foldout for if there are too many layers. Haven't decided yet.
				GUILayout.Label("Layers:");

				if(GUILayout.Button("Add Layer"))
				{
					AddLayer();
				}
				String[] minusPlus = {"-", "+", "x", "r"};
				
				
				ResetLayers();
				layers = ResortLayers(layers);//Sort the layers based on their sorting order instead of name
				int destroyFlag = -1;
				for(i = 0; i < layers.Count; i++)
				{
					//iterates through layers and displays gui for options.
					EditorGUILayout.BeginHorizontal();
					
					RectOffset tmpPadding = GUI.skin.button.padding;
					GUI.skin.button.padding = new RectOffset(3,3,3,3);
					
					if(layers[i].gameObject.activeSelf)
					{
						if(GUILayout.Button(texVisible,GUILayout.Width(15), GUILayout.Height(15)))
						{
							layers[i].gameObject.SetActive(false);
						}
					}else{
						if(GUILayout.Button(texHidden,GUILayout.Width(15), GUILayout.Height(15)))
						{
							layers[i].gameObject.SetActive(true);
						}
					}
					GUI.skin.button.padding = tmpPadding;
					
					if(i == selectedLayer)
					{
						//if selected layer, draw checked checkbox to show it's selected
						if(i != renameId)
						{	
							EditorGUILayout.ToggleLeft(layers[i].name + " - " + layers[i].GetComponent<SpriteRenderer>().sortingOrder,true);
						}else{
							layers[i].name = EditorGUILayout.TextField(layers[i].name);
						}
					}else{
						//if not the selected layer, and is clicked, set it as the selected layer
						if(i != renameId)
						{	
							if(EditorGUILayout.ToggleLeft(layers[i].name + " - " + layers[i].GetComponent<SpriteRenderer>().sortingOrder,false))
							{
								selectedLayer = i;
							}
						}else{
							layers[i].name = EditorGUILayout.TextField(layers[i].name);
						}
					}
					
					//sets pressed value to -1 if nothing is pressed.
					int pressed = GUILayout.Toolbar(-1, minusPlus);
					
					switch(pressed)
					{
					case 0:
						if(i > 0)
						{
							//moves layer and all tiles in it to move away from the camera, and moves the layer above it toward the camera.
							layers[i-1].GetComponent<SpriteRenderer>().sortingOrder += 1;
							int upLayer = layers[i-1].GetComponent<SpriteRenderer>().sortingOrder;
							
							foreach(SpriteRenderer sr in layers[i-1].GetComponentsInChildren<SpriteRenderer>())
							{
								sr.sortingOrder = upLayer;
							}
							
							//layers[i].GetComponent<SpriteRenderer>().sortingOrder -= 1;
							int downLayer = layers[i].GetComponent<SpriteRenderer>().sortingOrder -= 1;
							
							foreach(SpriteRenderer sr in layers[i].GetComponentsInChildren<SpriteRenderer>())
							{
								sr.sortingOrder = downLayer;
							}
							selectedLayer = i-1;
						}
						layers = ResortLayers(layers);
						break;
					case 1:
						if(i < layers.Count-1)
						{
							//moves layer and all tiles in it to move toward the camera, and moves the layer above it away from the camera.
							layers[i+1].GetComponent<SpriteRenderer>().sortingOrder -= 1;
							int upLayer = layers[i+1].GetComponent<SpriteRenderer>().sortingOrder;
							
							foreach(SpriteRenderer sr in layers[i+1].GetComponentsInChildren<SpriteRenderer>())
							{
								sr.sortingOrder = upLayer;
							}
							
							//layers[i].GetComponent<SpriteRenderer>().sortingOrder += 1;
							int downLayer = layers[i].GetComponent<SpriteRenderer>().sortingOrder += 1;
							
							foreach(SpriteRenderer sr in layers[i].GetComponentsInChildren<SpriteRenderer>())
							{
								sr.sortingOrder = downLayer;
							}
							selectedLayer = i+1;
						}
						layers = ResortLayers(layers);
						break;
					case 2:
						//deletes the layer game object, which also deletes all the children
						destroyFlag = i;
						break;
					case 3:
						if(renameId == -1)
						{
							renameId = i;
						}else{
							renameId = -1;
						}
						break;
					default:
						//do nothing if a button wasn't pressed (required or I get errors T_T)
						break;
					}
					EditorGUILayout.EndHorizontal(); //end the layer gui
				}
				if(selectedLayer <= layers.Count-1 && selectedLayer > 0)
				{
					//double check to make certain a layer of some sort is selected and is in valid range
					curLayer = layers[selectedLayer].gameObject;
				}

				if(selectedLayer <= layers.Count-1 && layers[selectedLayer] != null)
				{
					ResetHighlight(layers[selectedLayer].gameObject, highlightLayer);
					curLayer = layers[selectedLayer].gameObject;
				}else{
					if(layers.Count-1 > 0 && layers[selectedLayer] != null)
					{
						curLayer = layers[selectedLayer].gameObject;
					}else{
						
					}
				}
				if(destroyFlag != -1)
				{
					DestroyImmediate(layers[destroyFlag].gameObject);
					return; //Breaks method to not have errors down the line. Forces reload of tilesets to keep inside the bounds of the array.
				}
				destroyFlag = -1;
			}else if(curMode == 1){
				curMode = 0;
				Application.OpenURL("https://www.youtube.com/watch?v=mxy9HdNM-is");
				return;
			}else if(curMode == 2){
				int tmpInt = EditorGUILayout.Popup("Tileset", cmSelectedTileSet, names);
				if(tmpInt != cmSelectedTileSet) //Forces selection of first tileset if none are selected.
				{
					LoadTileset(tmpInt);
				}
				
				cmSelectedTileSet = tmpInt; //sets the selected tileset value

				GUILayout.BeginHorizontal();
				GUILayout.Label("Grid Size X", GUILayout.Width(200));
				gridSizeX = EditorGUILayout.IntField(gridSizeX);
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				GUILayout.Label("Grid Size Y", GUILayout.Width(200));
				gridSizeY = EditorGUILayout.IntField(gridSizeY);
				GUILayout.EndHorizontal();

				/*GUILayout.BeginHorizontal();
				GUILayout.Label("Pad Size X", GUILayout.Width(200));
				padSizeX = EditorGUILayout.IntField(padSizeX);
				GUILayout.EndHorizontal();
				
				GUILayout.BeginHorizontal();
				GUILayout.Label("Pad Size Y", GUILayout.Width(200));
				padSizeY = EditorGUILayout.IntField(padSizeY);
				GUILayout.EndHorizontal();*/


				if(GUILayout.Button("Generate New Texture"))
				{
					List<Rect> listOfNewSlices = new List<Rect>();
					Texture2D curTileSet = Resources.Load<Texture2D>("Tilesets/" + cmTileSets[cmSelectedTileSet].name);//cmTileSets[cmSelectedTileSet];
					int newWidth = (int)(curTileSet.width+(padSizeX*2*(curTileSet.width/gridSizeX)));
					int newHeight = (int)(curTileSet.height+(((curTileSet.height/gridSizeY))*(padSizeY*2)));

					Texture2D newTileSet = new Texture2D(newWidth,newHeight);

					Debug.Log("Generated new tile image with a size of " + newTileSet.width + ", " + newTileSet.height);
//					Debug.Log("Tilecount: " + (int)(curTileSet.width/gridSizeX) + ", " + (int)(curTileSet.height/gridSizeY));
					for(j = 0; j < (int)(curTileSet.width/gridSizeX); j++)
					{
						for(i = 0 ; i < (int)(curTileSet.height/gridSizeY); i++)
						{
							//Copies old image tiles to new image with padding.
							try{
							newTileSet.SetPixels(
								((j*gridSizeX)+(j*padSizeX*2))+padSizeX,
								((i*gridSizeY)+(i*padSizeY*2))+padSizeY,
								gridSizeX, 
								gridSizeY,
								curTileSet.GetPixels(
									j*gridSizeX,
									i*gridSizeY, 
									gridSizeX,
									gridSizeY));
								Debug.Log(i*32);

							//LeftSide
							/*newTileSet.SetPixels(
								(j*gridSizeX)+(j*padSizeX*2)+padSizeX-1,
								(i*gridSizeY)+(i*padSizeY*2),
								1,
								gridSizeY,
								curTileSet.GetPixels(
								j*gridSizeX,
								i*gridSizeY,
								1,
								gridSizeY));

							//RightSide
							newTileSet.SetPixels(
								(j*gridSizeX)+(j*padSizeX*2)+padSizeX + gridSizeX,
								(i*gridSizeY)+(i*padSizeY*2),
								1,
								gridSizeY,
								curTileSet.GetPixels(
								(j*gridSizeX)+gridSizeX-1,
								i*gridSizeY,
								1,
								gridSizeY));

							//BottomSide
							newTileSet.SetPixels(
								(j*gridSizeX)+(j*padSizeX*2)+padSizeX,
								(i*gridSizeY)+(i*padSizeY*2)-1,
								gridSizeX,
								1,
								curTileSet.GetPixels(
								j*gridSizeX,
								i*gridSizeY,
								gridSizeX,
								1));

							//TopSide
							newTileSet.SetPixels(
								(j*gridSizeX)+(j*padSizeX*2)+padSizeX,
								(i*gridSizeY)+(i*padSizeY*2)+gridSizeY,
								gridSizeX,
								1,
								curTileSet.GetPixels(
								(j*gridSizeX),
								(i*gridSizeY)+gridSizeY-1,
								gridSizeX,
								1));*/
								listOfNewSlices.Add(new Rect(
															((i*gridSizeY)+(i*padSizeY*2))+padSizeY,
															((j*gridSizeX)+(j*padSizeX*2))+padSizeX,
								                            gridSizeX, 
								                            gridSizeY));

							//Debug.Log("Drawing tile " + i + ", " + j + " at " + ((i*padSizeX)+(i*gridSizeX)) + ", " + ((j*padSizeY)+(j*gridSizeY)) + " from " + i*gridSizeX + ", " + j*gridSizeY);
							}catch(System.Exception ex)
							{
								Debug.Log("ERROR: " + ex.Message);
								if(0==0)
								{

								}
							}
						}
					} 
					newTileSet.Apply();
					//listOfNewSlices.Add(new Rect(0,newTileSet.height-gridSizeX,100,100));
					Debug.Log("Image generation completed, generating slices.");
					
					/*FileStream fs = new FileStream(Application.dataPath + "/Resources/Tilesets/" + curTileSet.name + "_padded.png", FileMode.Append);
					BinaryWriter bw = new BinaryWriter(fs);
					bw.Write(newTileSet.EncodeToPNG());
					bw.Close();
					fs.Close();*/
					
					//AssetDatabase.CreateAsset(newTileSet.EncodeToPNG(), "Assets/Resources/Tilesets/" + curTileSet.name + "_padded.png");
					bool isWriting = true;
					while(isWriting)
					{
						File.WriteAllBytes(Application.dataPath + "/Resources/Tilesets/" + curTileSet.name + "_padded.png", newTileSet.EncodeToPNG());
						isWriting = false;
					}
					AssetDatabase.Refresh();
					TextureImporter ti = new TextureImporter();
					ti = (TextureImporter)AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(Resources.Load<Texture>("Tilesets/" + curTileSet.name + "_padded")));
					TextureImporterType type = new TextureImporterType();
					type = TextureImporterType.Sprite;
					ti.textureType = type;
					ti.spritePixelsToUnits = gridSizeX;
					ti.spriteImportMode = SpriteImportMode.Multiple;
					ti.filterMode = FilterMode.Point;
										
					List<SpriteMetaData> spriteData = new List<SpriteMetaData>();//[listOfNewSlices.Count+1];
					//listOfNewSlices.Reverse();

					for(i = 0; i < listOfNewSlices.Count; i++)
					{
						float alpha = 0;
						foreach(Color pixel in newTileSet.GetPixels((int)listOfNewSlices[i].x, (int)listOfNewSlices[i].y, (int)listOfNewSlices[i].width, (int)listOfNewSlices[i].height))
						{
							alpha += pixel.a;
						}
						if(alpha > 0)
						{
							listOfNewSlices[i] = new Rect(listOfNewSlices[i].x, listOfNewSlices[i].y, listOfNewSlices[i].width, listOfNewSlices[i].height);
							SpriteMetaData tmpSpriteData = new SpriteMetaData();
							tmpSpriteData.rect = listOfNewSlices[i];
							tmpSpriteData.name = curTileSet.name + "_padded_" + i;// ti.spritesheet.GetLength(0);
							tmpSpriteData.alignment = 0;
							tmpSpriteData.pivot = new Vector2(gridSizeX/2,gridSizeY/2);
							spriteData.Add(tmpSpriteData);
						}else{
							listOfNewSlices.RemoveAt(i);
							//spriteData.RemoveAt(i);
							i--;
						}
					}

					ti.spritesheet = spriteData.ToArray();

					Debug.Log("Finished generating new padded tileset. Pausing thread to update file.");
					System.Threading.Thread.Sleep(4000);//Added to allow for saving and reimporting image in unity. Required to run without error.
					OnEnable();
				}
				if(GUILayout.Button("Regenerate to Original File"))
				{

				}
			}
		}
	}

	static bool IsFileInUse(string filePath)
	{
		try
		{
			//Try to open or create the file
			using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
			{
				//We can check whether the file can be read or written by using fs.CanRead or fs.CanWrite here.
			}
			return false;
		}
		catch (IOException ex)
		{
			//check if the message is about file io.
			string message = ex.Message.ToString();
			//Check if the file is in use
			if (message.Contains("The process cannot access the file"))
				return true;
			else
				throw;
		}
	}
	
	static private List<Transform> ResortLayers(List<Transform> layers)
	{
		//sorts layers based on the sorting order
		layers.Sort(delegate(Transform x, Transform y)
		{
			int sortOrderX = x.GetComponent<SpriteRenderer>().sortingOrder;
			int sortOrderY = y.GetComponent<SpriteRenderer>().sortingOrder;
			return sortOrderX.CompareTo(sortOrderY);
		});
		return layers;
	}

	private void ResetHighlight(GameObject layer, bool status)
	{
		//highlights the layer in red if status is true, unhighlights if false
		foreach(SpriteRenderer sr in cmquad.GetComponentsInChildren<SpriteRenderer>())
		{
			sr.color = new Color(1,1,1,1);
		}
		foreach(SpriteRenderer sr in layers[selectedLayer].GetComponentsInChildren<SpriteRenderer>())
		{
			if(status)
			{
				sr.color = highlightColor;
			}else{
				sr.color = new Color(1,1,1,1);
			}
		}
	}

	private void OnSelectionChange()
	{
		
		 //left over code for selecting a layer if selected in the heiarchy. Left in for if I want to do it again and need reference. Probably doesn't work atm.
		//if(Selection.activeObject != lastSelectedObject && Selection.activeObject != null)
		//{
//			if(Selection.activeTransform != null)
			//{
//				if(Selection.activeTransform.parent.name.StartsWith("TileMasterField"))
				//{
//					string[] tmpStr = Selection.activeObject.name.Split('r');
					//lastSelectedObject = Selection.activeObject;
				//}
			//}
		//}
		Repaint();
	}

	
	static void OnSceneGUI( SceneView sceneview )
    {
		int i, j;
		if(cmquad != null)
		{
			if(cmquad.transform.childCount <= 0)
			{
				//double checks there is at least 1 layer inside of cmquad.
				AddLayer();
				ResetLayers();
			}

			if (Event.current.type == EventType.layout)
			{
				HandleUtility.AddDefaultControl(
					GUIUtility.GetControlID(
					window.GetHashCode(),
				    FocusType.Passive));
			}

			e = Event.current;

	        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
	        RaycastHit hit;

	        if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity))
	        {
				//Draw gui elements in scene based on mouse position
		        Handles.BeginGUI();
		        Handles.color = Color.white;
		        Handles.Label(cmCurPos, " ", EditorStyles.boldLabel);
		        
		        if ((cmCurPos.x != drawBox.x || cmCurPos.y != drawBox.y) && curTool == 2)
		        {
					if(cmCurPos.x >= drawBox.x)
					{
						if(cmCurPos.y <= drawBox.y)
						{
			            	Handles.DrawLine(new Vector3(drawBox.x, drawBox.y + 1, 0), new Vector3(cmCurPos.x + 1, drawBox.y + 1, 0));
			            	Handles.DrawLine(new Vector3(cmCurPos.x + 1, drawBox.y + 1, 0), new Vector3(cmCurPos.x + 1, cmCurPos.y, 0));
			            	Handles.DrawLine(new Vector3(cmCurPos.x + 1, cmCurPos.y, 0), new Vector3(drawBox.x, cmCurPos.y, 0));
			            	Handles.DrawLine(new Vector3(drawBox.x, cmCurPos.y, 0), new Vector3(drawBox.x, drawBox.y + 1, 0));
						}else{
							Handles.DrawLine(new Vector3(drawBox.x, drawBox.y, 0), new Vector3(cmCurPos.x + 1, drawBox.y, 0));
							Handles.DrawLine(new Vector3(cmCurPos.x + 1, drawBox.y, 0), new Vector3(cmCurPos.x + 1, cmCurPos.y, 0));
							Handles.DrawLine(new Vector3(cmCurPos.x + 1, cmCurPos.y, 0), new Vector3(drawBox.x, cmCurPos.y, 0));
							Handles.DrawLine(new Vector3(drawBox.x, cmCurPos.y, 0), new Vector3(drawBox.x, drawBox.y, 0));
						}
					}else{
						if(cmCurPos.y <= drawBox.y)
						{
							Handles.DrawLine(new Vector3(drawBox.x+1, drawBox.y + 1, 0), new Vector3(cmCurPos.x + 1, drawBox.y + 1, 0));
							Handles.DrawLine(new Vector3(cmCurPos.x + 1, drawBox.y + 1, 0), new Vector3(cmCurPos.x + 1, cmCurPos.y, 0));
							Handles.DrawLine(new Vector3(cmCurPos.x + 1, cmCurPos.y, 0), new Vector3(drawBox.x+1, cmCurPos.y, 0));
							Handles.DrawLine(new Vector3(drawBox.x+1, cmCurPos.y, 0), new Vector3(drawBox.x+1, drawBox.y + 1, 0));
						}else{
							Handles.DrawLine(new Vector3(drawBox.x+1, drawBox.y, 0), new Vector3(cmCurPos.x + 1, drawBox.y, 0));
							Handles.DrawLine(new Vector3(cmCurPos.x + 1, drawBox.y, 0), new Vector3(cmCurPos.x + 1, cmCurPos.y, 0));
							Handles.DrawLine(new Vector3(cmCurPos.x + 1, cmCurPos.y, 0), new Vector3(drawBox.x+1, cmCurPos.y, 0));
							Handles.DrawLine(new Vector3(drawBox.x+1, cmCurPos.y, 0), new Vector3(drawBox.x+1, drawBox.y, 0));
						}
					}
				}
				else
				{
					Handles.DrawLine(cmCurPos + new Vector3(0, 0, 0), cmCurPos + new Vector3(1, 0, 0));
		            Handles.DrawLine(cmCurPos + new Vector3(1, 0, 0), cmCurPos + new Vector3(1, 1, 0));
		            Handles.DrawLine(cmCurPos + new Vector3(1, 1, 0), cmCurPos + new Vector3(0, 1, 0));
		            Handles.DrawLine(cmCurPos + new Vector3(0, 1, 0), cmCurPos + new Vector3(0, 0, 0));
		        }
		        Handles.EndGUI();

			    if(e.isMouse)
			    {
	                if (e.button == 0 && (e.type == EventType.MouseUp || e.type == EventType.MouseDrag))
	                {
	                    if (curTool == 0)
	                    {
	                        GameObject tmpObj = GenerateTile(hit.point.x, hit.point.y);
							Undo.RegisterCreatedObjectUndo(tmpObj, "Created Tile");
							Sprite[] tmpCurSprite = new Sprite[cmCurSprite.Count];
							cmCurSprite.CopyTo(tmpCurSprite);

							if(tmpCurSprite.Length > 0)
							{
								tmpObj.GetComponent<SpriteRenderer>().sprite = tmpCurSprite[UnityEngine.Random.Range((int)0,(int)tmpCurSprite.Length)];
		                        tmpObj.transform.localPosition = new Vector3(Mathf.Floor(hit.point.x) + .5f, Mathf.Floor(hit.point.y) + .5f, layers[selectedLayer].transform.position.z);
							}else{
								Debug.LogWarning("Tile not selected for painting. Please select a tile to paint.");
							}
	                    }
	                    else if (curTool == 1)
	                    {
	                        Transform tmpObj = layers[selectedLayer].Find("Tile|" + (Mathf.Floor(hit.point.x) + .5f) + "|" + (Mathf.Floor(hit.point.y) + .5f));
							if(tmpObj != null)
							{
								Undo.DestroyObjectImmediate(tmpObj.gameObject);
							}
	                    }
	                    else if (curTool == 2)
	                    {
	                        
	                        if (e.type == EventType.MouseUp)
	                        {
	                            Vector2 distance;
	                            bool drawLeft, drawUp;


	                            if (drawBox.x >= hit.point.x)
	                            {
	                                distance.x = drawBox.x - hit.point.x;
	                                drawLeft = true;
	                            }
	                            else
	                            {
	                                distance.x = hit.point.x - drawBox.x;
	                                drawLeft = false;
	                            }

	                            if (drawBox.y >= hit.point.y)
	                            {
	                                distance.y = drawBox.y - hit.point.y;
	                                drawUp = false;
	                            }
	                            else
	                            {
	                                distance.y = hit.point.y - drawBox.y;

	                                distance.y -= 1;
	                                drawUp = true;
	                            }

								if(cmCurPos.y > drawBox.y)
								{
									distance.y -= 1;
								}
								

	                            for (i = 0; i <= distance.x; i++)
	                            {
									for (j = 0; j <= Mathf.Ceil (distance.y); j++)
	                                {
	                                    int curI = i;
	                                    int curJ = j;
	                                    if (drawLeft)
	                                    {
	                                        curI = -curI;
	                                    }
	                                    if (drawUp)
	                                    {
	                                        curJ = -curJ;
	                                    }
										if(cmCurSprite != null)
										{
		                                    GameObject tmpObj = GenerateTile(drawBox.x + curI, drawBox.y - curJ);
											Undo.RegisterCreatedObjectUndo(tmpObj, "Created Tiles in Box");
											Sprite[] tmpCurSprite = new Sprite[cmCurSprite.Count];
											cmCurSprite.CopyTo(tmpCurSprite);
											tmpObj.GetComponent<SpriteRenderer>().sprite = tmpCurSprite[UnityEngine.Random.Range((int)0,(int)tmpCurSprite.Length)];
		                                    tmpObj.transform.localPosition = new Vector3(Mathf.Floor(drawBox.x + curI) + .5f, Mathf.Floor(drawBox.y - curJ) + .5f, layers[selectedLayer].transform.position.z);
										}else{
											Debug.LogWarning("No tiles selected."); 
										}
	                                }
	                            }
	                            
	                        }
	                    }
	                }
	                else if (e.button == 0 && e.type == EventType.MouseDown)
	                {
	                    drawBox.x = Mathf.Floor(hit.point.x);
	                    drawBox.y = Mathf.Floor(hit.point.y);
	                }
	                else if (e.type == EventType.MouseMove)
	                {
	                    drawBox.x = Mathf.Floor(hit.point.x);
	                    drawBox.y = Mathf.Floor(hit.point.y);
	                }

				}
	            cmCurPos.x = (float)Mathf.Floor(hit.point.x);
	            cmCurPos.y = (float)Mathf.Floor(hit.point.y);
				if(curLayer != null)
				{
					cmCurPos.z = curLayer.transform.position.z-1;
				}else{
					cmCurPos.z = 0;
				}
	        }
		}else{
			ResetManager();
		}
		SceneView.RepaintAll();
	}

    void OnDisable(){
       SceneView.onSceneGUIDelegate -= OnSceneGUI;
    }
	
    static GameObject GenerateTile(float x, float y)
    {
        GameObject tmpObj = null;
		if(curLayer != null)
		{
			Transform[] children = curLayer.GetComponentsInChildren<Transform>();
			if(children != null)
			{
				foreach(Transform current in children)
				{
					if(current.name == "Tile|" + (Mathf.Floor(x) + .5f) + "|" + (Mathf.Floor(y) + .5f) && current.parent == curLayer.transform)
					{
						tmpObj = current.gameObject;
					}
				}
			}
		}
		if(tmpObj == null)
        {
            tmpObj = new GameObject("Tile|" + (Mathf.Floor(x) + .5f) + "|" + (Mathf.Floor(y) + .5f));
            tmpObj.AddComponent<SpriteRenderer>();
        }
		if(selectedLayer > layers.Count-1)
		{
			selectedLayer = layers.Count-1;
			ResetLayers();
			layers = ResortLayers(layers);
		}
        tmpObj.transform.parent = layers[selectedLayer];
		tmpObj.GetComponent<SpriteRenderer>().sortingOrder = layers[selectedLayer].GetComponent<SpriteRenderer>().sortingOrder;
		tmpObj.transform.localScale = new Vector3 (1,1,1);
		PolygonCollider2D tmpCol = tmpObj.GetComponent<PolygonCollider2D>();
		if(tmpCol == null && makeCollider)
		{
			tmpCol = tmpObj.AddComponent<PolygonCollider2D>();
			Vector2[] points =
								{
									new Vector2(-.5f,.49f),
									new Vector2(-.45f,.5f),
									new Vector2(.45f,.5f),
									new Vector2(.5f,.49f),
									new Vector2(.5f,-.45f),
									new Vector2(.45f,-.5f),
									new Vector2(-.45f,-.5f),
									new Vector2(-.5f,-.45f)
							};
			tmpCol.points = points;
		}
		if(tmpCol != null && !makeCollider)
		{
			Undo.DestroyObjectImmediate(tmpCol);
		}
		//Repaint();
		SceneView.RepaintAll();
        return tmpObj;
    }
}
