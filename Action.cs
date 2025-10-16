using System;
using Gio;
using Gtk;

namespace e3d;

public static class ActionRegistry
{
    public static void RegisterToolbarActions(ApplicationWindow window)
    {
        Add(window, "calculate");
        Add(window, "draw-line");
        Add(window, "draw-circle");
        Add(window, "draw-ellipse");
        Add(window, "erase");
        Add(window, "pan");
        Add(window, "logo");
        Add(window, "hatch");
        Add(window, "polyline");
    }

    public static void Bind(Button button, string actionName)
    {
        button.SetActionName($"win.{actionName}");
    }

    static void Add(ApplicationWindow window, string actionName)
    {
        SimpleAction action = SimpleAction.New(actionName, null);
        
        action.OnActivate += (_, _) => Execute(actionName);
        window.AddAction(action);
    }

    static void Execute(string actionName)
    {
        switch (actionName)
        {
            case "calculate":
                Console.WriteLine("Executing calculation routine.");
                break;
            case "draw-line":
                Console.WriteLine("Preparing line creation.");
                break;
            case "draw-circle":
                Console.WriteLine("Preparing circle creation.");
                break;
            case "draw-ellipse":
                Console.WriteLine("Preparing ellipse creation.");
                break;
            case "erase":
                Console.WriteLine("Activating erase mode.");
                break;
            case "pan":
                Console.WriteLine("Entering pan mode.");
                break;
            case "logo":
                Console.WriteLine("Displaying application logo.");
                break;
            case "hatch":
                Console.WriteLine("Launching hatch tool.");
                break;
            case "polyline":
                Console.WriteLine("Preparing polyline tool.");
                break;
            default:
                Console.WriteLine($"Unhandled action: {actionName}");
                break;
        }
    }
}