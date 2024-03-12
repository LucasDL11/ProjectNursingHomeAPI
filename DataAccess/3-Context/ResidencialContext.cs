using Dominio;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Dominio
{
   public class ResidencialContext : DbContext
    {


            public DbSet<ActividadesDiarias> ActividadesDiarias { get; set; }

        public DbSet<ActividadesResidente> ActividadesResidente { get; set; }
        public DbSet<Agenda> Agenda  { get; set; }
            public DbSet<Documentos> Curso { get; set; }

        public DbSet<Curatela> Curatela { get; set; }
            public DbSet<PatologiaCronica> PatologiaCronica { get; set; }
            public DbSet<EstadoAgenda> EstadoAgenda { get; set; }
            public DbSet<EstadoTarea> EstadoTarea { get; set; }
            public DbSet<FuncionalidadesUsuario> FuncionalidadesUsuario { get; set; }
            public DbSet<Insumo> Insumo { get; set; }
        public DbSet<InsumoTareas> InsumoTareas { get; set; }
        public DbSet<InsumoResidente> InsumoResidente { get; set; }
            public DbSet<Medicamento> Medicamento { get; set; }
        public DbSet<VisitanteAgenda> VisitanteAgendas { get; set; }
        public DbSet<MisVisitantes> MisVisitantes { get; set; }
        public DbSet<Parametros> Parametros { get; set; }
            public DbSet<Parentesco> Parentesco { get; set; }
            public DbSet<Persona> Persona { get; set; }
            public DbSet<Personal> Personal { get; set; }
        public DbSet<PersonalTarea> PersonalTarea { get; set; }
        public DbSet<Residente> Residente { get; set; }
            public DbSet<Responsable> Responsable { get; set; }
        public DbSet<Sesion> Sesion { get; set; }
        public DbSet<SolicitudUsuario> SolicitudUsuario { get; set; }
            public DbSet<Tarea> Tarea  { get; set; }
        public DbSet<TerminosAceptados> TerminosAceptados { get; set; }
        public DbSet<TerminosYCondiciones> TerminosYCondiciones { get; set; }
        public DbSet<TipoInsumo> TipoInsumo  { get; set; }
            public DbSet<TipoUsuario> TipoUsuario  { get; set; }
        public DbSet<Token> Token { get; set; }
            public DbSet<Usuario> Usuario { get; set; }
            public DbSet<Visitante> Visitante { get; set; }
        public DbSet<Documentos> Documentos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
      IConfiguration Config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
     string connectionString = Config.GetConnectionString("DefaultDB");
        optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActividadesDiarias>();
            //modelBuilder.Entity<ActividadesResidente>();
            modelBuilder.Entity<Agenda>();
            //modelBuilder.Entity<Comprobante>();
            modelBuilder.Entity<Curatela>(); 
            modelBuilder.Entity<Documentos>();
            //modelBuilder.Entity<Diploma>();
            modelBuilder.Entity<PatologiaCronica>();
            modelBuilder.Entity<EstadoAgenda>();
            modelBuilder.Entity<FuncionalidadesUsuario>();
            modelBuilder.Entity<Insumo>();
            //modelBuilder.Entity<InsumoDelResidente>();
            modelBuilder.Entity<Medicamento>();
            modelBuilder.Entity<MisVisitantes>();
            modelBuilder.Entity<Parametros>();
            //modelBuilder.Entity<Parentesco>();
            modelBuilder.Entity<Persona>();
            modelBuilder.Entity<Personal>();
            modelBuilder.Entity<PersonalTarea>();
            modelBuilder.Entity<Residente>();
            modelBuilder.Entity<Responsable>();
            modelBuilder.Entity<Sesion>();
            modelBuilder.Entity<SolicitudUsuario>();
            modelBuilder.Entity<Tarea>();
            modelBuilder.Entity<TipoInsumo>();
            modelBuilder.Entity<TipoUsuario>();
            modelBuilder.Entity<Token>();
            modelBuilder.Entity<Usuario>();
            modelBuilder.Entity<Visitante>();
            modelBuilder.Entity<VisitanteAgenda>();
            modelBuilder.Entity<Documentos>();

            //Claves primarias compuesta
            modelBuilder.Entity<InsumoResidente>()
               .HasKey(t => new { t.IdInsumo, t.CedResidente });
            modelBuilder.Entity<InsumoTareas>()
               .HasKey(t => new { t.idTarea, t.IdInsumo });

           

            modelBuilder.Entity<Parentesco>()
              .HasKey(t => new { t.cedulaResponsable, t.cedulaResidente });

            modelBuilder.Entity<EstadoAgenda>()
            .HasKey(ea => new { ea.idAgenda, ea.idEstadoAgenda });

            modelBuilder.Entity<EstadoTarea>()
            .HasKey(et => new { et.idTarea,et.idEstadoTarea });

            modelBuilder.Entity<MisVisitantes>()
             .HasKey(mv => new { mv.CedResponsable, mv.CedVisitante });

            modelBuilder.Entity<VisitanteAgenda>()
            .HasKey(va => new { va.cedula, va.idAgenda });

            modelBuilder.Entity<TerminosAceptados>()
.HasKey(ta => new { ta.IdTerminosAceptados, ta.IdTerminosYCondiciones });

            modelBuilder.Entity<PersonalTarea>()
            .HasKey(p => new { p.Cedula, p.IdTarea });

            modelBuilder.Entity<ActividadesResidente>()
       .HasKey(a => new { a.idTarea, a.cedResidente });
            //modelBuilder.Entity<Residente>()
            //   .HasKey(t => new { t.CedulaPersona, t.CedulaResponsable });
        }

    }
}
