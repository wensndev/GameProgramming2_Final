using System;

namespace Game.Systems;

public class InfectionSystem
{
    public float InfectionLevel { get; private set; }

    public void Increase(float amount) =>
        InfectionLevel = Math.Clamp(InfectionLevel + amount, 0f, 1f);

    public void Decrease(float amount) =>
        InfectionLevel = Math.Clamp(InfectionLevel - amount, 0f, 1f);
}
