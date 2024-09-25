# Behaviour trees API from scratch

In this project I create a custom behaviour tree API from scratch using Unity and C#. It supports leafs (custom tasks), sequences, selectors, inverters, blackboards...

With this API I recreate a simulation of an art gallery, where there are four types of agents:​

- Robber: when the gallery is closed for customers he tries to steal a paint. If he see a cop he runs away from him and goes to his car, where he is secure. If he managed to steal a painting he returns to his car and if he doesn't have enough money he will try to steal another painting.

- Cop: he patrols through the gallery. If he sees the robber he starts chasing him and when he loses him he continues patrolling.

- Worker: he goes to the gallery door to give tickets to the customers. Then he returns to his office.

- Customer: when they bored they go to the art gallery and wait to get a ticket, if they already have one they don't have to wait. Once they have entered the gallery they start looking for paintings to decrease his boredorm and if his boredorm is less than a threshold they return to their home. They can enter the gallery from 6:00 to 20:00, otherwhise they return home.

​​​​​
In this video you can see the simulation: https://youtu.be/a7mVMQK4JKk
