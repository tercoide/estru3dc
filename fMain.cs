using e3d;
using Gtk;
using static Gtk.Orientation;
using Menu = Gio.Menu;
using OpenTK.Graphics.OpenGL4;

class FMain : ApplicationWindow
{
    // Variables publicas

    Shader shader;


    public FMain(Application app)   // = Form.Load()
    {
        Application = app;
        Title = "ESTRU3D";



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
        vc0_2.Append(Utils.CreateButton("/home/martin/estru3dc/data/png/tbBasica/calcular.png", 24, "my tool tip"));
        vc0_2.Append(Utils.CreateButton(Config.PathSvg + "line.svg", Config.ButtonSize, "my tool tip"));
        vc0_2.Append(Utils.CreateButton(Config.PathSvg + "circle.svg", 24, "my tool tip"));
        vc0_2.Append(Utils.CreateButton(Config.PathSvg + "ellipse.svg", 24, "my tool tip"));
        vc0_2.Append(Utils.CreateButton(Config.PathSvg + "erase.svg", 24, "my tool tip"));
        vc0_2.Append(Utils.CreateButton(Config.PathSvg + "pan.svg", 24, "my tool tip"));
        vc0_2.Append(Utils.CreateButton(Config.PathSvg + "GambasCAD-logo.svg", 24, "my tool tip"));
        vc0_2.Append(Utils.CreateButton(Config.PathSvg + "hatch.svg", 24, "my tool tip"));
        vc0_2.Append(Utils.CreateButton(Config.PathSvg + "lwpolyline.svg", 24, "my tool tip"));

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

        // Enable required capabilities
        glArea.HasDepthBuffer = true;
        glArea.HasStencilBuffer = true;
        glArea.CanFocus = true;

        glArea.SetRequiredVersion(3, 3);    // Sin esto estamos limitados a OpenGL 2.1 en Linux (GLX) y 1.1 en Windows (WGL)



        // Set minimum size
        glArea.SetSizeRequest(400, 300);

        central_box.Append(glArea);

        // --- OpenGL / shader demo: draw a simple triangle ---
        int vao = 0;
        int vbo = 0;



        // Realize: create GL objects and upload geometry
        glArea.OnRealize += (o, e) => Realize();



        // Render: clear and draw triangle
        glArea.OnRender += (o, e) => Draw();

        glArea.OnResize += (o, e) => Resize();



        // Cleanup when the GLArea is unrealized
        glArea.OnUnrealize += (o, e) => Unrealize();




        Grid grid = new()
        {
            ColumnSpacing = 5,
            RowSpacing = 3
        };
        grid.Attach(Label.New("field 1:"), 0, 0, 1, 1);
        grid.Attach(new Entry(), 1, 0, 1, 1);
        grid.Attach(Label.New("field 2:"), 0, 1, 1, 1);
        grid.Attach(new Entry(), 1, 1, 1, 1);
        Picture cube = Picture.NewForFilename("cube.png");
        cube.Halign = cube.Valign = Align.Center;
        grid.Attach(cube, 0, 2, 2, 1);




        vc0_3.Append(left_vbox);
        vc0_3.Append(Separator.New(Vertical));
        vc0_3.Append(central_box);
        vc0_3.Append(Separator.New(Vertical));
        vc0_3.Append(grid);

        // Box vbox = Box.New(Vertical, 5);
        // vbox.Append(top_hbox);
        // vbox.Append(Separator.New(Horizontal));
        // vbox.Append(hbox2);
        // vbox.MarginTop = vbox.MarginBottom = vbox.MarginStart = vbox.MarginEnd = 8;

        Child = vc0;

        static Menu create_menu()
        {
            Menu fileMenu = new();
            fileMenu.Append("New", "win.new");
            fileMenu.Append("Open...", "win.open");

            // Append a section with no label, which will draw a separator before the section.
            Menu exitSection = new();
            exitSection.Append("Exit", "app.exit");
            fileMenu.AppendSection(null, exitSection);

            Menu viewMenu = new();
            viewMenu.Append("Monospace", "win.monospace");

            Menu transformMenu = new();

            Menu caseSection = new();
            caseSection.Append("Uppercase", "win.uppercase");
            caseSection.Append("Lowercase", "win.lowercase");

            Menu orderingSection = new();
            orderingSection.Append("Reverse Words", "win.reverse_words");
            orderingSection.Append("Reverse Characters", "win.reverse_chars");

            transformMenu.AppendSection("Case", caseSection);
            transformMenu.AppendSection("Ordering", orderingSection);

            Menu documentMenu = new();
            documentMenu.Append("Information", "win.information");

            Menu aboutMenu = new();
            aboutMenu.Append("About", "win.about");

            Menu topMenu = new();
            topMenu.AppendSubmenu("File", fileMenu);
            topMenu.AppendSubmenu("View", viewMenu);
            topMenu.AppendSubmenu("Transform", transformMenu);
            topMenu.AppendSubmenu("Document", documentMenu);
            topMenu.AppendSubmenu("Help", aboutMenu);

            return topMenu;
        }
        ;

        void Realize()
        {
            // Make context current before calling GL functions

            glArea.MakeCurrent();
            GL.LoadBindings(new NativeBindingsContext());
            // Get OpenGL version
            string glVersion = GL.GetString(StringName.Version);
            // Get GLSL version
            string glslVersion = GL.GetString(StringName.ShadingLanguageVersion);

            Console.WriteLine("OpenGL Version: " + glVersion);
            Console.WriteLine("GLSL Version: " + glslVersion);


            shader = new Shader("/home/martin/estru3dc/data/shaders/basic.vert", "/home/martin/estru3dc/data/shaders/basic.frag");

            // Triangle vertices (x, y, z)
            float[] vertices = new float[] {
                0.0f,  0.5f, 0.0f,
               -0.5f, -0.5f, 0.0f,
                0.5f, -0.5f, 0.0f
            };

            vao = GL.GenVertexArray();
            GL.BindVertexArray(vao);

            vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            GL.BindVertexArray(0);
        }
        ;

        bool Resize()
        {
            int w = glArea.GetAllocatedWidth();
            int h = glArea.GetAllocatedHeight();
            GL.Viewport(0, 0, w, h);
            return true;
        }
        ;

        bool Draw()
        {

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            shader?.Use();
            GL.BindVertexArray(vao);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            GL.BindVertexArray(0);
            return true;

        }
        ;
        
        void Unrealize()
        {
            glArea.MakeCurrent();
            if (vbo != 0)
            {
                GL.DeleteBuffer(vbo);
                vbo = 0;
            }
            if (vao != 0)
            {
                GL.DeleteVertexArray(vao);
                vao = 0;
            }
            // if (shader != null)
            // {
            // shader.Delete();
            // }
        };

    }
    
    



 

   


  
}