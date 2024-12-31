class_name FollowAction extends ActionLeaf

func tick(actor: Node, _blackboard: Blackboard) -> int:
    actor.Animation.play("walk")
    actor.MoveTowardsPlayer()
    if actor.IsPlayerDetected:
        return SUCCESS
    return FAILURE
