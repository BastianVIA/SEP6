﻿namespace Frontend.Entities;

[Serializable]
public class Person
{
    public string ID { get; set; }
    
    public Uri? ImageUrl { get; set; }
    public string Name { get; set; }
    
    public string Bio { get; set; }
    public int? BirthYear { get; set; }
}