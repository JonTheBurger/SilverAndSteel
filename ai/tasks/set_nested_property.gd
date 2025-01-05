extends BTAction

@export var fields: Array[StringName]
@export var check_type: LimboUtility.CheckType
@export var value: BBVariant

func _tick(_delta: float) -> BT.Status:
    var current = scene_root
    for i in range(fields.size() - 1):
        current = current.get(fields[i])
    current.set(fields[-1], value.saved_value)
    return BT.SUCCESS
