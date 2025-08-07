import time;
import os;
from PIL import Image;
import random;


# configurations start

PATHPROJECT = os.getcwd().replace('\\', '/')
PATHSCRIPTS = os.path.join(PATHPROJECT, "Scripts").replace('\\', '/')
PATHSCENES = os.path.join(PATHPROJECT, "Scenes").replace('\\', '/')
PATHCHARACTERS = os.path.join(PATHPROJECT, "Scenes/Characters").replace('\\', '/')
CHARACTERS = [dir for dir in os.listdir(PATHCHARACTERS)]

MILISECONDPERFRAME = 200

# configurations end

uids = []
ids = []

def GetPathInResFormat(path):
    return path.replace(PATHPROJECT, "res:/").replace('\\', '/')

def JoinReplace(*args):
    return os.path.join(*args).replace('\\', '/')

def GetUidWithLine(line):
    start = line.find('uid=') + 5
    return line[start:line.find('"', start)]

def GetIdWithLine(line):
    start = line.find('id=') + 4
    return line[start:line.find('"', start)]

def GetUidWithARQ(path):
    with open(path, 'r') as arq:
        for line in arq.readlines():
            if 'uid=' not in line: continue
            return GetUidWithLine(line)

def CreateID(prefix = '', codes = 5):
    global ids
    id = None
    while id in ids or id == None: id = f"{prefix}{'_' if prefix else ''}{"".join(random.choices('abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789', k=codes))}"
    return (id)

def CreateUID(codes = 13):
    global uids
    uid = None
    while uid in uids or uid == None: uid = f"uid://{"".join(random.choices('abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789', k=codes))}"
    return uid

def GetFilesWithPath(path, exeptionsFolders = []):
    all_files = []
    is_exeption = False
    for root, folder, files in os.walk(path):
        for i in range(len(exeptionsFolders)):
            if exeptionsFolders[i] in root : 
                is_exeption = True
        if is_exeption:
            is_exeption = False
            continue
        for file in files:
            all_files.append((root + '/' + file).replace('\\', '/'))
    return all_files

def SetUidsAndIds(sceneFiles):
    global uids, ids
    uids = []
    ids = []
    for scene in sceneFiles:
        lines = open(scene, 'r').readlines()
        for line in lines:
            if 'uid=' in line: 
                uid = GetUidWithLine(line)
                if uid not in uids: uids.append(uid)
            positionId = line.find('id=')
            if positionId != -1 and line[positionId - 1] != 'u':
                id = GetIdWithLine(line)
                if id not in ids: ids.append(id)

