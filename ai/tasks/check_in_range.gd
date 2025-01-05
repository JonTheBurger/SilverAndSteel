extends BTCondition

func _tick(_delta: float) -> BT.Status:
    if scene_root.get("Senses.IsPlayerInRange"):
        return BT.SUCCESS
    return BT.FAILURE
