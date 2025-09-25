using e3d;
using Gtk;
using Gdk;
using static Gtk.Orientation;
using Menu = Gio.Menu;

class FMain : ApplicationWindow
{


    public FMain(Application app)
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

        // Set minimum size
        glArea.SetSizeRequest(400, 300);

        central_box.Append(glArea);



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
        };
    //       private static void OnRealize(sender, args)
    //         {
    //             // OJO OpenTK necesita el bondong context para funcionar
    //             // gtk_gl_area_get_context()

    //             glArea.MakeCurrent();
    //             //(glArea.Context);
    //             // Initialize OpenGL context here
    //             GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
    //             Console.WriteLine("GlArea initializad OK");

    //         };

    // private static void OnRender(sender, args)
    // {

    //     glArea.MakeCurrent();
    //     // Perform OpenGL drawing commands here
    //     GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

    //     // Example: Draw a simple colored triangle
    //     GL.Begin(PrimitiveType.Triangles);
    //     GL.Color3(1.0f, 0.0f, 0.0f); GL.Vertex2(-0.5f, -0.5f);
    //     GL.Color3(0.0f, 1.0f, 0.0f); GL.Vertex2(0.5f, -0.5f);
    //     GL.Color3(0.0f, 0.0f, 1.0f); GL.Vertex2(0.0f, 0.5f);
    //     GL.End();


    // };

    }

    class Hello : Application
    {
        public Hello() : base([])
        {
            OnActivate += On_activate;
        }

        void On_activate(Gio.Application app, EventArgs args)
        {
            FMain w = new((Application)app);

            w.Show();
        }

        static void Main(string[] args)
        {
            new Hello().Run(args.Length, args);
        }

        
    }


  
}