extends BTAction

func _tick(_delta: float) -> BT.Status:
    scene_root.Animation.play("walk")
    scene_root.MoveTowardsPlayer()
    if scene_root.IsPlayerDetected:
        return BT.SUCCESS
    return BT.FAILURE
