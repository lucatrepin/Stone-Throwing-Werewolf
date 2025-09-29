import os;

path = "C:/SOFTWARES/Git/stone-throwing-werewolf/Assets/src"
extensions = (".import", ".uid")

for root, dirs, files in os.walk(path):
  for file in files:
    pathFile = os.path.join(root, file)
    if pathFile.endswith(extensions):
      os.remove(pathFile)
      print("Removed", pathFile)