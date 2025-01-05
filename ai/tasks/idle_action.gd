extends BTAction

func _enter() -> void:
    scene_root.velocity.x = 0
    scene_root.Animation.play("idle")

func _tick(_delta: float) -> BT.Status:
    return BT.RUNNING
