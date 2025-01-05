extends BTCondition

@export var fields: Array[StringName]
@export var check_type: LimboUtility.CheckType
@export var value: BBVariant

func _tick(_delta: float) -> BT.Status:
    var current = scene_root

    for field in fields:
        current = current.get(field)

    match check_type:
        LimboUtility.CHECK_EQUAL:
            return BT.SUCCESS if (current == value.saved_value) else BT.FAILURE
        LimboUtility.CHECK_NOT_EQUAL:
            return BT.SUCCESS if (current != value.saved_value) else BT.FAILURE
        LimboUtility.CHECK_GREATER_THAN:
            return BT.SUCCESS if (current > value.saved_value) else BT.FAILURE
        LimboUtility.CHECK_GREATER_THAN_OR_EQUAL:
            return BT.SUCCESS if (current >= value.saved_value) else BT.FAILURE
        LimboUtility.CHECK_LESS_THAN:
            return BT.SUCCESS if (current < value.saved_value) else BT.FAILURE
        LimboUtility.CHECK_LESS_THAN_OR_EQUAL:
            return BT.SUCCESS if (current <= value.saved_value) else BT.FAILURE

    return BT.FAILURE
