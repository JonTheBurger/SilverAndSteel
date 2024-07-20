```mermaid
---
title: PlayerFsm
---
stateDiagram

    [*] --> Idle
    Idle --> Run : Direction Pressed
    Idle --> Jump : Jump Pressed
    Idle --> Jump : On Fall
    Jump --> Idle : On Land
    Run --> Idle : Velocity.X == 0
    Run --> Jump : Jump Pressed
    Idle --> Attack : Attack Pressed
    Run --> Attack : Attack Pressed
    Attack --> Idle : Animation Finished
```
