class_name IdleAction extends ActionLeaf

func before_run(actor: Node, _blackboard: Blackboard) -> void:
    actor.velocity.x = 0
    actor.Animation.play("idle")

func tick(_actor: Node, _blackboard: Blackboard) -> int:
    return RUNNING
