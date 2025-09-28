class_name StatsData
extends Resource

func _init() -> void: # fazer construtor
  pass

var Level: int = 1
var XP: int = 0
var xp_to_apply: int = 0

var Weight: int = 0
var Height: int = 170
var Age: int = 18

var MaxHealth: int = 100
var Health: int = 100
var MaxMana: int = 100
var Mana: int = 100
var MaxStamina: int = 100
var Stamina: int = 100

enum AttributesEnum {
  STRENGTH,
  DEXTERITY,
  INTELLIGENCE,
  FOCUS,
  STAMINA
}

var Attributes = {
  AttributesEnum.STRENGTH: 1,
  AttributesEnum.DEXTERITY: 1,
  AttributesEnum.INTELLIGENCE: 1,
  AttributesEnum.FOCUS: 1,
  AttributesEnum.STAMINA: 1
}

enum HabsEnum {
  FLY,
  FORCE_FALL,
  INVISIBILITY,
  MULT_JUMPS,
}

var Habs = {
  HabsEnum.FLY: false,
  HabsEnum.FORCE_FALL: false,
  HabsEnum.INVISIBILITY: false,
  HabsEnum.MULT_JUMPS: 1,
}

func gain_xp(amount: int) -> void:
  XP += amount
  var xp_to_next_level = Level * 100
  if XP >= xp_to_next_level:
    XP -= xp_to_next_level
    Level += 1

func apply_xp_to_attributes(Attribute: AttributesEnum) -> void:
  Attributes[Attribute] += 1
  xp_to_apply -= 1

func getJumpForce() -> int:
  return 300 + (10 * Attributes[AttributesEnum.STRENGTH]) + (5 * Attributes[AttributesEnum.DEXTERITY])