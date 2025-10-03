import os
import createAnimations


path_py = os.path.dirname(__file__)
path_scenes = os.path.dirname(os.path.dirname(path_py)) + '\\Scenes'
path_need_sprites = os.path.join(path_scenes, 'NeedSprites')

# print("path_py:", path_py)
# print("path_scenes:", path_scenes)
# print("path_need_sprites:", path_need_sprites)

path_folder_scene = "C:\\SOFTWARES\\Git\\stone-throwing-werewolf\\Scenes\\NeedSprites\\Characters\\Gangster_1"
path_scene = path_folder_scene + '\\Gangster_1.tscn'
path_atlas = path_folder_scene + '\\Gangster_1\\atlas_128.png'
path_json = path_folder_scene + '\\Gangster_1\\json_128.json'
createAnimations.create_animation_player(path_scene, path_atlas, path_json, [])