def Main():
    anim_names = []
    for CHARACTER in CHARACTERS:
        SetUidsAndIds(GetFilesWithPath(PATHSCENES, exeptionsFolders=["Characters"]))
        ASSENTSFOLDER = ''
        SCENE = ''
        for ARQ in os.listdir(os.path.join(PATHCHARACTERS, CHARACTER)):
            if 'Assents' in ARQ and ASSENTSFOLDER == '':
                ASSENTSFOLDER = JoinReplace(PATHCHARACTERS, CHARACTER, ARQ)
            elif '.tscn' in ARQ and SCENE == '':
                SCENE = JoinReplace(PATHCHARACTERS, CHARACTER, ARQ)
        if not ASSENTSFOLDER:
            print(f"Character {CHARACTER} does not have an Assents folder.")
            continue
        if not SCENE:
            print(f"Character {CHARACTER} does not have a scene file.")
            continue
        ASSENTS = [file for file in GetFilesWithPath(ASSENTSFOLDER) if '.import' not in file]
        SCENE_UID = GetUidWithARQ(SCENE) if None else CreateUID()
        text = f'[gd_scene format=3 uid="{SCENE_UID}"]\n\n'
        textAnims = ''
        textTextures = ''
        libraryID = CreateID(prefix="AnimationLibrary")
        textLibrary = f'[sub_resource type="AnimationLibrary" id="{libraryID}"]\n_data = {r"{"}\n'
        for ASSENT in ASSENTS:
            IMG = Image.open(ASSENT)
            IMG_UID = GetUidWithARQ(ASSENT + '.import')
            IMG_WIDTH, IMG_HEIGHT = IMG.size
            ANIMATIONS = IMG_WIDTH // IMG_HEIGHT
            SHAPE_POSITIONS = []
            SHAPE_SIZES = []
            for ANIMATION in range(ANIMATIONS):
                SUB_IMG = IMG.crop((ANIMATION * IMG_HEIGHT, 0, (ANIMATION + 1) * IMG_HEIGHT, IMG_HEIGHT))
                SUB_IMG_WIDTH, SUB_IMG_HEIGHT = SUB_IMG.size
                
                BBOX = SUB_IMG.getbbox()
                BBOX_WIDTH = BBOX[2] - BBOX[0]
                BBOX_HEIGHT = BBOX[3] - BBOX[1]
                LEFT = (SUB_IMG_WIDTH - BBOX_WIDTH) / 2
                TOP = (SUB_IMG_HEIGHT - BBOX_HEIGHT) / 2
                SHAPE_POSITIONS.append(f"Vector2({BBOX[0] - LEFT}, {BBOX[1] - TOP})")
                SHAPE_SIZES.append(f"Vector2({BBOX_WIDTH}, {BBOX_HEIGHT})")
            TIMES = [round(i * MILISECONDPERFRAME / 1000, 4) for i in range(ANIMATIONS)]
            REGION_RECTS = [f"Rect2({i * SUB_IMG_WIDTH}, {0}, {SUB_IMG_WIDTH}, {SUB_IMG_HEIGHT})" for i in range(ANIMATIONS)]
            
            AnimID = CreateID(prefix="Animation")
            TextureID = CreateID(prefix="1")
            textTextures += f'[ext_resource type="Texture2D" uid="{IMG_UID}" path="{GetPathInResFormat(ASSENT)}" id="{TextureID}"]\n'
            textAnims += f"""[sub_resource type="Animation" id="{AnimID}"]
length = {ANIMATIONS * MILISECONDPERFRAME / 1000.0}
tracks/0/type = "value"
tracks/0/imported = false
tracks/0/enabled = true
tracks/0/path = NodePath("Sprite2D:texture")
tracks/0/interp = 1
tracks/0/loop_wrap = true
tracks/0/keys = {{
"times": PackedFloat32Array(0),
"transitions": PackedFloat32Array(1),
"update": 1,
"values": [ExtResource("{TextureID}")]
}}
tracks/1/type = "value"
tracks/1/imported = false
tracks/1/enabled = true
tracks/1/path = NodePath("Sprite2D:region_rect")
tracks/1/interp = 1
tracks/1/loop_wrap = true
tracks/1/keys = {{
"times": PackedFloat32Array({', '.join(map(str, TIMES))}),
"transitions": PackedFloat32Array(1, 1, 1, 1, 1, 1, 1, 1, 1, 1),
"update": 1,
"values": {str(REGION_RECTS).replace("'", "")}
}}
tracks/2/type = "value"
tracks/2/imported = false
tracks/2/enabled = true
tracks/2/path = NodePath("CollisionShape2D:shape:size")
tracks/2/interp = 1
tracks/2/loop_wrap = true
tracks/2/keys = {{
"times": PackedFloat32Array({', '.join(map(str, TIMES))}),
"transitions": PackedFloat32Array(1, 1, 1, 1, 1, 1, 1, 1, 1, 1),
"update": 1,
"values": {str(SHAPE_SIZES).replace("'", "")}
}}
tracks/3/type = "value"
tracks/3/imported = false
tracks/3/enabled = true
tracks/3/path = NodePath("CollisionShape2D:position")
tracks/3/interp = 1
tracks/3/loop_wrap = true
tracks/3/keys = {{
"times": PackedFloat32Array({', '.join(map(str, TIMES))}),
"transitions": PackedFloat32Array(1, 1, 1, 1, 1, 1, 1, 1, 1, 1),
"update": 1,
"values": {str(SHAPE_POSITIONS).replace("'", "")}
}}
"""         
            
            name = os.path.splitext(os.path.basename(ASSENT))[0]
            if (name in anim_names): name += " " + CreateID()
            anim_names.append(name)
            textLibrary += f'&"{name}": SubResource("{AnimID}"),\n'
            
        CollisionShapeID = CreateID(prefix="RectangleShape2D")
        textLibrary += '}'

        textScript = ''
        ARQS_IN_SCRIPTS = GetFilesWithPath(PATHSCRIPTS, exeptionsFolders=["automations"])
        SCRIPTS = [arq for arq in ARQS_IN_SCRIPTS if '.uid' not in arq]
        SCRIPT_ID = CreateID(prefix='1')
        for i in range(len(SCRIPTS)):
            if CHARACTER in SCRIPTS[i]:
                textScript = f'[ext_resource type="Script" uid="{open(SCRIPTS[i] + '.uid').readline().replace("\n", "")}" path="{GetPathInResFormat(SCRIPTS[i])}" id="{SCRIPT_ID}"]\n'

        textComplement = f"""[sub_resource type="RectangleShape2D" id="{CollisionShapeID}"]

[node name="{os.path.splitext(os.path.basename(CHARACTER))[0]}" type="CharacterBody2D"]
{f'script = ExtResource("{SCRIPT_ID}")\n' if textScript else ''}
[node name="AnimationPlayer" type="AnimationPlayer" parent="."]
libraries = {{
&"": SubResource("{libraryID}")
}}

[node name="Sprite2D" type="Sprite2D" parent="."]
texture_filter = 1
region_enabled = true

[node name="CollisionShape2D" type="CollisionShape2D" parent="."]
shape = SubResource("{CollisionShapeID}")
"""
        
        
        text += textScript + textTextures + '\n' + textAnims + '\n' + textLibrary + '\n\n' + textComplement

        open(SCENE, 'w').write(text)

Main()