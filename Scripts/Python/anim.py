import os;

pathPy = os.path.dirname(__file__).replace("\\", "/");
pathScenes = pathPy.replace("/Scripts/Python", "/Scenes");
pathAssets = pathPy.replace("/Scripts/Python", "/Assets/dist/Sprites");

print("pathPy: " + pathPy);
print("pathScenes: " + pathScenes);

def VerifyIgnore(path):
  execptions = ["IGNORE", "ignore", "Ignore"]
  for root, dirs, files in os.walk(path):
    if execptions in dirs or execptions in files:
      if (PRINT): print("VerifyIgnore: Ignore", path)
      return True
  return False


def createAnim(pathScene, pathAtlas):
  print("pathScene: " + pathScene);
  print("pathAtlas: " + pathAtlas);
  
  pass

createAnim(pathScenes + "/Player.tscn", pathAssets + "/lobo_lobisomen/Black_Werewolf/atlas_128.png");