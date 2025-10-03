import os
import sceneTools

path_py = os.path.dirname(__file__)

def create_animation_player(path_scene, path_atlas, path_json, uids):
  #identificar gd_scene uid
  scene = sceneTools.Scene(path_scene)
  scene_uid = scene.get_elements('gd_scene')[0].properties['uid']
  #identificar script uid
  script = scene.get_elements('script')[0]
  script_uid = script.properties['uid']
  #identificar sprite uid
  #identificar animationplayer uid
  #identificar animationlibrary uid
  #identificar collisionshape2d uid
  #identificar rectangleshape2d uid
  pass

# [gd_scene load_steps=6 format=3 uid="uid://bqqeiutccr4es"]

# [ext_resource type="Texture2D" uid="uid://dsc5mjp11ryu3" path="res://Assets/dist/Sprites/briga de rua_gangster/Gangsters_1/atlas_128.png" id="1_ntpaq"]

# [sub_resource type="Animation" id="Animation_kyqiw"]
# resource_name = "new_animation"
# tracks/0/type = "value"
# tracks/0/imported = false
# tracks/0/enabled = true
# tracks/0/path = NodePath("Sprite2D:texture")
# tracks/0/interp = 1
# tracks/0/loop_wrap = true
# tracks/0/keys = {
# "times": PackedFloat32Array(0),
# "transitions": PackedFloat32Array(1),
# "update": 1,
# "values": [ExtResource("1_ntpaq")]
# }


# [sub_resource type="AnimationLibrary" id="AnimationLibrary_gntrk"]
# _data = {
# &"RESET": SubResource("Animation_n3cux"),
# &"new_animation": SubResource("Animation_kyqiw")
# }



# [sub_resource type="RectangleShape2D" id="RectangleShape2D_3gjq3"]
# size = Vector2(43, 68)

# [node name="Gangster_1" type="Node2D"]

# [node name="Sprite2D" type="Sprite2D" parent="."]
# texture = ExtResource("1_ntpaq")
# region_enabled = true
# region_rect = Rect2(48, 60, 43, 68)

# [node name="AnimationPlayer" type="AnimationPlayer" parent="."]
# libraries = {
# &"": SubResource("AnimationLibrary_gntrk")
# }

# [node name="CollisionShape2D" type="CollisionShape2D" parent="."]
# shape = SubResource("RectangleShape2D_3gjq3")
