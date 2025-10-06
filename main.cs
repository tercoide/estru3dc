
using Gtk;
class MainApp : Application
{
    // Aca empieza el programa y recibe los argumentos de la linea de comandos
    static void Main(string[] args)
    {
        new MainApp().Run(args.Length, args);
    }

    // Luego viene aca, donde crea la aplicacion y establece el evento de activacion
    public MainApp() : base([])
    {
        OnActivate += On_activate;
    }

    // Y cuando se activa, crea la ventana principal y la muestra
    void On_activate(Gio.Application app, EventArgs args)
    {
        FMain w = new((Application)app);

        w.Show();
    }

   


}