class_name AttackAction extends ActionLeaf

func before_run(actor: Node, _blackboard: Blackboard) -> void:
    actor.velocity.x = 0
    actor.Animation.play("attack")

func tick(actor: Node, _blackboard: Blackboard) -> int:
    var anim: AnimationPlayer = actor.Animation
    if anim.is_playing():
        return RUNNING
    return SUCCESS
