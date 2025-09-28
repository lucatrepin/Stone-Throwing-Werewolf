import os;

path = "C:/SOFTWARES/Git/stone-throwing-werewolf/Assets/src"

for root, dirs, files in os.walk(path):
  for file in files:
    pathFile = os.path.join(root, file)
    if pathFile.endswith(".import"):
      os.remove(pathFile)
      print("Removed", pathFile)