using Gio;
using Gtk;
using System.IO;
using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using OpenTK;
using e3d;


public class Utils
{
    public static Gtk.Image? LoadSvgToImage(string svgPath, int width, int height)
    {
        if (!System.IO.File.Exists(svgPath))
        {
            Console.WriteLine($"File not found: {svgPath}");
            return null;
        }

        try
        {
            return new Gtk.Image()
            {
                File = svgPath,
                WidthRequest = width,
                HeightRequest = height
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading SVG: {ex.Message}");
            return null;
        }
    }

    public static Gtk.Button CreateButton(string iconPath, int iconSize = 24, string? tooltip = null)
    {
    var image = null as Gtk.Image;

                image = LoadSvgToImage(iconPath, iconSize, iconSize);
            

            var button = new Gtk.Button();

            if (image != null)
            {
                button.Child = image;
            }

        if (tooltip != null)
        {
            button.TooltipText = tooltip;
        }
            
        //     ActionRegistry.Bind(button, Path.GetFileNameWithoutExtension(iconPath).Replace("-", "_"));
        // button.ActionName = $"win.{Path.GetFileNameWithoutExtension(iconPath).Replace("-", "_")}";
    
            return button;
        }
}

