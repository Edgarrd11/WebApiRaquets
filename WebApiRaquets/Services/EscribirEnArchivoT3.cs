/*
 * TAREA 3
 * 
1.- Modificar controlador principal método POST, deberemos de guardar en un archivo TXT 
(nuevosRegistros.txt) los datos del registro que creamos en base de datos.

2.- Modificar controlador principal método GET (obtener por nombre) deberemos de guardar 
en un archivo TXT (registroConsultado.txt) el registro que nos trae la base de datos 
al ejecutar el action.

3.- Crear un Timer que cada 2 minutos que escriba en un documento “El Profe Gustavo Rodriguez es el mejor”
 
 */
namespace WebApiRaquets.Services
{
    public class EscribirEnArchivoT3 : IHostedService
    {
        private readonly IWebHostEnvironment env;
        private readonly string nombreArchivo = "incisoTres.txt";
        private Timer timer;

        public EscribirEnArchivoT3(IWebHostEnvironment env)
        {
            this.env = env;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            //3.- Crear un Timer que cada 2 minutos que escriba en un documento “El Profe Gustavo Rodriguez es el mejor”

            //Se ejecuta cuando cargamos la aplicacion 1 vez
            timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(120));//Aqui definimos cada cuando se ejecuta un timer
            Escribir("Proceso Iniciado");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // Se ejecuta cuando detenemos la aplicacion aunque puede que no se ejecute por algun error. 
            timer.Dispose();
            Escribir("Proceso Finalizado");
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            Escribir("El Profe Gustavo Rodriguez es el mejor");
        }
        private void Escribir(string msg)
        {
            var ruta = $@"{env.ContentRootPath}\wwwroot\{nombreArchivo}";
            using (StreamWriter writer = new StreamWriter(ruta, append: true)) { writer.WriteLine(msg); }
        }
    }
}
