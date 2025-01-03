class_name AttackAction extends ActionLeaf

func before_run(actor: Node, _blackboard: Blackboard) -> void:
    actor.velocity.x = 0
    actor.Directional.CanTurn = false
    actor.Animation.play("attack")

func after_run(actor: Node, blackboard: Blackboard) -> void:
    actor.Directional.CanTurn = true

func tick(actor: Node, _blackboard: Blackboard) -> int:
    var anim: AnimationPlayer = actor.Animation
    if anim.is_playing():
        return RUNNING
    return SUCCESS
