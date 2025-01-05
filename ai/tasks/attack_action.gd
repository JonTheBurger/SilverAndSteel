extends BTAction

func _enter() -> void:
    scene_root.velocity.x = 0
    scene_root.Animation.play("attack")

func _tick(_delta: float) -> BT.Status:
    if scene_root.Animation.is_playing():
        return BT.RUNNING
    return BT.SUCCESS
