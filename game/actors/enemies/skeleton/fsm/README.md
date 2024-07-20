```mermaid
---
title: SkeletonFsm
---
stateDiagram

    [*] --> Idle
    Idle --> Attack : Player In Range
    Attack --> Idle : Animation Finished
    Idle --> Walk : Player Out Of Range
    Idle --> Die : Hp <= 0
    Walk --> Attack : Player In Range
    Attack --> Die : Hp <= 0
    Walk --> Die : Hp <= 0
    Die --> [*] : QueueFree
```
