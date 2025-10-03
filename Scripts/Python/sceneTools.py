import os

path_py = os.path.dirname(__file__)

def create_uid(digits=13):
  import random
  import string
  uid = 'uid://'
  uid += ''.join(random.choices(string.ascii_lowercase + string.digits, k=digits))
  uids.append(uid)
  return uid
  
class Element: # [gd_scene uid="uid://bqqeiutccr4es"], [node name="Sprite2D" type="Sprite2D" parent="."]
  def __init__(self, label: str = "", properties: dict = {}, lines: list = []):
    self.label = label
    self.properties = properties
    self.lines = lines

class Scene:
  def __init__(self, path: str):
    self.path = path
    self.lines = open(path, 'r', encoding='utf-8').readlines()
    self.elements: list[Element] = self.__get_elements()

    for e in self.elements:
      print("Element:", e.label, e.properties, len(e.lines))
      pass

  def __get_elements(self):
    lines_element = []
    elements = []
    for line in self.lines:
      if line == "\n": continue # não é linha em branco
      line_pieces = line.split()
      label = line_pieces[0]
      is_element = label[0] != '[' or line[-2] != ']'
      if is_element:
        pass
      else:
        continue # é elemento
      label = label[1:] # remove os colchetes
      # se tem label, cada '=' é uma propriedade
      properties = {}
      for piece in line_pieces[1:]:
        if '=' not in piece: continue
        key, value = piece.split('=', 1)
        properties[key] = value.replace('"', '').replace(']', '') # remove aspas
      elements.append(Element(label, properties))
    return elements

  def get_elements(self, label: str) -> list[Element]:
    elements = []
    for element in self.elements:
      if element.label == label:
        elements.append(element)
    return elements

  def save(self, path=None):
    if not path:
      path = self.path
    open(path, 'w', encoding='utf-8').writelines(self.lines)

scene = Scene("C:\\SOFTWARES\\Git\\stone-throwing-werewolf\\Scenes\\NeedSprites\\Characters\\Gangster_1\\Gangster_1.tscn")