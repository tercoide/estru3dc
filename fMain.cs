using e3d;
using Gtk;
using static Gtk.Orientation;
using Menu = Gio.Menu;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.GraphicsLibraryFramework;

class FMain : Gtk.ApplicationWindow
{
    Shader? shader;
    Gtk.Label statusLabel;

    public FMain(Gtk.Application app)
    {
        Application = app;
        Title = "ESTRU3D";

        ActionRegistry.RegisterToolbarActions(this);

        Menu topMenu = create_menu();
        Gtk.PopoverMenuBar bar = Gtk.PopoverMenuBar.NewFromModel(topMenu);

        Gtk.Box vc0 = Gtk.Box.New(Vertical, 1);
        Gtk.Box vc0_1 = Gtk.Box.New(Horizontal, 1);
        Gtk.Box vc0_2 = Gtk.Box.New(Horizontal, 5);
        Gtk.Box vc0_3 = Gtk.Box.New(Horizontal, 5);
        Gtk.Box vc0_4 = Gtk.Box.New(Horizontal, 5);

        vc0.Append(vc0_1);
        vc0.Append(vc0_2);
        vc0.Append(vc0_3);
        vc0.Append(vc0_4);

        vc0_1.Append(bar);

        // Create toolbar buttons
        vc0_2.Append(Utils.CreateButton("/home/martin/estru3dc/data/png/tbBasica/calcular.png", 24, "calculate", "Calculate"));
        vc0_2.Append(Utils.CreateButton(Config.PathSvg + "line.svg", Config.ButtonSize, "draw-line", "Draw Line"));
        vc0_2.Append(Utils.CreateButton(Config.PathSvg + "circle.svg", 24, "draw-circle", "Draw Circle"));
        vc0_2.Append(Utils.CreateButton(Config.PathSvg + "ellipse.svg", 24, "draw-ellipse", "Draw Ellipse"));
        vc0_2.Append(Utils.CreateButton(Config.PathSvg + "erase.svg", 24, "erase", "Erase"));
        vc0_2.Append(Utils.CreateButton(Config.PathSvg + "pan.svg", 24, "pan", "Pan"));
        vc0_2.Append(Utils.CreateButton(Config.PathSvg + "GambasCAD-logo.svg", 24, "logo", "Logo"));
        vc0_2.Append(Utils.CreateButton(Config.PathSvg + "hatch.svg", 24, "hatch", "Hatch"));
        vc0_2.Append(Utils.CreateButton(Config.PathSvg + "lwpolyline.svg", 24, "polyline", "Polyline"));

        Gtk.Box left_vbox = Gtk.Box.New(Vertical, 5);
        left_vbox.Append(Gtk.Label.New("three"));
        left_vbox.Append(Gtk.Button.NewWithLabel("button 3"));
        left_vbox.Append(Gtk.Label.New("four"));
        Gtk.Button button4 = Gtk.Button.NewWithLabel("button 4");
        button4.Vexpand = true;
        left_vbox.Append(button4);

        Gtk.Box central_box = Gtk.Box.New(Gtk.Orientation.Vertical, 1);

        // Create GLArea widget
        Gtk.GLArea glArea = Gtk.GLArea.New();
        glArea.Vexpand = true;
        glArea.Hexpand = true;
        glArea.HasDepthBuffer = true;
        glArea.HasStencilBuffer = true;
        glArea.CanFocus = true;
        glArea.SetRequiredVersion(3, 3);
        glArea.SetSizeRequest(400, 300);

        central_box.Append(glArea);

        int vao = 0;
        int vbo = 0;

        glArea.OnRealize += (o, e) =>
        {
            glArea.MakeCurrent();
            GL.LoadBindings(new NativeBindingsContext());
            
            string apis = "Allowed API: " + glArea.GetAllowedApis();
            Console.WriteLine(apis);
            glArea.SetAllowedApis(Gdk.GLAPI.Gl);

            string glVersion = GL.GetString(StringName.Version);
            string glslVersion = GL.GetString(StringName.ShadingLanguageVersion);
            Console.WriteLine("OpenGL Version: " + glVersion);
            Console.WriteLine("GLSL Version: " + glslVersion);

            int w = glArea.GetAllocatedWidth();
            int h = glArea.GetAllocatedHeight();
            Console.WriteLine($"Resized onRealized to {w}x{h}");
            GL.Viewport(0, 0, w, h);
            UpdateStatusBar(w, h);
            
            try
            {
                shader = new Shader("/home/martin/estru3dc/data/shaders/basic.vert", 
                                  "/home/martin/estru3dc/data/shaders/basic.frag");
                Console.WriteLine("Shader compiled successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Shader error: {ex.Message}");
            }

            float[] vertices = {
                -0.5f, -0.5f, 0.0f,
                 0.5f, -0.5f, 0.0f,
                 0.0f,  0.5f, 0.0f
            };

            vao = GL.GenVertexArray();
            vbo = GL.GenBuffer();

            GL.BindVertexArray(vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            
            Console.WriteLine($"VAO: {vao}, VBO: {vbo}");
            
            // Check for OpenGL errors
            var error = GL.GetError();
            if (error != OpenTK.Graphics.OpenGL4.ErrorCode.NoError)
            {
                Console.WriteLine($"OpenGL Error: {error}");
            }
        };

        glArea.OnResize += (o, e) =>
        {
            glArea.MakeCurrent();
            int w = glArea.GetAllocatedWidth();
            int h = glArea.GetAllocatedHeight();
            Console.WriteLine($"Resized to {w}x{h}");
            GL.Viewport(0, 0, w, h);
            UpdateStatusBar(w, h);
            glArea.QueueRender();
        };

        glArea.OnRender += (o, e) =>
        {
            glArea.MakeCurrent();
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            if (shader != null && vao != 0)
            {
                shader.Use();
                GL.BindVertexArray(vao);
                GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            }

            return true;
        };

        // Create right side grid with 2 columns
        Gtk.Grid rightGrid = Gtk.Grid.New();
        rightGrid.ColumnSpacing = 0;
        rightGrid.RowSpacing = 0;
        rightGrid.MarginStart = 5;
        rightGrid.MarginEnd = 5;
        rightGrid.MarginTop = 5;
        rightGrid.MarginBottom = 5;
        rightGrid.ColumnHomogeneous = true;
        rightGrid.RowHomogeneous = true;

        // Add CSS for grid borders
        var cssProvider = Gtk.CssProvider.New();
        cssProvider.LoadFromString("""
        .grid-cell {
            border: 1px solid #cccccc;
            padding: 5px;
        }
        """);

        Gtk.StyleContext.AddProviderForDisplay(
            Gdk.Display.GetDefault(),
            cssProvider,
            800  // GTK_STYLE_PROVIDER_PRIORITY_APPLICATION
        );

        // Add some example rows (you can modify this as needed)
        for (int row = 0; row < 10; row++)
        {
            Gtk.Label label = Gtk.Label.New($"Item {row + 1}:");
            label.Halign = Gtk.Align.Start;
            label.AddCssClass("grid-cell");
            rightGrid.Attach(label, 0, row, 1, 1);

            Gtk.Entry entry = Gtk.Entry.New();
            entry.Hexpand = true;
            entry.AddCssClass("grid-cell");
            rightGrid.Attach(entry, 1, row, 1, 1);
        }

        // Create horizontal container for all three sections
        Gtk.Box hc0 = Gtk.Box.New(Horizontal, 1);
        hc0.Append(left_vbox);
        hc0.Append(central_box);
        hc0.Append(rightGrid);

        vc0.Append(hc0);

        // Create status bar at the bottom
        statusLabel = Gtk.Label.New("GLArea: 0 x 0");
        statusLabel.Halign = Gtk.Align.Start;
        statusLabel.MarginStart = 10;
        statusLabel.MarginTop = 5;
        statusLabel.MarginBottom = 5;
        
        Gtk.Box statusBar = Gtk.Box.New(Horizontal, 0);
        statusBar.Append(statusLabel);
        
        vc0.Append(statusBar);

        SetChild(vc0);
        SetDefaultSize(1000, 600);
    }

    void UpdateStatusBar(int width, int height)
    {
        statusLabel.SetLabel($"GLArea: {width} x {height}");
    }

    static Menu create_menu()
    {
        Menu topMenu = Menu.New();
        Menu fileMenu = Menu.New();
        Menu transformMenu = Menu.New();
        Menu documentMenu = Menu.New();
        Menu aboutMenu = Menu.New();

        Menu openSection = Menu.New();
        openSection.Append("Open", "app.open");
        fileMenu.AppendSection(null, openSection);

        Menu exitSection = Menu.New();
        exitSection.Append("Exit", "app.exit");
        fileMenu.AppendSection(null, exitSection);

        transformMenu.Append("Monospace", "win.monospace");
        transformMenu.Append("Uppercase", "win.uppercase");
        transformMenu.Append("Reverse Words", "win.reverse_words");

        aboutMenu.Append("Information", "win.information");
        aboutMenu.Append("About", "win.about");

        topMenu.AppendSubmenu("File", fileMenu);
        topMenu.AppendSubmenu("Transform", transformMenu);
        topMenu.AppendSubmenu("Document", documentMenu);
        topMenu.AppendSubmenu("Help", aboutMenu);

        return topMenu;
    }
}

class Hello : Gtk.Application
{
    public Hello() : base([])
    {
        ApplicationId = "org.estru3d.app";
        OnActivate += On_activate;
    }

    void On_activate(Gio.Application app, EventArgs args)
    {
        FMain win = new FMain(this);
        win.Show();
    }
}