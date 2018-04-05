using UnityEngine;
using System.Collections;

public class Annotation {
    private Vector3 position;
    private Vector3 rotation;
    private string title;
    private string description;

    public Annotation() { }

    public Annotation(Vector3 position, Vector3 rotation, string title, string description) {
        this.position = position;
        this.rotation = rotation;
        this.title = title;
        this.description = description;
    }

    public string Description
    {
        get
        {
            return description;
        }

        set
        {
            description = value;
        }
    }

    public Vector3 Position
    {
        get
        {
            return position;
        }

        set
        {
            position = value;
        }
    }

    public string Title
    {
        get
        {
            return title;
        }

        set
        {
            title = value;
        }
    }

    public Vector3 Rotation
    {
        get
        {
            return rotation;
        }

        set
        {
            rotation = value;
        }
    }
}
