using e3d;
using Gtk;
using static Gtk.Orientation;
using Menu = Gio.Menu;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.GraphicsLibraryFramework;

class FMain : ApplicationWindow
{
    Shader? shader;

    public FMain(Application app)
    {
        Application = app;
        Title = "ESTRU3D";

        ActionRegistry.RegisterToolbarActions(this);

        Menu topMenu = create_menu();
        PopoverMenuBar bar = PopoverMenuBar.NewFromModel(topMenu);

        Box vc0 = Box.New(Vertical, 1);
        Box vc0_1 = Box.New(Horizontal, 1);
        Box vc0_2 = Box.New(Horizontal, 5);
        Box vc0_3 = Box.New(Horizontal, 5);
        Box vc0_4 = Box.New(Horizontal, 5);

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

        Box left_vbox = Box.New(Vertical, 5);
        left_vbox.Append(Label.New("three"));
        left_vbox.Append(Button.NewWithLabel("button 3"));
        left_vbox.Append(Label.New("four"));
        Button button4 = Button.NewWithLabel("button 4");
        button4.Vexpand = true;
        left_vbox.Append(button4);

        Box central_box = Box.New(Orientation.Vertical, 1);

        // Create GLArea widget
        GLArea glArea = Gtk.GLArea.New();
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
            
            shader = new Shader("/home/martin/estru3dc/data/shaders/basic.vert", 
                              "/home/martin/estru3dc/data/shaders/basic.frag");

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
        };

        glArea.OnResize += (o, e) =>
        {
            glArea.MakeCurrent();
            int w = glArea.GetAllocatedWidth();
            int h = glArea.GetAllocatedHeight();
            Console.WriteLine($"Resized to {w}x{h}");
            GL.Viewport(0, 0, w, h);
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

        Box hc0 = Box.New(Horizontal, 1);
        hc0.Append(left_vbox);
        hc0.Append(central_box);

        vc0.Append(hc0);
        SetChild(vc0);
        SetDefaultSize(800, 600);
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

class Hello : Application
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