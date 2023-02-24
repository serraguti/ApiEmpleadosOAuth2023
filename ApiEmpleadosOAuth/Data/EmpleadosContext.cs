using Microsoft.EntityFrameworkCore;
using ApiEmpleadosOAuth.Models;

namespace ApiEmpleadosOAuth.Data
{
    public class EmpleadosContext: DbContext
    {
        public EmpleadosContext(DbContextOptions<EmpleadosContext> options)
            : base(options) { }
        public DbSet<Empleado> Empleados { get; set; }
    }
}
