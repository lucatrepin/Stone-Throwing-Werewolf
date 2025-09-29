import os;
import shutil;
from PIL import Image;
from enum import Enum;
import math;
import json;
import cv2;
import time;

PRINT = True

pathPy = os.path.abspath(__file__).replace("\\", "/")
pathSrc = pathPy.replace("Scripts/Python/assets.py", "Assets/src/")
pathDist = pathPy.replace("Scripts/Python/assets.py", "Assets/dist/")
pathSrcSprites = os.path.join(pathSrc, "Sprites/")
pathDistSprites = os.path.join(pathDist, "Sprites/")
pathScenes = pathPy.replace("Scripts/Python/assets.py", "Scenes/")
pathCharacters = pathScenes + "Characters/"
pathProjectiles = pathScenes + "Projectiles/"

if (PRINT):
  print("pathPy", pathPy)
  print("pathSrc", pathSrc)
  print("pathDist", pathDist)
  print("pathSrcSprites", pathSrcSprites)
  print("pathDistSprites", pathDistSprites)
  print("pathScenes", pathScenes)
  print("pathCharacters", pathCharacters)

class ExtractModeEnum(Enum):
    CRAFTPIX_NORMAL = 1


def VerifyIgnore(path): # terminar de implementar / usar no codigo
  execptions = ["IGNORE", "ignore", "Ignore"]
  for root, dirs, files in os.walk(path):
    if execptions in dirs or execptions in files:
      if (PRINT): print("VerifyIgnore: Ignore", path)
      return True
  return False


def CreateAtlasWithFolderOfAssets(pathOrigin, pathDestiny): # Use Gangsters_1 for Create AtlasOfAssets
  if VerifyIgnore(pathOrigin): return # logica Ignore
  
  if os.path.exists(pathDestiny) == False:
    os.makedirs(pathDestiny)
  
  def GetDimensionsForAtlas(animations):
    raiz = animations ** 0.5
    rest = raiz % 1
    X = int(raiz)
    Y = int(raiz)
    if rest != 0:
      X += 1
      if (X * (X - 1)) >= animations: Y = X - 1
      else: Y = X
    return (X, Y)
  
  def SetAtlas(pathOrigin, pathDestiny):
    atlasDict = {}
    def CheckFiles():
      for root, dirs, files in os.walk(pathOrigin):
        for file in files:
          if os.path.isdir(os.path.join(root, file)):
            print("CreateAtlasWithFolderOfAssets: Folder inside folder not implemented", os.path.join(root, file))
            continue
          else:
            imgPath = os.path.join(root, file)
            if imgPath.endswith(".import"): continue
            img = cv2.imread(imgPath, cv2.IMREAD_UNCHANGED)
            width, height = img.size
            animations = width / height
            if animations != int(animations):
              print("Animation is not integer", os.path.join(root, file), animations)
              continue
            else:
              animations = int(animations)
              if (PRINT): print("Animation", os.path.join(root, file), animations)
            if height not in atlasDict:
              atlasDict[height] = []
            atlasDict[height].append((file, animations, img))
              
    CheckFiles()

    def CreateAtlas():
      for numAtlas in atlasDict:
        atlas = atlasDict[numAtlas]
        atlasSize = sum([a[1] for a in atlas])
        (X, Y) = GetDimensionsForAtlas(atlasSize)
        atlasImage = Image.new('RGBA', (X * numAtlas, Y * numAtlas), (0, 0, 0, 0))
        x = 0
        y = 0
        for file, animations, img in atlas:
          if (PRINT): print("CreateAtlasWithFolderOfAssets: Create Atlas", file, animations, "Size:", (X, Y), "Each:", numAtlas)
          for i in range(animations):
            imgCrop = img[0:numAtlas, i * numAtlas:(i + 1) * numAtlas]
            atlasImage.paste(imgCrop, (x * numAtlas, y * numAtlas))
            x += 1
            if x == X:
              x = 0
              y += 1

        cv2.imwrite(os.path.join(pathDestiny, f"atlas_{numAtlas}.png"), atlasImage)

    CreateAtlas()
    
    if (PRINT): print("AtlasDict", atlasDict)
    
    return atlasDict
    
  SetAtlas(pathOrigin, pathDestiny)


def CreateAnim(pathScene, atlasDict):
  for numAtlas in atlasDict:
    atlas = atlasDict[numAtlas]
    for file, animations, img in atlas:
      x = 0
      y = 0
      for i in range(animations):
        pass


# CreateAtlasWithFolderOfAssets(os.path.join(pathSrcSprites, "briga de rua_gangster/Gangsters_1"), os.path.join(pathDistSprites, "briga de rua_gangster/Gangsters_1"))


def SetScenes():
  for folder in os.listdir(pathCharacters): 
    folderPath = os.path.join(pathCharacters, folder) # Gangsters_1 dentro de Characters
    if os.path.isdir(folderPath):
      pathDest = os.path.join(pathScenes, "Characters", folder, )
      if os.path.exists(destPath) == False:
        os.makedirs(destPath)
      CreateAnim(pathDest, CreateAtlasWithFolderOfAssets(folderPath, destPath))

  for folder in os.listdir(pathProjectiles):
    folderPath = os.path.join(pathProjectiles, folder)
    if os.path.isdir(folderPath):
      destPath = os.path.join(pathScenes, "Projectiles", folder)
      if os.path.exists(destPath) == False:
        os.makedirs(destPath)
      for file in os.listdir(folderPath):
        if file.endswith(".png"):
          shutil.copy(os.path.join(folderPath, file), os.path.join(destPath, file))
          if (PRINT): print("SetScenes: Copy", os.path.join(folderPath, file), "to", os.path.join(destPath, file))

SetScenes()