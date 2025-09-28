extends CharacterBody2D


@export var speed: float = 100
var Stats: StatsData = StatsData.new()
var jumps: int = 0

func _ready():
  pass

func _process(delta):
  if Input.is_action_pressed("right"):
    velocity.x += 1
  if Input.is_action_pressed("left"):
    velocity.x -= 1
  if Input.is_action_pressed("down") and Stats.Habs[Stats.HabsEnum.FORCE_FALL]:
    velocity.y += 1
  if Input.is_action_just_pressed("jump"):
    if jumps < Stats.Habs[Stats.HabsEnum.MULT_JUMPS]:
      velocity.y = -Stats.getJumpForce()
      jumps += 1
  elif Input.is_action_pressed("up") and Stats.Habs[Stats.HabsEnum.FLY]:
    velocity.y -= 1
  velocity = velocity.normalized() * 150 * (speed * delta)

  move_and_slide()

  if is_on_floor():
    jumps = 0