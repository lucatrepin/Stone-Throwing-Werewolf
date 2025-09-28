import os;
import shutil;
from PIL import Image;
from enum import Enum;
import math;
import json;

pathPy = os.path.abspath(__file__).replace("\\", "/")
pathSrc = pathPy.replace("Scripts/Python/assets.py", "Assets/src/")
pathDist = pathPy.replace("Scripts/Python/assets.py", "Assets/dist/")
pathSrcSprites = os.path.join(pathSrc, "Sprites")
pathDistSprites = os.path.join(pathDist, "Sprites")
print("pathPy", pathPy)
print("pathSrc", pathSrc)
print("pathDist", pathDist)
print("pathSrcSprites", pathSrcSprites)
print("pathDistSprites", pathDistSprites)

PRINT = False

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
            img = Image.open(imgPath)
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
            imgCrop = img.crop((i * numAtlas, 0, (i + 1) * numAtlas, numAtlas))
            atlasImage.paste(imgCrop, (x * numAtlas, y * numAtlas))
            x += 1
            if x == X:
              x = 0
              y += 1
                          
        atlasImage.save(os.path.join(pathDestiny, f"atlas_{numAtlas}.png"))
                
        def CreateJson(): # dar um jeito de fazer isso melhor / checar se funciona corretamente com o ts
            x = 0
            y = 0
            jsonData = {"atlas": {}, "data": {"numAtlas": numAtlas, "size": {"X": X, "Y": Y}}}
            for file, animations, img in atlas:
              jsonData["atlas"][os.path.splitext(file)[0]] = {}
              timeBase = 1000
              jsonData["atlas"][os.path.splitext(file)[0]]["time"] = timeBase
              for i in range(animations):
                jsonData["atlas"][os.path.splitext(file)[0]][i] = {
                    "x": x * numAtlas,
                    "y": y * numAtlas,
                    "w": x * numAtlas + numAtlas,
                    "h": y * numAtlas + numAtlas,
                }
                x += 1
                if x == X:
                  x = 0
                  y += 1

            with open(os.path.join(pathDestiny, f"atlas_{numAtlas}.json"), "w") as jsonFile:
              json.dump(jsonData, jsonFile, indent=3)

        CreateJson()
        

    if (PRINT): print("AtlasDict", atlasDict)
    
    
  SetAtlas(pathOrigin, pathDestiny)


def extractAssetsCraftpixNormal(pathOrigin, pathDestiny): # Use briga de rua_gangster for create multiple atlas
  if VerifyIgnore(pathOrigin): v # logica Ignore

  execptionsFolders = ["PSD", "__MACOSX"]
  for item in os.listdir(pathOrigin): #  Use briga de rua_gangster For Gangsters_1
    if os.path.isdir(os.path.join(pathOrigin, item)):
      if item not in execptionsFolders:
        CreateAtlasWithFolderOfAssets(os.path.join(pathOrigin, item), os.path.join(pathDestiny, item))

def extractAssets(pathOrigin, pathDestiny, mode: ExtractModeEnum):
  if VerifyIgnore(pathOrigin): return # logica Ignore

  if os.path.exists(pathDestiny) == False:
    os.makedirs(pathDestiny)
    
  if mode == ExtractModeEnum.CRAFTPIX_NORMAL:
    extractAssetsCraftpixNormal(pathOrigin, pathDestiny)

def extractMultAssets(pathOrigin, pathDestiny, mode: ExtractModeEnum):
  if VerifyIgnore(pathOrigin): return # logica Ignore

  for item in os.listdir(pathOrigin):
    if os.path.isdir(os.path.join(pathOrigin, item)):
      extractAssets(os.path.join(pathOrigin, item), os.path.join(pathDestiny, item), mode)



extractMultAssets(pathSrcSprites, pathDistSprites, ExtractModeEnum.CRAFTPIX_NORMAL)



# def VerifyIgnore(path): # terminar de implementar / usar no codigo