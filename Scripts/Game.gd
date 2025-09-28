extends Node2D

@export var locations: Array = []
static var game_open = true
signal game_over

func _ready():
  pass # Replace with function body.

func _process(_delta):
  while game_open:
    await game_over
    