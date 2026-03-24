using System;

namespace Cw2.Domain.Models;

public abstract class Equipment
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Name { get; set; }
    public bool IsAvailable { get; set; } = true;

    protected Equipment(string name)
    {
        Name = name;
    }
}

public class Laptop : Equipment
{
    public string Processor { get; set; }
    public int RamGb { get; set; }
    public Laptop(string name, string processor, int ram) : base(name) { Processor = processor; RamGb = ram; }
}

public class Projector : Equipment
{
    public string Resolution { get; set; }
    public int Lumens { get; set; }
    public Projector(string name, string resolution, int lumens) : base(name) { Resolution = resolution; Lumens = lumens; }
}

public class Camera : Equipment
{
    public int Megapixels { get; set; }
    public string LensType { get; set; }
    public Camera(string name, int megapixels, string lens) : base(name) { Megapixels = megapixels; LensType = lens; }
}